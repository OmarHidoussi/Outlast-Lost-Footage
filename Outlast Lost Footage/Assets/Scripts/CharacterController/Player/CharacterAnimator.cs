using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterAnimator : MonoBehaviour
{

    #region Variables

    private InputManager input;

    [Header("Camera Animation")]
    [SerializeField] private Animator anim;

    [Header("Character Animation")]
    public Animator CharacterAnim;
    public Animator ColliderAnim;
    public Transform Target;
    public float LeftHandWeight;
    public TwoBoneIKConstraint LeftHandConstraint;
    public MultiRotationConstraint SpineConstraint;
    [Range(0,5f)]public float DistanceToGround;
    public LayerMask layerMask;


    #endregion

    #region BuiltInMethods
    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleCameraAnimation();
        HandeMovement();
        HandleCrouch();
        HandleInteraction();

        LeftHandConstraint.weight = LeftHandWeight;
    }

   /* private void OnAnimatorIK(int layerIndex)
    {
        if(CharacterAnim)
        {
            CharacterAnim.SetIKPositionWeight(AvatarIKGoal.LeftFoot,1f);
            CharacterAnim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);

            RaycastHit hit;
            Ray ray = new Ray(CharacterAnim.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);

            if (Physics.Raycast(ray, out hit, DistanceToGround + 1f,layerMask))
            {
                if(hit.transform.tag == "walkable")
                {
                    Vector3 footPosition = hit.point;
                    footPosition.y += DistanceToGround;
                    CharacterAnim.SetIKPosition(AvatarIKGoal.LeftFoot, footPosition);
                }
            }

        }
    }*/

    #endregion

    #region CustomMethods
    void HandleCameraAnimation()
    {
        anim.SetBool("CameraOn",input.CameraOn);
    }

    void HandeMovement()
    {
        CharacterAnim.SetFloat("VelocityX", input.Mov_Axis.y);
        CharacterAnim.SetFloat("VelocityY", input.Mov_Axis.x);
        CharacterAnim.SetBool("Sprinting", input.IsSprinting);
    }

    void HandleCrouch()
    {
        CharacterAnim.SetBool("Crouching", input.IsCrouching);
        ColliderAnim.SetBool("Crouch", input.IsCrouching);
    }

    void HandleInteraction()
    {
        CharacterAnim.SetBool("PickUp", input.Interact);

        if(input.Interact)
        {
            LeftHandWeight = 1;
            SpineConstraint.weight = 0.22f;
        }
        else
        {
            LeftHandWeight = Mathf.Lerp(LeftHandWeight, 0, 5f * Time.deltaTime);
            SpineConstraint.weight = Mathf.Lerp(SpineConstraint.weight, 0, 5f * Time.deltaTime);
        }

    }

    #endregion

}
