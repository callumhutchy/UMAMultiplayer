using UnityEngine;
using System.Collections;

public class GlobalValues
{
    public enum Seperate
    {
        ___________
    }


    public static Transform CharacterTransform;
    public static Rigidbody CharacterRigid;
    public static Transform CameraTransform;

    public const string MoveForward = "MoveForward";
    public static float ForwardAxis;
    public const string MoveBackward = "MoveBackward";
    public static float BackwardAxis;

    public const string Strafing = "Strafing";
    public static float StrafeAxis;
    public const string Turning = "Turning";
    public static float TurningAxis;
    public const string Jump = "Jump";
    public static float JumpAxis;

    //accessed by character controls (moving forward) AND camera
    public const string LeftMouse = "LeftMouse";
    public static float LeftClickAxis;
    public const string RightMouse = "RightMouse";
    public static float RightClickAxis;

    public const string MouseX = "MouseX";
    public static float MouseAxisX;
    public const string MouseY = "MouseY";
    public static float MouseAxisY;
    public const string ScrollWheel = "ScrollWheel";
    public static float ScrollWheelAxis;

    public const string Number1 = "Number1";
    public static float Number1Press;
    public const string Number2 = "Number2";
    public static float Number2Press;
    public const string Number3 = "Number3";
    public static float Number3Press;
    public const string Number4 = "Number4";
    public static float Number4Press;
    public const string Number5 = "Number5";
    public static float Number5Press;
    public const string Number6 = "Number6";
    public static float Number6Press;
    public const string Number7 = "Number7";
    public static float Number7Press;
    public const string Number8 = "Number8";
    public static float Number8Press;


}