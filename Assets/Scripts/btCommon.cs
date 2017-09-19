using UnityEngine;
using System.Collections;

/// <summary>
/// Storage and access for basic functions
/// </summary>
public class btCommon
{
	// The angle between dirA and dirB around axis
    public static float AngleAroundAxis( Vector3 _dirA, Vector3 _dirB, Vector3 _axis )
    {
        // Project A and B onto the plane orthogonal target axis
        _dirA = _dirA - Vector3.Project( _dirA, _axis );
        _dirB = _dirB - Vector3.Project( _dirB, _axis );

        // Find (positive) angle between A and B
        float angle = Vector3.Angle( _dirA, _dirB );

        // Return angle multiplied with 1 or -1
        return angle * (Vector3.Dot(_axis, Vector3.Cross(_dirA, _dirB)) < 0 ? -1 : 1);
    }
}
