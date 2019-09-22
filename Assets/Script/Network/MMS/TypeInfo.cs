using System;
using System.Collections.Generic;
using System.Text;

namespace Yippee.Net
{
	public class TypeInfo
	{
		private uint id_;
		private String name_;

		public TypeInfo(uint id, String name)
		{
			id_ = id;
			name_ = name;
		}

		public uint Id
		{
			get
			{
				return id_;
			}
		}
		
		public String Name
		{
			get
			{
				return name_;
			}
		}
	}
}
