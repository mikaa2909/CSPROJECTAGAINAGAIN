using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MAZEGAME
{
    public class PinkEnemy : Enemy
    {

        public PinkEnemy() : base()
        {
        }

        public override void setInitalState()
        {
            positionX = 13;
            positionY = 14;
            currentDirection = Direction.Right;
            currentEnemy = new Rectangle(1371, 243, 42, 42);
            currentMode = EnemyMode.LeavingHouse;
            leavingPath = new List<(int, int)> { (13, 14), (13, 13), (13, 12), (13, 11) }.ToArray();
            leavingIndex = -1;
            previousModeBeforeFrightened = EnemyMode.Scatter;
            modeAfterLeavingHouse = EnemyMode.Scatter;
        }

        // Set the target position to be a random position in the top right of the maze
        public override (int, int) getScatterTargetPosition(Tile[,] tileArray) 
        {
            Random rng = new Random();
            int x, y;
            // Keep generating a random position in the top right until the position is a valid one
            do 
            {
                x = rng.Next(15, 27); 
                y = rng.Next(9); 
            } while (!isTileMoveable(x, y, tileArray) || (x == positionX && y == positionY));
            return (x, y);
        }

        // Set the target position of the pink enemy when in chase mode - towards the tile two behind the player
        public override (int, int) getChaseTargetPosition((int, int) pacmanPosition, Direction pacmanDirection, Tile[,] tileArray, (int, int) redEnemyPosition) {
                        int targetX = pacmanPosition.Item1;
            int targetY = pacmanPosition.Item2;

            // Calculate the position 2 tiles behind the player depending on the direction
            if (pacmanDirection == Direction.Up)
            {
                if (targetY + 2 < 31) 
                {
                    targetY += 2;
                }
            }
            else if (pacmanDirection == Direction.Down)
            {
                if (targetY - 2 >= 0) 
                {
                    targetY -= 2;
                }
            }
            else if (pacmanDirection == Direction.Left)
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
            else if (pacmanDirection == Direction.Right)
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
                currentEnemy = new Rectangle(1659, 243, 42, 42);
            }
            else if (currentDirection == Direction.Up)
            {
                currentEnemy = new Rectangle(1563, 243, 42, 42);
            }
            else if (currentDirection == Direction.Right)
            {
                currentEnemy = new Rectangle(1371, 243, 42, 42);
            }
            else if (currentDirection == Direction.Left)
            {
                currentEnemy = new Rectangle(1467, 243, 42, 42);
            }

        }
    }
}