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

public abstract class PlayerFrameActionState : CharacterControllerState
{
	[SerializeField] private int deltaFrameCount = 1;
	[SerializeField] private List<TriggerInfo> triggers = new List<TriggerInfo>();

	private AnimationFrameReceiver receiver;
	protected int DeltaFrameCount => deltaFrameCount;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		base.OnStateEnter(animator, animatorStateInfo, layerIndex);

		receiver = animator.GetComponent<AnimationFrameReceiver>();
		CheckFrame();
	}

	public override void OnStatePrevUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		base.OnStatePrevUpdate(animator, animatorStateInfo, layerIndex);
		CheckFrame();
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		base.OnStateExit(animator, animatorStateInfo, layerIndex);
		CheckFrame();

		receiver = null;
	}

	private void CheckFrame()
	{
		if (receiver == null)
			return;

		foreach (var trigger in triggers)
		{
			if (trigger.Equals(receiver.frameIndex))
			{
				StartFrameAction();
				break;
			}
		}
	}

	protected abstract void StartFrameAction();
}

public abstract class PlayerFrameAttackState : PlayerFrameActionState
{
	[SerializeField] private Vector2 attackBox;
	[SerializeField] private Vector2 attackOffset;
	[SerializeField] private int attackCount;

	private int remainFrameCount = 0;
	private int remainAttackCount = 0;

	protected override void StartFrameAction()
	{
		remainFrameCount = DeltaFrameCount;
		remainAttackCount = attackCount;
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

	protected IEnumerable<Collider2D> GetHitObjects()
	{
		return Physics2D.OverlapBoxAll(attackOffset, attackBox, 0)
			.Where(h => h.gameObject.layer != LayerMask.NameToLayer("Player"));
	}

	protected IEnumerable<IDamageable> GetDamageableObjects()
	{
		return GetHitObjects().OfType<IDamageable>();
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

#if UNITY_EDITOR

public class PlayerFrameAttackDrawer : Editor
{
	private CharacterScene characterScene;
	private PlayerComponent playerComponent;

	private SerializedProperty attackBoxProperty;
	private SerializedProperty attackOffsetProperty;

	private void OnEnable()
	{
		characterScene = FindObjectOfType<CharacterScene>();
		if (characterScene == null)
			return;

		playerComponent = FindObjectOfType<PlayerComponent>();
		if (playerComponent == null)
			return;

		characterScene.onDrawGizmos += OnDrawGizmos;

		attackBoxProperty = serializedObject.FindProperty("attackBox");
		attackOffsetProperty = serializedObject.FindProperty("attackOffset");
	}

	private void OnDisable()
	{
		if (characterScene != null)
		{
			characterScene.onDrawGizmos -= OnDrawGizmos;
		}

		playerComponent = null;
	}

	private void OnDrawGizmos()
	{
		Vector2 playerPos = playerComponent.transform.position;
		Vector2 startPos = playerPos + attackOffsetProperty.vector2Value;
		Vector2 size = attackBoxProperty.vector2Value;

		Gizmos.DrawWireCube(startPos, size);
	}
} 

#endif