/// Vector4dNUnit.cs
/// 
/// Repeat random values to test whether the output of the method in struct Vector4d and struct Vector4 is similar.
/// 
/// 重复随机取值，测试Vector4d与Vector4中方法的输出是否近似。
/// 
/// Created by D子宇 on 2018.3.17
/// 
/// Email: darkziyu@126.com
using System;
using Mathd;
using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class Vector4dNUnit {

    private const double deviation = 0.1;
    private const int count = 100;

    [Test]
    [Category("Vector4d")]
    public void Distance()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az, aw;
            float bx, by, bz, bw;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);
            aw = UnityEngine.Random.Range(-10F, 10F);

            bx = UnityEngine.Random.Range(-10F, 10F);
            by = UnityEngine.Random.Range(-10F, 10F);
            bz = UnityEngine.Random.Range(-10F, 10F);
            bw = UnityEngine.Random.Range(-10F, 10F);

            Vector4 a = new Vector4(ax, ay, az, aw);
            Vector4 b = new Vector4(bx, by, bz, bw);

            Vector4d ad = new Vector4d(ax, ay, az, aw);
            Vector4d bd = new Vector4d(bx, by, bz, bw);

            float value = Vector4.Distance(a, b);
            double valued = Vector4d.Distance(ad, bd);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Vector4d")]
    public void Dot()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az, aw;
            float bx, by, bz, bw;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);
            aw = UnityEngine.Random.Range(-10F, 10F);

            bx = UnityEngine.Random.Range(-10F, 10F);
            by = UnityEngine.Random.Range(-10F, 10F);
            bz = UnityEngine.Random.Range(-10F, 10F);
            bw = UnityEngine.Random.Range(-10F, 10F);

            Vector4 a = new Vector4(ax, ay, az, aw);
            Vector4 b = new Vector4(bx, by, bz, bw);

            Vector4d ad = new Vector4d(ax, ay, az, aw);
            Vector4d bd = new Vector4d(bx, by, bz, bw);

            float value = Vector4.Dot(a, b);
            double valued = Vector4d.Dot(ad, bd);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Vector4d")]
    public void Lerp()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az, aw;
            float bx, by, bz, bw;
            float p = UnityEngine.Random.Range(-2F, 2F);

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);
            aw = UnityEngine.Random.Range(-10F, 10F);

            bx = UnityEngine.Random.Range(-10F, 10F);
            by = UnityEngine.Random.Range(-10F, 10F);
            bz = UnityEngine.Random.Range(-10F, 10F);
            bw = UnityEngine.Random.Range(-10F, 10F);

            Vector4 a = new Vector4(ax, ay, az, aw);
            Vector4 b = new Vector4(bx, by, bz, bw);

            Vector4d ad = new Vector4d(ax, ay, az, aw);
            Vector4d bd = new Vector4d(bx, by, bz, bw);

            Vector4 value = Vector4.Lerp(a, b, p);
            Vector4d valued = Vector4d.Lerp(ad, bd, p);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Vector4d")]
    public void LerpUnclamped()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az, aw;
            float bx, by, bz, bw;
            float p = UnityEngine.Random.Range(-2F, 2F);

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);
            aw = UnityEngine.Random.Range(-10F, 10F);

            bx = UnityEngine.Random.Range(-10F, 10F);
            by = UnityEngine.Random.Range(-10F, 10F);
            bz = UnityEngine.Random.Range(-10F, 10F);
            bw = UnityEngine.Random.Range(-10F, 10F);

            Vector4 a = new Vector4(ax, ay, az, aw);
            Vector4 b = new Vector4(bx, by, bz, bw);

            Vector4d ad = new Vector4d(ax, ay, az, aw);
            Vector4d bd = new Vector4d(bx, by, bz, bw);

            Vector4 value = Vector4.LerpUnclamped(a, b, p);
            Vector4d valued = Vector4d.LerpUnclamped(ad, bd, p);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Vector4d")]
    public void Max()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az, aw;
            float bx, by, bz, bw;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);
            aw = UnityEngine.Random.Range(-10F, 10F);

            bx = UnityEngine.Random.Range(-10F, 10F);
            by = UnityEngine.Random.Range(-10F, 10F);
            bz = UnityEngine.Random.Range(-10F, 10F);
            bw = UnityEngine.Random.Range(-10F, 10F);

            Vector4 a = new Vector4(ax, ay, az, aw);
            Vector4 b = new Vector4(bx, by, bz, bw);

            Vector4d ad = new Vector4d(ax, ay, az, aw);
            Vector4d bd = new Vector4d(bx, by, bz, bw);

            Vector4 value = Vector4.Max(a, b);
            Vector4d valued = Vector4d.Max(ad, bd);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Vector4d")]
    public void Min()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az, aw;
            float bx, by, bz, bw;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);
            aw = UnityEngine.Random.Range(-10F, 10F);

            bx = UnityEngine.Random.Range(-10F, 10F);
            by = UnityEngine.Random.Range(-10F, 10F);
            bz = UnityEngine.Random.Range(-10F, 10F);
            bw = UnityEngine.Random.Range(-10F, 10F);

            Vector4 a = new Vector4(ax, ay, az, aw);
            Vector4 b = new Vector4(bx, by, bz, bw);

            Vector4d ad = new Vector4d(ax, ay, az, aw);
            Vector4d bd = new Vector4d(bx, by, bz, bw);

            Vector4 value = Vector4.Min(a, b);
            Vector4d valued = Vector4d.Min(ad, bd);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Vector4d")]
    public void Magnitude1()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az, aw;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);
            aw = UnityEngine.Random.Range(-10F, 10F);

            Vector4 a = new Vector4(ax, ay, az, aw);

            Vector4d ad = new Vector4d(ax, ay, az, aw);

            float value = Vector4.Magnitude(a);
            double valued = Vector4d.Magnitude(ad);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Vector4d")]
    public void MoveTowards()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az, aw;
            float bx, by, bz, bw;
            float p = UnityEngine.Random.Range(-20F, 20F);

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);
            aw = UnityEngine.Random.Range(-10F, 10F);

            bx = UnityEngine.Random.Range(-10F, 10F);
            by = UnityEngine.Random.Range(-10F, 10F);
            bz = UnityEngine.Random.Range(-10F, 10F);
            bw = UnityEngine.Random.Range(-10F, 10F);

            Vector4 a = new Vector4(ax, ay, az, aw);
            Vector4 b = new Vector4(bx, by, bz, bw);

            Vector4d ad = new Vector4d(ax, ay, az, aw);
            Vector4d bd = new Vector4d(bx, by, bz, bw);

            Vector4 value = Vector4.MoveTowards(a, b, p);
            Vector4d valued = Vector4d.MoveTowards(ad, bd, p);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Vector4d")]
    public void Normalize1()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az, aw;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);
            aw = UnityEngine.Random.Range(-10F, 10F);

            Vector4 a = new Vector4(ax, ay, az, aw);

            Vector4d ad = new Vector4d(ax, ay, az, aw);

            Vector4 value = Vector4.Normalize(a);
            Vector4d valued = Vector4d.Normalize(ad);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Vector4d")]
    public void Project()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az, aw;
            float bx, by, bz, bw;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);
            aw = UnityEngine.Random.Range(-10F, 10F);

            bx = UnityEngine.Random.Range(-10F, 10F);
            by = UnityEngine.Random.Range(-10F, 10F);
            bz = UnityEngine.Random.Range(-10F, 10F);
            bw = UnityEngine.Random.Range(-10F, 10F);

            Vector4 a = new Vector4(ax, ay, az, aw);
            Vector4 b = new Vector4(bx, by, bz, bw);

            Vector4d ad = new Vector4d(ax, ay, az, aw);
            Vector4d bd = new Vector4d(bx, by, bz, bw);

            Vector4 value = Vector4.Project(a, b);
            Vector4d valued = Vector4d.Project(ad, bd);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Vector4d")]
    public void Scale1()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az, aw;
            float bx, by, bz, bw;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);
            aw = UnityEngine.Random.Range(-10F, 10F);

            bx = UnityEngine.Random.Range(-10F, 10F);
            by = UnityEngine.Random.Range(-10F, 10F);
            bz = UnityEngine.Random.Range(-10F, 10F);
            bw = UnityEngine.Random.Range(-10F, 10F);

            Vector4 a = new Vector4(ax, ay, az, aw);
            Vector4 b = new Vector4(bx, by, bz, bw);

            Vector4d ad = new Vector4d(ax, ay, az, aw);
            Vector4d bd = new Vector4d(bx, by, bz, bw);

            Vector4 value = Vector4.Scale(a, b);
            Vector4d valued = Vector4d.Scale(ad, bd);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Vector4d")]
    public void SqrMagnitude1()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az, aw;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);
            aw = UnityEngine.Random.Range(-10F, 10F);

            Vector4 a = new Vector4(ax, ay, az, aw);

            Vector4d ad = new Vector4d(ax, ay, az, aw);

            float value = Vector4.SqrMagnitude(a);
            double valued = Vector4d.SqrMagnitude(ad);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Vector4d")]
    public void Normalize2()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az, aw;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);
            aw = UnityEngine.Random.Range(-10F, 10F);

            Vector4 a = new Vector4(ax, ay, az, aw);

            Vector4d ad = new Vector4d(ax, ay, az, aw);

            a.Normalize();
            ad.Normalize();

            Assert.True(Approximate(a, ad));
        }
    }

    [Test]
    [Category("Vector4d")]
    public void Scale2()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az, aw;
            float bx, by, bz, bw;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);
            aw = UnityEngine.Random.Range(-10F, 10F);

            bx = UnityEngine.Random.Range(-10F, 10F);
            by = UnityEngine.Random.Range(-10F, 10F);
            bz = UnityEngine.Random.Range(-10F, 10F);
            bw = UnityEngine.Random.Range(-10F, 10F);

            Vector4 a = new Vector4(ax, ay, az, aw);
            Vector4 b = new Vector4(bx, by, bz, bw);

            Vector4d ad = new Vector4d(ax, ay, az, aw);
            Vector4d bd = new Vector4d(bx, by, bz, bw);

            a.Scale(b);
            ad.Scale(bd);

            Assert.True(Approximate(a, ad));
        }
    }

    [Test]
    [Category("Vector4d")]
    public void SqrMagnitude2()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az, aw;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);
            aw = UnityEngine.Random.Range(-10F, 10F);

            Vector4 a = new Vector4(ax, ay, az, aw);

            Vector4d ad = new Vector4d(ax, ay, az, aw);

            float value = a.SqrMagnitude();
            double valued = ad.SqrMagnitude();

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Vector4d")]
    public void Magnitude2()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az, aw;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);
            aw = UnityEngine.Random.Range(-10F, 10F);

            Vector4 a = new Vector4(ax, ay, az, aw);

            Vector4d ad = new Vector4d(ax, ay, az, aw);

            float value = a.magnitude;
            double valued = ad.magnitude;

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Vector4d")]
    public void Normalized()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az, aw;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);
            aw = UnityEngine.Random.Range(-10F, 10F);

            Vector4 a = new Vector4(ax, ay, az, aw);

            Vector4d ad = new Vector4d(ax, ay, az, aw);

            Vector4 value = a.normalized;
            Vector4d valued = ad.normalized;

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Vector4d")]
    public void SqrMagnitude()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az, aw;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);
            aw = UnityEngine.Random.Range(-10F, 10F);

            Vector4 a = new Vector4(ax, ay, az, aw);

            Vector4d ad = new Vector4d(ax, ay, az, aw);

            float value = a.sqrMagnitude;
            double valued = ad.sqrMagnitude;

            Assert.True(Approximate(value, valued));
        }
    }
    
    private bool Approximate(Vector4 v, Vector4d vd)
    {
        if (Math.Abs(v.x - vd.x) < deviation &&
            Math.Abs(v.y - vd.y) < deviation &&
            Math.Abs(v.z - vd.z) < deviation &&
            Math.Abs(v.w - vd.w) < deviation)
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
