// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Formula.ErrorCodeAttribute
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Formula;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class ErrorCodeAttribute : Attribute
{
  private string m_StringValue = string.Empty;
  private int m_ErrorCode;

  private ErrorCodeAttribute()
  {
  }

  public ErrorCodeAttribute(string stringValue, int errorCode)
  {
    this.m_StringValue = stringValue;
    this.m_ErrorCode = errorCode;
  }

  public string StringValue => this.m_StringValue;

  public int ErrorCode => this.m_ErrorCode;
}
