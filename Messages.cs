using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInterface
{
    public class Messages
    {
        public static readonly string sr_WinTopForm = "A Win!";
        public static readonly string sr_TieTopForm = "A Tie!";
        private static readonly string sr_WonMessage = " Won!!";
        private static readonly string sr_TieMessage = " Tie!!";
        private static readonly string sr_AnotherRoundMessage = "Another Round?";

        private string m_Message = null;

        public string Message
        {
            get { return m_Message; }
        }

        ////c'tor - build the message to the messageBox
        public Messages(string i_StatusMessage, string i_NameOfWinner)
        {
            m_Message = i_NameOfWinner;
            if (string.Compare(i_StatusMessage, Messages.sr_WinTopForm) == FormsManager.sr_StringEqual)
            {
                m_Message += Messages.sr_WonMessage;
            }
            else
            {
                m_Message += Messages.sr_TieMessage;
            }

            m_Message += Environment.NewLine + Messages.sr_AnotherRoundMessage;
        }
    }
}
