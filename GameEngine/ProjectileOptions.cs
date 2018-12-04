using System;
using GameEngine.Base;
using OpenTK;

namespace GameEngine
{
	public class ProjectileOptions
	{
		public Vector2 StartPosition { get; set; }
		public Texture2D Sprite { get; set; }
		public int MovementSpeed { get; set; }
		public ProjectileCollisionOptions Collision { get; set; }
	}

	public class ProjectileCollisionOptions
	{
		public ProjectileCollisionType Type { get; set; }	
		public ICollisionableLocatable SpecificTarget { get; set; }
		public Action<ICollisionable> OnCollision { get; set; }
	}

	public enum ProjectileCollisionType
	{
		SpecificTarget, FirstHit
	}
}