using System;
using System.IO;

namespace SplitEditor {
	static public class GenAsm {
		static string CpcVGA = "TDU\\X]LEMVFW^@_NGORBSZY[JCK";

		static private void WriteDebFile(StreamWriter wr) {
			wr.WriteLine("\tORG #8000");
			wr.WriteLine("\tRUN $");
			wr.WriteLine("");
			wr.WriteLine("	nolist");

			//			wr.WriteLine("	ld hl,#200:ld de,#201:ld bc,#7CFF:ld (hl),c:ldir");
			wr.WriteLine("\tLD\tHL,#C9FB");
			wr.WriteLine("\tLD\t(#38),HL");
			wr.WriteLine("\tEI");
			wr.WriteLine("\tLD\tHL,Overscan");
			wr.WriteLine("\tLD\tB,#BC");
			wr.WriteLine("BclCrtc:");
			wr.WriteLine("\tLD\tA,(HL)");
			wr.WriteLine("\tAND\tA");
			wr.WriteLine("\tJR\tZ,SetPalette");
			wr.WriteLine("\tINC\tHL");
			wr.WriteLine("\tOUT\t(C),A");
			wr.WriteLine("\tINC\tB");
			wr.WriteLine("\tINC\tB");
			wr.WriteLine("\tOUTI");
			wr.WriteLine("\tDEC\tB");
			wr.WriteLine("\tJR\tBclCrtc");
			wr.WriteLine("SetPalette:");
			wr.WriteLine("\tLD\tB,#7F");
			wr.WriteLine("\tLD\tHL,Palette");
			wr.WriteLine("BclPalette:");
			wr.WriteLine("\tOUT\t(C),A");
			wr.WriteLine("\tINC\tB");
			wr.WriteLine("\tOUTI");
			wr.WriteLine("\tINC\tA");
			wr.WriteLine("\tCP\t16");
			wr.WriteLine("\tJR\tNZ,BclPalette");
			wr.WriteLine("WaitVbl:");
			wr.WriteLine("\tLD\tB,#F5");
			wr.WriteLine("\tIN\tA,(C)");
			wr.WriteLine("\tRRA");
			wr.WriteLine("\tJR\tNC,WaitVbl");
			wr.WriteLine("\tEI");
			wr.WriteLine("\tHALT");
			wr.WriteLine("\tHALT");
			wr.WriteLine("\tHALT");
			wr.WriteLine("\tHALT");
			wr.WriteLine("\tHALT");
			wr.WriteLine("\tHALT");
			wr.WriteLine("\tHALT");
			wr.WriteLine("\tDI");
			wr.WriteLine("\tLD\tB,235");
			wr.WriteLine("WaitL0");
			wr.WriteLine("\tNEG");
			wr.WriteLine("\tNEG");
			wr.WriteLine("\tDJNZ\tWaitL0");
			wr.WriteLine("\tNEG");
			wr.WriteLine("\tNEG");
			wr.WriteLine("\tLD\tBC,#7F8D");
			wr.WriteLine("\tOUT\t(C),C");
			wr.WriteLine("DebImage:");
		}

		static private void GenereRetard(StreamWriter wr, int r) {
			int nbNops = r >> 3;
			if (nbNops >= 1024) {
				int bc = (nbNops - 4) / 7;
				wr.WriteLine("\tLD\tBC," + bc.ToString());
				wr.WriteLine("\tDEC\tBC");
				wr.WriteLine("\tLD\tA,B");
				wr.WriteLine("\tOR\tC");
				wr.WriteLine("\tJR\tNZ,$-3");
				wr.WriteLine("\tLD\tB,#7F");
				nbNops -= ((bc * 7) + 4);
			}
			if (nbNops > 7 && nbNops < 1024) {
				int b = (nbNops - 3) >> 2;
				wr.WriteLine("\tLD\tB," + b.ToString());
				wr.WriteLine("\tDJNZ\t$-0");
				wr.WriteLine("\tLD\tB,#7F");
				nbNops -= ((b << 2) + 3);
			}
			if (nbNops >= 4) {
				wr.WriteLine("\tADD\tIX,BC");
				nbNops -= 4;
			}
			if (nbNops >= 2) {
				wr.WriteLine("\tCP\t(HL)");
				nbNops -= 2;
			}
			for (; nbNops-- > 0; )
				wr.WriteLine("\tNOP");
		}

