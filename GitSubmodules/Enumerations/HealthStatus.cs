using GitSubmodules.Mvvm.Model;

namespace GitSubmodules.Enumerations
{
    /// <summary>
    /// Healthstatus for a <see cref="Submodule"/>
    /// </summary>
    internal enum HealthStatus
    {
        /// <summary>
        /// Status of the <see cref="Submodule"/> is unknown
        /// </summary>
        Unknown,

        /// <summary>
        /// <see cref="Submodule"/> is okay
        /// </summary>
        Okay,

        /// <summary>
        /// Warning, <see cref="Submodule"/> could need your attention
        /// </summary>
        Warning,

        /// <summary>
        /// Error <see cref="Submodule"/> need your investigation
        /// </summary>
        Error
    }
}
