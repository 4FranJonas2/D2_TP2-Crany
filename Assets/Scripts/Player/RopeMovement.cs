using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Threading;

public class RopeMovement : MonoBehaviour
{
    [Header("Soga")]
    [SerializeField] private int numOfRopeSegments = 20;
    [SerializeField] private float ropeSegmentLength = 0.225f;

    [Header("Fisicas")]
    [SerielizeField] private Vector2 gravityForce = new Vector2(0.0f, -2f);
    [SerializeField] private float dampingFactor = 0.98f;

    [Header("Limites")]
    [SerializeField] private int numOfConstrainsRuns = 20;

    private LineRenderer lineRenderer;
    private List<RopeSegment> ropeSegments = new List<RopeSegment>();

    private Vector3 ropeStartpoint;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = numOfRopeSegments;

        ropeStartpoint = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        for (int i = 0; i < numOfRopeSegments; i++)
        {
            ropeSegments.Add(new RopeSegment(ropeStartpoint));
            ropeStartpoint.y -= ropeSegmentLength;
        }
    }

    // Update is called once per frame
    void Update()
    {
        DrawRope();
    }

    void FixedUpdate()
    {
        Simulate();

        for (int i = 0; i < numOfConstrainsRuns; i++)
        {
            ApplyConstrains();
        }
    }

    private void DrawRope()
    {
        Vector3[] ropePositions = new Vector3[numOfRopeSegments]

        for (int i = 0; i < ropeSegments.Count; i++)
        {
            ropePositions[i] = ropeSegments[i].currentPosition;
        }

        lineRenderer.SetPositions(ropePositions);
    }

    private void Simulate()
    {
        for (int i = 0; i < ropeSegments.Count; i++)
        {
            RopeSegment segment = ropeSegments[i];
            Vector2 velocity = (segment.currentPosition - segment.oldPosition) * dampingFactor;

            segment.oldPosition = segment.currentPosition;
            segment.currentPosition += velocity;
            segment.currentPosition += gravityForce * Time.deltaTime;
            ropeSegments[i] = segment;
        }
    }

    private void ApplyConstrains()
    {
        RopeSegment firstSegment = ropeSegments[0];
        firstSegment.currentPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        ropeSegments[0] = firstSegment;

        for (int i = 0; i < ropeSegments.Count - 1; i++)
        {
            RopeSegment currentSeg = ropeSegments[i];
            RopeSegment nextSeg = ropeSegments[i + 1];

            float dist = (currentSeg.currentPosition - nextSeg.currentPosition).sqrMagnitude;
            float difference = (dist - ropeSegmentLength);

            Vector2 changeDir = (currentSeg.currentPosition - nextSeg.currentPosition).normalized;
            vector2 changeVector = changeDir * difference;

            if (i != 0)
            {
                currentSeg.currentPosition -= (changeVector * 0.5f);
                nextSeg.currentPosition += (changeVector * 0.5f);
            }
            else
            {
                nextSeg.currentPosition += changeVector;
            }

            ropeSegments[i] = currentSeg;
            ropeSegments[i + 1] = nextSeg;
        }
    }

    public struct RopeSegment
    {
        public Vector2 currentPosition;
        public Vector2 oldPosition;

        public RopeSegment(Vector2 pos)
        {
            currentPosition = pos;
            oldPosition = pos;
        }
    }
}
