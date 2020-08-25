using System;
using System.IO;

namespace SplitEditor {
	static public class GenAsm {
		static string CpcVGA = "TDU\\X]LEMVFW^@_NGORBSZY[JCK";

		static private void WriteDebFile(StreamWriter wr, int nbPixels) {
			wr.WriteLine("	ORG #8000");
			wr.WriteLine("	RUN $");
			wr.WriteLine("");
			wr.WriteLine("	nolist");
			wr.WriteLine("	DI");
			wr.WriteLine("	LD	HL,(#38)");
			wr.WriteLine("	LD	(RestoreIrq+1),HL");
			wr.WriteLine("	LD	HL,#C9FB");
			wr.WriteLine("	LD	(#38),HL");
			wr.WriteLine("	LD	HL,Overscan");
			wr.WriteLine("	LD	B,#BC");
			wr.WriteLine("BclCrtc:");
			wr.WriteLine("	LD	A,(HL)");
			wr.WriteLine("	AND	A");
			wr.WriteLine("	JR	Z,SetPalette");
			wr.WriteLine("	INC	HL");
			wr.WriteLine("	OUT	(C),A");
			wr.WriteLine("	INC	B");
			wr.WriteLine("	INC	B");
			wr.WriteLine("	OUTI");
			wr.WriteLine("	DEC	B");
			wr.WriteLine("	JR	BclCrtc");
			wr.WriteLine("SetPalette:");
			wr.WriteLine("	LD	B,#7F");
			wr.WriteLine("	LD	HL,Palette");
			wr.WriteLine("BclPalette:");
			wr.WriteLine("	OUT	(C),A");
			wr.WriteLine("	INC	B");
			wr.WriteLine("	OUTI");
			wr.WriteLine("	INC	A");
			wr.WriteLine("	CP	16");
			wr.WriteLine("	JR	NZ,BclPalette");
			wr.WriteLine("Boucle:");
			wr.WriteLine("	LD	B,#F5");
			wr.WriteLine("WaitVbl:");
			wr.WriteLine("	IN	A,(C)");
			wr.WriteLine("	RRA");
			wr.WriteLine("	JR	NC,WaitVbl");
			wr.WriteLine("	LD	BC,#F40E");
			wr.WriteLine("	OUT	(C),C");
			wr.WriteLine("	LD	BC,#F6C0");
			wr.WriteLine("	OUT	(C),C");
			wr.WriteLine("	XOR	A");
			wr.WriteLine("	OUT	(C),A");
			wr.WriteLine("	LD	BC,#F792");
			wr.WriteLine("	OUT	(C),C");
			wr.WriteLine("	LD	BC,#F645");
			wr.WriteLine("	OUT	(C),C");
			wr.WriteLine("	LD	B,#F4");
			wr.WriteLine("	IN	A,(c)");
			wr.WriteLine("	LD	bc,#F782");
			wr.WriteLine("	OUT	(C),C");
			wr.WriteLine("	ld	bc,#F600");
			wr.WriteLine("	OUT	(C),C");
			wr.WriteLine("	INC	A");
			wr.WriteLine("	JP	NZ,RestoreIrq");
			wr.WriteLine("	EI");
			wr.WriteLine("	HALT");
			wr.WriteLine("	DI");
			GenereRetard(wr, nbPixels + 15612);
		}

		static private void GenereRetard(StreamWriter wr, int nbPixels, bool crashBC = false) {
			int nbNops = nbPixels >> 3; // 1 Nop = 8 pixels
			if (nbNops >= 1024) {
				int bc = (nbNops - (crashBC ? 2 : 4)) / 7;
				int delai = ((bc * 7) + (crashBC ? 2 : 4));
				wr.WriteLine("	LD	BC," + bc.ToString() + "			; Attendre " + delai.ToString() + " NOPs (7*" + bc.ToString() + "+" + (crashBC ? 2 : 4).ToString() + ")");
				wr.WriteLine("	DEC	BC");
				wr.WriteLine("	LD	A,B");
				wr.WriteLine("	OR	C");
				wr.WriteLine("	JR	NZ,$-3");
				if (!crashBC)
					wr.WriteLine("	LD	B,#7F");

				nbNops -= ((bc * 7) + (crashBC ? 2 : 4));
			}
			if (nbNops > 7 && nbNops < 1024) {
				int b = (nbNops - (crashBC ? 1 : 3)) >> 2;
				int delai = ((b << 2) + (crashBC ? 1 : 3));
				wr.WriteLine("	LD	B," + b.ToString() + "			; Attendre " + delai.ToString() + " NOPs (4*" + b.ToString() + "+" + (crashBC ? 1 : 3).ToString() + ")");
				wr.WriteLine("	DJNZ	$-0");
				if (!crashBC)
					wr.WriteLine("	LD	B,#7F");

				nbNops -= delai;
			}
			if (nbNops >= 4) {
				wr.WriteLine("	ADD	IX,BC			; Attendre 4 NOPs");
				nbNops -= 4;
			}
			if (nbNops >= 2) {
				wr.WriteLine("	CP	(HL)			; Attendre 2 NOPs");
				nbNops -= 2;
			}
			for (; nbNops-- > 0; )
				wr.WriteLine("	NOP				; Attendre 1 NOP");
		}

