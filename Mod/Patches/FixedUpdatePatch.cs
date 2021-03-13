using EnemySense.Utils;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace EnemySense.Patches {

	[HarmonyPatch(typeof(Player), "FixedUpdate")]
	public static class FixedUpdatePatch {

		public static readonly float staminaDrain = 70F;

		public static void Postfix(ref Player __instance) {
			if(__instance == Player.m_localPlayer && __instance.TakeInput() && __instance.IsCrouching() && ZInput.GetButtonDown("ToggleWalk")) {
				
				//First we check if thep layer has enough Stamina to use the ping, if not we don't do it.
				if(__instance.HaveStamina(staminaDrain)) {
					__instance.UseStamina(staminaDrain);
				} else {
					Hud.instance.StaminaBarNoStaminaFlash();
					return;
				}

				//Create the ping effect.
				Transform visualObject = UnityEngine.Object.Instantiate<Transform>(PrefabGetter.getPingVisual(), __instance.GetHeadPoint(), Quaternion.identity);
				GameObject audioObject = UnityEngine.Object.Instantiate<GameObject>(PrefabGetter.getPingAudio(), __instance.GetHeadPoint(), Quaternion.identity);

				Debug.Log("Player location: " + __instance.transform.position.ToString());

				//Do the Get Creaturss.
				List<Character> guysList = Character.GetAllCharacters();

				bool flag = false;
				EnemyHud.instance.m_refPoint = __instance.transform.position;

				//Loop through the characters.
				foreach(Character character in guysList) {
					if(character != __instance && EnemyHud.instance.TestShow(character)) {
						flag = true;
						EnemyHud.instance.ShowHud(character);
						EnemyHud.instance.m_huds.TryGetValue(character, out EnemyHud.HudData hud);
						if(hud != null) {
							hud.m_hoverTimer = 0F;
							hud.m_gui.SetActive(true);
						}
					}
				}

				//Update the enemy huds list if we found anything
				if(flag) {
					EnemyHud.instance.UpdateHuds(__instance, Time.deltaTime);
				}
			}
		}
	}
}
