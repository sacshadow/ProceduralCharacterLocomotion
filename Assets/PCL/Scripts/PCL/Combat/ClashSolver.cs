using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

//using UnityEditor;
using UnityEngine;
//using UnityEngine.UI;
//using Uhity.Entities;

using URD = UnityEngine.Random;

public static class ClashSolver {
	
	/*
	public static void Process(RBGroup lhs, RBGroup rhs, List<Vector3> point) {
		var clashPoint = AverageOf(point);
		// var midPoint = (lhs.root.data.position + rhs.root.data.position)/2f;
		
		// clashPoint = Vector3.Lerp(midPoint, clashPoint, lhs.constraintAttribute.stiffness);
		
		var lhsData = lhs.root.data;
		var rhsData = rhs.root.data;
		
		var dispToLhs = clashPoint - lhs.root.data.position;
		var dispToRhs = clashPoint - rhs.root.data.position;
		
		var velocity_L = lhsData.velocity + Vector3.Cross(lhsData.angularVelocity, dispToLhs);
		var velocity_R = rhsData.velocity + Vector3.Cross(rhsData.angularVelocity, dispToRhs);
		
		var speed_L = Vector3.Dot(dispToLhs, velocity_L);
		// var speed_R = Vector3.Dot(dispToRhs, velocity_R);
		var clashSpeedR = Vector3.Dot(dispToLhs, velocity_R);
		
		//Return if not clash
		if(speed_L < 0)
			return;
		if(speed_L < clashSpeedR)
			return;
		if(Vector3.Dot(dispToLhs, dispToRhs) > 0)
			return;
		
		// var adjust = Mathf.Lerp(4f,0.25f, (dispToLhs.magnitude - 0.3f));
		var adjust = Mathf.Lerp(2f,0.25f, (dispToLhs.magnitude - 0.3f));
		// var modify = Mathf.Lerp(4f, 0.5f, lhs.constraintAttribute.stiffness);
		var modify = 1f;
		
		// adjust *= rhs.attribute.airTime <0.5f && rhs.attribute.isBalance? 1 : 0.1f;
		// adjust *= lhs.attribute.airTime <0.5f && lhs.attribute.isBalance ? 1 : 0.1f;
		
		// var lhsVA = Vector3.ClampMagnitude(dispToLhs.normalized * (speed_L - clashSpeedR),5f);
		var lhsVA = dispToLhs.normalized * (speed_L - clashSpeedR);
		
		var clashForce = lhsVA * lhsData.mass * PCLSimulation.reciDeltaTime;
		// var clashForce = Vector3.ClampMagnitude(lhsVA * lhsData.mass * PCLSimulation.reciDeltaTime,4*rhsData.mass);
		var clashVelocity = lhsVA;
		
		
		
		// var adjustForce = clashForce * lhs.constraintAttribute.stiffness * adjust;
		var adjustForce = clashForce * adjust;
		var reactForce = -dispToRhs.normalized * Vector3.Dot(dispToRhs.normalized, adjustForce);
		
		rhs.root.AddForce(adjustForce * modify);
		rhs.root.AddTorque(Vector3.Cross(dispToRhs, adjustForce));
		
		lhs.root.AddForce(reactForce * modify);
		lhs.root.AddTorque(Vector3.Cross(dispToLhs,-adjustForce));
		
		// var clashTorque = Vector3.Cross(dispToRhs, clashForce*0.25f*adjust);
		var clashTorque = Vector3.Cross(dispToRhs, adjustForce);
		rhs.ForEachConstraint(x=>ApplyBodyDeform(rhs, x, clashForce*adjust, clashTorque, clashPoint));
		lhs.ForEachConstraint(x=>ApplyBodyDeform(lhs, x, -clashForce*adjust, -clashTorque, clashPoint));
		
		// Debug.DrawRay(lhs.root.data.position, dispToLhs, rhs.debugColor);
		// Debug.DrawRay(rhs.root.data.position, dispToRhs, rhs.debugColor);
		// Debug.DrawRay(clashPoint, velocity_L, rhs.debugColor);
		
		// Debug.DrawRay(clashPoint, Vector3.Cross(dispToRhs, adjustForce * 0.25f), rhs.debugColor);
		// Debug.Break();
		
		CombatManager.ClashOnUnit(lhs, rhs, clashPoint, clashVelocity, clashForce);
	}
	*/
	
