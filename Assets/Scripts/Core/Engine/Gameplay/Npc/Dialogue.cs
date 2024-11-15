using System.Collections.Generic;
using UnityEngine;

namespace Infra.Handler
{
    [CreateAssetMenu(fileName = nameof(Dialogue), menuName = "Dialogues/" + nameof(Dialogue))]
    public class Dialogue : ScriptableObject
    {
        [SerializeField] private List<string> _sentences;

        public bool HasSentences => _sentences.Count > 0;

        public List<string> Sentences => _sentences;
    }
}