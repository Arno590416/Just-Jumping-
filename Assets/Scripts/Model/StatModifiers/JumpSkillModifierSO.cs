using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class JumpSkillModifierSO : CharacterStatModifierSO
{
    public override void AffectCharacter(GameObject character, float val)//override不影响基类charcterstatmodifierso的同名abstract函数
    {
       PlayerController jumpForce = character.GetComponent<PlayerController>();
        if (jumpForce != null)
        {
            jumpForce.maxJumpHeight += val;
            Debug.Log("JumpSkillModifierSO: " + jumpForce.maxJumpHeight);
        }
    }
}
