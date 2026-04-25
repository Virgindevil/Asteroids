using UnityEngine;
using UnityEngine.UI;

namespace Game.Presentation
{
    public class HeartView : MonoBehaviour
    {
        [SerializeField] private Image _icon;

        public void SetActive(bool isActive)
        {
            // Можно просто выключать объект
            gameObject.SetActive(isActive);
            
            // Или, если хочешь оставить «пустое» место, менять альфу или спрайт:
            // _icon.color = isActive ? Color.white : new Color(1, 1, 1, 0.2f);
        }
    }
}