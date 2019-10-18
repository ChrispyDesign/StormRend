using UnityEngine;

namespace StormRend.Systems
{
    public enum SelectMode
	{
		Idle,
		Attack,
		Move,
	}

	/// <summary>
	/// Handles the user gameplay interactions ie. selecting units, highlighting tiles
	/// </summary>
	public class xPlayerController : MonoBehaviour
	{
		//------------- OLD --------------------
		xPlayerUnit currentPlayer;
		SelectMode currentMode;
		bool isAbilityLocked;
		#region GettersAndSetters
		public xPlayerUnit GetCurrentPlayer() { return currentPlayer; }
		public SelectMode GetCurrentMode() { return currentMode; }
		public bool GetIsAbilityLocked() { return isAbilityLocked; }

		public void SetCurrentPlayer(xPlayerUnit _currentPlayer) { currentPlayer = _currentPlayer; }
		public void SetCurrentMode(SelectMode _curMode) { currentMode = _curMode; }
		public void SetIsAbilityLocked(bool _isLocked) { isAbilityLocked = _isLocked; }
		#endregion

		void Start()
		{
			currentMode = SelectMode.Idle;
		}

		string oldMode;
		void Update()
		{
			var newMode = currentMode.ToString();
			if (newMode != oldMode)
			{
				// Debug.Log(m_curMode.ToString());
				oldMode = newMode;
			}
		}
	}
}