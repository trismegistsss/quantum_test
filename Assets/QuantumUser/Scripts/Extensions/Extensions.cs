using System.Collections.Generic;
using System;
using UnityEngine;
using DG.Tweening;
using AGS.Geom;

public static class Extensions 
{
    //point to vector and inverse transformers
    public static Point ToPoint(this Vector2 v2)
    {
        return new Point(Mathf.RoundToInt(v2.x), Mathf.RoundToInt(v2.y));
    }

    public static Point ToPoint(this Vector3 v3)
    {
        return new Point(Mathf.RoundToInt(v3.x), Mathf.RoundToInt(v3.y));
    }

    public static Vector2 ToVector(this Point pt)
    {
        return new Vector2(pt.x, pt.y);
    }

    public static Vector3 ToVector3(this Vector2 v2)
    {
        return v2;
    }

    public static Vector3 ToVector3(this Point pt)
    {
        return new Vector3(pt.x, pt.y);
    }
    //
    
    //vector2 extensions
    public static float Angle(this Vector2 v2)
    {
        return Mathf.Atan2(v2.y, v2.x) * Mathf.Rad2Deg;
    }

    public static float NormalizedAngle(this Vector2 v2)
    {
        return NormalizeAngle(v2.Angle());
    }

    public static int NormalizedAngle(this Point p)
    {
        return NormalizeAngle(p.Angle());
    }
    //

    //collections utility
    public static void AddRange<T>(this HashSet<T> set, IEnumerable<T> range)
    {
        foreach (var e in range)
            set.Add(e);
    }

    public static void RemoveAll<K, V>(this Dictionary<K, V> dict, Predicate< KeyValuePair<K, V> > match)
    {
        var keys = new List<K>(dict.Keys);

        foreach(var key in keys)
        {
            if (match(new KeyValuePair<K, V>(key, dict[key])))
                dict.Remove(key);
        }
    }
    //

    //spriterender FadeIn, FadeOut, FadeTo shortcuts
    public static Tween FadeTo(this SpriteRenderer renderer, float from, float to, float t)
    {
        var color = renderer.material.color;

        renderer.material.color = new Color(color.r, color.g, color.b, from);

        return renderer.material.DOFade(to, t);
    }

    public static Tween FadeTo(this SpriteRenderer renderer, float to, float t)
    {
        return renderer.material.DOFade(to, t);
    }
    
    public static Tween FadeIn(this SpriteRenderer renderer, float t)
    {
        return FadeTo(renderer, 1.0f, t);
    }

    public static Tween FadeOut(this SpriteRenderer renderer, float t)
    {
        return FadeTo(renderer, 0f, t);
    }

    // utils
    public static float NormalizeAngle(float a)
    {
        if (a >= 360)
        {
            a -= 360;
        }

        if (a < 0)
        {
            a += 360;
        }

        return a;
    }

    public static int NormalizeAngle(int a)
    {
        return (int)NormalizeAngle((float)a);
    }
}
