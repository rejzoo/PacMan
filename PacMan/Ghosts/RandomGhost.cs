using PacMan.Maps;
using PacMan.Player;
using System.Windows.Media.Imaging;

namespace PacMan.Ghosts
{
    internal class RandomGhost(BitmapImage spriteSheet, GhostType ghostType) : Ghost(spriteSheet, ghostType)
    {
        public override void PathFinding(Map map, PacManPlayer player)
        {
            RandomMovement(map);
        }
    }
}
