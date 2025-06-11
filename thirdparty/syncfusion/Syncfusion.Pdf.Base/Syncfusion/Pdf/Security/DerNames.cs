// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.DerNames
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class DerNames : Asn1Encode
{
  private readonly DerName[] m_names;

  internal static DerNames GetDerNames(object obj)
  {
    switch (obj)
    {
      case null:
      case DerNames _:
        return (DerNames) obj;
      case Asn1Sequence _:
        return new DerNames((Asn1Sequence) obj);
      default:
        throw new ArgumentException("Invalid entry in sequence " + obj.GetType().Name, nameof (obj));
    }
  }

  private DerNames(Asn1Sequence sequence)
  {
    this.m_names = new DerName[sequence.Count];
    for (int index = 0; index != sequence.Count; ++index)
      this.m_names[index] = DerName.GetDerName((object) sequence[index]);
  }

  public override Asn1 GetAsn1() => (Asn1) new DerSequence((Asn1Encode[]) this.m_names);

  public override string ToString()
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("DerNames:");
    stringBuilder.Append(Environment.NewLine);
    foreach (DerName name in this.m_names)
    {
      stringBuilder.Append("    ");
      stringBuilder.Append((object) name);
      stringBuilder.Append(Environment.NewLine);
    }
    return stringBuilder.ToString();
  }
}
