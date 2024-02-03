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
	[SerializeField] private int deltaFrameCount = 1;
	[SerializeField] private List<TriggerInfo> triggers = new List<TriggerInfo>();

	private AnimationFrameReceiver receiver;
	protected int DeltaFrameCount => deltaFrameCount;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		receiver = animator.GetComponent<AnimationFrameReceiver>();
		CheckFrame();
	}

	public override void OnStatePrevUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		CheckFrame();
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		CheckFrame();

		receiver = null;
	}

	protected sealed override void CheckNextState(Animator animator, AnimatorStateInfo animatorStateInfo)
	{
		
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

	protected abstract void StartFrameAction(int triggerIndex);
}

public abstract class CharacterFrameAttackState : CharacterFrameActionState
{
	[SerializeField] List<AttackInfo> attackInfoList = new List<AttackInfo>();

	private Vector2 attackOffset;
	private Vector2 attackBox;

	private int remainFrameCount = 0;
	private int remainAttackCount = 0;

	protected override void StartFrameAction(int triggerIndex)
	{
		remainFrameCount = DeltaFrameCount;

		if (attackInfoList.Count > triggerIndex)
		{
			var info = attackInfoList[triggerIndex];

			remainAttackCount = info.attackCount;
			attackBox = info.attackBox;
			attackOffset = info.attackOffset;
		}

	}

	public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		base.OnStateEnter(animator, animatorStateInfo, layerIndex);
		ProgressAction();
	}

	public override void OnStatePrevUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		base.OnStatePrevUpdate(animator, animatorStateInfo, layerIndex);
		ProgressAction();
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		base.OnStateExit(animator, animatorStateInfo, layerIndex);
		ProgressAction();
	}

	protected IEnumerable<Collider2D> GetHitObjects(Transform centerObj)
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

	protected IEnumerable<IDamageable> GetDamageableObjects()
	{
		if (controller == null)
			return null;

		return GetHitObjects(controller.transform).OfType<IDamageable>();
	}

	private void ProgressAction()
	{
		if (ProgressFrameCount() == false)
			return;

		if (ProgressAttackCount() == false)
			return;

		// TO-DO
	}

	private bool ProgressFrameCount()
	{
		if (remainFrameCount == 0)
			return false;

		remainFrameCount--;
		return true;
	}

	private bool ProgressAttackCount()
	{
		if (remainAttackCount == 0)
			return false;

		if (DoFrameAction() == false)
			return false;

		remainAttackCount--;
		return true;
	}

	protected virtual bool DoFrameAction()
	{
		return true;
	}
}