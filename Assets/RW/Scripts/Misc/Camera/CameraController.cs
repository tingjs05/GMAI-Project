using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ThirdPersonCamera))]
public class CameraController : MonoBehaviour
{
    // inspector variables
    [SerializeField] float newCamRotSpeed = 15f;
    [SerializeField] CameraType newCameraType = CameraType.Follow_Independent;
    [SerializeField, Range(0, 2)] int mouseButton = 1;

    // hidden variables
    ThirdPersonCamera tpc;
    // cache original values before change
    CameraType originalCameraType;
    float originalRotSpeed;

    // Start is called before the first frame update
    void Start()
    {
        // get reference to camera script
        tpc = GetComponent<ThirdPersonCamera>();
        // set original values
        originalCameraType = tpc.mCameraType;
        originalRotSpeed = tpc.mRotationSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // apply changes
        if (Input.GetMouseButton(mouseButton))
        {
            // set cursor
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            // set camera
            tpc.mCameraType = newCameraType;
            tpc.mRotationSpeed = newCamRotSpeed;
            return;
        }

        // revert changes
        // set cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        // set camera
        tpc.mCameraType = originalCameraType;
        tpc.mRotationSpeed = originalRotSpeed;
    }
}
