using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OpenTK;

namespace GameEngine.V2.Text
{
	public class TextWriter
	{
		private static Dictionary<char, Dictionary<Font, Bitmap>> cache = new Dictionary<char, Dictionary<Font, Bitmap>>();
		
		public static List<Texture2D> LoadText(Font font, string text)
		{
			return text.Select(t => GetBitmapForCharacter(font, t))
				.Select(SpriteLoader.LoadTexture)
				.ToList();
		}

		public static void PrintText(IEnumerable<Texture2D> textures, Vector2 position)
		{
			foreach (var t in textures)
			{
				Sprite.Draw(t, position);
				position.X += t.Width;
			}
		}
		
		private static Bitmap GetBitmapForCharacter(Font font, char c)
		{
			if (cache.ContainsKey(c))
			{
				var fonts = cache[c];
				if (fonts.ContainsKey(font))
				{
					return fonts[font];
				}
			}

			if (!cache.ContainsKey(c))
			{
				cache[c] = new Dictionary<Font, Bitmap>();
			}
			var bitmap = BuildBitmapForCharacter(font, c);
			cache[c][font] = bitmap;
			return bitmap;
		}
		
		private static Bitmap BuildBitmapForCharacter(Font font, char c)
		{
			var size = GetCharacterSize(font, c);
			var bitmap = new Bitmap((int) size.Width, (int) size.Height);
			using (var gfx = Graphics.FromImage(bitmap))
			{
				gfx.DrawString(c.ToString(), font, Brushes.White, 0, 0);
			}
			return bitmap;
		}
		
		private static SizeF GetCharacterSize(Font font, char c)
		{
			using (var bmp = new Bitmap(512, 512))
			{
				using (var gfx = Graphics.FromImage(bmp))
				{
					return gfx.MeasureString(c.ToString(), font);
				}
			}
		}
	}
}