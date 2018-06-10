using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NP.Convex.Shape;

public class CircleCircleTest : MonoBehaviour {

	ConvexCircle c1;
	ConvexCircle c2;

	public Vector2 circle1Position;
	public float c1Radius = 3.0f;
	public Vector2 circle2Position;
	public float c2Radius = 3.0f;

	// Use this for initialization
	void Start () {

		c1 = new ConvexCircle (circle1Position, c1Radius);
		c2 = new ConvexCircle (circle2Position, c2Radius);
	}
	
	// Update is called once per frame
	void Update () {


		c1.Center = circle1Position;
		c1.Radius = c1Radius;
		c2.Center = circle2Position;
		c2.Radius = c2Radius;

		CheckCollision ();
		
	}

	void CheckCollision(){

		Debug.Log (c1.IntersectWithShape (c2));
	}

	void OnDrawGizmos(){

		if (c1 == null || c2 == null)
			return;

		//circle 1
		Gizmos.color = Color.white;
		float x = c1.Radius*Mathf.Cos(0);
		float y = c1.Radius*Mathf.Sin(0);
		Vector2 pos = c1.Center + new Vector2 (x, y);
		Vector2 newPos = pos;
		Vector2 lastPos = pos;
		for(float theta = 0.1f; theta<Mathf.PI*2.0f; theta+=0.1f){
			x = c1.Radius*Mathf.Cos(theta);
			y = c1.Radius*Mathf.Sin(theta);
			newPos = c1.Center+ new Vector2(x,y);
			Gizmos.DrawLine(pos,newPos);
			pos = newPos;
		}
		Gizmos.DrawLine(pos,lastPos);


		//circle 2
		Gizmos.color = Color.white;
		float c2x = c2.Radius*Mathf.Cos(0);
		float c2y = c2.Radius*Mathf.Sin(0);
		Vector2 c2pos = c2.Center + new Vector2 (c2x, c2y);
		Vector2 c2newPos = c2pos;
		Vector2 c2lastPos = c2pos;
		for(float theta = 0.1f; theta<Mathf.PI*2.0f; theta+=0.1f){
			c2x = c2.Radius*Mathf.Cos(theta);
			c2y = c2.Radius*Mathf.Sin(theta);
			c2newPos = c2.Center+ new Vector2(c2x,c2y);
			Gizmos.DrawLine(c2pos,c2newPos);
			c2pos = c2newPos;
		}
		Gizmos.DrawLine(c2pos,c2lastPos);
	}
}
