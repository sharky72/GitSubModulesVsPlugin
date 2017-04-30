using System.Windows.Media;

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
        internal static Color Convert(System.Drawing.Color color) => Color.FromArgb(color.A, color.R, color.B, color.G);

        /// <summary>
        /// Return a comptible <see cref="Brush"/> for a given <see cref="System.Drawing.Color"/>
        /// </summary>
        /// <param name="color">The <see cref="System.Drawing.Color"/> for the <see cref="Brush"/></param>
        /// <returns>The new <see cref="Brush"/></returns>
        internal static Brush GetBrush(System.Drawing.Color color) => new SolidColorBrush(Convert(color));
    }
}
