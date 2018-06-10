using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NP.Convex.Shape;
using NP.Convex.Collision;

namespace NP.Convex.Collision{

	/**
	 * Collision result
	 **/
	public enum CollisionResult{

		/**
		 * Whole object fit in node boundary
		 **/
		Fit,

		/**
		 * Part of object in node boundary or
		 * cover entire boundary
		 **/
		Overlap,

		/**
		 * Object not intersect or fit in node boundary
		 **/
		None
	}

	/**
	 * Collision interface for all kind of shape
	 **/
	public interface IConvexCollision{

		CollisionResult IntersectWithShape (ConvexShape shape);
		bool ContainPoint2D (Vector2 point);
	}
}

namespace NP.Convex.Shape{

	/**
	 * Definition of convex shape ID
	 **/
	public enum ConvexShapeID{
		Rectangle,
		Circle,
		Unknow
	}

	public class ConvexUtility{

		/**
		 * Get normal for vector
		 **/
		public static Vector2 GetVectorNormal(Vector2 vector, bool leftHand = true){

			if (leftHand)
				return new Vector2 (-vector.y, vector.x).normalized;
			else
				return new Vector2 (vector.y, -vector.x).normalized;
		}
	}

	public abstract class ConvexShape : IConvexCollision{

		#region Properties
		/**
		 * ID of this shape
		 **/
		protected ConvexShapeID _shapeId = ConvexShapeID.Unknow;

		/**
		 * Get id of this shape
		 **/
		public ConvexShapeID ShapeId{ get{ return _shapeId;}}
		#endregion

		#region IConvexCollision
		/**
		 * Check if agent intersect with shape
		 * 
		 * Agent will automatically detect which shape is used to calculate collision
		 **/
		public virtual CollisionResult IntersectWithShape (ConvexShape shape){

			switch (shape.ShapeId) {
			case ConvexShapeID.Rectangle:
				ConvexRect rectShape = shape as ConvexRect;
				if (rectShape == null) {
					#if DEBUG
					Debug.LogError("Unable to down cast ConvexShape to ConvexRect");
					#endif
				}
				return ContactWithRectangle (rectShape);
			case ConvexShapeID.Circle:
				ConvexCircle circleShape = shape as ConvexCircle;
				if (circleShape == null) {
					#if DEBUG
					Debug.LogError("Unable to down cast ConvexShape to ConvexCircle");
					#endif
				}
				return ContactWIthCircle (circleShape);
			case ConvexShapeID.Unknow:
				#if DEBUG
				Debug.LogError("Unknow convex shape");
				#endif
				break;
			}

			return CollisionResult.None;
		}
			
		public abstract bool ContainPoint2D (Vector2 point);
		#endregion

		#region For subclass override
		//public abstract ConvexShape GetShape ();

		/**
		 * Subclass must override
		 **/
		protected abstract CollisionResult ContactWithRectangle (ConvexRect otherRect);

		/**
		 * Subclass must override
		 **/
		protected abstract CollisionResult ContactWIthCircle (ConvexCircle otherCircle);
		#endregion
	}

	/**
	 * A convex of rectangle shape
	 * 
	 * Implement IConvexRectCollision interface
	 **/
	public class ConvexRect : ConvexShape{

		#region Properties
		float _x;

		/**
		 * Get x
		 * 
		 * Set x will move rectangle on x axis and remain
		 * same width. X of center will be changed
		 **/
		public float x {

			get {
				return _x;
			}

			set {
				_x = value;
				CalculateCenter ();
			}
		}

		float _y;

		/**
		 * Get y
		 * 
		 * Set y will move rectangle on y axis and remain
		 * same height. Y of center will be changed 
		 **/
		public float y {

			get {
				return _y;
			}

			set {
				_y = value;
				CalculateCenter ();
			}
		}

		float _width;

		/**
		 * Get width
		 * 
		 * Set width will cause bound's width extend from x while
		 * x remain same position. Center's x will be changed
		 **/
		public float width {

			get {
				return _width;
			}

			set {
				_width = value;
				CalculateCenter ();
			}
		}

		/**
		 * Set width for rectangle extend from center 
		 **/
		public float widthFromCenter{

			set{

				_width = value;

				_x = _center.x - _width / 2.0f;
			}
		}

		float _height;

		/**
		 * Get height
		 * 
		 * Set width will cause bound's height extend from y while
		 * x remain same position. Center's y will be changed
		 **/
		public float height {

			get {
				return _height;
			}

			set {
				_height = value;
				CalculateCenter ();
			}
		}

		/**
		 * Set height for rectangle extend from center 
		 **/
		public float heightFromCenter{

			set{

				_height = value;

				_y = _center.y + _height / 2.0f;
			}
		}

