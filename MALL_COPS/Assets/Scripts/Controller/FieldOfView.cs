using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();

    public float meshResolution;
    public int edgeResolveIterations;
    public float edgeDistThreshold;

    [Header("References")]
    public MeshFilter viewMeshFilter;
    Mesh viewMesh;

    private void Start()
    {
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;

        StartCoroutine(FindTargetsWithDelay(.2f));
    }

    private void LateUpdate()
    {
        DrawFieldOfView();
    }

    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        //Get targets in radius
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            //Check if the target is in the field of view
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle/2)
            {
                float distToTarget = Vector3.Distance(transform.position, target.position);
                //Check if there is a line of sight to the target
                if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                }
            }
        }
    }

    void DrawFieldOfView()
    {
        //Get number of rays to cast and the angle between them
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;

        List<Vector3> viewPoints = new List<Vector3>();  //The list of hit points
        ViewCastInfo oldViewCast = new ViewCastInfo();

        for (int i = 0; i <= stepCount; i++)
        {
            //Rotate clockwise from the left by incrementing with the step angle size
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            //Cast the ray
            ViewCastInfo newViewCast = ViewCast(angle);

            //Looking for edge
            if (i > 0)
            {
                bool edgeDistThresholdExceeded = Mathf.Abs(oldViewCast.distance - newViewCast.distance) > edgeDistThreshold;

                //If the casts didn't hit the same obstacle or if they're too far apart
                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDistThresholdExceeded))
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);

                    if (edge.pointA != Vector3.zero)
                        viewPoints.Add(edge.pointA);
                    if (edge.pointB != Vector3.zero)
                        viewPoints.Add(edge.pointB);
                }
            }

            //Add the hit point (or end of ray otherwise) to the list
            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        //The vertex count is the hit points + the origin
        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        //The mesh is in local space
        vertices[0] = Vector3.zero;
        //Get the vertices and triangles
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        //Build the mesh
        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    EdgeInfo FindEdge(ViewCastInfo _minViewCast, ViewCastInfo _maxViewCast)
    {
        float minAngle = _minViewCast.angle;
        float maxAngle = _maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        //Iterate around the edge 
        for (int i = 0; i < edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDistThresholdExceeded = Mathf.Abs(_minViewCast.distance - newViewCast.distance) > edgeDistThreshold;

            //If the raycasts hit the same obstacle and not too far apart
            if (newViewCast.hit == _minViewCast.hit && !edgeDistThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }

    public Vector3 DirFromAngle(float _angleInDegrees, bool _angleIsGlobal)
    {
        if (!_angleIsGlobal)
        {
            _angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(_angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(_angleInDegrees * Mathf.Deg2Rad));
    }

    ViewCastInfo ViewCast(float _globalAngle)
    {
        Vector3 dir = DirFromAngle(_globalAngle, true);
        if (Physics.Raycast(transform.position, dir, out RaycastHit hit, viewRadius, obstacleMask))
        {
            //The ray that hit
            return new ViewCastInfo(true, hit.point, hit.distance, _globalAngle);
        }
        else
        {
            //Full length of ray
            return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, _globalAngle);
        }
    }

    IEnumerator FindTargetsWithDelay(float _delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(_delay);
            FindVisibleTargets();
        }
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float distance;
        public float angle;

        public ViewCastInfo (bool _hit, Vector3 _point, float _dist, float _angle)
        {
            hit = _hit;
            point = _point;
            distance = _dist;
            angle = _angle;
        }
    }

    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }
    }
}
