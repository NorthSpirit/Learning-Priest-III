using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learning_Priest_III
{
    public class Player
    {
        public string name = "Protagonist";
        public string partyname = "The Drunken Sailors";
        public int Mana_M = 100;
        public int Mana_C = 100;
        public int Silver = 50;
        public int BRESes = 2;
        public int lives = 3;
        public bool isalive = true;
        public bool hadturn = false;
        public int healingWasted  = 0;
        public List<Figurine> partyM = new List<Figurine>() //Main Party (Currently in use)
        {
            new Figurine("Protagonist", 70, 1, 2, 4, 5, figTypes.Player)
        };
    }
}
