using UnityEngine;

/// <summary>
/// This class is responsible for controlling the wind effect. It will apply a force to the player in the wind direction while the wind zone is active and the player is inside it.
/// <para>
/// The wind effect is simulated with a particle system.
/// </para>
/// <para>
/// The effect is customizable with a wind direction and speed. The wind direction will set the rotation of the particle system, and the wind speed will set the simulation speed of the particle system.
/// </para>
/// </summary>
public class WindEffect : MonoBehaviour
{
	[SerializeField] ParticleSystem[] windParticleSystems;
	private int windDirection;
	private float windSpeed;
	[SerializeField] float windForceMultiplier = 100f;
	[SerializeField] private Collider2D windZoneCollider;
	private WindZone windZone;
	private bool effectEnabled;
	[SerializeField] PlayerMovement playerMovement;
	
	public delegate void WindEffectToggler(bool active);
	public static event WindEffectToggler OnWindEffectToggled;
	void Awake()
	{
		if (!playerMovement) Debug.LogError("WindEffect: PlayerMovement not set.");
		if (!windZoneCollider) Debug.LogError("WindEffect: WindZoneCollider not set.");
		windZoneCollider.isTrigger = true;
		windZoneCollider.enabled = false;
		
		windZone = windZoneCollider.gameObject.GetComponent<WindZone>();
		
		DeactivateWind();
	}
	
	void OnEnable()
	{
		if (!windZone) Debug.LogError("WindEffect: WindZone script not found on WindZone object.");
		else windZone.OnWindZoneTrigger += ApplyWindForceToPlayer;
	}
	
	void OnDisable()
	{
		if (windZone) windZone.OnWindZoneTrigger -= ApplyWindForceToPlayer;
	}

	/// <summary>
	/// While the wind zone is active and the player is inside it, apply a force to the player in the wind direction with the set wind speed.
	/// </summary>
	/// <param name="collider2D"></param>
	private void ApplyWindForceToPlayer(Collider2D collider2D)
	{
		if (collider2D.gameObject == playerMovement.gameObject &&
		playerMovement.GetIsWallJumping() || playerMovement.GetIsDashing()) return;
		
		if (effectEnabled && collider2D.TryGetComponent<Rigidbody2D>(out var rb)) 
		{
			rb.AddForce(new Vector2(windDirection, 0) * (windSpeed * windForceMultiplier), ForceMode2D.Force);
		}
	}
	
	/// <summary>
	/// Activate the wind effect. This will start the particle system and enable the wind zone collider.
	/// </summary>
	public void ActivateWind()
	{
		SetWindDirection(GetRandomWindDirection());
		foreach (ParticleSystem ps in windParticleSystems) if (!ps.isPlaying) ps.Play();
		windZoneCollider.enabled = true;
		effectEnabled = true; 
	}
	
	/// <summary>
	/// Deactivate the wind effect. This will stop the particle system and disable the collider.
	/// </summary>
	public void DeactivateWind()
	{
		foreach (ParticleSystem ps in windParticleSystems) ps.Stop(withChildren: true, stopBehavior: ParticleSystemStopBehavior.StopEmitting);
		windZoneCollider.enabled = false;
		effectEnabled = false;
	}
	
	/// <summary>
	/// Set the wind direction. This will set the rotation of the particle system.
	/// </summary>
	/// <param name="direction"></param>
	public void SetWindDirection(int direction)
	{
		windDirection = direction;
		foreach (ParticleSystem ps in windParticleSystems)
		{
			ps.transform.rotation = Quaternion.Euler(0, (windDirection * 90) - 90, 0);
		}
	}
	
	/// <summary>
	/// Set the wind speed. This will set the simulation speed of the particle system.
	/// </summary>
	/// <param name="speed"></param>
	public void SetWindSpeed(float speed)
	{
		if (speed <= .25f) {
			windSpeed = 0f;
			DeactivateWind();
			return;
		}
		else if (!effectEnabled) ActivateWind();
		
		windSpeed = speed;
		foreach (ParticleSystem ps in windParticleSystems)
		{
			ParticleSystem.MainModule main = ps.main;
			main.simulationSpeed = speed;
		}
		
		float windEffectResistance = 1 + (PlayerPrefs.GetInt("playerWindResistance", 0) * 0.1f);
		if (speed >= windEffectResistance) OnWindEffectToggled?.Invoke(true);
		else OnWindEffectToggled?.Invoke(false);
	}
	
	private int GetRandomWindDirection()
	{
		return Random.value < 0.5f ? -1 : 1;
	}
	
	public float GetWindSpeed()
	{
		return windSpeed;
	}
	
	public int GetWindDirection()
	{
		return windDirection;
	}
}

