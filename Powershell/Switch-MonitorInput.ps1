[CmdletBinding(SupportsShouldProcess=$true)]
param (
    [Parameter()]
    [string]$MonitorName = "get-help",
    [ValidateSet("Hdmi1", "Hdmi2", "DisplayPort", "UsbC")]
    [string]$InputSource = "DisplayPort"
)

# This project is licensed under the Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
#  See the LICENSE file for details.

<#
.SYNOPSIS
    This PowerShell script uses C# code to interface with Windows APIs for managing display monitors KVM.

.DESCRIPTION
    The script integrates C# structures and delegates with the `[StructLayout]` attribute to interact with Windows APIs. It includes:
    
    - **MonitorInfo**: Holds detailed information about a monitor.
    - **RECT**: Represents a rectangle specifying monitor boundaries.
    - **PhysicalMonitor**: Contains a handle and description of a physical monitor.
    - **MonitorEnumProc**: Delegate for callback functions during monitor enumeration.

    The `MonitorHelper` static class encapsulates several external functions imported from Windows DLLs (`user32.dll` and `dxva2.dll`):
    
    - **EnumDisplayMonitors**: Enumerates display monitors.
    - **GetMonitorInfo**: Retrieves monitor information.
    - **GetNumberOfPhysicalMonitorsFromHMONITOR**: Gets the number of physical monitors.
    - **GetPhysicalMonitorsFromHMONITOR**: Retrieves physical monitor details.
    - **SetVCPFeature**: Modifies VCP features like input source.

    Key methods in `MonitorHelper`:
    
    - **GetMonitorHandle**: Obtains a handle to the first available monitor.
    - **GetPhysicalMonitors**: Retrieves an array of physical monitors.
    - **SwitchInput**: Changes the input source of a monitor.
#>

try {
    $null = [MonitorHelper].Name
} catch {
   # Defining the 'CustomMonitor' namespace...

    Add-Type -TypeDefinition @"
using System;
using System.Runtime.InteropServices;


[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public struct MonitorInfo {
    public int cbSize;
    public RECT rcMonitor;
    public RECT rcWork;
    public uint dwFlags;
}

[StructLayout(LayoutKind.Sequential)]
public struct RECT {
    public int left;
    public int top;
    public int right;
    public int bottom;
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public struct PhysicalMonitor {
    public IntPtr hPhysicalMonitor;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
    public string szPhysicalMonitorDescription;
}

[UnmanagedFunctionPointer(CallingConvention.StdCall)]
public delegate bool MonitorEnumProc(IntPtr hMonitor, IntPtr hdcMonitor, IntPtr lprcMonitor, IntPtr dwData);

public static class MonitorHelper
{
    [DllImport("user32.dll")]
    public static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, MonitorEnumProc lpfnEnum, IntPtr dwData);

    [DllImport("user32.dll")]
    public static extern bool GetMonitorInfo(IntPtr hMonitor, ref MonitorInfo lpmi);

    [DllImport("dxva2.dll", SetLastError=true)]
    public static extern bool GetNumberOfPhysicalMonitorsFromHMONITOR(IntPtr hMonitor, out uint numberOfPhysicalMonitors);

    [DllImport("dxva2.dll", SetLastError=true)]
    public static extern bool GetPhysicalMonitorsFromHMONITOR(IntPtr hMonitor, uint monitorArraySize, [Out] PhysicalMonitor[] monitors);

    [DllImport("dxva2.dll", SetLastError=true)]
    public static extern bool SetVCPFeature(IntPtr hMonitor, byte bVCPCode, uint dwNewValue);

    public static IntPtr GetMonitorHandle()
    {
        IntPtr monitorHandle = IntPtr.Zero;
        EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, (hMonitor, hdcMonitor, lprcMonitor, dwData) =>
        {
            monitorHandle = hMonitor;
            // Stop enumeration after the first valid monitor
            return false;
        }, IntPtr.Zero);
        return monitorHandle;
    }

    public static PhysicalMonitor[] GetPhysicalMonitors(IntPtr hMonitor)
    {
        GetNumberOfPhysicalMonitorsFromHMONITOR(hMonitor, out uint count);
        if (count == 0) return new PhysicalMonitor[0];

        PhysicalMonitor[] monitors = new PhysicalMonitor[count];
        GetPhysicalMonitorsFromHMONITOR(hMonitor, count, monitors);
        return monitors;
    }

    public static bool SwitchInput(IntPtr hMonitor, uint inputSource)
    {
        // bVCPCode 0x60 = Input Source
        return SetVCPFeature(hMonitor, 0x60, inputSource);
    }

}
"@
} # End of try-catch block


