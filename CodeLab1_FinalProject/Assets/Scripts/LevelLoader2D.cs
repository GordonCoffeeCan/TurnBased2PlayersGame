using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelLoader2D : MonoBehaviour {

    private string _filePath;
    private string _fileName = "Level01.txt";
    private GameObject _levelHolder;
    private StreamReader _streamReader;
    private string _docLine;
    private int _posY;
    private float _posScale;

    public static GameObject playerHuman;
    public static GameObject playerZombie;

    public static float offSetX;
    public static float offSetY;

	// Use this for initialization
	void Start () {
        _levelHolder = new GameObject("Level Holder");
        _filePath = Application.dataPath + "/" + _fileName;
        _streamReader = new StreamReader(_filePath);
        offSetX = 0;
        offSetY = 0;
        SetLevel();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void SetLevel() {
        int _lineNumber = 0;
        while (!_streamReader.EndOfStream) {
            
            _docLine = _streamReader.ReadLine();

            for (int _posX = 0; _posX < _docLine.Length; _posX++) {
                switch (_docLine[_posX]) {
                    case 'x':
                        SetObject("Wall", _posX);
                        break;
                    case 'c':
                        SetObject("Spike", _posX);
                        break;
                    case 'p':
                        SetObject("Human", _posX);
                        break;
                    case 'z':
                        SetObject("Zombie", _posX);
                        break;
                }

                if (_lineNumber == 0) {
                    offSetX += _posScale;
                }
            }
            offSetY += _posScale;
            _posY--;
            _lineNumber++;
        }
        _streamReader.Close();

        _levelHolder.transform.position = new Vector3(-offSetX / 2 + _posScale / 2, offSetY / 2 - _posScale / 2, 0);
        playerHuman.transform.parent = null;
        playerZombie.transform.parent = null;

    }

    private void SetObject(string _name, int _posX) {
        GameObject _gameObject = Instantiate(Resources.Load("Prefabs/" + _name)) as GameObject;
        _posScale = _gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
        if (_name == "Human") {
            playerHuman = _gameObject;
        } else if (_name == "Zombie") {
            playerZombie = _gameObject;
        }
        _gameObject.transform.parent = _levelHolder.transform;
        _gameObject.transform.position = new Vector3(_posX * _posScale, _posY * _posScale, 0);
    }
}
