using System.Collections.Generic;
using UnityEngine;
using System.Collections;


public class CharacterControl : MonoBehaviour
{
    public GlobalValues.Seperate MovementSpeed;
    //moving forwards and backwards
    public float ForwardSpeed = 10;
    public float BackwardSpeed = 5;
    public float AerialLerpModifier = 2;
    public float MovementLerpSpeed = 0.5f;
    private bool _backPedaling;

    public GlobalValues.Seperate RotatingSpeed;
    //moving left and right turning left and right
    public float RotationSpeed = 0.05f;

    public GlobalValues.Seperate Gravity;
    public Vector3 GravityDrop = new Vector3(0, -0.3f, 0);
    public float MaximumGravity = 20;
    public bool IsGrounded;

    public GlobalValues.Seperate Jump;
    public float JumpSpeed = 10;
    public float ConsideredFloor = 0.5f;
    public float JumpLerpSpeed = 0.1f;
    private bool _jumpWasPressed;

    public Vector3 Velocity;

    //on the off hand we need to retain movment (air)
    private Vector3 _currentMovement;
    private Vector3 _currentGravity;
    private bool _hasJumped;

    public Animator anim;


    public bool turningLeft, turningRight, walkingForward, jump, strafingLeft, strafingRight;

   
    

    private void setWalkingForward()
    {
        anim.SetBool("Walking", true);
        anim.SetBool("StrafingRight", false);
        anim.SetBool("StrafingLeft", false);
        anim.SetBool("Attacking", false);
        anim.SetBool("Backwards", false);
        anim.SetBool("Jump", false);
    }
    private void setWalkingBackward()
    {
        anim.SetBool("Backwards", true);
        anim.SetBool("Walking", false);
        anim.SetBool("StrafingRight", false);
        anim.SetBool("StrafingLeft", false);
        anim.SetBool("Attacking", false);
        anim.SetBool("Jump", false);
    }
    private void setJumped()
    {
        anim.SetBool("Jump", true);
        anim.SetBool("Walking", false);
        anim.SetBool("StrafingRight", false);
        anim.SetBool("StrafingLeft", false);
        anim.SetBool("Attacking", false);
        anim.SetBool("Backwards", false);
    }
    private void setStrafingLeft()
    {
        anim.SetBool("Walking", false);
        anim.SetBool("StrafingRight", false);
        anim.SetBool("StrafingLeft",true);
        anim.SetBool("Attacking", false);
        anim.SetBool("Backwards", false);
        anim.SetBool("Jump", false);
    }
    private void setStrafingRight()
    {
        anim.SetBool("Walking",false);
        anim.SetBool("StrafingRight", true);
        anim.SetBool("StrafingLeft", false);
        anim.SetBool("Attacking", false);
        anim.SetBool("Backwards", false);
        anim.SetBool("Jump", false);
    }

    private void setIdle()
    {
        anim.SetBool("Walking", false);
        anim.SetBool("StrafingRight", false);
        anim.SetBool("StrafingLeft", false);
        anim.SetBool("Attacking", false);
        anim.SetBool("Backwards", false);
        anim.SetBool("Jump", false);
    }

   


    void Start()
    {
        GlobalValues.CharacterTransform = transform;
        GlobalValues.CharacterRigid = GetComponent<Rigidbody>();
        GlobalValues.CharacterRigid.freezeRotation = true;
        GlobalValues.CharacterRigid.useGravity = false;

        anim = GetComponent<Animator>();

    }
    void OnCollisionStay(Collision other)
    {
        IsGrounded = false;
        foreach (ContactPoint contact in other.contacts)
        {
            if (contact.normal.y < ConsideredFloor) continue;
            IsGrounded = true;
        }
    }
    void OnCollisionExit(Collision other)
    {
        IsGrounded = false;
    }

    void Update()
    {
        GlobalValues.TurningAxis = Input.GetAxis(GlobalValues.Turning);

        //is a button (if prev state pressed)
        _jumpWasPressed = Input.GetButton("Jump");

        Quaternion currentRotation = GlobalValues.CharacterTransform.rotation;
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        Quaternion cameraRotation = camera.transform.rotation;

        _currentMovement = Vector3.zero;
        //not back pedaling
        _backPedaling = false;

        //forward by mouse
        if (GlobalValues.LeftClickAxis > 0 && GlobalValues.RightClickAxis > 0)
        {
            _currentMovement += currentRotation * Vector3.forward;
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            //if moving forward
            if(currentRotation != cameraRotation)
            {
                _currentMovement += cameraRotation * Vector3.forward;
            }
            else
            {
                _currentMovement += currentRotation * Vector3.forward;
            }
           
            setWalkingForward();
                       
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            _backPedaling = true;
            setWalkingBackward();
            _currentMovement += cameraRotation * Vector3.back;
        }

        //if strafing
        if (Input.GetAxis("Vertical") != 0|| (GlobalValues.TurningAxis != 0 && GlobalValues.RightClickAxis > 0))
        {
            if (Input.GetAxis("Vertical") > 0)
            {
                _currentMovement += cameraRotation * Vector3.right;
                setStrafingRight();
            }
            else
            {
                _currentMovement += cameraRotation * Vector3.left;
                setStrafingLeft();
            }
        }
        
        //apply final direction
        _currentMovement = _currentMovement.normalized * (_backPedaling ? BackwardSpeed : ForwardSpeed);
        //if in air, we have little control over overall direction
        if (!IsGrounded)
        {
            _currentMovement = Vector3.Lerp(GlobalValues.CharacterRigid.velocity, _currentMovement, Time.deltaTime * AerialLerpModifier);
        }

        //If not facing camera direction

        GlobalValues.CharacterTransform.rotation = Quaternion.Slerp(GlobalValues.CharacterTransform.rotation, Quaternion.LookRotation(GlobalValues.CharacterTransform.position - camera.transform.position), RotationSpeed * Time.deltaTime);
        GlobalValues.CharacterTransform.rotation = Quaternion.Euler(new Vector3(0f, GlobalValues.CharacterTransform.rotation.eulerAngles.y, 0f));


        //if turning
        if (GlobalValues.TurningAxis != 0)
        {
            if (GlobalValues.TurningAxis > 0)
            {
                GlobalValues.CharacterTransform.Rotate(Vector3.up, RotationSpeed * Time.deltaTime);
                
            }
            else
            {
                GlobalValues.CharacterTransform.Rotate(Vector3.up, -RotationSpeed * Time.deltaTime);

            }
        }

        //if jumping
        if (!_jumpWasPressed)
        {
            if (IsGrounded || !IsGrounded)
            {
                setJumped();
                _hasJumped = true;
            }
        }
        if (IsGrounded)
        {
            _currentGravity = Vector3.zero;
        }
        else
        {
            _currentGravity += GravityDrop;
        }

        if(_currentMovement == Vector3.zero)
        {
            setIdle();
        }
        

    }
    void FixedUpdate()
    {
        float airVelocity = Mathf.Lerp(GlobalValues.CharacterRigid.velocity.y, _currentGravity.y, JumpLerpSpeed);
        if (_hasJumped)
        {
            _hasJumped = false;
            airVelocity += JumpSpeed;
        }
        if (airVelocity < -MaximumGravity) airVelocity = -MaximumGravity;
        //GlobalValues.CharacterRigid.velocity = new Vector3(_currentMovement.x, airVelocity, _currentMovement.z);
        Velocity = GlobalValues.CharacterRigid.velocity;
    }
}