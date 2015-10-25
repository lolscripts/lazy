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
            if (ObjectManager.Player.IsRecalling() && MenuGUI.IsChatOpen) return;

            if (Init.ComboMenu["useRmanually"].Cast<KeyBind>().CurrentValue)
            {
                var target = TargetSelector.GetTarget(Spells.R.Range, DamageType.Physical);
                if (target == null) return;
                {
                    Spells.R.Cast(target);
                }
            }

            if (Init.MiscMenu["useKs"].Cast<CheckBox>().CurrentValue)
            {
                Spells.Ks();
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
            var dashPosition = e.Start.Extend(ObjectManager.Player.Position,
                e.Start.Distance(ObjectManager.Player.ServerPosition) + Spells.E.Range);

            if (!Spells.E.IsReady() || !sender.IsValidTarget(Spells.E.Range) ||
                !Init.MiscMenu["gapcloser"].Cast<CheckBox>().CurrentValue || e.Type == Gapcloser.GapcloserType.Targeted)
                return;
            {
                if ((!dashPosition.ToNavMeshCell().CollFlags.HasFlag(CollisionFlags.Wall)) &&
                    Helpers.IsSafePosition((Vector3) dashPosition))
                {
                    Spells.E.Cast((Vector3) dashPosition);
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
                    Orbwalker.OrbwalkTo(Game.CursorPos);
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