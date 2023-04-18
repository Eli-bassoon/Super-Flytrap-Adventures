using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DamageCounter : MonoBehaviour
{
    // this script only returns damage
    // all health and respawning is handled in centralized respawn script

    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision != null)
        {
            // Instant death tag
            if (collision.gameObject.CompareTag("Instant Death"))
            {
                DamageHandler.instance.Respawn();
            }

            // Instant death tile
            else if (collision.transform.TryGetComponent(out Tilemap tm))
            {
                ContactPoint2D hit = collision.GetContact(0);
                Vector2 hitPosAdj = hit.point - 0.01f * hit.normal;
                Vector3Int tilePos = tm.WorldToCell(hitPosAdj);
                AdvancedRuleTile tile = tm.GetTile<AdvancedRuleTile>(tilePos);
                if (tile != null && tile.isDamaging)
                {
                    DamageHandler.instance.Respawn();
                }
            }

            // Enemy
            else if (collision.gameObject.tag == "Enemy")
            {
                print("ENEMYENEMY");
                if (collision.gameObject.TryGetComponent(out Enemy enemy))
                {
                    DamageHandler.instance.TakeDamage(enemy.damage);
                }
            }
        }
    }

}
