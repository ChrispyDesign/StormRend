/// <summary>
/// hoverable interface which forces hoverable objects to implement OnHover and OnUnhover
/// </summary>
public interface IHoverable
{
    void OnHover();
    void OnUnhover();
}