using DG.Tweening;
using GondrLib.Dependencies;
using GondrLib.ObjectPool.Runtime;
using KHG.Effects.Particles;
using System.Collections;
using UnityEngine;

namespace KHG.Gimmicks.ExplosionCode
{
    public class ArtilleryExplosion : Explosion
    {
        [Inject] private PoolManagerMono poolManager;

        [SerializeField] private Vector3 offset;
        [SerializeField] private AudioSource whistle;
        [SerializeField] private AudioSource bomb;
        [SerializeField] private PoolingItemSO particleItem;

        private Vector3 _defaultWhistlePosition;

        private Vector3 ExplosionPoint => transform.position + offset;

        private void Start()
        {
            _defaultWhistlePosition = whistle.transform.position;
        }

        public void Activate(float delay)
        {
            StartCoroutine(DetonationRoutine(delay));
        }

        private IEnumerator DetonationRoutine(float delay)
        {
            yield return PlayWhistleSequence(delay);
            Explode(ExplosionPoint);
        }

        private IEnumerator PlayWhistleSequence(float delay)
        {
            float clipLength = whistle.clip.length;

            if (delay > clipLength)
            {
                yield return new WaitForSeconds(delay - clipLength);
            }

            PlayWhistle();

            yield return new WaitForSeconds(clipLength);

            PlayExplosionEffects();
        }

        private void PlayWhistle()
        {
            whistle.transform.position = _defaultWhistlePosition;
            whistle.Play();

            whistle.transform
                .DOMove(transform.position, whistle.clip.length)
                .SetEase(Ease.InExpo);
        }

        private void PlayExplosionEffects()
        {
            bomb.Play();

            var particle = poolManager.Pop<Explosion5>(particleItem);
            particle.Play(transform.position, transform.rotation, true, 4f);
        }

        private void OnValidate()
        {
            SetOffsetPosition(ExplosionPoint);
        }
    }
}