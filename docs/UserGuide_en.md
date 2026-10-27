---
title: AnonPDF Pro - User Guide
lang: en
---

*English translation of the Polish help document*

# 1. Introduction

AnonPDF Pro is a tool for partially or fully removing (redacting) selected content in PDF files. It lets you mark areas in a document that should be removed and then generates a new PDF copy with the redactions applied. This makes it easy to remove sensitive data such as personal information or confidential content before sharing PDF documents with third parties.

# 2. Installation and Launch

> **1.** Download and install AnonPDF Pro (depending on the distribution method, this may be an MSI/EXE installer or a set of binaries).

> **2.** The application requires Windows and .NET Framework 4.8.

> **3.** Start the application, e.g., from the desktop shortcut, the Start menu, or directly by running AnonPDFPro.exe.

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

> \- File: open PDFs and projects, save the result, export page ranges, organize pages, change document settings, preview, and print.

> \- Search: quickly move focus to the search field.

> \- Options: change appearance, language, country (to tailor data scopes), and application behavior.

> \- Help: open this user guide.

- Right panel - contains the `Page List`, `Thumbnails`, `Layers`, and `Found` tabs. You can quickly switch pages, thumbnails, layer attributes, and review search results and detected personal data there.
- Main preview area - displays the currently selected PDF page and allows you to draw/select areas to be redacted.
- Toolbar - buttons for page navigation, zoom, and selection tools:

> \- Page navigation: "First", "Previous", "Next", "Last".

> \- Selection navigation: "First selection", "Previous selection", "Next selection", "Last selection".

> \- Zoom: "Zoom in", "Zoom out", "Min" (minimum), "Max" (maximum).

- Page number input - a text box for jumping to a specific page by entering its number.
- Redaction tool group:

> \- Cursor mode - clicking a word creates a redaction selection for that word. Clicking an existing selection or object selects it for further editing.

> \- Marker mode - marks horizontal areas (e.g., narrow strips for removing single text lines).

> \- Rectangle mode - marks any rectangular area.

> \- Button "Clear current page" - removes all selections from the currently displayed page.

> \- Button "Clear all pages" - removes all selections from every page in the document.

- Project I/O and PDF export controls:

> \- Open project - loads a .app project file containing saved redaction areas.

> \- Open last PDF and project - loads the most recently used PDF and its .app project.

> \- Save project - saves the current selections to the active .app project file.

> \- Save project as - saves the current selections to a new .app file at a chosen location.

> \- Save PDF (redaction) - generates a new PDF file with the selected areas removed.

# 4. Getting Started

## 4.1 Open a PDF

From the menu, select File → Open PDF (or click the "Select PDF" button), then choose the target PDF document.

## 4.2 Browse pages

Use `Page List` or `Thumbnails` in the right panel, or navigation buttons such as "Next" / "Previous", to select the page you want to edit.

## 4.3 Search

To search within the document:

> **1.** In the left panel, type the text you want to find and press Enter.

> **2.** Use the search navigation buttons to move between search results.

> **3.** Use the "PESEL, KW etc." button to check whether the document contains identifiers such as PESEL, Land and Mortgage Register number, Polish ID card number, NIP, REGON, KRS, bank account number, email address, postal code, website address, or VIN.

> **4.** Use the "Mark search results..." button to convert found results into redaction areas.

> **5.** To remove a search result, click the "X" button next to it.

Search also works in scanned documents and on images embedded in PDFs. For these documents, the application may first prepare the text for searching, so an indexing message may be visible for a short time.

If a scanned page is slightly skewed, the application tries to align search result positions with the actual text location on the page.

The `Personal data` button runs personal-data detection across the whole document. Search results and detected personal data - including from alternative text (ALT) and text outside the page area - appear in the `Found` tab (see section 6.14).

## 4.4 Mark areas for redaction

In `Cursor` mode, click the word you want to remove. This also works with words recognized by OCR. Adjust the selection with its handles if needed. To draw a selection in `Marker` or `Box` mode:

