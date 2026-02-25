using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Theme = Ledger.UIHelper.UIGraphicsHelper;
using Ledger.BitUI;
namespace Ledger.ScrollBar
{
    /// <summary>
    /// Custom vertical scrollbar control
    /// </summary>
    public class CuoreScrollBar : Control
    {
        private int value;
        private int minimum;
        private int maximum = 100;
        private int largeChange = 10;
        private int smallChange = 1;

        private bool isDragging;
        private int dragStartY;
        private int dragStartValue;

        private Rectangle thumbRect;
        private bool thumbHovered;

        public event EventHandler ValueChanged;
        public event ScrollEventHandler Scroll;

        public CuoreScrollBar()
        {
            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);

            Width = 15;
            DoubleBuffered = true;
        }

        [Browsable(true)]
        public int Value
        {
            get => value;
            set
            {
                int newValue = Math.Max(minimum, Math.Min(maximum, value));
                if (this.value != newValue)
                {
                    this.value = newValue;
                    ValueChanged?.Invoke(this, EventArgs.Empty);
                    Invalidate();
                }
            }
        }

        [Browsable(true)]
        public int Minimum
        {
            get => minimum;
            set
            {
                minimum = value;
                if (this.value < minimum)
                    Value = minimum;
                Invalidate();
            }
        }

        [Browsable(true)]
        public int Maximum
        {
            get => maximum;
            set
            {
                maximum = value;
                if (this.value > maximum)
                    Value = maximum;
                Invalidate();
            }
        }

        [Browsable(true)]
        public int LargeChange
        {
            get => largeChange;
            set
            {
                largeChange = Math.Max(1, value);
                Invalidate();
            }
        }

