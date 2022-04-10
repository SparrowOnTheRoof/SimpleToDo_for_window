using SimpleToDo.UserControls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace SimpleToDo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Windows.Forms.NotifyIcon notifyIcon;
        public static ContentTypes state = ContentTypes.Task;

        public MainWindow()
        {
	  InitializeComponent();

	  SettingTimer();

	  SaveLoadManager.Load();

	  SettingNotifyIcon();
        }

        // [NotifyIcon]
        private void SettingNotifyIcon()
        {
	  // NotifyIcon - contextMenu
	  System.Windows.Forms.ContextMenu contextMenu = new System.Windows.Forms.ContextMenu();
	  System.Windows.Forms.MenuItem itemSetting = new System.Windows.Forms.MenuItem();
	  itemSetting.Index = 0;
	  itemSetting.Text = "Setting";
	  itemSetting.Click += delegate { MessageBox.Show("Not Yet... Sorry :("); };
	  System.Windows.Forms.MenuItem itemClose = new System.Windows.Forms.MenuItem();
	  itemClose.Index = 0;
	  itemClose.Text = "Close";
	  itemClose.Click += delegate { Application.Current.Shutdown(); };

	  contextMenu.MenuItems.Add(itemSetting);
	  contextMenu.MenuItems.Add(itemClose);

	  // NotifyIcon Setting
	  // ref : <possemeeg.wordpress.com/2007/09/06/minimize-to-tray-icon-in-wpf/>
	  notifyIcon = new System.Windows.Forms.NotifyIcon();
	  Stream iconStream = Application.GetResourceStream(new Uri("pack://application:,,,/SimpleToDo;component/DesignWork/STM_Icon.ico")).Stream;
	  notifyIcon.Icon = new System.Drawing.Icon(iconStream);

	  notifyIcon.BalloonTipText = "SimpleToDo has been minimized. Click the tray icon to show.";
	  notifyIcon.BalloonTipTitle = "SimpleToDo";
	  notifyIcon.Text = "SimpleToDo";
	  notifyIcon.Click += new EventHandler(notifyIcon_Click);
	  notifyIcon.ContextMenu = contextMenu;

	  notifyIcon.Visible = true;
        }
        private WindowState storedWindowState = WindowState.Normal;
        private void Window_StateChanged(object sender, EventArgs e)
        {
	  if (WindowState == WindowState.Minimized)
	  {
	      Hide();
	      if (notifyIcon != null)
		if (SaveLoadManager.CheckFirstRun())
		    notifyIcon.ShowBalloonTip(2000);
	  }
	  else
	  {
	      storedWindowState = WindowState;
	  }
        }
        private void notifyIcon_Click(object sender, EventArgs e)
        {
	  Show();
	  WindowState = storedWindowState;
        }

        // [Timer]
        void SettingTimer()
        {
	  DispatcherTimer timer = new DispatcherTimer();
	  timer.Tick += timer_Tick;
	  timer.Start();
	  timer.Interval = TimeSpan.FromMinutes(1);
        }
        void timer_Tick(object sender, EventArgs e)
        {
	  txtDate.Text = DateTime.Now.ToString("MM - dd - yyyy") + " / " + DateTime.Now.ToString("ddd", new CultureInfo("en-US"));
        }

        // [Window Load] : Set App size and location
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
	  // Set WindowStartupLocation : Bottom-Right
	  var desktopWorkingArea = SystemParameters.WorkArea;
	  this.Left = desktopWorkingArea.Right - this.Width;
	  this.Top = desktopWorkingArea.Bottom - this.Height;

	  // Allow Resize mode only Vertical
	  this.MinWidth = this.Width;
	  this.MaxWidth = this.Width;
	  this.MinHeight = this.Height;
        }

        // [Buttons]
        private List<CreatingWindow> storedCreatingWindows = new List<CreatingWindow>();
        public void RemoveStoredWindow(CreatingWindow creatingWindow)
        {
	  storedCreatingWindows.Remove(creatingWindow);
	  if (storedCreatingWindows.Count > 0)
	      storedCreatingWindows.Last().Focus();
        }
        private void btAddTask_Click(object sender, RoutedEventArgs e)
        {
	  CreateCreatingWindow(ContentTypes.Task);

        }
        private void btAddMemo_Click(object sender, RoutedEventArgs e)
        {
	  CreateCreatingWindow(ContentTypes.Memo);
        }
        private void CreateCreatingWindow(ContentTypes contentTypes)
        {
	  CreatingWindow newWindow = new CreatingWindow(contentTypes);

	  if (storedCreatingWindows.Count == 0)
	  {
	      newWindow.Top = this.Top + (double)((this.Height - newWindow.Height) / 2);
	      newWindow.Left = this.Left + (double)((this.Width - newWindow.Width) / 2);
	  }
	  else
	  {
	      newWindow.Top = storedCreatingWindows.Last().Top - 100;
	      newWindow.Left = storedCreatingWindows.Last().Left - 100;
	  }

	  newWindow.Show();
	  storedCreatingWindows.Add(newWindow);
        }

        public void AddTask(string txtInput)
        {
	  SaveLoadManager.Save(CreateContentContainer(txtInput, ContentTypes.Task), ContentTypes.Task);
        }
        public void AddMemo(string txtInput)
        {
	  SaveLoadManager.Save(CreateContentContainer(txtInput, ContentTypes.Memo), ContentTypes.Memo);
        }
        public ContentContainer AddTaskWhenLoaded(string txtInput)
        {
	  return CreateContentContainer(txtInput, ContentTypes.Task);
        }
        public ContentContainer AddMemoWhenLoaded(string txtInput)
        {
	  return CreateContentContainer(txtInput, ContentTypes.Memo);
        }
        private ContentContainer CreateContentContainer(string txtInput, ContentTypes contentTypes)
        {
	  ContentContainer contentContainer = new ContentContainer();

	  contentContainer.Margin = new Thickness(15, 0, 0, 8);
	  contentContainer.HorizontalAlignment = HorizontalAlignment.Left;
	  contentContainer.ContentText = txtInput;

	  if (contentTypes == ContentTypes.Task)
	      spTask.Children.Add(contentContainer);
	  else if (contentTypes == ContentTypes.Memo)
	      spMemo.Children.Add(contentContainer);

	  return contentContainer;
        }

        public void DeleteAllTask()
        {
	  spTask.Children.Clear();
        }
        public void DeleteAllMemo()
        {
	  spMemo.Children.Clear();
        }

        private void btMemo_Click(object sender, RoutedEventArgs e)
        {
	  grid.RowDefinitions[2].Height = new GridLength(0, GridUnitType.Pixel);
	  grid.RowDefinitions[4].Height = new GridLength(1, GridUnitType.Star);
	  state = ContentTypes.Memo;
        }
        private void btTask_Click(object sender, RoutedEventArgs e)
        {
	  grid.RowDefinitions[2].Height = new GridLength(1, GridUnitType.Star);
	  grid.RowDefinitions[4].Height = new GridLength(0, GridUnitType.Pixel);
	  state = ContentTypes.Task;
        }

        private void btClearTask_Click(object sender, RoutedEventArgs e)
        {
	  SaveLoadManager.ResetTask();
        }
        private void btClearMemo_Click(object sender, RoutedEventArgs e)
        {
	  SaveLoadManager.ResetMemo();
        }

        //// Top Bar
        private void TopBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
	  if (e.ChangedButton == MouseButton.Left)
	      this.DragMove();
        }
        private void btClose_Click(object sender, RoutedEventArgs e)
        {
	  WindowState = WindowState.Minimized;
        }
        private void btSetPos_Click(object sender, RoutedEventArgs e)
        {
	  // Set size : default
	  this.Height = 600;
	  this.Width = 350;

	  // Set Window Location : Bottom-Right
	  var desktopWorkingArea = SystemParameters.WorkArea;
	  this.Left = desktopWorkingArea.Right - this.Width;
	  this.Top = desktopWorkingArea.Bottom - this.Height;
        }


        // [KeyBoard Shortcut]
        private bool isCtrlPressed = false;
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
	  if (e.Key == Key.T && !isCtrlPressed)
	  {
	      btAddTask.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
	  }
	  else if (e.Key == Key.M && !isCtrlPressed)
	  {
	      btAddMemo.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
	  }
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
	  if (Keyboard.IsKeyDown(Key.LeftCtrl))
	  {
	      isCtrlPressed = true;
	      DelayFromLeftCtrl();
	  }

	  if (e.Key == Key.Escape)
	  {
	      if (storedCreatingWindows.Count > 0)
	      {
		storedCreatingWindows.Last().Close();
		storedCreatingWindows.RemoveAt(storedCreatingWindows.Count - 1);
	      }
	      else
		btClose.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
	  }
	  else if (e.Key == Key.X && isCtrlPressed)
	  {
	      Application.Current.Shutdown();
	  }
	  else if (e.Key == Key.T && isCtrlPressed)
	  {
	      btTask.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
	  }
	  else if (e.Key == Key.M && isCtrlPressed)
	  {
	      btMemo.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
	  }
        }
        private async void DelayFromLeftCtrl()
        {
	  await Task.Delay(400);
	  isCtrlPressed = false;
        }
    }
}
