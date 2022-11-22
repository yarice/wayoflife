using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;



namespace WayOfLife.View
{


    public class ButtonScript : MonoBehaviour
    {

        private TextMeshPro txt;
        private UnityEvent onClick;

        // Start is called before the first frame update
        void Start()
        {
            txt = gameObject.GetComponent<TextMeshPro>();
        }

        public void Render(string text, UnityEvent onClick, bool disabled)
        {
            txt.text = text;
            this.onClick = onClick;
            gameObject.SetActive(!disabled);
        }

        private void OnMouseDown()
        {
            onClick.Invoke();
        }

    }
}
