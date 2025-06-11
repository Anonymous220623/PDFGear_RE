// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.TimeStampElement
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class TimeStampElement : Asn1Encode
{
  private DerObjectID m_type;
  private Asn1Set m_values;

  internal DerObjectID Type => this.m_type;

  internal Asn1Set Values => this.m_values;

  internal static TimeStampElement GetTimeStampElement(object obj)
  {
    switch (obj)
    {
      case null:
      case TimeStampElement _:
        return (TimeStampElement) obj;
      case Asn1Sequence _:
        return new TimeStampElement((Asn1Sequence) obj);
      default:
        throw new ArgumentException("Invalid entry in sequence " + obj.GetType().Name, nameof (obj));
    }
  }

  internal TimeStampElement(Asn1Sequence sequence)
  {
    this.m_type = (DerObjectID) sequence[0];
    this.m_values = (Asn1Set) sequence[1];
  }

  public override Asn1 GetAsn1()
  {
    return (Asn1) new DerSequence(new Asn1Encode[2]
    {
      (Asn1Encode) this.m_type,
      (Asn1Encode) this.m_values
    });
  }
}
