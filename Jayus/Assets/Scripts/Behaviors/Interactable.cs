using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Jayus.Behaviors
{
    public class Interactable : MonoBehaviour
	{
        public string InteractionMessage;

        public virtual string GetInteractionMessage()
        {
            return InteractionMessage ?? "Interact";
        }

        public virtual void Interact()
        {
            return;
        }
	}
}
