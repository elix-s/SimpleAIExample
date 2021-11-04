using UnityEngine;
using StarterAssets;

namespace ThirdPersonProject.Gameplay 
{
    /// <summary>
    /// Класс взрыва персонажа на несколько частей
    /// </summary>
    public class PlayerDamage : MonoBehaviour
    {
        #region Private Variables

        [SerializeField] private GameObject _player;
        [SerializeField] private SkinnedMeshRenderer _smRenderer;
        [SerializeField] private float _cubeSize; //размер частей после взрыва
        [SerializeField] private int _cubeInRow; //количество частей
        [SerializeField] private Material _cubeMaterial;
        [SerializeField] private float _explosionPower; //сила взрыва
        [SerializeField] private float _explosionRadius; //радиус взрыва
        [SerializeField] private float _addMass;
        [SerializeField] private EnemyController _enemyController;
        public bool IsDamage; //true - если персонаж столкнулся с ботом

        #endregion

        #region Public Methods
        
        ///<summary>
        ///взрыв персонажа после столкновения с препятствием
        ///</summary>
        public void Explosion()
        {
            IsDamage = true;
            _smRenderer.enabled = false;
           _player.GetComponent<ThirdPersonController>().enabled = false;

            //создание массива из кубов в цикле
            for(int x = 0; x < _cubeInRow; x++)
                for(int y = 0; y < _cubeInRow; y++)
                    for(int z = 0; z < _cubeInRow; z++)
                        CreatingParts(x,y,z);
                
            Vector3 explosionPosition = _player.transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPosition, _explosionRadius);

            foreach (Collider hit in colliders) 
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if(rb != null) 
                    rb.AddExplosionForce(_explosionPower, _player.transform.position, _explosionRadius);
            }
        }

        #endregion

        #region Private Methods

        ///<summary>
        ///создание элемента массива кубов
        ///</summary>
        private void CreatingParts(int x, int y, int z)
        {
            GameObject part;
            part = GameObject.CreatePrimitive(PrimitiveType.Cube);
            part.transform.position = _player.transform.position + new Vector3(_cubeSize*x, _cubeSize*y, _cubeSize*z);
            part.transform.localScale = new Vector3(_cubeSize, _cubeSize, _cubeSize);
            part.AddComponent<Rigidbody>();
            part.GetComponent<Rigidbody>().mass = _addMass;
            part.GetComponent<MeshRenderer>().material = _cubeMaterial;
        }

        #endregion            
    }
}
