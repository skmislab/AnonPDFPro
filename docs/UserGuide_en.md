## AnonPDF Pro - User Guide

*English translation of the Polish help document*

# 1. Introduction

AnonPDF Pro is a tool for partially or fully removing (redacting) selected content in PDF files. It lets you mark areas in a document that should be removed and then generates a new PDF copy with the redactions applied. This makes it easy to remove sensitive data such as personal information or confidential content before sharing PDF documents with third parties.

# 2. Installation and Launch

> **1.** Download and install AnonPDF Pro (depending on the distribution method, this may be an MSI/EXE installer or a set of binaries).

> **2.** Make sure the required .NET Framework / .NET Runtime is installed (the exact version depends on the application build).

> **3.** Start the application, e.g., from the desktop shortcut, the Start menu, or directly by running AnonPDF.exe.

> **4.** After launch, the main application window should open.

## 2.5 Activating PRO (upgrading from DEMO)

The activation method depends on the deployment mode.

### Standalone version

In standalone mode, activation is done by the end user inside the application:

1. Start AnonPDF Pro.
2. Open the `?` menu and select `Activation`.
3. Select the received license package.
4. Confirm and restart the application.
5. Open `? -> About` and verify that the license status is PRO.

### Central (network) version

In central mode, end users do not activate from the menu.
Activation is performed by an administrator:

1. Close the application on user workstations.
2. Extract the license package.
3. Copy the license files to the application folder (where `AnonPDFPro.exe` is located).
4. Confirm file replacement if prompted.
5. Start the application again and verify `? -> About`.

Notes:
- Keep original filenames and folder structure from the license package.
- If the application still runs in DEMO mode, verify file location and folder permissions.

# 3. Application Interface

The main window consists of the following elements:

- Top menu bar - options for managing projects and files, including:

> \- File: open a new PDF, save/open a redaction project, exit the application.

> \- Help: open this user guide.

- Side panel / page list - shows a text list of all pages in the loaded PDF ("Page 1", "Page 2", etc.).
- Main preview area - displays the currently selected PDF page and allows you to draw/select areas to be redacted.
- Toolbar - buttons for page navigation, zoom, and selection tools:

> \- Page navigation: "First", "Previous", "Next", "Last".

> \- Selection navigation: "First selection", "Previous selection", "Next selection", "Last selection".

> \- Zoom: "Zoom in", "Zoom out", "Min" (minimum), "Max" (maximum).

- Page number input - a text box for jumping to a specific page by entering its number.
- Redaction tool group:

> \- Marker mode - marks horizontal areas (e.g., narrow strips for removing single text lines).

> \- Rectangle mode - marks any rectangular area.

> \- Button "Clear current page" - removes all selections from the currently displayed page.

> \- Button "Clear all pages" - removes all selections from every page in the document.

- Project I/O and PDF export controls:

> \- Open project - loads a .pap project file containing saved redaction areas.

> \- Open last PDF and project - loads the most recently used PDF and its .pap project.

> \- Save project - saves the current selections to the active .pap project file.

> \- Save project as - saves the current selections to a new .pap file at a chosen location.

> \- Save PDF (redaction) - generates a new PDF file with the selected areas removed.

# 4. Getting Started

## 4.1 Open a PDF

From the menu, select File → Open PDF (or click the "Select PDF" button), then choose the target PDF document.

## 4.2 Browse pages

Use the page list on the left side (or the navigation buttons such as "Next" / "Previous") to select the page you want to edit.

## 4.3 Search

To search within the document:

> **1.** In the left panel, type the text you want to find and press Enter.

> **2.** Use the search navigation buttons to move between search results.

> **3.** Use the "PESEL, KW ?" button to check whether the document contains Polish identifiers such as a PESEL number, a Land and Mortgage Register number (KW), or a Polish ID card number.

> **4.** To remove a search result, click the "X" button next to it.

## 4.4 Mark areas for redaction

To create a redaction area:

> **1.** Select Marker mode or Rectangle mode.

> **2.** Press and hold the left mouse button on the page preview where you want to mark content.

> **3.** In Marker mode you draw a horizontal strip; in Rectangle (box) mode you draw a rectangular area of any size.

> **4.** In Marker mode, hold CTRL to draw a rectangular area as well.

> **5.** Release the mouse button to confirm the selection.

> **6.** To remove an existing selection (for example, if you added it by accident), right-click inside the selection area.

## 4.5 Preview pending redactions

After you create a selection, within about 2 seconds the text that will be removed is highlighted in gray.

## 4.6 Save your redaction project

