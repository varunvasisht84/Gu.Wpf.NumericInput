﻿namespace Gu.Wpf.NumericInput.UITests
{
    using System;
    using FlaUI.Core;
    using FlaUI.Core.AutomationElements;
    using FlaUI.Core.Definitions;
    using FlaUI.Core.Input;
    using FlaUI.Core.WindowsAPI;
    using FlaUI.UIA3;
    using NUnit.Framework;

    public sealed class FocusWindowTests : IDisposable
    {
        private readonly Application application;
        private readonly UIA3Automation automation;

        private bool disposed;

        public FocusWindowTests()
        {
            this.application = Application.Launch(Info.CreateStartInfo("FocusWindow"));
            this.automation = new UIA3Automation();
            this.Window = this.application.GetMainWindow(this.automation);
        }

        private Window Window { get; }

        [SetUp]
        public void SetUp()
        {
            this.Window.FindTextBox("TextBox2").Enter("2.345");
            this.Window.FindCheckBox("AllowSpinnersBox").State = ToggleState.Off;
        }

        [Test]
        public void NoSpinnersNoSuffix()
        {
            var doubleBoxes = this.Window.FindGroupBox("DoubleBoxes");
            var textBoxes = this.Window.FindGroupBox("TextBoxes");
            var textBox1 = doubleBoxes.FindTextBox("TextBox1");
            var doubleBox1 = doubleBoxes.FindTextBox("DoubleBox1");
            var doubleBox2 = doubleBoxes.FindTextBox("DoubleBox2");
            doubleBox1.Click();

            Assert.AreEqual(false, textBox1.Properties.HasKeyboardFocus);
            Assert.AreEqual(true, doubleBox1.Properties.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox2.Properties.HasKeyboardFocus);
            doubleBox1.Enter("2");
            Assert.AreEqual("2.345", textBoxes.FindTextBox("TextBox2").Text);

            Keyboard.Press(VirtualKeyShort.TAB);
            Assert.AreEqual("2", textBoxes.FindTextBox("TextBox2").Text);
            Assert.AreEqual(false, textBox1.Properties.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox1.Properties.HasKeyboardFocus);
            Assert.AreEqual(true, doubleBox2.Properties.HasKeyboardFocus);

            Keyboard.Press(VirtualKeyShort.TAB);
            Assert.AreEqual(true, textBox1.Properties.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox1.Properties.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox2.Properties.HasKeyboardFocus);

            Keyboard.Press(VirtualKeyShort.TAB);
            this.Window.WaitUntilResponsive();
            Assert.AreEqual(false, textBox1.Properties.HasKeyboardFocus);
            Assert.AreEqual(true, doubleBox1.Properties.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox2.Properties.HasKeyboardFocus);
        }

        [Test]
        public void WithSpinners()
        {
            var doubleBoxes = this.Window.FindGroupBox("DoubleBoxes");
            var textBoxes = this.Window.FindGroupBox("TextBoxes");
            var textBox1 = doubleBoxes.FindTextBox("TextBox1");
            var doubleBox1 = doubleBoxes.FindTextBox("DoubleBox1");
            var doubleBox2 = doubleBoxes.FindTextBox("DoubleBox2");
            this.Window.FindCheckBox("AllowSpinnersBox").State = ToggleState.On;
            doubleBox1.Click();
            Assert.AreEqual(false, textBox1.Properties.HasKeyboardFocus);
            Assert.AreEqual(true, doubleBox1.Properties.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox2.Properties.HasKeyboardFocus);
            doubleBox1.Enter("2");
            Assert.AreEqual("2.345", textBoxes.FindTextBox("TextBox2").Text);

            Keyboard.Press(VirtualKeyShort.TAB);
            Assert.AreEqual("2", textBoxes.FindTextBox("TextBox2").Text);
            Assert.AreEqual(false, textBox1.Properties.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox1.Properties.HasKeyboardFocus);
            Assert.AreEqual(true, doubleBox2.Properties.HasKeyboardFocus);
            doubleBox1.IncreaseButton().Click();
            Assert.AreEqual("3", doubleBox1.EditText());
            Assert.AreEqual("2", textBoxes.FindTextBox("TextBox2").Text);

            Keyboard.Press(VirtualKeyShort.TAB);
            Assert.AreEqual("3", doubleBox1.EditText());
            Assert.AreEqual("3", textBoxes.FindTextBox("TextBox2").Text);
            Assert.AreEqual(false, textBox1.Properties.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox1.Properties.HasKeyboardFocus);
            Assert.AreEqual(true, doubleBox2.Properties.HasKeyboardFocus);

            Keyboard.Press(VirtualKeyShort.TAB);
            Assert.AreEqual(true, textBox1.Properties.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox1.Properties.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox2.Properties.HasKeyboardFocus);
        }

        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            this.disposed = true;
            this.application?.Dispose();
            this.automation.Dispose();
        }
    }
}
