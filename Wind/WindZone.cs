using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class WindZone : MonoBehaviour
{
	public delegate void WindZoneTrigger(Collider2D other);
	public event WindZoneTrigger OnWindZoneTrigger;
	private bool playerInside = false;
	private Collider2D playerCollider;

	void OnEnable()
	{
		WindEffect.OnWindEffectToggled += ToggleWindZone;
	}
	
	void OnDisable()
	{
		WindEffect.OnWindEffectToggled -= ToggleWindZone;
	}

	void FixedUpdate()
	{
		if (playerInside) OnWindZoneTrigger?.Invoke(playerCollider);
	}
	
	private void OnTriggerStay2D(Collider2D other)
	{
		if (other.CompareTag("Player")) {
			playerInside = true;
			playerCollider = other;
		}
	}
	
	public void ToggleWindZone(bool active)
	{
		GetComponent<Collider2D>().enabled = active;
	}
}
