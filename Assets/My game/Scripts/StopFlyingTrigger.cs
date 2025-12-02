using UnityEngine;

public class StopFlyingTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerFly pf = other.GetComponent<PlayerFly>();
            if (pf != null)
            {
                pf.StopFlying();
            }
        }
    }
}