using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoundRect;
using NP.Convex.Shape;

public class RectCircleTest : MonoBehaviour {

	ConvexRect rect1;
	ConvexCircle circle;

	public Vector2 rectCenter;
	public Vector2 circlePosition;
	public float cRadius = 3.0f;


	// Use this for initialization
	void Start () {

		rect1 = new ConvexRect (rectCenter, new Vector2 (3.0f, 3.0f));
		circle = new ConvexCircle (circlePosition, cRadius);
	}
	
	// Update is called once per frame
	void Update () {

		rect1.center = rectCenter;
		circle.Center = circlePosition;
		circle.Radius = cRadius;

		//rect1.RotationDegree += 5 * Time.deltaTime;

		CheckCollision ();
	}

	void CheckCollision(){

		#region old code
		//all corners
//		Vector2[] corners = rect1.AllCorners;
//
//		bool collision = true;
//
//		/**
//		 * Find closest corner to circle center then make a line between that corner and circle center into projection axis
//		 * Projecting all corner to that axis then find min and max corner
//		 * Check if circle is overlap with rectangle
//		 * 
//		 * This solve problem when circle contact each corner vertex
//		 **/
//		//find closest corner and compare distance
//		int closestCornerIndex = 0;
//		int currentCornerIndex = 1;
//		while (currentCornerIndex < corners.Length) {
//
//			if ((circlePos - corners [closestCornerIndex]).sqrMagnitude > (circlePos - corners [currentCornerIndex]).sqrMagnitude)
//				closestCornerIndex = currentCornerIndex;
//
//			currentCornerIndex++;
//		}
//
//		//projection axis from closest corner to circle center
//		Vector2 p = circlePos - corners [closestCornerIndex];
//
//		//base on axis find min and max corner
//		float rMin = 0.0f;
//		float rMax = 0.0f;
//		for (int i = 0; i < corners.Length; i++) {
//
//			rMax = Mathf.Max (rMax, Vector2.Dot (p.normalized, corners [i]));
//			rMin = Mathf.Min (rMin, Vector2.Dot (p.normalized, corners [i]));
//		}
//
//		//find circle center projection
//		float cP = Vector2.Dot (p, circlePos);
//
//		//check if circle is overlap rectangle
//		if (rMax < (cP - circleRadius) || rMin > (cP + circleRadius))
//			collision = false;
//		else
//			collision = true;
//
//		/**
//		 * Find 4 corner's normal and make it as prjection axis
//		 * Go throught each normal(corner)
//		 * Find min and max projection of rectangle's corner base on that axis
//		 * Project circle center on that axis
//		 * Check if circle overlap rectangle
//		 * 
//		 * This solve problem while circle contact edge of rectangle
//		 **/
//		//Ignore edge check if circle not contact with 4 corners(vertices)
//		if (collision == true) {
//
//			Vector2[] normals = GetRectangleNormal (rect1);
//
//			for (int i = 0; i < normals.Length; i++) {
//
//				//4 corners projection
//				float r1Dot1 = Vector2.Dot (normals [i], corners [0]);
//				float r1Dot2 = Vector2.Dot (normals [i], corners [1]);
//				float r1Dot3 = Vector2.Dot (normals [i], corners [2]);
//				float r1Dot4 = Vector2.Dot (normals [i], corners [3]);
//
//				//corner min and max on this normal(projection axis)
//				float r1PMin = Mathf.Min (r1Dot1, Mathf.Min (r1Dot2, Mathf.Min (r1Dot3, r1Dot4)));
//				float r1PMax = Mathf.Max (r1Dot1, Mathf.Max (r1Dot2, Mathf.Max (r1Dot3, r1Dot4)));
//
//				//circle center projection on this normal(projection axis)
//				float circleP = Vector2.Dot (normals [i], circlePos);
//
//				//check if circle overlap rectangle
//				if ((circleP - circleRadius) > r1PMax || (circleP + circleRadius) < r1PMin) {
//
//					collision = false;
//					break;
//				}
//			}
//		}
//
//
//		if (collision) {
//			circleCollideDrawColor = collisionColor;
//			Debug.Log ("collision");
//		} else {
//			circleCollideDrawColor = nonCollisionColor;
//			Debug.Log ("no collision");
//		}
		#endregion

		Debug.Log (rect1.CollideWithCircle(circle));

	}

	/*
	Vector2[] GetRectangleNormal(NodeBound rect){

		Vector2[] normals = new Vector2 [4];

		Vector2[] corners = rect.AllCorners;

		Vector2 v;
		Vector2 normal;
		for (int i = 0; i < corners.Length - 1; i++) {

			v = corners [i + 1] - corners [i];
			normal = new Vector2 (-v.y, v.x);

			normals [i] = normal.normalized;
		}

		v = corners [0] - corners [3];
		normal = new Vector2 (-v.y, v.x);

		normals [3] = normal.normalized;

		return normals;
	}
	*/

