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
        private SkillType? _currentSkillType;

        #endregion Fields

        public int AvailableSkillsNumber
        {
            get
            {
                if (_availableSkillTypes == null)
                {
                    return 0;
                }
                if (_currentSkillType == null)
                {
                    return _availableSkillTypes.Length;
                }

                return _availableSkillTypes.Length - GetCurrentSkillIndex();
            }
        }



        #region Constructor

        public SkillGroup(SkillType[] availableSkillTypes)
        {
            _availableSkillTypes = availableSkillTypes;
        }

        #endregion

        #region Methods

        public SkillType? GetNextAvailableSkill()
        {
            if (_availableSkillTypes == null)
            {
                return null;
            }

            if (_currentSkillType == null)
            {
                _currentSkillType = _availableSkillTypes[0];
            }
            else
            {
                int index = GetCurrentSkillIndex();
                if (index < 0)
                {
                    return null;
                }

                if (index + 1 < _availableSkillTypes.Length)
                {
                    _currentSkillType = _availableSkillTypes[index + 1];
                }
            }

            return _currentSkillType;
        }

        public int GetCurrentSkillIndex()
        {
            int result = -1;

            if (_availableSkillTypes == null || _currentSkillType == null)
                return result;
            
            for (int i = 0; i < _availableSkillTypes.Length; i++)
            {
                if (_availableSkillTypes[i] == _currentSkillType.Value)
                {
                    return i;
                }
            }

            return result;

        }

        #endregion

    }
}