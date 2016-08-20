using System.Windows;
using GitSubmodules.Mvvm.Model;

namespace GitSubmodules.Helper
{
    /// <summary>
    /// Helper class to easier handle with <see cref="Submodule"/>s
    /// </summary>
    internal static class SubmoduleHelper
    {
        /// <summary>
        /// Try to get a <see cref="Submodule"/> from a <see cref="FrameworkElement.Tag"/>
        /// </summary>
        /// <param name="surfaceObject">The surface element that should be a <see cref="FrameworkElement"/>
        /// and should have a <see cref="Submodule"/> inside the tag</param>
        /// <returns>The <see cref="Submodule"/> or <c>null</c> when no <see cref="Submodule"/> found</returns>
        internal static Submodule TryToGetSubmoduleFromTag(object surfaceObject)
        {
            var frameworkElement = surfaceObject as FrameworkElement;
            if(frameworkElement == null)
            {
                return null;
            }

            return frameworkElement.Tag as Submodule;
        }
    }
}
