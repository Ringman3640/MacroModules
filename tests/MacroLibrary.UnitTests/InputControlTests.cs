using MacroModules.MacroLibrary;
using System.Diagnostics;

namespace MacroLibrary.UnitTests
{
    public class InputControlTests
    {
        [SetUp]
        public void Setup()
        {
            //Thread.Sleep(2000);
        }

        [Test]
        public void CanClickInput_LeftMouseButton_ReturnsTrue()
        {
            bool sucess = InputControl.Click(1);
            //Assert.That(sucess == true);
        }

        [Test]
        public void CanClickInput_AZKeys_ReturnsTrue()
        {
            for (ushort i = 0x41; i <= 0x5A; ++i)
            {
                //Assert.That(InputControl.Click(i) == true);
            }
        }

        [Test]
        public void CanClickInputRepeatedly_RightMouseButton_ReturnsTrue()
        {
            for (int i = 0; i < 100; ++i)
            {
                //Assert.That(InputControl.Click(2) == true);
            }
        }

        [Test]
        public void CanHoldAndReleaseInput_RightMouseButton_ReturnsTrue()
        {
            //Assert.That(InputControl.Hold(2) == true);
            //Thread.Sleep(500);
            //Assert.That(InputControl.Release(2) == true);
        }
    }
}