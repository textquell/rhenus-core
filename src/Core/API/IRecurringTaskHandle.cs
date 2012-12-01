namespace Rhenus.Core.API
{
	interface IRecurringTaskHandle
	{
		/// <summary>
		/// Cancels the associated recurring task. A recurring task may be cancelled before it is started.
		/// </summary>
		/// <exception cref="System.InvalidOperationException">if the task has already been cancelled</exception>
		void Cancel ();
		/// <summary>
		/// Starts the associated recurring task. A recurring task will not start running until this method is called.
		/// </summary>
		/// <exception cref="System.InvalidOperationException"> if the task has already been started, or has been cancelled</exception>
		void Start ();
	}
}