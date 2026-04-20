using GondrLib.Dependencies;
using GondrLib.ObjectPool.Runtime;
using KHG.Effects.Particles;
using System.Collections.Generic;
using UnityEngine;

namespace KHG.Gimmicks.ExplosionCode
{
    [RequireComponent(typeof(LineRenderer))]
    public class BoobyTrap : Explosion
    {
        [Inject] private PoolManagerMono poolManager;

        [SerializeField] private List<Transform> points = new();
        [SerializeField] private float detectDistance = 100f;
        [SerializeField] private PoolingItemSO particleItem;

        private LineRenderer _lineRenderer;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            InitializeLine();
        }

        private void Update()
        {
            if (!IsValidPoints()) return;
            DetectPlayer();
        }

        private void InitializeLine()
        {
            _lineRenderer.enabled = true;
            _lineRenderer.useWorldSpace = true;
            UpdateLinePositions();
        }

        private void UpdateLinePositions()
        {
            if (!IsValidPoints()) return;

            _lineRenderer.positionCount = points.Count;

            for (int i = 0; i < points.Count; i++)
            {
                if (points[i] != null)
                {
                    _lineRenderer.SetPosition(i, points[i].position);
                }
            }
        }

        private bool IsValidPoints()
        {
            return points != null && points.Count >= 2;
        }

        private void DetectPlayer()
        {
            for (int i = 0; i < points.Count - 1; i++)
            {
                if (!TryGetSegment(points[i], points[i + 1], out Vector3 start, out Vector3 dir, out float dist))
                    continue;

                if (Physics.Raycast(start, dir, out RaycastHit hit, dist, playerLayer))
                {
                    TriggerExplosion();
                    break;
                }
            }
        }

        private bool TryGetSegment(Transform a, Transform b, out Vector3 start, out Vector3 dir, out float dist)
        {
            start = Vector3.zero;
            dir = Vector3.zero;
            dist = 0f;

            if (a == null || b == null) return false;

            start = a.position;
            Vector3 end = b.position;
            dir = (end - start).normalized;
            dist = Vector3.Distance(start, end);

            return true;
        }

        private void TriggerExplosion()
        {
            Vector3 pos = transform.position;

            Explode(pos);
            SpawnEffect(pos);
            RemoveSelf();
        }

        private void SpawnEffect(Vector3 position)
        {
            var particle = poolManager.Pop<Explosion1>(particleItem);
            particle.Play(position, transform.rotation, true, 5f);
        }

        private void RemoveSelf()
        {
            Destroy(gameObject);
        }

        private void OnDrawGizmos()
        {
            if (!IsValidPoints()) return;

            Gizmos.color = Color.red;

            for (int i = 0; i < points.Count - 1; i++)
            {
                if (points[i] == null || points[i + 1] == null) continue;
                Gizmos.DrawLine(points[i].position, points[i + 1].position);
            }
        }

        private void OnValidate()
        {
            SetExplosionPoint(transform.position);
        }
    }
}