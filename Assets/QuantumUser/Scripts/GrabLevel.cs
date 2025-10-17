using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

namespace AGS.Graber.Model
{
    public class GrabLevel
    {
        public int CurentLevel { get; set; }
        public int ItemsInLevel { get; set; }
        public ReactiveProperty<int> CurentItems { get; private set; }

        [Inject]
        public GrabLevel()
        {
            CurentItems = new ReactiveProperty<int>(0);
        }

        public void CollectItem()
        {
            CurentItems.Value++;
        }
    }
}