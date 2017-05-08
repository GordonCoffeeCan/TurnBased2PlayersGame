using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public Image humanHP;
    public Image zombieHP;

    private float _humanHPTarget;
    private float _zombieHPTarget;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        _humanHPTarget = GameData.currenthumanHP / GameData.humanHP;
        _zombieHPTarget = GameData.currentzombieHP / GameData.zombieHP;

        humanHP.fillAmount = Mathf.Lerp(humanHP.fillAmount, _humanHPTarget, 8 * Time.deltaTime);
        zombieHP.fillAmount = Mathf.Lerp(zombieHP.fillAmount, _zombieHPTarget, 8 * Time.deltaTime);

    }
}
