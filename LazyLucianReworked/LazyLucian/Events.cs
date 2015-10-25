using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Constants;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using SharpDX;

namespace LazyLucian
{
    internal class Events
    {
        public static bool PassiveUp;
        public static float RLastCast = Game.Time;

        public static void OnTick(EventArgs args)
        {
            if (ObjectManager.Player.IsRecalling() || MenuGUI.IsChatOpen) return;

            if (Init.MiscMenu["useKs"].Cast<CheckBox>().CurrentValue)
            {
                Spells.Ks();
            }
            if (Init.ComboMenu["useRkillable"].Cast<CheckBox>().CurrentValue)
            {
                
                    if (Game.Time - RLastCast > 5)
                    {
                        Spells.CastR();
                    }
                
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                ComboHandler.Combo();
            }
            else if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                HarassHandler.Harass();
            }
            else if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                FarmHandler.LaneClear();
                FarmHandler.JungleClear();
            }
        }

        public static void OnGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            var playerPosition = ObjectManager.Player.Position.To2D();
            var direction1 = (ObjectManager.Player.ServerPosition - sender.ServerPosition).To2D().Normalized();
            const int distance = 475;
            const int stepSize = 40;

            if (!Spells.E.IsReady() || !sender.IsValidTarget(Spells.E.Range) ||
                !Init.MiscMenu["gapcloser"].Cast<CheckBox>().CurrentValue || e.Type == Gapcloser.GapcloserType.Targeted)
                return;
            {
                for (var step = 0f; step < 360; step += stepSize)
                {
                    var currentAngel = step * (float)Math.PI / 90;
                    var currentCheckPoint = playerPosition +
                                            distance * direction1.Rotated(currentAngel);

                    if (!Helpers.IsSafePosition((Vector3)currentCheckPoint) ||
                        currentCheckPoint.ToNavMeshCell().CollFlags.HasFlag(CollisionFlags.Wall))
                        continue;
                    {
                        Spells.E.Cast((Vector3)currentCheckPoint);
                    }
                }
            }
        }

        public static void OnCastSpell(GameObject sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsDead || !sender.IsMe) return;
            if (args.SData.IsAutoAttack())
            {
                PassiveUp = false;
            }
            switch (args.Slot)
            {
                case SpellSlot.Q:
                case SpellSlot.W:
                    Orbwalker.ResetAutoAttack();
                    break;
                case SpellSlot.R:
                    PassiveUp = true;
                    RLastCast = Game.Time;
                    if (Helpers.Youmuu.IsReady() && Init.ComboMenu["useYoumuu"].Cast<CheckBox>().CurrentValue)
                    {
                        Helpers.Youmuu.Cast();
                    }
                    if (Init.ComboMenu["useRlock"].Cast<CheckBox>().CurrentValue)
                    {
                        Spells.LockR();
                    }
                    break;
            }
        }

        public static void OnProcessSpellCast(GameObject sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsDead || !sender.IsMe) return;
            {
                switch (args.Slot)
                {
                    case SpellSlot.Q:
                    case SpellSlot.W:
                    case SpellSlot.R:
                        PassiveUp = true;
                        break;
                    case SpellSlot.E:
                        Orbwalker.ResetAutoAttack();
                        PassiveUp = true;
                        break;
                }
            }
        }

        public bool CheckPassive()
        {
            if (ObjectManager.Player.HasBuff("LucianPassiveBuff"))
            {
                return PassiveUp = true;
            }
            return false;
        }
    }
}