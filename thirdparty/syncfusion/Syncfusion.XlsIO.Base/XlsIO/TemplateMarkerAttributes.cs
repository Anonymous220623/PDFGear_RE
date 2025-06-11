// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.TemplateMarkerAttributes
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO;

[AttributeUsage(AttributeTargets.Property)]
public class TemplateMarkerAttributes : Attribute
{
  private string m_headerName;
  private string m_numberFormat;
  private bool m_bExclude;

  public TemplateMarkerAttributes(string headerName) => this.m_headerName = headerName;

  public TemplateMarkerAttributes(bool exclude)
  {
    this.m_headerName = (string) null;
    this.m_numberFormat = (string) null;
    this.m_bExclude = exclude;
  }

  public TemplateMarkerAttributes(bool exclude, string headerName, string numberFormat)
  {
    if (exclude)
    {
      this.m_headerName = headerName;
      this.m_numberFormat = numberFormat;
    }
    else
    {
      this.m_headerName = (string) null;
      this.m_numberFormat = (string) null;
      this.m_bExclude = exclude;
    }
  }

  public TemplateMarkerAttributes(bool exclude, string numberFormat)
  {
    if (exclude)
    {
      this.m_headerName = (string) null;
      this.m_numberFormat = numberFormat;
    }
    else
    {
      this.m_headerName = (string) null;
      this.m_numberFormat = (string) null;
      this.m_bExclude = exclude;
    }
  }

  public TemplateMarkerAttributes(string headerName, string numberFormat)
  {
    this.m_headerName = this.HeaderName;
    this.m_numberFormat = numberFormat;
  }

  public bool Exclude => this.m_bExclude;

  public string HeaderName => this.m_headerName;

  public string NumberFormat => this.m_numberFormat;
}
