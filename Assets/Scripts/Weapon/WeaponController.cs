using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Weapon
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private BaseWeapon currentWeapon;

        [SerializeField] private PlayerController playerController;
        
        [SerializeField] private Image icon;
        
        private InputSystem_Actions _controls;
        
        private bool _canFireWeapon = false;
        private bool _isItemPickedUp = false;

        private void Awake()
        {
            _controls = playerController.InputActions;
        }

        private void Start()
        {
            if (currentWeapon != null)
                currentWeapon.Initialize(transform);

            
        }

        private void OnEnable()
        {
            PickableItem.OnItemPicked += PickableItemOnOnItemPicked;
            _controls.Player.SwitchWeaponOnOff.performed += OnOnOffAutoWeapon;
        }
        
        private void OnDisable()
        {
            PickableItem.OnItemPicked -= PickableItemOnOnItemPicked;
            _controls.Player.SwitchWeaponOnOff.performed -= OnOnOffAutoWeapon;
        }

        private void PickableItemOnOnItemPicked(string itemName)
        {
            if (itemName.Equals(currentWeapon.data.weaponName))
            {
                _isItemPickedUp = true;
                _canFireWeapon = true;
                icon.color = Color.white;
            }
        }

        private void Update()
        {
            if (_canFireWeapon && _isItemPickedUp)
            {
                TriggerAutomaticWeapon();
            }
            
        }

        private void TriggerAutomaticWeapon()
        {
            Vector2 aimDir = GetAimDirection();
            Quaternion aimRot = RotateToDirection(aimDir);
            currentWeapon?.Attack(aimDir, aimRot);
        }

        public void OnFire(InputAction.CallbackContext context)
        {
            if (context.started || context.performed)
            {
                Vector2 aimDir = GetAimDirection();
                Quaternion aimRot = RotateToDirection(aimDir);
                currentWeapon?.Attack(aimDir, aimRot);
            }
            else if (context.canceled)
            {
                currentWeapon?.StopAttack();
            }
        }

        private Vector2 GetAimDirection()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            return (mousePos - transform.position).normalized;
        }

        private Quaternion RotateToDirection(Vector2 direction)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            return Quaternion.Euler(0f, 0f, angle);
        }

        public void EquipWeapon(BaseWeapon weapon)
        {
            currentWeapon = weapon;
            currentWeapon.Initialize(transform);
        }

        private void OnOnOffAutoWeapon(InputAction.CallbackContext context)
        {
            _canFireWeapon = !_canFireWeapon;

            icon.color = _canFireWeapon ? Color.white : Color.black;
        }
    }
}

