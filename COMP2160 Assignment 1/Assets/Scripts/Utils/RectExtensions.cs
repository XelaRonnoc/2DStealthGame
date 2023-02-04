using UnityEngine;
using System.Collections;

/**
 * Extension methods for the Rect class
 */

public static class RectExtensions  {


	/**
	 * Get corners of the rect in clockwise order.
	 * Note: the index wraps around, so r.Corner(0) == r.Corner(4);
	 *
	 * Example:
	 *     Rect r;
	 *     for (int i = 0; i < 4; i++)
	 *     {
	 *	       Vector2 p0 = r.Corner(i);
	 *         Vector2 p1 = r.Corner(i+1);
	 *         Gizmos.DrawLine(p0, p1);
	 *     }
	 */

	public static Vector2 Corner(this Rect r, int i) 
	{
		Vector2 p = Vector2.zero;

		switch ((i % 4) + 4 % 4) {
		case 0:
			p = r.min;
			break;
		case 1:
			p = new Vector2(r.xMin, r.yMax);
			break;
		case 2:
			p = r.max;
			break;
		case 3:
			p = new Vector3(r.xMax, r.yMin);
			break;
		}


		return p;
	}

	/**
	 * Clamp a Vector2 to lie within the rect.
	 *
	 * Example:
	 *     Vector2 p;
	 *     Rect r;
	 *     p = r.Clamp(p);  // the nearest point inside the rectangle
	 */

	public static Vector2 Clamp(this Rect r, Vector2 v) 
	{
		v.x = Mathf.Clamp(v.x, r.xMin, r.xMax);
		v.y = Mathf.Clamp(v.y, r.yMin, r.yMax);
		return v;
	}


	/**
	 * Clamp a Vector3 to lie within the rect (ignoring z).
	 *
	 * Example:
	 *     Vector3 p;
	 *     Rect r;
	 *     p = r.Clamp(p);  // the nearest point inside the rectangle
	 */

	public static Vector3 Clamp(this Rect r, Vector3 v) 
	{
		v.x = Mathf.Clamp(v.x, r.xMin, r.xMax);
		v.y = Mathf.Clamp(v.y, r.yMin, r.yMax);
		return v;
	}


	/**
	 * Pick a point inside the rect
	 *
	 * Example:
	 *     Rect r;
	 *     Vector3 p = r.Point(0.5f, 0.5f);  // the middle of the rectangle
	 */

	public static Vector3 Point(this Rect r, float x, float y)
	{
		Vector3 v = Vector3.zero;
		v.x = Mathf.Lerp(r.xMin, r.xMax, x);
		v.y = Mathf.Lerp(r.yMin, r.yMax, y);

		return v;
	}


	/**
	 * Pick a random point inside the rect
	 * 
	 * Example:
	 *     Rect r;
	 *     Vector3 p = r.RandomPoint();  // a random point in the rectangle
	 */

	public static Vector3 RandomPoint(this Rect r)
	{		
		return r.Point(Random.value, Random.value);
	}

	/**
	 * Lerp between two Rects.
     *
	 * Example:
	 *     Rect r1, r2;
	 *     Rect r = RectExtensions.Lerp(r1, r2, 0.5f);
	 */

	public static Rect Lerp(Rect r1, Rect r2, float t) 
	{
		Rect r = new Rect();

		r.xMin = Mathf.Lerp(r1.xMin, r2.xMin, t);
		r.xMax = Mathf.Lerp(r1.xMax, r2.xMax, t);
		r.yMin = Mathf.Lerp(r1.yMin, r2.yMin, t);
		r.yMax = Mathf.Lerp(r1.yMax, r2.yMax, t);

		return r;
	}


}
