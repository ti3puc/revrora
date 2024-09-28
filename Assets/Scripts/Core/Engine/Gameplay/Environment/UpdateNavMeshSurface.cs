using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

namespace Environment.NavMesh
{
    public class UpdateNavMeshSurface : MonoBehaviour
    {
        [SerializeField] private NavMeshSurface navMeshSurface;

        private void Update()
        {
            if (navMeshSurface == null) return;
            navMeshSurface.BuildNavMesh();
        }
    }
}