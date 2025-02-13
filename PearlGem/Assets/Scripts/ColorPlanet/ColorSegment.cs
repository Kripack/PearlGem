using System.Collections;
using UnityEngine;

namespace PearlGem.ColorPlanet
{
    [RequireComponent(typeof(Rigidbody))]
    public class ColorSegment : MonoBehaviour, IColorTarget
    {
        [SerializeField] GameObject destructionEffect;
        [SerializeField] float dropDelay = 0.2f;
        [SerializeField] float neighborSearchRadiusMultiplier = 0.2f;
        
        bool _hasDropped;
        
        Color _color;
        ColorCluster _cluster;
        Rigidbody _rb;
        
        void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        public void SetColorCluster(ColorCluster cluster)
        {
            _cluster = cluster;
            _color = cluster.Color;
            GetComponent<Renderer>().material.SetColor("_BaseColor", cluster.Color);
        }
        
        public bool TryToDrop(Color projectileColor)
        {
            if (projectileColor == _color)
            {
                DropChainReaction();
                return true;
            }

            return false;
        }

        void DropChainReaction()
        {
            if (_hasDropped) return;

            _hasDropped = true;
            StartCoroutine(DropChainCoroutine());
        }

        IEnumerator DropChainCoroutine()
        {
            DropSegment();
            
            yield return new WaitForSeconds(dropDelay);
            
            Collider[] hits = Physics.OverlapSphere(transform.position, transform.localScale.x * neighborSearchRadiusMultiplier);
            foreach (Collider col in hits)
            {
                if (col.gameObject.TryGetComponent<ColorSegment>(out var neighbor))
                {
                    if (neighbor == this) continue;
                    if (neighbor._color == _color && neighbor._cluster == _cluster) neighbor.DropChainReaction();
                }
            }
        }
        
        void DropSegment()
        {
            if (destructionEffect != null)
            {
                Instantiate(destructionEffect, transform.position, Quaternion.identity);
            }
            
            _rb.isKinematic = false;
            GlobalEventBus.Instance.TriggerScoreIncrease(1);
        }
    }
}