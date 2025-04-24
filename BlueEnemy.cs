using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MAZEGAME
{
    public class BlueEnemy : Enemy
    {
        public BlueEnemy() : base(){
        }

        public override void setInitalState()
        {
            positionX = 12;
            positionY = 14;
            currentDirection = Direction.Right;
            currentEnemy = new Rectangle(1371, 291, 42, 42);
            currentMode = EnemyMode.InsideHouse;
            leavingPath = new List<(int, int)> { (12, 14), (13, 14), (13, 13), (13, 12), (13, 11) }.ToArray();
            leavingIndex = -1;
            previousModeBeforeFrightened = EnemyMode.Scatter;
            modeAfterLeavingHouse = EnemyMode.Scatter;
        }

        // Set the target position to be a random position in the bottom right of the maze
        public override (int, int) getScatterTargetPosition(Tile[,] tileArray) 
        {
            Random rng = new Random();
            int x, y;
            do {
                // Keep generating a random position in the bottom right until the position is a valid one
                x = rng.Next(15, 27); 
                y = rng.Next(20, 30); 
            } while (!isTileMoveable(x, y, tileArray) || (x == positionX && y == positionY));
            return (x, y);
        }

        // Set the target position of the blue enemy when in chase mode - towards the player but according to the red enemies position
        public override (int, int) getChaseTargetPosition((int, int) pacmanPosition, Direction pacmanDirection, Tile[,] tileArray, (int, int) redEnemyPosition) 
        {
            int targetX = pacmanPosition.Item1;
            int targetY = pacmanPosition.Item2;

            // Calculate two in front of the players position
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

            // Work out the vector between this tile and the red enemies position
            int vectorX = redEnemyPosition.Item1 - targetX;
            int vectorY = redEnemyPosition.Item2 - targetY;

            // Double this vector
            vectorX *= 2;
            vectorY *= 2;

            // The tile the vector lands on is the blue enemy's target tile.
            targetX += vectorX;
            targetY += vectorY;

            // If this target is valid then return this else return the players direct position
            if (targetX >= 0 && targetX < 28 && targetY >= 0 && targetY < 31 && isTileMoveable(targetX, targetY, tileArray)  ) 
            {
                return (targetX, targetY);
            }

            return (pacmanPosition.Item1, pacmanPosition.Item2);
        }

        // Return the blue sprite facing in the current direction
        public override void updateDirection() 
        {
            if (currentDirection == Direction.Down)
            {
                currentEnemy = new Rectangle(1659, 291, 42, 42);
            }
            else if (currentDirection == Direction.Up)
            {
                currentEnemy = new Rectangle(1563, 291, 42, 42);
            }
            else if (currentDirection == Direction.Right)
            {
                currentEnemy = new Rectangle(1371, 291, 42, 42);
            }
            else if (currentDirection == Direction.Left)
            {
                currentEnemy = new Rectangle(1467, 291, 42, 42);
            }

        }
    }
}