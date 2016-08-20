using System;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AmCharts.Windows.QuickCharts
{
    /// <summary>
    /// Represents grid in serial chart.
    /// </summary>
    public class ValueGrid : Control
    {
        /// <summary>
        /// Initializes a new instance of ValueGrid class.
        /// </summary>
        public ValueGrid()
        {
            this.DefaultStyleKey = typeof(ValueGrid);

            VerticalAlignment = VerticalAlignment.Stretch;
            HorizontalAlignment = HorizontalAlignment.Stretch;

            this.LayoutUpdated += OnValueGridLayoutUpdated;
        }

        void OnValueGridLayoutUpdated(object sender, object e)
        {
            SetupLines();
        }

        private Canvas _gridCanvas;
        private List<double> _locations = new List<double>();
        private List<Line> _gridLines = new List<Line>();

        /// <summary>
        /// Assigns control template parts.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            _gridCanvas = (Canvas)TreeHelper.TemplateFindName("PART_GridCanvas", this);
        }

        /// <summary>
        /// Sets locations (coordinates) of grid lines.
        /// </summary>
        /// <param name="locations">Locations (coordinates) of grid lines.</param>
        public void SetLocations(IEnumerable<double> locations)
        {
            _locations = new List<double>(locations);
            SetupLines();
        }

        private void SetupLines()
        {
            int count = Math.Min(_locations.Count, _gridLines.Count);
            
            SetLineLocations(count);
            
            if (_locations.Count != _gridLines.Count)
            {
                if (_locations.Count > _gridLines.Count)
                {
                    AddGridLines(count);
                }
                else if (_locations.Count < _gridLines.Count)
                {
                    RemoveGridLines(count);
                }

            }
        }

        private void SetLineLocations(int count)
        {
            for (int i = 0; i < count; i++)
            {
                SetLineX(i);
                SetLineY(i);
            }
        }

        private void SetLineX(int i)
        {
            if (_gridCanvas != null)
            {
                if (_gridLines[i].X2 != _gridCanvas.ActualWidth)
                {
                    _gridLines[i].X2 = _gridCanvas.ActualWidth;
                }
            }
        }

        private void SetLineY(int i)
        {
            if (_gridLines[i].Y1 != _locations[i])
            {
                _gridLines[i].Y1 = _locations[i];
                _gridLines[i].Y2 = _gridLines[i].Y1;
            }
        }

        private void AddGridLines(int count)
        {
            for (int i = count; i < _locations.Count; i++)
            {
                Line line = new Line();
                line.Stroke = Foreground;
                line.StrokeThickness = 1;
                line.X1 = 0;
                _gridLines.Add(line);

                SetLineX(i);
                SetLineY(i);

                _gridCanvas.Children.Add(_gridLines[i]);
            }
        }


        private void RemoveGridLines(int count)
        {
            for (int i = _gridLines.Count - 1; i >= count; i--)
            {
                _gridCanvas.Children.Remove(_gridLines[i]);
                _gridLines.RemoveAt(i);
            }
        }
    }
}
