using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerFrontCheckComponent : PlayerChildComponent
{
	private SpriteRenderer[] spriteRenderers = null;

	[SerializeField] private Vector3 frontCheckOffset = Vector2.zero;
    [SerializeField] private Vector3 frontCheckSize = Vector2.zero;

	private bool isFrontRight = false;

    private void Awake()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    private void OnEnable()
	{
		controller.IsFrontRight += IsFrontRight;
	}

	private void OnDisable()
	{
		controller.IsFrontRight -= IsFrontRight;
	}

    private void Update()
    {
		DrawHelper.DrawOverlapBox(transform.position + frontCheckOffset, frontCheckSize, Color.red);

		var checkRightBox = Physics2D.OverlapBoxAll(transform.position + frontCheckOffset, frontCheckSize, 0)
			.Where(c => c.gameObject != gameObject)
			.Where(c => c.gameObject.layer == LayerMask.NameToLayer("Player"));

		isFrontRight = checkRightBox.Any();

		foreach(var r in spriteRenderers)
		{
			r.flipX = !isFrontRight;
		}
    }

    private bool IsFrontRight()
	{
		return isFrontRight;
	}
}