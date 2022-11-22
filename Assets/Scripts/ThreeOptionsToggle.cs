using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace WayOfLife.View
{
    public class ThreeOptionsToggle : MonoBehaviour
    {
        // Start is called before the first frame update
        private UnityEvent onClick;
        

        public void Render(ValuesEnum valueToChange, int value, UnityEvent onClick, bool disabled)
        {
            this.onClick = onClick;
            gameObject.GetComponent<TextMeshPro>().text = valueToChange + ":" + value;
            gameObject.SetActive(!disabled);

        }


        private void OnMouseDown()
        {
            onClick.Invoke();
        }
    }
}