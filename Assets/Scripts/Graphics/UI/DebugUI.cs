using System.Collections;
using System.Collections.Generic;
using Managers.Scenes;
using Managers.Party;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace UI.UIDebug
{
	/// <summary>
	/// This class is used just for prototype and debug. So, thats why we have hardcoded stuff and bad code >:)
	/// </summary>
	public class DebugUI : MonoBehaviour
	{
		[SerializeField] private Color _normalColor;
		[SerializeField] private Color _selectedColor;
		[SerializeField] private Button[] _partyMembersButtons;

		#region Unity Messages
		private void Awake()
		{
			PartyManager.OnPartyChangedEvent += UpdatePartyUI;
		}

		private void OnDestroy()
		{
			PartyManager.OnPartyChangedEvent -= UpdatePartyUI;
		}
		#endregion

		#region Public Methods
		public void ReloadGame() => ScenesManager.ReloadScene();

		public void GoToMenu() => ScenesManager.LoadScene("Main Menu");

		public void GoToSandbox() => ScenesManager.LoadScene("Sandbox");

		public void GoToCombat() => ScenesManager.LoadScene("Combat");

		public void RotateMember()
		{
			PartyManager.Instance.RotateMembers();
		}
		#endregion

		#region Private Methods
		[Obsolete]
		private void UpdatePartyUI()
		{
			// var partyMembers = PartyManager.Instance.PartyMembers;
			// for (int i = 0; i < partyMembers.Count; i++)
			// {
			// 	bool isActiveMember = i == PartyManager.Instance.ActiveMemberIndex;
			// 	_partyMembersButtons[i].image.color = isActiveMember ? _selectedColor : _normalColor;

			// 	var image = _partyMembersButtons[i].transform.GetChild(0).GetComponent<RawImage>();
			// 	image.color = new Color(1, 1, 1, 1);
			// 	image.texture = PartyManager.Instance.PartyMembers[i].CharacterDefinition.Icon;
			// }
		}
		#endregion
	}
}