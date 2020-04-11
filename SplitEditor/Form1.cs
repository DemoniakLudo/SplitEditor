using System;
using System.Drawing;
using System.Windows.Forms;

namespace SplitEditor {
	public partial class Form1 : Form {
		private Bitmap bmp;
		private LockBitmap bmpLock;
		public BitmapCpc bitmapCpc;
		private int offsetX = 0, offsetY = 0;
		private int numCol = 0;
		private int penWidth = 1;
		private int mode = 1;
		private int taillex = 384; // Résolution image horizontale en pixels mode 2
		private int tailley = 272; // Résolution image verticale en 2*pixels
		private LigneSplit curSplit;

		public Form1() {
			InitializeComponent();
			int tx = pictureBox.Width;
			int ty = pictureBox.Height;
			bmp = new Bitmap(tx, ty);
			pictureBox.Image = bmp;
			bmpLock = new LockBitmap(bmp);
			bitmapCpc = new BitmapCpc(taillex << 1, tailley << 1, mode); // ###
			for (int i = 0; i < 272; i++)
				bitmapCpc.ligneSplit[i] = new LigneSplit();

			Reset();
			Render();
			DisplayLigne();
		}

		private void DisplayLigne() {
			curSplit = bitmapCpc.ligneSplit[(int)numLigne.Value];
			chkActif.Checked = grpSplit.Visible = curSplit.enable;
			rbCouleur.Checked = curSplit.modeCouleur;
			numPenMode.Value = curSplit.coulOrMode;
			numPenMode.Maximum = rbCouleur.Checked ? 15 : 2;
			rbCouleur.Enabled = rbMode.Enabled = numPenMode.Enabled = curSplit.enable;
			if (curSplit.enable) {
				lblColor1.Visible = largSplit1.Visible = chkSplit1.Checked = curSplit.GetSplit(0).enable;
				largSplit1.Value = curSplit.GetSplit(0).longueur;
				lblColor1.BackColor = System.Drawing.Color.FromArgb(BitmapCpc.RgbCPC[curSplit.GetSplit(0).couleur].GetColorArgb);
				lblColor2.Visible = largSplit2.Visible = chkSplit2.Checked = curSplit.GetSplit(1).enable;
				largSplit2.Value = curSplit.GetSplit(1).longueur;
				lblColor2.BackColor = System.Drawing.Color.FromArgb(BitmapCpc.RgbCPC[curSplit.GetSplit(1).couleur].GetColorArgb);
				lblColor3.Visible = largSplit3.Visible = chkSplit3.Checked = curSplit.GetSplit(2).enable;
				largSplit3.Value = curSplit.GetSplit(2).longueur;
				lblColor3.BackColor = System.Drawing.Color.FromArgb(BitmapCpc.RgbCPC[curSplit.GetSplit(2).couleur].GetColorArgb);
				lblColor4.Visible = largSplit4.Visible = chkSplit4.Checked = curSplit.GetSplit(3).enable;
				largSplit4.Value = curSplit.GetSplit(3).longueur;
				lblColor4.BackColor = System.Drawing.Color.FromArgb(BitmapCpc.RgbCPC[curSplit.GetSplit(3).couleur].GetColorArgb);
				lblColor5.Visible = largSplit5.Visible = chkSplit5.Checked = curSplit.GetSplit(4).enable;
				largSplit5.Value = curSplit.GetSplit(4).longueur;
				lblColor5.BackColor = System.Drawing.Color.FromArgb(BitmapCpc.RgbCPC[curSplit.GetSplit(4).couleur].GetColorArgb);
				lblColor6.Visible = largSplit6.Visible = chkSplit6.Checked = curSplit.GetSplit(5).enable;
				largSplit6.Value = curSplit.GetSplit(5).longueur;
				lblColor6.BackColor = System.Drawing.Color.FromArgb(BitmapCpc.RgbCPC[curSplit.GetSplit(5).couleur].GetColorArgb);
			}
		}

		// Changement de la palette
		private void ClickColor(object sender, System.EventArgs e) {
			Label colorClick = sender as Label;
			numCol = colorClick.Tag != null ? (int)colorClick.Tag : 0;
		}

		public void Reset() {
			int col = System.Drawing.SystemColors.Control.ToArgb();
			bmpLock.LockBits();
			for (int x = 0; x < bmpLock.Width; x++)
				for (int y = 0; y < bmpLock.Height; y++)
					bmpLock.SetPixel(x, y, col);

			bmpLock.UnlockBits();
		}

