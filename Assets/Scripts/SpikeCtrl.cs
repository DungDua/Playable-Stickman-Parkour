using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skymare
{
    public class SpikeCtrl : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _fx;

        private Collider2D _trapBox;

        // Start is called before the first frame update
        void Start()
        {
            _trapBox = this.GetComponent<Collider2D>();
            StartCoroutine(IEShowTrap());
        }



        private IEnumerator IEShowTrap()
        {
            _fx.Play();
            _trapBox.enabled = true;
            yield return new WaitForSeconds(3f);
            _trapBox.enabled = false;
            yield return new WaitForSeconds(4f);
            StartCoroutine(IEShowTrap());
        }

    }
}