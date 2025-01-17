# Switch-MonitorInput.ps1

## Description

**Switch-MonitorInput.ps1** is a PowerShell script that allows you to switch the input source of your monitors programmatically. It supports monitors connected via HDMI, DisplayPort, and USB-C, providing a simple interface to automate input source changes based on your requirements.

**Note:** This script has only been tested with a Dell U4924DW monitor.

## Features

- Switch monitor input sources using PowerShell script parameters.
- Supports input sources: `Hdmi1`, `Hdmi2`, `DisplayPort`, `UsbC`.
- Displays a list of attached monitors with their descriptions.
- Includes functions to test input switching and retrieve monitor information.
- Robust error handling for invalid arguments and external exceptions.

## How to Use

1. **Prerequisites**

   - Ensure you have **Windows PowerShell** installed on your Windows operating system.
   - Make sure your monitor supports DDC/CI (Display Data Channel Command Interface).

2. **Run the Script**

   - Download the `Switch-MonitorInput.ps1` script to your local machine.
   - Open PowerShell with administrative privileges (if required for your environment).
   - Navigate to the directory containing the script.

3. **Execution Policy**

   - If necessary, adjust your PowerShell execution policy to allow script execution:
      Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser

4. **Examples**

   - **Switch Monitor Input Source**
      .\Switch-MonitorInput.ps1 -MonitorName "Dell U4924DW" -InputSource DisplayPort

      This command switches the input source of the monitor with the name "Dell U4924DW" to DisplayPort.

   - **Display Attached Monitors**
      .\Switch-MonitorInput.ps1

     Running the script without parameters will display a list of attached monitors with their descriptions.

   - **Test Input Switching**

     Uncomment the line `# Test-InputSwitching` at the end of the script to test input codes:
     # Test-InputSwitching

     This function cycles through a range of input codes to test which ones work with your monitor.

## Parameters

- `-MonitorName`: Specifies the name or a part of the monitor's description as recognized by your system. Defaults to `"get-help"`.

- `-InputSource`: Specifies the input source to switch to. Valid options are:

  - `Hdmi1`
  - `Hdmi2`
  - `DisplayPort`
  - `UsbC`

## Functions

- **Show-AttachedMonitors**

  Displays information about all monitors currently attached to the system.

- **Switch-MonitorInput**

  Switches the input source of the specified monitor.

- **Test-InputSwitching**

  Iterates through a range of input codes and attempts to switch the input source of the monitor to each code. Useful for finding the correct input codes for your specific monitor.

## Requirements

- **Operating System**: Windows with support for DDC/CI.
- **PowerShell Version**: Windows PowerShell 5.1 or newer.
- **Permissions**: May require administrative privileges to interact with monitor hardware.

## Notes

- The script integrates C# code within PowerShell using the `Add-Type` cmdlet to interact with Windows APIs for monitor control.

- Ensure that your monitor model supports the DDC/CI commands used in this script.

- The input codes provided are specific to the Dell U4924DW monitor. If you are using a different monitor, you may need to use the `Test-InputSwitching` function to find the correct codes.

## License

This project is licensed under the [Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License](https://creativecommons.org/licenses/by-nc-sa/4.0/).

---

     
     