using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonDash : MonoBehaviour
{
    public bool isDashing;

    private int dashAttempts;
    private float dashStartTime;

    MovementInput playerController;
    CharacterController characterController;

    void Start()
    {
        playerController.GetComponent<MovementInput>();
    }

    void Update()
    {
        HandleDash();
    }

    void HandleDash() 
    {
        bool isTryingtoDash = Input.GetKeyDown(KeyCode.LeftShift);

        if(isTryingtoDash && !isDashing)
        {
            if(dashAttempts <= 2)
            {
                OnStartDash();
            }

            if(Time.time - dashStartTime < 0.4f)
            {
                if (playerController.moveVector.Equals(Vector3.zero))
                {
                    characterController.Move(transform.forward * 30f * Time.deltaTime);
                }
                else
                {
                    characterController.Move(playerController.moveVector.normalized * 30f * Time.deltaTime);
                }
            }
            else{
                OnEndDash();
            }
        }
    }

    void OnStartDash()
    {

    }

    void OnEndDash()
    {

    }
}
