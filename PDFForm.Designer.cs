namespace AnonPDF
{
    partial class PDFForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PDFForm));
            this.pdfViewer = new System.Windows.Forms.PictureBox();
            this.mainAppSplitContainer = new System.Windows.Forms.SplitContainer();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuFileItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openPdfToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openLastPdfProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recentFilesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.saveProjectAsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveProjectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.savePdfMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.savePdfPageRangeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemPrintSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.printPdfMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.importProjectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemCloseDocumentSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.closeDocumentMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOptionsItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitPdfToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mergePdfToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deletePageMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rotatePageMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addTextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToClipboardMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportGraphicsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectSignaturesToRemoveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.themeToolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.themeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.themeBalticBreezeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.themeSoftLightMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.themeNordCoolLightMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.themeWarmSandMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.themeForestGreenMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.themeGraphiteDarkMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.themeOledDarkTealMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.themeBlackYellowHighContrastMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.themeBlackWhiteHighContrastMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.languageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.languageSystemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.languageEnglishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.languagePolishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.languageGermanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fullScreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ignorePdfRestrictionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelpItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.whatsNewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showLicenseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quickStartMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tutorialMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.diagnosticModeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveProjectAsButton = new System.Windows.Forms.Button();
            this.thirdPartyNoticesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pagesListView = new System.Windows.Forms.ListView();
            this.thumbnailsListView = new System.Windows.Forms.ListView();
            this.thumbnailsImageList = new System.Windows.Forms.ImageList(this.components);
            this.formSplitContainer = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pagesTabControl = new System.Windows.Forms.TabControl();
            this.pagesListTabPage = new System.Windows.Forms.TabPage();
            this.thumbnailsTabPage = new System.Windows.Forms.TabPage();
            this.layersTabPage = new System.Windows.Forms.TabPage();
            this.layersTabPlaceholderPanel = new System.Windows.Forms.Panel();
            this.groupBoxOptions = new AnonPDF.ThemedGroupBox();
            this.setSavePassword = new AnonPDF.ThemedCheckBox();
            this.safeModeCheckBox = new AnonPDF.ThemedCheckBox();
            this.openSavedPDFCheckBox = new AnonPDF.ThemedCheckBox();
            this.outlineCheckBox = new AnonPDF.ThemedCheckBox();
            this.colorCheckBox = new AnonPDF.ThemedCheckBox();
            this.groupBoxPagesToRemove = new AnonPDF.ThemedGroupBox();
            this.removePageButton = new System.Windows.Forms.Button();
            this.removePageRangeButton = new System.Windows.Forms.Button();
            this.groupBoxSignatures = new AnonPDF.ThemedGroupBox();
            this.signaturesReportRadioButton = new AnonPDF.ThemedRadioButton();
            this.signaturesOriginalRadioButton = new AnonPDF.ThemedRadioButton();
            this.signaturesRemoveRadioButton = new AnonPDF.ThemedRadioButton();
            this.groupBoxPages = new AnonPDF.ThemedGroupBox();
            this.pageNumberTextBox = new System.Windows.Forms.TextBox();
            this.numPagesLabel = new System.Windows.Forms.Label();
            this.buttonFirst = new System.Windows.Forms.Button();
            this.buttonNextPage = new System.Windows.Forms.Button();
            this.buttonPrevious = new System.Windows.Forms.Button();
            this.zoomMinButton = new System.Windows.Forms.Button();
            this.zoomMaxButton = new System.Windows.Forms.Button();
            this.zoomOutButton = new System.Windows.Forms.Button();
            this.zoomInButton = new System.Windows.Forms.Button();
            this.buttonLast = new System.Windows.Forms.Button();
            this.groupBoxSearch = new AnonPDF.ThemedGroupBox();
            this.searchButton = new System.Windows.Forms.Button();
            this.searchToSelectionButton = new System.Windows.Forms.Button();
            this.searchResultLabel = new System.Windows.Forms.Label();
            this.SearchClearButton = new System.Windows.Forms.Button();
            this.personalDataButton = new System.Windows.Forms.Button();
            this.searchLastButton = new System.Windows.Forms.Button();
            this.searchNextButton = new System.Windows.Forms.Button();
            this.searchPrevButton = new System.Windows.Forms.Button();
            this.searchFirstButton = new System.Windows.Forms.Button();
            this.searchTextBox = new System.Windows.Forms.TextBox();
            this.groupBoxOpen = new AnonPDF.ThemedGroupBox();
            this.openProjectButton = new System.Windows.Forms.Button();
            this.loadPdfButton = new System.Windows.Forms.Button();
            this.groupBoxSave = new AnonPDF.ThemedGroupBox();
            this.buttonRedactText = new System.Windows.Forms.Button();
            this.saveProjectButton = new System.Windows.Forms.Button();
            this.groupBoxSelections = new AnonPDF.ThemedGroupBox();
            this.selectionLastButton = new System.Windows.Forms.Button();
            this.selectionNextButton = new System.Windows.Forms.Button();
            this.selectionPrevButton = new System.Windows.Forms.Button();
            this.selectionFirstButton = new System.Windows.Forms.Button();
            this.cursorRadioButton = new AnonPDF.ThemedRadioButton();
            this.markerRadioButton = new AnonPDF.ThemedRadioButton();
            this.boxRadioButton = new AnonPDF.ThemedRadioButton();
            this.clearPageButton = new System.Windows.Forms.Button();
            this.clearSelectionButton = new System.Windows.Forms.Button();
            this.groupBoxFilter = new AnonPDF.ThemedGroupBox();
            this.filterComboBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.pdfViewer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainAppSplitContainer)).BeginInit();
            this.mainAppSplitContainer.Panel1.SuspendLayout();
            this.mainAppSplitContainer.Panel2.SuspendLayout();
            this.mainAppSplitContainer.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.formSplitContainer)).BeginInit();
            this.formSplitContainer.Panel1.SuspendLayout();
            this.formSplitContainer.Panel2.SuspendLayout();
            this.formSplitContainer.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.pagesTabControl.SuspendLayout();
            this.pagesListTabPage.SuspendLayout();
            this.thumbnailsTabPage.SuspendLayout();
            this.layersTabPage.SuspendLayout();
            this.groupBoxOptions.SuspendLayout();
            this.groupBoxPagesToRemove.SuspendLayout();
            this.groupBoxSignatures.SuspendLayout();
            this.groupBoxPages.SuspendLayout();
            this.groupBoxSearch.SuspendLayout();
            this.groupBoxOpen.SuspendLayout();
            this.groupBoxSave.SuspendLayout();
            this.groupBoxSelections.SuspendLayout();
            this.groupBoxFilter.SuspendLayout();
            this.SuspendLayout();
            //
            // pdfViewer
            //
            this.pdfViewer.Location = new System.Drawing.Point(0, 0);
            this.pdfViewer.Name = "pdfViewer";
            this.pdfViewer.Size = new System.Drawing.Size(300, 300);
            this.pdfViewer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pdfViewer.TabIndex = 1;
            this.pdfViewer.TabStop = false;
            //
            // mainAppSplitContainer
            //
            this.mainAppSplitContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.mainAppSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainAppSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.mainAppSplitContainer.IsSplitterFixed = true;
            this.mainAppSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.mainAppSplitContainer.Name = "mainAppSplitContainer";
            //
            // mainAppSplitContainer.Panel1
            //
            this.mainAppSplitContainer.Panel1.Controls.Add(this.groupBoxOptions);
            this.mainAppSplitContainer.Panel1.Controls.Add(this.groupBoxPagesToRemove);
            this.mainAppSplitContainer.Panel1.Controls.Add(this.groupBoxSignatures);
            this.mainAppSplitContainer.Panel1.Controls.Add(this.groupBoxPages);
            this.mainAppSplitContainer.Panel1.Controls.Add(this.groupBoxSearch);
            this.mainAppSplitContainer.Panel1.Controls.Add(this.groupBoxOpen);
            this.mainAppSplitContainer.Panel1.Controls.Add(this.groupBoxSave);
            this.mainAppSplitContainer.Panel1.Controls.Add(this.groupBoxSelections);
            this.mainAppSplitContainer.Panel1.Controls.Add(this.menuStrip1);
            //
            // mainAppSplitContainer.Panel2
            //
            this.mainAppSplitContainer.Panel2.Controls.Add(this.pdfViewer);
            this.mainAppSplitContainer.Panel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.mainAppSplitContainer.Size = new System.Drawing.Size(1014, 912);
            this.mainAppSplitContainer.SplitterDistance = 213;
            this.mainAppSplitContainer.TabIndex = 0;
            this.mainAppSplitContainer.TabStop = false;
            //
            // menuStrip1
            //
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFileItem,
            this.menuOptionsItem,
            this.menuHelpItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(209, 24);
            this.menuStrip1.TabIndex = 18;
            //
            // menuFileItem
            //
            this.menuFileItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openPdfToolStripMenuItem,
            this.openProjectToolStripMenuItem,
            this.openLastPdfProjectToolStripMenuItem,
            this.recentFilesMenuItem,
            this.toolStripMenuItem2,
            this.saveProjectAsMenuItem,
            this.saveProjectMenuItem,
            this.savePdfMenuItem,
            this.savePdfPageRangeMenuItem,
            this.toolStripMenuItemPrintSeparator,
            this.printPdfMenuItem,
            this.toolStripMenuItem1,
            this.importProjectMenuItem,
            this.toolStripMenuItemCloseDocumentSeparator,
            this.closeDocumentMenuItem,
            this.exitMenuItem});
            this.menuFileItem.Name = "menuFileItem";
            this.menuFileItem.Size = new System.Drawing.Size(73, 20);
            this.menuFileItem.Text = "Menu_File";
            //
            // openPdfToolStripMenuItem
            //
            this.openPdfToolStripMenuItem.Name = "openPdfToolStripMenuItem";
            this.openPdfToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.openPdfToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.openPdfToolStripMenuItem.Text = "Menu_OpenPdf";
            this.openPdfToolStripMenuItem.Click += new System.EventHandler(this.LoadPdfButton_Click);
            //
            // openProjectToolStripMenuItem
            //
            this.openProjectToolStripMenuItem.Name = "openProjectToolStripMenuItem";
            this.openProjectToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openProjectToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.openProjectToolStripMenuItem.Text = "Menu_OpenProject";
            this.openProjectToolStripMenuItem.Click += new System.EventHandler(this.OpenProjectButton_Click);
            //
            // openLastPdfProjectToolStripMenuItem
            //
            this.openLastPdfProjectToolStripMenuItem.Name = "openLastPdfProjectToolStripMenuItem";
            this.openLastPdfProjectToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.openLastPdfProjectToolStripMenuItem.Text = "Menu_OpenLast";
            this.openLastPdfProjectToolStripMenuItem.Click += new System.EventHandler(this.OpenLastPdfProjectToolStripMenuItem_Click);
            //
            // recentFilesMenuItem
            //
            this.recentFilesMenuItem.Name = "recentFilesMenuItem";
            this.recentFilesMenuItem.Size = new System.Drawing.Size(224, 22);
            this.recentFilesMenuItem.Text = "Menu_RecentFiles";
            this.recentFilesMenuItem.DropDownOpening += new System.EventHandler(this.RecentFilesMenuItem_DropDownOpening);
            //
            // toolStripMenuItem2
            //
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(221, 6);
            //
            // saveProjectAsMenuItem
            //
            this.saveProjectAsMenuItem.Enabled = false;
            this.saveProjectAsMenuItem.Name = "saveProjectAsMenuItem";
            this.saveProjectAsMenuItem.Size = new System.Drawing.Size(224, 22);
            this.saveProjectAsMenuItem.Text = "Menu_SaveProjectAs";
            this.saveProjectAsMenuItem.Click += new System.EventHandler(this.SaveProjectAsButton_Click);
            //
            // saveProjectMenuItem
            //
            this.saveProjectMenuItem.Enabled = false;
            this.saveProjectMenuItem.Name = "saveProjectMenuItem";
            this.saveProjectMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveProjectMenuItem.Size = new System.Drawing.Size(224, 22);
            this.saveProjectMenuItem.Text = "Menu_SaveProject";
            this.saveProjectMenuItem.Click += new System.EventHandler(this.SaveProjectButton_Click);
            //
            // savePdfMenuItem
            //
            this.savePdfMenuItem.Enabled = false;
            this.savePdfMenuItem.Name = "savePdfMenuItem";
            this.savePdfMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
            | System.Windows.Forms.Keys.S)));
            this.savePdfMenuItem.Size = new System.Drawing.Size(224, 22);
            this.savePdfMenuItem.Text = "Menu_SavePdf";
            this.savePdfMenuItem.Click += new System.EventHandler(this.ButtonRedactText_Click);
            //
            // savePdfPageRangeMenuItem
            //
            this.savePdfPageRangeMenuItem.Enabled = false;
            this.savePdfPageRangeMenuItem.Name = "savePdfPageRangeMenuItem";
            this.savePdfPageRangeMenuItem.Size = new System.Drawing.Size(224, 22);
            this.savePdfPageRangeMenuItem.Text = "Menu_SavePdfPageRange";
            this.savePdfPageRangeMenuItem.Click += new System.EventHandler(this.SavePdfPageRangeMenuItem_Click);
            //
            // toolStripMenuItemPrintSeparator
            //
            this.toolStripMenuItemPrintSeparator.Name = "toolStripMenuItemPrintSeparator";
            this.toolStripMenuItemPrintSeparator.Size = new System.Drawing.Size(221, 6);
            //
            // printPdfMenuItem
            //
            this.printPdfMenuItem.Enabled = false;
            this.printPdfMenuItem.Name = "printPdfMenuItem";
            this.printPdfMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.printPdfMenuItem.Size = new System.Drawing.Size(224, 22);
            this.printPdfMenuItem.Text = "Menu_Print";
            this.printPdfMenuItem.Click += new System.EventHandler(this.PrintPdfMenuItem_Click);
            //
            // toolStripMenuItem1
            //
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(221, 6);
            //
            // importProjectMenuItem
            //
            this.importProjectMenuItem.Name = "importProjectMenuItem";
            this.importProjectMenuItem.Size = new System.Drawing.Size(224, 22);
            this.importProjectMenuItem.Text = "Menu_ImportProject";
            this.importProjectMenuItem.Click += new System.EventHandler(this.ImportProjectMenuItem_Click);
            //
            // toolStripMenuItemCloseDocumentSeparator
            //
            this.toolStripMenuItemCloseDocumentSeparator.Name = "toolStripMenuItemCloseDocumentSeparator";
            this.toolStripMenuItemCloseDocumentSeparator.Size = new System.Drawing.Size(221, 6);
            //
            // closeDocumentMenuItem
            //
            this.closeDocumentMenuItem.Enabled = false;
            this.closeDocumentMenuItem.Name = "closeDocumentMenuItem";
            this.closeDocumentMenuItem.Size = new System.Drawing.Size(224, 22);
            this.closeDocumentMenuItem.Text = "Menu_CloseDocument";
            this.closeDocumentMenuItem.Click += new System.EventHandler(this.CloseDocumentMenuItem_Click);
            //
            // exitMenuItem
            //
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitMenuItem.Size = new System.Drawing.Size(224, 22);
            this.exitMenuItem.Text = "Menu_Exit";
            this.exitMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            //
            // menuOptionsItem
            //
            this.menuOptionsItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.splitPdfToolStripMenuItem,
            this.mergePdfToolStripMenuItem,
            this.deletePageMenuItem,
            this.rotatePageMenuItem,
            this.addTextMenuItem,
            this.copyToClipboardMenuItem,
            this.exportGraphicsMenuItem,
            this.selectSignaturesToRemoveMenuItem,
            this.themeToolStripSeparator,
            this.themeToolStripMenuItem,
            this.languageToolStripMenuItem,
            this.fullScreenToolStripMenuItem,
            this.toolStripSeparator1,
            this.ignorePdfRestrictionsToolStripMenuItem});
            this.menuOptionsItem.Name = "menuOptionsItem";
            this.menuOptionsItem.Size = new System.Drawing.Size(97, 20);
            this.menuOptionsItem.Text = "Menu_Options";
            //
            // splitPdfToolStripMenuItem
            //
            this.splitPdfToolStripMenuItem.Name = "splitPdfToolStripMenuItem";
            this.splitPdfToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.splitPdfToolStripMenuItem.Size = new System.Drawing.Size(252, 22);
            this.splitPdfToolStripMenuItem.Text = "Menu_SplitPdf";
            this.splitPdfToolStripMenuItem.Click += new System.EventHandler(this.SplitPdfToolStripMenuItem_Click);
            //
            // mergePdfToolStripMenuItem
            //
            this.mergePdfToolStripMenuItem.Name = "mergePdfToolStripMenuItem";
            this.mergePdfToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.mergePdfToolStripMenuItem.Size = new System.Drawing.Size(252, 22);
            this.mergePdfToolStripMenuItem.Text = "Menu_MergePdf";
            this.mergePdfToolStripMenuItem.Click += new System.EventHandler(this.MergePdfToolStripMenuItem_Click);
            //
            // deletePageMenuItem
            //
            this.deletePageMenuItem.Enabled = false;
            this.deletePageMenuItem.Name = "deletePageMenuItem";
            this.deletePageMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.deletePageMenuItem.Size = new System.Drawing.Size(252, 22);
            this.deletePageMenuItem.Text = "Menu_DeletePage";
            this.deletePageMenuItem.Click += new System.EventHandler(this.DeletePageToolStripMenuItem_Click);
            //
            // rotatePageMenuItem
            //
            this.rotatePageMenuItem.Enabled = false;
            this.rotatePageMenuItem.Name = "rotatePageMenuItem";
            this.rotatePageMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.rotatePageMenuItem.Size = new System.Drawing.Size(252, 22);
            this.rotatePageMenuItem.Text = "Menu_RotatePage";
            this.rotatePageMenuItem.Click += new System.EventHandler(this.RotatePageMenuItem_Click);
            //
            // addTextMenuItem
            //
            this.addTextMenuItem.Enabled = false;
            this.addTextMenuItem.Name = "addTextMenuItem";
            this.addTextMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.addTextMenuItem.Size = new System.Drawing.Size(252, 22);
            this.addTextMenuItem.Text = "Menu_AddText";
            this.addTextMenuItem.Click += new System.EventHandler(this.AddTextToolStripMenuItem_Click);
            //
            // copyToClipboardMenuItem
            //
            this.copyToClipboardMenuItem.Enabled = false;
            this.copyToClipboardMenuItem.Name = "copyToClipboardMenuItem";
            this.copyToClipboardMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyToClipboardMenuItem.Size = new System.Drawing.Size(252, 22);
            this.copyToClipboardMenuItem.Text = "Menu_CopyToClipboard";
            this.copyToClipboardMenuItem.Click += new System.EventHandler(this.CopyToClipboardToolStripMenuItem_Click);
            //
            // exportGraphicsMenuItem
            //
            this.exportGraphicsMenuItem.Enabled = false;
            this.exportGraphicsMenuItem.Name = "exportGraphicsMenuItem";
            this.exportGraphicsMenuItem.Size = new System.Drawing.Size(252, 22);
            this.exportGraphicsMenuItem.Text = "Menu_ExportGraphics";
            this.exportGraphicsMenuItem.Click += new System.EventHandler(this.ExportGraphicsToolStripMenuItem_Click);
            //
            // selectSignaturesToRemoveMenuItem
            //
            this.selectSignaturesToRemoveMenuItem.Enabled = false;
            this.selectSignaturesToRemoveMenuItem.Name = "selectSignaturesToRemoveMenuItem";
            this.selectSignaturesToRemoveMenuItem.Size = new System.Drawing.Size(252, 22);
            this.selectSignaturesToRemoveMenuItem.Text = "Menu_SelectSignaturesToRemove";
            this.selectSignaturesToRemoveMenuItem.Click += new System.EventHandler(this.SelectSignaturesToRemoveMenuItem_Click);
            //
            // themeToolStripSeparator
            //
            this.themeToolStripSeparator.Name = "themeToolStripSeparator";
            this.themeToolStripSeparator.Size = new System.Drawing.Size(249, 6);
            //
            // themeToolStripMenuItem
            //
            this.themeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.themeBalticBreezeMenuItem,
            this.themeSoftLightMenuItem,
            this.themeNordCoolLightMenuItem,
            this.themeWarmSandMenuItem,
            this.themeForestGreenMenuItem,
            this.themeGraphiteDarkMenuItem,
            this.themeOledDarkTealMenuItem,
            this.themeBlackYellowHighContrastMenuItem,
            this.themeBlackWhiteHighContrastMenuItem});
            this.themeToolStripMenuItem.Name = "themeToolStripMenuItem";
            this.themeToolStripMenuItem.Size = new System.Drawing.Size(252, 22);
            this.themeToolStripMenuItem.Text = "Menu_Options_Theme";
            //
            // themeBalticBreezeMenuItem
            //
            this.themeBalticBreezeMenuItem.Name = "themeBalticBreezeMenuItem";
            this.themeBalticBreezeMenuItem.Size = new System.Drawing.Size(195, 22);
            this.themeBalticBreezeMenuItem.Text = "Theme_BalticBreeze";
            this.themeBalticBreezeMenuItem.Click += new System.EventHandler(this.ThemeBalticBreezeMenuItem_Click);
            //
            // themeSoftLightMenuItem
            //
            this.themeSoftLightMenuItem.Name = "themeSoftLightMenuItem";
            this.themeSoftLightMenuItem.Size = new System.Drawing.Size(195, 22);
            this.themeSoftLightMenuItem.Text = "Theme_SoftLight";
            this.themeSoftLightMenuItem.Click += new System.EventHandler(this.ThemeSoftLightMenuItem_Click);
            //
            // themeNordCoolLightMenuItem
            //
            this.themeNordCoolLightMenuItem.Name = "themeNordCoolLightMenuItem";
            this.themeNordCoolLightMenuItem.Size = new System.Drawing.Size(195, 22);
            this.themeNordCoolLightMenuItem.Text = "Theme_NordCoolLight";
            this.themeNordCoolLightMenuItem.Click += new System.EventHandler(this.ThemeNordCoolLightMenuItem_Click);
            //
            // themeWarmSandMenuItem
            //
            this.themeWarmSandMenuItem.Name = "themeWarmSandMenuItem";
            this.themeWarmSandMenuItem.Size = new System.Drawing.Size(195, 22);
            this.themeWarmSandMenuItem.Text = "Theme_WarmSand";
            this.themeWarmSandMenuItem.Click += new System.EventHandler(this.ThemeWarmSandMenuItem_Click);
            //
            // themeForestGreenMenuItem
            //
            this.themeForestGreenMenuItem.Name = "themeForestGreenMenuItem";
            this.themeForestGreenMenuItem.Size = new System.Drawing.Size(195, 22);
            this.themeForestGreenMenuItem.Text = "Theme_ForestGreen";
            this.themeForestGreenMenuItem.Click += new System.EventHandler(this.ThemeForestGreenMenuItem_Click);
            //
            // themeGraphiteDarkMenuItem
            //
            this.themeGraphiteDarkMenuItem.Name = "themeGraphiteDarkMenuItem";
            this.themeGraphiteDarkMenuItem.Size = new System.Drawing.Size(195, 22);
            this.themeGraphiteDarkMenuItem.Text = "Theme_GraphiteDark";
            this.themeGraphiteDarkMenuItem.Click += new System.EventHandler(this.ThemeGraphiteDarkMenuItem_Click);
            //
            // themeOledDarkTealMenuItem
            //
            this.themeOledDarkTealMenuItem.Name = "themeOledDarkTealMenuItem";
            this.themeOledDarkTealMenuItem.Size = new System.Drawing.Size(195, 22);
            this.themeOledDarkTealMenuItem.Text = "Theme_OledDarkTeal";
            this.themeOledDarkTealMenuItem.Click += new System.EventHandler(this.ThemeOledDarkTealMenuItem_Click);
            //
            // themeBlackYellowHighContrastMenuItem
            //
            this.themeBlackYellowHighContrastMenuItem.Name = "themeBlackYellowHighContrastMenuItem";
            this.themeBlackYellowHighContrastMenuItem.Size = new System.Drawing.Size(195, 22);
            this.themeBlackYellowHighContrastMenuItem.Text = "Theme_BlackYellowHighContrast";
            this.themeBlackYellowHighContrastMenuItem.Click += new System.EventHandler(this.ThemeBlackYellowHighContrastMenuItem_Click);
            //
            // themeBlackWhiteHighContrastMenuItem
            //
            this.themeBlackWhiteHighContrastMenuItem.Name = "themeBlackWhiteHighContrastMenuItem";
            this.themeBlackWhiteHighContrastMenuItem.Size = new System.Drawing.Size(195, 22);
            this.themeBlackWhiteHighContrastMenuItem.Text = "Theme_BlackWhiteHighContrast";
            this.themeBlackWhiteHighContrastMenuItem.Click += new System.EventHandler(this.ThemeBlackWhiteHighContrastMenuItem_Click);
            //
            // languageToolStripMenuItem
            //
            this.languageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.languageSystemToolStripMenuItem,
            this.languageEnglishToolStripMenuItem,
            this.languagePolishToolStripMenuItem,
            this.languageGermanToolStripMenuItem});
            this.languageToolStripMenuItem.Name = "languageToolStripMenuItem";
            this.languageToolStripMenuItem.Size = new System.Drawing.Size(252, 22);
            this.languageToolStripMenuItem.Text = "Menu_Language";
            //
            // languageSystemToolStripMenuItem
            //
            this.languageSystemToolStripMenuItem.Name = "languageSystemToolStripMenuItem";
            this.languageSystemToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.languageSystemToolStripMenuItem.Text = "Menu_Language_System";
            this.languageSystemToolStripMenuItem.Click += new System.EventHandler(this.LanguageSystemToolStripMenuItem_Click);
            //
            // languageEnglishToolStripMenuItem
            //
            this.languageEnglishToolStripMenuItem.Name = "languageEnglishToolStripMenuItem";
            this.languageEnglishToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.languageEnglishToolStripMenuItem.Text = "Menu_Language_English";
            this.languageEnglishToolStripMenuItem.Click += new System.EventHandler(this.LanguageEnglishToolStripMenuItem_Click);
            //
            // languagePolishToolStripMenuItem
            //
            this.languagePolishToolStripMenuItem.Name = "languagePolishToolStripMenuItem";
            this.languagePolishToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.languagePolishToolStripMenuItem.Text = "Menu_Language_Polish";
            this.languagePolishToolStripMenuItem.Click += new System.EventHandler(this.LanguagePolishToolStripMenuItem_Click);
            //
            // languageGermanToolStripMenuItem
            //
            this.languageGermanToolStripMenuItem.Name = "languageGermanToolStripMenuItem";
            this.languageGermanToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.languageGermanToolStripMenuItem.Text = "Menu_Language_German";
            this.languageGermanToolStripMenuItem.Click += new System.EventHandler(this.LanguageGermanToolStripMenuItem_Click);
            //
            // fullScreenToolStripMenuItem
            //
            this.fullScreenToolStripMenuItem.Name = "fullScreenToolStripMenuItem";
            this.fullScreenToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F11;
            this.fullScreenToolStripMenuItem.Size = new System.Drawing.Size(252, 22);
            this.fullScreenToolStripMenuItem.Text = "Menu_Options_FullScreen";
            this.fullScreenToolStripMenuItem.Click += new System.EventHandler(this.FullScreenToolStripMenuItem_Click);
            //
            // toolStripSeparator1
            //
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(249, 6);
            //
            // ignorePdfRestrictionsToolStripMenuItem
            //
            this.ignorePdfRestrictionsToolStripMenuItem.CheckOnClick = true;
            this.ignorePdfRestrictionsToolStripMenuItem.Name = "ignorePdfRestrictionsToolStripMenuItem";
            this.ignorePdfRestrictionsToolStripMenuItem.Size = new System.Drawing.Size(252, 22);
            this.ignorePdfRestrictionsToolStripMenuItem.Text = "Menu_IgnorePdfRestrictions";
            this.ignorePdfRestrictionsToolStripMenuItem.ToolTipText = "Menu_IgnorePdfRestrictions_Tooltip";
            this.ignorePdfRestrictionsToolStripMenuItem.CheckedChanged += new System.EventHandler(this.IgnorePdfRestrictionsToolStripMenuItem_CheckedChanged);
            //
            // menuHelpItem
            //
            this.menuHelpItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpMenuItem,
            this.whatsNewMenuItem,
            this.showLicenseToolStripMenuItem,
            this.quickStartMenuItem,
            this.tutorialMenuItem,
            this.diagnosticModeMenuItem,
            this.aboutMenuItem});
            this.menuHelpItem.Name = "menuHelpItem";
            this.menuHelpItem.Size = new System.Drawing.Size(80, 20);
            this.menuHelpItem.Text = "Menu_Help";
            //
            // helpMenuItem
            //
            this.helpMenuItem.Name = "helpMenuItem";
            this.helpMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F1)));
            this.helpMenuItem.Size = new System.Drawing.Size(236, 22);
            this.helpMenuItem.Text = "Menu_Help_Help";
            this.helpMenuItem.Click += new System.EventHandler(this.HelpToolStripMenuItem_Click);
            //
            // whatsNewMenuItem
            //
            this.whatsNewMenuItem.Name = "whatsNewMenuItem";
            this.whatsNewMenuItem.Size = new System.Drawing.Size(236, 22);
            this.whatsNewMenuItem.Text = "Menu_Help_WhatsNew";
            this.whatsNewMenuItem.Click += new System.EventHandler(this.WhatsNewMenuItem_Click);
            //
            // showLicenseToolStripMenuItem
            //
            this.showLicenseToolStripMenuItem.Name = "showLicenseToolStripMenuItem";
            this.showLicenseToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.showLicenseToolStripMenuItem.Text = "Menu_Help_Licenses";
            this.showLicenseToolStripMenuItem.Click += new System.EventHandler(this.ShowLicenseToolStripMenuItem_Click);
            //
            // quickStartMenuItem
            //
            this.quickStartMenuItem.Name = "quickStartMenuItem";
            this.quickStartMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.quickStartMenuItem.Size = new System.Drawing.Size(236, 22);
            this.quickStartMenuItem.Text = "Menu_Help_QuickStart";
            this.quickStartMenuItem.Click += new System.EventHandler(this.QuickStartMenuItem_Click);
            //
            // tutorialMenuItem
            //
            this.tutorialMenuItem.Name = "tutorialMenuItem";
            this.tutorialMenuItem.Size = new System.Drawing.Size(236, 22);
            this.tutorialMenuItem.Text = "Menu_Help_Tutorial";
            this.tutorialMenuItem.Click += new System.EventHandler(this.TutorialToolStripMenuItem_Click);
            //
            // diagnosticModeMenuItem
            //
            this.diagnosticModeMenuItem.CheckOnClick = true;
            this.diagnosticModeMenuItem.Name = "diagnosticModeMenuItem";
            this.diagnosticModeMenuItem.Size = new System.Drawing.Size(236, 22);
            this.diagnosticModeMenuItem.Text = "Menu_Help_DiagnosticMode";
            this.diagnosticModeMenuItem.CheckedChanged += new System.EventHandler(this.DiagnosticModeMenuItem_CheckedChanged);
            //
            // aboutMenuItem
            //
            this.aboutMenuItem.Name = "aboutMenuItem";
            this.aboutMenuItem.Size = new System.Drawing.Size(236, 22);
            this.aboutMenuItem.Text = "Menu_Help_About";
            this.aboutMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
            //
            // saveProjectAsButton
            //
            this.saveProjectAsButton.Enabled = false;
            this.saveProjectAsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.saveProjectAsButton.Location = new System.Drawing.Point(15, 18);
            this.saveProjectAsButton.Name = "saveProjectAsButton";
            this.saveProjectAsButton.Size = new System.Drawing.Size(154, 23);
            this.saveProjectAsButton.TabIndex = 0;
            this.saveProjectAsButton.Text = "UI_Button_SaveProjectAs";
            this.toolTip1.SetToolTip(this.saveProjectAsButton, "Tooltip_SaveProjectAs");
            this.saveProjectAsButton.UseVisualStyleBackColor = true;
            this.saveProjectAsButton.Visible = false;
            this.saveProjectAsButton.Click += new System.EventHandler(this.SaveProjectAsButton_Click);
            //
            // thirdPartyNoticesToolStripMenuItem
            //
            this.thirdPartyNoticesToolStripMenuItem.Name = "thirdPartyNoticesToolStripMenuItem";
            this.thirdPartyNoticesToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.thirdPartyNoticesToolStripMenuItem.Text = "Menu_Help_ThirdParty";
            this.thirdPartyNoticesToolStripMenuItem.Click += new System.EventHandler(this.ThirdPartyNoticesToolStripMenuItem_Click);
            //
            // toolTip1
            //
            this.toolTip1.IsBalloon = true;
            //
            // pagesListView
            //
            this.pagesListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pagesListView.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.pagesListView.HideSelection = false;
            this.pagesListView.Location = new System.Drawing.Point(0, 0);
            this.pagesListView.MultiSelect = false;
            this.pagesListView.Name = "pagesListView";
            this.pagesListView.OwnerDraw = true;
            this.pagesListView.Size = new System.Drawing.Size(200, 827);
            this.pagesListView.TabIndex = 50;
            this.toolTip1.SetToolTip(this.pagesListView, "Tooltip_PagesList");
            this.pagesListView.UseCompatibleStateImageBehavior = false;
            this.pagesListView.View = System.Windows.Forms.View.List;
            this.pagesListView.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.PagesListView_DrawItem);
            this.pagesListView.SelectedIndexChanged += new System.EventHandler(this.PagesListView_SelectedIndexChanged);
            //
            // thumbnailsListView
            //
            this.thumbnailsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.thumbnailsListView.HideSelection = false;
            this.thumbnailsListView.LargeImageList = this.thumbnailsImageList;
            this.thumbnailsListView.Location = new System.Drawing.Point(0, 0);
            this.thumbnailsListView.MultiSelect = false;
            this.thumbnailsListView.Name = "thumbnailsListView";
            this.thumbnailsListView.OwnerDraw = true;
            this.thumbnailsListView.Size = new System.Drawing.Size(200, 827);
            this.thumbnailsListView.TabIndex = 0;
            this.toolTip1.SetToolTip(this.thumbnailsListView, "Tooltip_PageThumbnails");
            this.thumbnailsListView.UseCompatibleStateImageBehavior = false;
            this.thumbnailsListView.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.ThumbnailsListView_DrawItem);
            this.thumbnailsListView.SelectedIndexChanged += new System.EventHandler(this.ThumbnailsListView_SelectedIndexChanged);
            //
            // thumbnailsImageList
            //
            this.thumbnailsImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.thumbnailsImageList.ImageSize = new System.Drawing.Size(150, 150);
            this.thumbnailsImageList.TransparentColor = System.Drawing.Color.Transparent;
            //
            // formSplitContainer
            //
            this.formSplitContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.formSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.formSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.formSplitContainer.Name = "formSplitContainer";
            //
            // formSplitContainer.Panel1
            //
            this.formSplitContainer.Panel1.Controls.Add(this.mainAppSplitContainer);
            //
            // formSplitContainer.Panel2
            //
            this.formSplitContainer.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.formSplitContainer.Size = new System.Drawing.Size(1236, 912);
            this.formSplitContainer.SplitterDistance = 1014;
            this.formSplitContainer.TabIndex = 47;
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.pagesTabControl, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBoxFilter, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(214, 908);
            this.tableLayoutPanel1.TabIndex = 48;
            //
            // pagesTabControl
            //
            this.pagesTabControl.Controls.Add(this.pagesListTabPage);
            this.pagesTabControl.Controls.Add(this.thumbnailsTabPage);
            this.pagesTabControl.Controls.Add(this.layersTabPage);
            this.pagesTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pagesTabControl.Location = new System.Drawing.Point(3, 52);
            this.pagesTabControl.Name = "pagesTabControl";
            this.pagesTabControl.SelectedIndex = 0;
            this.pagesTabControl.Size = new System.Drawing.Size(208, 853);
            this.pagesTabControl.TabIndex = 51;
            this.pagesTabControl.Visible = false;
            this.pagesTabControl.SelectedIndexChanged += new System.EventHandler(this.PagesTabControl_SelectedIndexChanged);
            //
            // pagesListTabPage
            //
            this.pagesListTabPage.Controls.Add(this.pagesListView);
            this.pagesListTabPage.Location = new System.Drawing.Point(4, 22);
            this.pagesListTabPage.Name = "pagesListTabPage";
            this.pagesListTabPage.Size = new System.Drawing.Size(200, 827);
            this.pagesListTabPage.TabIndex = 0;
            this.pagesListTabPage.Text = "UI_Tab_PagesList";
            this.pagesListTabPage.UseVisualStyleBackColor = true;
            //
            // thumbnailsTabPage
            //
            this.thumbnailsTabPage.Controls.Add(this.thumbnailsListView);
            this.thumbnailsTabPage.Location = new System.Drawing.Point(4, 22);
            this.thumbnailsTabPage.Name = "thumbnailsTabPage";
            this.thumbnailsTabPage.Size = new System.Drawing.Size(200, 827);
            this.thumbnailsTabPage.TabIndex = 1;
            this.thumbnailsTabPage.Text = "UI_Tab_Thumbnails";
            this.thumbnailsTabPage.UseVisualStyleBackColor = true;
            //
            // layersTabPage
            //
            this.layersTabPage.Controls.Add(this.layersTabPlaceholderPanel);
            this.layersTabPage.Location = new System.Drawing.Point(4, 22);
            this.layersTabPage.Name = "layersTabPage";
            this.layersTabPage.Size = new System.Drawing.Size(200, 827);
            this.layersTabPage.TabIndex = 2;
            this.layersTabPage.Text = "UI_Tab_Layers";
            this.layersTabPage.UseVisualStyleBackColor = true;
            //
            // layersTabPlaceholderPanel
            //
            this.layersTabPlaceholderPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.layersTabPlaceholderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layersTabPlaceholderPanel.Location = new System.Drawing.Point(0, 0);
            this.layersTabPlaceholderPanel.Name = "layersTabPlaceholderPanel";
            this.layersTabPlaceholderPanel.Size = new System.Drawing.Size(200, 827);
            this.layersTabPlaceholderPanel.TabIndex = 0;
            //
            // groupBoxOptions
            //
            this.groupBoxOptions.Controls.Add(this.setSavePassword);
            this.groupBoxOptions.Controls.Add(this.safeModeCheckBox);
            this.groupBoxOptions.Controls.Add(this.openSavedPDFCheckBox);
            this.groupBoxOptions.Controls.Add(this.outlineCheckBox);
            this.groupBoxOptions.Controls.Add(this.colorCheckBox);
            this.groupBoxOptions.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.groupBoxOptions.Location = new System.Drawing.Point(12, 794);
            this.groupBoxOptions.Name = "groupBoxOptions";
            this.groupBoxOptions.Size = new System.Drawing.Size(184, 134);
            this.groupBoxOptions.TabIndex = 8;
            this.groupBoxOptions.TabStop = false;
            this.groupBoxOptions.Text = "UI_Group_Options";
            //
            // setSavePassword
            //
            this.setSavePassword.AccentColor = System.Drawing.SystemColors.Highlight;
            this.setSavePassword.AutoSize = true;
            this.setSavePassword.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.setSavePassword.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.setSavePassword.Enabled = false;
            this.setSavePassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.setSavePassword.Location = new System.Drawing.Point(15, 111);
            this.setSavePassword.Name = "setSavePassword";
            this.setSavePassword.Size = new System.Drawing.Size(162, 17);
            this.setSavePassword.TabIndex = 4;
            this.setSavePassword.Text = "UI_Check_SetPassword";
            this.toolTip1.SetToolTip(this.setSavePassword, "Tooltip_SetPassword");
            this.setSavePassword.UseVisualStyleBackColor = true;
            //
            // safeModeCheckBox
            //
            this.safeModeCheckBox.AccentColor = System.Drawing.SystemColors.Highlight;
            this.safeModeCheckBox.AutoSize = true;
            this.safeModeCheckBox.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.safeModeCheckBox.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.safeModeCheckBox.Enabled = false;
            this.safeModeCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.safeModeCheckBox.Location = new System.Drawing.Point(15, 88);
            this.safeModeCheckBox.Name = "safeModeCheckBox";
            this.safeModeCheckBox.Size = new System.Drawing.Size(146, 17);
            this.safeModeCheckBox.TabIndex = 3;
            this.safeModeCheckBox.Text = "UI_Check_SafeMode";
            this.toolTip1.SetToolTip(this.safeModeCheckBox, "Tooltip_SafeMode");
            this.safeModeCheckBox.UseVisualStyleBackColor = true;
            //
            // openSavedPDFCheckBox
            //
            this.openSavedPDFCheckBox.AccentColor = System.Drawing.SystemColors.Highlight;
            this.openSavedPDFCheckBox.AutoSize = true;
            this.openSavedPDFCheckBox.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.openSavedPDFCheckBox.Checked = true;
            this.openSavedPDFCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.openSavedPDFCheckBox.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.openSavedPDFCheckBox.Enabled = false;
            this.openSavedPDFCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.openSavedPDFCheckBox.Location = new System.Drawing.Point(15, 65);
            this.openSavedPDFCheckBox.Name = "openSavedPDFCheckBox";
            this.openSavedPDFCheckBox.Size = new System.Drawing.Size(190, 17);
            this.openSavedPDFCheckBox.TabIndex = 2;
            this.openSavedPDFCheckBox.Text = "UI_Check_PreviewAfterSave";
            this.toolTip1.SetToolTip(this.openSavedPDFCheckBox, "Tooltip_PreviewAfterSave");
            this.openSavedPDFCheckBox.UseVisualStyleBackColor = true;
            //
            // outlineCheckBox
            //
            this.outlineCheckBox.AccentColor = System.Drawing.SystemColors.Highlight;
            this.outlineCheckBox.AutoSize = true;
            this.outlineCheckBox.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.outlineCheckBox.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.outlineCheckBox.Enabled = false;
            this.outlineCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.outlineCheckBox.Location = new System.Drawing.Point(14, 42);
            this.outlineCheckBox.Name = "outlineCheckBox";
            this.outlineCheckBox.Size = new System.Drawing.Size(160, 17);
            this.outlineCheckBox.TabIndex = 1;
            this.outlineCheckBox.Text = "UI_Check_ShowOutline";
            this.toolTip1.SetToolTip(this.outlineCheckBox, "Tooltip_ShowOutline");
            this.outlineCheckBox.UseVisualStyleBackColor = true;
            //
            // colorCheckBox
            //
            this.colorCheckBox.AccentColor = System.Drawing.SystemColors.Highlight;
            this.colorCheckBox.AutoSize = true;
            this.colorCheckBox.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.colorCheckBox.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.colorCheckBox.Enabled = false;
            this.colorCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.colorCheckBox.Location = new System.Drawing.Point(14, 19);
            this.colorCheckBox.Name = "colorCheckBox";
            this.colorCheckBox.Size = new System.Drawing.Size(168, 17);
            this.colorCheckBox.TabIndex = 0;
            this.colorCheckBox.Text = "UI_Check_HighlightColor";
            this.toolTip1.SetToolTip(this.colorCheckBox, "Tooltip_HighlightColor");
            this.colorCheckBox.UseVisualStyleBackColor = true;
            //
            // groupBoxPagesToRemove
            //
            this.groupBoxPagesToRemove.Controls.Add(this.removePageButton);
            this.groupBoxPagesToRemove.Controls.Add(this.removePageRangeButton);
            this.groupBoxPagesToRemove.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.groupBoxPagesToRemove.Enabled = false;
            this.groupBoxPagesToRemove.Location = new System.Drawing.Point(12, 594);
            this.groupBoxPagesToRemove.Name = "groupBoxPagesToRemove";
            this.groupBoxPagesToRemove.Size = new System.Drawing.Size(184, 85);
            this.groupBoxPagesToRemove.TabIndex = 6;
            this.groupBoxPagesToRemove.TabStop = false;
            this.groupBoxPagesToRemove.Text = "UI_Group_PagesToRemove";
            //
            // removePageButton
            //
            this.removePageButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.removePageButton.BackColor = System.Drawing.SystemColors.Control;
            this.removePageButton.Enabled = false;
            this.removePageButton.Font = new System.Drawing.Font("Segoe MDL2 Assets", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.removePageButton.Location = new System.Drawing.Point(96, 22);
            this.removePageButton.Name = "removePageButton";
            this.removePageButton.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.removePageButton.Size = new System.Drawing.Size(56, 51);
            this.removePageButton.TabIndex = 35;
            this.toolTip1.SetToolTip(this.removePageButton, "Tooltip_RemovePage");
            this.removePageButton.UseVisualStyleBackColor = true;
            //
            // removePageRangeButton
            //
            this.removePageRangeButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.removePageRangeButton.Enabled = false;
            this.removePageRangeButton.Font = new System.Drawing.Font("Segoe MDL2 Assets", 12F);
            this.removePageRangeButton.Location = new System.Drawing.Point(32, 22);
            this.removePageRangeButton.Name = "removePageRangeButton";
            this.removePageRangeButton.Size = new System.Drawing.Size(56, 51);
            this.removePageRangeButton.TabIndex = 34;
            this.toolTip1.SetToolTip(this.removePageRangeButton, "Tooltip_RemovePageRange");
            this.removePageRangeButton.UseVisualStyleBackColor = true;
            //
            // groupBoxSignatures
            //
            this.groupBoxSignatures.Controls.Add(this.signaturesReportRadioButton);
            this.groupBoxSignatures.Controls.Add(this.signaturesOriginalRadioButton);
            this.groupBoxSignatures.Controls.Add(this.signaturesRemoveRadioButton);
            this.groupBoxSignatures.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.groupBoxSignatures.Enabled = false;
            this.groupBoxSignatures.Location = new System.Drawing.Point(12, 687);
            this.groupBoxSignatures.Name = "groupBoxSignatures";
            this.groupBoxSignatures.Size = new System.Drawing.Size(184, 99);
            this.groupBoxSignatures.TabIndex = 7;
            this.groupBoxSignatures.TabStop = false;
            this.groupBoxSignatures.Text = "UI_Group_Signatures";
            //
            // signaturesReportRadioButton
            //
            this.signaturesReportRadioButton.AccentColor = System.Drawing.SystemColors.Highlight;
            this.signaturesReportRadioButton.AutoSize = true;
            this.signaturesReportRadioButton.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.signaturesReportRadioButton.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.signaturesReportRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.signaturesReportRadioButton.Location = new System.Drawing.Point(15, 65);
            this.signaturesReportRadioButton.Name = "signaturesReportRadioButton";
            this.signaturesReportRadioButton.Size = new System.Drawing.Size(190, 17);
            this.signaturesReportRadioButton.TabIndex = 39;
            this.signaturesReportRadioButton.TabStop = true;
            this.signaturesReportRadioButton.Text = "UI_Radio_Signatures_Report";
            this.toolTip1.SetToolTip(this.signaturesReportRadioButton, "Tooltip_Signatures_Report");
            this.signaturesReportRadioButton.UseVisualStyleBackColor = true;
            //
            // signaturesOriginalRadioButton
            //
            this.signaturesOriginalRadioButton.AccentColor = System.Drawing.SystemColors.Highlight;
            this.signaturesOriginalRadioButton.AutoSize = true;
            this.signaturesOriginalRadioButton.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.signaturesOriginalRadioButton.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.signaturesOriginalRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.signaturesOriginalRadioButton.Location = new System.Drawing.Point(15, 42);
            this.signaturesOriginalRadioButton.Name = "signaturesOriginalRadioButton";
            this.signaturesOriginalRadioButton.Size = new System.Drawing.Size(195, 17);
            this.signaturesOriginalRadioButton.TabIndex = 38;
            this.signaturesOriginalRadioButton.TabStop = true;
            this.signaturesOriginalRadioButton.Text = "UI_Radio_Signatures_Original";
            this.toolTip1.SetToolTip(this.signaturesOriginalRadioButton, "Tooltip_Signatures_Original");
            this.signaturesOriginalRadioButton.UseVisualStyleBackColor = true;
            //
            // signaturesRemoveRadioButton
            //
            this.signaturesRemoveRadioButton.AccentColor = System.Drawing.SystemColors.Highlight;
            this.signaturesRemoveRadioButton.AutoSize = true;
            this.signaturesRemoveRadioButton.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.signaturesRemoveRadioButton.Checked = true;
            this.signaturesRemoveRadioButton.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.signaturesRemoveRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.signaturesRemoveRadioButton.Location = new System.Drawing.Point(15, 19);
            this.signaturesRemoveRadioButton.Name = "signaturesRemoveRadioButton";
            this.signaturesRemoveRadioButton.Size = new System.Drawing.Size(198, 17);
            this.signaturesRemoveRadioButton.TabIndex = 37;
            this.signaturesRemoveRadioButton.TabStop = true;
            this.signaturesRemoveRadioButton.Text = "UI_Radio_Signatures_Remove";
            this.toolTip1.SetToolTip(this.signaturesRemoveRadioButton, "Tooltip_Signatures_Remove");
            this.signaturesRemoveRadioButton.UseVisualStyleBackColor = true;
            //
            // groupBoxPages
            //
            this.groupBoxPages.BackColor = System.Drawing.SystemColors.Control;
            this.groupBoxPages.Controls.Add(this.pageNumberTextBox);
            this.groupBoxPages.Controls.Add(this.numPagesLabel);
            this.groupBoxPages.Controls.Add(this.buttonFirst);
            this.groupBoxPages.Controls.Add(this.buttonNextPage);
            this.groupBoxPages.Controls.Add(this.buttonPrevious);
            this.groupBoxPages.Controls.Add(this.zoomMinButton);
            this.groupBoxPages.Controls.Add(this.zoomMaxButton);
            this.groupBoxPages.Controls.Add(this.zoomOutButton);
            this.groupBoxPages.Controls.Add(this.zoomInButton);
            this.groupBoxPages.Controls.Add(this.buttonLast);
            this.groupBoxPages.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.groupBoxPages.Enabled = false;
            this.groupBoxPages.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBoxPages.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.groupBoxPages.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBoxPages.Location = new System.Drawing.Point(12, 196);
            this.groupBoxPages.Name = "groupBoxPages";
            this.groupBoxPages.Size = new System.Drawing.Size(184, 110);
            this.groupBoxPages.TabIndex = 3;
            this.groupBoxPages.TabStop = false;
            this.groupBoxPages.Text = "UI_Group_Pages";
            //
            // pageNumberTextBox
            //
            this.pageNumberTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.pageNumberTextBox.Location = new System.Drawing.Point(15, 19);
            this.pageNumberTextBox.Name = "pageNumberTextBox";
            this.pageNumberTextBox.Size = new System.Drawing.Size(70, 21);
            this.pageNumberTextBox.TabIndex = 4;
            this.pageNumberTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip1.SetToolTip(this.pageNumberTextBox, "Tooltip_PageNumber");
            this.pageNumberTextBox.Visible = false;
            this.pageNumberTextBox.Click += new System.EventHandler(this.PageNumberTextBox_Click);
            this.pageNumberTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PageNumberTextBox_KeyDown);
            this.pageNumberTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.PageNumberTextBox_KeyPress);
            //
            // numPagesLabel
            //
            this.numPagesLabel.AutoSize = true;
            this.numPagesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.numPagesLabel.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.numPagesLabel.Location = new System.Drawing.Point(93, 22);
            this.numPagesLabel.Name = "numPagesLabel";
            this.numPagesLabel.Size = new System.Drawing.Size(10, 15);
            this.numPagesLabel.TabIndex = 0;
            this.numPagesLabel.Text = " ";
            this.numPagesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.numPagesLabel.Visible = false;
            //
            // buttonFirst
            //
            this.buttonFirst.Enabled = false;
            this.buttonFirst.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonFirst.Location = new System.Drawing.Point(15, 47);
            this.buttonFirst.Name = "buttonFirst";
            this.buttonFirst.Size = new System.Drawing.Size(34, 23);
            this.buttonFirst.TabIndex = 5;
            this.buttonFirst.Text = "|<";
            this.toolTip1.SetToolTip(this.buttonFirst, "Tooltip_FirstPage");
            this.buttonFirst.UseVisualStyleBackColor = true;
            this.buttonFirst.Click += new System.EventHandler(this.ButtonFirst_Click);
            //
            // buttonNextPage
            //
            this.buttonNextPage.Enabled = false;
            this.buttonNextPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonNextPage.Location = new System.Drawing.Point(95, 47);
            this.buttonNextPage.Name = "buttonNextPage";
            this.buttonNextPage.Size = new System.Drawing.Size(34, 23);
            this.buttonNextPage.TabIndex = 7;
            this.buttonNextPage.Text = ">";
            this.toolTip1.SetToolTip(this.buttonNextPage, "Tooltip_NextPage");
            this.buttonNextPage.UseVisualStyleBackColor = true;
            this.buttonNextPage.Click += new System.EventHandler(this.ButtonNextPage_Click);
            //
            // buttonPrevious
            //
            this.buttonPrevious.Enabled = false;
            this.buttonPrevious.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonPrevious.Location = new System.Drawing.Point(55, 47);
            this.buttonPrevious.Name = "buttonPrevious";
            this.buttonPrevious.Size = new System.Drawing.Size(34, 23);
            this.buttonPrevious.TabIndex = 6;
            this.buttonPrevious.Text = "<";
            this.toolTip1.SetToolTip(this.buttonPrevious, "Tooltip_PrevPage");
            this.buttonPrevious.UseVisualStyleBackColor = true;
            this.buttonPrevious.Click += new System.EventHandler(this.ButtonPrevious_Click);
            //
            // zoomMinButton
            //
            this.zoomMinButton.Enabled = false;
            this.zoomMinButton.Font = new System.Drawing.Font("Segoe MDL2 Assets", 8F, System.Drawing.FontStyle.Bold);
            this.zoomMinButton.Location = new System.Drawing.Point(15, 76);
            this.zoomMinButton.Name = "zoomMinButton";
            this.zoomMinButton.Size = new System.Drawing.Size(34, 27);
            this.zoomMinButton.TabIndex = 9;
            this.toolTip1.SetToolTip(this.zoomMinButton, "Tooltip_ZoomMin");
            this.zoomMinButton.UseVisualStyleBackColor = true;
            this.zoomMinButton.Click += new System.EventHandler(this.ZoomMinButton_Click);
            //
            // zoomMaxButton
            //
            this.zoomMaxButton.Enabled = false;
            this.zoomMaxButton.Font = new System.Drawing.Font("Segoe MDL2 Assets", 12F, System.Drawing.FontStyle.Bold);
            this.zoomMaxButton.Location = new System.Drawing.Point(135, 76);
            this.zoomMaxButton.Name = "zoomMaxButton";
            this.zoomMaxButton.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.zoomMaxButton.Size = new System.Drawing.Size(34, 27);
            this.zoomMaxButton.TabIndex = 12;
            this.toolTip1.SetToolTip(this.zoomMaxButton, "Tooltip_ZoomMax");
            this.zoomMaxButton.UseVisualStyleBackColor = true;
            this.zoomMaxButton.Click += new System.EventHandler(this.ZoomMaxButton_Click);
            //
            // zoomOutButton
            //
            this.zoomOutButton.Enabled = false;
            this.zoomOutButton.Font = new System.Drawing.Font("Segoe MDL2 Assets", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.zoomOutButton.Location = new System.Drawing.Point(55, 76);
            this.zoomOutButton.Name = "zoomOutButton";
            this.zoomOutButton.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.zoomOutButton.Size = new System.Drawing.Size(34, 27);
            this.zoomOutButton.TabIndex = 10;
            this.toolTip1.SetToolTip(this.zoomOutButton, "Tooltip_ZoomOut");
            this.zoomOutButton.UseVisualStyleBackColor = true;
            this.zoomOutButton.Click += new System.EventHandler(this.ZoomOutButton_Click);
            //
            // zoomInButton
            //
            this.zoomInButton.Enabled = false;
            this.zoomInButton.Font = new System.Drawing.Font("Segoe MDL2 Assets", 12F, System.Drawing.FontStyle.Bold);
            this.zoomInButton.Location = new System.Drawing.Point(95, 76);
            this.zoomInButton.Name = "zoomInButton";
            this.zoomInButton.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.zoomInButton.Size = new System.Drawing.Size(34, 27);
            this.zoomInButton.TabIndex = 11;
            this.toolTip1.SetToolTip(this.zoomInButton, "Tooltip_ZoomIn");
            this.zoomInButton.UseVisualStyleBackColor = true;
            this.zoomInButton.Click += new System.EventHandler(this.ZoomInButton_Click);
            //
            // buttonLast
            //
            this.buttonLast.Enabled = false;
            this.buttonLast.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonLast.Location = new System.Drawing.Point(135, 47);
            this.buttonLast.Name = "buttonLast";
            this.buttonLast.Size = new System.Drawing.Size(34, 23);
            this.buttonLast.TabIndex = 8;
            this.buttonLast.Text = ">|";
            this.toolTip1.SetToolTip(this.buttonLast, "Tooltip_LastPage");
            this.buttonLast.UseVisualStyleBackColor = true;
            this.buttonLast.Click += new System.EventHandler(this.ButtonLast_Click);
            //
            // groupBoxSearch
            //
            this.groupBoxSearch.Controls.Add(this.searchButton);
            this.groupBoxSearch.Controls.Add(this.searchToSelectionButton);
            this.groupBoxSearch.Controls.Add(this.searchResultLabel);
            this.groupBoxSearch.Controls.Add(this.SearchClearButton);
            this.groupBoxSearch.Controls.Add(this.personalDataButton);
            this.groupBoxSearch.Controls.Add(this.searchLastButton);
            this.groupBoxSearch.Controls.Add(this.searchNextButton);
            this.groupBoxSearch.Controls.Add(this.searchPrevButton);
            this.groupBoxSearch.Controls.Add(this.searchFirstButton);
            this.groupBoxSearch.Controls.Add(this.searchTextBox);
            this.groupBoxSearch.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.groupBoxSearch.Enabled = false;
            this.groupBoxSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.groupBoxSearch.Location = new System.Drawing.Point(12, 314);
            this.groupBoxSearch.Name = "groupBoxSearch";
            this.groupBoxSearch.Size = new System.Drawing.Size(184, 127);
            this.groupBoxSearch.TabIndex = 4;
            this.groupBoxSearch.TabStop = false;
            this.groupBoxSearch.Text = "UI_Group_Search";
            //
            // searchButton
            //
            this.searchButton.Font = new System.Drawing.Font("Segoe MDL2 Assets", 12F, System.Drawing.FontStyle.Bold);
            this.searchButton.Location = new System.Drawing.Point(141, 18);
            this.searchButton.Name = "searchButton";
            this.searchButton.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.searchButton.Size = new System.Drawing.Size(28, 23);
            this.searchButton.TabIndex = 15;
            this.toolTip1.SetToolTip(this.searchButton, "Tooltip_Search");
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.SearchButton_Click);
            //
            // searchToSelectionButton
            //
            this.searchToSelectionButton.Font = new System.Drawing.Font("Segoe MDL2 Assets", 12F, System.Drawing.FontStyle.Bold);
            this.searchToSelectionButton.Location = new System.Drawing.Point(141, 71);
            this.searchToSelectionButton.Name = "searchToSelectionButton";
            this.searchToSelectionButton.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.searchToSelectionButton.Size = new System.Drawing.Size(28, 23);
            this.searchToSelectionButton.TabIndex = 19;
            this.toolTip1.SetToolTip(this.searchToSelectionButton, "Tooltip_SearchToSelection");
            this.searchToSelectionButton.UseVisualStyleBackColor = true;
            this.searchToSelectionButton.Click += new System.EventHandler(this.SearchToSelectionButton_Click);
            //
            // searchResultLabel
            //
            this.searchResultLabel.AutoSize = true;
            this.searchResultLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.searchResultLabel.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.searchResultLabel.Location = new System.Drawing.Point(15, 76);
            this.searchResultLabel.Name = "searchResultLabel";
            this.searchResultLabel.Size = new System.Drawing.Size(16, 13);
            this.searchResultLabel.TabIndex = 18;
            this.searchResultLabel.Text = "   ";
            this.searchResultLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // SearchClearButton
            //
            this.SearchClearButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.SearchClearButton.Location = new System.Drawing.Point(141, 45);
            this.SearchClearButton.Name = "SearchClearButton";
            this.SearchClearButton.Size = new System.Drawing.Size(28, 23);
            this.SearchClearButton.TabIndex = 17;
            this.SearchClearButton.Text = "UI_SearchClearGlyph";
            this.toolTip1.SetToolTip(this.SearchClearButton, "Tooltip_SearchClear");
            this.SearchClearButton.UseVisualStyleBackColor = true;
            this.SearchClearButton.Click += new System.EventHandler(this.SearchClearButton_Click);
            //
            // personalDataButton
            //
            this.personalDataButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.personalDataButton.Location = new System.Drawing.Point(15, 45);
            this.personalDataButton.Name = "personalDataButton";
            this.personalDataButton.Size = new System.Drawing.Size(120, 23);
            this.personalDataButton.TabIndex = 16;
            this.personalDataButton.Text = "UI_Button_PersonalData";
            this.toolTip1.SetToolTip(this.personalDataButton, "Tooltip_PersonalData");
            this.personalDataButton.UseVisualStyleBackColor = true;
            this.personalDataButton.Click += new System.EventHandler(this.PersonalDataButton_Click);
            //
            // searchLastButton
            //
            this.searchLastButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.searchLastButton.Location = new System.Drawing.Point(135, 97);
            this.searchLastButton.Name = "searchLastButton";
            this.searchLastButton.Size = new System.Drawing.Size(34, 23);
            this.searchLastButton.TabIndex = 23;
            this.searchLastButton.Text = ">|";
            this.toolTip1.SetToolTip(this.searchLastButton, "Tooltip_SearchResultLast");
            this.searchLastButton.UseVisualStyleBackColor = true;
            this.searchLastButton.Click += new System.EventHandler(this.SearchLastButton_Click);
            //
            // searchNextButton
            //
            this.searchNextButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.searchNextButton.Location = new System.Drawing.Point(95, 97);
            this.searchNextButton.Name = "searchNextButton";
            this.searchNextButton.Size = new System.Drawing.Size(34, 23);
            this.searchNextButton.TabIndex = 22;
            this.searchNextButton.Text = ">";
            this.toolTip1.SetToolTip(this.searchNextButton, "Tooltip_SearchResultNext");
            this.searchNextButton.UseVisualStyleBackColor = true;
            this.searchNextButton.Click += new System.EventHandler(this.SearchNextButton_Click);
            //
            // searchPrevButton
            //
            this.searchPrevButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.searchPrevButton.Location = new System.Drawing.Point(55, 97);
            this.searchPrevButton.Name = "searchPrevButton";
            this.searchPrevButton.Size = new System.Drawing.Size(34, 23);
            this.searchPrevButton.TabIndex = 21;
            this.searchPrevButton.Text = "<";
            this.toolTip1.SetToolTip(this.searchPrevButton, "Tooltip_SearchResultPrev");
            this.searchPrevButton.UseVisualStyleBackColor = true;
            this.searchPrevButton.Click += new System.EventHandler(this.SearchPrevButton_Click);
            //
            // searchFirstButton
            //
            this.searchFirstButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.searchFirstButton.Location = new System.Drawing.Point(15, 97);
            this.searchFirstButton.Name = "searchFirstButton";
            this.searchFirstButton.Size = new System.Drawing.Size(34, 23);
            this.searchFirstButton.TabIndex = 20;
            this.searchFirstButton.Text = "|<";
            this.toolTip1.SetToolTip(this.searchFirstButton, "Tooltip_SearchResultFirst");
            this.searchFirstButton.UseVisualStyleBackColor = true;
            this.searchFirstButton.Click += new System.EventHandler(this.SearchFirstButton_Click);
            //
            // searchTextBox
            //
            this.searchTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.searchTextBox.Location = new System.Drawing.Point(15, 18);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(120, 21);
            this.searchTextBox.TabIndex = 14;
            this.toolTip1.SetToolTip(this.searchTextBox, "Tooltip_SearchInput");
            this.searchTextBox.Click += new System.EventHandler(this.SearchTextBox_Click);
            this.searchTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SearchTextBox_KeyDown);
            //
            // groupBoxOpen
            //
            this.groupBoxOpen.Controls.Add(this.openProjectButton);
            this.groupBoxOpen.Controls.Add(this.loadPdfButton);
            this.groupBoxOpen.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.groupBoxOpen.Location = new System.Drawing.Point(12, 32);
            this.groupBoxOpen.Name = "groupBoxOpen";
            this.groupBoxOpen.Size = new System.Drawing.Size(184, 74);
            this.groupBoxOpen.TabIndex = 1;
            this.groupBoxOpen.TabStop = false;
            this.groupBoxOpen.Text = "UI_Group_Open";
            //
            // openProjectButton
            //
            this.openProjectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.openProjectButton.Location = new System.Drawing.Point(15, 43);
            this.openProjectButton.Name = "openProjectButton";
            this.openProjectButton.Size = new System.Drawing.Size(154, 23);
            this.openProjectButton.TabIndex = 1;
            this.openProjectButton.Text = "UI_Button_OpenProject";
            this.toolTip1.SetToolTip(this.openProjectButton, "Tooltip_OpenProject");
            this.openProjectButton.UseVisualStyleBackColor = true;
            this.openProjectButton.Click += new System.EventHandler(this.OpenProjectButton_Click);
            //
            // loadPdfButton
            //
            this.loadPdfButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.loadPdfButton.Location = new System.Drawing.Point(15, 18);
            this.loadPdfButton.Name = "loadPdfButton";
            this.loadPdfButton.Size = new System.Drawing.Size(154, 23);
            this.loadPdfButton.TabIndex = 0;
            this.loadPdfButton.Text = "UI_Button_OpenPdf";
            this.toolTip1.SetToolTip(this.loadPdfButton, "Tooltip_LoadPdf");
            this.loadPdfButton.UseVisualStyleBackColor = true;
            this.loadPdfButton.Click += new System.EventHandler(this.LoadPdfButton_Click);
            //
            // groupBoxSave
            //
            this.groupBoxSave.Controls.Add(this.buttonRedactText);
            this.groupBoxSave.Controls.Add(this.saveProjectButton);
            this.groupBoxSave.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.groupBoxSave.Enabled = false;
            this.groupBoxSave.Location = new System.Drawing.Point(12, 114);
            this.groupBoxSave.Name = "groupBoxSave";
            this.groupBoxSave.Size = new System.Drawing.Size(184, 74);
            this.groupBoxSave.TabIndex = 2;
            this.groupBoxSave.TabStop = false;
            this.groupBoxSave.Text = "UI_Group_Save";
            //
            // buttonRedactText
            //
            this.buttonRedactText.Enabled = false;
            this.buttonRedactText.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.buttonRedactText.Location = new System.Drawing.Point(15, 43);
            this.buttonRedactText.Name = "buttonRedactText";
            this.buttonRedactText.Size = new System.Drawing.Size(154, 23);
            this.buttonRedactText.TabIndex = 1;
            this.buttonRedactText.Text = "UI_Button_SavePdf";
            this.toolTip1.SetToolTip(this.buttonRedactText, "Tooltip_SavePdf");
            this.buttonRedactText.UseVisualStyleBackColor = true;
            this.buttonRedactText.Click += new System.EventHandler(this.ButtonRedactText_Click);
            //
            // saveProjectButton
            //
            this.saveProjectButton.Enabled = false;
            this.saveProjectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.saveProjectButton.Location = new System.Drawing.Point(15, 18);
            this.saveProjectButton.Name = "saveProjectButton";
            this.saveProjectButton.Size = new System.Drawing.Size(154, 23);
            this.saveProjectButton.TabIndex = 0;
            this.saveProjectButton.Text = "UI_Button_SaveProject";
            this.toolTip1.SetToolTip(this.saveProjectButton, "Tooltip_SaveProject");
            this.saveProjectButton.UseVisualStyleBackColor = true;
            this.saveProjectButton.Click += new System.EventHandler(this.SaveProjectButton_Click);
            //
            // groupBoxSelections
            //
            this.groupBoxSelections.Controls.Add(this.selectionLastButton);
            this.groupBoxSelections.Controls.Add(this.selectionNextButton);
            this.groupBoxSelections.Controls.Add(this.selectionPrevButton);
            this.groupBoxSelections.Controls.Add(this.selectionFirstButton);
            this.groupBoxSelections.Controls.Add(this.cursorRadioButton);
            this.groupBoxSelections.Controls.Add(this.markerRadioButton);
            this.groupBoxSelections.Controls.Add(this.boxRadioButton);
            this.groupBoxSelections.Controls.Add(this.clearPageButton);
            this.groupBoxSelections.Controls.Add(this.clearSelectionButton);
            this.groupBoxSelections.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.groupBoxSelections.Enabled = false;
            this.groupBoxSelections.Location = new System.Drawing.Point(12, 449);
            this.groupBoxSelections.Name = "groupBoxSelections";
            this.groupBoxSelections.Size = new System.Drawing.Size(184, 137);
            this.groupBoxSelections.TabIndex = 5;
            this.groupBoxSelections.TabStop = false;
            this.groupBoxSelections.Text = "UI_Group_Selections";
            //
            // selectionLastButton
            //
            this.selectionLastButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.selectionLastButton.Location = new System.Drawing.Point(135, 44);
            this.selectionLastButton.Name = "selectionLastButton";
            this.selectionLastButton.Size = new System.Drawing.Size(34, 23);
            this.selectionLastButton.TabIndex = 30;
            this.selectionLastButton.Text = ">|";
            this.toolTip1.SetToolTip(this.selectionLastButton, "Tooltip_SelectionLast");
            this.selectionLastButton.UseVisualStyleBackColor = true;
            this.selectionLastButton.Click += new System.EventHandler(this.SelectionLastButton_Click);
            //
            // selectionNextButton
            //
            this.selectionNextButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.selectionNextButton.Location = new System.Drawing.Point(95, 44);
            this.selectionNextButton.Name = "selectionNextButton";
            this.selectionNextButton.Size = new System.Drawing.Size(34, 23);
            this.selectionNextButton.TabIndex = 29;
            this.selectionNextButton.Text = ">";
            this.toolTip1.SetToolTip(this.selectionNextButton, "Tooltip_SelectionNext");
            this.selectionNextButton.UseVisualStyleBackColor = true;
            this.selectionNextButton.Click += new System.EventHandler(this.SelectionNextButton_Click);
            //
            // selectionPrevButton
            //
            this.selectionPrevButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.selectionPrevButton.Location = new System.Drawing.Point(55, 44);
            this.selectionPrevButton.Name = "selectionPrevButton";
            this.selectionPrevButton.Size = new System.Drawing.Size(34, 23);
            this.selectionPrevButton.TabIndex = 28;
            this.selectionPrevButton.Text = "<";
            this.toolTip1.SetToolTip(this.selectionPrevButton, "Tooltip_SelectionPrev");
            this.selectionPrevButton.UseVisualStyleBackColor = true;
            this.selectionPrevButton.Click += new System.EventHandler(this.SelectionPrevButton_Click);
            //
            // selectionFirstButton
            //
            this.selectionFirstButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.selectionFirstButton.Location = new System.Drawing.Point(15, 44);
            this.selectionFirstButton.Name = "selectionFirstButton";
            this.selectionFirstButton.Size = new System.Drawing.Size(34, 23);
            this.selectionFirstButton.TabIndex = 27;
            this.selectionFirstButton.Text = "|<";
            this.toolTip1.SetToolTip(this.selectionFirstButton, "Tooltip_SelectionFirst");
            this.selectionFirstButton.UseVisualStyleBackColor = true;
            this.selectionFirstButton.Click += new System.EventHandler(this.SelectionFirstButton_Click);
            //
            // cursorRadioButton
            //
            this.cursorRadioButton.AccentColor = System.Drawing.SystemColors.Highlight;
            this.cursorRadioButton.AutoSize = true;
            this.cursorRadioButton.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.cursorRadioButton.Checked = false;
            this.cursorRadioButton.Enabled = false;
            this.cursorRadioButton.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.cursorRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.cursorRadioButton.Location = new System.Drawing.Point(10, 19);
            this.cursorRadioButton.Name = "cursorRadioButton";
            this.cursorRadioButton.Size = new System.Drawing.Size(121, 17);
            this.cursorRadioButton.TabIndex = 24;
            this.cursorRadioButton.TabStop = true;
            this.cursorRadioButton.Text = "UI_Radio_Cursor";
            this.toolTip1.SetToolTip(this.cursorRadioButton, "Tooltip_Cursor");
            this.cursorRadioButton.UseVisualStyleBackColor = true;
            //
            // markerRadioButton
            //
            this.markerRadioButton.AccentColor = System.Drawing.SystemColors.Highlight;
            this.markerRadioButton.AutoSize = true;
            this.markerRadioButton.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.markerRadioButton.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.markerRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.markerRadioButton.Checked = true;
            this.markerRadioButton.Location = new System.Drawing.Point(72, 19);
            this.markerRadioButton.Name = "markerRadioButton";
            this.markerRadioButton.Size = new System.Drawing.Size(124, 17);
            this.markerRadioButton.TabIndex = 25;
            this.markerRadioButton.TabStop = true;
            this.markerRadioButton.Text = "UI_Radio_Marker";
            this.toolTip1.SetToolTip(this.markerRadioButton, "Tooltip_Marker");
            this.markerRadioButton.UseVisualStyleBackColor = true;
            //
            // boxRadioButton
            //
            this.boxRadioButton.AccentColor = System.Drawing.SystemColors.Highlight;
            this.boxRadioButton.AutoSize = true;
            this.boxRadioButton.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.boxRadioButton.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.boxRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.boxRadioButton.Location = new System.Drawing.Point(137, 19);
            this.boxRadioButton.Name = "boxRadioButton";
            this.boxRadioButton.Size = new System.Drawing.Size(106, 17);
            this.boxRadioButton.TabIndex = 26;
            this.boxRadioButton.TabStop = true;
            this.boxRadioButton.Text = "UI_Radio_Box";
            this.toolTip1.SetToolTip(this.boxRadioButton, "Tooltip_Box");
            this.boxRadioButton.UseVisualStyleBackColor = true;
            //
            // clearPageButton
            //
            this.clearPageButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.clearPageButton.Location = new System.Drawing.Point(15, 76);
            this.clearPageButton.Name = "clearPageButton";
            this.clearPageButton.Size = new System.Drawing.Size(154, 23);
            this.clearPageButton.TabIndex = 31;
            this.clearPageButton.Text = "UI_ClearPage";
            this.toolTip1.SetToolTip(this.clearPageButton, "Tooltip_ClearPage");
            this.clearPageButton.UseVisualStyleBackColor = true;
            this.clearPageButton.Click += new System.EventHandler(this.ClearPageButton_Click);
            //
            // clearSelectionButton
            //
            this.clearSelectionButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.clearSelectionButton.Location = new System.Drawing.Point(15, 105);
            this.clearSelectionButton.Name = "clearSelectionButton";
            this.clearSelectionButton.Size = new System.Drawing.Size(154, 23);
            this.clearSelectionButton.TabIndex = 32;
            this.clearSelectionButton.Text = "UI_ClearAll";
            this.toolTip1.SetToolTip(this.clearSelectionButton, "Tooltip_ClearAll");
            this.clearSelectionButton.UseVisualStyleBackColor = true;
            this.clearSelectionButton.Click += new System.EventHandler(this.ClearSelectionButton_Click);
            //
            // groupBoxFilter
            //
            this.groupBoxFilter.Controls.Add(this.filterComboBox);
            this.groupBoxFilter.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.groupBoxFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxFilter.Location = new System.Drawing.Point(3, 3);
            this.groupBoxFilter.Name = "groupBoxFilter";
            this.groupBoxFilter.Size = new System.Drawing.Size(208, 43);
            this.groupBoxFilter.TabIndex = 48;
            this.groupBoxFilter.TabStop = false;
            this.groupBoxFilter.Text = "UI_Group_Filter";
            this.groupBoxFilter.Visible = false;
            //
            // filterComboBox
            //
            this.filterComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filterComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.filterComboBox.FormattingEnabled = true;
            this.filterComboBox.Items.AddRange(new object[] {
            "UI_Filter_AllPages",
            "UI_Filter_AllCategories",
            "UI_Filter_Selections",
            "UI_Filter_Searches",
            "UI_Filter_Deletions",
            "UI_Filter_Annotations"});
            this.filterComboBox.Location = new System.Drawing.Point(3, 16);
            this.filterComboBox.Name = "filterComboBox";
            this.filterComboBox.Size = new System.Drawing.Size(202, 21);
            this.filterComboBox.TabIndex = 49;
            //
            // PDFForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1236, 912);
            this.Controls.Add(this.formSplitContainer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PDFForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.PDFForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pdfViewer)).EndInit();
            this.mainAppSplitContainer.Panel1.ResumeLayout(false);
            this.mainAppSplitContainer.Panel1.PerformLayout();
            this.mainAppSplitContainer.Panel2.ResumeLayout(false);
            this.mainAppSplitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainAppSplitContainer)).EndInit();
            this.mainAppSplitContainer.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.formSplitContainer.Panel1.ResumeLayout(false);
            this.formSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.formSplitContainer)).EndInit();
            this.formSplitContainer.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pagesTabControl.ResumeLayout(false);
            this.pagesListTabPage.ResumeLayout(false);
            this.thumbnailsTabPage.ResumeLayout(false);
            this.layersTabPage.ResumeLayout(false);
            this.groupBoxOptions.ResumeLayout(false);
            this.groupBoxOptions.PerformLayout();
            this.groupBoxPagesToRemove.ResumeLayout(false);
            this.groupBoxSignatures.ResumeLayout(false);
            this.groupBoxSignatures.PerformLayout();
            this.groupBoxPages.ResumeLayout(false);
            this.groupBoxPages.PerformLayout();
            this.groupBoxSearch.ResumeLayout(false);
            this.groupBoxSearch.PerformLayout();
            this.groupBoxOpen.ResumeLayout(false);
            this.groupBoxSave.ResumeLayout(false);
            this.groupBoxSelections.ResumeLayout(false);
            this.groupBoxSelections.PerformLayout();
            this.groupBoxFilter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button loadPdfButton;
        private System.Windows.Forms.PictureBox pdfViewer;
        private System.Windows.Forms.SplitContainer mainAppSplitContainer;
        private System.Windows.Forms.Button buttonNextPage;
        private System.Windows.Forms.Button buttonPrevious;
        private System.Windows.Forms.Button buttonRedactText;
        private System.Windows.Forms.Button clearSelectionButton;
        private System.Windows.Forms.Button clearPageButton;
        private System.Windows.Forms.Label numPagesLabel;
        private ThemedRadioButton cursorRadioButton;
        private ThemedRadioButton markerRadioButton;
        private ThemedRadioButton boxRadioButton;
        private ThemedGroupBox groupBoxSelections;
        private ThemedCheckBox outlineCheckBox;
        private ThemedCheckBox colorCheckBox;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox pageNumberTextBox;
        private System.Windows.Forms.Button zoomMinButton;
        private System.Windows.Forms.SplitContainer formSplitContainer;
        private System.Windows.Forms.ListView pagesListView;
        private System.Windows.Forms.Button zoomInButton;
        private System.Windows.Forms.Button zoomOutButton;
        private System.Windows.Forms.Button openProjectButton;
        private ThemedGroupBox groupBoxOpen;
        private ThemedGroupBox groupBoxSave;
        private ThemedGroupBox groupBoxOptions;
        private System.Windows.Forms.Button saveProjectAsButton;
        private System.Windows.Forms.Button selectionLastButton;
        private System.Windows.Forms.Button selectionNextButton;
        private System.Windows.Forms.Button selectionPrevButton;
        private System.Windows.Forms.Button selectionFirstButton;
        private System.Windows.Forms.Button buttonFirst;
        private System.Windows.Forms.Button buttonLast;
        private System.Windows.Forms.Button saveProjectButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuFileItem;
        private System.Windows.Forms.ToolStripMenuItem openPdfToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveProjectAsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveProjectMenuItem;
        private System.Windows.Forms.ToolStripMenuItem savePdfMenuItem;
        private System.Windows.Forms.ToolStripMenuItem savePdfPageRangeMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItemPrintSeparator;
        private System.Windows.Forms.ToolStripMenuItem printPdfMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem recentFilesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importProjectMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItemCloseDocumentSeparator;
        private System.Windows.Forms.ToolStripMenuItem closeDocumentMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.Button zoomMaxButton;
        private System.Windows.Forms.ToolStripMenuItem menuHelpItem;
        private System.Windows.Forms.ToolStripMenuItem helpMenuItem;
        private System.Windows.Forms.ToolStripMenuItem whatsNewMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutMenuItem;
        private ThemedCheckBox openSavedPDFCheckBox;
        private System.Windows.Forms.ToolStripMenuItem openLastPdfProjectToolStripMenuItem;
        private ThemedGroupBox groupBoxSignatures;
        private ThemedRadioButton signaturesRemoveRadioButton;
        private ThemedRadioButton signaturesReportRadioButton;
        private ThemedRadioButton signaturesOriginalRadioButton;
        private System.Windows.Forms.Button removePageButton;
        private System.Windows.Forms.Button removePageRangeButton;
        private ThemedGroupBox groupBoxSearch;
        private System.Windows.Forms.TextBox searchTextBox;
        private System.Windows.Forms.Button searchLastButton;
        private System.Windows.Forms.Button searchNextButton;
        private System.Windows.Forms.Button searchPrevButton;
        private System.Windows.Forms.Button searchFirstButton;
        private System.Windows.Forms.Button personalDataButton;
        private System.Windows.Forms.Button SearchClearButton;
        private System.Windows.Forms.Label searchResultLabel;
        private ThemedGroupBox groupBoxPages;
        private ThemedGroupBox groupBoxPagesToRemove;
        private System.Windows.Forms.Button searchToSelectionButton;
        private System.Windows.Forms.Button searchButton;
        private ThemedCheckBox safeModeCheckBox;
        private System.Windows.Forms.ToolStripMenuItem quickStartMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tutorialMenuItem;
        private System.Windows.Forms.ToolStripMenuItem diagnosticModeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showLicenseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem thirdPartyNoticesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuOptionsItem;
        private System.Windows.Forms.ToolStripMenuItem themeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem themeSoftLightMenuItem;
        private System.Windows.Forms.ToolStripMenuItem themeNordCoolLightMenuItem;
        private System.Windows.Forms.ToolStripMenuItem themeBalticBreezeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem themeWarmSandMenuItem;
        private System.Windows.Forms.ToolStripMenuItem themeForestGreenMenuItem;
        private System.Windows.Forms.ToolStripMenuItem themeGraphiteDarkMenuItem;
        private System.Windows.Forms.ToolStripMenuItem themeOledDarkTealMenuItem;
        private System.Windows.Forms.ToolStripMenuItem themeBlackYellowHighContrastMenuItem;
        private System.Windows.Forms.ToolStripMenuItem themeBlackWhiteHighContrastMenuItem;
        private System.Windows.Forms.ToolStripSeparator themeToolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem languageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem languageSystemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem languageEnglishToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem languagePolishToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem languageGermanToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fullScreenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem splitPdfToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mergePdfToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addTextMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem ignorePdfRestrictionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deletePageMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rotatePageMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private ThemedGroupBox groupBoxFilter;
        private System.Windows.Forms.ComboBox filterComboBox;
        private System.Windows.Forms.TabControl pagesTabControl;
        private System.Windows.Forms.TabPage pagesListTabPage;
        private System.Windows.Forms.TabPage thumbnailsTabPage;
        private System.Windows.Forms.TabPage layersTabPage;
        private System.Windows.Forms.Panel layersTabPlaceholderPanel;
        private System.Windows.Forms.ListView thumbnailsListView;
        private System.Windows.Forms.ImageList thumbnailsImageList;
        private System.Windows.Forms.ToolStripMenuItem copyToClipboardMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportGraphicsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectSignaturesToRemoveMenuItem;
        private ThemedCheckBox setSavePassword;
    }
}

