using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

    public GameObject Healthbar;
    public GameObject Soulsbar;
    public GameObject ScoreBoard;
    public GameObject PlayerObject;
    public GameObject SpecialEffectObject;
    public GameObject RampageEffect;

    private RectTransform _hpmask;
    private RectTransform _spmask;
    private Text _scoreText;
    private float _spMaxWidth, _hpMaxWidth, _spHeight,_hpHeight;
    private int oldPlayerDistance=0;

    void Start () {
        _hpmask = Healthbar.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
        _spmask = Soulsbar.transform.GetChild(0).gameObject.GetComponent<RectTransform>();

        _spMaxWidth = _spmask.GetComponent<RectTransform>().rect.width;
        _hpMaxWidth = _hpmask.GetComponent<RectTransform>().rect.width;
        _spHeight = _spmask.GetComponent<RectTransform>().rect.height;
        _hpHeight = _hpmask.GetComponent<RectTransform>().rect.height;

        oldPlayerDistance = (int)PlayerObject.transform.position.z;

        _scoreText = ScoreBoard.GetComponent<Text>();
    }
	
	void Update () {
        float fuelCurrent = GameController.Instance.GetFuel;
        float soulsCurrent = GameController.Instance.GetSouls;

        _hpmask.sizeDelta = new Vector2(_hpMaxWidth/100 * fuelCurrent, _hpHeight);// _hpHeight);
        _spmask.sizeDelta = new Vector2(_spMaxWidth/100 * (1+soulsCurrent), _spHeight);// _hpHeight);

        if (soulsCurrent >= 100 && !SpecialEffectObject.activeSelf) {//hardcoded max
            SpecialEffectObject.SetActive(true);
            RampageEffect.SetActive(true);
        }

        int newPlayerDistance = (int)PlayerObject.transform.position.z;

        if (oldPlayerDistance < newPlayerDistance) {
            oldPlayerDistance = newPlayerDistance;
            _scoreText.text = oldPlayerDistance.ToString();
        }
    }
}