> **1.** Select Marker mode or Rectangle mode.

> **2.** Press and hold the left mouse button on the page preview where you want to mark content.

> **3.** In Marker mode you draw a horizontal strip; in Rectangle (box) mode you draw a rectangular area of any size.

> **4.** In Marker mode, hold `CTRL` to switch drawing to rectangular (box) mode.

> **5.** Release the mouse button to confirm the selection.

> **6.** A newly created `box` stays selected immediately. You can adjust its size using the top-left and bottom-right handles.

> **7.** Clicking an existing selection selects it and shows a short note at the top of the view. It does not start drawing another selection inside the existing one.

> **8.** To remove an existing selection (for example, if you added it by accident), right-click inside the selection area.

You can also remove a selected `box` or `marker` with the `DELETE` key. In Cursor mode, clicking an existing redaction area selects it without drawing a new area.

The context menu for a `Box` selection contains `Whole words`. Disable it to remove only the rectangle from a scan, without expanding to entire touched OCR words. This choice also applies to subsequent boxes in this document and is saved in the project. `Marker` still covers touched words; if it covers too much text, delete the selection and draw it again in the correct place.

## 4.5 Preview pending redactions

Text intended for removal is shaded gray in the preview. On OCR scans this helps detect accidental selection of a neighboring line. For a box with `Whole words` disabled, the rectangle determines the removal area. Check the final result in the built-in Preview before sharing the document.

## 4.6 Save your redaction project

If you want to save the current selection state for later editing, choose Save project or Save project as and select where to store the .app file. When you reopen the PDF and the corresponding .app project, you can continue adding or adjusting redactions.

## 4.7 Export (redact) the PDF

After marking all required areas, click Save PDF (or File → Save PDF). Select the destination output file. The application generates a new redacted PDF with your selected areas removed.

The suggested filename ends in `_anon`, but you can change it. Further final-PDF saves in the same document session suggest the last filename you chose.

Before saving, you can use `File -> Preview` (`CTRL+F2`). This opens a built-in preview of the document after redaction, without launching an external PDF viewer. In the preview window you can move between pages, enter a page number, and print the document in the same form in which it will be saved.

Preview opens maximized on the current page. It also includes objects, page arrangement, and margins from Document settings. `Show outline` and `Black fill` affect the final output appearance.

If you need page images instead of a PDF file, choose `File -> Save page range to JPEG`. After you select a folder, the application creates a subfolder named after the current PDF file and saves the selected pages as separate, numbered `.jpg` files. Export progress is shown in a label at the top of the view.

For fully scanned documents without their own text layer, the save dialog may also offer the `PDF/A files (*.pdf)` type. This creates an archival PDF/A file with the page image and an invisible text layer based on OCR recognition. The result is easier to search and better suited for digital publication or archiving.

If you choose PDF/A for a scanned document, the application remembers that choice and uses PDF/A as the default save type the next time a similar scanned document is saved. You can always switch back to the regular PDF type in the same `Save as type` field.

# 5. Selections and Navigation

## 5.1 Page navigation

Use the "First", "Previous", "Next", and "Last" buttons, or enter a page number in the page input box and press Enter. You can rotate the current page with `CTRL + R`.

## 5.2 Selection navigation

If your document contains many selections, use "First selection", "Previous selection", "Next selection", and "Last selection" to quickly jump between pages that contain redaction areas.

## 5.3 Zoom and scrolling

- Zoom in / Zoom out: use the "+" and "-" buttons or the "Min / Max" buttons to set minimum/maximum zoom.
- Mouse wheel: scrolling moves the page vertically. When you reach the top/bottom edge, the application automatically switches to the previous/next page.
- CTRL + mouse wheel: zoom out/in.
- F11 switches full-screen mode on or off; ESC exits full-screen mode.

# 6. Additional Features

## 6.1 Clearing selections

- `Clear current page` removes all redaction areas from the current page and requires confirmation.
- `Clear all` removes all redaction areas from the entire document and also requires confirmation.

