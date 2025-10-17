using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UniRx;
using AGS.Graber.Weapon;

namespace AGS.Graber.Character
{
    public class CharacterSkinItems
    {
        [SerializeField] GameObject Armor;
        [SerializeField] GameObject Character;
        [SerializeField] GameObject Helmet;
    }

    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Rigidbody))]
    public class GrabCharacterBase : MonoBehaviour
    {
        [SerializeField] protected GrabHealhBar HealthBar;
        [SerializeField] public float hp = 100;
        [SerializeField] public float damage = 1;

        [Header("Weapons")]
        [SerializeField] protected GrabWeaponObject[] _weapons;

        [Header("Skins")]
        [SerializeField] protected CharacterSkinItems[] _skins;

        protected NavMeshAgent NavMeshAgent;
        protected CapsuleCollider Collider;
        protected Animator Animator;
        protected Rigidbody RBody;
        protected bool IsEvade;
        private Vector3 previousPosition;

        protected GrabWeaponObject _currentWeapon;

        protected virtual void Awake()
        {
            Animator = GetComponent<Animator>();
            NavMeshAgent = GetComponent<NavMeshAgent>();
            Collider = GetComponent<CapsuleCollider>();
            RBody = GetComponent<Rigidbody>();

            previousPosition = transform.position;
        }

        void OnTriggerEnter(Collider other)
        {
            Debug.Log("Collision : " + other.gameObject.name);
        }

        public virtual void Hit()
        {
            hp -= damage;
            HealthBar.UpdateCounter(hp);
        }

        public virtual void Escape()
        {

        }


        protected virtual void Fire()
        {
            Animator.Play("Fire");

            Observable.Timer(System.TimeSpan.FromSeconds(0.2f)).Subscribe(_ =>
            {
                var mf = Instantiate(_currentWeapon._muzzleflare, _currentWeapon._muzleLocator.position, _currentWeapon._muzleLocator.rotation);
                //   bombList[bombType].muzzleflare.Play();
                mf.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

                if (_currentWeapon._hasShells)
                {
                    var sh = Instantiate(_currentWeapon._shellPrefab, _currentWeapon._shellLocator.position, _currentWeapon._shellLocator.rotation);
                    sh.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                }

                Rigidbody rocketInstance;
                rocketInstance = Instantiate(_currentWeapon._bulet, _currentWeapon._bulletLocator.position, _currentWeapon._bulletLocator.rotation) as Rigidbody;
                rocketInstance.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                // Quaternion.Euler(0,90,0)
                rocketInstance.AddForce(_currentWeapon._bulletLocator.forward * UnityEngine.Random.Range(_currentWeapon._min, _currentWeapon._max));

                if (_currentWeapon._shotgunBehavior)
                {
                    for (int i = 0; i < _currentWeapon._shotgunPellets; i++)
                    {
                        Rigidbody rocketInstanceShotgun;
                        rocketInstanceShotgun = Instantiate(_currentWeapon._bulet, _currentWeapon._bulletLocator.position, _currentWeapon._bulletLocator.rotation) as Rigidbody;
                        rocketInstanceShotgun.transform.localScale = new Vector3(0.3f, 0.23f, 0.3f);
                        // Quaternion.Euler(0,90,0)
                        rocketInstanceShotgun.AddForce(_currentWeapon._bulletLocator.forward * UnityEngine.Random.Range(_currentWeapon._min, _currentWeapon._max));
                    }
                }
            }).AddTo(this);
        }

        public virtual void Idle()
        {

        }

        public virtual void Death()
        {

        }

        public virtual void MeleAttak()
        {

        }

        public virtual void Move()
        {

        }

        public virtual void Granade()
        {

        }
    }
}