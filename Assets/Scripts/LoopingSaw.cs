using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skymare
{
    public class LoopingSaw : MonoBehaviour
    {
        [SerializeField] private Transform _saw;
        [SerializeField] private Transform _mover;
        [SerializeField] private float _radius;
        [SerializeField] private bool _isVertical;
        [SerializeField] private float _speed;

        private float _currentSpeed;

        // Start is called before the first frame update
        void Start()
        {
            _currentSpeed = _speed;
        }

        // Update is called once per frame
        void Update()
        {

            _saw.Rotate(Vector3.forward * Time.deltaTime * -100f);
            if (_isVertical)
            {
                _mover.Translate(Vector3.up * _currentSpeed * Time.deltaTime);

                if (_mover.localPosition.y > _radius)
                    _currentSpeed = -_speed;
                if (_mover.localPosition.y < -_radius)
                    _currentSpeed = _speed;
            }
            else
            {
                _mover.Translate(Vector3.right * _currentSpeed * Time.deltaTime);
                if (_mover.localPosition.x > _radius)
                    _currentSpeed = -_speed;
                if (_mover.localPosition.x < -_radius)
                    _currentSpeed = _speed;
            }
        }
    }
}