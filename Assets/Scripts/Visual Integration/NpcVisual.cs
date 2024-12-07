using System.Collections;
using System.Collections.Generic;
using Character.Base;
using Character.StateMachine;
using Managers.Player;
using NaughtyAttributes;
using Npc;
using Player;
using UnityEngine;
using UnityEngine.AI;

namespace Visual.Player
{
    public class NpcVisual : MonoBehaviour
    {
        [Header("Animation")]
        [SerializeField] private Animator _animator;
        [SerializeField, AnimatorParam("_animator")] private int _walkParam;

        [Header("Debug")]
        [SerializeField, ReadOnly] private NavMeshAgent _navMeshAgent;

        private void Awake()
        {
            _navMeshAgent = GetComponentInParent<NavMeshAgent>();
        }

        private void FixedUpdate()
        {
            if (_navMeshAgent == null) return;
            _animator.SetBool(_walkParam, _navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance);
        }
    }
}
