using System;
using System.Collections.Generic;
using OpenTK;

namespace GameEngine
{
	public abstract class Entity
	{	
		public Guid Id { get; } = Guid.NewGuid();
		public List<Vector2> Vectors { get; set; }
		public Position Position { get; set; } = new Position();
		
		protected Entity()
		{
			Vectors = new List<Vector2>();
			Id = Guid.NewGuid();
			Position = new Position();
		}
	}
}