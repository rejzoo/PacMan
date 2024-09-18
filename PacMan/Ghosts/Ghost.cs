using PacMan.Maps;
using PacMan.Player;
using PacMan.Sprites;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PacMan.Ghosts
{
    public enum GhostType
    {
        Blinky,
        Pinky,
        Inky,
        Clyde
    }

    public static class GhostLoader
    {
        public static AnimatedSprite LoadSpriteNormal(BitmapImage spriteSheet, GhostType type)
        {
            int xCord = 0;

            switch (type)
            {
                case GhostType.Blinky:
                    xCord = 1;
                break;
                case GhostType.Pinky:
                    xCord = 201;
                break;
                case GhostType.Inky:
                    xCord = 401;
                break;
                case GhostType.Clyde:
                    xCord = 601;
                break;
            }

            Int32Rect[] cords =
            [
                new Int32Rect(xCord, 83, 15, 15),
                new Int32Rect(xCord + 17, 83, 15, 15),
                new Int32Rect(xCord + 34, 83, 15, 15),
                new Int32Rect(xCord + 51, 83, 15, 15),
                new Int32Rect(xCord + 68, 83, 15, 15),
                new Int32Rect(xCord + 85, 83, 15, 15),
                new Int32Rect(xCord + 102, 83, 15, 15),
                new Int32Rect(xCord + 119, 83, 15, 15),
            ];

            return new AnimatedSprite(spriteSheet, cords);
        }

        public static (int xCord, int yCord) GetCords(GhostType type)
        {
            int xCord = 0;
            int yCord = 0;

            switch (type)
            {
                case GhostType.Blinky:
                    xCord = 384;
                    yCord = 352;
                break;
                case GhostType.Pinky:
                    xCord = 320;
                    yCord = 352;
                break;
                case GhostType.Inky:
                    xCord = 416;
                    yCord = 352;
                break;
                case GhostType.Clyde:
                    xCord = 352;
                    yCord = 352;
                break;
            }

            return (xCord, yCord);
        }

        public static AnimatedSprite LoadSpriteWeak(BitmapImage spriteSheet)
        {
            Int32Rect[] cords =
            [
                new(1, 168, 15, 15),
                new(18, 354, 15, 15)
            ];

            return new AnimatedSprite(spriteSheet, cords);
        }
    }

    public abstract class Ghost
    {
        private readonly AnimatedSprite _spriteNormal;
        private readonly AnimatedSprite _spriteWeak;
        private readonly int _spriteSize = 32;
        protected int _direction = 0;
        protected int _speed = 4; //8
        protected int _xDirection = 0;
        protected int _yDirection = 0;
        protected List<int> _possibleDirections = [];
        private readonly GhostType _type;

        public bool Weak { get; set; } = false;
        public int XCord { get; set; }
        public int YCord { get; set; }
        public DateTime FreezeTime { get; set; }

        public Ghost(BitmapImage spriteSheet, GhostType ghostType)
        {
            _spriteNormal = GhostLoader.LoadSpriteNormal(spriteSheet, ghostType);
            _spriteWeak = GhostLoader.LoadSpriteWeak(spriteSheet);
            (XCord, YCord) = GhostLoader.GetCords(ghostType);
            _type = ghostType;
        }

        /*
         *  smer 0 - doprava
         *  smer 1 - dole
         *  smer 2 - vlavo
         *  smer 3 - hore
         */
        public void Update()
        {
            if (DateTime.Now - FreezeTime <= TimeSpan.FromSeconds(8)) return;

            switch (_direction)
            {
                case 0:
                    _xDirection = 1;
                    _yDirection = 0;
                break;
                case 1:
                    _xDirection = 0;
                    _yDirection = 1;
                break;
                case 2:
                    _xDirection = -1;
                    _yDirection = 0;
                break;
                case 3:
                    _xDirection = 0;
                    _yDirection = -1;
                break;
            }

            XCord += _xDirection * _speed;
            YCord += _yDirection * _speed;
        }

        public void UpdateAnimation()
        {
            if (Weak) _spriteWeak.Update();
            else _spriteNormal.Update(true, _direction);
        }

        public void Draw(DrawingContext dc)
        {
            if (Weak) _spriteWeak!.Draw(dc, new Point(XCord, YCord));
            else _spriteNormal!.Draw(dc, new Point(XCord, YCord));
        }

        public void RandomMovement(Map map)
        {
            DirectionCanBeChanged(map);
        }

        public void ResetToStart()
        {
            switch (_type)
            {
                case GhostType.Blinky:
                    XCord = 384;
                    YCord = 352;
                break;
                case GhostType.Pinky:
                    XCord = 320;
                    YCord = 352;
                break;
                case GhostType.Inky:
                    XCord = 416;
                    YCord = 352;
                break;
                case GhostType.Clyde:
                    XCord = 352;
                    YCord = 352;
                break;
            }
        }

        private void DirectionCanBeChanged(Map map)
        {
            _possibleDirections.Clear();
            if (map.AtIntersection(XCord / _spriteSize, YCord / _spriteSize, _direction, out _possibleDirections)
                && XCord % _spriteSize == 0 && YCord % _spriteSize == 0)
            {
                RandomDirectionChange();
            }
        }

        private void RandomDirectionChange()
        {
            Random random = new();
            int nextDirectionIndex = random.Next(0, _possibleDirections.Count);
            _direction = _possibleDirections[nextDirectionIndex];
        }

        public abstract void PathFinding(Map map, PacManPlayer player);
    }
}