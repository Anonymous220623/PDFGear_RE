// Decompiled with JetBrains decompiler
// Type: NAudio.SoundFont.Generator
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

#nullable disable
namespace NAudio.SoundFont;

public class Generator
{
  public GeneratorEnum GeneratorType { get; set; }

  public ushort UInt16Amount { get; set; }

  public short Int16Amount
  {
    get => (short) this.UInt16Amount;
    set => this.UInt16Amount = (ushort) value;
  }

  public byte LowByteAmount
  {
    get => (byte) ((uint) this.UInt16Amount & (uint) byte.MaxValue);
    set
    {
      this.UInt16Amount &= (ushort) 65280;
      this.UInt16Amount += (ushort) value;
    }
  }

  public byte HighByteAmount
  {
    get => (byte) (((int) this.UInt16Amount & 65280) >> 8);
    set
    {
      this.UInt16Amount &= (ushort) byte.MaxValue;
      this.UInt16Amount += (ushort) ((uint) value << 8);
    }
  }

  public Instrument Instrument { get; set; }

  public SampleHeader SampleHeader { get; set; }

  public override string ToString()
  {
    if (this.GeneratorType == GeneratorEnum.Instrument)
      return "Generator Instrument " + this.Instrument.Name;
    return this.GeneratorType == GeneratorEnum.SampleID ? $"Generator SampleID {this.SampleHeader}" : $"Generator {this.GeneratorType} {this.UInt16Amount}";
  }
}