		Vector2 _center;

		/**
		 * Get center of bound
		 * 
		 * Set center will move rectangle on both x and y axis and alter x and y position of topleft corner
		 * while width and height remain the same size.
		 **/
		public Vector2 center{

			get{ 

				return _center;
			}

			set{

				_center = value;

				_x = _center.x - _width / 2.0f;
				_y = _center.y + _height / 2.0f;
			}
		}

		public Vector2 size{ get{ return new Vector2 (_width, _height);}}

		/**
		 * Get 4 corners of bound in clockwise
		 **/
		public Vector2[] AllCorners{

			get{

				Vector2[] corners = new Vector2[4];

				corners [0] = new Vector2 (_x, _y);
				corners [1] = new Vector2 (_x + _width, _y);
				corners [2] = new Vector2 (_x + _width, _y - _height);
				corners [3] = new Vector2 (_x, _y - _height);

				return corners;
			}
		}

		/**
		 * Get TopLeft corner position
		 **/
		public Vector2 TLCorner{ get{  return new Vector2 (_x, _y);}}

		/**
		 * Get TopRight corner position
		 **/
		public Vector2 TRCorner{ get{  return new Vector2 (_x + _width, _y);}}

		/**
		 * Get BottomRight corner position
		 **/
		public Vector2 BRCorner{ get{  return new Vector2 (_x + _width, _y - _height);}}

		/**
		 * Get BottomLeft corner position
		 **/
		public Vector2 BLCorner{ get{  return new Vector2 (_x, _y - _height);}}

