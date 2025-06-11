// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.BiffAttribute
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class BiffAttribute : Attribute
{
  private TBIFFRecord m_code;

  public TBIFFRecord Code => this.m_code;

  private BiffAttribute()
  {
  }

  public BiffAttribute(TBIFFRecord code) => this.m_code = code;
}
