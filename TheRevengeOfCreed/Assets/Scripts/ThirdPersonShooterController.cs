using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class ThirdPersonShooterController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera aimCamera;
    [SerializeField] private float regularSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderMask;

    [SerializeField] private Transform debugTransform;
    [SerializeField] private Transform projectliePrefabTransform;
    [SerializeField] private Transform bulletSpawnTransform;
    [SerializeField] private Transform bulletSpawnRunningTransform;

    private StarterAssetsInputs starterAssetsInputs;
    private ThirdPersonController tpController;
    private PauseMenu pauseMenu;
    private PlayerUI playerUI;
    private Animator animator;

    [SerializeField] private AudioClip audioSource;

    public GameObject muzzleFlash;

    private bool pistolRunning;
    private Transform savedSpawnPoint;

    private void Awake()
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        tpController = GetComponent<ThirdPersonController>();
        playerUI = GetComponent<PlayerUI>();
        pauseMenu = GetComponent<PauseMenu>();
        animator = GetComponent<Animator>();
        //savedSpawnPoint = bulletSpawnTransform;
        pistolRunning = false;
    }

    void Update()
    {
        if(playerUI.GetHealth() > 0f )
        {
            Vector3 mouseWorldPosition = Vector3.zero;
            mouseWorldPosition = castRayForShooting();
            //check for Camera Switch

            CameraSwitch(mouseWorldPosition);
            Shoot(mouseWorldPosition);
            
        }
        else
        {
            animator.SetBool("DeathTriggered", true);
            StartCoroutine(waitForDeath());
        }
        Debug.Log(playerUI.GetHealth());
        CheckPause();

    }

    public void CheckPause()
    {
        if (starterAssetsInputs.pause)
        {
            pauseMenu.Pause();
        }
        else
        {
            if (pauseMenu.paused)
            {
                pauseMenu.Resume();
            }
            else
            {
                Debug.Log("UnPaused");
            }
        }
    }

    private void Shoot(Vector3 mouseWorldPosition)
    {
        if(starterAssetsInputs.shoot && starterAssetsInputs.aim)
        {
            muzzleFlash.SetActive(true);
            StartCoroutine(wait());
            if(pistolRunning)
            {
                Vector3 aimDir = (mouseWorldPosition - bulletSpawnRunningTransform.position).normalized;
                Instantiate(projectliePrefabTransform, bulletSpawnRunningTransform.position, Quaternion.LookRotation(aimDir, Vector3.up));
            }
            else
            {
                Vector3 aimDir = (mouseWorldPosition - bulletSpawnTransform.position).normalized;
                Instantiate(projectliePrefabTransform, bulletSpawnTransform.position, Quaternion.LookRotation(aimDir, Vector3.up));
            }      
            starterAssetsInputs.shoot = false;
            AudioSource.PlayClipAtPoint(audioSource, transform.position, 1);
            CameraShake.Instance.ShakeCamera(2.5f, 0.1f);
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
                pistolRunning = false;
                
            }
            else if (mySpeed > 6)
            {
                Debug.Log("Running");
                animator.SetBool("pistolWalking", false);
                animator.SetBool("pistolRunning", true);
                pistolRunning = true;
                //bulletSpawnTransform.position.x = 
            }
            else
            {
                Debug.Log("Aiming but not moving");
                animator.SetBool("pistolWalking", false);
                animator.SetBool("pistolRunning", false);
                pistolRunning = false;
            }
            
        }
        else
        {
            pistolRunning = false;
            Debug.Log("Not Moving, not aiming");
            animator.SetBool("pistolWalking", false);
            animator.SetBool("pistolRunning", false);
            aimCamera.gameObject.SetActive(false);
            tpController.SetSensitivity(regularSensitivity);
            tpController.SetOnMoveRotate(true);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
        }
    }

    IEnumerator wait()
    {

        yield return new WaitForSeconds(0.1f);
        muzzleFlash.SetActive(false);

    }

    IEnumerator waitForDeath()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //Destroy(gameObject);
/*        aimCamera.Follow = null;
        aimCamera.LookAt = gameObject;
*/   
    
    }

}
