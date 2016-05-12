using UnityEngine;
using System.Collections;

public class InteractableObject : MonoBehaviour {

    private Material m_DefaultMaterial;
    [SerializeField]
    private Material m_OutlineMaterial;
    private Renderer m_Renderer;
	// Use this for initialization
	void Awake ()
    {
        m_Renderer = GetComponent<Renderer>();
        m_DefaultMaterial = m_Renderer.material;
	}

    // Update is called once per frame
    public void ToggleHighlight()
    {
        if (ReferenceEquals(m_Renderer.material, m_DefaultMaterial))
        {
            m_Renderer.material = m_OutlineMaterial;
        }
        else
        {
            m_Renderer.material = m_DefaultMaterial;
        }
    }




}
