using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour {



    [Serializable]
    public class ScoreTable {
        public string[] playerNames = { "Peter", "Jesper", "Maja", "Emil", "Alex" };
        public int[] playerScores = { 10000, 10, 5, 1, -5 };
    }

    private ScoreTable _scoreData;
    private List<Text> _nameFields, _scoreFields;
    public GameObject LoadingText;

    void OnEnable() {
        SetupRefs();
        _scoreData = GetScores();

        ChangeScores();

        PrintScores();
    }

    void Update() {
        //this is for updating the score should change so when pressed new game or exit update score
    }
    void SetupRefs() {

        GameObject tf = transform.FindChild("HighscoreScreen").transform.FindChild("TableFields").gameObject;

        //Setting up references to the the fields of score and player name
        _nameFields = new List<Text>();
        _scoreFields = new List<Text>();

        for (int i = 0; i < tf.transform.childCount; i++) {
            if (tf.transform.GetChild(i).name.Contains("Name"))
                _nameFields.Add(tf.transform.GetChild(i).GetComponent<Text>());
            else
                _scoreFields.Add(tf.transform.GetChild(i).GetComponent<Text>());
        }
    }

    ScoreTable GetScores() {

        //if no scores are set return default values;
        if (!PlayerPrefs.HasKey("highscores")) {
            ScoreTable _scoreTable = new ScoreTable();
            string defaultScores = JsonUtility.ToJson(_scoreTable);
            PlayerPrefs.SetString("highscores", defaultScores);
        }

        string getScores = PlayerPrefs.GetString("highscores");

        return JsonUtility.FromJson<ScoreTable>(getScores);
    }

    void PrintScores() {
        for (int i = 0; i < _scoreData.playerNames.Length; i++) {
            Debug.Log(_scoreData.playerNames[i]);
            Debug.Log(_scoreData.playerScores[i]);
        }
    }

    void ChangeScores() {
        for (int i = 0; i < _scoreData.playerNames.Length; i++) {
            _scoreFields[i].text = _scoreData.playerScores[i].ToString();
            _nameFields[i].text = _scoreData.playerNames[i];
            Debug.Log(_scoreData.playerNames[i]);
            Debug.Log(_scoreData.playerScores[i]);
        }
    }

    void ResetScores() {
        ScoreTable s = new ScoreTable();
        string defaultScores = JsonUtility.ToJson(s);
        PlayerPrefs.SetString("highscores", defaultScores);
    }

    public void OnStartGame() {
        LoadingText.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    public void OnEndGame() {

    }
}