## 6.2 Signatures

The application recognizes electronic signatures in the source document.

- `Options -> Select signatures to remove` selects signatures whose visual appearance should not be retained. Initially none are selected for removal.
- In the `Signatures` section choose `Preserved` to keep the remaining signature appearances, or `Report` to replace them with a signature-information page.
- For preserved signatures, `Appearance` offers the original appearance or `AnonPDFPro`, which shows verification status in the current application language.
- Verification status refers to the signature in the source PDF. Editing a document does not create a new electronic signature; the saved copy must not be treated as a newly signed document.

## 6.3 Removing pages

- Click the "Trash 1..n" icon in the left panel to mark a selected page range for removal (or to cancel removal).
- Click the "Trash" icon in the left panel to mark a single page for removal. The icon background turns black.
- You can set or undo the same deletion marks for one or several pages in `File -> Organize pages`. Source pages remain in the project and are excluded from the final PDF when saved.

## 6.4 Black fill

By default, the application removes content located inside the selection area. For images, graphics, or scanned pages, the removed area is filled with white and matches the selection size.

Enabling the `Black fill` option makes the selected areas black in the saved PDF.

## 6.5 Open after saving

When "Preview after saving" is enabled, the application automatically opens the saved PDF after export.

## 6.6 Loading a project

You can load a .app project file at any time (for example, prepared by another person or created earlier). This updates the selections in the application to match the loaded project.

Project files with the `.app` extension can also be associated with AnonPDF Pro in Windows, so they can be opened directly from File Explorer.

If the PDF path stored in the project is no longer available, the application also looks for a PDF file with the same name in the `.app` project folder.

## 6.7 Closing the application

When closing, the application may ask you to confirm exiting, especially if you have unsaved changes.

## 6.8 Help menu

- "User Guide" - opens this help/instruction content.
- "How to start?" - opens the short quick-start tutorial.
- "About" - shows version information, author, and copyright.
- The new version window can show a change summary and a download link or button when an update is available.

## 6.9 Creating and managing objects

You can add objects from the `Tools` menu or by using shortcuts:

- `CTRL + SHIFT + A` - add an arrow.
- `CTRL + F` - add a shape.
- `CTRL + G` - add an image.
- `CTRL + K` - add a comment.
- `CTRL + A` - select all visible objects on the active layer of the current page.

When working with objects, you can also use:

- `CTRL + C` - copy the active object or active object group.
- `CTRL + V` - paste an object from the clipboard. The pasted object is created on the active layer, keeps the original stacking order, and can be scaled down automatically if the target page is smaller.
- Object copy and paste also works between two running instances of the application.
- `DELETE` - remove the active object or active object group.
- `ESC` - clear the object selection without deleting objects.
- In the text dialog, you can enable the `Arrow` option for the text box frame.
- A text object's integrated arrow has separate fill and border colors, border width, line width, and head dimensions. Zero arrow border width means no arrow border, independently of the text frame.
- Image editing also offers ink recoloring, preserving edge shades that match the selected color.
- The end of the text arrow can be dragged with the mouse, and the attachment point on the frame updates automatically to corners and side centers.
- Rotating the text does not rotate the arrow end; only the attachment point on the frame is updated.
- Scaling the text also scales the frame and attached arrow as a single object.
- In the shape dialog, the `Limit raster view` option is available for rectangles, ellipses, triangles, and regions.
- This option is not used for redaction. It only limits the visible area of raster objects located below it on the same layer.
- In the preview, the raster fragment inside the shape stays clear while the area outside is dimmed. In the final PDF only the area limited by the shape remains visible.
- While creating multi-point shapes, the application shows a short instruction at the top of the view. The left mouse button adds another point and the right mouse button finishes drawing.

Modifiers while editing objects:

