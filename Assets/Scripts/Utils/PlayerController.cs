using UnityEngine;

namespace VoxelEngine.Utils
{
    public class PlayerController : MonoBehaviour
    {
        private CharacterController _characterController;

        [Header("REFERENCES")]
        public new Transform camera;
        public Transform arm;

        [Header("SETTINGS")]
        public float gravityStrength = 20f;
        public float jumpStrength = 8f;
        public float walkSpeed = 5f;
        public float runSpeed = 10f;
        public float lerpSpeed = 10f;
        public float mouseSensitivity = 0.2f;
        public Vector3 defaultArmPosition = new Vector3(1, -0.5f, 0.1f);

        [Header("STATE")]
        public Vector3 velocity;
        public float camRot = 0;
        public bool creativeMode = false;

        public void OnEnable()
        {
            _characterController = GetComponent<CharacterController>();
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void Update()
        {
            float deltaTime = Time.deltaTime;

            #region Movement
            float inputX = Input.GetAxisRaw("Horizontal");
            float inputZ = Input.GetAxisRaw("Vertical");

            Vector3 result = transform.forward * inputZ + transform.right * inputX;
            float moveSpeed = Input.GetKey(KeyCode.LeftControl) ? runSpeed : walkSpeed;
            result *= moveSpeed;

            velocity.x = Mathf.Lerp(velocity.x, result.x, lerpSpeed * deltaTime);
            velocity.z = Mathf.Lerp(velocity.z, result.z, lerpSpeed * deltaTime);
            #endregion

            #region Gravity
            if(_characterController.isGrounded)
                velocity.y = Mathf.Max(velocity.y, 0); 
            else
                velocity.y -= gravityStrength * deltaTime;

            #endregion

            #region Jump
            if (_characterController.isGrounded && velocity.y >= -1f && Input.GetKey(KeyCode.Space))
                velocity.y = jumpStrength;
            #endregion

            #region Camera
            transform.eulerAngles += new Vector3(0, Input.GetAxis("Mouse X") * mouseSensitivity * 90f, 0);
            camRot -= Input.GetAxis("Mouse Y") * mouseSensitivity * 90f;
            camRot = Mathf.Clamp(camRot, -90f, 90f);

            camera.localEulerAngles = new Vector3(camRot, 0, 0);
            #endregion

            #region Creative Mode
            if(Input.GetKeyDown(KeyCode.C))
                creativeMode = !creativeMode;

            if(creativeMode)
            {
                velocity.y = Input.GetKey(KeyCode.Space) ? moveSpeed : (Input.GetKey(KeyCode.LeftShift) ? -moveSpeed : 0);
            }
            #endregion

            #region Apply Velocity
            _characterController.Move(velocity * deltaTime);
            #endregion

            #region Arm
            arm.transform.localPosition = Vector3.Lerp(
                arm.transform.localPosition,
                result.magnitude > 0.1f ?
                defaultArmPosition + new Vector3(0,  Mathf.Sin(Time.time * Mathf.PI * 4f) * 0.1f, 0) :
                defaultArmPosition,
                deltaTime * 5f
            );
            #endregion
        }
    }
}