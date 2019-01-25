using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Interfaces;

public class PickingBehaviour : GameBehaviour, IUser
{
    [SerializeField]
    Transform slotForItems = null;

    [SerializeField]
    SelectingBehaviour selecting = null;

    public bool IsHolding { get { return heldPickable != null; } }

    Transform SlotForItems { get { return slotForItems ? slotForItems : transform; } }
    IPickable heldPickable;
    

    public void PickUpOrRelease()
    {
        if (IsHolding)
        {
            TryThrow();
        }
        else
        {
            TryPickUp();
        }
    }

    protected virtual void Update()
    {
        if (IsHolding) UpdateHolding();
    }

    private void TryPickUp()
    {
        if (selecting == null) return;
        var selectedPickable = selecting.CurrentSelection;
        heldPickable = selectedPickable as IPickable;
        if (heldPickable == null) return;
        heldPickable.IsPicked = true;
        selecting.SetEnabled(false);
        selectedPickable = null;
    }

    private void TryThrow()
    {
        heldPickable.IsPicked = false;
        heldPickable = null;

        if (selecting == null) return;
        selecting.SetEnabled(true);
    }

    private void UpdateHolding()
    {
        var heldTransform = heldPickable.Mono.transform;
        var slotTransform = SlotForItems;
        heldTransform.position = slotTransform.position;
        heldTransform.rotation = slotTransform.rotation;
    }

    
}
