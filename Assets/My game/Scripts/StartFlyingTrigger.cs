using UnityEngine;

public class StartFlyingTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerFly pf = other.GetComponent<PlayerFly>();
            if (pf != null)
            {
                pf.StartFlying();
            }
        }
    }
}