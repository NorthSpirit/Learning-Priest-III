using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learning_Priest_III
{
    public class Figurine
    {
        public string figName; //Figurine Name
        public int figHP_M; //Health MAX
        public int figHP_C; //Health Current
        public int figHP_S; //Health Shield
        public int figArm; //Armor value
        public int figRes; //Resistance value
        public int figDam_Min; //Min damage
        public int figDam_Max; //Max damage
        public int figTP = 0; //Threat points, max 100
        public int figSp_C; //Special points current
        public int figSp_M; //Special points max
        public figTypes figtype; //Figurine type
        public int figHID = -1; //Temporary ID for targeting
        public int figTID = 1; //ID of the picked target

        //Constructor
        public Figurine(string figName = "Footman", int figHP = 80, int figArm = 3, int figRes = 1, int figDam_Min = 7, int figDam_Max = 13, figTypes figtype = figTypes.Basic, int figSp_M = 3)
        {
            this.figName = figName;
            this.figHP_M = figHP;
            this.figHP_C = figHP;
            this.figArm = figArm;
            this.figRes = figRes;
            this.figDam_Min = figDam_Min;
            this.figDam_Max = figDam_Max;
            this.figtype = figtype;
            this.figSp_M = figSp_M;
        }

        //Commands
        public void infoFigMenu()
        {
            if (figHP_C <= 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("{0}) {1} is currently dead.\n", figHID, figName);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else
            {
                Console.Write("{0}) {1}\nHealth: ", figHID, figName);
                Console.ForegroundColor= ConsoleColor.Green;
                Console.Write("{0}", figHP_M);
                Console.ForegroundColor= ConsoleColor.Gray;
                Console.Write("/");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("{0} + ({1})\n", figHP_C, figHP_S);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("Threat: ");
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write("{0}\n", figTP);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
        public void infoFigFull()
        {
            string flavourtype, flavourattack, flavourspecial, flavourdesc;
            if (figtype == figTypes.Player)
            {
                flavourtype = "Priest Disciple";
                flavourattack = "Wand Sparkle - a single target attack. Reduced by Resistance.";
                flavourspecial = "Holy Magic";
                flavourdesc = "Allows you to cast all sorts of beneficial spells.";
            }
            else if (figtype == figTypes.Magic)
            {
                flavourtype = "Pyromage";
                flavourattack = "Pyroblast - a high damage single target spell. Reduced by Resistance.";
                flavourspecial = "Molten Armor";
                flavourdesc = "Surrounds the mage with the protective shield.";
            }
            else if (figtype == figTypes.MagicAoe)
            {
                flavourtype = "Warlock";
                flavourattack = "Rein of Fire - a vile rain of fire, which damages everyone in the enemy party. Reduced by Resistance.";
                flavourspecial = "Vanish";
                flavourdesc = "Warlock surrounds itself with smoke, reducing Threat Points to 0.";
            }
            else if (figtype == figTypes.Basic)
            {
                flavourtype = "Soldier";
                flavourattack = "Mighty Strike - a powerful single target attack. Reduced by Armor.";
                flavourspecial = "Judgement";
                flavourdesc = "Deals damage to a single target, based on missing health points. Reduced by Resistance.";
            }
            else if (figtype == figTypes.BasicAoe)
            {
                flavourtype = "Marksman";
                flavourattack = "Rapid Firing - shoots arrows at every single target in the enemy party. Reduced by Armor.";
                flavourspecial = "Snipe";
                flavourdesc = "Fires a single target arrow at the target with lowest current health points. Reduced by Armor.";
            }
            else if (figtype == figTypes.Tank)
            {
                flavourtype = "Protector";
                flavourattack = "Shield Slam - slams a single enemy target, generating a lot of threat. Reduced by Armor.";
                flavourspecial = "Glorious Shout";
                flavourdesc = "Greatly reduces threat of every ally in the party.";
            }
            else
            {
                flavourtype = "Indescribable Horror";
                flavourattack = "Horrible Attack - all we know, it's a single target attack. Reduced by Armor.";
                flavourspecial = "Unknown";
                flavourdesc = "We are not really sure.";
            }
            infoFigGeneric(flavourtype, flavourattack, flavourspecial, flavourdesc);
            Console.ReadLine();
        }
        public void infoFigGeneric(string flavourtype, string flavourattack, string flavourspecial, string flavourdesc)
        {
            Console.Write(">---------------------------------------------<\n");
            Console.Write("Full Info:\n");
            Console.Write("{0} The {1}", figName, flavourtype);
            Console.Write(">---------------------------------------------<\n");
            Console.Write("Health Total: {0}\nHealth Current: ",figHP_M);

            if (figHP_C > 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("Dead\n");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else Console.Write("{0}\n", figHP_C);
            Console.Write("Shield Max: {0}\nShield Current: {1}\n", (figHP_M / 3), figHP_S);
            Console.Write("Attack: {0} - {1]\n{2}\n", figDam_Min, figDam_Max, flavourattack);
            Console.Write("Armor: {0}\nMagic Resistance: {1}\n", figArm, figRes);
            Console.Write("Threat: ");
            if (figTP == 100)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("{0}\n", figTP);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else if (figTP == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("None\n");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else Console.Write("{0}\n", figTP);
            Console.Write("Special move: {0}\n{1}\nSpecial points: {2} / {3}\n", flavourspecial, flavourdesc);
            Console.Write("Special points: ");
            if (figtype != figTypes.Player) Console.Write("{0} / {1}\n", figSp_M, figSp_C);
            else Console.Write("Protagonists need no special points.\n");
        }
        public void renameFig(string newname)
        {
            Console.Write("Please input a new name:\n");
            this.figName = newname;
            Console.Write("Character's name has been changed to \"{0}\"\n", figName);
            Console.Write(">---------------------------------------------<\n");
        }
        public void actionRegular(Figurine figR)
        {
            bool ismagic = false;
            string flavour;
            if (figtype == figTypes.Player)
            {
                flavour = "{0} uses it's wand to deal {2} damage to {1}.";
                ismagic = true;
            }
            else if (this.figtype == figTypes.Basic || this.figtype == figTypes.BasicAoe)
            {
                flavour = "{0} strikes {1} for {2} points of damage.";
            }
            else if (this.figtype == figTypes.Magic)
            {
                flavour = "{0} targets {1} with a spell, it deals {2} points of damage.";
                ismagic = true;
            }
            else if (this.figtype == figTypes.MagicAoe)
            {
                flavour = "{0} blasts {1} for {2} points of damage.";
                ismagic = true;
            }
            else if (this.figtype == figTypes.Tank)
            {
                flavour = "{0} bashes {1} with a shield, it deals {2} points of damage.";
            }
            else
            {
                flavour = "{0} deals {1} points of damage to {2}.";
            }
            calculateDamage(figR, flavour, ismagic);
            gainThreat();
        }
        public static void actionSpecial()
        {

        }
        public void gainThreat()
        {
            int threatgain;
            if (this.figtype == figTypes.Player || this.figtype == figTypes.Magic) threatgain = 6;
            else if (this.figtype == figTypes.MagicAoe || this.figtype == figTypes.BasicAoe) threatgain = 3;
            else if (this.figtype == figTypes.Tank) threatgain = 20;
            else threatgain = 8;

            if ((this.figTP + threatgain) > 100)
            {
                this.figTP = 100;
            }
            else
            {
                this.figTP += threatgain;
            }
        }
        public void pickTarget(List<Figurine> figurines)
        {
            //Checks the list of figurines and finds the corresponding HID
            int checkfor = 0;
            //Based on highest threat
            foreach (Figurine i in figurines)
            {
                if (i.figTP >= checkfor && i.figHP_C > 0)
                {
                    checkfor = i.figTP;
                    figTID = i.figHID;
                }
            }
        }
        public void calculateDamage(Figurine figR, string flavour, bool ismagic)
        {
            Random rand = new Random();
            int damagedeal = (rand.Next(this.figDam_Min, this.figDam_Max));
            if (ismagic == true)
            {
                damagedeal -= figRes;
            }
            else
            {
                damagedeal -= figArm;
            }
            if (damagedeal < 1) damagedeal = 1;
            var flav = string.Format(flavour, this.figName, figR.figName, damagedeal);
            if (figR.figHP_S > damagedeal)
            {
                figR.figHP_S = figR.figHP_S - damagedeal;
                damagedeal = 0;
            }
            else
            {
                damagedeal = damagedeal - figR.figHP_S;
                if (damagedeal < 0) damagedeal = 0;
                figR.figHP_S = 0;
            }
            if (figR.figHP_C > 0)
            {
                figR.figHP_C -= damagedeal;
                Console.WriteLine(flav);
                if (figR.figHP_C <= 0)
                {
                    figR.figHP_C = 0;
                    Console.Write("{0} drops dead.\n", figR.figName);
                }
            }
        }
        public void gainShield(int shieldgain)
        {
            int maxshield = figHP_M / 3;
            this.figHP_M += shieldgain;
            if (this.figHP_S > maxshield) figHP_S = maxshield;
        }
        /*        
        Player,
        Basic,
        BasicAoe,
        Magic,
        MagicAoe,
        Tank,
        Special,
        */
    }
}
