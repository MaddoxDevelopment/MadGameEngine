using System;

namespace GameEngine
{
	public class Position : ICloneable
	{
		public float X { get; private set; }
		public float Y { get; private set; }

		public Position()
		{
			X = 0;
			Y = 0;
		}

		public void Set(float x, float y)
		{
			X = x;
			Y = y;
		}
		
		public void Left(float x) => X -= x;
		public void Right(float x) => X += x;
		public void Up(float y) => Y -= y;
		public void Down(float y) => Y += y;

		public override bool Equals(object obj)
		{
			if (obj is Position casted)
			{
				var x = (int)casted.X;
				var y = (int)casted.Y;
				return x == (int)X && y == (int)Y;
			}
			return false;
		}

		public object Clone()
		{
			return MemberwiseClone();
		}
	}
}