using UnityEngine;

public class BossAttackRangeDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            BossController.instance.isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            BossController.instance.isPlayerInRange = false;
        }
    }
}
