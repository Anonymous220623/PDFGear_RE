// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.IBiffStorage
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Security;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

public interface IBiffStorage
{
  TBIFFRecord TypeCode { get; }

  int RecordCode { get; }

  bool NeedDataArray { get; }

  long StreamPos { get; set; }

  int GetStoreSize(ExcelVersion version);

  int FillStream(
    BinaryWriter writer,
    DataProvider provider,
    IEncryptor encryptor,
    int streamPosition);
}
