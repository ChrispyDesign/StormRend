namespace BhaVE.Core.Enums
{
    public enum TickMode
    {
        EveryFrame,		//Ticks every monobehaviour update frame
		Fixed,			//Ticks as per tickRate
		Manual			//Manual ticking by user
    }
}