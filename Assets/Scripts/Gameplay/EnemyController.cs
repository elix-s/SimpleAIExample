using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ThirdPersonProject.UI;

namespace ThirdPersonProject.Gameplay 
{
    ///<summary>
    ///Скрипт ботов, определение дистанции до персонажа, прокладывание пути через NavMeshAgent
    ///</summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyController : MonoBehaviour
    {
        #region Private Variables

        [Header("Параметры зона обзора бота")]
        [Range(0,90)] [SerializeField] private float _viewAngle; //угол обзора бота
        [Range(0,50)] [SerializeField] private float _viewDistance; //расстояние обзора бота
        [Range(0,30)] [SerializeField] private float _detectionDistance; //расстояние определения персонажа

        [SerializeField] private Transform _enemyTransform;
        [SerializeField] private Transform _targetTransform;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private PlayerDamage _damage;
        [SerializeField] private ReloadButton _reloadButton;

        private NavMeshAgent _navMeshComponent;
        private Animator _animator;
        private Transform _agentTransform;

        #endregion

        #region Private Methods

        ///<summary>
        ///Метод определение видимости ботом персонажа, true- если да
        ///</summary>
        private bool IsView()
        {
            float currentAngle = Vector3.Angle(_enemyTransform.forward, _targetTransform.position - _enemyTransform.position);
            RaycastHit hit;

            if(Physics.Raycast(_enemyTransform.position, _targetTransform.position - _enemyTransform.position, out hit, _viewDistance))
                if(currentAngle > _viewAngle/2.0f && Vector3.Distance(_enemyTransform.position, _targetTransform.position) <= _viewDistance && hit.transform == _targetTransform)
                    return true;
            
            return false;
        }

        ///<summary>
        ///Перемещение бота за персонажем
        ///</summary>
        private void MoveToTarget()
        {
            _navMeshComponent.SetDestination(_targetTransform.position);

            if(_animator.GetBool("Run") == false && _damage.IsDamage == false)
                _animator.SetBool("Run", true);
        }

        ///<summary>
        ///Вращение бота по направлению персонажа
        ///</summary>
        private void RotateToTarget()
        {
            Vector3 lookVector = _targetTransform.position - _agentTransform.position;
            lookVector.y = 0;

            if(lookVector == Vector3.zero) return;

            _agentTransform.rotation = Quaternion.RotateTowards
            (
                _agentTransform.rotation, 
                Quaternion.LookRotation(lookVector, Vector3.up),
                _rotationSpeed * Time.deltaTime
            );
        }

        #endregion

        #region Unity Events

        void OnCollisionEnter(Collision other) 
        {
            if(other.gameObject.tag == "Player" && _damage.IsDamage == false)
            {
                _animator.SetBool("Run", false);
                _damage.Explosion();
                _reloadButton.ShowButton();
                GetComponent<Rigidbody>().isKinematic = true;
            }
        }

        #endregion

        #region MonoBehavior

        void Start()
        {
            _navMeshComponent = GetComponent<NavMeshAgent>();
            _navMeshComponent.updateRotation = false;
            _rotationSpeed = _navMeshComponent.angularSpeed;
            _agentTransform = _navMeshComponent.transform;
            _animator = GetComponent<Animator>();
        }

        void Update()
        {
            //определение текущей дистанции до игрока
            float distanceToPlayer = Vector3.Distance(_targetTransform.position, _agentTransform.position);

            if(distanceToPlayer <= _detectionDistance || IsView())
            {
                RotateToTarget();
                MoveToTarget();
            }

        }

        #endregion    
    }
}
