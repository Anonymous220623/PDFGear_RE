// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ODF.Base.ODFImplementation.DefaultStyle
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.ODF.Base.ODFImplementation;

internal class DefaultStyle
{
  internal const byte MapKey = 0;
  internal const byte ParagraphPropertiesKey = 1;
  internal const byte TableCellPropertiesKey = 2;
  internal const byte TableColumnPropertiesKey = 3;
  internal const byte TablePropertiesKey = 4;
  internal const byte TableRowPropertiesKey = 5;
  internal const byte TextPropertiesKey = 6;
  internal const byte SectionPropertykey = 7;
  private ODFFontFamily m_family;
  private MapStyle m_map;
  private ODFParagraphProperties m_paragraphProperties;
  private OTableCellProperties m_tableCellProperties;
  private OTableColumnProperties m_tableColumnProperties;
  private OTableProperties m_tableProperties;
  private OTableRowProperties m_tableRowProperties;
  private TextProperties m_textProperties;
  private SectionProperties m_sectionProperties;
  private GraphicProperties m_graphicProperties;
  private string m_name;
  internal byte StylePropFlag;

  internal string Name
  {
    get => this.m_name;
    set => this.m_name = value;
  }

  internal ODFFontFamily Family
  {
    get => this.m_family;
    set => this.m_family = value;
  }

  internal MapStyle Map
  {
    get => this.m_map;
    set
    {
      this.StylePropFlag = (byte) ((int) this.StylePropFlag & 254 | 1);
      this.m_map = value;
    }
  }

  internal ODFParagraphProperties ParagraphProperties
  {
    get
    {
      if (this.m_paragraphProperties == null)
        this.m_paragraphProperties = new ODFParagraphProperties();
      return this.m_paragraphProperties;
    }
    set
    {
      this.StylePropFlag = (byte) ((int) this.StylePropFlag & 253 | 2);
      this.m_paragraphProperties = value;
    }
  }

  internal SectionProperties ODFSectionProperties
  {
    get
    {
      if (this.m_sectionProperties == null)
        this.m_sectionProperties = new SectionProperties();
      return this.m_sectionProperties;
    }
    set
    {
      this.StylePropFlag = (byte) ((int) this.StylePropFlag & (int) sbyte.MaxValue | 128 /*0x80*/);
      this.m_sectionProperties = value;
    }
  }

  internal OTableCellProperties TableCellProperties
  {
    get => this.m_tableCellProperties;
    set
    {
      this.StylePropFlag = (byte) ((int) this.StylePropFlag & 251 | 4);
      this.m_tableCellProperties = value;
    }
  }

  internal OTableColumnProperties TableColumnProperties
  {
    get => this.m_tableColumnProperties;
    set
    {
      this.StylePropFlag = (byte) ((int) this.StylePropFlag & 247 | 8);
      this.m_tableColumnProperties = value;
    }
  }

  internal OTableProperties TableProperties
  {
    get => this.m_tableProperties;
    set
    {
      this.StylePropFlag = (byte) ((int) this.StylePropFlag & 239 | 16 /*0x10*/);
      this.m_tableProperties = value;
    }
  }

  internal OTableRowProperties TableRowProperties
  {
    get => this.m_tableRowProperties;
    set
    {
      this.StylePropFlag = (byte) ((int) this.StylePropFlag & 223 | 32 /*0x20*/);
      this.m_tableRowProperties = value;
    }
  }

  internal TextProperties Textproperties
  {
    get => this.m_textProperties;
    set
    {
      this.StylePropFlag = (byte) ((int) this.StylePropFlag & 191 | 64 /*0x40*/);
      this.m_textProperties = value;
    }
  }

  internal GraphicProperties GraphicProperties
  {
    get => this.m_graphicProperties;
    set => this.m_graphicProperties = value;
  }

  internal void Dispose()
  {
    if (this.m_map != null)
      this.m_map = (MapStyle) null;
    if (this.m_paragraphProperties != null)
    {
      this.m_paragraphProperties.Close();
      this.m_paragraphProperties = (ODFParagraphProperties) null;
    }
    if (this.m_tableCellProperties != null)
      this.m_tableCellProperties = (OTableCellProperties) null;
    if (this.m_tableColumnProperties != null)
      this.m_tableColumnProperties = (OTableColumnProperties) null;
    if (this.m_tableRowProperties != null)
      this.m_tableRowProperties = (OTableRowProperties) null;
    if (this.m_tableProperties != null)
      this.m_tableProperties = (OTableProperties) null;
    if (this.m_textProperties != null)
      this.m_textProperties = (TextProperties) null;
    if (this.m_sectionProperties == null)
      return;
    this.m_sectionProperties.Close();
    this.m_sectionProperties = (SectionProperties) null;
  }
}
