using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems; 



namespace WayOfLife.View
{


    public class ButtonScript : MonoBehaviour, IPointerClickHandler
    {

        [SerializeField] private TextMeshProUGUI txt;
        public event Action onClick;
        
        
        public void OnPointerClick(PointerEventData eventData)
        {
            onClick.Invoke();
        }

        public void Render(string text, bool disabled)
        {
            txt.text = text;
            gameObject.SetActive(!disabled);
        }

        private void OnMouseDown()
        {
            onClick.Invoke();
        }

    }
}
