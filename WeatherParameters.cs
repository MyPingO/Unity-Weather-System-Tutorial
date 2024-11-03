using System;
using UnityEngine;

[Serializable]
public class WeatherEffectParameters
{
	public Color cloudColor;
	[Range(0, 1000)] public float cloudEmissionRate;
	[Range(0, 1000)] public float rainEmissionRate;
	[Range(0, 1000)] public float windSpeed;
	public bool lightningActive;
	public bool sunRaysActive;
}
