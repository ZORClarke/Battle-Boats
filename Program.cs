using System;
using System.IO;

namespace Battle_Boats
{
    class Program
    {
        static void Main(string[] args)
        {
            BattleBoats();
        }
        static void BattleBoats()
        {
            bool programLoop = true;
            while (programLoop)
            {
                Menu();
                int choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        NewGame();
                        break;
                    case 2:
                        ResumeGame();
                        break;
                    case 3:
                        ReadInstructions();
                        break;
                    case 4:
                        programLoop = false;
                        break;
                }
            }
        }
        static void Menu()
        {
            Console.WriteLine("\n1) Play new game");
            Console.WriteLine("2) Resume a game");
            Console.WriteLine("3) Read the instructions");
            Console.Write("4) Quit\n>>> ");
        }

        //CASE 1
        static void NewGame()
        {
            string playerFleetFile = "playerFleet.txt";
            string computerFleetFile = "computerFleet.txt";

            int computerBoats = 6;
            int playerBoats = 6;

            char[,] targetTracker =
                {
                {'T','A','B','C','D','E','F','G','H'},
                {'1',' ',' ',' ',' ',' ',' ',' ',' '},
                {'2',' ',' ',' ',' ',' ',' ',' ',' '},
                {'3',' ',' ',' ',' ',' ',' ',' ',' '},
                {'4',' ',' ',' ',' ',' ',' ',' ',' '},
                {'5',' ',' ',' ',' ',' ',' ',' ',' '},
                {'6',' ',' ',' ',' ',' ',' ',' ',' '},
                {'7',' ',' ',' ',' ',' ',' ',' ',' '},
                {'8',' ',' ',' ',' ',' ',' ',' ',' '}
                };

            char[,] computerMiss =
                {
                {'M','A','B','C','D','E','F','G','H'},
                {'1',' ',' ',' ',' ',' ',' ',' ',' '},
                {'2',' ',' ',' ',' ',' ',' ',' ',' '},
                {'3',' ',' ',' ',' ',' ',' ',' ',' '},
                {'4',' ',' ',' ',' ',' ',' ',' ',' '},
                {'5',' ',' ',' ',' ',' ',' ',' ',' '},
                {'6',' ',' ',' ',' ',' ',' ',' ',' '},
                {'7',' ',' ',' ',' ',' ',' ',' ',' '},
                {'8',' ',' ',' ',' ',' ',' ',' ',' '}
                };

            char[,] playerFleet = BuildPlayerFleetGrid();
            using (StreamWriter sw = new StreamWriter(playerFleetFile))
            {
                for (int row = 0; row < playerFleet.GetLength(0); row++)
                {
                    for (int col = 0; col < playerFleet.GetLength(1); col++)
                    {
                        sw.Write(playerFleet[row, col]);
                    }
                    sw.WriteLine();
                }
            }

            char[,] computerFleet = BuildComputerFleetGrid();
            using (StreamWriter sw = new StreamWriter(computerFleetFile))
            {
                for (int row = 0; row < computerFleet.GetLength(0); row++)
                {
                    for (int col = 0; col < computerFleet.GetLength(1); col++)
                    {
                        sw.Write(computerFleet[row, col]);
                    }
                    sw.WriteLine();
                }
            }

            while (computerBoats != 0 && playerBoats != 0)
            {
                computerBoats = 6;
                playerBoats = 6;

                targetTracker = PlayerTurn(computerFleet, targetTracker);

                for (int row = 0; row < targetTracker.GetLength(0); row++)
                {
                    for (int col = 0; col < targetTracker.GetLength(1); col++)
                    {
                        Console.Write(targetTracker[row, col]);
                    }
                    Console.WriteLine();
                }

                for (int row = 0; row < targetTracker.GetLength(0); row++)
                {
                    for (int col = 0; col < targetTracker.GetLength(1); col++)
                    {
                        if (targetTracker[row, col] == 'H')
                        {
                            computerBoats--;
                            if (computerBoats == 0)
                            {
                                Console.WriteLine("The opponent has no more ships, you win!");
                            }
                        }
                    }
                }

                if (computerBoats != 0)
                {
                    playerFleet = ComputerTurn(playerFleet, computerMiss);

                    for (int row = 0; row < playerFleet.GetLength(0); row++)
                    {
                        for (int col = 0; col < playerFleet.GetLength(1); col++)
                        {
                            if (playerFleet[row, col] == 'H')
                            {
                                playerBoats--;
                                if (playerBoats == 0)
                                {
                                    Console.WriteLine("You have no more ships, the computer wins!");
                                }
                            }
                        }
                    }
                }
            }

            char[,] fleetGrid = BuildPlayerFleetGrid();
            using (StreamWriter sw = new StreamWriter(playerFleetFile))
            {
                for (int row = 0; row < fleetGrid.GetLength(0); row++)
                {
                    for (int col = 0; col < fleetGrid.GetLength(1); col++)
                    {
                        sw.Write(fleetGrid[row, col]);
                    }
                    sw.WriteLine();
                }
            }

            char[,] computerFleetGrid = BuildComputerFleetGrid();
            using (StreamWriter sw = new StreamWriter(computerFleetFile))
            {
                for (int row = 0; row < computerFleetGrid.GetLength(0); row++)
                {
                    for (int col = 0; col < computerFleetGrid.GetLength(1); col++)
                    {
                        sw.Write(computerFleetGrid[row, col]);
                    }
                    sw.WriteLine();
                }
            }

            while (computerBoats != 0 && playerBoats != 0)
            {
                computerBoats = 6;
                playerBoats = 6;

                char[,] updateTargetTracker = PlayerTurn(computerFleetGrid, targetTracker);

                for (int row = 0; row < updateTargetTracker.GetLength(0); row++)
                {
                    for (int col = 0; col < updateTargetTracker.GetLength(1); col++)
                    {
                        Console.Write(updateTargetTracker[row, col]);
                    }
                    Console.WriteLine();
                }

                for (int row = 0; row < updateTargetTracker.GetLength(0); row++)
                {
                    for (int col = 0; col < updateTargetTracker.GetLength(1); col++)
                    {
                        if (updateTargetTracker[row, col] == 'H')
                        {
                            computerBoats--;
                            if (computerBoats == 0)
                            {
                                Console.WriteLine("The opponent has no more ships, you win!");
                            }
                        }
                    }
                }

                if (computerBoats != 0)
                {
                    fleetGrid = ComputerTurn(fleetGrid, computerMiss);

                    for (int row = 0; row < fleetGrid.GetLength(0); row++)
                    {
                        for (int col = 0; col < fleetGrid.GetLength(1); col++)
                        {
                            if (fleetGrid[row, col] == 'H')
                            {
                                playerBoats--;
                                if (playerBoats == 0)
                                {
                                    Console.WriteLine("You have no more ships, the computer wins!");
                                }
                            }
                        }
                    }
                }
            }
        }

