using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;
using SDTK.Cameras;

public class HumanoidControl : UnitControl {
	
	public HumanoidState state;
	public HumanoidBehaviour behaviour {get {return state.behaviour; }}
	
	public FightStyleBase unarmedFightStyle, weaponFightStyle;
	
	public WeaponBase weapon;
	
	public HA_DefaultAction defaultAction {get {return fightStyle.movement.defaultAction; }}
	public HA_Run run {get {return fightStyle.movement.run; }}
	public HA_Jump jump {get {return fightStyle.movement.jump; }}
	public HA_Fall fall {get {return fightStyle.movement.fall; }}
	public HA_Roll roll {get {return fightStyle.movement.roll; }}
	public HA_Flip flip {get {return fightStyle.movement.flip; }}
	public HA_Climb climb {get {return fightStyle.movement.climb; }}
	public HA_FallDown fallDown {get {return fightStyle.movement.fallDown; }}
	public HA_Gitup gitup {get {return fightStyle.movement.gitup; }}
	
	protected FightStyleBase fightStyle;
	
	public override void SetMove(Vector3 move, Vector3 direction) {
		state.UpdateInput(move, direction);
	}
	
	public virtual void PickupWeapon(WeaponBase weapon) {
		this.weapon = weapon;
		
		weapon.Attach(behaviour);
		
		weaponFightStyle = weapon.GetFightStyle();
		SetFightStyle(weaponFightStyle);
	}
	
	public virtual void DropWeapon() {
		if(weapon == null) return;
		
		SetFightStyle(unarmedFightStyle);
		
		weapon.Detach();
		weaponFightStyle = null;
		weapon = null;
	}
	
	public virtual void SetFightStyle(FightStyleBase fightStyle) {
		this.fightStyle = fightStyle;
		fightStyle.control = this;
		
		state.SetDefault(fightStyle.movement.defaultAction);
	}
	
	public override void SetPose(Vector3 position, Vector3 direction) {
		var q = Quaternion.LookRotation(direction.Flat().normalized);
		
		transform.position = position;
		transform.rotation = Quaternion.identity;
		
		state.UpdateInput(Vector3.zero, direction);
		
		behaviour.locomotion.transform.rotation = q;
		behaviour.Reset();
		
		Reset();
	}
	
	public override void Init() {
		InitAction();
		behaviour.Begin();
		
		state.SetDefault(defaultAction);
	}
	
	
	protected virtual void InitAction() {
		// defaultAction = new HA_DefaultAction();
		// run = new HA_Run();
		// jump = new HA_Jump();
		// fall = new HA_Fall();
		// roll = new HA_Roll();
		// flip = new HA_Flip();
		// climb = new HA_Climb();
		// fallDown = new HA_FallDown();
		// gitup = new HA_Gitup();
		
		// unarmedMoveStyle = InitUnarmedMoveStyle();
		unarmedFightStyle = InitUnarmedFightStyle();
		SetFightStyle(unarmedFightStyle);
	}
	
	// protected virtual MoveStyleBase InitUnarmedMoveStyle() {
		// return MoveStyleBase.Default();
	// }
	
	protected virtual FightStyleBase InitUnarmedFightStyle() {
		return new FS_Default();
	}
	
	protected override IEnumerator OnPlayerControl() {
		state.Reset();
		
		while(true) {
			if(Legal(state.defaultAction.isProcess))
				yield return StartCoroutine(OnGroundDefault());
			if(!attribute.isBalance && attribute.isStandup)
				yield return StartCoroutine(FallDown());
			if(attribute.isInAir && attribute.isAlive)
				yield return StartCoroutine(Fall());
			if(Legal())
				state.Reset();
			yield return null;
		}
	}
	
	protected override IEnumerator AIControl() {
		state.Reset();
		while(true) {
			if(Legal(state.defaultAction.isProcess))
				yield return StartCoroutine(AI_OnGroundDefault());
			else if(!attribute.isBalance && attribute.isStandup)
				yield return StartCoroutine(FallDown());
			else if(attribute.isInAir && attribute.isAlive)
				yield return StartCoroutine(AI_Fall());
			else if(Legal())
				state.Reset();
			
			yield return null;
		}
	}
	
	protected virtual IEnumerator AI_OnGroundDefault() {
		while(Legal(state.defaultAction.isProcess)) {
			yield return null;
		}
	}
	