        [Browsable(true)]
        public int SmallChange
        {
            get => smallChange;
            set
            {
                smallChange = Math.Max(1, value);
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Draw track
            using (SolidBrush trackBrush = new SolidBrush(Color.FromArgb(240, 240, 240)))
            {
                g.FillRectangle(trackBrush, ClientRectangle);
            }

            // Calculate thumb size and position
            int range = maximum - minimum;
            if (range <= 0) return;

            int thumbHeight = Math.Max(20, (int)((double)largeChange / range * Height));
            int trackHeight = Height - thumbHeight;
            int thumbY = trackHeight > 0
                ? (int)((double)(value - minimum) / range * trackHeight)
                : 0;

            thumbRect = new Rectangle(2, thumbY, Width - 4, thumbHeight);

            // Draw thumb
            Color thumbColor = isDragging
                ? Color.FromArgb(100, 100, 100)
                : thumbHovered
                    ? Color.FromArgb(150, 150, 150)
                    : Color.FromArgb(180, 180, 180);

            using (SolidBrush thumbBrush = new SolidBrush(thumbColor))
            using (GraphicsPath thumbPath = GetRoundedRect(thumbRect, 4))
            {
                g.FillPath(thumbBrush, thumbPath);
            }
        }

        private GraphicsPath GetRoundedRect(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;

            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();

            return path;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Left)
            {
                if (thumbRect.Contains(e.Location))
                {
                    isDragging = true;
                    dragStartY = e.Y;
                    dragStartValue = value;
                    Invalidate();
                }
                else
                {
                    // Click on track - page up/down
                    if (e.Y < thumbRect.Y)
                        Value -= largeChange;
                    else if (e.Y > thumbRect.Bottom)
                        Value += largeChange;
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            bool wasHovered = thumbHovered;
            thumbHovered = thumbRect.Contains(e.Location);

            if (thumbHovered != wasHovered)
                Invalidate();

            if (isDragging)
            {
                int deltaY = e.Y - dragStartY;
                int range = maximum - minimum;
                int trackHeight = Height - thumbRect.Height;

                if (trackHeight > 0)
                {
                    int deltaValue = (int)((double)deltaY / trackHeight * range);
                    Value = dragStartValue + deltaValue;
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (isDragging)
            {
                isDragging = false;
                Invalidate();
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (e.Delta > 0)
                Value -= smallChange;
            else if (e.Delta < 0)
                Value += smallChange;
        }
    }

    /// <summary>
    /// Owner-drawn grid/table control backed by a hidden DataGridView for binding/sorting.
    /// </summary>
    public class CustomDataGrid : ScrollableControl
    {
        private readonly DataGridView _hiddenGrid;

        private int _rowHeight = 25;
        private int _headerHeight = 30;

        private int _columnCount = 3;
        private int _visibleRows;

        private int _verticalScrollOffset;
        private int _horizontalScrollOffset;
        private int _columnWidth = 100;

        private int _hoveredRow = -1;
        private int _hoveredColumn = -1;

        private readonly Pen _borderPen;
        private Color _cellColor = Color.White;
        private Color _alternateCellColor = Color.LightGray;
        private Color _hoverCellColor = Color.Gray;
        private Color _selectedCellColor = Theme.PrimaryColor;
        private Color _borderColor = Color.Black;

        private Padding _rightColumnRounding;
        private Padding _leftColumnRounding;
        private Padding _middleColumnRounding = new Padding(0, 0, 0, 0);

        private Padding _topLeftCorner;
        private Padding _bottomLeftCorner;
        private Padding _topRightCorner;
        private Padding _bottomRightCorner;

        private int _cornerRadius = 8;

        private string[] _headers;

        private CuoreScrollBar _scrollBar;
        private bool _hasVerticalScrollbar;

        private Color _headerColor = Color.Gray;

        private readonly List<GridCellRef> _selectedCells = new List<GridCellRef>();

        public CustomDataGrid()
        {
            _hiddenGrid = new DataGridView
            {
                Visible = false
            };

            _hiddenGrid.DataSourceChanged += DataSourceChanged;
            _hiddenGrid.CellValueChanged += HiddenGrid_CellValueChanged;
            _hiddenGrid.SelectionChanged += HiddenGrid_SelectionChanged;
            _hiddenGrid.ColumnHeaderMouseClick += HiddenGrid_ColumnHeaderMouseClick;

            Controls.Add(_hiddenGrid);

            _borderPen = new Pen(_borderColor);
            DoubleBuffered = true;

            Rounding = _cornerRadius;
        }

        [Browsable(true)]
        [Category("CuoreUI")]
        [Description("Background color for regular cells.")]
        public Color Cell
        {
            get => _cellColor;
            set { _cellColor = value; Invalidate(); }
        }

        [Browsable(true)]
        [Category("CuoreUI")]
        [Description("Background color for alternating cells.")]
        public Color Cell2
        {
            get => _alternateCellColor;
            set { _alternateCellColor = value; Invalidate(); }
        }

        [Browsable(true)]
        [Category("CuoreUI")]
        [Description("Background color for hovered cells.")]
        public Color CellHover
        {
            get => _hoverCellColor;
            set { _hoverCellColor = value; Invalidate(); }
        }

        [Browsable(true)]
        [Category("CuoreUI")]
        [Description("Background color for selected cells.")]
        public Color CellSelect
        {
            get => _selectedCellColor;
            set { _selectedCellColor = value; Invalidate(); }
        }

        [Browsable(true)]
        [Category("CuoreUI")]
        [Description("Border color for the cells.")]
        public Color CellBorder
        {
            get => _borderColor;
            set
            {
                _borderColor = value;
                _borderPen.Color = _borderColor;
                Invalidate();
            }
        }

        [Browsable(true)]
        [Category("CuoreUI")]
        [Description("Color of the headers (top columns)")]
        public Color HeaderColor
        {
            get => _headerColor;
            set { _headerColor = value; Refresh(); }
        }

        [Browsable(true)]
        [Category("CuoreUI")]
        [Description("Corner radius of the cells that support it.")]
        public int Rounding
        {
            get => _cornerRadius;
            set
            {
                _cornerRadius = value;

                _leftColumnRounding = new Padding(value, 0, 0, 0);
                _rightColumnRounding = new Padding(0, value, 0, 0);

                _topLeftCorner = new Padding(0, 0, 0, 0);
                _bottomLeftCorner = new Padding(0, 0, value, 0);
                _topRightCorner = new Padding(0, 0, 0, 0);
                _bottomRightCorner = new Padding(0, 0, 0, value);

                Refresh();
            }
        }

        public object DataSource
        {
            get => _hiddenGrid.DataSource;
            set
            {
                _hiddenGrid.DataSource = value;
                Refresh();
            }
        }

        public void Sort(DataGridViewColumn column, ListSortDirection direction)
        {
            _hiddenGrid.Sort(column, direction);
            Refresh();
        }

        public DataGridViewSelectedCellCollection SelectedCells
        {
            get
            {
                _hiddenGrid.ClearSelection();

                foreach (var cell in _selectedCells)
                {
                    if (cell.RowId >= 0 && cell.RowId < _hiddenGrid.Rows.Count &&
                        cell.ColumnId >= 0 && cell.ColumnId < _hiddenGrid.Columns.Count)
                    {
                        _hiddenGrid.Rows[cell.RowId].Cells[cell.ColumnId].Selected = true;
                    }
                }

                return _hiddenGrid.SelectedCells;
            }
        }

        private void DataSourceChanged(object sender, EventArgs e)
        {
            if (_hiddenGrid.DataSource is DataTable table)
            {
                _columnCount = table.Columns.Count;

                var names = table.Columns
                    .Cast<DataColumn>()
                    .Select(c => c.ColumnName)
                    .ToArray();

                for (int i = 0; i < names.Length; i++)
                {
                    string name = names[i];
                    if (name.StartsWith("Column", StringComparison.Ordinal) &&
                        int.TryParse(name.Substring(6), out _))
                    {
                        names[i] = "";
                    }
                }

                _headers = names;
            }
            else if (_hiddenGrid.DataSource is IEnumerable enumerable)
            {
                var enumerator = enumerable.GetEnumerator();
                if (enumerator.MoveNext() && enumerator.Current != null)
                {
                    PropertyInfo[] props = enumerator.Current.GetType().GetProperties();
                    _columnCount = props.Length;
                    _headers = props.Select(p => p.Name).ToArray();
                }
            }

            Refresh();
        }

        private void HiddenGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e) => Refresh();
        private void HiddenGrid_SelectionChanged(object sender, EventArgs e) => Refresh();

        private void HiddenGrid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            _hiddenGrid.Sort(_hiddenGrid.Columns[e.ColumnIndex], ListSortDirection.Ascending);
            Refresh();
        }

        private int WidthConsideringScrollbar =>
            _scrollBar == null || !_hasVerticalScrollbar ? Width : Width - _scrollBar.Width;

        protected override void OnPaint(PaintEventArgs e)
        {
            if (_cornerRadius != Rounding)
                Rounding = Rounding;

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            DrawHeader(g);
            DrawRows(g);

            if (Controls.Count > 0 && Controls[0] is CuoreScrollBar sb)
            {
                sb.Visible = DesignMode;
                _scrollBar = sb;
                _hasVerticalScrollbar = true;
            }
            else
            {
                _scrollBar = null;
                _hasVerticalScrollbar = false;
            }

            base.OnPaint(e);
        }

        private void DrawHeader(Graphics g)
        {
            for (int col = 0; col < _columnCount; col++)
            {
                int cellWidth = (WidthConsideringScrollbar - 1) / _columnCount;

                Rectangle rect = new Rectangle(
                    col * cellWidth,
                    0,
                    cellWidth,
                    _headerHeight);

                using (GraphicsPath path = BitMapClass.RoundRect(rect, _cornerRadius))
                using (SolidBrush brush = new SolidBrush(HeaderColor))
                {
                    g.FillPath(brush, path);
                    g.DrawPath(Pens.Black, path);
                }

                string text = _headers == null || _headers.Length <= col
                    ? $"Header {col + 1}"
                    : _headers[col];

                g.DrawString(
                    text,
                    Font,
                    Brushes.White,
                    (RectangleF)rect,
                    new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                if (rect.Right > Width)
                    break;
            }
        }

        private void DrawRows(Graphics g)
        {
            _visibleRows = (Height - _headerHeight) / _rowHeight;

            for (int visibleRow = 0; visibleRow < _visibleRows; visibleRow++)
            {
                int dataRowIndex = visibleRow + _verticalScrollOffset;
                if (dataRowIndex >= _hiddenGrid.Rows.Count)
                    break;

                for (int col = 0; col < _columnCount; col++)
                {
                    int cellWidth = (WidthConsideringScrollbar - 1) / _columnCount;

                    Rectangle rect = new Rectangle(
                        col * cellWidth,
                        _headerHeight + visibleRow * _rowHeight,
                        cellWidth,
                        _rowHeight);

                    if (rect.Right > Width || rect.Bottom > Height)
                        continue;

                    bool isSelected = _selectedCells.Any(c => c.RowId == dataRowIndex && c.ColumnId == col);

                    Color fillColor = isSelected
                        ? CellSelect
                        : dataRowIndex == _hoveredRow && col == _hoveredColumn
                            ? CellHover
                            : dataRowIndex % 2 != 0 ? Cell2 : Cell;

                    using (SolidBrush fill = new SolidBrush(fillColor))
                    {
                        g.FillRectangle(fill, rect);
                        g.DrawRectangle(Pens.Black, rect);

                        string text = _hiddenGrid.Rows[dataRowIndex].Cells[col].Value?.ToString() ?? string.Empty;

                        using (SolidBrush textBrush = new SolidBrush(ForeColor))
                        {
                            g.DrawString(
                                text,
                                Font,
                                textBrush,
                                (RectangleF)rect,
                                new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                        }
                    }

                    if (rect.Right > Width)
                        break;
                }
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            int rowId = (e.Y - _headerHeight) / _rowHeight + _verticalScrollOffset;
            if (rowId < 0 || rowId >= _hiddenGrid.Rows.Count)
                return;

            var cell = new GridCellRef(rowId, _hoveredColumn);
            bool alreadySelected = _selectedCells.Contains(cell);

            if (ModifierKeys.HasFlag(Keys.Control))
            {
                if (alreadySelected)
                    _selectedCells.Remove(cell);
                else
                    _selectedCells.Add(cell);
            }
            else if (alreadySelected)
            {
                if (_selectedCells.Count >= 1)
                {
                    _selectedCells.Clear();
                    _selectedCells.Add(cell);
                }

                _selectedCells.Remove(cell);
            }
            else
            {
                _selectedCells.Clear();
                _selectedCells.Add(cell);
            }

            Refresh();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            int row = (e.Y - _headerHeight) / _rowHeight + _verticalScrollOffset;
            int col = e.X / ((WidthConsideringScrollbar - 1) / _columnCount);

            if (row >= 0 && row < _hiddenGrid.Rows.Count)
            {
                _hoveredRow = row;
                _hoveredColumn = col;
            }
            else
            {
                _hoveredRow = -1;
                _hoveredColumn = -1;
            }

            Refresh();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (ModifierKeys == Keys.Shift)
            {
                int pageColumns = WidthConsideringScrollbar / _columnWidth;

                if (e.Delta > 0 && _horizontalScrollOffset > 0)
                    _horizontalScrollOffset--;
                else if (e.Delta < 0 && _horizontalScrollOffset + pageColumns < _hiddenGrid.ColumnCount)
                    _horizontalScrollOffset++;
            }
            else
            {
                int pageRows = (Height - _headerHeight) / _rowHeight;

                if (e.Delta > 0 && _verticalScrollOffset > 0)
                    _verticalScrollOffset--;
                else if (e.Delta < 0 && _verticalScrollOffset + pageRows < _hiddenGrid.Rows.Count)
                    _verticalScrollOffset++;
            }

            Refresh();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _hoveredColumn = -1;
            _hoveredRow = -1;
            Refresh();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Refresh();
        }

        public class GridCellRef
        {
            public int RowId { get; set; }
            public int ColumnId { get; set; }

            public GridCellRef(int rowId, int columnId)
            {
                RowId = rowId;
                ColumnId = columnId;
            }

            public override bool Equals(object obj) =>
                obj is GridCellRef other && RowId == other.RowId && ColumnId == other.ColumnId;

            public override int GetHashCode() => (RowId, ColumnId).GetHashCode();
        }
    }
}