using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Yippee.Net
{
	public class Internal
	{
		public static bool DeserializeUTF8String(BinaryReader input, out String value)
		{
			value = "";
			try
			{
				int size = input.ReadInt32();
				value = Encoding.UTF8.GetString(input.ReadBytes(size)); 
			}
			catch (System.Exception)
			{
				return false;
			}

			return true;
		}

		public static bool DeserializeUnicodeString(BinaryReader input, out String value)
		{
			value = "";
			try
			{
				int size = input.ReadInt32();
				value = Encoding.Unicode.GetString(input.ReadBytes(size));
			}
			catch (System.Exception)
			{
				return false;
			}

			return true;
		}

		public static bool Deserialize(BinaryReader input, out bool value)
		{
			value = false;
			try
			{
				value = input.ReadBoolean();
			}
			catch (System.Exception)
			{
				return false;
			}

			return true;
		}

		public static bool Deserialize(BinaryReader input, out sbyte value)
		{
			value = 0;
			try
			{
				value = input.ReadSByte();
			}
			catch (System.Exception)
			{
				return false;
			}

			return true;
		}

		public static bool Deserialize(BinaryReader input, out short value)
		{
			value = 0;
			try
			{
				value = input.ReadInt16();
			}
			catch (System.Exception)
			{
				return false;
			}

			return true;
		}

		public static bool Deserialize(BinaryReader input, out int value)
		{
			value = 0;
			try
			{
				value = input.ReadInt32();
			}
			catch (System.Exception)
			{
				return false;
			}

			return true;
		}

		public static bool Deserialize(BinaryReader input, out long value)
		{
			value = 0;
			try
			{
				value = input.ReadInt64();
			}
			catch (System.Exception)
			{
				return false;
			}

			return true;
		}

		public static bool Deserialize(BinaryReader input, out byte value)
		{
			value = 0;
			try
			{
				value = input.ReadByte();
			}
			catch (System.Exception)
			{
				return false;
			}

			return true;
		}

		public static bool Deserialize(BinaryReader input, out ushort value)
		{
			value = 0;
			try
			{
				value = input.ReadUInt16();
			}
			catch (System.Exception)
			{
				return false;
			}

			return true;
		}

		public static bool Deserialize(BinaryReader input, out uint value)
		{
			value = 0;
			try
			{
				value = input.ReadUInt32();
			}
			catch (System.Exception)
			{
				return false;
			}

			return true;
		}

		public static bool Deserialize(BinaryReader input, out ulong value)
		{
			value = 0;
			try
			{
				value = input.ReadUInt64();
			}
			catch (System.Exception)
			{
				return false;
			}

			return true;
		}

		public static bool Deserialize(BinaryReader input, out float value)
		{
			value = 0.0f;
			try
			{
				value = input.ReadSingle();
			}
			catch (System.Exception)
			{
				return false;
			}

			return true;
		}

		public static bool Deserialize(BinaryReader input, out double value)
		{
			value = 0.0f;
			try
			{
				value = input.ReadDouble();
			}
			catch (System.Exception)
			{
				return false;
			}

			return true;
		}

		public static bool Deserialize(BinaryReader input, out byte[] value)
		{
			value = new byte[0];
			try
			{
				uint size = input.ReadUInt32();
				value = input.ReadBytes((int)size);
			}
			catch (System.Exception)
			{
				return false;
			}

			return true;
		}

		public static bool Deserialize(BinaryReader input, IObjectBase value)
		{
			try
			{
				value.Deserialize(input.BaseStream);
			}
			catch (System.Exception)
			{
				return false;
			}

			return true;
		}

		public static bool SerializeUTF8String(BinaryWriter output, String value)
		{
			try
			{
				byte[] encoded_str = Encoding.UTF8.GetBytes(value);
				output.Write((uint)encoded_str.Length);
				output.Write(encoded_str);
			}
			catch (System.Exception)
			{
				return false;
			}

			return true;
		}

		public static bool SerializeUnicodeString(BinaryWriter output, String value)
		{
			try
			{
				byte[] encoded_str = Encoding.Unicode.GetBytes(value);
				output.Write((uint)encoded_str.Length);
				output.Write(encoded_str);
			}
			catch (System.Exception)
			{
				return false;
			}

			return true;
		}

		public static bool Serialize(BinaryWriter output, bool value)
		{
			try
			{
				output.Write(value);
			}
			catch (System.Exception)
			{
				return false;
			}

			return true;
		}

		public static bool Serialize(BinaryWriter output, sbyte value)
		{
			try
			{
				output.Write(value);
			}
			catch (System.Exception)
			{
				return false;
			}

			return true;
		}

		public static bool Serialize(BinaryWriter output, short value)
		{
			try
			{
				output.Write(value);
			}
			catch (System.Exception)
			{
				return false;
			}

			return true;
		}

		public static bool Serialize(BinaryWriter output, int value)
		{
			try
			{
				output.Write(value);
			}
			catch (System.Exception)
			{
				return false;
			}

			return true;
		}

		public static bool Serialize(BinaryWriter output, long value)
		{
			try
			{
				output.Write(value);
			}
			catch (System.Exception)
			{
				return false;
			}

			return true;
		}

		public static bool Serialize(BinaryWriter output, byte value)
		{
			try
			{
				output.Write(value);
			}
			catch (System.Exception)
			{
				return false;
			}

			return true;
		}

		public static bool Serialize(BinaryWriter output, ushort value)
		{
			try
			{
				output.Write(value);
			}
			catch (System.Exception)
			{
				return false;
			}

			return true;
		}

		public static bool Serialize(BinaryWriter output, uint value)
		{
			try
			{
				output.Write(value);
			}
			catch (System.Exception)
			{
				return false;
			}

			return true;
		}

		public static bool Serialize(BinaryWriter output, ulong value)
		{
			try
			{
				output.Write(value);
			}
			catch (System.Exception)
			{
				return false;
			}

			return true;
		}

		public static bool Serialize(BinaryWriter output, float value)
		{
			try
			{
				output.Write(value);
			}
			catch (System.Exception)
			{
				return false;
			}

			return true;
		}

		public static bool Serialize(BinaryWriter output, double value)
		{
			try
			{
				output.Write(value);
			}
			catch (System.Exception)
			{
				return false;
			}

			return true;
		}

		public static bool Serialize(BinaryWriter output, byte[] value)
		{
			try
			{
				output.Write((uint)value.Length);
				output.Write(value);
			}
			catch (System.Exception)
			{
				return false;
			}

			return true;
		}

		public static bool Serialize(BinaryWriter output, IObjectBase value)
		{
			try
			{
				value.Serialize(output.BaseStream);
			}
			catch (System.Exception)
			{
				return false;
			}

			return true;
		}

		// Size
		public static int Size(bool value)
		{
			return sizeof(bool);
		}
		public static int Size(sbyte value)
		{
			return sizeof(sbyte);
		}
		public static int Size(short value)
		{
			return sizeof(short);
		}
		public static int Size(int value)
		{
			return sizeof(int);
		}
		public static int Size(long value)
		{
			return sizeof(long);
		}
		public static int Size(byte value)
		{
			return sizeof(byte);
		}
		public static int Size(ushort value)
		{
			return sizeof(ushort);
		}
		public static int Size(uint value)
		{
			return sizeof(uint);
		}
		public static int Size(ulong value)
		{
			return sizeof(ulong);
		}
		public static int Size(float value)
		{
			return sizeof(float);
		}
		public static int Size(double value)
		{
			return sizeof(double);
		}
		public static int SizeUTF8String(String value)
		{
			int size = sizeof(uint);
			size += Encoding.UTF8.GetByteCount(value);

			return size;
		}
		public static int SizeUnicodeString(String value)
		{
			int size = sizeof(uint);
			size += Encoding.Unicode.GetByteCount(value);

			return size;
		}
		public static int Size(IObjectBase message)
		{
			return message.Size();
		}
	}
}
