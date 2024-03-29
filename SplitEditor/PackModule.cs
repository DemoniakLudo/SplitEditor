﻿using System.Windows.Forms;

public partial class PackModule : Form {
	private const int MAX_OFFSET_ZX0 = 0x7F80;
	private const int MAX_SCALE = 50;
	private const int QTY_BLOCKS = 10000;
	private const int BUFFER_SIZE = 0x10000;

	private ProgressBar progressBar;
	private Block ghost_root;
	private Block[] dead_array;
	private int dead_array_size = 0;
	private int output_index, bit_index, bit_mask;
	private bool backtrack;
	private int input_index;
	private byte last_byte, bit_value;

	public PackModule() {
		progressBar = new ProgressBar();
		SuspendLayout();
		progressBar.Size = new System.Drawing.Size(408, 27);
		ClientSize = new System.Drawing.Size(414, 32);
		Controls.Add(progressBar);
		FormBorderStyle = FormBorderStyle.FixedToolWindow;
		Text = "Compression ZX0 de l'image...";
		ResumeLayout(false);
	}

	private Block allocate(int bits, int index, int offset, int length, Block chain) {
		Block ptr;
		if (ghost_root != null) {
			ptr = ghost_root;
			ghost_root = ptr.ghost_chain;
			if (ptr.chain != null) {
				if (--ptr.chain.references == 0) {
					ptr.chain.ghost_chain = ghost_root;
					ghost_root = ptr.chain;
				}
			}
		}
		else {
			if (dead_array_size == 0) {
				dead_array = new Block[QTY_BLOCKS];
				for (int i = 0; i < QTY_BLOCKS; i++)
					dead_array[i] = new Block();

				dead_array_size = QTY_BLOCKS;
			}
			ptr = dead_array[--dead_array_size];
		}
		ptr.bits = bits;
		ptr.index = index;
		ptr.offset = offset;
		ptr.length = length;
		if (chain != null)
			chain.references++;

		ptr.chain = chain;
		ptr.references = 0;
		return ptr;
	}

	private void assign(ref Block ptr, Block chain) {
		chain.references++;
		if (ptr != null) {
			if (--ptr.references == 0) {
				ptr.ghost_chain = ghost_root;
				ghost_root = ptr;
			}
		}
		ptr = chain;
	}

	private int elias_gamma_bits(int value) {
		int bits = 1;
		while (value > 1) {
			bits += 2;
			value >>= 1;
		}
		return bits;
	}

	private void write_bit(int value, byte[] output_data) {
		if (backtrack) {
			if (value != 0)
				output_data[output_index - 1] |= 1;

			backtrack = false;
		}
		else {
			if (bit_mask == 0) {
				bit_mask = 128;
				bit_index = output_index;
				output_data[output_index++] = 0;
			}
			if (value != 0)
				output_data[bit_index] |= (byte)bit_mask;

			bit_mask >>= 1;
		}
	}

	private void write_interlaced_elias_gammaZX0(int value, byte[] output_data) {
		int i;
		for (i = 2; i <= value; i <<= 1)
			;
		i >>= 1;
		while ((i >>= 1) > 0) {
			write_bit(0, output_data);
			write_bit(value & i, output_data);
		}
		write_bit(1, output_data);
	}

