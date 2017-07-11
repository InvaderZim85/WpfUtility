﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfUtility.GeneralUserControls
{
    public class NumberTextBox : TextBox
    {
        private static NumberTextBox _this;

        public NumberTextBox()
        {
            _this = this;
            TextChanged += OnTextChanged;
            KeyDown += OnKeyDown;
        }

        public new string Text
        {
            get => base.Text;
            set => base.Text = LeaveOnlyNumbers(value);
        }

        /// <summary>
        /// Check if the input is a number
        /// </summary>
        /// <param name="inKey">Pressed Key</param>
        /// <returns>True or false</returns>
        private static bool IsNumberKey(Key inKey)
        {
            if (inKey < Key.D0 || inKey > Key.D9)
            {
                if (inKey < Key.NumPad0 || inKey > Key.NumPad9)
                {
                    return false;
                }
            }
            return true;
        }

        public int Maximum
        {
            get => (int)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register(nameof(Maximum), typeof(int),
                typeof(NumberTextBox), new FrameworkPropertyMetadata(int.MaxValue, PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            _this.Text = _this.LeaveOnlyNumbers(_this.Text);
        }

        public int Minimum
        {
            get => (int)GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }

        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register(nameof(Minimum), typeof(int),
                typeof(NumberTextBox), new FrameworkPropertyMetadata(0, PropertyChangedCallback));

        /// <summary>
        /// Check if the input is a "delete commad"
        /// </summary>
        /// <param name="inKey">Pressed Key</param>
        /// <returns>True or false</returns>
        private static bool IsDelBackspaceOrEnterKey(Key inKey)
        {
            return inKey == Key.Delete || inKey == Key.Back || inKey == Key.Enter;
        }

        /// <summary>
        /// Remove every non numeric value
        /// </summary>
        /// <param name="inString">Entered string</param>
        /// <returns>Purged string</returns>
        private string LeaveOnlyNumbers(string inString)
        {
            var tmp = inString;
            foreach (var c in inString)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(c.ToString(), "^[0-9]*$"))
                {
                    tmp = tmp.Replace(c.ToString(), "");
                }
            }
            if(int.TryParse(tmp, out int number))
            {
                if (number > Maximum)
                    return Maximum.ToString();
                if (number < Minimum)
                    return Minimum.ToString();
            }

            return tmp;
        }

        protected void OnKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = !IsNumberKey(e.Key) && !IsDelBackspaceOrEnterKey(e.Key);
        }

        protected void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            base.Text = LeaveOnlyNumbers(Text);
        }
    }
}