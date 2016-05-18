using UnityEngine;

public class ScoreManager : MonoBehaviour {

    private int m_score;
    private Vector3 m_oldPos;
    private Vector3 m_curPos;
    [SerializeField]
    private int m_scoreMultiplier = 1;

    public int Score
    {
        get
        {
            return m_score;
        }
    }
	// Use this for initialization
	void Start ()
    {

        m_score = 0;
        m_oldPos = GameController.Instance.Player.transform.position;
    }
    public void GlobalReset()
    {
        m_curPos = GameController.Instance.Player.transform.position;
        m_oldPos = GameController.Instance.Player.transform.position;
    }
	// Update is called once per frame
	void Update () {

        ComputeScore();
	
	}

    void ComputeScore()
    {
        if (GameController.Instance.Player != null)
        {
            m_curPos = GameController.Instance.Player.transform.position;
            if (m_curPos.z > m_oldPos.z)
            {
                int dist = (int)((m_curPos.z - m_oldPos.z) * m_scoreMultiplier);
                if (dist > 0)
                {
                    m_score += dist;
                    m_oldPos = m_curPos;
                }
            }
        }

    }
}
