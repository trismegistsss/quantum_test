using AGS.Graber.Weapon;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Utils;
using Zenject;

namespace AGS.Graber.Character
{
    [RequireComponent(typeof(GrabPlayerController))]
    public class GrabPlayerCharacter : GrabCharacterBase
    {
        public class Factory : PlaceholderFactory<GrabPlayerCharacter> { }

        public float CharacterSpeed = 5;
        public bool Interacting { get; set; }
        public bool Atack { get; set; }

        protected GrabPlayerController PlayerController;

        private const float timer = 0.2f;
        private float time = 0;

        protected virtual void Awake()
        {
            base.Awake();

            PlayerController = GetComponent<GrabPlayerController>();

            _currentWeapon = _weapons[0];
            HealthBar.Initialize(2.4f, 100, HexColorUtils.HexToColor("31B74EFF"));
        }

        protected virtual void Update()
        {
           // if (!UpdateAnimation) return;
          //  UpdateAnimation = false;

            if (Interacting)
            {
                Move();
                time = 0;
            }
            else 
            {
                if (Atack)
                    Fire();
                else
                    Idle();
            }
        }

        protected override void Fire()
        {
            time += Time.deltaTime;

            if (time > timer)
                time = 0;
            else return;

            base.Fire();
        }

        public override void Idle()
        {
            base.Idle();
            Animator.Play("Idle");
        }

        public override void Move()
        {
            base.Move();
            Animator.Play("Run");
        }

        public override void Death()
        {
            base.Move();
            Animator.Play("Death");
        }
        public override void Granade()
        {
            base.Granade();
            Animator.Play("Granade");
        }
    }
}