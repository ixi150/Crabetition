using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Claw : GameBehaviour
{
    [SerializeField]
    ParticleSystem ps;

    [SerializeField]
    LayerMask layerMask = -1;

    Crab crab;
    bool isHolding;
    new SphereCollider collider;
    Collider[] results = new Collider[20];

    private void Start()
    {
        crab = Limb.FindInParent<Crab>(transform);
        collider = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        if (!collider || !crab) return;

        var count = Physics.OverlapSphereNonAlloc(transform.position, transform.lossyScale.x * collider.radius, results, layerMask, QueryTriggerInteraction.Collide);
        count = results.Take(count).Select(_ => _.GetComponent<Claw>()).Where(_ => _ && _.crab != crab).Count();
        isHolding = count > 0;
        if (ps)
        {
            var em = ps.emission;
            em.enabled = isHolding;
        }
    }

}
