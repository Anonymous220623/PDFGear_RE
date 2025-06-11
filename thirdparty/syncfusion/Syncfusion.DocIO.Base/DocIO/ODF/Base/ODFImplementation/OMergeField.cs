// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODF.Base.ODFImplementation.OMergeField
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.ODF.Base.ODFImplementation;

internal class OMergeField : OParagraphItem
{
  private string m_fieldName;
  private string m_textBefore;
  private string m_textAfter;
  private string m_text;

  internal string FieldName
  {
    get => this.m_fieldName;
    set => this.m_fieldName = value;
  }

  internal string TextBefore
  {
    get => this.m_textBefore;
    set => this.m_textBefore = value;
  }

  internal string TextAfter
  {
    get => this.m_textAfter;
    set => this.m_textAfter = value;
  }

  internal new string Text
  {
    get => this.m_text;
    set => this.m_text = value;
  }
}
