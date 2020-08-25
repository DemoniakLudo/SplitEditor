using System.IO;

namespace SplitEditor {
	public class BitmapCpc {
		public byte[] bmpCpc = new byte[0x10000];
		private byte[] bufTmp = new byte[0x10000];
		public int[, ,] Palette = new int[96, 272, 17];
		public int[] tabMode = new int[272];

		public SplitEcran splitEcran = new SplitEcran();

		static private int[] paletteStandardCPC = { 1, 24, 20, 6, 26, 0, 2, 7, 10, 12, 14, 16, 18, 22, 1, 14 };
		private const int maxColsCpc = 96;
		private const int maxLignesCpc = 272;

		private const int Lum0 = 0x00;
		private const int Lum1 = 0x66;
		private const int Lum2 = 0xFF;

		public const int retardMin = -4; // ### 4;

		static public RvbColor[] RgbCPC = {
							new RvbColor( Lum0, Lum0, Lum0),
							new RvbColor( Lum0, Lum0, Lum1),
							new RvbColor( Lum0, Lum0, Lum2),
							new RvbColor( Lum1, Lum0, Lum0),
							new RvbColor( Lum1, Lum0, Lum1),
							new RvbColor( Lum1, Lum0, Lum2),
							new RvbColor( Lum2, Lum0, Lum0),
							new RvbColor( Lum2, Lum0, Lum1),
							new RvbColor( Lum2, Lum0, Lum2),
							new RvbColor( Lum0, Lum1, Lum0),
							new RvbColor( Lum0, Lum1, Lum1),
							new RvbColor( Lum0, Lum1, Lum2),
							new RvbColor( Lum1, Lum1, Lum0),
							new RvbColor( Lum1, Lum1, Lum1),
							new RvbColor( Lum1, Lum1, Lum2),
							new RvbColor( Lum2, Lum1, Lum0),
							new RvbColor( Lum2, Lum1, Lum1),
							new RvbColor( Lum2, Lum1, Lum2),
							new RvbColor( Lum0, Lum2, Lum0),
							new RvbColor( Lum0, Lum2, Lum1),
							new RvbColor( Lum0, Lum2, Lum2),
							new RvbColor( Lum1, Lum2, Lum0),
							new RvbColor( Lum1, Lum2, Lum1),
							new RvbColor( Lum1, Lum2, Lum2),
							new RvbColor( Lum2, Lum2, Lum0),
							new RvbColor( Lum2, Lum2, Lum1),
							new RvbColor( Lum2, Lum2, Lum2)
							};

		private int nbCol = 80;
		private int nbLig = 200;
		private bool cpcPlus = false;

		public BitmapCpc(int tx, int ty, int mode) {
			nbCol = tx >> 3;
			nbLig = ty >> 1;
			for (int y = 0; y < 272; y++) {
				tabMode[y] = mode;
				LigneSplit l = new LigneSplit();
				l.retard = retardMin;
				for (int j = 0; j < 6; j++)
					l.ListeSplit.Add(new Split());

				splitEcran.LignesSplit.Add(l);
				for (int x = 0; x < 96; x++)
					for (int i = 0; i < 16; i++)
						Palette[x, y, i] = paletteStandardCPC[i]; // ### a reconstruire avec les splits ?
			}
		}

		private int AppliquePalette(int x, int y, int xpos, int[] curPal) {
			for (; x < xpos; x++)
				for (int i = 0; i < 16; i++)
					Palette[x, y, i] = curPal[i];

			return x;
		}

		public void CalcPaletteSplit() {
			int[] curPal = new int[16];
			for (int i = 0; i < 16; i++)
				curPal[i] = Palette[0, 0, i];

			// Raz splits
			for (int y = 0; y < 272; y++)
				for (int x = 0; x < 96; x++)
					for (int i = 0; i < 16; i++)
						Palette[x, y, i] = curPal[i];

			int numCol = 0;
			for (int y = 0; y < 272; y++) {
				LigneSplit lSpl = splitEcran.LignesSplit[y];
				numCol = lSpl.numPen;
				int xpos = lSpl.retard >> 2;
				// de x à xpos => faire palette = curPal
				int x = AppliquePalette(0, y, xpos, curPal);
				for (int ns = 0; ns < 6; ns++) {
					Split s = lSpl.GetSplit(ns);
					if (s.enable) {
						xpos += s.longueur >> 2;
						curPal[numCol] = s.couleur;
						if (xpos > 96)
							xpos = 96;

						// de x à xpos => faire palette = curPal
						x = AppliquePalette(x, y, xpos, curPal);
					}
					else
						break;
				}
				// Terminer ligne
				AppliquePalette(x, y, 96, curPal);
				if (y < 271 && lSpl.changeMode)
					tabMode[y + 1] = lSpl.newMode;
			}
		}

