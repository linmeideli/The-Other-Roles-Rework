//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using TheOtherRoles.Utilities;
//using UnityEngine.UIElements;
//using UnityEngine;

//namespace TheOtherRoles.Objects
//{
//    internal class TemporRock
//    {
//        public static List<TemporRock> rock = new();

//        private static int instanceCounter;

//        private static Sprite trapSprite;
//        public int instanceId;
//        public GameObject GOrock;

//        public Rock(Vector2 p)
//        {
//            GOrock = new GameObject("Trap") { layer = 11 };
//            var position = new Vector3(p.x, p.y, p.y / 1000 + 0.001f); // just behind player
//            GOrock.transform.position = position;

//            var trapRenderer = GOrock.AddComponent<SpriteRenderer>();
//            trapRenderer.sprite = getTrapSprite();
//            ////GOrock.SetActive(true);
//            FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(5, new Action<float>(x =>
//            {
//                if (x == 1f)
//                {
//                    triggerable = true;
//                    trapRenderer.color = Color.white;
//                }
//            })));
//        }
//        public static Sprite getTrapSprite()
//        {
//            if (trapSprite) return trapSprite;
//            trapSprite = Helpers.loadSpriteFromResources("Rock.png", 100f);
//            return trapSprite;
//        }
//    }
//}
