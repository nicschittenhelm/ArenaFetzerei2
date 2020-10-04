using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] public Transform debugHitPointTransform;
    [SerializeField] public Transform hookshotTransform;
    public float mouseSensitivityX = 1.5f;
    public float mouseSensitivityY = 1.5f;

    private CharacterController characterController;
    private float cameraVerticalAngle;
    private float characterVelocityY;
    private Camera playerCamera;
    private State state;
    private Vector3 hookshotPosition;
    private float hookshotSize;

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private enum State {
        Normal,
        HookshotFlyingPlayer,
        HookshotTrown
    }


    private void Awake() {
        characterController = GetComponent<CharacterController>();
        playerCamera = transform.Find("fpsCamera").GetComponent<Camera>();
        state = State.Normal;
        hookshotTransform.gameObject.SetActive(false); // DAMIT SEIL ANFANGS NICHT SICHBAR IST
    }

    private void Update() {
        switch (state) {
        default:
        case State.Normal: // WENN STATE NORMAL IST, DANN ALLES WIE ÜBLICH
            HandleCharacterCamera();
            HandleCharacterMovement();
            HandleHookshotStart();
            break;
        case State.HookshotTrown:
            HandleCharacterCamera();
            HandleHookshotThrow();
            break;
        case State.HookshotFlyingPlayer: // WENN STATE HOOKSHOTFLYING PLAYER, DANN WIRD HOOKSHOTFLYINGPLAYER AUFGERUFEN
            HandleCharacterCamera();
            HookshotMovement();
            break;
        }
    }

    private void HandleCharacterCamera() {
        float lookX = Input.GetAxisRaw("Mouse X");
        float lookY = Input.GetAxisRaw("Mouse Y");

        transform.Rotate(new Vector3(0f, lookX * mouseSensitivityX, 0f), Space.Self);

        cameraVerticalAngle -= lookY * mouseSensitivityY;

        cameraVerticalAngle = Mathf.Clamp(cameraVerticalAngle, -89f, 89f);

        playerCamera.transform.localEulerAngles = new Vector3(cameraVerticalAngle, 0, 0);
    }

    private void HandleCharacterMovement() {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        float moveSpeed = 12f;

        Vector3 characterVelocity = transform.right * moveX * moveSpeed + transform.forward * moveZ * moveSpeed;

        if (characterController.isGrounded) {
            characterVelocityY = 0f;
            //JUMP
            if (Input.GetKeyDown(KeyCode.Space)) {
                float jumpSpeed = 15f;
                characterVelocityY = jumpSpeed;
            }
        }

        float gravityDownForce = -50f;
        characterVelocityY += gravityDownForce * Time.deltaTime;

        characterVelocity.y = characterVelocityY;

        characterController.Move(characterVelocity * Time.deltaTime);
    }

    private void HandleHookshotStart() {
        if (Input.GetKeyDown(KeyCode.E)) {
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit raycastHit)) {
                // HIT SOMETHING
                debugHitPointTransform.position = raycastHit.point;
                hookshotPosition = raycastHit.point; // WENN MAN ETWAS TRIFFT, NEUE POSITION - DORT WILL MAN SICH HINZIEHEN
                //hookshotSize = 0f;
                hookshotTransform.gameObject.SetActive(true);
                state = State.HookshotTrown; // UND STATE AENDERN DAMIT HOOKSHOTMOVEMENT AUFGERUFEN WERDEN KANN
            }
        }
    }

    private void HandleHookshotThrow(){
        hookshotTransform.LookAt(hookshotPosition);
        
        float hookshotThrowSpeed = 100f;
        hookshotSize += hookshotThrowSpeed * Time.deltaTime;
        hookshotTransform.localScale = new Vector3(1,1,hookshotSize);

        if (hookshotSize >= Vector3.Distance(transform.position, hookshotPosition)){
            state = State.HookshotFlyingPlayer;
        }
    }
    private void HookshotMovement() { // CHARACTER MOVEMENT WENN HOOKSHOT AUF ETWAS TRIFFT, SIEHE HANDLEHOOKSHOT RAYCASTHIT.POINT
        hookshotTransform.LookAt(hookshotPosition);

        Vector3 hookshotDirection = (hookshotPosition - transform.position/*CURRENT POSITION*/).normalized;

        float hookshotSpeedMin = 10f;
        float hookshotSpeedMax = 30f;
        float hookshotSpeed = Mathf.Clamp(Vector3.Distance(transform.position, hookshotPosition), hookshotSpeedMin, hookshotSpeedMax);

        characterController.Move(hookshotDirection * hookshotSpeed * 2f * Time.deltaTime);

        // ENDE ERREICH, HOOKSHOT CANCEL
        if (Vector3.Distance(transform.position, hookshotPosition) < 9f){
            hookshotTransform.gameObject.SetActive(false);
            state = State.Normal;
        }

        if(Input.GetKeyDown(KeyCode.E)) {
            hookshotTransform.gameObject.SetActive(false);
            state = State.Normal;
        }
    }

}