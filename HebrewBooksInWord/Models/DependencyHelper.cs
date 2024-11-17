using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace HebrewBooks.Models
{
    public static class DependencyHelper
    {
        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            // Get the parent object
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            // Check if we've reached the end of the tree
            if (parentObject == null) return null;

            // Check if the parent is of the specified type
            if (parentObject is T parent)
                return parent;
            else
                // Recursively look up the tree
                return FindParent<T>(parentObject);
        }
    }
}