		static private void WriteEndFile(StreamWriter wr, int[, ,] palette) {
			wr.WriteLine("\tJP DebImage");
			wr.WriteLine("");
			wr.WriteLine("Overscan:");
			wr.WriteLine("\tDB\t1,48,2,50,3,#8E,6,34,7,35,12,13,13,0,0,0,0");
			wr.WriteLine("Palette:");
			wr.Write("\tDB\t");
			for (int i = 0; i < 16; i++) {
				int col = CpcVGA[palette[0, 0, i]];
				wr.Write("#" + col.ToString("X2"));
				if (i < 15)
					wr.Write(",");
			}
			wr.WriteLine("");
		}

		static public void CreeAsm(StreamWriter wr, BitmapCpc bmp) {
			WriteDebFile(wr);
			int nbLigneVide = 0;
			int tpsImage = 3;
			int reste = 0;
			for (int y = 0; y < 272; y++) {
				LigneSplit lSpl = bmp.splitEcran.LignesSplit[y];
				if (lSpl.ListeSplit[0].enable) {
					if (nbLigneVide > 0 || reste > 0) {
						GenereRetard(wr, (reste << 3) + (nbLigneVide * 512));
						nbLigneVide = 0;
					}
					int retard = lSpl.retard - BitmapCpc.retardMin;
					GenereRetard(wr, retard);
					wr.WriteLine("\tLD\tC," + lSpl.numPen.ToString());
					wr.WriteLine("\tOUT\t(C),C");
					int c1 = CpcVGA[lSpl.ListeSplit[0].couleur];
					wr.WriteLine("\tLD\tC,#" + c1.ToString("X2"));
					int c2 = CpcVGA[lSpl.ListeSplit[1].couleur];
					int c3 = CpcVGA[lSpl.ListeSplit[2].couleur];
					wr.WriteLine("\tLD\tDE,#" + c2.ToString("X2") + c3.ToString("X2"));
					int c4 = CpcVGA[lSpl.ListeSplit[3].couleur];
					int c5 = CpcVGA[lSpl.ListeSplit[4].couleur];
					wr.WriteLine("\tLD\tHL,#" + c4.ToString("X2") + c5.ToString("X2"));
					int c6 = CpcVGA[lSpl.ListeSplit[5].couleur];
					wr.WriteLine("\tLD\tA,#" + c6.ToString("X2"));
					wr.WriteLine("\tOUT\t(C),C");
					int tpsLine = (retard >> 3) + 20;
					int lg = lSpl.ListeSplit[0].longueur - 32;
					GenereRetard(wr, lg);
					tpsLine += lg >> 3;
					if (lSpl.ListeSplit[1].enable) {
						wr.WriteLine("\tOUT\t(C),D");
						tpsLine += 4;
						if (lSpl.ListeSplit[2].enable) {
							lg = lSpl.ListeSplit[1].longueur - 32;
							GenereRetard(wr, lg);
							tpsLine += (lg >> 3);
						}
					}
					if (lSpl.ListeSplit[2].enable) {
						wr.WriteLine("\tOUT\t(C),E");
						tpsLine += 4;
						if (lSpl.ListeSplit[3].enable) {
							lg = lSpl.ListeSplit[2].longueur - 32;
							GenereRetard(wr, lg);
							tpsLine += (lg >> 3);
						}
					}
					if (lSpl.ListeSplit[3].enable) {
						wr.WriteLine("\tOUT\t(C),H");
						tpsLine += 4;
						if (lSpl.ListeSplit[4].enable) {
							lg = lSpl.ListeSplit[3].longueur - 32;
							GenereRetard(wr, lg);
							tpsLine += (lg >> 3);
						}
					}
					if (lSpl.ListeSplit[4].enable) {
						wr.WriteLine("\tOUT\t(C),L");
						tpsLine += 4;
						if (lSpl.ListeSplit[5].enable) {
							lg = lSpl.ListeSplit[4].longueur - 32;
							GenereRetard(wr, lg);
							tpsLine += (lg >> 3);
						}
					}
					if (lSpl.ListeSplit[5].enable) {
						wr.WriteLine("\tOUT\t(C),A");
						tpsLine += 4;
					}
					reste = 64 - tpsLine;
				}
				else
					nbLigneVide++;

				tpsImage += 64;
			}
			if (nbLigneVide > 0 || reste > 0)
				tpsImage -= ((nbLigneVide * 64) + reste);

			GenereRetard(wr, (19968 - tpsImage) << 3);
			WriteEndFile(wr, bmp.Palette);
		}
	}
}
