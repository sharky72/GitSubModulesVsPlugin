using System;
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
        /// <summary>
        /// Returns the <see cref="ProcessStartInfo"/> for the git <see cref="Process"/>
        /// </summary>
        /// <param name="submodule">The <see cref="Submodule"/> for the given <paramref name="submoduleCommand"/>,
        /// use <c>null</c> for all submodules</param>
        /// <param name="submoduleCommand">The <see cref="SubmoduleCommand"/> for the git <see cref="Process"/></param>
        /// <returns>The <see cref="ProcessStartInfo"/> for the git <see cref="Process"/></returns>
        internal static ProcessStartInfo GetProcessStartInfo(Submodule submodule, SubmoduleCommand submoduleCommand)
        {
            return new ProcessStartInfo("git.exe")
            {
                Arguments              = GetArguments(submodule, submoduleCommand),
                CreateNoWindow         = true,
                RedirectStandardError  = true,
                RedirectStandardOutput = true,
                UseShellExecute        = false
            };
        }

        /// <summary>
        /// Returns a argument <see cref="string"/> for Git based on the given <see cref="Submodule"/>
        /// and <see cref="SubmoduleCommand"/>
        /// </summary>
        /// <param name="submodule">The <see cref="Submodule"/> for this argument, use <c>null</c> for all submodules</param>
        /// <param name="submoduleCommand">The <see cref="SubmoduleCommand"/> for this argument</param>
        /// <returns>Argument <see cref="string"/> for Git</returns>
        internal static string GetArguments(Submodule submodule, SubmoduleCommand submoduleCommand)
        {
            var submoduleName = (submodule != null) && !string.IsNullOrEmpty(submodule.Name)
                ? submodule.Name
                : string.Empty;

            switch(submoduleCommand)
            {
                case SubmoduleCommand.AllFetch:
                    return "fetch --recurse-submodules";

                case SubmoduleCommand.AllStatus:
                    return "submodule status";

                case SubmoduleCommand.AllInit:
                    return "submodule init";

                case SubmoduleCommand.AllDeinit:
                    return "submodule deinit --all";

                case SubmoduleCommand.AllDeinitForce:
                    return "submodule deinit --all --force";

                case SubmoduleCommand.AllUpdate:
                    return "submodule update";

                case SubmoduleCommand.AllUpdateForce:
                    return "submodule update --force";

                case SubmoduleCommand.AllPullOriginMaster:
                    return "FOREACH command is still broken under windows";

                case SubmoduleCommand.OneStatus:
                    return "submodule status " + submoduleName;

                case SubmoduleCommand.OneInit:
                    return "submodule init " + submoduleName;

                case SubmoduleCommand.OneDeinit:
                    return "submodule deinit " + submoduleName;

                case SubmoduleCommand.OneDeinitForce:
                    return "submodule deinit --force " + submoduleName;

                case SubmoduleCommand.OneUpdate:
                    return "submodule update " + submoduleName;

                case SubmoduleCommand.OneUpdateForce:
                    return "submodule update --force " + submoduleName;

                case SubmoduleCommand.OnePullOriginMaster:
                    return "pull origin master";

                case SubmoduleCommand.OtherGitVersion:
                    return "--version";

                case SubmoduleCommand.OneBranchList:
                case SubmoduleCommand.OtherBranchList:
                    return "branch";

                case SubmoduleCommand.OneRemoveSubmoduleFull:
                    // SubmoduleCommand.OneDeinitForce
                    // SubmoduleCommand.OneRemoveSubmoduleOnlyIndex
                    // SubmoduleCommand.OneRemoveSubmoduleOnlyEntry
                    // SubmoduleCommand.OneRemoveSubmoduleOnlyFolder
                    throw new NotImplementedException("TODO: " + submoduleCommand);

                case SubmoduleCommand.OneRemoveSubmoduleOnlyEntry:
                    SubmoduleHelper.RemoveSubmoduleEntry(submodule);
                    return string.Empty;

                case SubmoduleCommand.OneRemoveSubmoduleOnlyIndex:
                    return "rm --cached " + submoduleName;

                case SubmoduleCommand.OneRemoveSubmoduleOnlyFolder:
                    SubmoduleHelper.DeleteSubmoduleFolder(submodule);
                    return string.Empty;

                case SubmoduleCommand.OtherAddSubmodule:
                    return "submodule add " + submoduleName;

                case SubmoduleCommand.OtherAddSubmoduleWithInit:
                    return "submodule add " + submoduleName + " --init";

                case SubmoduleCommand.OtherAddSubmoduleWithUpdate:
                    throw new NotImplementedException("TODO: " + submoduleCommand);

                case SubmoduleCommand.OtherAddSubmoduleWithPullOrigin:
                    throw new NotImplementedException("TODO: " + submoduleCommand);

                default:
                    return string.Empty;
            }
        }
    }
}
