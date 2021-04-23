using System;
using System.IO;
using System.Linq;

namespace WfpBotConsole.Stickers
{
	public static class StickersSelector
	{
		public enum StickerSet : short
		{
			Yoba,
			Frog
		}

		public static string SelectRandomFromSet(StickerSet stickerSet)
		{
			var fileName = stickerSet switch
			{
				StickerSet.Yoba => Path.Combine("Stickers", "Yoba", "YobaUrls.txt"),
				StickerSet.Frog => Path.Combine("Stickers", "Frog", "FrogUrls.txt"),
				_ => string.Empty
			};

			var urls = File.ReadAllLines(fileName).ToArray();

			return urls[new Random().Next(urls.Length)];
		}
	}
}