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
	 * Collision interface for rectangle shape
	 **/
	public interface IConvexRectCollision{

		CollisionResult CollideWithRect(ConvexRect otherRect);
	}

	/**
	 * Collision interface for circle shape
	 **/
	public interface IConvexCircleCollision{

		CollisionResult CollideWithCircle (ConvexCircle otherCircle);
		CollisionResult CollideWithRect (ConvexRect otherRect);
	}
}

namespace NP.Convex.Shape{

	/**
	 * A convex of rectangle shape
	 * 
	 * Implement IConvexRectCollision interface
	 **/
	public class ConvexRect : IConvexRectCollision{

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

			_x = x;
			_y = y;
			_width = Mathf.Abs(width);
			_height = Mathf.Abs(height);
		}

		public ConvexRect(Vector2 center, Vector2 size){

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

		public bool ContainPoint2D(Vector2 point){

			if (point.x >= xMin && point.x <= xMax
				&& point.y >= yMin && point.y <= yMax)
				return true;

			return false;
		}
		#endregion

		#region IConvexRectCollision
		public CollisionResult CollideWithRect(ConvexRect otherRect){

			//TODO implement collision
			return CollisionResult.None;
		}
		#endregion
	}


	/**
	 * A convex of circle shape
	 * 
	 * Implement IConvexCircleCollision interface
	 **/
	public class ConvexCircle : IConvexCircleCollision{
		
		#region Properties
		float _center;

		public float Center{

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
		public ConvexCircle(float center, float radius){

			_center = center;
			_radius = radius;
		}
		#endregion

		#region IConvexCircleCollision
		public CollisionResult CollideWithCircle (ConvexCircle otherCircle){

			//TODO implement collision
			return CollisionResult.None;
		}

		public CollisionResult CollideWithRect (ConvexRect otherRect){

			//TODO implement collision
			return CollisionResult.None;
		}
		#endregion
	}


}



