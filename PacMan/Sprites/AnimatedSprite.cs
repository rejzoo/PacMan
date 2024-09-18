using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;

namespace PacMan.Sprites
{
    public class AnimatedSprite
    {
        private readonly Sprite[] _frames;
        private readonly int _frameCount;
        private int _currentFrame;
        private int _frameDirection = 1;
        private int _lastDirection = 0;

        public AnimatedSprite(BitmapImage spriteSheet, Int32Rect[] sourceRects)
        {
            _frameCount = sourceRects.Length;
            _frames = new Sprite[_frameCount];

            for (int i = 0; i < _frameCount; i++)
            {
                _frames[i] = new Sprite(spriteSheet, sourceRects[i]);
            }

            _currentFrame = 0;
        }

        public void Update(bool isGhost = false, int direction = 0)
        {
            int firstFrame = 0;
            int lastFrame = _frameCount - 1;

            if (isGhost)
            {             
                if (_lastDirection != direction)
                {
                    _frameDirection = 1;
                    _lastDirection = direction;
                }
                firstFrame = direction * 2;
                lastFrame = firstFrame + 1;
                _currentFrame = _frameDirection == 1 ? firstFrame : lastFrame;
            }

            _currentFrame += _frameDirection;
            
            if (_currentFrame == lastFrame || _currentFrame == firstFrame)
            {
                _frameDirection *= -1;
            }
        }

        public void Draw(DrawingContext dc, Point position)
        {
            _frames[_currentFrame].Draw(dc, position);
        }

        public void UpdateRotation(double angle)
        {
            for (int i = 0; i < _frameCount; i++)
            {
                _frames[i].Rotate(angle);
            }
        }
    }
}
