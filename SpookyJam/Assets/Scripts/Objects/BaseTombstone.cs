using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTombstone : BaseConditionalTrigger
{
    [SerializeField] GameObject _collectibleToReveal;
    [SerializeField] Transform _revealPosition;

    protected override void TriggerConditional()
    {
        Instantiate(_collectibleToReveal, _revealPosition.position, _revealPosition.rotation);
    }
}
