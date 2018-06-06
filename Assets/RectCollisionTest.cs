using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoundRect;
using NP.Convex.Shape;

public class RectCollisionTest : MonoBehaviour {

	ConvexRect rect1;
	ConvexRect rect2;

	public Vector2 center1;
	public Vector2 center2;

	// Use this for initialization
	void Start () {

		rect1 = new ConvexRect (center1, new Vector2 (3.0f, 3.0f));
		rect2 = new ConvexRect (center2, new Vector2 (2.0f, 2.0f));
	}
	
	// Update is called once per frame
	void Update () {

		rect1.center = center1;
		rect2.center = center2;

		CheckCollision ();
	}

	void CheckCollision(){

		Debug.Log (rect2.CollideWithRect (rect1));

		/*
		//projection axis
		Vector2 axisP = new Vector2 (1.0f, 0.0f);
		Vector2 axisQ = new Vector2 (0.0f, 1.0f);

		//max point of rect 
		Vector2 rect1MaxPoint = new Vector2 (rect1.xMax, rect1.yMax);
		Vector2 rect2MaxPoint = new Vector2 (rect2.xMax, rect2.yMax);
		Vector2 rect1MinPoint = new Vector2 (rect1.xMin, rect1.yMin);
		Vector2 rect2MinPoint = new Vector2 (rect2.xMin, rect2.yMin);

		//Vector of each max point from world space origin
		Vector2 vMaxRect1 = rect1MaxPoint - Vector2.zero;
		Vector2 vMaxRect2 = rect2MaxPoint - Vector2.zero;
		Vector2 vMinRect1 = rect1MinPoint - Vector2.zero;
		Vector2 vMinRect2 = rect2MinPoint - Vector2.zero;

		//project vector on x axis
		float maxProjRect1X = Vector2.Dot (vMaxRect1, axisP);
		float maxProjRect2X = Vector2.Dot (vMaxRect2, axisP);
		float minProjRect1X = Vector2.Dot (vMinRect1, axisP);
		float minProjRect2X = Vector2.Dot (vMinRect2, axisP);

		bool gapX = maxProjRect1X < minProjRect2X || minProjRect1X > maxProjRect2X;


		//project vector on y axis
		float maxProjRect1Y = Vector2.Dot (vMaxRect1, axisQ);
		float maxProjRect2Y = Vector2.Dot (vMaxRect2, axisQ);
		float minProjRect1Y = Vector2.Dot (vMinRect1, axisQ);
		float minProjRect2Y = Vector2.Dot (vMinRect2, axisQ);

		bool gapY = minProjRect2Y > maxProjRect1Y || maxProjRect2Y < minProjRect1Y;

		if (gapX || gapY)
			Debug.Log("No collision");
		else
			Debug.Log ("collision");
		*/
		/*
		bool collision = true;

		Vector2[] rect1Normals = GetRectangleNormal (rect1);
		Vector2[] rect1AllCorners = rect1.AllCorners;
		Vector2[] rect2AllCorners = rect2.AllCorners;

		//For each normal in rect1
		for (int i = 0; i < rect1Normals.Length; i++) {

			//find rect1 max min projection
			float r1Dot1 = Vector2.Dot (rect1Normals [i], rect1AllCorners [0]);
			float r1Dot2 = Vector2.Dot (rect1Normals [i], rect1AllCorners [1]);
			float r1Dot3 = Vector2.Dot (rect1Normals [i], rect1AllCorners [2]);
			float r1Dot4 = Vector2.Dot (rect1Normals [i], rect1AllCorners [3]);

			float r1PMin = Mathf.Min (r1Dot1, Mathf.Min (r1Dot2, Mathf.Min (r1Dot3, r1Dot4)));
			float r1PMax = Mathf.Max (r1Dot1, Mathf.Max (r1Dot2, Mathf.Max (r1Dot3, r1Dot4)));

			float r2Dot1 = Vector2.Dot (rect1Normals [i], rect2AllCorners [0]);
			float r2Dot2 = Vector2.Dot (rect1Normals [i], rect2AllCorners [1]);
			float r2Dot3 = Vector2.Dot (rect1Normals [i], rect2AllCorners [2]);
			float r2Dot4 = Vector2.Dot (rect1Normals [i], rect2AllCorners [3]);

			float r2PMin = Mathf.Min (r2Dot1, Mathf.Min (r2Dot2, Mathf.Min (r2Dot3, r2Dot4)));
			float r2PMax = Mathf.Max (r2Dot1, Mathf.Max (r2Dot2, Mathf.Max (r2Dot3, r2Dot4)));

			if (r2PMin > r1PMax || r2PMax < r1PMin) {

				collision = false;

				break;
			}
		}

		if (collision)
			Debug.Log ("Collision");
		else
			Debug.Log ("No collision");
		*/
		
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

			normals [i] = normal;
		}

		v = corners [0] - corners [3];
		normal = new Vector2 (-v.y, v.x);

		normals [3] = normal;

		return normals;
	}
	*/

	void OnDrawGizmos(){

		if (rect1 != null) {

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
			Gizmos.DrawLine (new Vector3 (0.0f, 0.0f), new Vector2 (rect1.xMin, rect1.yMin));

			Gizmos.color = Color.yellow;
			Gizmos.DrawLine (new Vector3 (0.0f, 0.0f), new Vector2 (rect1.xMax, rect1.yMax));
		}

		if (rect2 != null) {

			//draw bound
			Gizmos.color = Color.white;
			Vector2[] corners = rect2.AllCorners;

			for (int i = 0; i < corners.Length-1; i++) {

				Gizmos.DrawLine (new Vector3 (corners [i].x, corners [i].y),
					new Vector3 (corners [i + 1].x, corners [i + 1].y));
			}

			Gizmos.DrawLine (new Vector3 (corners [3].x, corners [3].y),
				new Vector3 (corners [0].x, corners [0].y));

			Gizmos.color = Color.red;
			Gizmos.DrawLine (new Vector3 (0.0f, 0.0f), new Vector2 (rect2.xMin, rect2.yMin));

			Gizmos.color = Color.yellow;
			Gizmos.DrawLine (new Vector3 (0.0f, 0.0f), new Vector2 (rect2.xMax, rect2.yMax));
		}
			
	}
}
