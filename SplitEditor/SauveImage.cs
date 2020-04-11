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
			for (int i = 0; i < 280; i++) {
				for (int e = 0; e < 4; e++) {
					RvbColor c = bitmap.GetPaletteColor(i, e);
					pal[0] = (byte)(bitmap.Palette[i, e] & 0xFF);
					pal[1] = (byte)((bitmap.Palette[i, e] >> 8) & 0xFF);
					pal[2] = (byte)((bitmap.Palette[i, e] >> 16) & 0xFF);
					pal[3] = (byte)((bitmap.Palette[i, e] >> 24) & 0xFF);
					fp.Write(pal, 0, 4);
				}
			}
			fp.Close();
			return (Lg);
		}

		static public void LectEcran(string nomFic, BitmapCpc bitmap) {
			BinaryReader fp = new BinaryReader(new FileStream(nomFic, FileMode.Open));
			byte[] entete = new byte[128];
			byte[] imgCpc = bitmap.bmpCpc;
			int Lg = bitmap.Size;
			fp.Read(entete, 0, 128);
			fp.Read(bitmap.bmpCpc, 0, Lg);
			byte[] pal = new byte[4 * 4 * 280];
			int l = fp.Read(pal, 0, pal.Length);
			for (int i = 0; i < 280; i++) {
				for (int e = 0; e < 4; e++) {
					int k = (e + (i * 4)) * 4;
					if (k < l)
						bitmap.SetPalette(i, e, pal[k] + (pal[k + 1] << 8) + (pal[k + 2] << 16) + (pal[k + 3] << 24));
				}
			}
			fp.Close();
		}
	}
}