	/*
	public static void Process(RBGroup lhs, RBGroup rhs, List<Vector3> point) {
		var averagePoint = AverageOf(point);
		// var midPoint = (lhs.root.data.position + rhs.root.data.position)/2f;
		// var clashPoint = Vector3.Lerp(midPoint, averagePoint, rhs.constraintAttribute.stiffness);
		
		var clashPoint = averagePoint;
		
		var lhsData = lhs.root.data;
		var rhsData = rhs.root.data;
		
		var dispToLhs = clashPoint - lhs.root.data.position;
		var dispToRhs = clashPoint - rhs.root.data.position;
		
		var velocity_L = lhsData.velocity + Vector3.Cross(lhsData.angularVelocity, dispToLhs);
		var velocity_R = rhsData.velocity + Vector3.Cross(rhsData.angularVelocity, dispToRhs);
		
		var speed_L = Vector3.Dot(dispToLhs, velocity_L);
		// var speed_R = Vector3.Dot(dispToRhs, velocity_R);
		var clashSpeedR = Vector3.Dot(dispToLhs, velocity_R);
		
		//Return if not clash
		if(speed_L < 0)
			return;
		if(speed_L < clashSpeedR)
			return;
		if(Vector3.Dot(dispToLhs, dispToRhs) > 0)
			return;
		
		var v = lhsData.velocity + rhsData.velocity;
		var s = Mathf.Lerp(1f,4f,v.magnitude/8f);
		// var c = Mathf.Lerp(1.25f,0.5f,rhs.constraintAttribute.stiffness);
		// var adjust = Mathf.Lerp(s,0.05f, (dispToLhs.magnitude - 0.3f)*2f);
		var adjust = Mathf.Lerp(s,0.05f, (dispToRhs.magnitude - 0.25f)*2f);
		
		// var adjust = Mathf.Lerp(4f,0.25f, (dispToLhs.magnitude - 0.3f));
		
		// var modify = Mathf.Lerp(4f, 0.5f, lhs.constraintAttribute.stiffness);
		var modify = 1f;
		var tModify = Mathf.Lerp(0.15f, 2f, lhs.constraintAttribute.stiffness - rhs.constraintAttribute.stiffness);
		// var tModify = Mathf.Lerp(0.25f, 1f, rhs.constraintAttribute.stiffness);
		
		// adjust *= rhs.attribute.airTime <0.5f && rhs.attribute.isBalance? 1 : 0.1f;
		// adjust *= lhs.attribute.airTime <0.5f && lhs.attribute.isBalance ? 1 : 0.1f;
		
		// var lhsVA = Vector3.ClampMagnitude(dispToLhs.normalized * (speed_L - clashSpeedR),3);
		// var lhsVA = dispToLhs.normalized * (speed_L - Mathf.Clamp(clashSpeedR,0,10));
		var lhsVA = dispToLhs.normalized * (speed_L - clashSpeedR);
		
		var clashForce = lhsVA * lhsData.mass * PCLSimulation.reciDeltaTime;
		// var clashForce = Vector3.ClampMagnitude(lhsVA * lhsData.mass * PCLSimulation.reciDeltaTime,2*rhsData.mass);
		var clashVelocity = lhsVA;
		
		// var adjustForce = clashForce * lhs.constraintAttribute.stiffness * adjust;
		var adjustForce = clashForce * adjust;
		// var reactForce = -dispToRhs.normalized * Vector3.Dot(dispToRhs.normalized, adjustForce);
		// var reactForce = -dispToRhs.normalized * adjustForce.magnitude;
		// adjustForce = -reactForce;
		
		// var k = Mathf.Lerp(1,0,(rhsData.velocity + rhsData.angularVelocity*10).magnitude/3f);
		// adjustForce *= k;
		
		rhs.root.AddForce(adjustForce * modify);
		rhs.root.AddTorque(Vector3.Cross(dispToRhs, adjustForce * tModify * modify));
		
		lhs.root.AddForce(-adjustForce * modify);
		lhs.root.AddTorque(Vector3.Cross(dispToLhs, -adjustForce * tModify * modify));
		
		// var clashTorque = Vector3.Cross(dispToRhs, clashForce*0.25f*adjust);
		
		var clashTorque = Vector3.Cross(dispToRhs, adjustForce);
		rhs.ForEachConstraint(x=>ApplyBodyDeform(rhs, x, clashForce*adjust, clashTorque, clashPoint));
		lhs.ForEachConstraint(x=>ApplyBodyDeform(lhs, x, -clashForce*adjust, -clashTorque , clashPoint));
		
		// Debug.DrawRay(lhs.root.data.position, dispToLhs, rhs.debugColor);
		// Debug.DrawRay(rhs.root.data.position, dispToRhs, rhs.debugColor);
		// Debug.DrawRay(clashPoint, velocity_L, rhs.debugColor);
		
		CombatManager.ClashOnUnit(lhs, rhs, clashPoint, clashVelocity, clashForce);
	}
	*/
	
