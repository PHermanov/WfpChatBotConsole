using System;
using System.IO;
using System.Linq;

namespace WfpBotConsole.Stickers
{
	public class StickersSelector
	{
		public enum SticketSet : short
		{
			Yoba
		}

		public static string SelectRandomFromSet(SticketSet stickerSet)
		{
			var fileName = stickerSet switch
			{
				SticketSet.Yoba => Path.Combine("Stickers", "Yoba", "YobaUrls.txt"),
				_ => string.Empty
			};

			var urls = File.ReadAllLines(fileName).ToArray();

			return urls[new Random().Next(urls.Length)];
		}
	}
}