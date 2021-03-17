using HarmonyLib;

namespace CreatureSense.Patches {
	[HarmonyPatch(typeof(Character), "SetWalk")]
	public static class SetWalkPatch {

		//We stop the walk toggle button from doing anything if we use it while crouching.
		//Because in this mod, it is a hotkey to trigger the sonar ping.
		//But we only do this if a custom keybind has not been set.
		public static void Prefix(Character __instance, ref bool walk) {
			if(__instance is Player && __instance.IsCrouching() && FixedUpdatePatch.keyBind.Value.Length <= 0) {
				walk = !walk;
			}
		}
	}
}