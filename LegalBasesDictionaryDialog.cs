using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using AnonPDF.Properties;

namespace AnonPDF
{
    internal sealed class LegalBasesDictionaryDialog : Form
    {
        private readonly BindingList<LegalBasisDefinition> globalLegalBases;
        private readonly BindingList<LegalBasisDefinition> localLegalBases;
        private readonly BindingList<ExclusionScopeDefinition> globalExclusionScopes;
        private readonly BindingList<ExclusionScopeDefinition> localExclusionScopes;
        private readonly bool canEditGlobal;
        private readonly bool canEditLocal;
        private readonly bool canEditGlobalScopes;
        private readonly bool canEditLocalScopes;
        private readonly DialogTheme dialogTheme;

        private readonly TabControl tabControl;
        private readonly TabPage globalTabPage;
        private readonly TabPage localTabPage;
        private readonly DataGridView globalGridView;
        private readonly DataGridView localGridView;
        private readonly Label globalStatusLabel;
        private readonly Label localStatusLabel;
        private readonly Button addButton;
        private readonly Button editButton;
        private readonly Button deleteButton;
        private readonly Button saveButton;
        private readonly Button cancelButton;

        internal LegalBasesDictionaryDialog(
            IEnumerable<LegalBasisDefinition> globalEntries,
            IEnumerable<LegalBasisDefinition> localEntries,
            IEnumerable<ExclusionScopeDefinition> globalScopes,
            IEnumerable<ExclusionScopeDefinition> localScopes,
            bool canEditGlobalEntries,
            bool canEditLocalEntries,
            bool canEditGlobalScopesEntries,
            bool canEditLocalScopesEntries,
            string globalFilePath,
            string localFilePath,
            DialogTheme dialogTheme = null)
        {
            this.dialogTheme = dialogTheme;
            canEditGlobal = canEditGlobalEntries;
            canEditLocal = canEditLocalEntries;
            canEditGlobalScopes = canEditGlobalScopesEntries;
            canEditLocalScopes = canEditLocalScopesEntries;
            globalLegalBases = new BindingList<LegalBasisDefinition>(CloneList(globalEntries, LegalBasisSource.Global));
            localLegalBases = new BindingList<LegalBasisDefinition>(CloneList(localEntries, LegalBasisSource.Local));
            globalExclusionScopes = new BindingList<ExclusionScopeDefinition>(CloneScopeList(globalScopes, ExclusionScopeSource.Global));
            localExclusionScopes = new BindingList<ExclusionScopeDefinition>(CloneScopeList(localScopes, ExclusionScopeSource.Local));

            Text = Tr("Słownik podstaw prawnych", "Legal bases dictionary", "Rechtsgrundlagen");
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.Sizable;
            MinimizeBox = false;
            MaximizeBox = true;
            ShowInTaskbar = false;
            Size = PDFForm.ScaleSizeForDpiStatic(900, 480);
            MinimumSize = PDFForm.ScaleSizeForDpiStatic(700, 360);

            tabControl = new TabControl
            {
                Dock = DockStyle.Fill
            };
            tabControl.SelectedIndexChanged += (_, __) => UpdateActionButtonsState();

            globalTabPage = new TabPage(Tr("Globalne", "Global", "Global"));
            localTabPage = new TabPage(Tr("Użytkownika", "User", "Benutzer"));

            globalGridView = CreateGrid(globalLegalBases);
            localGridView = CreateGrid(localLegalBases);

            globalStatusLabel = CreateStatusLabel();
            localStatusLabel = CreateStatusLabel();

            PopulateTab(globalTabPage, globalStatusLabel, globalGridView);
            PopulateTab(localTabPage, localStatusLabel, localGridView);
            tabControl.TabPages.Add(globalTabPage);
            tabControl.TabPages.Add(localTabPage);

            UpdateStatusLabel(globalStatusLabel, globalFilePath, canEditGlobal);
            UpdateStatusLabel(localStatusLabel, localFilePath, canEditLocal);

            addButton = new Button
            {
                Text = Tr("Dodaj", "Add", "Hinzufuegen"),
                Width = PDFForm.ScaleForDpiStatic(120),
                Height = PDFForm.ScaleForDpiStatic(28)
            };
            addButton.Click += AddButton_Click;

            editButton = new Button
            {
                Text = Tr("Edytuj", "Edit", "Bearbeiten"),
                Width = PDFForm.ScaleForDpiStatic(120),
                Height = PDFForm.ScaleForDpiStatic(28)
            };
            editButton.Click += EditButton_Click;

            deleteButton = new Button
            {
                Text = Tr("Usuń", "Delete", "Loeschen"),
                Width = PDFForm.ScaleForDpiStatic(120),
                Height = PDFForm.ScaleForDpiStatic(28)
            };
            deleteButton.Click += DeleteButton_Click;

            saveButton = new Button
            {
                Text = Tr("Zapisz", "Save", "Speichern"),
                Width = PDFForm.ScaleForDpiStatic(120),
                Height = PDFForm.ScaleForDpiStatic(28)
            };
            saveButton.Click += SaveButton_Click;

            cancelButton = new Button
            {
                Text = Tr("Anuluj", "Cancel", "Abbrechen"),
                Width = PDFForm.ScaleForDpiStatic(120),
                Height = PDFForm.ScaleForDpiStatic(28),
                DialogResult = DialogResult.Cancel
            };

            var leftButtonsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Left,
                Width = PDFForm.ScaleForDpiStatic(420),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };
            leftButtonsPanel.Controls.Add(addButton);
            leftButtonsPanel.Controls.Add(editButton);
            leftButtonsPanel.Controls.Add(deleteButton);

