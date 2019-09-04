namespace StormRend.Systems.Mapping
{
	/// <summary>
	/// Holds a reference to the connected tile and it's traversal cost
	/// </summary>
	public struct Link
	{
        public Tile target { get; }
        public float cost { get; }

		public Link(Tile target, float cost)
		{
			this.target = target;
			this.cost = cost;
		}
	}
}
