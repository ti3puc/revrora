using System.Collections;
using System.Collections.Generic;
using Character.Base;
using Managers.Party;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Graphs
{
    public class PartyGraphsSelectionUI : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _maxRadarValue = 209f;

        [Header("References")]
        [SerializeField] private Button[] _selectGraphButtons;
        [SerializeField] private CanvasRenderer _graphCanvasRenderer;
        [SerializeField] private Material _graphMaterial;
        [SerializeField] private Texture2D _graphRadarTexture;

        [Header("Debug")]
        [SerializeField, ReadOnly] private BaseCharacter _currentCharacter;

        private void Awake()
        {
            PartyManager.OnPartyChangedEvent += UpdatePartyUI;
        }

        private void OnDestroy()
        {
            PartyManager.OnPartyChangedEvent -= UpdatePartyUI;
        }

        private void OnEnable()
        {
            UpdatePartyUI();
        }

        public void SelectPartyMember(int memberIndex)
        {
            if (PartyManager.Instance.PartyMembers.Count <= memberIndex)
                return;

            PartyManager.Instance.SwitchActiveMemberIndex(memberIndex);
            UpdatePartyUI();
        }

        private void UpdatePartyUI()
        {
            _currentCharacter = PartyManager.Instance.ActivePartyMember;
            if (_currentCharacter == null)
                return;

            // create radar mesh
            CreateRadarMesh();

            // buttons update
            for (int i = 0; i < PartyManager.Instance.MaxPartySize; i++)
            {
                var image = _selectGraphButtons[i].transform.GetChild(0).GetComponent<RawImage>();
                if (PartyManager.Instance.PartyMembers.Count <= i)
                {
                    image.color = new Color(1, 1, 1, 0);
                    continue;
                }

                image.color = new Color(1, 1, 1, 1);
                image.texture = PartyManager.Instance.PartyMembers[i].CharacterDefinition.Icon;

                bool isSelected = i == PartyManager.Instance.ActiveMemberIndex;
                _selectGraphButtons[i].interactable = !isSelected;
            }
        }

        private void CreateRadarMesh()
        {
            Mesh mesh = new Mesh();
            Vector3[] vertices = new Vector3[6];
            Vector2[] uvs = new Vector2[6];
            int[] triangles = new int[3 * 5];

            float angleIncrement = 360f / 5 * -1;

            Vector3 hpVertex = Quaternion.Euler(0, 0, angleIncrement * 0) * Vector3.up * _maxRadarValue * (_currentCharacter.BaseHP / 100f);
            int hpVertexIndex = 1;

            Vector3 attackVertex = Quaternion.Euler(0, 0, angleIncrement * 1) * Vector3.up * _maxRadarValue * (_currentCharacter.BaseStrength / 100f);
            int attackVertexIndex = 2;

            Vector3 defenseVertex = Quaternion.Euler(0, 0, angleIncrement * 2) * Vector3.up * _maxRadarValue * (_currentCharacter.BaseDefense / 100f);
            int defenseVertexIndex = 3;

            Vector3 agilityVertex = Quaternion.Euler(0, 0, angleIncrement * 3) * Vector3.up * _maxRadarValue * (_currentCharacter.BaseAgility / 100f);
            int agilityVertexIndex = 4;

            Vector3 wisdomVertex = Quaternion.Euler(0, 0, angleIncrement * 4) * Vector3.up * _maxRadarValue * (_currentCharacter.BaseWisdom / 100f);
            int wisdomVertexIndex = 5;

            vertices[0] = Vector3.zero;
            vertices[hpVertexIndex] = hpVertex;
            vertices[attackVertexIndex] = attackVertex;
            vertices[defenseVertexIndex] = defenseVertex;
            vertices[agilityVertexIndex] = agilityVertex;
            vertices[wisdomVertexIndex] = wisdomVertex;

            uvs[0] = Vector2.zero;
            uvs[hpVertexIndex] = Vector2.one;
            uvs[attackVertexIndex] = Vector2.one;
            uvs[defenseVertexIndex] = Vector2.one;
            uvs[agilityVertexIndex] = Vector2.one;
            uvs[wisdomVertexIndex] = Vector2.one;

            triangles[0] = 0;
            triangles[1] = hpVertexIndex;
            triangles[2] = attackVertexIndex;

            triangles[3] = 0;
            triangles[4] = attackVertexIndex;
            triangles[5] = defenseVertexIndex;

            triangles[6] = 0;
            triangles[7] = defenseVertexIndex;
            triangles[8] = agilityVertexIndex;

            triangles[9] = 0;
            triangles[10] = agilityVertexIndex;
            triangles[11] = wisdomVertexIndex;

            triangles[12] = 0;
            triangles[13] = wisdomVertexIndex;
            triangles[14] = hpVertexIndex;

            mesh.vertices = vertices;
            mesh.uv = uvs;
            mesh.triangles = triangles;

            _graphCanvasRenderer.SetMesh(mesh);
            _graphCanvasRenderer.SetMaterial(_graphMaterial, _graphRadarTexture);
        }
    }
}