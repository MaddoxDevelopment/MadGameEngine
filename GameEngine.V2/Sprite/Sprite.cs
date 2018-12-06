using System;
using System.Drawing;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GameEngine.V2.Sprite
{
	internal class Sprite
	{
		public static void Draw(Texture2D texture, RectangleF rectangle)
		{
			Draw(texture, new Vector2(rectangle.X, rectangle.Y),
				new Vector2(rectangle.Width / texture.Width, rectangle.Height / texture.Height), Color.White,
				Vector2.Zero);
		}

		public static void Draw(Texture2D texture, RectangleF rectangle, Color color, RectangleF? sourceRec = null)
		{
			Draw(texture, new Vector2(rectangle.X, rectangle.Y),
				new Vector2(rectangle.Width / texture.Width, rectangle.Height / texture.Height), color, Vector2.Zero,
				sourceRec);
		}

		public static void Draw(Texture2D texture, Vector2 position)
		{
			Draw(texture, position, Vector2.One, Color.White, Vector2.Zero);
		}

		public static void Draw(Texture2D texture, Vector2 position, Vector2 scale)
		{
			Draw(texture, position, scale, Color.White, Vector2.Zero);
		}

		public static void Draw(Texture2D texture, Vector2 position, Vector2 scale, Color color)
		{
			Draw(texture, position, scale, color, Vector2.Zero);
		}

		public static void Draw(Texture2D texture, Vector2 position, Vector2 scale, Color color, Vector2 origin,
			RectangleF? source = null)
		{
			var verts = new Vector2[4]
			{
				new Vector2(0, 0),
				new Vector2(1, 0),
				new Vector2(1, 1),
				new Vector2(0, 1)
			};

			GL.BindTexture(TextureTarget.Texture2D, texture.Id);

			GL.Begin(PrimitiveType.Quads);
		
			GL.MatrixMode(MatrixMode.Modelview);
			//GL.LoadIdentity();

			for (var i = 0; i < verts.Length; i++)
			{
				GL.Color3(color);

				
				
				if (source == null)
					GL.TexCoord2(verts[i].X, verts[i].Y);
				else
					GL.TexCoord2(
						(source.Value.X + verts[i].X * source.Value.Width) / texture.Width,
						(source.Value.Y + verts[i].Y * source.Value.Height) / texture.Height);

				
				
				verts[i].X *= texture.Width;
				verts[i].Y *= texture.Height;
				verts[i] -= origin;
				verts[i] *= scale;
				verts[i] += position;

				
				GL.Vertex2(verts[i]);
			}
			
			GL.End();
		}

		public static void Begin(GameWindow window)
		{
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(0, window.Width, window.Height, 0, -1, 1);
		}
	}
}