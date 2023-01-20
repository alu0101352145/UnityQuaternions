using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MyQuaternion
{
    public float realPart;
    public float iPart;
    public float jPart;
    public float kPart;

    public MyQuaternion(float newRealPart, float newIPart, float newJPart, float newKPart) 
    {
        realPart = newRealPart;
        iPart = newIPart;
        jPart = newJPart;
        kPart = newKPart;
    }

    public static MyQuaternion operator +(MyQuaternion a, MyQuaternion b) 
    {
        return new MyQuaternion(a.realPart + b.realPart, a.iPart + b.iPart, a.jPart + b.jPart, a.kPart + b.kPart);
    }

    public static MyQuaternion operator -(MyQuaternion a, MyQuaternion b) 
    {
        return new MyQuaternion(a.realPart - b.realPart, a.iPart - b.iPart, a.jPart - b.jPart, a.kPart - b.kPart);
    }

    public static MyQuaternion operator *(MyQuaternion a, MyQuaternion b) 
    {
        float newRealPart = (a.realPart * b.realPart) - (a.iPart * b.iPart) - (a.jPart * b.jPart) - (a.kPart * b.kPart);
        float newIPart = (a.realPart * b.iPart) + (a.iPart * b.realPart) + (a.jPart * b.kPart) - (a.kPart * b.jPart);
        float newJPart = (a.realPart * b.jPart) - (a.iPart * b.kPart) + (a.jPart * b.realPart) + (a.kPart * b.iPart);
        float newKPart = (a.realPart * b.kPart) + (a.iPart * b.jPart) - (a.jPart * b.iPart) + (a.kPart * b.realPart);

        return new MyQuaternion(newRealPart, newIPart, newJPart, newKPart);
    }

    public static MyQuaternion operator /(MyQuaternion a, MyQuaternion b) 
    {
        MyQuaternion numerator = a * b.Conjugate();
        float absoluteValueB = b.AbsoluteValue();
        float denominator = absoluteValueB * absoluteValueB;
        
        if (denominator != 0) 
        {
            return new MyQuaternion(numerator.realPart / denominator, numerator.iPart / denominator, 
                                    numerator.jPart / denominator, numerator.kPart / denominator);
        } else 
        {
            throw new InvalidOperationException("Denominator can't be zero");
        }        
    }

    public static MyQuaternion operator %(MyQuaternion a, MyQuaternion b) 
    {
        MyQuaternion numerator = b.Conjugate() * a;
        float absoluteValueB = b.AbsoluteValue();
        float denominator = absoluteValueB * absoluteValueB;
        
        if (denominator != 0) 
        {
            return new MyQuaternion(numerator.realPart / denominator, numerator.iPart / denominator, 
                                    numerator.jPart / denominator, numerator.kPart / denominator);
        } else 
        {
            throw new InvalidOperationException("Denominator can't be zero");
        }        
    }

    public MyQuaternion Conjugate() 
    {
        return new MyQuaternion(realPart, -iPart, -jPart, -kPart);
    }

    public float AbsoluteValue() 
    {
        MyQuaternion productWithConjugate = this * this.Conjugate();
        float productRealPart = productWithConjugate.realPart;
        return Mathf.Sqrt(productRealPart);
    }

    public override String ToString() 
    {
        return $"{realPart} + ({iPart})i + ({jPart})j + ({kPart})k";
    }
}
