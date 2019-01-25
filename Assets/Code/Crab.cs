using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Crab : GameBehaviour
{
    public bool isPlayer = true;
    public float EPS = .1f;

    [SerializeField]
    public float
        skyHeight = 100,
        minDistance = 4f,
        maxDistance = 8f,
        angleTollerance = 30;

    [SerializeField]
    Transform[] legs = null;

    [SerializeField]
    float rotateSpeed = 1, moveSpeed = 1;

    [SerializeField]
    float heightOverGround = 5, heightDelta = 1, heightSpeed = 1, heightLerpFactor = 2;

    [SerializeField]
    float leanAngle = 30;

    Vector3 lastGroundPosition;

    private void Awake()
    {
        foreach (var leg in legs)
        {
            leg.GetChild(0).gameObject.AddComponent<Limb>();
            RecursiveAddJoints(leg.GetChild(0));
        }
    }

    private void Start()
    {
        var groundpos = CastOnNavmesh(transform.position);
        if (groundpos.HasValue) lastGroundPosition = groundpos.Value;
        else groundpos = transform.position;
    }

    void RecursiveAddJoints(Transform t)
    {
        if (t == null || t.childCount < 1) return;
        t.gameObject.AddComponent<Joint>();
        RecursiveAddJoints(t.GetChild(0));
    }

    float angleRight;
    float angleLeft;
    bool goRight;

    void Update()
    {
        var inputScreen = GetMoveInput();
        var inputWorld = new Vector3(inputScreen.x, 0, inputScreen.y);

        angleRight = Vector3.Angle(transform.right, inputWorld);
        angleLeft = Vector3.Angle(-transform.right, inputWorld);
        goRight = angleLeft > angleRight;
        var directionMultiplier = goRight ? 1 : -1;
        var angle = goRight ? angleRight : angleLeft;
        var movePercent = 1 - angle / 180f;
        //var targetLean = leanAngle * (goRight ? 1 : -1);
        var inputScreenMagnitude = inputScreen.magnitude;
        if (inputScreenMagnitude > 0)
        {
            transform.right = Vector3.MoveTowards(transform.right, directionMultiplier * inputWorld, Time.deltaTime * rotateSpeed);
            transform.position += transform.right * movePercent * moveSpeed * directionMultiplier * inputScreenMagnitude * Time.deltaTime;
        }
        //var euler = transform.eulerAngles;
        //euler.z = Mathf.MoveTowards(euler.z, targetLean, Time.deltaTime * 10);
        //transform.eulerAngles = euler;

        UpdateHeight();
    }

    void UpdateHeight()
    {
        var sin = Mathf.Sin(Time.time * heightSpeed);
        var pos = transform.position;
        var navPos = CastOnNavmesh(pos + Vector3.up * 100);
        if (navPos.HasValue) lastGroundPosition = navPos.Value;
        var wantedY = lastGroundPosition.y + heightOverGround + sin * heightDelta;
        pos.y = Mathf.Lerp(pos.y, wantedY, Time.deltaTime * heightLerpFactor);
        transform.position = pos;
    }

    Vector3? CastOnNavmesh(Vector3 pos)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(pos, out hit, 10000, -1))
        {
            return hit.position;
        }
        return null;
    }

    Vector2 GetMoveInput()
    {
        return isPlayer ? GetCameraRelativeMovement() : Vector2.zero;
    }

    Vector2 GetCameraRelativeMovement()
    {
        Transform cameraTransform = Camera.main.transform;

        Vector2 forward = cameraTransform.TransformDirection(Vector3.forward).ToSceenPosition();
        forward = forward.normalized;
        Vector2 right = new Vector2(forward.y, -forward.x);

        float v = GetAxis("Vertical");
        float h = GetAxis("Horizontal");
        return h * right + v * forward;
    }

    float GetAxis(string axisName)
    {
        return Input.GetAxisRaw(axisName);
    }

    bool IsButton(string buttonName)
    {
        return Input.GetButton(buttonName);
    }

    bool IsButtonDown(string buttonName)
    {
        return Input.GetButtonDown(buttonName);
    }

    bool IsButtonUp(string buttonName)
    {
        return Input.GetButtonUp(buttonName);
    }
}
