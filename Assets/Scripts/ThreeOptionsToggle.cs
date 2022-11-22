using System;
using TMPro;
using UnityEngine;


namespace WayOfLife.View
{
    public class ThreeOptionsToggle : MonoBehaviour
    {
        public event Action onClick;
        [SerializeField] private TextMeshPro label;
        
        
        public void Render(ValuesEnum valueToChange, int value, bool disabled)
        {
            label.text = valueToChange + ":" + value;
            gameObject.SetActive(!disabled);
        }


        private void OnMouseDown()
        {
            onClick.Invoke();
        }
    }
}