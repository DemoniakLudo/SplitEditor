using System;
using System.Runtime.InteropServices;

namespace SplitEditor {
	static public class CpcSystem {
		static public bool CheckAmsdos(byte[] entete) {
			CpcAmsdos enteteAms = new CpcAmsdos();
			int size = Marshal.SizeOf(enteteAms);
			IntPtr ptr = Marshal.AllocHGlobal(size);
			Marshal.Copy(entete, 0, ptr, size);
			enteteAms = (CpcAmsdos)Marshal.PtrToStructure(ptr, typeof(CpcAmsdos));
			Marshal.FreeHGlobal(ptr);
			int checkSum = 0;
			for (int i = 0; i < 67; i++) // Checksum = somme des 67 premiers octets
				checkSum += entete[i];

			return checkSum == enteteAms.CheckSum;
		}
	}

	[StructLayoutAttribute(LayoutKind.Sequential, Pack = 1)]
	public struct CpcAmsdos {
		//
		// Structure d'une entrée AMSDOS
		//
		public byte UserNumber;				// User
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
		public string FileName;				// Nom + extension
		public byte BlockNum;				// Numéro du premier bloc (disquette)
		public byte LastBlock;				// Numéro du dernier bloc (disquette)
		public byte FileType;				// Type de fichier
		public short Length;				// Longueur
		public short Adress;				// Adresse
		public byte FirstBlock;				// Premier bloc de fichier (disquette)
		public short LogicalLength;			// Longueur logique
		public short EntryAdress;			// Point d'entrée
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x24)]
		public string Unused;
		public short RealLength;			// Longueur réelle
		public byte BigLength;				// Longueur réelle (3 octets)
		public short CheckSum;				// CheckSum Amsdos
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x3B)]
		public string Unused2;
	}
}