If you want to save the current selection state for later editing, choose Save project or Save project as and select where to store the .pap file. When you reopen the PDF and the corresponding .pap project, you can continue adding or adjusting redactions.

## 4.7 Export (redact) the PDF

After marking all required areas, click Save PDF (or File → Save PDF). Select the destination output file. The application generates a new redacted PDF with your selected areas removed.

# 5. Selections and Navigation

## 5.1 Page navigation

Use the "First", "Previous", "Next", and "Last" buttons, or enter a page number in the page input box and press Enter.

## 5.2 Selection navigation

If your document contains many selections, use "First selection", "Previous selection", "Next selection", and "Last selection" to quickly jump between pages that contain redaction areas.

## 5.3 Zoom and scrolling

- Zoom in / Zoom out: use the "+" and "-" buttons or the "Min / Max" buttons to set minimum/maximum zoom.
- Mouse wheel: scrolling moves the page vertically. When you reach the top/bottom edge, the application automatically switches to the previous/next page.
- CTRL + mouse wheel: zoom out/in.

# 6. Additional Features

## 6.1 Clearing selections

- "Clear current page" removes all redaction areas from the current page.
- "Clear all pages" removes all redaction areas from the entire document.

## 6.2 Signatures

The application provides options related to qualified electronic signatures:

- "Removed" - removes all qualified signatures from the document.
- "Original" - keeps the original signatures, but due to document modifications they will not be verifiable.
- "Report" - adds an extra page listing signature information instead of the original signature visualization.

## 6.3 Removing pages

- Click the "Trash 1..n" icon in the left panel to mark a selected page range for removal (or to cancel removal).
- Click the "Trash" icon in the left panel to mark a single page for removal. The icon background turns black.

## 6.4 Redaction fill color

By default, the application removes content located inside the selection area. For images, graphics, or scanned pages, the removed area is filled with white and matches the selection size.

Enabling the "Highlight color" option changes the fill color of redaction areas in the output PDF to black.

## 6.5 Open after saving

When "Preview after saving" is enabled, the application automatically opens the saved PDF after export.

## 6.6 Loading a project

You can load a .pap project file at any time (for example, prepared by another person or created earlier). This updates the selections in the application to match the loaded project.

## 6.7 Closing the application

When closing, the application may ask you to confirm exiting, especially if you have unsaved changes.

## 6.8 Help menu

- "User Guide" - opens this help/instruction content.
- "About" - shows version information, author, and copyright.

## 6.9 Duplicating selections and objects

In the context menu (right mouse button), you can use:

- `Duplicate selection` - for redaction selections.
- `Duplicate object` - for objects (for example text, images, arrows, vector shapes, comments).

After selecting one of these options, a page range dialog is shown:

- `From` / `To` - page range where copies should be created.
- The current source page is skipped automatically.

For duplicated objects, an additional option is available:

- `Delete duplicated objects` - removes duplicated object copies in the selected page range.

# 7. Common Issues and Tips

## 7.1 Cannot save the output PDF

- Make sure the PDF is not open in another application. The system may block saving if the file is currently in use.
- Choose a different output filename to avoid overwriting the original file.

## 7.2 Invalid project file format (.pap)

Project files (.pap) store redaction areas as JSON internally. Make sure you open the correct project file for the currently loaded PDF.

- If you see a message that the project contains more pages than the PDF, load the project into the matching PDF document.

## 7.3 No selections / Save PDF is disabled

- If "Save PDF" is disabled or nothing is selected, verify that you actually added at least one redaction area.

## 7.4 Slow performance

- Large documents or a high number of selections can increase processing time, especially on less powerful computers.
- Saving your project and working on smaller parts of the document can reduce memory usage and speed up processing.

# 8. Summary

AnonPDF Pro provides a quick and intuitive way to remove selected areas in PDF documents. Saving and reloading projects enables staged processing and makes it easier to prepare multiple documents consistently. This helps protect sensitive information when sharing PDF materials.

# 9. New Features

Recent versions include:

- Printing from the `File` menu (`CTRL+P`) with range options: current page, selected page range, or whole document.
- `Save page range to PDF` in the `File` menu, with the current page prefilled by default.
- Two page preview tabs on the right: `Page List` and `Thumbnails`.
- Dynamic thumbnail generation plus thumbnail cache for better performance on large documents.
- Remembering the last selected right-side tab (`Page List` / `Thumbnails`).
- Remembering right panel width between application launches.
- Remembering and restoring scroll positions in right panels (`Page List` and `Thumbnails`) when resuming a project.
- Extended context menus for selections and objects (copy, cut, duplicate, remove duplicated copies).
- `Undo` (`CTRL+Z`) and `Redo` (`CTRL+Y`).
