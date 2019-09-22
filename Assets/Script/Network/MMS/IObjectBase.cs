using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Yippee.Net
{
	public interface IObjectBase
	{
		TypeInfo RuntimeTypeInfo();

		IObjectBase NewInstance();
		IObjectBase Clone();

		void Clear();

		bool Initialized();
		int Size();

		void CopyFrom(IObjectBase from);
		void MergeFrom(IObjectBase from);

		bool Serialize(Stream stream);
		bool Deserialize(Stream stream);
	}
}
