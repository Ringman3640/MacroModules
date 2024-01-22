using MacroModules.MacroLibrary;
using MacroModules.MacroLibrary.Types;

namespace MacroLibrary.UnitTests
{
    internal class DisplayPositionTranslatorTests
    {
        public DisplayPositionTranslatorTests()
        {
            HashSet<DisplayData> displaysToInsert = [new DisplayData(new Position(0, 0), 1920, 1080)];
            singleDisplay1080 = new(
                primaryScreenWidth: 1920,
                primaryScreenHeight: 1080,
                virtualScreenOrigin: new(0, 0),
                virtualScreenWidth: 1920,
                virtualScreenHeight: 1080,
                displays: displaysToInsert
                );

            displaysToInsert = [new DisplayData(new Position(0, 0), 1280, 720)];
            singleDisplay720 = new(
                primaryScreenWidth: 1280,
                primaryScreenHeight: 720,
                virtualScreenOrigin: new(0, 0),
                virtualScreenWidth: 1280,
                virtualScreenHeight: 720,
                displays: displaysToInsert
                );

            displaysToInsert = [
                new DisplayData(new Position(0, 0), 1920, 1080),
                new DisplayData(new Position(-1920, 0), 1920, 1080)
            ];
            dualDisplay1080 = new(
                primaryScreenWidth: 1920,
                primaryScreenHeight: 1080,
                virtualScreenOrigin: new(-1920, 0),
                virtualScreenWidth: 3840,
                virtualScreenHeight: 1080,
                displays: displaysToInsert
                );
        }

        [SetUp]
        public void Setup()
        {
            DisplayPositionTranslator.RemoveNativeSystem();
            DisplayPositionTranslator.SetCurrentSystemForTesting(new SystemDisplayData());
        }

        [Test, NonParallelizable]
        public void TranslateWithoutNativeSystem_PrimaryOrigin_ReturnsPrimaryOrigin()
        {
            Position origin = new(0, 0);
            Position translatedPos = DisplayPositionTranslator.Translate(origin);
            Assert.That(translatedPos, Is.EqualTo(origin));
        }

        [Test, NonParallelizable]
        public void TranslateWithoutNativeSystem_MaxPosition_ReturnsMaxPosition()
        {
            Position maxPosition = new(int.MaxValue, int.MaxValue);
            Position translatedPos = DisplayPositionTranslator.Translate(maxPosition);
            Assert.That(translatedPos, Is.EqualTo(maxPosition));
        }

        [Test, NonParallelizable]
        public void TranslateWithoutNativeSystem_MinPosition_ReturnsMinPosition()
        {
            Position minPosition = new(int.MinValue, int.MinValue);
            Position translatedPos = DisplayPositionTranslator.Translate(minPosition);
            Assert.That(translatedPos, Is.EqualTo(minPosition));
        }

        [Test, NonParallelizable]
        public void TranslateWithCurrentAsNativeSystem_PrimaryOrigin_ReturnsPrimaryOrigin()
        {
            Position origin = new(0, 0);
            DisplayPositionTranslator.SetNativeSystem(new SystemDisplayData());
            Position translatedPos = DisplayPositionTranslator.Translate(origin);
            Assert.That(translatedPos, Is.EqualTo(origin));
        }

        [Test, NonParallelizable]
        public void TranslateWithCurrentAsNativeSystem_MaxPosition_ReturnsMaxPosition()
        {
            Position maxPosition = new(int.MaxValue, int.MaxValue);
            DisplayPositionTranslator.SetNativeSystem(new SystemDisplayData());
            Position translatedPos = DisplayPositionTranslator.Translate(maxPosition);
            Assert.That(translatedPos, Is.EqualTo(maxPosition));
        }

        [Test, NonParallelizable]
        public void TranslateWithCurrentAsNativeSystem_MinPosition_ReturnsMinPosition()
        {
            Position minPosition = new(int.MinValue, int.MinValue);
            DisplayPositionTranslator.SetNativeSystem(new SystemDisplayData());
            Position translatedPos = DisplayPositionTranslator.Translate(minPosition);
            Assert.That(translatedPos, Is.EqualTo(minPosition));
        }

