using System.Collections.Generic;

namespace Utils
{
    public static class Skills
    {
        public static readonly Dictionary<SkillButton, string> ButtonActions = new() {
            { SkillButton.JUMP, "jump" },
            { SkillButton.ATTACK, "attack" },
            { SkillButton.SKILL_ONE, "skill_one" },
            { SkillButton.SKILL_TWO, "skill_two" },
            { SkillButton.SKILL_THREE, "skill_three" },
        };
    }
}