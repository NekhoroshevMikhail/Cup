// ---------------------------------------------------------------------------------------------------------------------------------------------------
// Copyright ElcomPlus LLC. All rights reserved.
// Author: Нехорошев М. В.
// ---------------------------------------------------------------------------------------------------------------------------------------------------
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

        #endregion Constants

        #region Fields

        private SkillsManager _skillsManager;
        private Game _game;

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

        public MyWizard(Wizard w, Game g) :
            base(
                w.Id,
                w.X,
                w.Y,
                w.SpeedX,
                w.SpeedY,
                w.Angle,
                w.Faction,
                w.Radius,
                w.Life,
                w.MaxLife,
                w.Statuses,
                w.OwnerPlayerId,
                w.IsMe,
                w.Mana,
                w.MaxMana,
                w.VisionRange,
                w.CastRange,
                w.Xp,
                w.Level,
                w.Skills,
                w.RemainingActionCooldownTicks,
                w.RemainingCooldownTicksByAction,
                w.IsMaster, w.Messages)
        {
            _skillsManager = new SkillsManager();
            _game = g;
        }

        #endregion Constructors

        #region Methods


        #endregion Methods

        public void GetNewSkillIfICan(Move move)
        {
            if (CanGetNewSkill)
            {
                move.SkillToLearn = _skillsManager.GetNextSkillTypeToLearn(Skills);
            }
        }

        private bool CanGetNewSkill
        {
            get { return Skills.Length < Level; }
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

    }
}