using UnityEngine;
using UnityEngine.InputSystem;

public class Movement2D : MonoBehaviour
{
    public Controller2D controller;
    public float runSpeed = 40f;

    private float _horizontalMove = 0f;
    private bool _jumping;
    private bool _crouching;
    
    private void FixedUpdate()
    {
        controller.Move(_horizontalMove * Time.fixedDeltaTime, _crouching, _jumping);
        _jumping = false;
    }

    public void Move(InputAction.CallbackContext context)
    {
        var input = context.ReadValue<Vector2>();
        _horizontalMove = input.x * runSpeed;
        if (input.y > 0)
            _jumping = true;
        if (input.y < 0)
            _crouching = true;
        else
            _crouching = false;
    }
}

/* This uses the new InputSystem package for unity, which is more 
   efficient because it relies on events being fired off instead of
   the Update() method. I think. */