// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODF.Base.ODFImplementation.ODFParagraphProperties
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.ODF.Base.ODFImplementation;

internal class ODFParagraphProperties : CommonTableParaProperties
{
  private const int VerticalAlignKey = 0;
  private const int TextAutoSpaceKey = 1;
  private const int PunctuationWrapKey = 2;
  private const int LineBreakKey = 3;
  private const int NumberLinesKey = 4;
  private const int LineNumberKey = 5;
  private const int TapToDistanceKey = 6;
  private const int SnapToLayoutGridKey = 7;
  private const int RegisterTrueKey = 8;
  private const int LineSpacingKey = 9;
  private const int LineHeightAtLeastKey = 10;
  private const int JustifySingleWordKey = 11;
  private const int BorderLineWidthRightKey = 12;
  private const int BorderLineWidthLeftKey = 13;
  private const int BorderLineWidthBottomKey = 14;
  private const int BorderLineWidthTopKey = 15;
  private const int BorderLineWidthKey = 16 /*0x10*/;
  private const int BackgroundTransparancyKey = 17;
  private const int WindowsKey = 18;
  private const int TextIndentKey = 19;
  private const int TextAlignLastKey = 20;
  private const int TextAlignKey = 21;
  private const int PaddingRightKey = 22;
  private const int PaddingLeftKey = 23;
  private const int PaddingTopKey = 24;
  private const int PaddingBottomKey = 25;
  private const int PaddingKey = 26;
  private const int OrphansKey = 27;
  private const int LineHeightKey = 28;
  private const int KeepTogetherKey = 29;
  private const int HyphenationLadderCountKey = 30;
  private const int HyphenationKeepKey = 31 /*0x1F*/;
  private const byte BeforeSpacingKey = 0;
  private const byte AfterSpacingKey = 1;
  private const byte LeftIndentKey = 2;
  private const byte RightIndentKey = 3;
  private HyphenationKeep m_hyphenationKeep;
  private int m_hyphenationLadderCount;
  private KeepTogether m_keepTogether;
  private float m_lineHeight;
  private uint m_orphans;
  private float m_padding;
  private float m_paddingBottom;
  private float m_paddingTop;
  private float m_paddingLeft;
  private float m_paddingRight;
  private TextAlign m_textAlign;
  private TextAlignLast m_textAlignLast;
  private float m_textIndent;
  private uint m_windows;
  private uint m_backgroundTransparancy;
  private float m_borderLineWidth;
  private float m_borderLineWidthTop;
  private float m_borderLineWidthBottom;
  private float m_borderLineWidthLeft;
  private float m_borderLineWidthRight;
  private bool m_fontIndependentLineSpacing;
  private bool m_justifySingleWord;
  private double m_lineHeightAtLeast;
  private double m_lineSpacing;
  private bool m_registerTrue;
  private bool m_snapToLayoutGrid;
  private uint m_tapToDistance;
  private uint m_lineNumber;
  private bool m_numberLines;
  private bool m_lineBreak;
  private PunctuationWrap m_punctuationWrap;
  private TextAutoSpace m_textAutoSpace;
  private Syncfusion.DocIO.ODF.Base.VerticalAlign? m_verticalAlign;
  private bool m_isTab;
  private double m_afterSpacing;
  private double m_beforeSpacing;
  private double m_leftIndent;
  private double m_rightIndent;
  private List<Syncfusion.DocIO.ODFConverter.Base.ODFImplementation.TabStops> m_tabStops;
  internal int m_styleFlag1;
  internal byte m_styleFlag2;

  internal List<Syncfusion.DocIO.ODFConverter.Base.ODFImplementation.TabStops> TabStops
  {
    get
    {
      if (this.m_tabStops == null)
        this.m_tabStops = new List<Syncfusion.DocIO.ODFConverter.Base.ODFImplementation.TabStops>();
      return this.m_tabStops;
    }
    set => this.m_tabStops = value;
  }

  internal Syncfusion.DocIO.ODF.Base.VerticalAlign? VerticalAlign
  {
    get => this.m_verticalAlign;
    set
    {
      this.m_styleFlag1 = (int) ((long) this.m_styleFlag1 & 4294967294L | 1L);
      this.m_verticalAlign = value;
    }
  }

  internal TextAutoSpace TextAutoSpace
  {
    get => this.m_textAutoSpace;
    set
    {
      this.m_styleFlag1 = (int) ((long) this.m_styleFlag1 & 4294967293L | 2L);
      this.m_textAutoSpace = value;
    }
  }

