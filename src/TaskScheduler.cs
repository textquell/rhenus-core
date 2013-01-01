using System.Threading.Tasks;

using Rhenus.Auth.API;

using Rhenus.Core.API;
using Rhenus.Core.Properties;

using log4net;


namespace Rhenus.Core
{
    sealed class TaskScheduler : ITaskScheduler
    {
        #region fields 
        private readonly ILog logger = LogManager.GetLogger( "Rhenus.Core.TaskScheduler" );
        private readonly TaskFactory factory = new TaskFactory();
        #endregion

        public TaskScheduler ()
        {
            // TODO: Hand over a handle to a profiler so the task scheduler can tell it how fast and effective it is working
            logger.Debug( DebugMessages.TaskScheduler_Constructor_Created);
        }

        #region ITaskScheduler Member
        public ITaskQueue CreateTaskQueue ()
        {
            logger.Debug( DebugMessages.TaskScheduler_CreateNewTaskQueue );
            // Task Queue is a private class inside the task scheduler
            return new TaskQueue();
        }

        public ITaskReservation ReserveTask ( IKernelRunnable task, Auth.API.IIdentity owner )
        {
            throw new System.NotImplementedException();
        }

        public ITaskReservation ReserveTask ( IKernelRunnable task, Auth.API.IIdentity owner, long startTime )
        {
            throw new System.NotImplementedException();
        }

        public IRecurringTaskHandle ScheduleRecurringTask ( IKernelRunnable task, Auth.API.IIdentity owner, System.DateTime startTime, long period )
        {
            throw new System.NotImplementedException();
        }

        public void ScheduleTask ( IKernelRunnable task, Auth.API.IIdentity owner )
        {
            factory.StartNew( new System.Action (task.Run) );
        }

        public void ScheduleTask ( IKernelRunnable task, Auth.API.IIdentity owner, System.DateTime startTime )
        {
            throw new System.NotImplementedException();
        }
        #endregion

        private class TaskQueue: ITaskQueue
        {
            // TODO: Take a System.Collection that is a Queue and make it accept ITask instances. 
            #region ITaskQueue Member
            public void AddTask ( IKernelRunnable task, Auth.API.IIdentity owner )
            {
                throw new System.NotImplementedException();
            }
            #endregion
        }

        private class TaskDetail
        {
            #region Fields
            readonly ILog logger = LogManager.GetLogger( "Rhenus.Core.TaskScheduler.TaskDetail" );
            readonly IKernelRunnable task;
            readonly IIdentity owner;

            // TODO: This field is a long value in the original project and the internal representation of the System.DateTime struct is a long value (System.Int64) too. The original project is making this filed volatile, thus assuming that it may be changed by accessing threads and intending to make sure that each accessing thread works with the latest value of the field. We will have to find a way to make this possible here as well but "volatile" only works with int
            readonly System.Int64 startTime;
            
            readonly int period;
            readonly ITaskQueue queue;
            #endregion

            #region Constructors
            public TaskDetail ( IKernelRunnable task, IIdentity owner, System.DateTime startTime )
                : this( task, owner, startTime, 0 )
            {
                logger.Debug( DebugMessages.TaskScheduler_TaskDetail_Constructor );
            }

            public TaskDetail ( IKernelRunnable task, IIdentity owner, System.DateTime startTime, int period )
            {
                if ( task == null ) throw new System.ArgumentNullException( "task", ExceptionMessages.TaskScheduler_TaskDetail_TaskNullException );
                if ( owner == null ) throw new System.ArgumentNullException( "owner", ExceptionMessages.TaskScheduler_TaskDetail_OwnerNullException );

                logger.Debug( DebugMessages.TaskScheduler_TaskDetail_Constructor );

                this.task = task;
                this.owner = owner;
                this.startTime = startTime.Ticks;
                this.period = period;
                this.queue = null;
            }

            public TaskDetail ( IKernelRunnable task, IIdentity owner, ITaskQueue queue )
            {
                if ( task == null ) throw new System.ArgumentNullException( "task", ExceptionMessages.TaskScheduler_TaskDetail_TaskNullException );
                if ( owner == null ) throw new System.ArgumentNullException( "owner", ExceptionMessages.TaskScheduler_TaskDetail_OwnerNullException );

                logger.Debug( DebugMessages.TaskScheduler_TaskDetail_Constructor );

                this.task = task;
                this.owner = owner;
                this.startTime = System.DateTime.Now.Ticks;
                this.period = 0;
                this.queue = queue;
            }
            #endregion

            public bool isRecurring () { return period != 0; }
        }
    }
}
