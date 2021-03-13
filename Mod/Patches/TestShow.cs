using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnemySense.Patches {

	[HarmonyPatch(typeof(EnemyHud), "TestShow")]
	public static class TestShowPatch {

		//Temporary value that gets set properly as soon as we need it.
		private static float baseDistance = -0.1F;

		//This is so we use the game's default m_maxShowDistance value.
		public static float getBaseDistance() {
			if(baseDistance < 0F) {
				baseDistance = EnemyHud.instance.m_maxShowDistance;
			}
			return baseDistance;
		}

		public static void Prefix(ref Player __instance) {

			//Gets the player's sneak skill level and adds it to the default max distance you can be away from creatures to be able to view their health bar.
			if(Player.m_localPlayer != null && Player.m_localPlayer.GetSkills().m_skillData.ContainsKey(Skills.SkillType.Sneak)) {

				Player.m_localPlayer.GetSkills().m_skillData.TryGetValue(Skills.SkillType.Sneak, out Skills.Skill value);
				if(value != null) {
					//The game's base value is 30f, this mod makes it scale up to 60f at max sneak skill.
					EnemyHud.instance.m_maxShowDistance = (1F + value.m_level * 0.01F) * getBaseDistance();
				}

			//If the player has no sneak skill level just set it back to 10F which is the default.
			} else {
				EnemyHud.instance.m_maxShowDistance = getBaseDistance();
			}
		}
	}
}
