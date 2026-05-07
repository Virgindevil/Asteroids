using UnityEngine;

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