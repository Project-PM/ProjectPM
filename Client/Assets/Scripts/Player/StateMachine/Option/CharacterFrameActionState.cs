using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class TriggerInfo
{
	[SerializeField] private int startFrameIndex;

	public bool Equals(int startFrameCount)
	{
		return this.startFrameIndex == startFrameCount;
	}
}

[System.Serializable]
public class AttackInfo
{
	[SerializeField] public Vector2 attackBox;
	[SerializeField] public Vector2 attackOffset;
	[SerializeField] public int attackCount;
}

public abstract class CharacterFrameActionState : CharacterControllerState
{
    [Header("[적용할 캐릭터 타입]")]
    [SerializeField] private ENUM_CHARACTER_TYPE applyCharacterType = ENUM_CHARACTER_TYPE.Red;

    [Header("[지속 프레임 수]")]
    [SerializeField] private int deltaFrameCount = 1;
    
	[Header("[적용할 시작 프레임]")]
    [SerializeField] private List<TriggerInfo> triggers = new List<TriggerInfo>();

    private AnimationFrameReceiver receiver;
    private int remainFrameCount = 0;


    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		receiver = animator.GetComponent<AnimationFrameReceiver>();
		
		CheckFrame();
        ProgressAction();
    }

    public override void OnStatePrevUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		CheckFrame();
        ProgressAction();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		CheckFrame();
        ProgressAction();

        receiver = null;
	}

    protected sealed override void CheckNextState(Animator animator, AnimatorStateInfo animatorStateInfo)
	{
		
	}

    private void ProgressAction()
    {
        if (characterType != applyCharacterType)
            return;

        if (ProgressFrameCount() == false)
            return;

		OnProgressAction();
    }

	protected virtual void OnProgressAction()
	{

	}

    private bool ProgressFrameCount()
    {
        if (remainFrameCount == 0)
            return false;

        remainFrameCount--;
        return true;
    }

    private void CheckFrame()
	{
		if (receiver == null)
			return;

		for(int triggerIndex = 0; triggerIndex < triggers.Count; triggerIndex++)
		{
			var trigger = triggers[triggerIndex];
			int frameIndex = receiver.frameIndex;

			if (trigger.Equals(frameIndex))
			{
				StartFrameAction(triggerIndex);
				break;
			}
		}
	}

	protected virtual void StartFrameAction(int triggerIndex)
	{
        remainFrameCount = deltaFrameCount;
    }
}

public class CharacterFrameAttackState : CharacterFrameActionState
{
	[Header("[공격할 시작 프레임에 연결되는 공격 정보]")]
	[SerializeField] List<AttackInfo> attackInfoList = new List<AttackInfo>();

    [Header("[데미지 정보]")]
    [SerializeField] private DamageInfo damageInfo;

    private Vector2 attackOffset;
	private Vector2 attackBox;

	private int remainAttackCount = 0;

	protected override void StartFrameAction(int triggerIndex)
	{
		base.StartFrameAction(triggerIndex);

		if (attackInfoList.Count > triggerIndex)
		{
			var info = attackInfoList[triggerIndex];

			remainAttackCount = info.attackCount;
			attackBox = info.attackBox;
			attackOffset = info.attackOffset;
		}

	}

    protected override void OnProgressAction()
    {
		ProgressAttackCount();
    }

    protected IEnumerable<Collider2D> GetOverlapBoxAll(Transform centerObj)
	{
		Vector2 centerPos = centerObj.position;

		DrawOverlapBox(centerPos + attackOffset, attackBox);

		return Physics2D.OverlapBoxAll(centerPos + attackOffset, attackBox, 0)
			.Where(h => h.gameObject.layer != LayerMask.NameToLayer("Player"));
	}

	private void DrawOverlapBox(Vector2 point, Vector2 size)
	{
		Vector2 leftDown = point - size / 2;
		Vector2 rightUp = point + size / 2;
		Vector2 leftUp = new Vector2(point.x - size.x / 2, point.y + size.y / 2);
		Vector2 rightDown = new Vector2(point.x + size.x / 2, point.y - size.y / 2);

		Debug.DrawLine(leftDown, leftUp);
		Debug.DrawLine(rightDown, rightUp);
		Debug.DrawLine(leftDown, rightDown);
		Debug.DrawLine(leftUp, rightUp);
	}

	protected IEnumerable<IDamageable> GetDamageableOverlapBoxAll()
	{
		if (controller == null)
			return null;

		return GetOverlapBoxAll(controller.transform).OfType<IDamageable>();
	}

	private void ProgressAttackCount()
	{
		if (remainAttackCount == 0)
			return;

        if (DoFrameAttack() == false)
			return;

		remainAttackCount--;
		return;
	}

    protected virtual bool DoFrameAttack()
    {
        var hitObjects = GetDamageableOverlapBoxAll();
        if (hitObjects.Any() == false)
            return false;

        bool isSuccess = false;

        foreach (var hitObject in hitObjects)
        {
            isSuccess |= hitObject.OnHit(controller, damageInfo);
        }

        return isSuccess;
    }
}