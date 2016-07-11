﻿namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Windows.Controls;

    /// <summary>This <see cref="ValidationResult"/> is returned when <see cref="Value"/> in the valid range <see cref="Max"/></summary>
    public abstract class OutOfRangeValidationResult : NumericValidationResult
    {
        protected OutOfRangeValidationResult(
            IFormattable value,
            IFormattable min,
            IFormattable max,
            IFormatProvider currentBoxCulture,
            OneParameterFormatAndCulture formatAndCulture,
            bool isValid,
            object errorContent)
            : base(currentBoxCulture, formatAndCulture, isValid, errorContent)
        {
            this.Value = value;
            this.Min = min;
            this.Max = max;
        }

        /// <summary>Gets the current value at the time of validation.</summary>
        public IFormattable Value { get; }

        /// <summary>Gets the maximum allowed value, can be null if no lower limit.</summary>
        public IFormattable Min { get; }

        /// <summary>Gets the maximum allowed value, can be null if no upper limit..</summary>
        public IFormattable Max { get; }
    }
}