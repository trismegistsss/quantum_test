using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

namespace CinemaControl.NavMesh
{
    public class NavMeshController : MonoBehaviour
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
            foreach (var sf in _surfaces)
                sf.BuildNavMesh();
        }
    }
}