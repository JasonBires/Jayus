using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Jayus.Core
{
    public class ObjectBehavior : MonoBehaviour
	{
        public virtual void Start()
        {
            this.Inject();
        }
	}
}
