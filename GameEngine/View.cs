using GameEngine.Base;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GameEngine
{
	public class View : IUpdateable, IMoveable
	{
		private static View _instance;
		
		public static View Get()
		{
			return _instance;
		}
		
		private readonly Movement.TweenMovement _tweenMovement;
		
		public View(Vector2 startPosition, float startZoom = 1f, double rotation = 0.0)
		{
			Position = new Position { Current = startPosition, Destination = startPosition };
			Zoom = startZoom;
			Rotation = rotation;
			_tweenMovement = new Movement.TweenMovement(this);
			_instance = this;
		}

		public Position Position { get; set; }
		public double Rotation { get; set; }
		public float Zoom { get; set; }

		public void ApplyMatrix()
		{
			var transform = Matrix4.Identity;
			transform = Matrix4.Mult(transform, Matrix4.CreateTranslation(-Position.Current.X, -Position.Current.Y, 0));
			transform = Matrix4.Mult(transform, Matrix4.CreateRotationZ(-(float)Rotation));
			transform = Matrix4.Mult(transform, Matrix4.CreateScale(Zoom, Zoom, 1.0f));
			GL.MultMatrix(ref transform);
		}

		public Direction Direction { get; set; }

		public void SetPosition(Vector2 position)
		{
			_tweenMovement.SetPosition(position);
		}
		
		public void Move()
		{
			_tweenMovement.Step();	
		}

		public void Update()
		{
			Move();
		}
	}
}