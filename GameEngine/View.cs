using GameEngine.Base;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GameEngine
{
	public class View : IUpdateable
	{
		public Position Position { get; set; }
		public double Rotation { get; set; }
		public float Zoom { get; set; }

		private float _tweenSteps;
		private float _currentStep;

		public View(Vector2 startPosition, float startZoom = 1f, double rotation = 0.0)
		{
			Position = new Position { Current = startPosition, Destination = startPosition };
			Zoom = startZoom;
			Rotation = rotation;
		}

		public void Update()
		{
			if (_currentStep >= _tweenSteps)
			{
				Position.Current = Position.Destination;
				return;
			}
			_currentStep++;
			Position.Current = Position.Current + (Position.Destination - Position.Current) *
			                  QuarticOut(_currentStep / _tweenSteps);
		}

		private float QuarticOut(float t) => -((t - 1) * (t - 1) * (t - 1) * (t - 1)) + 1;
		
		public void SetPosition(Vector2 position, int steps = 15)
		{
			if (Position.Current == position)
				return;
			Position.Destination = position;
			_currentStep = 0;
			_tweenSteps = steps;
		}

		public void ApplyMatrix()
		{
			var transform = Matrix4.Identity;
			transform = Matrix4.Mult(transform, Matrix4.CreateTranslation(-Position.Current.X, -Position.Current.Y, 0));
			transform = Matrix4.Mult(transform, Matrix4.CreateRotationZ(-(float)Rotation));
			transform = Matrix4.Mult(transform, Matrix4.CreateScale(Zoom, Zoom, 1.0f));
			GL.MultMatrix(ref transform);
		}
	}
}