using BoilerplateProject.Debugging;

namespace BoilerplateProject
{
    public class BoilerplateProjectConsts
    {
        public const string LocalizationSourceName = "BoilerplateProject";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = true;


        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public static readonly string DefaultPassPhrase =
            DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "fdc59f7a58e3409b886402892fefd37e";
    }
}
