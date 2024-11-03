using UnityEngine;

public class LightningEffect : MonoBehaviour
{
	[SerializeField] ParticleSystem ps;
	
	public void ActivateLightningEffect()
	{
		if (!ps.isPlaying) ps.Play();
	}
	public void DeactivateLightningEffect()
	{
		ps.Stop(withChildren: true, stopBehavior: ParticleSystemStopBehavior.StopEmitting);
	}
}
