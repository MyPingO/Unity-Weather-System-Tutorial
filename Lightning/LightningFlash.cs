using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class LightningFlash : MonoBehaviour
{
	[SerializeField] ParticleSystem ps;
	[SerializeField] Animator lightAnimator;
	[SerializeField] string animationName = "Lightning Flash";
	[SerializeField] float playChance = 1f;

	private bool checking = true;
	

	void Start()
	{
		if (ps == null)
		{
			Debug.LogError("ParticleSystem reference is missing!");
			return;
		}
	}

	void Update()
	{
		if (checking && ps.particleCount > 0)
		{
			// Start the coroutine to wait for the particle system duration
			StartCoroutine(CheckParticleSystem());
		}
	}
	
	IEnumerator CheckParticleSystem()
	{
		checking = false;

		// Trigger the animation if a particle is alive
		if (Random.value < playChance) PlayLightningFlash();

		// Wait for the particle system's duration
		yield return new WaitForSeconds(ps.main.startLifetime.constantMax);

		// After waiting, check again in the next Update cycle
		checking = true;
	}

	void PlayLightningFlash()
	{
		// Play the animation using the trigger
		lightAnimator.Play(animationName);
	}
	
	private void OnValidate()
	{
		if (ps == null || lightAnimator == null)
		{
			Debug.LogError("ParticleSystem or Animator reference is missing on the " + gameObject.name + "GameObject!");
		}
	}
}