		/**
		 * Return 4 corners' normal vector
		 * 
		 * Normal is perpendicular to the vector
		 **/
		public Vector2[] Normals{

			get{ 
			
				Vector2[] normals = new Vector2 [4];

				Vector2[] corners = AllCorners;

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
		}

		public float xMin{

			get{

				return Mathf.Min (_x, _x + _width);
			}
		}

		public float xMax{

			get{

				return Mathf.Max (_x, _x + _width);
			}
		}

		public float yMin{

			get{

				return Mathf.Min (_y, _y - _height);
			}
		}

		public float yMax{

			get{

				return Mathf.Max (_y, _y - _height);
			}
		}

		public Vector2 minCorner{

			get{

				return new Vector2 (xMin, yMin);
			}
		}

		public Vector2 maxCorner{

			get{

				return new Vector2 (xMax, yMax);
			}
		}
		#endregion

		#region Class methods
		public static ConvexRect zero{

			get{
				return new ConvexRect (0.0f, 0.0f, 0.0f, 0.0f);
			}
		}
		#endregion

		#region Constructor
		public ConvexRect(float x, float y, float width, float height){

			_shapeId = ConvexShapeID.Rectangle;

			_x = x;
			_y = y;
			_width = Mathf.Abs(width);
			_height = Mathf.Abs(height);
			CalculateCenter ();
		}

		public ConvexRect(Vector2 center, Vector2 size){

			_shapeId = ConvexShapeID.Rectangle;

			_x = center.x - size.x / 2.0f;
			_y = center.y + size.y / 2.0f;
			_width = Mathf.Abs(size.x);
			_height = Mathf.Abs(size.y);
		}
		#endregion

		#region Private methods
		void CalculateCenter(){

			_center = new Vector2 (_x + _width / 2.0f, _y - _height / 2.0f);
		}
		#endregion

		#region Public methods
		/**
		 * Extend rectangle with amount of value on x and y
		 * 
		 * Extend is from center
		 **/
		public void ExtendBound(Vector2 amount){

			_width = Mathf.Abs (_width + amount.x);
			_height = Mathf.Abs (_height + amount.y);

			_x = _center.x - _width / 2.0f;
			_y = _center.y + _height / 2.0f;
		}
		#endregion

		#region override from ConvexShape class
		protected override CollisionResult ContactWithRectangle (ConvexRect otherRect)
		{
			bool collision = true;

			//this rectangle's normal of 4 corner and use it as projection axis
			Vector2[] rect1Normals = this.Normals;
			Vector2[] rect1AllCorners = this.AllCorners;
			Vector2[] rect2AllCorners = otherRect.AllCorners;

			//For each normals in this rectangle
			for (int i = 0; i < rect1Normals.Length; i++) {

				//Projecting all corners from rect1 to rect1's normal
				float r1Dot1 = Vector2.Dot (rect1Normals [i], rect1AllCorners [0]);
				float r1Dot2 = Vector2.Dot (rect1Normals [i], rect1AllCorners [1]);
				float r1Dot3 = Vector2.Dot (rect1Normals [i], rect1AllCorners [2]);
				float r1Dot4 = Vector2.Dot (rect1Normals [i], rect1AllCorners [3]);

				//Find rect1 max and min projection
				float r1PMin = Mathf.Min (r1Dot1, Mathf.Min (r1Dot2, Mathf.Min (r1Dot3, r1Dot4)));
				float r1PMax = Mathf.Max (r1Dot1, Mathf.Max (r1Dot2, Mathf.Max (r1Dot3, r1Dot4)));

				//Projecting all corners from rect2 to rect1's normal
				float r2Dot1 = Vector2.Dot (rect1Normals [i], rect2AllCorners [0]);
				float r2Dot2 = Vector2.Dot (rect1Normals [i], rect2AllCorners [1]);
				float r2Dot3 = Vector2.Dot (rect1Normals [i], rect2AllCorners [2]);
				float r2Dot4 = Vector2.Dot (rect1Normals [i], rect2AllCorners [3]);

				//Find rect2 max and min projection
				float r2PMin = Mathf.Min (r2Dot1, Mathf.Min (r2Dot2, Mathf.Min (r2Dot3, r2Dot4)));
				float r2PMax = Mathf.Max (r2Dot1, Mathf.Max (r2Dot2, Mathf.Max (r2Dot3, r2Dot4)));

				//Two rectangles not collide each other if there is a gap
				//and we do not check further
				if (r2PMin > r1PMax || r2PMax < r1PMin) {

					collision = false;

					break;
				}
			}

			//Check if this rectangle is inside another rectangle
			if (collision == true) {

				bool inside = true;

				//4 corners of this rectangle
				foreach (Vector2 corner in AllCorners) {

					//if other rectangle not contain this corner
					if (!otherRect.ContainPoint2D (corner)) {

						inside = false;
						break;
					}
				}

				if (inside == true)
					return CollisionResult.Fit;

				return CollisionResult.Overlap;
			}

			//There is no collision between two rectangles;
			return CollisionResult.None;
		}

		protected override CollisionResult ContactWIthCircle (ConvexCircle otherCircle)
		{
			//We use circle collide rect to chcek
			//reduce duplicate code because code is all most the same only
			//rectangle inside circle need to be checked
			CollisionResult result = otherCircle.IntersectWithShape(this);

			//Check rectangle is inside circle when overlap
			if (result == CollisionResult.Overlap) {


				/**
				 * For each corner of recntangle
				 * we check if corner inside circle
				 * 
				 * Return fit if all corner insdie circle
				 **/
				Vector2[] corners = AllCorners;

				for (int i = 0; i < corners.Length; i++) {

					//projection axis from corner to circle center
					Vector2 p = otherCircle.Center - corners [i];

					//corner projection
					float cornerP = Vector2.Dot (p.normalized, corners [i]);

					//circle center projection
					float circleP = Vector2.Dot (p.normalized, otherCircle.Center);


					//if corner projection is outside of circle return overlap
					if ((circleP - otherCircle.Radius) > cornerP || (circleP + otherCircle.Radius) < cornerP) {

						return result;

					} 
				}

				return CollisionResult.Fit;
			}

			//If circle fit in this rectangle return overlap
			//as from rectangle poit of view rectangle overlap circle
			if (result == CollisionResult.Fit)
				return CollisionResult.Overlap;

			return result;
		}
		#endregion


		#region IConvexCollisioin
		/**
		 * Return true if rectangle contain point
		 **/

		public override bool ContainPoint2D(Vector2 point){

			if (point.x >= xMin && point.x <= xMax
				&& point.y >= yMin && point.y <= yMax)
				return true;

			return false;
		}

		#endregion

	}


	/**
	 * A convex of circle shape
	 * 
	 * Implement IConvexCircleCollision interface
	 **/
	public class ConvexCircle :ConvexShape{
		
		#region Properties
		Vector2 _center;

		public Vector2 Center{

			get{
				return _center;
			}

			set{

				_center = value;
			}
		}

		float _radius;

		public float Radius{

			get{
				return _radius;
			}

			set{

				_radius = value;
			}
		}
		#endregion

		#region Constructor
		public ConvexCircle(Vector2 center, float radius){

			_shapeId = ConvexShapeID.Circle;

			_center = center;
			_radius = radius;
		}
		#endregion

