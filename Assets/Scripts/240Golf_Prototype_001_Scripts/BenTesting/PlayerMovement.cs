﻿using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    Animator animator;
    CharacterController characterController;

    [System.Serializable]
    public class AnimationSettings {
        public string verticalVelocityFloat = "Forward";
        public string horizontalVelocityFloat = "Strafe";
        public string groundedBool = "isGrounded";
        public string jumpBool = "isJumping";
    }
    [SerializeField]
    public AnimationSettings animations;

    [System.Serializable]
    public class PhysicsSettings {
        public float gravityModifier = 9.81f;
        public float baseGravity = 50.0f;
        public float resetGravityValue = 1.2f;
    }
    [SerializeField]
    public PhysicsSettings physics;

    [System.Serializable]
    public class MovementSettings {
        public float jumpSpeed = 6;
        public float jumpTime = 0.25f;
    }
    [SerializeField]
    public MovementSettings movement;

    bool isGrounded = true;
    bool isJumping;
    bool resetGravity;
    float gravity;


    void Start() {
        // no need to check null as these are required components
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        SetupAnimator();
    }

    void Update() {
        ApplyGravity();
        isGrounded = characterController.isGrounded;
    }

    private void ApplyGravity()
    {
        if (!isGrounded) {
            if (!resetGravity) {
                gravity = physics.resetGravityValue;
                resetGravity = true;
            }
            gravity += Time.deltaTime * physics.gravityModifier;
        } else {
            gravity = physics.baseGravity;
            resetGravity = false;
        }

        Vector3 gravityVector = new Vector3();
        if (!isJumping) {
            gravityVector.y -= gravity;
        } else {
            gravityVector.y = movement.jumpSpeed;
        }
        characterController.Move(gravityVector * Time.deltaTime);
    }

    // Animates the character and root motion handles the movement
    public void Animate(float forward, float strafe) {
        animator.SetFloat(animations.verticalVelocityFloat, forward);
        animator.SetFloat(animations.horizontalVelocityFloat, strafe);
        animator.SetBool(animations.groundedBool, isGrounded);
        animator.SetBool(animations.jumpBool, isJumping);
    }

    public void Jump() {
        if (isJumping) {
            return;
        }

        if (isGrounded) {
            isJumping = true;
            StartCoroutine(StopJump());
        }
    }

    IEnumerator StopJump() {
        yield return new WaitForSeconds(movement.jumpTime);
        isJumping = false;
    }

    // Setup animator with child avatar
    void SetupAnimator()
    {
        Animator wantedAnim = GetComponentsInChildren<Animator>()[1];
        Avatar wantedAvatar = wantedAnim.avatar;
        animator.avatar = wantedAvatar;
        Destroy(wantedAnim);
    }
}



