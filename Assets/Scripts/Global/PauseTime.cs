using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseTime : MonoBehaviour
{
    #region Serialized Fields
#pragma warning disable CS0649
    [SerializeField] private Material _grayscaleMat;
#pragma warning restore CS0649
    #endregion

    private float _transitionTime = 0.5f;
    private bool transitioning = false;
    public static bool Paused { private set; get; }

    public void Pause(UnityEngine.InputSystem.InputAction.CallbackContext c)
    {
        Paused = !Paused;
        // The callback context will fire three times, we only want one coroutine
        if (transitioning == false)
        {
            transitioning = true;
            StartCoroutine(GrayscaleRoutine(Paused));
        }
    }

    private IEnumerator GrayscaleRoutine(bool turnGray)
    {
        float time = 0f;
        while (time < _transitionTime)
        {
            float ratio = time / _transitionTime;
            float grayAmount = turnGray ? ratio : 1 - ratio;
            SetGrayscale(grayAmount);
            time += Time.deltaTime;
            yield return null;
        }
        SetGrayscale(turnGray ? 1 : 0);
        transitioning = false;
        yield return null;
    }

    private void SetGrayscale(float amount) =>
        _grayscaleMat.SetFloat("_GrayscaleAmount", amount);

}

/* This script controls both the movement of all enemies (by
 * short circuiting their FixedUpdate() methods) and the color
 * of all sprites with the Grayscale Material. This is more 
 * performant than starting coroutines for every object. */