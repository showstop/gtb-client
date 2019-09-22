using System;
using System.Collections.Generic;
using System.Text;

namespace Yippee.Net {
	public class Buffer
	{
		private byte[] data_ = null;
		uint len_ = 0;
		uint pos_ = 0;

		public Buffer(byte[] data, uint len, uint pos)
		{
			if (data.Length < len)
			{
				throw new System.Exception();
			}

			if (data.Length < pos)
			{
				throw new System.Exception();
			}

			if (data.Length < len + pos)
			{
				throw new System.Exception();
			}

			data_ = data;
			len_ = len;
			pos_ = pos;
		}

		~Buffer()
		{
			try
			{
				Clear();
			}
			catch (System.Exception)
			{
			}
		}

		public void Clear()
		{
			len_ = 0;
			pos_ = 0;
			data_ = null;
		}

		public void Consume(uint len)
		{
			if (len >= len_)
			{
				len_ = 0;
				pos_ = 0;
			}
			else
			{
				len_ -= len;
				pos_ += len;
			}

		}

		public void Reserve(uint len)
		{
			byte[] new_data = new byte[len];
			Array.Copy(data_, new_data, data_.Length < len ? data_.Length : (int)len);
			data_ = new_data;
		}

		public void SetLength(uint len)
		{
			Reserve(len);
			len_ = len;
		}

		public uint Len
		{
			get
			{
				return len_;
			}
		}

		public bool IsEmpty()
		{
			return len_ == 0;
		}

		public byte[] Data
		{
			get
			{
				return data_;
			}
		}
	}
}