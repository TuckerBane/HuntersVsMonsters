using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MessageTypes
{
    // parameterless messages:
    /*
     DeathMessage
         */

   public class DamageMessage
    {
        public DamageType type = DamageType.Bludgeoning;
        public float amount = 1.0f;
        public DamageMessage() { }
        public DamageMessage(DamageType newType, float newAmount)
        {
            type = newType;
            amount = newAmount;
        }
    }

    

}