		public RvbColor GetPaletteColor(int x, int y, int col) {
			if (cpcPlus)
				return new RvbColor((byte)(((Palette[x, y, col] & 0xF0) >> 4) * 17), (byte)(((Palette[x, y, col] & 0xF00) >> 8) * 17), (byte)((Palette[x, y, col] & 0x0F) * 17));

			return RgbCPC[Palette[x, y, col] < 27 ? Palette[x, y, col] : 0];
		}

		private int GetPalCPC(int c) {
			if (cpcPlus) {
				byte b = (byte)((c & 0x0F) * 17);
				byte r = (byte)(((c & 0xF0) >> 4) * 17);
				byte v = (byte)(((c & 0xF00) >> 8) * 17);
				return (int)(r + (v << 8) + (b << 16) + 0xFF000000);
			}
			return RgbCPC[c < 27 ? c : 0].GetColor;
		}

		public DirectBitmap Render(DirectBitmap bmp, int offsetX, int offsetY, bool getPalMode) {
			for (int y = 0; y < (nbLig << 1); y += 2) {
				int adrCPC = (y >> 4) * nbCol + (y & 14) * 0x400;
				if (y > 255 && (nbCol * nbLig > 0x3FFF))
					adrCPC += 0x3800;

				adrCPC += offsetX >> 3;
				int xBitmap = 0;
				for (int x = 0; x < nbCol; x++) {
					byte octet = bmpCpc[adrCPC + x];
					switch (tabMode[y >> 1]) {
						case 0:
							bmp.SetHorLineDouble(xBitmap, y, 4, GetPalCPC(Palette[x, y >> 1, (octet >> 7) + ((octet & 0x20) >> 3) + ((octet & 0x08) >> 2) + ((octet & 0x02) << 2)]));
							bmp.SetHorLineDouble(xBitmap + 4, y, 4, GetPalCPC(Palette[x, y >> 1, ((octet & 0x40) >> 6) + ((octet & 0x10) >> 2) + ((octet & 0x04) >> 1) + ((octet & 0x01) << 3)]));
							xBitmap += 8;
							break;

						case 1:
						case 3:
							bmp.SetHorLineDouble(xBitmap, y, 2, GetPalCPC(Palette[x, y >> 1, ((octet >> 7) & 1) + ((octet >> 2) & 2)]));
							bmp.SetHorLineDouble(xBitmap + 2, y, 2, GetPalCPC(Palette[x, y >> 1, ((octet >> 6) & 1) + ((octet >> 1) & 2)]));
							bmp.SetHorLineDouble(xBitmap + 4, y, 2, GetPalCPC(Palette[x, y >> 1, ((octet >> 5) & 1) + ((octet >> 0) & 2)]));
							bmp.SetHorLineDouble(xBitmap + 6, y, 2, GetPalCPC(Palette[x, y >> 1, ((octet >> 4) & 1) + ((octet << 1) & 2)]));
							xBitmap += 8;
							break;

						case 2:
							for (int i = 8; i-- > 0; ) {
								bmp.SetPixel(xBitmap, y, GetPalCPC(Palette[x, y >> 1, (octet >> i) & 1]));
								bmp.SetPixel(xBitmap++, y + 1, GetPalCPC(Palette[x, y >> 1, (octet >> i) & 1]));
							}
							break;
					}
				}
			}

			return bmp;
		}

		private void DepactOCP() {
			int PosIn = 0, PosOut = 0;
			int LgOut, CntBlock = 0;

			bmpCpc.CopyTo(bufTmp, 0x10000);
			while (PosOut < 0x4000) {
				if (bufTmp[PosIn] == 'M' && bufTmp[PosIn + 1] == 'J' && bufTmp[PosIn + 2] == 'H') {
					PosIn += 3;
					LgOut = bufTmp[PosIn++];
					LgOut += (bufTmp[PosIn++] << 8);
					CntBlock = 0;
					while (CntBlock < LgOut) {
						if (bufTmp[PosIn] == 'M' && bufTmp[PosIn + 1] == 'J' && bufTmp[PosIn + 2] == 'H')
							break;

						byte a = bufTmp[PosIn++];
						if (a == 1) { // MARKER_OCP
							int c = bufTmp[PosIn++];
							a = bufTmp[PosIn++];
							if (c == 0)
								c = 0x100;

							for (int i = 0; i < c && CntBlock < LgOut; i++) {
								bmpCpc[PosOut++] = a;
								CntBlock++;
							}
						}
						else {
							bmpCpc[PosOut++] = a;
							CntBlock++;
						}
					}
				}
				else
					PosOut = 0x4000;
			}
		}

