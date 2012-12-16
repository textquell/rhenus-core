namespace Rhenus.Core.API
{
	/// <summary>
	/// This is the base interface used for all tasks that can be submitted to instances of 
	/// Scheduler.
	/// </summary>
	public interface IKernelRunnable
	{
		/// <summary>
		/// Returns the fully qualified type of the base task that is run by this KernelRunnable.
		/// Many types of runnables wrap around other instances of KernelRunnable or Task. 
		/// This method provides the type of the base task that is being wrapped by any number of 
		/// KernelRunnables, where a given task that wraps another task will return that other 
		/// task's base type such that any wrapping task can be queried and will return the same 
		/// base task type.
		/// </summary>
		/// <returns>
		/// The fully-qualified name of the base task class type.
		/// </returns>
		string GetBaseTaskType ();
		
		/// <summary>
		/// Runs this KernelRunnable. 
		/// If this is run by a Scheduler that support re-try logic, and if an Exception is thrown 
		/// that implements ExceptionRetryStatus then the Scheduler will consult the shouldRetry 
		/// method of the Exception to see if this task should be re-run.
		/// </summary>
		/// <exception cref="System.Exception">if any errors occur</exception>
		void Run ();
	}
}

