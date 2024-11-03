using UnityEngine;

/// <summary>
/// This class is responsible for controlling the rain effect.
/// </summary>
public class RainEffect : MonoBehaviour
{
	[SerializeField] ParticleSystem ps;
	private ParticleSystem.EmissionModule emissionModule;
	
	/// <summary>
	/// These events are for knowing when the rain is falling really hard to stop the player from doing certain things like flying or holding onto walls
	/// </summary>
	public delegate void RainIntensity();
	public static event RainIntensity OnRainIntensityLimitReached;
	public static event RainIntensity OnRainIntensityDroppedBelowLimit;
	
	/// <summary>
	/// This is the value that the particle system will use for particles per second.
	/// </summary>
	private float rainIntensity;
	private float flyRainIntensityLimit = 45f;
	private bool intenseRaining = false;
	
	void Awake()
	{
		emissionModule = ps.emission;
	}
	
	/// <summary>
	/// Set the rain intensity. This will set the rate of the particle system.
	/// <para>
	/// The particle system will stop and start automatically based on the rate.
	/// </para>
	/// </summary>
	/// <param name="intensity"></param>
	public void SetRainIntensity(float intensity, PlayerMovement playerMovement)
	{
		if (intensity <= 0) DeactivateRainEffect();
		else if (!ps.isPlaying) ActivateRainEffect();
		
		rainIntensity = intensity;
		emissionModule.rateOverTime = rainIntensity;
		
		if (rainIntensity > flyRainIntensityLimit) {
			if (!intenseRaining) {
				intenseRaining = true;
				OnRainIntensityLimitReached?.Invoke();
			}
		}
		else if (rainIntensity <= flyRainIntensityLimit) {
			if (intenseRaining) {
				intenseRaining = false;
				OnRainIntensityDroppedBelowLimit?.Invoke();
			}
		}
	}
	
	public void ActivateRainEffect()
	{
		if (!ps.isPlaying) ps.Play();
	}
	
	public void DeactivateRainEffect()
	{
		ps.Stop(withChildren: true, stopBehavior: ParticleSystemStopBehavior.StopEmitting);
	}
}
