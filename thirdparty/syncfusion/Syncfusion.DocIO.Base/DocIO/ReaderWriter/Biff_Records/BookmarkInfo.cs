// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.BookmarkInfo
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

internal class BookmarkInfo
{
  private string m_strName;
  private int m_iStartPos;
  private int m_iEndPos;
  private bool m_isCellGroup;
  private short m_startCell = -1;
  private short m_endCell = -1;
  private int m_bookmarkIndex;

  internal BookmarkInfo(
    string name,
    int startPos,
    int endPos,
    bool isCellGroup,
    short startCellIndex,
    short endCellIndex)
  {
    this.m_strName = name;
    this.m_iStartPos = startPos;
    this.m_iEndPos = endPos;
    if (!isCellGroup)
      return;
    this.m_isCellGroup = isCellGroup;
    this.m_startCell = startCellIndex;
    this.m_endCell = (short) ((int) endCellIndex - 1);
  }

  internal string Name
  {
    get => this.m_strName;
    set => this.m_strName = value;
  }

  internal int EndPos
  {
    get => this.m_iEndPos;
    set => this.m_iEndPos = value;
  }

  internal int StartPos
  {
    get => this.m_iStartPos;
    set => this.m_iStartPos = value;
  }

  internal bool IsCellGroupBookmark => this.m_isCellGroup;

  internal short StartCellIndex
  {
    get => this.m_startCell;
    set => this.m_startCell = value;
  }

  internal short EndCellIndex
  {
    get => this.m_endCell;
    set => this.m_endCell = value;
  }

  internal int Index
  {
    get => this.m_bookmarkIndex;
    set => this.m_bookmarkIndex = value;
  }

  internal BookmarkInfo Clone() => (BookmarkInfo) this.MemberwiseClone();
}
