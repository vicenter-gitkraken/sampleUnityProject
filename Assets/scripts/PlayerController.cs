using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public GameObject playerModel;
    public Transform camera;
    public float rightMultiplier = 1;
    public float forwardMultiplier = 1;
    public float walkSpeed = 4;
    public float runSpeed = 8;

    public float cameraX = 1;
    public float cameraY = 1;
    public float cameraSensitivity = 25;

	// Use this for initialization
	void Start () {
        playerModel = gameObject;
        camera = transform.Find("CameraHolder");
	}
	
	// Update is called once per frame
	void Update () {

        MoveCharacter();
        MoveCamera();
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("main fire!");
            Transform hand = camera.Find("RightHand");
            ActivateHand(hand);
        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("alt fire!");
            Transform hand = camera.Find("LeftHand");
            ActivateHand(hand);
        }
	}
    
    private void ActivateHand(Transform handTrans) {
        if (handTrans != null)
        {
            HandMovement hand = handTrans.gameObject.GetComponent<HandMovement>();
            hand.Swing();
        }
    }

    private void MoveCharacter()
    {
        float right = Input.GetAxis("Horizontal") * rightMultiplier;
        float forward = Input.GetAxis("Vertical") * forwardMultiplier;
        Vector3 move = new Vector3(right, 0, forward) * Time.deltaTime * walkSpeed;
        //playerModel.GetComponent<Rigidbody>().AddForce(move);
        playerModel.transform.Translate(move);
    }

    private void MoveCamera()
    {
        float rotY = Input.GetAxis("Mouse X") * cameraX* Time.deltaTime * cameraSensitivity;
        float rotX = Input.GetAxis("Mouse Y") * cameraY* Time.deltaTime * cameraSensitivity;
        playerModel.transform.Rotate(new Vector3(0, rotY, 0));

        camera.Rotate(new Vector3(-rotX, 0, 0));
    }
}
