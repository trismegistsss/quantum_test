using AGS.EditorView.ShipEditor;
using AGS.EditorView.ShipEditor.Data;
using AGS.EditorView.ShipEditor.Objects;
using AGS.Graber.Character;
using AGS.Graber.Model;
//using Com.LuisPedroFonseca.ProCamera2D;
using CustomUtils;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

namespace AGS.Graber
{
    public class GrabLevelController : MonoBehaviour
    {
        [Inject] private GrabLevel _level;
        
        
       // [SerializeField] ProCamera2D _camera;
        [SerializeField] string[] _Levels;

        private List<GrabEnemyCharacter> _enemies;
        private ShipElementView _player;
        private GrabPlayerCharacter _playerControll;
        private ShipElementView _target;
        private GrabEnemyCharacter _targetControll;
        private GrabLevelView _view;
        private bool _roomFinish;
        private int _room;


        private void Start()
        {
            NextLevel();
        }

        private void Subscriptions()
        {

        }

        public void NextLevel()
        {
            DestroyLevel();
            if (_level.CurentLevel > 2) _level.CurentLevel = 0;
            LoadLevel(_Levels[_level.CurentLevel]);
        }

        public void DestroyLevel()
        {
            _view = GetComponent<GrabLevelView>();
        }

        public void LoadLevel(string level)
        {
            try
            {
                _enemies = new List<GrabEnemyCharacter>();
                _view = GetComponent<GrabLevelView>();
                _view.Refresh();

                //_control = GetComponent<PlayerControl>();

                ShipElementsLoader.Instance.Refresh();

                var data = XMLHelper.DeserealizeFromXML<ShipLevelData>(level, ShipElementDataTypes.Serializable);

                _view.Load(data);

                _view.CreateNavMesh();

                // fix camera and add control to player
                _player = _view.GetPlayer();
                _playerControll = _player.GetComponent<GrabPlayerCharacter>();
                //_camera.AddCameraTarget(_player.transform);
                // _control.SetPlayer = _player.transform;

                //Subscription();

                NextRoom();
            }

            catch (Exception e)
            {
                Debug.LogException(e);
                Destroy(this);
            }
        }

        private void NextRoom()
        {
            Vector2 pos = new Vector2(_player.transform.position.x, _player.transform.position.z);
            var r = _view.GetCurrentRoom(_view.ConvertWorldPointToCell(pos));

            if (r.EnemyCount>0)
            {
                _room = r.Index;
                _roomFinish = false;
            }
        }

        private void Update()
        {
            if (_room > 0 && !_roomFinish)
            {
                if (!_playerControll.Interacting)
                {
                    if (_target != null && !_target.Death)
                    {
                        Vector3 rotation = _target.transform.position - _player.transform.position;
                        _player.transform.forward = Vector3.Lerp(_player.transform.forward, rotation, Time.deltaTime * 20f);

                        _playerControll.Atack = true;

                        if (_targetControll.hp <= 0)
                        {
                            _target.Death = true;
                            _targetControll.Death();
                            _view.KillEnemyInRoom(_room, _target);
                        }
                    }
                    else
                    {
                        _target = _view.GetRoomTargetEnemy(_room, _player.transform);

                        if (null == _target)
                        {
                            var doors = _view.GetRoomDoors(_room);
                            
                            _roomFinish = true;
                            _room = 0;
                            _playerControll.Atack = false;
                            
                            Observable.Timer(TimeSpan.FromSeconds(1f)).Subscribe(_ =>
                            {
                                foreach (var dr in doors)
                                {
                                    dr.TweenAnimation(true).OnKill(() =>
                                    {
                                         _view.UpdateNavMesh();
                                    });
                                }
                            }).AddTo(this);
                        }
                        else
                        {
                            _targetControll = _target.GetComponent<GrabEnemyCharacter>();
                           
                           /* Observable.Timer(TimeSpan.FromSeconds(5))
                             .Subscribe(_ =>
                             {
                                 _target.Death = true;
                                 var doors = _view.GetRoomDoors(_room);
                                 foreach(var dr in doors)
                                 {
                                     dr.TweenAnimation(true).OnKill(()=> {
                                        Observable.Timer(TimeSpan.FromSeconds(0.2f)).Subscribe(_ =>
                                        {
                                            _view.UpdateNavMesh();
                                        }).AddTo(this);
                                     });   
                                 }  
                             }).AddTo(this);*/
                        }
                    }
                }
                else
                {
                    _target = null;
                    _targetControll = null;
                }
            }
            else
            {
                NextRoom();
            }
        }
    }
}