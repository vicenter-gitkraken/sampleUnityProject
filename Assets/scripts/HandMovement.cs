using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMovement : MonoBehaviour {

    private enum HandState
    {
        Start,
        Adjusting,
        FinishedAdjusting,
        Reversing,
        Swinging
    }

    public float intervalMax = 30f;
    public float intervalMin = 10f;

    public float durationMax = 1;
    public float durationMin = .5f;

    public float xRotMax = 10;
    public float xRotMin = 0;

    public float yRotMax = 20;
    public float yRotMin = 10;

    public float zRotMax = 10;
    public float zRotMin = 0;

    public float swingRotSpeed = -360;
    public float swingTransSpeed = 5f;
    public float swingDuration = .33f;

    private Vector3 startPosition;
    private Vector3 startRotation;
    private HandState state;
    private bool shiftBack = false;
    private Vector3 adjustRotation;
    private float rotationDuration = 2;

    private float countDown = 0;

	// Use this for initialization
	void Start () {
        state = HandState.Start;
        startPosition = transform.position;
        startRotation = transform.rotation.eulerAngles;
        Debug.Log("StartPosition: " + startPosition);
	}

    public void Swing()
    {
        ResetHand();
        state = HandState.Swinging;
        countDown = swingDuration;
    }

    private void HandleSwinging() {
        countDown -= Time.deltaTime;
        if (countDown <= 0)
        {
            ResetHand();
            state = HandState.Start;
            countDown = Random.Range(intervalMin, intervalMax);
            return;
        }
        float swingRot = swingRotSpeed * Time.deltaTime;
        transform.Rotate(new Vector3(0, swingRot, 0));

    }

    private void SetNextAdjust()
    {
        countDown = Random.Range(intervalMin, intervalMax);
    }
	
	// Update is called once per frame
	void Update () {
        if (state == HandState.Start) {
            HandleStartState();
        } else if (state == HandState.Adjusting) {
            HandleAdjustingState();
        } else if (state == HandState.FinishedAdjusting) {
            HandleAdjustFinishedState();
        } else if (state == HandState.Reversing) {
            HandleReversingState();
        } else if (state == HandState.Swinging) {
            HandleSwinging();
        }
	}

    private void HandleStartState()
    {
        countDown -= Time.deltaTime;
        if (countDown <= 0)
        {
            StartAdjustingHand();
            countDown = Random.Range(durationMin, durationMax);
            NextState();
            rotationDuration = countDown;
        }
    }

    private void HandleAdjustingState()
    {
        float rotationTime = Time.deltaTime;
        if (Time.deltaTime >= countDown)
        {
            rotationTime = countDown;
        }
        transform.Rotate(adjustRotation * rotationTime / rotationDuration);
        countDown -= Time.deltaTime;
        if (countDown <= 0)
        {
            countDown = Random.Range(durationMin, durationMax);
            NextState();
        }
    }

    public void ResetHand()
    {
        Vector3 rot = transform.parent.rotation.eulerAngles + startRotation;
        transform.rotation = Quaternion.Euler(rot);
        // transform.position = startPosition;
        Debug.Log("new Pos: " + transform.position);
    }

    private void HandleAdjustFinishedState()
    {
        countDown -= Time.deltaTime;
        if (countDown <= 0)
        {
            countDown = Random.Range(durationMin, durationMax);
            rotationDuration = countDown;
            NextState();
        }
    }

    private void HandleReversingState()
    {
        float rotationTime = Time.deltaTime;
        if (Time.deltaTime >= countDown)
        {
            rotationTime = countDown;
        }
        transform.Rotate(-adjustRotation * rotationTime / rotationDuration);
        countDown -= Time.deltaTime;
        if (countDown <= 0)
        {
            countDown = Random.Range(intervalMin, intervalMax);
            NextState();
        }
    }

    private void StartAdjustingHand()
    {
        float rotX = Random.Range(xRotMin, xRotMax);
        float rotY = Random.Range(yRotMin, yRotMax);
        float rotZ = Random.Range(zRotMin, zRotMax);
        adjustRotation = new Vector3(rotX, rotY, rotZ);
    }

    private float NextState()
    {
        if (state == HandState.Start) {
            state = HandState.Adjusting;
        } else if (state == HandState.Adjusting) {
            state = HandState.FinishedAdjusting;
        } else if (state == HandState.FinishedAdjusting) {
            state = HandState.Reversing;
        } else if (state == HandState.Reversing) {
            state = HandState.Start;
        }
        Debug.Log("Next State: " + this.state);
        return countDown;
    }
}
