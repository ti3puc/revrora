using System.Collections;
using System.Collections.Generic;
using Managers.Party;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Graphs
{
    public class PartyGraphsSelectionUI : MonoBehaviour
    {
        [SerializeField] private Button[] selectGraphButtons;
        [SerializeField] private GameObject[] graphs;

        [Header("Debug")]
        [SerializeField, ReadOnly] private GameObject selectedGraph;

        private void Awake()
        {
            PartyManager.OnPartyChangedEvent += UpdatePartyUI;
        }

        private void OnDestroy()
        {
            PartyManager.OnPartyChangedEvent -= UpdatePartyUI;
        }

        private void Start()
        {
            SelectGraph(0);
        }

        public void SelectGraph(int index)
        {
            if (index > graphs.Length - 1) return;

            selectedGraph = graphs[index];
            for (int i = 0; i < graphs.Length; i++)
            {
                bool isSelected = graphs[i] == selectedGraph;
                graphs[i].SetActive(isSelected);
                selectGraphButtons[i].interactable = !isSelected;
            }

            UpdatePartyUI();
        }

        public void RotateMember()
        {
            PartyManager.Instance.RotateMembers();
        }

        private void UpdatePartyUI()
        {
            for (int i = 0; i < PartyManager.Instance.PartyMembers.Count; i++)
            {
                var graphImage = graphs[i].GetComponent<Image>();
                graphImage.sprite = PartyManager.Instance.PartyMembers[i].CharacterDefinition.Graph;

                var image = selectGraphButtons[i].transform.GetChild(0).GetComponent<RawImage>();
                image.texture = PartyManager.Instance.PartyMembers[i].CharacterDefinition.Icon;
            }
        }
    }
}