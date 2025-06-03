# LANShareManager

LANShareManager is a Windows desktop application designed to simplify the management of Local Area Network (LAN) sharing functionalities. It aims to provide a modern, user-friendly interface for tasks such as sharing folders, setting file permissions, and managing shared printers on Windows 10 and Windows 11 systems.

## Features (Planned)

*   **Folder Sharing**: Easily share and unshare folders on your local network.
*   **File Permissions**: View and modify access permissions for files and folders.
*   **Printer Sharing**: Manage the sharing of local printers with other users on the LAN.
*   **Modern UI**: A contemporary graphical interface for intuitive operation.
*   **Network Speed Optimization**: Includes guidance and considerations for maximizing sharing performance over the LAN.

## Project Structure

The project is organized as follows:

*   `src/`: Contains the source code for the WPF application.
    *   `Views/`: XAML files and code-behind for UI screens.
    *   `ViewModels/`: ViewModels that implement the application logic and data binding.
    *   `Models/`: Data model classes.
    *   `Services/`: Business logic services (e.g., for network sharing, permissions).
*   `docs/`: Contains documentation files, including UI design concepts and optimization notes.
*   `assets/`: Intended for static assets like icons and images (currently empty).
*   `LANShareManager.sln`: Visual Studio Solution file.

## Technology Stack

*   **Language**: C#
*   **Framework**: .NET (targetting net8.0-windows)
*   **UI**: Windows Presentation Foundation (WPF)

## Current Status

*   Initial project structure has been set up.
*   Core service logic for Folder Sharing, File Permissions, and Printer Sharing has been implemented (using WMI and .NET APIs).
*   Basic UI placeholders and navigation are in place.
*   Conceptual UI design and Network Optimization notes have been documented.

## Getting Started (Placeholder)

1.  **Prerequisites**:
    *   .NET 8 SDK (or newer compatible version).
    *   Visual Studio 2022 (or a compatible IDE) with .NET desktop development workload.
2.  **Building**:
    *   Clone this repository.
    *   Open `LANShareManager.sln` in Visual Studio.
    *   Build the solution.
3.  **Running**:
    *   Run the application from Visual Studio or by executing the compiled `.exe` file (typically found in `src/bin/Debug/net8.0-windows/`).

## Contributing

Contributions are welcome. Please open an issue to discuss any changes or new features. (Further contribution guidelines to be added).

## License

This project is intended to be open source. A specific license (e.g., MIT) will be added soon.
