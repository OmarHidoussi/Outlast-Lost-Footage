
using UnityEngine;

[RequireComponent (typeof(Collider))]
public class AudioTriggerSystem : MonoBehaviour
{

    #region Variables

    public enum States
    {
        Simple,
        WhileInZone,
        Path
    }

    [Space]
    public States TriggerType;

    [Space] [Space]
    [Header("Trigger Zone")]
    public BoxCollider col;
    public float BlendDistance; //Create Another Collider At the Same (Position, Center, Scale) + BlendDistance On X,Y,Z
    public bool DestroyOnEnter;
    public bool DestroyOnExit;

    [Space]
    [Header("Audio Properties")]
    public AudioSource source;
    public AudioClip audioClip;
    public float Volume;
    public float Pitch;

    [Space] [Space]
    [Header("Path")]
    public Transform audioMover;

    private Transform Player;
    private Vector3[] SplinePoint;
    private int SplineCount;
    private bool HasPlayed;

    #endregion

    #region BuiltInMethods

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<BoxCollider>();
        col.size += new Vector3(BlendDistance, BlendDistance, BlendDistance);

        SplineCount = transform.childCount;
        SplinePoint = new Vector3[SplineCount];

        Player = FindObjectOfType<InputManager>().transform;

        for (int i = 0; i < SplineCount; i++)
        {
            SplinePoint[i] = transform.GetChild(i).position;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(TriggerType != States.Path)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(col.center + transform.position, (col.size + new Vector3(BlendDistance, BlendDistance, BlendDistance)));
        }
        else
        {
            SplineCount = transform.childCount;
            SplinePoint = new Vector3[SplineCount];

            for (int i = 0; i < SplineCount; i++)
            {
                SplinePoint[i] = transform.GetChild(i).position;
            }

            if (SplineCount > 1)
            {
                for (int i = 0; i < SplineCount - 1; i++)
                {
                    Debug.DrawLine(SplinePoint[i], SplinePoint[i + 1], Color.magenta);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //audioMover.position = PositionOnSpline(Player.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (TriggerType == States.Simple && !HasPlayed)
            {
                source.volume = Volume;
                source.pitch = Pitch;
                source.PlayOneShot(audioClip);

                HasPlayed = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(TriggerType == States.WhileInZone)
            {
                source.loop = true;
                source.volume = Volume;
                source.pitch = Pitch;
                source.PlayOneShot(audioClip);
            }

            Player = other.transform;
            if (TriggerType == States.Path)
            {
                audioMover.position = PositionOnSpline(Player.position);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (TriggerType == States.Simple)
            {
                HasPlayed = false;

                if (DestroyOnExit)
                    Destroy(this.gameObject);
            }

            if (TriggerType == States.WhileInZone)
            {
                source.Stop();
            }

        }
    }

    #endregion

    #region CustomMethods

    private Vector3 PositionOnSpline(Vector3 pos)
    {
        int ClosestSplinePoint = GetClosestSplinePoint(pos);

        if(ClosestSplinePoint == 0)
        {
            return SplineSegment(SplinePoint[0], SplinePoint[1], pos);
        }
        else if(ClosestSplinePoint == SplineCount - 1)
        {
            return SplineSegment(SplinePoint[SplineCount - 1], SplinePoint[SplineCount - 2], pos);
        }
        else
        {
            Vector3 LeftSeg = SplineSegment(SplinePoint[ClosestSplinePoint - 1], SplinePoint[ClosestSplinePoint], pos);
            Vector3 RightSeg = SplineSegment(SplinePoint[ClosestSplinePoint + 1], SplinePoint[ClosestSplinePoint], pos);

            if ((pos - LeftSeg).sqrMagnitude <= (pos - RightSeg).sqrMagnitude)
                return LeftSeg;
            else
                return RightSeg;
        }
    }

    private int GetClosestSplinePoint(Vector3 pos)
    {
        int closestPoint = -1;
        float shortestDistance = 0.0f;

        for (int i = 0; i < SplineCount; i++)
        {
            float SqrDistance = (SplinePoint[i] - pos).sqrMagnitude;
            if(shortestDistance == 0.0f || SqrDistance < shortestDistance)
            {
                shortestDistance = SqrDistance;
                closestPoint = i;
            }
        }

        return closestPoint;
    }

    public Vector3 SplineSegment (Vector3 v1, Vector3 v2, Vector3 pos)
    {
        Vector3 v1ToPos = pos - v1;
        Vector3 sqrDurection = (v2 - v1).normalized;

        float distanceFromV1 = Vector3.Dot(sqrDurection, v1ToPos);

        if(distanceFromV1 < 0.0f)
        {
            return v1;
        }
        else if( distanceFromV1 * distanceFromV1 > (v2 - v1).sqrMagnitude)
        {
            return v2;
        }
        else
        {
            Vector3 FromV1 = sqrDurection * distanceFromV1;
            return v1 + FromV1;
        }

    }

    #endregion

}
