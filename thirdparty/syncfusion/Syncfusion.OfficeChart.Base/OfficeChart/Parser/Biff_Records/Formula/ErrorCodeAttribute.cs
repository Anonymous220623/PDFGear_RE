// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Formula.ErrorCodeAttribute
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Formula;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
internal sealed class ErrorCodeAttribute : Attribute
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
