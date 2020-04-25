using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace SplitEditor {
	public class LockBitmap {
		private Bitmap source = null;
		private IntPtr Iptr = IntPtr.Zero;
		private BitmapData bitmapData = null;

		private byte[] pixels;
		public int Width { get; private set; }
		public int Height { get; private set; }

		public LockBitmap(Bitmap source) {
			this.source = source;
			Width = source.Width;
			Height = source.Height;
			int depth = Bitmap.GetPixelFormatSize(source.PixelFormat);
			pixels = new byte[Width * Height * (depth / 8)];
		}

		public void LockBits() {
			bitmapData = source.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppRgb);
			Iptr = bitmapData.Scan0;
			Marshal.Copy(Iptr, pixels, 0, pixels.Length);
		}

		public Bitmap UnlockBits() {
			Marshal.Copy(pixels, 0, Iptr, pixels.Length);
			source.UnlockBits(bitmapData);
			return source;
		}

		public RvbColor GetPixelColor(int pixelX, int pixelY) {
			int adr = ((pixelY * Width) + pixelX) << 2;
			return new RvbColor(pixels[adr] + (pixels[adr + 1] << 8) + (pixels[adr + 2] << 16));
		}

		public void SetPixel(int pixelX, int pixelY, int color) {
			int adr = ((pixelY * Width) + pixelX) << 2;
			pixels[adr++] = (byte)(color);
			pixels[adr++] = (byte)(color >> 8);
			pixels[adr++] = (byte)(color >> 16);
			pixels[adr] = 0xFF;
		}

		public void SetFullPixel(int pixelX, int pixelY, int color, int tx) {
			for (int i = 0; i < tx; i++) {
				int adr = (i + (pixelY * Width) + pixelX) << 2;
				pixels[adr++] = (byte)(color);
				pixels[adr++] = (byte)(color >> 8);
				pixels[adr++] = (byte)(color >> 16);
				pixels[adr] = 0xFF;
				adr = (i + ((1 + pixelY) * Width) + pixelX) << 2;
				pixels[adr++] = (byte)(color);
				pixels[adr++] = (byte)(color >> 8);
				pixels[adr++] = (byte)(color >> 16);
				pixels[adr] = 0xFF;
			}
		}
	}

	public class RvbColor {
		public int red, green, blue;

		public RvbColor(int compR, int compV, int compB) {
			red = compR;
			green = compV;
			blue = compB;
		}

		public RvbColor(int value) {
			red = value & 0xFF;
			green = (value >> 8) & 0xFF;
			blue = (value >> 16) & 0xFF;
		}

		public int GetColor { get { return red + (green << 8) + (blue << 16); } }
		public int GetColorArgb { get { return red + (green << 8) + (blue << 16) + (255 << 24); } }
	}
}