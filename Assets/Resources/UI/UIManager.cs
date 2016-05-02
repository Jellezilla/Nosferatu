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
    public GameObject SoulEffectObject;

    private bool _bloodFading=false;
    private GameObject PlayerObject;
    private Image _hpFill;
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

        //this is a really good one, have to dig deep due to mask parenting
        _hpFill = Healthbar.GetComponent<Image>();
    }

    void Update() {
        float fuelCurrent = GameController.Instance.GetFuel;
        float soulsCurrent = GameController.Instance.GetSouls;

        _hpmask.sizeDelta = new Vector2(_hpMaxWidth / 100 * fuelCurrent, _hpHeight);// _hpHeight);
        _spmask.sizeDelta = new Vector2(_spMaxWidth / 100 * (1 + soulsCurrent), _spHeight);// _hpHeight);

        //Effect for rampage mode, when souls max has been reached
        if (soulsCurrent >= 100 && !SpecialEffectObject.activeSelf) {//hardcoded max
            SpecialEffectObject.SetActive(true);
            RampageReady.SetActive(true);
        }

        //Taking care of score
        int newPlayerDistance = (int)PlayerObject.transform.position.z;

        if (_oldPlayerDistance < newPlayerDistance) {

            _oldPlayerDistance = newPlayerDistance;
            _scoreText.text = _oldPlayerDistance.ToString();
        }

        //Checking for gameover
        if (IsGameOver() && !GameOverScreen.activeSelf) {
            SetupGameOverScreen();
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
    }

    public void RestartLevel() {
        Application.LoadLevel(Application.loadedLevel); //DPRCTD
    }

    public void AddSoulEffect(Vector2 spawnPos) {
        GameObject s = Instantiate(SoulEffectObject,new Vector3(spawnPos.x,1,spawnPos.y),transform.rotation) as GameObject;
        s.transform.SetParent(this.transform);
    }

    public void AddBloodEffect() {
        _bloodFading = true;
        StartCoroutine("FadeBlood");
    }

    public bool IsBloodFading() {
        return _bloodFading;
    }

    IEnumerator FadeBlood() {
        for (float f = 0.3f; f >= 0; f -= 0.1f) {
            if (f < 0.1) f = 0;
            _hpFill.material.SetFloat("_FlashAmount", f);

            yield return new WaitForSeconds(.1f);
        }
        _bloodFading = false;
    }
}
