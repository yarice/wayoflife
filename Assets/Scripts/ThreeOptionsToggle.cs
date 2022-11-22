using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems; 


namespace WayOfLife.View
{
    public class ThreeOptionsToggle : MonoBehaviour
        , IPointerClickHandler
    {
        public event Action onClick;
        [SerializeField] private TextMeshProUGUI label;
        
        
        public void Render(ValuesEnum valueToChange, int value, bool disabled)
        {
            label.text = valueToChange + ":" + value;
            gameObject.SetActive(!disabled);
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            onClick.Invoke();
        }
        
    }
}