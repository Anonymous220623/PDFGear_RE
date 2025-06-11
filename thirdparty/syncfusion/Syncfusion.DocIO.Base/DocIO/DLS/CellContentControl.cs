// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.CellContentControl
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class CellContentControl : WTextBody, ICellContentControl
{
  private ContentControlProperties m_contentControlProperties;
  private WCharacterFormat m_BreakCharacterFormat;
  private WTableCell m_ownerCell;
  private WTableCell m_mappedCell;

  internal WTableCell MappedCell
  {
    get => this.m_mappedCell;
    set => this.m_mappedCell = value;
  }

  internal WTableCell OwnerCell
  {
    get => this.m_ownerCell;
    set => this.m_ownerCell = value;
  }

  public ContentControlProperties ContentControlProperties
  {
    get => this.m_contentControlProperties;
    set => this.m_contentControlProperties = value;
  }

  public WCharacterFormat BreakCharacterFormat
  {
    get => this.m_BreakCharacterFormat;
    set => this.m_BreakCharacterFormat = value;
  }

  public CellContentControl(WordDocument document)
    : base(document, (Entity) null)
  {
    this.m_contentControlProperties = new ContentControlProperties(document, (Entity) this);
    this.m_BreakCharacterFormat = new WCharacterFormat((IWordDocument) document);
  }

  internal new void Close()
  {
    if (this.m_contentControlProperties != null)
    {
      this.m_contentControlProperties.Close();
      this.m_contentControlProperties = (ContentControlProperties) null;
    }
    if (this.m_BreakCharacterFormat == null)
      return;
    this.m_BreakCharacterFormat.Close();
    this.m_BreakCharacterFormat = (WCharacterFormat) null;
  }
}
