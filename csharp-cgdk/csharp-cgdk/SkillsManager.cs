﻿// ---------------------------------------------------------------------------------------------------------------------------------------------------
// Copyright ElcomPlus LLC. All rights reserved.
// Author: Нехорошев М. В.
// ---------------------------------------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using Com.CodeGame.CodeWizards2016.DevKit.CSharpCgdk.Model;

namespace Com.CodeGame.CodeWizards2016.DevKit.CSharpCgdk
{
    public class SkillsManager
    {
        #region Fields
        private List<SkillGroup> _skillGroups;
        #endregion

        #region Constructors

        public SkillsManager()
        {
            CreateSkillGroups();
        }

        #endregion
        
        #region Methods


        private void CreateSkillGroups()
        {
            _skillGroups = new List<SkillGroup>();
            _skillGroups.Add(
                new SkillGroup(new SkillType[]
                    {
                        SkillType.RangeBonusPassive1,
                        SkillType.RangeBonusAura1,
                        SkillType.RangeBonusPassive2,
                        SkillType.RangeBonusAura2,
                        SkillType.AdvancedMagicMissile
                    }, 2));

            _skillGroups.Add(
                    new SkillGroup(new SkillType[]
                    {
                        SkillType.MagicalDamageBonusPassive1,
                        SkillType.MagicalDamageBonusAura1,
                        SkillType.MagicalDamageBonusPassive2,
                        SkillType.MagicalDamageBonusAura2,
                        SkillType.FrostBolt
                    }, 3));
            _skillGroups.Add(
                new SkillGroup(
                    new SkillType[]
                    {
                        SkillType.StaffDamageBonusPassive1,
                        SkillType.StaffDamageBonusAura1,
                        SkillType.StaffDamageBonusPassive2,
                        SkillType.StaffDamageBonusAura2,
                        SkillType.Fireball
                    }, 4));
            _skillGroups.Add(
                new SkillGroup(
                    new SkillType[]
                    {
                        SkillType.MovementBonusFactorPassive1,
                        SkillType.MovementBonusFactorAura1,
                        SkillType.MovementBonusFactorPassive2,
                        SkillType.MovementBonusFactorAura2,
                        SkillType.Haste
                    }, 5));
            _skillGroups.Add(
                new SkillGroup(
                    new SkillType[]
                    {
                        SkillType.MagicalDamageAbsorptionPassive1,
                        SkillType.MagicalDamageAbsorptionAura1,
                        SkillType.MagicalDamageAbsorptionPassive2,
                        SkillType.MagicalDamageAbsorptionAura2,
                        SkillType.Shield
                    }, 1));
            _skillGroups = _skillGroups.OrderBy(o => o.Priority).ToList();

        }

        public SkillType? GetNextSkillTypeToLearn(SkillType[] wizardSkills)
        {
            for (int i = 0; i < _skillGroups.Count; i++)
            {
                SkillType? result = _skillGroups[i].GetNextAvailableSkill(wizardSkills);
                if (result.HasValue)
                {
                    return result;
                }
            }
            return null;
        }

        #endregion
    }
}