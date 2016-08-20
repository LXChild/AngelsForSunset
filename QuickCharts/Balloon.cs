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
    /// Represents a value balloon (tooltip).
    /// </summary>
    public class Balloon : Control
    {
        /// <summary>
        /// Instantiates Balloon.
        /// </summary>
        public Balloon()
        {
            this.DefaultStyleKey = typeof(Balloon);
            this.IsHitTestVisible = false;
        }

        /// <summary>
        /// Identifies <see cref="Text"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(Balloon),
            new PropertyMetadata(null)
            );

        /// <summary>
        /// Gets or sets balloon text.
        /// This is a dependency property.
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
    }
}
