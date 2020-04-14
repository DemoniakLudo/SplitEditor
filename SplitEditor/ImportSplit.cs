using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace SplitEditor {
	public partial class ImportSplit : Form {
		private SplitEcran splitEcran;

		private const int SEUIL_LUM_1 = 85;		// 0x40;
		private const int SEUIL_LUM_2 = 170;	// 0x80;

		public ImportSplit(SplitEcran spl) {
			splitEcran = spl;
			InitializeComponent();
		}

		private void bpLire_Click(object sender, EventArgs e) {
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = "Images (*.bmp, *.gif, *.png, *.jpg)|*.bmp;*.gif;*.png;*.jpg|Tous fichiers|*.*";
			DialogResult result = dlg.ShowDialog();
			if (result == DialogResult.OK) {
				try {
					FileStream file = new FileStream(dlg.FileName, FileMode.Open, FileAccess.Read);
					byte[] tabBytes = new byte[file.Length];
					file.Read(tabBytes, 0, tabBytes.Length);
					file.Close();
					bool bitmapOk = false;
					try {
						MemoryStream ms = new MemoryStream(tabBytes);
						Bitmap bmpRead = new Bitmap(ms);
						bmpRead = bmpRead.Clone(new Rectangle(0, 0, bmpRead.Width, bmpRead.Height), PixelFormat.Format32bppArgb);
						if (bmpRead.Width == 384 && bmpRead.Height == 272) {
							LockBitmap locRead = new LockBitmap(bmpRead);
							locRead.LockBits();
							Bitmap bmp = new Bitmap(768, 544);
							for (int y = 0; y < 272; y++)
								for (int x = 0; x < 384; x++) {
									RvbColor p = locRead.GetPixelColor(x, y);
									int indexChoix = (p.red > SEUIL_LUM_2 ? 2 : p.red > SEUIL_LUM_1 ? 1 : 0) + (p.blue > SEUIL_LUM_2 ? 6 : p.blue > SEUIL_LUM_1 ? 3 : 0) + (p.green > SEUIL_LUM_2 ? 18 : p.green > SEUIL_LUM_1 ? 9 : 0);
									locRead.SetPixel(x, y, BitmapCpc.RgbCPC[indexChoix].GetColor);
								}
							locRead.UnlockBits();
							Graphics g = Graphics.FromImage(bmp);
							g.SmoothingMode = SmoothingMode.None;
							g.InterpolationMode = InterpolationMode.NearestNeighbor;
							g.PixelOffsetMode = PixelOffsetMode.HighQuality;
							g.CompositingQuality = CompositingQuality.AssumeLinear;
							g.DrawImage(bmpRead, new Rectangle(0, 0, 768, 544));
							pictureSplit.Image = bmp;
							bitmapOk = true;
							ValideSplit();
						}
						else
							MessageBox.Show("L'image doit avoir une dimension de 384 pixels par 272");

						ms.Dispose();
					}
					catch (Exception ex) {
						MessageBox.Show("Impossible de lire l'image (format inconnu ???)");
					}
					if (bitmapOk) {
					}
				}
				catch (Exception ex) {
					MessageBox.Show(ex.StackTrace, ex.Message);
				}
			}
		}

		private void AddErr(string err) {
			listErr.Items.Add(err);
			listErr.SelectedIndex = listErr.Items.Count - 1;
		}

		private void ValideSplit() {
			listErr.Items.Clear();
			LockBitmap loc = new LockBitmap((Bitmap)pictureSplit.Image);
			loc.LockBits();
			int lastLineWrite = 0;
			for (int y = 0; y < 272; y++) {
				int numSplit = 0, longSplit = 0;
				RvbColor p = loc.GetPixelColor(0, y << 1);
				int curCol = (p.red > SEUIL_LUM_2 ? 2 : p.red > SEUIL_LUM_1 ? 1 : 0) + (p.blue > SEUIL_LUM_2 ? 6 : p.blue > SEUIL_LUM_1 ? 3 : 0) + (p.green > SEUIL_LUM_2 ? 18 : p.green > SEUIL_LUM_1 ? 9 : 0);
				LigneSplit lSplit = splitEcran.LignesSplit[y];
				for (int i = 0; i < 6; i++)
					lSplit.ListeSplit[i].enable = false;

				lSplit.numPen = (int)numPen.Value;
				for (int x = 0; x < 96; x++) {
					int posY = y << 1;
					int posX = (x << 3) + (BitmapCpc.retardMin << 1);
					if (posX < 768) {
						p = loc.GetPixelColor(posX, posY);
						// Recherche la couleur cpc la plus proche
						int indexChoix = (p.red > SEUIL_LUM_2 ? 2 : p.red > SEUIL_LUM_1 ? 1 : 0) + (p.blue > SEUIL_LUM_2 ? 6 : p.blue > SEUIL_LUM_1 ? 3 : 0) + (p.green > SEUIL_LUM_2 ? 18 : p.green > SEUIL_LUM_1 ? 9 : 0);
						if (indexChoix != curCol && longSplit >= 32) {
							lSplit.ListeSplit[numSplit].couleur = curCol;
							lSplit.ListeSplit[numSplit].longueur = longSplit;
							lSplit.ListeSplit[numSplit].enable = true;
							curCol = indexChoix;
							longSplit = 0;
							if (++numSplit > 6)
								AddErr("Plus de 6 splits trouvés sur la ligne " + y + ", seulement 6 seront pris en compte.");
						}
						else
							if (indexChoix != curCol && longSplit > 0)
								AddErr("Split de longueur inférieure à 32 trouvé sur la ligne " + y + ", la longueur sera ramenée à 32 pixels.");

						longSplit += 4;
					}
				}
				if (numSplit < 6) {
					bool sameLine = y > 0;
					for (int k = y - 1; k-- > lastLineWrite; ) {
						for (int j = 6; --j >= 0; )
							if (splitEcran.LignesSplit[k].ListeSplit[j].enable && splitEcran.LignesSplit[k].ListeSplit[j].couleur != curCol) {
								sameLine = false;
								break;
							}
						if (!sameLine)
							break;
					}
					if (y == 0 || numSplit > 0 || !sameLine) {
						lSplit.ListeSplit[numSplit].couleur = curCol;
						lSplit.ListeSplit[numSplit].longueur = longSplit;
						lSplit.ListeSplit[numSplit].enable = true;
						lastLineWrite = y;
					}
				}
			}
			loc.UnlockBits();
		}
	}
}
