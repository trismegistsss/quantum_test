using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CinemaControl.Elements
{
    public class NavPointElement : MonoBehaviour
    {
        [SerializeField] MeshRenderer _renderPoint;

        public bool IsBusy { get; set; }

        public void Start()
        {
            _renderPoint.enabled = false;
        }
    }
}