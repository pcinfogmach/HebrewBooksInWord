using Microsoft.Office.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Office = Microsoft.Office.Core;



namespace HebrewBooksInWord.UI
{
    [ComVisible(true)]
    public class Ribbon1 : Office.IRibbonExtensibility
    {
        private Office.IRibbonUI ribbon;

        public Ribbon1()
        {
        }

        #region IRibbonExtensibility Members

        public string GetCustomUI(string ribbonID)
        {
            return GetResourceText("HebrewBooksInWord.UI.Ribbon1.xml");
        }

        #endregion

        #region Ribbon Callbacks
        //Create callback methods here. For more information about adding callback methods, visit https://go.microsoft.com/fwlink/?LinkID=271226

        public void Ribbon_Load(Office.IRibbonUI ribbonUI)
        {
            this.ribbon = ribbonUI;
        }

        public System.Drawing.Image GetImage(Office.IRibbonControl control)
        {
            // Construct the path to the image
            string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "HebrewBooksIcon2.png");

            // Check if the file exists
            if (!System.IO.File.Exists(path))
            {
                throw new System.IO.FileNotFoundException("Image file not found", path);
            }

            // Load the image into a Bitmap
            System.Drawing.Bitmap image = new System.Drawing.Bitmap(path);
            return image;
        }


        public void OpenTaskPaneButton_Click(IRibbonControl control)
        {
            new HebrewBooksTaskPane().Show();
        }

        #endregion

        #region Helpers

        private static string GetResourceText(string resourceName)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string[] resourceNames = asm.GetManifestResourceNames();
            for (int i = 0; i < resourceNames.Length; ++i)
            {
                if (string.Compare(resourceName, resourceNames[i], StringComparison.OrdinalIgnoreCase) == 0)
                {
                    using (StreamReader resourceReader = new StreamReader(asm.GetManifestResourceStream(resourceNames[i])))
                    {
                        if (resourceReader != null)
                        {
                            return resourceReader.ReadToEnd();
                        }
                    }
                }
            }
            return null;
        }

        #endregion
    }
}
