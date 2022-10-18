namespace Server.Utils
{
	public class Result<T>
	{
		private T? _value;
		private Exception? _error;

		public Result(T val)
		{
			this._result = val;
			this._error = null;
		}

		public Result(Exception err)
		{
			this._result = null;
			this._error = err;
		}

		public bool Ok => this._result != null;

		public T Value => this._value.Value;

		public void Throw()
		{
			if (!this.Ok())
			{
				throw this._error;
			}
		}
	}
}