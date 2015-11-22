using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;

namespace LazyGraves
{
    internal class ComboHandler
    {
        public static AIHeroClient Player = ObjectManager.Player;

        public static void Combo()
        {
            var target = TargetSelector.SelectedTarget != null &&
                         TargetSelector.SelectedTarget.Distance(Player) < 2000
                ? TargetSelector.SelectedTarget
                : TargetSelector.GetTarget(1500, DamageType.Physical);

            if (!target.IsValidTarget() ||
                Player.IsDashing() || target.HasBuffOfType(BuffType.Invulnerability)
                || target.IsDead || target.IsZombie || Orbwalker.IsAutoAttacking)
                return;

            var tHp = target.Health + 20;

            if (tHp < Player.GetSpellDamage(target, SpellSlot.W) &&
                Spells.W.IsReady())
            {
                Spells.CastWkill(target);
            }

            else if (tHp < Player.GetSpellDamage(target, SpellSlot.Q) &&
                     Spells.Q.IsReady())
            {
                Spells.CastQkill(target);
                if (Spells.E.IsReady())
                    Spells.CastEq();
            }

            else if (tHp < Player.GetSpellDamage(target, SpellSlot.R) &&
                     Spells.R.IsReady())
            {
                Spells.CastRkill(target);
            }

            else if (tHp < Helpers.Rdmg(target) + Player.GetSpellDamage(target, SpellSlot.Q) &&
                     Spells.R.IsReady() && Spells.Q.IsReady())
            {
                Spells.CastQrCombo(target);
            }

            else
            {
                if (Spells.Q.IsReady())
                    Spells.CastCollisionQ(target);
                if (!target.CanMove)
                    Spells.CastSimpleQ(target);

                if (Spells.W.IsReady())
                    Spells.CastWGrass();
                Spells.CastWslow(target);

                if (Spells.E.IsReady())
                    Spells.CastEenemy();
                Spells.CastEself();
            }
        }
    }
}