  internal PunctuationWrap PunctuationWrap
  {
    get => this.m_punctuationWrap;
    set
    {
      this.m_styleFlag1 = (int) ((long) this.m_styleFlag1 & 4294967291L | 4L);
      this.m_punctuationWrap = value;
    }
  }

  internal bool LineBreak
  {
    get => this.m_lineBreak;
    set
    {
      this.m_styleFlag1 = (int) ((long) this.m_styleFlag1 & 4294967287L | 8L);
      this.m_lineBreak = value;
    }
  }

  internal bool NumberLines
  {
    get => this.m_numberLines;
    set
    {
      this.m_styleFlag1 = (int) ((long) this.m_styleFlag1 & 4294967279L | 16L /*0x10*/);
      this.m_numberLines = value;
    }
  }

  internal uint LineNumber
  {
    get => this.m_lineNumber;
    set
    {
      this.m_styleFlag1 = (int) ((long) this.m_styleFlag1 & 4294967263L | 32L /*0x20*/);
      this.m_lineNumber = value;
    }
  }

  internal uint TapToDistance
  {
    get => this.m_tapToDistance;
    set
    {
      this.m_styleFlag1 = (int) ((long) this.m_styleFlag1 & 4294967231L | 64L /*0x40*/);
      this.m_tapToDistance = value;
    }
  }

  internal bool SnapToLayoutGrid
  {
    get => this.m_snapToLayoutGrid;
    set
    {
      this.m_styleFlag1 = (int) ((long) this.m_styleFlag1 & 4294967167L | 128L /*0x80*/);
      this.m_snapToLayoutGrid = value;
    }
  }

  internal bool RegisterTrue
  {
    get => this.m_registerTrue;
    set
    {
      this.m_styleFlag1 = (int) ((long) this.m_styleFlag1 & 4294967039L | 256L /*0x0100*/);
      this.m_registerTrue = value;
    }
  }

  internal double LineSpacing
  {
    get => this.m_lineSpacing;
    set
    {
      this.m_styleFlag1 = (int) ((long) this.m_styleFlag1 & 4294966783L | 512L /*0x0200*/);
      this.m_lineSpacing = value;
    }
  }

  internal double LineHeightAtLeast
  {
    get => this.m_lineHeightAtLeast;
    set
    {
      this.m_styleFlag1 = (int) ((long) this.m_styleFlag1 & 4294966271L | 1024L /*0x0400*/);
      this.m_lineHeightAtLeast = value;
    }
  }

  internal bool JustifySingleWord
  {
    get => this.m_justifySingleWord;
    set
    {
      this.m_styleFlag1 = (int) ((long) this.m_styleFlag1 & 4294965247L | 2048L /*0x0800*/);
      this.m_justifySingleWord = value;
    }
  }

  internal float BorderLineWidthRight
  {
    get => this.m_borderLineWidthRight;
    set
    {
      this.m_styleFlag1 = (int) ((long) this.m_styleFlag1 & 4294963199L | 4096L /*0x1000*/);
      this.m_borderLineWidthRight = value;
    }
  }

  internal float BorderLineWidthLeft
  {
    get => this.m_borderLineWidthLeft;
    set
    {
      this.m_styleFlag1 = (int) ((long) this.m_styleFlag1 & 4294959103L | 8192L /*0x2000*/);
      this.m_borderLineWidthLeft = value;
    }
  }

  internal float BorderLineWidthBottom
  {
    get => this.m_borderLineWidthBottom;
    set
    {
      this.m_styleFlag1 = (int) ((long) this.m_styleFlag1 & 4294950911L | 16384L /*0x4000*/);
      this.m_borderLineWidthBottom = value;
    }
  }

  internal float BorderLineWidthTop
  {
    get => this.m_borderLineWidthTop;
    set
    {
      this.m_styleFlag1 = (int) ((long) this.m_styleFlag1 & 4294934527L | 32768L /*0x8000*/);
      this.m_borderLineWidthTop = value;
    }
  }

  internal float BorderLineWidth
  {
    get => this.m_borderLineWidth;
    set
    {
      this.m_styleFlag1 = (int) ((long) this.m_styleFlag1 & 4294901759L | 65536L /*0x010000*/);
      this.m_borderLineWidth = value;
    }
  }

  internal uint BackgroundTransparancy
  {
    get => this.m_backgroundTransparancy;
    set
    {
      this.m_styleFlag1 = (int) ((long) this.m_styleFlag1 & 4294836223L | 131072L /*0x020000*/);
      this.m_backgroundTransparancy = value;
    }
  }

