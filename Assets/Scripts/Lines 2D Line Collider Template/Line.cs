using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
 * 2D Polygon Line Collider Package
 *
 * @license		    Unity Asset Store EULA https://unity3d.com/legal/as_terms
 * @author		    Indie Studio - Baraa Nasser
 * @Website		    https://indiestd.com
 * @Asset Store     https://assetstore.unity.com/publishers/9268
 * @Unity Connect   https://connect.unity.com/u/5822191d090915001dbaf653/column
 * @email		    info@indiestd.com
 *
 */

[RequireComponent(typeof(EdgeCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(LineRenderer))]
[DisallowMultipleComponent]
public class Line : MonoBehaviour
{
	/// <summary>
	/// The Points list of the Line .
	/// </summary>
	[SerializeField]
	public List<Vector2> points;

	/// <summary>
	/// The line renderer reference.
	/// </summary>
	private LineRenderer lineRenderer;

	/// <summary>
	/// The collider of the line.
	/// </summary>
	private EdgeCollider2D edgeCollider2D;

	/// <summary>
	/// The rigid body of the line.
	/// </summary>
	private Rigidbody2D rigidBody2D;

	/// <summary>
	/// The material of the line renderer.
	/// </summary>
	public Material lineMaterial;

	/// <summary>
	/// The point Z position.
	/// </summary>
	public float pointZPosition = -3;

	/// <summary>
	/// Whether to set/assign points to the collider or not.
	/// </summary>
	public bool autoSetColliderPoints = true;

	/// <summary>
	/// The max points.
	/// </summary>
	[Range(0,5000)]
	public float maxPoints = Mathf.Infinity;

	/// <summary>
	/// An offset added between line and collider
	/// </summary>
	[Range(0, 0.5f)]
	public float colliderOffset;

    /// <summary>
    /// The minimum offset between points in the line.
    /// </summary>
    public float pointMinOffset = 0.1f;

    // Use this for initialization
    void Awake ()
	{
		points = new List<Vector2> ();
		lineRenderer = GetComponent<LineRenderer> ();
		edgeCollider2D = GetComponent<EdgeCollider2D> ();
		rigidBody2D = GetComponent<Rigidbody2D> ();

		if (lineMaterial == null) {
			//Create the material of the line
			lineMaterial = new Material (Shader.Find ("Sprites/Default"));
		}

		lineRenderer.material = lineMaterial;
		edgeCollider2D.edgeRadius = lineRenderer.startWidth / 2.0f + colliderOffset;
	}

	/// <summary>
	/// Adds new point.
	/// </summary>
	/// <param name="point">Vector3 Point.</param>
	public void AddPoint (Vector3 point)
	{
		//If the given point already exists ,then skip it
		if (points.Contains (point)) {
			return;
		}

		if (points.Count > 1) {
			if (Vector2.Distance (point, points [points.Count - 1]) < pointMinOffset) {
				return;//skip the point
			}
		}

		//z-position of the point
		point.z = pointZPosition;

		//Add the point to the points list
		points.Add (point);
		lineRenderer.positionCount++;
		lineRenderer.SetPosition (lineRenderer.positionCount - 1, point);

		if (autoSetColliderPoints) {
			edgeCollider2D.points = points.Select(x => new Vector2(x.x, x.y)).ToArray();
		}
	}

	/// <summary>
	/// Enable the collider of the line.
	/// </summary>
	public void EnableCollider ()
	{
		edgeCollider2D.enabled = true;
	}
	
	/// <summary>
	/// Disable the collider of the line.
	/// </summary>
	public void DisableCollider ()
	{
		edgeCollider2D.enabled = false;
	}

	/// <summary>
	/// Set the type of the rigid body.
	/// </summary>
	/// <param name="type">Type.</param>
	public void SetRigidBodyType(RigidbodyType2D type){
		rigidBody2D.bodyType = type;
	}

	/// <summary>
	/// Simulate the rigid body.
	/// </summary>
	public void SimulateRigidBody ()
	{
		rigidBody2D.simulated = true;
	}

	/// <summary>
	/// Whether reached points limit or not.
	/// </summary>
	/// <returns><c>true</c>, if limit was reacheded, <c>false</c> otherwise.</returns>
	public bool ReachedPointsLimit(){
		return points.Count >= maxPoints;
	}

	
}
