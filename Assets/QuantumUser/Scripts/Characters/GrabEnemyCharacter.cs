using AGS.Graber.Weapon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;
using AGS.Graber.Character.Behaviors;
using Utils;

namespace AGS.Graber.Character
{ 
    public class GrabEnemyCharacter : GrabCharacterBase
    {
        public class Factory : PlaceholderFactory<GrabEnemyCharacter> { }

        [Header("Bhaveors")]
        [SerializeField] public BaseBehavior[] _behaviors;

        private BaseBehavior _behavior;

        protected virtual void Awake()
        {
            base.Awake();

          //  _behavior = GetComponent<BehaviorTree>();

            if(null!=_weapons && _weapons.Length>0)
                _currentWeapon = _weapons[0];

            HealthBar.Initialize(2.4f, 100, HexColorUtils.HexToColor("B32121FF"));

            SetBehaviourData(_behaviors[UnityEngine.Random.Range(0, _behaviors.Length)]);
        }

        private void SetBehaviourData(BaseBehavior behavior)
        {
            _behavior = behavior;
        }

        public void InitBehavior(GrabCharacterBase player)
        {
           // _behavior.Initialize(this, player);
        }

        // animations

        public override void Idle()
        {
            base.Idle();
            Animator.Play("Idle");
        }

        public override void Move()
        {
            base.Move();
            Animator.Play("Move");
        }

        public override void Death()
        {
            base.Move();
            Animator.Play("Death");
            HealthBar.Death();
        }
        public override void Granade()
        {
            base.Granade();
            Animator.Play("Granade");
        }

        public override void MeleAttak()
        {
            base.MeleAttak();
            Animator.Play("Idle");
        }
    }
}
