using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Logic;

namespace UserInterface
{
    public partial class BoardForm : Form
    {
        private readonly int r_NumberOfPlayers = 2; ////number of player
        private readonly int r_IndexFix = 1;

        private GameManager m_LogicGame; ////The logic game

        private int m_Rows;
        private int m_Cols;
        private int m_countTurns = 0; ////count whos turn play now

        private string m_FirstPlayerName;
        private string m_SecondPlayerName;

        private bool m_IsOpponentComputer;
        private bool m_AnotherGame = false;

        private Messages m_Message = null; ////display messege to messageBox

        private int[] m_LimitOfClicksForButton; ////limit the click the buttons
        private Button[] m_TopBoardButtons; ////button of the top board
        private Button[,] m_BoardButtons; ////button of the board itself
        private Label[] m_CompetingNames; ////labels of the competing names

        public static readonly int sr_XYStartLocation = 10; ////start XY location
        public static readonly int sr_SizeOfObjectInForm = 50; ////size of every oobject in the game

        ////c'tor - build the board
        public BoardForm(int i_RowsOfBoard, int i_ColsOfBoard, string i_FirstPlayerName, string i_SecondPlayerName)
        {
            int matchString;
            m_LimitOfClicksForButton = new int[i_ColsOfBoard];
            m_TopBoardButtons = new Button[i_ColsOfBoard];
            m_BoardButtons = new Button[i_RowsOfBoard, i_ColsOfBoard];

            m_Rows = i_RowsOfBoard;
            m_Cols = i_ColsOfBoard;

            m_FirstPlayerName = i_FirstPlayerName;
            m_SecondPlayerName = i_SecondPlayerName;

            matchString = string.Compare(i_SecondPlayerName, "[Computer]");
            m_IsOpponentComputer = matchString == FormsManager.sr_StringEqual;

            designBoard();

            m_LogicGame = new GameManager(
            i_RowsOfBoard, i_ColsOfBoard, m_IsOpponentComputer, i_FirstPlayerName, m_SecondPlayerName);
            InitializeComponent();
        }

        ////design the board form with buttons
        private void designBoard()
        {
            int locationButtonX = BoardForm.sr_XYStartLocation;
            int locationButtonY = BoardForm.sr_XYStartLocation;

            for (int i = 0; i < m_TopBoardButtons.Length; i++)
            {
                m_TopBoardButtons[i] = new Button();
                m_TopBoardButtons[i].Size = new Size(
                BoardForm.sr_SizeOfObjectInForm, BoardForm.sr_SizeOfObjectInForm);
                m_TopBoardButtons[i].Enabled = true;
                m_TopBoardButtons[i].Location = new Point(locationButtonX, locationButtonY);
                m_TopBoardButtons[i].Text = Convert.ToString(i + r_IndexFix);
                m_TopBoardButtons[i].Visible = true;
                m_TopBoardButtons[i].Click += new EventHandler(OnClicked);
                Controls.Add(m_TopBoardButtons[i]);
                m_LimitOfClicksForButton[i] = 0;
                locationButtonX += 2 * BoardForm.sr_SizeOfObjectInForm;
            }

            locationButtonX = BoardForm.sr_XYStartLocation;
            locationButtonY += BoardForm.sr_SizeOfObjectInForm;

            for (int i = 0; i < m_Rows; i++)
            {
                for (int j = 0; j < m_Cols; j++)
                {
                    m_BoardButtons[i, j] = new Button();
                    m_BoardButtons[i, j].Size = new Size(
                    BoardForm.sr_SizeOfObjectInForm, BoardForm.sr_SizeOfObjectInForm);
                    m_BoardButtons[i, j].Location = new Point(locationButtonX, locationButtonY);
                    locationButtonX += 2 * BoardForm.sr_SizeOfObjectInForm;
                    m_BoardButtons[i, j].Visible = true;
                    Controls.Add(m_BoardButtons[i, j]);
                }

                locationButtonX = BoardForm.sr_XYStartLocation;
                locationButtonY += BoardForm.sr_SizeOfObjectInForm;
            }

            designLabelsForScore(
            m_BoardButtons[m_Rows - r_IndexFix, 0].Location,
            m_BoardButtons[m_Rows - r_IndexFix, m_Cols - r_IndexFix].Location);
        }

