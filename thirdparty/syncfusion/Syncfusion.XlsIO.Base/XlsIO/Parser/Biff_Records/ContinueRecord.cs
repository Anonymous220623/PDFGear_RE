// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.ContinueRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[Biff(TBIFFRecord.Continue)]
[CLSCompliant(false)]
public class ContinueRecord : BiffRecordRawWithArray, ILengthSetter
{
  public override bool NeedDataArray => true;

  public void SetLength(int len) => this.m_iLength = len;

  public void SetData(byte[] arrData) => this.m_data = arrData;

  public override void ParseStructure()
  {
  }

  public override void InfillInternalData(ExcelVersion version)
  {
  }

  public override int GetStoreSize(ExcelVersion version) => this.m_iLength;
}
