using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using UnityEngine.UI;

namespace Skymare
{

    public class TrySkinCtrl : MonoBehaviour
    {
        public UnlockTypes Unlock;
        public ParticleSystem FxBuff;
        public string SkinID;
        public HeroType Hero;

        [SerializeField] private SkeletonAnimation _skelSkin;
        [SerializeField] private GameObject _fxCharge;
        [SerializeField] private GameObject _board;
        [SerializeField] private Transform _player;


        // Start is called before the first frame update
        void Start()
        {
            if (this.AllSkinHasUnlock())
            {
                this.gameObject.SetActive(false);
                return;
            }

            skin: int id = Random.Range(1, 67);
            Hero = (HeroType)id;
            bool IsOpen = PlayerPrefs.GetInt(Const.KEY_HERO_OPEN + Hero.ToString(), 0) != 0;
            if (IsOpen) goto skin;

            SkinID = "Skin" + (id + 2);
            _skelSkin.Skeleton.SetSkin(SkinID);
            _skelSkin.Skeleton.SetSlotsToSetupPose();
            _skelSkin.LateUpdate();
            _skelSkin.Skeleton.SetColor(Color.white);
           
        }

        private void Update()
        {
            if(_player != null)
            {
                float x = _player.position.x - this.transform.position.x;
                float euler = x > 0 ? 0f : 180f;
                this._skelSkin.transform.eulerAngles = Vector3.up * euler;
            }
        }


        public void OnChangeSkin()
        {
            _skelSkin.gameObject.SetActive(false);
            _fxCharge.SetActive(false);
            this.GetComponent<Collider2D>().enabled = false;
            _board.SetActive(false);
        }

        private bool AllSkinHasUnlock()
        {
            for(int i = 0; i < 67; i++)
            {
                HeroType hero = (HeroType)i;
                if (PlayerPrefs.GetInt(Const.KEY_HERO_OPEN + hero.ToString(), 0) == 0)
                    return false;
            }

            return true;
        }


    }
}