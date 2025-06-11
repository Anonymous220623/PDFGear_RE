// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODFConverter.Base.ODFImplementation.Heading
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ODF.Base.ODFImplementation;

#nullable disable
namespace Syncfusion.DocIO.ODFConverter.Base.ODFImplementation;

internal class Heading : ODFParagraphProperties
{
  private string m_classNames;
  private string m_condStyleName;
  private int m_id;
  private bool m_isListHeader;
  private int m_outlineLevel;
  private bool m_restartNumbering;
  private uint m_startValue;
  private string m_styleName;

  internal string StyleName
  {
    get => this.m_styleName;
    set => this.m_styleName = value;
  }

  internal uint StartValue
  {
    get => this.m_startValue;
    set => this.m_startValue = value;
  }

  internal bool RestartNumbering
  {
    get => this.m_restartNumbering;
    set => this.m_restartNumbering = value;
  }

  internal int OutlineLevel
  {
    get => this.m_outlineLevel;
    set => this.m_outlineLevel = value;
  }

  internal bool IsListHeader
  {
    get => this.m_isListHeader;
    set => this.m_isListHeader = value;
  }

  internal int Id
  {
    get => this.m_id;
    set => this.m_id = value;
  }

  internal string CondStyleName
  {
    get => this.m_condStyleName;
    set => this.m_condStyleName = value;
  }

  internal string ClassNames
  {
    get => this.m_classNames;
    set => this.m_classNames = value;
  }
}
