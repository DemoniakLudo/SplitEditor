using System.IO;

namespace SplitEditor {
	static public class SauveImage {
		static public int SauveEcran(string NomFic, BitmapCpc bitmap) {
			bool Overscan = (bitmap.NbLig * bitmap.NbCol > 0x3F00);
			byte[] imgCpc = bitmap.bmpCpc;
			int Lg = bitmap.Size;
			BinaryWriter fp = new BinaryWriter(new FileStream(NomFic, FileMode.Create));
			CpcAmsdos entete = CpcSystem.CreeEntete(NomFic, (short)(Overscan ? 0x200 : 0xC000), (short)Lg, 0);
			fp.Write(CpcSystem.AmsdosToByte(entete), 0, 128);
			fp.Write(bitmap.bmpCpc, 0, Lg);
			byte[] pal = new byte[4];
			for (int y = 0; y < 272; y++) {
				for (int x = 0; x < 96; x++) {
					for (int e = 0; e < 4; e++) {
						RvbColor c = bitmap.GetPaletteColor(x, y, e);
						pal[0] = (byte)(bitmap.Palette[x, y, e] & 0xFF);
						pal[1] = (byte)((bitmap.Palette[x, y, e] >> 8) & 0xFF);
						pal[2] = (byte)((bitmap.Palette[x, y, e] >> 16) & 0xFF);
						pal[3] = (byte)((bitmap.Palette[x, y, e] >> 24) & 0xFF);
						fp.Write(pal, 0, 4);
					}
				}
			}
			fp.Close();
			return (Lg);
		}
	}
}
