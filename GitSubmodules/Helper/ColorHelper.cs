using System.Windows.Media;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;

namespace GitSubmodules.Helper
{
    /// <summary>
    /// Helper-class to easier handle with colors
    /// </summary>
    internal static class ColorHelper
    {
        /// <summary>
        /// Convert a <see cref="System.Drawing.Color"/> to a <see cref="Color"/>
        /// </summary>
        /// <param name="color">The <see cref="System.Drawing.Color"/> to convert</param>
        /// <returns>The converted <see cref="Color"/></returns>
        internal static Color Convert(System.Drawing.Color color)
        {
            return Color.FromArgb(color.A, color.R, color.B, color.G);
        }

        /// <summary>
        /// Convert a <see cref="ThemeResourceKey"/> to a <see cref="Brush"/>
        /// </summary>
        /// <param name="themeResourceKey">The <see cref="ThemeResourceKey"/> to convert</param>
        /// <returns>The converted <see cref="Brush"/></returns>
        internal static Brush GetThemedBrush(ThemeResourceKey themeResourceKey)
        {
            return new SolidColorBrush(Convert(VSColorTheme.GetThemedColor(themeResourceKey)));
        }
    }
}
