using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AnonPDF
{
    internal sealed class PageThumbnailCache : IDisposable
    {
        private readonly Dictionary<string, Bitmap> images = new Dictionary<string, Bitmap>();
        internal int Count => images.Count;
        internal bool ContainsKey(string key) => images.ContainsKey(key);
        internal bool TryGetValue(string key, out Bitmap image) => images.TryGetValue(key, out image);
        internal void Add(string key, Bitmap image) => images.Add(key, image);

        internal void Retain(IEnumerable<string> keys)
        {
            var retained = new HashSet<string>(keys);
            foreach (string key in images.Keys.Where(key => !retained.Contains(key)).ToArray())
            {
                images[key].Dispose();
                images.Remove(key);
            }
        }

        internal void Clear() { foreach (var image in images.Values) image.Dispose(); images.Clear(); }
        public void Dispose() => Clear();
    }

    internal sealed class PageThumbnailLayout
    {
        internal int Count { get; }
        internal int Columns { get; }
        internal Size ItemSize { get; }
        internal int ContentHeight => ((Count + Columns - 1) / Columns) * ItemSize.Height;

        internal PageThumbnailLayout(int count, int viewportWidth, Size preferredItemSize)
        {
            Count = Math.Max(0, count);
            Columns = Math.Max(1, viewportWidth / Math.Max(1, preferredItemSize.Width));
            ItemSize = new Size(Math.Max(1, viewportWidth / Columns), Math.Max(1, preferredItemSize.Height));
        }

        internal Rectangle GetItemRectangle(int index, int scrollY) => new Rectangle(
            index % Columns * ItemSize.Width, index / Columns * ItemSize.Height - scrollY,
            ItemSize.Width, ItemSize.Height);

        internal int IndexFromPoint(Point point, int scrollY)
        {
            if (point.X < 0 || point.Y < 0 || point.X >= Columns * ItemSize.Width) return -1;
            int index = (point.Y + scrollY) / ItemSize.Height * Columns + point.X / ItemSize.Width;
            return index >= 0 && index < Count ? index : -1;
        }

        internal IEnumerable<int> VisibleIndices(int scrollY, int viewportHeight, int extraRows = 0)
        {
            if (viewportHeight <= 0) yield break;
            int firstRow = Math.Max(0, scrollY / ItemSize.Height - extraRows);
            int lastRow = (scrollY + viewportHeight - 1) / ItemSize.Height + extraRows;
            int end = Math.Min(Count, (lastRow + 1) * Columns);
            for (int index = firstRow * Columns; index < end; index++) yield return index;
        }

        internal int DropGap(Point point, int scrollY, out int itemIndex, out bool after)
        {
            if (Count == 0) { itemIndex = -1; after = false; return 0; }
            int row = Math.Max(0, point.Y + scrollY) / ItemSize.Height;
            int column = Math.Max(0, Math.Min(Columns - 1, point.X / ItemSize.Width));
            int candidate = row * Columns + column;
            itemIndex = Math.Min(Count - 1, candidate);
            after = candidate >= Count || point.X >= GetItemRectangle(itemIndex, scrollY).Left + ItemSize.Width / 2;
            return itemIndex + (after ? 1 : 0);
        }
    }

    internal sealed class PageThumbnailSelection
    {
        private readonly SortedSet<int> selected = new SortedSet<int>();
        private int anchor = -1;
        internal int Count { get; private set; }
        internal int FocusIndex { get; private set; } = -1;
        internal IReadOnlyCollection<int> Indices => selected;

        internal void Reset(int count, IEnumerable<int> indices)
        {
            Count = count;
            selected.Clear();
            selected.UnionWith(indices.Where(i => i >= 0 && i < count));
            FocusIndex = selected.Count > 0 ? selected.Min : count > 0 ? 0 : -1;
            anchor = FocusIndex;
        }

        internal void Focus(int index) { if (index >= 0 && index < Count) FocusIndex = index; }

        internal void Select(int index, bool control, bool shift)
        {
            if (index < 0 || index >= Count) return;
            if (shift)
            {
                if (anchor < 0) anchor = index;
                if (!control) selected.Clear();
                for (int i = Math.Min(anchor, index); i <= Math.Max(anchor, index); i++) selected.Add(i);
            }
            else
            {
                if (!control) selected.Clear();
                if (!selected.Add(index) && control) selected.Remove(index);
                anchor = index;
            }
            FocusIndex = index;
        }

        internal void SelectAll(bool value)
        {
            selected.Clear();
            if (value) selected.UnionWith(Enumerable.Range(0, Count));
        }
    }

    internal sealed class PageThumbnailView : Control
    {
        private readonly VScrollBar scrollBar;
        private readonly PageThumbnailSelection selection = new PageThumbnailSelection();
        private OrganizationPage[] items = Array.Empty<OrganizationPage>();
        private Size itemSize = new Size(180, 220);
        private PageThumbnailLayout layout;
        private bool updatingLayout;
        private int pendingSingleSelect = -1;
        private Point pendingPoint;
        private int wheelRemainder;

        internal event DrawItemEventHandler DrawItem;
        internal event EventHandler SelectedIndexChanged;
        internal event EventHandler ViewportChanged;
        internal Func<int, string> ItemText { get; set; }
        internal IReadOnlyList<OrganizationPage> Items => items;
        internal IReadOnlyCollection<int> SelectedIndices => selection.Indices;
        internal IEnumerable<OrganizationPage> SelectedItems => selection.Indices.Select(i => items[i]);
        internal Rectangle ViewportRectangle => new Rectangle(0, 0, Math.Max(1, ClientSize.Width - scrollBar.Width), Math.Max(0, ClientSize.Height));
        internal IEnumerable<int> VisibleIndices(int extraRows = 0) => layout.VisibleIndices(scrollBar.Value, ViewportRectangle.Height, extraRows);

        internal PageThumbnailView()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.Selectable, true);
            TabStop = true;
            AccessibleRole = AccessibleRole.List;
            scrollBar = new VScrollBar { TabStop = false };
            scrollBar.ValueChanged += (_, __) => { if (!updatingLayout) { Invalidate(); ViewportChanged?.Invoke(this, EventArgs.Empty); } };
            Controls.Add(scrollBar);
            UpdateLayout();
        }

        internal void SetItems(IEnumerable<OrganizationPage> pages, IEnumerable<int> selected)
        {
            items = pages.ToArray();
            selection.Reset(items.Length, selected);
            pendingSingleSelect = -1;
            UpdateLayout();
            Invalidate();
            SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
        }

        internal void SetItemSize(Size size) { itemSize = size; UpdateLayout(); }
        internal Rectangle GetItemRectangle(int index) => layout.GetItemRectangle(index, scrollBar.Value);
        internal int IndexFromPoint(Point point) => ViewportRectangle.Contains(point) ? layout.IndexFromPoint(point, scrollBar.Value) : -1;
        internal void SelectAll(bool value) => ChangeSelection(() => selection.SelectAll(value));

        internal int GetDropGap(Point point, out Rectangle marker)
        {
            int gap = layout.DropGap(point, scrollBar.Value, out int index, out bool after);
            marker = index < 0 ? Rectangle.Empty : PageThumbnailDragFeedback.GetMarkerBounds(GetItemRectangle(index), after, DeviceDpi);
            return gap;
        }

        internal void ScrollRows(int direction) => SetScrollPosition(scrollBar.Value + direction * layout.ItemSize.Height);

        internal void EnsureVisible(int index)
        {
            if (index < 0 || index >= items.Length) return;
            var bounds = GetItemRectangle(index);
            if (bounds.Top < 0) SetScrollPosition(scrollBar.Value + bounds.Top);
            else if (bounds.Bottom > ViewportRectangle.Height)
                SetScrollPosition(scrollBar.Value + Math.Min(bounds.Top, bounds.Bottom - ViewportRectangle.Height));
        }

        private void SetScrollPosition(int value) => scrollBar.Value = Math.Max(0,
            Math.Min(Math.Max(0, layout.ContentHeight - ViewportRectangle.Height), value));

        private void UpdateLayout()
        {
            if (scrollBar == null || updatingLayout) return;
            updatingLayout = true;
            try
            {
                int barWidth = Math.Max(12, (int)Math.Round(17 * DeviceDpi / 96f));
                scrollBar.Bounds = new Rectangle(Math.Max(0, ClientSize.Width - barWidth), 0, barWidth, Math.Max(0, ClientSize.Height));
                layout = new PageThumbnailLayout(items.Length, ViewportRectangle.Width, itemSize);
                int value = Math.Min(scrollBar.Value, Math.Max(0, layout.ContentHeight - ViewportRectangle.Height));
                scrollBar.Value = value;
                scrollBar.Maximum = Math.Max(0, layout.ContentHeight - 1);
                scrollBar.LargeChange = Math.Max(1, ViewportRectangle.Height);
                scrollBar.SmallChange = Math.Max(1, layout.ItemSize.Height / 3);
                scrollBar.Enabled = layout.ContentHeight > ViewportRectangle.Height;
                scrollBar.Value = value;
            }
            finally { updatingLayout = false; }
            Invalidate();
            ViewportChanged?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnSizeChanged(EventArgs e) { base.OnSizeChanged(e); UpdateLayout(); }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (layout == null) return;
            var state = e.Graphics.Save();
            try
            {
                e.Graphics.SetClip(ViewportRectangle, System.Drawing.Drawing2D.CombineMode.Intersect);
                foreach (int index in VisibleIndices())
                {
                    var bounds = GetItemRectangle(index);
                    if (!bounds.IntersectsWith(e.ClipRectangle)) continue;
                    DrawItemState flags = selection.Indices.Contains(index) ? DrawItemState.Selected : DrawItemState.None;
                    if (Focused && index == selection.FocusIndex) flags |= DrawItemState.Focus;
                    DrawItem?.Invoke(this, new DrawItemEventArgs(e.Graphics, Font, bounds, index, flags, ForeColor, BackColor));
                }
                base.OnPaint(e);
            }
            finally { e.Graphics.Restore(state); }
        }

        private void ChangeSelection(Action change)
        {
            var previous = new HashSet<int>(selection.Indices);
            int oldFocus = selection.FocusIndex;
            change();
            var changed = new HashSet<int>(previous);
            changed.SymmetricExceptWith(selection.Indices);
            foreach (int index in changed.Concat(new[] { oldFocus, selection.FocusIndex }).Distinct())
            {
                if (index < 0 || index >= items.Length) continue;
                var visible = Rectangle.Intersect(GetItemRectangle(index), ViewportRectangle);
                if (!visible.IsEmpty) Invalidate(visible);
            }
            if (!previous.SetEquals(selection.Indices)) SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            Focus();
            if (e.Button == MouseButtons.Left)
            {
                int index = IndexFromPoint(e.Location);
                bool control = (ModifierKeys & Keys.Control) != 0, shift = (ModifierKeys & Keys.Shift) != 0;
                if (index >= 0)
                {
                    if (!control && !shift && selection.Indices.Count > 1 && selection.Indices.Contains(index))
                    {
                        pendingSingleSelect = index;
                        pendingPoint = e.Location;
                        ChangeSelection(() => selection.Focus(index));
                    }
                    else ChangeSelection(() => selection.Select(index, control, shift));
                }
                else if (!control && !shift) SelectAll(false);
                Capture = true;
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (pendingSingleSelect >= 0 && !new Rectangle(pendingPoint.X - SystemInformation.DragSize.Width / 2,
                pendingPoint.Y - SystemInformation.DragSize.Height / 2, SystemInformation.DragSize.Width, SystemInformation.DragSize.Height).Contains(e.Location))
                pendingSingleSelect = -1;
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            int index = pendingSingleSelect;
            pendingSingleSelect = -1;
            if (e.Button == MouseButtons.Left)
            {
                if (index >= 0) ChangeSelection(() => selection.Select(index, false, false));
                Capture = false;
            }
            base.OnMouseUp(e);
        }

        protected override void OnMouseCaptureChanged(EventArgs e)
        {
            if (!Capture) pendingSingleSelect = -1;
            base.OnMouseCaptureChanged(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            wheelRemainder += e.Delta;
            int steps = wheelRemainder / SystemInformation.MouseWheelScrollDelta;
            wheelRemainder %= SystemInformation.MouseWheelScrollDelta;
            int lines = SystemInformation.MouseWheelScrollLines;
            int distance = lines < 0 ? Math.Max(1, ViewportRectangle.Height) : lines * scrollBar.SmallChange;
            SetScrollPosition(scrollBar.Value - steps * distance);
            base.OnMouseWheel(e);
        }

        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData & Keys.KeyCode)
            {
                case Keys.Left: case Keys.Right: case Keys.Up: case Keys.Down:
                case Keys.Home: case Keys.End: case Keys.PageUp: case Keys.PageDown: return true;
                default: return base.IsInputKey(keyData);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A) { SelectAll(true); e.Handled = e.SuppressKeyPress = true; return; }
            int focus = Math.Max(0, selection.FocusIndex), target = focus;
            int page = Math.Max(1, ViewportRectangle.Height / layout.ItemSize.Height) * layout.Columns;
            switch (e.KeyCode)
            {
                case Keys.Left: target--; break;
                case Keys.Right: target++; break;
                case Keys.Up: target -= layout.Columns; break;
                case Keys.Down: target += layout.Columns; break;
                case Keys.Home: target = e.Control ? 0 : focus / layout.Columns * layout.Columns; break;
                case Keys.End: target = e.Control ? items.Length - 1 : focus / layout.Columns * layout.Columns + layout.Columns - 1; break;
                case Keys.PageUp: target -= page; break;
                case Keys.PageDown: target += page; break;
                case Keys.Space: ChangeSelection(() => selection.Select(focus, e.Control, e.Shift)); e.Handled = e.SuppressKeyPress = true; base.OnKeyDown(e); return;
                default: base.OnKeyDown(e); return;
            }
            target = Math.Max(0, Math.Min(items.Length - 1, target));
            ChangeSelection(() => { if (e.Control && !e.Shift) selection.Focus(target); else selection.Select(target, e.Control, e.Shift); });
            EnsureVisible(target);
            e.Handled = e.SuppressKeyPress = true;
            base.OnKeyDown(e);
        }

        protected override void OnGotFocus(EventArgs e) { base.OnGotFocus(e); Invalidate(); }
        protected override void OnLostFocus(EventArgs e) { base.OnLostFocus(e); Invalidate(); }

        protected override AccessibleObject CreateAccessibilityInstance() => new ThumbnailAccessibleObject(this);

        private sealed class ThumbnailAccessibleObject : ControlAccessibleObject
        {
            private readonly PageThumbnailView view;
            internal ThumbnailAccessibleObject(PageThumbnailView view) : base(view) { this.view = view; }
            public override int GetChildCount() => view.items.Length;
            public override AccessibleObject GetChild(int index) => index >= 0 && index < view.items.Length ? new ThumbnailItemAccessibleObject(view, index, this) : null;
        }

        private sealed class ThumbnailItemAccessibleObject : AccessibleObject
        {
            private readonly PageThumbnailView view;
            private readonly int index;
            private readonly AccessibleObject parent;
            private bool IsValid => !view.IsDisposed && index >= 0 && index < view.items.Length;
            internal ThumbnailItemAccessibleObject(PageThumbnailView view, int index, AccessibleObject parent)
            { this.view = view; this.index = index; this.parent = parent; }
            public override AccessibleObject Parent => parent;
            public override string Name => IsValid ? view.ItemText?.Invoke(index) ?? (index + 1).ToString() : string.Empty;
            public override AccessibleRole Role => AccessibleRole.ListItem;
            public override Rectangle Bounds => IsValid && view.IsHandleCreated && view.GetItemRectangle(index).IntersectsWith(view.ViewportRectangle)
                ? view.RectangleToScreen(Rectangle.Intersect(view.GetItemRectangle(index), view.ViewportRectangle)) : Rectangle.Empty;
            public override AccessibleStates State => !IsValid ? AccessibleStates.Unavailable : AccessibleStates.Selectable | AccessibleStates.Focusable |
                (view.selection.Indices.Contains(index) ? AccessibleStates.Selected : 0) |
                (view.Focused && view.selection.FocusIndex == index ? AccessibleStates.Focused : 0) |
                (view.GetItemRectangle(index).IntersectsWith(view.ViewportRectangle) ? 0 : AccessibleStates.Offscreen);
            public override void DoDefaultAction()
            {
                if (!IsValid) return;
                view.Focus();
                view.ChangeSelection(() => view.selection.Select(index, false, false));
                view.EnsureVisible(index);
            }
        }
    }
}
