/// QuaterniondNUnit.cs
/// 
/// Repeat random values to test whether the output of the method in struct Quaterniond and struct Quaternion is similar.
/// 
/// 重复随机取值，测试Quaterniond与Quaternion中方法的输出是否近似。
/// 
/// Created by D子宇 on 2018.3.17
/// 
/// Email: darkziyu@126.com
using System;
using Mathd;
using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class QuaterniondNUnit {
    private const double deviation = 0.1;
    private const int count = 1;


    [Test]
    [Category("Quaterniond")]
    public void Angle()
    {
        for (int i = 0; i < count; i++)
        {
            Quaternion a = new Quaternion();
            Quaternion b = new Quaternion();
            Quaterniond ad = new Quaterniond();
            Quaterniond bd = new Quaterniond();

            RandomQuaternion(ref a, ref ad);
            RandomQuaternion(ref b, ref bd);

            float value = Quaternion.Angle(a, b);
            double valued = Quaterniond.Angle(ad, bd);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Quaterniond")]
    public void AngleAxis()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az;
            float p;

            ax = UnityEngine.Random.Range(0, 360F);
            ay = UnityEngine.Random.Range(0, 360F);
            az = UnityEngine.Random.Range(0, 360F);

            p = UnityEngine.Random.Range(0, 360F);

            Quaternion value = Quaternion.AngleAxis(p, new Vector3(ax, ay, az));
            Quaterniond valued = Quaterniond.AngleAxis(p, new Vector3d(ax, ay, az));

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Quaterniond")]
    public void Dot()
    {
        for (int i = 0; i < count; i++)
        {
            Quaternion a = new Quaternion();
            Quaternion b = new Quaternion();
            Quaterniond ad = new Quaterniond();
            Quaterniond bd = new Quaterniond();

            RandomQuaternion(ref a, ref ad);
            RandomQuaternion(ref b, ref bd);

            float value = Quaternion.Dot(a, b);
            double valued = Quaterniond.Dot(ad, bd);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Quaterniond")]
    public void Euler()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az;

            ax = UnityEngine.Random.Range(0, 360F);
            ay = UnityEngine.Random.Range(0, 360F);
            az = UnityEngine.Random.Range(0, 360F);

            Quaternion value = Quaternion.Euler(new Vector3(ax, ay, az));
            Quaterniond valued = Quaterniond.Euler(new Vector3d(ax, ay, az));

            Assert.True(Approximate(value, valued));
        }
    }

    //[Test]
    //[Category("Quaterniond")]
    //public void FromToRotation()
    //{
    //    for (int i = 0; i < count; i++)
    //    {
    //        float ax, ay, az;
    //        float bx, by, bz;

    //        ax = UnityEngine.Random.Range(-10F, 10F);
    //        ay = UnityEngine.Random.Range(-10F, 10F);
    //        az = UnityEngine.Random.Range(-10F, 10F);

    //        bx = UnityEngine.Random.Range(-10F, 10F);
    //        by = UnityEngine.Random.Range(-10F, 10F);
    //        bz = UnityEngine.Random.Range(-10F, 10F);

    //        Quaternion value = Quaternion.FromToRotation(new Vector3(ax, ay, az), new Vector3(bx, by, bz));
    //        Quaterniond valued = Quaterniond.FromToRotation(new Vector3d(ax, ay, az), new Vector3d(bx, by, bz));

    //        Assert.True(Approximate(value, valued));
    //    }
    //}

    [Test]
    [Category("Quaterniond")]
    public void Inverse()
    {
        for (int i = 0; i < count; i++)
        {
            Quaternion a = new Quaternion();
            Quaterniond ad = new Quaterniond();

            RandomQuaternion(ref a, ref ad);

            Quaternion value = Quaternion.Inverse(a);
            Quaterniond valued = Quaterniond.Inverse(ad);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Quaterniond")]
    public void Lerp()
    {
        for (int i = 0; i < count; i++)
        {
            Quaternion a = new Quaternion();
            Quaternion b = new Quaternion();
            Quaterniond ad = new Quaterniond();
            Quaterniond bd = new Quaterniond();

            RandomQuaternion(ref a, ref ad);
            RandomQuaternion(ref b, ref bd);

            float p = UnityEngine.Random.Range(-2F, 2F);

            Quaternion value = Quaternion.Lerp(a, b, p);
            Quaterniond valued = Quaterniond.Lerp(ad, bd, p);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Quaterniond")]
    public void LerpUnclamped()
    {
        for (int i = 0; i < count; i++)
        {
            Quaternion a = new Quaternion();
            Quaternion b = new Quaternion();
            Quaterniond ad = new Quaterniond();
            Quaterniond bd = new Quaterniond();

            RandomQuaternion(ref a, ref ad);
            RandomQuaternion(ref b, ref bd);

            float p = UnityEngine.Random.Range(2F, 3F);

            Quaternion value = Quaternion.LerpUnclamped(a, b, p);
            Quaterniond valued = Quaterniond.LerpUnclamped(ad, bd, p);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Quaterniond")]
    public void LookRotation()
    {
        for (int i = 0; i < count; i++)
        {
            float ax, ay, az;

            ax = UnityEngine.Random.Range(-10F, 10F);
            ay = UnityEngine.Random.Range(-10F, 10F);
            az = UnityEngine.Random.Range(-10F, 10F);

            Quaternion value = Quaternion.LookRotation(new Vector3(ax, ay, az));
            Quaterniond valued = Quaterniond.LookRotation(new Vector3d(ax, ay, az));

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Quaterniond")]
    public void LookRotation1()
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

            Quaternion value = Quaternion.LookRotation(new Vector3(ax, ay, az), new Vector3(bx, by, bz));
            Quaterniond valued = Quaterniond.LookRotation(new Vector3d(ax, ay, az), new Vector3d(bx, by, bz));

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Quaterniond")]
    public void RotateTowards()
    {
        for (int i = 0; i < count; i++)
        {
            Quaternion a = new Quaternion();
            Quaternion b = new Quaternion();
            Quaterniond ad = new Quaterniond();
            Quaterniond bd = new Quaterniond();

            float p = UnityEngine.Random.Range(-2F, 2F);

            RandomQuaternion(ref a, ref ad);
            RandomQuaternion(ref b, ref bd);

            Quaternion value = Quaternion.RotateTowards(a, b, p);
            Quaterniond valued = Quaterniond.RotateTowards(ad, bd, p);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Quaterniond")]
    public void Slerp()
    {
        for (int i = 0; i < count; i++)
        {
            Quaternion a = new Quaternion();
            Quaternion b = new Quaternion();
            Quaterniond ad = new Quaterniond();
            Quaterniond bd = new Quaterniond();

            float p = UnityEngine.Random.Range(-2F, 2F);

            RandomQuaternion(ref a, ref ad);
            RandomQuaternion(ref b, ref bd);

            Quaternion value = Quaternion.Slerp(a, b, p);
            Quaterniond valued = Quaterniond.Slerp(ad, bd, p);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Quaterniond")]
    public void SlerpUnclamped()
    {
        for (int i = 0; i < count; i++)
        {
            Quaternion a = new Quaternion();
            Quaternion b = new Quaternion();
            Quaterniond ad = new Quaterniond();
            Quaterniond bd = new Quaterniond();

            float p = UnityEngine.Random.Range(2F, 3F);

            RandomQuaternion(ref a, ref ad);
            RandomQuaternion(ref b, ref bd);

            Quaternion value = Quaternion.SlerpUnclamped(a, b, p);
            Quaterniond valued = Quaterniond.SlerpUnclamped(ad, bd, p);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Quaterniond")]
    public void ToAngleAxis()
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 a;
            Vector3d b;
            float aa;
            double bb;

            Quaternion quat = new Quaternion();
            Quaterniond quatd = new Quaterniond();

            RandomQuaternion(ref quat, ref quatd);

            quat.ToAngleAxis(out aa, out a);
            quatd.ToAngleAxis(out bb, out b);

            Assert.True(Approximate(aa, bb) && Approximate(a, b));
        }
    }
    [Test]
    [Category("Quaterniond")]
    public void EulerAngles()
    {
        for (int i = 0; i < count; i++)
        {
            Quaternion a = new Quaternion();
            Quaterniond ad = new Quaterniond();
            
            RandomQuaternion(ref a, ref ad);
            
            Assert.True(Approximate(a.eulerAngles, ad.eulerAngles));
        }
    }


    private void RandomQuaternion(ref Quaternion quat, ref Quaterniond quatd) {
        float ax, ay, az;

        ax = UnityEngine.Random.Range(0, 360F);
        ay = UnityEngine.Random.Range(0, 360F);
        az = UnityEngine.Random.Range(0, 360F);

        quat = Quaternion.Euler(new Vector3(ax, ay, az));
        quatd = Quaterniond.Euler(new Vector3d(ax, ay, az));
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

    private bool Approximate(Quaternion v, Quaterniond vd)
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
