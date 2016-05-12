using UnityEngine;
using System.Collections.Generic;



public enum CursorState
{
    idle,
    Interactable
}

public class InteractionManager : MonoBehaviour
{

    [SerializeField]
    private List<Texture2D> m_CursorImages;
    [SerializeField]
    private LayerMask m_InteractionLayer;
    private bool m_Clicked;

    CursorState currentState;

    // Use this for initialization
    void Start()
    {

        //     currentState = CursorState.idle;

    }

    // Update is called once per frame
    void Update()
    {
        getCurrentTarget();
      //  handleCursor();

    }

    void getCurrentTarget()
    {


        if (m_Clicked) // retarded but meh
        {
            currentState = CursorState.Interactable;
        }
        else
        {
            currentState = CursorState.idle;
        }

    }
    void handleCursor()
    {
        if (currentState == CursorState.idle)
        {
            if (m_Clicked)
            {
                Cursor.SetCursor(m_CursorImages[1], Vector2.zero, CursorMode.Auto);
            }
            else
            {
                Cursor.SetCursor(m_CursorImages[0], Vector2.zero, CursorMode.Auto);
            }
        }
        else if (currentState == CursorState.Interactable)
        {
            if (m_Clicked)
            {
                Cursor.SetCursor(m_CursorImages[3], Vector2.zero, CursorMode.Auto);
            }
            else
            {
                Cursor.SetCursor(m_CursorImages[2], Vector2.zero, CursorMode.Auto);
            }
        }

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            m_Clicked = true;
        }
        else
        {
            m_Clicked = false;
        }

    }

}
