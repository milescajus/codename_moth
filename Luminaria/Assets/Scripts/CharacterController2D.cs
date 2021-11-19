using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using UnityEngine.InputSystem;

public enum GroundType
{
    None,
    Soft,
    Hard
}

public class CharacterController2D : MonoBehaviour
{
    static readonly float charScale = 0.3f;
    readonly Vector3 flippedScale = new Vector3(-1 * charScale, charScale, charScale);
    readonly Quaternion flippedRotation = new Quaternion(0, 0, 1, 0);

    [Header("Character")]
    [SerializeField] Animator animator = null;
    [SerializeField] Transform puppet = null;
    [SerializeField] CharacterAudio audioPlayer = null;

    [Header("Equipment")]
    [SerializeField] Transform handAnchor = null;
    [SerializeField] UnityEngine.U2D.Animation.SpriteLibrary spriteLibrary = null;

    [Header("Movement")]
    [SerializeField] float acceleration = 4.0f;
    [SerializeField] float maxSpeed = 0.0f;
    [SerializeField] float jumpForce = 1.0f;
    [SerializeField] float minFlipSpeed = 0.1f;
    [SerializeField] float jumpGravityScale = 3.0f;
    [SerializeField] float fallGravityScale = 5.0f;
    [SerializeField] float groundedGravityScale = 2.0f;
    [SerializeField] bool resetSpeedOnLand = false;
    [SerializeField] UnityEvent test;

    private Rigidbody2D controllerRigidbody;
    private CapsuleCollider2D controllerCollider;
    private float colliderOffset = .4f;
    private LayerMask softGroundMask;
    private LayerMask hardGroundMask;

    private Vector2 movementInput;
    private bool jumpInput;

    public AnimatorOverrideController AspenLeft;
    public AnimatorOverrideController AspenRight;
    public bool isBurning = false;
    public bool faceRight = false;

    private Vector2 prevVelocity;
    private GroundType groundType;
    private bool isJumping;
    private bool isFalling;
    private bool isGliding;
    private bool doubleJump = false;
    private bool hasTransitioned;

    private int animatorGroundedBool;
    private int animatorRunningSpeed;
    private int animatorJumpTrigger;
    private int animatorBurnTrigger;
    private int animatorBurningBool;

    public int chargeLevel = 0;

    public bool CanMove { get; set; }

    void Start()
    {
        StartCoroutine(UpdateStaffCharge());
#if UNITY_EDITOR
        if (Keyboard.current == null)
        {
            var playerSettings = new UnityEditor.SerializedObject(Resources.FindObjectsOfTypeAll<UnityEditor.PlayerSettings>()[0]);
            var newInputSystemProperty = playerSettings.FindProperty("enableNativePlatformBackendsForNewInputSystem");
            bool newInputSystemEnabled = newInputSystemProperty != null ? newInputSystemProperty.boolValue : false;

            if (newInputSystemEnabled)
            {
                var msg = "New Input System backend is enabled but it requires you to restart Unity, otherwise the player controls won't work. Do you want to restart now?";
                if (UnityEditor.EditorUtility.DisplayDialog("Warning", msg, "Yes", "No"))
                {
                    UnityEditor.EditorApplication.ExitPlaymode();
                    var dataPath = Application.dataPath;
                    var projectPath = dataPath.Substring(0, dataPath.Length - 7);
                    UnityEditor.EditorApplication.OpenProject(projectPath);
                }
            }
        }
#endif

        controllerRigidbody = GetComponent<Rigidbody2D>();
        controllerCollider = GetComponent<CapsuleCollider2D>();
        softGroundMask = LayerMask.GetMask("Ground Soft");
        hardGroundMask = LayerMask.GetMask("Ground Hard");

        animatorGroundedBool = Animator.StringToHash("Grounded");
        animatorRunningSpeed = Animator.StringToHash("RunningSpeed");
        animatorJumpTrigger = Animator.StringToHash("Jump");
        animatorBurnTrigger = Animator.StringToHash("Burn");
        animatorBurningBool = Animator.StringToHash("Burning");

        CanMove = true;
    }

    void Update()
    {
        var keyboard = Keyboard.current;

        if (!CanMove || keyboard == null)
            return;

        // Horizontal movement
        float moveHorizontal = 0.0f;

        if (!keyboard.shiftKey.isPressed)
        {
            isBurning = false;
            hasTransitioned = false;

            if (keyboard.leftArrowKey.isPressed || keyboard.aKey.isPressed)
                moveHorizontal = -1.0f;
            else if (keyboard.rightArrowKey.isPressed || keyboard.dKey.isPressed)
                moveHorizontal = 1.0f;

        } else if (!isFalling) {
            // only allow burning while falling and not moving
            if (!hasTransitioned) {
                animator.SetTrigger(animatorBurnTrigger);
                hasTransitioned = true;
            }
            isBurning = true;
        }

        movementInput = new Vector2(moveHorizontal, 0);

        // Jumping input
        if (keyboard.spaceKey.wasPressedThisFrame)
        {
            if (!isJumping)
                jumpInput = true;
            else if (!doubleJump)
            {
                jumpInput = true;
                doubleJump = true;
            }
        }

        // Gliding
        if (keyboard.spaceKey.isPressed && isFalling)
        {
            isGliding = true;
        } else {
            isGliding = false;
        }

        // Debug Charging
        if (keyboard.cKey.wasPressedThisFrame)
            chargeLevel = 3;
    }

