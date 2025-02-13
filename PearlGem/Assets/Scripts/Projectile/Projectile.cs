using System;
using PearlGem.ColorPlanet;
using PearlGem.Utils;
using UnityEngine;

namespace PearlGem.Projectile
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] GameObject hitEffect;
        [SerializeField] GameObject missEffect;
        [SerializeField] AudioClip spawnSound;
        [SerializeField] AudioClip hitSound;
        public Color Color { get; private set; }

        void Start()
        {
            SoundFXManager.Instance.PlayAudioClip(spawnSound, transform.position);
        }

        public void SetColor(Color color)
        {
            Color = color;
            GetComponent<Renderer>().material.SetColor("_BaseColor", color);
        }
        
        void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent<IColorTarget>(out var colorTarget))
            {
                if (colorTarget.TryToDrop(Color))
                {
                    if (hitEffect != null) Instantiate(hitEffect, transform.position, Quaternion.identity);
                }
                else
                {
                    var offset = new Vector3(0, 0, -0.3f);
                    if (missEffect != null) Instantiate(missEffect, transform.position + offset, Quaternion.identity);
                }
            }
            

            SoundFXManager.Instance.PlayAudioClip(hitSound, transform.position);
            
            Destroy(gameObject);
        }
        
    }
}
