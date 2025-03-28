using Assets.Scripts.SO;
using Assets.Scripts.Systems;
using Assets.Scripts.WaveSystem;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Turrets
{
    public abstract class BaseTurret : MonoBehaviour
    {
        [SerializeField] protected TurretInfoSO _turretInfo;
        [SerializeField] protected Transform _rotationPoint, _barrel;     

        protected GameObject _targetEnemy;
        protected float _timeSinceLastShot = 0f;
        protected bool _targetInRange;

        private bool _targetInAim;

        protected virtual void Update()
        {
            _timeSinceLastShot += Time.deltaTime;
            Attack();
        }

        protected virtual void Attack()
        {            
            TargetFirst();
            AimTowardsTarget();

            // Get spd bonus from GameManager and calculate effective fire rate
            float effectiveFireRate = _turretInfo.FireRate / (1f + GameManager.Instance.spdBonus / 100f);

            if (_timeSinceLastShot < effectiveFireRate)
                return;

            if (_targetInAim && _targetInRange)
                Shoot();
        }

        protected virtual void Shoot()
        {
            //Debug.LogWarning($"[BASETURRET] Shoot not implemented");
        }

        protected virtual void TargetFirst()
        {
            _targetEnemy = EnemySpawner.Instance.EnemiesAlive
                .OrderBy(enemy => enemy.transform.position.y)
                .FirstOrDefault(y => y.transform.position.y <= 7.5f);
        }

        protected virtual void AimTowardsTarget()
        {
            if (_targetEnemy == null)
            {
                _targetInRange = false;
                return;
            }

            _targetInRange = true;

            Vector3 direction = _targetEnemy.transform.position - _rotationPoint.position;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;

            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

            _rotationPoint.localRotation = Quaternion.Slerp(
                _rotationPoint.rotation, targetRotation, _turretInfo.RotationSpeed * Time.deltaTime);

            IsAimingOnTarget(angle);
        }

        private void IsAimingOnTarget(float targetAngle)
        {
            if (_targetEnemy == null)
            {
                _targetInAim = false;
                return;
            }

            float currentAngle = _rotationPoint.localRotation.eulerAngles.z;

            float angleDifference = Mathf.Abs(Mathf.DeltaAngle(currentAngle, targetAngle));

            _targetInAim = angleDifference <= _turretInfo.AngleThreshold;
        }
    }
}