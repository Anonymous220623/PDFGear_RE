// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.ConditionalFormattingStyle
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class ConditionalFormattingStyle : Style
{
  private WParagraphFormat m_paragraphFormat;
  private TableStyleCellProperties m_cellProperties;
  private TableStyleRowProperties m_rowProperties;
  private TableStyleTableProperties m_tableProperties;
  private ConditionalFormattingType m_conditionalFormattingType;

  public WParagraphFormat ParagraphFormat => this.m_paragraphFormat;

  public TableStyleCellProperties CellProperties => this.m_cellProperties;

  public TableStyleRowProperties RowProperties => this.m_rowProperties;

  public TableStyleTableProperties TableProperties => this.m_tableProperties;

  public ConditionalFormattingType ConditionalFormattingType => this.m_conditionalFormattingType;

  public override StyleType StyleType => StyleType.TableStyle;

  internal ConditionalFormattingStyle(ConditionalFormattingType conditionCode, IWordDocument doc)
    : base((WordDocument) doc)
  {
    this.m_conditionalFormattingType = conditionCode;
    this.m_paragraphFormat = new WParagraphFormat((IWordDocument) this.Document);
    this.m_paragraphFormat.SetOwner((OwnerHolder) this);
    this.m_cellProperties = new TableStyleCellProperties((IWordDocument) this.Document);
    this.m_cellProperties.SetOwner((OwnerHolder) this);
    this.m_rowProperties = new TableStyleRowProperties((IWordDocument) this.Document);
    this.m_rowProperties.SetOwner((OwnerHolder) this);
    this.m_tableProperties = new TableStyleTableProperties((IWordDocument) this.Document);
    this.m_tableProperties.SetOwner((OwnerHolder) this);
  }

  public override IStyle Clone() => (IStyle) this.CloneImpl();

  protected override object CloneImpl()
  {
    ConditionalFormattingStyle owner = (ConditionalFormattingStyle) base.CloneImpl();
    owner.m_paragraphFormat = new WParagraphFormat((IWordDocument) this.Document);
    owner.m_paragraphFormat.ImportContainer((FormatBase) this.ParagraphFormat);
    owner.m_paragraphFormat.SetOwner((OwnerHolder) owner);
    owner.m_cellProperties = new TableStyleCellProperties((IWordDocument) this.Document);
    owner.m_cellProperties.ImportContainer((FormatBase) this.CellProperties);
    owner.m_cellProperties.SetOwner((OwnerHolder) this);
    owner.m_rowProperties = new TableStyleRowProperties((IWordDocument) this.Document);
    owner.m_rowProperties.ImportContainer((FormatBase) this.RowProperties);
    owner.m_rowProperties.SetOwner((OwnerHolder) this);
    owner.m_tableProperties = new TableStyleTableProperties((IWordDocument) this.Document);
    owner.m_tableProperties.ImportContainer((FormatBase) this.TableProperties);
    owner.m_tableProperties.SetOwner((OwnerHolder) this);
    return (object) owner;
  }

  internal override void Close()
  {
    base.Close();
    if (this.m_paragraphFormat != null)
    {
      this.m_paragraphFormat.Close();
      this.m_paragraphFormat = (WParagraphFormat) null;
    }
    if (this.m_cellProperties != null)
    {
      this.m_cellProperties.Close();
      this.m_cellProperties = (TableStyleCellProperties) null;
    }
    if (this.m_rowProperties != null)
    {
      this.m_rowProperties.Close();
      this.m_rowProperties = (TableStyleRowProperties) null;
    }
    if (this.m_tableProperties == null)
      return;
    this.m_tableProperties.Close();
    this.m_tableProperties = (TableStyleTableProperties) null;
  }
}
