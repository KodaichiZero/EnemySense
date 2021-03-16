using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace CreatureSense.Utils {
	public static class PrefabGetter {

		private static Transform pingVisual;
		private static GameObject pingAudio;

		//Used to get the shockwave effect.
		public static Transform getPingVisual() {
			if(pingVisual == null) {
				GameObject fetch = ZNetScene.instance.GetPrefab("vfx_sledge_hit");
				Transform fetch2 = fetch.transform.Find("waves");
				pingVisual = UnityEngine.Object.Instantiate<Transform>(fetch2);
				MainModule mainModule = pingVisual.GetComponent<ParticleSystem>().main;
				mainModule.simulationSpeed = 0.2F;
				mainModule.startSize = 0.1F;
			}

			//Resize the effect every time to match your sneak skill.
			MainModule main = pingVisual.GetComponent<ParticleSystem>().main;
			main.startSizeMultiplier = EnemyHud.instance.m_maxShowDistance * 2F;
			return pingVisual;
		}

		//Used to get the sound effect.
		public static GameObject getPingAudio() {
			if(pingAudio == null) {
				GameObject fetch = ZNetScene.instance.GetPrefab("sfx_lootspawn");
				pingAudio = UnityEngine.Object.Instantiate<GameObject>(fetch);
				ZSFX audioModule = pingAudio.GetComponent<ZSFX>();

				//Adjusting the audio settings to give it some cool reverb.
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
