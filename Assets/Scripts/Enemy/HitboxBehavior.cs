using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxBehavior : MonoBehaviour, IDamage<float> 
{
    public enum hitBoxType { head, body, instant}
    public hitBoxType myHitboxType;

    public int damageOnHit;

    public EnemyGunner myGunnerBase;

    public void Damage(float damageTaken) {
        myGunnerBase.takeDamage(damageTaken * damageOnHit);
    }
}
