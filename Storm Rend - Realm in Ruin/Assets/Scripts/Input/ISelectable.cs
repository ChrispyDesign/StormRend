/// <summary>
/// selectable interface which forces selectable objects to implement OnSelect and OnDeselect
/// </summary>
public interface ISelectable
{
    void OnSelect();
    void OnDeselect();
}