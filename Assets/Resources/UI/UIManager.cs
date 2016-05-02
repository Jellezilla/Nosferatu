using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

    public GameObject Healthbar;
    public GameObject Soulsbar;
    public GameObject ScoreBoard;
    public GameObject SpecialEffectObject;
    public GameObject RampageEffect;
    public GameObject RampageReady;
    public GameObject GameOverScreen;
    public GameObject HudScreen;

    private GameObject PlayerObject;
    private RectTransform _hpmask;
    private RectTransform _spmask;
    private Text _scoreText;
    private float _spMaxWidth, _hpMaxWidth, _spHeight, _hpHeight;
    private int _oldPlayerDistance = 0;
    private Vector3 _oldPlayerPos;

    //TODO: Update with implementing only one variable for holding player pos datatatatata

    void Start() {
        PlayerObject = GameController.Instance.Player;
        _hpmask = Healthbar.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
        _spmask = Soulsbar.transform.GetChild(0).gameObject.GetComponent<RectTransform>();

        _spMaxWidth = _spmask.GetComponent<RectTransform>().rect.width;
        _hpMaxWidth = _hpmask.GetComponent<RectTransform>().rect.width;
        _spHeight = _spmask.GetComponent<RectTransform>().rect.height;
        _hpHeight = _hpmask.GetComponent<RectTransform>().rect.height;

        _oldPlayerDistance = 0;//(int)PlayerObject.transform.position.z;
        _oldPlayerPos = GameController.Instance.Player.transform.position;
        _scoreText = ScoreBoard.GetComponent<Text>();
    }

    void Update() {
        float fuelCurrent = GameController.Instance.GetFuel;
        float soulsCurrent = GameController.Instance.GetSouls;

        _hpmask.sizeDelta = new Vector2(_hpMaxWidth / 100 * fuelCurrent, _hpHeight);// _hpHeight);
        _spmask.sizeDelta = new Vector2(_spMaxWidth / 100 * (1 + soulsCurrent), _spHeight);// _hpHeight);

        if (soulsCurrent >= 100 && !SpecialEffectObject.activeSelf) {//hardcoded max
            SpecialEffectObject.SetActive(true);
            RampageReady.SetActive(true);
        }

        int newPlayerDistance = (int)PlayerObject.transform.position.z;

        Debug.Log("new pos"+newPlayerDistance);
        Debug.Log("old pos"+_oldPlayerDistance);
        if (_oldPlayerDistance < newPlayerDistance) {
            Debug.Log("update score");
            _oldPlayerDistance = newPlayerDistance;
            _scoreText.text = _oldPlayerDistance.ToString();
        }

        //Checking for gameover
        if (IsGameOver() && !GameOverScreen.activeSelf) {
            SetupGameOverScreen();
            //Go to scoreboard, or have functionality on gameover screen
        }
    }

    void StartRampage() {
        RampageEffect.SetActive(true);
        SpecialEffectObject.SetActive(false);
    }

    bool IsGameOver() {
        Vector3 playerPos = GameController.Instance.Player.transform.position;
        float squaredMag = (playerPos - _oldPlayerPos).sqrMagnitude;

        if (GameController.Instance.OutOfFuel && squaredMag < .05f && squaredMag > -.05f) {
            return true;
        }
        _oldPlayerPos = playerPos;
        return false;
    }

    void SetupGameOverScreen(){
        HudScreen.SetActive(false);
        GameOverScreen.SetActive(true);

        Text scorefield = GameOverScreen.transform.Find("ScoreField").GetComponent<Text>();

        scorefield.text = _scoreText.text;
        //GameOverScreen;
    }

    public void RestartLevel() {
        Application.LoadLevel(Application.loadedLevel);
    }
}
