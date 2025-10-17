using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AGS.Graber.Weapon
{
    public enum GrabWeaponType
    {
        close,
        round,
        granate
    }

    public class GrabWeaponObject : MonoBehaviour
    {
        [Header("Type")]
        [SerializeField] public GrabWeaponType _typeAtack;

        [Header("Weapon")]
        [SerializeField] public Transform _muzleLocator;
        [SerializeField] public Transform _bulletLocator;
        [SerializeField] public Transform _shellLocator;

        [Header("Bullet")]
        [SerializeField] public string _name;
        [SerializeField] public Rigidbody _bulet;
        [SerializeField] public GameObject _muzzleflare;
        [SerializeField] public float _min, _max;
        [SerializeField] public bool _rapidFire;
        [SerializeField] public float _rapidFireCooldown;

        [Header("Shell")]
        [SerializeField] public bool _shotgunBehavior;
        [SerializeField] public int _shotgunPellets;
        [SerializeField] public GameObject _shellPrefab;
        [SerializeField] public bool _hasShells;

    }
}