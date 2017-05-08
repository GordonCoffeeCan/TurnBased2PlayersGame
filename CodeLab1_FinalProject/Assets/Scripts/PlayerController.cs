using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed = 8;
    public float jumpSpeed = 5;
    public float aimSpeed = 70;
    public float reticleFadeSpeed = 10;

    public static bool isGround;
    public static Transform reticlePivot;
    public static Transform spriteHolder;
    public static float moveAxis = 0;

    private Rigidbody2D _rig;
    private Animator _anim;
    //private Transform _headPivot;

    private Transform _reticlePivot;
    private Transform _spriteHolder;

    private Animator _reticleAnim;
    private SpriteRenderer _reticle;
    private float _reticleCounterAngle;
    private float _rotZ;

    private float _speed;
    private bool _faceingRight;

    private bool _characterSwitched;

    private string _ctrMove;
    private string _ctrAim;
    private string _ctrJump;

    private bool _isGround = false;

    private void Awake() {
        _rig = this.GetComponent<Rigidbody2D>();
        _anim = this.GetComponent<Animator>();
        _reticlePivot = this.transform.Find("ReticlePivot");
        _reticle = _reticlePivot.Find("Reticle").GetComponent<SpriteRenderer>();
        _reticleAnim = _reticle.GetComponent<Animator>();
        _spriteHolder = this.transform.Find("SpriteHolder");
        _speed = speed;
        _characterSwitched = false;
    }

    // Use this for initialization
    void Start () {
		if(this.tag == "Human") {
            _ctrMove = "JoyHorizontal";
            _ctrAim = "JoyVertical";
            _ctrJump = "JoyJump";
            //_headPivot = this.transform.Find("Human_body/HeadPivot");
        } else if (this.tag == "Zombie") {
            _ctrMove = "ZomHorizontal";
            _ctrAim = "ZomVertical";
            _ctrJump = "ZomJump";
            //_headPivot = this.transform.Find("Zombie_body/HeadPivot");
        }
        _reticleCounterAngle = 0;
        _faceingRight = true;
        reticlePivot = _reticlePivot;
    }
	
	// Update is called once per frame
	void Update () {
        
        if (this.tag == "Human") {
            if (GameData.isHumanTurn == true) {
                PlayerControl();
                if (_characterSwitched == false) {
                    _reticleAnim.SetBool("ShowReticle", false);
                    _characterSwitched = true;
                }
            } else {
                ResetCharacterAnim();
            }

        }

        if (this.tag == "Zombie") {
            if (GameData.isHumanTurn == false) {
                PlayerControl();
                if (_characterSwitched == false) {
                    _reticleAnim.SetBool("ShowReticle", false);
                    _characterSwitched = true;
                }
            } else {
                ResetCharacterAnim();
            }
        }
        spriteHolder = _spriteHolder;
    }

    private void PlayerControl() {
        if (_isGround == true) {
            
            //If Veriticle Axis value is greater than 0.75, rotate aim reticle;
            if (Mathf.Abs(Input.GetAxis(_ctrAim)) > 0.75f) {
                _reticleAnim.SetBool("ShowReticle", true);

                //Rotate Aim Reticle;
                if (_spriteHolder.localScale.x > 0) {
                    _reticlePivot.Rotate(new Vector3(0, 0, Input.GetAxis(_ctrAim) * Time.deltaTime * 100));

                    _rotZ = _reticlePivot.eulerAngles.z;

                    if (_rotZ >= 0 && _rotZ <= 270) {
                        
                        _reticlePivot.rotation = Quaternion.Euler(0, 0, Mathf.Clamp(_rotZ, 0, 80));
                        _reticleCounterAngle = (90 - _rotZ) * 2;
                    } else if (_rotZ >= 270 && _rotZ < 360) {
                        _reticlePivot.rotation = Quaternion.Euler(0, 0, Mathf.Clamp(_rotZ, 280, 360));
                        _reticleCounterAngle = (_rotZ - 270) * 2;
                        
                    }

                } else if (_spriteHolder.localScale.x < 0) {
                    _reticlePivot.Rotate(new Vector3(0, 0, -Input.GetAxis(_ctrAim) * Time.deltaTime * 100));

                    _rotZ = _reticlePivot.eulerAngles.z;

                    if (_rotZ >= 90 && _rotZ <= 270) {
                        _reticlePivot.rotation = Quaternion.Euler(0, 0, Mathf.Clamp(_rotZ, 100, 260));
                        if (_rotZ <= 180) {
                            _reticleCounterAngle = (_rotZ - 90) * 2;
                        } else {
                            _reticleCounterAngle = (270 - _rotZ) * 2;
                        }
                    }
                }
                //Rotate Aim Reticle; ------End

                _anim.SetFloat("Speed", 0);
                speed = 0;
            } else {
                //Else if Veriticle Axis value is lesser than 0.75, do not rotate aim reticle;

                //Calculating Reticle Pivot counter angle;
                if (_rotZ >= 0 && _rotZ <= 270) {
                    _reticleCounterAngle = (90 - _rotZ) * 2;
                } else if (_rotZ >= 270 && _rotZ < 360) {
                    _reticleCounterAngle = (_rotZ - 270) * 2;
                }
                if (_rotZ >= 90 && _rotZ <= 270) {
                    if (_rotZ <= 180) {
                        _reticleCounterAngle = (_rotZ - 90) * 2;
                    } else {
                        _reticleCounterAngle = (270 - _rotZ) * 2;
                    }
                }
                //Calculating Reticle Pivot counter angle ------End

                //Horizontal Axis value greater than 0, move the player rightwards, lesser than 0, move the player leftwards;
                if (Input.GetAxis(_ctrMove) > 0) {
                    _anim.SetFloat("Speed", 1);
                    _anim.SetLayerWeight(1, 0);

                    //Detect the previous facing direction whether right or left;
                    FacingRight();

                } else if (Input.GetAxis(_ctrMove) < 0) {
                    _anim.SetFloat("Speed", 1);
                    _anim.SetLayerWeight(1, 1);

                    //Detect the previous facing direction whether right or left;
                    FacingLeft();
                } else {
                    _anim.SetFloat("Speed", 0);
                }
                speed = _speed;

                if (Mathf.Abs(Input.GetAxis(_ctrMove)) > 0) {
                    //_reticle.color = new Color(1, 1, 1, Mathf.Lerp(_reticle.color.a, 0, reticleFadeSpeed * Time.deltaTime));
                    _reticleAnim.SetBool("ShowReticle", false);
                }
            }

            if (Input.GetButtonDown(_ctrJump)) {
                _rig.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
            }
        } else {

            //_reticle.color = new Color(1, 1, 1, Mathf.Lerp(_reticle.color.a, 0, reticleFadeSpeed * Time.deltaTime));
            _reticleAnim.SetBool("ShowReticle", false);
            if (Input.GetAxis(_ctrMove) > 0) {
                _anim.SetFloat("Speed", 1);
                _anim.SetLayerWeight(1, 0);

                //Detect the previous facing direction whether right or left;
                FacingRight();

            } else if (Input.GetAxis(_ctrMove) < 0) {
                _anim.SetFloat("Speed", 1);
                _anim.SetLayerWeight(1, 1);

                //Detect the previous facing direction whether right or left;
                FacingLeft();
            } else {
                _anim.SetFloat("Speed", 0);
            }

            if (_rig.velocity.y > 0.5f || _rig.velocity.y < -0.5f) {
                _anim.SetFloat("Speed", 0);
            }
        }
        _rig.velocity = new Vector2(Input.GetAxis(_ctrMove) * speed * Time.deltaTime * 10, _rig.velocity.y);
        _anim.SetBool("OnGround", _isGround);

        isGround = _isGround;
        reticlePivot = _reticlePivot;

        moveAxis = Input.GetAxis(_ctrMove);
    }

    private void FixedUpdate() {

        //Get a raycast2D to detect is on the Ground or not;
        //0.6f is amount of offset on Y axis to avoid the ray collide with Character itself;
        //0.01f is the amount of distence for the ray. Character in the middle air, the ray will not collide with the Ground;
        //Give the _isGround value with both true or false to detect whether the character is on the ground or not. And will able to jump or not.
        RaycastHit2D _groudnHit = Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y - 0.6f), -Vector2.up, 0.01f);
        if (_groudnHit.collider != null) {
            if (_groudnHit.collider.gameObject.tag == "Ground") {
                _isGround = true;
            }
        } else if(_groudnHit.collider == null) {
            _isGround = false;
        }
        //Get a raycast2D to detect is on the Ground or not ------End
    }

    private void ResetCharacterAnim() {
        _reticleAnim.SetBool("ShowReticle", false);
        _anim.SetFloat("Speed", 0);
        _rig.velocity = new Vector3(0, _rig.velocity.y, 0);
        _anim.SetBool("OnGround", _isGround);
        _characterSwitched = false;
    }

    private void FacingRight() {
        if (_faceingRight == false) {
            if (_reticlePivot.eulerAngles.z <= 180) {
                _reticlePivot.rotation = Quaternion.Euler(0, 0, _reticlePivot.eulerAngles.z - _reticleCounterAngle);
            } else {
                _reticlePivot.rotation = Quaternion.Euler(0, 0, _reticlePivot.eulerAngles.z + _reticleCounterAngle);
            }
            reticlePivot = _reticlePivot;
            _faceingRight = true;
        }
    }

    private void FacingLeft() {
        if (_faceingRight == true) {
            if (_reticlePivot.eulerAngles.z <= 90) {
                _reticlePivot.rotation = Quaternion.Euler(0, 0, _reticlePivot.eulerAngles.z + _reticleCounterAngle);
            } else {
                _reticlePivot.rotation = Quaternion.Euler(0, 0, _reticlePivot.eulerAngles.z - _reticleCounterAngle);
            }
            reticlePivot = _reticlePivot;
            _faceingRight = false;
        }
    }
}
