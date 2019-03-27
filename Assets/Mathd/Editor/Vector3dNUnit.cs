/// Vector3dNUnit.cs
/// 
/// Repeat random values to test whether the output of the method in struct Vector3d and struct Vector3 is similar.
/// 
/// 重复随机取值，测试Vector3d与Vector3中方法的输出是否近似。
/// 
/// Created by D子宇 on 2018.3.17
/// 
/// Email: darkziyu@126.com
using System;
using Mathd;
using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class Vector3dNUnit {

    private const double deviation = 0.1;
    private const int count = 100;

    [Test]
    [Category("Vector3d")]
    public void Angle() {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az;
            float bx, by, bz;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);

            bx = UnityEngine.Random.Range(-10F, 10F);
            by = UnityEngine.Random.Range(-10F, 10F);
            bz = UnityEngine.Random.Range(-10F, 10F);

            Vector3 a = new Vector3(ax, ay, az);
            Vector3 b = new Vector3(bx, by, bz);

            Vector3d ad = new Vector3d(ax, ay, az);
            Vector3d bd = new Vector3d(bx, by, bz);

            float value = Vector3.Angle(a, b);
            float valued = Vector3d.Angle(ad, bd);
            
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
    [Category("Vector3d")]
    public void AngleBetween()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az;
            float bx, by, bz;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);

            bx = UnityEngine.Random.Range(-10F, 10F);
            by = UnityEngine.Random.Range(-10F, 10F);
            bz = UnityEngine.Random.Range(-10F, 10F);

            Vector3 a = new Vector3(ax, ay, az);
            Vector3 b = new Vector3(bx, by, bz);

            Vector3d ad = new Vector3d(ax, ay, az);
            Vector3d bd = new Vector3d(bx, by, bz);

            float value = Vector3.AngleBetween(a, b);
            float valued = Vector3d.AngleBetween(ad, bd);
            
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
    [Category("Vector3d")]
    public void ClampMagnitude()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az;
            float p;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);
            p = UnityEngine.Random.Range(-20F, 20F);

            Vector3 a = new Vector3(ax, ay, az);
            Vector3d ad = new Vector3d(ax, ay, az);

            Vector3 value = Vector3.ClampMagnitude(a, p);
            Vector3d valued = Vector3d.ClampMagnitude(ad, p);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Vector3d")]
    public void Cross()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az;
            float bx, by, bz;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);

            bx = UnityEngine.Random.Range(-10F, 10F);
            by = UnityEngine.Random.Range(-10F, 10F);
            bz = UnityEngine.Random.Range(-10F, 10F);

            Vector3 a = new Vector3(ax, ay, az);
            Vector3 b = new Vector3(bx, by, bz);

            Vector3d ad = new Vector3d(ax, ay, az);
            Vector3d bd = new Vector3d(bx, by, bz);

            Vector3 value = Vector3.Cross(a, b);
            Vector3d valued = Vector3d.Cross(ad, bd);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Vector3d")]
    public void Distance()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az;
            float bx, by, bz;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);

            bx = UnityEngine.Random.Range(-10F, 10F);
            by = UnityEngine.Random.Range(-10F, 10F);
            bz = UnityEngine.Random.Range(-10F, 10F);

            Vector3 a = new Vector3(ax, ay, az);
            Vector3 b = new Vector3(bx, by, bz);

            Vector3d ad = new Vector3d(ax, ay, az);
            Vector3d bd = new Vector3d(bx, by, bz);

            float value = Vector3.Distance(a, b);
            double valued = Vector3d.Distance(ad, bd);

            Assert.True(Approximate(value, valued));
        }
    }
    
    [Test]
    [Category("Vector3d")]
    public void Dot()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az;
            float bx, by, bz;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);

            bx = UnityEngine.Random.Range(-10F, 10F);
            by = UnityEngine.Random.Range(-10F, 10F);
            bz = UnityEngine.Random.Range(-10F, 10F);

            Vector3 a = new Vector3(ax, ay, az);
            Vector3 b = new Vector3(bx, by, bz);

            Vector3d ad = new Vector3d(ax, ay, az);
            Vector3d bd = new Vector3d(bx, by, bz);

            float value = Vector3.Dot(a, b);
            double valued = Vector3d.Dot(ad, bd);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Vector3d")]
    public void Exclude()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az;
            float bx, by, bz;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);

            bx = UnityEngine.Random.Range(-10F, 10F);
            by = UnityEngine.Random.Range(-10F, 10F);
            bz = UnityEngine.Random.Range(-10F, 10F);

            Vector3 a = new Vector3(ax, ay, az);
            Vector3 b = new Vector3(bx, by, bz);

            Vector3d ad = new Vector3d(ax, ay, az);
            Vector3d bd = new Vector3d(bx, by, bz);

            Vector3 value = Vector3.Exclude(a, b);
            Vector3d valued = Vector3d.Exclude(ad, bd);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Vector3d")]
    public void Lerp()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az;
            float bx, by, bz;
            float p;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);

            bx = UnityEngine.Random.Range(-10F, 10F);
            by = UnityEngine.Random.Range(-10F, 10F);
            bz = UnityEngine.Random.Range(-10F, 10F);

            p = UnityEngine.Random.Range(-1F, 2F);

            Vector3 a = new Vector3(ax, ay, az);
            Vector3 b = new Vector3(bx, by, bz);

            Vector3d ad = new Vector3d(ax, ay, az);
            Vector3d bd = new Vector3d(bx, by, bz);

            Vector3 value = Vector3.Lerp(a, b, p);
            Vector3d valued = Vector3d.Lerp(ad, bd, p);

            Assert.True(Approximate(value, valued));
        }
    }
    
    [Test]
    [Category("Vector3d")]
    public void LerpUnclamped()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az;
            float bx, by, bz;
            float p;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);

            bx = UnityEngine.Random.Range(-10F, 10F);
            by = UnityEngine.Random.Range(-10F, 10F);
            bz = UnityEngine.Random.Range(-10F, 10F);

            p = UnityEngine.Random.Range(-1F, 2F);

            Vector3 a = new Vector3(ax, ay, az);
            Vector3 b = new Vector3(bx, by, bz);

            Vector3d ad = new Vector3d(ax, ay, az);
            Vector3d bd = new Vector3d(bx, by, bz);

            Vector3 value = Vector3.LerpUnclamped(a, b, p);
            Vector3d valued = Vector3d.LerpUnclamped(ad, bd, p);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Vector3d")]
    public void Magnitude1()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);

            Vector3 a = new Vector3(ax, ay, az);

            Vector3d ad = new Vector3d(ax, ay, az);

            float value = Vector3.Magnitude(a);
            double valued = Vector3d.Magnitude(ad);

            Assert.True(Approximate(value, valued));
        }
    }
    
    [Test]
    [Category("Vector3d")]
    public void Max()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az;
            float bx, by, bz;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);

            bx = UnityEngine.Random.Range(-10F, 10F);
            by = UnityEngine.Random.Range(-10F, 10F);
            bz = UnityEngine.Random.Range(-10F, 10F);

            Vector3 a = new Vector3(ax, ay, az);
            Vector3 b = new Vector3(bx, by, bz);

            Vector3d ad = new Vector3d(ax, ay, az);
            Vector3d bd = new Vector3d(bx, by, bz);

            Vector3 value = Vector3.Max(a, b);
            Vector3d valued = Vector3d.Max(ad, bd);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Vector3d")]
    public void Min()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az;
            float bx, by, bz;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);

            bx = UnityEngine.Random.Range(-10F, 10F);
            by = UnityEngine.Random.Range(-10F, 10F);
            bz = UnityEngine.Random.Range(-10F, 10F);

            Vector3 a = new Vector3(ax, ay, az);
            Vector3 b = new Vector3(bx, by, bz);

            Vector3d ad = new Vector3d(ax, ay, az);
            Vector3d bd = new Vector3d(bx, by, bz);

            Vector3 value = Vector3.Min(a, b);
            Vector3d valued = Vector3d.Min(ad, bd);

            Assert.True(Approximate(value, valued));
        }
    }
    
    [Test]
    [Category("Vector3d")]
    public void MoveTowards()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az;
            float bx, by, bz;
            float p;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);

            bx = UnityEngine.Random.Range(-10F, 10F);
            by = UnityEngine.Random.Range(-10F, 10F);
            bz = UnityEngine.Random.Range(-10F, 10F);

            p = UnityEngine.Random.Range(-20F, 20F);

            Vector3 a = new Vector3(ax, ay, az);
            Vector3 b = new Vector3(bx, by, bz);

            Vector3d ad = new Vector3d(ax, ay, az);
            Vector3d bd = new Vector3d(bx, by, bz);

            Vector3 value = Vector3.MoveTowards(a, b, p);
            Vector3d valued = Vector3d.MoveTowards(ad, bd, p);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Vector3d")]
    public void Normalize1()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);

            Vector3 a = new Vector3(ax, ay, az);

            Vector3d ad = new Vector3d(ax, ay, az);

            Vector3 value = Vector3.Normalize(a);
            Vector3d valued = Vector3d.Normalize(ad);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Vector3d")]
    public void OrthoNormalize1()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az;
            float bx, by, bz;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);

            bx = UnityEngine.Random.Range(-10F, 10F);
            by = UnityEngine.Random.Range(-10F, 10F);
            bz = UnityEngine.Random.Range(-10F, 10F);

            Vector3 a = new Vector3(ax, ay, az);
            Vector3 b = new Vector3(bx, by, bz);

            Vector3d ad = new Vector3d(ax, ay, az);
            Vector3d bd = new Vector3d(bx, by, bz);

            Vector3.OrthoNormalize(ref a, ref b);
            Vector3d.OrthoNormalize(ref ad, ref bd);

            Assert.True(Approximate(a, ad) && Approximate(b, bd));
        }
    }

    [Test]
    [Category("Vector3d")]
    public void OrthoNormalize2()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az;
            float bx, by, bz;
            float cx, cy, cz;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);

            bx = UnityEngine.Random.Range(-10F, 10F);
            by = UnityEngine.Random.Range(-10F, 10F);
            bz = UnityEngine.Random.Range(-10F, 10F);

            cx = UnityEngine.Random.Range(-10F, 10F);
            cy = UnityEngine.Random.Range(-10F, 10F);
            cz = UnityEngine.Random.Range(-10F, 10F);

            Vector3 a = new Vector3(ax, ay, az);
            Vector3 b = new Vector3(bx, by, bz);
            Vector3 c = new Vector3(cx, cy, cz);

            Vector3d ad = new Vector3d(ax, ay, az);
            Vector3d bd = new Vector3d(bx, by, bz);
            Vector3d cd = new Vector3d(cx, cy, cz);

            Vector3.OrthoNormalize(ref a, ref b, ref c);
            Vector3d.OrthoNormalize(ref ad, ref bd, ref cd);

            Assert.True(Approximate(a, ad) && Approximate(b, bd) && Approximate(c, cd));
        }
    }

    [Test]
    [Category("Vector3d")]
    public void Project()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az;
            float bx, by, bz;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);

            bx = UnityEngine.Random.Range(-10F, 10F);
            by = UnityEngine.Random.Range(-10F, 10F);
            bz = UnityEngine.Random.Range(-10F, 10F);

            Vector3 a = new Vector3(ax, ay, az);
            Vector3 b = new Vector3(bx, by, bz);

            Vector3d ad = new Vector3d(ax, ay, az);
            Vector3d bd = new Vector3d(bx, by, bz);

            Vector3 value = Vector3.Project(a, b);
            Vector3d valued = Vector3d.Project(ad, bd);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Vector3d")]
    public void ProjectOnPlane()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az;
            float bx, by, bz;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);

            bx = UnityEngine.Random.Range(-10F, 10F);
            by = UnityEngine.Random.Range(-10F, 10F);
            bz = UnityEngine.Random.Range(-10F, 10F);

            Vector3 a = new Vector3(ax, ay, az);
            Vector3 b = new Vector3(bx, by, bz);

            Vector3d ad = new Vector3d(ax, ay, az);
            Vector3d bd = new Vector3d(bx, by, bz);

            Vector3 value = Vector3.ProjectOnPlane(a, b);
            Vector3d valued = Vector3d.ProjectOnPlane(ad, bd);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Vector3d")]
    public void Reflect()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az;
            float bx, by, bz;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);

            bx = UnityEngine.Random.Range(-10F, 10F);
            by = UnityEngine.Random.Range(-10F, 10F);
            bz = UnityEngine.Random.Range(-10F, 10F);

            Vector3 a = new Vector3(ax, ay, az);
            Vector3 b = new Vector3(bx, by, bz);

            Vector3d ad = new Vector3d(ax, ay, az);
            Vector3d bd = new Vector3d(bx, by, bz);

            Vector3 value = Vector3.Reflect(a, b);
            Vector3d valued = Vector3d.Reflect(ad, bd);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Vector3d")]
    public void RotateTowards()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az;
            float bx, by, bz;
            float p, q;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);

            bx = UnityEngine.Random.Range(-10F, 10F);
            by = UnityEngine.Random.Range(-10F, 10F);
            bz = UnityEngine.Random.Range(-10F, 10F);

            p = UnityEngine.Random.Range(-2F, 2F);
            q = UnityEngine.Random.Range(-2F, 2F);

            Vector3 a = new Vector3(ax, ay, az);
            Vector3 b = new Vector3(bx, by, bz);

            Vector3d ad = new Vector3d(ax, ay, az);
            Vector3d bd = new Vector3d(bx, by, bz);

            Vector3 value = Vector3.RotateTowards(a, b, p, q);
            Vector3d valued = Vector3d.RotateTowards(ad, bd, p, q);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Vector3d")]
    public void Scale1()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az;
            float bx, by, bz;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);

            bx = UnityEngine.Random.Range(-10F, 10F);
            by = UnityEngine.Random.Range(-10F, 10F);
            bz = UnityEngine.Random.Range(-10F, 10F);

            Vector3 a = new Vector3(ax, ay, az);
            Vector3 b = new Vector3(bx, by, bz);

            Vector3d ad = new Vector3d(ax, ay, az);
            Vector3d bd = new Vector3d(bx, by, bz);

            Vector3 value = Vector3.Scale(a, b);
            Vector3d valued = Vector3d.Scale(ad, bd);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Vector3d")]
    public void Slerp()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az;
            float bx, by, bz;
            float p;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);

            bx = UnityEngine.Random.Range(-10F, 10F);
            by = UnityEngine.Random.Range(-10F, 10F);
            bz = UnityEngine.Random.Range(-10F, 10F);

            p = UnityEngine.Random.Range(-2F, 2F);

            Vector3 a = new Vector3(ax, ay, az);
            Vector3 b = new Vector3(bx, by, bz);

            Vector3d ad = new Vector3d(ax, ay, az);
            Vector3d bd = new Vector3d(bx, by, bz);

            Vector3 value = Vector3.Slerp(a, b, p);
            Vector3d valued = Vector3d.Slerp(ad, bd, p);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Vector3d")]
    public void SlerpUnclamped()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az;
            float bx, by, bz;
            float p;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);

            bx = UnityEngine.Random.Range(-10F, 10F);
            by = UnityEngine.Random.Range(-10F, 10F);
            bz = UnityEngine.Random.Range(-10F, 10F);

            p = UnityEngine.Random.Range(-2F, 2F);

            Vector3 a = new Vector3(ax, ay, az);
            Vector3 b = new Vector3(bx, by, bz);

            Vector3d ad = new Vector3d(ax, ay, az);
            Vector3d bd = new Vector3d(bx, by, bz);

            Vector3 value = Vector3.SlerpUnclamped(a, b, p);
            Vector3d valued = Vector3d.SlerpUnclamped(ad, bd, p);

            Assert.True(Approximate(value, valued));
        }
    }
    
    [Test]
    [Category("Vector3d")]
    public void SmoothDamp1()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az;
            float bx, by, bz;
            float cx, cy, cz;
            float p, q;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);

            bx = UnityEngine.Random.Range(-10F, 10F);
            by = UnityEngine.Random.Range(-10F, 10F);
            bz = UnityEngine.Random.Range(-10F, 10F);

            cx = UnityEngine.Random.Range(-10F, 10F);
            cy = UnityEngine.Random.Range(-10F, 10F);
            cz = UnityEngine.Random.Range(-10F, 10F);

            p = UnityEngine.Random.Range(-2F, 2F);
            q = UnityEngine.Random.Range(-2F, 2F);

            Vector3 a = new Vector3(ax, ay, az);
            Vector3 b = new Vector3(bx, by, bz);
            Vector3 c = new Vector3(cx, cy, cz);

            Vector3d ad = new Vector3d(ax, ay, az);
            Vector3d bd = new Vector3d(bx, by, bz);
            Vector3d cd = new Vector3d(cx, cy, cz);

            Vector3 value = Vector3.SmoothDamp(a, b, ref c, p, q);
            Vector3d valued = Vector3d.SmoothDamp(ad, bd, ref cd, p, q);

            Assert.True(Approximate(value, valued) && Approximate(c, cd));
        }
    }

    [Test]
    [Category("Vector3d")]
    public void SmoothDamp2()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az;
            float bx, by, bz;
            float cx, cy, cz;
            float p;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);

            bx = UnityEngine.Random.Range(-10F, 10F);
            by = UnityEngine.Random.Range(-10F, 10F);
            bz = UnityEngine.Random.Range(-10F, 10F);

            cx = UnityEngine.Random.Range(-10F, 10F);
            cy = UnityEngine.Random.Range(-10F, 10F);
            cz = UnityEngine.Random.Range(-10F, 10F);

            p = UnityEngine.Random.Range(-2F, 2F);

            Vector3 a = new Vector3(ax, ay, az);
            Vector3 b = new Vector3(bx, by, bz);
            Vector3 c = new Vector3(cx, cy, cz);

            Vector3d ad = new Vector3d(ax, ay, az);
            Vector3d bd = new Vector3d(bx, by, bz);
            Vector3d cd = new Vector3d(cx, cy, cz);

            Vector3 value = Vector3.SmoothDamp(a, b, ref c, p);
            Vector3d valued = Vector3d.SmoothDamp(ad, bd, ref cd, p);

            Assert.True(Approximate(value, valued) && Approximate(c, cd));
        }
    }

    [Test]
    [Category("Vector3d")]
    public void SmoothDamp3()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az;
            float bx, by, bz;
            float cx, cy, cz;
            float p, q, r;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);

            bx = UnityEngine.Random.Range(-10F, 10F);
            by = UnityEngine.Random.Range(-10F, 10F);
            bz = UnityEngine.Random.Range(-10F, 10F);

            cx = UnityEngine.Random.Range(-10F, 10F);
            cy = UnityEngine.Random.Range(-10F, 10F);
            cz = UnityEngine.Random.Range(-10F, 10F);

            p = UnityEngine.Random.Range(-2F, 2F);
            q = UnityEngine.Random.Range(-2F, 2F);
            r = UnityEngine.Random.Range(-2F, 2F);

            Vector3 a = new Vector3(ax, ay, az);
            Vector3 b = new Vector3(bx, by, bz);
            Vector3 c = new Vector3(cx, cy, cz);

            Vector3d ad = new Vector3d(ax, ay, az);
            Vector3d bd = new Vector3d(bx, by, bz);
            Vector3d cd = new Vector3d(cx, cy, cz);

            Vector3 value = Vector3.SmoothDamp(a, b, ref c, p, q, r);
            Vector3d valued = Vector3d.SmoothDamp(ad, bd, ref cd, p, q, r);

            Assert.True(Approximate(value, valued) && Approximate(c, cd));
        }
    }

    [Test]
    [Category("Vector3d")]
    public void SqrMagnitude()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);

            Vector3 a = new Vector3(ax, ay, az);

            Vector3d ad = new Vector3d(ax, ay, az);

            float value = Vector3.SqrMagnitude(a);
            double valued = Vector3d.SqrMagnitude(ad);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Vector3d")]
    public void Normalize2()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);

            Vector3 a = new Vector3(ax, ay, az);

            Vector3d ad = new Vector3d(ax, ay, az);

            a.Normalize();
            ad.Normalize();

            Assert.True(Approximate(a, ad));
        }
    }
    
    [Test]
    [Category("Vector3d")]
    public void Scale2()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az;
            float bx, by, bz;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);

            bx = UnityEngine.Random.Range(-10F, 10F);
            by = UnityEngine.Random.Range(-10F, 10F);
            bz = UnityEngine.Random.Range(-10F, 10F);

            Vector3 a = new Vector3(ax, ay, az);
            Vector3 b = new Vector3(bx, by, bz);

            Vector3d ad = new Vector3d(ax, ay, az);
            Vector3d bd = new Vector3d(bx, by, bz);

            a.Scale(b);
            ad.Scale(bd);

            Assert.True(Approximate(a, ad));
        }
    }

    [Test]
    [Category("Vector3d")]
    public void Magnitude2()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);

            Vector3 a = new Vector3(ax, ay, az);

            Vector3d ad = new Vector3d(ax, ay, az);
            
            Assert.True(Approximate(a.magnitude, ad.magnitude));
        }
    }

    [Test]
    [Category("Vector3d")]
    public void Normalized()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);

            Vector3 a = new Vector3(ax, ay, az);

            Vector3d ad = new Vector3d(ax, ay, az);

            Assert.True(Approximate(a.normalized, ad.normalized));
        }
    }
    
    private bool Approximate(Vector3 v, Vector3d vd)
    {
        if (Math.Abs(v.x - vd.x) < deviation &&
            Math.Abs(v.y - vd.y) < deviation &&
            Math.Abs(v.z - vd.z) < deviation)
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
