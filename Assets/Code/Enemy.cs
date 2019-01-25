using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;
using cakeslice;

public class Enemy : Character, ISelectable
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


    [SerializeField]
    bool isSelected = false;

    Outline outline;
    bool hasOutline = false;

    protected override void Awake()
    {
        base.Awake();

        hasOutline = outline = GetComponentInChildren<Outline>(includeInactive: true);
    }

}
