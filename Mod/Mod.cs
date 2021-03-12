using BepInEx;
using HarmonyLib;
using System.Linq;

namespace EnemySense {
	[BepInPlugin("org.kodaichizero.enemysense", "EnemySense", "1.0.0.0")]
	public class Mod : BaseUnityPlugin {
		private static readonly Harmony harmony = new(typeof(Mod).GetCustomAttributes(typeof(BepInPlugin), false)
			.Cast<BepInPlugin>()
			.First()
			.GUID);

		private void Awake() {
			harmony.PatchAll();
		}

		private void OnDestroy() {
			harmony.UnpatchSelf();
		}
	}
}
