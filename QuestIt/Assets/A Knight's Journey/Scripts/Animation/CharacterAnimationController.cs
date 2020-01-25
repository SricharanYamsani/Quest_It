using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    public Animator animator;
    private CharacterAnimationStateEnum lastStateEnum = CharacterAnimationStateEnum.None;

    public CharacterAnimationStateEnum CurrentStateEnum { get; private set; } = CharacterAnimationStateEnum.None;

    private CharacterAnimationState currentState = null;

    void Start()
    {
        SetState(CharacterAnimationStateEnum.Idle);
    }

    void Update()
    {
        if(currentState != null)
        {
            currentState.Update();
        }
    }

    void SetState(CharacterAnimationStateEnum stateEnum)
    {
        if (stateEnum.Equals(CurrentStateEnum))
        {
            return;
        }

        lastStateEnum = CurrentStateEnum;
        CurrentStateEnum = stateEnum;

        currentState.Exit();

        currentState = GetState(stateEnum);

        animator.Play(stateEnum.ToString());

        currentState.Init();
    }

    CharacterAnimationState GetState(CharacterAnimationStateEnum stateEnum)
    {
        switch(stateEnum)
        {
            case CharacterAnimationStateEnum.Idle:
                return new IdleState();
            case CharacterAnimationStateEnum.SwordSlash:
                return new SwordSlashState();
            case CharacterAnimationStateEnum.Swordister:
                return new SwordisterState();
            case CharacterAnimationStateEnum.ArrowHit:
                return new ArrowHitState();
            default:
                return new IdleState();
        }
    }


}
public enum CharacterAnimationStateEnum
{
    None = -1,
    Idle,
    //Walk,
    //Run,
    SwordSlash,
    Swordister,
    ArrowHit
}

