using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Utilities
{
    public class WalletTrailObject : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer rewardField;
        [SerializeField] private ParticleSystem trailParticle;
        [SerializeField] private Transform worldTarget;
        [SerializeField] private float tweenDuration;
        [SerializeField] private float defaultScale;
        [SerializeField] private float punchScale;
        [SerializeField] private AnimationCurve moveCurve;

        public void ConfigureSelf(RewardConfig rewardConfig)
        {
            rewardField.sprite = rewardConfig.rewardSprite;
            rewardField.enabled = true;
            var main = trailParticle.main;
            var renderer = trailParticle.GetComponent<ParticleSystemRenderer>();
            Material material = new Material(Shader.Find("Sprites/Default"));
            material.mainTexture = rewardConfig.rewardSprite.texture;
            renderer.material = material;
        }

        public void AnimateParticle(Action onComplete)
        {
            float insertDuration = 0;
            Sequence sequence = DOTween.Sequence();
            sequence.InsertCallback(insertDuration, () =>
            {
                transform.DOScale(new Vector3(defaultScale, defaultScale, defaultScale), tweenDuration).SetEase(Ease.Linear);
            });
            insertDuration += tweenDuration;
            sequence.InsertCallback(insertDuration, () =>
            {
                transform.DOPunchScale(new Vector3(punchScale, punchScale, punchScale), tweenDuration).SetEase(Ease.Linear);
            });
            insertDuration += tweenDuration;
            sequence.InsertCallback(insertDuration, () =>
            {
                trailParticle.gameObject.SetActive(true);
                trailParticle.Play();
            });
            sequence.AppendInterval(trailParticle.main.duration);
            sequence.Append(transform.DOMove(worldTarget.position, .75f).SetEase(moveCurve));
            sequence.AppendCallback(() =>
            {
                ResetSelf();
                onComplete?.Invoke();
            });
        }

        private void ResetSelf()
        {
            rewardField.enabled = false;
            rewardField.enabled = false;
            trailParticle.gameObject.SetActive(false);
            trailParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            transform.position = Vector3.zero;
            transform.localScale = Vector3.zero;
        }
    }
}
