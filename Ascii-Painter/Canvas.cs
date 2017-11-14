using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Ascii_Painter
{

    public enum CanvasTool
    {
        None = -1,
        Select,
        Freetype,
        Dragdrop,
        Brush,
        Line,
        Rectangle,
        Circle,
    }

    public delegate void CanvasSelectionChanged(Object sender, EventArgs args);

    public delegate void CanvasToolChanged(Object sender, EventArgs args);

    public partial class Canvas : UserControl
    {

        public CanvasTool Tool { get => _tool; set { if (_tool != value) { _tool = value; if (value == ToolFallback) ToolFallback = CanvasTool.None; if (CausesValidation) Invalidate(); ToolChanged?.Invoke(this, EventArgs.Empty); } } }

        public CanvasTool ToolFallback { get; set; } = CanvasTool.None;

        public char ToolArt { get; set; } = '\0';

        /// <summary>
        /// A 2D array flattened as row by row array.
        /// </summary>
        private char[] characters;

        private LinkedList<CanvasState> undostack = new LinkedList<CanvasState>();

        private Size _imgsize = new Size(5, 5);

        private CanvasTool _tool;

        private Rectangle _selection;

        private Point _seldown;

        private Point _drgdown;

        private Point _cursor;

        private char[] _drwshot;

        private bool _gridlines = true;

        public Color BorderColor { get; set; } = Color.Gray;

        public bool Gridlines { get => _gridlines; set { _gridlines = value; if (CausesValidation) Invalidate(); } }

        public Size BlockSize { get
            {
                var s = TextRenderer.MeasureText("\0", Font, Size, TextFormatFlags.NoClipping | TextFormatFlags.NoPadding);
                return new Size(s.Width, s.Height);
            }
        }//new Size(Font.Height, Font.Height);

        public Size ImageSize { get => _imgsize; set { if (value != _imgsize) { _imgsize = value; characters = new char[value.Width * value.Height]; if (CausesValidation) Invalidate(); undostack.Clear(); Size = GetPreferredSize(Size); } } }

        public Rectangle Selection { get => _selection; set { if (value != _selection) { _selection = Rectangle.Intersect(value, new Rectangle(Point.Empty, ImageSize)); _selection.Size = new Size(Math.Max(_selection.Width, 1), Math.Max(_selection.Height, 1)); SelectionCursor = _selection.Location; if (CausesValidation) Invalidate(); SelectionChanged?.Invoke(this, EventArgs.Empty); } } }

        public Point SelectionCursor { get => _cursor; set { if (value != _cursor) { _cursor = value; if (CausesValidation) { Invalidate(); } } } }

        public Color SelectionColor { get; set; } = SystemColors.Highlight;

        public Color SelectionCursorColor { get; set; } = SystemColors.Info;

        public event CanvasSelectionChanged SelectionChanged;

        public event CanvasToolChanged ToolChanged;

        public override string Text
        {
            get
            {
                var s = new StringBuilder(characters.Length);
                for (int i = 0; i < characters.Length; i++)
                {
                    var c = characters[i];
                    s.Append(Utility.IsBlank(c) ? ' ' : c);
                    if (i > 0 && (i % _imgsize.Width) == 0)
                        s.AppendLine();
                }
                return s.ToString();
            }
            set
            {
                int seek = 0;
                var text = value.Replace("\r", string.Empty);
                for (int i = 0; i < value.Length; i++)
                {
                    if (text[i] == '\n')
                    {
                        var dest = seek / _imgsize.Width + _imgsize.Width;
                        while (seek < dest)
                            characters[seek++] = ' ';
                    }
                    else
                        characters[seek++] = text[i];
                }
                if (CausesValidation)
                    Invalidate();
            }
        }

        public Canvas()
        {
            InitializeComponent();

            characters = new char[ImageSize.Width * ImageSize.Height];
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            var g = e.Graphics;
            g.Clear(BackColor);


            // Borders
            if (_gridlines)
            {
                int x = 0, y = 0, i = 0, j = 0;
                int m = ImageSize.Width, n = ImageSize.Height;
                int u = BlockSize.Width, v = BlockSize.Height;
                //int p = Selection.X, q = Selection.Y, r = Selection.Width, s = Selection.Height;
                int w = Math.Min(u * m, Width), h = Math.Min(n * v, Height);
                var bc = new Pen(new SolidBrush(BorderColor));

                while (x <= w && i++ <= m)
                {
                    g.DrawLine(bc, new Point(x, 0), new Point(x, h));
                    x += u;
                }

                while (y <= h && j++ <= n)
                {
                    g.DrawLine(bc, new Point(0, y), new Point(w, y));
                    y += v;
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var g = e.Graphics;
            int m = ImageSize.Width, n = ImageSize.Height;
            int u = BlockSize.Width, v = BlockSize.Height;
            int a = SelectionCursor.X, b = SelectionCursor.Y;
            int p = Selection.X, q = Selection.Y, r = Selection.Width, s = Selection.Height;
            //int w = Math.Min(u * m, Width), h = Math.Min(n * v, Height);
            var tc = new SolidBrush(SelectionColor);
            var cc = new SolidBrush(SelectionCursorColor);
            var fc = new SolidBrush(ForeColor);

            // Selection
            g.FillRectangle(tc, new Rectangle(p * u, q * v, r * u, s * v));

            // Cursor
            if (Tool == CanvasTool.Freetype)
                g.FillRectangle(cc, new Rectangle(a * u, b * v, u, v));

            for (int z = 0; z < characters.Length; z++)
            {
                var c = characters[z];
                if (!Utility.IsBlank(c))
                {
                    var zm = Math.DivRem(z, m, out var zr);
                    g.DrawString(c < '\x256' ? asciis[c] : new string(c, 1), Font, fc,
                     new PointF(zr * u, zm * v));
                    //TextRenderer.DrawText(g, c < '\x256' ? asciis[c] : new string(c, 1)
                    //    , Font, new Point(zr * u, zm * v), ForeColor, TextFormatFlags.NoClipping | TextFormatFlags.NoPadding);

                }
            }
        }

        static string[] asciis;

        static Canvas()
        {
            asciis = new string[256];
            for (int i = 0; i <= 255; i++)
                asciis[i] = new string((char)i, 1);
        }

        static Rectangle Flexible(Rectangle r)
        {
            if (r.Width < 0)
            {
                r.X += r.Width - 1;
                r.Width *= -1;
                r.Width++;
            }
            if (r.Height < 0)
            {
                r.Y += r.Height - 1;
                r.Height *= -1;
                r.Height++;
            }
            r.Width++;
            r.Height++;
            return r;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button != MouseButtons.Left) return;
            int u = BlockSize.Width, v = BlockSize.Height;

            switch (Tool)
            {
                case CanvasTool.Select:
                    break;
                case CanvasTool.Freetype:
                    break;
                case CanvasTool.Dragdrop:
                    _selection.Location = _drgdown;
                    MoveSelected(new Point((e.X - _seldown.X + _drgdown.X * u) / u, (e.Y - _seldown.Y + _drgdown.Y * v) / v));
                    if (ToolFallback == CanvasTool.Select)
                    {
                        Tool = CanvasTool.Select;
                        Cursor = Cursors.Default;
                    }
                    break;
                case CanvasTool.Brush:
                    break;
                case CanvasTool.Line:
                    break;
                case CanvasTool.Rectangle:
                    break;
                case CanvasTool.Circle:
                    break;
                default:
                    break;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.Button != MouseButtons.Left) return;
            int u = BlockSize.Width, v = BlockSize.Height;

            switch (Tool)
            {
                case CanvasTool.Select:
                    Selection = Flexible(new Rectangle(_seldown.X / u, _seldown.Y / v,
                        (e.X - _seldown.X) / u, (e.Y - _seldown.Y) / v));
                    break;
                case CanvasTool.Freetype:
                    break;
                case CanvasTool.Dragdrop:
                    var sel = Selection;
                    sel.Location = new Point((e.X - _seldown.X + _drgdown.X * u) / u, (e.Y - _seldown.Y + _drgdown.Y * v) / v);
                    Selection = sel;
                    break;
                case CanvasTool.Brush:
                    Selection = new Rectangle(e.X / u, e.Y / v, 1, 1);
                    CharacterAt(Selection.Location, ToolArt == '\0' ? ' ' : ToolArt);
                    break;
                case CanvasTool.Line:
                case CanvasTool.Rectangle:
                case CanvasTool.Circle:
                    Selection = Flexible(new Rectangle(_seldown.X, _seldown.Y,
                            (e.X / u - _seldown.X), (e.Y / v - _seldown.Y)));
                    Array.Copy(_drwshot, characters, characters.Length);
                    switch (Tool)
                    {
                        case CanvasTool.Line:
                            DrawLine(new Point(e.X / u, e.Y / v), _seldown);
                            break;
                        case CanvasTool.Rectangle:
                            DrawRectangle(new Point(e.X / u, e.Y / v), _seldown);
                            break;
                        case CanvasTool.Circle:
                            DrawEllipse(new Point(e.X / u, e.Y / v), _seldown);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button != MouseButtons.Left) return;
            int u = BlockSize.Width, v = BlockSize.Height;

            if (ModifierKeys == Keys.Control)
            {
                switch (Tool)
                {
                    case CanvasTool.Select:
                        Tool = CanvasTool.Dragdrop;
                        ToolFallback = CanvasTool.Select;
                        Cursor = Cursors.SizeAll;
                        break;
                    default:
                        return;
                }
            }

            switch (Tool)
            {
                case CanvasTool.Select:
                    _seldown = new Point(e.Location.X / u * u, e.Location.Y / v * v);
                    Selection = new Rectangle(_seldown.X / u, _seldown.Y / v, 1, 1);
                    break;
                case CanvasTool.Freetype:
                    if (ToolFallback == CanvasTool.Select)
                    {
                        Tool = CanvasTool.Select;
                        Cursor = Cursors.Default;
                    }
                    break;
                case CanvasTool.Dragdrop:
                    _seldown = new Point(e.Location.X / u * u, e.Location.Y / v * v);
                    _drgdown = Selection.Location;
                    RecordUndo();
                    break;
                case CanvasTool.Brush:
                    Selection = new Rectangle(e.X / u, e.Y / v, 1, 1);
                    RecordUndo();
                    break;
                case CanvasTool.Line:
                case CanvasTool.Circle:
                case CanvasTool.Rectangle:
                    _seldown = new Point(e.Location.X / u, e.Location.Y / v);
                    _drwshot = (char[])characters.Clone();
                    RecordUndo();
                    break;
                default:
                    break;
            }

        }

        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Up:
                case Keys.Left:
                case Keys.Right:
                case Keys.Down:
                    return true;
                default:
                    return base.IsInputKey(keyData);
            }
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Delete && Tool != CanvasTool.Freetype)
            {
                ClearSelected();
            }
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.A:
                        Selection = new Rectangle(Point.Empty, ImageSize);
                        break;
                    case Keys.V:
                        Paste(Clipboard.GetText(TextDataFormat.UnicodeText), e.Shift);
                        break;
                    case Keys.C:
                        Clipboard.SetText(Copy(), TextDataFormat.UnicodeText);
                        break;
                    case Keys.Z:
                        Undo();
                        break;
                    default:
                        return;
                }
                e.Handled = true;
            }

            if (e.Modifiers == Keys.None | e.Modifiers == Keys.Shift)
            {
                if (Tool == CanvasTool.Select)
                {
                    if (Utility.KeyCodeToUnicode(e.KeyCode).Length >= 1)
                    {
                        Tool = CanvasTool.Freetype;
                        ToolFallback = CanvasTool.Select;
                        Cursor = Cursors.IBeam;
                        RecordUndo();
                    }
                }

                if (Tool == CanvasTool.Freetype)
                {
                    switch (e.KeyCode)
                    {
                        case Keys.Enter:
                            if (ToolFallback == CanvasTool.Select && e.Modifiers == Keys.None)
                            {
                                Tool = CanvasTool.Select;
                                Cursor = Cursors.Default;
                            }
                            else
                                GoToNextLine();
                            break;
                        case Keys.Back:
                            Backspace(e.Modifiers == Keys.Shift);
                            break;
                        case Keys.Delete:
                            Delete(e.Modifiers != Keys.Shift);
                            break;
                        case Keys.Insert:
                            Insert(e.Modifiers == Keys.Shift);
                            break;
                        case Keys.Up:
                            GoToUp();
                            break;
                        case Keys.Down:
                            GoToDown();
                            break;
                        case Keys.Right:
                            GoToRight();
                            break;
                        case Keys.Left:
                            GoToLeft();
                            break;
                        default:
                            var s = Utility.KeyCodeToUnicode(e.KeyCode);
                            if (s.Length >= 1)
                                Append(s[0]);
                            break;
                    }

                }
            }
        
           
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            return new Size(ImageSize.Width * BlockSize.Width, ImageSize.Height * BlockSize.Height);
        }
    }
}
