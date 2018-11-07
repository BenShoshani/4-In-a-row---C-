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
    public partial class FormsManager : Form
    {
        public static readonly int sr_StringEqual = 0;

        private GameSettingForm m_GameSettingForm;
        private BoardForm m_BoardForm;

        ////c'tor of formManager
        public FormsManager()
        {
            ////build the setting form
            InitializeComponent();
            m_GameSettingForm = new GameSettingForm();
            m_GameSettingForm.ShowDialog();

            ////build the board form to play the game
            m_BoardForm = new BoardForm(
            m_GameSettingForm.Rows, m_GameSettingForm.Cols, m_GameSettingForm.FirstPlayerName, m_GameSettingForm.SecondPlayerName);
            m_BoardForm.ShowDialog();
            m_GameSettingForm.Close();
        }
    }
}
