using UnityEngine;

[RequireComponent(typeof(WeatherTimer))]
[RequireComponent(typeof(WeatherState))]
[RequireComponent(typeof(WeatherEffects))]
public class WeatherSystem : MonoBehaviour
{
	public WeatherTimer weatherTimer;
	public WeatherState weatherState;
	public WeatherEffects weatherEffects;
	
	void Awake()
	{
		if (weatherTimer == null) weatherTimer = GetComponent<WeatherTimer>();
		if (weatherState == null) weatherState = GetComponent<WeatherState>();
		if (weatherEffects == null) weatherEffects = GetComponent<WeatherEffects>();
	}

	void OnEnable()
	{
		weatherTimer.OnTimerExpired += HandleTimerExpired;
		weatherState.OnWeatherChanged += HandleWeatherChanged;
	}

	void OnDisable()
	{
		weatherTimer.OnTimerExpired -= HandleTimerExpired;
	}

	private void HandleTimerExpired()
	{
		weatherState.CycleWeatherState();
	}
	
	private void HandleWeatherChanged(WeatherState.State state)
	{
		weatherEffects.SetWeatherEffect(state);
	}
}
