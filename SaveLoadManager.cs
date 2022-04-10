using SimpleToDo.UserControls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public enum ContentTypes { Task, Memo }

namespace SimpleToDo
{
    class SaveLoadManager
    {
        private static string appInfo = "//SimpleToDo_ver 1.0//" + '\n' + "//Application Run Complete...//";

        static List<ContentContainer> contentsTask = new List<ContentContainer>();
        static List<ContentContainer> contentsMemo = new List<ContentContainer>();

        public static void Save(ContentContainer contentContainer, ContentTypes contentTypes)
        {
	  if (contentTypes == ContentTypes.Task)
	  {
	      File.AppendAllText("data_task.txt", contentContainer.ContentText + Environment.NewLine);
	      contentsTask.Add(contentContainer);
	  }
	  else if (contentTypes == ContentTypes.Memo)
	  {
	      File.AppendAllText("data_memo.txt", contentContainer.ContentText + Environment.NewLine);
	      contentsMemo.Add(contentContainer);
	  }

        }
        public static void Delete(ContentContainer contentContainer, ContentTypes contentTypes)
        {
	  if (contentTypes == ContentTypes.Task)
	  {
	      string[] allLine = File.ReadAllLines("data_task.txt");
	      int index = contentsTask.IndexOf(contentContainer);
	      allLine = allLine.Where(w => w != allLine[index]).ToArray();
	      File.WriteAllLines("data_task.txt", allLine);

	      contentsTask.Remove(contentContainer);
	  }
	  else if (contentTypes == ContentTypes.Memo)
	  {
	      string[] allLine = File.ReadAllLines("data_memo.txt");
	      int index = contentsMemo.IndexOf(contentContainer);
	      allLine = allLine.Where(w => w != allLine[index]).ToArray();
	      File.WriteAllLines("data_memo.txt", allLine);

	      contentsMemo.Remove(contentContainer);
	  }
        }
        public static void Edit(ContentContainer contentContainer, ContentTypes contentTypes)
        {
	  if (contentTypes == ContentTypes.Task)
	  {
	      string[] allLine = File.ReadAllLines("data_task.txt");
	      allLine[contentsTask.IndexOf(contentContainer)] = contentContainer.ContentText;
	      File.WriteAllLines("data_task.txt", allLine);
	  }
	  else if (contentTypes == ContentTypes.Memo)
	  {
	      string[] allLine = File.ReadAllLines("data_memo.txt");
	      allLine[contentsMemo.IndexOf(contentContainer)] = contentContainer.ContentText;
	      File.WriteAllLines("data_memo.txt", allLine);
	  }
        }
        public static void Load()
        {
	  // If there no file
	  if (!File.Exists("data_task.txt"))
	      File.Create("data_task.txt").Dispose();
	  if (!File.Exists("data_memo.txt"))
	      File.Create("data_memo.txt").Dispose();

	  // Set dat in wpf panels
	  // Save in List automatically in AddTask/AddMemo function
	  string[] allLineTask = File.ReadAllLines("data_task.txt");
	  if (allLineTask != null)
	      foreach (var item in allLineTask)
	      {
		ContentContainer contentContainer = (System.Windows.Application.Current.MainWindow as MainWindow).AddTaskWhenLoaded(item);
		contentsTask.Add(contentContainer);
	      }
	  string[] allLineMemo = File.ReadAllLines("data_memo.txt");
	  if (allLineMemo != null)
	      foreach (var item in allLineMemo)
	      {
		ContentContainer contentContainer = (System.Windows.Application.Current.MainWindow as MainWindow).AddMemoWhenLoaded(item);
		contentsMemo.Add(contentContainer);
	      }
        }
        public static void ResetTask()
        {
	  (System.Windows.Application.Current.MainWindow as MainWindow).DeleteAllTask();
	  contentsTask.Clear();
	  File.WriteAllText("data_task.txt", "");
        }
        public static void ResetMemo()
        {
	  (System.Windows.Application.Current.MainWindow as MainWindow).DeleteAllMemo();
	  contentsMemo.Clear();
	  File.WriteAllText("data_memo.txt", "");
        }
        public static bool CheckFirstRun()
        {
	  if (!File.Exists("AppInfo.txt"))
	  {
	      File.Create("AppInfo.txt").Dispose();
	      File.AppendAllText("AppInfo.txt", appInfo);
	      return true;
	  }
	  else
	  {
	      return false;
	  }
        }
    }
}
