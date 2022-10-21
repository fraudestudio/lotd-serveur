using System.Threading;

namespace Server.Utils
{
	public class Promise<T> : EventWaitHandle 
	where T : class
	{
		private Result<T>? _value;

        /// <summary>
        ///  A promise that will be resolved with a value of type <typeparamref name="T"/>
        /// </summary>
        public Promise() : base (false, EventResetMode.ManualReset) 
		{
			this._value = null;
		}

        /// <summary>
		///  Resolve the promise with a value of type <typeparamref name="T"/>
		/// </summary>
		/// <param name="val"></param>
		public void SetValue(T val)
		{
			this._value = new Result<T>(val);
			base.Set();
		}
        /// <summary>
        /// Resolve the promise with an error 
        /// </summary>
        /// <param name="err"></param>
        public void SetError(Exception err)
		{
			this._value = new Result<T>(err);
			base.Set();
   		}

        /// <summary>
        /// Wait for the promise to be resolved and return the value
        /// </summary>
        /// <returns></returns>
		public bool Wait(out Result<T> result)
		{
			bool sig = this.WaitOne();
            result = this._value ?? throw new NullReferenceException("Expected a value");
            return sig;
            
		}

        /// <summary>
        /// /// Wait for the promise to be resolved and return the value
        /// </summary>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait</param>
        /// <returns></returns>
        public bool Wait(out Result<T> result, int millisecondsTimeout)
        {
            if (this.WaitOne(millisecondsTimeout))
            {
                result = this._value ?? throw new NullReferenceException("Expected a value");
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }

        /// <summary>
        /// Wait for the promise to be resolved and return the value
        /// </summary>
        /// <param name="timeout">A TimeSpan that represent the number of milliseconds to wait</param>
        /// <returns></returns>
        public bool Wait(out Result<T> result, TimeSpan timeout)
        {
            if (this.WaitOne(timeout))
            {
                result = this._value ?? throw new NullReferenceException("Expected a value");
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }
    }
}