    void FixedUpdate()
    {
        UpdateGrounding();
        UpdateVelocity();
        UpdateAnimation();
        UpdateJump();
        UpdateGravityScale();

        prevVelocity = controllerRigidbody.velocity;
    }

    private IEnumerator UpdateStaffCharge()
    {
        while(true) {
            yield return new WaitForSeconds(1.0f);

            var levels = new HashSet<int> {2, 3, 4, 5};

            transform.GetChild(chargeLevel).gameObject.SetActive(true);
            levels.Remove(chargeLevel);

            foreach (int n in levels) {
                transform.GetChild(n).gameObject.SetActive(false);
            }
        }
    }

    private void UpdateGrounding()
    {
        // Use character collider to check if touching ground layers
        if (controllerCollider.IsTouchingLayers(softGroundMask))
            groundType = GroundType.Soft;
        else if (controllerCollider.IsTouchingLayers(hardGroundMask))
            groundType = GroundType.Hard;
        else
            groundType = GroundType.None;

        // Update animator
        animator.SetBool(animatorGroundedBool, groundType != GroundType.None);
    }

    private void UpdateVelocity()
    {
        Vector2 velocity = controllerRigidbody.velocity;

        // Apply acceleration directly as we'll want to clamp
        // prior to assigning back to the body.
        velocity += movementInput * acceleration * Time.fixedDeltaTime;

        // We've consumed the movement, reset it.
        movementInput = Vector2.zero;

        // Clamp horizontal speed.
        velocity.x = Mathf.Clamp(velocity.x, -maxSpeed, maxSpeed);

        // Assign back to the body.
        controllerRigidbody.velocity = velocity;

        // Update animator running speed
        var horizontalSpeedNormalized = Mathf.Abs(velocity.x) / maxSpeed;
        animator.SetFloat(animatorRunningSpeed, horizontalSpeedNormalized);

        // Play audio
        audioPlayer.PlaySteps(groundType, horizontalSpeedNormalized);
    }

    private void UpdateJump()
    {
        // Set falling flag
        if (isJumping && controllerRigidbody.velocity.y < 0)
            isFalling = true;

        // Jump
        if (jumpInput)
        {
            // Set animator
            animator.SetTrigger(animatorJumpTrigger);

            // Jump using impulse force
            controllerRigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            // We've consumed the jump, reset it.
            jumpInput = false;

            // Set jumping flag
            isJumping = true;

            // Play audio
            audioPlayer.PlayJump();
        }

        // Landed
        else if (isJumping && isFalling && groundType != GroundType.None)
        {
            // Since collision with ground stops rigidbody, reset velocity
            if (resetSpeedOnLand)
            {
                prevVelocity.y = controllerRigidbody.velocity.y;
                controllerRigidbody.velocity = prevVelocity;
            }

            // Reset jumping flags
            isJumping = false;
            isFalling = false;
            doubleJump = false;

            // Play audio
            audioPlayer.PlayLanding(groundType);
        }
    }

    private void UpdateAnimation()
    {
        animator.SetBool(animatorBurningBool, isBurning);

        animator.runtimeAnimatorController = faceRight? AspenRight : AspenLeft;

        // Use animator overrides to flip character depending on direction
        if (controllerRigidbody.velocity.x > minFlipSpeed)
        {
            faceRight = true;
            controllerCollider.GetComponent<CapsuleCollider2D>().offset = new Vector2(colliderOffset, controllerCollider.offset.y);
        }
        else if (controllerRigidbody.velocity.x < -minFlipSpeed)
        {
            faceRight = false;
            controllerCollider.GetComponent<CapsuleCollider2D>().offset = new Vector2(-colliderOffset, controllerCollider.offset.y);
        }
    }


    private void UpdateGravityScale()
    {
        // Use grounded gravity scale by default.
        var gravityScale = groundedGravityScale;

        if (groundType == GroundType.None)
        {
            // If not grounded then set the gravity scale according to upwards (jump) or downwards (falling) motion.
            gravityScale = controllerRigidbody.velocity.y > 0.0f ? jumpGravityScale : fallGravityScale;           
        }

        controllerRigidbody.gravityScale = isGliding ? gravityScale / 3 : gravityScale;
    }

    public void GrabItem(Transform item)
    {
        // Attach item to hand
        item.SetParent(handAnchor, false);
        item.localPosition = Vector3.zero;
        item.localRotation = Quaternion.identity;
    }

    public void SwapSprites(UnityEngine.U2D.Animation.SpriteLibraryAsset spriteLibraryAsset)
    {
        spriteLibrary.spriteLibraryAsset = spriteLibraryAsset;
    }
}
