using UnityEngine;
using UnityEngine.UI;

namespace Game.Presentation
{
    public class HeartView : MonoBehaviour
    {
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}