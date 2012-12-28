using System.Threading;

using Rhenus.Core.API;
using Rhenus.Core.Properties;

using log4net;


namespace Rhenus.Core
{
    sealed class TaskScheduler : ITaskScheduler
    {
        #region fields 
        private readonly ILog logger = LogManager.GetLogger( "Rhenus.Core.TaskScheduler" );
        #endregion

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
            ThreadPool.QueueUserWorkItem(new WaitCallback(task.Run));
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
    }
}
