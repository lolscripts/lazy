using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;

namespace LazyLucian
{
    internal class HarassHandler
    {
        public static void Harass()
        {
            var target = TargetSelector.SelectedTarget != null &&
                         TargetSelector.SelectedTarget.Distance(ObjectManager.Player) < 20000
                ? TargetSelector.SelectedTarget
                : TargetSelector.GetTarget(1500, DamageType.Physical);

            if (target == null ||
                (Init.HarassMenu["spellWeaving"].Cast<CheckBox>().CurrentValue && Events.PassiveUp) ||
                Orbwalker.IsAutoAttacking ||
                ObjectManager.Player.IsDashing())
                return;


            if (Spells.Q.IsReady())
            {
                if (Init.HarassMenu["useQharass"].Cast<CheckBox>().CurrentValue)
                {
                    Spells.CastQ();
                }
                if (Init.HarassMenu["useQextended"].Cast<CheckBox>().CurrentValue)
                {
                    Spells.CastExtendedQ();
                }
            }

            if (!Spells.W.IsReady()) return;
            {
                if (Init.HarassMenu["useWaaRange"].Cast<CheckBox>().CurrentValue)
                {
                    Spells.CastWinRange();
                }
                if (Init.HarassMenu["useWalways"].Cast<CheckBox>().CurrentValue)
                {
                    Spells.CastWcombo();
                }
            }
        }
    }
}