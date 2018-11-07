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
    public partial class GameSettingForm : Form
    {
        private int m_Rows;
        private int m_Cols;
        private string m_FirstPlayerName; ////name of first player
        private string m_SecondPlayerName; ////name of second player
        private bool m_IsOpponentComputer; ////if the opponent computer

        public int Rows
        {
            get { return m_Rows; }
        }

        public int Cols
        {
            get { return m_Cols; }
        }

        public string FirstPlayerName
        {
            get { return m_FirstPlayerName; }
        }

        public string SecondPlayerName
        {
            get { return m_SecondPlayerName; }
        }

        public bool IsOpponentComputer
        {
            get { return m_IsOpponentComputer; }
        }

        public GameSettingForm()
        {
            InitializeComponent();
        }

        private void checkBoxPlayer2_CheckedChanged(object sender, EventArgs e)
        {
            textBoxPlayer2.Enabled = !textBoxPlayer2.Enabled;
            if (checkBoxPlayer2.Checked == true)
            {
                textBoxPlayer2.Text = string.Empty;
            }
            else
            {
                textBoxPlayer2.Text = "[Computer]";
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            m_FirstPlayerName = textBoxPlayer1.Text;
            m_SecondPlayerName = textBoxPlayer2.Text;
            m_Rows = (int)numericUpDownRowes.Value;
            m_Cols = (int)numericUpDownCols.Value;
            m_IsOpponentComputer = !textBoxPlayer2.Enabled;
            this.Hide();
        }

        private void textBoxPlayer2_TextChanged(object sender, EventArgs e)
        {
            startButton.Enabled =
                string.Compare(textBoxPlayer1.Text, string.Empty) != FormsManager.sr_StringEqual &&
                string.Compare(textBoxPlayer2.Text, string.Empty) != FormsManager.sr_StringEqual;
        }

        private void textBoxPlayer1_TextChanged(object sender, EventArgs e)
        {
            if (!checkBoxPlayer2.Checked)
            {
                startButton.Enabled = true;
                if (string.Compare(textBoxPlayer1.Text, string.Empty) == FormsManager.sr_StringEqual)
                {
                    startButton.Enabled = false;
                }
            }
        }
    }
}
