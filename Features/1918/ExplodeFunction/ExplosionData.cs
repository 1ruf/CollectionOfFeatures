using Alchemy.Inspector;
using UnityEngine;

namespace KHG.Gimmicks.ExplosionCode
{
    [CreateAssetMenu(fileName = "ExplosionData", menuName = "SO/Obstacle/Explosion/ExplosionData")]
    public class ExplosionDataSO : ScriptableObject
    {
        public LayerMask ExplodeableLayer;
        public int MaxDamage;
        public int MaxPressure;
        [FoldoutGroup("Impact Level")]
        public float lv1;
        [FoldoutGroup("Impact Level")]
        public float lv2;
        [FoldoutGroup("Impact Level")]
        public float lv3;
        [FoldoutGroup("Impact Level")]
        public float lv4;
    }
}