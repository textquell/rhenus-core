using Rhenus.Core.API;
using Rhenus.Core.Properties;

using log4net;

namespace Rhenus.Core
{
    sealed class Kernel: System.IDisposable
    {
        static void Main(string[] args)
        {
            using ( Kernel kernel = new Kernel() );
        }

        #region Helper Classes
        readonly Settings settings = new Settings();
        readonly ITaskScheduler taskScheduler = new TaskScheduler();
        readonly ILog KernelLogger = LogManager.GetLogger( "Rhenus.Core.Kernel" );
        #endregion

        #region Default Values
        public const int DEFAULTTHREADCOUNTPROPERTY = 4;
        public const int DEFAULTTIMEOUTPROPERTY = 15 * 60 * 1000; // = 15 minutes
        #endregion

        #region Current Settings
        public int ThreadCount { get; set; }
        bool isShuttingDown;
        #endregion

        Kernel ()
        {
            KernelLogger.Info( InfoMessages.Kernel_Initializing );

            // check if ThreadCount setting is configurated and set ThreadCount accordingly
            if (settings.ThreadCount.Equals(null)) ThreadCount = DEFAULTTHREADCOUNTPROPERTY;
            else ThreadCount = settings.ThreadCount;

            System.Console.WriteLine( UserMessages.Kernel_ThreadCount + " " + ThreadCount );

            isShuttingDown = false;
        }

        #region Shutdown Handling
        void Shutdown ()
        {
            if ( isShuttingDown ) return;

            KernelLogger.Info(InfoMessages.Kernel_ShutDownCalled);

            startShutDownTimeOut( DEFAULTTIMEOUTPROPERTY );
            isShuttingDown = true;
        }

        void startShutDownTimeOut ( int timeout )
        {
            System.Timers.Timer timer = new System.Timers.Timer( timeout );
            timer.Start();
            timer.Elapsed += delegate( object sender, System.Timers.ElapsedEventArgs e )
            {
                // forcefully terminate the application
                System.Environment.Exit( 1 );
            };
        }
        #endregion

        #region IDisposable Member

        public void Dispose ()
        {
            // TODO: Create a mechanism here that is shutting down the server in an orderly way. At least it should be logged that the GC attempts to finalize the kernel
            KernelLogger.Info(InfoMessages.Kernel_GettingDisposed);
            Shutdown();
        }

        #endregion
    }
}
