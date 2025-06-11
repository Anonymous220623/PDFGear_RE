// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODF.Base.ODFImplementation.NumberType
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.ODF.Base.ODFImplementation;

internal class NumberType : CommonType
{
  private string m_decimalReplacement;
  private string m_displayFactor;
  private EmbeddedTextType m_embeddedText;

  internal string DecimalReplacement
  {
    get => this.m_decimalReplacement;
    set => this.m_decimalReplacement = value;
  }

  internal string DisplayFactor
  {
    get => this.m_displayFactor;
    set => this.m_displayFactor = value;
  }

  internal EmbeddedTextType EmbeddedText
  {
    get => this.m_embeddedText;
    set => this.m_embeddedText = value;
  }

  internal NumberType() => this.m_embeddedText = new EmbeddedTextType();

  public override bool Equals(object obj)
  {
    if (!(obj is NumberType numberType))
      return false;
    bool flag1 = this.DecimalPlaces.Equals(numberType.DecimalPlaces);
    if (!flag1)
      return flag1;
    bool flag2 = this.MinIntegerDigits.Equals(numberType.MinIntegerDigits);
    if (!flag2)
      return flag2;
    bool flag3 = this.Grouping.Equals(numberType.Grouping);
    return !flag3 ? flag3 : flag3;
  }

  internal void Dispose()
  {
    if (this.m_embeddedText == null)
      return;
    this.m_embeddedText = (EmbeddedTextType) null;
  }
}
