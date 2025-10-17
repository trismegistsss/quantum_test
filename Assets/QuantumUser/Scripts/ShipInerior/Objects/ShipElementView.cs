using AGS.EditorView.ShipEditor.Config;
using AGS.EditorView.ShipEditor.Data;
using AGS.Geom;
using DG.Tweening;
using System;
using UniRx;
using UnityEngine;

namespace AGS.EditorView.ShipEditor.Objects
{
    public class ShipElementView : MonoBehaviour
    {
        [SerializeField]
        private ShipElementData data = new ShipElementData();

        [SerializeField]
        private Transform _doorObject;

        [SerializeField]
        private BoxCollider _doorCollider;

        private Point _cell = Point.Zero;
        private ShipBaseEditorView _level;

        private bool _death;

        #region Parameters

        public BoxCollider DoorCollider
        {
            get { return _doorCollider; }
            set { _doorCollider = value; }
        }

        public Transform DoorObject
        {
            get { return _doorObject; }
            set { _doorObject = value; }
        }

        public ShipElementLayer Layer
        {
            get { return data.Layer; }
        }

        public int Type
        {
            get { return data.Type; }
        }
        public string Id
        {
            get; set;
        }
        public ShipElementData Data
        {
            get { return data; }
        }

        public Point Cell
        {
            get { return _cell; }
            set { _cell = value; }
        }

        public void SortOrder(int order)
        {
            GetComponent<SpriteRenderer>().sortingOrder = order;
        }

        public Sprite GetView()
        {
            return GetComponent<SpriteRenderer>().sprite;
        }

        public ShipBaseEditorView Level
        {
            get { return _level; }
            set { _level = value; }
        }

        public bool Maped
        {
            get { return data.Maped; }
        }

        // ElementType

        public bool Grounds
        {
            get { return data.Grounds; }
        }

        public bool Dangerous
        {
            get { return data.Dangerous; }
        }
        public bool TempDangerous
        {
            get { return data.TempDangerous; }
        }

        public float TimeDangerous;
        public float TimeUndangerous;

        public bool Movable
        {
            get { return data.Movable; }
        }

        public float MoveTime;

        public bool Invisible
        {
            get { return data.Invisible; }
        }
        public bool TempInvisible
        {
            get { return data.TempInvisible; }
        }

        public float InvisibleTime;

        public bool ActionInvisible
        {
            get { return data.ActionInvisible; }
        }

        public bool Barrier
        {
            get { return data.Barrier; }
        }
        public bool Item
        {
            get { return data.Item; }
        }

        public bool Decor
        {
            get { return data.Decor; }
        }

        // exit
        public bool Exit
        {
            get { return data.Exit; }
        }

        // character
        public bool Player
        {
            get { return data.Player; }
        }

        // character
        public bool Enemy
        {
            get { return data.Enemy; }
        }

        public bool Death
        {
            get { return _death; }
            set { _death = value; }
        }

        private float _closeY;

        private void Start()
        {
            if(null!= _doorObject)
                _closeY = _doorObject.localPosition.z;
        }

        #endregion
        bool _move;
        public Tween Move(Point target)
        {
            if (_move) return null;
            _move = true;

            Observable.Timer(TimeSpan.FromSeconds(ShipConfig.ShipPlayerSpeed - ShipConfig.ShipPlayerSpeed / 3)).Subscribe(_ => { _move = false; }).AddTo(this);

            var swap = transform.DOMove(_level.ConvertCellToWorldPoint(target), ShipConfig.ShipPlayerSpeed);
            swap.SetEase(Ease.Unset);
            _cell = target;
            swap.Play();

            return swap;
        }

        public Tween TweenAnimation(bool open)
        {
           return _doorObject.DOLocalMoveY(open ? -3 : _closeY,1F).Play().OnComplete(() => {
                _doorCollider.enabled = !open;
            });           
        }

        /*    public Tween MoveBack(OnDirectionSignal data)
            {
                if (_move) return null;
                _move = true;

                var cpos = _level.ConvertCellToWorldPoint(data.Start);
                var tpos = _level.ConvertCellToWorldPoint(data.Target);

                var delta = 3f;
                transform.position = cpos;
                if (data.Start.x > data.Target.x)
                    tpos.x = cpos.x - (cpos.x - tpos.x) / delta;
                if (data.Start.x < data.Target.x)
                    tpos.x = cpos.x + (tpos.x - cpos.x) / delta;

                if (data.Start.y > data.Target.y)
                    tpos.y = cpos.y - (cpos.y - tpos.y) / delta;
                if (data.Start.y < data.Target.y)
                    tpos.y = cpos.y + (tpos.y - cpos.y) / delta;

                var swap = transform.DOMove(tpos, AppConfig.PlayerSpeed / 2).SetLoops(2, LoopType.Yoyo).Play();
                swap.SetEase(Ease.Linear);
                swap.Play();
                swap.OnComplete(() => {
                    transform.position = cpos;
                    _move = false;
                });

                return swap;
            }

            public void TimeOutDestroy(float time)
            {
                Observable.Timer(TimeSpan.FromSeconds(time))
                    .Subscribe(_ => {
                        GetComponent<SpriteRenderer>().DOFade(0, 0.2f).Play().OnComplete(() => {
                            _level.DestroyItem(this);
                        });
                    }).AddTo(this);
            }*/
    }
}