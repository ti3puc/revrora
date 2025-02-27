using System.Collections;
using System.Collections.Generic;
using Managers.Player;
using NaughtyAttributes;
using Player;
using UnityEngine;

namespace Visual.Player
{
    public class PlayerVisual : MonoBehaviour
    {
        [Header("Animation")]
        [SerializeField] private Animator _animator;
        [SerializeField, AnimatorParam("_animator")] private int _walkParam;
        [SerializeField, AnimatorParam("_animator")] private int _runParam;

        private void FixedUpdate()
        {
            if (PlayerManager.Instance == null) return;
            if (PlayerManager.Instance.PlayerMovement == null) return;

            if (PlayerManager.Instance.PlayerMovement.IsMoving)
            {
                _animator.SetBool(_walkParam, true);
                _animator.SetBool(_runParam, PlayerManager.Instance.PlayerMovement.IsRunning);
            }
            else
            {
                _animator.SetBool(_walkParam, false);
                _animator.SetBool(_runParam, false);
            }
        }
    }
}
