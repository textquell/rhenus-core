using Rhenus.Core.API;
using Rhenus.Core.Properties;

using log4net;

namespace Rhenus.Core
{
    sealed class Kernel
    {
        static int Main(string[] args)
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

        #region HelperClasses
        readonly Settings settings = new Settings();
        readonly ITaskScheduler taskScheduler = new TaskScheduler();
        readonly ILog KernelLogger = LogManager.GetLogger( "Rhenus.Core.Kernel" );
        #endregion

        #region DefaultValues
        public int DEFAULTTHREADCOUNTPROPERTY = 4;
        #endregion

        #region CurrentSettings
        public int ThreadCount { get; set; }
        bool isShuttingDown = false;
        #endregion

        Kernel ()
        {
            KernelLogger.Debug( DebugMessages.Kernel_Setup );

            // check if ThreadCount setting is configurated
            if (settings.ThreadCount.Equals(null)) ThreadCount = DEFAULTTHREADCOUNTPROPERTY;
            else ThreadCount = settings.ThreadCount;

            System.Console.WriteLine( UserMessages.Kernel_ThreadCount + " " + ThreadCount );
        }
    }
}