	public int PackZX0(byte[] input_data, int input_size, byte[] output_data, int output_size) {
		Show();
		int bits, length;
		int max_offset = input_size - 1 > MAX_OFFSET_ZX0 ? MAX_OFFSET_ZX0 : input_size - 1 < 1 ? 1 : input_size - 1;
		int dots = 0;

		Block[] last_literal = new Block[max_offset + 1];
		Block[] last_match = new Block[max_offset + 1];
		Block[] tabOptimal = new Block[input_size + 1];
		int[] match_length = new int[max_offset + 1];
		int[] best_length = new int[input_size + 2];
		best_length[2] = 2;
		assign(ref last_match[1], allocate(-1, -1, 1, 0, null));
		for (int index = 0; index < input_size; index++) {
			int best_length_size = 2;
			max_offset = index > MAX_OFFSET_ZX0 ? MAX_OFFSET_ZX0 : index < 1 ? 1 : index;
			for (int offset = 1; offset <= max_offset; offset++) {
				if (index != 0 && index >= offset && input_data[index] == input_data[index - offset]) {
					if (last_literal[offset] != null) {
						length = index - last_literal[offset].index;
						bits = last_literal[offset].bits + 1 + elias_gamma_bits(length);
						assign(ref last_match[offset], allocate(bits, index, offset, length, last_literal[offset]));
						if (tabOptimal[index] == null || tabOptimal[index].bits > bits)
							assign(ref tabOptimal[index], last_match[offset]);
					}
					if (++match_length[offset] > 1) {
						if (best_length_size < match_length[offset]) {
							bits = tabOptimal[index - best_length[best_length_size]].bits + elias_gamma_bits(best_length[best_length_size] - 1);
							do {
								best_length_size++;
								int bits2 = tabOptimal[index - best_length_size].bits + elias_gamma_bits(best_length_size - 1);
								if (bits2 <= bits) {
									best_length[best_length_size] = best_length_size;
									bits = bits2;
								}
								else
									best_length[best_length_size] = best_length[best_length_size - 1];
							}
							while (best_length_size < match_length[offset]);
						}
						length = best_length[match_length[offset]];
						bits = tabOptimal[index - length].bits + 8 + elias_gamma_bits((offset - 1) / 128 + 1) + elias_gamma_bits(length - 1);
						if (last_match[offset] == null || last_match[offset].index != index || last_match[offset].bits > bits) {
							assign(ref last_match[offset], allocate(bits, index, offset, length, tabOptimal[index - length]));
							if (tabOptimal[index] == null || tabOptimal[index].bits > bits)
								assign(ref tabOptimal[index], last_match[offset]);
						}
					}
				}
				else {
					match_length[offset] = 0;
					if (last_match[offset] != null) {
						length = index - last_match[offset].index;
						bits = last_match[offset].bits + 1 + elias_gamma_bits(length) + length * 8;
						assign(ref last_literal[offset], allocate(bits, index, 0, length, last_match[offset]));
						if (tabOptimal[index] == null || tabOptimal[index].bits > bits)
							assign(ref tabOptimal[index], last_literal[offset]);
					}
				}
			}
			if (index * MAX_SCALE / input_size > dots) {
				progressBar.Value = 100 * ++dots / MAX_SCALE;
				Application.DoEvents();
			}
		}
		Block prev, next = null, optimal = tabOptimal[input_size - 1];
		output_size = (optimal.bits + 18 + 7) / 8;
		while (optimal != null) {
			prev = optimal.chain;
			optimal.chain = next;
			next = optimal;
			optimal = prev;
		}
		output_index = 0;
		bit_mask = 0;
		int last_offset = 1;
		int input_index = 0;
		bool first = true;
		for (optimal = next.chain; optimal != null; optimal = optimal.chain) {
			if (optimal.offset == 0) {
				if (first)
					first = false;
				else
					write_bit(0, output_data);

				write_interlaced_elias_gammaZX0(optimal.length, output_data);
				for (int i = 0; i < optimal.length; i++)
					output_data[output_index++] = input_data[input_index++];
			}
			else
				if (optimal.offset == last_offset) {
				write_bit(0, output_data);
				write_interlaced_elias_gammaZX0(optimal.length, output_data);
				input_index += optimal.length;
			}
			else {
				write_bit(1, output_data);
				write_interlaced_elias_gammaZX0((optimal.offset - 1) / 128 + 1, output_data);
				output_data[output_index++] = (byte)((255 - ((optimal.offset - 1) % 128)) << 1);
				backtrack = true;
				write_interlaced_elias_gammaZX0(optimal.length - 1, output_data);
				input_index += optimal.length;
				last_offset = optimal.offset;
			}
		}
		write_bit(1, output_data);
		write_interlaced_elias_gammaZX0(256, output_data);
		Hide();
		return output_size;
	}

	private byte read_byte(byte[] input_data) {
		last_byte = input_data[input_index++];
		return last_byte;
	}

	private int read_bit(byte[] input_data) {
		if (backtrack) {
			backtrack = false;
			return last_byte & 1;
		}
		bit_mask >>= 1;
		if (bit_mask == 0) {
			bit_mask = 128;
			bit_value = read_byte(input_data);
		}
		return (bit_value & bit_mask) != 0 ? 1 : 0;
	}

	private int read_interlaced_elias_gamma(byte[] input_data) {
		int value = 1;
		while (read_bit(input_data) == 0) {
			value = value << 1 | read_bit(input_data);
		}
		return value;
	}

	private void write_byte(byte value, byte[] output_data) {
		output_data[output_index++] = value;
	}

	private void write_bytes(int offset, int length, byte[] output_data) {
		while (length-- > 0) {
			int i = output_index - offset;
			write_byte(output_data[i >= 0 ? i : BUFFER_SIZE + i], output_data);
		}
	}

	public int DepackZX0(int classic_mode, byte[] input_data, byte[] output_data) {
		int last_offset = 1;
		int length;

		input_index = output_index = 0;
		bit_mask = 0;
		backtrack = false;

	COPY_LITERALS:
		length = read_interlaced_elias_gamma(input_data);
		for (int i = 0; i < length; i++)
			write_byte(read_byte(input_data), output_data);
		if (read_bit(input_data) != 0)
			goto COPY_FROM_NEW_OFFSET;

		length = read_interlaced_elias_gamma(input_data);
		write_bytes(last_offset, length, output_data);
		if (read_bit(input_data) == 0)
			goto COPY_LITERALS;

	COPY_FROM_NEW_OFFSET:
		last_offset = read_interlaced_elias_gamma(input_data);
		if (last_offset == 256)
			return output_index;

		last_offset = last_offset * 128 - (read_byte(input_data) >> 1);
		backtrack = true;
		length = read_interlaced_elias_gamma(input_data) + 1;
		write_bytes(last_offset, length, output_data);
		if (read_bit(input_data) != 0)
			goto COPY_FROM_NEW_OFFSET;
		else
			goto COPY_LITERALS;
	}
}

public class Block {
	public Block chain, ghost_chain;
	public int bits, index, offset, length, references;
}
