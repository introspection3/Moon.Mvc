using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace Moon.Mvc
{
	public sealed class RandomImage
	{
		private const string RandCharString = "ABCDEFGHJKLNPQSTUVXYZ";
		private int width;
		private int height;
		private int length;
		/// <summary>
		/// 默认构造函数，生成的图片宽度为80,28，随机字符串四个字符
		/// </summary>
		public RandomImage():this(80,28,4)
		{
		}
		/// <summary>
		/// 指定生成图片的宽和高，随机字符串四个字符
		/// </summary>
		/// <param name="width">图片宽度</param>
		/// <param name="height">图片高度</param>
		public RandomImage(int width, int height):this(width,height,4)
		{
		}
		/// <summary>
		/// 指定生成图片的宽和高以及生成图片的字符串字符个数
		/// </summary>
		/// <param name="width">图片宽度</param>
		/// <param name="height">图片高度</param>
		/// <param name="length">图片字符</param>
		public RandomImage(int width, int height, int length)
		{
			this.width = width;
			this.height = height;
			this.length = length;
		}
		/// <summary>
		/// 获取当前的验证码对应的值
		/// </summary>
		public string Value
		{
			get;
			private set;
		}
		/// <summary>
		/// 以默认的大小和默认的字符个数产生图片
		/// </summary>
		/// <returns></returns>
		public Image GetImage()
		{
			Bitmap image = new Bitmap(width, height);
			Graphics g = Graphics.FromImage(image);
			g.Clear(Color.White);
			string randString = "";
			Random random=new Random();
			do
			{
				randString += RandCharString.Substring(random.Next(DateTime.Now.Millisecond)%RandCharString.Length, 1);
			}
			while (randString.Length < 4);
			Value=randString;
			float emSize=(float)width/randString.Length;
			Font font = new Font("Arial", emSize, (System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic));
			Pen pen = new Pen(Color.Silver);
			#region 画图片的背景噪音线
			int x1,y1,x2,y2;
			
			for (int i = 0; i < 25; i++)
			{
				x1 = random.Next(image.Width);
				y1 = random.Next(image.Height);
				x2 = random.Next(image.Width);
				y2 = random.Next(image.Height);
				g.DrawLine(pen, x1, y1, x2, y2);
			}
			#endregion

			#region 画图片的前景噪音点
			for (int i = 0; i < 100; i++)
			{
				x1 = random.Next(image.Width);
				y1 = random.Next(image.Height);
				image.SetPixel(x1, y1, Color.FromArgb(random.Next(Int32.MaxValue)));
			}
			#endregion

			g.DrawString(randString, font, Brushes.Red, 2, 2);
			g.Dispose();
			return image;
			
		}
		public byte[] GetImageByte(){
			RandomImage randImage=new RandomImage();
			System.Drawing.Image image = randImage.GetImage();
			System.IO.MemoryStream memoryStream = new MemoryStream();
			image.Save(memoryStream, ImageFormat.Jpeg);
			var r= memoryStream.ToArray();
			memoryStream.Close();
			image.Dispose();
			this.Value=randImage.Value;
			return r;
		}
	}
}
