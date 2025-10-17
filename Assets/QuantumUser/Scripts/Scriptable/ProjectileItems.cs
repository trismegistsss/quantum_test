using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AGS.Graber.Scriptable
{
    [Serializable]
    public class projectile
    {
        [Header("Weapon")]
        public GameObject Weapon;
        public Transform MuzleLocator;
        public Transform BulletLocator;
        public Transform ShellLocator;

        [Header("Bullet")]
        public string Name;
        public Rigidbody Bulet;
        public GameObject Muzzleflare;
        public float Min, Max;
        public bool RapidFire;
        public float RapidFireCooldown;

        [Header("Shell")]
        public bool ShotgunBehavior;
        public int ShotgunPellets;
        public GameObject ShellPrefab;
        public bool HasShells;
    }

    public class ProjectileItems : ScriptableObject
    {
        public projectile[] bombList;
    }
}
