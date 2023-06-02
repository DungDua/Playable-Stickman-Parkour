using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Skymare
{
    public class MenuCtrl : MonoBehaviour
    {
        private static MenuCtrl _instance;
        public static MenuCtrl Instance => _instance;

        [SerializeField] private UIButton _btnStart;
        [SerializeField] private List<InitSkinIntro> _listCharacter;
        [SerializeField] private Transform _port;


        // Start is called before the first frame update
        private void Awake()
        {
            _instance = this;
        }

        void Start()
        {

        }


        private IEnumerator IEShowStart()
        {
            yield return new WaitForSeconds(3f);
            _btnStart.gameObject.SetActive(true);
        }


    }
}