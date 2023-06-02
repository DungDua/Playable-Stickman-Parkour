using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using DG.Tweening;

namespace Skymare
{
    public class InitSkinIntro : MonoBehaviour
    {
        public enum Skin
        {
            Orange, Red, Cyan,Yellow,Green
        }


        [SerializeField] private Skin _mySkin;
        private SkeletonAnimation _skeleton;

        private float _speed;
        private float _beginX;
        private bool _targeted;

        // Start is called before the first frame update
        void Start()
        {
            _skeleton = this.GetComponent<SkeletonAnimation>();
            switch (_mySkin)
            {
                case Skin.Orange:
                    _skeleton.Skeleton.SetColor(new Color(1f, 0.394f, 0f, 1f));
                    break;
                case Skin.Red:
                    _skeleton.Skeleton.SetColor(Color.red);
                    break;
                case Skin.Cyan:
                    _skeleton.Skeleton.SetColor(new Color(37f / 255f, 193 / 255f, 1f, 1f));
                    break;
                case Skin.Yellow:
                    _skeleton.Skeleton.SetColor(Color.yellow);
                    break;
                case Skin.Green:
                    _skeleton.Skeleton.SetColor(Color.green);
                    break;
            }

        }

       

    }
}