	protected virtual IEnumerator AI_Fall() {
		var detection = behaviour.locomotion.detection;
		state.Play(fall);
		while(Legal(fall.isProcess)) {
			if(CanPerform(detection.canClimb))
				yield return StartCoroutine(DoStateLegal(climb));
			else if(CanPerform(detection.canFlip))
				yield return StartCoroutine(DoStateLegal(flip));
			
			yield return null;
		}
		if(fall.dropVelocity.y < -5.5f || fall.dropVelocity.Flat().magnitude > 5.5f) {
			behaviour.attribute.isOnGround = false;
			behaviour.locomotion.body.data.velocity = fall.dropVelocity;
			yield return StartCoroutine(DoStateLegal(roll));
		}
	}
	
	protected virtual IEnumerator OnGroundDefault() {
		while(Legal(state.defaultAction.isProcess)) {
			UpdateInput();
			
			if(input.action_0.IsPress()) {
				yield return StartCoroutine(fightStyle.FightStanceA());
				state.Reset();
			}
			
			if(input.action_1.IsPress()) {
				yield return StartCoroutine(fightStyle.FightStanceB());
				state.Reset();
			}
			
			if(IsRun())
				yield return StartCoroutine(Run());
			if(IsJump())
				yield return StartCoroutine(CheckJump());
			
			yield return null;
		}
	}
	
	// protected virtual IEnumerator FightStanceA() {
		// state.SetDefault(fightStance);
		// state.Play(fightStance);
		// if(punch.isProcess) punch.Stop();
		
		// while(Legal(fightStance.isProcess || punch.isProcess)) {
			// UpdateInput();
			
			// if(input.action_0.IsDown() && !punch.isProcess)
				// state.Play(punch);
			
			// if(input.action_0.IsPress()) {
				// if(input.jump.IsDown())
					// yield return StartCoroutine(DoStateLegal(kick));
				
				// if(input.run.IsDown() && attribute.airTime < 0.5f)
					// yield return StartCoroutine(DoStateLegal(kaoDa));
			// } else if(IsRun() && !punch.isProcess)
				// break;
			// else if(input.action_1.IsPress() && !punch.isProcess)
				// break;
			
			// if(input.jump.IsDown())
				// yield return StartCoroutine(DoStateLegal(doubleKick));
			
			// yield return null;
		// }
		// state.SetDefault(defaultAction);
		// state.Reset();
	// }
	
	// protected virtual IEnumerator FightStanceB() {
		// state.Play(yinZhang);
		// while(Legal(yinZhang.isProcess && input.action_1.IsPress())) {
			// UpdateInput();
			
			// if(input.action_0.IsDown() || input.run.IsDown()) {
				// pianNing.throwTarget = yinZhang.GetGrabTarget();
			
				// var f = Vector3.Dot(state.behaviour.locomotion.transform.forward, state.inputDirection);
				// if(f > 0.35f)
					// yield return StartCoroutine(DoStateLegal(rfsb));
				// else
					// yield return StartCoroutine(DoStateLegal(pianNing));
			// }
			
			// yield return null;
		// }
		// state.Reset();
	// }
	
	protected IEnumerator Run() {
		var detection = behaviour.locomotion.detection;
		state.Play(run);
		while(Legal(run.isProcess && !input.run.IsUp() && !IsJump())) {
			UpdateInput();
			
			if(CanPerform(detection.canClimb))
				yield return StartCoroutine(DoStateLegal(climb));
			else if(CanPerform(detection.canFlip))
				yield return StartCoroutine(DoStateLegal(flip));
			// else if(CanPerform(detection.flip.isHit))
				// yield return StartCoroutine(DoJump(1f,1.15f));
			
			yield return null;
		}
		run.Stop();
	}
	
	protected IEnumerator CheckJump() {
		var detection = behaviour.locomotion.detection;
		if(CanPerform(detection.canClimb))
			yield return StartCoroutine(DoStateLegal(climb));
		else if(CanPerform(detection.canFlip))
			yield return StartCoroutine(DoStateLegal(flip));
		else
			yield return StartCoroutine(DoJump(5,0.5f));
	}
	
