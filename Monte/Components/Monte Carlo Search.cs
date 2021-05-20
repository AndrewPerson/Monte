using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Monte.Game;
using Monte.Moves;

namespace Monte.Carlo
{
    public class Node
    {
        public Board board;
        public Direction originalMove;
    }

    public static class Search
    {
        private static readonly List<Direction> Moves = new List<Direction>
        {
            Direction.Up,
            Direction.Down,
            Direction.Left,
            Direction.Right
        };

        public static async Task<Direction> GetBest(List<Node> nodes, string myId)
        {
            int upTotal = 0;
            int upAlive = 0;
            
            int downTotal = 0;
            int downAlive = 0;

            int leftTotal = 0;
            int leftAlive = 0;

            int rightTotal = 0;
            int rightAlive = 0;

            foreach (var node in nodes)
            {
                if (node.originalMove == Direction.Up)
                {
                    upTotal++;
                    if (await FindMe(myId, node.board) != -1) upAlive++;
                }
                else if (node.originalMove == Direction.Down)
                {
                    downTotal++;
                    if (await FindMe(myId, node.board) != -1) downAlive++;
                }
                else if (node.originalMove == Direction.Left)
                {
                    leftTotal++;
                    if (await FindMe(myId, node.board) != -1) leftAlive++;
                }
                else
                {
                    rightTotal++;
                    if (await FindMe(myId, node.board) != -1) rightAlive++;
                }
            }

            Direction result = Direction.Up;
            float maxRanking = upTotal == 0 ? 0 : upAlive / upTotal;

            float downRanking = downTotal == 0 ? 0 : downAlive / downTotal;
            if (downRanking > maxRanking)
            {
                maxRanking = downRanking;
                result = Direction.Down;
            }

            float leftRanking = leftTotal == 0 ? 0 : leftAlive / leftTotal;
            if (leftRanking > maxRanking)
            {
                maxRanking = leftRanking;
                result = Direction.Left;
            }

            float rightRanking = rightTotal == 0 ? 0: rightAlive / rightTotal;
            if (rightRanking > maxRanking)
            {
                maxRanking = rightRanking;
                result = Direction.Right;
            }

            Console.WriteLine("INFO: UP RANKING: " + (upTotal == 0 ? 0 : upAlive / upTotal));
            Console.WriteLine("INFO: DOWN RANKING: " + downRanking);
            Console.WriteLine("INFO: LEFT RANKING: " + leftRanking);
            Console.WriteLine("INFO: RIGHT RANKING: " + rightRanking);

            return result;
        }

        public static async Task<List<Node>> PropagateForwards(int moves, Snake me, Board board)
        {
            Board[] futures = await Peek(board);
            List<Node> nodes = new List<Node>(futures.Length);
            
            foreach (var future in futures)
            {
                int index = await FindMe(me.ID, future);

                Node node = new Node
                {
                    board = future
                };

                var neck = me.Body[1];

                if (me.Head.X > neck.X) node.originalMove = Direction.Right;
                else if (me.Head.X < neck.X) node.originalMove = Direction.Left;
                else if (me.Head.Y > neck.Y) node.originalMove = Direction.Up;
                else node.originalMove = Direction.Down;

                nodes.Add(node);
            }

            return await PropagateRecursive(moves, 1, nodes);
        }

        private static async Task<int> FindMe(string myId, Board board)
        {
            //TODO Find out why some boards are null.
            if (board == null)
            {
                Console.WriteLine("INFO: NULL BOARD");
                return -1;
            }
            return await Task.Run(() => {
                for (int i = 0; i < board.Snakes.Count; i++) if (board.Snakes[i].ID == myId) return i; 
                return -1;
            });
        }

        private static async Task<List<Node>> PropagateRecursive(int maxMoves, int currentMoves, List<Node> nodes)
        {
            List<Node> result = new List<Node>();

            foreach (var node in nodes)
            {
                var futures = await Peek(node.board);
                var newNodes = new Node[futures.Length];
                
                await Task.Run(() => {
                    for (int i = 0; i < newNodes.Length; i++)
                    {
                        newNodes[i] = new Node
                        {
                            board = futures[i],
                            originalMove = node.originalMove
                        };
                    }
                });

                result.AddRange(newNodes);
            }

            currentMoves++;
            if (currentMoves < maxMoves) return await PropagateRecursive(maxMoves, currentMoves, result);
            else return result;
        }

