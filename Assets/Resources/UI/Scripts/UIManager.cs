﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

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
    private GameObject PlayerObject;
    private Image _hpFill;
    private RectTransform _hpmask;
    private RectTransform _spmask;
    private Text _scoreText;
    private float _spMaxWidth, _hpMaxWidth, _spHeight, _hpHeight;
    private float m_oldFuelValue = 0;
    private WaitForSeconds m_bloodPulse;
    private float m_maxFuel;
    private float m_maxSouls;
    float soulsCurrent;
    //TODO: Update with implementing only one variable for holding player pos datatatatata

    void Start()
    {
        EventController.Instance.SubscribeEvent(UIEvents.Rampage, StartRampage);
        m_maxFuel = GameController.Instance.MaxFuel;
        m_oldFuelValue = m_maxFuel;
        m_maxSouls = GameController.Instance.MaxSouls;
        PlayerObject = GameController.Instance.Player;
        m_bloodPulse = new WaitForSeconds(.1f);
        _hpmask = Healthbar.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
        _spmask = Soulsbar.transform.GetChild(0).gameObject.GetComponent<RectTransform>();

        _spMaxWidth = _spmask.GetComponent<RectTransform>().rect.width;
        _hpMaxWidth = _hpmask.GetComponent<RectTransform>().rect.width;
        _spHeight = _spmask.GetComponent<RectTransform>().rect.height;
        _hpHeight = _hpmask.GetComponent<RectTransform>().rect.height;

        _scoreText = ScoreBoard.GetComponent<Text>();

        //this is a really good one, have to dig deep due to mask parenting
        _hpFill = Healthbar.GetComponent<Image>();
    }

    void Update() {

        float fuelCurrent = 0;
        if (PlayerObject != null)
        {
            fuelCurrent = GameController.Instance.GetFuel;
            soulsCurrent = GameController.Instance.GetSouls;
         //   newPlayerDistance = (int)PlayerObject.transform.position.z;
        }

        if (fuelCurrent > m_oldFuelValue)
        {
            StartCoroutine(FadeBlood());
        }
        m_oldFuelValue = fuelCurrent;
        _hpmask.sizeDelta = new Vector2(_hpMaxWidth / 100 * fuelCurrent, _hpHeight);// _hpHeight);
        _spmask.sizeDelta = new Vector2(_spMaxWidth / 100 * (1 + soulsCurrent), _spHeight);// _hpHeight);

        //Effect for rampage mode, when souls max has been reached
        if (soulsCurrent >= m_maxSouls && !SpecialEffectObject.activeSelf)
        {//hardcoded max
            SpecialEffectObject.SetActive(true);
            //  RampageReady.SetActive(true);
        }
        else if (SpecialEffectObject.activeSelf && soulsCurrent < m_maxSouls)
        {
            SpecialEffectObject.SetActive(false);
        }
        _scoreText.text = GameController.Instance.ScoreManager.Score.ToString();
       
        //Checking for gameover
        if (IsGameOver() && !GameOverScreen.activeSelf) {
            SetupGameOverScreen();
        }
    }

    void StartRampage() {
        if (soulsCurrent == m_maxSouls)
        {
            RampageEffect.SetActive(true);
            SpecialEffectObject.SetActive(false);
        }

    }

    bool IsGameOver() {

        if (GameController.Instance.PlayerDead)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    void SetupGameOverScreen(){
        HudScreen.SetActive(false);
        GameOverScreen.SetActive(true);

        Text scorefield = GameOverScreen.transform.Find("ScoreField").GetComponent<Text>();

        scorefield.text = _scoreText.text;
    }

    public void RestartLevel() {
       // Application.LoadLevel(Application.loadedLevel); //DPRCTD
        SceneManager.LoadScene(1);
    }

    public void EndGame()
    {
        Application.Quit();
    }

    public void AddSoulEffect(Vector2 spawnPos) {
    }

    IEnumerator FadeBlood() {
        for (float f = 0.3f; f >= 0; f -= 0.1f) {
            if (f < 0.1) f = 0;
            _hpFill.material.SetFloat("_FlashAmount", f);
            yield return m_bloodPulse;
        }
    }
}
