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
    private InteractableObject m_currentInteractable;
    private InteractableObject m_newInteractable;

    public InteractableObject SelectedObject
    {
        get
        {
            return m_currentInteractable;
        }
    }
    CursorState currentState;

    // Use this for initialization
    void Start()
    {
        currentState = CursorState.idle;
    }

    // Update is called once per frame
    void Update()
    {

        handleCursor();

    }

    void FixedUpdate()
    {
        getCurrentTarget();
    }

    void getCurrentTarget()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 1000, m_InteractionLayer,QueryTriggerInteraction.Ignore))
        {
            if (m_currentInteractable == null)
            {
                m_currentInteractable = hit.collider.gameObject.GetComponent<InteractableObject>();
                m_currentInteractable.ToggleHighlight();
            }
            else if (!ReferenceEquals(m_currentInteractable.gameObject,hit.collider.gameObject))
            {
                m_currentInteractable.ToggleHighlight();
                m_currentInteractable = hit.collider.gameObject.GetComponent<InteractableObject>();
                m_currentInteractable.ToggleHighlight();
            }
            currentState = CursorState.Interactable;
        }
        else
        {
            if (m_currentInteractable == null)
            {
                currentState = CursorState.idle;
            }
            else if (m_currentInteractable != null && !m_Clicked)
            {
                m_currentInteractable.ToggleHighlight();
                m_currentInteractable = null;
            }

        }

    }
    void handleCursor()
    {
        if (currentState == CursorState.idle)
        {
            if (m_Clicked)
            {
              //  Cursor.SetCursor(m_CursorImages[1], Vector2.zero, CursorMode.Auto);
            }
            else
            {
                //Cursor.SetCursor(m_CursorImages[0], Vector2.zero, CursorMode.Auto);
            }
        }
        else if (currentState == CursorState.Interactable)
        {
            if (m_Clicked)
            {
               // Cursor.SetCursor(m_CursorImages[3], Vector2.zero, CursorMode.Auto);
            }
            else
            {
               // Cursor.SetCursor(m_CursorImages[2], Vector2.zero, CursorMode.Auto);
            }
        }

        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            m_Clicked = true;
        }
        else
        {
            m_Clicked = false;
        }

    }

}
