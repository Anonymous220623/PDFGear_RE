// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODF.Base.ODFImplementation.OHyperlink
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.ODF.Base.ODFImplementation;

internal class OHyperlink : OParagraphItem
{
  private string m_fieldValue;
  private string m_text;

  internal string FieldValue
  {
    get => this.m_fieldValue;
    set => this.m_fieldValue = value;
  }

  internal new string Text
  {
    get => this.m_text;
    set => this.m_text = value;
  }
}
