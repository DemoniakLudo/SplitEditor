﻿using System.IO;

namespace SplitEditor {
	public class BitmapCpc {
		public byte[] bmpCpc = new byte[0x10000];
		private byte[] bufTmp = new byte[0x10000];
		public int[, ,] Palette = new int[96, 272, 17];
		public int[] tabMode = new int[272];

		public SplitEcran splitEcran = new SplitEcran();

		static private int[] paletteStandardCPC = { 1, 24, 20, 6, 26, 0, 2, 7, 10, 12, 14, 16, 18, 22, 1, 14 };
		static private int[] tabMasqueMode0 = { 0xAA, 0x55 };
		static private int[] tabMasqueMode1 = { 0x88, 0x44, 0x22, 0x11 };
		static private int[] tabMasqueMode2 = { 0x80, 0x40, 0x20, 0x10, 0x08, 0x04, 0x02, 0x01 };
		static private int[] tabOctetMode01 = { 0x00, 0x80, 0x08, 0x88, 0x20, 0xA0, 0x28, 0xA8, 0x02, 0x82, 0x0A, 0x8A, 0x22, 0xA2, 0x2A, 0xAA };
		const int maxColsCpc = 96;
		const int maxLignesCpc = 272;
		private int AdrCPC = -1;

		public const int Lum0 = 0x00;
		public const int Lum1 = 0x66;
		public const int Lum2 = 0xFF;

		static public RvbColor[] RgbCPC = new RvbColor[27] {
							new RvbColor( Lum0, Lum0, Lum0),
							new RvbColor( Lum1, Lum0, Lum0),
							new RvbColor( Lum2, Lum0, Lum0),
							new RvbColor( Lum0, Lum0, Lum1),
							new RvbColor( Lum1, Lum0, Lum1),
							new RvbColor( Lum2, Lum0, Lum1),
							new RvbColor( Lum0, Lum0, Lum2),
							new RvbColor( Lum1, Lum0, Lum2),
							new RvbColor( Lum2, Lum0, Lum2),
							new RvbColor( Lum0, Lum1, Lum0),
							new RvbColor( Lum1, Lum1, Lum0),
							new RvbColor( Lum2, Lum1, Lum0),
							new RvbColor( Lum0, Lum1, Lum1),
							new RvbColor( Lum1, Lum1, Lum1),
							new RvbColor( Lum2, Lum1, Lum1),
							new RvbColor( Lum0, Lum1, Lum2),
							new RvbColor( Lum1, Lum1, Lum2),
							new RvbColor( Lum2, Lum1, Lum2),
							new RvbColor( Lum0, Lum2, Lum0),
							new RvbColor( Lum1, Lum2, Lum0),
							new RvbColor( Lum2, Lum2, Lum0),
							new RvbColor( Lum0, Lum2, Lum1),
							new RvbColor( Lum1, Lum2, Lum1),
							new RvbColor( Lum2, Lum2, Lum1),
							new RvbColor( Lum0, Lum2, Lum2),
							new RvbColor( Lum1, Lum2, Lum2),
							new RvbColor( Lum2, Lum2, Lum2)
							};

		private int nbCol = 80;
		public int NbCol { get { return nbCol; } }
		public int TailleX {
			get { return nbCol << 3; }
			set { nbCol = value >> 3; }
		}
		private int nbLig = 200;
		public int NbLig { get { return nbLig; } }
		public int TailleY {
			get { return nbLig << 1; }
			set { nbLig = value >> 1; }
		}
		public bool cpcPlus = false;

		public int Size {
			get {
				GetAdrCpc(TailleY - 2);
				return AdrCPC;
			}
		}

		public BitmapCpc(int tx, int ty, int mode) {
			TailleX = tx;
			TailleY = ty;
			for (int y = 0; y < 272; y++) {
				tabMode[y] = mode;
				LigneSplit l = new LigneSplit();
				for (int j = 0; j < 6; j++)
					l.ListeSplit.Add(new Split());

				splitEcran.LignesSplit.Add(l);
				for (int x = 0; x < 96; x++)
					for (int i = 0; i < 16; i++)
						Palette[x, y, i] = paletteStandardCPC[i]; // ### a reconstruire avec les splits ?
			}
		}

		public void ClearBmp() {
			System.Array.Clear(bmpCpc, 0, bmpCpc.Length);
		}

