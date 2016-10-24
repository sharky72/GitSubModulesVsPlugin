namespace GitSubmodules.Enumerations
{
    /// <summary>
    /// This enumeration contains all supported git commands by this application
    /// </summary>
    internal enum SubModuleCommand
    {
        /// <summary>
        /// Command for the fetch of all submodules
        /// </summary>
        AllFetch,

        /// <summary>
        /// Command for the status of all submodules
        /// </summary>
        AllStatus,

        /// <summary>
        /// Command for the registration of all submodules
        /// </summary>
        AllInit,

        /// <summary>
        /// Command for the deregistration of all submodules
        /// </summary>
        AllDeinit,

        /// <summary>
        /// Command for the forced de-registration all submodules (lose changes)
        /// </summary>
        AllDeinitForce,

        /// <summary>
        /// Command for the update of all submodules
        /// </summary>
        AllUpdate,

        /// <summary>
        /// Command for the force update of all submodules (lose changes)
        /// </summary>
        AllUpdateForce,

        /// <summary>
        /// Command for pull origin master for all submodules
        /// </summary>
        AllPullOriginMaster,

        /// <summary>
        /// Command for the status of one submodule
        /// </summary>
        OneStatus,

        /// <summary>
        /// Command for the registration of one submodule
        /// </summary>
        OneInit,

        /// <summary>
        /// Command for the de-registration of one submodule
        /// </summary>
        OneDeinit,

        /// <summary>
        /// Command for the forced de-registration of one submodule (lose changes)
        /// </summary>
        OneDeinitForce,

        /// <summary>
        /// Command for the update of one submodule
        /// </summary>
        OneUpdate,

        /// <summary>
        /// Command for the fore update of one submodule (lose changes)
        /// </summary>
        OneUpdateForce,

        /// <summary>
        /// Command for pull origin master of one submodule
        /// </summary>
        OnePullOriginMaster,

        /// <summary>
        /// Command to check the current installed git version
        /// </summary>
        OtherGitVersion,

        /// <summary>
        /// Command to get a list of all branches of the current repository
        /// </summary>
        OtherBranchList
    }
}
