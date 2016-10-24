using System.Diagnostics;
using GitSubmodules.Enumerations;
using GitSubmodules.Mvvm.Model;

namespace GitSubmodules.Helper
{
    /// <summary>
    /// Helper class to easier handle with Git source control
    /// </summary>
    internal static class GitHelper
    {
        internal static ProcessStartInfo GetProcessStartInfo(Submodule submodule,SubModuleCommand subModuleCommand)
        {
            return new ProcessStartInfo("git.exe")
            {
                Arguments              = GetArguments(submodule, subModuleCommand),
                CreateNoWindow         = true,
                RedirectStandardError  = true,
                RedirectStandardOutput = true,
                UseShellExecute        = false
            };
        }

        /// <summary>
        /// Returns a argument <see cref="string"/> for Git based on the given <see cref="Submodule"/>
        /// and <see cref="SubModuleCommand"/>
        /// </summary>
        /// <param name="submodule">The <see cref="Submodule"/> for this argument, use <c>null</c> for all submodules</param>
        /// <param name="subModuleCommand">The <see cref="SubModuleCommand"/> for this argument</param>
        /// <returns>Argument <see cref="string"/> for Git</returns>
        internal static string GetArguments(Submodule submodule, SubModuleCommand subModuleCommand)
        {
            var submoduleName = (submodule != null) && !string.IsNullOrEmpty(submodule.Name)
                ? submodule.Name
                : string.Empty;

            switch(subModuleCommand)
            {
                case SubModuleCommand.AllFetch:
                    return "fetch --recurse-submodules";

                case SubModuleCommand.AllStatus:
                    return "submodule status";

                case SubModuleCommand.AllInit:
                    return "submodule init";

                case SubModuleCommand.AllDeinit:
                    return "submodule deinit --all";

                case SubModuleCommand.AllDeinitForce:
                    return "submodule deinit --all --force";

                case SubModuleCommand.AllUpdate:
                    return "submodule update";

                case SubModuleCommand.AllUpdateForce:
                    return "submodule update --force";

                case SubModuleCommand.AllPullOriginMaster:
                    return "FOREACH command is still broken under windows";

                case SubModuleCommand.OneStatus:
                    return "submodule status " + submoduleName;

                case SubModuleCommand.OneInit:
                    return "submodule init " + submoduleName;

                case SubModuleCommand.OneDeinit:
                    return "submodule deinit " + submoduleName;

                case SubModuleCommand.OneDeinitForce:
                    return "submodule deinit --force " + submoduleName;

                case SubModuleCommand.OneUpdate:
                    return "submodule update " + submoduleName;

                case SubModuleCommand.OneUpdateForce:
                    return "submodule update --force " + submoduleName;

                case SubModuleCommand.OnePullOriginMaster:
                    return "pull origin master";

                case SubModuleCommand.OtherGitVersion:
                    return "--version";

                case SubModuleCommand.OtherBranchList:
                    return "branch";

                default:
                    return string.Empty;
            }
        }
    }
}
