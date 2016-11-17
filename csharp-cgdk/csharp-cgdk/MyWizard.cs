// ---------------------------------------------------------------------------------------------------------------------------------------------------
// Copyright ElcomPlus LLC. All rights reserved.
// Author: Нехорошев М. В.
// ---------------------------------------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Com.CodeGame.CodeWizards2016.DevKit.CSharpCgdk.Model;

namespace Com.CodeGame.CodeWizards2016.DevKit.CSharpCgdk
{
    public class MyWizard : Wizard
    {
        #region Constants

        /// <summary>
        /// минимальное значение регенерации на нулевом уровне
        /// </summary>
        private const double MIN_LIVE_ENERGY_REGENERATION = 0.05;

        /// <summary>
        /// каждый уровень прирост регенерации увеличивается на это значение.
        /// </summary>
        private const double INCREASE_LIVE_ENERGY_REGENARATION_LEVEL = 0.005;

        private const double MIN_MANA_REGENERATION = 0.2;

        private const double INCREASE_MANA_REGENERATION_LEVEL = 0.02;

        private static double LOW_HP_FACTOR = 0.29D;

        #endregion Constants

        #region Fields

        private SkillsManager _skillsManager;
        private Game _game;
        private World _world;
        private Move _move;
        private MapRouter _routeBuilder;

        #endregion Fields

        #region Properties

        /// <summary>
        /// за каждый 1 тик значение жизненной энергии приростает на это значение
        /// </summary>
        public double LiveEnergyRegeneration
        {
            get { return MIN_LIVE_ENERGY_REGENERATION + (Level * INCREASE_LIVE_ENERGY_REGENARATION_LEVEL); }
        }

        /// <summary>
        /// /// за каждый 1 тик количество маны приростает на это значение
        /// </summary>
        public double ManaRegeneration
        {
            get { return MIN_MANA_REGENERATION + (Level * INCREASE_MANA_REGENERATION_LEVEL); }
        }
        
        #endregion Properties

        #region Constructors

        public MyWizard(Wizard wizard, Game g, World world, Move move) :
            base(
                wizard.Id,
                wizard.X,
                wizard.Y,
                wizard.SpeedX,
                wizard.SpeedY,
                wizard.Angle,
                wizard.Faction,
                wizard.Radius,
                wizard.Life,
                wizard.MaxLife,
                wizard.Statuses,
                wizard.OwnerPlayerId,
                wizard.IsMe,
                wizard.Mana,
                wizard.MaxMana,
                wizard.VisionRange,
                wizard.CastRange,
                wizard.Xp,
                wizard.Level,
                wizard.Skills,
                wizard.RemainingActionCooldownTicks,
                wizard.RemainingCooldownTicksByAction,
                wizard.IsMaster, 
                wizard.Messages)
        {
            _skillsManager = new SkillsManager();
            _game = g;
            _world = world;
            _move = move;
            _routeBuilder = new MapRouter();
        }

        #endregion Constructors

        #region Methods

        public void GetNewSkillIfICan()
        {
            if (CanGetNewSkill)
            {
                _move.SkillToLearn = _skillsManager.GetNextSkillTypeToLearn(Skills);
            }
        }

        public void TryAttackEnemy()
        {
            LivingUnit nearestTarget = getNearestTarget();

            // Если видим противника ...
            if (nearestTarget != null)
            {
                double distance = GetDistanceTo(nearestTarget);

                // ... и он в пределах досягаемости наших заклинаний, ...
                if (distance <= CastRange)
                {
                    double angle = GetAngleTo(nearestTarget);

                    // ... то поворачиваемся к цели.
                    _move.Turn = angle;

                    // Если цель перед нами, ...
                    if (Math.Abs(angle) < _game.StaffSector / 2.0D)
                    {
                        // ... то атакуем.
                        _move.Action = ChoseBestAttackMethod(); ;
                        _move.CastAngle = angle;
                        _move.MinCastDistance = distance - nearestTarget.Radius + _game.MagicMissileRadius;
                    }

                    return;
                }
            }
        }

        public bool IsLowHealth()
        {
            return Life < MaxLife*LOW_HP_FACTOR;
        }

        public ActionType? ChoseBestAttackMethod()
        {
            
            if (Skills.Contains(SkillType.Fireball))
            {
                int remainingFrostBoltTicks = RemainingCooldownTicksByAction[(int)ActionType.Fireball];
                bool enoghMana = _game.FireballManacost < Mana;
                if (remainingFrostBoltTicks == 0 && enoghMana)
                {
                    return ActionType.Fireball;
                }
            }
            if (Skills.Contains(SkillType.FrostBolt))
            {
                int remainingFrostBoltTicks = RemainingCooldownTicksByAction[(int)ActionType.FrostBolt];
                bool enoghMana = _game.FrostBoltManacost < Mana;
                if (remainingFrostBoltTicks == 0 && enoghMana)
                {
                    return ActionType.FrostBolt;
                }
            }
            return ActionType.MagicMissile;
        }

        private void goTo(Point2D point)
        {
            double angle = GetAngleTo(point.X, point.Y);

            _move.Turn = angle;

            if (Math.Abs(angle) < _game.StaffSector / 4.0D)
            {
                _move.Speed = _game.WizardForwardSpeed;
            }
        }

        private LivingUnit getNearestTarget()
        {
            List<LivingUnit> targets = new List<LivingUnit>();
            targets.AddRange(_world.Buildings);
            targets.AddRange(_world.Wizards);
            targets.AddRange(_world.Minions);

            LivingUnit nearestTarget = null;
            double nearestTargetDistance = Double.MaxValue;

            foreach (LivingUnit target in targets)
            {
                if (target.Faction == Faction.Neutral || target.Faction == Faction)
                {
                    continue;
                }

                double distance = GetDistanceTo(target);

                if (distance < nearestTargetDistance)
                {
                    nearestTarget = target;
                    nearestTargetDistance = distance;
                }
            }

            return nearestTarget;
        }

        private bool CanGetNewSkill
        {
            get { return Skills.Length < Level; }
        }

        #endregion Methods

        public void RunAwayAndAttack()
        {
            Point2D wizardLocation = GetLocation();
            _routeBuilder.GetNextPointToGo(wizardLocation)
            throw new NotImplementedException();
        }

        private Point2D GetLocation()
        {
            double distanseToLeftUpperCorner = GetDistanceTo(0, 0);
            double distanseToLeftDownCorner = GetDistanceTo(0, _game.MapSize);
            
        }

        public void GoToEnemyBase()
        {
            throw new NotImplementedException();
        }
    }
}