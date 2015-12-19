using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;

namespace MPB
{
    public static class TextBoxExtensions
    {
        public static void AppendLine(this TextBox textBox)
        {
            textBox.InvokeIfNeeded(delegate
            {
                textBox.AppendText("\r\n");
            }, InvocationMethod.Asynchronous);
        }

        public static void AppendTextLine(this TextBox textBox, string message)
        {
            if (!String.IsNullOrEmpty(message))
            {
                textBox.InvokeIfNeeded(delegate
                {
                    textBox.AppendText(message);
                    textBox.AppendText("\r\n");
                }, InvocationMethod.Asynchronous);
            }
        }

        public static void AppendText(this TextBox textBox, string message)
        {
            if (!String.IsNullOrEmpty(message))
            {
                textBox.InvokeIfNeeded(delegate
                {
                    textBox.AppendText(message);
                }, InvocationMethod.Asynchronous);
            }
        }
        public static void WriteText(this TextBox textBox, string message)
        {
            if (!String.IsNullOrEmpty(message))
            {
                textBox.InvokeIfNeeded(delegate
                {
                    textBox.Text=message;
                }, InvocationMethod.Asynchronous);
            }
        }
    }
}
