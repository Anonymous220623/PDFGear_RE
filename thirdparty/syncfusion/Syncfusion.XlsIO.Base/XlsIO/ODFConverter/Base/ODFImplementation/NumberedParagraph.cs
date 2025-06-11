// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ODFConverter.Base.ODFImplementation.NumberedParagraph
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.ODF.Base.ODFImplementation;

#nullable disable
namespace Syncfusion.XlsIO.ODFConverter.Base.ODFImplementation;

internal class NumberedParagraph
{
  private bool m_isContinueNumbering;
  private int m_level;
  private int m_listId;
  private int m_startValue;
  private string m_styleName;
  private ODFParagraphProperties m_paragraphStyle;
  private Heading m_headingStyle;

  internal Heading HeadingStyle
  {
    get => this.m_headingStyle;
    set => this.m_headingStyle = value;
  }

  internal ODFParagraphProperties ParagraphStyle
  {
    get => this.m_paragraphStyle;
    set => this.m_paragraphStyle = value;
  }

  internal string StyleName
  {
    get => this.m_styleName;
    set => this.m_styleName = value;
  }

  internal int StartValue
  {
    get => this.m_startValue;
    set => this.m_startValue = value;
  }

  internal int ListId
  {
    get => this.m_listId;
    set => this.m_listId = value;
  }

  internal int Level
  {
    get => this.m_level;
    set => this.m_level = value;
  }

  internal bool IsContinueNumbering
  {
    get => this.m_isContinueNumbering;
    set => this.m_isContinueNumbering = value;
  }
}
