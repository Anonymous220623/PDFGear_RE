// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.TemplateMarkers.MarkerOptionsImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.TemplateMarkers;

public class MarkerOptionsImpl
{
  private MarkerDirection m_direction;
  private int m_iMarkerIndex;
  private IWorkbook m_book;
  private string m_strOriginalMarker;
  private bool m_isMergeApplied;

  public MarkerOptionsImpl(IWorkbook book)
  {
    this.m_book = book != null ? book : throw new ArgumentNullException(nameof (book));
  }

  public MarkerDirection Direction
  {
    get => this.m_direction;
    set => this.m_direction = value;
  }

  public int MarkerIndex
  {
    get => this.m_iMarkerIndex;
    set => this.m_iMarkerIndex = value;
  }

  public IWorkbook Workbook => this.m_book;

  public string OriginalMarker
  {
    get => this.m_strOriginalMarker;
    set => this.m_strOriginalMarker = value;
  }

  internal bool IsMergeApplied
  {
    get => this.m_isMergeApplied;
    set => this.m_isMergeApplied = value;
  }
}
