using EnemySense.Utils;
using HarmonyLib;
using UnityEngine;

namespace EnemySense.Patches {
	[HarmonyPatch(typeof(Player), "FixedUpdate")]
	public static class FixedUpdatePatch {

		public static void Postfix(ref Player __instance) {
			if(__instance == Player.m_localPlayer && __instance.TakeInput() && __instance.IsCrouching() && ZInput.GetButtonDown("ToggleWalk")) {

				Transform visualObject = UnityEngine.Object.Instantiate<Transform>(PrefabGetter.getPingVisual(), __instance.GetHeadPoint(), Quaternion.identity);


				GameObject audio = PrefabGetter.getPingAudio();
				if(audio != null) {
					GameObject audioObject = UnityEngine.Object.Instantiate<GameObject>(audio, __instance.GetHeadPoint(), Quaternion.identity);
				}

				Debug.Log("Player location: " + __instance.transform.position.ToString());

				//Do the Get Creaturss.
				int rayMask = LayerMask.GetMask(new string[] { "character", "character_net", "character_ghost" });
				Collider[] array = Physics.OverlapSphere(__instance.transform.position, 30F, rayMask);

				bool flag = false;
				foreach(Collider collider in array) {
					Debug.Log("Found a nearby Doofer at "+ Vector3.Distance(__instance.transform.position, collider.transform.position) + "m: " + collider.name + "|" + collider.transform.position.ToString());
					if(collider.GetComponent<Character>() != null) {
						Character boobo = collider.GetComponent<Character>();
						if(boobo != null && boobo != __instance) {
							flag = true;
							Debug.Log("Character component found, attempting to add now. " + boobo.name);
							bool test = EnemyHud.instance.TestShow(boobo);
							Debug.Log("Test successed?: " + test);
							EnemyHud.instance.ShowHud(boobo);
							EnemyHud.HudData hud = null;
							EnemyHud.instance.m_huds.TryGetValue(boobo, out hud);
							if(hud != null) {
								hud.m_hoverTimer = 0F;
								hud.m_gui.SetActive(true);
							}
						}
						
					}
				}

				foreach(EnemyHud.HudData hud in EnemyHud.m_instance.m_huds.Values) {
					Debug.Log("Enemy Hud Entry: " + hud.m_character.m_name + ", " + hud.m_hoverTimer + ", " + hud.m_gui.ToString() + ", " + hud.m_alerted + "," + hud.m_aware + "," + hud.m_gui.activeSelf + "," + hud.m_gui.activeInHierarchy + "," + hud.m_gui.hideFlags);
				}

				if(flag) {
					EnemyHud.instance.UpdateHuds(__instance, Time.deltaTime);
				}
			}
		}
	}
}
