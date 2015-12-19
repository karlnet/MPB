using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
namespace MPB
{
    public static class ListViewExtensions
    {
        
        public static void InvokeIfNeeded(this Control control,
           Action action, InvocationMethod invocationMethod)
        {
            if (control.InvokeRequired)
            {
                switch (invocationMethod)
                {
                    case InvocationMethod.Synchronous: control.Invoke(action); break;
                    case InvocationMethod.Asynchronous: control.BeginInvoke(action); break;
                    default: throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                action();
            }
        }

        public static int NumberOfRows(this ListView listView)
        {
            int numberOfRows = listView.Items.Count;
            return numberOfRows;
        }

        public static void AddRow(this ListView listView, string key,
           InvocationMethod invocationMethod,
           params string[] valuesForSecondColumnToLastColumn)
        {
            key.MustBeNonEmpty();
            if (invocationMethod < InvocationMethod.Synchronous ||
                invocationMethod > InvocationMethod.Asynchronous)
                throw new ArgumentOutOfRangeException("invocationMethod");
            ConfirmThatValuesForSecondColumnToLastColumnAreValid(
               listView, valuesForSecondColumnToLastColumn);

            ListViewItem newRow = new ListViewItem(key);
            // Name is the key used for listView.Items.ContainsKey()
            newRow.Name = key;
          
            if (valuesForSecondColumnToLastColumn != null)
            {
                newRow.SubItems.AddRange(valuesForSecondColumnToLastColumn);
            }
            listView.InvokeIfNeeded(delegate
            {
                listView.Items.Add(newRow);
            }, invocationMethod);
        }

        public static bool ContainsRow(this ListView listView, string key)
        {
            bool doesContainRow = false;
            listView.InvokeIfNeeded(() =>
            {
                if (listView.Items.ContainsKey(key))
                {
                    doesContainRow = true;
                }
            }, InvocationMethod.Synchronous);
            return doesContainRow;
        }

        public static ListViewItem.ListViewSubItemCollection GetRow(
           this ListView listView, string key)
        {
            ListViewItem.ListViewSubItemCollection listViewRow = null;
            listView.InvokeIfNeeded(() =>
            {
                if (!listView.Items.ContainsKey(key))
                    throw new InvalidOperationException("Row with key \"" + key + "\" does not exist");

                listViewRow = listView.Items[key].SubItems;
            }, InvocationMethod.Synchronous);
            Debug.Assert(listViewRow != null);
            return listViewRow;
        }

        public static void UpdateRow(this ListView listView, string key,
           InvocationMethod invocationMethod,
           params string[] newValuesForSecondColumnToLastColumn)
        {
            
            ConfirmThatValuesForSecondColumnToLastColumnAreValid(
               listView, newValuesForSecondColumnToLastColumn);

            listView.InvokeIfNeeded(delegate
            {
                if (!listView.Items.ContainsKey(key))
                    throw new InvalidOperationException("Row with key \"" + key + "\" does not exist");
                
                ListViewItem.ListViewSubItemCollection columns = listView.Items[key].SubItems;
                for (int i = 0; i < newValuesForSecondColumnToLastColumn.Length; i++)
                {
                    columns[i + 1].Text = newValuesForSecondColumnToLastColumn[i];
                }
                
            }, invocationMethod);
        }

        public static void RemoveRow(
           this ListView listView, string key, InvocationMethod invocationMethod)
        {
            listView.InvokeIfNeeded(delegate
            {
                if (!listView.Items.ContainsKey(key))
                    throw new InvalidOperationException(
                       "Row with key \"" + key + "\" does not exist");

                listView.Items.RemoveByKey(key);
            }, invocationMethod);
        }

        private static void ConfirmThatValuesForSecondColumnToLastColumnAreValid(
           ListView listView, string[] valuesForSecondColumnToLastColumn)
        {
            if (listView.Columns.Count > 1 &&
                valuesForSecondColumnToLastColumn == null)
                throw new ArgumentNullException(
                   "valuesForSecondColumnToLastColumn cannot be null when"
                   + " listView.Columns.Count is > 1");
            if (valuesForSecondColumnToLastColumn != null &&
               1 + valuesForSecondColumnToLastColumn.Length != listView.Columns.Count)
                throw new ArgumentOutOfRangeException(
                   "1 + valuesForSecondColumnToLastColumn.Length"
                   + " must equal listView.Columns.Count");
        }
        public static void DisableItems(this ListView listView, string key,int s)
        {
          
            listView.InvokeIfNeeded(() =>
            {
                if (listView.Items.ContainsKey(key))
                {
                  switch (s)
                  { 
                      case 1:
                        Color c = listView.Items[key].BackColor;
                        listView.Items[key].BackColor = Color.Green;
                       break;
                      case 2:
                       listView.Items[key].BackColor = Color.White;
                      break;
                     case 3:
                      listView.Items[key].BackColor = Color.Red;
                      break;
                    }
               
                }
            }, InvocationMethod.Synchronous);
           
        }

    }
}
