namespace Rhenus.Core.API
{
	/// <summary>
	/// This interface is used to schedule transactional tasks for immediate, delayed, or periodic 
	/// execution. Transactional tasks are short-lived: typically on the order of a few 10s of 
	/// milliseconds and not longer than the value of the property for transaction timeouts. All 
	/// tasks run through an implementation of ITransactionScheduler will run transactionally, 
	/// and may be re-tried in the event of failure.
	/// 
	/// Many methods will make a best effort to schedule a given task to run, but based on the 
	/// policy of the implementation, the task and its owner, may be unable to accept the given 
	/// task. In this case a TaskRejectedException is thrown. To ensure that a task will be 
	/// accepted, methods are provided to get an ITaskReservation. This is especially useful for 
	/// Service methods working within a transaction that need to ensure that a task will be 
	/// accepted before they can commit.
	/// 
	/// If the result of running a task via the reserveTask or scheduleTask methods is an exception
	/// which implements IExceptionRetryStatus, then its shouldRetry method is called to decide if 
	/// the task should be re-tried. It is up to the scheduler implementation's policy to decide 
	/// how and when tasks are re-run, but all failing tasks run through an ITransactionScheduler 
	/// that wish to be re-tried will eventually be re-run given available resources.
	///
	/// Note that re-try is handled slightly differently for runTask. See the documentation on that
	/// method for more details.
	/// </summary>
	/// TODO: Create a setting for the transaction timeout
	public interface ITransactionScheduler
	{

		/// <summary>
		/// Reserves the ability to run the given task.
		/// </summary>
		/// <returns>
		/// An ITaskReservation for the task
		/// </returns>
		/// <param name='task'>
		/// The IKernelRunnable to execute.
		/// </param>
		/// <param name='owner'>
		/// The entity on who's behalf this task is run.
		/// </param>
		ITaskReservation reserveTask (KernelRunnable task, Identity owner);

		/// <summary>
		/// Reserves the ability to run the given task at a specified point in the future. The 
		/// startTime is a <see cref="System.DateTime"/> struct.
		/// </summary>
		/// <returns>
		/// An ITaskReservation for the task
		/// </returns>
		/// <param name='task'>
		/// The IKernelRunnable to execute.
		/// </param>
		/// <param name='owner'>
		/// The entity on who's behalf this task is run.
		/// </param>
		/// <param name='startTime'>
		/// The time at which to start the task.
		/// </param>
		/// <exception cref="TaskRejectedException"> if a reservation cannot be made.</exception>
		/// TODO: DateTime is not aware of the timezone it is created in. There should be a
		/// mechanism that is transforming Times to UTC time or casts all of the times in the 
		/// connected nodes to the same time zone.
		ITaskReservation reserveTask (IKernelRunnable task, IIdentity owner, DateTime startTime);

		/// <summary>
		/// Schedules a task to run as soon as possible based on the specific scheduler 
		/// implementation.
		/// </summary>
		/// <param name='task'>
		/// The IKernelRunnable to execute.
		/// </param>
		/// <param name='owner'>
		/// The entity on who's behalf the task is run.
		/// </param>
		/// <exception cref="TaskRejectedException">if the given task is not accepted.</exception>
		void scheduleTask (IKernelRunnable task, IIdentity owner);

		/// <summary>
		/// Schedules a task to run at a specified point in the future. The startTime is a 
		/// <see cref="System.DateTime"/> struct.
		/// </summary>
		/// <param name='task'>
		/// The IKernelRunnable to execute.
		/// </param>
		/// <param name='owner'>
		/// The entity on who's behalf this task is run.
		/// </param>
		/// <param name='startTime'>
		/// The time at which to start the task.
		/// </param>
		/// <exception cref="TaskRejectedException">if the given task is not accepted.</exception>
		void scheduleTask (IKernelRunnable task, IIdentity owner, DateTime startTime);

		/// <summary>
		/// Schedules a task to start running at a specified point in the future, and continuing 
		/// running on a regular period starting from that initial point. Unlike the other 
		/// scheduleTask methods, this method will never fail to accept to the task so there is no 
		/// need for a reservation. Note, however, that the task will not actually start executing 
		/// until start is called on the returned IRecurringTaskHandle. 
		/// 
		/// At each execution point the scheduler will make a best effort to run the task, but 
		/// based on available resources scheduling the task may fail. Regardless, the scheduler 
		/// will always try again at the next execution time.
		/// </summary>
		/// <returns>
		/// An IRecurringTaskHandle used to manage the recurring task.
		/// </returns>
		/// <param name='task'>
		/// The IKernelRunnable to execute.
		/// </param>
		/// <param name='owner'>
		/// The entity on who's behalf this task is run.
		/// </param>
		/// <param name='startTime'>
		/// The time at which to start the task.
		/// </param>
		/// <param name='period'>
		/// The length of time in milliseconds between each recurring task execution.
		/// </param>
		/// <exception cref="System.InvalidOperationException">if the period is equal to or less 
		/// than zero.
		/// </exception>
		IRecurringTaskHandle scheduleRecurringTask (IKernelRunnable task, IIdentity owner, 
		                                            DateTime startTime, long period);

		/// <summary>
		/// Runs the given task synchronously, returning when the task has completed or throwing 
		/// an exception if the task fails. It is up to the ITransactionScheduler's implementation 
		/// to decide when to run this task, so the task may be run immediately or it might be 
		/// queued behind waiting tasks. The task may be handed off to another thread of control
		/// for execution. In all cases, the caller will block until the task completes or fails 
		/// permanently.
		/// 
		/// As with all methods of {@code TransactionScheduler}, tasks run with runTask will be run 
		/// transactionally. If the caller is not in an active transaction, then a transaction is 
		/// created to run the task. If the caller is already part of an active transaction, then 
		/// the task is run as part of that transaction, and the owner paramater is ignored.
		/// 
		/// When the caller is not part of an active transaction, then when the given task 
		/// completes it will also attempt to commit. If committing the transaction fails, normal 
		/// re-try behavior is applied. If the task requests to be re-tried, then it will be re-run 
		/// according to the scheduler implementation's policy. In this case, runTask will not 
		/// return until the task finally succeeds, or is no longer re-tried.
		/// 
		/// In the event that the caller is part of an active transaction, then there is no re-try 
		/// applied in the case of a failure, and the transaction is not committed if the task 
		/// completes successfully. This is because the system does not support nested 
		/// transactions, and so the decision to commit or re-try is left to the active 
		/// transaction.
		/// </summary>
		/// <param name="task">
		/// The IKernelRunnable to execute.
		/// </param>
		/// <param name="owner">
		/// The entity on who's behalf this task is run.
		/// </param>
		/// <exception cref="System.Exception">if the task fails and is not re-tried.</exception>
		/// <exception cref="TaskRejectedException">if the given task is not accepted.</exception>
		/// <exception cref="InterruptedException">if the calling thread is interrupted and he 
		/// associated task does not complete.</exception>
		void runTask (IKernelRunnable task, IIdentity owner);

		/// <summary>
		/// Creates a new ITaskQueue to use in scheduling dependent tasks. Each task added to the 
		/// queue will be run in a separate transaction. Re-try is applied to each transaction, 
		/// and the next task in the queue is run only after the current task either completes 
		/// successfully or fails permanently.
		/// </summary>
		/// <returns>
		/// A new task queue.
		/// </returns>
		ITaskQueue createTaskQueue ();

	}
}