using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RegExpose.UI
{
    public class RegExposeRichTextBox : RichTextBox
    {
        private readonly Stack<UndoState> _undoStack = new Stack<UndoState>();
        private readonly Stack<UndoState> _redoStack = new Stack<UndoState>();

        private string _previousText;
        private UndoState _currentUndoState;
        private bool _isUndoingOrRedoing;

        public new event PaintEventHandler Paint;

        protected override void OnTextChanged(EventArgs e)
        {
            if (_previousText == null)
            {
                _previousText = Text;
            }

            if (_previousText == Text)
            {
                return;
            }

            _previousText = Text;

            if (_isUndoingOrRedoing)
            {
                return;
            }

            _undoStack.Push(_currentUndoState);
            _redoStack.Clear();

            base.OnTextChanged(e);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (_previousText == null)
            {
                _previousText = Text;
            }

            _currentUndoState = new UndoState
            {
                Value = Text,
                SelectionStart = SelectionStart,
                SelectionLength = SelectionLength
            };

            if ((keyData & Keys.Control) != Keys.Control
                || ((keyData & Keys.V) != Keys.V
                    && (keyData & Keys.Z) != Keys.Z
                    && (keyData & Keys.Y) != Keys.Y))
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }

            if ((keyData & Keys.V) == Keys.V)
            {
                // If the user has pasted via Ctrl + V, don't paste the text with its formatting, just the text itself.
                var data = Clipboard.GetDataObject();

                if (data == null || !data.GetDataPresent(DataFormats.Text))
                {
                    return base.ProcessCmdKey(ref msg, keyData);
                }

                var contents = Clipboard.GetText();
                Clipboard.SetData(DataFormats.Text, contents);
                Paste();

                return true;
            }

            // We need to handle the Undo/Redo stack ourselves, since the native RichTextBox Undo/Redo is kinda wonky.

            if ((keyData & Keys.Z) == Keys.Z)
            {
                if (_undoStack.Count > 0)
                {
                    _redoStack.Push(_currentUndoState);
                    var undo = _undoStack.Pop();
                    _isUndoingOrRedoing = true;
                    Text = undo.Value;
                    SelectionStart = undo.SelectionStart;
                    SelectionLength = undo.SelectionLength;
                    _isUndoingOrRedoing = false;
                }

                return true;
            }

            if ((keyData & Keys.Y) == Keys.Y)
            {
                if (_redoStack.Count > 0)
                {
                    _undoStack.Push(_currentUndoState);
                    var redo = _redoStack.Pop();
                    _isUndoingOrRedoing = true;
                    Text = redo.Value;
                    SelectionStart = redo.SelectionStart;
                    SelectionLength = redo.SelectionLength;
                    _isUndoingOrRedoing = false;
                }

                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg != 0x000F) // WM_PAINT
            {
                return;
            }

            using (var graphic = CreateGraphics())
            {
                OnPaint(new PaintEventArgs(graphic, ClientRectangle));
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var handler = Paint;
            if (handler != null)
            {
                handler(this, e);
            }

            base.OnPaint(e);
        }

        private class UndoState
        {
            public string Value { get; set; }
            public int SelectionStart { get; set; }
            public int SelectionLength { get; set; }
        }
    }
}