using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;

namespace Skymare
{
    public class Utils 
    {
        public static void OnInitSkin(Skeleton skeleton,int charID,int wpID)
        {
            //    var skeleton = baseChar.Skeleton;
            var skeletonData = skeleton.Data;
            Skin characterSkin = new Skin("character-base");

            //Bell
            string character = $"Char/B{charID}";
            characterSkin.AddSkin(skeletonData.FindSkin(character));

            // Body
            string wepon = $"Weapon/S{wpID}";
            characterSkin.AddSkin(skeletonData.FindSkin(wepon));

            skeleton.SetSkin(characterSkin);
            skeleton.SetSlotsToSetupPose();
        }
    }
}