        [Test, NonParallelizable]
        public void TranslateWithSingleDisplay1080CurrentAndNativeSystem_10IntervalPoints_ReturnsSamePointEachTime()
        {
            DisplayPositionTranslator.SetCurrentSystemForTesting(singleDisplay1080);
            DisplayPositionTranslator.SetNativeSystem(singleDisplay1080);
            for (int x = 0; x < 1920; x+= 10)
            {
                for (int y = 0; y < 1080; y += 10)
                {
                    Position currPos = new(x, y);
                    Position translatedPos = DisplayPositionTranslator.Translate(currPos);
                    Assert.That(translatedPos, Is.EqualTo(currPos));
                }
            }
        }

        [Test, NonParallelizable]
        public void TranslateWithCurrent720Native1080_PrimaryOrigin_ReturnsPrimaryOrigin()
        {
            Position origin = new(0, 0);
            DisplayPositionTranslator.SetCurrentSystemForTesting(singleDisplay720);
            DisplayPositionTranslator.SetNativeSystem(singleDisplay1080);
            Position translatedPos = DisplayPositionTranslator.Translate(origin);
            Assert.That(translatedPos, Is.EqualTo(origin));
        }

        [Test, NonParallelizable]
        public void TranslateWithCurrent720Native1080_BottomRightNativePosition_ReturnsBottomRightCurrentPosition()
        {
            DisplayPositionTranslator.SetCurrentSystemForTesting(singleDisplay720);
            DisplayPositionTranslator.SetNativeSystem(singleDisplay1080);
            Position nativeBottomRight = new(1919, 1079);
            Position translatedPos = DisplayPositionTranslator.Translate(nativeBottomRight);
            Assert.That(translatedPos, Is.EqualTo(new Position(1279, 719)));
        }

        [Test, NonParallelizable]
        public void TranslateWithCurrent720Native1080_PositionX100Y100_ReturnsAccurateTranslation()
        {
            DisplayPositionTranslator.SetCurrentSystemForTesting(singleDisplay720);
            DisplayPositionTranslator.SetNativeSystem(singleDisplay1080);
            Position nativePos = new(100, 100);
            Position translatedPos = DisplayPositionTranslator.Translate(nativePos);

            Position expectedValue = nativePos;
            double scaler = 1280.0 / 1920.0;
            expectedValue.X = (int)(expectedValue.X * scaler);
            expectedValue.Y = (int)(expectedValue.Y * scaler);

            Assert.That(translatedPos.X, Is.EqualTo(expectedValue.X));
            Assert.That(translatedPos.Y, Is.EqualTo(expectedValue.Y));
        }

        [Test, NonParallelizable]
        public void TranslateWithCurrent1080Native720_PositionX100Y100_ReturnsAccurateTranslation()
        {
            DisplayPositionTranslator.SetCurrentSystemForTesting(singleDisplay1080);
            DisplayPositionTranslator.SetNativeSystem(singleDisplay720);
            Position nativePos = new(100, 100);
            Position translatedPos = DisplayPositionTranslator.Translate(nativePos);

            Position expectedValue = nativePos;
            double scaler = 1920.0 / 1280.0;
            expectedValue.X = (int)(expectedValue.X * scaler);
            expectedValue.Y = (int)(expectedValue.Y * scaler);

            Assert.That(translatedPos.X, Is.EqualTo(expectedValue.X));
            Assert.That(translatedPos.Y, Is.EqualTo(expectedValue.Y));
        }

        [Test, NonParallelizable]
        public void TranslateWithCurrentSingle720NativeDual1080_PositionXnegative434Y1003_ReturnsAccurateTranslation()
        {
            DisplayPositionTranslator.SetCurrentSystemForTesting(singleDisplay720);
            DisplayPositionTranslator.SetNativeSystem(dualDisplay1080);
            Position nativePos = new(-434, 1003);
            Position translatedPos = DisplayPositionTranslator.Translate(nativePos);

            Position expectedValue = new(nativePos.X - -1920, nativePos.Y);
            double scaler = 1280.0 / 1920.0;
            expectedValue.X = (int)(expectedValue.X * scaler);
            expectedValue.Y = (int)(expectedValue.Y * scaler);

            Assert.That(translatedPos.X, Is.EqualTo(expectedValue.X));
            Assert.That(translatedPos.Y, Is.EqualTo(expectedValue.Y));
        }

        private SystemDisplayData singleDisplay1080;
        private SystemDisplayData singleDisplay720;
        private SystemDisplayData dualDisplay1080;
    }
}
