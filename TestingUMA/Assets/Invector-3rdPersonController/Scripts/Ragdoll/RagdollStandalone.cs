using UnityEngine;
using System.Collections;
using Invector;
using System;

/// <summary>
/// Use this script on Non-AI or Non-CharacterController to use the Ragdoll feature on normal Characters 
/// </summary>

[RequireComponent (typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class RagdollStandalone : MonoBehaviour, ICharacter
{
    [SerializeField]
    private float startHealth = 100f;
    public float startingHealth { get { return startHealth; } }
    public float currentHealth { get; set; }
    public bool ragdolled { get; set; }    
    Rigidbody _rigidbody;
    CapsuleCollider _capsuleCollider;

    void Start()
    {
        currentHealth = startingHealth;
        GetComponent<Animator>().updateMode = AnimatorUpdateMode.AnimatePhysics;
        _rigidbody = GetComponent<Rigidbody>();        
        _capsuleCollider = GetComponent<CapsuleCollider>();

    }

    public void ResetRagdoll()
    {
        ragdolled = false;
    }

    public void RagdollGettingUp()
    {
        _rigidbody.useGravity = true;
        _rigidbody.isKinematic = false;
        _capsuleCollider.enabled = true;
    }

    public void EnableRagdoll()
    {
        ragdolled = true;
        _capsuleCollider.enabled = false;
        _rigidbody.useGravity = false;
        _rigidbody.isKinematic = true;
    }

    /// <summary>
    /// APPLY DAMAGE - call this method by a SendMessage with the damage value
    /// </summary>
    /// <param name="damage"> damage to apply </param>
    public void TakeDamage(Damage damage)
    {       
        // reduce the current health by the damage amount.
        currentHealth -= damage.value;        
       
        if (damage.activeRagdoll)
            transform.SendMessage("ActivateRagdoll", SendMessageOptions.DontRequireReceiver);
    }
}