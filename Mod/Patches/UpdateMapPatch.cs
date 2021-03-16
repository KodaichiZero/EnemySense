using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatureSense.Patches {

	[HarmonyPatch(typeof(Minimap), "UpdateMap")]
	public static class UpdateMapPatch {

		//Values loaded via the config file.
		public static ConfigEntry<bool> allowAdditionalZoom;

		public static void Prefix(Minimap __instance, Player player, float dt, bool takeInput) {

			//Allow additional map zoom if set in the config.
			if(allowAdditionalZoom.Value) {
				__instance.m_minZoom = 0.00375F;
			}
		}
	}
}
