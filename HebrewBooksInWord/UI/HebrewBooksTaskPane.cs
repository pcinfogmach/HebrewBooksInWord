using HebrewBooks;
using HebrewBooksInWord.Resources;
using Microsoft.Office.Tools;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Forms.Integration;
using DockPosition = Microsoft.Office.Core.MsoCTPDockPosition;

namespace HebrewBooksInWord
{
    internal class HebrewBooksTaskPane : HebrewBooksViewer
    {
        public CustomTaskPane Show()
        {
            try
            {
                // First, check if the task pane already exists
                CustomTaskPane taskPane = Globals.ThisAddIn.CustomTaskPanes
                        .OfType<CustomTaskPane>()
                        .FirstOrDefault(pane => pane.Control is System.Windows.Forms.UserControl hostControl &&
                            hostControl.Controls.OfType<ElementHost>().Any(host => host.Child is HebrewBooksTaskPane) &&
                            pane.Window == Globals.ThisAddIn.Application.ActiveWindow);

                // If the task pane does not exist, create a new one
                if (taskPane == null)
                {
                    // Create a new side panel
                    var sidePanel = new System.Windows.Forms.UserControl();

                    // Create and configure the task pane
                    taskPane = Globals.ThisAddIn.CustomTaskPanes.Add(sidePanel, "HebrewBooks");
                    taskPane.Width = SettingsHandler.LoadSetting("TaskPaneWidth", 600);

                    // Load the saved dock position
                    if (Enum.TryParse(SettingsHandler.LoadSetting("DockPosition",
                        DockPosition.msoCTPDockPositionRight.ToString()),
                        out DockPosition savedDockPosition))
                    {
                        taskPane.DockPosition = savedDockPosition;
                    }

                    // Save the dock position and width when it changes
                    taskPane.DockPositionChanged += (sender, e) => { SettingsHandler.SaveSetting("DockPosition", taskPane.DockPosition.ToString()); };
                    Globals.ThisAddIn.Application.DocumentChange += () => { try { SettingsHandler.SaveSetting("TaskPaneWidth", taskPane.Width); } catch { } };
                    Globals.ThisAddIn.Shutdown += (s, e) => { try { SettingsHandler.SaveSetting("TaskPaneWidth", taskPane.Width); } catch { } };

                    // Add an ElementHost and attach the HebrewBooksTaskPane to it
                    ElementHost elementHost = new ElementHost { Dock = System.Windows.Forms.DockStyle.Fill };
                    sidePanel.Controls.Add(elementHost);
                    elementHost.Child = this;
                }

                // Show the task pane
                taskPane.Visible = true;

                return taskPane;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
    }
}
