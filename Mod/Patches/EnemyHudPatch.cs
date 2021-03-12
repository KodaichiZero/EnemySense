using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnemySense.Patches {

	[HarmonyPatch(typeof(EnemyHud), "TestShow")]
	public static class EnemyHudPatch {

		public static void Prefix(ref Player __instance) {

			//Gets the player's sneak skill level and adds it to the default max distance you can be away from creatures to be able to view their health bar.
			if(Player.m_localPlayer != null && Player.m_localPlayer.GetSkills().m_skillData.ContainsKey(Skills.SkillType.Sneak)) {
				Skills.Skill value;
				Player.m_localPlayer.GetSkills().m_skillData.TryGetValue(Skills.SkillType.Sneak, out value);
				if(value != null) {
					EnemyHud.instance.m_maxShowDistance = 10F + value.m_level * 0.1F;
				}
				
			}
			
			//If the player has no sneak skill level just set it back to 10F which is the default.
			EnemyHud.instance.m_maxShowDistance = 10F;
		}
	}
}
