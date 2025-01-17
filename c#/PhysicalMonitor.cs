// This project is licensed under the Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
// See the LICENSE file for details.

namespace Switch_MonitorInput;

/// <summary>
/// Represents a physical monitor with a handle and a description.
/// </summary>
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public struct PhysicalMonitor
{
    /// <summary>
    /// Handle to the physical monitor.
    /// </summary>
    public IntPtr hPhysicalMonitor;

    /// <summary>
    /// Description of the physical monitor.
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
    public string szPhysicalMonitorDescription;
}