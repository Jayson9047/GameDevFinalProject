using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;


public class ThirdPersonShooterController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera aimCamera;
    [SerializeField] private float regularSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderMask;

    [SerializeField] private Transform debugTransform;
    [SerializeField] private Transform projectliePrefabTransform;
    [SerializeField] private Transform bulletSpawnTransform;

    private StarterAssetsInputs starterAssetsInputs;
    private ThirdPersonController tpController;
    private Animator animator;

    private void Awake()
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        tpController = GetComponent<ThirdPersonController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {

        Vector3 mouseWorldPosition = Vector3.zero;
        mouseWorldPosition = castRayForShooting();
        //check for Camera Switch

        CameraSwitch(mouseWorldPosition);
        if (starterAssetsInputs.shoot)
        {
            Vector3 aimDir = (mouseWorldPosition - bulletSpawnTransform.position).normalized;
            Instantiate(projectliePrefabTransform, bulletSpawnTransform.position, Quaternion.LookRotation(aimDir, Vector3.up));
            starterAssetsInputs.shoot = false;
        }
    }

    private void Shoot(Vector3 mouseWorldPosition)
    {
        if(starterAssetsInputs.shoot)
        {
            Vector3 aimDir = (mouseWorldPosition - bulletSpawnTransform.position).normalized;
            Instantiate(projectliePrefabTransform, bulletSpawnTransform.position, Quaternion.LookRotation(aimDir,Vector3.up));
            starterAssetsInputs.shoot = false;
        }
    }

    private Vector3 castRayForShooting()
    {

        Vector3 mouseWorldPosition = Vector3.zero;
        //get the middle point of the screen
        Vector2 centerPointOfScreen = new Vector2(Screen.width / 2f, Screen.height / 2f);

        //cast ray to the middle point of the screen to determine where the projetile should go
        Ray ray = Camera.main.ScreenPointToRay(centerPointOfScreen);

        //if ray cast hit anything get the hit point/position. And change transform.
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderMask))
        {
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
        }
        return mouseWorldPosition;
    }



    private void CameraSwitch(Vector3 mousePositionInWorld)
    {
        if (starterAssetsInputs.aim)
        {
            aimCamera.gameObject.SetActive(true);
            tpController.SetSensitivity(aimSensitivity);
            tpController.SetOnMoveRotate(false);

            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));
            Vector3 worldAimTarget = mousePositionInWorld;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);

            int mySpeed = Mathf.RoundToInt(animator.GetFloat("Speed"));
            
            //Debug.Log(tpController.GetSpeed());
            if (mySpeed > 0 && mySpeed < 7)
            {
                Debug.Log(mySpeed);
                
                animator.SetBool("pistolWalking", true);
                animator.SetBool("pistolRunning", false);
                
            }
            else if (mySpeed > 6)
            {
                Debug.Log("Running");
                animator.SetBool("pistolWalking", false);
                animator.SetBool("pistolRunning", true);
            }
            else
            {
                Debug.Log("Aiming but not moving");
                animator.SetBool("pistolWalking", false);
                animator.SetBool("pistolRunning", false);
            }
            
        }
        else
        {
            Debug.Log("Not Moving, not aiming");
            animator.SetBool("pistolWalking", false);
            animator.SetBool("pistolRunning", false);
            aimCamera.gameObject.SetActive(false);
            tpController.SetSensitivity(regularSensitivity);
            tpController.SetOnMoveRotate(true);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
        }
    }

}
