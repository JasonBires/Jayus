using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Jayus.TimeControl;
using Jayus.Behaviors;

namespace Jayus.Character
{
    class HUD : MonoBehaviour
	{
        public Texture2D ReticleImage;
        private float reach = 5.0F;

        void OnGUI()
        {
            ConstructReticle();

            var focusedObject = GetCurrentlyFocusedObject();
            if (focusedObject != null)
            {
                ShowInteractionText(focusedObject.GetInteractionMessage());
            }
        }

        void ConstructReticle()
        {
            //GUI.Box(new Rect(Screen.width / 2, Screen.height / 2, 25, 25), "");
            GUI.DrawTexture(new Rect(Screen.width / 2, Screen.height / 2, 25, 25), ReticleImage);
        }

        void ShowInteractionText(string text)
        {
            GUI.Label(new Rect(Screen.width / 2, Screen.height / 2 + 25, 100, 50), text);
        }

        Interactable GetCurrentlyFocusedObject()
        {
            var ray = new Ray(transform.position, transform.forward);
	        var hit = new RaycastHit();

	        if (Physics.Raycast(ray, out hit, reach))
            {
                return hit.collider.gameObject.GetComponent<Interactable>();
	        }
            return null;
        }
	}
}