  internal uint Windows
  {
    get => this.m_windows;
    set
    {
      this.m_styleFlag1 = (int) ((long) this.m_styleFlag1 & 4294705151L | 262144L /*0x040000*/);
      this.m_windows = value;
    }
  }

  internal float TextIndent
  {
    get => this.m_textIndent;
    set
    {
      this.m_styleFlag1 = (int) ((long) this.m_styleFlag1 & 4294443007L | 524288L /*0x080000*/);
      this.m_textIndent = value;
    }
  }

  internal TextAlignLast TextAlignLast
  {
    get => this.m_textAlignLast;
    set
    {
      this.m_styleFlag1 = (int) ((long) this.m_styleFlag1 & 4293918719L | 1048576L /*0x100000*/);
      this.m_textAlignLast = value;
    }
  }

  internal TextAlign TextAlign
  {
    get => this.m_textAlign;
    set
    {
      this.m_styleFlag1 = (int) ((long) this.m_styleFlag1 & 4292870143L | 2097152L /*0x200000*/);
      this.m_textAlign = value;
    }
  }

  internal float PaddingRight
  {
    get => this.m_paddingRight;
    set
    {
      this.m_styleFlag1 = (int) ((long) this.m_styleFlag1 & 4290772991L | 4194304L /*0x400000*/);
      this.m_paddingRight = value;
    }
  }

  internal float PaddingLeft
  {
    get => this.m_paddingLeft;
    set
    {
      this.m_styleFlag1 = (int) ((long) this.m_styleFlag1 & 4286578687L | 8388608L /*0x800000*/);
      this.m_paddingLeft = value;
    }
  }

  internal float PaddingTop
  {
    get => this.m_paddingTop;
    set
    {
      this.m_styleFlag1 = (int) ((long) this.m_styleFlag1 & 4278190079L | 16777216L /*0x01000000*/);
      this.m_paddingTop = value;
    }
  }

  internal float PaddingBottom
  {
    get => this.m_paddingBottom;
    set
    {
      this.m_styleFlag1 = (int) ((long) this.m_styleFlag1 & 4261412863L | 33554432L /*0x02000000*/);
      this.m_paddingBottom = value;
    }
  }

  internal float Padding
  {
    get => this.m_padding;
    set
    {
      this.m_styleFlag1 = (int) ((long) this.m_styleFlag1 & 4227858431L | 67108864L /*0x04000000*/);
      this.m_padding = value;
    }
  }

  internal uint Orphans
  {
    get => this.m_orphans;
    set
    {
      this.m_styleFlag1 = (int) ((long) this.m_styleFlag1 & 4160749567L | 134217728L /*0x08000000*/);
      this.m_orphans = value;
    }
  }

  internal float LineHeight
  {
    get => this.m_lineHeight;
    set
    {
      this.m_styleFlag1 = (int) ((long) this.m_styleFlag1 & 4026531839L /*0xEFFFFFFF*/ | 268435456L /*0x10000000*/);
      this.m_lineHeight = value;
    }
  }

  internal KeepTogether KeepTogether
  {
    get => this.m_keepTogether;
    set
    {
      this.m_styleFlag1 = (int) ((long) this.m_styleFlag1 & 3758096383L /*0xDFFFFFFF*/ | 536870912L /*0x20000000*/);
      this.m_keepTogether = value;
    }
  }

  internal int HyphenationLadderCount
  {
    get => this.m_hyphenationLadderCount;
    set
    {
      this.m_styleFlag1 = (int) ((long) this.m_styleFlag1 & 3221225471L /*0xBFFFFFFF*/ | 1073741824L /*0x40000000*/);
      this.m_hyphenationLadderCount = value;
    }
  }

  internal HyphenationKeep HyphenationKeep
  {
    get => this.m_hyphenationKeep;
    set
    {
      this.m_styleFlag1 = this.m_styleFlag1 & int.MaxValue | int.MinValue;
      this.m_hyphenationKeep = value;
    }
  }

  internal bool FontIndependentLineSpacing
  {
    get => this.m_fontIndependentLineSpacing;
    set => this.m_fontIndependentLineSpacing = value;
  }

  internal bool IsTab
  {
    get => this.m_isTab;
    set => this.m_isTab = value;
  }

  internal double BeforeSpacing
  {
    get => this.m_beforeSpacing;
    set
    {
      this.m_styleFlag2 = (byte) ((int) this.m_styleFlag2 & 254 | 1);
      this.m_beforeSpacing = value;
    }
  }

  internal double AfterSpacing
  {
    get => this.m_afterSpacing;
    set
    {
      this.m_styleFlag2 = (byte) ((int) this.m_styleFlag2 & 253 | 2);
      this.m_afterSpacing = value;
    }
  }

