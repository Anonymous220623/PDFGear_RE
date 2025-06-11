// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.BiffOffsetOrderAttribute
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
[CLSCompliant(false)]
public sealed class BiffOffsetOrderAttribute : Attribute
{
  private TBIFFRecord[] m_order;

  public TBIFFRecord[] OrderArray => this.m_order;

  private BiffOffsetOrderAttribute()
  {
  }

  public BiffOffsetOrderAttribute(params TBIFFRecord[] order) => this.m_order = order;
}
