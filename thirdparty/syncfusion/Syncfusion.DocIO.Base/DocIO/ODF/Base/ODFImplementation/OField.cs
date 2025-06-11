// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODF.Base.ODFImplementation.OField
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Globalization;

#nullable disable
namespace Syncfusion.DocIO.ODF.Base.ODFImplementation;

internal class OField : OParagraphItem
{
  private string m_formattingstring;
  private string m_fieldValue;
  private string m_text;
  private OFieldType m_oFieldType;
  private PageNumberFormat m_pageNumberFormat;
  private CultureInfo m_fieldCulture;

  internal string FormattingString
  {
    get => this.m_formattingstring;
    set => this.m_formattingstring = value;
  }

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

  internal CultureInfo FieldCulture
  {
    get => this.m_fieldCulture;
    set => this.m_fieldCulture = value;
  }

  internal OFieldType OFieldType
  {
    get => this.m_oFieldType;
    set => this.m_oFieldType = value;
  }

  internal PageNumberFormat PageNumberFormat
  {
    get => this.m_pageNumberFormat;
    set => this.m_pageNumberFormat = value;
  }
}
