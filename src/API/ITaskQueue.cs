using Rhenus.Auth.API;

namespace Rhenus.Core.API
{
	/// <summary>
	/// This interface defines a dependency between tasks, such that tasks are run in the order in 
	/// which they are submitted, and the next task isn't started until the current task has 
	/// completed.
	/// </summary>
	public interface ITaskQueue
	{
		/// <summary>
		/// Adds a task to this dependency queue.
		/// </summary>
		/// <param name='task'>
		/// the KernelRunnable to add
		/// </param>
		/// <param name='owner'>
		/// the Identity that owns the task
		/// </param>
		void AddTask (IKernelRunnable task, IIdentity owner);
	}
}

