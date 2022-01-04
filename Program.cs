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

            //Save the player fleet grid to an external file.
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

            //Save the computer fleet grid to an external file.
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

            //Check whether either side has lost all their boats.
            while (computerBoats != 0 && playerBoats != 0)
            {
                computerBoats = 6;
                playerBoats = 6;

                targetTracker = PlayerTurn(computerFleet, targetTracker);

                //Display target tracker to player.
                for (int row = 0; row < targetTracker.GetLength(0); row++)
                {
                    for (int col = 0; col < targetTracker.GetLength(1); col++)
                    {
                        Console.Write(targetTracker[row, col]);
                    }
                    Console.WriteLine();
                }

                //Check every cell within the target tracker grid for a 'H'. If it finds 5, the player wins and the game is over.
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
                
                //Only continue program when computer still has boats.
                if (computerBoats != 0)
                {
                    playerFleet = ComputerTurn(playerFleet, computerMiss);

                    //Check every cell within the player fleet grid for a 'H'. If it finds 5, the computer wins and the game is over.
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

        static char[,] BuildPlayerFleetGrid()
        {
            char[,] playerFleet =
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

            //Display empty player fleet grid to user.
            for (int row = 0; row < playerFleet.GetLength(0); row++)
            {
                for (int col = 0; col < playerFleet.GetLength(1); col++)
                {
                    Console.Write(playerFleet[row, col]);
                }
                Console.WriteLine();
            }

            //Loop until all 5 boats have been chosen.
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
                    //Column then row in x, y format (easier to input).
                    Console.WriteLine("Enter boat column: ");
                    int boatCol = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Enter boat row: ");
                    int boatRow = Convert.ToInt32(Console.ReadLine());

                    //Disable user from picking cells that are already occupied by a character.
                    if (playerFleet[boatRow, boatCol] != ' ')
                    {
                        Console.WriteLine("Invalid input, try again.");
                    }
                    else
                    {
                        //Put the character 'B' wherever there is a boat.
                        playerFleet[boatRow, boatCol] = 'B';

                        //Display new player fleet grid to user on every loop.
                        for (int row = 0; row < playerFleet.GetLength(0); row++)
                        {
                            for (int col = 0; col < playerFleet.GetLength(1); col++)
                            {
                                Console.Write(playerFleet[row, col]);
                            }
                            Console.WriteLine();
                        }
                        //To stop loop once we get 5 boat positions.
                        sum += 1;
                    }
                }
            }
            return playerFleet;
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
            
            //Initialise random seed
            Random random = new Random();
            bool choosing = true;
            int sum = 0;
            while (choosing)
            {   
                //Stop getting random boat positions once we have 5 boats.
                if (sum == 5)
                {
                    choosing = false;
                }
                else
                {
                    int row = random.Next(1, 9);
                    int col = random.Next(1, 9);

                    //Ensure valid input.
                    if (computerFleetGrid[row, col] == ' ')
                    {
                        //Create a boat at random position.
                        computerFleetGrid[row, col] = 'B';
                        //Keep going till 5 boats.
                        sum += 1;
                    }
                }
            }
            return computerFleetGrid;
        }
        static char[,] PlayerTurn(char[,] computerFleetGrid, char[,] targetTracker)
        {
            string targetTrackerFile = "targettracker.txt";

            //Display target tracker grid to player on their turn.
            for (int row = 0; row < targetTracker.GetLength(0); row++)
            {
                for (int col = 0; col < targetTracker.GetLength(1); col++)
                {
                    Console.Write(targetTracker[row, col]);
                }
                Console.WriteLine();
            }

            //Allow user to start targetting enemy boats.
            Console.WriteLine("Enter target column: ");
            int targetCol = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter target row: ");
            int targetRow = Convert.ToInt32(Console.ReadLine());

            //Stop user from targetting same area twice.
            while (targetTracker[targetRow, targetCol] != ' ')
            {
                if (targetTracker[targetRow, targetCol] == 'H' || targetTracker[targetRow, targetCol] == 'M')
                {
                    Console.WriteLine("You have already targetted this coordinate.");
                }
                else
                {
                    Console.WriteLine("Invalid input.");
                }
                Console.WriteLine("Enter target column: ");
                targetCol = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Enter target row: ");
                targetRow = Convert.ToInt32(Console.ReadLine());
            }

            //Detect hit.
            if (computerFleetGrid[targetRow, targetCol] == 'B')
            {
                Console.WriteLine("Hit!");
                targetTracker[targetRow, targetCol] = 'H';
            }
            //Detect miss.
            else
            {
                Console.WriteLine("Miss!");
                targetTracker[targetRow, targetCol] = 'M';
            }

            //Save target tracker grid to an external file.
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

            //Stop computer from targetting same area twice.
            while (fleetGrid[targetRow, targetCol] == 'H' || computerMiss[targetRow, targetCol] == 'M')
            {
                targetRow = ComputerTargetRow();
                targetCol = ComputerTargetCol();
            }

            Console.WriteLine("The computer has selected coordinates: (" + targetCol + ", " + targetRow + ")");

            //Detect hit.
            if (fleetGrid[targetRow, targetCol] == 'B')
            {
                Console.WriteLine("Hit!");
                fleetGrid[targetRow, targetCol] = 'H';
                return fleetGrid;
            }
            //Detect miss.
            else
            {
                Console.WriteLine("Miss!");
                computerMiss[targetRow, targetCol] = 'M';

                //Save computer miss grid to an external file.
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
            //Re-initialise all the necessary variables.
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

            //Save the player fleet grid to an external file.
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

            //Save the computer fleet grid to an external file.
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

            //Check whether either side has lost all their boats.
            while (computerBoats != 0 && playerBoats != 0)
            {
                computerBoats = 6;
                playerBoats = 6;

                targetTracker = PlayerTurn(computerFleet, targetTracker);

                //Display target tracker to player.
                for (int row = 0; row < targetTracker.GetLength(0); row++)
                {
                    for (int col = 0; col < targetTracker.GetLength(1); col++)
                    {
                        Console.Write(targetTracker[row, col]);
                    }
                    Console.WriteLine();
                }

                //Check every cell within the target tracker grid for a 'H'. If it finds 5, the player wins and the game is over.
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

                //Only continue program when computer still has boats.
                if (computerBoats != 0)
                {
                    playerFleet = ComputerTurn(playerFleet, computerMiss);

                    //Check every cell within the player fleet grid for a 'H'. If it finds 5, the computer wins and the game is over.
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
            //Create an empty 9x9 array.
            char[,] rebuiltArray = new char[9, 9];
            
            //Load file.
            using (StreamReader sr = new StreamReader(file))
            {
                string line;
                int i = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    //Convert line into an array.
                    char[] lineArray = line.ToCharArray();

                    //Go through each character in line.
                    for (int j = 0; j < line.Length; j++)
                    {
                        //Transfer each character in each line to the 2D array.
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