            var rightButtonsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                Width = PDFForm.ScaleForDpiStatic(260),
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };
            rightButtonsPanel.Controls.Add(cancelButton);
            rightButtonsPanel.Controls.Add(saveButton);

            var buttonsPanel = new Panel
            {
                Dock = DockStyle.Fill
            };
            buttonsPanel.Controls.Add(leftButtonsPanel);
            buttonsPanel.Controls.Add(rightButtonsPanel);

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, PDFForm.ScaleForDpiStatic(40)));
            layout.Controls.Add(tabControl, 0, 0);
            layout.Controls.Add(buttonsPanel, 0, 1);

            Controls.Add(layout);
            AcceptButton = saveButton;
            CancelButton = cancelButton;

            DialogThemeApplier.ApplyTo(this, dialogTheme);
            UpdateStatusLabel(globalStatusLabel, globalFilePath, canEditGlobal);
            UpdateStatusLabel(localStatusLabel, localFilePath, canEditLocal);

            saveButton.Enabled = canEditGlobal || canEditLocal;
            UpdateActionButtonsState();
        }

        internal List<LegalBasisDefinition> GetGlobalLegalBases()
        {
            return CloneList(globalLegalBases, LegalBasisSource.Global);
        }

        internal List<LegalBasisDefinition> GetLocalLegalBases()
        {
            return CloneList(localLegalBases, LegalBasisSource.Local);
        }

        internal List<ExclusionScopeDefinition> GetGlobalExclusionScopes()
        {
            return CloneScopeList(globalExclusionScopes, ExclusionScopeSource.Global);
        }

        internal List<ExclusionScopeDefinition> GetLocalExclusionScopes()
        {
            return CloneScopeList(localExclusionScopes, ExclusionScopeSource.Local);
        }

        private static List<LegalBasisDefinition> CloneList(IEnumerable<LegalBasisDefinition> source, LegalBasisSource sourceKind)
        {
            return (source ?? Enumerable.Empty<LegalBasisDefinition>())
                .Select(item => new LegalBasisDefinition
                {
                    Id = item?.Id ?? string.Empty,
                    Title = item?.Title ?? string.Empty,
                    FullCitation = item?.FullCitation ?? string.Empty,
                    DescriptionHint = item?.DescriptionHint ?? string.Empty,
                    RequiresInterestSubject = item != null && item.RequiresInterestSubject,
                    SourceKind = sourceKind
                })
                .ToList();
        }

        private static List<ExclusionScopeDefinition> CloneScopeList(IEnumerable<ExclusionScopeDefinition> source, ExclusionScopeSource sourceKind)
        {
            return (source ?? Enumerable.Empty<ExclusionScopeDefinition>())
                .Select(item => new ExclusionScopeDefinition
                {
                    ScopeId = item?.ScopeId ?? string.Empty,
                    FriendlyName = item?.FriendlyName ?? string.Empty,
                    Category = item?.Category ?? string.Empty,
                    Description = item?.Description ?? string.Empty,
                    AutoDetectTags = new List<string>(item?.AutoDetectTags ?? new List<string>()),
                    DefaultBasisIds = new List<string>(item?.DefaultBasisIds ?? new List<string>()),
                    UiColor = item?.UiColor ?? string.Empty,
                    SourceKind = sourceKind
                })
                .ToList();
        }

        private static Label CreateStatusLabel()
        {
            return new Label
            {
                AutoSize = true,
                AutoEllipsis = true,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(4, 4, 4, 4)
            };
        }

        private void PopulateTab(TabPage tabPage, Label statusLabel, DataGridView gridView)
        {
            var tabLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2
            };
            tabLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tabLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            tabLayout.Controls.Add(statusLabel, 0, 0);
            tabLayout.Controls.Add(gridView, 0, 1);
            tabPage.Controls.Add(tabLayout);
        }

        private DataGridView CreateGrid(BindingList<LegalBasisDefinition> source)
        {
            var grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true,
                RowHeadersVisible = false
            };

            grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(LegalBasisDefinition.Id),
                HeaderText = Tr("ID", "ID", "ID"),
                Width = 180
            });
            grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(LegalBasisDefinition.Title),
                HeaderText = Tr("Nazwa skrócona", "Short title", "Kurztitel"),
                Width = 220
            });
            grid.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = nameof(LegalBasisDefinition.RequiresInterestSubject),
                HeaderText = Tr("Wymaga podmiotu", "Needs subject", "Subjekt erforderlich"),
                Width = PDFForm.ScaleForDpiStatic(160)
            });
            grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(LegalBasisDefinition.FullCitation),
                HeaderText = Tr("Podstawa szczegółowa", "Full citation", "Vollzitat"),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            grid.DataSource = source;
            grid.SelectionChanged += (_, __) => UpdateActionButtonsState();
            grid.CellDoubleClick += (_, __) => EditCurrentEntry();

            return grid;
        }

        private void UpdateStatusLabel(Label label, string path, bool canEdit)
        {
            string mode = canEdit
                ? Tr("zapis dostępny", "write enabled", "schreibbar")
                : Tr("tylko odczyt", "read only", "nur lesen");
            label.Text = string.Format(Tr("Plik: {0} ({1})", "File: {0} ({1})", "Datei: {0} ({1})"), path, mode);
            label.ForeColor = dialogTheme == null
                ? (canEdit ? SystemColors.ControlText : Color.DarkRed)
                : (canEdit ? dialogTheme.TextSecondaryColor : dialogTheme.TextPrimaryColor);
        }

        private BindingList<LegalBasisDefinition> GetCurrentBinding()
        {
            return tabControl.SelectedTab == globalTabPage ? globalLegalBases : localLegalBases;
        }

        private DataGridView GetCurrentGrid()
        {
            return tabControl.SelectedTab == globalTabPage ? globalGridView : localGridView;
        }

        private bool CanEditCurrentTab()
        {
            return tabControl.SelectedTab == globalTabPage ? canEditGlobal : canEditLocal;
        }

        private List<ExclusionScopeDefinition> GetScopesForSource(LegalBasisSource sourceKind)
        {
            if (sourceKind == LegalBasisSource.Global)
            {
                return globalExclusionScopes.ToList();
            }

            EnsureLocalScopesSeededForEditing();
            if (localExclusionScopes.Count > 0)
            {
                return localExclusionScopes.ToList();
            }

            return globalExclusionScopes.ToList();
        }

        private bool CanEditScopesForSource(LegalBasisSource sourceKind)
        {
            if (sourceKind == LegalBasisSource.Global)
            {
                return canEditGlobalScopes;
            }

            return canEditLocalScopes;
        }

        private BindingList<ExclusionScopeDefinition> GetTargetScopeBindingForSource(LegalBasisSource sourceKind)
        {
            if (sourceKind == LegalBasisSource.Global)
            {
                return globalExclusionScopes;
            }

            EnsureLocalScopesSeededForEditing();
            return localExclusionScopes.Count > 0 ? localExclusionScopes : globalExclusionScopes;
        }

        private void EnsureLocalScopesSeededForEditing()
        {
            if (localExclusionScopes.Count > 0 || !canEditLocalScopes)
            {
                return;
            }

            var seenScopeIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var scope in globalExclusionScopes)
            {
                string scopeId = (scope?.ScopeId ?? string.Empty).Trim();
                if (string.IsNullOrWhiteSpace(scopeId) || !seenScopeIds.Add(scopeId))
                {
                    continue;
                }

                localExclusionScopes.Add(new ExclusionScopeDefinition
                {
                    ScopeId = scopeId,
                    FriendlyName = string.IsNullOrWhiteSpace(scope?.FriendlyName) ? scopeId : scope.FriendlyName.Trim(),
                    Category = string.IsNullOrWhiteSpace(scope?.Category) ? string.Empty : scope.Category.Trim(),
                    Description = string.IsNullOrWhiteSpace(scope?.Description) ? string.Empty : scope.Description.Trim(),
                    AutoDetectTags = new List<string>(scope?.AutoDetectTags ?? new List<string>()),
                    DefaultBasisIds = new List<string>(),
                    UiColor = string.IsNullOrWhiteSpace(scope?.UiColor) ? string.Empty : scope.UiColor.Trim(),
                    SourceKind = ExclusionScopeSource.Local
                });
            }
        }

        private List<string> GetAssignedScopeIds(string basisId, LegalBasisSource sourceKind)
        {
            if (string.IsNullOrWhiteSpace(basisId))
            {
                return new List<string>();
            }

            string targetId = basisId.Trim();
            return GetScopesForSource(sourceKind)
                .Where(scope => (scope.DefaultBasisIds ?? new List<string>())
                    .Any(id => string.Equals(id?.Trim(), targetId, StringComparison.OrdinalIgnoreCase)))
                .Select(scope => (scope.ScopeId ?? string.Empty).Trim())
                .Where(scopeId => !string.IsNullOrWhiteSpace(scopeId))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        private void ApplyBasisAssignmentsToScopes(string basisId, LegalBasisSource sourceKind, IEnumerable<string> selectedScopeIds)
        {
            if (string.IsNullOrWhiteSpace(basisId))
            {
                return;
            }

            string targetId = basisId.Trim();
            var selected = new HashSet<string>(
                (selectedScopeIds ?? Enumerable.Empty<string>())
                    .Where(id => !string.IsNullOrWhiteSpace(id))
                    .Select(id => id.Trim()),
                StringComparer.OrdinalIgnoreCase);

            var scopes = GetTargetScopeBindingForSource(sourceKind);
            foreach (var scope in scopes)
            {
                if (scope == null)
                {
                    continue;
                }

                var defaultIds = scope.DefaultBasisIds ?? new List<string>();
                defaultIds = defaultIds
                    .Where(id => !string.IsNullOrWhiteSpace(id))
                    .Select(id => id.Trim())
                    .Where(id => !string.Equals(id, targetId, StringComparison.OrdinalIgnoreCase))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

                if (selected.Contains((scope.ScopeId ?? string.Empty).Trim()))
                {
                    defaultIds.Add(targetId);
                }

                scope.DefaultBasisIds = defaultIds
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();
            }
        }

        private List<string> GetScopeUsageDescriptions(string basisId, LegalBasisSource basisSourceKind)
        {
            if (string.IsNullOrWhiteSpace(basisId))
            {
                return new List<string>();
            }

            string targetId = basisId.Trim();
            var usage = new List<string>();
            bool includeGlobalScopes = basisSourceKind == LegalBasisSource.Global;
            bool includeLocalScopes = basisSourceKind == LegalBasisSource.Local || basisSourceKind == LegalBasisSource.Global;

            if (includeGlobalScopes)
            {
                string globalSourceName = Tr("globalny", "global", "global");
                foreach (var scope in globalExclusionScopes ?? new BindingList<ExclusionScopeDefinition>())
                {
                    if (scope == null)
                    {
                        continue;
                    }

                    bool isAssigned = (scope.DefaultBasisIds ?? new List<string>())
                        .Any(id => string.Equals(id?.Trim(), targetId, StringComparison.OrdinalIgnoreCase));
                    if (!isAssigned)
                    {
                        continue;
                    }

                    string scopeId = string.IsNullOrWhiteSpace(scope.ScopeId) ? "-" : scope.ScopeId.Trim();
                    string scopeName = string.IsNullOrWhiteSpace(scope.FriendlyName) ? scopeId : scope.FriendlyName.Trim();
                    usage.Add(string.Format("{0}: {1} ({2})", globalSourceName, scopeName, scopeId));
                }
            }

            if (includeLocalScopes)
            {
                string localSourceName = Tr("użytkownika", "user", "benutzer");
                foreach (var scope in localExclusionScopes ?? new BindingList<ExclusionScopeDefinition>())
                {
                    if (scope == null)
                    {
                        continue;
                    }

                    bool isAssigned = (scope.DefaultBasisIds ?? new List<string>())
                        .Any(id => string.Equals(id?.Trim(), targetId, StringComparison.OrdinalIgnoreCase));
                    if (!isAssigned)
                    {
                        continue;
                    }

                    string scopeId = string.IsNullOrWhiteSpace(scope.ScopeId) ? "-" : scope.ScopeId.Trim();
                    string scopeName = string.IsNullOrWhiteSpace(scope.FriendlyName) ? scopeId : scope.FriendlyName.Trim();
                    usage.Add(string.Format("{0}: {1} ({2})", localSourceName, scopeName, scopeId));
                }
            }

            return usage;
        }

        private LegalBasisDefinition GetCurrentSelectedItem()
        {
            var grid = GetCurrentGrid();
            if (grid.CurrentRow == null)
            {
                return null;
            }

            return grid.CurrentRow.DataBoundItem as LegalBasisDefinition;
        }

        private void UpdateActionButtonsState()
        {
            bool tabEditable = CanEditCurrentTab();
            bool hasSelection = GetCurrentSelectedItem() != null;
            addButton.Enabled = tabEditable;
            editButton.Enabled = tabEditable && hasSelection;
            deleteButton.Enabled = tabEditable && hasSelection;
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            if (!CanEditCurrentTab())
            {
                return;
            }

            var targetList = GetCurrentBinding();
            LegalBasisSource sourceKind = tabControl.SelectedTab == globalTabPage ? LegalBasisSource.Global : LegalBasisSource.Local;
            string generatedId = GenerateNextLegalBasisId(sourceKind);
            var availableScopes = GetScopesForSource(sourceKind);
            bool canEditScopes = CanEditScopesForSource(sourceKind);

            using (var editor = new LegalBasisEditDialog(
                null,
                sourceKind,
                generatedId,
                availableScopes,
                Enumerable.Empty<string>(),
                canEditScopes,
                dialogTheme))
            {
                if (editor.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                LegalBasisDefinition entry = editor.Result;
                if (IsDuplicateShortTitle(entry?.Title, null))
                {
                    ShowDialogMessage(
                        string.Format(
                            Tr("Nazwa skrócona już istnieje: {0}", "Short title already exists: {0}", "Kurztitel existiert bereits: {0}"),
                            entry?.Title ?? string.Empty),
                        MessageBoxIcon.Warning);
                    return;
                }

                targetList.Add(entry);

                if (canEditScopes)
                {
                    ApplyBasisAssignmentsToScopes(entry.Id, sourceKind, editor.SelectedScopeIds);
                }

                SelectItemInCurrentGrid(entry);
            }
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            EditCurrentEntry();
        }

        private void EditCurrentEntry()
        {
            if (!CanEditCurrentTab())
            {
                return;
            }

            LegalBasisDefinition currentEntry = GetCurrentSelectedItem();
            if (currentEntry == null)
            {
                return;
            }

            LegalBasisSource sourceKind = tabControl.SelectedTab == globalTabPage ? LegalBasisSource.Global : LegalBasisSource.Local;
            var availableScopes = GetScopesForSource(sourceKind);
            bool canEditScopes = CanEditScopesForSource(sourceKind);
            var selectedScopeIds = GetAssignedScopeIds(currentEntry.Id, sourceKind);
            using (var editor = new LegalBasisEditDialog(
                currentEntry,
                sourceKind,
                currentEntry.Id,
                availableScopes,
                selectedScopeIds,
                canEditScopes,
                dialogTheme))
            {
                if (editor.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                LegalBasisDefinition updatedEntry = editor.Result;
                if (IsDuplicateShortTitle(updatedEntry?.Title, currentEntry))
                {
                    ShowDialogMessage(
                        string.Format(
                            Tr("Nazwa skrócona już istnieje: {0}", "Short title already exists: {0}", "Kurztitel existiert bereits: {0}"),
                            updatedEntry?.Title ?? string.Empty),
                        MessageBoxIcon.Warning);
                    return;
                }

                currentEntry.Title = updatedEntry.Title;
                currentEntry.FullCitation = updatedEntry.FullCitation;
                currentEntry.DescriptionHint = updatedEntry.DescriptionHint;
                currentEntry.RequiresInterestSubject = updatedEntry.RequiresInterestSubject;

                if (canEditScopes)
                {
                    ApplyBasisAssignmentsToScopes(currentEntry.Id, sourceKind, editor.SelectedScopeIds);
                }

                GetCurrentGrid().Refresh();
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (!CanEditCurrentTab())
            {
                return;
            }

            LegalBasisDefinition selectedItem = GetCurrentSelectedItem();
            if (selectedItem == null)
            {
                return;
            }

            LegalBasisSource selectedSource = tabControl.SelectedTab == globalTabPage ? LegalBasisSource.Global : LegalBasisSource.Local;
            List<string> scopeUsage = GetScopeUsageDescriptions(selectedItem.Id, selectedSource);
            if (scopeUsage.Count > 0)
            {
                string usageList = string.Join(Environment.NewLine, scopeUsage.Select(item => "- " + item));
                ShowDialogMessage(
                    string.Format(
                        Tr(
                            "Nie można usunąć podstawy '{0}', bo jest przypisana do zakresów:{1}{2}",
                            "Cannot delete legal basis '{0}' because it is assigned to scopes:{1}{2}",
                            "Die Rechtsgrundlage '{0}' kann nicht geloescht werden, da sie Bereichen zugewiesen ist:{1}{2}"),
                        selectedItem.Title,
                        Environment.NewLine,
                        usageList),
                    MessageBoxIcon.Warning);
                return;
            }

            var decision = MessageBox.Show(
                this,
                string.Format(
                    Tr("Czy usunąć podstawę '{0}'?", "Delete legal basis '{0}'?", "Rechtsgrundlage '{0}' entfernen?"),
                    selectedItem.Title),
                Resources.Title_Confirmation,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
            if (decision != DialogResult.Yes)
            {
                return;
            }

            GetCurrentBinding().Remove(selectedItem);
            UpdateActionButtonsState();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (!ValidateForSave(out string validationMessage))
            {
                ShowDialogMessage(validationMessage, MessageBoxIcon.Warning);
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private bool ValidateForSave(out string message)
        {
            message = string.Empty;
            var idSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var titleSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var entry in globalLegalBases.Concat(localLegalBases))
            {
                string id = (entry?.Id ?? string.Empty).Trim();
                string title = (entry?.Title ?? string.Empty).Trim();
                if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(title))
                {
                    message = Tr(
                        "Każda podstawa musi mieć ID i nazwę skróconą.",
                        "Each legal basis must have ID and short title.",
                        "Jede Rechtsgrundlage muss eine ID und einen Kurztitel haben.");
                    return false;
                }

                if (!idSet.Add(id))
                {
                    message = string.Format(
                        Tr("Wykryto zduplikowane ID: {0}", "Duplicate ID detected: {0}", "Doppelte ID gefunden: {0}"),
                        id);
                    return false;
                }

                if (!titleSet.Add(title))
                {
                    message = string.Format(
                        Tr("Wykryto zduplikowaną nazwę skróconą: {0}", "Duplicate short title detected: {0}", "Doppelter Kurztitel gefunden: {0}"),
                        title);
                    return false;
                }
            }

            return true;
        }

        private bool IsDuplicateId(string id, LegalBasisDefinition except)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return false;
            }

            string normalizedId = id.Trim();
            foreach (var entry in globalLegalBases.Concat(localLegalBases))
            {
                if (entry == null || ReferenceEquals(entry, except))
                {
                    continue;
                }

                if (string.Equals((entry.Id ?? string.Empty).Trim(), normalizedId, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsDuplicateShortTitle(string title, LegalBasisDefinition except)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return false;
            }

            string normalizedTitle = title.Trim();
            foreach (var entry in globalLegalBases.Concat(localLegalBases))
            {
                if (entry == null || ReferenceEquals(entry, except))
                {
                    continue;
                }

                if (string.Equals((entry.Title ?? string.Empty).Trim(), normalizedTitle, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private string GenerateNextLegalBasisId(LegalBasisSource sourceKind)
        {
            string prefix = sourceKind == LegalBasisSource.Global ? "G_BASE_" : "L_BASE_";
            int maxNumber = 0;

            foreach (var entry in globalLegalBases.Concat(localLegalBases))
            {
                string candidateId = (entry?.Id ?? string.Empty).Trim();
                if (!candidateId.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                string numericPart = candidateId.Substring(prefix.Length);
                if (int.TryParse(numericPart, NumberStyles.Integer, CultureInfo.InvariantCulture, out int parsed) && parsed > maxNumber)
                {
                    maxNumber = parsed;
                }
            }

            int nextNumber = Math.Max(1, maxNumber + 1);
            string nextId;
            do
            {
                nextId = prefix + nextNumber.ToString("00", CultureInfo.InvariantCulture);
                nextNumber++;
            } while (IsDuplicateId(nextId, null));

            return nextId;
        }

        private void SelectItemInCurrentGrid(LegalBasisDefinition item)
        {
            var grid = GetCurrentGrid();
            foreach (DataGridViewRow row in grid.Rows)
            {
                if (ReferenceEquals(row.DataBoundItem, item))
                {
                    row.Selected = true;
                    grid.CurrentCell = row.Cells[0];
                    break;
                }
            }
        }

        private void ShowDialogMessage(string message, MessageBoxIcon icon)
        {
            MessageBox.Show(this, message, Resources.Title_Info, MessageBoxButtons.OK, icon);
        }

        private static string Tr(string pl, string en, string de)
        {
            string lang = (Resources.Culture ?? CultureInfo.CurrentUICulture).TwoLetterISOLanguageName;
            if (string.Equals(lang, "de", StringComparison.OrdinalIgnoreCase))
            {
                return de;
            }

            if (string.Equals(lang, "en", StringComparison.OrdinalIgnoreCase))
            {
                return en;
            }

            return pl;
        }
    }

    internal sealed class LegalBasisEditDialog : Form
    {
        private readonly TextBox idTextBox;
        private readonly TextBox titleTextBox;
        private readonly TextBox fullCitationTextBox;
        private readonly TextBox descriptionHintTextBox;
        private readonly CheckBox requiresInterestSubjectCheckBox;
        private readonly CheckedListBox scopesCheckedListBox;
        private readonly Label scopesStatusLabel;
        private readonly bool canEditScopeAssignments;
        private readonly Button okButton;
        private readonly Button cancelButton;

        internal LegalBasisDefinition Result { get; private set; }
        internal List<string> SelectedScopeIds { get; private set; } = new List<string>();

        internal LegalBasisEditDialog(
            LegalBasisDefinition source,
            LegalBasisSource sourceKind,
            string fixedId,
            IEnumerable<ExclusionScopeDefinition> availableScopes,
            IEnumerable<string> selectedScopeIds,
            bool canEditScopes,
            DialogTheme dialogTheme = null)
        {
            canEditScopeAssignments = canEditScopes;
            Text = source == null
                ? Tr("Dodaj podstawę prawną", "Add legal basis", "Rechtsgrundlage hinzufuegen")
                : Tr("Edytuj podstawę prawną", "Edit legal basis", "Rechtsgrundlage bearbeiten");

            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MinimizeBox = false;
            MaximizeBox = false;
            ShowInTaskbar = false;
            Width = PDFForm.ScaleForDpiStatic(780);
            Height = PDFForm.ScaleForDpiStatic(510);

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 8,
                Padding = new Padding(10)
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, PDFForm.ScaleForDpiStatic(160)));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, PDFForm.ScaleForDpiStatic(120)));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, PDFForm.ScaleForDpiStatic(180)));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            var sourceLabel = new Label
            {
                Text = string.Format(
                    Tr("Źródło: {0}", "Source: {0}", "Quelle: {0}"),
                    sourceKind == LegalBasisSource.Global
                        ? Tr("Globalne", "Global", "Global")
                        : Tr("Użytkownika", "User", "Benutzer")),
                AutoSize = true,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            layout.Controls.Add(sourceLabel, 0, 0);
            layout.SetColumnSpan(sourceLabel, 2);

            layout.Controls.Add(new Label
            {
                Text = Tr("ID (automatyczne)", "ID (automatic)", "ID (automatisch)"),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill
            }, 0, 1);
            idTextBox = new TextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                TabStop = false
            };
            layout.Controls.Add(idTextBox, 1, 1);

            layout.Controls.Add(new Label
            {
                Text = Tr("Nazwa skrócona", "Short title", "Kurztitel"),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill
            }, 0, 2);
            titleTextBox = new TextBox { Dock = DockStyle.Fill };
            layout.Controls.Add(titleTextBox, 1, 2);

            layout.Controls.Add(new Label
            {
                Text = Tr("Podstawa szczegółowa", "Full citation", "Vollzitat"),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill
            }, 0, 3);
            fullCitationTextBox = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };
            layout.Controls.Add(fullCitationTextBox, 1, 3);

            layout.Controls.Add(new Label
            {
                Text = Tr("Zakresy", "Scopes", "Bereiche"),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill
            }, 0, 4);

            scopesCheckedListBox = new CheckedListBox
            {
                Dock = DockStyle.Fill,
                CheckOnClick = true,
                HorizontalScrollbar = true,
                Enabled = canEditScopeAssignments
            };

            var selectedScopeSet = new HashSet<string>(
                (selectedScopeIds ?? Enumerable.Empty<string>())
                    .Where(id => !string.IsNullOrWhiteSpace(id))
                    .Select(id => id.Trim()),
                StringComparer.OrdinalIgnoreCase);

            foreach (var scope in (availableScopes ?? Enumerable.Empty<ExclusionScopeDefinition>())
                .Where(scope => scope != null && !string.IsNullOrWhiteSpace(scope.ScopeId))
                .OrderBy(scope => scope.FriendlyName ?? scope.ScopeId, StringComparer.CurrentCultureIgnoreCase))
            {
                string scopeId = scope.ScopeId.Trim();
                string scopeName = string.IsNullOrWhiteSpace(scope.FriendlyName) ? scopeId : scope.FriendlyName.Trim();
                string display = string.Equals(scopeName, scopeId, StringComparison.OrdinalIgnoreCase)
                    ? scopeName
                    : string.Format("{0} ({1})", scopeName, scopeId);

                scopesCheckedListBox.Items.Add(new ScopeListItem(scopeId, display), selectedScopeSet.Contains(scopeId));
            }

            layout.Controls.Add(scopesCheckedListBox, 1, 4);

            layout.Controls.Add(new Label
            {
                Text = Tr("Nazwa pomocnicza", "Hint", "Hinweis"),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill
            }, 0, 5);
            descriptionHintTextBox = new TextBox { Dock = DockStyle.Fill };
            layout.Controls.Add(descriptionHintTextBox, 1, 5);

            requiresInterestSubjectCheckBox = new CheckBox
            {
                Text = Tr(
                    "Wymaga podmiotu, w interesie którego dokonano wyłączenia",
                    "Requires interest subject",
                    "Subjekt erforderlich"),
                Dock = DockStyle.Fill,
                AutoSize = true
            };
            layout.Controls.Add(requiresInterestSubjectCheckBox, 0, 6);
            layout.SetColumnSpan(requiresInterestSubjectCheckBox, 2);

            scopesStatusLabel = new Label
            {
                AutoSize = true,
                ForeColor = SystemColors.GrayText,
                Text = canEditScopeAssignments
                    ? Tr("Zaznacz zakresy, dla których ta podstawa ma być domyślna.", "Select scopes where this basis is default.", "Waehlen Sie Bereiche, in denen diese Grundlage standard ist.")
                    : Tr("Brak uprawnień do zapisu zakresów - przypisania są tylko do odczytu.", "No permission to save scopes - assignments are read only.", "Keine Berechtigung zum Speichern von Bereichen - Zuordnungen sind nur lesbar.")
            };
            layout.Controls.Add(scopesStatusLabel, 0, 7);
            layout.SetColumnSpan(scopesStatusLabel, 2);

            okButton = new Button
            {
                Text = Tr("OK", "OK", "OK"),
                DialogResult = DialogResult.None,
                Width = PDFForm.ScaleForDpiStatic(110),
                Height = PDFForm.ScaleForDpiStatic(28)
            };
            okButton.Click += OkButton_Click;

            cancelButton = new Button
            {
                Text = Tr("Anuluj", "Cancel", "Abbrechen"),
                DialogResult = DialogResult.Cancel,
                Width = PDFForm.ScaleForDpiStatic(110),
                Height = PDFForm.ScaleForDpiStatic(28)
            };

            var buttonsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = PDFForm.ScaleForDpiStatic(40),
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(0, 0, 10, 0)
            };
            buttonsPanel.Controls.Add(cancelButton);
            buttonsPanel.Controls.Add(okButton);

            Controls.Add(layout);
            Controls.Add(buttonsPanel);

            AcceptButton = okButton;
            CancelButton = cancelButton;

            DialogThemeApplier.ApplyTo(this, dialogTheme);

            idTextBox.Text = source == null
                ? (fixedId ?? string.Empty)
                : (source.Id ?? string.Empty);

            if (source != null)
            {
                titleTextBox.Text = source.Title ?? string.Empty;
                fullCitationTextBox.Text = source.FullCitation ?? string.Empty;
                descriptionHintTextBox.Text = source.DescriptionHint ?? string.Empty;
                requiresInterestSubjectCheckBox.Checked = source.RequiresInterestSubject;
            }
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            string id = (idTextBox.Text ?? string.Empty).Trim();
            string title = (titleTextBox.Text ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(id))
            {
                MessageBox.Show(
                    this,
                    Tr("Nie można wygenerować identyfikatora podstawy.", "Cannot generate legal basis ID.", "Die ID der Rechtsgrundlage konnte nicht erstellt werden."),
                    Resources.Title_Warning,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(title))
            {
                MessageBox.Show(
                    this,
                    Tr("Pole nazwy skróconej jest wymagane.", "Short title is required.", "Kurztitel ist erforderlich."),
                    Resources.Title_Warning,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                titleTextBox.Focus();
                return;
            }

            Result = new LegalBasisDefinition
            {
                Id = id,
                Title = title,
                FullCitation = (fullCitationTextBox.Text ?? string.Empty).Trim(),
                DescriptionHint = (descriptionHintTextBox.Text ?? string.Empty).Trim(),
                RequiresInterestSubject = requiresInterestSubjectCheckBox.Checked
            };

            SelectedScopeIds = scopesCheckedListBox.Items
                .Cast<object>()
                .Where((item, index) => scopesCheckedListBox.GetItemChecked(index))
                .OfType<ScopeListItem>()
                .Select(item => item.ScopeId)
                .Where(scopeId => !string.IsNullOrWhiteSpace(scopeId))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            DialogResult = DialogResult.OK;
            Close();
        }

        private sealed class ScopeListItem
        {
            internal ScopeListItem(string scopeId, string displayText)
            {
                ScopeId = scopeId ?? string.Empty;
                DisplayText = string.IsNullOrWhiteSpace(displayText) ? ScopeId : displayText;
            }

            internal string ScopeId { get; }
            internal string DisplayText { get; }

            public override string ToString()
            {
                return DisplayText;
            }
        }

        private static string Tr(string pl, string en, string de)
        {
            string lang = (Resources.Culture ?? CultureInfo.CurrentUICulture).TwoLetterISOLanguageName;
            if (string.Equals(lang, "de", StringComparison.OrdinalIgnoreCase))
            {
                return de;
            }

            if (string.Equals(lang, "en", StringComparison.OrdinalIgnoreCase))
            {
                return en;
            }

            return pl;
        }
    }
}

