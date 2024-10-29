using System.Collections.Generic;
using UnityEngine;

namespace Infra.Handler
{
    [CreateAssetMenu(fileName = nameof(DialogueHandler), menuName = "Dialogue/" + nameof(DialogueHandler))]
    public class DialogueHandler : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private List<string> _sentences;
        
        public bool HasSentences => _sentences.Count > 0;
        
        public string Name => _name;
        public List<string> Sentences => _sentences;
    }
}