		public void Render() {
			bitmapCpc.Render(bmpLock, offsetX, offsetY, false);
			pictureBox.Refresh();
		}

		#region à voir...
		private void DrawPen(MouseEventArgs e, bool erase = false) {
			int Tx = (4 >> (bitmapCpc.modeCPC == 3 ? 1 : bitmapCpc.modeCPC));
			for (int y = 0; y < penWidth * 2; y += 2)
				for (int x = 0; x < penWidth * Tx; x += Tx) {
					int xReel = x + offsetX + e.X - ((penWidth * Tx) >> 1) + 1;
					int yReel = y + offsetY + e.Y - penWidth + 1;
					if (xReel >= 0 && yReel > 0 && xReel < bitmapCpc.TailleX && yReel < bitmapCpc.TailleY)
						bitmapCpc.SetPixelCpc(xReel, yReel, erase ? 0 : numCol);
				}
			Render();
		}

		private void pictureBox_MouseDown(object sender, MouseEventArgs e) {
			if (e.Button != MouseButtons.None)
				DrawPen(e, e.Button == MouseButtons.Right);
		}

		private void pictureBox_MouseMove(object sender, MouseEventArgs e) {
			if (e.Button != MouseButtons.None)
				DrawPen(e, e.Button == MouseButtons.Right);
		}

		private void bpSave_Click(object sender, EventArgs e) {
			SaveFileDialog saveFileDialog1 = new SaveFileDialog();
			saveFileDialog1.Title = "Sauvegarde Image";
			saveFileDialog1.CheckPathExists = true;
			saveFileDialog1.DefaultExt = "rpa";
			saveFileDialog1.Filter = "Fichiers rpa (*.rpa)|*.rpa|Tous (*.*)|*.*";
			saveFileDialog1.FilterIndex = 2;
			saveFileDialog1.RestoreDirectory = true;
			if (saveFileDialog1.ShowDialog() == DialogResult.OK) {
				SauveImage.SauveEcran(saveFileDialog1.FileName, bitmapCpc);
			}
		}

		private void bpLoad_Click(object sender, EventArgs e) {
			OpenFileDialog openFileDialog1 = new OpenFileDialog {
				Title = "Lecture Image",
				CheckFileExists = true,
				CheckPathExists = true,
				DefaultExt = "rpa",
				Filter = "Fichers rpa  (*.rpa)|*.rpa",
				FilterIndex = 2,
				RestoreDirectory = true,
				ReadOnlyChecked = true,
				ShowReadOnly = true
			};
			if (openFileDialog1.ShowDialog() == DialogResult.OK) {
				SauveImage.LectEcran(openFileDialog1.FileName, bitmapCpc);
				Render();
			}
		}
		#endregion

		private void numLigne_ValueChanged(object sender, EventArgs e) {
			DisplayLigne();
		}

		private void chkActif_CheckedChanged(object sender, EventArgs e) {
			curSplit.enable = chkActif.Checked;
			DisplayLigne();
		}

		private void largSplit1_ValueChanged(object sender, EventArgs e) {
			curSplit.GetSplit(0).longueur = (int)largSplit1.Value;
			DisplayLigne();
		}

		private void largSplit2_ValueChanged(object sender, EventArgs e) {
			curSplit.GetSplit(1).longueur = (int)largSplit2.Value;
			DisplayLigne();
		}

		private void largSplit3_ValueChanged(object sender, EventArgs e) {
			curSplit.GetSplit(2).longueur = (int)largSplit3.Value;
			DisplayLigne();
		}

		private void largSplit4_ValueChanged(object sender, EventArgs e) {
			curSplit.GetSplit(3).longueur = (int)largSplit4.Value;
			DisplayLigne();
		}

		private void largSplit5_ValueChanged(object sender, EventArgs e) {
			curSplit.GetSplit(4).longueur = (int)largSplit5.Value;
			DisplayLigne();
		}

		private void largSplit6_ValueChanged(object sender, EventArgs e) {
			curSplit.GetSplit(5).longueur = (int)largSplit6.Value;
			DisplayLigne();
		}

		private void chkSplit1_CheckedChanged(object sender, EventArgs e) {
			curSplit.GetSplit(0).enable = chkSplit1.Checked;
			DisplayLigne();
		}

