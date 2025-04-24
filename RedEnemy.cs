using System;
using Microsoft.Xna.Framework;

namespace MAZEGAME
{
    public class RedEnemy : Enemy
    {
        public RedEnemy() : base()
        {
            
        }

        public override void setInitalState()
        {
            positionX = 13;
            positionY = 11;
            currentDirection = Direction.Right;
            currentEnemy = new Rectangle(1371, 195, 42, 42);
            currentMode = EnemyMode.Scatter;
            leavingIndex = -1;
            previousModeBeforeFrightened = EnemyMode.Scatter;
            modeAfterLeavingHouse = EnemyMode.Scatter;
        }

        // Set the target position to be a random position in the top left of the maze
        public override (int, int) getScatterTargetPosition(Tile[,] tileArray) 
        {
            Random rng = new Random();
            int x, y;
            // Keep generating a random position in the top left until the position is a valid one
            do 
            {
                x = rng.Next(13); 
                y = rng.Next(9); 
            } while (!isTileMoveable(x, y, tileArray) || (x == positionX && y == positionY));
            return (x, y);
        }

        // Set the target position of the red enemy when in chase mode - towards the tile two in front of the player
        public override (int, int) getChaseTargetPosition((int, int) pacmanPosition, Direction pacmanDirection, Tile[,] tileArray, (int, int) redEnemyPosition) {
            int targetX = pacmanPosition.Item1;
            int targetY = pacmanPosition.Item2;

            // Calculate the position 2 tiles in front of the player depending on the direction
            if (pacmanDirection == Direction.Down)
            {
                if (targetY + 2 < 31) 
                {
                    targetY += 2;
                }
            }
            else if (pacmanDirection == Direction.Up)
            {
                if (targetY - 2 >= 0) 
                {
                    targetY -= 2;
                }
            }
            else if (pacmanDirection == Direction.Right)
            {
                if (targetX + 2 < 28 || targetY == 14) 
                {
                    targetX += 2;
                }
                if (targetY == 14 && targetX > 27)
                {
                    targetX = 0;
                }
            }
            else if (pacmanDirection == Direction.Left)
            {
                if (targetX - 2 >= 0 || targetY == 14) 
                {
                    targetX -= 2;
                }
                targetX -= 2;
                if (targetY == 14 && targetX < 0)
                {
                    targetX = 27;
                }
            }

            // If this tile is a valid one then this is the target for the red enemy
            if (isTileMoveable(targetX, targetY, tileArray)) 
            {
                return (targetX, targetY);
            }

            // If not, the target is the position of the player
            return (pacmanPosition.Item1, pacmanPosition.Item2);
        }

        // Return the red sprite facing in the current direction
        public override void updateDirection() 
        {
            if (currentDirection == Direction.Down)
            {
                currentEnemy = new Rectangle(1659, 195, 42, 42);
            }
            else if (currentDirection == Direction.Up)
            {
                currentEnemy = new Rectangle(1563, 195, 42, 42);
            }
            else if (currentDirection == Direction.Right)
            {
                currentEnemy = new Rectangle(1371, 195, 42, 42);
            }
            else if (currentDirection == Direction.Left)
            {
                currentEnemy = new Rectangle(1467, 195, 42, 42);
            }
        }
    }
}