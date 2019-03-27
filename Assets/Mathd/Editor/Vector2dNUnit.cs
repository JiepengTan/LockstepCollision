/// Vector2dNUnit.cs
/// 
/// Repeat random values to test whether the output of the method in struct Vector2d and struct Vector2 is similar.
/// 
/// 重复随机取值，测试Vector2d与Vector2中方法的输出是否近似。
/// 
/// Created by D子宇 on 2018.3.17
/// 
/// Email: darkziyu@126.com
using System;
using Mathd;
using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class Vector2dNUnit {

    private const double deviation = 0.1;
    private const int count = 100;

    [Test]
    [Category("Vector2d")]
    public void Angle()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay;
            float bx, by;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);

            bx = UnityEngine.Random.Range(-10F, 10F);
            by = UnityEngine.Random.Range(-10F, 10F);

            Vector2 a = new Vector2(ax, ay);
            Vector2 b = new Vector2(bx, by);

            Vector2d ad = new Vector2d(ax, ay);
            Vector2d bd = new Vector2d(bx, by);

            float value = Vector2.Angle(a, b);
            float valued = Vector2d.Angle(ad, bd);

            if ((Mathf.Abs(value - valued) < deviation))
            {
                Assert.True(true);
            }
            else
            {
                Assert.Fail(string.Format("{0}\n{1}", value.ToString("0.00000"), valued.ToString("0.00000")));
            }
        }
    }

    [Test]
    [Category("Vector2d")]
    public void ClampMagnitude()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay;
            float p;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            p = UnityEngine.Random.Range(-20F, 20F);

            Vector2 a = new Vector2(ax, ay);

            Vector2d ad = new Vector2d(ax, ay);

            Vector2 value = Vector2.ClampMagnitude(a, p);
            Vector2d valued = Vector2d.ClampMagnitude(ad, p);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Vector2d")]
    public void Distance()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay;
            float bx, by;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);

            bx = UnityEngine.Random.Range(-10F, 10F);
            by = UnityEngine.Random.Range(-10F, 10F);

            Vector2 a = new Vector2(ax, ay);
            Vector2 b = new Vector2(bx, by);

            Vector2d ad = new Vector2d(ax, ay);
            Vector2d bd = new Vector2d(bx, by);

            float value = Vector2.Distance(a, b);
            double valued = Vector2d.Distance(ad, bd);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Vector2d")]
    public void Dot()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay;
            float bx, by;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);

            bx = UnityEngine.Random.Range(-10F, 10F);
            by = UnityEngine.Random.Range(-10F, 10F);

            Vector2 a = new Vector2(ax, ay);
            Vector2 b = new Vector2(bx, by);

            Vector2d ad = new Vector2d(ax, ay);
            Vector2d bd = new Vector2d(bx, by);

            float value = Vector2.Dot(a, b);
            double valued = Vector2d.Dot(ad, bd);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Vector2d")]
    public void Lerp()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay;
            float bx, by;
            float p = UnityEngine.Random.Range(-2F, 2F);

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);

            bx = UnityEngine.Random.Range(-10F, 10F);
            by = UnityEngine.Random.Range(-10F, 10F);

            Vector2 a = new Vector2(ax, ay);
            Vector2 b = new Vector2(bx, by);

            Vector2d ad = new Vector2d(ax, ay);
            Vector2d bd = new Vector2d(bx, by);

            Vector2 value = Vector2.Lerp(a, b, p);
            Vector2d valued = Vector2d.Lerp(ad, bd, p);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Vector2d")]
    public void LerpUnclamped()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay;
            float bx, by;
            float p = UnityEngine.Random.Range(-2F, 2F);

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);

            bx = UnityEngine.Random.Range(-10F, 10F);
            by = UnityEngine.Random.Range(-10F, 10F);

            Vector2 a = new Vector2(ax, ay);
            Vector2 b = new Vector2(bx, by);

            Vector2d ad = new Vector2d(ax, ay);
            Vector2d bd = new Vector2d(bx, by);

            Vector2 value = Vector2.LerpUnclamped(a, b, p);
            Vector2d valued = Vector2d.LerpUnclamped(ad, bd, p);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Vector2d")]
    public void Max()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay;
            float bx, by;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);

            bx = UnityEngine.Random.Range(-10F, 10F);
            by = UnityEngine.Random.Range(-10F, 10F);

            Vector2 a = new Vector2(ax, ay);
            Vector2 b = new Vector2(bx, by);

            Vector2d ad = new Vector2d(ax, ay);
            Vector2d bd = new Vector2d(bx, by);

            Vector2 value = Vector2.Max(a, b);
            Vector2d valued = Vector2d.Max(ad, bd);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Vector2d")]
    public void Min()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay;
            float bx, by;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);

            bx = UnityEngine.Random.Range(-10F, 10F);
            by = UnityEngine.Random.Range(-10F, 10F);

            Vector2 a = new Vector2(ax, ay);
            Vector2 b = new Vector2(bx, by);

            Vector2d ad = new Vector2d(ax, ay);
            Vector2d bd = new Vector2d(bx, by);

            Vector2 value = Vector2.Min(a, b);
            Vector2d valued = Vector2d.Min(ad, bd);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Vector2d")]
    public void MoveTowards()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay;
            float bx, by;
            float p = UnityEngine.Random.Range(-20F, 20F);

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);

            bx = UnityEngine.Random.Range(-10F, 10F);
            by = UnityEngine.Random.Range(-10F, 10F);

            Vector2 a = new Vector2(ax, ay);
            Vector2 b = new Vector2(bx, by);

            Vector2d ad = new Vector2d(ax, ay);
            Vector2d bd = new Vector2d(bx, by);

            Vector2 value = Vector2.MoveTowards(a, b, p);
            Vector2d valued = Vector2d.MoveTowards(ad, bd, p);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Vector2d")]
    public void Reflect()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay;
            float bx, by;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);

            bx = UnityEngine.Random.Range(-10F, 10F);
            by = UnityEngine.Random.Range(-10F, 10F);

            Vector2 a = new Vector2(ax, ay);
            Vector2 b = new Vector2(bx, by);

            Vector2d ad = new Vector2d(ax, ay);
            Vector2d bd = new Vector2d(bx, by);

            Vector2 value = Vector2.Reflect(a, b);
            Vector2d valued = Vector2d.Reflect(ad, bd);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Vector2d")]
    public void Scale1()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay;
            float bx, by;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);

            bx = UnityEngine.Random.Range(-10F, 10F);
            by = UnityEngine.Random.Range(-10F, 10F);

            Vector2 a = new Vector2(ax, ay);
            Vector2 b = new Vector2(bx, by);

            Vector2d ad = new Vector2d(ax, ay);
            Vector2d bd = new Vector2d(bx, by);

            Vector2 value = Vector2.Scale(a, b);
            Vector2d valued = Vector2d.Scale(ad, bd);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Vector2d")]
    public void SqrMagnitude1()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);

            Vector2 a = new Vector2(ax, ay);

            Vector2d ad = new Vector2d(ax, ay);

            float value = Vector2.SqrMagnitude(a);
            double valued = Vector2d.SqrMagnitude(ad);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Vector2d")]
    public void Scale2()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay;
            float bx, by;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);

            bx = UnityEngine.Random.Range(-10F, 10F);
            by = UnityEngine.Random.Range(-10F, 10F);

            Vector2 a = new Vector2(ax, ay);
            Vector2 b = new Vector2(bx, by);

            Vector2d ad = new Vector2d(ax, ay);
            Vector2d bd = new Vector2d(bx, by);

            a.Scale(b);
            ad.Scale(bd);

            Assert.True(Approximate(a, ad));
        }
    }

    [Test]
    [Category("Vector2d")]
    public void SmoothDamp()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay;
            float bx, by;
            float cx, cy;
            float p, q, w;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);

            bx = UnityEngine.Random.Range(-10F, 10F);
            by = UnityEngine.Random.Range(-10F, 10F);

            cx = UnityEngine.Random.Range(-10F, 10F);
            cy = UnityEngine.Random.Range(-10F, 10F);

            p = UnityEngine.Random.Range(-2F, 2F);
            q = UnityEngine.Random.Range(-2F, 2F);
            w = UnityEngine.Random.Range(-2F, 2F);

            Vector2 a = new Vector2(ax, ay);
            Vector2 b = new Vector2(bx, by);
            Vector2 c = new Vector2(cx, cy);

            Vector2d ad = new Vector2d(ax, ay);
            Vector2d bd = new Vector2d(bx, by);
            Vector2d cd = new Vector2(cx, cy);

            Vector2 value = Vector2.SmoothDamp(a, b, ref c, p, q, w);
            Vector2d valued = Vector2d.SmoothDamp(ad, bd, ref cd, p, q, w);

            Assert.True(Approximate(value, valued) && Approximate(c, cd));
        }
    }

    [Test]
    [Category("Vector2d")]
    public void SqrMagnitude2()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);

            Vector2 a = new Vector2(ax, ay);

            Vector2d ad = new Vector2d(ax, ay);

            float value = a.SqrMagnitude();
            double valued = ad.SqrMagnitude();

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Vector2d")]
    public void Magnitude()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);

            Vector2 a = new Vector2(ax, ay);

            Vector2d ad = new Vector2d(ax, ay);

            float value = a.magnitude;
            double valued = ad.magnitude;

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Vector2d")]
    public void Normalized()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);

            Vector2 a = new Vector2(ax, ay);

            Vector2d ad = new Vector2d(ax, ay);

            Vector2 value = a.normalized;
            Vector2d valued = ad.normalized;

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Vector2d")]
    public void Normalize()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);

            Vector2 a = new Vector2(ax, ay);

            Vector2d ad = new Vector2d(ax, ay);

            a.Normalize();
            ad.Normalize();

            Assert.True(Approximate(a, ad));
        }
    }
    
    private bool Approximate(Vector2 v, Vector2d vd)
    {
        if (Math.Abs(v.x - vd.x) < deviation &&
            Math.Abs(v.y - vd.y) < deviation)
        {
            return true;
        }
        else
        {
            Assert.Fail(string.Format("{0}\n{1}", v.ToString("0.00000"), vd.ToString("0.00000")));
            return false;
        }
    }

    private bool Approximate(float v, double vd)
    {
        if (Math.Abs(v - vd) < deviation)
        {
            return true;
        }
        else
        {
            Assert.Fail(string.Format("{0}\n{1}", v.ToString("0.00000"), vd.ToString("0.00000")));
            return false;
        }
    }
}
