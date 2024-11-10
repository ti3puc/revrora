using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Graphs
{
    public class PartyGraphsSelectionUI : MonoBehaviour
    {
        [SerializeField] private GameObject[] graphs;

        [Header("Debug")]
        [SerializeField, ReadOnly] private GameObject selectedGraph;

        private void Awake()
        {
            SelectGraph(0);
        }

        public void SelectGraph(int index)
        {
            if (index > graphs.Length - 1) return;

            selectedGraph = graphs[index];
            for (int i = 0; i < graphs.Length; i++)
                graphs[i].SetActive(graphs[i] == selectedGraph);
        }
    }
}