// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.WaveFormatExtraData
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System.IO;
using System.Runtime.InteropServices;

#nullable disable
namespace NAudio.Wave;

[StructLayout(LayoutKind.Sequential, Pack = 2)]
public class WaveFormatExtraData : WaveFormat
{
  [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
  private byte[] extraData = new byte[100];

  public byte[] ExtraData => this.extraData;

  internal WaveFormatExtraData()
  {
  }

  public WaveFormatExtraData(BinaryReader reader)
    : base(reader)
  {
    this.ReadExtraData(reader);
  }

  internal void ReadExtraData(BinaryReader reader)
  {
    if (this.extraSize <= (short) 0)
      return;
    reader.Read(this.extraData, 0, (int) this.extraSize);
  }

  public override void Serialize(BinaryWriter writer)
  {
    base.Serialize(writer);
    if (this.extraSize <= (short) 0)
      return;
    writer.Write(this.extraData, 0, (int) this.extraSize);
  }
}
