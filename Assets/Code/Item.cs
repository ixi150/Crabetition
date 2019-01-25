using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Interfaces;
using cakeslice;

public class Item : GameBehaviour, IPickable
{
    bool ISelectable.IsSelected
    {
        get { return isSelected; }
        set
        {
            isSelected = value;
            if (hasOutline)
                outline.enabled = isSelected;
        }
    }

    bool IPickable.IsPicked
    {
        get { return isPicked; }
        set
        {
            isPicked = value;
            if (hasRigidbody)
                rigidbody.isKinematic = value;
        }
    }

    [SerializeField]
    bool isSelected = false;

    [SerializeField]
    bool isPicked = false;

    Outline outline;
    new Rigidbody rigidbody;
    bool hasOutline = false;
    bool hasRigidbody = false;

    private void Awake()
    {
        hasOutline = outline = GetComponent<Outline>();
        hasRigidbody = rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        
    }
}