using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

// TODO: better naming
public class MainUI : MonoBehaviour
{
	public void ReloadGame() => GameManager.Instance.ReloadScene();
}
