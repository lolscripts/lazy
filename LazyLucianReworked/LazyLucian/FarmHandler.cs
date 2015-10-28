using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using SharpDX;

namespace LazyLucian
{
    internal class FarmHandler
    {
        public static void LaneClear()
        {
            var minion =
                EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,
                    ObjectManager.Player.Position, 500);

            if (minion != null &&
                (Init.FarmMenu["spellWeaving"].Cast<CheckBox>().CurrentValue &&
                Events.PassiveUp) ||
                Orbwalker.IsAutoAttacking ||
                ObjectManager.Player.IsDashing())
                return;

            if (Spells.Q.IsReady() &&
                Init.FarmMenu["useQfarm"].Cast<CheckBox>().CurrentValue &&
                ObjectManager.Player.ManaPercent > Init.FarmMenu["qManaLane"].Cast<Slider>().CurrentValue)
            {
                var minions =
                    EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,
                        ObjectManager.Player.Position, Spells.Q.Range);
                var aiMinions = minions as Obj_AI_Minion[] ?? minions.ToArray();

                for (var index = 0; index < aiMinions.Length; index++)
                {
                    var m = aiMinions[index];
                    var p = new Geometry.Polygon.Rectangle((Vector2) ObjectManager.Player.ServerPosition,
                        ObjectManager.Player.ServerPosition.Extend(m.ServerPosition, Spells.Q1.Range), 65);
                    if (aiMinions.Count(x => p.IsInside(x.ServerPosition)) < 3) continue;
                    Spells.Q.Cast(m);
                    break;
                }
            }

            if (!Spells.W.IsReady() ||
                !Init.FarmMenu["useWfarm"].Cast<CheckBox>().CurrentValue ||
                ObjectManager.Player.ManaPercent < Init.FarmMenu["wManaLane"].Cast<Slider>().CurrentValue) return;
            {
                var minions =
                    EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,
                        ObjectManager.Player.Position, 500)
                        .FirstOrDefault(x => x.IsValidTarget(500));
                if (minions != null)
                    Spells.W.Cast(minions);
            }
        }

        public static void JungleClear()
        {
            if ((Init.FarmMenu["spellWeaving"].Cast<CheckBox>().CurrentValue &&
                Events.PassiveUp) ||
                Orbwalker.IsAutoAttacking ||
                ObjectManager.Player.IsDashing())
                return;

            if (Spells.Q.IsReady() &&
                Init.FarmMenu["useQjungle"].Cast<CheckBox>().CurrentValue &&
                ObjectManager.Player.ManaPercent > Init.FarmMenu["qManaJungle"].Cast<Slider>().CurrentValue)
            {
                var monsters =
                    EntityManager.MinionsAndMonsters.GetJungleMonsters(ObjectManager.Player.ServerPosition,
                        Spells.Q.Range)
                        .FirstOrDefault(x => x.IsValidTarget(Spells.Q.Range));
                if (monsters != null)
                    Spells.Q.Cast(monsters);

            }

            if (!Spells.W.IsReady() ||
                !Init.FarmMenu["useWjungle"].Cast<CheckBox>().CurrentValue ||
                ObjectManager.Player.ManaPercent < Init.FarmMenu["wManaJungle"].Cast<Slider>().CurrentValue) return;
            {
                var monsters =
                    EntityManager.MinionsAndMonsters.GetJungleMonsters(ObjectManager.Player.ServerPosition,500)
                        .FirstOrDefault(x => x.IsValidTarget(500));
                if (monsters != null)
                    Spells.W.Cast(monsters);
            }
        }
    }
}