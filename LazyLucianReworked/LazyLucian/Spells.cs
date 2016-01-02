using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using SharpDX;

namespace LazyLucian
{
    internal class Spells
    {
        //-------------------------------------------------------------------------------------------------------------------
        /*
        *       _____            _ _   _____            _                 _   _             
        *      / ____|          | | | |  __ \          | |               | | (_)            
        *     | (___  _ __   ___| | | | |  | | ___  ___| | __ _ _ __ __ _| |_ _  ___  _ __  
        *      \___ \| '_ \ / _ \ | | | |  | |/ _ \/ __| |/ _` | '__/ _` | __| |/ _ \| '_ \ 
        *      ____) | |_) |  __/ | | | |__| |  __/ (__| | (_| | | | (_| | |_| | (_) | | | |
        *     |_____/| .__/ \___|_|_| |_____/ \___|\___|_|\__,_|_|  \__,_|\__|_|\___/|_| |_|
        *            | |                                                                    
        *            |_|                                                                    
        */

        public static Spell.Targeted Q = new Spell.Targeted(SpellSlot.Q, 675);
        public static Spell.Skillshot W = new Spell.Skillshot(SpellSlot.W, 1200, SkillShotType.Linear, 300, 1600, 80);
        public static Spell.Skillshot W1 = new Spell.Skillshot(SpellSlot.W, 500, SkillShotType.Linear, 300, 1600, 80);
        public static Spell.Skillshot E = new Spell.Skillshot(SpellSlot.E, 475, SkillShotType.Linear);
        public static Spell.Skillshot R = new Spell.Skillshot(SpellSlot.R, 1400, SkillShotType.Linear, 500, 2800, 110);

        public static Spell.Skillshot Q1 = new Spell.Skillshot(SpellSlot.Q, 1100, SkillShotType.Linear, 400,
            int.MaxValue, 65);

        //-------------------------------------------------------------------------------------------------------------------
        /*
        *       ____    _                 _      
        *      / __ \  | |               (_)     
        *     | |  | | | |     ___   __ _ _  ___ 
        *     | |  | | | |    / _ \ / _` | |/ __|
        *     | |__| | | |___| (_) | (_| | | (__ 
        *      \___\_\ |______\___/ \__, |_|\___|
        *                            __/ |       
        *                           |___/        
        */

        public static void CastQ()
        {
            var target = TargetSelector.SelectedTarget != null &&
                         TargetSelector.SelectedTarget.Distance(ObjectManager.Player) < 20000
                ? TargetSelector.SelectedTarget
                : TargetSelector.GetTarget(800, DamageType.Physical);

            if (!target.IsValidTarget(Q.Range))
                return;
            {
                Q.Cast(target);
            }
        }

        public static void Ks()
        {
            if (ObjectManager.Player.IsDashing())
            {
                return;
            }
            if (Q.IsReady())
            {
                var target = (TargetSelector.GetTarget(Q.Range, DamageType.Physical) ??
                              TargetSelector.GetTarget(Q1.Range, DamageType.Physical));
                if (target != null && (ObjectManager.Player.GetSpellDamage(target, SpellSlot.Q) >= target.Health + 20))
                {
                    if (target.IsValidTarget(Q.Range))
                    {
                        Q.Cast(target);
                    }
                    else if (target.IsValidTarget(Q1.Range))
                    {
                        CastExtendedQ();
                        if (Helpers.IsSafePosition(target.ServerPosition) && E.IsReady())
                        {
                            DashToExtendedQ();
                        }
                    }
                }
            }
            if (!W.IsReady()) return;
            {
                var target = (TargetSelector.GetTarget(W.Range, DamageType.Magical));
                if (target != null && (ObjectManager.Player.GetSpellDamage(target, SpellSlot.W) >= target.Health + 20))
                {
                    CastWcombo();
                }
            }
        }

