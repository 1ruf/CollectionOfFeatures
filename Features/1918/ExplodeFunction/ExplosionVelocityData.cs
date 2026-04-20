using UnityEngine;

namespace KHG.Gimmicks.ExplosionCode
{
    [CreateAssetMenu(fileName = "ExplosionVelocity", menuName = "SO/Obstacle/Explosion/ExplosionVelocity")]
    public class ExplosionVelocityDataSO : ScriptableObject
    {
        public float VelocityLv1;
        public float VelocityLv2;
        public float VelocityLv3;
        public float VelocityLv4;
    }
}