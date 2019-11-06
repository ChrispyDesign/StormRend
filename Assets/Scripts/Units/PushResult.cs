namespace StormRend.Units
{
    /// <summary>
    /// The possible results when a unit is pushed
    /// </summary>
    public enum PushResult
    {
        Nothing,    //Nothing hit
        HitUnit,       //Unit hit
        HitBlockedTile,     //Unwalkable tile hit
        OverEdge    //Pushed over the edge
    }
}