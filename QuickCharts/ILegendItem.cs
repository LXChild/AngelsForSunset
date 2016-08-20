using System;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace AmCharts.Windows.QuickCharts
{
    /// <summary>
    /// Interface which should be implemented by classes used as legend items.
    /// </summary>
    public interface ILegendItem
    {
        /// <summary>
        /// Gets or sets title shown in Legend.
        /// </summary>
        string Title { get; set; }
        /// <summary>
        /// Gets or sets brush for the Legend key.
        /// </summary>
        Brush Brush { get; set; }
    }
}
