using PacMan.Maps;
using PacMan.Player;
using System.Windows.Media.Imaging;

namespace PacMan.Ghosts
{
    internal class RoamingGhost(BitmapImage spriteSheet, GhostType ghostType) : Ghost(spriteSheet, ghostType)
    {
        public override void PathFinding(Map map, PacManPlayer player)
        {
            if (HasTarget(map, player))
            {
                Chase();
            }
            else
            {
                RandomMovement(map);
            }
        }

        private bool HasTarget(Map map, PacManPlayer player)
        {
            return false;
        } 

        private void Chase()
        {
            throw new NotImplementedException();
        }
    }
}
