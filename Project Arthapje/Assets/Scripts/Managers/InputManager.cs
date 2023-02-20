using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviourSingleton<InputManager>
{
    //public static PlayerInput m_playerInput;
    public PlayerInput m_playerInput;

    // Start is called before the first frame update
    void Start()
    {
        //m_playerInput = m_playerInputMap;
        DontDestroyOnLoad(gameObject);
    }

    public Vector2 MoveDirection()
    {
        return m_playerInput.actions.FindAction("Move").ReadValue<Vector2>();
    }

    public bool ActionExecuted()
    {
        
        if(m_playerInput.actions.FindAction("Action").WasPressedThisFrame())
        {
            //Debug.Log("Action executed");
            return true;
        }
        else
        {
            return false;
        }
    }

  public bool ActionPerformed()
  {

    if (m_playerInput.actions.FindAction("Action").WasPerformedThisFrame())
    {
      //Debug.Log("Action executed");
      return true;
    }
    else
    {
      return false;
    }
  }

  public bool ActionCanceled()
    {
        if (m_playerInput.actions.FindAction("Action").WasReleasedThisFrame())
        {
            //Debug.Log("Action canceled");
            return true;
        }
        else
        {
            return false;
        }
    }
}
