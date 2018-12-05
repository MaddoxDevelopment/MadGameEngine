using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace GameEngine
{
	internal class SpriteLoader
	{
		public static Bitmap TrimBitmap(Bitmap source)
		{
			Rectangle srcRect;
			BitmapData data = null;
			try
			{
				data = source.LockBits(new Rectangle(0, 0, source.Width, source.Height), ImageLockMode.ReadOnly,
					PixelFormat.Format32bppArgb);
				var buffer = new byte[data.Height * data.Stride];
				Marshal.Copy(data.Scan0, buffer, 0, buffer.Length);
				var xMin = int.MaxValue;
				var xMax = 0;
				var yMin = int.MaxValue;
				var yMax = 0;
				for (var y = 0; y < data.Height; y++)
				for (var x = 0; x < data.Width; x++)
				{
					var alpha = buffer[y * data.Stride + 4 * x + 3];
					if (alpha == 0) continue;
					if (x < xMin) xMin = x;
					if (x > xMax) xMax = x;
					if (y < yMin) yMin = y;
					if (y > yMax) yMax = y;
				}

				if (xMax < xMin || yMax < yMin) return null;
				srcRect = Rectangle.FromLTRB(xMin, yMin, xMax, yMax);
			}
			finally
			{
				if (data != null)
					source.UnlockBits(data);
			}

			var dest = new Bitmap(srcRect.Width, srcRect.Height);
			Console.WriteLine("DONE: " + srcRect.Width + " " + srcRect.Height);
			var destRect = new Rectangle(0, 0, srcRect.Width, srcRect.Height);
			using (var graphics = Graphics.FromImage(dest))
			{
				graphics.DrawImage(source, destRect, srcRect, GraphicsUnit.Pixel);
			}

			return dest;
		}

		public static Texture2D LoadTexture(string filePath)
		{
			if (!File.Exists("Content/" + filePath))
				throw new FileNotFoundException("We could not open the file at 'Content/" + filePath + "'");

			var id = GL.GenTexture();
			GL.BindTexture(TextureTarget.Texture2D, id);
			var bmp = TrimBitmap(new Bitmap("Content/" + filePath));

			var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly,
				PixelFormat.Format32bppArgb);

			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmpData.Width, bmpData.Height, 0,
				OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmpData.Scan0);

			bmp.UnlockBits(bmpData);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS,
				(int)TextureWrapMode.ClampToEdge);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT,
				(int)TextureWrapMode.ClampToEdge);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
				(int)TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
				(int)TextureMagFilter.Linear);

			return new Texture2D(id, bmp.Width, bmp.Height);
		}

		public static Texture2D LoadTexture(Bitmap bmp)
		{
			if (bmp == null)
			{
				return new Texture2D();
			}
			var id = GL.GenTexture();
			GL.BindTexture(TextureTarget.Texture2D, id);

			var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly,
				PixelFormat.Format32bppArgb);

			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmpData.Width, bmpData.Height, 0,
				OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmpData.Scan0);

			bmp.UnlockBits(bmpData);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS,
				(int)TextureWrapMode.ClampToEdge);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT,
				(int)TextureWrapMode.ClampToEdge);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
				(int)TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
				(int)TextureMagFilter.Linear);

			return new Texture2D(id, bmp.Width, bmp.Height);
		}
	}
}