using MacroModules.MacroLibrary.Types;
using MacroModules.MacroLibrary.WinApi;
using System.Collections.Generic;
using static MacroModules.MacroLibrary.WinApi.SystemMetricsApi;

namespace MacroLibrary.UnitTests
{
    internal class SystemDisplayDataTests
    {
        [SetUp]
        public void Setup()
        {
            Position origin1 = new(0, 0);
            Position origin2 = new(-width, 0);
            DisplayData display1 = new(origin1, width, height);
            DisplayData display2= new(origin2, width, height);

            displays1.Add(display1);
            displays2.Add(display1);
            displays2.Add(display2);

            system1 = new(
                primaryScreenWidth: width,
                primaryScreenHeight: height,
                virtualScreenOrigin: origin1,
                virtualScreenWidth: width,
                virtualScreenHeight: height,
                displays: displays1);

            system2 = new(
                primaryScreenWidth: width,
                primaryScreenHeight: height,
                virtualScreenOrigin: origin2,
                virtualScreenWidth: width * 2,
                virtualScreenHeight: height,
                displays: displays2);
        }

        [Test]
        public void CanCreate_DefaultConstructor_MetricsAreValid()
        {
            SystemDisplayData system = new();

            Assert.That(system.PrimaryScreenWidth, Is.EqualTo(GetSystemMetrics((int)Metric.PrimaryScreenWidth)));
            Assert.That(system.PrimaryScreenHeight, Is.EqualTo(GetSystemMetrics((int)Metric.PrimaryScreenHeight)));
            Assert.That(system.VirtualScreenOrigin.X, Is.EqualTo(GetSystemMetrics((int)Metric.VirtualScreenPosX)));
            Assert.That(system.VirtualScreenOrigin.Y, Is.EqualTo(GetSystemMetrics((int)Metric.VirtualScreenPosY)));
            Assert.That(system.VirtualScreenWidth, Is.EqualTo(GetSystemMetrics((int)Metric.VirtualScreenWidth)));
            Assert.That(system.VirtualScreenHeight, Is.EqualTo(GetSystemMetrics((int)Metric.VirtualScreenHeight)));

            // Can't test Displays and DisplayCount since those cannot be accurately measured by
            // GetSystemMetrics
        }

        [Test]
        public void CanCompareEquality_TwoDefaultConstructed_ReturnsTrue()
        {
            SystemDisplayData system1 = new();
            SystemDisplayData system2 = new();

            Assert.That(system1 == system2, Is.True);
        }

        [Test]
        public void CanCompareInequality_TwoDefaultConstructed_ReturnsFalse()
        {
            SystemDisplayData new1 = new();
            SystemDisplayData new2 = new();

            Assert.That(new1 != new2, Is.False);
        }

        [Test]
        public void CanCompareEquality_TwoDifferentCustomConstructed_ReturnsFalse()
        {
            Assert.That(system1 == system2, Is.False);
        }

        [Test]
        public void CanCompareInequality_TwoDifferentCustomConstructed_ReturnsTrue()
        {
            Assert.That(system1 != system2, Is.True);
        }

        [Test]
        public void CanCompareEquality_SameInstance_ReturnsTrue()
        {
            Assert.That(system1 == system1, Is.True);
        }

        [Test]
        public void CanCompareEquality_EquivalentSystems_ReturnsTrue()
        {
            Position origin1 = new(0, 0);
            Position origin2 = new(-width, 0);
            DisplayData display1 = new(origin1, width, height);
            DisplayData display2 = new(origin2, width, height);

            HashSet<DisplayData> displaySet = [display1, display2];

            SystemDisplayData system = new(
                primaryScreenWidth: width,
                primaryScreenHeight: height,
                virtualScreenOrigin: origin2,
                virtualScreenWidth: width * 2,
                virtualScreenHeight: height,
                displays: displays2);

            Assert.That(system == system2, Is.True);
        }

        private int width = 1920;
        private int height = 1080;
        private HashSet<DisplayData> displays1 = new();
        private HashSet<DisplayData> displays2 = new();
        private SystemDisplayData system1;
        private SystemDisplayData system2;
    }
}
