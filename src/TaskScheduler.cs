using Rhenus.Core.API;

using log4net;

namespace Rhenus.Core
{
    class TaskScheduler : ITaskScheduler
    {
        #region ITaskScheduler Member

        public ITaskQueue CreateTaskQueue ()
        {
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
            throw new System.NotImplementedException();
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
