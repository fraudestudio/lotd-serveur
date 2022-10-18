using System.Threading;

namespace Server.Utils
{
	public class Promise<T>: EventWaitHandle
	{
		private Result<T>? _value;

		public EventWaitHandle() : base (false, EventResetMode.ManualReset)
		{
			this._value = null;
		}

		public void SetValue(T val)
		{
			this._value = new Result(val);
			base.Set();
		}

		public void SetError(Exception err)
		{
			this._value = new Result(err);
			base.Set();
		}

		public Result<T> Wait()
		{
			this.WaitOne();
		}
	}
}