	public static void Process(RBGroup lhs, RBGroup rhs, ClashData clashData) {
		var clashPoint = AverageOf(clashData.clashPoint);
		var dataL = lhs.root.data;
		var dataR = rhs.root.data;
		
		var lhsStiffness = lhs.constraintAttribute.stiffness;
		var rhsStiffness = rhs.constraintAttribute.stiffness;
		
		var dispToLhs = clashPoint - lhs.constraintSkeleton.GetCOM();
		var dispToRhs = clashPoint - rhs.constraintSkeleton.GetCOM();
		
		var lhsVelo = dataL.velocity + Vector3.Cross(dataL.angularVelocity, dispToLhs);
		var rhsVelo = dataR.velocity + Vector3.Cross(dataR.angularVelocity, dispToRhs);
		
		var lhsSpeed = Vector3.Dot(dispToLhs, lhsVelo);
		var rhsSpeed = Vector3.Dot(dispToRhs, rhsVelo);
		
		if(lhsSpeed - rhsSpeed > 0)
			return;
		if(Vector3.Dot(dispToLhs, dispToRhs) > 0)
			return;
		
		// var adjust = Mathf.Lerp(1,0.5f, dispToLhs.magnitude) * (16f - lhsStiffness * 16f + rhsStiffness * 16f);
		// var adjust = (2 - lhsStiffness + rhsStiffness) * 8f * Mathf.Lerp(1,0.25f, dispToLhs.magnitude*2f);
		var adjust = (4 - lhsStiffness*4 + rhsStiffness*4) * 1f;
		
		if(lhs.attribute.isBalance)
			adjust *= Mathf.Lerp(1,0.5f, (dispToLhs.magnitude - 0.3f)*2f);
		
		var lhsVA = lhsSpeed * dispToLhs.normalized;
		var lhsVB = lhsVelo - lhsVA;
		
		var rhsVA = rhsSpeed * dispToRhs.normalized;
		var rhsVB = rhsVelo - rhsVA;
		
		var clashVelocity = 
			(lhsVA * (dataL.mass - dataR.mass) + 2f * dataR.mass * rhsVA) /
			(dataL.mass + dataR.mass) 
			+ lhsVB;
		
		var clashForce = (clashVelocity - lhsVelo) * adjust * dataR.mass;
		
		// var modify = Mathf.Lerp(1,0.5f, (dispToLhs - dispToRhs).magnitude*2f);
		// var modify = Mathf.Lerp(1,0.5f, (dispToRhs).magnitude*3f);
		
		var modify = Mathf.Lerp(2f, 12f, clashData.clashMass/12.5f);
		
		if(!lhs.attribute.isBalance)
			modify = Mathf.Max(modify, lhs.attribute.isInAir ? 32f : 16f);
		
		var groundPoint = lhs.GetGroundPoint();
		var clashDisp = clashPoint - groundPoint;
		
		// if(lhs.attribute.isOnGround)
		
		var cf = clashForce * 4f;
		// var cf = clashForce * modify;
		// cf.y /= modify;
		
		// var ct = clashForce * modify * 0.125f * 0.5f;
		var k = dataL.position.y - groundPoint.y;
		var ct = clashForce * Mathf.Lerp(12f, modify*0.125f, clashDisp.y + (1- k));
		
		// var ct = clashForce * Mathf.Lerp(0.5f, 0.85f,  rhsStiffness) * modify * Mathf.Lerp(1,0.25f, (dispToRhs).magnitude*3f);
		// if(rhs.attribute.airTime > 0.25f || (rhs.attribute.isInAir && rhsSpeed > 2f)) {
			// cf *= 1.5f;
			// ct = clashForce * 4 * modify;
		// }
		
		// if(lhs.attribute.clashShock > 0.05f) {
			// cf *= Mathf.Lerp(1,4,lhs.attribute.clashShock - lhsStiffness);
			// ct *= Mathf.Lerp(1,4,lhs.attribute.clashShock - lhsStiffness);
		// }
		
		lhs.root.AddForce(cf);
		// lhs.root.AddForce(clashForce * 8f * modify);
		lhs.root.AddTorque(Vector3.Cross(dispToLhs, ct));
		// lhs.root.AddTorque(Vector3.Cross(dispToLhs, clashForce *0.125f * modify));
		
		lhs.attribute.clashShock = Mathf.Lerp(0,1, clashForce.magnitude/4f/dataL.mass);
		
		// var offshapeForce = (lhs.constraintSkeleton.GetCOM() - dataL.position) * 8000f;
		// lhs.root.AddForce(offshapeForce);
		
		// rhs.root.AddTorque(Vector3.Cross(rhsVB, dispToRhs) * dataL.mass * adjust);
		
		var clashTorque = Vector3.Cross(dispToRhs, clashForce * 0.5f * modify);
		rhs.ForEachConstraint(x=>ApplyBodyDeform(rhs, x, clashForce*adjust, clashTorque, clashPoint));
		lhs.ForEachConstraint(x=>ApplyBodyDeform(lhs, x, -clashForce*adjust, -clashTorque , clashPoint));
		
		CombatManager.ClashOnUnit(lhs, rhs, clashPoint, clashVelocity, clashForce);
	}
	
	public static void ApplyBodyDeform(RBGroup group, ConstraintBase cb, Vector3 clashForce, Vector3 clashTorque, Vector3 clashPoint) {
		var disp = clashPoint - cb.rb.worldCenterOfMass;
		var amount = cb.rb.mass / group.mass * Mathf.Lerp(1,0, disp.magnitude) * 0.25f;
		// var amount = cb.rb.mass / group.mass * Mathf.Lerp(1,0, disp.magnitude);
		var force = clashForce * amount;
		var torqueForce = Vector3.Cross(clashTorque * amount, disp);
		// var torqueForce = Vector3.zero;
		
		// Debug.DrawRay(cb.rb.worldCenterOfMass, torqueForce, group.debugColor);
		// Debug.DrawRay(cb.rb.worldCenterOfMass, force, group.debugColor);
		// Debug.Break();
		
		cb.ApplyAsHit(Vector3.ClampMagnitude(force + torqueForce, 5f * cb.rb.mass), cb.rb.worldCenterOfMass);
	}
	
	private static Vector3 AverageOf(List<Vector3> point) {
		var sum = Loop.Calculate(point, (a,b)=>a+b);
		return sum / point.Count;
	}
}
