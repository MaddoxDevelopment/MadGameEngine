namespace GameEngine.V2.Sprite
{
	public struct Texture2D
	{
		public int Id { get; }
		public int Width { get; }
		public int Height { get; set; }

		public Texture2D(int id, int width, int height)
		{
			Id = id;
			Width = width;
			Height = height;
		}
	}
}