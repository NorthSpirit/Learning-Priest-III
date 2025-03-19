using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Learning_Priest_III
{
    class Encounters
    {
        public bool runcombat = true;
        //For a single combat encounter
        public static void Combat(bool runcombat, Player pl, List<Figurine> en)
        {
            bool roundend = true;
            //Assign ids
            int hid = 1;
            foreach (Figurine i in pl.partyM) 
            {
                i.figHID = hid;
                hid++;
            }
            hid = 1;
            foreach (Figurine i in en)
            {
                i.figHID = hid;
                hid++;
            }
            foreach (Figurine f in en)
            {
                f.gainThreat();
            }
            foreach (Figurine f in  pl.partyM)
            {
                f.gainThreat();
            }
            while (runcombat == true)
            {
                while (pl.hadturn == false)
                {
                    if (pl.isalive == true) runRound(pl, en);
                    else runRoundDead(pl, en);
                }
                //Rest of your party acts
                Console.Clear();
                drawMenu(pl, en);
                Console.Write("Your party acts:\n");
                foreach (Figurine f in pl.partyM)
                {
                    fightRoundParty(f, en);
                }
                Console.ReadKey();
                Console.Clear();
                //Enemy acts
                drawMenu(pl, en);
                Console.Write("Enemy acts:\n");
                foreach(Figurine f in en)
                {
                    fightRoundParty(f, pl.partyM);
                }
                Console.ReadKey();
                Console.Clear();
                pl.hadturn = false;
                if (pl.partyM[0].figHP_C == 0) pl.isalive = false;
            }
        }
        //NPC Action
        public static void fightRoundParty(Figurine figG, List<Figurine> figurines)
        {
            if (figG.figtype != figTypes.Player) //Prevents player figurine from auto-acting
            {
                if (figG.figHP_C > 0)
                {
                    if (figG.figtype == figTypes.MagicAoe || figG.figtype == figTypes.BasicAoe)
                    {
                        if (figG.figtype == figTypes.MagicAoe) Console.Write("{0} casts a vicious spell.\n", figG.figName);
                        if (figG.figtype == figTypes.BasicAoe) Console.Write("{0} shoots rapidly.\n", figG.figName);
                        foreach (Figurine f in figurines)
                        {
                            figG.actionRegular(f);
                        }
                    }
                    else
                    {
                        figG.pickTarget(figurines);
                        foreach (Figurine f in figurines)
                        {
                            if (f.figHID == figG.figTID)
                            {
                                figG.actionRegular(f);
                            }
                        }
                    }
                }
            }
        }

        //Player actions
        /*
        public static void useWandOld(Player pl, int mana, List<Figurine> en)
        {
            string fail1 = "Please specify a target by writing it's position number in your Party. Number has to be between 0 and the size of your party.";
            string fail2 = "Your specified target must be still alive to be targeted by wand.";

            string input;
            int inputtoint;
            bool inputparse;
            //Add mana to the pool, prevent overflow
            if (pl.Mana_C < pl.Mana_M)
            {
                if ((pl.Mana_C + mana) > pl.Mana_M) pl.Mana_C = pl.Mana_M;
                else pl.Mana_C += mana;
            }
            //Target selection
            Console.Write("Please specify the target.\n");
            input = Convert.ToString(Console.ReadLine().ToLower());
            inputparse = int.TryParse(input, out inputtoint);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("TESTS:\nInput: {0}\nInputtoint: {1}\nnputparse: {2}\n.", input, inputtoint, inputparse);
            Console.ForegroundColor = ConsoleColor.Gray;
            //Not valid target - bad input
            if (!inputparse || inputtoint < 1 || inputtoint > pl.partyM.Count)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Please specify a target by writing it's position number in your Party. Number has to be between 0 and the size of your party.");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.ReadKey();
            }
            else
            {
                if (en[inputtoint - 1].figHP_C <= 0) //Not valid target - dead target
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Your specified target must be still alive to be targeted by wand.");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.ReadKey();
                }
                //Perform wand action.
                else
                {
                    pl.partyM[0].actionnormalFig(en[inputtoint - 1]);
                }
            }
        }
        */
        public static void useWand(Player pl, int mana, List<Figurine> en)
        {
            string fail1 = "Please specify a target enemy by writing it's position number in their Party.";
            string fail2 = "Your specified target must be still alive to be targeted by your wand.";
            (int target, bool legal) = checkTargetLegality(en, fail1, fail2);
            if (legal == true)
            {
                pl.partyM[0].actionRegular(en[target]);
                if (pl.Mana_C < pl.Mana_M)
                {
                    if ((pl.Mana_C + mana) > pl.Mana_M) pl.Mana_C = pl.Mana_M;
                    else pl.Mana_C += mana;
                }
                pl.hadturn = true;
            }   
        }
        public static void useHeal(Player pl, int mana)
        {
            int healmod = 3;
            string fail1 = "Please specify a friendly target by writing it's position number in your Party.";
            string fail2 = "Sorry, heal spell is to weak to revive a dead target. Please select a living target instead.";
            (int target, bool legal) = checkTargetLegality(pl.partyM, fail1, fail2);
            //Mana cost check
            if (pl.Mana_C < mana)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("You don't have enough mana to cast Heal. You should use your wand for a couple of turns to restore it.\n");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else
            {
                if (legal == true)
                {
                    Console.Write("You cast Heal.\n");
                    pl.Mana_C -= mana;
                    pl.hadturn = true;
                    healNoOverheal(pl.partyM[target], pl.partyM[0], healmod);
                }
            }
        }
        public static void useBindingHeal(Player pl, int mana)
        {
            int healmod = 3;
            string fail1 = "Please specify a friendly target by writing it's position number in your Party.";
            string fail2 = "Sorry, binding heal spell is to weak to revive a dead friend, but you can still use simple healing spell to heal yourself.";
            (int target, bool legal) = checkTargetLegality(pl.partyM, fail1, fail2);
            //Mana cost check
            if (pl.Mana_C < mana)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("You don't have enough mana to cast Binding Heal. You should use your wand for a couple of turns to restore it.\n");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else
            {
                if (legal == true)
                {
                    Console.Write("You cast Binding Heal.\n");
                    pl.Mana_C -= mana;
                    pl.hadturn = true;
                    healNoOverheal(pl.partyM[target], pl.partyM[0], healmod);
                    healNoOverheal(pl.partyM[0], pl.partyM[0], healmod);
                }
            }
        }
        public static void useHealingWave(Player pl, int mana)
        {
            int deadcounter = 0;
            int half = pl.partyM.Count / 2;
            int healmod = 2;
            //Mana cost check
            if (pl.Mana_C < mana)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("You don't have enough mana to cast Binding Heal. You should use your wand for a couple of turns to restore it.\n");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else
            {
                foreach (Figurine f in pl.partyM)
                {
                    if (f.figHP_C <= 0 && f.figHID > half)
                    {
                        deadcounter++;
                        Console.Write("Testing: is selected for search: {0}\n", f.figName);
                    }
                }
                //Prevent casting if second half of the party is dead.
                if (deadcounter == half) 
                {
                    Console.Write("The second half of your party is dead, you shouldn't waste mana on casting Healing Wave.\n");
                }
                else
                {
                    Console.Write("You cast Healing Wave in the form of torrent of light.\n");
                    pl.Mana_M -= mana;
                    pl.hadturn = true;
                    foreach (Figurine f in pl.partyM)
                    {
                        if (f.figHP_C > 0 && f.figHID > half)
                        {
                            healNoOverheal(f, pl.partyM[0], healmod);
                        }
                    }
                }
            }
        }
        public static void useHolyNova(Player pl, List<Figurine> en, int mana)
        {
            int healmod = 2;
            Random rand = new Random();
            bool ismagic = true;
            string flavour = "{0} dazzles {1} for {2} points of damage.";
            //Mana cost check
            if (pl.Mana_C < mana)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("You don't have enough mana to cast Holy Nova, perhaps you can find a cheaper spell to cast instead.\n");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else
            {
                Console.Write("You cast Holy Nova.\n");
                pl.Mana_C -= mana;
                pl.hadturn = true;
                foreach (Figurine f in pl.partyM)
                {
                    if (f.figHP_C > 0) healNoOverheal(f, pl.partyM[0], healmod);
                }
                foreach (Figurine f in en)
                {
                    pl.partyM[0].calculateDamage(f, flavour, ismagic);
                }
            }
        }
        public static void usePowerShield(Player pl, int mana)
        {
            Random rand = new Random();
            int healmod = 4;
            int shielda = (rand.Next(pl.partyM[0].figDam_Min * healmod, pl.partyM[0].figDam_Max * healmod));
            string input;
            int inputtoint;
            bool inputparse;
            //Mana check
            if (pl.Mana_C < mana)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("You don't have enough mana to cast Binding Heal. You should use your wand for a couple of turns to restore it.\n");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else
            {
                //Target picker
                Console.Write("Please specify the target:\n");
                input = Convert.ToString(Console.ReadLine().ToLower());
                inputparse = int.TryParse(input, out inputtoint);
                if (!inputparse || inputtoint < 1 || inputtoint > pl.partyM.Count) //Taget illegal - bad input.
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Please specify a friendly target by writing it's position number in your Party.\n");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                else
                {
                    if (pl.partyM[inputtoint - 1].figHP_C <= 0) //Target illegal - dead.
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Sorry, it's too late, the target you want to shield is already dead.");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    else 
                    {
                        if (pl.partyM[inputtoint - 1].figHP_S >= (pl.partyM[inputtoint - 1].figHP_M / 3)) //Target illegal - too much shield.
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("Sorry, but this target can't receive any more shield.\n");
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                        else
                        {
                            if ((pl.partyM[inputtoint - 1].figHP_S + shielda) > (pl.partyM[inputtoint - 1].figHP_M / 3))
                            {
                                shielda = (pl.partyM[inputtoint - 1].figHP_M / 3) - pl.partyM[inputtoint - 1].figHP_S;
                                pl.partyM[inputtoint - 1].figHP_S = (pl.partyM[inputtoint - 1].figHP_M / 3);
                            }
                            else
                            {
                                pl.partyM[inputtoint - 1].figHP_S += shielda;
                            }
                            Console.Write("You cast Shield of Power.\n");
                            Console.Write("{0} received {1} points of shielding.\n", pl.partyM[inputtoint - 1].figName, shielda);
                            pl.Mana_C -= mana;
                            pl.hadturn = true;
                        }
                    }
                }
            }

        }
        public static void useRevive(Player pl)
        {
            string input;
            int inputtoint;
            bool inputparse;
            //BRESES check
            if (pl.BRESes < 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Sorry, you are all out of Battle Resurect points.\n");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else
            {
                Console.Write("Please specify the target:\n");
                input = Convert.ToString(Console.ReadLine().ToLower());
                inputparse = int.TryParse(input, out inputtoint);
                if (!inputparse || inputtoint < 1 || inputtoint > pl.partyM.Count) //Taget illegal - bad input.
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Please select a dead member of your party.\n");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                else
                {
                    if (pl.partyM[inputtoint - 1].figHP_C > 0) //Target is still alive
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Rejoyce, it appears {0} is still alive and kicking. Please select a dead party member.\n", pl.partyM[inputtoint - 1].figName);
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    else
                    {
                        pl.BRESes--;
                        pl.partyM[inputtoint - 1].figHP_C = pl.partyM[inputtoint - 1].figHP_M / 2;
                        Console.Write("{0} has returned to the land of living.\n", pl.partyM[inputtoint - 1].figName);
                        pl.hadturn = true;
                    }
                }
            }
        }
        public static void useTESTDamageFriendly(Player pl)
        {
            string fail1 = "Please specify a friendly target by writing it's position number in your Party.";
            string fail2 = "You can't damage this target, it's already dead..";
            (int target, bool legal) = checkTargetLegality(pl.partyM, fail1, fail2);
            if (legal == true)
            {
                pl.partyM[0].actionRegular(pl.partyM[target]);
            }
        }
        public static void useTESTKillFriendly(Player pl)
        {
            string fail1 = "Please specify a friendly target by writing it's position number in your Party.";
            string fail2 = "You can't damage this target, it's already dead..";
            (int target, bool legal) = checkTargetLegality(pl.partyM, fail1, fail2);
            if (legal == true)
            {
                pl.partyM[target].figHP_C = 0;
                Console.Write("Test Action: {0} has been killed.\n", pl.partyM[target].figName);
            }
        }
        public static void useTESTKillEnemy(List<Figurine> en)
        {
            string fail1 = "Please specify a friendly target by writing it's position number in your Party.";
            string fail2 = "You can't damage this target, it's already dead..";
            (int target, bool legal) = checkTargetLegality(en, fail1, fail2);
            if (legal == true)
            {
                en[target].figHP_C = 0;
                Console.Write("Test Action: {0} has been killed.\n", en[target].figName);
            }
        }
        //Aidles:
        //Tuple to return both bool and an int
        static public (int target, bool legaltarget) checkTargetLegality(List<Figurine> figurines, string fail1, string fail2)
        {
            string input;
            int inputtoint;
            bool inputparse;
            Console.Write("Please specify the target:\n");
            input = Convert.ToString(Console.ReadLine().ToLower());
            inputparse = int.TryParse(input, out inputtoint);
            if (!inputparse || inputtoint < 1 || inputtoint > figurines.Count) //Taget illegal - bad input.
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(fail1);
                Console.ForegroundColor = ConsoleColor.Gray;
                return (-1, false);
            }
            else
            {
                if (figurines[inputtoint - 1].figHP_C <= 0) //Target illegal - dead.
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(fail2);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    return (-1, false);
                }
                else //Target is legal.
                {
                    return ((inputtoint - 1), true);
                }
            }
        }
        public static void healNoOverheal(Figurine figR, Figurine figG, int hmod)
        {
            Random rand = new Random();
            int heala = (rand.Next(figG.figDam_Min * hmod, figG.figDam_Max * hmod));
            int healr;
            if ((figR.figHP_C + heala) > figR.figHP_M)
            {
                healr = Math.Abs(figR.figHP_M - figR.figHP_C);
                figR.figHP_C = figR.figHP_M;
            }
            else
            {
                figR.figHP_C += heala;
                healr = heala;
            }
            Console.Write("{0} received {1} points of healing.\n", figR.figName, healr);
        }
        public static void drawMenu(Player pl, List<Figurine> en)
        {
            Console.Write(">---------------------------------------------<\n");
            Console.Write("Enemies:\n");
            foreach (Figurine i in en)
            {
                i.infoFigMenu();
            }
            Console.Write(">---------------------------------------------<\n");
            Console.Write("{0}:\n", pl.partyname);
            Console.Write("Mana: (");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("{0} / {1}", pl.Mana_M, pl.Mana_C);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(")\nBattle Ressurects: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("{0}\n", pl.BRESes);
            Console.ForegroundColor = ConsoleColor.Gray;
            foreach (Figurine i in pl.partyM)
            {
                i.infoFigMenu();
            }
            Console.Write(">---------------------------------------------<\n");
        }
        public static void runRound(Player pl, List<Figurine> en)
        {
            int mana1 = 5;
            int mana2 = 10;
            int mana3 = 15;
            int mana4 = 20;
            int mana5 = 40;
            int mana6 = 15;
            string input;
            Console.Clear();
            drawMenu(pl, en);
            Console.Write("Actions:\n");
            Console.Write("(1) Wand - Deals damage to a specified enemy. Restores: ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("{0} mana\n", mana1);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("(2) Heal - Heals a specified friendly target. Costs: ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("{0} mana\n", mana2);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("(3) Binding Heal - Heals yourself and another friendly target. Costs: ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("{0} mana\n", mana3);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("(4) Healing Wave - Heals the 2nd half of your party. Costs: ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("{0} mana\n", mana4);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("(5) Holy Nova - Heals every member of your party and deals damage to every enemy. Costs: ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("{0} mana\n", mana5);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("(6) Power Shield - Surrounds a party member with a protective barrier, shield can't get above 1/3rd of targets total health. Costs: ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("{0} mana\n", mana6);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("(7) Revives a fallen party member. Costs: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("1 battle resurrect point.\n");
            Console.ForegroundColor = ConsoleColor.Gray;
            //Action picker
            input = Convert.ToString(Console.ReadLine().ToLower());
            //Action 1 (Wand)
            if (input == "wand" || input == "1")
            {
                useWand(pl, mana1, en);
            }
            //Action 2 (Heal)
            else if (input == "heal" || input == "2")
            {
                useHeal(pl, mana2);
            }

            //Action 3 (Bind Heal)
            else if (input == "binding heal" || input == "3")
            {
                useBindingHeal(pl, mana3);
            }
            //Action 4 (Healing wave)
            else if (input == "healing wave" || input == "4")
            {
                useHealingWave(pl, mana4);
            }
            //Action 5 (Holy Nova)
            else if (input == "holy nova" || input == "5")
            {
                useHolyNova(pl, en, mana5);
            }
            //Action 6 (Power Shield)
            else if (input == "power shield" || input == "6")
            {
                usePowerShield(pl, mana6);
            }
            //Action 7 (Revive)
            else if (input == "revive" || input == "7")
            {
                useRevive(pl);
            }
            //Testing Action: Damage everyone
            else if (input == "testaoe")
            {
                Console.Write("TESTING ACTION, DAMAGE EVERYTHING.\n");
                foreach (Figurine i in pl.partyM)
                {
                    i.figHP_C -= 15;
                }
                foreach (Figurine i in en)
                {
                    i.figHP_C -= 15;
                }
            }
            //Testing Action: Damage friendly
            else if (input == "testdmg")
            {
                useTESTDamageFriendly(pl);
            }
            //Testing Action Kill friend
            else if (input == "testkillf")
            {
                useTESTKillFriendly(pl);
            }
            //Testing Action Kill enemy
            else if (input == "testkille")
            {
                useTESTKillEnemy(en);
            }
            //Testing Action: End round
            else if (input == "testend")
            {
                Console.Write("Round ended manually.\n");
                pl.hadturn = true;
            }
            Console.ReadKey();
        }
        public static void runRoundDead(Player pl, List<Figurine> en)
        {
            Console.Clear();
            drawMenu(pl, en);
            Console.Write("You are currently dead, with nobody to revive you, you will remain so for the rest of the combat.\n");
            Console.Write("Should your party survive the encounter, you will be resurrected after combat.\n");
            Console.ReadKey();
            pl.hadturn = true;
        }
    }
}
