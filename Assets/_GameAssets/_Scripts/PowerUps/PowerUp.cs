using System;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    public Action OnActivated;
    public abstract void Activate();
}