// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.DocIO.ClipboardData
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.CompoundFile.DocIO.Net;
using System;
using System.IO;

#nullable disable
namespace Syncfusion.CompoundFile.DocIO;

public class ClipboardData : ICloneable
{
  public int Format;
  public byte[] Data;

  public object Clone()
  {
    ClipboardData clipboardData = (ClipboardData) this.MemberwiseClone();
    clipboardData.Data = CloneUtils.CloneByteArray(this.Data);
    return (object) clipboardData;
  }

  public int Serialize(Stream stream)
  {
    int num1 = 0;
    int length = this.Data.Length;
    int num2 = num1 + StreamHelper.WriteInt32(stream, length) + StreamHelper.WriteInt32(stream, this.Format);
    stream.Write(this.Data, 0, length);
    return num2 + length;
  }

  public void Parse(Stream stream)
  {
    byte[] buffer = new byte[4];
    int count = StreamHelper.ReadInt32(stream, buffer);
    this.Format = StreamHelper.ReadInt32(stream, buffer);
    this.Data = new byte[count];
    stream.Read(this.Data, 0, count);
  }
}
