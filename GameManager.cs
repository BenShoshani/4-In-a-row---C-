using System;

namespace Logic
{
    public class GameManager
    {
        private const int k_IndexFix = 1;
        private const int k_NumOfShapesToWin = 4;
        private readonly int r_NumberOfPlayers = 2;

        private Board m_BoardGame; ////the board of the game
        private Player[] m_Players; ////the players

        ////c'tor - create the game
        public GameManager(int i_RowsFromUser, int i_ColsFromUser, bool i_IsOpponentComputer, string i_UserName, string i_OpponentName)
        {
            m_BoardGame = new Board(i_RowsFromUser, i_ColsFromUser);
            m_Players = new Player[r_NumberOfPlayers];
            m_Players[(int)Player.eNumberOfPlayer.User] = new Player(Player.eShapeOfCoin.O, Player.eTypeOfPlayer.Human, i_UserName);
            if (i_IsOpponentComputer)
            {
                m_Players[(int)Player.eNumberOfPlayer.Opponent] = new Player(Player.eShapeOfCoin.X, Player.eTypeOfPlayer.Computer, i_OpponentName);
            }
            else
            {
                m_Players[(int)Player.eNumberOfPlayer.Opponent] = new Player(Player.eShapeOfCoin.X, Player.eTypeOfPlayer.Human, i_OpponentName);
            }
        }

        //// get the board game
        public Board BoardGame
        {
            get { return m_BoardGame; }
        }

        ////do the turn of the computer
        public int computerTurn(ref bool io_EndOfTheGame, ref int io_CounterOfTurn)
        {
            Random colRandomForComputer = new Random();
            bool stillMakeHisMove = false;
            int randomIndexFromList = 0;
            int randomCol = 0;

            while (!stillMakeHisMove)
            {
                randomIndexFromList = colRandomForComputer.Next(1, m_BoardGame.freeColInBoard.Count); ////choose random index from the list
                randomCol = m_BoardGame.freeColInBoard[randomIndexFromList - k_IndexFix];

                stillMakeHisMove = MakeMove(randomCol.ToString(), Player.eNumberOfPlayer.Opponent);

                if (stillMakeHisMove)
                {
                    if (checkWinsOfTheMatch(Player.eNumberOfPlayer.Opponent))
                    {
                        IncreaseScore(Player.eNumberOfPlayer.Opponent);
                        io_EndOfTheGame = true;
                        io_CounterOfTurn = 0;
                    }
                }
            }

            return randomCol;
        }

        ////check if the board is full
        public bool IsBoardFull()
        {
            return m_BoardGame.IsBoardFull();
        }

        ////increase the score of player
        public void IncreaseScore(Player.eNumberOfPlayer i_PlayerToInceaseScore)
        {
            m_Players[(int)i_PlayerToInceaseScore].ScoreOfPlayer++;
        }

        ////return the scores of both players
        public void GetScoresOfPlayers(out int o_ScoreOfUser, out int o_ScoreOfOpponent)
        {
            o_ScoreOfUser = m_Players[(int)Player.eNumberOfPlayer.User].ScoreOfPlayer;
            o_ScoreOfOpponent = m_Players[(int)Player.eNumberOfPlayer.Opponent].ScoreOfPlayer;
        }

        ////make the move that have been choosen
        public bool MakeMove(string i_ColsFromUser, Player.eNumberOfPlayer i_NumberOfPlayer)
        {
            int convertedCols = int.Parse(i_ColsFromUser) - k_IndexFix;
            Nullable<int> freeIndex;
            bool isInsertedToBoard = false;

            freeIndex = m_BoardGame.GetFreeIndex(convertedCols);
            if (freeIndex != null)
            {
                m_BoardGame.PlacePlayerShapeCoint(freeIndex.Value, convertedCols, (char)m_Players[(int)i_NumberOfPlayer].ShapeOfCoin);
                isInsertedToBoard = true;
            }

            return isInsertedToBoard;
        }

        ////check if someone win
        public bool checkWinsOfTheMatch(Player.eNumberOfPlayer i_NumberOfPlayer)
        {
            Player.eShapeOfCoin shapeOfCurrentPlayer = (Player.eShapeOfCoin)m_Players[(int)i_NumberOfPlayer].ShapeOfCoin;
            bool foundedFourCoins = false;

            for (int i = m_BoardGame.Rows - k_IndexFix; i >= 0 && !foundedFourCoins; i--)
            {
                for (int j = m_BoardGame.Cols - k_IndexFix; j >= 0 && !foundedFourCoins; j--)
                {
                    if (m_BoardGame.GetCharByIndexes(i, j) == (char)shapeOfCurrentPlayer)
                    {
                        foundedFourCoins = fourInVerticalDown(i, j, shapeOfCurrentPlayer) ||
                                           fourInHorizonRight(i, j, shapeOfCurrentPlayer) ||
                                           fourInDiagonalDownRight(i, j, shapeOfCurrentPlayer) ||
                                           fourInDiagonalUpRight(i, j, shapeOfCurrentPlayer);
                    }
                }
            }

            return foundedFourCoins;
        }

