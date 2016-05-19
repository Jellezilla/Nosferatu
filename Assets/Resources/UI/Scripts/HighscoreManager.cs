using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class HighscoreManager : MonoBehaviour {

    [Serializable]
    public class ScoreTable {
        public string[] playerNames = {"Peter","Jesper","Maja","Emil","Alex"};
        public int [] playerScores = {500,10,5,1,-5};
    }

    private ScoreTable _scoreData;
    private int _currentScore = 1000;
    private int _newScoreIndex = 0;
    private string _newScoreName = "AAA";
    private GameObject _inputField;
    private List<Text> _nameFields,_scoreFields;
    public GameObject ScoreBoardRef;
    public GameObject HighscoreEffect;

    void OnEnable () {
        SetupRefs();

        _scoreData = GetScores();

        ChangeScores();
        CheckScore();

        PrintScores();
    }

    void Update() {
        //this is for updating the score should change so when pressed new game or exit update score
        if (Input.GetKeyDown(KeyCode.Return)){
            Debug.Log("ENTER");
            _newScoreName = _inputField.transform.GetChild(2).GetComponent<Text>().text;
            if (_newScoreName == "") _newScoreName = "A Nameless soul";
            _inputField.SetActive(false);
            _nameFields[_newScoreIndex].text = _newScoreName;
            UpdateScore();
        }

    }
    void SetupRefs() {
        //tightly coupled reference to score until new system is setup
        _currentScore = int.Parse (ScoreBoardRef.GetComponent<Text>().text);
        //_currentScore = 310;
        _inputField = transform.FindChild("InputField").gameObject;
        _inputField.SetActive(false);

        GameObject tf = transform.FindChild("TableFields").gameObject;

        //Setting up references to the the fields of score and player name
        _nameFields = new List<Text>();
        _scoreFields = new List<Text>();

        for (int i = 0; i < tf.transform.childCount; i++) {
            if (tf.transform.GetChild(i).name.Contains("Name"))
                _nameFields.Add(tf.transform.GetChild(i).GetComponent<Text>());
            else
                _scoreFields.Add(tf.transform.GetChild(i).GetComponent<Text>());
        }

        _newScoreIndex = 5;//_scoreData.playerNames.Length;
    }

    void CheckScore() {
        int len = _scoreData.playerNames.Length;
        for (int i = 0; i < len; i++) {
            if (_currentScore > _scoreData.playerScores[i]) {
                //Do new score;
                _newScoreIndex = i;
                SetNewScore(i);
                break;
            }
        }
     //   Debug.Log("hello");
        //need setup for moving all the scores down by starting at last
        for (int i = len-1; i > _newScoreIndex; i--) {
           // Debug.Log("handling score movement"+i);
            _scoreData.playerNames[i] = _scoreData.playerNames[i - 1];
            _scoreData.playerScores[i] = _scoreData.playerScores[i - 1];

            _scoreFields[i].text = _scoreData.playerScores[i].ToString();
            _nameFields[i].text = _scoreData.playerNames[i];
        }
    }

    void SetNewScore(int index) {
      //  Debug.Log("set new score");
        _inputField.transform.position = new Vector3 (_nameFields[index].transform.position.x-0.26f, _nameFields[index].transform.position.y, _nameFields[index].transform.position.z);
        _inputField.SetActive(true);
        //InputField f = _inputField.GetComponent<InputField>();
        //f.Select();
        _nameFields[index].text = "";
        _scoreFields[index].text = _currentScore.ToString();
        HighscoreEffect.SetActive(true);
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
         //   Debug.Log(_scoreData.playerNames[i]);
          //  Debug.Log(_scoreData.playerScores[i]);
        }
    }

    void UpdateScore() {
        _scoreData.playerNames[_newScoreIndex] = _newScoreName;
        _scoreData.playerScores[_newScoreIndex] = _currentScore;

        string newScores = JsonUtility.ToJson(_scoreData);

        PlayerPrefs.SetString("highscores",newScores);
    }

    void ChangeScores() {
        for (int i = 0; i < _scoreData.playerNames.Length; i++) {
            _scoreFields[i].text = _scoreData.playerScores[i].ToString();
            _nameFields[i].text = _scoreData.playerNames[i];
          //  Debug.Log(_scoreData.playerNames[i]);
          //  Debug.Log(_scoreData.playerScores[i]);
        }
    }

    void ResetScores() {
        ScoreTable s = new ScoreTable();
        string defaultScores = JsonUtility.ToJson(s);
        PlayerPrefs.SetString("highscores", defaultScores);

    }
}
