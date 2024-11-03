using UnityEngine;

public class CloudEffect : MonoBehaviour
{
	[SerializeField] ParticleSystem ps;
	private ParticleSystem.MainModule mainModule;
	private ParticleSystem.MinMaxGradient minMaxGradient;
	private ParticleSystem.EmissionModule emissionModule;
	private Color cloudDarkness;
	private float cloudEmissionRate;
	
	void Awake()
	{
		mainModule = ps.main;
		minMaxGradient = mainModule.startColor;
		emissionModule = ps.emission;
		cloudDarkness = minMaxGradient.colorMin;
		cloudEmissionRate = ps.emission.rateOverTime.constant;
	}
	
	/// <summary>
	/// Set the cloud darkness. This will set the second color of the particle systems start color (Random Between Two Colors).
	/// </summary>
	/// <param name="color"></param>
	public void SetCloudDarkness(Color color)
	{
		cloudDarkness = color;
		minMaxGradient = mainModule.startColor;
		minMaxGradient.colorMin = cloudDarkness;
		mainModule.startColor = minMaxGradient;
	}
	
	/// <summary>
	/// Set the cloud emission rate. This will set the rate of the particle system.
	/// <para>
	/// The particle system will stop and start automatically based on the rate.
	/// </para>
	/// </summary>
	/// <param name="rate"></param>
	public void SetCloudEmissionRate(float rate)
	{
		if (rate <= 0) DeactivateCloudEffect();
		else if (!ps.isPlaying) ActivateCloudEffect();
		
		cloudEmissionRate = rate;
		emissionModule.rateOverTime = cloudEmissionRate;
	}
	
	public void ActivateCloudEffect()
	{
		if (!ps.isPlaying) ps.Play();
	}
	
	public void DeactivateCloudEffect()
	{
		ps.Stop(withChildren: true, stopBehavior: ParticleSystemStopBehavior.StopEmitting);
	}
}
