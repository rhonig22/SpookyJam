using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    private IInteractable _interactable;

    public void SetInteractable(IInteractable interactable)
    {
        _interactable = interactable;
        if (_interactable != null)
        {
            _playerInput.actions["Flip"].Disable();
        }
        else
        {
            _playerInput.actions["Flip"].Enable();
        }
    }

    public void Interact()
    {
        if (_interactable != null)
            _interactable.Interact();
    }
}
