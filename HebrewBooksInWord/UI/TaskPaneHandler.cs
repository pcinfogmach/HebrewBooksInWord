//using Microsoft.Office.Interop.Word;
//using Microsoft.Office.Tools;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace HebrewBooksInWord
//{
//    public static class TaskPaneHandler
//    {
//        public static CustomTaskPane LaunchTaskPane()
//        {
//            CustomTaskPane taskPane = GetCurrentTaskPane();

//            if (taskPane == null)
//            {
                
//            }

//            taskPane.Visible = true;

//            return taskPane;
//        }

//        public static CustomTaskPane NewTaskPane()
//        {
//            UserControl hostControl = new UserControl();
//            CustomTaskPane taskPane = Globals.ThisAddIn.CustomTaskPanes.Add(hostControl, "תורת אמת");
//            taskPane.Width = Math.Min(500, Properties.Settings.Default.TaskPaneWidth);

//            if (Enum.TryParse(Properties.Settings.Default.DockPosition, out Microsoft.Office.Core.MsoCTPDockPosition savedDockPosition))
//            {
//                taskPane.DockPosition = savedDockPosition;
//            }
//            else
//            {
//                taskPane.DockPosition = Microsoft.Office.Core.MsoCTPDockPosition.msoCTPDockPositionRight;
//            }

//            taskPane.DockPositionChanged += (sender, e) =>
//            {
//                Properties.Settings.Default.DockPosition = taskPane.DockPosition.ToString();
//                Properties.Settings.Default.Save();
//            };
//        }

//        public static CustomTaskPane GetCurrentTaskPane()
//        {
//            return Globals.ThisAddIn.CustomTaskPanes
//                      .OfType<CustomTaskPane>()
//            .FirstOrDefault(pane =>
//                          pane.Control is HostControl &&
//                          pane.Window == Globals.ThisAddIn.Application.ActiveWindow);
//        }

//        public static void SaveCurrentTaskPaneWidth()
//        {
//            var taskpane = Globals.ThisAddIn.CustomTaskPanes.OfType<CustomTaskPane>().FirstOrDefault(pane => pane.Control is HostControl);
//            if (taskpane != null)
//            {
//                Properties.Settings.Default.TaskPaneWidth = taskpane.Width;
//                Properties.Settings.Default.Save();
//            }
//        }
//    }
//}
