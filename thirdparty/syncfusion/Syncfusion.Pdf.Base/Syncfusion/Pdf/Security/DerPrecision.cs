// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.DerPrecision
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class DerPrecision : Asn1Encode
{
  private readonly DerInteger m_seconds;
  private readonly DerInteger m_milliSeconds;
  private readonly DerInteger m_microSeconds;
  private readonly int m_minMilliSeconds = 1;
  private readonly int m_maxMilliSeconds = 999;
  private readonly int m_minMicroSeconds = 1;
  private readonly int m_maxMicroSeconds = 999;

  internal DerPrecision(DerInteger seconds, DerInteger milliSeconds, DerInteger microSeconds)
  {
    if (milliSeconds != null && (milliSeconds.Value.IntValue < this.m_minMilliSeconds || milliSeconds.Value.IntValue > this.m_maxMilliSeconds))
      throw new ArgumentException("Specified milli seconds value is not in range");
    if (microSeconds != null && (microSeconds.Value.IntValue < this.m_minMicroSeconds || microSeconds.Value.IntValue > this.m_maxMicroSeconds))
      throw new ArgumentException("Specified micro seconds value is not in range");
    this.m_seconds = seconds;
    this.m_milliSeconds = milliSeconds;
    this.m_microSeconds = microSeconds;
  }

  private DerPrecision(Asn1Sequence sequence)
  {
    for (int index = 0; index < sequence.Count; ++index)
    {
      if (sequence[index] is DerInteger)
        this.m_seconds = (DerInteger) sequence[index];
      else if (sequence[index] is DerTag)
      {
        DerTag tag = (DerTag) sequence[index];
        switch (tag.TagNumber)
        {
          case 0:
            this.m_milliSeconds = DerInteger.GetNumber((Asn1Tag) tag, false);
            if (this.m_milliSeconds.Value.IntValue < this.m_minMilliSeconds || this.m_milliSeconds.Value.IntValue > this.m_maxMilliSeconds)
              throw new ArgumentException("Specified value is not in range");
            continue;
          case 1:
            this.m_microSeconds = DerInteger.GetNumber((Asn1Tag) tag, false);
            if (this.m_microSeconds.Value.IntValue < this.m_minMicroSeconds || this.m_microSeconds.Value.IntValue > this.m_maxMicroSeconds)
              throw new ArgumentException("Specified value is not in range");
            continue;
          default:
            throw new ArgumentException("Invalid entry in sequence");
        }
      }
    }
  }

  internal static DerPrecision GetDerPrecision(object obj)
  {
    switch (obj)
    {
      case null:
      case DerPrecision _:
        return (DerPrecision) obj;
      case Asn1Sequence _:
        return new DerPrecision((Asn1Sequence) obj);
      default:
        throw new ArgumentException("Invalid entry in precision check : " + obj.GetType().FullName);
    }
  }

  internal DerInteger Seconds => this.m_seconds;

  internal DerInteger MilliSeconds => this.m_milliSeconds;

  internal DerInteger MicroSeconds => this.m_microSeconds;

  public override Asn1 GetAsn1()
  {
    Asn1EncodeCollection collection = new Asn1EncodeCollection(new Asn1Encode[0]);
    if (this.m_seconds != null)
      collection.Add((Asn1Encode) this.m_seconds);
    if (this.m_milliSeconds != null)
      collection.Add((Asn1Encode) new DerTag(false, 0, (Asn1Encode) this.m_milliSeconds));
    if (this.m_microSeconds != null)
      collection.Add((Asn1Encode) new DerTag(false, 1, (Asn1Encode) this.m_microSeconds));
    return (Asn1) new DerSequence(collection);
  }
}
