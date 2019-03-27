/// Matrix4x4dNUnit.cs
/// 
/// Repeat random values to test whether the output of the method in struct Matrix4x4d and struct Matrix4x4 is similar.
/// 
/// 重复随机取值，测试Matrix4x4d与Matrix4x4中方法的输出是否近似。
/// 
/// Created by D子宇 on 2018.3.17
/// 
/// Email: darkziyu@126.com
using System;
using Mathd;
using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class Matrix4x4dNUnit {

    private const double deviation = 0.1;
    private const int count = 100;

    [Test]
    [Category("Matrix4x4d")]
    public void Determinant()
    {
        for (int i = 0; i < count; i++)
        {
            Matrix4x4 a = new Matrix4x4();
            Matrix4x4d ad = new Matrix4x4d();

            RandomMatrix(ref a, ref ad);

            float value = Matrix4x4.Determinant(a);
            double valued = Matrix4x4d.Determinant(ad);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Matrix4x4d")]
    public void Inverse()
    {
        for (int i = 0; i < count; i++)
        {
            Matrix4x4 a = new Matrix4x4();
            Matrix4x4d ad = new Matrix4x4d();

            RandomMatrix(ref a, ref ad);

            Matrix4x4 value = Matrix4x4.Inverse(a);
            Matrix4x4d valued = Matrix4x4d.Inverse(ad);

            Assert.True(Approximate(value, valued));
        }
    }

    //[Test]
    //[Category("Matrix4x4d")]
    //public void LookAt()
    //{
    //    for (int i = 0; i < count; i++)
    //    {
    //        float ax, ay, az;
    //        float bx, by, bz;
    //        float cx, cy, cz;

    //        ax = UnityEngine.Random.Range(-10F, 10F);
    //        ay = UnityEngine.Random.Range(-10F, 10F);
    //        az = UnityEngine.Random.Range(-10F, 10F);

    //        bx = UnityEngine.Random.Range(-10F, 10F);
    //        by = UnityEngine.Random.Range(-10F, 10F);
    //        bz = UnityEngine.Random.Range(-10F, 10F);

    //        cx = UnityEngine.Random.Range(-10F, 10F);
    //        cy = UnityEngine.Random.Range(-10F, 10F);
    //        cz = UnityEngine.Random.Range(-10F, 10F);
            
    //        Matrix4x4 value = Matrix4x4.LookAt(new Vector3(ax, ay, az), new Vector3(bx, by, bz), new Vector3(cx, cy, cz));
    //        Matrix4x4d valued = Matrix4x4d.LookAt(new Vector3d(ax, ay, az), new Vector3d(bx, by, bz), new Vector3d(cx, cy, cz));

    //        Assert.True(Approximate(value, valued));
    //    }
    //}

    [Test]
    [Category("Matrix4x4d")]
    public void Ortho()
    {
        for (int i = 0; i < count; i++)
        {
            float a, b, c, d, e, f;

            a = UnityEngine.Random.Range(-10F, 10F);
            b = UnityEngine.Random.Range(-10F, 10F);
            c = UnityEngine.Random.Range(-10F, 10F);
            d = UnityEngine.Random.Range(-10F, 10F);
            e = UnityEngine.Random.Range(-10F, 10F);
            f = UnityEngine.Random.Range(-10F, 10F);
            
            Matrix4x4 value = Matrix4x4.Ortho(a, b, c, d, e, f);
            Matrix4x4d valued = Matrix4x4d.Ortho(a, b, c, d, e, f);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Matrix4x4d")]
    public void Perspective()
    {
        for (int i = 0; i < count; i++)
        {
            float a, b, c, d;

            a = UnityEngine.Random.Range(-10F, 10F);
            b = UnityEngine.Random.Range(-10F, 10F);
            c = UnityEngine.Random.Range(-10F, 10F);
            d = UnityEngine.Random.Range(-10F, 10F);

            Matrix4x4 value = Matrix4x4.Perspective(a, b, c, d);
            Matrix4x4d valued = Matrix4x4d.Perspective(a, b, c, d);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Matrix4x4d")]
    public void Scale()
    {
        for (int i = 0; i < count; i++)
        {
            float a, b, c;

            a = UnityEngine.Random.Range(-10F, 10F);
            b = UnityEngine.Random.Range(-10F, 10F);
            c = UnityEngine.Random.Range(-10F, 10F);

            Matrix4x4 value = Matrix4x4.Scale(new Vector3(a, b, c));
            Matrix4x4d valued = Matrix4x4d.Scale(new Vector3d(a, b, c));

            Assert.True(Approximate(value, valued));
        }
    }

    //[Test]
    //[Category("Matrix4x4d")]
    //public void Translate()
    //{
    //    for (int i = 0; i < count; i++)
    //    {
    //        float a, b, c;

    //        a = UnityEngine.Random.Range(-10F, 10F);
    //        b = UnityEngine.Random.Range(-10F, 10F);
    //        c = UnityEngine.Random.Range(-10F, 10F);

    //        Matrix4x4 value = Matrix4x4.Translate(new Vector3(a, b, c));
    //        Matrix4x4d valued = Matrix4x4d.Translate(new Vector3d(a, b, c));

    //        Assert.True(Approximate(value, valued));
    //    }
    //}

    [Test]
    [Category("Matrix4x4d")]
    public void Transpose()
    {
        for (int i = 0; i < count; i++)
        {
            Matrix4x4 m = new Matrix4x4();
            Matrix4x4d md = new Matrix4x4d();

            RandomMatrix(ref m, ref md);

            Matrix4x4 value = Matrix4x4.Transpose(m);
            Matrix4x4d valued = Matrix4x4d.Transpose(md);

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Matrix4x4d")]
    public void TRS()
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

            Matrix4x4 value = Matrix4x4.TRS(new Vector3(ax, ay, az), Quaternion.Euler(new Vector3(bx,by,bz)), new Vector3(cx, cy, cz));
            Matrix4x4d valued = Matrix4x4d.TRS(new Vector3d(ax, ay, az), Quaterniond.Euler(new Vector3d(bx, by, bz)), new Vector3d(cx, cy, cz));

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Matrix4x4d")]
    public void MultiplyPoint()
    {
        for (int i = 0; i < count; i++)
        {
            float a, b, c;

            a = UnityEngine.Random.Range(-10F, 10F);
            b = UnityEngine.Random.Range(-10F, 10F);
            c = UnityEngine.Random.Range(-10F, 10F);

            Matrix4x4 m = new Matrix4x4();
            Matrix4x4d md = new Matrix4x4d();

            RandomMatrix(ref m, ref md);

            Vector3 value = m.MultiplyPoint(new Vector3(a, b, c));
            Vector3d valued = md.MultiplyPoint(new Vector3d(a, b, c));

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Matrix4x4d")]
    public void MultiplyPoint3x4()
    {
        for (int i = 0; i < count; i++)
        {
            float a, b, c;

            a = UnityEngine.Random.Range(-10F, 10F);
            b = UnityEngine.Random.Range(-10F, 10F);
            c = UnityEngine.Random.Range(-10F, 10F);

            Matrix4x4 m = new Matrix4x4();
            Matrix4x4d md = new Matrix4x4d();

            RandomMatrix(ref m, ref md);

            Vector3 value = m.MultiplyPoint3x4(new Vector3(a, b, c));
            Vector3d valued = md.MultiplyPoint3x4(new Vector3d(a, b, c));

            Assert.True(Approximate(value, valued));
        }
    }

    [Test]
    [Category("Matrix4x4d")]
    public void MultiplyVector()
    {
        for (int i = 0; i < count; i++)
        {
            float a, b, c;

            a = UnityEngine.Random.Range(-10F, 10F);
            b = UnityEngine.Random.Range(-10F, 10F);
            c = UnityEngine.Random.Range(-10F, 10F);

            Matrix4x4 m = new Matrix4x4();
            Matrix4x4d md = new Matrix4x4d();

            RandomMatrix(ref m, ref md);

            Vector3 value = m.MultiplyVector(new Vector3(a, b, c));
            Vector3d valued = md.MultiplyVector(new Vector3d(a, b, c));

            Assert.True(Approximate(value, valued));
        }
    }
    
    private void RandomMatrix(ref Matrix4x4 matrix, ref Matrix4x4d matrixd) {
        for (int i = 0; i < 16; i++)
        {
            float temp = UnityEngine.Random.Range(-10F, 10F);
            matrix[i] = temp;
            matrixd[i] = temp;
        }
    }
    
    private bool Approximate(Matrix4x4 v, Matrix4x4d vd)
    {
        for (int i = 0; i < 16; i++)
        {
            if (Math.Abs(v[i] - vd[i]) > deviation)
            {
                Assert.Fail(string.Format("{0}\n{1}", v.ToString("0.00000"), vd.ToString("0.00000")));
                return false;
            }
        }
        return true;
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
