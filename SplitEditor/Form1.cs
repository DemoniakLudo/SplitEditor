using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

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
		private LigneSplit curLigneSplit;

		public Form1() {
			InitializeComponent();
			int tx = pictureBox.Width;
			int ty = pictureBox.Height;
			bmp = new Bitmap(tx, ty);
			pictureBox.Image = bmp;
			bmpLock = new LockBitmap(bmp);
			bitmapCpc = new BitmapCpc(taillex << 1, tailley << 1, mode); // ###
			Reset();
			Render();
			DisplayLigne();
		}

		private void DisplayLigne() {
			curLigneSplit = bitmapCpc.splitEcran.GetLigne((int)numLigne.Value - 1);
			chkActif.Checked = grpSplit.Visible = curLigneSplit.enable;
			rbCouleur.Checked = curLigneSplit.modeCouleur;
			numPenMode.Value = curLigneSplit.coulOrMode;
			numPenMode.Maximum = rbCouleur.Checked ? 15 : 2;
			rbCouleur.Enabled = rbMode.Enabled = numPenMode.Enabled = curLigneSplit.enable;
			if (curLigneSplit.enable) {
				lblColor0.Visible = largSplit0.Visible = chkSplit0.Checked = curLigneSplit.GetSplit(0).enable;
				largSplit0.Value = curLigneSplit.GetSplit(0).longueur;
				lblColor0.BackColor = System.Drawing.Color.FromArgb(BitmapCpc.RgbCPC[curLigneSplit.GetSplit(0).couleur].GetColorArgb);
				lblColor1.Visible = largSplit1.Visible = chkSplit1.Checked = curLigneSplit.GetSplit(1).enable;
				largSplit1.Value = curLigneSplit.GetSplit(1).longueur;
				lblColor1.BackColor = System.Drawing.Color.FromArgb(BitmapCpc.RgbCPC[curLigneSplit.GetSplit(1).couleur].GetColorArgb);
				lblColor2.Visible = largSplit2.Visible = chkSplit2.Checked = curLigneSplit.GetSplit(2).enable;
				largSplit2.Value = curLigneSplit.GetSplit(2).longueur;
				lblColor2.BackColor = System.Drawing.Color.FromArgb(BitmapCpc.RgbCPC[curLigneSplit.GetSplit(2).couleur].GetColorArgb);
				lblColor3.Visible = largSplit3.Visible = chkSplit3.Checked = curLigneSplit.GetSplit(3).enable;
				largSplit3.Value = curLigneSplit.GetSplit(3).longueur;
				lblColor3.BackColor = System.Drawing.Color.FromArgb(BitmapCpc.RgbCPC[curLigneSplit.GetSplit(3).couleur].GetColorArgb);
				lblColor4.Visible = largSplit4.Visible = chkSplit4.Checked = curLigneSplit.GetSplit(4).enable;
				largSplit4.Value = curLigneSplit.GetSplit(4).longueur;
				lblColor4.BackColor = System.Drawing.Color.FromArgb(BitmapCpc.RgbCPC[curLigneSplit.GetSplit(4).couleur].GetColorArgb);
				lblColor5.Visible = largSplit5.Visible = chkSplit5.Checked = curLigneSplit.GetSplit(5).enable;
				largSplit5.Value = curLigneSplit.GetSplit(5).longueur;
				lblColor5.BackColor = System.Drawing.Color.FromArgb(BitmapCpc.RgbCPC[curLigneSplit.GetSplit(5).couleur].GetColorArgb);
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
			SaveFileDialog dlg = new SaveFileDialog();
			dlg.Filter = "Fichiers SplitEditor (*.xml)|*.xml";
			DialogResult result = dlg.ShowDialog();
			if (result == DialogResult.OK) {
				FileStream file = File.Open(dlg.FileName, FileMode.Create);
#if TRY_CATCH
				try {
#endif
				new XmlSerializer(typeof(SplitEcran)).Serialize(file, bitmapCpc.splitEcran);
#if TRY_CATCH
				}
				catch (Exception ex) {
					MessageBox.Show(ex.StackTrace, ex.Message);
				}
#endif
				file.Close();
			}

		}

		private void bpLoad_Click(object sender, EventArgs e) {
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = "Fichiers SplitEditor (*.xml)|*.xml";
			DialogResult result = dlg.ShowDialog();
			if (result == DialogResult.OK) {
				FileStream file = File.Open(dlg.FileName, FileMode.Open);
#if TRY_CATCH
				try {
#endif
				SplitEcran spl = (SplitEcran)new XmlSerializer(typeof(SplitEcran)).Deserialize(file);
				bitmapCpc.splitEcran = spl;
#if TRY_CATCH
				}
				catch (Exception ex) {
					MessageBox.Show(ex.StackTrace, ex.Message);
				}
#endif
				file.Close();
				DisplayLigne();
			}
		}
		#endregion

		private void numLigne_ValueChanged(object sender, EventArgs e) {
			DisplayLigne();
		}

		private void chkActif_CheckedChanged(object sender, EventArgs e) {
			curLigneSplit.enable = chkActif.Checked;
			DisplayLigne();
		}

		private void rbCouleur_CheckedChanged(object sender, EventArgs e) {
			curLigneSplit.modeCouleur = rbCouleur.Checked;
			DisplayLigne();
		}

		private void numPenMode_ValueChanged(object sender, EventArgs e) {
			curLigneSplit.coulOrMode = (int)numPenMode.Value;
		}

		private void ChangeLargeur(int index, NumericUpDown val) {
			curLigneSplit.GetSplit(index).longueur = ((int)val.Value) & 0xFFC;
			DisplayLigne();
		}

		private void largSplit1_ValueChanged(object sender, EventArgs e) {
			ChangeLargeur(0, largSplit0);
		}

		private void largSplit2_ValueChanged(object sender, EventArgs e) {
			ChangeLargeur(1, largSplit1);
		}

		private void largSplit3_ValueChanged(object sender, EventArgs e) {
			ChangeLargeur(2, largSplit2);
		}

		private void largSplit4_ValueChanged(object sender, EventArgs e) {
			ChangeLargeur(3, largSplit3);
		}

		private void largSplit5_ValueChanged(object sender, EventArgs e) {
			ChangeLargeur(4, largSplit4);
		}

		private void largSplit6_ValueChanged(object sender, EventArgs e) {
			ChangeLargeur(5, largSplit5);
		}

		private void EnableSplit(int index, CheckBox chk, CheckBox prec) {
			if (prec == null || prec.Checked) {
				curLigneSplit.GetSplit(index).enable = chk.Checked;
				DisplayLigne();
			}
			else
				chk.Checked = false;
		}

		private void chkSplit1_CheckedChanged(object sender, EventArgs e) {
			EnableSplit(0, chkSplit0, null);
		}

		private void chkSplit2_CheckedChanged(object sender, EventArgs e) {
			EnableSplit(1, chkSplit1, chkSplit0);
		}

		private void chkSplit3_CheckedChanged(object sender, EventArgs e) {
			EnableSplit(2, chkSplit2, chkSplit1);
		}

		private void chkSplit4_CheckedChanged(object sender, EventArgs e) {
			EnableSplit(3, chkSplit3, chkSplit2);
		}

		private void chkSplit5_CheckedChanged(object sender, EventArgs e) {
			EnableSplit(4, chkSplit4, chkSplit3);
		}

		private void chkSplit6_CheckedChanged(object sender, EventArgs e) {
			EnableSplit(5, chkSplit5, chkSplit4);
		}

		private void ChangeColor(int index) {
			Split curXSplit = curLigneSplit.GetSplit(index);
			EditColor ed = new EditColor(curXSplit.couleur, BitmapCpc.RgbCPC[curXSplit.couleur].GetColorArgb, false);
			ed.ShowDialog(this);
			if (ed.isValide) {
				curXSplit.couleur = ed.ValColor;
				DisplayLigne();
			}
		}

		private void lblColor1_Click(object sender, EventArgs e) {
			ChangeColor(0);
		}

		private void lblColor2_Click(object sender, EventArgs e) {
			ChangeColor(1);
		}

		private void lblColor3_Click(object sender, EventArgs e) {
			ChangeColor(2);
		}

		private void lblColor4_Click(object sender, EventArgs e) {
			ChangeColor(3);
		}

		private void lblColor5_Click(object sender, EventArgs e) {
			ChangeColor(4);
		}

		private void lblColor6_Click(object sender, EventArgs e) {
			ChangeColor(5);
		}
	}
}
