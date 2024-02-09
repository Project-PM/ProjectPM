using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChildComponent : MonoBehaviour
{
    [SerializeField] protected PlayerCharacterController controller;
    protected int playerId = 0;

    protected virtual void Reset()
    {
        controller = GetComponent<PlayerCharacterController>();
    }

    public void SetPlayerId(int playerId)
    {
        this.playerId = playerId;
    }
}

public interface IAttackable
{
    void OnAttack();
}

public class PlayerAttackCheckComponent : PlayerChildComponent, IAttackable
{
    private Coroutine endOfFrameCoroutine = null;
    private WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();

    private bool isSuccessAttack = false;

    private void OnEnable()
    {
        controller.IsSuccessAttack += IsSuccessAttack;

        if (endOfFrameCoroutine != null)
        {
            StopCoroutine(endOfFrameCoroutine);
        }

        endOfFrameCoroutine = StartCoroutine(OnLateUpdate());
    }

    private void OnDisable()
    {
        if (endOfFrameCoroutine != null)
        {
            StopCoroutine(endOfFrameCoroutine);
        }

        controller.IsSuccessAttack -= IsSuccessAttack;
    }

    private IEnumerator OnLateUpdate()
    {
        while (true)
        {
            yield return endOfFrame;
            isSuccessAttack = false;
        }
    }

    private bool IsSuccessAttack()
    {
        return isSuccessAttack;
    }

    public void OnAttack()
    {
        isSuccessAttack = true;
    }
}
