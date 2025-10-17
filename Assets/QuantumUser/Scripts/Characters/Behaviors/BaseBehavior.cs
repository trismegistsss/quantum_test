using System;
using System.Collections.Generic;
using UnityEngine;

namespace AGS.Graber.Character.Behaviors
{
    [Serializable]
    public class BaseBehavior:ScriptableObject
    {
        protected GrabCharacterBase Character;
        protected Transform target;

        public virtual void Destroy()
        {

        }

        public virtual void Initialize(GrabCharacterBase character, GrabCharacterBase target)
        {

        }

        public virtual void UpradeBehavior()
        {

        }
    }
}