        public static void CastExtendedQ()
        {
            var target = TargetSelector.SelectedTarget != null &&
                         TargetSelector.SelectedTarget.Distance(ObjectManager.Player) < 20000
                ? TargetSelector.SelectedTarget
                : TargetSelector.GetTarget(1800, DamageType.Physical);

            var predPos = Q1.GetPrediction(target);
            var minions = EntityManager.MinionsAndMonsters.EnemyMinions;
            var champs = EntityManager.Heroes.Enemies;
            var monsters = EntityManager.MinionsAndMonsters.Monsters;

            if (!target.IsValidTarget(Q1.Range))
                return;

            {
                foreach (var minion in from minion in minions
                    let polygon = new Geometry.Polygon.Rectangle(
                        (Vector2) ObjectManager.Player.ServerPosition,
                        ObjectManager.Player.ServerPosition.Extend(minion.ServerPosition, Q1.Range), 65f)
                    where polygon.IsInside(predPos.CastPosition)
                    select minion)
                {
                    Q.Cast(minion);
                }

                foreach (var champ in from champ in champs
                    let polygon = new Geometry.Polygon.Rectangle(
                        (Vector2) ObjectManager.Player.ServerPosition,
                        ObjectManager.Player.ServerPosition.Extend(champ.ServerPosition, Q1.Range), 65f)
                    where polygon.IsInside(predPos.CastPosition)
                    select champ)
                {
                    Q.Cast(champ);
                }

                foreach (var monster in from monster in monsters
                    let polygon = new Geometry.Polygon.Rectangle(
                        (Vector2) ObjectManager.Player.ServerPosition,
                        ObjectManager.Player.ServerPosition.Extend(monster.ServerPosition, Q1.Range), 65f)
                    where polygon.IsInside(predPos.CastPosition)
                    select monster)
                {
                    Q.Cast(monster);
                }
            }
        }

        //-------------------------------------------------------------------------------------------------------------------
        /*
        *     __          __  _                 _      
        *     \ \        / / | |               (_)     
        *      \ \  /\  / /  | |     ___   __ _ _  ___ 
        *       \ \/  \/ /   | |    / _ \ / _` | |/ __|
        *        \  /\  /    | |___| (_) | (_| | | (__ 
        *         \/  \/     |______\___/ \__, |_|\___|
        *                                  __/ |       
        *                                 |___/        
        */

        public static void CastWinRange()
        {
            var target = TargetSelector.GetTarget(600, DamageType.Magical);

            if (!target.IsValidTarget(500) ||
                (W1.GetPrediction(target).HitChance == HitChance.Collision) ||
                (W1.GetPrediction(target).HitChance < HitChance.Medium))
                return;
            {
                W.Cast(target);
            }
        }

        public static void CastWcombo()
        {
            var target = TargetSelector.GetTarget(1500, DamageType.Magical);

            if (!target.IsValidTarget(W.Range) ||
                (W.GetPrediction(target).HitChance == HitChance.Collision) ||
                (W.GetPrediction(target).HitChance < HitChance.Medium))
                return;
            {
                W.Cast(target);
            }
        }

        //-------------------------------------------------------------------------------------------------------------------
        /*
        *      ______   _                 _      
        *     |  ____| | |               (_)     
        *     | |__    | |     ___   __ _ _  ___ 
        *     |  __|   | |    / _ \ / _` | |/ __|
        *     | |____  | |___| (_) | (_| | | (__ 
        *     |______| |______\___/ \__, |_|\___|
        *                            __/ |       
        *                           |___/        
        */

