using System;
using System.Windows.Media;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;

namespace GitSubmodules.Helper
{
    /// <summary>
    /// Helper class to easier handle with themes
    /// </summary>
    internal static class ThemeHelper
    {
        // All elements inside these class need Visual Studio 2012 SDK or higher
        //
        // - Microsoft.VisualStudio.Shell.ThemeResourceKey
        // - Microsoft.VisualStudio.PlatformUI.EnvironmentColors
        // - Microsoft.VisualStudio.PlatformUI.VSColorTheme

        /// <summary>
        /// Convert a ThemeResourceKey to a <see cref="Brush"/>
        /// </summary>
        /// <param name="themeResourceKey">The ThemeResourceKey to convert</param>
        /// <returns>The converted <see cref="Brush"/></returns>
        internal static Brush GetThemedBrush(ThemeResourceKey themeResourceKey)
        {
            return ColorHelper.GetBrush(VSColorTheme.GetThemedColor(themeResourceKey));
        }

        /// <summary>
        /// Return a compatible <see cref="Brush"/> for the ToolWindowTextColorKey
        /// </summary>
        /// <returns>A compatible <see cref="Brush"/></returns>
        internal static Brush GetWindowTextColor()
        {
            return GetThemedBrush(EnvironmentColors.ToolWindowTextColorKey);
        }

        /// <summary>
        /// Subscribe the theme changed event
        /// </summary>
        /// <param name="themeChangedMethod">The callback method for the event</param>
        internal static void SubscribeThemeChanged(EventHandler<Brush> themeChangedMethod)
        {
            VSColorTheme.ThemeChanged += delegate
                                         {
                                             themeChangedMethod(null, GetWindowTextColor());
                                         };
        }
    }
}