  internal double LeftIndent
  {
    get => this.m_leftIndent;
    set
    {
      this.m_styleFlag2 = (byte) ((int) this.m_styleFlag2 & 251 | 4);
      this.m_leftIndent = value;
    }
  }

  internal double RightIndent
  {
    get => this.m_rightIndent;
    set
    {
      this.m_styleFlag2 = (byte) ((int) this.m_styleFlag2 & 247 | 8);
      this.m_rightIndent = value;
    }
  }

  internal bool HasKey(int propertyKey, int flagName)
  {
    return (flagName & (int) Math.Pow(2.0, (double) propertyKey)) >> propertyKey != 0;
  }

  public override bool Equals(object obj)
  {
    if (!(obj is ODFParagraphProperties paragraphProperties))
      return false;
    bool flag = false;
    if (this.HasKey(0, (int) this.m_CommonstyleFlags) && paragraphProperties.HasKey(0, (int) this.m_CommonstyleFlags) && this.BackgroundColor != null)
      flag = this.BackgroundColor.Equals(paragraphProperties.BackgroundColor);
    if (this.HasKey(16 /*0x10*/, this.m_styleFlag1))
      flag = this.BorderLineWidth.Equals(paragraphProperties.BorderLineWidth);
    if (this.HasKey(14, this.m_styleFlag1))
      flag = this.BorderLineWidthBottom.Equals(paragraphProperties.BorderLineWidthBottom);
    if (this.HasKey(13, this.m_styleFlag1))
      flag = this.BorderLineWidthLeft.Equals(paragraphProperties.BorderLineWidthLeft);
    if (this.HasKey(12, this.m_styleFlag1))
      flag = this.BorderLineWidthRight.Equals(paragraphProperties.BorderLineWidthRight);
    if (this.HasKey(15, this.m_styleFlag1))
      flag = this.BorderLineWidthTop.Equals(paragraphProperties.BorderLineWidthTop);
    if (this.HasKey(2, (int) this.m_CommonstyleFlags))
      flag = this.AfterBreak.Equals((object) paragraphProperties.AfterBreak);
    if (this.HasKey(17, this.m_styleFlag1))
      flag = this.BackgroundTransparancy.Equals(paragraphProperties.BackgroundTransparancy);
    if (this.HasKey(1, (int) this.m_CommonstyleFlags))
      flag = this.BeforeBreak.Equals((object) paragraphProperties.BeforeBreak);
    if (this.FontIndependentLineSpacing)
      flag = this.FontIndependentLineSpacing.Equals(paragraphProperties.FontIndependentLineSpacing);
    if (this.HasKey(31 /*0x1F*/, this.m_styleFlag1))
      flag = this.HyphenationKeep.Equals((object) paragraphProperties.HyphenationKeep);
    if (this.HasKey(30, this.m_styleFlag1))
      flag = this.HyphenationLadderCount.Equals(paragraphProperties.HyphenationLadderCount);
    if (this.HasKey(11, this.m_styleFlag1))
      flag = this.JustifySingleWord.Equals(paragraphProperties.JustifySingleWord);
    if (this.HasKey(29, this.m_styleFlag1))
      flag = this.KeepTogether.Equals((object) paragraphProperties.KeepTogether);
    if (this.HasKey(3, (int) this.m_CommonstyleFlags))
      flag = this.KeepWithNext.Equals((object) paragraphProperties.KeepWithNext);
    if (this.HasKey(3, this.m_styleFlag1))
      flag = this.LineBreak.Equals(paragraphProperties.LineBreak);
    if (this.HasKey(28, this.m_styleFlag1))
      flag = this.LineHeight.Equals(paragraphProperties.LineHeight);
    if (this.HasKey(10, this.m_styleFlag1))
      flag = this.LineHeightAtLeast.Equals(paragraphProperties.LineHeightAtLeast);
    if (this.HasKey(5, this.m_styleFlag1))
      flag = this.LineNumber.Equals(paragraphProperties.LineNumber);
    if (this.HasKey(9, this.m_styleFlag1))
      flag = this.LineSpacing.Equals(paragraphProperties.LineSpacing);
    if (this.HasKey(3, (int) this.m_marginFlag))
      flag = this.MarginBottom.Equals(paragraphProperties.MarginBottom);
    if (this.HasKey(0, (int) this.m_marginFlag))
      flag = this.MarginLeft.Equals(paragraphProperties.MarginLeft);
    if (this.HasKey(1, (int) this.m_marginFlag))
      flag = this.MarginRight.Equals(paragraphProperties.MarginRight);
    if (this.HasKey(2, (int) this.m_marginFlag))
      flag = this.MarginTop.Equals(paragraphProperties.MarginTop);
    if (this.HasKey(4, this.m_styleFlag1))
      flag = this.NumberLines.Equals(paragraphProperties.NumberLines);
    if (this.HasKey(27, this.m_styleFlag1))
      flag = this.Orphans.Equals(paragraphProperties.Orphans);
    if (this.HasKey(26, this.m_styleFlag1))
      flag = this.Padding.Equals(paragraphProperties.Padding);
    if (this.HasKey(25, this.m_styleFlag1))
      flag = this.PaddingBottom.Equals(paragraphProperties.PaddingBottom);
    if (this.HasKey(23, this.m_styleFlag1))
      flag = this.PaddingLeft.Equals(paragraphProperties.PaddingLeft);
    if (this.HasKey(25, this.m_styleFlag1))
      flag = this.PaddingRight.Equals(paragraphProperties.PaddingRight);
    if (this.HasKey(24, this.m_styleFlag1))
      flag = this.PaddingTop.Equals(paragraphProperties.PaddingTop);
    if (this.HasKey(5, (int) this.m_CommonstyleFlags))
      flag = this.PageNumber.Equals(paragraphProperties.PageNumber);
    if (this.HasKey(2, this.m_styleFlag1))
      flag = this.PunctuationWrap.Equals((object) paragraphProperties.PunctuationWrap);
    if (this.HasKey(8, this.m_styleFlag1))
      flag = this.RegisterTrue.Equals(paragraphProperties.RegisterTrue);
    if (this.HasKey(4, (int) this.m_CommonstyleFlags))
      flag = this.ShadowType.Equals(paragraphProperties.ShadowType);
    if (this.HasKey(7, this.m_styleFlag1))
      flag = this.SnapToLayoutGrid.Equals(paragraphProperties.SnapToLayoutGrid);
    if (this.HasKey(6, this.m_styleFlag1))
      flag = this.TapToDistance.Equals(paragraphProperties.TapToDistance);
    if (this.HasKey(21, this.m_styleFlag1))
      flag = this.TextAlign.Equals((object) paragraphProperties.TextAlign);
    if (this.HasKey(20, this.m_styleFlag1))
      flag = this.TextAlignLast.Equals((object) paragraphProperties.TextAlignLast);
    if (this.HasKey(1, this.m_styleFlag1))
      flag = this.TextAutoSpace.Equals((object) paragraphProperties.TextAutoSpace);
    if (this.HasKey(19, this.m_styleFlag1))
      flag = this.TextIndent.Equals(paragraphProperties.TextIndent);
    if (this.HasKey(0, this.m_styleFlag1))
      flag = this.VerticalAlign.Equals((object) paragraphProperties.VerticalAlign);
    if (this.HasKey(18, this.m_styleFlag1))
      flag = this.Windows.Equals(paragraphProperties.Windows);
    if (this.HasKey(0, (int) this.m_CommonstyleFlags))
      flag = this.WritingMode.Equals((object) paragraphProperties.WritingMode);
    if (this.HasKey(1, (int) this.m_styleFlag2))
      flag = this.AfterSpacing.Equals(paragraphProperties.AfterSpacing);
    if (this.HasKey(0, (int) this.m_styleFlag2))
      flag = this.BeforeSpacing.Equals(paragraphProperties.BeforeSpacing);
    if (this.HasKey(2, (int) this.m_styleFlag2))
      flag = this.LeftIndent.Equals(paragraphProperties.LeftIndent);
    if (this.HasKey(3, (int) this.m_styleFlag2))
      flag = this.RightIndent.Equals(paragraphProperties.RightIndent);
    if (this.HasKey(0, (int) this.m_marginFlag))
      flag = this.MarginLeft.Equals(paragraphProperties.MarginLeft);
    if (this.HasKey(1, (int) this.m_marginFlag))
      flag = this.MarginRight.Equals(paragraphProperties.MarginRight);
    if (this.HasKey(2, (int) this.m_marginFlag))
      flag = this.MarginTop.Equals(paragraphProperties.MarginTop);
    if (this.HasKey(3, (int) this.m_marginFlag))
      flag = this.MarginBottom.Equals(paragraphProperties.MarginBottom);
    return flag;
  }

  internal void Close()
  {
    if (this.m_tabStops == null)
      return;
    this.m_tabStops.Clear();
    this.m_tabStops = (List<Syncfusion.DocIO.ODFConverter.Base.ODFImplementation.TabStops>) null;
  }
}
