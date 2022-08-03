using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CanvasParticleCollider : MonoBehaviour {

//	#define WorldNormalVector(data,normal) fixed3(dot(data.TtoW0, dot(data.TtoW1,normal), dot(data.TtoW2,normal))

	public enum ColliderType{
		Box,
		Oval
	}

	[RangeAttribute(0, 2)]
	public float Bounciness;

	[RangeAttribute(0,1)]
	public float Friction;

	//public bool isColliding = false;
	public Rect ColliderBoxRect;
	public Vector2 ColliderOvalRadius;

	public RectTransform ColliderTransform;


	// Use this for initialization
	void Start () {
		if (ColliderTransform != null){
			ColliderBoxRect = ColliderTransform.rect;
		//var bounds = RectTransformUtility.CalculateRelativeRectTransformBounds(transform as RectTransform);
//		Bounds colliderBounds = RectTransformUtility.CalculateRelativeRectTransformBounds(transform as RectTransform);

			Bounds colliderBounds = RectTransformUtility.CalculateRelativeRectTransformBounds(transform.root, transform);
			ColliderBoxRect = new Rect(colliderBounds.min.x, colliderBounds.max.y, colliderBounds.size.x, colliderBounds.size.y);
		}

	

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool DetectColliding(CanvasParticle particle){
		bool isColliding = false;

//		if (ColliderTransform != null){
////			for (int i = 0; i < particle.ParticleVertices.Length; i++){
////				Vector2 BoundPoint = particle.ParticleVertices[i];
////				if (ColliderBoxRect.Contains(BoundPoint)){
////				    isColliding = true;
////					Debug.Log("Particle point: " + particle.ParticleVertices[i].ToString());
////					Debug.Log("Collider rect: " + ColliderBoxRect.ToString());
////				}
////			}
//				if (ColliderBoxRect.Contains(particle.ParticlePosition)){
//					isColliding = true;
//					Debug.Log("Particle point: " + particle.ParticlePosition.ToString());
//					Debug.Log("Collider rect: " + ColliderBoxRect.ToString());
//				}
//		}


//		Vector2 p0 = new Vector2();
//		Vector2 p1 = new Vector2();
//		float IntersectionDist = float.MinValue;
//		float PositiveDist = float.MaxValue;
//
//		for (int i = 0; i < particle.ParticleVertices.Length; i++){
//			// world space normal
//			//		#define WorldNormalVector(data,normal) fixed3(dot(data.TtoW0, dot(data.TtoW1,normal), dot(data.TtoW2,normal))
//
//
//
//		}


			/*
		// face of A, vertices of B
		for ( var i:int = 0; i<A.m_numPoints; i++ )
		{
			// get world space normal
			var wsN:Vector2 = A.GetWorldSpaceNormal( i );
			
			// world space edge
			var wsV0:Vector2 = A.GetWorldSpacePoint( i );
			var wsV1:Vector2 = A.GetWorldSpacePoint( ( i+1 )%A.m_numPoints );
			
			// get supporting vertices of B, most opposite face normal
			var s:Vector.<SupportVertex> = B.GetSupportVertices(wsN.m_Neg);
			
			for (var j:int=0; j<s.length; j++)
			{
				// form point on plane of minkowski face
				var mfp0:Vector2 = s[j].m_v.Sub(wsV0);
				var mfp1:Vector2 = s[j].m_v.Sub(wsV1);
				
				var faceDist:Number = mfp0.Dot( wsN );
				
				// project onto minkowski edge
				var p:Vector2 = ProjectPointOntoEdge( new Vector2( ), mfp0, mfp1 );
				
				// get distance
				var dist:Number = p.m_Len*Scalar.Sign(faceDist);
				
				// track negative
				if ( dist>leastPenetratingDist )
				{
					p0 = p;
					leastPenetratingDist = dist;
				}
				
				// track positive
				if ( dist > 0 && dist<leastPositiveDist )
				{
					p1 = p;
					leastPositiveDist = dist;
				}
			}
		}
		
		// face of B, vertices of A
		for ( i = 0; i<B.m_numPoints; i++ )
		{
			// get world space normal
			wsN = B.GetWorldSpaceNormal( i );
			
			// world space edge
			wsV0 = B.GetWorldSpacePoint( i );
			wsV1 = B.GetWorldSpacePoint( ( i+1 )%B.m_numPoints );
			
			// get supporting vertices of A, most opposite face normal
			s = A.GetSupportVertices(wsN.m_Neg);
			
			for (j=0; j<s.length; j++)
			{
				// form point on plane of minkowski face
				mfp0 = wsV0.Sub(s[j].m_v);
				mfp1 = wsV1.Sub(s[j].m_v);
				
				faceDist = -mfp0.Dot( wsN );
				
				// project onto minkowski edge
				p = ProjectPointOntoEdge( new Vector2( ), mfp0, mfp1 );
				
				// get distance
				dist = p.m_Len*Scalar.Sign(faceDist);
				
				// track negative
				if ( dist>leastPenetratingDist )
				{
					p0 = p;
					leastPenetratingDist = dist;
				}
				
				// track positive
				if ( dist > 0 && dist<leastPositiveDist )
				{
					p1 = p;
					leastPositiveDist = dist;
				}
			}
		}
		
		if ( leastPenetratingDist<0 )
		{
			// penetration by leastPenetratingDist
		}
		else 
		{
			// separated by leastPositiveDist
		}
		*/
				    
				return isColliding;
	}
}
