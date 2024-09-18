using PacMan.Sprites;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PacMan.Maps
{
    public class Map : Canvas
    {
        private readonly int[,] _mapMatrix;
        private readonly int _tileSize = 32;
        private readonly int _spriteSize = 8;
        private readonly int _numberOfTiles = 24;
        private readonly List<Sprite> _listOfSprites = [];
        private readonly string _pathToMap = "C:\\Users\\rejzh\\OneDrive\\Desktop\\škola\\4 semester\\Jazyk C# a .NET\\semestralka\\PacMan\\PacMan\\Maps\\DefaultMap.txt";

        public Map(BitmapImage spriteSheet)
        {
            _mapMatrix = new int[_numberOfTiles, _numberOfTiles];
            LoadMap(_pathToMap);
            LoadEveryPossibleMapSprite(spriteSheet);
        }

        private void LoadMap(string pathToFile)
        {
            try
            {
                string[] lines = File.ReadAllLines(pathToFile);

                for (int i = 0; i < _numberOfTiles; i++)
                {
                    string[] values = lines[i].Split(' ');
                    for (int j = 0; j < _numberOfTiles; j++)
                    {
                        _mapMatrix[j, i] = int.Parse(values[j]);
                    }
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("The file could not be opened!");
            }
        }

        private void LoadEveryPossibleMapSprite(BitmapImage spriteSheet)
        {
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    if (i == 6 && (j == 1 || j == 2))
                    {
                        continue;
                    }
                    var sprite = new Sprite(spriteSheet, new Int32Rect(145 + j * 9, 1 + i * 9, _spriteSize, _spriteSize), true);
                    _listOfSprites.Add(sprite);
                }
            }

            var item1 = new Sprite(spriteSheet, new Int32Rect(536, 568, _spriteSize, _spriteSize), true);
            _listOfSprites.Add(item1);
            var item2 = new Sprite(spriteSheet, new Int32Rect(536, 586, _spriteSize, _spriteSize), true);
            _listOfSprites.Add(item2);
        }

        protected override void OnRender(DrawingContext dc)
        {
            for (int i = 0; i < _numberOfTiles; i++)
            {
                for (int j = 0; j < _numberOfTiles; j++)
                {
                    var spriteNumber = _mapMatrix[i, j];
                    if (!(spriteNumber >= 0 && spriteNumber <= _listOfSprites.Count))
                    {
                        continue;
                    }

                    var sprite = _listOfSprites.ElementAt(spriteNumber);
                    sprite.Draw(dc, new Point((_tileSize * i), _tileSize * j));
                }
            }
        }

        public bool MovableTo(int xCordDest, int yCordDest)
        {
            if (xCordDest == -1 || xCordDest > _numberOfTiles - 2 ||
                yCordDest == -1 || yCordDest > _numberOfTiles - 2)
            {
                return false;
            }

            int typeOfTile = _mapMatrix[xCordDest, yCordDest];
            if (!(typeOfTile == 40 || typeOfTile == 41 || typeOfTile == 99))
            {
                return false;
            }

            return true;
        }

        public int CheckPickUps(int xCordNorm, int yCordNorm)
        {
            int typeOfTile = _mapMatrix[xCordNorm, yCordNorm];

            if (typeOfTile == 40)
            {
                _mapMatrix[xCordNorm, yCordNorm] = 99;
                this.InvalidateVisual();
                return 0;
            }
            else if (typeOfTile == 41)
            {
                _mapMatrix[xCordNorm, yCordNorm] = 99;
                this.InvalidateVisual();
                return 1;
            }

            return -1;
        }

        public bool NoPickUps()
        {
            for (int i = 0; i < _numberOfTiles; i++)
            {
                for (int j = 0; j < _numberOfTiles; j++)
                {
                    if (_mapMatrix[i, j] == 40 || _mapMatrix[i, j] == 41)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool AtIntersection(int xCord, int yCord, int direction, out List<int> possibleDirections)
        {
            possibleDirections = [];
            bool canChange = false;

            if (_mapMatrix[xCord, yCord] < -1)
            {
                possibleDirections.Add(PathOutOfHouse(xCord, yCord));
                return true;
            }

            if (_mapMatrix[xCord, yCord - 1] >= 40) // kontrola hore
            {
                if (!canChange) canChange = direction % 2 == 0;
                possibleDirections.Add(3);
            }
            if (_mapMatrix[xCord, yCord + 1] >= 40) // kontrola dole
            {
                if (!canChange) canChange = direction % 2 == 0;
                possibleDirections.Add(1);
            }
            if (_mapMatrix[xCord + 1, yCord] >= 40) // kontrola vpravo
            {
                if (!canChange) canChange = direction % 2 != 0;
                possibleDirections.Add(0);
            }
            if (_mapMatrix[xCord - 1, yCord] >= 40) // kontrola vlavo
            {
                if (!canChange) canChange = direction % 2 != 0;
                possibleDirections.Add(2);
            }

            return canChange;
        }

        public void ResetMap()
        {
            LoadMap(_pathToMap);
        }

        private int PathOutOfHouse(int xCordNorm, int yCordNorm)
        {
            int numberDirection = -999;
            int direction = 0;
            int number = _mapMatrix[xCordNorm - 1, yCordNorm];
            if (number < 0 && number > numberDirection)
            {
                direction = 2;
                numberDirection = number;
            }
            number = _mapMatrix[xCordNorm, yCordNorm - 1];
            if (number < 0 && number > numberDirection)
            {
                direction = 3;
                numberDirection = number;
            }
            number = _mapMatrix[xCordNorm + 1, yCordNorm];
            if (number < 0 && number > numberDirection)
            {
                direction = 0;
            }

            return direction;
        }
    }
}