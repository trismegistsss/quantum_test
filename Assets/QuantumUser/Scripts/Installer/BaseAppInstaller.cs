using System;
using UnityEngine;
using Zenject;

namespace GameCore.Installer
{
    public class BaseAppInstaller : MonoInstaller<BaseAppInstaller>
    {
        public override void InstallBindings()
        {
            SetState();
            SetInfo();
            SetModels();
            SetManagers();
            SetHelpers();
            SetSignals();
            SetPrefabs();
            InstallMemoryPools();
        }

        protected virtual void SetState()
        {

        }

        protected virtual void SetInfo()
        {

        }

        protected virtual void SetModels()
        {

        }

        protected virtual void SetManagers()
        {

        }

        protected virtual void SetHelpers()
        {
            /*Container.Bind<SocialHelper>().AsSingle().NonLazy();*/
        }

        protected virtual void SetSignals()
        {
            /*Container.DeclareSignal<OnSelectMainMenuItemSignal>();*/
        }

        protected virtual void SetPrefabs()
        {
            /*BindPrefabFactory<MainMenuPage, MainMenuPage.Factory>(_uiPrefabs.MainPage);*/
        }

        protected virtual void InstallMemoryPools()
        {

        }

        // ----------- FACTORIES //

        protected virtual void BindPrefabFactory<TPrefab, TFactory>(GameObject prefab) where TFactory : PlaceholderFactory<TPrefab>
        {
            if (prefab == null)
                throw new ArgumentException();

            Container.BindFactory<TPrefab, TFactory>()
                .FromComponentInNewPrefab(prefab)
                .WithGameObjectName(prefab.name);
        }

        protected virtual void BindGameObjectFactory<TGameObject, TFactory>() where TFactory : PlaceholderFactory<TGameObject>
        {
            Container.BindFactory<TGameObject, TFactory>()
                .FromNewComponentOnNewGameObject();
        }
    }
}