        public static void CastEcombo()
        {
            var target = TargetSelector.GetTarget(2000, DamageType.Physical);
            var direction1 = (ObjectManager.Player.ServerPosition - target.ServerPosition).To2D().Normalized();
            var direction2 = (target.ServerPosition - ObjectManager.Player.ServerPosition).To2D().Normalized();
            const int maxDistance = 475;
            const int stepSize = 20;

            if (target.HealthPercent <= ObjectManager.Player.HealthPercent &&
                (ObjectManager.Player.HealthPercent > 30 ||
                 Helpers.GetComboDamage(target) >= target.Health) &&
                target.Distance(ObjectManager.Player) <= 975)
            {
                for (var step = 0f; step < 360; step += stepSize)
                {
                    var currentAngle = step*(float) Math.PI/360;
                    var currentCheckPoint = target.ServerPosition.To2D() +
                                            maxDistance*direction2.Rotated(currentAngle);

                    if (!Helpers.IsSafePosition((Vector3) currentCheckPoint) ||
                        currentCheckPoint.ToNavMeshCell().CollFlags.HasFlag(CollisionFlags.Wall)) continue;
                    {
                        E.Cast((Vector3) currentCheckPoint);
                    }
                }
            }

            else if (target.HealthPercent > ObjectManager.Player.HealthPercent &&
                     target.Distance(ObjectManager.Player) <= 400)
            {
                for (var step = 0f; step < 360; step += stepSize)
                {
                    var currentAngle = step*(float) Math.PI/180;
                    var currentCheckPoint = target.ServerPosition.To2D() +
                                            maxDistance*direction1.Rotated(currentAngle);

                    if (!Helpers.IsSafePosition((Vector3) currentCheckPoint) ||
                        currentCheckPoint.ToNavMeshCell().CollFlags.HasFlag(CollisionFlags.Wall))
                        continue;
                    {
                        E.Cast((Vector3) currentCheckPoint);
                    }
                }
            }
        }

        public static void DashToExtendedQ()
        {
            var target = TargetSelector.GetTarget(E.Range + Q1.Range, DamageType.Physical);
            var dashSpeed = (int) (E.Range/(700 + ObjectManager.Player.MoveSpeed));
            var targetPrediction = Helpers.EqExtendedPrediction(target, dashSpeed).To2D();

            var minions = EntityManager.MinionsAndMonsters.EnemyMinions
                .Where(minion => minion.Distance(targetPrediction, true) < 900*900)
                .OrderByDescending(min => min.Distance(targetPrediction, true));
            var monsters = EntityManager.MinionsAndMonsters.Monsters
                .Where(monster => monster.Distance(targetPrediction, true) < 900*900)
                .OrderByDescending(min => min.Distance(targetPrediction, true));
            var champs = EntityManager.MinionsAndMonsters.EnemyMinions
                .Where(champ => champ.Distance(targetPrediction, true) < 900*900)
                .OrderByDescending(min => min.Distance(targetPrediction, true));

            if (target.IsValidTarget(Q.Range)) return;

            foreach (
                var bestChamp in
                    champs.Select(champ => Prediction.Position.PredictUnitPosition(champ, dashSpeed))
                        .Select(minionPrediction => Helpers.GetCircleLineInteraction(
                            minionPrediction.To3D(),
                            targetPrediction,
                            ObjectManager.Player.ServerPosition.To2D(),
                            E.Range)).Select(inter => inter.GetBestInter(target)))

                foreach (
                    var bestMinion in
                        minions.Select(minion => Prediction.Position.PredictUnitPosition(minion, dashSpeed))
                            .Select(minionPrediction => Helpers.GetCircleLineInteraction(
                                minionPrediction.To3D(),
                                targetPrediction,
                                ObjectManager.Player.ServerPosition.To2D(),
                                E.Range)).Select(inter => inter.GetBestInter(target)))
                    foreach (
                        var bestMonster in
                            monsters.Select(monster => Prediction.Position.PredictUnitPosition(monster, dashSpeed))
                                .Select(monsterPrediction => Helpers.GetCircleLineInteraction(
                                    monsterPrediction.To3D(),
                                    targetPrediction,
                                    ObjectManager.Player.ServerPosition.To2D(),
                                    E.Range)).Select(inter => inter.GetBestInter(target)))


                    {
                        if (Math.Abs(bestMinion.X) < 1 && Math.Abs(bestChamp.X) < 1 && Math.Abs(bestMonster.X) < 1)
                        {
                            return;
                        }

                        if (!bestChamp.ToNavMeshCell().CollFlags.HasFlag(CollisionFlags.Wall) &&
                            !bestChamp.ToNavMeshCell().CollFlags.HasFlag(CollisionFlags.Building) &&
                            Helpers.IsSafePosition((Vector3) bestChamp))
                        {
                            E.Cast((Vector3) bestChamp);
                        }
                        else if (!bestMinion.ToNavMeshCell().CollFlags.HasFlag(CollisionFlags.Wall) &&
                            !bestMinion.ToNavMeshCell().CollFlags.HasFlag(CollisionFlags.Building) &&
                                 Helpers.IsSafePosition((Vector3) bestMinion))
                        {
                            E.Cast((Vector3) bestMinion);
                        }
                        else if (!bestMonster.ToNavMeshCell().CollFlags.HasFlag(CollisionFlags.Wall) &&
                            !bestMonster.ToNavMeshCell().CollFlags.HasFlag(CollisionFlags.Building) &&
                                 Helpers.IsSafePosition((Vector3) bestMonster))
                        {
                            E.Cast((Vector3) bestMonster);
                        }
                    }
        }

