// This project is licensed under the Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
// See the LICENSE file for details.

namespace Switch_MonitorInput;

/// <summary>
/// Represents information about a display monitor.
/// </summary>
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public struct MonitorInfo
{
    /// <summary>
    /// The size, in bytes, of the structure.
    /// </summary>
    public int cbSize;

    /// <summary>
    /// A RECT structure that specifies the display monitor rectangle.
    /// </summary>
    public RECT rcMonitor;

    /// <summary>
    /// A RECT structure that specifies the work area rectangle of the display monitor.
    /// </summary>
    public RECT rcWork;

    /// <summary>
    /// The attributes of the display monitor.
    /// </summary>
    public uint dwFlags;
}