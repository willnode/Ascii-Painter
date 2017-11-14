using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ascii_Painter
{
    public partial class Canvas
    {
        public void Paste(string text, bool trim)
        {
            if (!Clipboard.ContainsText()) { SystemSounds.Beep.Play(); return; }
            text = text.Replace("\r", "");
            if (string.IsNullOrEmpty(text)) { ClearSelected(); return; }

            CausesValidation = false;
            if (IsCursorFree)
            {
                var sel = Selection;
                int w = 0, h = 0;
                for (int i = 0; i < text.Length; i++)
                {
                    if (text[i] == '\n')
                    {
                        sel.Width = Math.Max(sel.Width, w);
                        h++;
                        w = 0;
                    }
                    else
                    {
                        w++;
                    }

                }
                sel.Height = Math.Min(ImageSize.Height - sel.Y, h + 1);
                Selection = sel;
            }

            bool intrimming = trim;
            SelectionCursor = Selection.Location;
            for (int i = 0; i < text.Length; i++)
            {
                if (intrimming && Utility.IsBlank(text[i]))
                    continue;
                intrimming = false;
                if (Append(text[i]))
                {
                    if (text[i] != '\n')
                        while (i < text.Length && text[i] != '\n')
                            i++;
                    else
                        intrimming = trim;

                }
            }
            CausesValidation = true;
            Invalidate();
        }

        public string Copy()
        {
            var s = new StringBuilder(characters.Length);

            for (int y = Selection.Top; y < Selection.Bottom; y++)
            {
                for (int x = Selection.Left; x < Selection.Right; x++)
                {
                    s.Append(CharacterAt(new Point(x, y)));
                }
                if (y < Selection.Bottom - 1)
                    s.AppendLine();
            }
            return (s.ToString().Replace('\0', ' '));
        }

        /// <returns>Return if the cursor just moves down</returns>
        public bool Append(char c)
        {
            if (c == '\n')
            {
                GoToNextLine();
                return true;
            }
            var cur = SelectionCursor;
            CharacterAt(cur, c);
            return GoToRight();
        }

        public void GoToUp()
        {
            var cur = SelectionCursor;
            if (IsCursorFree)
            {
                if (cur.Y == 0)
                    cur.Y = ImageSize.Height;
            }
            else
            {
                if (cur.Y == Selection.Top)
                    cur.Y = Selection.Bottom;
            }
            cur.Y--;
            SelectionCursor = cur;
        }

        public void GoToDown()
        {
            var cur = SelectionCursor;
            cur.Y++;
            if (IsCursorFree)
            {
                if (cur.Y == ImageSize.Height)
                    cur.Y = 0;
            }
            else
            {
                if (cur.Y == Selection.Bottom)
                    cur.Y = Selection.Top;
            }
            SelectionCursor = cur;
        }

        public bool GoToRight()
        {
            var cur = SelectionCursor;
            cur.X++;
            if (IsCursorFree)
            {
                if (cur.X == ImageSize.Width)
                {
                    cur.X = 0;
                    cur.Y++;
                    if (cur.Y == ImageSize.Height)
                        cur.Y = 0;
                }
            }
            else
            {
                if (cur.X == Selection.Right)
                {
                    cur.X = Selection.Left;
                    cur.Y++;
                    if (cur.Y == Selection.Bottom)
                        cur.Y = Selection.Top;
                }
            }
            var r = cur.Y != SelectionCursor.Y;
            SelectionCursor = cur;
            return r;
        }

        public bool GoToLeft()
        {
            var cur = SelectionCursor;
            if (IsCursorFree)
            {
                if (cur.X == 0)
                {
                    if (cur.Y == 0)
                        cur.Y = ImageSize.Height;
                    cur.Y--;
                    cur.X = ImageSize.Width;
                }
            }
            else
            {
                if (cur.X == Selection.Left)
                {
                    if (cur.Y == Selection.Top)
                        cur.Y = Selection.Bottom;
                    cur.Y--;
                    cur.X = Selection.Right;
                }
            }
            cur.X--;
            var r = cur.Y != SelectionCursor.Y;
            SelectionCursor = cur;
            return r;
        }

        public void GoToNextLine()
        {
            var cur = SelectionCursor;
            if (IsCursorFree)
            {
                cur.X = 0;
                cur.Y = (cur.Y + 1) % ImageSize.Height;
            }
            else
            {
                cur.X = Selection.Left;
                cur.Y = (cur.Y - Selection.Y + 1) % Selection.Height + Selection.Y;
            }
            SelectionCursor = cur;
            if (CausesValidation)
                Invalidate();
        }

        public void ClearSelected()
        {
            CausesValidation = false;
            for (int x = Selection.Left; x < Selection.Right; x++)
            {
                for (int y = Selection.Top; y < Selection.Bottom; y++)
                {
                    CharacterAt(new Point(x, y), ' ');
                }
            }
            CausesValidation = true;
            Invalidate();
        }

        public void MoveSelected(Point dest)
        {
            if (Selection.Location == dest) return;
            CausesValidation = false;
            var txt = Copy();
            ClearSelected();
            _selection.Location = dest;
            Paste(txt, false);
        }

        public void Backspace(bool drag)
        {
            if (drag)
            {
                var c = CharactersAt(Selection);
                var cr = (SelectionCursor.X - Selection.X) + (SelectionCursor.Y - Selection.Y) * Selection.Width;
                Array.Copy(c, 0, c, 1, cr - 1);
                c[0] = ' ';
                CharactersAt(Selection, c);
            }
            else
            {
                GoToLeft();
                CharacterAt(SelectionCursor, ' ');
            }
        }

        public void Delete(bool drag)
        {
            if (drag)
            {
                var c = CharactersAt(Selection);
                var cr = (SelectionCursor.X - Selection.X) + (SelectionCursor.Y - Selection.Y) * Selection.Width;
                Array.Copy(c, cr + 1, c, cr, c.Length - cr - 1);
                c[c.Length - 1] = ' ';
                CharactersAt(Selection, c);
            }
            else
            {
                CharacterAt(SelectionCursor, ' ');
                GoToRight();
            }
        }

        public void Insert(bool leftside)
        {
            if (leftside)
            {
                var c = CharactersAt(Selection);
                var cr = (SelectionCursor.X - Selection.X) + (SelectionCursor.Y - Selection.Y) * Selection.Width;
                Array.Copy(c, 1, c, 0, cr - 1);
                c[c.Length - 1] = ' ';
                CharactersAt(Selection, c);
            }
            else
            {
                var c = CharactersAt(Selection);
                var cr = (SelectionCursor.X - Selection.X) + (SelectionCursor.Y - Selection.Y) * Selection.Width;
                Array.Copy(c, cr, c, cr + 1, c.Length - cr - 1);
                c[cr] = ' ';
                CharactersAt(Selection, c);
            }
        }

        public void Mirror(CanvasMirror op)
        {
            var c = CharactersAt(Selection);
            int w = Selection.Width, h = Selection.Height;
            char[] dest = new char[c.Length];
            switch (op)
            {
                case CanvasMirror.MirrorX:
                    for (int y = 0; y < h; y++)
                        for (int x = 0; x < w; x++)
                            dest[(w - x - 1) + y * w] = c[x + y * w];
                    break;

                case CanvasMirror.MirrorY:
                    for (int y = 0; y < h; y++)
                        for (int x = 0; x < w; x++)
                            dest[x + (h - y - 1) * w] = c[x + y * w];
                    break;

                case CanvasMirror.RotateCW:
                    Selection = new Rectangle(Selection.X, Selection.Y, h, w);
                    for (int y = 0; y < h; y++)
                        for (int x = 0; x < w; x++)
                            dest[x * h + (h - y - 1)] = c[x + y * w];
                    break;

                case CanvasMirror.RotateCCW:
                    Selection = new Rectangle(Selection.X, Selection.Y, h, w);
                    for (int y = 0; y < h; y++)
                        for (int x = 0; x < w; x++)
                            dest[(w - x - 1) * h + y] = c[x + y * w];
                    break;

                case CanvasMirror.TransposeXY:
                    Selection = new Rectangle(Selection.X, Selection.Y, h, w);
                    for (int y = 0; y < h; y++)
                        for (int x = 0; x < w; x++)
                            dest[y + x * w] = c[x + y * w];
                    break;

                case CanvasMirror.TransposeYX:
                    Selection = new Rectangle(Selection.X, Selection.Y, h, w);
                    for (int y = 0; y < h; y++)
                        for (int x = 0; x < w; x++)
                            dest[(w - x - 1) * w + (h - y - 1)] = c[x + y * w];
                    break;
            }

            CharactersAt(Selection, dest);
        }

        public char CharacterAt(Point point)
        {
            try
            {
                return characters[point.X + point.Y * ImageSize.Width];
            }
            catch (Exception)
            {
                return '\0';
            }
        }

        public void CharacterAt(int x, int y, char c)
        {
            CharacterAt(new Point(x, y), c);
        }

        public void CharacterAt(Point point, char c)
        {
            try
            {
                characters[point.X + point.Y * ImageSize.Width] = Utility.IsBlank(c) ? ' ' : c;

                if (CausesValidation)
                    Invalidate();
            }
            catch (Exception)
            {
            }
        }

        public char[] CharactersAt(Rectangle r)
        {
            var c = new char[r.Height * r.Width];
            for (int y = r.Top; y < r.Bottom; y++)
                Array.Copy(characters, y * ImageSize.Width + r.X, c, (y - r.Y) * r.Width, r.Width);
            return c;
        }

        public void CharactersAt(Rectangle r, char[] c)
        {
            if (c.Length != r.Width * r.Height)
                throw new ArgumentException("unmatching buffer count!");

            for (int y = r.Top; y < r.Bottom; y++)
                Array.Copy(c, (y - r.Y) * r.Width, characters, y * ImageSize.Width + r.X, r.Width);

            if (CausesValidation)
                Invalidate();
        }

        public bool IsCursorFree => Selection.Width <= 1 && Selection.Height <= 1;

        public void RecordUndo()
        {
            RecordUndo(MakeState(true));
        }

        public void RecordUndo(CanvasState state)
        {
            if (undostack.Count > 0 && undostack.Last.Equals(state))
                return;
            undostack.AddLast(state);
            if (undostack.Count > 20)
                undostack.RemoveFirst();
        }

        public void Undo()
        {
            if (undostack.Count > 0)
            {
                var txt = Text; var sel = Selection;
                ApplyState(undostack.Last.Value);
                undostack.RemoveLast();
                if (undostack.Count > 0 && Text == txt && Selection == sel)
                {
                    // do it again
                    Undo();
                }
            }
            else
                SystemSounds.Beep.Play();
        }

        CanvasState MakeState(bool whole)
        {
            return MakeState(whole ? new Rectangle(Point.Empty, ImageSize) : Selection);
        }

        CanvasState MakeState(Rectangle area)
        {
            var sel = Selection;
            _selection = area;
            var r = new CanvasState()
            {
                data = Copy(),
                area = area,
                selection = sel,
            };
            _selection = sel;
            return r;
        }

        void ApplyState(CanvasState state)
        {
            _selection = state.area;
            Paste(state.data, false);
            Selection = state.selection;
        }

        public void DrawLine(Point A, Point B)
        {
            if (A == B)
            {
                CharacterAt(A, ToolArt == '\0' ? Utility.GetLineChar() : ToolArt);
                return;
            }
            Point m, n;
            var D = new Point(A.X - B.X, A.Y - B.Y);
            var L = (int)Math.Sqrt(D.X * D.X + D.Y * D.Y) + 1;
            for (int i = 0; i <= L;)
            {
                m = new Point(A.X + ((B.X - A.X) * i) / L, A.Y + ((B.Y - A.Y) * i) / L);
                do
                {
                    i++;
                    n = new Point(A.X + ((B.X - A.X) * i) / L, A.Y + ((B.Y - A.Y) * i) / L);
                } while (m == n); // avoid duplicate write

                CharacterAt(m, ToolArt == '\0' ? Utility.GetLineChar(m, n) : ToolArt);
            }
        }

        public void DrawRectangle(Point A, Point B)
        {
            if (A == B)
            {
                CharacterAt(A, ToolArt == '\0' ? Utility.GetLineChar() : ToolArt);
                return;
            }

            DrawLine(new Point(A.X, A.Y), new Point(B.X, A.Y));
            DrawLine(new Point(A.X, B.Y), new Point(B.X, B.Y));
            DrawLine(new Point(A.X, A.Y), new Point(A.X, B.Y));
            DrawLine(new Point(B.X, A.Y), new Point(B.X, B.Y));
            var c = ToolArt == '\0' ? Utility.GetLineChar() : ToolArt;

            CharacterAt(A, c);
            CharacterAt(B, c);
            CharacterAt(new Point(A.X, B.Y), c);
            CharacterAt(new Point(B.X, A.Y), c);
        }

        public void DrawEllipse(Point A, Point B)
        {
            if (A == B)
            {
                CharacterAt(A, ToolArt == '\0' ? Utility.GetLineChar() : ToolArt);
                return;
            }

            int a = (A.X - B.X) / 2, b = (A.Y - B.Y) / 2, xc = (B.X), yc = (B.Y);

            if (a < 0)
            {
                xc += a * 2;
                a = Math.Abs(a);
            }

            if (b < 0)
            {
                yc += b * 2;
                b = Math.Abs(b);
            }

            xc += a;
            yc += b;

            var x = 0;
            var y = b;

            var a2 = a * a;
            var b2 = b * b;

            var crit1 = -(a2 / 4 + a % 2 + b2);
            var crit2 = -(b2 / 4 + b % 2 + a2);
            var crit3 = -(b2 / 4 + b % 2);

            var t = -a2 * y;
            var dxt = 2 * b2 * x;
            var dyt = -2 * a2 * y;

            var d2xt = 2 * b2;
            var d2yt = 2 * a2;

            Action incX = delegate () { x++; dxt += d2xt; t += dxt; };
            Action incY = delegate () { y--; dyt += d2yt; t += dyt; };

            while (y >= 0 && x <= a)
            {
                var character = ToolArt == '\0' ? Utility.GetLineChar() : ToolArt;
                CharacterAt(xc + x, yc + y, character);

                if (x != 0 || y != 0)
                {
                    CharacterAt(xc - x, yc - y, character);
                }

                if (x != 0 && y != 0)
                {
                    CharacterAt(xc + x, yc - y, character);
                    CharacterAt(xc - x, yc + y, character);
                }

                if (t + b2 * x <= crit1 || t + a2 * y <= crit3)
                {
                    incX();
                }
                else if (t - a2 * y > crit2)
                {
                    incY();
                }
                else
                {
                    incX();
                    incY();
                }
            }


            //var D = new Point(A.X - B.X, A.Y - B.Y);
            //var C = new Point((A.X + B.X) / 2, (A.Y + B.Y) / 2);
            //var L = Utility.EllipsePerimeter(D.X, D.Y);
            //Point m, n;
            //for (int i = 0; i <= L;)
            //{
            //    m = new Point(C.X + (int)(Math.Cos(i / L * 2 * Math.PI) * D.X / 2), C.Y + (int)(Math.Sin(i / L * 2 * Math.PI) * D.Y / 2));
            //    do
            //    {
            //        i++;
            //        n = new Point(C.X + (int)(Math.Cos(i / L * 2 * Math.PI) * D.X / 2), C.Y + (int)(Math.Sin(i / L * 2 * Math.PI) * D.Y / 2));
            //    } while (m == n && i <= L); // avoid duplicate write

            //    CharacterAt(m, ToolArt == '\0' ? Utility.GetLineChar(m, n) : ToolArt);
            //}
        }

    }

    public struct CanvasState : IEquatable<CanvasState>
    {
        public string data;
        public Rectangle area;
        public Rectangle selection;

        public bool Equals(CanvasState other)
        {
            return other.data == data && other.area == area && other.selection == selection;
        }
    }

    public enum CanvasMirror
    {
        MirrorX,
        MirrorY,
        RotateCW,
        RotateCCW,
        TransposeXY,
        TransposeYX,
    }
}
