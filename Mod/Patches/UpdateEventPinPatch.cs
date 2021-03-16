using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CreatureSense.Patches {

	[HarmonyPatch(typeof(Minimap), "UpdateEventPin")]
	public static class UpdateEventPinPatch {

		public static ConfigEntry<bool> showMinimapIcons;

		public static void Postfix(Minimap __instance) {

			//Skip this entirely if disabled in the config.
			if(showMinimapIcons.Value) {

				//Populate the list of current HUD characters.
				List<Character> guysList = new List<Character>();
				foreach(EnemyHud.HudData hud in EnemyHud.instance.m_huds.Values) {
					if(hud.m_character != null && hud.m_hoverTimer < EnemyHud.instance.m_hoverShowDuration) {
						guysList.Add(hud.m_character);
					}
				}

				//Add minimap pins if they haven't been added already.
				foreach(Character character in guysList) {
					if(character is Player) {
						continue;
					}
					
					bool flag = false;

					foreach(Minimap.PinData pin in __instance.m_pins) {
						if(pin.m_name.Equals("CS__" + character.GetZDOID().ToString())) {
							flag = true;
							break;
						}
					}

					if(!flag) {
						__instance.AddPin(character.GetCenterPoint(), Minimap.PinType.RandomEvent, "CS__" + character.GetZDOID().ToString(), false, false);
					}
				}

				//Remove minimap pins which are not needed anymore.
				List<Minimap.PinData> removePins = new List<Minimap.PinData>();

				foreach(Minimap.PinData pin in __instance.m_pins) {
					if(pin.m_type == Minimap.PinType.RandomEvent && pin.m_name.Contains("CS__")) {
						bool flag = false;
						foreach(Character character in guysList) {
							if(pin.m_name.Equals("CS__" + character.GetZDOID().ToString())) {
								pin.m_pos.x = character.GetCenterPoint().x;
								pin.m_pos.y = character.GetCenterPoint().y;
								pin.m_pos.z = character.GetCenterPoint().z;
								flag = true;
								break;
							}
						}

						if(!flag) {
							removePins.Add(pin);
						}
					}
				}

				foreach(Minimap.PinData pin in removePins) {
					__instance.RemovePin(pin);
					Debug.Log("removing pin for " + pin.m_name);
				}
			}
		}
	}
}
