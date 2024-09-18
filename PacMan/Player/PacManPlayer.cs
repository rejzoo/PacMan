using PacMan.Sprites;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PacMan.Player
{
    public class PacManPlayer
    {
        private AnimatedSprite? _sprite;
        private int _xDirection = 0;
        private int _yDirection = 0;
        private int _xDestinationCord;
        private int _yDestinationCord;
        private readonly int _spriteSize = 32;
        private readonly int _speed = 8; //16

        public string CurrentOrientation { get; set; } = "RIGHT";
        public int XCordNorm { get { return XCord / _spriteSize; } }
        public int YCordNorm { get { return YCord / _spriteSize; } }
        public int XCord { get; set; } = 384;
        public int YCord { get; set; } = 480;
        public int Lifes { get; set; } = 3;
        public PacManPlayer(BitmapImage spriteSheet)
        {
            _xDestinationCord = XCord;
            _yDestinationCord = YCord;
            CreateAnimatedSprite(spriteSheet);
        }

        private void CreateAnimatedSprite(BitmapImage spriteSheet)
        {
            Int32Rect[] cords =
            [
                new Int32Rect(103, 709, 15, 15),
                new Int32Rect(103, 692, 15, 15)
            ];

            _sprite = new AnimatedSprite(spriteSheet, cords);
        }

        public void Movement(string direction)
        {
            switch (direction)
            {
                case "UP":
                    if (CheckX()) break;
                    CurrentOrientation = direction;
                    _xDirection = 0;
                    _yDirection = -1;
                    _sprite!.UpdateRotation(270);
                break;
                case "DOWN":
                    if (CheckX()) break;
                    CurrentOrientation = direction;
                    _xDirection = 0;
                    _yDirection = 1;
                    _sprite!.UpdateRotation(90);
                break;
                case "LEFT":
                    if (CheckY()) break;
                    CurrentOrientation = direction;
                    _xDirection = -1;
                    _yDirection = 0;
                    _sprite!.UpdateRotation(180);
                break;
                case "RIGHT":
                    if (CheckY()) break;
                    CurrentOrientation = direction;
                    _xDirection = 1;
                    _yDirection = 0;
                    _sprite!.UpdateRotation(0);
                break;
            }
        }

        private bool CheckX()
        {
            return XCord % _spriteSize != 0;
        }

        private bool CheckY()
        {
            return YCord % _spriteSize != 0;
        }

        public void Update()
        {
            if (XCord != _xDestinationCord)
            {
                XCord += _xDirection * _speed;
            }

            if (YCord != _yDestinationCord)
            {
                YCord += _yDirection * _speed;
            }
        }

        public void UpdateAnimation()
        {
            _sprite!.Update();
        }

        public void Draw(DrawingContext dc)
        {
            _sprite!.Draw(dc, new Point(XCord, YCord));
        }

        public void NewDestination(int xCord, int yCord)
        {
            _xDestinationCord = xCord * _spriteSize;
            _yDestinationCord = yCord * _spriteSize;
        }

        public void ResetToStart()
        {
            XCord = 384;
            YCord = 480;
            _xDirection = 0;
            _yDirection = 0;
        }

        public void TakeDamage()
        {
            Lifes--;
        }

        public bool IsDead()
        {
            return Lifes == 0;
        }
    }
}
