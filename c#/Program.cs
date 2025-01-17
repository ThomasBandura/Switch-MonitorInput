// This project is licensed under the Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
// See the LICENSE file for details.

namespace Switch_MonitorInput;

/// <summary>
/// The main program class.
/// </summary>
public class Program
{
    /// <summary>
    /// The main entry point of the application.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    public static void Main(string[] args)
    {
        try
        {
            // Check if enough arguments were passed
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: Switch-MonitorInput.exe <MonitorName> <InputSource>");
                return;
            }

            string monitorName = args[0];
            string inputSource = args[1];

            // Check if the input source is valid
            if (!new[] { "Hdmi1", "Hdmi2", "DisplayPort", "UsbC" }.Contains(inputSource))
            {
                Console.WriteLine("Invalid input source. Valid options are: Hdmi1, Hdmi2, DisplayPort, UsbC");
                return;
            }

            // Display the attached monitors
            Show_AttachedMonitors();

            // Get the monitor handle
            IntPtr monitorHandle = MonitorHelper.GetMonitorHandle();
            PhysicalMonitor[] monitors = MonitorHelper.GetPhysicalMonitors(monitorHandle);

            // Iterate through the monitors and look for the specified monitor
            foreach (PhysicalMonitor monitor in monitors)
            {
                if (monitor.szPhysicalMonitorDescription.Contains(monitorName))
                {
                    Console.WriteLine($"Switching {monitor.szPhysicalMonitorDescription} to {inputSource}");
                    // Switch the input source of the monitor
                    MonitorHelper.SwitchInput(monitor.hPhysicalMonitor, MonitorHelper.GetInputSourceCode(inputSource));
                    return;
                }
            }

            // If the monitor was not found
            Console.WriteLine($"Monitor {monitorName} not found.");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Argument error: {ex.Message}");
        }
        catch (DllNotFoundException ex)
        {
            Console.WriteLine($"DLL not found: {ex.Message}");
        }
        catch (ExternalException ex)
        {
            Console.WriteLine($"External error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
        }
    }

    /// <summary>
    /// Displays the attached monitors.
    /// </summary>
    public static void Show_AttachedMonitors()
    {
        Console.WriteLine("Attempting to retrieve physical monitors...");
        IntPtr monitorHandle = MonitorHelper.GetMonitorHandle();

        try
        {
            PhysicalMonitor[] monitors = MonitorHelper.GetPhysicalMonitors(monitorHandle);
            if (monitors != null && monitors.Length > 0)
            {
                Console.WriteLine("Found monitors:");
                for (int i = 0; i < monitors.Length; i++)
                {
                    Console.WriteLine($"Monitor {i} -> {monitors[i].szPhysicalMonitorDescription} -> Handle {monitors[i].hPhysicalMonitor}");
                }
            }
            else
            {
                Console.WriteLine("No monitors found.");
            }
        }
        catch (ExternalException ex)
        {
            Console.WriteLine($"Error retrieving monitors: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred while retrieving monitors: {ex.Message}");
        }
    }
}