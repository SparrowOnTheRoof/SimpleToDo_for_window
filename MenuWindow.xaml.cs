using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SimpleToDo
{
    /// <summary>
    /// Interaction logic for MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        public MenuWindow()
        {
	  InitializeComponent();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
	  if (e.ChangedButton == MouseButton.Left)
	      this.DragMove();
        }

        private void ResetTaskBt_Click(object sender, RoutedEventArgs e)
        {
	  SaveLoadManager.ResetTask();
        }

        private void ResetMeMoBt_Click(object sender, RoutedEventArgs e)
        {
	  SaveLoadManager.ResetMemo();
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
	  Close();
        }
    }
}
