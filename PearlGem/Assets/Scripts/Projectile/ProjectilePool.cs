using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PearlGem.ColorPlanet;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace PearlGem.Projectile
{
    public class ProjectilePool : MonoBehaviour
    {
        [SerializeField] Projectile projectilePrefab;
        [SerializeField] Transform spawnPoint;
        [SerializeField] Transform spawnPointAlternative;
        [SerializeField] bool allowProjectileSwap = true;
        [SerializeField] Button swapButton;
        
        ObjectPool<Projectile> _projectilePool;
        
        List<ColorBallCount> _projectileInventory = new List<ColorBallCount>();
        List<Projectile> _availableProjectiles = new List<Projectile>();

        Projectile _activeProjectile;
        Projectile _alternativeProjectile;
        
        Color? _lastFiredColor;
        int _totalProjectileCount;
        
        public void SetBallInventory(List<ColorBallCount> inventory, float ballRadius)
        {
            _projectileInventory = inventory;
            _totalProjectileCount = _projectileInventory.Sum(item => item.Count);
            GenerateProjectiles(ballRadius);
            SetInitialProjectiles();
        }
        
        void GenerateProjectiles(float ballRadius)
        {
            if (_projectilePool == null)
            {
                _projectilePool = new ObjectPool<Projectile>(
                    createFunc: () =>
                    {
                        Projectile newProj = Instantiate(projectilePrefab);
                        newProj.gameObject.SetActive(false);
                        return newProj;
                    },
                    actionOnGet: (proj) => { proj.gameObject.SetActive(true); },
                    actionOnRelease: (proj) => { proj.gameObject.SetActive(false); },
                    actionOnDestroy: (proj) => { Destroy(proj.gameObject); },
                    collectionCheck: false,
                    defaultCapacity: _totalProjectileCount,
                    maxSize: _totalProjectileCount
                );
            }
            else
            {
                foreach (var proj in _availableProjectiles)
                {
                    _projectilePool.Release(proj);
                }
                _availableProjectiles.Clear();
            }
            
            foreach (var item in _projectileInventory)
            {
                for (int i = 0; i < item.Count; i++)
                {
                    Projectile proj = _projectilePool.Get();
                    proj.SetColor(item.Color);
                    proj.transform.localScale = Vector3.one * ballRadius * 2;
                    proj.transform.position = spawnPoint.position;
                    proj.gameObject.SetActive(false);
                    _availableProjectiles.Add(proj);
                }
            }
            
            _availableProjectiles = _availableProjectiles.OrderBy(x => Random.value).ToList();
        }
        
        void SetInitialProjectiles()
        {
            _activeProjectile = GetNextProjectile(_lastFiredColor);
            if (_activeProjectile != null)
            {
                _availableProjectiles.Remove(_activeProjectile);
                _activeProjectile.gameObject.SetActive(true);
                _activeProjectile.transform.position = spawnPoint.position;
            }

            if (allowProjectileSwap)
            {
                _alternativeProjectile = GetAlternativeProjectile(_activeProjectile);
                if (_alternativeProjectile != null)
                {
                    _availableProjectiles.Remove(_alternativeProjectile);
                    _alternativeProjectile.gameObject.SetActive(true);
                    _alternativeProjectile.transform.position = spawnPointAlternative.position;
                }
            }
        }
        
        Projectile GetNextProjectile(Color? lastColor)
        {
            if (_availableProjectiles.Count == 0)
            {
                StartCoroutine(TriggerLevelLoseCoroutine());
                return null;
            }

            if (lastColor == null)
                return _availableProjectiles[0];

            var candidate = _availableProjectiles.FirstOrDefault(p => p.Color != lastColor.Value);
            return candidate ?? _availableProjectiles[0];
        }
        
        Projectile GetAlternativeProjectile(Projectile active)
        {
            if (active == null || _availableProjectiles.Count == 0)
                return null;

            var candidate = _availableProjectiles.FirstOrDefault(p => p.Color != active.Color);
            return candidate;
        }
        
        public void Shot(Vector3 shotVector)
        {
            if (_activeProjectile == null)
                return;

            if (_activeProjectile.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.isKinematic = false;
                rb.AddForce(shotVector, ForceMode.Impulse);
            }
            
            _lastFiredColor = _activeProjectile.Color;
            
            _activeProjectile = _alternativeProjectile;
            _alternativeProjectile = null;

            if (_activeProjectile != null)
            {
                _activeProjectile.transform.position = spawnPoint.position;
            }
            else
            {
                _activeProjectile = GetNextProjectile(_lastFiredColor);
                if (_activeProjectile != null)
                {
                    _availableProjectiles.Remove(_activeProjectile);
                    _activeProjectile.gameObject.SetActive(true);
                    _activeProjectile.transform.position = spawnPoint.position;
                }
            }

            if (allowProjectileSwap)
            {
                _alternativeProjectile = GetAlternativeProjectile(_activeProjectile);
                if (_alternativeProjectile != null)
                {
                    _availableProjectiles.Remove(_alternativeProjectile);
                    _alternativeProjectile.gameObject.SetActive(true);
                    _alternativeProjectile.transform.position = spawnPointAlternative.position;
                }
            }

            if (_activeProjectile == null)
            {
                GlobalEventBus.Instance.TriggerProjectilesAreOver();
            }
        }
        
        public void SwapProjectiles()
        {
            if (!allowProjectileSwap || _alternativeProjectile == null)
                return;

            (_activeProjectile, _alternativeProjectile) = (_alternativeProjectile, _activeProjectile);

            _activeProjectile.transform.position = spawnPoint.position;
            _alternativeProjectile.transform.position = spawnPointAlternative.position;
        }
        
        IEnumerator TriggerLevelLoseCoroutine()
        {
            yield return new WaitForSeconds(3f);
            
            GlobalEventBus.Instance.TriggerLevelLose();
        }
        
        // void OnEnable()
        // {
        //     swapButton.onClick.AddListener(SwapProjectiles);
        // }
        //
        // void OnDisable()
        // {
        //     swapButton.onClick.RemoveAllListeners();
        // }
    }
}