        public static async Task<Board[]> Peek(Board board)
        {
            if (board == null)
            {
                Console.WriteLine("INFO: NULL BOARD WHEN PEEKING");
                return new Board[0];
            }

            Board[] result = new Board[(int) Math.Pow(4, board.Snakes.Count)];

            int permCount = 0;
            await Combinations.RepeatingCombinations(Moves, board.Snakes.Count, async (perm) =>
            {
                Board newBoard = await Clone(board);

                bool[] delete = new bool[newBoard.Snakes.Count];

                for (int i = 0; i < newBoard.Snakes.Count; i++) await Task.Run(() => {MoveSnake(i, perm[i], board, ref delete);});
                
                await Task.Run(() => {for (int i = delete.Length - 1; i >= 0; i--) if (delete[i]) newBoard.Snakes.RemoveAt(i);});

                result[permCount] = newBoard;
                permCount++;
            });

            return result;
        }

        private static void MoveSnake(int snakeIndex, Direction move, Board board, ref bool[] delete)
        {
            int moveX = 0;
            int moveY = 0;

            if (move == Direction.Up) moveY = 1;
            else if (move == Direction.Down) moveY = -1;
            else if (move == Direction.Left) moveX = -1;
            else if (move == Direction.Right) moveX = 1;

            var snake = board.Snakes[snakeIndex];

            snake.Health--;

            snake.Head = new Point
            {
                X = snake.Head.X + moveX,
                Y = snake.Head.Y + moveY
            };

            if (snake.Head.X < 0 || snake.Head.X >= board.Width || snake.Head.Y < 0 || snake.Head.Y >= board.Height)
            {
                for (int n = snake.Body.Count - 1; n > 0; n--) snake.Body[n] = snake.Body[n - 1];
                snake.Body[0] = snake.Head;

                delete[snakeIndex] = true;
                return;
            }

            if (board.Food.Contains(snake.Head))
            {
                snake.Body.Add(snake.Body[^1]);
                for (int n = snake.Body.Count - 1; n > 0; n--) snake.Body[n] = snake.Body[n - 1];
                board.Food.Remove(snake.Head);
                snake.Health = 100;
            }
            else for (int n = snake.Body.Count - 1; n > 0; n--) snake.Body[n] = snake.Body[n - 1];
            
            snake.Body[0] = snake.Head;

            if (snake.Body.LastIndexOf(snake.Head) != 0)
            {
                delete[snakeIndex] = true;
                return;
            }

            for (int n = 0; n < snakeIndex; n++)
            {
                Snake other = board.Snakes[n];

                for (int x = 0; x < other.Body.Count; x++)
                {
                    if (snake.Head == other.Body[x])
                    {
                        if (x == 0)
                        {
                            if (snake.Length == other.Length)
                            {
                                delete[snakeIndex] = true;
                                delete[n] = true;
                            }
                            else if (snake.Length > other.Length) delete[n] = true;
                            else delete[snakeIndex] = true;
                        }
                        else delete[snakeIndex] = true;

                        if (snake.Health == 0) delete[snakeIndex] = true;

                        return;
                    }
                }
            }

            if (snake.Health == 0) delete[snakeIndex] = true;
        }

        private static async Task<Board> Clone(Board board)
        {
            var newBoard = new Board
            {
                Height = board.Height,
                Width = board.Width,
                Food = new List<Point>(board.Food.Count),
                Snakes = await Clone(board.Snakes)
            };

            newBoard.Food.AddRange(board.Food);

            return newBoard;
        }

        private static async Task<List<Snake>> Clone(List<Snake> snakes)
        {
            List<Snake> result = new List<Snake>(snakes.Count);
            
            for (int i = 0; i < snakes.Count; i++) result.Add(await Clone(snakes[i]));
            
            return result;
        }

        private static async Task<Snake> Clone(Snake snake)
        {
            var result = new Snake
            {
                ID = snake.ID,
                Head = snake.Head,
                Body = new List<Point>(snake.Body.Count),
                Health = snake.Health
            };

            await Task.Run(() => {result.Body.AddRange(snake.Body);});

            return result;
        }
    }
}