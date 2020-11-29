using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;

using UnityEngine;
using URD = UnityEngine.Random;

public class BloodPopManager : InstanceBehaviour<BloodPopManager> {
	private const float groundOffset = 0.1f;
	
	public LayerMask mask = 1;
	public BloodPop bloodPop_prefab, playerBloodPop_prefab;
	public ParticleSystem bloodPop, bloodPool;
	
	public static void PlayerPopDamage(float damage, Vector3 point, Vector3 hitPoint) {
		if(damage > URD.Range(5,10)) {
			var temp = GT.Instantiate(Instance.playerBloodPop_prefab, point + Vector3.up*0.4f, Quaternion.identity);
			temp.Set(damage);
		}
		
		Instance.bloodPop.transform.position = hitPoint;
		Instance.bloodPop.Emit((int)Mathf.Clamp(damage/5, 5, 30));
		
		if(damage > 15 || URD.value > 0.35f) {
			SetDeathBlood(point, Mathf.Clamp(damage/30, 1, 5));
		}
		ScreenBlood.Splash(damage);
	}
	
	public static void PopDamage(float damage, Vector3 point, Vector3 hitPoint) {
		if(damage > URD.Range(5,10)) {
			var temp = GT.Instantiate(Instance.bloodPop_prefab, point + Vector3.up*0.4f, Quaternion.identity);
			temp.Set(damage);
		}
		
		Instance.bloodPop.transform.position = hitPoint;
		Instance.bloodPop.Emit((int)Mathf.Clamp(damage/5, 5, 30));
		
		if(damage > 15 || URD.value > 0.35f) {
			SetDeathBlood(point, Mathf.Clamp(damage/30, 1, 5));
		}
	}
	
	public static void PopBlood(Vector3 point, float amount) {
		Instance.bloodPop.transform.position = point;
		Instance.bloodPop.Emit((int)(URD.Range(3f,5f) * amount));
	}
	
	public static void SetDeathBlood(Vector3 point, float amount) {
		var p = point;
		bool isHit = true;
		Cast.LineRay(point, -Vector3.up, 4, Instance.mask, hit=>p = hit.point, ()=>isHit = false);
		
		if(!isHit) return;
		
		Instance.bloodPool.transform.position = new Vector3(p.x, p.y + groundOffset, p.z);
		Instance.bloodPool.Emit((int)(URD.Range(1.75f,2.25f) * amount));
	}
	
}
