using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHPManager : MonoBehaviour {
    public int health = 100;

    private int _currentHealth;

    //private CapsuleCollider2D _playerCol;

    private void Awake() {
        //_playerCol = this.GetComponent<CapsuleCollider2D>();
    }

    // Use this for initialization
    void Start () {
        _currentHealth = health;

        if (this.tag == "Human") {
            GameData.humanHP = health;
            GameData.currenthumanHP = _currentHealth;
        } else if (this.tag == "Zombie") {
            GameData.zombieHP = health;
            GameData.currentzombieHP = _currentHealth;
        }
    }
	
	// Update is called once per frame
	void Update () {
		if(_currentHealth <= 0) {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
	}

    private void OnCollisionEnter2D(Collision2D _col) {
        if (this.tag == "Human") {
            if(_col.gameObject.tag == "Weapon_Skull") {
                _currentHealth -= (int)_col.relativeVelocity.magnitude * 2;
                health = _currentHealth;

                GameData.currenthumanHP = _currentHealth;
            }
        }else if (this.tag == "Zombie") {
            if (_col.gameObject.tag == "Weapon_Stone") {
                _currentHealth -= (int)_col.relativeVelocity.magnitude * 2;
                health = _currentHealth;

                GameData.currentzombieHP = _currentHealth;
            }
        }

        if(_col.gameObject.tag == "Spike") {
            _currentHealth -= (int)_col.relativeVelocity.magnitude * 2;

            if (this.tag == "Human") {
                health = _currentHealth;
                GameData.currenthumanHP = _currentHealth;
            } else if (this.tag == "Zombie") {
                GameData.currentzombieHP = _currentHealth;
                health = _currentHealth;
            }
        }
    }
}
