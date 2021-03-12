using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace EnemySense.Utils {
	public static class PrefabGetter {
		private static Transform pingVisual;
		private static GameObject pingAudio;

		public static Transform getPingVisual() {
			if(pingVisual == null) {
				GameObject fetch = ZNetScene.instance.GetPrefab("vfx_sledge_hit");
				Transform fetch2 = fetch.transform.Find("waves");
				pingVisual = UnityEngine.Object.Instantiate<Transform>(fetch2);
				MainModule mainModule = pingVisual.GetComponent<ParticleSystem>().main;
				mainModule.simulationSpeed = 0.2F;
				mainModule.startSize = 0.1F;
				mainModule.startSizeMultiplier = 60F;


				/*
				GameObject fetch = ZNetScene.instance.GetPrefab("vfx_blocked");
				Transform fetch2 = fetch.transform.Find("waves");
				pingVisual = UnityEngine.Object.Instantiate<Transform>(fetch2);
				MainModule mainModule = pingVisual.GetComponent<ParticleSystem>().main;
				mainModule.simulationSpeed = 0.2F;
				mainModule.startSize = 0.1F;
				mainModule.startSizeMultiplier = 6F;
				*/
			}


			return pingVisual;
		}

		public static GameObject getPingAudio() {
			if(pingAudio == null) {
				GameObject fetch = ZNetScene.instance.GetPrefab("sfx_lootspawn");

				if(fetch != null) {
					pingAudio = fetch;
					Debug.Log("Loaded " + pingAudio.ToString() + " prefab.");
					return fetch;
				}

				Debug.Log("Failed to load the audio prefab.");
				return null;
			}

			return pingAudio;
		}
	}
}