        //-------------------------------------------------------------------------------------------------------------------
        /*
        *      _____    _                 _      
        *     |  __ \  | |               (_)     
        *     | |__) | | |     ___   __ _ _  ___ 
        *     |  _  /  | |    / _ \ / _` | |/ __|
        *     | | \ \  | |___| (_) | (_| | | (__ 
        *     |_|  \_\ |______\___/ \__, |_|\___|
        *                            __/ |       
        *                           |___/        
        */


        public static void CastR(AIHeroClient unit)
        {
            var col = R.GetPrediction(unit);
            var allies = EntityManager.Heroes.Allies.Count(
              allied => !allied.IsDead && allied.Distance(unit) <= 500);
            var rDmg = ObjectManager.Player.GetSpellDamage(unit, SpellSlot.R)*Helpers.NumShots();
            var tDis = ObjectManager.Player.Distance(unit.ServerPosition);

            if (((unit.Distance(ObjectManager.Player.ServerPosition) < 500) &&
                 ObjectManager.Player.HealthPercent > unit.HealthPercent) ||
                col.HitChance == HitChance.Collision ||
                !unit.IsValidTarget() ||
                unit.HasBuffOfType(BuffType.Invulnerability) ||
                allies > 1) return;

            if (!(rDmg/1.4 > unit.Health) || !(tDis < 1200)) return;
            {
                R.Cast(unit);
            }
            {
                if (unit.Distance(ObjectManager.Player.ServerPosition) <= 500 &&
                    unit.Health <= (ObjectManager.Player.GetSpellDamage(unit, SpellSlot.Q) + Helpers.AaDmg(unit)))
                {
                    R.Cast(unit);
                }
            }
        }

        public static void LockR() //credits Brian(L$)
        {
            var target = TargetSelector.GetTarget(R.Range, DamageType.Physical);
            if (target == null)
            {
                return;
            }
            var endPos = (ObjectManager.Player.ServerPosition - target.ServerPosition).Normalized();
            var predPos = R.GetPrediction(target).CastPosition.To2D();
            var fullPoint = new Vector2(predPos.X + endPos.X*R.Range*0.98f, predPos.Y + endPos.Y*R.Range*0.98f);
            var closestPoint = ObjectManager.Player.ServerPosition.To2D()
                .Closest(new List<Vector2> {predPos, fullPoint});

            if (closestPoint.IsValid() &&
                !closestPoint.ToNavMeshCell().CollFlags.HasFlag(CollisionFlags.Wall) &&
                !closestPoint.ToNavMeshCell().CollFlags.HasFlag(CollisionFlags.Building) &&
                predPos.Distance(closestPoint) > E.Range)
            {
                Orbwalker.MoveTo(closestPoint.To3D());
            }
            else if (fullPoint.IsValid() &&
                     !fullPoint.ToNavMeshCell().CollFlags.HasFlag(CollisionFlags.Wall) &&
                     !fullPoint.ToNavMeshCell().CollFlags.HasFlag(CollisionFlags.Building) &&
                     predPos.Distance(fullPoint) < R.Range &&
                     predPos.Distance(fullPoint) > 100)
            {
                Orbwalker.MoveTo(fullPoint.To3D());
            }
        }
    }
}