namespace SplitEditor {
	partial class Form1 {
		/// <summary>
		/// Variable nécessaire au concepteur.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Nettoyage des ressources utilisées.
		/// </summary>
		/// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Code généré par le Concepteur Windows Form

		/// <summary>
		/// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
		/// le contenu de cette méthode avec l'éditeur de code.
		/// </summary>
		private void InitializeComponent() {
			this.pictureBox = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.numLigne = new System.Windows.Forms.NumericUpDown();
			this.chkSplit1 = new System.Windows.Forms.CheckBox();
			this.chkSplit2 = new System.Windows.Forms.CheckBox();
			this.chkSplit3 = new System.Windows.Forms.CheckBox();
			this.chkSplit4 = new System.Windows.Forms.CheckBox();
			this.chkSplit5 = new System.Windows.Forms.CheckBox();
			this.chkSplit6 = new System.Windows.Forms.CheckBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.largSplit1 = new System.Windows.Forms.NumericUpDown();
			this.largSplit2 = new System.Windows.Forms.NumericUpDown();
			this.largSplit3 = new System.Windows.Forms.NumericUpDown();
			this.largSplit4 = new System.Windows.Forms.NumericUpDown();
			this.largSplit5 = new System.Windows.Forms.NumericUpDown();
			this.largSplit6 = new System.Windows.Forms.NumericUpDown();
			this.chkActif = new System.Windows.Forms.CheckBox();
			this.grpSplit = new System.Windows.Forms.GroupBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.lblColor6 = new System.Windows.Forms.Label();
			this.lblColor5 = new System.Windows.Forms.Label();
			this.lblColor4 = new System.Windows.Forms.Label();
			this.lblColor3 = new System.Windows.Forms.Label();
			this.lblColor2 = new System.Windows.Forms.Label();
			this.lblColor1 = new System.Windows.Forms.Label();
			this.rbMode = new System.Windows.Forms.RadioButton();
			this.rbCouleur = new System.Windows.Forms.RadioButton();
			this.numPenMode = new System.Windows.Forms.NumericUpDown();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numLigne)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.largSplit1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.largSplit2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.largSplit3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.largSplit4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.largSplit5)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.largSplit6)).BeginInit();
			this.grpSplit.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numPenMode)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBox
			// 
			this.pictureBox.Location = new System.Drawing.Point(467, 1);
			this.pictureBox.Name = "pictureBox";
			this.pictureBox.Size = new System.Drawing.Size(768, 544);
			this.pictureBox.TabIndex = 0;
			this.pictureBox.TabStop = false;
			this.pictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseDown);
			this.pictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseMove);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(16, 22);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(38, 15);
			this.label1.TabIndex = 1;
			this.label1.Text = "Ligne";
			// 
			// numLigne
			// 
			this.numLigne.Location = new System.Drawing.Point(66, 22);
			this.numLigne.Maximum = new decimal(new int[] {
            272,
            0,
            0,
            0});
			this.numLigne.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numLigne.Name = "numLigne";
			this.numLigne.Size = new System.Drawing.Size(51, 20);
			this.numLigne.TabIndex = 2;
			this.numLigne.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numLigne.ValueChanged += new System.EventHandler(this.numLigne_ValueChanged);
			// 
			// chkSplit1
			// 
			this.chkSplit1.AutoSize = true;
			this.chkSplit1.Location = new System.Drawing.Point(18, 26);
			this.chkSplit1.Name = "chkSplit1";
			this.chkSplit1.Size = new System.Drawing.Size(60, 19);
			this.chkSplit1.TabIndex = 3;
			this.chkSplit1.Text = "Split 1";
			this.chkSplit1.UseVisualStyleBackColor = true;
			this.chkSplit1.CheckedChanged += new System.EventHandler(this.chkSplit1_CheckedChanged);
			// 
			// chkSplit2
			// 
			this.chkSplit2.AutoSize = true;
			this.chkSplit2.Location = new System.Drawing.Point(18, 105);
			this.chkSplit2.Name = "chkSplit2";
			this.chkSplit2.Size = new System.Drawing.Size(60, 19);
			this.chkSplit2.TabIndex = 3;
			this.chkSplit2.Text = "Split 2";
			this.chkSplit2.UseVisualStyleBackColor = true;
			this.chkSplit2.CheckedChanged += new System.EventHandler(this.chkSplit2_CheckedChanged);
			// 
			// chkSplit3
			// 
			this.chkSplit3.AutoSize = true;
			this.chkSplit3.Location = new System.Drawing.Point(18, 184);
			this.chkSplit3.Name = "chkSplit3";
			this.chkSplit3.Size = new System.Drawing.Size(60, 19);
			this.chkSplit3.TabIndex = 3;
			this.chkSplit3.Text = "Split 3";
			this.chkSplit3.UseVisualStyleBackColor = true;
			this.chkSplit3.CheckedChanged += new System.EventHandler(this.chkSplit3_CheckedChanged);
			// 
			// chkSplit4
			// 
			this.chkSplit4.AutoSize = true;
			this.chkSplit4.Location = new System.Drawing.Point(18, 263);
			this.chkSplit4.Name = "chkSplit4";
			this.chkSplit4.Size = new System.Drawing.Size(60, 19);
			this.chkSplit4.TabIndex = 3;
			this.chkSplit4.Text = "Split 4";
			this.chkSplit4.UseVisualStyleBackColor = true;
			this.chkSplit4.CheckedChanged += new System.EventHandler(this.chkSplit4_CheckedChanged);
			// 
			// chkSplit5
			// 
			this.chkSplit5.AutoSize = true;
			this.chkSplit5.Location = new System.Drawing.Point(18, 342);
			this.chkSplit5.Name = "chkSplit5";
			this.chkSplit5.Size = new System.Drawing.Size(60, 19);
			this.chkSplit5.TabIndex = 3;
			this.chkSplit5.Text = "Split 5";
			this.chkSplit5.UseVisualStyleBackColor = true;
			this.chkSplit5.CheckedChanged += new System.EventHandler(this.chkSplit5_CheckedChanged);
			// 
			// chkSplit6
			// 
			this.chkSplit6.AutoSize = true;
			this.chkSplit6.Location = new System.Drawing.Point(18, 421);
			this.chkSplit6.Name = "chkSplit6";
			this.chkSplit6.Size = new System.Drawing.Size(60, 19);
			this.chkSplit6.TabIndex = 3;
			this.chkSplit6.Text = "Split 6";
			this.chkSplit6.UseVisualStyleBackColor = true;
			this.chkSplit6.CheckedChanged += new System.EventHandler(this.chkSplit6_CheckedChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(84, 26);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(50, 15);
			this.label2.TabIndex = 4;
			this.label2.Text = "Largeur";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(84, 105);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(50, 15);
			this.label3.TabIndex = 4;
			this.label3.Text = "Largeur";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(84, 184);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(50, 15);
			this.label4.TabIndex = 4;
			this.label4.Text = "Largeur";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(84, 263);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(50, 15);
			this.label5.TabIndex = 4;
			this.label5.Text = "Largeur";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(84, 342);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(50, 15);
			this.label6.TabIndex = 4;
			this.label6.Text = "Largeur";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(84, 421);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(50, 15);
			this.label7.TabIndex = 4;
			this.label7.Text = "Largeur";
			// 
			// largSplit1
			// 
			this.largSplit1.Location = new System.Drawing.Point(140, 24);
			this.largSplit1.Maximum = new decimal(new int[] {
            384,
            0,
            0,
            0});
			this.largSplit1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.largSplit1.Name = "largSplit1";
			this.largSplit1.Size = new System.Drawing.Size(51, 20);
			this.largSplit1.TabIndex = 2;
			this.largSplit1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.largSplit1.ValueChanged += new System.EventHandler(this.largSplit1_ValueChanged);
			// 
			// largSplit2
			// 
			this.largSplit2.Location = new System.Drawing.Point(140, 103);
			this.largSplit2.Maximum = new decimal(new int[] {
            384,
            0,
            0,
            0});
			this.largSplit2.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.largSplit2.Name = "largSplit2";
			this.largSplit2.Size = new System.Drawing.Size(51, 20);
			this.largSplit2.TabIndex = 2;
			this.largSplit2.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.largSplit2.ValueChanged += new System.EventHandler(this.largSplit2_ValueChanged);
			// 
			// largSplit3
			// 
			this.largSplit3.Location = new System.Drawing.Point(140, 182);
			this.largSplit3.Maximum = new decimal(new int[] {
            384,
            0,
            0,
            0});
			this.largSplit3.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.largSplit3.Name = "largSplit3";
			this.largSplit3.Size = new System.Drawing.Size(51, 20);
			this.largSplit3.TabIndex = 2;
			this.largSplit3.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.largSplit3.ValueChanged += new System.EventHandler(this.largSplit3_ValueChanged);
			// 
			// largSplit4
			// 
			this.largSplit4.Location = new System.Drawing.Point(140, 261);
			this.largSplit4.Maximum = new decimal(new int[] {
            384,
            0,
            0,
            0});
			this.largSplit4.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.largSplit4.Name = "largSplit4";
			this.largSplit4.Size = new System.Drawing.Size(51, 20);
			this.largSplit4.TabIndex = 2;
			this.largSplit4.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.largSplit4.ValueChanged += new System.EventHandler(this.largSplit4_ValueChanged);
			// 
			// largSplit5
			// 
			this.largSplit5.Location = new System.Drawing.Point(140, 340);
			this.largSplit5.Maximum = new decimal(new int[] {
            384,
            0,
            0,
            0});
			this.largSplit5.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.largSplit5.Name = "largSplit5";
			this.largSplit5.Size = new System.Drawing.Size(51, 20);
			this.largSplit5.TabIndex = 2;
			this.largSplit5.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.largSplit5.ValueChanged += new System.EventHandler(this.largSplit5_ValueChanged);
			// 
			// largSplit6
			// 
			this.largSplit6.Location = new System.Drawing.Point(140, 419);
			this.largSplit6.Maximum = new decimal(new int[] {
            384,
            0,
            0,
            0});
			this.largSplit6.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.largSplit6.Name = "largSplit6";
			this.largSplit6.Size = new System.Drawing.Size(51, 20);
			this.largSplit6.TabIndex = 2;
			this.largSplit6.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.largSplit6.ValueChanged += new System.EventHandler(this.largSplit6_ValueChanged);
			// 
			// chkActif
			// 
			this.chkActif.AutoSize = true;
			this.chkActif.Location = new System.Drawing.Point(123, 23);
			this.chkActif.Name = "chkActif";
			this.chkActif.Size = new System.Drawing.Size(75, 19);
			this.chkActif.TabIndex = 5;
			this.chkActif.Text = "Split actif";
			this.chkActif.UseVisualStyleBackColor = true;
			this.chkActif.CheckedChanged += new System.EventHandler(this.chkActif_CheckedChanged);
			// 
			// grpSplit
			// 
			this.grpSplit.Controls.Add(this.label8);
			this.grpSplit.Controls.Add(this.label9);
			this.grpSplit.Controls.Add(this.label10);
			this.grpSplit.Controls.Add(this.label11);
			this.grpSplit.Controls.Add(this.label12);
			this.grpSplit.Controls.Add(this.label13);
			this.grpSplit.Controls.Add(this.lblColor6);
			this.grpSplit.Controls.Add(this.lblColor5);
			this.grpSplit.Controls.Add(this.lblColor4);
			this.grpSplit.Controls.Add(this.lblColor3);
			this.grpSplit.Controls.Add(this.lblColor2);
			this.grpSplit.Controls.Add(this.lblColor1);
			this.grpSplit.Controls.Add(this.label7);
			this.grpSplit.Controls.Add(this.label6);
			this.grpSplit.Controls.Add(this.label5);
			this.grpSplit.Controls.Add(this.label4);
			this.grpSplit.Controls.Add(this.label3);
			this.grpSplit.Controls.Add(this.label2);
			this.grpSplit.Controls.Add(this.chkSplit6);
			this.grpSplit.Controls.Add(this.chkSplit5);
			this.grpSplit.Controls.Add(this.chkSplit4);
			this.grpSplit.Controls.Add(this.chkSplit3);
			this.grpSplit.Controls.Add(this.chkSplit2);
			this.grpSplit.Controls.Add(this.chkSplit1);
			this.grpSplit.Controls.Add(this.largSplit6);
			this.grpSplit.Controls.Add(this.largSplit5);
			this.grpSplit.Controls.Add(this.largSplit4);
			this.grpSplit.Controls.Add(this.largSplit3);
			this.grpSplit.Controls.Add(this.largSplit2);
			this.grpSplit.Controls.Add(this.largSplit1);
			this.grpSplit.Location = new System.Drawing.Point(4, 59);
			this.grpSplit.Name = "grpSplit";
			this.grpSplit.Size = new System.Drawing.Size(414, 455);
			this.grpSplit.TabIndex = 6;
			this.grpSplit.TabStop = false;
			this.grpSplit.Text = "Splits";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(209, 421);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(50, 15);
			this.label8.TabIndex = 6;
			this.label8.Text = "Couleur";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(209, 342);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(50, 15);
			this.label9.TabIndex = 7;
			this.label9.Text = "Couleur";
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(209, 263);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(50, 15);
			this.label10.TabIndex = 8;
			this.label10.Text = "Couleur";
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(209, 184);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(50, 15);
			this.label11.TabIndex = 9;
			this.label11.Text = "Couleur";
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(209, 105);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(50, 15);
			this.label12.TabIndex = 10;
			this.label12.Text = "Couleur";
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Location = new System.Drawing.Point(209, 26);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(50, 15);
			this.label13.TabIndex = 11;
			this.label13.Text = "Couleur";
			// 
			// lblColor6
			// 
			this.lblColor6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblColor6.Location = new System.Drawing.Point(279, 416);
			this.lblColor6.Name = "lblColor6";
			this.lblColor6.Size = new System.Drawing.Size(35, 35);
			this.lblColor6.TabIndex = 5;
			this.lblColor6.Click += new System.EventHandler(this.lblColor6_Click);
			// 
			// lblColor5
			// 
			this.lblColor5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblColor5.Location = new System.Drawing.Point(279, 336);
			this.lblColor5.Name = "lblColor5";
			this.lblColor5.Size = new System.Drawing.Size(35, 35);
			this.lblColor5.TabIndex = 5;
			this.lblColor5.Click += new System.EventHandler(this.lblColor5_Click);
			// 
			// lblColor4
			// 
			this.lblColor4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblColor4.Location = new System.Drawing.Point(279, 256);
			this.lblColor4.Name = "lblColor4";
			this.lblColor4.Size = new System.Drawing.Size(35, 35);
			this.lblColor4.TabIndex = 5;
			this.lblColor4.Click += new System.EventHandler(this.lblColor4_Click);
			// 
			// lblColor3
			// 
			this.lblColor3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblColor3.Location = new System.Drawing.Point(279, 176);
			this.lblColor3.Name = "lblColor3";
			this.lblColor3.Size = new System.Drawing.Size(35, 35);
			this.lblColor3.TabIndex = 5;
			this.lblColor3.Click += new System.EventHandler(this.lblColor3_Click);
			// 
			// lblColor2
			// 
			this.lblColor2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblColor2.Location = new System.Drawing.Point(279, 96);
			this.lblColor2.Name = "lblColor2";
			this.lblColor2.Size = new System.Drawing.Size(35, 35);
			this.lblColor2.TabIndex = 5;
			this.lblColor2.Click += new System.EventHandler(this.lblColor2_Click);
			// 
			// lblColor1
			// 
			this.lblColor1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblColor1.Location = new System.Drawing.Point(279, 16);
			this.lblColor1.Name = "lblColor1";
			this.lblColor1.Size = new System.Drawing.Size(35, 35);
			this.lblColor1.TabIndex = 5;
			this.lblColor1.Click += new System.EventHandler(this.lblColor1_Click);
			// 
			// rbMode
			// 
			this.rbMode.AutoSize = true;
			this.rbMode.Location = new System.Drawing.Point(204, 34);
			this.rbMode.Name = "rbMode";
			this.rbMode.Size = new System.Drawing.Size(148, 19);
			this.rbMode.TabIndex = 7;
			this.rbMode.TabStop = true;
			this.rbMode.Text = "Changement de mode";
			this.rbMode.UseVisualStyleBackColor = true;
			// 
			// rbCouleur
			// 
			this.rbCouleur.AutoSize = true;
			this.rbCouleur.Location = new System.Drawing.Point(204, 12);
			this.rbCouleur.Name = "rbCouleur";
			this.rbCouleur.Size = new System.Drawing.Size(142, 19);
			this.rbCouleur.TabIndex = 7;
			this.rbCouleur.TabStop = true;
			this.rbCouleur.Text = "Changement de Stylo";
			this.rbCouleur.UseVisualStyleBackColor = true;
			this.rbCouleur.CheckedChanged += new System.EventHandler(this.rbCouleur_CheckedChanged);
			// 
			// numPenMode
			// 
			this.numPenMode.Location = new System.Drawing.Point(358, 20);
			this.numPenMode.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
			this.numPenMode.Name = "numPenMode";
			this.numPenMode.Size = new System.Drawing.Size(51, 20);
			this.numPenMode.TabIndex = 2;
			this.numPenMode.ValueChanged += new System.EventHandler(this.numPenMode_ValueChanged);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1238, 641);
			this.Controls.Add(this.rbCouleur);
			this.Controls.Add(this.rbMode);
			this.Controls.Add(this.grpSplit);
			this.Controls.Add(this.chkActif);
			this.Controls.Add(this.numPenMode);
			this.Controls.Add(this.numLigne);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.pictureBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Form1";
			this.Text = "Split Editor";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numLigne)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.largSplit1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.largSplit2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.largSplit3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.largSplit4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.largSplit5)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.largSplit6)).EndInit();
			this.grpSplit.ResumeLayout(false);
			this.grpSplit.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numPenMode)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		private System.Windows.Forms.PictureBox pictureBox;

		#endregion
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown numLigne;
		private System.Windows.Forms.CheckBox chkSplit1;
		private System.Windows.Forms.CheckBox chkSplit2;
		private System.Windows.Forms.CheckBox chkSplit3;
		private System.Windows.Forms.CheckBox chkSplit4;
		private System.Windows.Forms.CheckBox chkSplit5;
		private System.Windows.Forms.CheckBox chkSplit6;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.NumericUpDown largSplit1;
		private System.Windows.Forms.NumericUpDown largSplit2;
		private System.Windows.Forms.NumericUpDown largSplit3;
		private System.Windows.Forms.NumericUpDown largSplit4;
		private System.Windows.Forms.NumericUpDown largSplit5;
		private System.Windows.Forms.NumericUpDown largSplit6;
		private System.Windows.Forms.CheckBox chkActif;
		private System.Windows.Forms.GroupBox grpSplit;
		private System.Windows.Forms.Label lblColor1;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label lblColor6;
		private System.Windows.Forms.Label lblColor5;
		private System.Windows.Forms.Label lblColor4;
		private System.Windows.Forms.Label lblColor3;
		private System.Windows.Forms.Label lblColor2;
		private System.Windows.Forms.RadioButton rbMode;
		private System.Windows.Forms.RadioButton rbCouleur;
		private System.Windows.Forms.NumericUpDown numPenMode;

	}
}

