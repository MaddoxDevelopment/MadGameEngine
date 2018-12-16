namespace GameEngine._3D
{
	public class RawModel
	{
		public int VaoId { get; }
		
		public int VboId { get; }
		
		public int IboId { get; }
		public int VertexCount { get; }
		
		public int IntBufferCount { get; }

		public RawModel(int vaoId, int vboId, int iboId, int vertexCount, int intBufferCount)
		{
			VaoId = vaoId;
			VboId = vboId;
			IboId = iboId;
			VertexCount = vertexCount;
			IntBufferCount = intBufferCount;
		}
	}
}