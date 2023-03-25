using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pit : MonoBehaviour
{
    
   private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Entity" && GetComponent<Building>().isActive())
        {
            Entity entity = collision.gameObject.GetComponent<Entity>();
            entity.Hurt(entity.getMaxHealth() / 2f);
            Destroy(gameObject);
        }
    }

    
}
