using System;

namespace Rhenus.Core.API
{
	/// <summary>
	/// The managing instance for tasks.
	/// This interface is used to run tasks that may take an arbitrarily long time to complete, 
	/// but are expected to complete eventually. These tasks may be scheduled to run immediately 
	/// or after some delay, possibly re-executing at regular intervals.
	/// 
	/// Based on an implementation's policy, a task and its owner, that task may not be accepted 
	/// for execution. In this case a TaskRejectedException is thrown. To ensure that a task 
	/// will be accepted methods are provided to get a TaskReservation. This is especially 
	/// useful for Service methods working within a transaction that need to ensure that a task 
	/// will be accepted before they can commit.
	/// 
	/// Note that, because the tasks submitted through this interface may run any length of time,
	/// there are no guarantees about when a given task will start. If a task is scheduled to run
	/// immediately, or at some point in the future, then this means that the scheduler will try 
	/// to acquire resources to run the task at that point. It may still be some indefinite length
	/// of time before the task can actually be run. 
	/// </summary>
	public interface ITaskScheduler
	{
		/// <summary>
		/// Creates a new TaskQueue to use in scheduling dependent tasks. 
		/// Once a given task has completed the next task will be submitted to the scheduler to 
		/// run.
		/// </summary>
		/// <returns>
		/// A new TaskQueue.
		/// </returns>
		ITaskQueue CreateTaskQueue ();

		/// <summary>
		/// Reserves the ability to run the given task.
		/// </summary>
		/// <returns>
		/// A TaskReservation for the task.
		/// </returns>
		/// <param name='task'>
		/// The KernelRunnable to execute.
		/// </param>
		/// <param name='owner'>
		/// The entity on who's behalf this task is run.
		/// </param>
		/// <exception cref="TaskRejectedException">if a reservation cannot be made</exception>
		ITaskReservation ReserveTask (IKernelRunnable task, IIdentity owner);

		/// <summary>
		/// Reserves the ability to run the given task at a specified point in the future.
		/// The startTime is a System.DateTime struct.
		/// </summary>
		/// <returns>
		/// a TaskReservation for the task
		/// </returns>
		/// <param name='task'>
		/// the KernelRunnable to execute
		/// </param>
		/// <param name='owner'>
		/// the entity on who's behalf this task is run
		/// </param>
		/// <param name='startTime'>
		/// the time at which to start the task
		/// </param>
		/// <exception cref="TaskRejectedException">if a reservation cannot be made</exception>
		ITaskReservation ReserveTask (IKernelRunnable task, IIdentity owner, long startTime);

		/// <summary>
		/// Schedules a task to start running at a specified point in the future, and continuing 
		/// running on a regular period starting from that initial point. Unlike the other 
		/// scheduleTask methods, this method will never fail to accept to the task so there is no 
		/// need for a reservation. Note, however, that the task will not actually start executing 
		/// until <see cref="IRecurringTaskHandle.Start()"/> is called on the returned IRecurringTaskHandle. At each execution point the 
		/// scheduler will make a best effort to run the task, but based on available resources 
		/// scheduling the task may fail. Regardless, the scheduler will always try again at the 
		/// next execution time.
		/// </summary>
		/// <returns>
		/// a RecurringTaskHandle used to manage the recurring task
		/// </returns>
		/// <param name='task'>
		/// the KernelRunnable to execute
		/// </param>
		/// <param name='owner'>
		/// the entity on who's behalf this task is run
		/// </param>
		/// <param name='startTime'>
		/// the time at which to start the task.
		/// </param>
		/// <param name='period'>
		/// the length of time in milliseconds between each recurring task execution
		/// </param>
		/// <exception cref="IllegalArgumentException">if period is less than or equal to zero
		/// </exception>
		IRecurringTaskHandle ScheduleRecurringTask (IKernelRunnable task, IIdentity owner, 
		                                            DateTime startTime, long period);

		/// <summary>
		/// Schedules a task to run as soon as possible based on the specific scheduler 
		/// implementation.
		/// </summary>
		/// <param name='task'>
		/// the KernelRunnable to execute
		/// </param>
		/// <param name='owner'>
		/// the entity on who's behalf this task is run
		/// </param>
		/// <exception cref="TaskRejectedException">if a reservation cannot be made</exception>
		void ScheduleTask (IKernelRunnable task, IIdentity owner);

		/// <summary>
		/// Schedules a task to run at a specified point in the future. The startTime is a 
		/// System.DateTime struct. If the starting time has already passed, then the task 
		/// is run immediately.
		/// </summary>
		/// <param name='task'>
		/// The KernelRunnable to execute.
		/// </param>
		/// <param name='owner'>
		/// The entity on who's behalf this task is run.
		/// </param>
		/// <param name='startTime'>
		/// The time at which to start the task.
		/// </param>
		/// <exception cref="TaskRejectedException">if a reservation cannot be made
		/// </exception>
		void ScheduleTask (IKernelRunnable task, IIdentity owner, DateTime startTime);
	}
}