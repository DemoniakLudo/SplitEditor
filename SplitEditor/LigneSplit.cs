namespace SplitEditor {
	public class LigneSplit {
		Split[] spl = new Split[6];
		public int coulOrMode = 0;
		public bool modeCouleur = true;
		public bool enable = false;

		public LigneSplit() {;
			for (int i = 0; i < 6; i++)
				spl[i] = new Split();
		}

		public Split GetSplit(int num) {
			return spl[num];
		}
	}

	public class Split {
		public int longueur;
		public int couleur;
		public bool enable = false;

		public Split() {
			couleur = 0;
			longueur = 32;
		}
	}
}
