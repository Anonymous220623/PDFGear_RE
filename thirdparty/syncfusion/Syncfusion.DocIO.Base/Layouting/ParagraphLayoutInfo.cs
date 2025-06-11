// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.ParagraphLayoutInfo
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Layouting;

internal class ParagraphLayoutInfo : LayoutInfo, ILayoutSpacingsInfo
{
  private byte m_bFlags = 10;
  private float m_topMargin;
  private float m_bottomMargin;
  private float m_topPadding;
  private float m_bottomPadding;
  private int m_levelNumber = -1;
  private HAlignment m_justification;
  private float m_firstLineIndent;
  private float m_listTab;
  private float m_yPosition;
  private float m_pargaraphOriginalYPosition = float.MinValue;
  private float m_listTabWidth;
  private List<float> m_listYPositions;
  private string m_listValue = string.Empty;
  private WCharacterFormat m_characterFormat;
  private ListNumberAlignment m_listAlignment;
  private TabsLayoutInfo.LayoutTab m_listTabStop;
  private float m_xPosition;
  private ListType m_listType;
  private Spacings m_paddings;
  private Spacings m_margins;
  private SyncFont m_listfont;
  private bool m_skipTopBorder;
  private bool m_skipBottomBorder;
  private bool m_skipLeftBorder;
  private bool m_skipRightBorder;
  private bool m_skipHorizontalBorder;

  internal bool SkipTopBorder
  {
    get => this.m_skipTopBorder;
    set => this.m_skipTopBorder = value;
  }

  internal bool SkipBottomBorder
  {
    get => this.m_skipBottomBorder;
    set => this.m_skipBottomBorder = value;
  }

  internal bool SkipLeftBorder
  {
    get => this.m_skipLeftBorder;
    set => this.m_skipLeftBorder = value;
  }

  internal bool SkipRightBorder
  {
    get => this.m_skipRightBorder;
    set => this.m_skipRightBorder = value;
  }

  internal bool SkipHorizonatalBorder
  {
    get => this.m_skipHorizontalBorder;
    set => this.m_skipHorizontalBorder = value;
  }

  public bool IsPageBreak
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  public int LevelNumber
  {
    get => this.m_levelNumber;
    set => this.m_levelNumber = value;
  }

  public HAlignment Justification
  {
    get => this.m_justification;
    set => this.m_justification = value;
  }

  public float FirstLineIndent
  {
    get => this.m_firstLineIndent;
    set => this.m_firstLineIndent = value;
  }

  public bool IsKeepTogether
  {
    get => ((int) this.m_bFlags & 8) >> 3 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 247 | (value ? 1 : 0) << 3);
  }

  public float ListTab
  {
    get => this.m_listTab;
    set => this.m_listTab = value;
  }

  internal float ListTabWidth
  {
    get => this.m_listTabWidth;
    set => this.m_listTabWidth = value;
  }

  internal float YPosition
  {
    get => this.m_yPosition;
    set => this.m_yPosition = value;
  }

  internal float PargaraphOriginalYPosition
  {
    get => this.m_pargaraphOriginalYPosition;
    set => this.m_pargaraphOriginalYPosition = value;
  }

  internal List<float> ListYPositions
  {
    get => this.m_listYPositions ?? (this.m_listYPositions = new List<float>());
  }

  public string ListValue
  {
    get => this.m_listValue;
    set => this.m_listValue = value;
  }

  public ListType CurrentListType
  {
    get => this.m_listType;
    set => this.m_listType = value;
  }

  public WCharacterFormat CharacterFormat
  {
    get => this.m_characterFormat;
    set => this.m_characterFormat = value;
  }

  public ListNumberAlignment ListAlignment
  {
    get => this.m_listAlignment;
    set => this.m_listAlignment = value;
  }

  public TabsLayoutInfo.LayoutTab ListTabStop
  {
    get => this.m_listTabStop;
    set => this.m_listTabStop = value;
  }

  public bool IsFirstLine
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 253 | (value ? 1 : 0) << 1);
  }

  public float TopPadding
  {
    get => this.m_topPadding;
    set => this.m_topPadding = value;
  }

  public float BottomPadding
  {
    get => this.m_bottomPadding;
    set => this.m_bottomPadding = value;
  }

  public float TopMargin
  {
    get => this.m_topMargin;
    set => this.m_topMargin = value;
  }

  public float BottomMargin
  {
    get => this.m_bottomMargin;
    set => this.m_bottomMargin = value;
  }

  public bool IsNotFitted
  {
    get => ((int) this.m_bFlags & 4) >> 2 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 251 | (value ? 1 : 0) << 2);
  }

  internal bool IsXPositionReUpdate
  {
    get => ((int) this.m_bFlags & 32 /*0x20*/) >> 5 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 223 | (value ? 1 : 0) << 5);
  }

  internal float XPosition
  {
    get => this.m_xPosition;
    set => this.m_xPosition = value;
  }

  internal SyncFont ListFont
  {
    get => this.m_listfont;
    set => this.m_listfont = value;
  }

  internal bool IsSectionEndMark
  {
    get => ((int) this.m_bFlags & 16 /*0x10*/) >> 4 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 239 | (value ? 1 : 0) << 4);
  }

  public Spacings Paddings
  {
    get
    {
      if (this.m_paddings == null)
        this.m_paddings = new Spacings();
      return this.m_paddings;
    }
  }

  public Spacings Margins
  {
    get
    {
      if (this.m_margins == null)
        this.m_margins = new Spacings();
      return this.m_margins;
    }
  }

  public ParagraphLayoutInfo(ChildrenLayoutDirection childLayoutDirection)
    : base(childLayoutDirection)
  {
  }

  public ParagraphLayoutInfo(ChildrenLayoutDirection childLayoutDirection, bool isPageBreak)
    : this(childLayoutDirection)
  {
    this.IsPageBreak = isPageBreak;
  }

  public ParagraphLayoutInfo()
  {
  }

  internal void InitLayoutInfo()
  {
    if (this.m_listYPositions == null)
      return;
    this.m_listYPositions.Clear();
    this.m_listYPositions = (List<float>) null;
  }
}
