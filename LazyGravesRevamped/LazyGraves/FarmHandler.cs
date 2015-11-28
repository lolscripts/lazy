﻿using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using SharpDX;

namespace LazyGraves
{
    internal class FarmHandler
    {
        public static AIHeroClient Player = ObjectManager.Player;

        public static void LaneClear()
        {
            if (!Init.FarmMenu["useQlane"].Cast<CheckBox>().CurrentValue ||
                Player.ManaPercent < Init.FarmMenu["qManaLane"].Cast<Slider>().CurrentValue) return;
            var minions =
                EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,
                    ObjectManager.Player.Position, Spells.Q.Range);
            var aiMinions = minions as Obj_AI_Minion[] ?? minions.ToArray();

            foreach (var m in from m in aiMinions
                let p = new Geometry.Polygon.Rectangle((Vector2) ObjectManager.Player.ServerPosition,
                    ObjectManager.Player.ServerPosition.Extend(m.ServerPosition, Spells.Q.Range), Spells.Q.Width)
                where aiMinions.Count(x =>
                    p.IsInside(x.ServerPosition)) >= Init.FarmMenu["qMinionsLane"].Cast<Slider>().CurrentValue
                select m)
            {
                Spells.Q.Cast(m);
                break;
            }
        }

        public static void JungleClear()
        {
            if (!Spells.Q.IsReady() || !Init.FarmMenu["useQjungle"].Cast<CheckBox>().CurrentValue) return;
            var monster = EntityManager.MinionsAndMonsters.GetJungleMonsters(ObjectManager.Player.ServerPosition,
                Spells.Q.Range)
                .First(x => x.IsValidTarget(Spells.Q.Range));
            if (monster != null)
                Spells.Q.Cast(monster);
        }
    }
}