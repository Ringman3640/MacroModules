using MacroModules.MacroLibrary.Types;

namespace MacroLibrary.UnitTests
{
    internal class DisplayDataTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void CanCreate_PositiveDimensions_CreateIsSameDimensions()
        {
            int width = 1920;
            int height = 1080;
            Position origin = new(0, 0);
            DisplayData data = new(origin, width, height);

            Assert.That(data.Origin, Is.EqualTo(origin));
            Assert.That(data.Width, Is.EqualTo(width));
            Assert.That(data.Height, Is.EqualTo(height));
        }

        [Test]
        public void CanCreate_NegativeWidth_WidthIsZero()
        {
            int width = -1920;
            int height = 1080;
            Position origin = new(0, 0);
            DisplayData data = new(origin, width, height);

            Assert.That(data.Origin, Is.EqualTo(origin));
            Assert.That(data.Width, Is.Zero);
            Assert.That(data.Height, Is.EqualTo(height));
        }

        [Test]
        public void CanCreate_NegativeHeight_HeightIsZero()
        {
            int width = 1920;
            int height = -1080;
            Position origin = new(0, 0);
            DisplayData data = new(origin, width, height);

            Assert.That(data.Origin, Is.EqualTo(origin));
            Assert.That(data.Width, Is.EqualTo(width));
            Assert.That(data.Height, Is.Zero);
        }

        [Test]
        public void CanCreate_NegativeDimensions_DimensionsAreZero()
        {
            int width = -1920;
            int height = -1080;
            Position origin = new(0, 0);
            DisplayData data = new(origin, width, height);

            Assert.That(data.Origin, Is.EqualTo(origin));
            Assert.That(data.Width, Is.Zero);
            Assert.That(data.Height, Is.Zero);
        }

        [Test]
        public void CanCompareEquality_EqualDisplays_ReturnsTrue()
        {
            int width = 100;
            int height = 100;
            Position origin = new(990, -190);

            DisplayData display1 = new(origin, width, height);
            DisplayData display2 = new(origin, width, height);

            Assert.That(display1 == display2, Is.True);
        }

        [Test]
        public void CanCompareInequality_EqualDisplays_ReturnsFalse()
        {
            int width = 100;
            int height = 100;
            Position origin = new(990, -190);

            DisplayData display1 = new(origin, width, height);
            DisplayData display2 = new(origin, width, height);

            Assert.That(display1 != display2, Is.False);
        }

        [Test]
        public void CanCompareEquality_InequalWidth_ReturnsFalse()
        {
            int width = 100;
            int height = 100;
            Position origin = new(990, -190);

            DisplayData display1 = new(origin, width, height);
            DisplayData display2 = new(origin, width + 1, height);

            Assert.That(display1 == display2, Is.False);
        }

        [Test]
        public void CanCompareEquality_InequalHeight_ReturnsFalse()
        {
            int width = 100;
            int height = 100;
            Position origin = new(990, -190);

            DisplayData display1 = new(origin, width, height);
            DisplayData display2 = new(origin, width, height + 1);

            Assert.That(display1 == display2, Is.False);
        }

        [Test]
        public void CanCompareEquality_InequalOrigin_ReturnsFalse()
        {
            int width = 100;
            int height = 100;
            Position origin1 = new(990, -190);
            Position origin2 = new(-990, 190);

            DisplayData display1 = new(origin1, width, height);
            DisplayData display2 = new(origin2, width, height);

            Assert.That(display1 == display2, Is.False);
        }

        [Test]
        public void CanCompareEquality_InequalAll_ReturnsFalse()
        {
            int width = 100;
            int height = 100;
            Position origin1 = new(990, -190);
            Position origin2 = new(-990, 190);

            DisplayData display1 = new(origin1, width, height);
            DisplayData display2 = new(origin2, width + 1000, height - 50);

            Assert.That(display1 == display2, Is.False);
        }
    }
}
