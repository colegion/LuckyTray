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

        public void AnimateParticle(Vector3 target, Action onComplete)
        {
            trailParticle.Play();
            transform.DOMove(target, 0.5f).SetEase(Ease.OutBounce).OnComplete(() =>
            {
                ResetSelf();
                onComplete?.Invoke();
            });
        }

        private void ResetSelf()
        {
            rewardField.enabled = false;
            trailParticle.Stop();
            transform.position = Vector3.zero;
        }
    }
}
