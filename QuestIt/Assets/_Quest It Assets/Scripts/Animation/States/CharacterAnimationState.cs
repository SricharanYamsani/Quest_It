using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterAnimationState
{
    public CharacterAnimationStateEnum characterState = CharacterAnimationStateEnum.None;
    public abstract void Init();

    public abstract void Update();

    public abstract void Exit();
}
