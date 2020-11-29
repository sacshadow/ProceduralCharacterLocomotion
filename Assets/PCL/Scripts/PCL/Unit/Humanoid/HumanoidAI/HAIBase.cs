using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public class HAIBase : HumanoidControl {
	
	public static List<HAIBase> ai = new List<HAIBase>();
	public static HumanoidControl target {get {return PlayerControl.Instance.unit as HumanoidControl; }}
	
	public float keepRadio = 4f;
	public float keepEachOtherForce = 20;
	
	[NonSerialized]
	public float mKeepRadio = 4f;
	
	public static int GetAliveAICount() {
		int count = 0;
		ai.ForEach(x=>count+=x.attribute.isDead ? 0 : 1);
		return count;
	}
	
	public static void ResetAllAI() {
		ai.ForEach(x=>x.Reset());
	}
	
	public virtual void TurnOff() {
		if(ai!=null)
			ai.Remove(this);
	}
	
	public void ActiveAI() {
		if(gameObject.activeSelf)
			Reset();
		else
			gameObject.SetActive(true);
	}
	
	public override void Reset() {
		state.UpdateInput(Vector3.zero, transform.forward);
		mKeepRadio = keepRadio;
		
		base.Reset();
	}
	
	
	protected virtual void AI_Init() {
	
	}
	
	//TOOLS
	/***********************************************************************/
	
	protected Vector3 ModifyMovement(Vector3 orgMovement) {
		var keepMove = Vector3.zero;
		var count = 0;
		var om = orgMovement.normalized * Mathf.Clamp01(orgMovement.magnitude);
		
		for(int i=0; i<ai.Count; i++) {
			if(ai[i] == this)
				continue;
			if(ai[i].attribute.isDead || !ai[i].attribute.isBalance)
				continue;
				
			var other = ai[i];	
			var dis = Vector3.Distance(transform.position, other.transform.position);
			
			var keepDis = mKeepRadio + other.mKeepRadio;
			if(dis > keepDis)
				continue;
			
			var disp = other.transform.position - transform.position;
			keepMove += disp.normalized * Mathf.Lerp(-keepEachOtherForce, 0, dis / keepDis);
			
			count ++;
		}
		
		var obsMove = Vector3.zero;
		
		for(int i=0; i<AI_AreaInfulence.area.Count; i++) {
			obsMove += AI_AreaInfulence.area[i].GetInfulenceOnPoint(transform.position);	
		}
		
		obsMove = Vector3.ClampMagnitude(obsMove,1);
		
		if(count == 0)
			return (om + obsMove).normalized;
		
		var km = keepMove / count;
		
		if(km.magnitude < 1f)
			km = Vector3.zero;
		
		var keepToTarget = Vector3.zero;
		if(target != null) {
			var disp = transform.position - targetPoint;
			if(disp.magnitude < 1.25f)
				keepToTarget = disp * 5f;
		}
		
		var output = om + obsMove + km + keepToTarget;
		
		if(output.magnitude < 0.1f)
			return Vector3.zero;
		
		return Vector3.ClampMagnitude(output, 1);
	}
	
	protected virtual float DisToTarget(float time = 0.2f) {
		if(target == null)
			return 0;
		
		// var p0 = target.transform.position + target.state.behaviour.root.data.velocity * time;
		var p0 = targetPoint + target.state.behaviour.root.data.velocity * time;
		var p1 = transform.position + state.behaviour.root.data.velocity * time;
		
		return Vector3.Distance(p0, p1);
	}
	
	protected virtual bool IsTargetApproach() {
		if(target == null) return false;
		
		var dir = GetTargetDirection();
		return DisToTarget(1) < 2.5f && Vector3.Dot(dir, target.behaviour.root.data.velocity) < -2f;
	}
	
	protected virtual Vector3 GetTargetDirection() {
		if(target == null)
			return transform.forward;
	
		// return (target.transform.position - transform.position).Flat().normalized;
		return (targetPoint - transform.position).Flat().normalized;
	}
	
	protected virtual void ModifyedMove(Vector3 move) {
		Move(ModifyMovement(move));
	}
	protected virtual void Move(Vector3 move) {
		if(target == null)
			state.UpdateInput(move, transform.forward);
		else
			state.UpdateInput(move, GetTargetDirection());
	}
	
	
	//MonoBehaviour
	/***********************************************************************/
	protected virtual void Awake() {
		AI_Init();
	}
	
	protected virtual void Start() {
		if(!isControl)
			ai.Add(this);
		// AI_Init();
	}
	
	protected Vector3 targetPoint;
	// float updateInterval = 0f;
	
	protected void UpdateTargetPoint() {
		// updateInterval -= Time.deltaTime;
		
		// if(updateInterval > 0) return;
		
		targetPoint = target.transform.position;
		// updateInterval = URD.Range(0.025f, 0.05f);
	}
	
	void Update() {
		if(target != null)
			UpdateTargetPoint();
		
		if(attribute.isDead) {
			// StopAllCoroutines();
			// this.enabled = false;
			if(ai!=null)
				ai.Remove(this);
		}
	}
	
	void OnDestroy() {
		if(ai!=null)
			ai.Remove(this);
	}
}