	protected virtual IEnumerator Fall() {
		var detection = behaviour.locomotion.detection;
		state.Play(fall);
		while(Legal(fall.isProcess)) {
			
			if(input.jump.IsDown() && behaviour.locomotion.body.data.velocity.y > 0 && attribute.airTime < 1f)
				yield return StartCoroutine(DoStateLegal(roll));
			else if(CanPerform(detection.canClimb))
				yield return StartCoroutine(DoStateLegal(climb));
			else if(CanPerform(detection.canFlip))
				yield return StartCoroutine(DoStateLegal(flip));
			
			yield return null;
		}
		if(fall.dropVelocity.y < - 5.5f || fall.dropVelocity.Flat().magnitude > 5.5f) {
			behaviour.attribute.isOnGround = false;
			behaviour.locomotion.body.data.velocity = fall.dropVelocity;
			yield return StartCoroutine(DoStateLegal(roll));
		}
	}
	
	protected IEnumerator FallDown() {
		state.Play(fallDown);
		while(fallDown.isProcess) {
			yield return null;
		}
		
		if(attribute.isAlive)
			yield return StartCoroutine(Gitup());
		else {
			behaviour.enabled = false;
			yield return new WaitForSeconds(5f);
			
			StopAllCoroutines();
			this.enabled = false;
			behaviour.constraint.ForEach(x=>x.rb.collisionDetectionMode = CollisionDetectionMode.Discrete);
			behaviour.constraint.ForEach(x=>x.rb.isKinematic = true);
		}
		state.Reset();
		state.UpdateInput(Vector3.zero, behaviour.locomotion.direction.forward);
	}
	
	protected IEnumerator Gitup() {
		state.Play(gitup);
		while(gitup.isProcess) {
			yield return null;
		}
	}
	
	protected IEnumerator Dead() {
		while(true) {
			yield return null;
		}
	}
	
	protected IEnumerator DoJump(float jumpSpeed, float crouchHeight) {
		var t = 0f;
		var b = false;
		jump.jumpSpeed = jumpSpeed;
		jump.crouchHeight = crouchHeight;
		state.Play(jump);
		yield return null;
		while(Legal(jump.isProcess)) {
			t += Time.deltaTime;
			UpdateInput();
			if(input.jump.IsDown()) {
				if(t < 0.3f)
					yield return StartCoroutine(DoStateLegal(roll));
				else
					b = true;
			}
			else if(b && behaviour.locomotion.body.data.velocity.y > 2.5f)
				yield return StartCoroutine(DoStateLegal(roll));
			
			yield return null;
		}
	}
	
//TOOL
/****************************************************************************************/	
	
	public bool Legal(bool state = true) {
		return attribute.isAlive && attribute.isStandup && attribute.isBalance && state && isPlaying;
	}
	
	public virtual IEnumerator DoState(HumanoidAction ha) {
		state.Play(ha);
		while(ha.isProcess) {
			UpdateInput();
			yield return null;
		}
	}
	
	public virtual IEnumerator DoStateLegal(HumanoidAction ha) {
		state.Play(ha);
		while(Legal(ha.isProcess)) {
			UpdateInput();
			yield return null;
		}
	}
	
	public bool CanPerform(bool flipClimbAction) {
		return flipClimbAction && 
			Vector3.Dot(behaviour.locomotion.direction.forward, behaviour.locomotion.body.data.velocity.Flat().normalized) > -0.1f;
	}
	
	public bool IsRun() {
		return input.run.IsPress() && !input.jump.IsDown() && attribute.airTime < 0.5f && state.inputMove.sqrMagnitude > 0.1f;
	}
	
	public bool IsJump() {
		return input.jump.IsDown() && attribute.airTime < 0.5f && behaviour.locomotion.detection.down.HitLessThan(1.25f);
	}
	
	public bool FightKeyDown() {
		return input.action_0.IsDown() || input.action_1.IsDown() || input.action_2.IsDown();
	}
	public bool FightKeyOn() {
		return input.action_0.IsPress() || input.action_1.IsPress() || input.action_2.IsPress();
	}
	
	public void UpdateInput() {
		if(!isControl) return;
		// state.UpdateInput(
			// CameraFollow.Convert(IPT.AxisXZ("Horizontal", "Vertical")), 
			// CameraFollow.Instance.transform.forward);
		
		var t = CameraFollow.Instance.transform;
		
		input.UpdateAxis(t.position - t.forward * 0.25f,
			(move, face)=>
				state.UpdateInput(CameraFollow.Convert(move), face));
	}
}
