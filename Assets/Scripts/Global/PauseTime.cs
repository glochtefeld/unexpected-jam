using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseTime : MonoBehaviour
{
    public static bool Paused { set; get; }

    public void Pause(UnityEngine.InputSystem.InputAction.CallbackContext c)
    {
        Paused = !Paused;
    }
}
