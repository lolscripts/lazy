using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using SharpDX;

namespace LazyLucian
{
    internal class Helpers
    {
        //-------------------------------------------------------------------------------------------------------------------
        /*
        *      _____            _                 _   _                 
        *     |  __ \          | |               | | (_)                
        *     | |  | | ___  ___| | __ _ _ __ __ _| |_ _  ___  _ __  ___ 
        *     | |  | |/ _ \/ __| |/ _` | '__/ _` | __| |/ _ \| '_ \/ __|
        *     | |__| |  __/ (__| | (_| | | | (_| | |_| | (_) | | | \__ \
        *     |_____/ \___|\___|_|\__,_|_|  \__,_|\__|_|\___/|_| |_|___/
        *                                                               
        *                                                               
        */

        public static readonly Item Youmuu = new Item((int) ItemId.Youmuus_Ghostblade);
        public static float Qmana;
        public static float Wmana;
        public static float Emana;
        public static float Rmana;
        //-------------------------------------------------------------------------------------------------------------------
        /*
        *       _____      _     __  __                   
        *      / ____|    | |   |  \/  |                  
        *     | (___   ___| |_  | \  / | __ _ _ __   __ _ 
        *      \___ \ / _ \ __| | |\/| |/ _` | '_ \ / _` |
        *      ____) |  __/ |_  | |  | | (_| | | | | (_| |
        *     |_____/ \___|\__| |_|  |_|\__,_|_| |_|\__,_|
        *                                                 
        *                                                 
        */

