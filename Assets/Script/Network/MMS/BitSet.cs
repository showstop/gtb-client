using System;
using System.Collections.Generic;
using System.Text;

namespace Yippee.Net {
	public class BitSet
	{
		public BitSet(int fieldCount)
		{
			bits_ = new byte[fieldCount / bitPerByte_ + 1];
		}

		public void Clear()
		{
			for (int i = 0; i < bits_.Length; ++i)
			{
				bits_[i] = 0;
			}
		}

		public bool Test(int index)
		{
			return 0 != (bits_[index / bitPerByte_] & (byte)(1 << (index % bitPerByte_)));
		}

		public void Set(int index)
		{
			bits_[index / bitPerByte_] |= (byte)(1 << (index % bitPerByte_));
		}

		public void Reset(int index)
		{
			bits_[index / bitPerByte_] &= (byte)(~(1 << (index % bitPerByte_)));
		}

		private const int bitPerByte_ = 8;
		private byte[] bits_;
	}
}
