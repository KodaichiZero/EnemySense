using BepInEx;
using CreatureSense.Patches;
using HarmonyLib;
using System.Linq;

namespace CreatureSense {
	[BepInPlugin("org.kodaichizero.creaturesense", "CreatureSense", "1.0.0.0")]
	public class Mod : BaseUnityPlugin {
		private static readonly Harmony harmony = new(typeof(Mod).GetCustomAttributes(typeof(BepInPlugin), false)
			.Cast<BepInPlugin>()
			.First()
			.GUID);

		private void Awake() {
			TestShowPatch.baseRange = base.Config.Bind<float>("Adjustments", "Base Detection Range", 30.0F, "How far you can see an enemy's health bar by default. The built-in value in the game is 30 meters. Keep in mind that a very large value will not allow you to see enemies unloaded from the game due to distance.");
			TestShowPatch.skillMultiplier = base.Config.Bind<float>("Adjustments", "Skill Multiplier", 1.0F, "How much to multiply the increase in detection range granted by Sneak skill level. A mupltiplier of 1 grants 30 additional meters of range at Sneak 100, for a total of 60 meters. A multiplier of 5 would grant 150 additional meters, and so on. ");
			FixedUpdatePatch.staminaDrain = base.Config.Bind<float>("Adjustments", "Stamina Drain", 70.0F, "How much stamina to drain when using the skill. By default this is 70, rather large to make the game balanced. Setting this to 0 would cause no drain to occur.");
			FixedUpdatePatch.showMessage = base.Config.Bind<bool>("Features", "Show Message", true, "When using the ability, show a message confirming how many creatures are nearby.");
			FixedUpdatePatch.showVisual = base.Config.Bind<bool>("Features", "Visual Effect", true, "Show an expanding circle upon using the ability, representing the detection range.");
			FixedUpdatePatch.playAudio = base.Config.Bind<bool>("Features", "Audio Effect", true, "Play a shwimsical sound when using the ability. Can be heard by other players.");
			UpdateEventPinPatch.showMinimapIcons = base.Config.Bind<bool>("Features", "Minimap Icons", true, "Show detected enemies on the minimap.");
			UpdateMapPatch.allowAdditionalZoom = base.Config.Bind<bool>("Features", "Additional Zoom", true, "Lets you zoom in two additional steps on the minimap.");
			harmony.PatchAll();
		}

		private void OnDestroy() {
			harmony.UnpatchSelf();
		}
	}
}
