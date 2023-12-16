using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterIk : MonoBehaviour
{
    public Animator CharacterAnim;
    [Range(0, 1f)] public float DistanceToGround;
    public LayerMask layerMask;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if(CharacterAnim)
        {

            CharacterAnim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, CharacterAnim.GetFloat("IKLeftFootWeight"));
            CharacterAnim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, CharacterAnim.GetFloat("IKLeftFootWeight"));

            CharacterAnim.SetIKPositionWeight(AvatarIKGoal.RightFoot, CharacterAnim.GetFloat("IKRightFootWeight"));
            CharacterAnim.SetIKRotationWeight(AvatarIKGoal.RightFoot, CharacterAnim.GetFloat("IKRightFootWeight"));

            //Left foot

            RaycastHit hit;
            Ray ray = new Ray(CharacterAnim.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);

            if (Physics.Raycast(ray, out hit, DistanceToGround + 1f,layerMask))
            {
                if(hit.transform.tag == "Walkable")
                {
                    Vector3 footPosition = hit.point;
                    footPosition.y += DistanceToGround;
                    CharacterAnim.SetIKPosition(AvatarIKGoal.LeftFoot, footPosition);
                    CharacterAnim.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(transform.forward, hit.normal));
                }
            }

            //Right Foot

            ray = new Ray(CharacterAnim.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up, Vector3.down);

            if (Physics.Raycast(ray, out hit, DistanceToGround + 1f, layerMask))
            {
                if (hit.transform.tag == "Walkable")
                {
                    Vector3 footPosition = hit.point;
                    footPosition.y += DistanceToGround;
                    CharacterAnim.SetIKPosition(AvatarIKGoal.RightFoot, footPosition);
                    CharacterAnim.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(transform.forward, hit.normal));
                }
            }
        }
    }
}
