using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrapplingGun : MonoBehaviour
{
    [SerializeField] public static GrapplingGun gg;
    private LineRenderer lr;
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public Transform gunTip, camera, player;
    private float maxDistance = 100f;
    private SpringJoint joint;


    private void Awake()
    {
        gg = this;
        lr = GetComponent<LineRenderer>();
    }

    private void FixedUpdate()
    {
        DrawRope();
    }

    public void StartGrapple(InputAction.CallbackContext context)
    {
        if (context.started)
        {

            RaycastHit hit;
            if (Physics.Raycast(camera.position, camera.forward, out hit, maxDistance))
            {
                grapplePoint = hit.point;
                joint = player.gameObject.AddComponent<SpringJoint>();
                joint.autoConfigureConnectedAnchor = false;
                joint.connectedAnchor = grapplePoint;

                float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

                //customizable variables (floats)
                joint.maxDistance = distanceFromPoint * 0.8f;
                joint.minDistance = distanceFromPoint * 0.25f;


                //more or less pull/push
                joint.spring = 4.5f;
                joint.damper = 7f;
                joint.massScale = 4.5f;

                lr.positionCount = 2;
            }
        }
    }

    void DrawRope()
    {
        //if not grappling dont draw rope
        if (!joint) return;
        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, grapplePoint);
    }

    public void StopGrapple(InputAction.CallbackContext context)
    {
        lr.positionCount = 0;
        Destroy(joint);
    }

    public bool IsGrappling()
    {
        return joint != null;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
}
