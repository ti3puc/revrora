using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: better naming
public class MainUI : MonoBehaviour
{
	public void ReloadGame() => GameManager.Instance.ReloadScene();
}
