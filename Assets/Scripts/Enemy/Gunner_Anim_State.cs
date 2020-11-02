using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunner_Anim_State : MonoBehaviour
{

    public Animator GunnerAnim;

    // Update is called once per frame
    void Update()
    {
        if (EnemyGunner.shooting == true)
        {
            GunnerAnim.SetTrigger("shoot");
        }
        else if (EnemyGunner.dashing == true)
        {
            GunnerAnim.SetTrigger("dash");
        }
        else if (EnemyGunner.moving == true) 
        {
            GunnerAnim.SetTrigger("walk");
        }
        else
        {
            GunnerAnim.SetTrigger("idle");
        }
       
    }
}
