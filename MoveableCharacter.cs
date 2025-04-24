using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace MAZEGAME
{
    // Class that depecits a moving object in the game (i.e both the players and enemeies)
    public class MoveableCharacter
    {
        // Given a direction and the current position, return what the next position would be
        public Vector2 calculateBasedOnDirection(Direction nextDirection, int currentX, int currentY)
        {
            int toBeX = currentX;
            int toBeY = currentY;

            if (nextDirection == Direction.Down)
            {
                toBeY += 1;
            }
            else if (nextDirection == Direction.Up)
            {
                toBeY -= 1;
            }
            else if (nextDirection == Direction.Right)
            {
                toBeX += 1;
                // Allow for the warp tunnels to be considered
                if (toBeY == 14 && toBeX > 27)
                {
                    toBeX = 0;
                }
            }
            else if (nextDirection == Direction.Left)
            {
                toBeX -= 1;
                // Allow for the warp tunnels to be considered
                if (toBeY == 14 && toBeX < 0)
                {
                    toBeX = 27;
                }
            }

            return new Vector2(toBeX, toBeY);
        }

        // Checks if a tile can be moved onto i.e. it is not a wall or ghost house
        public bool isTileMoveable(int x, int y, Tile[,] tileArray) {
            if (x > 27 || x < 0 || y < 0 || y > 30) {
                return false;
            }
            return tileArray[x, y].tileType != Tile.TileType.Wall && tileArray[x, y].tileType != Tile.TileType.GhostHouse;
        }

        // Used to calculate the manhattan distance between two coordinates
        public int manhattanDistance((int, int) a, (int, int) b)
        {
            return Math.Abs( a.Item1 - b.Item2) + Math.Abs(a.Item1 - b.Item2);
        }

        // This is the pathfinding algorithm used to find a path between a start positoin and an end position.
        // It returns the first move to take in that path that would lead towards the end position.
        // The foundation of this is the A* algorithm which uses the manhattan distance as a heuristic
        public (int, int) findPath((int, int) startPosition, (int, int) targetPosition, Tile[,] tileArray, Direction currentDirection) {
            int rows = tileArray.GetLength(0);
            int cols = tileArray.GetLength(1);
            var openSet = new SortedSet<(int, (int, int))>();
            openSet.Add((0, startPosition));
            var cameFrom = new Dictionary<(int, int), (int, int)>();
            var gScore = new Dictionary<(int, int), int> { [startPosition] = 0 };
            var fScore = new Dictionary<(int, int), int> { [startPosition] = manhattanDistance(startPosition, targetPosition) };
            
            while (openSet.Count > 0)
            {
                // Get the position with the lowest cost and remove it from the open set
                var current = openSet.Min;
                openSet.Remove(current);

                // If we have reached the destination then create the path
                if (current.Item2 == targetPosition)
                {
                    var path = new List<(int, int)>();

                    // Follow the chain of position and the position before that etc...
                    while (cameFrom.ContainsKey(current.Item2))
                    {
                        path.Add(current.Item2);
                        current.Item2 = cameFrom[current.Item2];
                    }
                    path.Add(startPosition);

                    // Reverse the path so that we begin with the start position
                    path.Reverse();
                    if (path.Count <= 1){ // If the start position is the target position return the start
                        return path[0];
                    }

                    // Return the position that is the beginning of the path
                    return path[1];
                }

                foreach (var dir in Enum.GetValues<Direction>())
                {
                    // Make sure that the start position isn't included and that we do not back track by using the opposite direction
                    if (current.Item2 != startPosition || dir != getOppositeDirection(currentDirection)) {
                        // Get the position based on the direction
                        var neighborVector = calculateBasedOnDirection(dir, current.Item2.Item1, current.Item2.Item2);
                        var neighbor = ((int) neighborVector.X, (int) neighborVector.Y);

                        // If the position is a valid position, then calculate the g and f score
                        if (neighbor.Item1 >= 0 && neighbor.Item1 < rows && neighbor.Item2 >= 0 && neighbor.Item2 < cols && isTileMoveable(neighbor.Item1, neighbor.Item2, tileArray))
                        {
                            // Increase the gscore of the current position
                            int tentativeGScore = gScore[current.Item2] + 1;

                            // If the neighbour isn't included in the g score dictionary, then add it 
                            // Also if the new g score of the current position is less than the neighbour update the dictionaries
                            if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                            {
                                cameFrom[neighbor] = current.Item2;
                                gScore[neighbor] = tentativeGScore;
                                int fScoreNeighbor = tentativeGScore + manhattanDistance(neighbor, targetPosition);
                                fScore[neighbor] = fScoreNeighbor;
                                openSet.Add((fScoreNeighbor, neighbor));
                            }
                        }
                    }
                }
            }
        return startPosition;
        }

        // Return the opposite of a given direction
        public Direction getOppositeDirection(Direction currentDir)
        {
            switch (currentDir)
            {
                case Direction.Right:
                    return Direction.Left;
                case Direction.Left:
                    return Direction.Right;
                case Direction.Down:
                    return Direction.Up;
                default:
                    return Direction.Down;
            }
        }
    }
}