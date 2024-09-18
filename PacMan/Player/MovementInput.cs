using System.Windows.Input;
using PacMan.Maps;

namespace PacMan.Player
{
    public class MovementInput(PacManPlayer player)
    {
        private readonly PacManPlayer _player = player;

        public void HandleInput(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    _player.Movement("UP");
                break;
                case Key.S:
                    _player.Movement("DOWN");
                break;
                case Key.D:
                    _player.Movement("RIGHT");
                break;
                case Key.A:
                    _player.Movement("LEFT");
                break;
            }
        }

        public void MoveInDire(string direction, Map mainMap)
        {
            int xCordNorm = _player.XCordNorm;
            int yCordNorm = _player.YCordNorm;

            int newX = xCordNorm, newY = yCordNorm;

            switch (direction)
            {
                case "UP":
                    newY--;
                break;
                case "DOWN":
                    newY++;
                break;
                case "LEFT":
                    newX--;
                break;
                case "RIGHT":
                    newX++;
                break;
            }

            if (mainMap.MovableTo(newX, newY)) _player.NewDestination(newX, newY);
            else _player.NewDestination(xCordNorm, yCordNorm);
        }
    }
}
