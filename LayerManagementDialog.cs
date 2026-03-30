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
    internal sealed class LayerManagementDialog : Form
    {
        private readonly BindingList<LayerRowModel> layerRows;
        private readonly Dictionary<string, int> layerUsageCounts;
        private readonly DataGridView layersGridView;
        private readonly Button addButton;
        private readonly Button deleteButton;
        private readonly Button moveUpButton;
        private readonly Button moveDownButton;
        private readonly Button saveButton;
        private readonly Button cancelButton;
        private readonly CheckBox exportVisibleLayersOnlyCheckBox;
        private readonly List<LayerDeletionAction> pendingLayerDeletionActions = new List<LayerDeletionAction>();

        internal event Action<List<LayerDefinition>, string> PreviewChanged;

        internal LayerManagementDialog(
            IEnumerable<LayerDefinition> layers,
            string activeLayerId,
            bool exportVisibleLayersOnly,
            IDictionary<string, int> usageCounts,
            Color checkBoxBorderColor,
            Color checkBoxAccentColor,
            Color checkBoxBackColor)
        {
            layerUsageCounts = new Dictionary<string, int>(usageCounts ?? new Dictionary<string, int>(), StringComparer.OrdinalIgnoreCase);
            layerRows = new BindingList<LayerRowModel>(
                (layers ?? Enumerable.Empty<LayerDefinition>())
                    .OrderBy(layer => layer?.Order ?? int.MaxValue)
                    .Select(layer => LayerRowModel.FromLayer(layer, string.Equals(PDFForm.NormalizeLayerIdForExternalUse(layer?.Id), PDFForm.NormalizeLayerIdForExternalUse(activeLayerId), StringComparison.OrdinalIgnoreCase)))
                    .ToList());

            foreach (LayerRowModel row in layerRows)
            {
                row.UsageCount = layerUsageCounts.TryGetValue(PDFForm.NormalizeLayerIdForExternalUse(row.Id), out int count) ? count : 0;
            }

            EnsureSingleActiveRow();

            Text = Tr("Dialog_Layers_Title");
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MinimizeBox = false;
            MaximizeBox = false;
            ShowInTaskbar = false;
            ClientSize = new Size(840, 470);

            layersGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                EditMode = DataGridViewEditMode.EditProgrammatically,
                DataSource = layerRows
            };
            layersGridView.CurrentCellDirtyStateChanged += LayersGridView_CurrentCellDirtyStateChanged;
            layersGridView.CellValueChanged += LayersGridView_CellValueChanged;
            layersGridView.CellDoubleClick += LayersGridView_CellDoubleClick;
            layersGridView.CellContentClick += LayersGridView_CellContentClick;
            layersGridView.CellPainting += LayersGridView_CellPainting;
            layersGridView.SelectionChanged += (_, __) => UpdateButtonState();

            layersGridView.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "DialogLayersActiveColumn",
                DataPropertyName = nameof(LayerRowModel.IsActive),
                HeaderText = Tr("Dialog_Layers_Column_Active"),
                Width = 56,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });
            layersGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DialogLayersNameColumn",
                DataPropertyName = nameof(LayerRowModel.Name),
                HeaderText = Tr("Dialog_Layers_Column_Name"),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 45f,
                MinimumWidth = 160,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });
            layersGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DialogLayersGroupColumn",
                DataPropertyName = nameof(LayerRowModel.GroupName),
                HeaderText = Tr("Dialog_Layers_Column_Group"),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 35f,
                MinimumWidth = 140,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });
            layersGridView.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "DialogLayersVisibleColumn",
                DataPropertyName = nameof(LayerRowModel.IsVisible),
                HeaderText = Tr("Dialog_Layers_Column_Visible"),
                Width = 76,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });
            layersGridView.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "DialogLayersLockedColumn",
                DataPropertyName = nameof(LayerRowModel.IsLocked),
                HeaderText = Tr("Dialog_Layers_Column_Locked"),
                Width = 82,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });
            layersGridView.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "DialogLayersExportColumn",
                DataPropertyName = nameof(LayerRowModel.ExportEnabled),
                HeaderText = Tr("Dialog_Layers_Column_Export"),
                Width = 82,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });
            layersGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DialogLayersUsageColumn",
                DataPropertyName = nameof(LayerRowModel.UsageCount),
                HeaderText = Tr("Dialog_Layers_Column_ObjectCount"),
                Width = 72,
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });

            addButton = CreateButton("Dialog_Layers_Button_Add", AddButton_Click);
            deleteButton = CreateButton("Dialog_Layers_Button_Delete", DeleteButton_Click);
            moveUpButton = CreateButton("Merge_Up", MoveUpButton_Click);
            moveDownButton = CreateButton("Merge_Down", MoveDownButton_Click);
            saveButton = CreateButton("Dialog_Button_Save", SaveButton_Click);
            cancelButton = CreateButton("Dialog_Button_Cancel", (_, __) => DialogResult = DialogResult.Cancel);
            cancelButton.DialogResult = DialogResult.Cancel;
            exportVisibleLayersOnlyCheckBox = new CheckBox
            {
                AutoSize = true,
                Text = Tr("Dialog_Layers_ExportVisibleOnly"),
                Checked = exportVisibleLayersOnly,
                Padding = new Padding(addButton.Margin.Left, 6, 0, 0)
            };

            var leftPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Left,
                Width = 520,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
            leftPanel.Controls.Add(addButton);
            leftPanel.Controls.Add(deleteButton);
            leftPanel.Controls.Add(moveUpButton);
            leftPanel.Controls.Add(moveDownButton);

            var rightPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                Width = 280,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
            rightPanel.Controls.Add(cancelButton);
            rightPanel.Controls.Add(saveButton);

            var buttonsPanel = new Panel
            {
                Dock = DockStyle.Fill
            };
            buttonsPanel.Controls.Add(leftPanel);
            buttonsPanel.Controls.Add(rightPanel);

            var exportOptionsPanel = new Panel
            {
                Dock = DockStyle.Fill
            };
            exportOptionsPanel.Controls.Add(exportVisibleLayersOnlyCheckBox);

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30f));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42f));
            layout.Controls.Add(layersGridView, 0, 0);
            layout.Controls.Add(exportOptionsPanel, 0, 1);
            layout.Controls.Add(buttonsPanel, 0, 2);

            Controls.Add(layout);
            AcceptButton = saveButton;
            CancelButton = cancelButton;

            if (layerRows.Count > 0)
            {
                layersGridView.DataBindingComplete += LayersGridView_DataBindingComplete;
            }

            UpdateButtonState();
        }

        internal List<LayerDefinition> GetLayers()
        {
            NormalizeRowOrder();
            return layerRows
                .Select((row, index) => row.ToLayer(index))
                .ToList();
        }

        internal string GetActiveLayerId()
        {
            LayerRowModel activeRow = layerRows.FirstOrDefault(row => row.IsActive);
            return activeRow?.Id ?? PDFForm.DefaultLayerId;
        }

        internal List<LayerDeletionAction> GetLayerDeletionActions()
        {
            return pendingLayerDeletionActions
                .Select(action => action?.Clone())
                .Where(action => action != null)
                .ToList();
        }

        internal bool GetExportVisibleLayersOnly()
        {
            return exportVisibleLayersOnlyCheckBox?.Checked == true;
        }

        private void RaisePreviewChanged()
        {
            if (pendingLayerDeletionActions.Count > 0)
            {
                return;
            }

            PreviewChanged?.Invoke(GetLayers(), GetActiveLayerId());
        }

        private static string Tr(string key)
        {
            return Resources.ResourceManager.GetString(key, CultureInfo.CurrentUICulture) ?? key;
        }

        private static Button CreateButton(string textResourceKey, EventHandler clickHandler)
        {
            var button = new Button
            {
                Text = Tr(textResourceKey),
                Width = 120,
                Height = 28
            };
            button.Click += clickHandler;
            return button;
        }

        private LayerRowModel GetSelectedRow()
        {
            if (layersGridView.CurrentRow?.DataBoundItem is LayerRowModel currentRow)
            {
                return currentRow;
            }

            return null;
        }

        private void UpdateButtonState()
        {
            LayerRowModel selectedRow = GetSelectedRow();
            bool hasSelection = selectedRow != null;
            bool canDelete = hasSelection && !selectedRow.IsSystem;
            bool canMoveUp = hasSelection && CanMoveRow(selectedRow, moveUp: true);
            bool canMoveDown = hasSelection && CanMoveRow(selectedRow, moveUp: false);

            deleteButton.Enabled = canDelete;
            moveUpButton.Enabled = canMoveUp;
            moveDownButton.Enabled = canMoveDown;
        }

        private void EnsureSingleActiveRow()
        {
            LayerRowModel activeRow = layerRows.FirstOrDefault(row => row.IsActive)
                ?? layerRows.FirstOrDefault(row => string.Equals(row.Id, PDFForm.DefaultLayerId, StringComparison.OrdinalIgnoreCase))
                ?? layerRows.FirstOrDefault();

            foreach (LayerRowModel row in layerRows)
            {
                row.IsActive = ReferenceEquals(row, activeRow);
            }
        }

        private void NormalizeRowOrder()
        {
            if (layerRows.Count == 0)
            {
                return;
            }

            LayerRowModel workRow = layerRows.FirstOrDefault(row => string.Equals(row.Id, PDFForm.WorkLayerId, StringComparison.OrdinalIgnoreCase));
            if (workRow != null)
            {
                int workIndex = layerRows.IndexOf(workRow);
                if (workIndex > 0)
                {
                    layerRows.RemoveAt(workIndex);
                    layerRows.Insert(0, workRow);
                }
            }
        }

        private bool CanMoveRow(LayerRowModel row, bool moveUp)
        {
            if (row == null)
            {
                return false;
            }

            int index = layerRows.IndexOf(row);
            if (index < 0)
            {
                return false;
            }

            if (moveUp)
            {
                return index > 1;
            }

            return index >= 1 && index < layerRows.Count - 1;
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            var row = new LayerRowModel
            {
                Id = Guid.NewGuid().ToString("N"),
                Name = GenerateNewLayerName(),
                GroupName = string.Empty,
                IsVisible = true,
                IsLocked = false,
                ExportEnabled = true,
                IsSystem = false,
                UsageCount = 0,
                IsActive = false
            };

            int insertIndex = layerRows.Count > 0 ? 1 : 0;
            layerRows.Insert(insertIndex, row);
            int rowIndex = layerRows.IndexOf(row);
            if (rowIndex >= 0 && rowIndex < layersGridView.Rows.Count)
            {
                layersGridView.ClearSelection();
                layersGridView.Rows[rowIndex].Selected = true;
                layersGridView.CurrentCell = layersGridView.Rows[rowIndex].Cells[1];
            }

            UpdateButtonState();
            RaisePreviewChanged();
        }

        private string GenerateNewLayerName()
        {
            string baseName = Tr("Dialog_Layers_NewLayerName");
            if (!layerRows.Any(row => string.Equals(row.Name?.Trim(), baseName, StringComparison.CurrentCultureIgnoreCase)))
            {
                return baseName;
            }

            int number = 2;
            while (true)
            {
                string candidate = string.Format(CultureInfo.CurrentCulture, Tr("Dialog_Layers_NewLayerName_Numbered"), number);
                if (!layerRows.Any(row => string.Equals(row.Name?.Trim(), candidate, StringComparison.CurrentCultureIgnoreCase)))
                {
                    return candidate;
                }

                number++;
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            LayerRowModel selectedRow = GetSelectedRow();
            if (selectedRow == null)
            {
                return;
            }

            if (selectedRow.IsSystem)
            {
                MessageBox.Show(this, Tr("Dialog_Layers_DeleteSystemDenied"), Resources.Title_Warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (pendingLayerDeletionActions.Any(action => string.Equals(action.SourceLayerId, selectedRow.Id, StringComparison.OrdinalIgnoreCase)))
            {
                return;
            }

            if (selectedRow.UsageCount > 0)
            {
                List<LayerMoveTargetOption> targetOptions = layerRows
                    .Where(row => row != null &&
                        !string.Equals(row.Id, selectedRow.Id, StringComparison.OrdinalIgnoreCase))
                    .Select(row => new LayerMoveTargetOption
                    {
                        Id = row.Id,
                        Name = row.Name
                    })
                    .ToList();
                string defaultTargetLayerId = targetOptions
                    .FirstOrDefault(option => string.Equals(option.Id, PDFForm.WorkLayerId, StringComparison.OrdinalIgnoreCase))?.Id
                    ?? targetOptions.FirstOrDefault()?.Id
                    ?? PDFForm.WorkLayerId;

                LayerDeleteDecision decision = PromptForDeleteLayerDecision(selectedRow, defaultTargetLayerId, targetOptions);
                if (decision.Action == LayerDeleteAction.Cancel)
                {
                    return;
                }

                pendingLayerDeletionActions.Add(new LayerDeletionAction
                {
                    SourceLayerId = selectedRow.Id,
                    Action = decision.Action,
                    TargetLayerId = decision.TargetLayerId
                });
            }
            else
            {
                if (!ConfirmDeleteEmptyLayer(selectedRow))
                {
                    return;
                }
            }

            bool wasActive = selectedRow.IsActive;
            int index = layerRows.IndexOf(selectedRow);
            layerRows.Remove(selectedRow);

            if (wasActive)
            {
                LayerRowModel defaultRow = layerRows.FirstOrDefault(row => string.Equals(row.Id, PDFForm.DefaultLayerId, StringComparison.OrdinalIgnoreCase));
                if (defaultRow != null)
                {
                    defaultRow.IsActive = true;
                }
            }

            EnsureSingleActiveRow();

            if (layerRows.Count > 0)
            {
                int targetIndex = Math.Max(0, Math.Min(index, layerRows.Count - 1));
                layersGridView.ClearSelection();
                layersGridView.Rows[targetIndex].Selected = true;
                layersGridView.CurrentCell = layersGridView.Rows[targetIndex].Cells[1];
            }

            UpdateButtonState();
            RaisePreviewChanged();
        }

        private LayerDeleteDecision PromptForDeleteLayerDecision(
            LayerRowModel selectedRow,
            string defaultTargetLayerId,
            IReadOnlyList<LayerMoveTargetOption> targetOptions)
        {
            using (var prompt = new Form())
            {
                prompt.Text = Tr("Dialog_Layers_DeleteNonEmpty_Title");
                prompt.StartPosition = FormStartPosition.CenterParent;
                prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
                prompt.MinimizeBox = false;
                prompt.MaximizeBox = false;
                prompt.ShowInTaskbar = false;
                prompt.ClientSize = new Size(520, 180);

                var infoLabel = new Label
                {
                    Left = 16,
                    Top = 16,
                    Width = 488,
                    Height = 46,
                    Text = string.Format(CultureInfo.CurrentCulture, Tr("Dialog_Layers_DeleteNonEmpty"), selectedRow.Name, selectedRow.UsageCount)
                };

                var comboLabel = new Label
                {
                    Left = 16,
                    Top = 76,
                    Width = 180,
                    Text = Tr("Dialog_Layers_DeleteMoveTarget")
                };

                var layerCombo = new ComboBox
                {
                    Left = 16,
                    Top = 100,
                    Width = 488,
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    DisplayMember = nameof(LayerMoveTargetOption.Name),
                    ValueMember = nameof(LayerMoveTargetOption.Id)
                };
                layerCombo.Items.AddRange(targetOptions.Cast<object>().ToArray());
                if (layerCombo.Items.Count > 0)
                {
                    int selectedIndex = targetOptions
                        .Select((option, index) => new { option, index })
                        .FirstOrDefault(item => string.Equals(item.option.Id, defaultTargetLayerId, StringComparison.OrdinalIgnoreCase))?.index ?? 0;
                    layerCombo.SelectedIndex = selectedIndex;
                }

                var deleteButton = new Button
                {
                    Left = 16,
                    Top = 138,
                    Width = 110,
                    Text = Tr("Dialog_Layers_DeleteObjects")
                };

                var cancelButton = new Button
                {
                    Left = 205,
                    Top = 138,
                    Width = 110,
                    Text = Tr("Dialog_Button_Cancel")
                };

                var moveButton = new Button
                {
                    Left = 394,
                    Top = 138,
                    Width = 110,
                    Text = Tr("Dialog_Layers_MoveObjects")
                };

                LayerDeleteDecision decision = new LayerDeleteDecision
                {
                    Action = LayerDeleteAction.Cancel,
                    TargetLayerId = defaultTargetLayerId
                };

                deleteButton.Click += (_, __) =>
                {
                    decision.Action = LayerDeleteAction.DeleteObjects;
                    prompt.DialogResult = DialogResult.OK;
                    prompt.Close();
                };
                cancelButton.Click += (_, __) =>
                {
                    decision.Action = LayerDeleteAction.Cancel;
                    prompt.DialogResult = DialogResult.Cancel;
                    prompt.Close();
                };
                moveButton.Click += (_, __) =>
                {
                    string targetLayerId = layerCombo.SelectedValue as string ?? defaultTargetLayerId;
                    if (string.IsNullOrWhiteSpace(targetLayerId))
                    {
                        return;
                    }

                    decision.Action = LayerDeleteAction.MoveObjects;
                    decision.TargetLayerId = targetLayerId;
                    prompt.DialogResult = DialogResult.OK;
                    prompt.Close();
                };

                prompt.AcceptButton = moveButton;
                prompt.CancelButton = cancelButton;
                prompt.Controls.Add(infoLabel);
                prompt.Controls.Add(comboLabel);
                prompt.Controls.Add(layerCombo);
                prompt.Controls.Add(deleteButton);
                prompt.Controls.Add(cancelButton);
                prompt.Controls.Add(moveButton);
                prompt.ShowDialog(this);
                return decision;
            }
        }

        private bool ConfirmDeleteEmptyLayer(LayerRowModel selectedRow)
        {
            DialogResult result = MessageBox.Show(
                this,
                string.Format(CultureInfo.CurrentCulture, Tr("Dialog_Layers_DeleteConfirmEmpty"), selectedRow.Name),
                Tr("Dialog_Layers_DeleteNonEmpty_Title"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Exclamation,
                MessageBoxDefaultButton.Button2);
            return result == DialogResult.Yes;
        }

        private void MoveUpButton_Click(object sender, EventArgs e)
        {
            MoveSelectedRow(-1);
        }

        private void MoveDownButton_Click(object sender, EventArgs e)
        {
            MoveSelectedRow(1);
        }

        private void MoveSelectedRow(int delta)
        {
            LayerRowModel selectedRow = GetSelectedRow();
            if (selectedRow == null)
            {
                return;
            }

            int oldIndex = layerRows.IndexOf(selectedRow);
            int newIndex = oldIndex + delta;
            if (newIndex < 1 || newIndex >= layerRows.Count)
            {
                return;
            }

            layerRows.RemoveAt(oldIndex);
            layerRows.Insert(newIndex, selectedRow);
            layersGridView.ClearSelection();
            layersGridView.Rows[newIndex].Selected = true;
            layersGridView.CurrentCell = layersGridView.Rows[newIndex].Cells[1];
            UpdateButtonState();
            RaisePreviewChanged();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            foreach (LayerRowModel row in layerRows)
            {
                row.Name = (row.Name ?? string.Empty).Trim();
                row.GroupName = (row.GroupName ?? string.Empty).Trim();
                if (string.IsNullOrWhiteSpace(row.Name))
                {
                    row.Name = string.Equals(row.Id, PDFForm.WorkLayerId, StringComparison.OrdinalIgnoreCase)
                        ? Tr("Layer_Work_DefaultName")
                        : string.Equals(row.Id, PDFForm.DefaultLayerId, StringComparison.OrdinalIgnoreCase)
                            ? Tr("Layer_Default_DefaultName")
                            : GenerateNewLayerName();
                }
            }

            NormalizeRowOrder();
            EnsureSingleActiveRow();
            DialogResult = DialogResult.OK;
        }

        private void LayersGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (layersGridView.IsCurrentCellDirty)
            {
                layersGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void LayersGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            layersGridView.DataBindingComplete -= LayersGridView_DataBindingComplete;
            if (layersGridView.Rows.Count > 0)
            {
                layersGridView.ClearSelection();
                layersGridView.Rows[0].Selected = true;
                layersGridView.CurrentCell = layersGridView.Rows[0].Cells.Cast<DataGridViewCell>().FirstOrDefault(cell => cell.Visible);
            }

            UpdateButtonState();
        }

        private void LayersGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= layerRows.Count || e.ColumnIndex < 0)
            {
                return;
            }

            DataGridViewColumn column = layersGridView.Columns[e.ColumnIndex];
            bool isNameColumn = string.Equals(column.DataPropertyName, nameof(LayerRowModel.Name), StringComparison.Ordinal);
            bool isGroupColumn = string.Equals(column.DataPropertyName, nameof(LayerRowModel.GroupName), StringComparison.Ordinal);
            if (!isNameColumn && !isGroupColumn)
            {
                return;
            }

            LayerRowModel row = layerRows[e.RowIndex];
            if (string.Equals(row.Id, PDFForm.WorkLayerId, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            if (isNameColumn && PromptForValue(Tr("Dialog_Layers_Rename_Title"), Tr("Dialog_Layers_Rename_Label"), row.Name, out string updatedName))
            {
                row.Name = updatedName;
                layersGridView.Refresh();
                RaisePreviewChanged();
            }
            else if (isGroupColumn && PromptForValue(Tr("Dialog_Layers_Group_Title"), Tr("Dialog_Layers_Group_Label"), row.GroupName, out string updatedGroupName))
            {
                row.GroupName = updatedGroupName;
                layersGridView.Refresh();
                RaisePreviewChanged();
            }
        }

        private bool PromptForValue(string title, string labelText, string initialValue, out string value)
        {
            value = initialValue ?? string.Empty;

            using (Form prompt = new Form())
            using (TextBox inputTextBox = new TextBox())
            using (Label label = new Label())
            using (Button okButton = new Button())
            using (Button cancelButton = new Button())
            {
                prompt.Text = title;
                prompt.StartPosition = FormStartPosition.CenterParent;
                prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
                prompt.MinimizeBox = false;
                prompt.MaximizeBox = false;
                prompt.ShowInTaskbar = false;
                prompt.ClientSize = new Size(420, 125);

                label.AutoSize = true;
                label.Left = 12;
                label.Top = 14;
                label.Text = labelText;

                inputTextBox.Left = 12;
                inputTextBox.Top = 38;
                inputTextBox.Width = 396;
                inputTextBox.Text = value;

                okButton.Text = Tr("Dialog_Button_Save");
                okButton.Width = 100;
                okButton.Height = 28;
                okButton.Left = 206;
                okButton.Top = 82;
                okButton.DialogResult = DialogResult.OK;

                cancelButton.Text = Tr("Dialog_Button_Cancel");
                cancelButton.Width = 100;
                cancelButton.Height = 28;
                cancelButton.Left = 308;
                cancelButton.Top = 82;
                cancelButton.DialogResult = DialogResult.Cancel;

                prompt.Controls.Add(label);
                prompt.Controls.Add(inputTextBox);
                prompt.Controls.Add(okButton);
                prompt.Controls.Add(cancelButton);
                prompt.AcceptButton = okButton;
                prompt.CancelButton = cancelButton;

                prompt.Shown += (_, __) =>
                {
                    inputTextBox.Focus();
                    inputTextBox.SelectAll();
                };

                if (prompt.ShowDialog(this) != DialogResult.OK)
                {
                    return false;
                }

                value = (inputTextBox.Text ?? string.Empty).Trim();
                return true;
            }
        }

        private void LayersGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= layerRows.Count || e.ColumnIndex < 0)
            {
                return;
            }

            DataGridViewColumn column = layersGridView.Columns[e.ColumnIndex];
            string propertyName = column.DataPropertyName;
            if (!string.Equals(propertyName, nameof(LayerRowModel.IsActive), StringComparison.Ordinal) &&
                !string.Equals(propertyName, nameof(LayerRowModel.IsVisible), StringComparison.Ordinal) &&
                !string.Equals(propertyName, nameof(LayerRowModel.IsLocked), StringComparison.Ordinal) &&
                !string.Equals(propertyName, nameof(LayerRowModel.ExportEnabled), StringComparison.Ordinal))
            {
                return;
            }

            LayerRowModel row = layerRows[e.RowIndex];
            layersGridView.ClearSelection();
            layersGridView.Rows[e.RowIndex].Selected = true;
            layersGridView.CurrentCell = layersGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];

            if (string.Equals(propertyName, nameof(LayerRowModel.IsActive), StringComparison.Ordinal))
            {
                row.IsActive = true;
            }
            else if (string.Equals(propertyName, nameof(LayerRowModel.IsVisible), StringComparison.Ordinal))
            {
                row.IsVisible = !row.IsVisible;
            }
            else if (string.Equals(propertyName, nameof(LayerRowModel.IsLocked), StringComparison.Ordinal))
            {
                row.IsLocked = !row.IsLocked;
            }
            else if (string.Equals(propertyName, nameof(LayerRowModel.ExportEnabled), StringComparison.Ordinal))
            {
                if (!string.Equals(row.Id, PDFForm.WorkLayerId, StringComparison.OrdinalIgnoreCase))
                {
                    row.ExportEnabled = !row.ExportEnabled;
                }
            }

            LayersGridView_CellValueChanged(sender, new DataGridViewCellEventArgs(e.ColumnIndex, e.RowIndex));
        }

        private void LayersGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= layerRows.Count || e.ColumnIndex < 0)
            {
                return;
            }

            LayerRowModel row = layerRows[e.RowIndex];
            string propertyName = layersGridView.Columns[e.ColumnIndex].DataPropertyName;
            if (!string.Equals(propertyName, nameof(LayerRowModel.ExportEnabled), StringComparison.Ordinal) ||
                !string.Equals(row.Id, PDFForm.WorkLayerId, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            e.Paint(e.CellBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);
            e.Handled = true;
        }

        private void LayersGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= layerRows.Count || e.ColumnIndex < 0)
            {
                return;
            }

            LayerRowModel row = layerRows[e.RowIndex];
            string propertyName = layersGridView.Columns[e.ColumnIndex].DataPropertyName;

            if (string.Equals(propertyName, nameof(LayerRowModel.ExportEnabled), StringComparison.Ordinal) &&
                string.Equals(row.Id, PDFForm.WorkLayerId, StringComparison.OrdinalIgnoreCase))
            {
                row.ExportEnabled = false;
            }

            if (string.Equals(propertyName, nameof(LayerRowModel.IsActive), StringComparison.Ordinal))
            {
                if (row.IsActive)
                {
                    foreach (LayerRowModel otherRow in layerRows.Where(otherRow => !ReferenceEquals(otherRow, row)))
                    {
                        otherRow.IsActive = false;
                    }
                }
                else if (!layerRows.Any(otherRow => otherRow.IsActive))
                {
                    row.IsActive = true;
                }
            }

            layersGridView.Refresh();
            UpdateButtonState();
            RaisePreviewChanged();
        }

        private sealed class LayerRowModel : INotifyPropertyChanged
        {
            private string name = string.Empty;
            private string groupName = string.Empty;
            private bool isActive;
            private bool isVisible = true;
            private bool isLocked;
            private bool exportEnabled = true;

            public string Id { get; set; } = PDFForm.DefaultLayerId;

            public string Name
            {
                get => name;
                set
                {
                    if (!string.Equals(name, value, StringComparison.Ordinal))
                    {
                        name = value;
                        OnPropertyChanged(nameof(Name));
                    }
                }
            }

            public string GroupName
            {
                get => groupName;
                set
                {
                    string normalized = (value ?? string.Empty).Trim();
                    if (!string.Equals(groupName, normalized, StringComparison.Ordinal))
                    {
                        groupName = normalized;
                        OnPropertyChanged(nameof(GroupName));
                    }
                }
            }

            public bool IsActive
            {
                get => isActive;
                set
                {
                    if (isActive != value)
                    {
                        isActive = value;
                        OnPropertyChanged(nameof(IsActive));
                    }
                }
            }

            public bool IsVisible
            {
                get => isVisible;
                set
                {
                    if (isVisible != value)
                    {
                        isVisible = value;
                        OnPropertyChanged(nameof(IsVisible));
                    }
                }
            }

            public bool IsLocked
            {
                get => isLocked;
                set
                {
                    if (isLocked != value)
                    {
                        isLocked = value;
                        OnPropertyChanged(nameof(IsLocked));
                    }
                }
            }

            public bool ExportEnabled
            {
                get => exportEnabled;
                set
                {
                    if (exportEnabled != value)
                    {
                        exportEnabled = value;
                        OnPropertyChanged(nameof(ExportEnabled));
                    }
                }
            }

            public bool IsSystem { get; set; }

            public int UsageCount { get; set; }

            public static LayerRowModel FromLayer(LayerDefinition layer, bool isActive)
            {
                return new LayerRowModel
                {
                    Id = PDFForm.NormalizeLayerIdForExternalUse(layer?.Id),
                    Name = layer?.Name ?? string.Empty,
                    GroupName = layer?.GroupName ?? string.Empty,
                    IsActive = isActive,
                    IsVisible = layer == null || layer.IsVisible,
                    IsLocked = layer != null && layer.IsLocked,
                    ExportEnabled = layer == null || !layer.ExcludeFromExport,
                    IsSystem = layer != null && layer.IsSystem,
                    UsageCount = 0
                };
            }

            public LayerDefinition ToLayer(int order)
            {
                return new LayerDefinition
                {
                    Id = PDFForm.NormalizeLayerIdForExternalUse(Id),
                    Name = (Name ?? string.Empty).Trim(),
                    GroupName = (GroupName ?? string.Empty).Trim(),
                    Order = order,
                    IsVisible = IsVisible,
                    IsLocked = IsLocked,
                    ExcludeFromExport = !ExportEnabled,
                    IsSystem = IsSystem
                };
            }

            public event PropertyChangedEventHandler PropertyChanged;

            private void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        internal enum LayerDeleteAction
        {
            Cancel = 0,
            DeleteObjects = 1,
            MoveObjects = 2
        }

        internal sealed class LayerDeletionAction
        {
            public string SourceLayerId { get; set; }
            public LayerDeleteAction Action { get; set; }
            public string TargetLayerId { get; set; }

            public LayerDeletionAction Clone()
            {
                return new LayerDeletionAction
                {
                    SourceLayerId = SourceLayerId,
                    Action = Action,
                    TargetLayerId = TargetLayerId
                };
            }
        }

        private sealed class LayerDeleteDecision
        {
            public LayerDeleteAction Action { get; set; }
            public string TargetLayerId { get; set; }
        }

        private sealed class LayerMoveTargetOption
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }

    }
}
