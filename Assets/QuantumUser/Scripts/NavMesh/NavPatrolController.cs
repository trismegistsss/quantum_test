using CinemaControl.Elements;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

namespace CinemaControl.NavMesh
{
    public class NavPatrolController : MonoBehaviour
    {
        [SerializeField] Animator _animation;
        [SerializeField] NavMeshAgent _agent;
        [SerializeField] float _minPause;
        [SerializeField] float _maxPause;

        NavPointElement[] _wayPoints;
        NavPointElement _target;
        bool _walk;
        bool _walked;
        bool _movToRile;
        float _speed;

        Action _back;

        private void OnDestroy()
        {
            _back = null;
            _target = null;
            _wayPoints = null;
        }

        public void SetPoints(NavPointElement[] points)
        {
            _wayPoints = points;
        }

        public void MoveToQueePoint(NavPointElement point, Action back)
        {
            _target.IsBusy = false;
            _back = back;
            _target = point;
            _movToRile = true;
            _agent.enabled = true;
            _walk = true;
            _walked = true;
        }

        public void SetPosition(float speed)
        {
            _speed = speed;

            if (null == _agent) _agent = GetComponent<NavMeshAgent>();
            if (null == _wayPoints || _movToRile) return;

            _agent.enabled = false;
            _target = GetPoint();
            _target.IsBusy = true;

            transform.position = _target.gameObject.transform.position;

            var r = UnityEngine.Random.Range(_minPause, _maxPause);
            Observable.Timer(TimeSpan.FromSeconds(r)).Subscribe(_ =>
            {
                _agent.enabled = true;
                _target.IsBusy = false;
                NextPoint();
            }).AddTo(this);
        }

        private void NextPoint()
        {
            if (null == _wayPoints || _movToRile) return;

            _target = GetPoint();
                
            if(_target == null)
            {
                var r = UnityEngine.Random.Range(_minPause, _maxPause);
                Observable.Timer(TimeSpan.FromSeconds(r)).Subscribe(_ =>
                {
                    NextPoint();
                }).AddTo(this);
            }
            else
            {
                _target.IsBusy = true;
                _walk = true;
                _walked = true;
            }
        }

        private NavPointElement GetPoint()
        {
            NavPointElement target = null;
            var i = 0;
            while(null == target && i< _wayPoints.Length * 2)
            {
                var rv = UnityEngine.Random.Range(0, _wayPoints.Length);
                var tpoint = _wayPoints[rv];
                    
                if (!tpoint.IsBusy)
                    target = tpoint;

                i++;
            }

            return target;
        }

        private void Update()
        {
            if (_walk && _target!=null)
            {
                var tp = _target.gameObject.transform.position;
                if (Vector3.Distance(transform.position, tp) > 0.5f)
                {
                    _agent.speed = _speed;
                    _agent.SetDestination(tp);

                    if (_walked)
                    {
                        _walked = false;
                        _animation.Play("walk", 0);
                        _target.IsBusy = true;
                    }
                }
                else
                {
                    _walk = false; 
                    _animation.Play("Idle", 0);
                    _back?.Invoke();
                    var r = UnityEngine.Random.Range(_minPause, _maxPause);
                    Observable.Timer(TimeSpan.FromSeconds(r)).Subscribe(_ =>
                    { 
                        _target.IsBusy = false;
                        NextPoint();     
                    }).AddTo(this);
                }
            }
        }
    }
}