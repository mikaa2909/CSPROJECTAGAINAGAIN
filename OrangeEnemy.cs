using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MAZEGAME
{
    public class OrangeEnemy : Enemy
    {
        public OrangeEnemy() : base()
        {
        }

        public override void setInitalState()
        {
            positionX = 15;
            positionY = 14;
            currentDirection = Direction.Right;
            currentEnemy = new Rectangle(1371, 339, 42, 42);
            currentMode = EnemyMode.InsideHouse;
            leavingPath = new List<(int, int)> { (15, 14), (14, 14), (13, 14), (13, 13), (13, 12), (13, 11) }.ToArray();
            leavingIndex = -1;
            previousModeBeforeFrightened = EnemyMode.Scatter;
            modeAfterLeavingHouse = EnemyMode.Scatter;
        }

        // Set the target position to be a random position in the bottom left of the maze
        public override (int, int) getScatterTargetPosition(Tile[,] tileArray) 
        {
            Random rng = new Random();
            int x, y;
            do 
            {
                // Keep generating a random position in the bottom left until the position is a valid one
                x = rng.Next(13); 
                y = rng.Next(20, 30); 
            } while (!isTileMoveable(x, y, tileArray) || (x == positionX && y == positionY));
            return (x, y);
        }

        // Set the target position of the orange enemy when in chase mode - towards the player but when close to the player,
        // return a scatter mode position
        public override (int, int) getChaseTargetPosition((int, int) pacmanPosition, Direction pacmanDirection, Tile[,] tileArray, (int, int) redEnemyPosition) 
        {
            // Calculate the distance betwwen the orange enemy and the player
            int distance = manhattanDistance(pacmanPosition, (positionX, positionY));

            // If the distance is greater than 6, move towards the player
            if (distance > 6) 
            {

                // Calculate the tile two in front of the player and use that as the target if the tile is valid
                int targetX = pacmanPosition.Item1;
                int targetY = pacmanPosition.Item2;

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

                if (isTileMoveable(targetX, targetY, tileArray)) 
                {
                    return (targetX, targetY);
                }
                return (pacmanPosition.Item1, pacmanPosition.Item2);
            } else 
            {
                // If the distance is less than 6, return the position if the ghost is in scatter mode
                return getScatterTargetPosition(tileArray);
            }
        }

        // Return the orange sprite facing in the current direction
        public override void updateDirection() 
        {
            if (currentDirection == Direction.Down)
            {
                currentEnemy = new Rectangle(1707, 339, 42, 42);
            }
            else if (currentDirection == Direction.Up)
            {
                currentEnemy = new Rectangle(1611, 339, 42, 42);
            }
            else if (currentDirection == Direction.Right)
            {
                currentEnemy = new Rectangle(1419, 339, 42, 42);
            }
            else if (currentDirection == Direction.Left)
            {
                currentEnemy = new Rectangle(1515, 339, 42, 42);
            }

        }
    }
}