		private void chkSplit2_CheckedChanged(object sender, EventArgs e) {
			if (chkSplit1.Checked) {
				curSplit.GetSplit(1).enable = chkSplit2.Checked;
				DisplayLigne();
			}
			else
				chkSplit2.Checked = false;
		}

		private void chkSplit3_CheckedChanged(object sender, EventArgs e) {
			if (chkSplit2.Checked) {
				curSplit.GetSplit(2).enable = chkSplit3.Checked;
				DisplayLigne();
			}
			else
				chkSplit3.Checked = false;
		}

		private void chkSplit4_CheckedChanged(object sender, EventArgs e) {
			if (chkSplit3.Checked) {
				curSplit.GetSplit(3).enable = chkSplit4.Checked;
				DisplayLigne();
			}
			else
				chkSplit4.Checked = false;
		}

		private void chkSplit5_CheckedChanged(object sender, EventArgs e) {
			if (chkSplit4.Checked) {
				curSplit.GetSplit(4).enable = chkSplit5.Checked;
				DisplayLigne();
			}
			else
				chkSplit5.Checked = false;
		}

		private void chkSplit6_CheckedChanged(object sender, EventArgs e) {
			if (chkSplit5.Checked) {
				curSplit.GetSplit(5).enable = chkSplit6.Checked;
				DisplayLigne();
			}
			else
				chkSplit6.Checked = false;
		}

		private void lblColor1_Click(object sender, EventArgs e) {
			EditColor ed = new EditColor(curSplit.GetSplit(0).couleur, BitmapCpc.RgbCPC[curSplit.GetSplit(0).couleur].GetColorArgb, false);
			ed.ShowDialog(this);
			if (ed.isValide) {
				curSplit.GetSplit(0).couleur = ed.ValColor;
				DisplayLigne();
			}
		}

		private void lblColor2_Click(object sender, EventArgs e) {
			EditColor ed = new EditColor(curSplit.GetSplit(1).couleur, BitmapCpc.RgbCPC[curSplit.GetSplit(1).couleur].GetColorArgb, false);
			ed.ShowDialog(this);
			if (ed.isValide) {
				curSplit.GetSplit(1).couleur = ed.ValColor;
				DisplayLigne();
			}
		}

		private void lblColor3_Click(object sender, EventArgs e) {
			EditColor ed = new EditColor(curSplit.GetSplit(2).couleur, BitmapCpc.RgbCPC[curSplit.GetSplit(2).couleur].GetColorArgb, false);
			ed.ShowDialog(this);
			if (ed.isValide) {
				curSplit.GetSplit(2).couleur = ed.ValColor;
				DisplayLigne();
			}
		}

		private void lblColor4_Click(object sender, EventArgs e) {
			EditColor ed = new EditColor(curSplit.GetSplit(3).couleur, BitmapCpc.RgbCPC[curSplit.GetSplit(3).couleur].GetColorArgb, false);
			ed.ShowDialog(this);
			if (ed.isValide) {
				curSplit.GetSplit(3).couleur = ed.ValColor;
				DisplayLigne();
			}
		}

		private void lblColor5_Click(object sender, EventArgs e) {
			EditColor ed = new EditColor(curSplit.GetSplit(4).couleur, BitmapCpc.RgbCPC[curSplit.GetSplit(4).couleur].GetColorArgb, false);
			ed.ShowDialog(this);
			if (ed.isValide) {
				curSplit.GetSplit(4).couleur = ed.ValColor;
				DisplayLigne();
			}
		}

		private void lblColor6_Click(object sender, EventArgs e) {
			EditColor ed = new EditColor(curSplit.GetSplit(5).couleur, BitmapCpc.RgbCPC[curSplit.GetSplit(5).couleur].GetColorArgb, false);
			ed.ShowDialog(this);
			if (ed.isValide) {
				curSplit.GetSplit(5).couleur = ed.ValColor;
				DisplayLigne();
			}
		}

		private void rbCouleur_CheckedChanged(object sender, EventArgs e) {
			curSplit.modeCouleur = rbCouleur.Checked;
			DisplayLigne();
		}

		private void numPenMode_ValueChanged(object sender, EventArgs e) {
			curSplit.coulOrMode = (int)numPenMode.Value;
		}
	}
}
