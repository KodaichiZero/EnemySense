using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatureSense.Patches {

	[HarmonyPatch(typeof(EnemyHud), "TestShow")]
	public static class TestShowPatch {

		//Values loaded via the config file.
		public static ConfigEntry<float> baseRange;
		public static ConfigEntry<float> skillMultiplier;

		public static void Prefix(Player __instance) {

			//Sets the enemy detection range to the game's default, multiplied by the config value.
			EnemyHud.instance.m_maxShowDistance = baseRange.Value;

			//Gets the player's sneak skill level and adds it to the default max distance you can be away from creatures to be able to view their health bar.
			if(Player.m_localPlayer != null && Player.m_localPlayer.GetSkills().m_skillData.ContainsKey(Skills.SkillType.Sneak)) {

				Player.m_localPlayer.GetSkills().m_skillData.TryGetValue(Skills.SkillType.Sneak, out Skills.Skill value);
				if(value != null) {
					//The game's base value is 30f, this mod makes it scale up to 60f at max sneak skill.
					EnemyHud.instance.m_maxShowDistance += (value.m_level * 0.01F) * 30F * skillMultiplier.Value;
				}
			}
		}
	}
}
