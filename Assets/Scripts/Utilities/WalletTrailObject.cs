using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Utilities
{
    public class WalletTrailObject : MonoBehaviour
    {
        [SerializeField] private Image rewardField;
        [SerializeField] private ParticleSystem trailParticle;

        public void ConfigureSelf(RewardConfig rewardConfig)
        {
            rewardField.sprite = rewardConfig.rewardSprite;
            var main = trailParticle.main;
            var renderer = trailParticle.GetComponent<ParticleSystemRenderer>();
            Material material = new Material(Shader.Find("Sprites/Default"));
            material.mainTexture = rewardConfig.rewardSprite.texture;
            renderer.material = material;
        }

        public void AnimateParticle(RectTransform target, Action onComplete)
        {
            trailParticle.Play();
            transform.DOLocalMove(target.anchoredPosition, 0.5f).SetEase(Ease.OutBounce).OnComplete(() =>
            {
                onComplete?.Invoke();
            });
        }
    }
}
