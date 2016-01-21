﻿namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    /// <summary>
    /// Base class that adds a couple of dependency properties to TextBox
    /// </summary>
    public abstract partial class BaseBox
    {
        public static readonly DependencyProperty TextValueConverterProperty = DependencyProperty.Register(
            "TextValueConverter", 
            typeof (IValueConverter), 
            typeof (BaseBox), 
            new PropertyMetadata(default(IValueConverter), OnTextValueConverterChanged));

        public static readonly DependencyProperty ValidationRulesProperty = DependencyProperty.Register(
            "ValidationRules", 
            typeof (IReadOnlyList<ValidationRule>),
            typeof (BaseBox),
            new PropertyMetadata(default(IReadOnlyList<ValidationRule>), OnValidationRulesChanged));

        private static readonly DependencyPropertyKey FormattedTextPropertyKey = DependencyProperty.RegisterReadOnly(
            "FormattedText",
            typeof(string),
            typeof(BaseBox),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty FormattedTextProperty = FormattedTextPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey IsValidationDirtyPropertyKey = DependencyProperty.RegisterReadOnly(
            "IsValidationDirty",
            typeof(bool),
            typeof(BaseBox),
            new PropertyMetadata(
                BooleanBoxes.False,
                OnIsValidationDirtyChanged));

        internal static readonly DependencyProperty IsValidationDirtyProperty = IsValidationDirtyPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey IsFormattingDirtyPropertyKey = DependencyProperty.RegisterReadOnly(
            "IsFormattingDirty",
            typeof(bool),
            typeof(BaseBox),
            new PropertyMetadata(
                BooleanBoxes.False,
                OnIsFormattingDirtyChanged));

        internal static readonly DependencyProperty IsFormattingDirtyProperty = IsFormattingDirtyPropertyKey.DependencyProperty;

        public static readonly DependencyProperty StringFormatProperty = NumericBox.StringFormatProperty.AddOwner(
            typeof(BaseBox),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.Inherits, OnStringFormatChanged));

        public static readonly DependencyProperty CultureProperty = NumericBox.CultureProperty.AddOwner(
            typeof(BaseBox),
            new FrameworkPropertyMetadata(
                Thread.CurrentThread.CurrentUICulture,
                FrameworkPropertyMetadataOptions.Inherits,
                OnCultureChanged));

        public static readonly DependencyProperty RegexPatternProperty = DependencyProperty.Register(
            "RegexPattern",
            typeof(string),
            typeof(BaseBox),
            new PropertyMetadata(default(string), OnPatternChanged));

        public static readonly DependencyProperty AllowSpinnersProperty = NumericBox.AllowSpinnersProperty.AddOwner(
            typeof(BaseBox),
            new FrameworkPropertyMetadata(
                BooleanBoxes.False,
                FrameworkPropertyMetadataOptions.Inherits,
                OnAllowSpinnersChanged));

        private static readonly DependencyPropertyKey IncreaseCommandPropertyKey = DependencyProperty.RegisterReadOnly(
            "IncreaseCommand",
            typeof(ICommand),
            typeof(BaseBox),
            new PropertyMetadata(null));

        public static readonly DependencyProperty IncreaseCommandProperty = IncreaseCommandPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey DecreaseCommandPropertyKey = DependencyProperty.RegisterReadOnly(
            "DecreaseCommand",
            typeof(ICommand),
            typeof(BaseBox),
            new PropertyMetadata(null));

        public static readonly DependencyProperty DecreaseCommandProperty = DecreaseCommandPropertyKey.DependencyProperty;

        private static readonly DependencyProperty TextProxyProperty = DependencyProperty.Register(
            "TextProxy",
            typeof(string),
            typeof(BaseBox),
            new PropertyMetadata(
                string.Empty,
                OnTextProxyChanged));

        internal static readonly DependencyProperty TextBindableProperty = DependencyProperty.Register(
            "TextBindable",
            typeof(string),
            typeof(BaseBox),
            new PropertyMetadata(
                string.Empty,
                OnTextBindableChanged));

        protected static readonly DependencyPropertyKey TextSourcePropertyKey = DependencyProperty.RegisterReadOnly(
            "TextSource",
            typeof(TextSource),
            typeof(BaseBox),
            new PropertyMetadata(TextSource.None, OnTextSourceChanged));

        public static readonly DependencyProperty TextSourceProperty = TextSourcePropertyKey.DependencyProperty;

        internal static readonly DependencyProperty StatusProperty = DependencyProperty.Register(
            "Status",
            typeof(Status),
            typeof(BaseBox),
            new PropertyMetadata(Status.Idle, OnStatusChanged));

        static BaseBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BaseBox), new FrameworkPropertyMetadata(typeof(BaseBox)));
            Validator.Start();
        }

        public IValueConverter TextValueConverter
        {
            get { return (IValueConverter) this.GetValue(TextValueConverterProperty); }
            set { this.SetValue(TextValueConverterProperty, value); }
        }

        public IReadOnlyList<ValidationRule> ValidationRules
        {
            get { return (IReadOnlyList<ValidationRule>) this.GetValue(ValidationRulesProperty); }
            set { this.SetValue(ValidationRulesProperty, value); }
        }

        [Category(nameof(NumericBox))]
        [Browsable(true)]
        public string FormattedText
        {
            get { return (string)this.GetValue(FormattedTextProperty); }
            protected set { this.SetValue(FormattedTextPropertyKey, value); }
        }

        internal bool IsFormattingDirty
        {
            get { return (bool)this.GetValue(IsFormattingDirtyProperty); }
            set { this.SetValue(IsFormattingDirtyPropertyKey, value ? BooleanBoxes.True : BooleanBoxes.False); }
        }

        internal bool IsValidationDirty
        {
            get { return (bool)this.GetValue(IsValidationDirtyProperty); }
            set { this.SetValue(IsValidationDirtyPropertyKey, value ? BooleanBoxes.True : BooleanBoxes.False); }
        }

        /// <summary>
        /// Gets or sets the  culture for the control.
        /// The control has an explicit culture and does not use <see cref="System.Threading.Thread.CurrentUICulture"/>
        /// </summary>
        [Category(nameof(NumericBox))]
        [Browsable(true)]
        public IFormatProvider Culture
        {
            get { return (IFormatProvider)this.GetValue(CultureProperty); }
            set { this.SetValue(CultureProperty, value); }
        }

        [Category(nameof(NumericBox))]
        [Browsable(true)]
        public string StringFormat
        {
            get { return (string)this.GetValue(StringFormatProperty); }
            set { this.SetValue(StringFormatProperty, value); }
        }

        /// <summary>
        /// Gets or sets a regex pattern for validation
        /// </summary>
        [Category(nameof(NumericBox))]
        [Browsable(true)]
        public string RegexPattern
        {
            get { return (string)this.GetValue(RegexPatternProperty); }
            set { this.SetValue(RegexPatternProperty, value); }
        }

        [Category(nameof(NumericBox))]
        [Browsable(true)]
        public bool AllowSpinners
        {
            get { return (bool)this.GetValue(AllowSpinnersProperty); }
            set { this.SetValue(AllowSpinnersProperty, value); }
        }

        [Category(nameof(NumericBox))]
        [Browsable(true)]
        public ICommand IncreaseCommand
        {
            get { return (ICommand)this.GetValue(IncreaseCommandProperty); }
            private set { this.SetValue(IncreaseCommandPropertyKey, value); }
        }

        [Category(nameof(NumericBox))]
        [Browsable(true)]
        public ICommand DecreaseCommand
        {
            get { return (ICommand)this.GetValue(DecreaseCommandProperty); }
            private set { this.SetValue(DecreaseCommandPropertyKey, value); }
        }

        public TextSource TextSource
        {
            get { return (TextSource)this.GetValue(TextSourceProperty); }
            protected set { this.SetValue(TextSourcePropertyKey, value); }
        }

        protected internal string TextBindable
        {
            get { return (string)this.GetValue(TextBindableProperty); }
            protected set { this.SetValue(TextBindableProperty, value); }
        }

        internal Status Status
        {
            get { return (Status)this.GetValue(StatusProperty); }
            set { this.SetValue(StatusProperty, value); }
        }

        private static void OnIsValidationDirtyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine(e);
            var box = (BaseBox)d;
            if (box.TextSource != TextSource.None && Equals(e.NewValue, BooleanBoxes.True))
            {
                box.RaiseEvent(ValidationDirtyEventArgs);
            }
        }

        private static void OnIsFormattingDirtyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine(e);
            if (Equals(e.NewValue, BooleanBoxes.True))
            {
                ((BaseBox)d).RaiseEvent(FormatDirtyEventArgs);
            }
        }

        private static void OnTextValueConverterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = (BaseBox)d;
            if (box.TextSource != TextSource.None)
            {
                box.IsFormattingDirty = true;
                box.IsValidationDirty = true;
            }
        }

        private static void OnValidationRulesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = (BaseBox)d;
            if (box.TextSource != TextSource.None)
            {
                box.IsValidationDirty = true;
            }
        }

        private static void OnStringFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = (BaseBox)d;
            if (box.TextSource != TextSource.None)
            {
                box.OnStringFormatChanged((string)e.OldValue, (string)e.NewValue);
                box.IsFormattingDirty = true;
                box.IsValidationDirty = true;
            }
        }

        private static void OnCultureChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = (BaseBox)d;
            if (box.TextSource != TextSource.None)
            {
                box.OnCultureChanged((IFormatProvider)e.OldValue, (IFormatProvider)e.NewValue);
                box.IsFormattingDirty = true;
                box.IsValidationDirty = true;
            }
        }

        private static void OnPatternChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = (BaseBox)d;
            if (box.TextSource != TextSource.None)
            {
                box.IsValidationDirty = true;
            }
        }

        private static void OnAllowSpinnersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = (BaseBox)d;
            (box.IncreaseCommand as ManualRelayCommand)?.RaiseCanExecuteChanged();
            (box.DecreaseCommand as ManualRelayCommand)?.RaiseCanExecuteChanged();
        }

        private static void OnTextProxyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine(e);
            var baseBox = (BaseBox)d;

            if (baseBox.Status == Status.Idle)
            {
                baseBox.Status = Status.UpdatingFromUserInput;
                baseBox.TextSource = TextSource.UserInput;
                d.SetCurrentValue(TextBindableProperty, e.NewValue);
                baseBox.Status = Status.Idle;
            }
        }

        private static void OnTextBindableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine(e);
            var box = (BaseBox)d;
            box.CheckSpinners();
        }

        private static void OnStatusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine(e);
        }

        private static void OnTextSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine(e);
        }
    }
}