// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.ObjRecords.ftEnd
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.ObjRecords;

[CLSCompliant(false)]
public class ftEnd : ObjSubRecord
{
  private const int DEF_RECORD_SIZE = 4;

  public ftEnd()
    : base(TObjSubRecordType.ftEnd)
  {
  }

  public ftEnd(TObjSubRecordType type, ushort length, byte[] buffer)
    : base(type, length, buffer)
  {
  }

  protected override void Parse(byte[] buffer)
  {
  }

  public override int GetStoreSize(ExcelVersion version) => 4;
}