		public void SetPalette(int x, int y, int entree, int valeur) {
			Palette[x, y, entree] = valeur;
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
				int x = 0, xpos = lSpl.retard >> 2;
				// de x à xpos => faire palette = curPal
				while (x < xpos)
					Palette[x++, y, numCol] = curPal[numCol];

				for (int ns = 0; ns < 6; ns++) {
					Split s = lSpl.GetSplit(ns);
					if (s.enable) {
						xpos += s.longueur >> 2;
						curPal[numCol] = s.couleur;
						if (xpos > 96)
							xpos = 96;

						// de x à xpos => faire palette = curPal
						while (x < xpos)
							Palette[x++, y, numCol] = curPal[numCol];
					}
					else
						break;
				}
				// Terminer ligne
				while (x < 96)
					Palette[x++, y, numCol] = curPal[numCol];

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

		private void GetAdrCpc(int y) {
			AdrCPC = (y >> 4) * nbCol + (y & 14) * 0x400;
			if (y > 255 && (nbCol * nbLig > 0x3FFF))
				AdrCPC += 0x3800;
		}

		public void SetPixelCpc(int xPos, int yPos, int col) {
			GetAdrCpc(yPos);
			byte octet = bmpCpc[AdrCPC + (xPos >> 3)];
			switch (tabMode[yPos >> 1]) {
				case 0:
					octet = (byte)((octet & ~tabMasqueMode0[(xPos >> 2) & 1]) | (tabOctetMode01[col & 15] >> ((xPos >> 2) & 1)));
					break;

				case 1:
				case 3:
					octet = (byte)((octet & ~tabMasqueMode1[(xPos >> 1) & 3]) | (tabOctetMode01[col & 3] >> ((xPos >> 1) & 3)));
					break;

				case 2:
					octet = ((col & 1) == 1) ? (byte)(octet | tabMasqueMode2[xPos & 7]) : (byte)(octet & ~tabMasqueMode2[xPos & 7]);
					break;
			}
			bmpCpc[AdrCPC + (xPos >> 3)] = octet;
		}

		public int GetPixelCpc(int x, int y) {
			GetAdrCpc(y);
			byte octet = bmpCpc[AdrCPC + (x >> 3)];
			switch (tabMode[y >> 1]) {
				case 0:
					octet = (byte)((octet & tabMasqueMode0[(x >> 2) & 1]) << ((x >> 2) & 1));
					break;

				case 1:
				case 3:
					octet = (byte)((octet & tabMasqueMode1[(x >> 1) & 3]) << ((x >> 1) & 3));
					break;

				case 2:
					octet = (byte)((octet & tabMasqueMode2[x & 7]) << (x & 7));
					break;
			}
			int Tx = 4 >> (tabMode[y >> 1] == 3 ? 1 : tabMode[y >> 1]);
			for (int i = 0; i < Tx; i++)
				if (octet == tabOctetMode01[i])
					return i;

			return 0;
		}

		public LockBitmap Render(LockBitmap bmp, int offsetX, int offsetY, bool getPalMode) {
			bmp.LockBits();
			for (int y = 0; y < TailleY; y += 2) {
				int line = offsetY + y;
				GetAdrCpc(line);
				AdrCPC += offsetX >> 3;
				int xBitmap = 0;
				for (int x = 0; x < nbCol; x++) {
					byte Octet = bmpCpc[AdrCPC++];
					switch (tabMode[y >> 1]) {
						case 0:
							bmp.SetFullPixel(xBitmap, y, GetPalCPC(Palette[x, line / 2, (Octet >> 7) + ((Octet & 0x20) >> 3) + ((Octet & 0x08) >> 2) + ((Octet & 0x02) << 2)]), 4);
							bmp.SetFullPixel(xBitmap + 4, y, GetPalCPC(Palette[x, line / 2, ((Octet & 0x40) >> 6) + ((Octet & 0x10) >> 2) + ((Octet & 0x04) >> 1) + ((Octet & 0x01) << 3)]), 4);
							xBitmap += 8;
							break;

						case 1:
						case 3:
							bmp.SetFullPixel(xBitmap, y, GetPalCPC(Palette[x, line / 2, ((Octet >> 7) & 1) + ((Octet >> 2) & 2)]), 2);
							bmp.SetFullPixel(xBitmap + 2, y, GetPalCPC(Palette[x, line / 2, ((Octet >> 6) & 1) + ((Octet >> 1) & 2)]), 2);
							bmp.SetFullPixel(xBitmap + 4, y, GetPalCPC(Palette[x, line / 2, ((Octet >> 5) & 1) + ((Octet >> 0) & 2)]), 2);
							bmp.SetFullPixel(xBitmap + 6, y, GetPalCPC(Palette[x, line / 2, ((Octet >> 4) & 1) + ((Octet << 1) & 2)]), 2);
							xBitmap += 8;
							break;

						case 2:
							for (int i = 8; i-- > 0; ) {
								bmp.SetFullPixel(xBitmap, y, GetPalCPC(Palette[x, line / 2, (Octet >> i) & 1]), 1);
								xBitmap++;
							}
							break;
					}
				}
			}
			bmp.UnlockBits();
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
				tabMode[y] = palStart[startAdr] & 0x03;
				for (int x = 0; x < 96; x++)
					for (int i = 0; i < 16; i++)
						Palette[x, y, i] = plus ? palStart[startAdr + 1 + (i << 1)] + (palStart[startAdr + 2 + (i << 1)] << 8) : palStart[startAdr + i + 1];
			}
		}
	}
}
