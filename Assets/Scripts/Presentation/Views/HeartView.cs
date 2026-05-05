using UnityEngine;
using UnityEngine.UI;

namespace Game.Presentation
{
    public class HeartView : MonoBehaviour
    {
        [SerializeField] private Image _icon;

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}