// ---------------------------------------------------------------------------------------------------------------------------------------------------
// Copyright ElcomPlus LLC. All rights reserved.
// Author: Нехорошев М. В.
// ---------------------------------------------------------------------------------------------------------------------------------------------------

using Com.CodeGame.CodeWizards2016.DevKit.CSharpCgdk.Model;

namespace Com.CodeGame.CodeWizards2016.DevKit.CSharpCgdk
{
    public class SkillGroup
    {
        #region Fields
        private SkillType[] _availableSkillTypes;

        #endregion Fields

        #region Properties

        public int Priority { get; private set; }

        #endregion Properties
        
        #region Constructor

        public SkillGroup(SkillType[] availableSkillTypes, int priority)
        {
            _availableSkillTypes = availableSkillTypes;
            Priority = priority;
        }

        #endregion

        #region Methods

        public SkillType? GetNextAvailableSkill(SkillType[] skillsOfWizard)
        {
            if (_availableSkillTypes == null)
            {
                return null;
            }
            else
            {
                for (int i = 0; i < _availableSkillTypes.Length; i++)
                {
                    if (!ExistInWizardSkills(_availableSkillTypes[i], skillsOfWizard))
                    {
                        return _availableSkillTypes[i];
                    }
                }
            }

            return null;
        }

        private bool ExistInWizardSkills(SkillType typeToFind, SkillType[] skillsOfWizard)
        {
            for (int i = 0; i < skillsOfWizard.Length; i++)
            {
                if (skillsOfWizard[i] == typeToFind)
                {
                    return true;
                }
            }
            return false;
        }


        #endregion

    }
}