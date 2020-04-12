﻿using System;
using System.Collections.Generic;

namespace SplitEditor {
	[Serializable]
	public class SplitEcran {
		private List<LigneSplit> lignesSplit = new List<LigneSplit>();
		public List<LigneSplit> LignesSplit {
			get { return lignesSplit; }
			set { lignesSplit = value; }
		}

		public SplitEcran() {
		}

		public LigneSplit GetLigne(int num) {
			return lignesSplit[num];
		}
	}

	[Serializable]
	public class LigneSplit {
		private List<Split> listeSplit = new List<Split>();
		public List<Split> ListeSplit {
			get { return listeSplit; }
			set { listeSplit = value; }
		}

		public int coulOrMode = 0;
		public bool modeCouleur = true;
		public bool enable = false;

		public LigneSplit() {
		}

		public Split GetSplit(int num) {
			return listeSplit[num];
		}
	}

	[Serializable]
	public class Split {
		public int longueur = 32;
		public int couleur = 0;
		public bool enable = false;

		public Split() {
		}
	}
}