- `ALT` while creating shapes forces proportions: rectangle -> square, ellipse -> circle, triangle -> equilateral.
- `ALT` while resizing an image works symmetrically relative to the object center.
- Texts, arrows, and vector shapes can be scaled using corner handles.
- `SHIFT` while scaling preserves object proportions.
- `ALT` while scaling resizes relative to the object center.
- `SHIFT + ALT` while scaling preserves proportions and keeps the object center fixed.

## 6.10 Duplicating selections and objects

In the context menu (right mouse button), you can use:

- `Duplicate selection` - for redaction selections.
- `Duplicate object` - for objects (for example text, images, arrows, vector shapes, comments).

After selecting one of these options, a page range dialog is shown:

- `From` / `To` - page range where copies should be created.
- The current source page is skipped automatically.

For duplicated objects, an additional option is available:

- `Delete duplicated objects` - removes duplicated object copies in the selected page range.

## 6.11 Layers

The application supports working layers for redaction selections and objects.

- The `Layers` tab in the right panel lets you quickly change the active layer, visibility, and lock state.
- Layers can also be grouped by a shared group name.
- A group checkbox lets you change visibility or lock state for all layers in that group with one click.
- If layers in a group have different states, the group checkbox shows a mixed state.
- Double-clicking a group name in the `Layers` tab lets you rename that group.
- `Tools -> Layers` opens the full layer management window.
- In the layer management window you can assign a layer to a group using the `Group` column.
- In the layer management window, the `Export and print only visible layers` option overrides the `Export` column settings.
- New selections and new objects are created on the currently active layer.
- Pasted objects are created on the active layer and keep the source stacking order.
- The `Work` layer is always on top and is excluded from export by default. Check layer export settings before saving: exporting only visible layers uses visibility instead of the `Export` column.
- Reorder layers other than `Work` by dragging them in the layer management window.
- Hidden layers cannot be interacted with in the preview.
- Locked layers block moving, editing, scaling, and deleting objects.
- Project import keeps object-to-layer assignments.

## 6.12 Appearance and accessibility

- You can choose the application theme from the `Options` menu.
- High-contrast themes are available: black-yellow and black-white.
- The application better adapts interface sizes to Windows display scaling.

## 6.13 Save current view

`File -> Save current view...` saves the currently visible page view to a PNG or BMP image file. This saves the preview image and can be useful for documenting work or quickly sharing a page snapshot.

## 6.14 The "Found" tab

The `Found` tab in the right panel collects search results and detected personal data in one place.

- Items are grouped by page and have checkboxes.
- Each result shows the found content and its type. The filter can include several available categories at once, for example only NIP and PESEL.
- Toggling filter checkboxes immediately updates the list and yellow result highlights on the page. Hiding a category does not delete redaction selections already created.
- Clicking an item jumps to the relevant page and highlights its location in the document.
- Ticking an item creates a redaction selection; unticking it removes the selection. Checkbox states stay synchronized both ways with the selections on the page.
- Select individual results, all results on one page, or all results visible under the current filter. Clicking a highlighted word in `Cursor` mode is synchronized with its matching list item.

## 6.15 Personal data detection

The `Personal data` button analyzes the whole document and shows the results in the `Found` tab.

