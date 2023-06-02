using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skymare
{
    public class MoveToZoom : MonoBehaviour
    {
        [SerializeField] private Transform _stickMan;
        [SerializeField] private Transform _parallax;
        [SerializeField] private float _speed;

        private Vector3 _oldPosition,_beginParallax;
        private float _camSize,_targetSize;
        private Camera _myCam;
        private Vector3 _beginScale;

        // Start is called before the first frame update
        void Start()
        {
            if (_stickMan == null || _parallax == null) return;
            _myCam = this.GetComponent<Camera>();
            _camSize = _targetSize = _myCam.orthographicSize;
            _oldPosition = _stickMan.position;
            _beginParallax = _parallax.position;
            _beginScale = _parallax.localScale;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if (_stickMan == null || _parallax == null) return;
            if(_stickMan.position != _oldPosition)
            {
                _targetSize = 6f;
                _oldPosition = _stickMan.position;
                float x = this.transform.position.x * _speed;
                _parallax.localScale = Vector3.Lerp(_parallax.localScale, _beginScale * 0.9f, Time.deltaTime);
                Vector3 pos = _beginParallax + new Vector3(x, 0f, 0f);
                _parallax.position = Vector3.Lerp(_parallax.position, pos, Time.deltaTime * 2f);
            }
            else
            {
                _targetSize = 5f;
                _parallax.localScale = Vector3.Lerp(_parallax.localScale, _beginScale , Time.deltaTime);
            }

            _camSize = Mathf.Lerp(_camSize, _targetSize, Time.deltaTime * 2f);
            _myCam.orthographicSize = _camSize;
        }
    }
}