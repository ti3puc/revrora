using System.Collections;
using System.Collections.Generic;
using Managers.Scenes;
using Managers.Party;
using UnityEngine;

namespace UI.UIDebug
{
	/// <summary>
	/// This class is used just for prototype and debug. So, thats why we have hardcoded stuff and bad code >:)
	/// </summary>
	public class DebugUI : MonoBehaviour
	{
		[Header("References")]
		[SerializeField] private RectTransform[] _partyMembersUI;

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

		public void SwitchToPokemon1()
		{
			PartyManager.Instance.SwitchActiveMember(0);
		}

		public void SwitchToPokemon2()
		{
			PartyManager.Instance.SwitchActiveMember(1);
		}

		public void SwitchToPokemon3()
		{
			PartyManager.Instance.SwitchActiveMember(2);
		}
		#endregion

		#region Private Methods
		private void UpdatePartyUI()
		{
			var partyMembers = PartyManager.Instance.PartyMembers;
			for (int i = 0; i < partyMembers.Count; i++)
			{
				bool isActiveMember = i == partyMembers.IndexOf(PartyManager.Instance.ActivePartyMember);

				var width = isActiveMember ? 200 : 150;
				var height = isActiveMember ? 200 : 150;

				_partyMembersUI[i].sizeDelta = new Vector2(width, height);
			}
		}
		#endregion
	}
}