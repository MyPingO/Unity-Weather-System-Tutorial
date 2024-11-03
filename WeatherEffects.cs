using System.Collections;
using UnityEngine;

public class WeatherEffects : MonoBehaviour
{
	[SerializeField] WeatherEffectParameters currentWeatherEffectParameters;
	[SerializeField] WeatherEffectParameters targetWeatherEffectParameters;
	[SerializeField] WeatherEffectParameters sunnyWeatherParameters;
	[SerializeField] WeatherEffectParameters cloudyWeatherParameters;
	
	[SerializeField] WeatherEffectParameters windyWeatherParameters;
	
	[SerializeField] WeatherEffectParameters rainyWeatherParameters;
	
	[SerializeField] WeatherEffectParameters stormyWeatherParameters;
	
	[SerializeField] SunnyEffect sunnyEffect;
	[SerializeField] CloudEffect cloudEffect;
	[SerializeField] WindEffect windEffect;
	[SerializeField] RainEffect rainEffect;
	[SerializeField] LightningEffect lightningEffect;
	
	
	void Awake()
	{
		currentWeatherEffectParameters = sunnyWeatherParameters;
		targetWeatherEffectParameters = new WeatherEffectParameters();
	}
	
	void Start()
	{
		SetWeatherEffect(WeatherState.State.Sunny);
	}
	
	public void SetWeatherEffect(WeatherState.State weatherState)
	{
		switch (weatherState)
		{
			case WeatherState.State.Sunny:
				targetWeatherEffectParameters = sunnyWeatherParameters;
				break;
			case WeatherState.State.Cloudy:
				targetWeatherEffectParameters = cloudyWeatherParameters;
				break;
			case WeatherState.State.Windy:
				targetWeatherEffectParameters = windyWeatherParameters;
				break;
			case WeatherState.State.Rainy:
				targetWeatherEffectParameters = rainyWeatherParameters;
				break;
			case WeatherState.State.Stormy:
				targetWeatherEffectParameters = stormyWeatherParameters;
				break;
		}
		StartCoroutine(TransitionToNextEffect());
	}
	
	private IEnumerator TransitionToNextEffect()
	{
		float transitionTime = 3f;
		float elapsedTime = 0;
		
		WeatherEffectParameters startWeatherEffectParameters = currentWeatherEffectParameters;
		
		while (elapsedTime < transitionTime)
		{
			elapsedTime += Time.deltaTime;
			float t = Mathf.Clamp01(elapsedTime / transitionTime);
			
			currentWeatherEffectParameters = LerpWeatherEffectParameters(startWeatherEffectParameters, targetWeatherEffectParameters, t);
			UpdateWeatherEffects(currentWeatherEffectParameters);
			yield return null;
		}
		currentWeatherEffectParameters = targetWeatherEffectParameters;
		UpdateWeatherEffects(currentWeatherEffectParameters);
	}
	
	private WeatherEffectParameters LerpWeatherEffectParameters(WeatherEffectParameters from, WeatherEffectParameters to, float t)
	{
		WeatherEffectParameters result = new WeatherEffectParameters();
		result.cloudColor = Color.Lerp(from.cloudColor, to.cloudColor, t);
		result.cloudEmissionRate = Mathf.Lerp(from.cloudEmissionRate, to.cloudEmissionRate, t);
		result.rainEmissionRate = Mathf.Lerp(from.rainEmissionRate, to.rainEmissionRate, t);
		result.windSpeed = Mathf.Lerp(from.windSpeed, to.windSpeed, t);
		result.lightningActive = to.lightningActive;
		result.sunRaysActive = to.sunRaysActive;
		return result;
	}
	
	private void UpdateWeatherEffects(WeatherEffectParameters weatherEffectParameters)
	{
		cloudEffect.SetCloudDarkness(weatherEffectParameters.cloudColor);
		cloudEffect.SetCloudEmissionRate(weatherEffectParameters.cloudEmissionRate);
		rainEffect.SetRainIntensity(weatherEffectParameters.rainEmissionRate, playerMovement);
		windEffect.SetWindSpeed(weatherEffectParameters.windSpeed);
		
		if (weatherEffectParameters.lightningActive) lightningEffect.ActivateLightningEffect();
		else lightningEffect.DeactivateLightningEffect();
	
		if (weatherEffectParameters.sunRaysActive) sunnyEffect.ActivateSunnyEffect();
		else sunnyEffect.DeactivateSunnyEffect();
	}
	
	public WeatherEffectParameters GetCurrentWeatherEffectParameters()
	{
		return currentWeatherEffectParameters;
	}
	
	public SunnyEffect GetSunnyEffect()
	{
		return sunnyEffect;
	}
	
	public CloudEffect GetCloudEffect()
	{
		return cloudEffect;
	}
	
	public WindEffect GetWindEffect()
	{
		return windEffect;
	}
	
	public RainEffect GetRainEffect()
	{
		return rainEffect;
	}
	
	public LightningEffect GetLightningEffect()
	{
		return lightningEffect;
	}

}
