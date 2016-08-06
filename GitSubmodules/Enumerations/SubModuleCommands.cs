namespace GitSubmodules.Enumerations
{
    /// <summary>
    /// This enumeration contains all supported git commands by this application
    /// </summary>
    internal enum SubModuleCommand
    {
        /// <summary>
        /// Command for the status of all submodules
        /// </summary>
        AllStatus,

        /// <summary>
        /// Command for the registration of all submodules
        /// </summary>
        AllRegister,

        /// <summary>
        /// Command for the deregistration of all submodules
        /// </summary>
        AllDeRegister,

        /// <summary>
        /// Command for the forced de-registration all submodules
        /// </summary>
        AllDeRegisterForce,

        /// <summary>
        /// Command for the update of all submodules
        /// </summary>
        AllUpdate,

        /// <summary>
        /// Command for the pull of latest state of all submodules
        /// </summary>
        AllGetLatest,

        /// <summary>
        /// Command for the registration of one submodule
        /// </summary>
        OneRegister,

        /// <summary>
        /// Command for the de-registration of one submodule
        /// </summary>
        OneDeRegister,

        /// <summary>
        /// Command for the forced de-registration of one submodule
        /// </summary>
        OneDeRegisterForce,

        /// <summary>
        /// Command for the update of one submodule
        /// </summary>
        OneUpdate,

        /// <summary>
        /// Command for the pull latest state of one submodule
        /// </summary>
        OneGetLatest
    }
}
