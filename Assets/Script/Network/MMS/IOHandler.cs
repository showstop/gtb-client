using System;
using System.Collections.Generic;
using System.Text;

namespace Yippee.Net {

	public abstract class IOHandler
	{
		internal abstract void OnOpened(Link link);
		internal abstract void OnClosed(Link link);
		internal abstract void OnReceived(Link link);
	}
}