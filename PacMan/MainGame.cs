using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows.Input;
using PacMan.Player;
using PacMan.Maps;
using PacMan.Ghosts;
using System.Windows;
using PacMan.Gui;

namespace PacMan
{
    public class MainGame : Canvas
    {
        private readonly DispatcherTimer _gameTimer = new();
        private readonly PacManPlayer _player;
        private readonly MovementInput _movementInput;
        private readonly Map _mainMap;
        private readonly MainGUI _mainGUI;
        private readonly List<Ghost> _listOfGhosts = [];
        private DateTime _weakModeStartTime;
        private bool _weakMode = false;

        public MainGame(BitmapImage spriteSheet, Map mainMap, MainGUI mainGUI)
        {
            _gameTimer.Interval = TimeSpan.FromSeconds(0.1);
            _gameTimer.Tick += TimerTick;

            _player = new PacManPlayer(spriteSheet);
            _movementInput = new MovementInput(_player);

            _mainMap = mainMap;

            _mainGUI = mainGUI;

            CreateGhosts(spriteSheet);
        }

        protected override void OnRender(DrawingContext dc)
        {
            _player.Draw(dc);
            DrawGhosts(dc);
            base.OnRender(dc);
        }

        private void TimerTick(object? sender, EventArgs e)
        {
            _movementInput.MoveInDire(_player.CurrentOrientation, _mainMap);
            _player.Update();
            _player.UpdateAnimation();

            UpdateGhosts();
            UpdateWeakMode();
            CheckIfGhostEated();

            int typeOfPickUp = _mainMap.CheckPickUps(_player.XCordNorm, _player.YCordNorm);
            HandlePickUps(typeOfPickUp);

            CheckConditionsForMapReset();

            InvalidateVisual();
        }

        public void KeyBoardInput(KeyEventArgs e)
        {
            if (e.Key == Key.Space) StartGame();

            CheckKeyPress(e);
        }

        public void CheckKeyPress(KeyEventArgs e)
        {
            if (e.Key == Key.W && _mainMap.MovableTo(_player.XCordNorm, _player.YCordNorm - 1)) _movementInput.HandleInput(e);
            if (e.Key == Key.S && _mainMap.MovableTo(_player.XCordNorm, _player.YCordNorm + 1)) _movementInput.HandleInput(e);
            if (e.Key == Key.A && _mainMap.MovableTo(_player.XCordNorm - 1, _player.YCordNorm)) _movementInput.HandleInput(e);
            if (e.Key == Key.D && _mainMap.MovableTo(_player.XCordNorm + 1, _player.YCordNorm)) _movementInput.HandleInput(e);
        }

        public void StartGame()
        {
            _gameTimer.Start();
        }

        public void StopGame()
        {
            _gameTimer.Stop();
        }

        private void HandlePickUps(int typeOfPickUp)
        {
            if (typeOfPickUp == -1) return;

            if (typeOfPickUp == 0)
            {
                _mainGUI.AddScore(10);
            }
            else if (typeOfPickUp == 1)
            {
                _mainGUI.AddScore(50);
                StartWeakGhosts();
            }

            _mainGUI.ReDraw();
        }

        private void CreateGhosts(BitmapImage spriteSheet)
        {
            _listOfGhosts.Add(new RoamingGhost(spriteSheet, GhostType.Blinky));
            _listOfGhosts.Add(new RoamingGhost(spriteSheet, GhostType.Inky));
            _listOfGhosts.Add(new RandomGhost(spriteSheet, GhostType.Pinky));
            _listOfGhosts.Add(new RandomGhost(spriteSheet, GhostType.Clyde));
        }

        private void UpdateGhosts()
        {
            foreach (var ghost in _listOfGhosts)
            {
                ghost.PathFinding(_mainMap, _player);
                ghost.Update();
                ghost.UpdateAnimation();
            }
        }

        private void DrawGhosts(DrawingContext dc)
        {
            foreach (var ghost in _listOfGhosts)
            {
                ghost.Draw(dc);
            }
        }

        private void StartWeakGhosts()
        {
            if (_weakMode)
            {
                _weakModeStartTime = _weakModeStartTime.AddSeconds(10);
                return;
            }

            _weakModeStartTime = DateTime.Now;

            foreach (var ghost in _listOfGhosts)
            {
                _weakMode = true;
                ghost.Weak = true;
            }
        }

        private void StopWeakGhosts()
        {
            foreach (var ghost in _listOfGhosts)
            {
                _weakMode = false;
                ghost.Weak = false;
            }
        }

        private void UpdateWeakMode()
        {
            if (!_weakMode) return;

            if (DateTime.Now - _weakModeStartTime >= TimeSpan.FromSeconds(8))
            {
                _weakMode = false;
                StopWeakGhosts();
            }
        }

        private void CheckConditionsForMapReset()
        {
            bool resetGame = false;
            bool noPickUps = _mainMap.NoPickUps();
            bool playerDead = _player.IsDead();
            bool ghostHit = GhostHit() != null;

            if (noPickUps) resetGame = true;
            if (playerDead) resetGame = true;
            if (ghostHit && !_weakMode) resetGame = true;

            if (resetGame)
            {
                ResetGame(noPickUps, playerDead, ghostHit);
            }
        }

        private void CheckIfGhostEated()
        {
            if (!_weakMode) return;

            Ghost? hitGhost = GhostHit();
            if (hitGhost != null)
            {
                _mainGUI.AddScore(200);
                hitGhost.ResetToStart();
                hitGhost.Weak = false;
                hitGhost.FreezeTime = DateTime.Now;
            }
        }

        private Ghost? GhostHit()
        {
            int spriteSize = 32;

            int playerX = _player.XCord;
            int playerY = _player.YCord;

            foreach (var ghost in _listOfGhosts)
            {
                int ghostX = ghost.XCord;
                int ghostY = ghost.YCord;

                bool collisionX = playerX < ghostX + spriteSize && playerX + spriteSize > ghostX;
                bool collisionY = playerY < ghostY + spriteSize && playerY + spriteSize > ghostY;

                if (collisionX && collisionY)
                {
                    return ghost;
                }
            }

            return null;
        }

        private void ResetGame(bool noPickUps, bool playerDead, bool ghostHit)
        {
            if (noPickUps)
            {
                _mainMap.ResetMap();
                ResetAllEntities();
            }

            if (playerDead)
            {
                _mainMap.ResetMap();
                _mainGUI.SaveHighScore();
                ResetAllEntities();
                Application.Current.Shutdown();
            }

            if (ghostHit)
            {
                ResetAllEntities();
                _player.TakeDamage();
                _mainGUI.UpdateAndDrawHP(_player.Lifes);
                _mainGUI.ReDraw();
            }
        }

        private void ResetAllEntities()
        {
            _player.ResetToStart();
            
            foreach (var ghost in _listOfGhosts)
            {
                ghost.ResetToStart();
            }
        }
    }
}