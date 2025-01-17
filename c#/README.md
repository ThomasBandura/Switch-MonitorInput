# Switch-MonitorInput

## Description

**Switch-MonitorInput** is a command-line application that allows you to switch the input source of your monitors programmatically. It supports monitors connected via HDMI, DisplayPort, and USB-C, providing a simple interface to automate input source changes based on your requirements.

**Note:** This application has only been tested with a Dell U4924DW monitor.

## Features

- Switch monitor input sources using command-line arguments.
- Supports input sources: `Hdmi1`, `Hdmi2`, `DisplayPort`, `UsbC`.
- Displays a list of attached monitors with their descriptions.
- Robust error handling for invalid arguments and external exceptions.

## How to Use

1. **Build the Application**

   - Compile the project using Visual Studio 2022 or the .NET CLI targeting **.NET 8.0**.
   - Ensure all project dependencies are resolved during the build process.

2. **Run the Application**

   - Open a command prompt or terminal in the directory containing the compiled executable.
   - Use the following syntax to execute the application:
     Switch-MonitorInput.exe <MonitorName> <InputSource>
        - `<MonitorName>`: The name or a part of the monitor's description as recognized by your system.
        - `<InputSource>`: The input source to switch to. Valid options are `Hdmi1`, `Hdmi2`, `DisplayPort`, `UsbC`.

3. **Example Usage**

   - To switch a monitor named "Dell U4924DW" to DisplayPort:
     Switch-MonitorInput.exe "Dell U4924DW" DisplayPort
   - The application will output the status and any relevant messages to the console.

4. **Display Attached Monitors**

   - The application automatically lists all attached monitors with their indices, descriptions, and handles.
   - Use this information to identify the correct `<MonitorName>` for your monitor.

## Requirements

- **Operating System**: Windows with support for DDC/CI (Display Data Channel Command Interface).
- **.NET Runtime**: .NET 8.0 or higher installed on your machine.

## License

This project is licensed under the [Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License](https://creativecommons.org/licenses/by-nc-sa/4.0/).

---