using PacMan.Sprites;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PacMan.Gui
{
    public class MainGUI : Canvas
    {
        private readonly List<Sprite> _lettersSpritesPacManTextHS = [];
        private readonly List<Sprite> _lettersSpritesPacManTextTS = [];
        private readonly List<Sprite> _numbers = [];
        private List<Sprite> _numbersToRender = [];
        private List<Sprite> _numbersToRenderHS = [];
        private readonly List<Sprite> _pacManHealth = [];
        private readonly List<Sprite> _pacManGuiToRender = [];
        private readonly BitmapImage _spriteSheet;
        private int _score = 0;
        private int _highScore = 0;
        private readonly string _highScoreFile = "highScore.json";

        public MainGUI(BitmapImage spriteSheet)
        {
            _spriteSheet = spriteSheet;

            LoadHighScore();
            LoadTopScoreText();
            LoadHighScoreText();
            LoadNumbersText();
            LoadPacManHealth();
        }

        protected override void OnRender(DrawingContext dc)
        {
            int spacing = 32;

            DrawLetters(dc, spacing);

            DrawScore(dc, spacing);

            DrawPacManHealth(dc, spacing);
        }

        private void DrawLetters(DrawingContext dc, int spacing)
        {
            int startXCordText = 100;
            int startYCordText = 20;

            foreach (Sprite sprite in _lettersSpritesPacManTextTS)
            {
                sprite.Draw(dc, new Point(startXCordText, startYCordText));
                startXCordText += spacing;
            }

            startXCordText += 100;

            foreach (Sprite sprite in _lettersSpritesPacManTextHS)
            {
                sprite.Draw(dc, new Point(startXCordText, startYCordText));
                startXCordText += spacing;
            }
        }

        private void DrawScore(DrawingContext dc, int spacing)
        {
            int startXCordNumberActual = 100;
            int startYCordNumber = 50;

            int startXCordNumberHighScore = 300;
            foreach (Sprite sprite in _numbersToRender)
            {
                sprite.Draw(dc, new Point(startXCordNumberActual, startYCordNumber));
                startXCordNumberActual += spacing;

                if (_score >= _highScore)
                {
                    sprite.Draw(dc, new Point(startXCordNumberHighScore, startYCordNumber));
                    startXCordNumberHighScore += spacing;
                }
            }

            if (_highScore > _score)
            {
                foreach (Sprite sprite in _numbersToRenderHS)
                {
                    sprite.Draw(dc, new Point(startXCordNumberHighScore, startYCordNumber));
                    startXCordNumberHighScore += spacing;
                }
            }
        }

        private void DrawPacManHealth(DrawingContext dc, int spacing)
        {
            int startXCordPacManGui = 20;
            int startYCordPacManGui = 920;

            foreach (Sprite sprite in _pacManGuiToRender)
            {
                sprite.Draw(dc, new Point(startXCordPacManGui, startYCordPacManGui));
                startXCordPacManGui += spacing;
            }
        }

        public void ReDraw()
        {
            InvalidateVisual();
        }

        public void AddScore(int scoreToAdd)
        {
            _score += scoreToAdd;
            _numbersToRender = PrepareNumbersToRender(_score);
        }

        public void UpdateAndDrawHP(int lifes)
        {
            _pacManGuiToRender.Clear();
            for (int i = 0; i < lifes; i++)
            {
                _pacManGuiToRender.Add(_pacManHealth[0]);
            }
        }

        public void SaveHighScore()
        {
            if (_score > _highScore)
            {
                try
                {
                    string json = JsonSerializer.Serialize(_score);
                    File.WriteAllText(_highScoreFile, json);
                }
                catch (Exception)
                {
                    Console.WriteLine("Could not write!");
                }
            }
        }

        private void LoadTopScoreText()
        {
            _lettersSpritesPacManTextTS.Add(new Sprite(_spriteSheet, new Int32Rect(55, 595, 8, 8), true));
            _lettersSpritesPacManTextTS.Add(new Sprite(_spriteSheet, new Int32Rect(10, 595, 8, 8), true));
            _lettersSpritesPacManTextTS.Add(new Sprite(_spriteSheet, new Int32Rect(19, 595, 8, 8), true));
        }

        private void LoadHighScoreText()
        {
            _lettersSpritesPacManTextHS.Add(new Sprite(_spriteSheet, new Int32Rect(64, 586, 8, 8), true));
            _lettersSpritesPacManTextHS.Add(new Sprite(_spriteSheet, new Int32Rect(73, 586, 8, 8), true));
            _lettersSpritesPacManTextHS.Add(new Sprite(_spriteSheet, new Int32Rect(55, 586, 8, 8), true));
            _lettersSpritesPacManTextHS.Add(new Sprite(_spriteSheet, new Int32Rect(64, 586, 8, 8), true));

            _lettersSpritesPacManTextHS.Add(new Sprite(_spriteSheet, new Int32Rect(46, 595, 8, 8), true));
            _lettersSpritesPacManTextHS.Add(new Sprite(_spriteSheet, new Int32Rect(19, 586, 8, 8), true));
            _lettersSpritesPacManTextHS.Add(new Sprite(_spriteSheet, new Int32Rect(10, 595, 8, 8), true));
            _lettersSpritesPacManTextHS.Add(new Sprite(_spriteSheet, new Int32Rect(37, 595, 8, 8), true));
            _lettersSpritesPacManTextHS.Add(new Sprite(_spriteSheet, new Int32Rect(37, 586, 8, 8), true));
        }

        private void LoadNumbersText()
        {
            _numbers.Add(new Sprite(_spriteSheet, new Int32Rect(1, 559, 8, 8), true));
            _numbers.Add(new Sprite(_spriteSheet, new Int32Rect(10, 559, 8, 8), true));
            _numbers.Add(new Sprite(_spriteSheet, new Int32Rect(19, 559, 8, 8), true));
            _numbers.Add(new Sprite(_spriteSheet, new Int32Rect(28, 559, 8, 8), true));
            _numbers.Add(new Sprite(_spriteSheet, new Int32Rect(37, 559, 8, 8), true));
            _numbers.Add(new Sprite(_spriteSheet, new Int32Rect(46, 559, 8, 8), true));
            _numbers.Add(new Sprite(_spriteSheet, new Int32Rect(55, 559, 8, 8), true));
            _numbers.Add(new Sprite(_spriteSheet, new Int32Rect(64, 559, 8, 8), true));
            _numbers.Add(new Sprite(_spriteSheet, new Int32Rect(73, 559, 8, 8), true));
            _numbers.Add(new Sprite(_spriteSheet, new Int32Rect(82, 559, 8, 8), true));

            _numbersToRender.Add(_numbers[0]);

            _numbersToRenderHS = PrepareNumbersToRender(_highScore);
        }

        private void LoadPacManHealth()
        {
            _pacManHealth.Add(new Sprite(_spriteSheet, new Int32Rect(303, 709, 16, 16)));
            UpdateAndDrawHP(3);
        }

        private void LoadHighScore()
        {
            try
            {
                string json = File.ReadAllText(_highScoreFile);
                _highScore = JsonSerializer.Deserialize<int>(json);
            }
            catch (Exception)
            {
                Console.WriteLine("Could not load!");
                _highScore = 0;
            }
        }

        private List<Sprite> PrepareNumbersToRender(int numberToRender)
        {
            List<int> digits = [];
            List<Sprite> numbersToRender = [];

            while (numberToRender > 0)
            {
                int digit = numberToRender % 10;
                digits.Add(digit);
                numberToRender /= 10;
            }

            digits.Reverse();

            foreach (int digit in digits)
            {
                numbersToRender.Add(_numbers[digit]);
            }

            return numbersToRender;
        }
    }
}