		#region Shape collision calculation
		protected override CollisionResult ContactWithRectangle (ConvexRect otherRect)
		{
			//All corners from rectangle
			Vector2[] corners = otherRect.AllCorners;

			bool collision = true;

			/**
			 * Find closest corner to circle center then make a line between that corner and circle center into projection axis
		 	* Projecting all corner to that axis then find min and max corner
			 * Check if circle is overlap with rectangle
			 * 
			 * This solve problem when circle contact each corner vertex
			 **/
			//find closest corner and compare distance
			int closestCornerIndex = 0;
			int currentCornerIndex = 1;
			while (currentCornerIndex < corners.Length) {

				if ((_center - corners [closestCornerIndex]).sqrMagnitude > (_center - corners [currentCornerIndex]).sqrMagnitude)
					closestCornerIndex = currentCornerIndex;

				currentCornerIndex++;
			}

			//projection axis from closest corner to circle center
			Vector2 p = _center - corners [closestCornerIndex];

			//base on axis find min and max corner
			float rMin = 0.0f;
			float rMax = 0.0f;
			for (int i = 0; i < corners.Length; i++) {

				rMax = Mathf.Max (rMax, Vector2.Dot (p.normalized, corners [i]));
				rMin = Mathf.Min (rMin, Vector2.Dot (p.normalized, corners [i]));
			}

			//find circle center projection
			float cP = Vector2.Dot (p.normalized, _center);

			//check if circle is overlap rectangle
			if (rMax < (cP - _radius) || rMin > (cP + _radius))
				collision = false;
			else
				collision = true;

			/**
			 * Find 4 corner's normal and make it as prjection axis
			 * Go throught each normal(corner)
			 * Find min and max projection of rectangle's corner base on that axis
			 * Project circle center on that axis
		 	* Check if circle overlap rectangle
		 	* 
		 	* This solve problem while circle contact edge of rectangle
		 	* 
		 	* We also check if circle inside rectangle
		 	**/
			//Ignore edge check if circle not contact with 4 corners(vertices)
			if (collision == true) {

				bool inside = true;

				Vector2[] normals = otherRect.Normals;
				float r1Dot1, r1Dot2, r1Dot3, r1Dot4, r1PMin, r1PMax;

				for (int i = 0; i < normals.Length; i++) {

					//4 corners projection
					r1Dot1 = Vector2.Dot (normals [i], corners [0]);
					r1Dot2 = Vector2.Dot (normals [i], corners [1]);
					r1Dot3 = Vector2.Dot (normals [i], corners [2]);
					r1Dot4 = Vector2.Dot (normals [i], corners [3]);

					//corner min and max on this normal(projection axis)
					r1PMin = Mathf.Min (r1Dot1, Mathf.Min (r1Dot2, Mathf.Min (r1Dot3, r1Dot4)));
					r1PMax = Mathf.Max (r1Dot1, Mathf.Max (r1Dot2, Mathf.Max (r1Dot3, r1Dot4)));

					//circle center projection on this normal(projection axis)
					float circleP = Vector2.Dot (normals [i], _center);

					//check if circle overlap rectangle
					if ((circleP - _radius) > r1PMax || (circleP + _radius) < r1PMin) {

						collision = false;
						break;
					}

					//Circle intersect with rectangle then check if circle inside rectangle
					//If insde we &(AND) value with true other wise false
					if ((circleP - _radius) > r1PMin && (circleP + _radius) > r1PMin) {

						inside &= true;
					} else {

						inside &= false;
					}
				}

				//if circle collide rectangle 
				if (collision) {

					//If inside rectangle
					if (inside)
						return CollisionResult.Fit;
					else
						return CollisionResult.Overlap;
				}
			}

			return CollisionResult.None;
		}

		protected override CollisionResult ContactWIthCircle (ConvexCircle otherCircle)
		{
			bool collision = true;

			Vector2 p = otherCircle.Center - _center;

			//This circle projection
			float circleProj = Vector2.Dot (p.normalized, _center);
			float circleProjMin = circleProj - _radius;
			float circleProjMax = circleProj + _radius;

			//Another circle projection
			float otherCircleProj = Vector2.Dot (p.normalized, otherCircle.Center);
			float otherCircleProjMin = otherCircleProj - otherCircle.Radius;
			float otherCircleProjMax = otherCircleProj + otherCircle.Radius;

			if (circleProjMin > otherCircleProjMax || otherCircleProjMin > circleProjMax)
				collision = false;

			//Check if circle fit inside another circle
			if (collision == true) {

				if (circleProjMin > otherCircleProjMin && circleProjMax < otherCircleProjMax)
					return CollisionResult.Fit;

				return CollisionResult.Overlap;
			}

			return CollisionResult.None;
		}
		#endregion

		#region IConvexCollisioin
		/**
		 * Return true if circle contain point
		 **/
		public override bool ContainPoint2D(Vector2 point){

			float xMin = _center.x - _radius;
			float xMax = _center.x + _radius;
			float yMin = _center.y - _radius;
			float yMax = _center.y + _radius;

			if (point.x >= xMin && point.x <= xMax
				&& point.y >= yMin && point.y <= yMax)
				return true;

			return false;
		}
		#endregion

	}


}



