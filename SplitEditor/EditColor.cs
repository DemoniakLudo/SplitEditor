using System.Drawing;
using System.Windows.Forms;

namespace SplitEditor {
	public partial class EditColor: Form {
		private Label[] tabColors = new Label[27];
		private Label lblValColor = new Label();
		private TextBox[] tabVal = new TextBox[3];
		private TrackBar[] tabTrack = new TrackBar[3];
		private int valColor;
		public int ValColor { get { return valColor; } }
		public bool isValide;

		public EditColor(int val, int rgbColor, bool cpcPlus) {
			InitializeComponent();
			selColor.BackColor = Color.FromArgb(rgbColor);
			if (cpcPlus) {
				for (int i = 0; i < 3; i++) {
					tabColors[i] = new Label();
					tabVal[i] = new TextBox();
					tabTrack[i] = new TrackBar();
					tabColors[i].Location = new Point(277, 8 + 40 * i);
					tabVal[i].Location = new Point(298, 5 + 40 * i);
					tabTrack[i].Location = new Point(331, 5 + 40 * i);
					tabColors[i].AutoSize = true;
					tabVal[i].Size = new Size(27, 20);
					tabTrack[i].Size = new Size(104, 42);
					tabVal[i].TextChanged += val_TextChanged;
					tabTrack[i].Scroll += track_Scroll;
					tabTrack[i].Maximum = 15;
					tabVal[i].Tag = tabTrack[i].Tag = i;
					tabColors[i].Text = "RVB".Substring(i, 1);
					tabVal[i].Text = (((rgbColor >> (2 - i) * 8) & 0xFF) / 17).ToString();
					Controls.Add(tabColors[i]);
					Controls.Add(tabTrack[i]);
					Controls.Add(tabVal[i]);
				}
			}
			else {
				valColor = val;
				int i = 0;
				for (int y = 0; y < 3; y++)
					for (int x = 0; x < 9; x++) {
						tabColors[i] = new Label();
						tabColors[i].BorderStyle = BorderStyle.FixedSingle;
						tabColors[i].Location = new Point(4 + x * 48, 80 + y * 40);
						tabColors[i].Size = new Size(40, 32);
						tabColors[i].Tag = i;
						tabColors[i].BackColor = Color.FromArgb(BitmapCpc.RgbCPC[i].GetColorArgb);
						tabColors[i].Click += ClickColor;
						tabColors[i].DoubleClick += DblClickColor;
						Controls.Add(tabColors[i++]);
					}
				lblValColor.AutoSize = true;
				lblValColor.Location = new Point(271, 34);
				lblValColor.Text = "=" + val;
				Controls.Add(lblValColor);
			}
		}

		private void ClickColor(object sender, System.EventArgs e) {
			Label colorClick = sender as Label;
			valColor = colorClick.Tag != null ? (int)colorClick.Tag : 0;
			lblValColor.Text = "=" + valColor;
			selColor.BackColor = colorClick.BackColor;
		}

		private void DblClickColor(object sender, System.EventArgs e) {
			ClickColor(sender, e);
			bpValide_Click(sender, e);
		}

		private void NewColor() {
			try {
				int r = int.Parse(tabVal[0].Text);
				int v = int.Parse(tabVal[1].Text);
				int b = int.Parse(tabVal[2].Text);
				valColor = (v << 8) + (b << 4) + r;
				selColor.BackColor = Color.FromArgb(r * 17, v * 17, b * 17);
			}
			catch { }
		}

		private void track_Scroll(object sender, System.EventArgs e) {
			int i = (int)((TrackBar)sender).Tag;
			tabVal[i].Text = tabTrack[i].Value.ToString();
		}

		private void val_TextChanged(object sender, System.EventArgs e) {
			try {
				int i = (int)((TextBox)sender).Tag;
				tabTrack[i].Value = int.Parse(tabVal[i].Text);
				NewColor();
			}
			catch { }
		}

		private void bpValide_Click(object sender, System.EventArgs e) {
			isValide = true;
			Close();
		}

		private void bpAnnule_Click(object sender, System.EventArgs e) {
			Close();
		}
	}
}
