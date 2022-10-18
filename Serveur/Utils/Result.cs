namespace Server.Utils
{
	public class Result<T> where T : class
	{
		private T? _value;
		private Exception? _error;

        /// <summary>
		///	Constructor that create a Results with a value and no error
		/// </summary>
		/// <param name="val"></param>
		public Result(T val)
		{
			this._value = val;
			this._error = null;
		}

		/// <summary>
		/// Constructor that create a Error and no Results
		/// </summary>
		/// <param name="err"></param>
		public Result(Exception err)
		{
			this._value = null;
			this._error = err;	
		}

		/// <summary>
		/// check if value is null or not
		/// </summary>
		public bool Ok => this._value != null;

		/// <summary>
		/// A Property that return the Value if it is OK else the Error 
		/// </summary>
		public T Value
		{
			get
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
}