using System.Drawing;
using System.Windows.Forms;

namespace RegExpose.UI
{
    public static class RichTextExtensions
    {
        public static void SetHighlight(this RichTextBox textBox, int startIndex, int length, Color color)
        {
            textBox.SelectionStart = startIndex;
            textBox.SelectionLength = length;
            textBox.SelectionBackColor = color;
        }

        public static void ClearHighlights(this RichTextBox textBox)
        {
            var selectionStart = textBox.SelectionStart;
            var selectionLength = textBox.SelectionLength;

            textBox.SelectionStart = 0;
            textBox.SelectionLength = textBox.TextLength;
            textBox.SelectionBackColor = textBox.BackColor;

            textBox.SelectionStart = selectionStart;
            textBox.SelectionLength = selectionLength;
        }
    }
}