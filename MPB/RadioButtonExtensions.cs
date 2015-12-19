using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace MPB
{
    public static class RadioButtonExtensions
    {

        public static void ON(this RadioButton radioButton)
        {
            radioButton.InvokeIfNeeded(delegate
            {
                  //radioButton.CheckedChanged
                    radioButton.Checked = true;   
                }, InvocationMethod.Asynchronous);
           
        }

        public static void OFF(this RadioButton radioButton)
        {
            radioButton.InvokeIfNeeded(delegate
            {
                radioButton.Checked = false;
            }, InvocationMethod.Asynchronous);

        }
        //public static void INDETERMINATE(this RadioButton radioButton)
        //{
        //    radioButton.InvokeIfNeeded(delegate
        //    {               
        //        radioButton.CheckState = CheckState.Indeterminate;
        //    }, InvocationMethod.Asynchronous);

        //}
        public static void None(this RadioButton radioButton,bool flag)
        {
            radioButton.InvokeIfNeeded(delegate
            {
                if (flag)
                    radioButton.Enabled = false;
                else 
                    radioButton.Enabled = true;

            }, InvocationMethod.Asynchronous);

        }
      
    }

}
