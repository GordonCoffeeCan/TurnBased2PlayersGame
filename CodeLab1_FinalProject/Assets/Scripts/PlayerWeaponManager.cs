using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour {

    private Transform _throwWeaponPoint;
    private GameObject _throwableObject;
    private Animator _anim;

    private float _powerSpeed = 20;
    private float _currentPower;
    private float _fullPower = 35;

    private bool _isAbleToFire;

    private string _throwWeaponName;
    private string _throw;
    private Vector3 _throwDirection;

    private float _turnCoolDownTimer = 3;

    private void Awake() {
        if(this.tag == "Human") {
            _throw = "JoyThrow";
            _throwWeaponName = "Weapon_Stone";
        } else if (this.tag == "Zombie") {
            _throw = "ZomThrow";
            _throwWeaponName = "Weapon_Skull";
        }

        _throwWeaponPoint = this.transform.Find("SpriteHolder/ThrowWeaponPoint");
        _anim = this.GetComponent<Animator>();
    }

    // Use this for initialization
    void Start () {
        _isAbleToFire = true;
        _throwDirection = Vector3.zero;

    }
	
	// Update is called once per frame
	void Update () {

        if (this.tag == "Human") {
            if (GameData.isHumanTurn == true) {
                UseWeapon();
            } else {
                
            }

        }

        if (this.tag == "Zombie") {
            if (GameData.isHumanTurn == false) {
                UseWeapon();
            } else {
                
            }
        }

        
	}

    private void UseWeapon() {
        _throwDirection = Quaternion.AngleAxis(PlayerController.reticlePivot.eulerAngles.z, Vector3.forward) * Vector3.right;
        if (PlayerController.isGround == true) {

            if(PlayerController.moveAxis == 0) {
                if (Input.GetButton(_throw) && _isAbleToFire == true) {
                    _currentPower += _powerSpeed * Time.deltaTime;
                    _currentPower = Mathf.Clamp(_currentPower, 0, _fullPower);

                    if (_currentPower == _fullPower) {
                        _anim.SetTrigger("Throw");
                        ThrowWeapon();
                        _isAbleToFire = false;
                    }
                }

                if ((Input.GetButtonUp(_throw)) && _isAbleToFire == true) {
                    _anim.SetTrigger("Throw");
                    ThrowWeapon();
                    _isAbleToFire = false;
                }
            }
        } else {

        }

        if (_isAbleToFire == false) {
            _turnCoolDownTimer -= Time.deltaTime;
            if (_turnCoolDownTimer <= 0) {
                _turnCoolDownTimer = 3;
                FinishTurn();
            }
        }
    }

    /*private void ThrowWeaponRight() {
        if (PlayerController.spriteHolder.localScale.x > 0) {
            ThrowWeapon();
        }
    }

    private void ThrowWeaponLeft() {
        if (PlayerController.spriteHolder.localScale.x < 0) {
            ThrowWeapon();
        }
    }*/

    private void ThrowWeapon() {
        _throwableObject = Instantiate(Resources.Load("Prefabs/" + _throwWeaponName), _throwWeaponPoint.position, Quaternion.identity) as GameObject;
        Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), _throwableObject.GetComponent<Collider2D>());
        _throwableObject.GetComponent<Rigidbody2D>().AddForceAtPosition(new Vector2(_throwDirection.x, _throwDirection.y) * _currentPower, _throwWeaponPoint.position, ForceMode2D.Impulse);
        _currentPower = 0;
    }

    private void FinishTurn() {
        GameData.isTurnOnGoing = false;
        if(GameData.isHumanTurn == true) {
            GameData.isHumanTurn = false;
            GameData.isTurnOnGoing = true;
            CameraBehavior.playerShowed = false;
            _isAbleToFire = true;
        } else {
            GameData.isHumanTurn = true;
            GameData.isTurnOnGoing = true;
            CameraBehavior.playerShowed = false;
            _isAbleToFire = true;
        }
    }
}
