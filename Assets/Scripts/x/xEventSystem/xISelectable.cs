/// <summary>
/// selectable interface which forces selectable objects to implement OnSelect and OnDeselect
/// </summary>

namespace StormRend
{
	public interface xISelectable
	{
		void OnSelect();
		void OnDeselect();
	}
}