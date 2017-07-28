using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GomeOfLife
{
	public class LifeContainer
	{
		public event EventHandler OnLifeCycleCompleted;

		public void Tick()
		{
			OnLifeCycleCompleted?.Invoke(null, null);
		}
	}
}