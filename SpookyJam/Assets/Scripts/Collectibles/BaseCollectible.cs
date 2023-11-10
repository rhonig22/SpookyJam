using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCollectible : MonoBehaviour
{
    [SerializeField] private string _collectibleName = "Pumpkin";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var controller = collision.GetComponent<PlayerController>();
        if (controller != null)
        {
            PickupCollectible();
        }
    }

    private void PickupCollectible() {
        DataManager.Instance.PickupCollectible(_collectibleName);
        Destroy(gameObject);
    }

}
