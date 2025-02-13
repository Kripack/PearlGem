using System.Collections;
using PearlGem.Projectile;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace PearlGem
{
    [RequireComponent(typeof(ProjectilePool))]
    public class AimController : MonoBehaviour
    {
        [Header("AimSettings")]
        [SerializeField] Transform launchPoint;
        [SerializeField] TrajectoryPrediction trajectoryPrediction;
        [SerializeField] float maxDragDistance = 3f;
        [SerializeField] float forceMultiplier = 500f;
        [SerializeField] int shotsAvailable = 10;
        
        bool _isAiming;
        Vector3 _dragVector;
        
        Camera _mainCamera;
        ProjectilePool _projectilePool;
        InputSystem_Actions _playerInput;
        
        void Awake()
        {
            _mainCamera = Camera.main;
            _projectilePool = GetComponent<ProjectilePool>();
            _playerInput = new ();
        }
        
        void OnEnable()
        {
            _playerInput.Enable();
            _playerInput.Player.TouchPress.started += OnPress;
            _playerInput.Player.TouchPress.canceled += OnRelease;
            _playerInput.Player.TouchPosition.performed += OnDrag;
            
            _playerInput.Player.MousePress.started += OnPress;
            _playerInput.Player.MousePress.canceled += OnRelease;
            _playerInput.Player.MousePosition.performed += OnDrag;
        }
        
        void OnDisable()
        {
            _playerInput.Player.TouchPress.started -= OnPress;
            _playerInput.Player.TouchPress.canceled -= OnRelease;
            _playerInput.Player.TouchPosition.performed -= OnDrag;
            
            _playerInput.Player.MousePress.started -= OnPress;
            _playerInput.Player.MousePress.canceled -= OnRelease;
            _playerInput.Player.MousePosition.performed -= OnDrag;
            
            _playerInput.Disable();
        }
        
        void OnPress(InputAction.CallbackContext context)
        {
            // Запустити перевірку в наступному кадрі через коутину
            StartCoroutine(CheckUIOverlap(context));
        }

        private IEnumerator CheckUIOverlap(InputAction.CallbackContext context)
        {
            // Чекаємо до наступного кадру
            yield return null;

            bool isOverUI = false;

            if (context.control is TouchControl touchControl)
            {
                int touchId = touchControl.touchId.ReadValue();
                // Для дотиків pointerId = touchId + 1
                isOverUI = EventSystem.current.IsPointerOverGameObject(touchId + 1);
            }
            else if (context.control.device is Mouse)
            {
                isOverUI = EventSystem.current.IsPointerOverGameObject();
            }

            // Якщо не над UI, почати прицілювання
            if (!isOverUI)
            {
                _isAiming = true;
            }
        }

        void OnDrag(InputAction.CallbackContext context)
        {
            if (!_isAiming)
                return;
            
            Vector2 inputPosition = context.ReadValue<Vector2>();
            HandleDrag(inputPosition);
        }
        
        void OnRelease(InputAction.CallbackContext context)
        {
            if (!_isAiming)
                return;
            HandleRelease();
        }
        
        void HandleDrag(Vector2 inputPosition)
        {
            Vector3 referencePoint = Vector3.zero;
            Vector3 screenPoint = new Vector3(inputPosition.x, inputPosition.y, _mainCamera.WorldToScreenPoint(referencePoint).z);
            Vector3 worldPos = _mainCamera.ScreenToWorldPoint(screenPoint);
            
            _dragVector = worldPos - launchPoint.position;
            
            float dot = Vector3.Dot(_dragVector, transform.forward);
            if (dot < 0)
            {
                _dragVector -= transform.forward * dot;
            }
            
            if (_dragVector.magnitude > maxDragDistance)
            {
                _dragVector = _dragVector.normalized * maxDragDistance;
            }
            
            trajectoryPrediction?.DrawTrajectory(launchPoint.position, _dragVector * forceMultiplier);
        }
        
        void HandleRelease()
        {
            trajectoryPrediction?.HideTrajectory();
            _isAiming = false;
            
            if (shotsAvailable > 0)
            {
                Vector3 shotVector = _dragVector * forceMultiplier;
                _projectilePool.Shot(shotVector);
                shotsAvailable--;
            }
        }
    }
}
