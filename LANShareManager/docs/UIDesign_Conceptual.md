# LAN Share Manager - Conceptual UI Design

## 1. Overall Design Philosophy

*   **Modern Aesthetics**: The UI should adopt a modern, clean, and intuitive design, taking cues from Fluent Design System principles (used in Windows 10/11). This includes considerations for acrylic materials (subtle transparency), connected animations (where appropriate), and clear visual hierarchy.
*   **User-Friendly**: The application should be easy to navigate and use, even for users not deeply familiar with network sharing settings.
*   **Responsive (Consideration)**: While primarily a desktop application, the layout should be adaptable to different window sizes.

## 2. Main Window Layout

*   **Structure**: A two-pane layout.
    *   **Left Navigation Pane**:
        *   Width: Fixed or minimally collapsible (e.g., icon-only view).
        *   Content: Icons and labels for main sections:
            *   Dashboard (Overview)
            *   Folder Sharing
            *   File Permissions
            *   Printer Sharing
            *   Settings
    *   **Right Content Pane**:
        *   Displays the content for the selected section from the navigation pane.
        *   Will contain specific controls and information relevant to each module.

## 3. Section-Specific UI Elements

### 3.1. Dashboard (Overview)

*   **Purpose**: Provide a quick summary of shared resources and application status.
*   **Elements**:
    *   Welcome message.
    *   Summary cards/tiles for:
        *   Number of active folder shares.
        *   Number of shared printers.
        *   Quick status indicators (e.g., network connectivity, service status if any).
    *   Possibly shortcuts to common actions.

### 3.2. Folder Sharing

*   **Purpose**: Manage shared folders on the LAN.
*   **Elements**:
    *   **Toolbar/Action Bar**:
        *   "Add Share" button.
        *   "Remove Share" button (enabled when a share is selected).
        *   "Refresh List" button.
    *   **Shared Folders List**:
        *   A data grid or list view displaying:
            *   Share Name
            *   Local Path
            *   Number of active connections (if obtainable)
            *   Read/Write status (summary)
    *   **Add/Edit Share Dialog (Modal or Inline)**:
        *   Fields for:
            *   Local folder path (with a browse button).
            *   Share name (auto-suggested from folder name, editable).
            *   Description (optional).
            *   Basic permission settings (e.g., "Read-only for Everyone", "Read/Write for Everyone", or a button to open advanced permissions).

### 3.3. File Permissions

*   **Purpose**: View and modify permissions for specific files and folders (primarily for those being shared or considered for sharing).
*   **Elements**:
    *   **File/Folder Selector**:
        *   A way to browse to and select a file or folder (e.g., a tree view or a path input with a browse button).
    *   **Permissions Display**:
        *   List of users/groups with access (e.g., "Everyone", "Administrators", specific users).
        *   For each user/group, display their permissions (Read, Write, Execute, Full Control) often using checkboxes or toggles.
    *   **Action Buttons**:
        *   "Add User/Group" button.
        *   "Remove User/Group" button.
        *   "Apply Changes" button.
        *   "Advanced" button (to open the standard Windows Security tab if necessary, or a more detailed custom interface).

### 3.4. Printer Sharing

*   **Purpose**: Manage shared printers on the LAN.
*   **Elements**:
    *   **Toolbar/Action Bar**:
        *   "Refresh List" button.
    *   **Printers List**:
        *   A data grid or list view displaying:
            *   Printer Name
            *   Status (e.g., Ready, Offline)
            *   Shared (Yes/No)
            *   Share Name (if shared)
    *   **Action Buttons (per printer or on toolbar when printer selected)**:
        *   "Share Printer" button (opens a dialog to set share name).
        *   "Unshare Printer" button.
        *   "Set as Default" (local setting).
        *   "Printer Properties".

### 3.5. Settings

*   **Purpose**: Configure application-specific settings.
*   **Elements**:
    *   Application theme (Light/Dark/System).
    *   Default sharing permissions (if applicable).
    *   Network settings relevant to the app (e.g., preferred network interface if multiple are present).
    *   Logging options.
    *   "About" section.

## 4. General UI Controls and Style

*   **Controls**: Utilize standard WPF controls styled for a modern look (e.g., `Button`, `TextBox`, `DataGrid`, `ListView`, `CheckBox`, `RadioButton`).
*   **Icons**: Use a consistent icon set (e.g., Segoe Fluent Icons, Material Design Icons, or a custom set).
*   **Typography**: Clear and legible fonts.
*   **Feedback**: Visual feedback for actions (e.g., loading indicators, success/error messages).

This conceptual design will be translated into XAML and corresponding ViewModel logic during the implementation phases.
