using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostCollectible : BaseCollectible
{
    [SerializeField] private AudioClip _ghostSound;

    private void OnEnable()
    {
        PickupCollectible();
    }

    protected override void PickupCollectible()
    {
        SoundManager.Instance.PlaySound(_ghostSound, transform.position, 1f);
        _collected = true;
    }
}
