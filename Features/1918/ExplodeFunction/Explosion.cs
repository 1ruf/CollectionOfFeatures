using _01_Work.LCM._01.Scripts.Core;
using _01_Work.LCM._01.Scripts.Events;
using Assets._01_Work.YHB.Scripts.Casters;
using UnityEngine;

namespace KHG.Gimmicks.ExplosionCode
{
    public abstract class Explosion : MonoBehaviour
    {
        [SerializeField] private ExplosionDataSO explosionData;
        [SerializeField] private ExplosionVelocityDataSO velocityData;
        [SerializeField] private GameEventChannelSO playerChannel;
        [SerializeField] private OverlapCaster explodeCaster;

        [SerializeField] protected LayerMask playerLayer;

        private int _deadCall;
        private Vector3 _explosionPoint;

        protected void Explode(Vector3 point)
        {
            _explosionPoint = point;

            Collider[] targets = Physics.OverlapSphere(_explosionPoint, explosionData.lv4);
            ProcessTargets(targets);

            explodeCaster?.SetSize(explosionData.lv3);
            explodeCaster?.Cast();
        }

        protected void SetExplosionPoint(Vector3 point)
        {
            _explosionPoint = point;
        }

        private void ProcessTargets(Collider[] targets)
        {
            LayerMask explodeLayer = explosionData.ExplodeableLayer;

            foreach (var col in targets)
            {
                if (!IsVisible(col)) continue;

                float distance = GetDistance(col);
                int layerBit = 1 << col.gameObject.layer;

                if (IsPlayer(layerBit))
                {
                    HandlePlayer(distance);
                    break;
                }

                if (!IsExplodable(layerBit, explodeLayer)) continue;
                if (!col.TryGetComponent(out Rigidbody rigid)) continue;

                ApplyExplosionForce(rigid, distance);
            }
        }

        private bool IsVisible(Collider col)
        {
            Vector3 direction = (col.ClosestPoint(_explosionPoint) - _explosionPoint).normalized;

            if (Physics.Raycast(_explosionPoint, direction, out RaycastHit hit, explosionData.lv4))
            {
                return hit.collider == col;
            }

            return true;
        }

        private float GetDistance(Collider col)
        {
            return Vector3.Distance(transform.position, col.transform.position);
        }

        private bool IsPlayer(int layerBit)
        {
            return (layerBit & playerLayer) != 0;
        }

        private bool IsExplodable(int layerBit, LayerMask explodeLayer)
        {
            return (layerBit & explodeLayer) != 0;
        }

        private void HandlePlayer(float distance)
        {
            GameEvent evt = null;

            if (distance <= explosionData.lv2 && _deadCall == 0)
            {
                evt = PlayerChannel.PlayerDeadEvent.Initialize(_explosionPoint, 5);
                _deadCall++;
            }
            else if (distance <= explosionData.lv3)
            {
                evt = PlayerChannel.PlayerBlurEvent.Initialize(7.5f);
            }

            if (evt != null)
            {
                playerChannel.RaiseEvent(evt);
            }
        }

        private void ApplyExplosionForce(Rigidbody rigid, float distance)
        {
            if (velocityData == null)
            {
                Debug.LogError($"{gameObject.name}:ExplosionVelocity가 필요합니다!");
                return;
            }

            float multiplier = GetForceMultiplier(distance);
            AddForce(rigid, multiplier);
        }

        private float GetForceMultiplier(float distance)
        {
            if (distance <= explosionData.lv2) return velocityData.VelocityLv2;
            if (distance <= explosionData.lv3) return velocityData.VelocityLv3;
            if (distance <= explosionData.lv4) return velocityData.VelocityLv4;

            return velocityData.VelocityLv1;
        }

        private void AddForce(Rigidbody rigid, float multiplier)
        {
            float power = explosionData.MaxPressure * multiplier;

            Vector3 direction = rigid.transform.position - transform.position;
            Vector3 upward = Vector3.up * (5f * power);

            rigid.AddForce(power * direction + upward);
            rigid.AddTorque(power * direction);
        }

        protected virtual void SetOffsetPosition(Vector3 position)
        {
            _explosionPoint = position;
        }

        protected virtual void OnDrawGizmosSelected()
        {
            if (explosionData == null)
            {
                Debug.LogError($"{gameObject.name}:ExplosionData가 필요합니다!");
                return;
            }

            DrawGizmo(explosionData.lv1, Color.red);
            DrawGizmo(explosionData.lv2, new Color(1f, 0.27f, 0f));
            DrawGizmo(explosionData.lv3, Color.yellow);
            DrawGizmo(explosionData.lv4, new Color(0.96f, 0.96f, 0.96f));
        }

        private void DrawGizmo(float radius, Color color)
        {
            Gizmos.color = color;
            Gizmos.DrawWireSphere(_explosionPoint, radius);
        }
    }
}