namespace Switch_MonitorInput

// This project is licensed under the Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
// See the LICENSE file for details.

{
    /// <summary>
    /// Provides helper methods for interacting with monitor input sources.
    /// </summary>
    public static class MonitorHelper
    {
        [DllImport("user32.dll")]
        private static extern bool EnumDisplayMonitors(nint hdc, nint lprcClip, MonitorEnumProc lpfnEnum, nint dwData);

        /// <summary>
        /// Retrieves information about a display monitor.
        /// </summary>
        /// <param name="hMonitor">A handle to the display monitor.</param>
        /// <param name="lpmi">A pointer to a MonitorInfo structure that receives information about the display monitor.</param>
        /// <returns>True if the function succeeds; otherwise, false.</returns>
        [DllImport("user32.dll")]
        public static extern bool GetMonitorInfo(nint hMonitor, ref MonitorInfo lpmi);

        [DllImport("dxva2.dll", SetLastError = true)]
        private static extern bool GetNumberOfPhysicalMonitorsFromHMONITOR(nint hMonitor, out uint numberOfPhysicalMonitors);

        [DllImport("dxva2.dll", SetLastError = true)]
        private static extern bool GetPhysicalMonitorsFromHMONITOR(nint hMonitor, uint monitorArraySize, [Out] PhysicalMonitor[] monitors);

        [DllImport("dxva2.dll", SetLastError = true)]
        private static extern bool SetVCPFeature(nint hMonitor, byte bVcpCode, uint dwNewValue);

        /// <summary>
        /// Retrieves the handle of the first found monitor.
        /// </summary>
        /// <returns>The handle of the first found monitor.</returns>
        public static nint GetMonitorHandle()
        {
            nint monitorHandle = nint.Zero;
            EnumDisplayMonitors(nint.Zero, nint.Zero, (hMonitor, hdcMonitor, lprcMonitor, dwData) =>
            {
                monitorHandle = hMonitor;
                // Stops enumeration after the first valid monitor
                return false;
            }, nint.Zero);
            return monitorHandle;
        }

        /// <summary>
        /// Retrieves the physical monitors for a given monitor handle.
        /// </summary>
        /// <param name="hMonitor">The handle of the monitor.</param>
        /// <returns>An array of physical monitors.</returns>
        public static PhysicalMonitor[] GetPhysicalMonitors(nint hMonitor)
        {
            GetNumberOfPhysicalMonitorsFromHMONITOR(hMonitor, out uint count);
            if (count == 0) return Array.Empty<PhysicalMonitor>();

            PhysicalMonitor[] monitors = new PhysicalMonitor[count];
            GetPhysicalMonitorsFromHMONITOR(hMonitor, count, monitors);
            return monitors;
        }

        /// <summary>
        /// Switches the input source of a monitor.
        /// </summary>
        /// <param name="hMonitor">The handle of the monitor.</param>
        /// <param name="inputSource">The input source code.</param>
        /// <returns>True if the input source was successfully switched; otherwise, false.</returns>
        public static bool SwitchInput(nint hMonitor, uint inputSource)
        {
            // bVCPCode 0x60 = Input source
            return SetVCPFeature(hMonitor, 0x60, inputSource);
        }

        /// <summary>
        /// Returns the code for a given input source.
        /// </summary>
        /// <param name="inputSource">The name of the input source.</param>
        /// <returns>The code of the input source.</returns>
        /// <exception cref="ArgumentException">Thrown when the input source is invalid.</exception>
        public static uint GetInputSourceCode(string inputSource)
        {
            return inputSource switch
            {
                "Hdmi1" => 0x11,
                "Hdmi2" => 0x12,
                "DisplayPort" => 0x0F,
                "UsbC" => 0x1B,
                _ => throw new ArgumentException("Invalid input source")
            };
        }
    }
}