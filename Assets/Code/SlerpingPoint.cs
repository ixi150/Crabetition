using UnityEngine;
using System.Collections;

public class SlerpingPoint : MonoBehaviour
{
    //[SerializeField]
    //float lerpingMultiplier = 1;

    //[SerializeField, Range(.1f, 2f)]
    //float transitionDuration = .5f;

    //Vector3 lastPosition;
    //Vector3 targetPosition;
    //Vector3 slerpedPosition;
    //bool isSlerping = false;
    //float slerpTime = 0;

    //private void Update()
    //{
    //    //if (isSlerping)
    //    {
    //        //float deltaTime = Time.time - slerpTime;
    //        //slerpedPosition = Vector3.Lerp(lastPosition, targetPosition, Time.deltaTime);
    //        //if (deltaTime >= transitionDuration) isSlerping = false;
    //    }
    //    transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * lerpingMultiplier);

    //}

    public void SetWantedPosition(Vector3 position)
    {
        transform.position = position;
        //if (!isSlerping) lastPosition = transform.position;
        //slerpTime = Time.time;
        //isSlerping = true;
    }

    //private void Start()
    //{
    //    slerpedPosition = transform.position;
    //}

//    private void OnDrawGizmos()
//    {
//        Gizmos.color = Color.yellow;
//        Gizmos.DrawLine(transform.position, slerpedPosition);
//    }
}