        ////design the label on the board form
        private void designLabelsForScore(Point i_PointOfFirstButtonInLastRow, Point i_PointOfLastButtonInLastRow)
        {
            m_CompetingNames = createLabelArray(m_CompetingNames);

            settingsForLabelsInBoardForm(m_CompetingNames);

            m_CompetingNames[0].Location = new Point(
            i_PointOfFirstButtonInLastRow.X + (2 * BoardForm.sr_SizeOfObjectInForm),
            i_PointOfFirstButtonInLastRow.Y + (2 * BoardForm.sr_SizeOfObjectInForm));

            m_CompetingNames[1].Location = new Point(
            i_PointOfLastButtonInLastRow.X - (2 * BoardForm.sr_SizeOfObjectInForm),
            i_PointOfLastButtonInLastRow.Y + (2 * BoardForm.sr_SizeOfObjectInForm));

            putScoreInlabel();

            m_CompetingNames[0].Text = m_FirstPlayerName + ":";
            if (m_IsOpponentComputer == true)
            {
                m_SecondPlayerName = "Computer";
            }

            m_CompetingNames[1].Text = m_SecondPlayerName + ":";

            putScoreInlabel();
        }

        ////initialize the score in the labels
        private void putScoreInlabel()
        {
            foreach (Label label in m_CompetingNames)
            {
                label.Text += "0";
            }
        }

        ////place the labels in the board form
        private void settingsForLabelsInBoardForm(Label[] i_ArrayOflabels)
        {
            foreach (Label label in i_ArrayOflabels)
            {
                label.Size = new Size(
                BoardForm.sr_SizeOfObjectInForm * r_NumberOfPlayers,
                BoardForm.sr_SizeOfObjectInForm * r_NumberOfPlayers);

                label.Enabled = false;
                label.Visible = true;
                label.AutoEllipsis = true;
                Controls.Add(label);
            }
        }

        ////create the label array
        private Label[] createLabelArray(Label[] i_ArrayOfLabels)
        {
            i_ArrayOfLabels = new Label[r_NumberOfPlayers];

            for (int i = 0; i < i_ArrayOfLabels.Length; i++)
            {
                i_ArrayOfLabels[i] = new Label();
            }

            return i_ArrayOfLabels;
        }

        ////when player pressed a button the methos make the move
        private void OnClicked(object sender, EventArgs e)
        {
            Button colSelectedByUser = null;
            bool hasWinnterInMatch = false;
            Player.eNumberOfPlayer currentPlayer;
            Player.eTypeOfPlayer typeOfOpponent;

            typeOfOpponent = convertBoolToOpponentType();
            colSelectedByUser = colSelected();

            currentPlayer = (Player.eNumberOfPlayer)(m_countTurns % 2);

            makeMoveByOpponentType(typeOfOpponent, currentPlayer, colSelectedByUser, ref hasWinnterInMatch);

            if (m_AnotherGame == true)
            {
                startAnotherGame();
            }
        }

        ////setting for start another game
        private void startAnotherGame()
        {
            m_LogicGame.ResetBoard();
            copyLogicBoardToFormBoard();
            enabelButtons();
            resetClicksForButton();
            m_countTurns = 0;
            m_AnotherGame = false;
            updateScoreLabels();
        }

        ////update the score when finishing a game
        private void updateScoreLabels()
        {
            string firstLabelText, secondLabelText;
            int updatedScoreOfUser, updatedScoreOfOpponent;

            firstLabelText = m_CompetingNames[0].Text.Substring
            (0, m_CompetingNames[0].Text.Length - r_IndexFix);

            secondLabelText = m_CompetingNames[1].Text.Substring
            (0, m_CompetingNames[1].Text.Length - r_IndexFix);

            m_LogicGame.GetScoresOfPlayers(out updatedScoreOfUser, out updatedScoreOfOpponent);

            firstLabelText += updatedScoreOfUser.ToString();
            secondLabelText += updatedScoreOfOpponent.ToString();

            m_CompetingNames[0].Text = firstLabelText;
            m_CompetingNames[1].Text = secondLabelText;
        }

        ////reset the click limit after finishing a game
        private void resetClicksForButton()
        {
            for (int i = 0; i < m_LimitOfClicksForButton.Length; i++)
            {
                m_LimitOfClicksForButton[i] = 0;
            }
        }

        ////enabel all buttons after finishing a game
        private void enabelButtons()
        {
            foreach (Button button in m_TopBoardButtons)
            {
                button.Enabled = true;
            }
        }

