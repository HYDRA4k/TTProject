using System.Collections;
using System.Collections.Generic;
using TTProject.Manager;
using UnityEngine;

namespace TTProject.PlayerControl
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float AnimBlendSpeed = 8.9f;
        [SerializeField] private Transform cameraRoot;
        [SerializeField] private Transform mainCamera;
        [SerializeField] private float UpperLimit = -40f;
        [SerializeField] private float BottomLimit = 70f;
        [SerializeField] private float MouseSensitivity = 21.9f;

        private Rigidbody playerRigidbody;
        private InputManager inputManager;
        private Animator animator;
        private bool hasAnimator;
        private int _xVelHash;
        private int _yVelHash;
        private float xRotation;

        private const float walkSpeed = 2f;
        private const float runSpeed = 6f;
        private Vector2 currentVelocity;

        void Start()
        {
            hasAnimator = TryGetComponent<Animator>(out animator);
            playerRigidbody = GetComponent<Rigidbody>();
            inputManager = GetComponent<InputManager>();

            _xVelHash = Animator.StringToHash("X_Velocity");
            _yVelHash = Animator.StringToHash("Y_Velocity");

            // Lock Cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void FixedUpdate()
        {
            Move();
        }
        
        private void LateUpdate()
        {
            CamMovements();
        }
        
        private void Move()
        {
            if (!hasAnimator) return;

            float targetSpeed = inputManager.Run ? runSpeed : walkSpeed;
            if(inputManager.Move == Vector2.zero) targetSpeed = 0.1f;

            currentVelocity.x = Mathf.Lerp(currentVelocity.x, inputManager.Move.x * targetSpeed, AnimBlendSpeed * Time.deltaTime);
            currentVelocity.y = Mathf.Lerp(currentVelocity.y, inputManager.Move.y * targetSpeed, AnimBlendSpeed * Time.deltaTime);

            var xVelDifference = currentVelocity.x - playerRigidbody.velocity.x;
            var zVelDifference = currentVelocity.y - playerRigidbody.velocity.z;

            playerRigidbody.AddForce(transform.TransformVector(new Vector3(xVelDifference, 0, zVelDifference)), ForceMode.VelocityChange);

            animator.SetFloat(_xVelHash, currentVelocity.x);
            animator.SetFloat(_yVelHash, currentVelocity.y);
        }

        // Camera Movement
        private void CamMovements()
        {
            if (!hasAnimator) return;

            var Mouse_X = inputManager.Look.x;
            //var Mouse_Y = inputManager.Look.y;
            //mainCamera.position = cameraRoot.position;

            //xRotation -= Mouse_Y * MouseSensitivity * Time.deltaTime;
            xRotation = Mathf.Clamp(xRotation, UpperLimit, BottomLimit);

            mainCamera.localRotation = Quaternion.Euler(xRotation, 0, 0);
            transform.Rotate(Vector3.up, Mouse_X * MouseSensitivity * Time.deltaTime);
        }
    }
}
