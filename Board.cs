using System;
using System.Collections.Generic;

namespace Logic
{
    public class Board
    {
        private const int k_IndexFix = 1;

        private readonly int m_Rows; ////rows of the board
        private readonly int m_Cols; ////cols of the board
        public static readonly char sr_Space = ' ';

        private char[,] m_BoardOfTheGame; //// the board of the game
        private List<int> m_freeColInBoard; ////save the cols that is not full in the board

        ////c'tor - create the board and anitialize the list of the non empty cols.
        public Board(int i_RowsToInput, int i_ColsToInput)
        {
            m_Cols = i_ColsToInput;
            m_Rows = i_RowsToInput;
            InitilizeBoard();
            InitilizeFreeColInBoardArray();
        }

        ////save the not full col in the board
        public List<int> freeColInBoard
        {
            get { return m_freeColInBoard; }
        }

        ////build a new list, the list will save the random cols that we can random 
        public void InitilizeFreeColInBoardArray()
        {
            m_freeColInBoard = new List<int>(m_Cols);

            for (int i = 0; i < m_Cols; i++)
            {
                m_freeColInBoard.Add(i + 1);
            }
        }

        ////get char by 2 index (row and cols)
        public char GetCharByIndexes(int i_RowsIndex, int i_ColsIndex)
        {
            return m_BoardOfTheGame[i_RowsIndex, i_ColsIndex];
        }

        ////get rows of the board
        public int Rows
        {
            get { return m_Rows; }
        }

        ////get cols of the board
        public int Cols
        {
            get { return m_Cols; }
        }

        ////check if the board full
        public bool IsBoardFull()
        {
            bool isFull = true;

            for (int i = 0; i < m_Rows && isFull; i++)
            {
                for (int j = 0; j < m_Cols && isFull; j++)
                {
                    if (m_BoardOfTheGame[i, j] == Board.sr_Space)
                    {
                        isFull = false;
                    }
                }
            }

            return isFull;
        }

        ////check if there is a free cell in secific col
        public Nullable<int> GetFreeIndex(int i_colToCheck)
        {
            Nullable<int> freeIndexFound = null;
            bool foundIndex = false;

            for (int i = Rows - k_IndexFix; i >= 0 && !foundIndex; i--)
            {
                if (GetCharByIndexes(i, i_colToCheck) == Board.sr_Space)
                {
                    freeIndexFound = i;
                    foundIndex = true;
                }
            }

            return freeIndexFound;
        }

        ////create a empty board
        public void InitilizeBoard()
        {
            m_BoardOfTheGame = new char[m_Rows, m_Cols];

            for (int i = 0; i < m_Rows; i++)
            {
                for (int j = 0; j < m_Cols; j++)
                {
                    m_BoardOfTheGame[i, j] = Board.sr_Space;
                }
            }

            InitilizeFreeColInBoardArray();
        }

        ////place the coin of player by row and col
        public void PlacePlayerShapeCoint(int i_Row, int i_Cols, char i_ShapeOfCoin)
        {
            if (m_BoardOfTheGame[i_Row, i_Cols] == sr_Space)
            {
                m_BoardOfTheGame[i_Row, i_Cols] = i_ShapeOfCoin;
            }

            if (i_Row == 0)
            {
                m_freeColInBoard.Remove(i_Cols + k_IndexFix);
            }
        }
    }
}