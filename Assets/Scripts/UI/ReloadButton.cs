using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace ThirdPersonProject.UI 
{
    /// <summary>
    /// Класс кнопки перезагрузки уровня
    /// </summary>
    public class ReloadButton : MonoBehaviour
    {
        #region Private Variables

        [SerializeField] private GameObject _reloadButton;

        #endregion

        #region Public Methods

        public void ShowButton()
        {
            _reloadButton.SetActive(true);
        }

        public void LevelReload()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        #endregion
        
        #region MonoBehaviour

        void Start()
        {
            _reloadButton.SetActive(false);
        }

        #endregion
    }
}
