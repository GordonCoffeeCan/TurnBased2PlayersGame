using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour {

    public static bool playerShowed;

    private GameObject _currentPlayer;

    private Camera _camera;
    private float _speed = 5;
    private float _camZoomSpeed = 30;
    private bool _levelShowed = false;

    private float _minCamOrthographicSize = 5;
    private float _maxCamOrthographicSize = 0;
    private float _zoomCam;

    private string _zoomCtrName;

    private enum CameraState {
        Null,
        FocusingOnLevel,
        FocusingOnPlayer,
        FocusingOnObject,
        InControl
    }

    private CameraState _currentCameraState;

    private void Awake() {
        _camera = Camera.main;
    }

    // Use this for initialization
    void Start () {
        _zoomCam = 0;
        playerShowed = false;
        _levelShowed = false;

        _currentCameraState = CameraState.Null;
    }

    // Update is called once per frame
    void Update () {
        if (GameData.isHumanTurn == true) {
            _currentPlayer = LevelLoader2D.playerHuman;
            _zoomCtrName = "JoyZoom";
        } else {
            _currentPlayer = LevelLoader2D.playerZombie;
            _zoomCtrName = "ZomZoom";
        }

        if (_levelShowed == false) {
            Invoke("ShowLevel", 0.8f); // Show Level
        } else if(_levelShowed == true) {
            Invoke("CameraInGame", 2); //Level showed, now focus on Game;
        }

        _maxCamOrthographicSize = LevelLoader2D._offSetX / 4;

        switch (_currentCameraState) {
            case CameraState.FocusingOnLevel:
                FocusCamera(Vector3.zero, _maxCamOrthographicSize);
                break;
            case CameraState.FocusingOnPlayer:
                FocusCamera(_currentPlayer.transform.position, _minCamOrthographicSize - 0.2f);
                break;
            case CameraState.FocusingOnObject:
                break;
            case CameraState.InControl:
                FocusCamera(_currentPlayer.transform.position, ZoomCamera(_zoomCtrName));
                break;
            default:
                break;
        }
    }

    private void ShowLevel() {
        _currentCameraState = CameraState.FocusingOnLevel;
        _levelShowed = true;
    }

    private void CameraInGame() {
        _currentCameraState = CameraState.FocusingOnPlayer;

        if (playerShowed == false) {
            if (_camera.orthographicSize < _minCamOrthographicSize) {
                playerShowed = true;
            }
        }

        if (playerShowed == true) {
            _currentCameraState = CameraState.InControl;
        }

        _zoomCam = _camera.orthographicSize;
    }

    private float ZoomCamera(string _axisName) {
        if (Mathf.Abs(Input.GetAxis(_axisName)) > 0.3f){
            _zoomCam = Mathf.Lerp(_zoomCam, _zoomCam += Input.GetAxis(_axisName), _camZoomSpeed * Time.deltaTime);
            _zoomCam = Mathf.Clamp(_zoomCam, _minCamOrthographicSize, _maxCamOrthographicSize);
        }
        return _zoomCam;
    }

    private void FocusCamera(Vector3 _position, float _targetCamSize) {
        _camera.transform.position = new Vector3(Mathf.Lerp(_camera.transform.position.x, _position.x, _speed * Time.deltaTime), Mathf.Lerp(_camera.transform.position.y, _position.y, _speed * Time.deltaTime), _camera.transform.position.z);
        _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, _targetCamSize, _speed * Time.deltaTime);
    }
}