        ////check if there are 4 in-a-row - diagonal up right
        private bool fourInDiagonalUpRight(int i_RowToCheck, int i_ColToCheck, Player.eShapeOfCoin i_ShapeToCheck)
        {
            bool foundedFourCoins = false;

            if (i_ColToCheck + 3 < m_BoardGame.Cols && i_RowToCheck - 3 >= 0)
            {
                foundedFourCoins = m_BoardGame.GetCharByIndexes(i_RowToCheck - 1, i_ColToCheck + 1) == (char)i_ShapeToCheck &&
                                    m_BoardGame.GetCharByIndexes(i_RowToCheck - 2, i_ColToCheck + 2) == (char)i_ShapeToCheck &&
                                    m_BoardGame.GetCharByIndexes(i_RowToCheck - 3, i_ColToCheck + 3) == (char)i_ShapeToCheck;
            }

            return foundedFourCoins;
        }

        ////check if there are 4 in-a-row - diagonal down right
        private bool fourInDiagonalDownRight(int i_RowToCheck, int i_ColToCheck, Player.eShapeOfCoin i_ShapeToCheck)
        {
            bool foundedFourCoins = false;

            if (i_ColToCheck + 3 < m_BoardGame.Cols && i_RowToCheck + 3 < m_BoardGame.Rows)
            {
                foundedFourCoins = m_BoardGame.GetCharByIndexes(i_RowToCheck + 1, i_ColToCheck + 1) == (char)i_ShapeToCheck &&
                                    m_BoardGame.GetCharByIndexes(i_RowToCheck + 2, i_ColToCheck + 2) == (char)i_ShapeToCheck &&
                                    m_BoardGame.GetCharByIndexes(i_RowToCheck + 3, i_ColToCheck + 3) == (char)i_ShapeToCheck;
            }

            return foundedFourCoins;
        }

        ////check if there are 4 in-a-row - Horizion right
        private bool fourInHorizonRight(int i_RowToCheck, int i_ColToCheck, Player.eShapeOfCoin i_ShapeToCheck)
        {
            bool foundedFourCoins = false;

            if (i_ColToCheck + 3 < m_BoardGame.Cols)
            {
                foundedFourCoins = m_BoardGame.GetCharByIndexes(i_RowToCheck, i_ColToCheck + 1) == (char)i_ShapeToCheck &&
                                    m_BoardGame.GetCharByIndexes(i_RowToCheck, i_ColToCheck + 2) == (char)i_ShapeToCheck &&
                                    m_BoardGame.GetCharByIndexes(i_RowToCheck, i_ColToCheck + 3) == (char)i_ShapeToCheck;
            }

            return foundedFourCoins;
        }

        ////check if there are 4 in-a-row - vertical up down
        private bool fourInVerticalDown(int i_RowToCheck, int i_ColToCheck, Player.eShapeOfCoin i_ShapeToCheck)
        {
            bool foundedFourCoins = false;

            if (i_RowToCheck + 3 < m_BoardGame.Rows)
            {
                foundedFourCoins = m_BoardGame.GetCharByIndexes(i_RowToCheck + 1, i_ColToCheck) == (char)i_ShapeToCheck &&
                                    m_BoardGame.GetCharByIndexes(i_RowToCheck + 2, i_ColToCheck) == (char)i_ShapeToCheck &&
                                    m_BoardGame.GetCharByIndexes(i_RowToCheck + 3, i_ColToCheck) == (char)i_ShapeToCheck;
            }

            return foundedFourCoins;
        }

        ////clear the board and make new one with the same sizes 
        public void ResetBoard()
        {
            m_BoardGame.InitilizeBoard();
            m_BoardGame.InitilizeFreeColInBoardArray();
        }

        public void PlayOneTurn(
        string i_UserChoiceOfCol,
        ref int io_CountOfTurns,
        ref bool io_IsThereIsAWinner,
        ref bool io_MoveSucceeded)
        {
            if (MakeMove(i_UserChoiceOfCol, (Player.eNumberOfPlayer)(io_CountOfTurns % 2)))
            {
                io_MoveSucceeded = true;
                if (checkWinsOfTheMatch((Player.eNumberOfPlayer)(io_CountOfTurns % 2)))
                {
                    handleWinOfMatch(ref io_IsThereIsAWinner, io_CountOfTurns);
                }
            }
        }

        ////handle a win in a match
        private void handleWinOfMatch(ref bool io_IsThereIsAWinner, int i_CountOfTurns)
        {
            io_IsThereIsAWinner = true;
            IncreaseScore((Player.eNumberOfPlayer)(i_CountOfTurns % 2));
        }

        ////return the name of the winner
        public string extractNameOfWinner(Player.eNumberOfPlayer i_WhoMakeMove)
        {
            string nameOfWinner = string.Empty;
            if (i_WhoMakeMove == Player.eNumberOfPlayer.User)
            {
                nameOfWinner = m_Players[0].PlayerName;
            }
            else
            {
                nameOfWinner = m_Players[1].PlayerName;
            }

            return nameOfWinner;
        }
    }
}