using UniRx;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

namespace VW.View
{
    public class NavMeshView : MonoBehaviour
    {
        [SerializeField] private NavMeshSurface[] _surfaces;
        public NavMeshSurface[] Surfaces { get { return _surfaces; } }

        public void Reset()
        {
            foreach (var sf in _surfaces)
                sf.RemoveData();
        }

        public void Backe()
        {
           // Observable.Start(() =>
           // {
                foreach (var sf in _surfaces)
                    sf.BuildNavMesh();
            //}).Subscribe().AddTo(this);
        }
    }
}