        static char[,] BuildPlayerFleetGrid()
        {
            char[,] fleetGrid =
                {
                {'F','A','B','C','D','E','F','G','H'},
                {'1',' ',' ',' ',' ',' ',' ',' ',' '},
                {'2',' ',' ',' ',' ',' ',' ',' ',' '},
                {'3',' ',' ',' ',' ',' ',' ',' ',' '},
                {'4',' ',' ',' ',' ',' ',' ',' ',' '},
                {'5',' ',' ',' ',' ',' ',' ',' ',' '},
                {'6',' ',' ',' ',' ',' ',' ',' ',' '},
                {'7',' ',' ',' ',' ',' ',' ',' ',' '},
                {'8',' ',' ',' ',' ',' ',' ',' ',' '}
                };

            for (int row = 0; row < fleetGrid.GetLength(0); row++)
            {
                for (int col = 0; col < fleetGrid.GetLength(1); col++)
                {
                    Console.Write(fleetGrid[row, col]);
                }
                Console.WriteLine();
            }

            bool choosing = true;
            int sum = 0;
            while (choosing)
            {
                if (sum == 5)
                {
                    choosing = false;
                }
                else
                {
                    Console.WriteLine("Enter boat column: ");
                    int boatCol = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Enter boat row: ");
                    int boatRow = Convert.ToInt32(Console.ReadLine());

                    if (fleetGrid[boatRow, boatCol] != ' ')
                    {
                        Console.WriteLine("Invalid input, try again.");
                    }
                    else
                    {
                        fleetGrid[boatRow, boatCol] = 'B';
                        for (int row = 0; row < fleetGrid.GetLength(0); row++)
                        {
                            for (int col = 0; col < fleetGrid.GetLength(1); col++)
                            {
                                Console.Write(fleetGrid[row, col]);
                            }
                            Console.WriteLine();
                        }
                        sum += 1;
                    }
                }
            }
            return fleetGrid;
        }
        static char[,] BuildComputerFleetGrid()
        {
            char[,] computerFleetGrid =
                {
                {'F','A','B','C','D','E','F','G','H'},
                {'1',' ',' ',' ',' ',' ',' ',' ',' '},
                {'2',' ',' ',' ',' ',' ',' ',' ',' '},
                {'3',' ',' ',' ',' ',' ',' ',' ',' '},
                {'4',' ',' ',' ',' ',' ',' ',' ',' '},
                {'5',' ',' ',' ',' ',' ',' ',' ',' '},
                {'6',' ',' ',' ',' ',' ',' ',' ',' '},
                {'7',' ',' ',' ',' ',' ',' ',' ',' '},
                {'8',' ',' ',' ',' ',' ',' ',' ',' '}
                };

            Random random = new Random();
            bool choosing = true;
            int sum = 0;
            while (choosing)
            {
                if (sum == 5)
                {
                    choosing = false;
                }
                else
                {
                    int row = random.Next(1, 9);
                    int col = random.Next(1, 9);

                    if (computerFleetGrid[row, col] == ' ')
                    {
                        computerFleetGrid[row, col] = 'B';
                        sum += 1;
                    }
                }
            }
            return computerFleetGrid;
        }
        static char[,] PlayerTurn(char[,] computerFleetGrid, char[,] targetTracker)
        {
            string targetTrackerFile = "targettracker.txt";

            for (int row = 0; row < targetTracker.GetLength(0); row++)
            {
                for (int col = 0; col < targetTracker.GetLength(1); col++)
                {
                    Console.Write(targetTracker[row, col]);
                }
                Console.WriteLine();
            }

            Console.WriteLine("Enter target column: ");
            int targetCol = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter target row: ");
            int targetRow = Convert.ToInt32(Console.ReadLine());

            while (targetTracker[targetRow, targetCol] == 'H' || targetTracker[targetRow, targetCol] == 'M')
            {
                Console.WriteLine("You have already targetted this coordinate.");
                Console.WriteLine("Enter target column: ");
                targetCol = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Enter target row: ");
                targetRow = Convert.ToInt32(Console.ReadLine());
            }

            if (computerFleetGrid[targetRow, targetCol] == 'B')
            {
                Console.WriteLine("Hit!");
                targetTracker[targetRow, targetCol] = 'H';
            }
            else
            {
                Console.WriteLine("Miss!");
                targetTracker[targetRow, targetCol] = 'M';
            }
            using (StreamWriter sw = new StreamWriter(targetTrackerFile))
            {
                for (int row = 0; row < targetTracker.GetLength(0); row++)
                {
                    for (int col = 0; col < targetTracker.GetLength(1); col++)
                    {
                        sw.Write(targetTracker[row, col]);
                    }
                    sw.WriteLine();
                }
            }
            return targetTracker;
        }
        static int ComputerTargetRow()
        {
            Random random = new Random();
            int targetRow = random.Next(1, 9);
            return targetRow;
        }
        static int ComputerTargetCol()
        {
            Random random = new Random();
            int targetCol = random.Next(1, 9);
            return targetCol;
        }
        static char[,] ComputerTurn(char[,] fleetGrid, char[,] computerMiss)
        {
            string computerMissFile = "computermiss.txt";
            int targetRow = ComputerTargetRow();
            int targetCol = ComputerTargetCol();

            while (fleetGrid[targetRow, targetCol] == 'H' || computerMiss[targetRow, targetCol] == 'M')
            {
                targetRow = ComputerTargetRow();
                targetCol = ComputerTargetCol();
            }

            Console.WriteLine("The computer has selected coordinates: (" + targetCol + ", " + targetRow + ")");
            if (fleetGrid[targetRow, targetCol] == 'B')
            {
                Console.WriteLine("Hit!");
                fleetGrid[targetRow, targetCol] = 'H';
                return fleetGrid;
            }
            else
            {
                Console.WriteLine("Miss!");
                computerMiss[targetRow, targetCol] = 'M';
                using (StreamWriter sw = new StreamWriter(computerMissFile))
                {
                    for (int row = 0; row < computerMiss.GetLength(0); row++)
                    {
                        for (int col = 0; col < computerMiss.GetLength(1); col++)
                        {
                            sw.Write(computerMiss[row, col]);
                        }
                        sw.WriteLine();
                    }
                }
                return computerMiss;
            }
        }
        //CASE 2
        static void ResumeGame()
        {
            int playerBoats = 6;
            int computerBoats = 6;
            string targetTrackerFile = "targettracker.txt";
            string computerMissFile = "computermiss.txt";
            string playerFleetFile = "playerFleet.txt";
            string computerFleetFile = "computerFleet.txt";

            char[,] targetTracker = Rebuild2DArray(targetTrackerFile);
            char[,] computerMiss = Rebuild2DArray(computerMissFile);
            char[,] playerFleet = Rebuild2DArray(playerFleetFile);
            char[,] computerFleet = Rebuild2DArray(computerFleetFile);

            using (StreamWriter sw = new StreamWriter(playerFleetFile))
            {
                for (int row = 0; row < playerFleet.GetLength(0); row++)
                {
                    for (int col = 0; col < playerFleet.GetLength(1); col++)
                    {
                        sw.Write(playerFleet[row, col]);
                    }
                    sw.WriteLine();
                }
            }

            using (StreamWriter sw = new StreamWriter(computerFleetFile))
            {
                for (int row = 0; row < computerFleet.GetLength(0); row++)
                {
                    for (int col = 0; col < computerFleet.GetLength(1); col++)
                    {
                        sw.Write(computerFleet[row, col]);
                    }
                    sw.WriteLine();
                }
            }

            while (computerBoats != 0 && playerBoats != 0)
            {
                computerBoats = 6;
                playerBoats = 6;

                targetTracker = PlayerTurn(computerFleet, targetTracker);

                for (int row = 0; row < targetTracker.GetLength(0); row++)
                {
                    for (int col = 0; col < targetTracker.GetLength(1); col++)
                    {
                        Console.Write(targetTracker[row, col]);
                    }
                    Console.WriteLine();
                }

                for (int row = 0; row < targetTracker.GetLength(0); row++)
                {
                    for (int col = 0; col < targetTracker.GetLength(1); col++)
                    {
                        if (targetTracker[row, col] == 'H')
                        {
                            computerBoats--;
                            if (computerBoats == 0)
                            {
                                Console.WriteLine("The opponent has no more ships, you win!");
                            }
                        }
                    }
                }

                if (computerBoats != 0)
                {
                    playerFleet = ComputerTurn(playerFleet, computerMiss);

                    for (int row = 0; row < playerFleet.GetLength(0); row++)
                    {
                        for (int col = 0; col < playerFleet.GetLength(1); col++)
                        {
                            if (playerFleet[row, col] == 'H')
                            {
                                playerBoats--;
                                if (playerBoats == 0)
                                {
                                    Console.WriteLine("You have no more ships, the computer wins!");
                                }
                            }
                        }
                    }
                }
            }

        }

