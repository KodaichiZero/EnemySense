using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnemySense.Patches {
	[HarmonyPatch(typeof(Character), "SetWalk")]
	public static class SetWalkPatch {

		//We stop the walk toggle button from doing anything if we use it while crouching.
		//Because in this mod, it is a hotkey to trigger the sonar ping.
		public static void Prefix(Character __instance, ref bool walk) {
			if(__instance is Player && __instance.IsCrouching()) {
				walk = !walk;
			}
		}
	}
}