		static private void WriteEndFile(StreamWriter wr, int[, ,] palette) {
			wr.WriteLine("	JP	Boucle");
			wr.WriteLine("");
			wr.WriteLine("	RestoreIrq:");
			wr.WriteLine("	LD	HL,0");
			wr.WriteLine("	LD	(#38),HL");
			wr.WriteLine("	EI");
			wr.WriteLine("	RET");
			wr.WriteLine("");
			wr.WriteLine("Overscan:");
			wr.WriteLine("	DB	1,48,2,50,3,#8E,6,34,7,35,12,13,13,0,0,0,0");
			wr.WriteLine("Palette:");
			wr.Write("	DB	");
			for (int i = 0; i < 16; i++) {
				int col = CpcVGA[palette[0, 0, i]];
				wr.Write("#" + col.ToString("X2"));
				if (i < 15)
					wr.Write(",");
			}
			wr.WriteLine("");
		}

		static public void CreeAsm(StreamWriter wr, BitmapCpc bmp) {
			WriteDebFile(wr, 32 + BitmapCpc.retardMin);
			int nbLigneVide = 0;
			int tpsImage = 3;
			int reste = 0;
			int oldc2 = 0, oldc3 = 0, oldc4 = 0, oldc5 = 0, oldc6 = 0;
			for (int y = 0; y < 272; y++) {
				LigneSplit lSpl = bmp.splitEcran.LignesSplit[y];
				if (lSpl.ListeSplit[0].enable) {
					int retPrec = 0;
					if (nbLigneVide > 0 || reste > 0) {
						retPrec = (reste << 3) + (nbLigneVide * 512);
						nbLigneVide = 0;
					}
					int retard = lSpl.retard - BitmapCpc.retardMin;
					GenereRetard(wr, retard + retPrec);
					int retSameCol = 0;
					wr.WriteLine("; ---- Ligne " + y.ToString() + " ----");
					wr.WriteLine("	LD	C," + lSpl.numPen.ToString() + "			; (2 NOPs)");
					wr.WriteLine("	OUT	(C),C			; (4 NOPs)");
					int c1 = CpcVGA[lSpl.ListeSplit[0].couleur];
					wr.WriteLine("	LD	C,#" + c1.ToString("X2") + "			; (2 NOPs)");
					int c2 = CpcVGA[lSpl.ListeSplit[1].couleur];
					int c3 = CpcVGA[lSpl.ListeSplit[2].couleur];
					if (c2 != oldc2 || c3 != oldc3)
						wr.WriteLine("	LD	DE,#" + c2.ToString("X2") + c3.ToString("X2") + "		; (3 NOPs)");
					else
						retSameCol += 24;

					int c4 = CpcVGA[lSpl.ListeSplit[3].couleur];
					int c5 = CpcVGA[lSpl.ListeSplit[4].couleur];
					if (c4 != oldc4 || c5 != oldc5)
						wr.WriteLine("	LD	HL,#" + c4.ToString("X2") + c5.ToString("X2") + "		; (3 NOPs)");
					else
						retSameCol += 24;

					int c6 = CpcVGA[lSpl.ListeSplit[5].couleur];
					if (c6 != oldc6)
						wr.WriteLine("	LD	A,#" + c6.ToString("X2") + "			; (2 NOPs)");
					else
						retSameCol += 16;

					if (retSameCol > 0)
						GenereRetard(wr, retSameCol);

					wr.WriteLine("	OUT	(C),C			; (4 NOPs)");
					int tpsLine = (retard >> 3) + 20;
					int lg = lSpl.ListeSplit[0].longueur - 32;
					tpsLine += lg >> 3;
					if (lSpl.ListeSplit[1].enable) {
						if (lg > 0) {
							GenereRetard(wr, lg);
							lg = 0;
						}
						wr.WriteLine("	OUT	(C),D			; (4 NOPs)");
						tpsLine += 4;
						if (lSpl.ListeSplit[2].enable) {
							int lgs = lSpl.ListeSplit[1].longueur - 32;
							GenereRetard(wr, lgs);
							wr.WriteLine("	OUT	(C),E			; (4 NOPs)");
							tpsLine += 4 + (lgs >> 3);
							if (lSpl.ListeSplit[3].enable) {
								lgs = lSpl.ListeSplit[2].longueur - 32;
								GenereRetard(wr, lgs);
								wr.WriteLine("	OUT	(C),H			; (4 NOPs)");
								tpsLine += 4 + (lgs >> 3);
								if (lSpl.ListeSplit[4].enable) {
									lgs = lSpl.ListeSplit[3].longueur - 32;
									GenereRetard(wr, lgs);
									wr.WriteLine("	OUT	(C),L			; (4 NOPs)");
									tpsLine += 4 + (lgs >> 3);
									if (lSpl.ListeSplit[5].enable) {
										lgs = lSpl.ListeSplit[4].longueur - 32;
										GenereRetard(wr, lgs);
										wr.WriteLine("	OUT	(C),A			; (4 NOPs)");
										tpsLine += 4 + (lgs >> 3);
									}
								}
							}
						}
					}
					oldc2 = c2;
					oldc3 = c3;
					oldc4 = c4;
					oldc5 = c5;
					oldc6 = c6;
					reste = 64 - tpsLine + (lg >> 3);
				}
				else {
					nbLigneVide++;
					wr.WriteLine("; ---- Ligne " + y.ToString() + " (vide) ----");
				}

				tpsImage += 64;
			}
			if (nbLigneVide > 0 || reste > 0)
				tpsImage -= ((nbLigneVide * 64) + reste);

			GenereRetard(wr, (17439 - tpsImage) << 3);
			WriteEndFile(wr, bmp.Palette);
		}
	}
}
