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
        /// Returns a argument <see cref="string"/> for Git based on the given <see cref="Submodule"/>
        /// and <see cref="SubModuleCommand"/>
        /// </summary>
        /// <param name="submodule">The <see cref="Submodule"/> for this argument, use <c>null</c> for all submodules</param>
        /// <param name="submoduleCommand">The <see cref="SubModuleCommand"/> for this argument</param>
        /// <returns>Argument <see cref="string"/> for Git</returns>
        internal static string GetGitArguments(Submodule submodule, SubModuleCommand submoduleCommand)
        {
            var submoduleName = (submodule != null) && !string.IsNullOrEmpty(submodule.Name)
                ? submodule.Name
                : string.Empty;

            switch(submoduleCommand)
            {
                case SubModuleCommand.AllStatus:
                    return "submodule status";

                case SubModuleCommand.AllRegister:
                    return "submodule init";

                case SubModuleCommand.AllDeRegister:
                    return "submodule deinit --all";

                case SubModuleCommand.AllDeRegisterForce:
                    return "submodule deinit --all --force";

                case SubModuleCommand.AllUpdate:
                    return "submodule update";

                case SubModuleCommand.AllGetLatest:
                    // TODO:
                    // replaced with own method, FOREACH command is still broken on mysys under windows
                    // return "submodule foreach git pull origin master";
                    return "TODO";

                case SubModuleCommand.OneRegister:
                    return "submodule init " + submoduleName;

                case SubModuleCommand.OneDeRegister:
                    return "submodule deinit " + submoduleName;

                case SubModuleCommand.OneDeRegisterForce:
                    return "submodule deinit --force " + submoduleName;

                case SubModuleCommand.OneUpdate:
                    return "submodule update " + submoduleName;

                case SubModuleCommand.OneGetLatest:
                    return "TODO";

                default:
                    return string.Empty;
            }
        }
    }
}
