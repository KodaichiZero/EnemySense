using BepInEx.Configuration;
using CreatureSense.Utils;
using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CreatureSense.Patches {

	[HarmonyPatch(typeof(Player), "FixedUpdate")]
	public static class FixedUpdatePatch {

		public static ConfigEntry<float> staminaDrain;
		public static ConfigEntry<bool> showMessage;
		public static ConfigEntry<bool> showVisual;
		public static ConfigEntry<bool> playAudio;
		public static ConfigEntry<string> keyBind;
		public static bool invalidKey = false;
		public static ConfigEntry<string> customMessage;
		public static ConfigEntry<string> messageColor;

		public static void Postfix(ref Player __instance) {
			if(__instance == Player.m_localPlayer && __instance.TakeInput()) {

				bool keyPressed = false;

				//We check to see if the custom hotkey is pressed, or if the walk key is pressed while crouched.
				if(keyBind.Value.Length > 0 && !invalidKey) {
					try {
						if(Input.GetKeyDown(keyBind.Value.ToLower())) {
							keyPressed = true;
						}
					} catch(Exception e) {

						//We only warn about an invalid keybind once. Then we revert to the walk key while crouching.
						if(!invalidKey) {
							invalidKey = true;
							Debug.Log("CreatureSense ERROR: You bound an invalid key! Please check the config file and try again. You can consult a Unity guide for a list of valid key codes.");
						}
					}
				} else if(__instance.IsCrouching() && ZInput.GetButtonDown("ToggleWalk")) {
					keyPressed = true;
				}

				//We only proceed if the key was pressed.
				if(keyPressed) {

					//First we check if thep layer has enough Stamina to use the ping, if not we don't do it.
					if(__instance.HaveStamina(staminaDrain.Value)) {
						__instance.UseStamina(staminaDrain.Value);
					} else {
						Hud.instance.StaminaBarNoStaminaFlash();
						return;
					}

					//Create the ping effect.
					if(showVisual.Value) {
						Transform visualObject = UnityEngine.Object.Instantiate<Transform>(PrefabGetter.getPingVisual(), __instance.GetHeadPoint(), Quaternion.identity);
					}
					if(playAudio.Value) {
						GameObject audioObject = UnityEngine.Object.Instantiate<GameObject>(PrefabGetter.getPingAudio(), __instance.GetHeadPoint(), Quaternion.identity);
					}

					//Do the Get Creaturss.
					List<Character> guysList = Character.GetAllCharacters();
					EnemyHud.instance.m_refPoint = __instance.transform.position;
					int guysNum = 0;

					//Loop through the characters.
					foreach(Character character in guysList) {
						if(character is not Player && EnemyHud.instance.TestShow(character)) {
							guysNum++;
							EnemyHud.instance.ShowHud(character);
							EnemyHud.instance.m_huds.TryGetValue(character, out EnemyHud.HudData hud);
							if(hud != null) {
								hud.m_hoverTimer = 0F;
								hud.m_gui.SetActive(true);
							}
						}
					}

					//Update the enemy huds list if we found anything
					if(guysNum > 0) {
						EnemyHud.instance.UpdateHuds(__instance, Time.deltaTime);
					}

					//Show message about how many enemies are nearby.
					if(showMessage.Value) {
						if(customMessage.Value.Length > 0) {
							string newMessage = new string(customMessage.Value.Replace("#", guysNum.ToString()).ToCharArray());
							__instance.Message(MessageHud.MessageType.Center, "<color=" + messageColor.Value + ">" + newMessage + "</color>");
						} else {
							__instance.Message(MessageHud.MessageType.Center, "<color=" + messageColor.Value + ">" + guysNum + " creature" + (guysNum == 1 ? "" : "s") + " found nearby.</color>");
						}
					}
				}
			}
		}
	}
}
