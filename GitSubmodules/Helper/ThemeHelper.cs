using System.Windows.Media;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace GitSubmodules.Helper
{
    /// <summary>
    /// Helper class to easier handle with themes
    /// </summary>
    internal static class ThemeHelper
    {
        /// <summary>
        /// Convert a Win32 colour value to a <see cref="Brush"/>
        /// </summary>
        /// <param name="win32ColorValue">The Win32 colour value for the compatible <see cref="Brush"/></param>
        /// <returns>The converted <see cref="Brush"/></returns>
        internal static Brush GetThemedBrush(uint win32ColorValue)
            => win32ColorValue <= int.MaxValue
                ? ColorHelper.GetBrush(System.Drawing.ColorTranslator.FromWin32((int)win32ColorValue))
                : Brushes.Black;

        /// <summary>
        /// Return a compatible <see cref="Brush"/> for the ToolWindowTextColorKey
        /// </summary>
        /// <param name="iVsUiShell2">The <see cref="IVsUIShell2"/> for surface handling inside Visual Studio</param>
        /// <returns>A compatible <see cref="Brush"/></returns>
        internal static Brush GetWindowTextColor(IVsUIShell2 iVsUiShell2 = null)
        {
            if(iVsUiShell2 == null)
            {
                return Brushes.Black;
            }

            const int systemColor = (int)__VSSYSCOLOREX.VSCOLOR_TOOLWINDOW_TEXT;

            uint color;

            return iVsUiShell2.GetVSSysColorEx(systemColor, out color) == VSConstants.S_OK
                ? GetThemedBrush(color)
                : Brushes.Black;
        }
    }
}
