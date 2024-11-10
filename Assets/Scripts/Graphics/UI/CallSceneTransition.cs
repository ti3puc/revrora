using System.Collections;
using System.Collections.Generic;
using Managers.Scenes;
using UnityEngine;

namespace UI.Transition
{
    public class CallSceneTransition : MonoBehaviour
    {
        private void Start()
        {
            ScenesManager.FadeOut();
        }
    }
}
