using System.Collections.Generic;
using System.Collections.ObjectModel;
using OpenTK;

namespace GameEngine
{
	public class Initializer
	{
		public static ReadOnlyCollection<GameObject> BuildGameObjects(GameWindow window)
		{
			var offset = 200;
			var vectors = new[]
			{
				new Vector2(0, 0),
				new Vector2(200, 0),
				new Vector2(200, 200),
				new Vector2(0, 200)
			};
			var temp = new List<GameObject>();
			var rows = 0;
			for (var i = 0; i < 10; i++)
			{
				var obj = new GameObject
				{
					Vectors = vectors
				};		
				obj.Position.Down(100);
				var multiplier = rows * 100 + offset;
				obj.Position.Right(multiplier);
				offset += 200;
				rows++;
				temp.Add(obj);
			}
			return new ReadOnlyCollection<GameObject>(temp);
		}
	}
}