        static char[,] Rebuild2DArray(string file)
        {
            char[,] rebuiltArray = new char[9, 9];
            using (StreamReader sr = new StreamReader(file))
            {
                string line;
                int i = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    char[] lineArray = line.ToCharArray();
                    for (int j = 0; j < line.Length; j++)
                    {
                        rebuiltArray[i, j] = lineArray[j];
                    }
                    i++;
                }
            }
            return rebuiltArray;
        }

        //CASE 3
        static void ReadInstructions()
        {
            Console.WriteLine(
                             "Battle boats is a turn-based strategy game where players eliminate their opponents fleet of boats by ‘firing’ at a location on" +
                             " a grid in an attempt to sink them.\nThe first player to sink all of their opponents’ battle boats is declared the winner. " +
                             "\n\nEach player has two eight by eight grids.\nOne grid is used for their own battle boats and the other is used to record any" +
                             " hits or misses placed on their opponents.\nAt the beginning of the game, players decide where they wish to place their fleet" +
                             " of five battle boats. " +
                             "\n\nDuring game play, players take it in turns to fire at a location on their opponent’s board.\nThey do this by stating the" +
                             " coordinates for their target.\nIf a player hits their opponent's boat, then this is recorded as a hit.\nIf they miss, then" +
                             " this is recorded as a miss." +
                             "\n\nThe game ends when a player's fleet of boats have been sunk.\nThe winner is the player with boats remaining at the end of" +
                             " the game."
                             );
        }
    }
}
