using MacroModules.MacroLibrary.Types;
using static MacroModules.MacroLibrary.MouseControl;

namespace MacroLibrary.UnitTests
{
    public class MouseControlTests
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void CanSetCursorPos_PosTopLeftCorner_ReturnsTrue()
        {
            Position topLeft = new(0, 0);
            Assert.That(MoveCursor(topLeft), Is.True);
        }

        [Test]
        public void CanSetCursorPos_PosSquare_ReturnsTrue()
        {
            Position topLeft = new(100, 100);
            Position topRight = new(200, 100);
            Position bottomRight = new(200, 200);
            Position bottomLeft = new(100, 200);
            Assert.That(MoveCursor(topLeft), Is.True);
            Assert.That(MoveCursor(topRight), Is.True);
            Assert.That(MoveCursor(bottomRight), Is.True);
            Assert.That(MoveCursor(bottomLeft), Is.True);
        }

        [Test]
        public void CanSetCursorPos_OffsetDownRight_ReturnsTrue()
        {
            Offset move = new(200, 200);
            Assert.That(MoveCursor(move), Is.True);
        }

        [Test]
        public void CanSetCursorPos_OffsetSquare_ReturnsTrue()
        {
            Offset right = new(200, 0);
            Offset down = new(0, 200);
            Offset left = new(-200, 0);
            Offset up = new(0, -200);
            Assert.That(MoveCursor(right), Is.True);
            Assert.That(MoveCursor(down), Is.True);
            Assert.That(MoveCursor(left), Is.True);
            Assert.That(MoveCursor(up), Is.True);
        }

        [Test]
        public void CanSetCursorPos_LargePositiveValue_ReturnsTrue()
        {
            Position pos = new(9999999, 9999999);
            Assert.That(MoveCursor(pos), Is.True);
        }

        [Test]
        public void CanSetCursorPos_LargeNegativeValue_ReturnsTrue()
        {
            Position pos = new(-9999999, -9999999);
            Assert.That(MoveCursor(pos), Is.True);
        }

        [Test]
        public void CanGetCursorPos_CurrentPosition_ReturnsPos()
        {
            GetCursorPosition();
        }

        [Test, NonParallelizable]
        public void CanGetCursorPos_SetPosition_GetPositionIsSame()
        {
            Position pos = new(1000, 1000);
            MoveCursor(pos);
            Assert.That(GetCursorPosition(), Is.EqualTo(pos));
        }

        [Test, NonParallelizable]
        public void CanGetCursorPos_SetPositionRandom100_GetPositionIsSame()
        {
            Random rand = new();
            for (int i = 0; i < 100; ++i)
            {
                Position pos = new(rand.Next(1920), rand.Next(1080));
                MoveCursor(pos);
                Assert.That(GetCursorPosition(), Is.EqualTo(pos));
            }
        }

        [Test, NonParallelizable]
        public void CanGetCursorPos_SetOffsetRight_GetPositionIsAccurate()
        {
            Position startPos = new(100, 100);
            Offset moveVect = new(120, 0);
            MoveCursor(startPos);
            MoveCursor(moveVect);
            Assert.That(GetCursorPosition(), Is.EqualTo(startPos + moveVect));
        }

        [Test, NonParallelizable]
        public void CanGetCursorPos_SetOffsetDown_GetPositionIsAccurate()
        {
            Position startPos = new(100, 100);
            Offset moveVect = new(0, 230);
            MoveCursor(startPos);
            MoveCursor(moveVect);
            Assert.That(GetCursorPosition(), Is.EqualTo(startPos + moveVect));
        }

        [Test]
        public void CanScroll_Up_ReturnsTrue()
        {
            Assert.That(Scroll(120) == true);
        }

        [Test]
        public void CanScroll_Down_ReturnsTrue()
        {
            Assert.That(Scroll(-120) == true);
        }

        [Test]
        public void CanScroll_Right_ReturnsTrue()
        {
            Assert.That(HorizontalScroll(120) == true);
        }

        [Test]
        public void CanScroll_Left_ReturnsTrue()
        {
            Assert.That(HorizontalScroll(-120) == true);
        }
    }
}