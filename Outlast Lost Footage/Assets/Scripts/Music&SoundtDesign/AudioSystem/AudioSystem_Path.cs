using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSystem_Path : MonoBehaviour
{

    #region Variables

    [Header("Path")]
    public Transform PathObject;
    public Transform audioMover;
    public AudioMixerGroup MixerGroup;

    public float Volume;
    public float Pitch = 1f;

    private Transform Player;
    private Vector3[] SplinePoint;
    private int SplineCount;

    [Space]
    public bool DestroyOnEnter;
    public bool DestroyOnExit;

    #endregion

    #region BuiltInMethods

    // Start is called before the first frame update
    void Start()
    {

        AudioSource source = PathObject.GetComponent<AudioSource>();

        if (MixerGroup != null)
        {
            source.outputAudioMixerGroup = MixerGroup;
        }

        SplineCount = transform.childCount;
        SplinePoint = new Vector3[SplineCount];

        Player = FindObjectOfType<InputManager>().transform;

        for (int i = 0; i < SplineCount; i++)
        {
            SplinePoint[i] = transform.GetChild(i).position;
        }

        source.volume = Volume;
        source.pitch = Pitch;

    }

    private void OnDrawGizmosSelected()
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

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player = other.transform;
            audioMover.position = PositionOnSpline(Player.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            if (DestroyOnExit)
                StartCoroutine(destroy());
        }
    }

    #endregion

    #region CustomMethods
    IEnumerator destroy()
    {
        yield return new WaitForSeconds(audioMover.GetComponent<AudioSource>().clip.length);
        Destroy(this.gameObject);
    }

    private Vector3 PositionOnSpline(Vector3 pos)
    {
        int ClosestSplinePoint = GetClosestSplinePoint(pos);

        if (ClosestSplinePoint == 0)
        {
            return SplineSegment(SplinePoint[0], SplinePoint[1], pos);
        }
        else if (ClosestSplinePoint == SplineCount - 1)
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
            if (shortestDistance == 0.0f || SqrDistance < shortestDistance)
            {
                shortestDistance = SqrDistance;
                closestPoint = i;
            }
        }

        return closestPoint;
    }

    public Vector3 SplineSegment(Vector3 v1, Vector3 v2, Vector3 pos)
    {
        Vector3 v1ToPos = pos - v1;
        Vector3 sqrDurection = (v2 - v1).normalized;

        float distanceFromV1 = Vector3.Dot(sqrDurection, v1ToPos);

        if (distanceFromV1 < 0.0f)
        {
            return v1;
        }
        else if (distanceFromV1 * distanceFromV1 > (v2 - v1).sqrMagnitude)
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
