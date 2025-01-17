namespace Switch_MonitorInput

// This project is licensed under the Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
// See the LICENSE file for details.

{

    /// <summary>
    /// Delegate for the callback function that is called by the EnumDisplayMonitors function.
    /// </summary>
    /// <param name="hMonitor">A handle to the display monitor.</param>
    /// <param name="hdcMonitor">A handle to a device context.</param>
    /// <param name="lprcMonitor">A pointer to a RECT structure.</param>
    /// <param name="dwData">Application-defined data that EnumDisplayMonitors passes to the callback function.</param>
    /// <returns>Returns true to continue enumeration or false to stop.</returns>
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate bool MonitorEnumProc(nint hMonitor, nint hdcMonitor, nint lprcMonitor, nint dwData);
}