        public static void SetMana()
        {
            if ((!Init.ComboMenu["manaCheck"].Cast<CheckBox>().CurrentValue &&
                 Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) ||
                ObjectManager.Player.HealthPercent < 20)
            {
                Qmana = 0;
                Wmana = 0;
                Emana = 0;
                Rmana = 0;
                return;
            }

            Qmana = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Q).SData.Mana;
            Wmana = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).SData.Mana;
            Emana = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.E).SData.Mana;

            if (!Spells.R.IsReady())
                Rmana = Qmana -
                        ObjectManager.Player.PARRegenRate*ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Q).Cooldown;
            else
                Rmana = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.R).SData.Mana;
        }

        //-------------------------------------------------------------------------------------------------------------------
        /*
        *       _____      _                          _____  __  __  _____ 
        *      / ____|    | |       /\        /\     |  __ \|  \/  |/ ____|
        *     | |  __  ___| |_     /  \      /  \    | |  | | \  / | |  __ 
        *     | | |_ |/ _ \ __|   / /\ \    / /\ \   | |  | | |\/| | | |_ |
        *     | |__| |  __/ |_   / ____ \  / ____ \  | |__| | |  | | |__| |
        *      \_____|\___|\__| /_/    \_\/_/    \_\ |_____/|_|  |_|\_____|
        *                                                                  
        *                                                                  
        */

        public static double AaDmg(Obj_AI_Base target)
        {
            if (ObjectManager.Player.Level > 12)
                return ObjectManager.Player.GetAutoAttackDamage(target)*1.3;
            if (ObjectManager.Player.Level > 6)
                return ObjectManager.Player.GetAutoAttackDamage(target)*1.4;
            if (ObjectManager.Player.Level > 0)
                return ObjectManager.Player.GetAutoAttackDamage(target)*1.5;
            return 0;
        }

        //-------------------------------------------------------------------------------------------------------------------
        /*
        *      _____       _                      _   _____              _ _      _   _             
        *     |  __ \     | |                    | | |  __ \            | (_)    | | (_)            
        *     | |  | | ___| | __ _ _   _  ___  __| | | |__) | __ ___  __| |_  ___| |_ _  ___  _ __  
        *     | |  | |/ _ \ |/ _` | | | |/ _ \/ _` | |  ___/ '__/ _ \/ _` | |/ __| __| |/ _ \| '_ \ 
        *     | |__| |  __/ | (_| | |_| |  __/ (_| | | |   | | |  __/ (_| | | (__| |_| | (_) | | | |
        *     |_____/ \___|_|\__,_|\__, |\___|\__,_| |_|   |_|  \___|\__,_|_|\___|\__|_|\___/|_| |_|
        *                           __/ |                                                           
        *                          |___/                                                            
        */

        public static Vector3 EqExtendedPrediction(Obj_AI_Base target, int delay)
        {
            var res = Spells.Q1.GetPrediction(target);
            var del = Prediction.Position.PredictUnitPosition(target, delay);

            var dif = del.To3D() - target.ServerPosition;
            return res.CastPosition + dif;
        }

        //-------------------------------------------------------------------------------------------------------------------
        /*
        *       _____ _          _             _      _                 _____       _                          _   _             
        *      / ____(_)        | |           | |    (_)               |_   _|     | |                        | | (_)            
        *     | |     _ _ __ ___| | ___ ______| |     _ _ __   ___ ______| |  _ __ | |_ ___ _ __ ___  ___  ___| |_ _  ___  _ __  
        *     | |    | | '__/ __| |/ _ \______| |    | | '_ \ / _ \______| | | '_ \| __/ _ \ '__/ __|/ _ \/ __| __| |/ _ \| '_ \ 
        *     | |____| | | | (__| |  __/      | |____| | | | |  __/     _| |_| | | | ||  __/ |  \__ \  __/ (__| |_| | (_) | | | |
        *      \_____|_|_|  \___|_|\___|      |______|_|_| |_|\___|    |_____|_| |_|\__\___|_|  |___/\___|\___|\__|_|\___/|_| |_|
        *                                                                                                       *Credits Corey(L$)             
        *                                                                                                                       
        */

        public static CircInter GetCircleLineInteraction(Vector3 from, Vector2 to, Vector2 cPos, float radius)
        {
            var res = new CircInter();
            var dx = from.X - to.X;
            var dy = from.Y - to.Y;
            var a = dx*dx + dy*dy;
            var b = 2*(dx*(to.X - cPos.X) + dy*(to.Y - cPos.Y));
            var c = (to.X - cPos.X)*(to.X - cPos.X) + (to.Y - cPos.Y)*(to.Y - cPos.Y) - radius*radius;
            var det = b*b - 4*a*c;

            if ((a <= 0.0000001) || (det < 0))
            {
                res.None = true;
            }
            else if (det == 0)
            {
                res.One = true;
                var t = -b/(2*a);
                res.Inter1 = new Vector2(to.X + t*dx, to.Y + t*dy);
            }
            else
            {
                var t = (float) ((-b + Math.Sqrt(det))/(2*a));
                res.Inter1 = new Vector2(to.X + t*dx, to.Y + t*dy);
                t = (float) ((-b - Math.Sqrt(det))/(2*a));
                res.Inter2 = new Vector2(to.X + t*dx, to.Y + t*dy);
            }
            return res;
        }

        //-------------------------------------------------------------------------------------------------------------------
        /*
        *      _    _ _ _      _____ _           _       
        *     | |  | | | |    / ____| |         | |      
        *     | |  | | | |_  | (___ | |__   ___ | |_ ___ 
        *     | |  | | | __|  \___ \| '_ \ / _ \| __/ __|
        *     | |__| | | |_   ____) | | | | (_) | |_\__ \
        *      \____/|_|\__| |_____/|_| |_|\___/ \__|___/
        *                               *Credits Sebby(L$)          
        *                                                
        */

        public static double NumShots()
        {
            var num = 7.5;
            switch (Spells.R.Level)
            {
                case 1:
                    num += 7.5*ObjectManager.Player.AttackSpeedMod*0.5;
                    break;
                case 2:
                    num += 9*ObjectManager.Player.AttackSpeedMod*0.5;
                    break;
                case 3:
                    num += 10.5*ObjectManager.Player.AttackSpeedMod*0.5;
                    break;
            }
            return num;
        }

        public static List<AIHeroClient> GetLowaiAiHeroClients(Vector3 position, float range)
        {
            return
                EntityManager.Heroes.Enemies.Where(
                    hero => hero.IsValidTarget() && (hero.Distance(position) <= 1000) && hero.HealthPercent <= 15)
                    .ToList();
        }

        public static bool IsSafePosition(Vector3 position)
        {
            var underTurret =
                EntityManager.Turrets.Enemies.FirstOrDefault(
                    turret => !turret.IsDead && turret.Distance(position) <= 950);
            var allies = EntityManager.Heroes.Allies.Count(
                allied => !allied.IsDead && allied.Distance(position) <= 800);
            var enemies = position.CountEnemiesInRange(1000);
            var lhEnemies = GetLowaiAiHeroClients(position, 800).Count();
            if (underTurret != null) return false;

            if (enemies == 1)
            {
                return true;
            }
            return allies + 1 > enemies - lhEnemies;
        }

        public class CircInter
        {
            public Vector2 Inter1;
            public Vector2 Inter2;
            public bool None;
            public bool One;

            public CircInter()
            {
                One = false;
                None = false;
                Inter1 = new Vector2();
                Inter2 = new Vector2();
            }

            public Vector2 GetBestInter(Obj_AI_Base target)
            {
                if (None)
                {
                    return new Vector2(0, 0);
                }

                if (One)
                {
                    return Inter1;
                }

                var dist1 = target.Distance(Inter1, true);
                var dist2 = target.Distance(Inter2, true);
                return dist1 > dist2 ? Inter2 : Inter1;
            }
        }
    }
}