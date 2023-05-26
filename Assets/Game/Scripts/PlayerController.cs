using System.Collections;
using System.Collections.Generic;
using TTProject.Manager;
using UnityEngine;

namespace TTProject.PlayerControl
{
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody playerRigidbody;
        private InputManager inputManager;
        private Animator animator;
        private bool hasAnimator;
        private int _xVelHash;
        private int _yVelHash;

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
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            if (!hasAnimator) return;

            float targetSpeed = inputManager.Run ? runSpeed : walkSpeed;
            if(inputManager.Move == Vector2.zero) targetSpeed = 0.1f;

            currentVelocity.x = targetSpeed * inputManager.Move.x;
            currentVelocity.y = targetSpeed * inputManager.Move.y;

            var xVelDifference = currentVelocity.x - playerRigidbody.velocity.x;
            var zVelDifference = currentVelocity.y - playerRigidbody.velocity.z;

            playerRigidbody.AddForce(transform.TransformVector(new Vector3(xVelDifference, 0, zVelDifference)), ForceMode.VelocityChange);

            animator.SetFloat(_xVelHash, currentVelocity.x);
            animator.SetFloat(_yVelHash, currentVelocity.y);
        }
    }
}
