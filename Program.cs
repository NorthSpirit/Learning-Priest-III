using System;
using System.Reflection.Metadata.Ecma335;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Learning_Priest_III
{
    public enum figTypes
    {
        Player,
        Basic,
        BasicAoe,
        Magic,
        MagicAoe,
        Tank,
        Special,
    }
    class Program
    {
        static void Main()
        {
            Player player = new Player();
            string input;
            Console.Write("Hello and welcome to Learning Priest III\n");
            while (true)
            {
                Console.Write("Please write:\n(N)ew Game to start a new game\nOr\n(L)oad Game to load an old one.\n");
                input = Convert.ToString(Console.ReadLine().ToLower());
                if (input == "n" || input == "new" || input == "new game")
                {
                    Console.Clear();
                    startTest(player);
                }
                //TODO
                else if (input == "l" || input == "load" || input == "load game")
                {
                    Console.Write("Input for Loading received\nLoading functioality WIP\nPress any button to start a new game, sorry.");
                    Console.ReadKey();
                    Console.Clear();
                    startTest(player);
                }
                else
                {
                    Console.Clear();
                    Console.Write("Error: Unknown Input\n");
                }
            }
        }

        static void startTest(Player pl)
        {
            string input = "";
            Console.Write("Hello and welcome to Learning Priest III TESTING\nPlease tell us your name.\n");
            input = Convert.ToString(Console.ReadLine());
            if (input == "") input = "Protagonist";
            pl.name = input;
            pl.partyM[0].figName = input;
            pl.partyM.Add(new Figurine("TheHolyGuy", 80, 3, 3, 6, 16, figTypes.Basic));
            pl.partyM.Add(new Figurine("Asykum", 65, 2, 2, 5, 7, figTypes.BasicAoe));
            pl.partyM.Add(new Figurine("Pewpew", 50, 0, 2, 0, 30, figTypes.Magic));
            pl.partyM.Add(new Figurine("Sadiste", 45, 2, 2, 3, 9, figTypes.MagicAoe));
            pl.partyM.Add(new Figurine("Mahowla", 130, 5, 4, 4, 11, figTypes.Tank));
            List<Figurine> en = new List<Figurine>();
            addfigurine(en, "fi1");
            addfigurine(en, "fi2");
            addfigurine(en, "fi4");
            addfigurine(en, "fi1");
            addfigurine(en, "fi1");
            addfigurine(en, "fi3");
            Console.ReadKey();
            Console.Write("{0}", pl.name);
            Encounters.Combat(true, pl, en);
        }
        //Quick add
        static void addfigurine(List<Figurine> group, string input)
        {
            //Name, HP, Arm, Res, MinDmg, MaxDmg, Type
            input = input.ToLower();
            if (input == "player" || input == "fi0")
            {
                group.Add(new Figurine("Player", 40, 0, 2, 1, 4, figTypes.Player));
            }
            else if (input == "footman" || input == "fi1")
            {
                group.Add(new Figurine());
            }
            else if (input == "mage" || input == "fi2")
            {
                group.Add(new Figurine("Mage", 55,0,4,1,12,figTypes.Magic));
            }
            else if (input == "knight" || input == "fi3")
            {
                group.Add(new Figurine("Knight", 100, 5, 4, 5, 9, figTypes.Tank));
            }
            else if (input == "warlock" || input == "fi4")
            {
                group.Add(new Figurine("Warlock", 25, 0, 4, 1, 12, figTypes.MagicAoe));
            }
            else
            {
                Console.Write("\nError: UNKNOWN FIGURINE\n");
            }
        }
    }
}