using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OpenTK;

namespace GameEngine
{
	public class Initializer
	{
		public static ReadOnlyCollection<Entity> BuildGameObjects(GameWindow window)
		{
			var offset = 200;
			var vectors = new[]
			{
				new Vector2(0, 0),
				new Vector2(200, 0),
				new Vector2(200, 200),
				new Vector2(0, 200)
			};
			var temp = new List<Entity>();
			var rows = 0;
			for (var i = 0; i < 10; i++)
			{
				var obj = new GameObject
				{
					Vectors = vectors.ToList()
				};		
				obj.Position.Down(500);
				var multiplier = rows * 100 + offset;
				obj.Position.Right(multiplier);
				offset += 200;
				rows++;
				temp.Add(obj);
			}
			return new ReadOnlyCollection<Entity>(temp);
		}
	}
}