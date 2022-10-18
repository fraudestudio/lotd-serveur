using System.Threading;

namespace Server.Utils
{
	public class Promise<T>  : EventWaitHandle 
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
		public Result<T> Wait()
		{
       
			this.WaitOne();
            return this._value!;
            
		}

        /// <summary>
        /// /// Wait for the promise to be resolved and return the value
        /// </summary>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait</param>
        /// <returns></returns>
        public Result<T> Wait(int millisecondsTimeout)
        {
            this.WaitOne(millisecondsTimeout);
            return this._value!;
        }
        /// <summary>
        /// Wait for the promise to be resolved and return the value
        /// </summary>
        /// <param name="timeout">A TimeSpan that represent the number of milliseconds to wait</param>
        /// <returns></returns>
        public Result<T> Wait(TimeSpan timeout)
        {
            this.WaitOne(timeout);
            return this._value!;
        }

    }
}