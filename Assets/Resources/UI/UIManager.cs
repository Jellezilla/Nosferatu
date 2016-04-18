using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

    public GameObject Healthbar;
    public GameObject Soulsbar;
    public GameObject SpecialEffectObject;
    private RectTransform _hpmask;
    private RectTransform _spmask;
    private float _spMaxWidth, _hpMaxWidth, _spHeight,_hpHeight;

    void Start () {
        _hpmask = Healthbar.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
        _spmask = Soulsbar.transform.GetChild(0).gameObject.GetComponent<RectTransform>();

        _spMaxWidth = _spmask.GetComponent<RectTransform>().rect.width;
        _hpMaxWidth = _hpmask.GetComponent<RectTransform>().rect.width;
        _spHeight = _spmask.GetComponent<RectTransform>().rect.height;
        _hpHeight = _hpmask.GetComponent<RectTransform>().rect.height;

        Debug.Log(_hpHeight);
    }
	
	void Update () {
        float fuelCurrent = GameController.Instance.GetFuel;
        float soulsCurrent = GameController.Instance.GetSouls;
        Debug.Log(soulsCurrent);
        _hpmask.sizeDelta = new Vector2(_hpMaxWidth/100 * fuelCurrent, _hpHeight);// _hpHeight);
        _spmask.sizeDelta = new Vector2(_spMaxWidth/100 * (1+soulsCurrent), _spHeight);// _hpHeight);

        if (soulsCurrent >= 100 && !SpecialEffectObject.activeSelf) {//hardcoded max
            SpecialEffectObject.SetActive(true);
        }
    }
}
