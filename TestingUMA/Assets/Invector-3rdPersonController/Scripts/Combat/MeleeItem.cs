using UnityEngine;
using System.Collections;
using System;

public abstract class MeleeItem : MonoBehaviour
{
    protected bool active;
    public void SetActive(bool value)
    {
        active = value;
    }
}
