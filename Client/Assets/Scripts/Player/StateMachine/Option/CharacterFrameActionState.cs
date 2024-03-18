using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class DrawHelper
{
    public static void DrawOverlapBox(Vector2 point, Vector2 size, Color color = default)
    {
#if UNITY_EDITOR
        Vector2 leftDown = point - size / 2;
        Vector2 rightUp = point + size / 2;
        Vector2 leftUp = new Vector2(point.x - size.x / 2, point.y + size.y / 2);
        Vector2 rightDown = new Vector2(point.x + size.x / 2, point.y - size.y / 2);

        Debug.DrawLine(leftDown, leftUp, color);
        Debug.DrawLine(rightDown, rightUp, color);
        Debug.DrawLine(leftDown, rightDown, color);
        Debug.DrawLine(leftUp, rightUp, color);
#endif
    }
}

[System.Serializable]
public class TriggerInfo
{
	[SerializeField] private int startFrameIndex;
	[SerializeField] public int endFrameIndex;

	public bool IsValid(int frameIndex)
	{
		return startFrameIndex == frameIndex && endFrameIndex >= frameIndex;
	}
}

[System.Serializable]
public class AttackInfo
{
	[SerializeField] public Vector2 attackBox;
	[SerializeField] public Vector2 attackOffset;
	[SerializeField] public int attackCount;
	[SerializeField] public DamageInfo damageInfo;
}

public abstract class CharacterFrameActionState : CharacterControllerState
{
    [Header("[적용할 캐릭터 타입]")]
    [SerializeField] private ENUM_CHARACTER_TYPE applyCharacterType = ENUM_CHARACTER_TYPE.Red;

	[Header("[적용할 시작, 끝 프레임]")]
    [SerializeField] protected List<TriggerInfo> triggers = new List<TriggerInfo>();

    private AnimationFrameReceiver receiver;

    private int currentFrameIndex = -1;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		receiver = animator.GetComponent<AnimationFrameReceiver>();
		receiver.Reset();

		currentFrameIndex = -1;
		CheckFrame();
		OnProgressAction(currentFrameIndex);
	}

    public override void OnStatePrevUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		CheckFrame();
		OnProgressAction(currentFrameIndex);
	}

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		CheckFrame();
		OnProgressAction(currentFrameIndex);

		currentFrameIndex = -1;
		receiver = null;
	}

    protected sealed override void CheckNextState(Animator animator, AnimatorStateInfo animatorStateInfo)
	{
		
	}

	protected virtual void OnProgressAction(int frameIndex)
	{

	}

	protected virtual void StartAction(int triggerIndex, int endFrameIndex)
	{

	}

    private void CheckFrame()
	{
		if (receiver == null)
			return;

		if (characterType != applyCharacterType)
			return;

		if (currentFrameIndex >= receiver.frameIndex)
			return;

		currentFrameIndex = receiver.frameIndex;

		for (int triggerIndex = 0; triggerIndex < triggers.Count; triggerIndex++)
		{
			var trigger = triggers[triggerIndex];
			if (trigger.IsValid(currentFrameIndex))
			{
				StartAction(triggerIndex, trigger.endFrameIndex);
				break;
			}
		}
	}
}

public class CharacterFrameAttackState : CharacterFrameActionState
{
	[Header("[공격할 시작 프레임에 연결되는 공격 정보]")]
	[SerializeField] List<AttackInfo> attackInfoList = new List<AttackInfo>();

    private Vector2 attackOffset;
	private Vector2 attackBox;
	private DamageInfo damageInfo;

	private int endFrameIndex = 0;

	private IAttackable myAttackable = null;
	protected IAttackable MyAttackable
	{
		get
		{
			if (myAttackable == null)
			{
				myAttackable = controller.GetComponent<IAttackable>();
			}

			return myAttackable;
		}
	}

	private int remainAttackCount = 0;

    protected override void StartAction(int triggerIndex, int endFrameIndex)
    {
		if (attackInfoList.Count > triggerIndex)
		{
			var info = attackInfoList[triggerIndex];

			remainAttackCount = info.attackCount;
			attackBox = info.attackBox;
			attackOffset = info.attackOffset;
			damageInfo = info.damageInfo;
		}

		this.endFrameIndex = endFrameIndex;
    }

	protected override void OnProgressAction(int frameIndex)
	{
		base.OnProgressAction(frameIndex);

		if (frameIndex <= endFrameIndex)
		{
			ProgressAttackCount();
		}
	}

	protected IEnumerable<Collider2D> GetOverlapBoxAll(Transform centerObj)
	{
		Vector2 centerPos = centerObj.position;

		DrawHelper.DrawOverlapBox(centerPos + attackOffset, attackBox, Color.green);

        return Physics2D.OverlapBoxAll(centerPos + attackOffset, attackBox, 0)
            .Where(c => c.transform != centerObj)
            .Where(h => h.gameObject.layer == LayerMask.NameToLayer("Player"));
    }

	protected IEnumerable<IDamageable> GetDamageableOverlapBoxAll()
	{
		if (controller == null)
			return null;

        return GetOverlapBoxAll(controller.transform)
			.Where(c => c.GetComponent<IDamageable>() != null)
			.Select(c => c.GetComponent<IDamageable>());
	}

	private void ProgressAttackCount()
	{
		if (remainAttackCount == 0)
			return;

        if (DoFrameAttack() == false)
			return;

		MyAttackable?.OnAttack();
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