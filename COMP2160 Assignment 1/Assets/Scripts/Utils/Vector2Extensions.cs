using UnityEngine;
using System.Collections;

/**
 * Extension methods for the Vector2 class
 */

public static class Vector2Extensions  {

	/**
	 * Test if vector v1 is on the left of v2.
	 * 
	 * Example:
	 * 		Vector2 v1, v2;
	 * 		if (v1.IsOnLeft(v2))
	 *      {
     *			// v1 is on the left of v2
	 *      }
	 */

	public static bool IsOnLeft(this Vector2 v1, Vector2 v2) 
	{
		return v1.x * v2.y < v1.y * v2.x;
	}

	/**
	 * Rotate a 2D vector anticlockwise by the given angle (in degrees)
	 *
	 * Example:
	 * 		Vector2 v;
	 *      v.Rotate(45); // rotate 45 degrees anticlockwise
	 */

	public static Vector2 Rotate(this Vector2 v, float angle) 
	{
		Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
		return q * v;
	}

}
