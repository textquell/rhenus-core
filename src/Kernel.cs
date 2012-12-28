using Rhenus.Core.API;
using Rhenus.Core.Properties;

using log4net;

namespace Rhenus.Core
{
    sealed class Kernel
    {
        static void Main(string[] args)
        {
            try
            {
                Kernel kernel = new Kernel();
            }
            catch ( System.Exception exception )
            {
                // TODO: Make any user-visible messages localizable
                System.Console.WriteLine( UserMessages.Kernel_BootError );
                System.Console.WriteLine( UserMessages.Kernel_BootError_Description + " " + exception.Message);
            }
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
            KernelLogger.Debug( DebugMessages.Kernel_Setup );

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
    }
}
