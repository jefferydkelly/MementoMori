using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{

    /// <summary>
    /// Clamps the X Velocity of the given Rigidbody2D to the value given if it is greater.
    /// </summary>
    /// <param name="rb">The Rigidbody2D that will have its velocity checked.</param>
    /// <param name="maxX">The maximum X speed (regardless of direction) for the object.</param>
    public static void ClampXVelocity(this Rigidbody2D rb, float maxX)
    {
        if (Mathf.Abs(rb.velocity.x) > maxX)
        {
            rb.velocity = rb.velocity.SetX(Mathf.Sign(rb.velocity.x) * maxX);
        }
    }

    /// <summary>
    /// Clamps the Y Velocity of the given Rigidbody2D to the value given if it is greater.
    /// </summary>
    /// <param name="rb">The Rigidbody2D that will have its velocity checked.</param>
    /// <param name="maxY">The maximum Y speed (regardless of direction) for the object.</param>
    public static void ClampYVelocity(this Rigidbody2D rb, float maxY)
    {
        if (Mathf.Abs(rb.velocity.y) > maxY)
        {
            rb.velocity = rb.velocity.SetY(Mathf.Sign(rb.velocity.y) * maxY);
        }
    }

    /// <summary>
    /// Clamps the velocity to the max speed.
    /// </summary>
    /// <param name="rb">Rb.</param>
    /// <param name="maxSpeed">Max speed.</param>
    public static void ClampVelocity(this Rigidbody2D rb, float maxSpeed)
    {
        if (rb.velocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    /// <summary>
    /// Checks whether the Collision2d's gameobject is on the given layer
    /// </summary>
    /// <returns><c>true</c>, if the gameobject is on the given layer, <c>false</c> otherwise.</returns>
    /// <param name="col">The Collision2D to be checked.</param>
    /// <param name="layer">The string representing the layer name.</param>
    public static bool IsOnLayer(this Collision2D col, string layer)
    {
        return col.gameObject.IsOnLayer(layer);
    }

    /// <summary>
    /// Checks whether the gameobject is on the given layer
    /// </summary>
    /// <returns><c>true</c>, if the gameobject is on the given layer, <c>false</c> otherwise.</returns>
    /// <param name="go">The gameobject to be checked.</param>
    /// <param name="layer">The string representing the layer name.</param>
    public static bool IsOnLayer(this GameObject go, string layer)
    {
        return LayerMask.LayerToName(go.layer) == layer;
    }

    /// <summary>
    /// Returns a random element from the list.
    /// </summary>
    /// <returns>A random element from the list.</returns>
    /// <param name="list">The list.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
	public static T RandomElement<T>(this List<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }

    /// <summary>
    /// Determines if the float is between min and max.  (Exclusive)
    /// </summary>
    /// <returns><c>true</c>, if the value is between min and max, <c>false</c> otherwise.</returns>
    /// <param name="f">The value to be checked.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
	public static bool BetweenEx(this float f, float min, float max)
    {
        return f > min && f < max;
    }

    /// <summary>
    /// Determines if the float is between min and max.
    /// </summary>
    /// <returns><c>true</c>, if the value is between min and max, <c>false</c> otherwise.</returns>
    /// <param name="f">The value to be checked.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    public static bool Between(this float f, float min, float max)
    {
        return f >= min && f <= max;
    }

    /// <summary>
    /// Converts the angle from radians to degrees and returns it.
    /// </summary>
    /// <returns>The angle in radians.</returns>
    /// <param name="f">The angle in degrees.</param>
    public static float ToDegrees(this float f)
    {
        return f * Mathf.Rad2Deg;
    }

    /// <summary>
    /// Converts the angle from degrees to radians and returns it.
    /// </summary>
    /// <returns>The angle in degrees.</returns>
    /// <param name="f">The angle in degrees.</param>
    public static float ToRadians(this float f)
    {
        return f * Mathf.Deg2Rad;
    }

    /// <summary>
    /// Rounds to nearest multiple of the given value.
    /// </summary>
    /// <returns>The given float rounded to the nearet multiple of val.</returns>
    /// <param name="f">The float to be rounded.</param>
    /// <param name="val">The value whose multiple f will be rounded to.</param>
    public static float RoundToNearest(this float f, float val)
    {
        return Mathf.Round(f / val) * val;
    }

    /// <summary>
    /// Calculates a Vector2 from the sin and cos of the angle.
    /// </summary>
    /// <returns>A unit vector pointing in the direction of the angle.</returns>
    /// <param name="ang">The angle in radians.</param>
    public static Vector2 FromRadianToVector(this float ang)
    {
        return new Vector2(Mathf.Cos(ang), Mathf.Sin(ang));
    }

    /// <summary>
    /// Calculates a Vector2 from the sin and cos of the angle.
    /// </summary>
    /// <returns>A unit vector pointing in the direction of the angle.</returns>
    /// <param name="ang">The angle in degrees.</param>
    public static Vector2 FromDegreesToVector(this float deg)
    {
        return deg.ToRadians().FromRadianToVector();
    }

    /// <summary>
    /// Returns a new Vector3 with the given X value and YZ values that match the Vector3.
    /// </summary>
    /// <returns>The x.</returns>
    /// <param name="vec">Vec.</param>
    /// <param name="x">The x coordinate.</param>
    public static Vector3 SetX(this Vector3 vec, float x)
    {
        return new Vector3(x, vec.y, vec.z);
    }

    /// <summary>
    /// Sets the y.
    /// </summary>
    /// <returns>The y.</returns>
    /// <param name="vec">Vec.</param>
    /// <param name="y">The y coordinate.</param>
	public static Vector3 SetY(this Vector3 vec, float y)
    {
        return new Vector3(vec.x, y, vec.z);
    }

    /// <summary>
    /// Sets the z.
    /// </summary>
    /// <returns>The z.</returns>
    /// <param name="vec">Vec.</param>
    /// <param name="z">The z coordinate.</param>
	public static Vector3 SetZ(this Vector3 vec, float z)
    {
        return new Vector3(vec.x, vec.y, z);
    }

    /// <summary>
    /// Gets a vector composed of the X and Y components of the Vector3
    /// </summary>
    /// <param name="vec">This Vector 3</param>
    /// <returns>A vector2 composed of the X and Y components</returns>
    public static Vector2 GetXY(this Vector3 vec)
    {
        return new Vector2(vec.x, vec.y);
    }

    /// <summary>
    /// Gets a vector composed of the X and Z components of the Vector3
    /// </summary>
    /// <param name="vec">This Vector 3</param>
    /// <returns>A vector2 composed of the X and Z components</returns>
    public static Vector2 GetXZ(this Vector3 vec)
    {
        return new Vector2(vec.x, vec.z);
    }

    /// <summary>
    /// Gets a vector composed of the Y and Z components of the Vector3
    /// </summary>
    /// <param name="vec">This Vector 3</param>
    /// <returns>A vector2 composed of the Y and Z components</returns>
    public static Vector2 GetYZ(this Vector3 vec)
    {
        return new Vector2(vec.y, vec.z);
    }

    /// <summary>
    /// Sets the x.
    /// </summary>
    /// <returns>The x.</returns>
    /// <param name="vec">Vec.</param>
    /// <param name="x">The x coordinate.</param>
	public static Vector2 SetX(this Vector2 vec, float x)
    {
        return new Vector2(x, vec.y);
    }

    /// <summary>
    /// Sets the y.
    /// </summary>
    /// <returns>The y.</returns>
    /// <param name="vec">Vec.</param>
    /// <param name="y">The y coordinate.</param>
	public static Vector2 SetY(this Vector2 vec, float y)
    {
        return new Vector2(vec.x, y);
    }

    /// <summary>
    /// Calculates the angle of the vector
    /// </summary>
    /// <returns>The angle calculated from the x and y of the vector.</returns>
    /// <param name="vec">The vector.</param>
    public static float Angle(this Vector2 vec)
    {
        return Mathf.Atan2(vec.y, vec.x);
    }

    /// <summary>
    /// Returns a copy of the vector rotated by the angle in radians
    /// </summary>
    /// <returns>A copy of the vector rotated by the angle in radians.</returns>
    /// <param name="v">The vector.</param>
    /// <param name="ang">The angle in radians.</param>
    public static Vector2 Rotate(this Vector2 v, float ang)
    {
        float cos = Mathf.Cos(ang);
        float sin = Mathf.Sin(ang);
        return new Vector2(v.x * cos - v.y * sin, v.x * sin + v.y * cos);
    }

    /// <summary>
    /// Returns a copy of the vector rotated by the angle in degrees
    /// </summary>
    /// <returns>A copy of the vector rotated by the angle in degrees.</returns>
    /// <param name="v">The vector.</param>
    /// <param name="ang">The angle in degrees.</param>
    public static Vector2 RotateDeg(this Vector2 v, float ang)
    {
        return v.Rotate(ang.ToRadians());
    }

    /// <summary>
    /// Swaps the Vector's X and Y values
    /// </summary>
    /// <param name="v">This vector</param>
    /// <returns>A new vector with the X and Y values swapped.</returns>
    public static Vector2 Reverse(this Vector2 v)
    {
        return new Vector2(v.y, v.x);
    }

    /// <summary>
    /// Sets the red.
    /// </summary>
    /// <returns>The red.</returns>
    /// <param name="col">Col.</param>
    /// <param name="r">The red component.</param>
    public static Color SetRed(this Color col, float r)
    {
        return new Color(r, col.g, col.b, col.a);
    }

    /// <summary>
    /// Sets the green.
    /// </summary>
    /// <returns>The green.</returns>
    /// <param name="col">Col.</param>
    /// <param name="g">The green component.</param>
	public static Color SetGreen(this Color col, float g)
    {
        return new Color(col.r, g, col.b, col.a);
    }

    /// <summary>
    /// Sets the blue.
    /// </summary>
    /// <returns>The blue.</returns>
    /// <param name="col">Col.</param>
    /// <param name="b">The blue component.</param>
	public static Color SetBlue(this Color col, float b)
    {
        return new Color(col.r, col.g, b, col.a);
    }

    /// <summary>
    /// Sets the alpha.
    /// </summary>
    /// <returns>The alpha.</returns>
    /// <param name="col">Col.</param>
    /// <param name="a">The alpha component.</param>
	public static Color SetAlpha(this Color col, float a)
    {
        return new Color(col.r, col.g, col.b, a);
    }

    /// <summary>
    /// Gets the closest direction to the normal of this collision
    /// </summary>
    /// <param name="collision">The collision</param>
    /// <returns>The closest direction to the normal of this collision</returns>
    public static Directions GetClosestDirection(this Collision2D collision)
    {
        return DirectionHandler.GetClosestDirection(collision.GetContact(0).normal);
    }

    /// <summary>
    /// Splits the a camel case string into a string with spaces in it.
    /// </summary>
    /// <returns>The camel case.</returns>
    /// <param name="s">S.</param>
    public static string SplitCamelCase(this string s)
    {
        string cpy = s;
        string upper = s.ToUpper();
        int spacesAdded = 0;
        for (int i = 1; i < s.Length; i++)
        {
            if (s[i].Equals(upper[i]))
            {
                cpy = cpy.Insert(i + spacesAdded, " ");
                spacesAdded++;
            }
        }
        return cpy;
    }

    /// <summary>
    /// Gets whether or not the game object has the given component
    /// </summary>
    /// <returns><c>true</c>, if compnent was hased, <c>false</c> otherwise.</returns>
    /// <param name="go">Go.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    public static bool HasComponent<T>(this GameObject go)
    {
        T comp = go.GetComponent<T>();
        return (comp != null);
    }

    /// <summary>
    /// Gets whether or not the monobehaviour's game object has the given component
    /// </summary>
    /// <returns><c>true</c>, if compnent was hased, <c>false</c> otherwise.</returns>
    /// <param name="mo">Mo.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    public static bool HasCompnent<T>(this MonoBehaviour mo)
    {
        T comp = mo.GetComponent<T>();
        return (comp != null);
    }

    /// <summary>
    /// Gets the last child of the transform if it exists.  Otherwise, returns null.
    /// </summary>
    /// <param name="t">This transform</param>
    /// <returns>The last child of T.</returns>
    public static Transform GetLastChild(this Transform t)
    {
        if (t.childCount > 0)
        {
            return t.GetChild(t.childCount - 1);
        }
        return null;
    }
}

public enum Directions
{
    Up = 0,
    Left = 1,
    Down = 2,
    Right = 3
}

public class DirectionHandler
{
    /// <summary>
    /// Returns the direction opposite of the one passed in.
    /// </summary>
    /// <param name="dir">The first direction</param>
    /// <returns>The opposite of the first direction</returns>
    public static Directions GetOppositeDirection(Directions dir)
    {
        switch (dir)
        {
            case Directions.Down:
                return Directions.Up;
            case Directions.Up:
                return Directions.Down;
            case Directions.Left:
                return Directions.Right;
            case Directions.Right:
                return Directions.Left;
            default:
                return Directions.Up;
        }
    }

    /// <summary>
    /// Gets the opposite direction of the given direction as a Vector2
    /// </summary>
    /// <param name="dir">The first direction</param>
    /// <returns>The opposing vector of the first diection</returns>
    public static Vector2 GetOppositeDirectionVec2(Directions dir)
    {
        return ConvertDirectionToVector2(GetOppositeDirection(dir));
    }

    /// <summary>
    /// Gets the opposite direction of the given direction as a Vector3
    /// </summary>
    /// <param name="dir">The first direction</param>
    /// <returns>The opposing vector of the first diection</returns>
    public static Vector3 GetOppositeDirectionVec3(Directions dir)
    {
        return ConvertDirectionToVector3(GetOppositeDirection(dir));
    }

    /// <summary>
    /// Convets the direction to Vector2
    /// </summary>
    /// <param name="dir">The direction</param>
    /// <returns>The direction as a two-dimensional vector</returns>
    public static Vector2 ConvertDirectionToVector2(Directions dir)
    {
        switch (dir)
        {
            case Directions.Down:
                return Vector2.down;
            case Directions.Up:
                return Vector2.up;
            case Directions.Left:
                return Vector2.left;
            case Directions.Right:
                return Vector2.right;
        }

        return Vector2.zero;
    }

    /// <summary>
    /// Convets the direction to Vector2
    /// </summary>
    /// <param name="dir">The direction</param>
    /// <returns>The direction as a two-dimensional vector</returns>
    public static Vector2Int ConvertDirectionToVector2Int(Directions dir)
    {
        switch (dir)
        {
            case Directions.Down:
                return Vector2Int.down;
            case Directions.Up:
                return Vector2Int.up;
            case Directions.Left:
                return Vector2Int.left;
            case Directions.Right:
                return Vector2Int.right;
        }

        return Vector2Int.zero;
    }

    /// <summary>
    /// Convets the direction to Vector3
    /// </summary>
    /// <param name="dir">The direction</param>
    /// <returns>The direction as a three-dimensional vector</returns>
    public static Vector3 ConvertDirectionToVector3(Directions dir)
    {
        switch (dir)
        {
            case Directions.Down:
                return Vector3.down;
            case Directions.Up:
                return Vector3.up;
            case Directions.Left:
                return Vector3.left;
            case Directions.Right:
                return Vector3.right;
        }

        return Vector3.zero;
    }

    /// <summary>
    /// Gets the closest direction to the given vector
    /// </summary>
    /// <param name="vec">The vector</param>
    /// <returns>The direction closest to it.</returns>
    public static Directions GetClosestDirection(Vector3 vec)
    {
        int ind = -1;
        float maxDot = 0;
        Vector3[] directions = { Vector3.up, Vector3.left, Vector3.down, Vector3.right };
        for (int i = 0; i < 4; i++)
        {
            float dot = Vector3.Dot(vec, directions[i]);

            if (dot > maxDot)
            {
                ind = i;
                maxDot = dot;
            }
        }

        return (Directions)ind;
    }

    /// <summary>
    /// Gets the closest direction to the given vector
    /// </summary>
    /// <param name="vec">The vector</param>
    /// <returns>The direction closest to it.</returns>
    public static Directions GetClosestDirection(Vector2 vec)
    {
        int ind = -1;
        float maxDot = 0;
        Vector2[] directions = { Vector2.up, Vector2.left, Vector2.down, Vector2.right };
        for (int i = 0; i < 4; i++)
        {

            float dot = Vector2.Dot(vec, directions[i]);
            if (dot > maxDot)
            {
                ind = i;
                maxDot = dot;
            }
        }

        return (Directions)ind;
    }

    /// <summary>
    /// Gets the closest direction to the given vector
    /// </summary>
    /// <param name="vec">The vector</param>
    /// <returns>The direction closest to it.</returns>
    public static Directions GetClosestDirection(Vector2Int vec)
    {
        int ind = -1;
        float maxDot = 0;
        Vector2[] directions = { Vector2Int.up, Vector2Int.left, Vector2Int.down, Vector2Int.right };
        for (int i = 0; i < 4; i++)
        {

            float dot = Vector2.Dot(vec, directions[i]);
            if (dot > maxDot)
            {
                ind = i;
                maxDot = dot;
            }
        }

        return (Directions)ind;
    }

    /// <summary>
    /// Returns whether or not the direction is up or down
    /// </summary>
    /// <param name="dir">The direction</param>
    /// <returns>Returns true if the diections is up or down.  False, otherwise.</returns>
    public static bool IsVertical(Directions dir)
    {
        return dir == Directions.Up || dir == Directions.Down;
    }

    /// <summary>
    /// Returns whether or not the direction is left or right
    /// </summary>
    /// <param name="dir">The direction</param>
    /// <returns>Returns true if the diections is left or right.  False, otherwise.</returns>
    public static bool IsHorizontal(Directions dir)
    {
        return dir == Directions.Left || dir == Directions.Right;
    }
}