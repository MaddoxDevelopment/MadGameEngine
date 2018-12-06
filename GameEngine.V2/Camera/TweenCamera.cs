using GameEngine.Movement;
using GameEngine.V2.Positioning;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GameEngine.V2.Camera
{
	public class TweenCamera : IMoveable
	{
		public Position Position { get; set; }
		public float Rotation { get; set; }
		public float Zoom { get; set; }

		private readonly TweenMovement _movement;

		public TweenCamera()
		{
			_movement = new TweenMovement(this);
		}

		public void Render(GameWindow window)
		{
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(-window.Width / 2f, window.Width / 2f, window.Height / 2f,
				-window.Height / 2f, 0, 1);
			
			var transform = Matrix4.Identity;
			transform = Matrix4.Mult(transform, Matrix4.CreateTranslation(-Position.Current.X, -Position.Current.Y, 0));
			transform = Matrix4.Mult(transform, Matrix4.CreateRotationZ(-Rotation));
			transform = Matrix4.Mult(transform, Matrix4.CreateScale(Zoom, Zoom, 1.0f));
			GL.MultMatrix(ref transform);
		}
		
		public void SetPosition(Vector2 position)
		{
			_movement.SetPosition(position);
		}
		
		public void Move()
		{
			_movement.Step();	
		}
		
	}
}