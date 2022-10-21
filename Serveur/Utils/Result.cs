namespace Server.Utils
{
	/// <summary>
	/// Either a value or an exception
	/// </summary>
	public class Result<T>
	where T : class
	{
		private T? _value;
		private Exception? _error;

        /// <summary>
		///	Initalises a new <see cref="Result"> with a value
		/// </summary>
		/// <param name="val"></param>
		public Result(T val)
		{
			this._value = val;
			this._error = null;
		}

		/// <summary>
		/// Initialises a new <see cref="Result"> with an exception
		/// </summary>
		/// <param name="err"></param>
		public Result(Exception err)
		{
			this._value = null;
			this._error = err;	
		}

		/// <summary>
		/// The result is a value
		/// </summary>
		public bool Ok => this._value != null;

		/// <summary>
		/// Get the value or throw the exception
		/// </summary>
		public T Value()
		{
			if (this.Ok)
			{
				return _value;
			}
			else
			{
				throw this._error;   
			}
		}
	}
}