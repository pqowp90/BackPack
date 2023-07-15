using System;
using UnityEngine;

public class BezierCurve
{

    public static Vector3 HolyCurve(Vector3 a, Vector3 b, Vector3 c, Vector3 d, Vector3 e, Vector3 f, float t)
    {
        var p0 = SexyCurve(a, b, c, d, e, t);
        var p1 = SexyCurve(b, c, d, e, f, t);
        return Lerp(p0, p1, t);
    }
    public static Vector3 SexyCurve(Vector3 a, Vector3 b, Vector3 c, Vector3 d, Vector3 e, float t)
    {
        var p0 = CubicCurve(a, b, c, d, t);
        var p1 = CubicCurve(b, c, d, e, t);
        return Lerp(p0, p1, t);
    }
    public static Vector3 CubicCurve(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
    {
        var p0 = QuadraticCurve(a, b, c, t);
        var p1 = QuadraticCurve(b, c, d, t);
        return Lerp(p0, p1, t);
    }

    public static Vector3 QuadraticCurve(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        var p0 = Lerp(a, b, t);
        var p1 = Lerp(b, c, t);
        return Lerp(p0, p1, t);
    }

    private static Vector3 Lerp(Vector3 a, Vector3 b, float t)
    {
        return a + (b - a) * t;
    }
}