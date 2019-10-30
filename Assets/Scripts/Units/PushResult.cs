namespace StormRend.Units
{
    /// <summary>
    /// The possible results when a unit is pushed
    /// </summary>
    public enum PushResult
    {
        Nothing,    //Nothing hit
        Unit,       //Unit hit
        UnWalkableTile,     //Unwalkable tile hit
        OverEdge    //Pushed over the edge
    }
}