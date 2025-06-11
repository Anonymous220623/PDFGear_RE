// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.PosStructReader
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

internal class PosStructReader
{
  internal PosStructReader()
  {
  }

  internal static void Read(
    BinaryReader binaryReader,
    int structCount,
    PosStructReaderDelegate deleg)
  {
    int length = structCount + 1;
    int[] numArray = new int[length];
    for (int index = 0; index < length; ++index)
      numArray[index] = binaryReader.ReadInt32();
    for (int index = 0; index < structCount; ++index)
      deleg(binaryReader, numArray[index], numArray[index + 1]);
  }

  internal static void Read(
    BinaryReader reader,
    int dataLength,
    int structLength,
    PosStructReaderDelegate deleg)
  {
    if (dataLength == 0)
      return;
    int structCount = (dataLength - 4) / (4 + structLength);
    PosStructReader.Read(reader, structCount, deleg);
  }
}
