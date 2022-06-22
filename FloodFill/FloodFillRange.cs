namespace FloodFill
{
	public struct FloodFillRange
	{
		public int minX;

		public int maxX;

		public int y;

		public FloodFillRange(int minX, int maxX, int y)
		{
			this.minX = minX;
			this.maxX = maxX;
			this.y = y;
		}
	}
}
