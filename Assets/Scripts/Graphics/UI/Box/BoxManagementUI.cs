using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Box
{
    public class BoxManagementUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _content;
        [SerializeField] private GameObject _partyMemberPrefab;

        private void Start()
        {
            UpdateBoxMembers();
        }

        private void UpdateBoxMembers()
        {

        }
    }
}
