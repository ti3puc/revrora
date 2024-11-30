using System.Collections;
using System.Collections.Generic;
using Managers.Player;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;

namespace Creatures
{
    public class CreatureSpeedSetter : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _baseSpeed = 15f;
        [SerializeField] private float _runningSpeed = 30f;
        [SerializeField] private float _angularSpeed = 3600f;
        [SerializeField] private float _runningAngularSpeed = 500f;
        [SerializeField] private float _acceleration = 50f;
        [SerializeField] private float _runningAcceleration = 500f;

        [Header("Debug")]
        [SerializeField, ReadOnly] private NavMeshAgent _navMeshAgent;

        public float Speed => PlayerManager.Instance.PlayerMovement.IsRunning ? _runningSpeed : _baseSpeed;
        public float AngularSpeed => PlayerManager.Instance.PlayerMovement.IsRunning ? _runningAngularSpeed : _angularSpeed;
        public float Acceleration => PlayerManager.Instance.PlayerMovement.IsRunning ? _runningAcceleration : _acceleration;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void FixedUpdate()
        {
            if (_navMeshAgent == null) return;

            if (Speed != _navMeshAgent.speed)
                _navMeshAgent.speed = Speed;

            if (AngularSpeed != _navMeshAgent.angularSpeed)
                _navMeshAgent.angularSpeed = AngularSpeed;

            if (Acceleration != _navMeshAgent.acceleration)
                _navMeshAgent.acceleration = Acceleration;
        }
    }
}