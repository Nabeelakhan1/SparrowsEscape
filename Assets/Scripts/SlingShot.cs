using UnityEngine;

public class SlingShot : MonoBehaviour
{
    public LineRenderer[] HandlesLineRenderers;
    public Transform[] HandleAnchorTrnsforms;
    public DragHandle DragHandle;
    public Transform ReleasePointTransform;
    public Transform ProjectileSpawnTransform;
    public Transform AimerTransform;
    public GameObject ProjectilePrefab;
    public float StartPower = 0; // Expose this variable in the Inspector
    public LineRenderer Trajectory_line;

    private float[] LineLengths;

    public float GetVelocity()
    {
        return Vector3.Distance(DragHandle.transform.position, ReleasePointTransform.transform.position) * 3.5f;
    }

    public float GetDistance(float Vinit)
    {
        var g = Physics.gravity.y;
        var Vvert = Vinit * (Mathf.Sin(GetAngle() * Mathf.Deg2Rad));
        var Vhor = Vinit * (Mathf.Cos(GetAngle() * Mathf.Deg2Rad));
        var Tvert = (0 - Vvert) / g;
        var Thor = 2 * Tvert;
        var distance = Vhor * Thor;
        return distance;
    }

    public float GetHeight(float Vinit, int amountPoints, int pointIndex)
    {
        var g = Physics.gravity.y;
        var Vvert = Vinit * (Mathf.Sin(GetAngle() * Mathf.Deg2Rad));
        var Vhor = Vinit * (Mathf.Cos(GetAngle() * Mathf.Deg2Rad));
        var Tvert = (0 - Vvert) / g;
        var Thor = 2 * Tvert;
        var Dtot = Vhor * Thor;
        var Dp = (Dtot / (amountPoints)) * pointIndex;
        var T2 = Dp / Vhor;
        var height = ((Vvert * Dp) / Vhor) + 0.5f * g * Mathf.Pow(T2, 2);
        return height;
    }

    private void SetTrajectoryLineActive(bool active)
    {
        Trajectory_line.enabled = active;
    }

    public void MakeShot()
    {
        var _projectile = Instantiate(ProjectilePrefab, ProjectileSpawnTransform.position, Quaternion.identity) as GameObject;
        _projectile.GetComponent<Rigidbody>().AddForce(GetShotDirection() * StartPower * 4.5f, ForceMode.Impulse);

        Destroy(_projectile, 4.0f);
        SetTrajectoryLineActive(true); // Keep the trajectory line active after the shot
    }

    public float GetAngle()
    {
        var angle = Vector3.Angle((ReleasePointTransform.transform.position - DragHandle.transform.position).normalized, Vector3.right);

        if (DragHandle.transform.position.y > AimerTransform.position.y)
        {
            angle = angle * -1;
        }

        return angle;
    }

    private void Start()
    {
        LineLengths = new float[2];
        AimerTransform.position = new Vector3(1, 1, 0);

        for (var i = 0; i < HandlesLineRenderers.Length; i++)
        {
            HandlesLineRenderers[i].SetPosition(0, HandleAnchorTrnsforms[i].position);
            HandlesLineRenderers[i].SetPosition(1, DragHandle.transform.position);
            HandlesLineRenderers[i].startWidth = 0.15f;
            HandlesLineRenderers[i].endWidth = 0.05f;
        }

        SetTrajectoryLineActive(true); // Ensure the trajectory line is active at the start
    }

    private void OnEnable()
    {
        DragHandle.OnDragHandleReleaseEvent += DragHandle_OnDragHandleReleaseEvent;
    }

    private void OnDisable()
    {
        DragHandle.OnDragHandleReleaseEvent -= DragHandle_OnDragHandleReleaseEvent;
    }

    private void OnDestroy()
    {
        DragHandle.OnDragHandleReleaseEvent -= DragHandle_OnDragHandleReleaseEvent;
    }

    private void DragHandle_OnDragHandleReleaseEvent()
    {
        MakeShot();
    }

    private void Update()
    {
        UpdateLines();
        UpdateAim();
        UpdateTrajectoryLine(); // Update the trajectory line while the player is aiming
        StartPower = Vector3.Distance(DragHandle.transform.position, ReleasePointTransform.transform.position);
    }

    private void UpdateLines()
    {
        for (var i = 0; i < HandlesLineRenderers.Length; i++)
        {
            HandlesLineRenderers[i].SetPosition(1, DragHandle.transform.position);
            HandlesLineRenderers[i].SetPosition(0, HandleAnchorTrnsforms[i].position);

            HandlesLineRenderers[i].GetComponent<LineRenderer>().startWidth = 0.15f / LineLengths[i];
            HandlesLineRenderers[i].GetComponent<LineRenderer>().endWidth = 0.05f / LineLengths[i];

            LineLengths[i] = Vector3.Distance(DragHandle.transform.position, HandleAnchorTrnsforms[i].position);

            if (LineLengths[i] <= 0.65f)
            {
                LineLengths[i] = 0.65f;
            }
        }
    }

    private void UpdateAim()
    {
        var pullDirection = ReleasePointTransform.position - (DragHandle.transform.position - ReleasePointTransform.position).normalized;
        AimerTransform.position = pullDirection;
    }

    private void UpdateTrajectoryLine()
    {
        int numberOfPoints = 30; // Number of points in the line renderer
        Trajectory_line.positionCount = numberOfPoints;

        Vector3 startPoint = ProjectileSpawnTransform.position;
        Vector3 startVelocity = GetShotDirection() * StartPower * 4.5f;

        for (int i = 0; i < numberOfPoints; i++)
        {
            float t = i / (float)numberOfPoints; // Time between each point
            Vector3 point = startPoint + t * startVelocity + 0.5f * Physics.gravity * t * t;
            Trajectory_line.SetPosition(i, point);
        }
    }

    private Vector3 GetShotDirection()
    {
        return (AimerTransform.position - ReleasePointTransform.transform.position).normalized;
    }
}
