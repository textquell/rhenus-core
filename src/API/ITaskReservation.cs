namespace Rhenus.Core.API
{
	/// <summary>
	/// This interface manages task reservations. 
	/// Reservations are used to guarantee space in a scheduler for tasks. 
	/// Once a reservation is acquired, the associated tasks will always have space to run. 
	/// Acquiring a reservation does not actually schedule the tasks, so until use is called, the 
	/// tasks will never run. The reservation may be cancelled if it has not yet been run, and no 
	/// cost will be charged to the owner.
	/// 
	/// If this reservation includes tasks scheduled to be run at a specified time, and that time 
	/// has already passed when use is called, then the tasks are run immediately.
	/// </summary>
	public interface ITaskReservation
	{
		/// <summary>
		/// Cancels this reservation, releaseing the reserved space in the scheduler for the 
		/// associated task or tasks.
		/// <exception cref="System.InvalidOperationException">if the reservation has already been 
		/// used or cancelled</exception>
		/// </summary>
		void Cancel ();
		
		/// <summary>
		/// Uses the reservation, scheduling all associated tasks to run.
		/// </summary>
		/// <exception cref="System.InvalidOperationException"></exception>
		void Use ();
	}
}