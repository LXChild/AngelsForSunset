﻿using System;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Data;

namespace AmCharts.Windows.QuickCharts
{
    /// <summary>
    /// Utility class to facilitate temporary binding evaluation
    /// </summary>
    public class BindingEvaluator : FrameworkElement
    {
        /// <summary>
        /// Created binding evaluator and set path to the property which's value should be evaluated.
        /// </summary>
        /// <param name="propertyPath">Path to the property</param>
        public BindingEvaluator(string propertyPath)
        {
            _propertyPath = propertyPath;
        }

        private string _propertyPath;

        /// <summary>
        /// Dependency property used to evaluate values.
        /// </summary>
        public static readonly DependencyProperty EvaluatorProperty = DependencyProperty.Register(
            "Evaluator", typeof(object), typeof(BindingEvaluator), null);

        /// <summary>
        /// Returns evaluated value of property on provided object source.
        /// </summary>
        /// <param name="source">Object for which property value should be evaluated</param>
        /// <returns>Value of the property</returns>
        public object Eval(object source)
        {
            ClearValue(BindingEvaluator.EvaluatorProperty);
            var binding = new Binding();
            binding.Path = new PropertyPath(_propertyPath);
            binding.Mode = BindingMode.OneTime;
            binding.Source = source;
            SetBinding(BindingEvaluator.EvaluatorProperty, binding);
            return GetValue(BindingEvaluator.EvaluatorProperty);
        }
    }
}
