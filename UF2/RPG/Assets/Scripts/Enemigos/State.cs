using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    public abstract void init();
    public abstract void update();
    public abstract void exit();
}