        ////make the move by user choise, also do the computer move
        private void makeMoveByOpponentType(
        Player.eTypeOfPlayer i_TypeOfOpponent,
        Player.eNumberOfPlayer i_WhoMakeMove,
        Button i_ColChoise,
        ref bool io_HaveWinner)
        {
            bool isTheMoveSucceeded = false;
            int colFromConputer = 0;
            if (i_WhoMakeMove == Player.eNumberOfPlayer.User ||
              (i_WhoMakeMove == Player.eNumberOfPlayer.Opponent && i_TypeOfOpponent == Player.eTypeOfPlayer.Human))
            {
                m_LogicGame.PlayOneTurn(i_ColChoise.Text, ref m_countTurns, ref io_HaveWinner, ref isTheMoveSucceeded);
                updateAndCheckAfterMakeMove(i_ColChoise, isTheMoveSucceeded, io_HaveWinner, i_WhoMakeMove);
                if (i_TypeOfOpponent == Player.eTypeOfPlayer.Computer && !io_HaveWinner && !m_AnotherGame)
                {
                    colFromConputer = m_LogicGame.computerTurn(ref io_HaveWinner, ref m_countTurns);
                    updateAndCheckAfterMakeMove(
                    m_TopBoardButtons[colFromConputer - r_IndexFix],
                    isTheMoveSucceeded,
                    io_HaveWinner,
                    Player.eNumberOfPlayer.Opponent);
                }
            }
        }

        ////after every move the method update the board and check for winner/Tie
        private void updateAndCheckAfterMakeMove(
        Button i_ColToChoose,
        bool i_IsTheMoveSucceeded,
        bool i_HaveWinner,
        Player.eNumberOfPlayer i_WhoMakeMove)
        {
            string nameOfWinner = string.Empty;
            string topMessage = null;
            DialogResult resultFromMessageBox = DialogResult.No;
            m_LimitOfClicksForButton[int.Parse(i_ColToChoose.Text) - r_IndexFix]++;

            if (i_IsTheMoveSucceeded)
            {
                copyLogicBoardToFormBoard();
                if (i_HaveWinner || m_LogicGame.BoardGame.IsBoardFull())
                {
                    if (i_HaveWinner)
                    {
                        nameOfWinner = m_LogicGame.extractNameOfWinner(i_WhoMakeMove);
                        topMessage = Messages.sr_WinTopForm;
                    }
                    else if (m_LogicGame.BoardGame.IsBoardFull())
                    {
                        topMessage = Messages.sr_TieTopForm;
                    }

                    m_Message = new Messages(topMessage, nameOfWinner);

                    resultFromMessageBox = MessageBox.Show(
                    m_Message.Message, topMessage, MessageBoxButtons.YesNo);

                    m_countTurns = 0;
                }
                else
                {
                    m_countTurns++;
                }

                ////if there is a winner/Tie create messege and show it
                if (m_Message != null)
                {
                    if (resultFromMessageBox == DialogResult.No)
                    {
                        this.Close();
                    }
                    else
                    {
                        m_AnotherGame = true;
                    }

                    m_Message = null;
                }
            }

            ////if the col full the button will not be enabel anymore
            if (m_LimitOfClicksForButton[int.Parse(i_ColToChoose.Text) - r_IndexFix] == m_Rows)
            {
                i_ColToChoose.Enabled = false;
            }
        }

        ////check the opponent type
        private Player.eTypeOfPlayer convertBoolToOpponentType()
        {
            Player.eTypeOfPlayer typeOfPoayer;

            if (m_IsOpponentComputer == true)
            {
                typeOfPoayer = Player.eTypeOfPlayer.Computer;
            }
            else
            {
                typeOfPoayer = Player.eTypeOfPlayer.Human;
            }

            return typeOfPoayer;
        }

        ////copy the logic board to form board
        private void copyLogicBoardToFormBoard()
        {
            Board copiedLogicBoard = m_LogicGame.BoardGame;
            for (int i = 0; i < m_Rows; i++)
            {
                for (int j = 0; j < m_Cols; j++)
                {
                    m_BoardButtons[i, j].Text = Convert.ToString(copiedLogicBoard.GetCharByIndexes(i, j));
                }
            }
        }

        ////check which button the user/computer selected
        private Button colSelected()
        {
            Button userChoiseOfCol = null;
            foreach (Button button in m_TopBoardButtons)
            {
                if (button.Focused == true)
                {
                    userChoiseOfCol = button;
                    break;
                }
            }

            return userChoiseOfCol;
        }
    }
}