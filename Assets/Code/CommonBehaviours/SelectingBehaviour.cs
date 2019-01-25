using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using UnityEngine;

public class SelectingBehaviour : GameBehaviour
{
    const int MaxOverlapResults = 50;

    [SerializeField]
    LayerMask layerMask = -1;

    [SerializeField]
    Vector3 offset = Vector3.zero,
        size = Vector3.one,
        rotation = Vector3.zero;

    [SerializeField]
    bool canOutlineItems = false;


    public ISelectable CurrentSelection { get; private set; }

    Collider[] overlapResults = new Collider[MaxOverlapResults];

    private void OnEnable()
    {

    }

    private void OnDisable()
    {
        TrySetIsSelectedPickable(false);
    }

    private void Update()
    {
        UpdateSelection();
    }

    private void UpdateSelection()
    {
        if (canOutlineItems == false) return;
        var pickable = ChoosePreferedSelectable(FindAllPickables());
        if (CurrentSelection != pickable)
        {
            TrySetIsSelectedPickable(false);
            CurrentSelection = pickable;
            TrySetIsSelectedPickable(true);
        }
    }

    private void TrySetIsSelectedPickable(bool isSelected)
    {
        if (CurrentSelection == null) return;
        CurrentSelection.IsSelected = isSelected;
    }

    private IEnumerable<ISelectable> FindAllPickables()
    {
        var center = GetOverlapCenter();
        var orientation = GetOverlapRotation();
        var count = Physics.OverlapBoxNonAlloc(center, size / 2, overlapResults, orientation, layerMask, QueryTriggerInteraction.Collide);
        return overlapResults
            .Take(count)
            .Select(_ => _.GetComponent<ISelectable>())
            .Where(_ => _ != null);
    }

    private ISelectable ChoosePreferedSelectable(IEnumerable<ISelectable> pickables)
    {
        if (pickables == null) return null;
        var pos = transform.position + Vector3.up * offset.y;
        return pickables
            .OrderBy(_ => Vector3.SqrMagnitude(_.Mono.transform.position - pos))
            .FirstOrDefault();
    }

    private Vector3 GetOverlapCenter()
    {
        return transform.position + transform.TransformDirection(offset);
    }

    private Quaternion GetOverlapRotation()
    {
        return Quaternion.Euler(rotation) * transform.rotation;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.matrix = Matrix4x4.TRS(GetOverlapCenter(), GetOverlapRotation(), Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, size);
    }
}