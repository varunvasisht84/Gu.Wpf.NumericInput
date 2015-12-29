namespace Gu.Wpf.NumericInput.Tests
{
    using System;
    using NUnit.Framework;

    [TestFixture, RequiresSTA]
    public class LongBoxTests : NumericBoxTests<LongBox,long>
    {
        protected override long Max => 10;

        protected override long Min => -10;

        protected override long Increment => 1;

        protected override Func<LongBox> Creator
        {
            get { return () => new LongBox(); }
        }
    }
}