- This feature requires the appropriate PII plugin. Download it from the [product website](https://misart.pl/anonpdfpro/?lang=en#pii), extract it into the application's `plugins` folder, and restart the application. Wait for plugin initialization and document analysis to finish.
- Regardless of the module, identifier search (PESEL, Land and Mortgage Register number, NIP, REGON, KRS, bank account, ID card, email, postal code, website address, VIN) described in section 4.3 remains available.
- Results can be converted to redaction selections in bulk from the `Found` tab.

## 6.16 Alternative text (ALT)

The application also detects personal data in the alternative text (descriptions) of images and document elements.

- Such results appear in the `Found` tab under the `ALT text` category.
- The context menu (right-click) offers `Edit alternative text` and `Clear alternative text`. Changes are preserved in the exported PDF.
- Image descriptions can also be edited from the `Tools` menu. ALT descriptions or PDF/A export alone do not automatically make the entire document WCAG-compliant.

## 6.17 Copy selection text

From a redaction selection's context menu you can choose `Copy to clipboard` to copy the text covered by the selection.

- This works for both native text and pure scans (text from OCR).
- The text is assembled in natural reading order (top to bottom, left to right), including on rotated pages.

## 6.18 Country selection

In `Options -> Country` you can set the document country: `Automatic`, `Poland`, or `Germany`.

- This tailors the anonymization data scopes to the chosen country and language.
- `Automatic` uses the system regional settings and the detected document language.

## 6.19 JPEG 2000 and JBIG2 images

Redaction supports JPEG 2000 (JP2/JPX) images and black-and-white JBIG2 scans without prior conversion. Compression and output optimization settings still affect the saved PDF's size.

## 6.20 Faster document re-review

Document analysis results (including OCR text) are stored in an on-disk cache. Reopening the same file is therefore faster and does not require repeating the whole analysis.

## 6.21 Organizing pages

Choose `File -> Organize pages` to change the arrangement of the open document.

Thumbnails run from left to right and continue on the next row. A vertical line shows the insertion position while dragging. Thumbnails also show added objects, including on pages that were initially blank.

- `Add pages from PDF` lets you select pages from another document and insert them before or after the selected page.
- `Add blank page` lets you choose the format, orientation or custom dimensions in millimetres. The selected page's format is used by default.
- Change the order by dragging pages or using the move buttons. Several selected pages can be moved together.
- In a long document, select pages and click `Move`. Enter the destination page number and choose before or after that page. The number cannot exceed the current page count, including added pages. The moved pages are then shown. `Previous view` beside `Move` switches the view between the last source and destination locations without changing the page order or selection.
- The trash icon marks pages of the current document for removal when saving the final PDF, just like the existing page removal feature. Marked thumbnails are crossed out. Using the button again on marked pages undoes the mark. Pages newly added in this dialog are removed from the arrangement immediately.
- After `Apply`, redaction selections, objects, rotations and ALT edits stay with their pages. The changes also appear in preview, printing and the saved PDF.

Changes can be undone. Saving the project and resuming work preserve the modified document, including inserted pages. Organizing pages does not modify the original PDF files.

**Important:** after reorganization, the `.app` project also contains the PDF before redaction. Do not share it instead of the final redacted PDF. Changing page arrangement invalidates digital signatures in the modified document, not in the original files.

Use `CTRL` with clicks for separate pages and `SHIFT` for a range. `Apply` commits changes to the document; `Cancel` discards them. Thumbnails are prepared for the visible area and nearby rows to limit memory usage on large documents.

## 6.22 Document settings

In `File -> Document settings`, enter top, bottom, left, and right margins in whole millimetres. `0` means no margin. Each page's content is proportionally fitted into the remaining area, for example to leave room for a heading. Preview, printing, and final PDF/PDF/A output show the result. Settings are saved in the project and restored when work is resumed.

## 6.23 Automatically editing multiple documents

A saved project can serve as a template for the same redactions, headings, stamps, and other objects in similar documents. Use a `.cmd` script to run without an application window:

```bat
start "" /wait AnonPDFPro.exe "document.pdf" ^
  --headless --template "template.app" ^
  --pdfout "output.pdf" --lang en
```

- `--template` uses the project layout but ignores its saved PDF path; specify the input PDF separately.
- `--overwrite` allows replacement of an existing output file. Without it, the file is protected from overwriting.
- `--text` enables native-text indexing; `--ocr` also recognizes pages without text. This extra analysis is not run by default. The PII plugin is not loaded in this mode.
- `--format pdfa` selects PDF/A and automatically enables the required OCR. Normal PDF is the default output.
- `--skip-template-validation` skips the default comparison of page count, sizes, and rotations against the template, for example when later pages vary in number. It does not reposition or resize selections: the user must verify that they still cover the intended areas.
- `--help --lang en` lists all options and exit codes. Message languages are `pl`, `en`, and `de`.

Check representative output files before processing the full set. Prepare templates for documents with the same page layout.

## 6.24 Exclusion footnotes

In `Options`, set `Authority or person performing exclusions` and review the legal-basis dictionary. Then use a selection's context menu to choose an exclusion scope and legal basis; the automatic-footnotes option adds footnotes to the result.

An administrator maintains the global dictionary; users can manage local entries. Supplied legal-basis excerpts are examples that the organization must complete and keep current. Always review footnotes in Preview before publishing.

# 7. Common Issues and Tips

## 7.1 Cannot save the output PDF

- Make sure the PDF is not open in another application. The system may block saving if the file is currently in use.
- Choose a different output filename to avoid overwriting the original file.

## 7.2 Invalid project file format (.app)

Project files (.app) store redaction areas as JSON internally. Make sure you open the correct project file for the currently loaded PDF.

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
- Built-in `Preview` in the `File` menu (`CTRL+F2`) showing the document after redaction without an external PDF viewer.
- `Organize pages`: inserting pages from other PDFs and blank pages, moving groups by dragging or destination number, and `Previous view` navigation.
- `Document settings` with margins saved in the project.
- `Whole words` for Box selections and automatic processing of multiple documents using a project template.
- `Save page range to PDF` in the `File` menu, with the current page prefilled by default.
- `Save page range to JPEG` in the `File` menu, saving selected pages as numbered `.jpg` files in a separate folder.
- Saving fully scanned documents as PDF/A with an invisible OCR text layer, including remembering the last selected save type for scanned documents.
- Four right-side tabs: `Page List`, `Thumbnails`, `Layers`, and `Found`.
- Dynamic thumbnail generation plus thumbnail cache for better performance on large documents.
- Improved thumbnail refresh after changing the page filter.
- Thumbnails that also show redaction selections, comments, objects, and pages marked for deletion.
- Text search in scanned documents and images embedded in PDFs.
- Detection of personal data and identifiers also in scanned documents.
- Converting search results from scanned pages into redaction areas.
- `Cursor` mode for redacting words with a click and selecting existing redactions and objects.
- High-contrast themes: black-yellow and black-white.
- `Save current view...` in the `File` menu.
- Remembering the last selected right-side tab (`Page List` / `Thumbnails` / `Layers`).
- Remembering right panel width between application launches.
- Remembering and restoring scroll positions in right panels (`Page List` and `Thumbnails`) when resuming a project.
- Working layers with separate visibility, lock state, and active layer selection.
- Grouping layers by a shared name, with quick visibility and lock toggles for the whole group.
- Extended context menus for selections and objects (copy, cut, duplicate, remove duplicated copies).
- Corner-handle scaling for single objects and object groups (`SHIFT` - proportional, `ALT` - from center).
- The `Arrow` option in the text dialog, with adjustable dimensions, fill and border colors, and border width.
- The `Limit raster view` option in the shape dialog for rectangles, ellipses, triangles, and regions.
- Object copy and paste through the system clipboard, including between two running application instances, with preserved stacking order.
- `Undo` (`CTRL+Z`) and `Redo` (`CTRL+Y`).
- A short quick-start tutorial available from `? -> How to start?`.
- A new `Found` tab with search results and detected personal data, checkboxes, a category filter, and jump-to-page.
- A `Personal data` button that runs personal-data detection (name and location recognition requires an optional module for the selected country).
- Detection and editing of personal data in the alternative text (ALT) of images and elements.
- Grouping of text outside the page area under the `Outside page` category.
- Document country selection in `Options -> Country` that tailors the anonymization data scopes.
- Copying selection text to the clipboard (`Copy to clipboard`) in correct reading order, including on rotated pages and pure scans.
- Gray preview of touched text also on pure scans (based on OCR).
- Saving a selected page range to numbered JPEG files.
- Redaction support for JPEG 2000 and JBIG2 images.
- Faster document re-review thanks to an on-disk cache.
