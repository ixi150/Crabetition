using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

public class GameBehaviour : MonoBehaviour, IMonoBehaviour
{
    MonoBehaviour IMonoBehaviour.Mono { get { return this as MonoBehaviour; } }

    public virtual void SetEnabled(bool enabled)
    {
        this.enabled = enabled;
    }

    protected T GetComponentOnlyInChildren<T>() where T : MonoBehaviour
    {
        foreach (Transform child in transform)
        {
            var component = child.GetComponent<T>();
            if (component) return component;
        }
        return null;
    }

    protected T GetComponentOnlyInParent<T>() where T : MonoBehaviour
    {
        Transform parent = transform.parent;
        if (parent)
        {
            var component = parent.GetComponent<T>();
            if (component) return component;
        }
        return null;
    }
}
