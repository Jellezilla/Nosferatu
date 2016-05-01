using UnityEngine;
using System.Collections;

public class EndGameScript : MonoBehaviour {

    private Vector3 m_playerOldPos;

    public GameObject GameoverScreen;

    void Start(){
        m_playerOldPos = GameController.Instance.Player.transform.position; 
    } 
    /// <summary>
    /// Is the game over. Eg. the player is out of fuels and can't continue.
    /// </summary>
    /// <returns></returns>
    bool IsGameOver()
    {
        Vector3 playerPos = GameController.Instance.Player.transform.position;
        float squaredMag = (playerPos - m_playerOldPos).sqrMagnitude;

        if (GameController.Instance.OutOfFuel && squaredMag < .05f && squaredMag > -.05f)
        {
            return true;
        }
        m_playerOldPos = playerPos;
        return false;
    }
	// Update is called once per frame
	void Update () {
        if (IsGameOver())
        {
            GameoverScreen.SetActive(true);
            //Go to scoreboard, or have functionality on gameover screen
        }
	}
}
