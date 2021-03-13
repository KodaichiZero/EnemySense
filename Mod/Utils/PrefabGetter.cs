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
			}

			MainModule main = pingVisual.GetComponent<ParticleSystem>().main;
			main.startSizeMultiplier = EnemyHud.instance.m_maxShowDistance * 2F;
			return pingVisual;
		}

		public static GameObject getPingAudio() {
			if(pingAudio == null) {
				GameObject fetch = ZNetScene.instance.GetPrefab("sfx_lootspawn");
				pingAudio = UnityEngine.Object.Instantiate<GameObject>(fetch);
				ZSFX audioModule = pingAudio.GetComponent<ZSFX>();
				audioModule.m_minPitch = 0.8F;
				audioModule.m_maxPitch = 0.85F;
				audioModule.m_distanceReverb = true;
				audioModule.m_vol = 1F;
				audioModule.m_useCustomReverbDistance = true;
				audioModule.m_customReverbDistance = 10F;
				audioModule.m_delay = 1;
				audioModule.m_time = 1;
			}

			return pingAudio;
		}
	}
}
