namespace StormRend.Systems.Mapping
{
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
