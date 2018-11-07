using System;

namespace Logic
{
    public struct Player
    {
        private readonly eShapeOfCoin r_ShapeOfCoin; //// coin of the player
        private readonly eTypeOfPlayer r_TypeOfPlayer; //// if the player Human of Computer
        private string m_PlayerName;
        private int m_ScoreOfPlayer; ////score of the player

        ////the shape of the coin
        public enum eShapeOfCoin
        {
            X = 'X',
            O = 'O',
        }

        ////the type of the player
        public enum eTypeOfPlayer
        {
            Human = 'H',
            Computer = 'C',
        }

        ////the number of the player
        public enum eNumberOfPlayer
        {
            User,
            Opponent,
        }

        ////who pressed Q
        public enum ePlayerWhoPressedQ
        {
            User,
            Opponent,
            None,
        }

        ////c'tor - create new player
        public Player(eShapeOfCoin i_ShapeOfCoinToInput, eTypeOfPlayer i_TypeOfPlayerToInput, string i_PlayerName)
        {
            this.m_ScoreOfPlayer = 0;
            this.r_ShapeOfCoin = i_ShapeOfCoinToInput;
            this.r_TypeOfPlayer = i_TypeOfPlayerToInput;
            this.m_PlayerName = i_PlayerName;
        }

        ////get the shape of coin
        public eShapeOfCoin ShapeOfCoin
        {
            get { return r_ShapeOfCoin; }
        }

        ////get/set score of player
        public int ScoreOfPlayer
        {
            get { return m_ScoreOfPlayer; }
            set { m_ScoreOfPlayer = value; }
        }

        public string PlayerName
        {
            get { return m_PlayerName; }
            set { m_PlayerName = value; }
        }
    }
}
