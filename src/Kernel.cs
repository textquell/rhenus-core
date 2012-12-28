using Rhenus.Core.API;
using Rhenus.Core.Properties;

using log4net;

namespace Rhenus.Core
{
    class Kernel
    {
        static void Main(string[] args)
        {
            try
            {
                Kernel kernel = new Kernel();
                kernel.SetUp();
            }
            catch ( System.Exception exception )
            {
                // TODO: Make any user-visible messages localizable
                System.Console.WriteLine( "Exception on booting the Kernel" );
                System.Console.WriteLine( "Exception was " + exception.Message);
            }
        }

        #region HelperClasses
        readonly Settings settings = new Settings();
        readonly ITaskScheduler taskScheduler = new TaskScheduler();
        readonly ILog KernelLogger = LogManager.GetLogger( "Rhenus.Core.Kernel" );
        #endregion

        #region DefaultValues
        public readonly int DEFAULTTHREADCOUNTPROPERTY = 4;
        #endregion

        #region CurrentSettings
        int ThreadCount { get; set; }
        #endregion

        void SetUp ()
        {
            KernelLogger.Debug( "Setting up the kernel..." );

            // check if ThreadCount setting is configurated
            if (settings.ThreadCount.Equals(null)) ThreadCount = DEFAULTTHREADCOUNTPROPERTY;
            else ThreadCount = settings.ThreadCount;

            System.Console.WriteLine( "ThreadCount is " + ThreadCount );

        }
    }
}
