using UnityEngine;

public class SunnyEffect : MonoBehaviour
{
	[SerializeField] ParticleSystem ps;
	private ParticleSystem.EmissionModule emissionModule;
	
	void Awake()
	{
		emissionModule = ps.emission;
	}
	
	public void SetSunIntensity(float intensity)
	{
		if (intensity <= 0) DeactivateSunnyEffect();
		else if (!ps.isPlaying) ActivateSunnyEffect();
		
		emissionModule.rateOverTime = intensity;
	}
	
	public void ActivateSunnyEffect()
	{
		if (!ps.isPlaying) ps.Play();
	}
	
	public void DeactivateSunnyEffect()
	{
		ps.Stop(withChildren: true, stopBehavior: ParticleSystemStopBehavior.StopEmitting);
	}
}
