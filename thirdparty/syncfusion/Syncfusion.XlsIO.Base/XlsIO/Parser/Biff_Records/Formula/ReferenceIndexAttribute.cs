// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Formula.ReferenceIndexAttribute
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Formula;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public sealed class ReferenceIndexAttribute : Attribute
{
  private int m_iIndex = 1;
  private int[] m_arrIndex;
  private Type m_TargetType;

  private ReferenceIndexAttribute()
  {
  }

  public ReferenceIndexAttribute(int index)
  {
    this.m_iIndex = index >= 1 && index <= 3 ? index : throw new ArgumentOutOfRangeException();
    this.m_TargetType = typeof (RefPtg);
  }

  public ReferenceIndexAttribute(params int[] arrParams)
    : this(typeof (RefPtg), arrParams)
  {
  }

  public ReferenceIndexAttribute(Type targetType, params int[] arrParams)
  {
    this.m_arrIndex = new int[arrParams.Length];
    arrParams.CopyTo((Array) this.m_arrIndex, 0);
    this.m_TargetType = targetType;
  }

  public ReferenceIndexAttribute(Type targetType, int index)
  {
    this.m_TargetType = targetType;
    this.m_iIndex = index;
  }

  public int Index => this.m_iIndex;

  public int this[int index]
  {
    get
    {
      return this.m_arrIndex != null && index >= 0 && index < this.m_arrIndex.Length ? this.m_arrIndex[index] : this.Index;
    }
  }

  public Type TargetType => this.m_TargetType;

  public int Count => this.m_arrIndex == null ? 0 : this.m_arrIndex.Length;
}
