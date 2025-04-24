using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace MAZEGAME
{
    public abstract class Enemy : MoveableCharacter
    {

        // Different modes the ghosts can be in
        public enum EnemyMode{Chase, Scatter, Frightened, InsideHouse, LeavingHouse, ReturningBack}

        public int positionX;
        public int positionY;
        public Direction currentDirection;
        public Rectangle currentEnemy;
        public EnemyMode currentMode;
        public EnemyMode previousModeBeforeFrightened;
        public EnemyMode modeAfterLeavingHouse;
        public Rectangle frightenedEnemy;
        public bool isInGhostHouse;
        public (int, int)[] leavingPath;
        public int leavingIndex;

        public Enemy() 
        {
            setInitalState();
            frightenedEnemy = new Rectangle(1755, 195, 42, 42);
        }

        public abstract void setInitalState(); // Set the initial state of the enemy

        // Move the enemy to a certain position
        public void updateEnemy(Tile[,] tileArray, (int, int) pacmanPosition, Direction pacmanDirection, (int, int) redEnemyPosition) 
        {
            (int, int) targetPosition;
            (int, int) toBePosition;

            // Get the position to move the enemy to, based on the current mode the enemy is in 
            if (currentMode == EnemyMode.InsideHouse)
            {
                return;
            } else if (currentMode == EnemyMode.LeavingHouse) 
            {
                toBePosition = getLeavingPosition();
            } else if (currentMode == EnemyMode.Chase) 
            {
                targetPosition = getChaseTargetPosition(pacmanPosition, pacmanDirection, tileArray, redEnemyPosition);
                toBePosition = findPath((positionX, positionY), targetPosition, tileArray, currentDirection);
            } else if (currentMode == EnemyMode.Scatter)
            {
                targetPosition = getScatterTargetPosition(tileArray);
                toBePosition = findPath((positionX, positionY), targetPosition, tileArray, currentDirection);
            } else if (currentMode == EnemyMode.ReturningBack) 
            {
                // If the ghost has been caught, it is sent to the beginning position
                if (positionX == 13 && positionY == 11) 
                {
                    setInitalState();
                    return;
                }
                toBePosition = findPath((positionX, positionY), (13, 11), tileArray, currentDirection);
            } else 
            {
                toBePosition = getFrightenedPosition(tileArray);
            }

            // Based on the position to move to, get the direction the enemy will be facing
            currentDirection = getDirectionBasedOnNextPosition(positionX, positionY, toBePosition.Item1, toBePosition.Item2);

            // Move the enemy to the new position
            positionX = toBePosition.Item1;
            positionY = toBePosition.Item2;

            // Change the sprite according to the direction the enemy is facing
            // Doesn't apply to the frightened sprite, because that is the same sprite for all directions (dark blue ghost)
            if (currentMode != EnemyMode.Frightened && currentMode != EnemyMode.ReturningBack
             && !(currentMode == EnemyMode.LeavingHouse && modeAfterLeavingHouse == EnemyMode.Frightened)) 
            {
                updateDirection();
            }
        }


        public void setX(int newX) 
        {
            positionX = newX;
        }

        public void setY(int newY) 
        {
            positionY = newY;
        }

        // Return x position of ghost in the tile array
        public int getX() 
        {
            return positionX;
        }

        // Return y position of ghost in the tile array
        public int getY() 
        {
            return positionY;
        }

        // Return the current sprite to be used
        public Rectangle getCurrentEnemy() 
        {
            return currentEnemy;
        }

        // Return the current mode the enemy is in 
        public EnemyMode getMode() 
        {
            return currentMode;
        }

        // Update the enemy to the frightened mode
        public void frighten() 
        {
            currentEnemy = frightenedEnemy;
            if (currentMode == EnemyMode.InsideHouse || currentMode == EnemyMode.LeavingHouse) 
            {
                return;
            }
             // Store the mode the enemy is in before being frightened
            if (currentMode != EnemyMode.Frightened) 
            {
                previousModeBeforeFrightened = currentMode;
            }
            // Change the mode and the sprite to the frightened enemy sprite (dark blue ghost)
            currentMode = EnemyMode.Frightened;

            // Move the ghost in the opposite direction (away from the player)
            currentDirection = getOppositeDirection(currentDirection);
        }

        // Puts the enemy in a state to leave the house
        public void leaveHouse(EnemyMode mode) 
        {
            if (currentMode == EnemyMode.InsideHouse) 
            {
                currentMode = EnemyMode.LeavingHouse;
                modeAfterLeavingHouse = mode;
            } 
        }

        // Change the sprite (if in frightened mode) to the white ghost
        public void setWhiteEnemy() 
        {
            if (currentMode == EnemyMode.Frightened || currentMode == EnemyMode.InsideHouse) 
            {
                currentEnemy = new Rectangle(1851, 195, 42, 42);
            }
        }

        // Return to the previous mode before being frightened
        public void endFrightenedMode() 
        {
            if (currentMode == EnemyMode.Frightened) 
            {
                currentMode = previousModeBeforeFrightened;
            } else if (currentMode == EnemyMode.InsideHouse) 
            {
                updateDirection();
            }
        }

        // Change modes to the given mode
        public void changeMode(EnemyMode modeToChangeTo) 
        {
            if (currentMode != EnemyMode.InsideHouse && currentMode != EnemyMode.LeavingHouse) 
            {
                currentMode = modeToChangeTo;
            }
        }

        // Moves the ghost along the path to leave the house.
        public (int, int) getLeavingPosition() 
        {
            leavingIndex++;
            if (leavingIndex == leavingPath.Count()) 
            {
                currentMode = modeAfterLeavingHouse;
                return (positionX, positionY);
            } else 
            {
                return leavingPath[leavingIndex];
            }
        }

        public abstract (int, int) getScatterTargetPosition(Tile[,] tileArray);
        public abstract (int, int) getChaseTargetPosition((int, int) pacmanPosition, Direction pacmanDirection, Tile[,] tileArray, (int, int) redEnemyPosition);

        public abstract void updateDirection();

        // Returns the position the enemy should be in when in frightened mode
        // The ghost moves in one direction and makes random decisions at every intersection
        public (int, int) getFrightenedPosition(Tile[,] tileArray) 
        {
            List<(int, int)> possibleNextPositions = new List<(int, int)>();
            foreach (var dir in Enum.GetValues<Direction>()) 
            {
                // Calculate all the positions that the ghost can go in based on the different directions
                // (aside from the opposite - to prevent backtracking)
                if (dir != getOppositeDirection(currentDirection)) 
                {
                    Vector2 position = calculateBasedOnDirection(dir, positionX, positionY);
                    // Only valid positions are added to the list
                    if (isTileMoveable((int)position.X, (int)position.Y, tileArray)) 
                    {
                        possibleNextPositions.Add(((int)position.X, (int)position.Y));
                    }
                }
            }

            // Pick a random option from the valid moves
            Random random = new Random();
            int index = random.Next(possibleNextPositions.Count);
            return possibleNextPositions[index];
        }

        // Given a current position and a next position, calculate the direction that the enemy is moving in
        public Direction getDirectionBasedOnNextPosition(int currentX, int currentY, int toBeX, int toBeY) 
        {
            var vector = (currentX - toBeX, currentY - toBeY);
            if (vector == (-1, 0)) 
            {
                return Direction.Right;
            } else if (vector == (1, 0)) 
            {
                return Direction.Left;
            } else if (vector == (0, -1)) 
            {
                return Direction.Down;
            } else if (vector == (0, 1))
            {
                return Direction.Up;
            } else 
            {
                return currentDirection;
            }
        }
    }
}