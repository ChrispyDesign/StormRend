﻿/// <summary>
/// selectable interface which forces selectable objects to implement OnSelect and OnDeselect
/// </summary>

namespace StormRend
{
	public interface ISelectable
	{
		void OnSelect();
		void OnDeselect();
	}
}