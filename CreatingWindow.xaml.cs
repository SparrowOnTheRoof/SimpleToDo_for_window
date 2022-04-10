using SimpleToDo.UserControls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SimpleToDo
{
    /// <summary>
    /// Interaction logic for CreatingWindow.xaml
    /// </summary>

    public partial class CreatingWindow : Window
    {
        private ContentTypes NowState;
        private ContentContainer tempSaved_editContainer = null;

        public CreatingWindow(ContentTypes ContentType)
        {
	  InitializeComponent();

	  if (ContentType == ContentTypes.Task)
	  {
	      SwitchTask();
	  }
	  else if (ContentType == ContentTypes.Memo)
	  {
	      SwitchMemo();
	  }
        }


        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
	  (Application.Current.MainWindow as MainWindow).RemoveStoredWindow(this);
	  Close();
        }
        private void btApply_Click(object sender, RoutedEventArgs e)
        {
	  if (txtContent.Text != "" && txtContent.Text != null)
	  {
	      if (NowState == ContentTypes.Task)
		(Application.Current.MainWindow as MainWindow).AddTask(txtContent.Text);
	      if (NowState == ContentTypes.Memo)
		(Application.Current.MainWindow as MainWindow).AddMemo(txtContent.Text);

	      (Application.Current.MainWindow as MainWindow).RemoveStoredWindow(this);
	      Close();
	  }
	  else
	      txtContent.Focus();
        }
        private void btEdit_Click(object sender, RoutedEventArgs e)
        {
	  tempSaved_editContainer.ContentText = txtContent.Text;
	  SaveLoadManager.Edit(tempSaved_editContainer, MainWindow.state);

	  (Application.Current.MainWindow as MainWindow).RemoveStoredWindow(this);
	  Close();
        }
        public void ChangeClickEvent_ApplyToEdit(ContentContainer contentContainer)
        {
	  btApply.Click -= btApply_Click;
	  btApply.Click += btEdit_Click;

	  tempSaved_editContainer = contentContainer;
        }


        private void btSwitchTask_Click(object sender, RoutedEventArgs e)
        {
	  SwitchTask();
        }
        private void SwitchTask()
        {
	  if (NowState == ContentTypes.Task) return;
	  NowState = ContentTypes.Task;

	  Button button1 = btSwitchTask;
	  button1.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3D5473"));
	  button1.Background = Brushes.White;

	  Button button2 = btSwitchMemo;
	  button2.Foreground = Brushes.White;
	  button2.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3D5473"));

	  return;
        }
        private void btSwitchMemo_Click(object sender, RoutedEventArgs e)
        {
	  SwitchMemo();
        }
        private void SwitchMemo()
        {
	  //if (NowState == ContentTypes.Memo) return;
	  NowState = ContentTypes.Memo;

	  Button button1 = btSwitchTask;
	  button1.Foreground = Brushes.White;
	  button1.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3D5473"));

	  Button button2 = btSwitchMemo;
	  button2.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3D5473"));
	  button2.Background = Brushes.White;

	  return;
        }


        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
	  if (e.ChangedButton == MouseButton.Left)
	      this.DragMove();
        }
        private void txtContent_KeyDown(object sender, KeyEventArgs e)
        {
	  if (e.Key == Key.Enter)
	  {
	      btApply.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
	      e.Handled = true;
	  }
	  else if (e.Key == Key.Escape)
	  {
	      btCancel.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
	      e.Handled = true;
	  }
        }

        private void Window_Activated(object sender, EventArgs e)
        {
	  Topmost = true;
	  txtContent.Focus();
        }
    }
}
