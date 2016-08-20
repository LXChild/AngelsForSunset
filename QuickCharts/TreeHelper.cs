﻿using System;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace AmCharts.Windows.QuickCharts
{
    /// <summary>
    /// Helper class to unify certain aspects of navigating VisualTree in WPF and Silvelright.
    /// </summary>
    public static class TreeHelper
    {
        /// <summary>
        /// Finds object in control's template by it's name.
        /// </summary>
        /// <param name="name">Objects name.</param>
        /// <param name="templatedParent">Templated parent.</param>
        /// <returns>Object reference if found, null otherwise.</returns>
        public static object TemplateFindName(string name, FrameworkElement templatedParent)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(templatedParent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(templatedParent, i);
                if (child is FrameworkElement)
                {
                    if (((FrameworkElement)child).Name == name)
                    {
                        return child;
                    }
                    else
                    {
                        object subChild = TreeHelper.TemplateFindName(name, (FrameworkElement)child);
                        if (subChild != null && subChild is FrameworkElement && ((FrameworkElement)subChild).Name == name)
                        {
                            return subChild;
                        }
                    }
                }
            }
            return null;
        }
    }
}