<#
.SYNOPSIS
Defines an enumeration for monitor input types.

.DESCRIPTION
The MonitorInput enumeration is used to specify different types of input sources for a monitor. This can be useful in scripts that need to handle multiple input sources for display devices.
The Values are based on the VCP code for input source selection. (in this Case "Dell U4924DW")
Use Test-inputSwitching to test the input switching on a monitor and find the right Values for your Type.
#>

enum MonitorInput {
    Hdmi1 = 0x11
    Hdmi2 = 0x12
    DisplayPort = 0x0F
    UsbC = 0x1b
}

<#
.SYNOPSIS
    Tests input switching on a monitor by cycling through a range of input codes.

.DESCRIPTION
    The Test-InputSwitching function iterates through a range of input codes (0x00 to 0x2F) and attempts to switch the input source of a monitor to each code. 
    It writes the current input code being tested to the host and pauses for 2 seconds between each switch.

.PARAMETER None
    This function does not take any parameters.

.EXAMPLE
    Test-InputSwitching
    This example runs the function to test input switching on the monitor.

.NOTES
    Ensure that the [MonitorHelper]::SwitchInput method and $targetMonitorHandle are properly defined and accessible in the script's context.
#>
function Test-InputSwitching {
    foreach ($code in 0x00..0x2F) {
        Write-Host "Teste Eingangscode: $([Convert]::ToString($code,16))"
        [MonitorHelper]::SwitchInput($targetMonitorHandle, $code)
        Start-Sleep -Seconds 2
    }
}

<#
.SYNOPSIS
Displays the list of attached monitors.

.DESCRIPTION
The Show-AttachedMonitors function retrieves and displays information about all monitors currently attached to the system.

.EXAMPLE
Show-AttachedMonitors

This example runs the function and outputs the list of attached monitors.

.NOTES
File Name: Switch-MonitorInput.ps1
#>
function Show-AttachedMonitors{
    "Attempting to retrieve physical monitors..."
    $monitorHandle = [MonitorHelper]::GetMonitorHandle()
    # "MonitorHandle: $monitorHandle"

    try {
        $monitors = [MonitorHelper]::GetPhysicalMonitors($monitorHandle)
        if ($monitors -and $monitors.Count -gt 0) {
            "Found monitors:"
            $monitors | ForEach-Object { 
                "Monitor {0} -> {1} -> Handle {2}" -f $monitors.IndexOf($_), $_.szPhysicalMonitorDescription, $_.hPhysicalMonitor
            }
        } else {
            "No monitors found."
        }
    }
    catch {
        "Error retrieving monitors: $_"
    }    
}

<#
.SYNOPSIS
Switches the input source of a monitor.

.DESCRIPTION
This function allows you to switch the input source of a monitor to a different input type.

.PARAMETER Monitor
Specifies the monitor to switch the input source for.

.PARAMETER InputSource
Specifies the input source to switch to. This could be values like HDMI, DisplayPort, etc.

.EXAMPLE
Switch-MonitorInput -Monitor 1 -InputSource HDMI
This example switches the input source of monitor 1 to HDMI.

.NOTES
Make sure to have the necessary permissions and tools installed to control the monitor input source.

#>
function Switch-MonitorInput {
    [CmdletBinding(SupportsShouldProcess=$true)]
    param (
        [string]$MonitorName,
        [MonitorInput]$InputSource
    )

    $rc=$false

    
    $monitorHandle = [MonitorHelper]::GetMonitorHandle()
    $monitors = [MonitorHelper]::GetPhysicalMonitors($monitorHandle)


    $selectedMonitors = $monitors.Where({$_.szPhysicalMonitorDescription -like "*$MonitorName*"})
    if ($selectedMonitors.Count -eq 0) {
        Write-Host "No monitors found matching the name: $MonitorName"
    } else {
        $selectedMonitors | ForEach-Object {
            $targetMonitorHandle = $_.hPhysicalMonitor
            $Message = "{0} -> change input source to -> {1}" -f $_.szPhysicalMonitorDescription, [MonitorInput]::$InputSource
            if ( $PSCmdlet.ShouldProcess($Message)) {
                $rc=[MonitorHelper]::SwitchInput($targetMonitorHandle, [MonitorInput]::$InputSource)
                $Message
            }
        }
    }

    return $rc
}

Show-AttachedMonitors
Switch-MonitorInput -MonitorName $MonitorName -InputSource $InputSource
# Test-InputSwitching
