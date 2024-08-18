using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerMovementController : MonoBehaviour
{
    public float speed = 5f;
    [SerializeField, Range(0f,1f)] float speedHold;
    public float rotationSpeed = 700f;

    public UnityEvent<bool> stepEvent;
    private bool isWalking;
    private bool isSoft;
    private float walkingTime;
    [SerializeField] private float timeToStepEvent;


    private PicakbleObject objectToPick;


    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float currentSpeed = speed;
       
        if (Input.GetButton("Jump"))
        {
            currentSpeed = speed * speedHold;
            isSoft = true;
        }
        else
        {
            isSoft = false;
        }

        Vector3 movement = new Vector3(horizontal, 0.0f, vertical);
        transform.Translate(movement * currentSpeed * Time.deltaTime, Space.World);
        if (movement != Vector3.zero)
        {
            isWalking = true;
            walkingTime += Time.deltaTime;
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            if (walkingTime >= timeToStepEvent)
            {
                stepEvent?.Invoke(isSoft);
                walkingTime = 0f;
            }
        }
        else
        {
            isWalking = false;
            walkingTime = 0f;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            if(objectToPick!= null) objectToPick.PickObject();
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent(out PicakbleObject pickable))
        {
            objectToPick = pickable;
        }

        if (other.transform.tag == "Finish")
        {
            TransitionManager.Instance.LoadScene("Crafting",true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.TryGetComponent(out PicakbleObject pickable))
        {
            objectToPick = null;
        }
    }
    
}
