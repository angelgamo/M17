using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flipflop : MonoBehaviour
{
	HingeJoint joint;
	JointSpring spring;

	private void Awake()
	{
		joint = GetComponent<HingeJoint>();
		spring = joint.spring;
	}

	private void Start()
	{
		StartCoroutine(move());
	}

	IEnumerator move()
	{
		spring.targetPosition = 30;
		joint.spring = spring;
		yield return new WaitForSeconds(.5f);
		spring.targetPosition = -30;
		joint.spring = spring;
		yield return new WaitForSeconds(.5f);
		StartCoroutine(move());
	}
}