		private void DepactPK() {
			byte[] Palette = new byte[0x100];

			// Valeurs par défaut
			cpcPlus = false;
			nbCol = 80;
			nbLig = 200;

			//
			//PKSL -> 320x200 STD
			//PKS3 -> 320x200 Mode 3
			//PKSP -> 320x200 PLUS
			//PKVL -> Overscan STD
			//PKVP -> Overscan PLUS
			//

			cpcPlus = (bmpCpc[3] == 'P') || (bmpCpc[2] == 'O');
			bool Overscan = (bmpCpc[2] == 'V') || (bmpCpc[3] == 'V');
			bool Std = (bmpCpc[2] == 'S' && bmpCpc[3] == 'L');
			if (Std)
				for (int i = 0; i < 17; i++)
					Palette[i] = bmpCpc[i + 4];

			PackDepack.Depack(bmpCpc, Std ? 21 : 4, bufTmp);
			System.Array.Copy(bufTmp, bmpCpc, 0x10000);
			if (Overscan) {
				nbCol = maxColsCpc;
				nbLig = maxLignesCpc;
				SetPalette(bmpCpc, 0x600, cpcPlus);
			}
			else {
				if (Std)
					SetPalette(Palette, 0, cpcPlus);
				else
					SetPalette(bmpCpc, 0x17D0, cpcPlus);
			}
		}

		public bool CreateImageFile(string nom) {
			bool Ret = false;
			byte[] Entete = new byte[0x80];
			FileStream fs = new FileStream(nom, FileMode.Open, FileAccess.Read);
			BinaryReader br = new BinaryReader(fs);
			br.Read(Entete, 0, 0x80);
			if (CpcSystem.CheckAmsdos(Entete)) {
				br.Read(bmpCpc, 0, 0x10000);
				if (bmpCpc[0] == 'M' && bmpCpc[1] == 'J' && bmpCpc[2] == 'H')
					DepactOCP();
				else
					if (bmpCpc[0] == 'P' && bmpCpc[1] == 'K' && (bmpCpc[2] == 'O' || bmpCpc[2] == 'V' || bmpCpc[2] == 'S')) {
						DepactPK();
						//GetPalMode = 0;
						//memcpy(p, Palette, sizeof(Palette));
					}
					else
						InitDatas();
				//memcpy(Palette, p, sizeof(Palette));
				//if (GetPalMode) {
				//	if (InitDatas(out Mode))
				//		memcpy(p, Palette, sizeof(Palette));
				//}

				Ret = true;
			}
			br.Close();
			return (Ret);
		}

		private bool InitDatas() {
			bool Ret = false;
			// Si sauvegardé avec ConvImgCpc, alors la palette se trouve dans l'image...
			// CPC OLD, écran standard
			if (bmpCpc[0x7D0] == 0x3A && bmpCpc[0x7D1] == 0xD0 && bmpCpc[0x7D2] == 0xD7 && bmpCpc[0x7D3] == 0xCD) {
				cpcPlus = false;
				nbCol = 80;
				nbLig = 200;
				SetPalette(bmpCpc, 0x17D0, cpcPlus);
				Ret = true;
			}
			else
				// CPC +, écran standard
				if (bmpCpc[0x7D0] == 0xF3 && bmpCpc[0x7D1] == 0x01 && bmpCpc[0x7D2] == 0x11 && bmpCpc[0x7D3] == 0xBC) {
					cpcPlus = true;
					nbCol = 80;
					nbLig = 200;
					SetPalette(bmpCpc, 0x17D0, cpcPlus);
					Ret = true;
				}
				else
					// CPC OLD, écran overscan
					if (bmpCpc[0x611] == 0x21 && bmpCpc[0x612] == 0x47 && bmpCpc[0x613] == 0x08 && bmpCpc[0x614] == 0xCD) {
						cpcPlus = false;
						nbCol = maxColsCpc;
						nbLig = maxLignesCpc;
						SetPalette(bmpCpc, 0x600, cpcPlus);
						Ret = true;
					}
					else
						// CPC +, écran overscan
						if (bmpCpc[0x621] == 0xF3 && bmpCpc[0x622] == 0x01 && bmpCpc[0x623] == 0x11 && bmpCpc[0x624] == 0xBC) {
							cpcPlus = true;
							nbCol = maxColsCpc;
							nbLig = maxLignesCpc;
							SetPalette(bmpCpc, 0x600, cpcPlus);
							Ret = true;
						}
			return (Ret);
		}

		private void SetPalette(byte[] palStart, int startAdr, bool plus) {
			for (int y = 0; y < 272; y++) {
				tabMode[y] = palStart[startAdr] == 6 ? 1 : palStart[startAdr] & 0x03;
				for (int x = 0; x < 96; x++)
					for (int i = 0; i < 16; i++)
						Palette[x, y, i] = plus ? palStart[startAdr + 1 + (i << 1)] + (palStart[startAdr + 2 + (i << 1)] << 8) : palStart[startAdr + i + 1];
			}
		}
	}
}
