using GameCore.Installer;
using System;
using UnityEngine;
using Zenject;

namespace AGS.Installers
{
    public class GraberInstaller : BaseAppInstaller
    {
        [Inject] private ShipSectionObjects _prefabObjects;

        private void SetPrefabs()
        {
           // BindPrefabFactory<UISubstrate, UISubstrate.Factory>(_prefabObjects.UISubstrate);
        }

        protected virtual void BindPrefabFactory<TPrefab, TFactory>(GameObject prefab) where TFactory : PlaceholderFactory<TPrefab>
        {
            if (prefab == null)
                throw new ArgumentException();

            Container.BindFactory<TPrefab, TFactory>()
                .FromComponentInNewPrefab(prefab)
                .WithGameObjectName(prefab.name);
        }

        [Serializable]
        public class ShipSectionObjects
        {
           // [Header("Standart Ship Sections")]
           // public GameObject Place_1_1;
        }
    }
}
