using UnityEngine;

public class CombatZone : MonoBehaviour
{
    private Player player;
    private int enemyCount = 0;

    private void Start()
    {
        player = GetComponentInParent<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemyCount++;
            player.inCombat = true;
            player.animatorController.SetInCombat(player.inCombat);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemyCount = Mathf.Max(0, enemyCount - 1);

            if (enemyCount == 0)
            {
                player.inCombat = false;
                player.animatorController.SetInCombat(player.inCombat);
            }
        }
    }
}