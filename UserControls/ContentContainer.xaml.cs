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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimpleToDo.UserControls
{
    /// <summary>
    /// Interaction logic for ContentContainer.xaml
    /// </summary>
    public partial class ContentContainer : UserControl
    {
        public ContentContainer()
        {
	  InitializeComponent();
        }

        public string ContentText
        {
	  get { return (string)GetValue(ContentTextProperty); }
	  set { SetValue(ContentTextProperty, value); }
        }

        public static readonly DependencyProperty ContentTextProperty = DependencyProperty.Register("ContentText", typeof(string), typeof(ContentContainer));

        private void btEdit_Click(object sender, RoutedEventArgs e)
        {
	  ContentTypes thisContentType = (this.Parent as Panel).Name == "spTask" ? ContentTypes.Task : ContentTypes.Memo;
	  CreatingWindow newWindow = new CreatingWindow(thisContentType);

	  // Case : Task Edit
	  if (thisContentType == ContentTypes.Task)
	  {
	      Button button = newWindow.FindName("btSwitchMemo") as Button;
	      Grid grid = button.Parent as Grid;
	      grid.Children.Remove(button);
	  }
	  // Case : Memo Edit
	  else
	  {
	      Button button = newWindow.FindName("btSwitchTask") as Button;
	      Grid grid = button.Parent as Grid;
	      grid.Children.Remove(button);

	      button = newWindow.FindName("btSwitchMemo") as Button;
	      button.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3D5473"));
	      button.Background = Brushes.White;
	  }
	  // Change Title
	  Label title = newWindow.FindName("title") as Label;
	  title.Content = "edit content";

	  // Change Apply Button
	  Button btApply = newWindow.FindName("btApply") as Button;
	  btApply.Content = "Edit";
	  newWindow.ChangeClickEvent_ApplyToEdit(this);

	  // Insert TextBox
	  TextBox textBox = (newWindow.FindName("txtContent") as TextBox);
	  textBox.Text = ContentText;
	  textBox.Select(textBox.Text.Length, 0);

	  // Show new Window
	  newWindow.Top = Application.Current.MainWindow.Top + (double)((Application.Current.MainWindow.Height - newWindow.Height) / 2);
	  newWindow.Left = Application.Current.MainWindow.Left + (double)((Application.Current.MainWindow.Width - newWindow.Width) / 2);
	  newWindow.Show();
        }
        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
	  SaveLoadManager.Delete(this, MainWindow.state);

	  Panel parentPanel = this.Parent as Panel;
	  parentPanel.Children.Remove(this);
        }
    }
}