	/*
	Vector2 GetVectorNormal(Vector2 vector, bool leftHand = true){

		if (leftHand)
			return new Vector2 (-vector.y, vector.x).normalized;
		else
			return new Vector2 (vector.y, -vector.x).normalized;
	}
	*/
	void OnDrawGizmos(){

		if (rect1 == null)
			return;
		
		//draw bound
		Gizmos.color = Color.white;
		Vector2[] corners = rect1.AllCorners;

		for (int i = 0; i < corners.Length-1; i++) {

			Gizmos.DrawLine (new Vector3 (corners [i].x, corners [i].y),
				new Vector3 (corners [i + 1].x, corners [i + 1].y));
		}

		Gizmos.DrawLine (new Vector3 (corners [3].x, corners [3].y),
			new Vector3 (corners [0].x, corners [0].y));


		Gizmos.color = Color.red;
		Vector2 nVector;
		Vector2 nVectorNormalize;
		Vector2 normalVector;
		Vector2 endPoint;
		for (int i = 0; i < corners.Length-1; i++) {

			nVector = corners [i + 1] - corners [i];
			nVectorNormalize = nVector.normalized;
			normalVector = new Vector2 (-nVectorNormalize.y, nVectorNormalize.x);
			endPoint = corners [i] + normalVector * 2.0f;

			Gizmos.DrawLine (new Vector3 (corners [i].x, corners [i].y), 
				new Vector3(endPoint.x, endPoint.y));
		}

		nVector = corners [0] - corners [3];
		nVectorNormalize = nVector.normalized;
		normalVector = new Vector2 (-nVectorNormalize.y, nVectorNormalize.x);
		endPoint = corners [3] + normalVector * 2.0f;

		Gizmos.DrawLine (new Vector3 (corners [3].x, corners [3].y), 
			new Vector3(endPoint.x, endPoint.y));

		//			Gizmos.color = Color.red;
		//			Gizmos.DrawLine (new Vector3 (0.0f, 0.0f), new Vector2 (rect1.xMin, rect1.yMin));
		//
		//			Gizmos.color = Color.yellow;
		//			Gizmos.DrawLine (new Vector3 (0.0f, 0.0f), new Vector2 (rect1.xMax, rect1.yMax));

		//Gizmos.color = Color.white;
		//Gizmos.DrawSphere (new Vector3 (circlePos.x, circlePos.y), circleRadius);

		//circle
		Gizmos.color = Color.white;
		float x = circle.Radius*Mathf.Cos(0);
		float y = circle.Radius*Mathf.Sin(0);
		Vector2 pos = circle.Center + new Vector2 (x, y);
		Vector2 newPos = pos;
		Vector2 lastPos = pos;
		for(float theta = 0.1f; theta<Mathf.PI*2.0f; theta+=0.1f){
			x = circle.Radius*Mathf.Cos(theta);
			y = circle.Radius*Mathf.Sin(theta);
			newPos = circle.Center+ new Vector2(x,y);
			Gizmos.DrawLine(pos,newPos);
			pos = newPos;
		}
		Gizmos.DrawLine(pos,lastPos);

		/*
		Gizmos.color = Color.green;
		Gizmos.DrawLine (new Vector3 (rect1.center.x, rect1.center.y),
			new Vector3 (circlePos.x, circlePos.y));
		
		Vector2 n1 = GetVectorNormal (rect1.center - circlePos);
		Vector2 n2 = GetVectorNormal (rect1.center - circlePos, false);
		Vector2 n1Point = circlePos + n1 * circleRadius;
		Vector2 n2Point = circlePos + n2 * circleRadius;

		Gizmos.DrawLine (new Vector3 (circlePos.x, circlePos.y),
			new Vector3 (n1Point.x, n1Point.y));
		Gizmos.DrawLine (new Vector3 (circlePos.x, circlePos.y),
			new Vector3 (n2Point.x, n2Point.y));

		Vector2 n1End = (n1Point - rect1.center).normalized * ((rect1.center - circlePos).magnitude + circleRadius) + rect1.center;
		Vector2 n2End = (n2Point - rect1.center).normalized * ((rect1.center - circlePos).magnitude + circleRadius) + rect1.center;
		Gizmos.DrawLine (new Vector3 (rect1.center.x, rect1.center.y),
			new Vector3 (n1End.x, n1End.y));
		Gizmos.DrawLine (new Vector3 (rect1.center.x, rect1.center.y),
			new Vector3 (n2End.x, n2End.y));
			*/
		
	}
}
