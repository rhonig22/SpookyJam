using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] protected int _entranceNumber;
    [SerializeField] protected Transform _entrancePosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.GetComponent<PlayerInteractor>() != null)
        {
            PlayerInteractor player = collision.gameObject.GetComponent<PlayerInteractor>();
            player.SetInteractable(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.GetComponent<PlayerInteractor>() != null)
        {
            PlayerInteractor player = collision.gameObject.GetComponent<PlayerInteractor>();
            player.SetInteractable(null);
        }
    }

    public Boolean IsInteractable()
    {
        return true;
    }

    public void Interact()
    {
        if (IsInteractable())
        {
            Debug.Log("interacted");
            OpenDoor();
        }
    }

    public Vector3 GetEntrancePosition()
    {
        return _entrancePosition.position;
    }

    public int GetEntranceNumber()
    {
        return _entranceNumber;
    }

    protected virtual void OpenDoor()
    {
        Debug.Log("not implemented");
    }
}
