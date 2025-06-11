// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.Rendering.DrawingContext
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO.DLS.Rendering;
using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;
using Syncfusion.Layouting;
using Syncfusion.Office;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.DocIO.Rendering;

internal class DrawingContext : DocumentLayouter
{
  private const float DEF_SCRIPT_FACTOR = 1.5f;
  private const float DEF_EMBOSS_ENGRAVE_FACTOR = 0.2f;
  private const float DEF_DINOFFC_TEXT_FACTOR = 0.2f;
  private const float DEF_DINOFFC_LISTTEXT_FACTOR = 0.1f;
  private const float DEF_FONT_SIZE = 12f;
  private const float DEF_PICBULLET_MIN_FONT_SIZE = 4f;
  private const float DEF_PICBULLET_SCALE_FACTOR = 10f;
  private const char NONBREAK_HYPHEN = '\u001E';
  private const char Zero_Width_Joiner = '\u200D';
  private const char SOFT_HYPHEN = '\u00AD';
  private const char SPACE = ' ';
  private bool m_enableComplexScript;
  private PrivateFontCollection m_privateFontCollection;
  private Dictionary<string, string> m_fontName;
  private Dictionary<string, Stream> m_privateFontStream;
  private Graphics m_graphics;
  private Graphics m_graphicsBmp;
  private List<Dictionary<string, RectangleF>> m_hyperLinks = new List<Dictionary<string, RectangleF>>();
  internal WParagraph currParagraph;
  internal WTextRange currTextRange;
  internal Hyperlink currHyperlink;
  internal WFieldMark formFieldEnd;
  private WField m_currentField;
  private string m_currentBkName;
  private new ExportBookmarkType m_exportBookmarkType;
  internal RectangleF CurrParagraphBounds = new RectangleF();
  private byte m_bFlag;
  private byte m_bFlag1;
  internal int m_orderIndex = 1;
  internal float m_pageMarginLeft;
  private Dictionary<int, LayoutedWidget> m_overLappedShapeWidgets;
  private FontMetric m_fontmetric;
  private StringFormat m_stringformat;
  internal Stack<RectangleF> ClipBoundsContainer;
  private List<LayoutedWidget> m_editableFormFieldinEMF = new List<LayoutedWidget>();
  private RectangleF m_editableTextFormBounds = new RectangleF();
  private RectangleF m_rotateTransform = new RectangleF();
  private string m_editableTextFormText = "";
  private WTextRange m_editableTextFormTextRange;
  private int m_lastTextRangeIndex;
  private Dictionary<int, int> autoTagIndex;
  private string m_commentId;
  private List<PointF[]> m_commentMarks;
  internal List<KeyValuePair<string, bool>> m_previousLineCommentStartMarks;
  private int autoTagCount;
  [ThreadStatic]
  private static List<BookmarkPosition> m_bookmarks;
  private int currParagraphIndex = -1;
  private Dictionary<WCharacterFormat, List<RectangleF>> underLineValues;
  private Dictionary<WCharacterFormat, RectangleF> strikeThroughValues;

  private bool IsListCharacter
  {
    get => ((int) this.m_bFlag & 1) != 0;
    set => this.m_bFlag = (byte) ((int) this.m_bFlag & 254 | (value ? 1 : 0));
  }

  internal bool RecreateNestedMetafile
  {
    get => ((int) this.m_bFlag & 4) >> 2 != 0;
    set => this.m_bFlag = (byte) ((int) this.m_bFlag & 251 | (value ? 1 : 0) << 2);
  }

  internal Dictionary<int, int> AutoTagIndex
  {
    get
    {
      if (this.autoTagIndex == null)
        this.autoTagIndex = new Dictionary<int, int>();
      return this.autoTagIndex;
    }
  }

  internal new List<LayoutedWidget> EditableFormFieldinEMF
  {
    get => this.m_editableFormFieldinEMF;
    set => this.m_editableFormFieldinEMF = value;
  }

  internal WField CurrentRefField
  {
    get => this.m_currentField;
    set => this.m_currentField = value;
  }

  internal string CurrentBookmarkName
  {
    get => this.m_currentBkName;
    set => this.m_currentBkName = value;
  }

  internal new ExportBookmarkType ExportBookmarks
  {
    get => this.m_exportBookmarkType;
    set => this.m_exportBookmarkType = value;
  }

  internal bool EnableComplexScript
  {
    get => this.m_enableComplexScript;
    set => this.m_enableComplexScript = value;
  }

  internal PrivateFontCollection PrivateFonts
  {
    get => this.m_privateFontCollection;
    set
    {
      if (value == null)
        return;
      this.m_privateFontCollection = value;
    }
  }

  internal Dictionary<string, string> FontNames
  {
    get => this.m_fontName;
    set => this.m_fontName = value;
  }

  public Dictionary<string, Stream> FontStreams
  {
    get => this.m_privateFontStream;
    set
    {
      if (value == null)
        return;
      this.m_privateFontStream = value;
    }
  }

  public Graphics Graphics
  {
    get => this.m_graphics;
    set
    {
      if (this.m_graphics == value)
        return;
      this.m_graphics = value;
    }
  }

  internal Graphics GraphicsBmp
  {
    get
    {
      if (this.m_graphicsBmp == null)
      {
        Bitmap bitmap = this.CreateBitmap(1, 1);
        this.m_graphicsBmp = this.GetGraphicsFromImage(bitmap);
        bitmap.SetResolution(120f, 120f);
        this.m_graphicsBmp.PageUnit = GraphicsUnit.Point;
      }
      return this.m_graphicsBmp;
    }
  }

  internal List<Dictionary<string, RectangleF>> Hyperlinks => this.m_hyperLinks;

  internal static List<Dictionary<string, DocumentLayouter.BookmarkHyperlink>> BookmarkHyperlinksList
  {
    get => DocumentLayouter.BookmarkHyperlinks;
  }

  internal new static List<BookmarkPosition> Bookmarks
  {
    get
    {
      if (DrawingContext.m_bookmarks == null)
        DrawingContext.m_bookmarks = new List<BookmarkPosition>();
      return DrawingContext.m_bookmarks;
    }
  }

  internal Dictionary<int, LayoutedWidget> OverLappedShapeWidgets
  {
    get
    {
      if (this.m_overLappedShapeWidgets == null)
        this.m_overLappedShapeWidgets = new Dictionary<int, LayoutedWidget>();
      return this.m_overLappedShapeWidgets;
    }
  }

  public FontMetric FontMetric
  {
    get
    {
      if (this.m_fontmetric == null)
        this.m_fontmetric = new FontMetric();
      return this.m_fontmetric;
    }
  }

  public StringFormat StringFormt
  {
    get
    {
      if (this.m_stringformat == null)
      {
        this.m_stringformat = new StringFormat(StringFormat.GenericTypographic);
        this.m_stringformat.FormatFlags &= ~StringFormatFlags.LineLimit;
        this.m_stringformat.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
        this.m_stringformat.FormatFlags |= StringFormatFlags.NoClip;
        this.m_stringformat.Trimming = StringTrimming.Word;
      }
      return this.m_stringformat;
    }
  }

  internal bool EmbedFonts
  {
    get => ((int) this.m_bFlag & 8) >> 3 != 0;
    set => this.m_bFlag = (byte) ((int) this.m_bFlag & 247 | (value ? 1 : 0) << 3);
  }

  internal new bool PreserveOleEquationAsBitmap
  {
    get => ((int) this.m_bFlag & 64 /*0x40*/) >> 6 != 0;
    set => this.m_bFlag = (byte) ((int) this.m_bFlag & 191 | (value ? 1 : 0) << 6);
  }

  internal bool EmbedCompleteFonts
  {
    get => ((int) this.m_bFlag & 16 /*0x10*/) >> 4 != 0;
    set => this.m_bFlag = (byte) ((int) this.m_bFlag & 239 | (value ? 1 : 0) << 4);
  }

  internal bool AutoTag
  {
    get => ((int) this.m_bFlag1 & 1) != 0;
    set => this.m_bFlag1 = (byte) ((int) this.m_bFlag1 & 254 | (value ? 1 : 0));
  }

  internal DrawingContext()
  {
    Bitmap bitmap = this.CreateBitmap(1, 1);
    this.Graphics = this.GetGraphicsFromImage(bitmap);
    bitmap.SetResolution(120f, 120f);
    this.Graphics.PageUnit = GraphicsUnit.Point;
  }

  internal DrawingContext(Graphics graphics, GraphicsUnit pageUnit)
  {
    this.m_graphics = graphics != null ? graphics : throw new ArgumentException(nameof (Graphics));
    this.m_graphics.PageUnit = pageUnit;
  }

  internal void DrawOverLappedShapeWidgets(bool isHaveToInitLayoutInfo)
  {
    List<int> intList = new List<int>((IEnumerable<int>) this.OverLappedShapeWidgets.Keys);
    intList.Sort();
    int num = 0;
    if (this.AutoTag)
      num = this.autoTagCount;
    foreach (int key in intList)
    {
      if (this.AutoTag)
        this.autoTagCount = this.AutoTagIndex[key];
      this.currParagraph = this.GetOwnerParagraph(this.OverLappedShapeWidgets[key]);
      this.Draw(this.OverLappedShapeWidgets[key], isHaveToInitLayoutInfo);
      if (isHaveToInitLayoutInfo)
        this.OverLappedShapeWidgets[key].InitLayoutInfoAll();
    }
    if (this.AutoTag)
    {
      this.autoTagCount = num;
      this.AutoTagIndex.Clear();
    }
    this.OverLappedShapeWidgets.Clear();
    this.m_orderIndex = 1;
  }

  internal bool IsColorMismatched(IEntity nextsibling, WParagraph paragraph)
  {
    return nextsibling != null && nextsibling is WParagraph && !(nextsibling as WParagraph).ParagraphFormat.BackColor.IsEmpty && (!(nextsibling as WParagraph).ParagraphFormat.Borders.Top.Color.Equals((object) paragraph.ParagraphFormat.Borders.Top.Color) || !(nextsibling as WParagraph).ParagraphFormat.Borders.Bottom.Color.Equals((object) paragraph.ParagraphFormat.Borders.Bottom.Color) || !(nextsibling as WParagraph).ParagraphFormat.Borders.Left.Color.Equals((object) paragraph.ParagraphFormat.Borders.Left.Color) || !(nextsibling as WParagraph).ParagraphFormat.Borders.Right.Color.Equals((object) paragraph.ParagraphFormat.Borders.Right.Color));
  }

  internal void DrawParagraph(WParagraph paragraph, LayoutedWidget ltWidget)
  {
    WListFormat listFormatValue = paragraph.GetListFormatValue();
    this.currTextRange = (WTextRange) null;
    this.CurrParagraphBounds = ltWidget.Bounds;
    if (paragraph.m_layoutInfo is ParagraphLayoutInfo && (paragraph.m_layoutInfo as ParagraphLayoutInfo).IsSectionEndMark)
      return;
    bool isParagraphMarkIsHidden = false;
    if (paragraph.BreakCharacterFormat.Hidden)
      isParagraphMarkIsHidden = true;
    WParagraphFormat paragraphFormat = paragraph.ParagraphFormat;
    bool isEmpty = paragraphFormat.BackColor.IsEmpty;
    if ((paragraphFormat.TextureStyle != TextureStyle.TextureNone || !isEmpty) && !this.IsLinesInteresectWithFloatingItems(ltWidget, true))
    {
      bool resetTransform = false;
      RectangleF paragraphBackGroundColor = this.GetBoundsToDrawParagraphBackGroundColor(paragraph, ltWidget, isParagraphMarkIsHidden, false, ref resetTransform);
      IEntity nextSibling = paragraph.NextSibling;
      Entity ownerEntity = paragraph.GetOwnerEntity();
      if (ownerEntity is WTableCell && (ownerEntity as WTableCell).OwnerRow != null && ((IWidget) (ownerEntity as WTableCell).OwnerRow).LayoutInfo is RowLayoutInfo)
      {
        RowLayoutInfo layoutInfo = ((IWidget) (ownerEntity as WTableCell).OwnerRow).LayoutInfo as RowLayoutInfo;
        if (paragraph.IsInCell && !layoutInfo.IsExactlyRowHeight && (nextSibling == null || !(nextSibling is WParagraph) || !(nextSibling as WParagraph).ParagraphFormat.BackColor.IsEmpty || !ltWidget.IsLastItemInPage))
          paragraphBackGroundColor.Height -= paragraphFormat.AfterSpacing;
      }
      if (this.IsColorMismatched(nextSibling, paragraph))
        paragraphBackGroundColor.Height -= paragraphFormat.AfterSpacing;
      if (paragraphFormat.TextureStyle != TextureStyle.TextureNone)
        this.DrawTextureStyle(paragraphFormat.TextureStyle, paragraphFormat.ForeColor, paragraphFormat.BackColor, paragraphBackGroundColor);
      else if (!isEmpty)
        this.Graphics.FillRectangle((Brush) this.CreateSolidBrush(paragraphFormat.BackColor), paragraphBackGroundColor);
      if (resetTransform)
        this.ResetTransform();
    }
    if (ltWidget.TextTag != "Splitted" && ltWidget.ChildWidgets.Count > 0)
    {
      LayoutedWidget childWidget = ltWidget.ChildWidgets[ltWidget.ChildWidgets.Count - 1];
      if (childWidget.ChildWidgets.Count > 0 && childWidget.ChildWidgets[0].HorizontalAlign == HAlignment.Justify)
      {
        for (int index = 0; index < childWidget.ChildWidgets.Count; ++index)
          childWidget.ChildWidgets[index].IsLastLine = true;
        this.AlignChildWidgets(childWidget, paragraph);
      }
      this.DrawBarTabStop(paragraph, ltWidget);
    }
    if (!paragraphFormat.Borders.NoBorder && listFormatValue != null && listFormatValue.ListType == ListType.NoList)
      this.DrawParagraphBorders(paragraph, paragraphFormat, ltWidget, isParagraphMarkIsHidden);
    else
      this.DrawParagraphBorders(paragraph, paragraphFormat, ltWidget, isParagraphMarkIsHidden);
    if (this.IsParagraphContainingListHasBreak(ltWidget))
      return;
    this.DrawList(paragraph, ltWidget, listFormatValue);
  }

  internal void DrawTextBox(WTextBox textBox, LayoutedWidget ltWidget)
  {
    Color color1 = new Color();
    WTextBoxFormat textBoxFormat = textBox.TextBoxFormat;
    Color color2 = !textBox.IsShape || textBox.Shape.FillFormat.FillType != FillType.FillSolid ? textBoxFormat.FillColor : (!textBox.Shape.FillFormat.Fill || textBox.Shape.FillFormat.IsDefaultFill && !textBox.Shape.IsFillStyleInline || (double) textBox.Shape.FillFormat.Transparency == 100.0 ? Color.Empty : textBox.Shape.FillFormat.Color);
    Borders borders = new Borders();
    float num = textBox.Shape != null ? textBox.Shape.Rotation : textBox.TextBoxFormat.Rotation;
    if (textBox.Owner is GroupShape || textBox.Owner is ChildGroupShape)
    {
      RectangleF clipBounds = ltWidget.Bounds;
      if (textBox.TextBoxFormat.TextDirection != TextDirection.Horizontal && textBox.TextBoxFormat.TextDirection != TextDirection.HorizontalFarEast)
      {
        RectangleF rectangleF = clipBounds;
        rectangleF.Width += rectangleF.Height;
        rectangleF.Height = rectangleF.Width - rectangleF.Height;
        rectangleF.Width -= rectangleF.Height;
        clipBounds = rectangleF;
      }
      this.ClipBoundsContainer.Push((double) textBoxFormat.Rotation == 0.0 ? this.UpdateClipBounds(clipBounds, false) : this.UpdateClipBounds(clipBounds));
    }
    else if ((double) textBoxFormat.Rotation != 0.0)
    {
      RectangleF empty = RectangleF.Empty;
      if ((double) num != 0.0 && textBoxFormat.AutoFit && textBoxFormat.TextWrappingStyle == TextWrappingStyle.Square)
      {
        float height = textBox.TextLayoutingBounds.Height + textBoxFormat.InternalMargin.Top + textBoxFormat.InternalMargin.Bottom;
        RectangleF boundingBoxCoordinates = this.GetBoundingBoxCoordinates(new RectangleF(0.0f, 0.0f, textBoxFormat.Width, height), num);
        ltWidget.Bounds = new RectangleF(ltWidget.Bounds.X - boundingBoxCoordinates.X, ltWidget.Bounds.Y - boundingBoxCoordinates.Y, textBoxFormat.Width, height);
        textBox.TextLayoutingBounds = ltWidget.Bounds;
      }
      else
      {
        RectangleF boundingBoxCoordinates = this.GetBoundingBoxCoordinates(new RectangleF(0.0f, 0.0f, textBoxFormat.Width, textBoxFormat.Height), num);
        ltWidget.Bounds = new RectangleF(ltWidget.Bounds.X - boundingBoxCoordinates.X, ltWidget.Bounds.Y - boundingBoxCoordinates.Y, textBoxFormat.Width, textBoxFormat.Height);
        textBox.TextLayoutingBounds = ltWidget.Bounds;
      }
      if (!textBox.IsShape || !textBox.Shape.TextFrame.Upright || textBoxFormat.AutoFit)
      {
        this.ClipBoundsContainer.Pop();
        this.ClipBoundsContainer.Push(ltWidget.Bounds);
      }
    }
    bool flipHorizontal = textBoxFormat.FlipHorizontal;
    bool flipVertical = textBoxFormat.FlipVertical;
    if (((double) num != 0.0 || (double) num == 0.0 && (flipHorizontal || flipVertical)) && textBoxFormat.TextWrappingStyle != TextWrappingStyle.Tight && textBoxFormat.TextWrappingStyle != TextWrappingStyle.Through)
    {
      if ((double) num > 360.0)
        num %= 360f;
      if ((double) num != 0.0 || flipVertical || flipHorizontal)
        this.Graphics.Transform = this.GetTransformMatrix(ltWidget.Bounds, num, flipHorizontal, flipVertical);
    }
    if (textBoxFormat.FillEfects.Type == BackgroundType.NoBackground)
      color2 = textBoxFormat.TextWrappingStyle != TextWrappingStyle.InFrontOfText ? Color.Transparent : textBoxFormat.FillColor;
    GraphicsPath path = (GraphicsPath) null;
    if (textBoxFormat.VMLPathPoints == null)
    {
      if (textBoxFormat.FillEfects.Type == BackgroundType.Gradient)
      {
        Color color2_1 = textBoxFormat.FillEfects.Gradient.Color2;
        this.DrawTextureStyle(TextureStyle.Texture30Percent, textBoxFormat.TextThemeColor, color2_1, ltWidget.Bounds);
      }
      else
        this.Graphics.FillRectangle((Brush) this.CreateSolidBrush(color2), ltWidget.Bounds);
      if (this.IsTexBoxHaveBackgroundPicture(textBox))
        this.Graphics.DrawImage(this.GetImage(textBoxFormat.FillEfects.Picture), ltWidget.Bounds);
    }
    else
    {
      path = new ShapePath(ltWidget.Bounds, (Dictionary<string, string>) null).GetVMLCustomShapePath(textBoxFormat.VMLPathPoints);
      if (path.PointCount > 0)
        this.Graphics.FillPath((Brush) this.CreateSolidBrush(color2), path);
    }
    borders.Color = textBoxFormat.LineColor;
    borders.BorderType = textBox.GetBordersStyle(textBoxFormat.LineStyle);
    if (!textBoxFormat.NoLine)
    {
      float lineWidth = textBoxFormat.LineWidth;
      if (textBoxFormat.LineStyle == TextBoxLineStyle.Double)
        lineWidth /= 3f;
      else if (textBoxFormat.LineStyle == TextBoxLineStyle.Triple)
        lineWidth /= 5f;
      borders.LineWidth = lineWidth;
      borders.Color = textBoxFormat.LineColor;
      Pen pen = this.GetPen(borders.Left, false);
      if (textBox.Shape != null)
        pen.LineJoin = this.GetLineJoin(textBox.Shape.LineFormat.LineJoin);
      if (path != null && path.PointCount > 0)
        this.Graphics.DrawPath(pen, path);
      else
        this.Graphics.DrawRectangle(pen, ltWidget.Bounds.X, ltWidget.Bounds.Y, ltWidget.Bounds.Width, ltWidget.Bounds.Height);
    }
    this.Graphics.ResetTransform();
    if ((double) num == 0.0 || textBox.Shape == null || !textBox.Shape.TextFrame.Upright)
      return;
    textBox.TextLayoutingBounds = this.GetBoundingBoxCoordinates(ltWidget.Bounds, num);
  }

  private RectangleF UpdateClipBounds(RectangleF clipBounds)
  {
    if (this.ClipBoundsContainer == null)
      this.ClipBoundsContainer = new Stack<RectangleF>();
    if (this.ClipBoundsContainer.Count > 0)
    {
      RectangleF rectangleF = this.ClipBoundsContainer.Peek();
      if ((double) rectangleF.Height < (double) clipBounds.Width)
        clipBounds.Width = rectangleF.Height;
      if ((double) rectangleF.Width < (double) clipBounds.Height)
        clipBounds.Height = rectangleF.Width;
    }
    return clipBounds;
  }

  private void DrawBarTabStop(WParagraph paragraph, LayoutedWidget ltWidget)
  {
    TabCollection tabs = paragraph.ParagraphFormat.Tabs;
    for (int index = 0; index < tabs.Count; ++index)
    {
      if (tabs[index].Justification == Syncfusion.DocIO.DLS.TabJustification.Bar)
      {
        PointF pt1 = new PointF(tabs[index].Position + ltWidget.Bounds.X, ltWidget.Bounds.Y);
        PointF pt2 = new PointF(tabs[index].Position + ltWidget.Bounds.X, ltWidget.Bounds.Bottom + paragraph.ParagraphFormat.AfterSpacing);
        this.Graphics.DrawLine(this.CreatePen(Color.Black), pt1, pt2);
      }
    }
  }

  private RectangleF GetBoundsToDrawParagraphBackGroundColor(
    WParagraph paragraph,
    LayoutedWidget ltWidget,
    bool isParagraphMarkIsHidden,
    bool isLineDrawing,
    ref bool resetTransform)
  {
    RectangleF bounds = ltWidget.Bounds;
    bool flag1 = false;
    bool flag2 = false;
    if (isLineDrawing && ltWidget.Owner.ChildWidgets.Count > 0)
    {
      LayoutedWidget owner = ltWidget.Owner;
      if (owner.ChildWidgets.Count == 1)
      {
        flag1 = true;
        flag2 = true;
      }
      else if (owner.ChildWidgets[0] == ltWidget)
        flag1 = true;
      else if (owner.ChildWidgets[owner.ChildWidgets.Count - 1] == ltWidget)
        flag2 = true;
      ltWidget = owner;
    }
    if (ltWidget.Owner != null && isParagraphMarkIsHidden && (isLineDrawing ? (flag2 ? 1 : 0) : 1) != 0)
      this.AddNextParagraphBounds(ltWidget, ref bounds);
    ParagraphLayoutInfo layoutInfo1 = paragraph.m_layoutInfo as ParagraphLayoutInfo;
    if (ltWidget.ChildWidgets.Count > 0 && Math.Round((double) ltWidget.Bounds.Y, 2) != Math.Round((double) ltWidget.ChildWidgets[0].Bounds.Y, 2) && (isLineDrawing ? (flag1 ? 1 : 0) : 1) != 0 && layoutInfo1 != null)
    {
      bounds.Y = ltWidget.ChildWidgets[0].Bounds.Y - (layoutInfo1.SkipTopBorder ? (layoutInfo1.SkipHorizonatalBorder ? 0.0f : paragraph.ParagraphFormat.Borders.Horizontal.Space) : paragraph.ParagraphFormat.Borders.Top.Space);
      bounds.Height = ltWidget.Bounds.Bottom - bounds.Y;
    }
    bool flag3 = false;
    WParagraph nextSibling1 = paragraph.NextSibling as WParagraph;
    if (layoutInfo1 != null && nextSibling1 != null && !paragraph.ParagraphFormat.Borders.NoBorder && paragraph.IsAdjacentParagraphHaveSameBorders(nextSibling1, layoutInfo1.Margins.Left + ((double) layoutInfo1.FirstLineIndent > 0.0 ? 0.0f : layoutInfo1.FirstLineIndent)) && !ltWidget.IsLastItemInPage && (isLineDrawing ? (flag2 ? 1 : 0) : 1) != 0)
    {
      bounds.Height += layoutInfo1.Margins.Bottom;
      flag3 = true;
    }
    Entity owner1 = paragraph.Owner;
    if (!(owner1 is WSection))
    {
      while (owner1 != null && !(owner1 is WSection) && !(owner1 is WTableCell) && !(owner1 is WTextBox) && !(owner1 is Shape) && !(owner1 is WFootnote))
        owner1 = owner1.Owner;
    }
    float num1 = layoutInfo1.FirstLineIndent;
    if ((double) num1 > 0.0)
      num1 = 0.0f;
    IEntity previousSibling = paragraph.PreviousSibling;
    WParagraph wparagraph = previousSibling is WParagraph ? previousSibling as WParagraph : (WParagraph) null;
    if (wparagraph != null && !wparagraph.ParagraphFormat.Borders.NoBorder && !wparagraph.SectionEndMark && (isLineDrawing ? (flag1 ? 1 : 0) : 1) != 0)
    {
      float num2 = 0.0f;
      if (paragraph.ParagraphFormat.Borders.Top.BorderType == BorderStyle.None && wparagraph.ParagraphFormat.Borders.Bottom.BorderType != BorderStyle.None)
        num2 += wparagraph.ParagraphFormat.Borders.Bottom.LineWidth / 2f;
      bounds.Y += num2;
      bounds.Height -= num2;
    }
    ILayoutSpacingsInfo layoutInfo2 = ltWidget.Widget.LayoutInfo is ILayoutSpacingsInfo ? ltWidget.Widget.LayoutInfo as ILayoutSpacingsInfo : (ILayoutSpacingsInfo) null;
    float num3 = num1;
    float num4 = 0.0f;
    if (layoutInfo2 != null)
    {
      num3 += layoutInfo2.Margins.Left + layoutInfo2.Paddings.Left;
      num4 = layoutInfo2.Margins.Right + layoutInfo2.Paddings.Right;
    }
    float height1 = bounds.Height;
    IEntity nextSibling2 = paragraph.NextSibling;
    if (nextSibling2 != null && nextSibling2 is WParagraph && !(nextSibling2 as WParagraph).ParagraphFormat.BackColor.IsEmpty && !ltWidget.IsLastItemInPage && !flag3 && (isLineDrawing ? (flag2 ? 1 : 0) : 1) != 0)
      height1 += layoutInfo2 != null ? layoutInfo2.Margins.Bottom : 0.0f;
    switch (owner1)
    {
      case WSection _:
        WSection section = owner1 as WSection;
        float num5 = section.PageSetup.ClientWidth;
        float leftMargin = Layouter.GetLeftMargin(section);
        if (section.Columns.Count > 1)
        {
          int columnIndex = this.GetColumnIndex(section, ltWidget.Owner.Bounds);
          if (columnIndex > -1 && columnIndex < section.Columns.Count)
          {
            num5 = section.Columns[columnIndex].Width;
            for (; columnIndex > 0; --columnIndex)
              leftMargin += section.Columns[columnIndex - 1].Width + section.Columns[columnIndex - 1].Space;
          }
        }
        if (!paragraph.ParagraphFormat.IsInFrame())
          return new RectangleF(paragraph.ParagraphFormat.Bidi ? leftMargin - num4 : leftMargin + num3, bounds.Y, num5 - num3 - num4, height1);
        RectangleF itemsRenderingBounds = this.GetInnerItemsRenderingBounds(ltWidget.ChildWidgets[0]);
        float width1 = (double) paragraph.ParagraphFormat.FrameWidth != 0.0 ? paragraph.ParagraphFormat.FrameWidth : ltWidget.Bounds.Width;
        float num6 = paragraph.ParagraphFormat.IsNextParagraphInSameFrame() ? 0.0f : paragraph.ParagraphFormat.Borders.Bottom.GetLineWidthValue();
        float y1 = itemsRenderingBounds.Y;
        return new RectangleF(ltWidget.Bounds.X, y1, width1, ltWidget.Bounds.Height - num6);
      case WTableCell _ when paragraph.IsInCell:
        CellLayoutInfo layoutInfo3 = (owner1 as WTableCell).m_layoutInfo as CellLayoutInfo;
        LayoutedWidget cellLayoutedWidget = this.GetOwnerCellLayoutedWidget(ltWidget);
        if (cellLayoutedWidget == null)
          return bounds;
        float x = bounds.X;
        if ((double) x <= (double) cellLayoutedWidget.Owner.Bounds.X)
          x = cellLayoutedWidget.Owner.Bounds.X + layoutInfo3.Paddings.Left;
        float width2 = (double) bounds.Right <= (double) cellLayoutedWidget.Owner.Bounds.Right ? ((double) bounds.Right <= (double) cellLayoutedWidget.Bounds.Right ? cellLayoutedWidget.Bounds.Right - x : bounds.Right - x) : cellLayoutedWidget.Owner.Bounds.Right - x;
        float y2 = bounds.Y;
        if ((double) y2 < (double) cellLayoutedWidget.Bounds.Top)
          y2 = cellLayoutedWidget.Bounds.Top;
        float height2 = bounds.Bottom - y2;
        if ((double) bounds.Bottom > (double) cellLayoutedWidget.Bounds.Bottom)
          height2 = cellLayoutedWidget.Bounds.Bottom - y2;
        return new RectangleF(x, y2, width2, height2);
      case WTextBox _:
      case Shape _:
        if (ltWidget.Widget.LayoutInfo.IsVerticalText)
        {
          this.ResetTransform();
          resetTransform = true;
          PointF empty = PointF.Empty;
          float rotationAngle = 0.0f;
          this.TransformGraphicsPosition(ltWidget, false, ref empty, ref rotationAngle, paragraph);
        }
        else
        {
          for (LayoutedWidget owner2 = ltWidget.Owner; owner2 != null; owner2 = owner2.Owner)
          {
            if (owner2.Widget is WTextBox || owner2.Widget is Shape)
            {
              RectangleF rectangleF = owner2.Bounds;
              Shape shape = owner1 as Shape;
              WTextBox wtextBox = owner1 as WTextBox;
              rectangleF = new RectangleF(rectangleF.X, rectangleF.Y, rectangleF.Width - (owner1 is Shape ? shape.TextFrame.InternalMargin.Right : wtextBox.TextBoxFormat.InternalMargin.Right), rectangleF.Height - (owner1 is Shape ? shape.TextFrame.InternalMargin.Bottom : wtextBox.TextBoxFormat.InternalMargin.Bottom));
              if ((double) rectangleF.Right > (double) bounds.Right)
                bounds = new RectangleF(bounds.X, bounds.Y, bounds.Width + (rectangleF.Right - bounds.Right), bounds.Height);
              if ((flag2 || !isLineDrawing && paragraph.NextSibling == null) && (double) rectangleF.Bottom > (double) bounds.Bottom)
              {
                bounds = new RectangleF(bounds.X, bounds.Y, bounds.Width, bounds.Height + (rectangleF.Bottom - bounds.Bottom));
                break;
              }
              break;
            }
          }
        }
        return bounds;
      default:
        return bounds;
    }
  }

  internal bool IsParagraphContainingListHasBreak(LayoutedWidget ltWidget)
  {
    if (ltWidget != null && ltWidget.ChildWidgets.Count > 0)
    {
      for (int index = 0; index < ltWidget.ChildWidgets[0].ChildWidgets.Count; ++index)
      {
        if (!(ltWidget.ChildWidgets[0].ChildWidgets[index].Widget is BookmarkStart) && !(ltWidget.ChildWidgets[0].ChildWidgets[index].Widget is BookmarkEnd))
        {
          if (ltWidget.ChildWidgets[0].ChildWidgets[index].Widget is Break && (ltWidget.ChildWidgets[0].ChildWidgets[index].Widget as Break).BreakType != BreakType.LineBreak && !this.GetOwnerParagraph(ltWidget).IsInCell)
            return true;
          break;
        }
      }
    }
    return false;
  }

  private Entity GetBaseEntity(Entity entity)
  {
    Entity baseEntity = entity;
    while (baseEntity.Owner != null)
    {
      baseEntity = baseEntity.Owner;
      switch (baseEntity)
      {
        case WSection _:
        case HeaderFooter _:
        case WTextBox _:
        case Shape _:
        case GroupShape _:
          return baseEntity;
        default:
          continue;
      }
    }
    return baseEntity;
  }

  public int GetTextTopPosition(string text, Font font, SizeF size)
  {
    int textTopPosition = -1;
    Bitmap bitmap = new Bitmap((int) size.Width, (int) size.Height);
    Graphics graphics = Graphics.FromImage((Image) bitmap);
    Brush brush1 = (Brush) new SolidBrush(Color.White);
    graphics.FillRectangle(brush1, new Rectangle(0, 0, (int) size.Width, (int) size.Height));
    Brush brush2 = (Brush) new SolidBrush(Color.Black);
    graphics.DrawString(text, font, brush2, 0.0f, 0.0f, StringFormat.GenericTypographic);
    for (int y = 0; y < (int) size.Height; ++y)
    {
      for (int x = 0; x < (int) size.Width; ++x)
      {
        Color pixel = bitmap.GetPixel(x, y);
        if (pixel.R != byte.MaxValue || pixel.G != byte.MaxValue || pixel.B != byte.MaxValue)
        {
          textTopPosition = y;
          goto label_8;
        }
      }
    }
label_8:
    bitmap.Dispose();
    graphics.Dispose();
    brush2.Dispose();
    return textTopPosition;
  }

  public void UpdateTabPosition(LayoutedWidget widget, RectangleF clientArea)
  {
    if (widget == null)
      return;
    float width1 = 0.0f;
    int num1 = 0;
    bool flag1 = true;
    RectangleF bounds;
    for (int index = 0; index < widget.ChildWidgets.Count; ++index)
    {
      TabsLayoutInfo layoutInfo1 = widget.ChildWidgets[index].Widget.LayoutInfo as TabsLayoutInfo;
      if (widget.ChildWidgets[index].PrevTabJustification == Syncfusion.Layouting.TabJustification.Right && layoutInfo1 == null || widget.ChildWidgets[index].PrevTabJustification == Syncfusion.Layouting.TabJustification.Left && widget.ChildWidgets[index].Widget.LayoutInfo is TabsLayoutInfo)
      {
        if (flag1)
        {
          num1 = index;
          flag1 = false;
          if (widget.ChildWidgets.Count > index - 1 && index != 0)
          {
            TabsLayoutInfo layoutInfo2 = widget.ChildWidgets[index - 1].Widget.LayoutInfo as TabsLayoutInfo;
            int tabEndIndex = this.GetTabEndIndex(widget, num1);
            if (layoutInfo2 != null)
            {
              float pageMarginLeft = (float) layoutInfo2.PageMarginLeft;
              bounds = widget.ChildWidgets[tabEndIndex].Bounds;
              float num2 = bounds.Right - widget.ChildWidgets[num1].Bounds.X;
              WParagraph ownerParagraph = this.GetOwnerParagraph(widget);
              float num3 = (ownerParagraph == null || ownerParagraph.Document.Settings.CompatibilityMode != CompatibilityMode.Word2013 || (double) layoutInfo2.m_currTab.Position + (double) pageMarginLeft <= layoutInfo2.PageMarginRight ? layoutInfo2.m_currTab.Position + pageMarginLeft : (float) layoutInfo2.PageMarginRight - ownerParagraph.ParagraphFormat.RightIndent) - widget.ChildWidgets[num1].Bounds.X;
              if ((double) num2 < (double) num3)
              {
                width1 = num3 - num2;
                widget.ChildWidgets[num1 - 1].Bounds = new RectangleF(widget.ChildWidgets[num1 - 1].Bounds.X, widget.ChildWidgets[num1 - 1].Bounds.Y, width1, widget.ChildWidgets[num1 - 1].Bounds.Height);
                widget.Bounds = new RectangleF(widget.Bounds.X, widget.Bounds.Y, widget.Bounds.Width + width1, widget.Bounds.Height);
              }
              else
              {
                width1 = 0.0f;
                layoutInfo2.TabWidth = 0.0f;
              }
            }
          }
        }
      }
      else
      {
        for (; num1 <= index; ++num1)
        {
          if (widget.ChildWidgets[num1].PrevTabJustification == Syncfusion.Layouting.TabJustification.Right && !(widget.ChildWidgets[num1].Widget.LayoutInfo is TabsLayoutInfo))
            widget.ChildWidgets[num1].Bounds = new RectangleF(widget.ChildWidgets[num1].Bounds.X + width1, widget.ChildWidgets[num1].Bounds.Y, widget.ChildWidgets[num1].Bounds.Width, widget.ChildWidgets[num1].Bounds.Height);
          flag1 = true;
        }
      }
      if (index == widget.ChildWidgets.Count - 1)
      {
        for (; num1 <= index; ++num1)
        {
          if (widget.ChildWidgets[num1].PrevTabJustification == Syncfusion.Layouting.TabJustification.Right && !(widget.ChildWidgets[num1].Widget.LayoutInfo is TabsLayoutInfo))
            widget.ChildWidgets[num1].Bounds = new RectangleF(widget.ChildWidgets[num1].Bounds.X + width1, widget.ChildWidgets[num1].Bounds.Y, widget.ChildWidgets[num1].Bounds.Width, widget.ChildWidgets[num1].Bounds.Height);
          flag1 = true;
        }
        if (widget.ChildWidgets[index].Widget.LayoutInfo is TabsLayoutInfo layoutInfo3 && layoutInfo3.m_currTab.Justification == Syncfusion.Layouting.TabJustification.Right)
          widget.ChildWidgets[index].Bounds = new RectangleF(widget.ChildWidgets[index].Bounds.X, widget.ChildWidgets[index].Bounds.Y, layoutInfo3.m_currTab.Position + (float) layoutInfo3.PageMarginLeft - widget.ChildWidgets[index].Bounds.X, widget.ChildWidgets[index].Bounds.Height);
      }
    }
    bool flag2 = true;
    int num4 = 0;
    for (int index = 0; index < widget.ChildWidgets.Count; ++index)
    {
      TabsLayoutInfo layoutInfo4 = widget.ChildWidgets[index].Widget.LayoutInfo as TabsLayoutInfo;
      if (widget.ChildWidgets[index].PrevTabJustification == Syncfusion.Layouting.TabJustification.Centered && layoutInfo4 == null)
      {
        if (flag2)
        {
          num4 = index;
          flag2 = false;
          if (widget.ChildWidgets.Count > index - 1 && index != 0)
          {
            TabsLayoutInfo layoutInfo5 = widget.ChildWidgets[index - 1].Widget.LayoutInfo as TabsLayoutInfo;
            int tabEndIndex = this.GetTabEndIndex(widget, num4);
            float num5 = (float) (((double) widget.ChildWidgets[tabEndIndex].Bounds.Right - (double) widget.ChildWidgets[num4].Bounds.X) / 2.0);
            if (layoutInfo5 != null)
            {
              float num6 = layoutInfo5.m_currTab.Position + (float) layoutInfo5.PageMarginLeft - widget.ChildWidgets[num4].Bounds.X;
              WParagraph ownerParagraph1 = this.GetOwnerParagraph(widget);
              if (ownerParagraph1 != null && (ownerParagraph1.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013 && (double) layoutInfo5.m_currTab.Position + layoutInfo5.PageMarginLeft > layoutInfo5.PageMarginRight || (double) ownerParagraph1.ParagraphFormat.RightIndent == 0.0 && (double) layoutInfo5.m_currTab.Position + layoutInfo5.PageMarginLeft + (double) num5 > layoutInfo5.PageMarginRight))
              {
                num6 = (float) layoutInfo5.PageMarginRight - ownerParagraph1.ParagraphFormat.RightIndent - widget.ChildWidgets[num4].Bounds.X;
                num5 *= 2f;
              }
              WParagraph ownerParagraph2 = this.GetOwnerParagraph(widget);
              if ((double) num5 < (double) num6 && (ownerParagraph2 == null || ownerParagraph2.ParagraphFormat.GetAlignmentToRender() != HorizontalAlignment.Justify || (double) widget.ChildWidgets[tabEndIndex].Bounds.Right + (double) num6 - (double) num5 < layoutInfo5.PageMarginRight))
              {
                width1 = num6 - num5;
                widget.ChildWidgets[num4 - 1].Bounds = new RectangleF(widget.ChildWidgets[num4 - 1].Bounds.X, widget.ChildWidgets[num4 - 1].Bounds.Y, width1, widget.ChildWidgets[num4 - 1].Bounds.Height);
              }
              else
                layoutInfo5.TabWidth = 0.0f;
            }
          }
        }
      }
      else
      {
        for (; num4 <= index; ++num4)
        {
          if (widget.ChildWidgets[num4].PrevTabJustification == Syncfusion.Layouting.TabJustification.Centered && !(widget.ChildWidgets[num4].Widget.LayoutInfo is TabsLayoutInfo))
            widget.ChildWidgets[num4].Bounds = new RectangleF(widget.ChildWidgets[num4].Bounds.X + width1, widget.ChildWidgets[num4].Bounds.Y, widget.ChildWidgets[num4].Bounds.Width, widget.ChildWidgets[num4].Bounds.Height);
        }
        flag2 = true;
      }
      if (index == widget.ChildWidgets.Count - 1)
      {
        for (; num4 <= index; ++num4)
        {
          if (widget.ChildWidgets[num4].PrevTabJustification == Syncfusion.Layouting.TabJustification.Centered && !(widget.ChildWidgets[num4].Widget.LayoutInfo is TabsLayoutInfo))
            widget.ChildWidgets[num4].Bounds = new RectangleF(widget.ChildWidgets[num4].Bounds.X + width1, widget.ChildWidgets[num4].Bounds.Y, widget.ChildWidgets[num4].Bounds.Width, widget.ChildWidgets[num4].Bounds.Height);
        }
        flag2 = true;
        if (widget.ChildWidgets[index].Widget.LayoutInfo is TabsLayoutInfo layoutInfo6 && layoutInfo6.m_currTab.Justification == Syncfusion.Layouting.TabJustification.Centered)
        {
          LayoutedWidget childWidget = widget.ChildWidgets[index];
          bounds = widget.ChildWidgets[index].Bounds;
          double x1 = (double) bounds.X;
          bounds = widget.ChildWidgets[index].Bounds;
          double y = (double) bounds.Y;
          double num7 = (double) layoutInfo6.m_currTab.Position + layoutInfo6.PageMarginLeft;
          bounds = widget.ChildWidgets[index].Bounds;
          double x2 = (double) bounds.X;
          double width2 = num7 - x2;
          bounds = widget.ChildWidgets[index].Bounds;
          double height = (double) bounds.Height;
          RectangleF rectangleF = new RectangleF((float) x1, (float) y, (float) width2, (float) height);
          childWidget.Bounds = rectangleF;
        }
      }
    }
    this.UpdateDecimalTabPosition(widget, clientArea);
    this.UpdateDecimalTabPositionInCell(widget, clientArea);
  }

  private void UpdateDecimalTabPosition(LayoutedWidget ltWidget, RectangleF clientArea)
  {
    bool flag = false;
    bool isDecimalTab = false;
    int decimalTabStart = 0;
    float num = 0.0f;
    for (int index = 0; index < ltWidget.ChildWidgets.Count; ++index)
    {
      TabsLayoutInfo layoutInfo1 = ltWidget.ChildWidgets[index].Widget.LayoutInfo as TabsLayoutInfo;
      if (ltWidget.ChildWidgets[index].PrevTabJustification == Syncfusion.Layouting.TabJustification.Decimal && layoutInfo1 == null)
      {
        if (flag)
        {
          decimalTabStart = index;
          flag = false;
          isDecimalTab = true;
          num = this.GetWidthToShift(ltWidget, decimalTabStart, false, clientArea);
          ltWidget.ChildWidgets[decimalTabStart - 1].Bounds = new RectangleF(ltWidget.ChildWidgets[decimalTabStart - 1].Bounds.X, ltWidget.ChildWidgets[decimalTabStart - 1].Bounds.Y, num, ltWidget.ChildWidgets[decimalTabStart - 1].Bounds.Height);
        }
      }
      else
      {
        flag = this.IsDecimalTabStart(ltWidget, decimalTabStart, isDecimalTab, index, num, false);
        isDecimalTab = false;
      }
      if (index == ltWidget.ChildWidgets.Count - 1)
      {
        flag = this.IsDecimalTabStart(ltWidget, decimalTabStart, isDecimalTab, index, num, false);
        if (ltWidget.ChildWidgets[index].Widget.LayoutInfo is TabsLayoutInfo layoutInfo2 && layoutInfo2.m_currTab.Justification == Syncfusion.Layouting.TabJustification.Decimal)
          ltWidget.ChildWidgets[index].Bounds = new RectangleF(ltWidget.ChildWidgets[index].Bounds.X, ltWidget.ChildWidgets[index].Bounds.Y, layoutInfo2.m_currTab.Position + (float) layoutInfo2.PageMarginLeft - ltWidget.ChildWidgets[index].Bounds.X, ltWidget.ChildWidgets[index].Bounds.Height);
      }
    }
  }

  private void UpdateDecimalTabPositionInCell(LayoutedWidget ltWidget, RectangleF clientArea)
  {
    int decimalTabStart = 0;
    WParagraph ownerParagraph = this.GetOwnerParagraph(ltWidget);
    if (ownerParagraph == null)
      return;
    for (int index = 0; index < ltWidget.ChildWidgets.Count; ++index)
    {
      if ((!ownerParagraph.IsInCell || ownerParagraph.ParagraphFormat.Tabs.Count != 1 ? 0 : (ownerParagraph.ParagraphFormat.Tabs[0].Justification == Syncfusion.DocIO.DLS.TabJustification.Decimal ? 1 : 0)) != 0)
      {
        float widthToShift = this.GetWidthToShift(ltWidget, decimalTabStart, true, clientArea);
        this.IsDecimalTabStart(ltWidget, index, false, index, widthToShift, true);
      }
    }
  }

  private bool IsDecimalTabStart(
    LayoutedWidget ltWidget,
    int decimalTabStart,
    bool isDecimalTab,
    int i,
    float widthToShift,
    bool isInCell)
  {
    for (; decimalTabStart <= i; ++decimalTabStart)
    {
      if (isInCell)
        ltWidget.ChildWidgets[decimalTabStart].Bounds = new RectangleF(ltWidget.ChildWidgets[decimalTabStart].Bounds.X + widthToShift, ltWidget.ChildWidgets[decimalTabStart].Bounds.Y, ltWidget.ChildWidgets[decimalTabStart].Bounds.Width, ltWidget.ChildWidgets[decimalTabStart].Bounds.Height);
      else if (ltWidget.ChildWidgets[decimalTabStart].PrevTabJustification == Syncfusion.Layouting.TabJustification.Decimal && !(ltWidget.ChildWidgets[decimalTabStart].Widget.LayoutInfo is TabsLayoutInfo) && isDecimalTab)
        ltWidget.ChildWidgets[decimalTabStart].Bounds = new RectangleF(ltWidget.ChildWidgets[decimalTabStart].Bounds.X + widthToShift, ltWidget.ChildWidgets[decimalTabStart].Bounds.Y, ltWidget.ChildWidgets[decimalTabStart].Bounds.Width, ltWidget.ChildWidgets[decimalTabStart].Bounds.Height);
    }
    return true;
  }

  private float GetWidthToShift(
    LayoutedWidget ltWidget,
    int decimalTabStart,
    bool isInCell,
    RectangleF clientArea)
  {
    float widthToShift = 0.0f;
    float num1 = 0.0f;
    int tabEndIndex = this.GetTabEndIndex(ltWidget, decimalTabStart);
    WParagraph ownerParagraph = this.GetOwnerParagraph(ltWidget);
    float leftWidth = this.GetLeftWidth(ltWidget, decimalTabStart, tabEndIndex);
    WParagraphFormat currentTabFormat = this.GetCurrentTabFormat(ownerParagraph);
    float num2 = 0.0f;
    if (currentTabFormat != null && currentTabFormat.Tabs.Count > 0)
      num2 = currentTabFormat.Tabs[0].Position;
    float num3 = 0.0f;
    for (int index = decimalTabStart; index <= tabEndIndex; ++index)
      num1 += ltWidget.ChildWidgets[index].Bounds.Width;
    float num4;
    if (isInCell)
    {
      num4 = clientArea.Width;
      num3 = clientArea.X;
    }
    else
    {
      num4 = (double) this.GetColumnWidth(ownerParagraph) > (double) num2 ? this.GetColumnWidth(ownerParagraph) : 1584f;
      if (ltWidget.ChildWidgets[decimalTabStart - 1].Widget.LayoutInfo is TabsLayoutInfo layoutInfo)
      {
        num3 = (float) layoutInfo.PageMarginLeft;
        num2 = (ownerParagraph == null || ownerParagraph.Document.Settings.CompatibilityMode != CompatibilityMode.Word2013 || (double) layoutInfo.m_currTab.Position + layoutInfo.PageMarginLeft <= layoutInfo.PageMarginRight ? layoutInfo.m_currTab.Position + num3 : (float) layoutInfo.PageMarginRight - ownerParagraph.ParagraphFormat.RightIndent) - ltWidget.ChildWidgets[decimalTabStart].Bounds.X;
      }
    }
    if ((double) leftWidth < (double) num2)
    {
      if ((double) num1 - (double) leftWidth < (double) num4 - (double) num2)
      {
        widthToShift = num2 - leftWidth;
        float num5 = ltWidget.ChildWidgets[decimalTabStart].Bounds.X - num3;
        if (!isInCell && (double) num4 < (double) num5 + (double) num2 + (double) num1 - (double) leftWidth)
        {
          float num6 = num5 + num2 + num1 - leftWidth - num4;
          widthToShift -= num6;
        }
      }
      else
        widthToShift = num4 - num1;
    }
    return widthToShift;
  }

  public WParagraphFormat GetCurrentTabFormat(WParagraph paragraph)
  {
    WParagraphFormat currentTabFormat = paragraph.ParagraphFormat;
    while (currentTabFormat != null && currentTabFormat.Tabs.Count <= 0)
      currentTabFormat = currentTabFormat.BaseFormat as WParagraphFormat;
    return currentTabFormat;
  }

  private float GetColumnWidth(WParagraph paragraph)
  {
    Entity entity = (Entity) paragraph;
    float columnWidth = 0.0f;
    while (true)
    {
      switch (entity)
      {
        case WSection _:
        case null:
          goto label_3;
        default:
          entity = entity.Owner;
          continue;
      }
    }
label_3:
    if (entity is WSection && paragraph.m_layoutInfo is ParagraphLayoutInfo)
      columnWidth = (entity as WSection).PageSetup.ClientWidth - (paragraph.m_layoutInfo as ParagraphLayoutInfo).Margins.Right;
    return columnWidth;
  }

  public float GetLeftWidth(WParagraph paragraph, int decimalTabStart, int decimalTabEnd)
  {
    float leftWidth = 0.0f;
    if (paragraph.ChildEntities.Count != 0)
    {
      int decimalSeparator = 0;
      bool isSeparator = false;
      int decimalseparator = this.GetIndexOfDecimalseparator(paragraph, decimalTabStart, decimalTabEnd, ref leftWidth, ref decimalSeparator, ref isSeparator);
      if (paragraph.ChildEntities[decimalseparator] is WTextRange && isSeparator)
      {
        string[] strArray = (paragraph.ChildEntities[decimalseparator] as WTextRange).Text.Split((char) decimalSeparator);
        SizeF sizeF = this.MeasureTextRange(paragraph.ChildEntities[decimalseparator] as WTextRange, strArray[0]);
        leftWidth += sizeF.Width;
      }
    }
    return leftWidth;
  }

  internal float GetLeftWidth(LayoutedWidget ltWidget, int decimalTabStart, int decimalTabEnd)
  {
    float leftWidth = 0.0f;
    if (ltWidget.ChildWidgets.Count != 0)
    {
      int decimalSeparator = 0;
      bool isSeparator = false;
      int decimalseparator = this.GetIndexOfDecimalseparator(ltWidget, decimalTabStart, decimalTabEnd, ref leftWidth, ref decimalSeparator, ref isSeparator);
      if (ltWidget.ChildWidgets[decimalseparator].Widget is WTextRange && isSeparator)
      {
        string[] strArray = (ltWidget.ChildWidgets[decimalseparator].Widget as WTextRange).Text.Split((char) decimalSeparator);
        SizeF sizeF = this.MeasureTextRange(ltWidget.ChildWidgets[decimalseparator].Widget as WTextRange, strArray[0]);
        leftWidth += sizeF.Width;
      }
      else if (ltWidget.ChildWidgets[decimalseparator].Widget is SplitStringWidget && isSeparator)
      {
        string[] strArray = (ltWidget.ChildWidgets[decimalseparator].Widget as SplitStringWidget).SplittedText.Split((char) decimalSeparator);
        SizeF sizeF = this.MeasureTextRange((ltWidget.ChildWidgets[decimalseparator].Widget as SplitStringWidget).RealStringWidget as WTextRange, strArray[0]);
        leftWidth += sizeF.Width;
      }
    }
    return leftWidth;
  }

  private int GetIndexOfDecimalseparator(
    WParagraph paragraph,
    int decimalTabStart,
    int decimalTabEnd,
    ref float leftWidth,
    ref int decimalSeparator,
    ref bool isSeparator)
  {
    char ch = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];
    bool flag1 = false;
    bool flag2 = false;
    int decimalseparator = 0;
    bool isPrevTextHasNumber = false;
    for (int index1 = decimalTabStart; index1 <= decimalTabEnd; ++index1)
    {
      if (paragraph.ChildEntities[index1] is WTextRange && !(paragraph.ChildEntities[index1] as IWidget).LayoutInfo.IsSkip)
      {
        decimalSeparator = 0;
        char[] charArray = (paragraph.ChildEntities[index1] as WTextRange).Text.ToCharArray();
        for (int index2 = 0; index2 < charArray.Length; ++index2)
        {
          if (char.IsNumber(charArray[index2]))
          {
            flag2 = true;
            break;
          }
        }
        if (flag2)
          flag1 = this.IsDecimalSeparator(charArray, ref decimalSeparator, isPrevTextHasNumber);
        if (!flag2 && !flag1)
        {
          if ((paragraph.ChildEntities[index1] as WTextRange).Text.Contains(ch.ToString()))
            flag1 = true;
          decimalSeparator = (int) ch;
        }
        if (!flag1)
        {
          SizeF size = ((IWidget) (paragraph.ChildEntities[index1] as WTextRange)).LayoutInfo.Size;
          leftWidth += size.Width;
          isPrevTextHasNumber = flag2;
        }
        else
        {
          decimalseparator = index1;
          isSeparator = true;
          break;
        }
      }
    }
    return decimalseparator;
  }

  private int GetIndexOfDecimalseparator(
    LayoutedWidget ltWidget,
    int decimalTabStart,
    int decimalTabEnd,
    ref float leftWidth,
    ref int decimalSeparator,
    ref bool isSeparator)
  {
    char ch1 = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];
    bool flag1 = false;
    bool flag2 = false;
    int decimalseparator = decimalTabStart;
    bool isPrevTextHasNumber = false;
    for (int index1 = decimalTabStart; index1 <= decimalTabEnd; ++index1)
    {
      if (ltWidget.ChildWidgets[index1].Widget is WTextRange || ltWidget.ChildWidgets[index1].Widget is SplitStringWidget)
      {
        bool flag3 = ltWidget.ChildWidgets[index1].Widget is WTextRange;
        decimalSeparator = 0;
        char[] ch2 = flag3 ? (ltWidget.ChildWidgets[index1].Widget as WTextRange).Text.ToCharArray() : (ltWidget.ChildWidgets[index1].Widget as SplitStringWidget).SplittedText.ToCharArray();
        for (int index2 = 0; index2 < ch2.Length; ++index2)
        {
          if (char.IsNumber(ch2[index2]))
          {
            flag2 = true;
            break;
          }
        }
        if (flag2)
          flag1 = this.IsDecimalSeparator(ch2, ref decimalSeparator, isPrevTextHasNumber);
        if (!flag2 && !flag1)
        {
          flag1 = !flag3 ? (ltWidget.ChildWidgets[index1].Widget as SplitStringWidget).SplittedText.Contains(ch1.ToString()) : (ltWidget.ChildWidgets[index1].Widget as WTextRange).Text.Contains(ch1.ToString());
          decimalSeparator = (int) ch1;
        }
        if (!flag1)
        {
          SizeF sizeF = flag3 ? ((IWidget) (ltWidget.ChildWidgets[index1].Widget as WTextRange)).LayoutInfo.Size : this.MeasureTextRange((ltWidget.ChildWidgets[index1].Widget as SplitStringWidget).RealStringWidget as WTextRange, (ltWidget.ChildWidgets[index1].Widget as SplitStringWidget).SplittedText);
          leftWidth += sizeF.Width;
          isPrevTextHasNumber = flag2;
        }
        else
        {
          decimalseparator = index1;
          isSeparator = true;
          break;
        }
      }
    }
    return decimalseparator;
  }

  private bool IsDecimalSeparator(char[] ch, ref int decimalSeparator, bool isPrevTextHasNumber)
  {
    int num1 = 8217;
    int num2 = 8221;
    for (int index = 0; index < ch.Length; ++index)
    {
      int num3 = (int) ch[index];
      if (!char.IsNumber(ch[index]) && (num3 > 31 /*0x1F*/ && num3 < (int) sbyte.MaxValue || num3 == num1 || num3 == num2) && (!isPrevTextHasNumber && (num3 == (int) CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator[0] || index > 0 && char.IsNumber(ch[index - 1])) || isPrevTextHasNumber) && num3 != (int) CultureInfo.CurrentCulture.NumberFormat.CurrencyGroupSeparator[0])
      {
        decimalSeparator = num3;
        return true;
      }
    }
    return false;
  }

  private WParagraph GetOwnerParagraph(LayoutedWidget ltWidget)
  {
    WParagraph ownerParagraph = (WParagraph) null;
    if (ltWidget.Widget != null)
    {
      if (ltWidget.Widget is WParagraph)
        ownerParagraph = ltWidget.Widget as WParagraph;
      else if ((ltWidget.Widget is SplitWidgetContainer ? ((ltWidget.Widget as SplitWidgetContainer).RealWidgetContainer is WParagraph ? 1 : 0) : 0) != 0)
        ownerParagraph = (ltWidget.Widget as SplitWidgetContainer).RealWidgetContainer as WParagraph;
    }
    return ownerParagraph;
  }

  private int GetTabEndIndex(LayoutedWidget ltWidget, int startIndex)
  {
    for (int index = startIndex; index < ltWidget.ChildWidgets.Count; ++index)
    {
      if (ltWidget.ChildWidgets[index].Widget.LayoutInfo is TabsLayoutInfo)
        return index - 1;
    }
    return ltWidget.ChildWidgets.Count - 1;
  }

  public float GetListValue(
    WParagraph paragraph,
    ParagraphLayoutInfo paragraphInfo,
    WListFormat listFormat)
  {
    float listValue = -Math.Abs(paragraphInfo.ListTab);
    SizeF sizeF1 = new SizeF();
    Font font = paragraphInfo.ListFont.GetFont(paragraph.Document);
    WListLevel listLevel = paragraph.GetListLevel(listFormat);
    if (paragraphInfo.CurrentListType == ListType.Bulleted && listLevel.PicBullet != null)
      return this.MeasurePictureBulletSize(listLevel.PicBullet, font).Width;
    SizeF sizeF2 = this.MeasureString(paragraphInfo.ListValue, font, (StringFormat) null, paragraphInfo.CharacterFormat, true);
    if (paragraphInfo.ListAlignment == ListNumberAlignment.Center)
      listValue -= sizeF2.Width / 2f;
    else if (paragraphInfo.ListAlignment == ListNumberAlignment.Right)
      listValue -= sizeF2.Width;
    if (paragraph.ParagraphFormat.Bidi)
      listValue = -listValue;
    return listValue;
  }

  internal void DrawList(WParagraph paragraph, LayoutedWidget ltWidget, WListFormat listFormat)
  {
    if (!(ltWidget.Widget.LayoutInfo is ParagraphLayoutInfo layoutInfo) || ltWidget.ChildWidgets.Count == 0 || layoutInfo.ListValue == string.Empty)
      return;
    float num1 = -Math.Abs(layoutInfo.ListTab);
    bool flag = false;
    Font font = layoutInfo.ListFont.GetFont(paragraph.Document);
    WListLevel listLevel = paragraph.GetListLevel(listFormat);
    SizeF sizeF;
    if (layoutInfo.CurrentListType == ListType.Bulleted && listLevel.PicBullet != null)
    {
      flag = true;
      sizeF = this.MeasurePictureBulletSize(listLevel.PicBullet, font);
    }
    else
      sizeF = this.MeasureString(layoutInfo.ListValue, font, (StringFormat) null, layoutInfo.CharacterFormat, true);
    if (layoutInfo.ListAlignment == ListNumberAlignment.Center)
      num1 -= sizeF.Width / 2f;
    else if (layoutInfo.ListAlignment == ListNumberAlignment.Right)
      num1 -= sizeF.Width;
    if (paragraph.ParagraphFormat.Bidi)
      num1 = -num1;
    float num2 = 0.0f;
    if (ltWidget.ChildWidgets[0].ChildWidgets.Count > 0)
    {
      foreach (LayoutedWidget childWidget in (List<LayoutedWidget>) ltWidget.ChildWidgets[0].ChildWidgets)
      {
        if ((double) num2 < (double) childWidget.Bounds.Right)
          num2 = childWidget.Bounds.Right;
      }
    }
    else
      num2 = ltWidget.ChildWidgets[0].Bounds.Right;
    float x = paragraph.ParagraphFormat.Bidi ? num2 + (num1 - sizeF.Width) : ltWidget.ChildWidgets[0].Bounds.X + num1;
    string listValue = layoutInfo.ListValue;
    if (ltWidget.ChildWidgets[0].ChildWidgets.Count > 0)
    {
      int index1 = 0;
      if (ltWidget.ChildWidgets[0].ChildWidgets.Count > 1 && paragraph.IsLineNumbersEnabled())
        index1 = 1;
      IWidget widget1 = ltWidget.ChildWidgets[0].ChildWidgets[index1].Widget;
      switch (widget1)
      {
        case WTextRange _:
        case SplitStringWidget _:
label_30:
          x = paragraph.ParagraphFormat.Bidi ? num2 + (num1 - sizeF.Width) : ltWidget.ChildWidgets[0].ChildWidgets[index1].Bounds.X + num1;
          goto label_31;
        case WPicture _:
        case Shape _:
        case WTextBox _:
        case WChart _:
        case GroupShape _:
          if ((widget1 as ParagraphItem).GetTextWrappingStyle() == TextWrappingStyle.Inline)
            goto label_30;
          break;
      }
      for (int index2 = 1; index2 < ltWidget.ChildWidgets[0].ChildWidgets.Count; ++index2)
      {
        IWidget widget2 = ltWidget.ChildWidgets[0].ChildWidgets[index2].Widget;
        switch (widget2)
        {
          case WTextRange _:
          case SplitStringWidget _:
label_27:
            x = paragraph.ParagraphFormat.Bidi ? ltWidget.ChildWidgets[0].ChildWidgets[index2].Bounds.Right + (num1 - sizeF.Width) : ltWidget.ChildWidgets[0].ChildWidgets[index2].Bounds.X + num1;
            goto label_31;
          case WPicture _:
          case Shape _:
          case WTextBox _:
          case WChart _:
          case GroupShape _:
            if ((widget2 as ParagraphItem).GetTextWrappingStyle() != TextWrappingStyle.Inline)
              break;
            goto label_27;
        }
      }
    }
label_31:
    float num3 = 0.0f;
    if (layoutInfo.ListYPositions.Count >= 1)
    {
      num3 = layoutInfo.ListYPositions[0];
      layoutInfo.ListYPositions.RemoveAt(0);
      if (paragraph.IsContainDinOffcFont())
        num3 += sizeF.Height * 0.1f;
    }
    this.IsListCharacter = true;
    if (flag && listLevel.PicBullet.GetImage(listLevel.PicBullet.ImageBytes, false) != null)
    {
      this.Graphics.DrawImage(listLevel.PicBullet.GetImage(listLevel.PicBullet.ImageBytes, false), new RectangleF(x, num3, sizeF.Width, sizeF.Height));
      this.IsListCharacter = false;
    }
    else
      this.DrawString(FontScriptType.English, listValue, layoutInfo.CharacterFormat, paragraph.ParagraphFormat, new RectangleF(x, num3, Math.Abs(num1), sizeF.Height), Math.Abs(num1), ltWidget);
    if (layoutInfo.ListTabStop == null || layoutInfo.ListTabStop.TabLeader == Syncfusion.Layouting.TabLeader.NoLeader)
      return;
    this.DrawListTabLeader(paragraph, layoutInfo, layoutInfo.ListTab + sizeF.Width + num1, ltWidget.ChildWidgets[0].Bounds.X, num3);
  }

  private void DrawListTabLeader(
    WParagraph paragraph,
    ParagraphLayoutInfo paragraphInfo,
    float listWidth,
    float xPosition,
    float yPosition)
  {
    string tabLeader = this.GetTabLeader(paragraphInfo);
    if (!(tabLeader != string.Empty))
      return;
    float width = this.MeasureString(tabLeader, paragraphInfo.ListFont.GetFont(paragraph.Document), (StringFormat) null, paragraphInfo.CharacterFormat, true, false).Width;
    float num1 = paragraphInfo.ListTab - (float) Math.Ceiling((double) listWidth / (double) width) * width;
    string empty = string.Empty;
    int num2 = (int) Math.Floor((double) num1 / (double) width);
    for (int index = 0; index < num2; ++index)
      empty += tabLeader;
    SizeF sizeF = this.MeasureString(empty, paragraphInfo.ListFont.GetFont(paragraph.Document), (StringFormat) null, paragraphInfo.CharacterFormat, true, false);
    this.DrawString(FontScriptType.English, empty, paragraphInfo.CharacterFormat, paragraph.ParagraphFormat, new RectangleF(xPosition - num1, yPosition, sizeF.Width, sizeF.Height), sizeF.Width, (LayoutedWidget) null);
  }

  private string GetTabLeader(ParagraphLayoutInfo paragraphInfo)
  {
    string tabLeader = string.Empty;
    switch (paragraphInfo.ListTabStop.TabLeader)
    {
      case Syncfusion.Layouting.TabLeader.Dotted:
        tabLeader = ".";
        break;
      case Syncfusion.Layouting.TabLeader.Hyphenated:
        tabLeader = "-";
        break;
      case Syncfusion.Layouting.TabLeader.Single:
        tabLeader = "_";
        break;
    }
    return tabLeader;
  }

  public float GetAscentValueForEQField(WField field)
  {
    float ascentValueForEqField = this.GetAscent(field.GetCharacterFormatValue().GetFontToRender(field.ScriptType));
    for (int index = 0; index < DocumentLayouter.EquationFields.Count; ++index)
    {
      if (DocumentLayouter.EquationFields[index].EQFieldEntity == field)
      {
        ascentValueForEqField = -DocumentLayouter.EquationFields[index].LayouttedEQField.Bounds.Y;
        break;
      }
    }
    return ascentValueForEqField;
  }

  internal float IsLineContainsEQfield(LayoutedWidget ltWidget)
  {
    for (int index = 0; index < ltWidget.ChildWidgets.Count; ++index)
    {
      LayoutedWidget childWidget = ltWidget.ChildWidgets[index];
      if (childWidget.Widget is WField && (childWidget.Widget as WField).FieldType == FieldType.FieldExpression)
      {
        WCharacterFormat charFormat = (childWidget.Widget as WField).GetCharFormat();
        FontScriptType scriptType = (childWidget.Widget as WField).ScriptType;
        float num = this.MeasureString(" ", charFormat.GetFontToRender(scriptType), (StringFormat) null, charFormat, false).Height - this.GetAscent(charFormat.GetFontToRender(scriptType));
        return ltWidget.ChildWidgets[index].Bounds.Height - (this.GetAscentValueForEQField(childWidget.Widget as WField) + num);
      }
    }
    return float.MinValue;
  }

  internal bool IsEmptyParagraph(WParagraph para)
  {
    bool flag = false;
    if (para != null && para.Text == "" && para.Items.Count == 0)
      flag = true;
    return flag;
  }

  private void AddCommentMark(WCommentMark commentMark, LayoutedWidget ltWidget)
  {
    this.GetRevisionColor(commentMark.Document.RevisionOptions.CommentColor);
    if (this.m_commentMarks == null)
      this.m_commentMarks = new List<PointF[]>();
    float height = ltWidget.Bounds.Height;
    if ((double) height == 0.0 && commentMark.OwnerParagraph != null)
      height = this.MeasureString(" ", commentMark.OwnerParagraph.BreakCharacterFormat.GetFontToRender(FontScriptType.English), (StringFormat) null).Height;
    PointF pointF1 = new PointF(ltWidget.Bounds.X, ltWidget.Bounds.Y);
    PointF pointF2 = new PointF(ltWidget.Bounds.X, ltWidget.Bounds.Y + height);
    float num = 0.3f;
    PointF[] pointFArray = new PointF[4];
    if (commentMark.Type == CommentMarkType.CommentStart)
    {
      pointFArray[0] = new PointF(pointF1.X + num, pointF1.Y - num);
      pointFArray[1] = pointF1;
      pointFArray[2] = pointF2;
      pointFArray[3] = new PointF(pointF2.X + num, pointF2.Y + num);
      if (this.m_commentId == null)
        this.m_commentId = commentMark.CommentId;
    }
    else
    {
      pointFArray[0] = new PointF(pointF1.X - num, pointF1.Y - num);
      pointFArray[1] = pointF1;
      pointFArray[2] = pointF2;
      pointFArray[3] = new PointF(pointF2.X - num, pointF2.Y + num);
      if (this.m_commentId == commentMark.CommentId)
        this.m_commentId = (string) null;
    }
    this.m_commentMarks.Add(pointFArray);
  }

  private void DrawCommentMarks(RevisionOptions revisionOptions)
  {
    Color revisionColor = this.GetRevisionColor(revisionOptions.CommentColor);
    foreach (PointF[] commentMark in this.m_commentMarks)
    {
      Pen pen = this.CreatePen(revisionColor, 0.2f);
      GraphicsPath graphicsPath = this.CreateGraphicsPath();
      graphicsPath.AddLines(commentMark);
      this.Graphics.DrawPath(pen, graphicsPath);
    }
  }

  internal void DrawAbsoluteTab(WAbsoluteTab absoluteTab, LayoutedWidget ltWidget)
  {
    float left = ltWidget.Bounds.Left;
    float top = ltWidget.Bounds.Top;
    float height = ltWidget.Bounds.Height;
    StringFormat format = new StringFormat(this.StringFormt);
    if (absoluteTab.CharacterFormat.Bidi)
      format.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
    else
      format.FormatFlags &= ~StringFormatFlags.DirectionRightToLeft;
    string empty = string.Empty;
    if (absoluteTab.m_layoutInfo is TabsLayoutInfo && absoluteTab.Alignment != AbsoluteTabAlignment.Left)
      this.UpdateAbsoluteTabLeader(absoluteTab, ltWidget, ref empty);
    SizeF sizeF = this.MeasureString(empty, absoluteTab.CharacterFormat.GetFontToRender(FontScriptType.English), format);
    RectangleF bounds = new RectangleF(left, top, sizeF.Width + ltWidget.SubWidth, height);
    this.DrawString(FontScriptType.English, empty, absoluteTab.CharacterFormat, absoluteTab.GetOwnerParagraphValue().ParagraphFormat, bounds, ltWidget.Bounds.Width, ltWidget);
  }

  private void UpdateAbsoluteTabLeader(
    WAbsoluteTab absoluteTab,
    LayoutedWidget ltWidget,
    ref string text)
  {
    TabsLayoutInfo layoutInfo = absoluteTab.m_layoutInfo as TabsLayoutInfo;
    text = string.Empty;
    StringFormat format = new StringFormat(this.StringFormt);
    WCharacterFormat characterFormat = absoluteTab.CharacterFormat;
    if (characterFormat.Bidi)
      format.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
    else
      format.FormatFlags &= ~StringFormatFlags.DirectionRightToLeft;
    new WTextRange((IWordDocument) absoluteTab.Document).ApplyCharacterFormat(characterFormat);
    if (characterFormat.GetFontToRender(FontScriptType.English).Underline || characterFormat.GetFontToRender(FontScriptType.English).Strikeout)
      this.FillSpace(absoluteTab.m_layoutInfo.Font.GetFont(absoluteTab.Document), ltWidget, format, ref text);
    if (layoutInfo == null)
      return;
    switch (layoutInfo.CurrTabLeader)
    {
      case Syncfusion.Layouting.TabLeader.Dotted:
        this.FillDots(absoluteTab.m_layoutInfo.Font.GetFont(absoluteTab.Document), ltWidget, characterFormat, format, ref text);
        break;
      case Syncfusion.Layouting.TabLeader.Hyphenated:
        this.FillHyphens(absoluteTab.m_layoutInfo.Font.GetFont(absoluteTab.Document), ltWidget, characterFormat, format, ref text);
        break;
      case Syncfusion.Layouting.TabLeader.Single:
        this.FillSingle(absoluteTab.m_layoutInfo.Font.GetFont(absoluteTab.Document), ltWidget, characterFormat, format, ref text);
        break;
    }
  }

  internal void DrawSeparator(WTextRange txtRange, LayoutedWidget ltWidget)
  {
    this.Graphics.DrawLine(this.CreatePen(Color.Black, 0.5f), new PointF(ltWidget.Bounds.X, ltWidget.Bounds.Y + ltWidget.Bounds.Height / 2f), new PointF(ltWidget.Bounds.Right, ltWidget.Bounds.Y + ltWidget.Bounds.Height / 2f));
  }

  internal void DrawTextRange(WTextRange txtRange, LayoutedWidget ltWidget, string text)
  {
    if (text == '\u0003'.ToString() || text == '\u0004'.ToString())
    {
      this.DrawSeparator(txtRange, ltWidget);
    }
    else
    {
      this.currTextRange = txtRange;
      float left = ltWidget.Bounds.Left;
      float top = ltWidget.Bounds.Top;
      float height = ltWidget.Bounds.Height;
      StringFormat stringFormat = new StringFormat(this.StringFormt);
      if (txtRange.CharacterFormat.Bidi)
        stringFormat.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
      else
        stringFormat.FormatFlags &= ~StringFormatFlags.DirectionRightToLeft;
      if (txtRange.m_layoutInfo is TabsLayoutInfo layoutInfo)
      {
        if (!layoutInfo.IsTabWidthUpdatedBasedOnIndent)
          this.UpdateTabLeader(txtRange, ltWidget, ref text);
        txtRange.Text = ControlChar.Tab;
      }
      SizeF sizeF = new SizeF(ltWidget.Widget.LayoutInfo.Size);
      if (text == '\u0002'.ToString() && txtRange.GetOwnerParagraphValue().OwnerTextBody.Owner is WFootnote)
      {
        WFootnote owner = txtRange.GetOwnerParagraphValue().OwnerTextBody.Owner as WFootnote;
        if (owner.m_layoutInfo is FootnoteLayoutInfo)
          text = (owner.m_layoutInfo as FootnoteLayoutInfo).FootnoteID;
      }
      RectangleF bounds = new RectangleF();
      if (txtRange.Owner is WParagraph && txtRange.Text == text && txtRange.GetIndexInOwnerCollection() == txtRange.OwnerParagraph.Items.Count - 1 && txtRange.Text.Trim() != string.Empty)
        text = text.TrimEnd();
      else
        sizeF = this.MeasureTextRange(txtRange, text);
      bounds = !(txtRange.Text == "\t") ? new RectangleF(left, top, sizeF.Width + ltWidget.SubWidth, height) : new RectangleF(left, top, ltWidget.Bounds.Width, ltWidget.Bounds.Height);
      WParagraphFormat paraFormat = (WParagraphFormat) null;
      if (txtRange.Owner is WParagraph)
        paraFormat = txtRange.OwnerParagraph.ParagraphFormat;
      WCharacterFormat characterFormat = txtRange.CharacterFormat;
      bool flag = ltWidget.HorizontalAlign == HAlignment.Distributed || ltWidget.HorizontalAlign == HAlignment.Justify;
      if ((!flag || ltWidget.IsLastLine) && ltWidget.HorizontalAlign != HAlignment.Distributed && !this.IsOwnerParagraphEmpty(text))
        this.DrawString(txtRange.ScriptType, text, characterFormat, paraFormat, bounds, ltWidget.Bounds.Width, ltWidget);
      else if (flag)
      {
        if (this.IsTextRangeFollowWithTab(ltWidget))
          this.DrawString(txtRange.ScriptType, text, characterFormat, paraFormat, bounds, ltWidget.Bounds.Width, ltWidget);
        else
          this.DrawJustifiedLine(txtRange, text, characterFormat, paraFormat, bounds, ltWidget);
      }
      if (this.currHyperlink == null)
        return;
      if (this.IsValidFieldResult(this.currHyperlink.Field, ltWidget.Widget))
        this.AddHyperLink(this.currHyperlink, ltWidget.Bounds);
      else
        this.currHyperlink = (Hyperlink) null;
    }
  }

  private bool IsValidFieldResult(WField hyperLinkField, IWidget widget)
  {
    WTextRange wtextRange = (WTextRange) null;
    switch (widget)
    {
      case WTextRange _:
        wtextRange = widget as WTextRange;
        break;
      case SplitStringWidget _:
        wtextRange = (widget as SplitStringWidget).RealStringWidget as WTextRange;
        break;
    }
    for (int index = 0; index < hyperLinkField.Range.Count && wtextRange != null; ++index)
    {
      if (hyperLinkField.Range.InnerList[index] is WTextRange && hyperLinkField.Range.InnerList[index] == wtextRange || hyperLinkField.Range.InnerList[index] is WParagraph && hyperLinkField.Range.InnerList[index] == wtextRange.OwnerParagraph)
        return true;
    }
    return false;
  }

  internal void UpdateBookmarkTargetPosition(Entity ent, LayoutedWidget ltWidget)
  {
    bool flag = false;
    if (!(ent.PreviousSibling is BookmarkStart))
      return;
    BookmarkStart previousSibling = ent.PreviousSibling as BookmarkStart;
    if (DrawingContext.BookmarkHyperlinksList.Count != 0)
    {
      for (int index = 0; index < DrawingContext.BookmarkHyperlinksList.Count; ++index)
      {
        foreach (KeyValuePair<string, DocumentLayouter.BookmarkHyperlink> keyValuePair in DrawingContext.BookmarkHyperlinksList[index])
        {
          if (keyValuePair.Key == previousSibling.Name)
          {
            flag = true;
            keyValuePair.Value.TargetBounds = ltWidget.Bounds;
            keyValuePair.Value.TargetPageNumber = DocumentLayouter.PageNumber;
            if (ent is ParagraphItem)
              this.UpdateTOCLevel((ent as ParagraphItem).OwnerParagraph, keyValuePair.Value);
          }
        }
      }
    }
    if (flag)
      return;
    Dictionary<string, DocumentLayouter.BookmarkHyperlink> dictionary = new Dictionary<string, DocumentLayouter.BookmarkHyperlink>();
    DocumentLayouter.BookmarkHyperlink bookmarkHyperlink = new DocumentLayouter.BookmarkHyperlink();
    bookmarkHyperlink.HyperlinkValue = previousSibling.Name;
    bookmarkHyperlink.TargetBounds = ltWidget.Bounds;
    bookmarkHyperlink.TargetPageNumber = DocumentLayouter.PageNumber;
    dictionary.Add(bookmarkHyperlink.HyperlinkValue, bookmarkHyperlink);
    DrawingContext.BookmarkHyperlinksList.Add(dictionary);
  }

  internal void CreateBookmarkRerefeceLink(Entity ent, LayoutedWidget ltWidget)
  {
    if (this.CurrentBookmarkName == null || this.CurrentBookmarkName == null)
      return;
    if (!(ent is WField) || (double) ltWidget.Bounds.Width > 0.0)
      this.AddLinkToBookmark(ltWidget.Bounds, this.CurrentBookmarkName, false);
    if (!(ent.NextSibling is WFieldMark) || (ent.NextSibling as WFieldMark).Type != FieldMarkType.FieldEnd || ent.NextSibling as WFieldMark != this.CurrentRefField.FieldEnd)
      return;
    this.CurrentBookmarkName = (string) null;
    this.CurrentRefField = (WField) null;
  }

  private bool IsTextRangeFollowWithTab(LayoutedWidget ltWidget)
  {
    int num = ltWidget.Owner.ChildWidgets.IndexOf(ltWidget);
    if (num >= 0)
    {
      for (int index = num; index < ltWidget.Owner.ChildWidgets.Count; ++index)
      {
        if (ltWidget.Owner.ChildWidgets[index].Widget.LayoutInfo is TabsLayoutInfo)
          return true;
      }
    }
    return false;
  }

  private void UpdateTabLeader(WTextRange txtRange, LayoutedWidget ltWidget, ref string text)
  {
    TabsLayoutInfo layoutInfo = txtRange.m_layoutInfo as TabsLayoutInfo;
    text = string.Empty;
    StringFormat format = new StringFormat(this.StringFormt);
    if (txtRange.CharacterFormat.Bidi)
      format.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
    else
      format.FormatFlags &= ~StringFormatFlags.DirectionRightToLeft;
    if (this.IsTOC(txtRange) && txtRange.GetOwnerParagraphValue().ParaStyle != null)
    {
      format = this.StringFormt;
      if (txtRange.GetOwnerParagraphValue().ParaStyle.CharacterFormat.Bidi)
        format.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
      else
        format.FormatFlags &= ~StringFormatFlags.DirectionRightToLeft;
    }
    if (txtRange.CharacterFormat != null && (txtRange.CharacterFormat.GetFontToRender(txtRange.ScriptType).Underline || txtRange.CharacterFormat.GetFontToRender(txtRange.ScriptType).Strikeout))
      this.FillSpace(txtRange.m_layoutInfo.Font.GetFont(txtRange.Document), ltWidget, format, ref text);
    if (layoutInfo == null)
      return;
    switch (layoutInfo.CurrTabLeader)
    {
      case Syncfusion.Layouting.TabLeader.Dotted:
        this.FillDots(txtRange.m_layoutInfo.Font.GetFont(txtRange.Document), ltWidget, txtRange.CharacterFormat, format, ref text);
        break;
      case Syncfusion.Layouting.TabLeader.Hyphenated:
        this.FillHyphens(txtRange.m_layoutInfo.Font.GetFont(txtRange.Document), ltWidget, txtRange.CharacterFormat, format, ref text);
        break;
      case Syncfusion.Layouting.TabLeader.Single:
        this.FillSingle(txtRange.m_layoutInfo.Font.GetFont(txtRange.Document), ltWidget, txtRange.CharacterFormat, format, ref text);
        break;
    }
  }

  private void FillDots(
    Font font,
    LayoutedWidget ltWidget,
    WCharacterFormat charFormat,
    StringFormat format,
    ref string text)
  {
    text = string.Empty;
    float width = this.MeasureString(".", font, format, charFormat, true, false).Width;
    for (float num = width; (double) num <= (double) ltWidget.Bounds.Width; num += width)
      text += ".";
  }

  private void FillSingle(
    Font font,
    LayoutedWidget ltWidget,
    WCharacterFormat charFormat,
    StringFormat format,
    ref string text)
  {
    text = string.Empty;
    float width = this.MeasureString("_", font, format, charFormat, true, false).Width;
    for (float num = width; (double) num <= (double) ltWidget.Bounds.Width; num += width)
      text += "_";
  }

  private void FillHyphens(
    Font font,
    LayoutedWidget ltWidget,
    WCharacterFormat charFormat,
    StringFormat format,
    ref string text)
  {
    text = string.Empty;
    float width = this.MeasureString("-", font, format, charFormat, true, false).Width;
    for (float num = width; (double) num <= (double) ltWidget.Bounds.Width; num += width)
      text += "-";
  }

  private void FillSpace(Font font, LayoutedWidget ltWidget, StringFormat format, ref string text)
  {
    text = string.Empty;
    while ((double) this.MeasureString(text, font, format).Width <= (double) ltWidget.Bounds.Width)
      text += " ";
  }

  internal void DrawSymbol(WSymbol symbol, LayoutedWidget ltWidget)
  {
    string str = char.ConvertFromUtf32((int) symbol.CharacterCode);
    WCharacterFormat characterFormat = symbol.CharacterFormat;
    RectangleF bounds = ltWidget.Bounds;
    float width = ltWidget.Bounds.Width;
    RectangleF clipBounds = new RectangleF(ltWidget.Bounds.X, ltWidget.Bounds.Y, width, ltWidget.Bounds.Height);
    float scaling = characterFormat.Scaling;
    bool isNeedToScale = (double) scaling != 100.0 && ((double) scaling >= 1.0 || (double) scaling <= 600.0);
    PointF empty = PointF.Empty;
    float rotationAngle = 0.0f;
    bool drawLines = false;
    if (symbol != null && symbol.m_layoutInfo != null && symbol.m_layoutInfo.IsVerticalText)
    {
      drawLines = true;
      this.TransformGraphicsPosition(ltWidget, isNeedToScale, ref empty, ref rotationAngle, symbol.OwnerParagraph);
    }
    if ((double) width == 0.0 || this.IsWidgetNeedToClipBasedOnXPosition(ltWidget, ref width, bounds))
    {
      this.ResetTransform();
    }
    else
    {
      bool clip = this.IsNeedToClip(clipBounds);
      if (clip)
      {
        clipBounds = this.ClipBoundsContainer.Peek();
        if ((double) clipBounds.Width == 0.0)
        {
          this.ResetTransform();
          return;
        }
      }
      if ((double) ltWidget.Bounds.Width > 0.0 && isNeedToScale)
        this.RotateAndScaleTransform(ref bounds, ref clipBounds, scaling, PointF.Empty, 0.0f, false);
      if (clip)
      {
        this.Graphics.SetClip(clipBounds, CombineMode.Replace);
        drawLines = true;
      }
      StringFormat format = new StringFormat(this.StringFormt);
      if (characterFormat.Bidi)
        format.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
      else
        format.FormatFlags &= ~StringFormatFlags.DirectionRightToLeft;
      if (symbol.m_layoutInfo == null)
        return;
      Font font1 = symbol.m_layoutInfo.Font.GetFont(symbol.Document);
      Font font2 = characterFormat == null || characterFormat.SubSuperScript == SubSuperScript.None ? font1 : characterFormat.Document.FontSettings.GetFont(font1.Name, this.GetSubSuperScriptFontSize(font1), font1.Style);
      if (this.m_privateFontStream != null && this.m_privateFontStream.ContainsKey(symbol.FontName))
        font2 = this.GetPrivateFont(font2.Name, font2.Size, font2.Style);
      float num = 0.0f;
      string fontNameToRender = characterFormat.GetFontNameToRender(FontScriptType.English);
      if (!this.IsListCharacter && (fontNameToRender == "DIN Offc" || fontNameToRender == "DIN OT") && this.currParagraph != null && this.currParagraph.IsContainDinOffcFont())
        num = bounds.Height * 0.2f;
      this.EnsureComplexScript(characterFormat);
      RectangleF textBounds = new RectangleF(bounds.X, bounds.Y + num, bounds.Width, bounds.Height);
      if ((double) characterFormat.CharacterSpacing != 0.0)
        this.DrawStringBasedOnCharSpacing(FontScriptType.English, ltWidget != null ? ltWidget.CharacterRange : CharacterRangeType.LTR, font2, (Brush) this.GetBrush(this.GetTextColor(symbol.CharacterFormat)), bounds, str, format, characterFormat);
      else
        this.Graphics.DrawString(str, font2, (Brush) this.GetBrush(this.GetTextColor(symbol.CharacterFormat)), textBounds, format);
      if (characterFormat.UnderlineStyle != UnderlineStyle.None || characterFormat.Strikeout || characterFormat.DoubleStrike)
      {
        bool isSameLine = false;
        this.CheckPreOrNextSiblingIsTab(ref characterFormat, ref textBounds, ltWidget, ref isSameLine);
        this.AddLineToCollection(str, characterFormat.SubSuperScript != SubSuperScript.None, font1, characterFormat, drawLines, textBounds, isSameLine);
      }
      this.ResetTransform();
      if (!clip)
        return;
      this.Graphics.ResetClip();
    }
  }

  internal void DrawPicture(WPicture picture, LayoutedWidget ltwidget)
  {
    if (picture != null)
    {
      Image image1 = picture.GetImage(picture.ImageBytes, false);
      if (image1 is Metafile && DocumentLayouter.IsAzureCompatible)
        image1 = picture.GetDefaultImage();
      if (image1 == null)
        return;
      SizeF size = this.MeasureImage(picture);
      RectangleF bounds1 = ltwidget.Bounds;
      if (float.IsNaN(bounds1.X))
        bounds1.X = 0.0f;
      this.ResetTransform();
      if (!picture.IsShape && (picture.TextWrappingStyle == TextWrappingStyle.Tight || picture.TextWrappingStyle == TextWrappingStyle.Through))
      {
        float lineWidth = this.GetLineWidth(picture);
        if ((double) lineWidth > 0.0)
          bounds1 = new RectangleF(bounds1.X - lineWidth, bounds1.Y - lineWidth, bounds1.Width + lineWidth * 2f, bounds1.Height + lineWidth * 2f);
      }
      RectangleF rectangleF;
      if (ltwidget.Widget.LayoutInfo.IsVerticalText && ltwidget.Widget is WPicture)
      {
        WParagraph ownerParagraphValue = (ltwidget.Widget as WPicture).GetOwnerParagraphValue();
        Entity ownerEntity1 = ownerParagraphValue.GetOwnerEntity();
        if (ownerParagraphValue.IsInCell)
        {
          WTableCell ownerEntity2 = ownerParagraphValue.GetOwnerEntity() as WTableCell;
          LayoutedWidget cellLayoutedWidget = this.GetOwnerCellLayoutedWidget(ltwidget);
          if (cellLayoutedWidget != null)
          {
            RectangleF bounds2 = cellLayoutedWidget.Owner.Bounds;
            if (ownerEntity2.CellFormat.TextDirection == TextDirection.VerticalTopToBottom)
            {
              this.Graphics.TranslateTransform(bounds2.X + bounds2.Y + bounds2.Width, bounds2.Y - bounds2.X);
              this.Graphics.RotateTransform(90f);
            }
            else
            {
              this.Graphics.TranslateTransform(bounds2.X - bounds2.Y, bounds2.X + bounds2.Y + bounds2.Height);
              this.Graphics.RotateTransform(270f);
            }
          }
        }
        else if (ownerEntity1 is WTextBox)
        {
          WTextBox wtextBox = ownerEntity1 as WTextBox;
          float left1 = wtextBox.TextLayoutingBounds.Left;
          float top = wtextBox.TextLayoutingBounds.Top;
          float left2 = wtextBox.TextBoxFormat.InternalMargin.Left;
          float dy1 = (float) ((double) top - (double) bounds1.Y + ((double) bounds1.X - (double) left1)) + left2;
          if (wtextBox.TextBoxFormat.TextDirection == TextDirection.VerticalTopToBottom)
          {
            float boxContentHeight = this.GetLayoutedTextBoxContentHeight(ltwidget);
            float shiftVerticalText = this.GetWidthToShiftVerticalText(wtextBox.TextBoxFormat.TextVerticalAlignment, boxContentHeight, wtextBox.TextLayoutingBounds.Height);
            Graphics graphics = this.Graphics;
            double num = (double) wtextBox.TextLayoutingBounds.X + (double) wtextBox.TextLayoutingBounds.Y;
            rectangleF = wtextBox.TextLayoutingBounds;
            double height = (double) rectangleF.Height;
            double dx = num + height - (double) shiftVerticalText;
            rectangleF = wtextBox.TextLayoutingBounds;
            double y = (double) rectangleF.Y;
            rectangleF = wtextBox.TextLayoutingBounds;
            double x = (double) rectangleF.X;
            double dy2 = y - x;
            graphics.TranslateTransform((float) dx, (float) dy2);
            this.Graphics.RotateTransform(90f);
          }
          else
            this.Graphics.TranslateTransform(bounds1.Y - top, dy1);
        }
        else if (ownerEntity1 is Shape)
        {
          Shape shape = ownerEntity1 as Shape;
          float left3 = shape.TextLayoutingBounds.Left;
          rectangleF = shape.TextLayoutingBounds;
          float top = rectangleF.Top;
          float left4 = shape.TextFrame.InternalMargin.Left;
          float dy = (float) ((double) top - (double) bounds1.Y + ((double) bounds1.X - (double) left3)) + left4;
          if (shape.TextFrame.TextDirection == TextDirection.VerticalTopToBottom || shape.TextFrame.TextDirection == TextDirection.VerticalFarEast)
            this.Graphics.TranslateTransform(this.GetCellHeightForVerticalText((Entity) picture) + left3 - bounds1.Y - bounds1.Width - left4, dy);
          else
            this.Graphics.TranslateTransform(bounds1.Y - top, dy);
        }
        else if (ownerParagraphValue.Owner.Owner is ChildShape)
        {
          ChildShape owner = ownerParagraphValue.Owner.Owner as ChildShape;
          float left5 = owner.TextLayoutingBounds.Left;
          float top = owner.TextLayoutingBounds.Top;
          float left6 = owner.TextFrame.InternalMargin.Left;
          float dy = (float) ((double) top - (double) bounds1.Y + ((double) bounds1.X - (double) left5)) + left6;
          if (owner.TextFrame.TextDirection == TextDirection.VerticalTopToBottom || owner.TextFrame.TextDirection == TextDirection.VerticalFarEast)
            this.Graphics.TranslateTransform(this.GetCellHeightForVerticalText((Entity) picture) + left5 - bounds1.Y - bounds1.Width - left6, dy);
          else
            this.Graphics.TranslateTransform(bounds1.Y - top, dy);
        }
      }
      float clipTop = 0.0f;
      WParagraph ownerParagraphValue1 = picture.GetOwnerParagraphValue();
      Entity ownerEntity3 = ownerParagraphValue1.GetOwnerEntity();
      if (picture.TextWrappingStyle == TextWrappingStyle.Inline && ownerParagraphValue1 != null && !ownerParagraphValue1.IsInCell)
      {
        switch (ownerEntity3)
        {
          case Shape _:
          case WTextBox _:
            break;
          default:
            clipTop = this.GetClipTopPosition(bounds1, true) * 2f;
            if ((double) clipTop > 0.0)
            {
              clipTop += (float) this.FontMetric.Descent(picture.GetOwnerParagraphValue().BreakCharacterFormat.GetFontToRender(FontScriptType.English));
              break;
            }
            break;
        }
      }
      float width = bounds1.Width;
      if ((double) width == 0.0 || this.IsWidgetNeedToClipBasedOnXPosition(ltwidget, ref width, bounds1))
      {
        this.ResetTransform();
        return;
      }
      if ((ownerParagraphValue1 == null || (!picture.LayoutInCell || picture.Document.Settings.CompatibilityMode != CompatibilityMode.Word2013) && picture.TextWrappingStyle != TextWrappingStyle.Inline || !ownerParagraphValue1.IsInCell ? 1 : (!this.IsTableInTextBoxOrShape(ownerParagraphValue1.GetOwnerEntity(), true) ? 1 : 0)) != 0)
      {
        RectangleF clipBounds = this.GetClipBounds(bounds1, width, clipTop);
        bool flag = false;
        if (this.IsNeedToClip(clipBounds))
        {
          this.SetClip((double) clipTop <= 0.0 ? this.ClipBoundsContainer.Peek() : this.UpdateClipBounds(clipBounds, false));
          flag = true;
        }
        else if ((double) clipTop > 0.0)
        {
          this.SetClip(clipBounds);
          flag = true;
        }
        if (flag)
        {
          rectangleF = this.Graphics.ClipBounds;
          if ((double) rectangleF.Width == 0.0)
          {
            this.Graphics.ResetTransform();
            return;
          }
        }
      }
      if (picture.Owner is GroupShape || picture.Owner is ChildGroupShape)
        size = new SizeF(bounds1.Width, bounds1.Height);
      if ((double) picture.FillRectangle.LeftOffset != 0.0 || (double) picture.FillRectangle.RightOffset != 0.0 || (double) picture.FillRectangle.TopOffset != 0.0 || (double) picture.FillRectangle.BottomOffset != 0.0)
      {
        if (image1 is Bitmap)
        {
          RectangleF rect = this.CropPosition(picture);
          using (Image image2 = (Image) (image1 as Bitmap).Clone(rect, image1.PixelFormat))
          {
            using (MemoryStream memoryStream = new MemoryStream())
            {
              image2.Save((Stream) memoryStream, image1.RawFormat);
              memoryStream.Position = 0L;
              image1 = Image.FromStream((Stream) new MemoryStream(memoryStream.ToArray()));
            }
          }
          if ((double) picture.FillRectangle.LeftOffset < 0.0 || (double) picture.FillRectangle.RightOffset < 0.0 || (double) picture.FillRectangle.TopOffset < 0.0 || (double) picture.FillRectangle.BottomOffset < 0.0)
            this.CropImageBounds(picture, ref bounds1, ref size);
        }
        else
        {
          this.Graphics.SetClip(bounds1, CombineMode.Replace);
          TileRectangle fillRectangle = picture.FillRectangle;
          float num1 = size.Height - (float) ((double) size.Height * ((double) fillRectangle.TopOffset + (double) fillRectangle.BottomOffset) / 100.0);
          float num2 = size.Width - (float) ((double) size.Width * ((double) fillRectangle.LeftOffset + (double) fillRectangle.RightOffset) / 100.0);
          float num3 = size.Height * 100f / num1;
          float f1 = (float) ((double) (size.Width * 100f / num2) * (double) size.Width / 100.0);
          float f2 = (float) ((double) num3 * (double) size.Height / 100.0);
          float x = bounds1.X;
          float y = bounds1.Y;
          if ((double) fillRectangle.LeftOffset != 0.0 && (double) fillRectangle.RightOffset != 0.0)
          {
            float f3 = fillRectangle.LeftOffset / (fillRectangle.LeftOffset + fillRectangle.RightOffset);
            x += (float) ((float.IsNaN(f3) || float.IsInfinity(f3) ? 0.0 : (double) f3) * ((double) size.Width - (double) f1));
          }
          else if ((double) fillRectangle.LeftOffset != 0.0)
            x -= (float) ((double) fillRectangle.LeftOffset * (double) f1 / 100.0);
          if ((double) fillRectangle.TopOffset != 0.0 && (double) fillRectangle.BottomOffset != 0.0)
          {
            float f4 = fillRectangle.TopOffset / (fillRectangle.TopOffset + fillRectangle.BottomOffset);
            y += (float) ((float.IsNaN(f4) || float.IsInfinity(f4) ? 0.0 : (double) f4) * ((double) size.Height - (double) f2));
          }
          else if ((double) fillRectangle.TopOffset != 0.0)
            y -= (float) ((double) fillRectangle.TopOffset * (double) f2 / 100.0);
          if (!float.IsNaN(x) && !float.IsInfinity(x))
            bounds1.X = x;
          if (!float.IsNaN(y) && !float.IsInfinity(y))
            bounds1.Y = y;
          if (!float.IsNaN(f1) && !float.IsInfinity(f1))
            size.Width = f1;
          if (!float.IsNaN(f2) && !float.IsInfinity(f2))
            size.Height = f2;
        }
      }
      float rotation = picture.Rotation;
      if (((double) rotation != 0.0 || (double) rotation == 0.0 && (picture.FlipHorizontal || picture.FlipVertical)) && !ltwidget.Widget.LayoutInfo.IsVerticalText && picture.TextWrappingStyle != TextWrappingStyle.Tight && picture.TextWrappingStyle != TextWrappingStyle.Through)
      {
        if (!(picture.Owner is GroupShape) && !(picture.Owner is ChildGroupShape) && (double) rotation != 0.0)
        {
          RectangleF boundingBoxCoordinates = this.GetBoundingBoxCoordinates(new RectangleF(0.0f, 0.0f, picture.Width, picture.Height), rotation);
          bounds1 = new RectangleF(bounds1.X - boundingBoxCoordinates.X, bounds1.Y - boundingBoxCoordinates.Y, picture.Width, picture.Height);
        }
        this.Graphics.Transform = this.GetTransformMatrix(bounds1, rotation, picture.FlipHorizontal, picture.FlipVertical);
      }
      if (((double) rotation != 0.0 || picture.FlipHorizontal || picture.FlipVertical) && !ltwidget.Widget.LayoutInfo.IsVerticalText && (picture.TextWrappingStyle == TextWrappingStyle.Tight || picture.TextWrappingStyle == TextWrappingStyle.Through))
        this.Graphics.Transform = this.GetTransformMatrix(bounds1, rotation, picture.FlipHorizontal, picture.FlipVertical);
      Entity ownerEntity4 = picture.GetOwnerParagraphValue().GetOwnerEntity();
      switch (ownerEntity4)
      {
        case ChildShape _:
          this.Rotate((ParagraphItem) (ownerEntity4 as ChildShape), (ownerEntity4 as ChildShape).Rotation, (ownerEntity4 as ChildShape).FlipVertical, (ownerEntity4 as ChildShape).FlipHorizantal, (ownerEntity4 as ChildShape).TextLayoutingBounds);
          break;
        case WTextBox _:
          WTextBox wtextBox1 = ownerEntity4 as WTextBox;
          if ((double) wtextBox1.TextBoxFormat.Rotation != 0.0 && (!wtextBox1.IsShape || !wtextBox1.Shape.TextFrame.Upright))
            this.Graphics.Transform = this.GetTransformMatrix(this.m_rotateTransform, wtextBox1.TextBoxFormat.Rotation, wtextBox1.TextBoxFormat.FlipHorizontal, wtextBox1.TextBoxFormat.FlipVertical);
          if ((double) wtextBox1.TextBoxFormat.Rotation != 0.0 && wtextBox1.IsShape && wtextBox1.Shape.TextFrame.Upright && !wtextBox1.TextBoxFormat.AutoFit)
          {
            RectangleF clipBounds = this.Graphics.ClipBounds;
            if ((double) clipBounds.Y > (double) bounds1.Y && (double) clipBounds.Bottom < (double) bounds1.Bottom)
            {
              bounds1.Y = clipBounds.Y;
              break;
            }
            if ((double) clipBounds.X > (double) bounds1.X && (double) clipBounds.Right < (double) bounds1.Right)
            {
              bounds1.X = clipBounds.X;
              break;
            }
            break;
          }
          break;
        case Shape _:
          Shape shape1 = ownerEntity4 as Shape;
          if ((double) shape1.Rotation != 0.0)
          {
            this.Graphics.Transform = this.GetTransformMatrix(this.m_rotateTransform, shape1.Rotation, shape1.FlipHorizontal, shape1.FlipVertical);
            break;
          }
          break;
      }
      ImageAttributes imageAttr = (ImageAttributes) null;
      if (image1 is Bitmap && picture.FillFormat.BlipFormat.BlipTransparency == BlipTransparency.GrayScale)
      {
        ColorMatrix colorMatrix = this.CreateColorMatrix(0.0f, 1f, 0.0f);
        imageAttr = this.CreateImageAttributes();
        imageAttr.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
      }
      if (picture.IsShape)
      {
        this.DrawInlinePictureShape(picture, bounds1, size, image1);
      }
      else
      {
        float lineWidth = this.GetLineWidth(picture);
        if (imageAttr == null)
          this.Graphics.DrawImage(this.GetImage(image1), new RectangleF(bounds1.X + lineWidth, bounds1.Y + lineWidth, size.Width, size.Height));
        else
          this.Graphics.DrawImage(this.GetImage(image1), new Rectangle((int) ((double) bounds1.X + (double) lineWidth), (int) ((double) bounds1.Y + (double) lineWidth), (int) size.Width, (int) size.Height), 0, 0, image1.Width, image1.Height, GraphicsUnit.Pixel, imageAttr);
        if ((double) lineWidth > 0.0)
        {
          Pen pen = this.CreatePen(picture.PictureShape);
          pen.Width = lineWidth;
          this.Graphics.DrawRectangle(pen, bounds1.X + lineWidth / 2f, bounds1.Y + lineWidth / 2f, bounds1.Width - lineWidth, bounds1.Height - lineWidth);
        }
      }
      image1?.Dispose();
      if (this.currHyperlink != null)
      {
        this.AddHyperLink(this.currHyperlink, ltwidget.Bounds);
        if (picture.NextSibling is WFieldMark && (picture.NextSibling as WFieldMark).Type == FieldMarkType.FieldEnd)
          this.currHyperlink = (Hyperlink) null;
      }
      this.ResetTransform();
    }
    this.ResetClip();
  }

  private ColorMatrix CreateColorMatrix(float b, float c, float s)
  {
    float num1 = 0.3086f;
    float num2 = 0.6094f;
    float num3 = 0.082f;
    float num4 = (float) ((1.0 - (double) c) / 2.0);
    float num5 = (1f - s) * num1;
    float num6 = (1f - s) * num2;
    float num7 = (1f - s) * num3;
    return this.CreateColorMatrix(new float[5][]
    {
      new float[5]
      {
        c * (num5 + s),
        c * num5,
        c * num5,
        0.0f,
        0.0f
      },
      new float[5]
      {
        c * num6,
        c * (num6 + s),
        c * num6,
        0.0f,
        0.0f
      },
      new float[5]
      {
        c * num7,
        c * num7,
        c * (num7 + s),
        0.0f,
        0.0f
      },
      new float[5]{ 0.0f, 0.0f, 0.0f, 1f, 0.0f },
      new float[5]{ num4 + b, num4 + b, num4 + b, 0.0f, 1f }
    });
  }

  private Bitmap UpdateColoring(Image original, ImageAttributes attributes)
  {
    Bitmap bitmap = new Bitmap(original.Width, original.Height);
    Graphics graphics = Graphics.FromImage((Image) bitmap);
    graphics.FillRectangle(Brushes.White, 0, 0, original.Width, original.Height);
    graphics.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height), 0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
    graphics.Dispose();
    return bitmap;
  }

  private bool IsTableInTextBoxOrShape(Entity entity, bool checkTextBoxOnly)
  {
    if (!(entity is WTableCell))
      return false;
    while (entity is WTableCell)
      entity = (entity as WTableCell).OwnerRow.OwnerTable.Owner;
    if (checkTextBoxOnly)
      return entity.Owner is WTextBox;
    return entity.Owner is WTextBox || entity.Owner is Shape;
  }

  public RectangleF GetBoundingBoxCoordinates(RectangleF bounds, float angle)
  {
    if ((double) bounds.Width <= 0.0 || (double) bounds.Height <= 0.0)
      return bounds;
    GraphicsPath graphicsPath = this.CreateGraphicsPath();
    graphicsPath.AddRectangle(bounds);
    graphicsPath.Transform(this.GetTransformMatrix(bounds, angle));
    RectangleF boundingBox = this.CalculateBoundingBox(graphicsPath.PathPoints);
    graphicsPath.Dispose();
    return boundingBox;
  }

  private RectangleF CalculateBoundingBox(PointF[] imageCoordinates)
  {
    float x1 = imageCoordinates[0].X;
    float x2 = imageCoordinates[3].X;
    float y1 = imageCoordinates[0].Y;
    float y2 = imageCoordinates[3].Y;
    for (int index = 0; index < 4; ++index)
    {
      if ((double) imageCoordinates[index].X < (double) x1)
        x1 = imageCoordinates[index].X;
      if ((double) imageCoordinates[index].X > (double) x2)
        x2 = imageCoordinates[index].X;
      if ((double) imageCoordinates[index].Y < (double) y1)
        y1 = imageCoordinates[index].Y;
      if ((double) imageCoordinates[index].Y > (double) y2)
        y2 = imageCoordinates[index].Y;
    }
    return new RectangleF(x1, y1, x2 - x1, y2 - y1);
  }

  internal Matrix GetTransformMatrix(RectangleF bounds, float angle)
  {
    Matrix transformMatrix = new Matrix();
    PointF point = new PointF(bounds.X + bounds.Width / 2f, bounds.Y + bounds.Height / 2f);
    transformMatrix.RotateAt(angle, point, MatrixOrder.Append);
    return transformMatrix;
  }

  private Matrix GetMatrixValuesFromSkia(Matrix matrix)
  {
    Matrix matrixValuesFromSkia = new Matrix();
    matrixValuesFromSkia.Elements[0] = matrix.Elements[0];
    matrixValuesFromSkia.Elements[1] = matrix.Elements[3];
    matrixValuesFromSkia.Elements[2] = matrix.Elements[1];
    matrixValuesFromSkia.Elements[3] = matrix.Elements[4];
    matrixValuesFromSkia.Elements[4] = matrix.Elements[2];
    matrixValuesFromSkia.Elements[5] = matrix.Elements[5];
    return matrixValuesFromSkia;
  }

  private Image ConvertOleEquationAsBitmap(WPicture picture, Image image)
  {
    WOleObject previousSibling = !(picture.PreviousSibling is WFieldMark) || !(picture.PreviousSibling.PreviousSibling is WTextRange) || !(picture.PreviousSibling.PreviousSibling.PreviousSibling is WOleObject) ? (WOleObject) null : picture.PreviousSibling.PreviousSibling.PreviousSibling as WOleObject;
    if (previousSibling == null || previousSibling.OleObjectType != OleObjectType.Equation || !image.RawFormat.Equals((object) ImageFormat.Wmf))
      return image;
    Bitmap bitmap = new Bitmap(image.Width, image.Height);
    Graphics graphics = Graphics.FromImage((Image) bitmap);
    graphics.FillRectangle(Brushes.White, 0, 0, image.Width, image.Height);
    graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
    graphics.Dispose();
    return (Image) bitmap;
  }

  private void DrawInlinePictureShape(
    WPicture picture,
    RectangleF bounds,
    SizeF size,
    Image image)
  {
    float lineWidth1 = this.GetLineWidth(picture.PictureShape.PictureDescriptor.BorderLeft);
    float lineWidth2 = this.GetLineWidth(picture.PictureShape.PictureDescriptor.BorderTop);
    float lineWidth3 = this.GetLineWidth(picture.PictureShape.PictureDescriptor.BorderRight);
    float lineWidth4 = this.GetLineWidth(picture.PictureShape.PictureDescriptor.BorderBottom);
    if (this.PreserveOleEquationAsBitmap)
      image = this.ConvertOleEquationAsBitmap(picture, image);
    this.Graphics.DrawImage(this.GetImage(image), new RectangleF(bounds.X + lineWidth1, bounds.Y + lineWidth2, size.Width, size.Height));
    Color color1 = picture.PictureShape.PictureDescriptor.BorderBottom.LineColorExt;
    Color color2 = picture.PictureShape.PictureDescriptor.BorderLeft.LineColorExt;
    Color color3 = picture.PictureShape.PictureDescriptor.BorderRight.LineColorExt;
    Color color4 = picture.PictureShape.PictureDescriptor.BorderTop.LineColorExt;
    if (picture.PictureShape.ShapeContainer != null && picture.PictureShape.ShapeContainer.ShapePosition != null)
    {
      if (picture.PictureShape.ShapeContainer.ShapePosition.Properties.ContainsKey(924))
        color2 = WordColor.ConvertRGBToColor(picture.PictureShape.ShapeContainer.ShapePosition.GetPropertyValue(924));
      if (picture.PictureShape.ShapeContainer.ShapePosition.Properties.ContainsKey(926))
        color3 = WordColor.ConvertRGBToColor(picture.PictureShape.ShapeContainer.ShapePosition.GetPropertyValue(926));
      if (picture.PictureShape.ShapeContainer.ShapePosition.Properties.ContainsKey(923))
        color4 = WordColor.ConvertRGBToColor(picture.PictureShape.ShapeContainer.ShapePosition.GetPropertyValue(923));
      if (picture.PictureShape.ShapeContainer.ShapePosition.Properties.ContainsKey(925))
        color1 = WordColor.ConvertRGBToColor(picture.PictureShape.ShapeContainer.ShapePosition.GetPropertyValue(925));
    }
    if (color2.IsEmpty || color2.ToArgb() == 0)
      color2 = Color.Black;
    if (color3.IsEmpty || color3.ToArgb() == 0)
      color3 = Color.Black;
    if (color1.IsEmpty || color1.ToArgb() == 0)
      color1 = Color.Black;
    if (color4.IsEmpty || color4.ToArgb() == 0)
      color4 = Color.Black;
    Pen pen = this.CreatePen(Color.Black);
    if ((double) lineWidth1 > 0.0)
    {
      pen.Color = color2;
      pen.Width = lineWidth1;
      pen = this.GetPictureBorderPen(picture.PictureShape, picture.PictureShape.PictureDescriptor.BorderLeft, pen);
      this.Graphics.DrawLine(pen, new PointF(bounds.Left + lineWidth1 / 2f, bounds.Top), new PointF(bounds.Left + lineWidth1 / 2f, bounds.Bottom));
    }
    if ((double) lineWidth2 > 0.0)
    {
      pen.Color = color4;
      pen.Width = lineWidth2;
      pen = this.GetPictureBorderPen(picture.PictureShape, picture.PictureShape.PictureDescriptor.BorderTop, pen);
      this.Graphics.DrawLine(pen, new PointF(bounds.Left, bounds.Top + lineWidth2 / 2f), new PointF(bounds.Right, bounds.Top + lineWidth2 / 2f));
    }
    if ((double) lineWidth3 > 0.0)
    {
      pen.Color = color3;
      pen.Width = lineWidth3;
      pen = this.GetPictureBorderPen(picture.PictureShape, picture.PictureShape.PictureDescriptor.BorderRight, pen);
      this.Graphics.DrawLine(pen, new PointF(bounds.Right - lineWidth3 / 2f, bounds.Top), new PointF(bounds.Right - lineWidth3 / 2f, bounds.Bottom));
    }
    if ((double) lineWidth4 <= 0.0)
      return;
    pen.Color = color1;
    pen.Width = lineWidth4;
    this.Graphics.DrawLine(this.GetPictureBorderPen(picture.PictureShape, picture.PictureShape.PictureDescriptor.BorderBottom, pen), new PointF(bounds.Left, bounds.Bottom - lineWidth4 / 2f), new PointF(bounds.Right, bounds.Bottom - lineWidth4 / 2f));
  }

  private Pen CreatePen(InlineShapeObject inlineShapeObject)
  {
    Pen pen = this.CreatePen(this.GetColorBorder(inlineShapeObject));
    Pen dashAndLineStyle = this.GetDashAndLineStyle(inlineShapeObject, pen);
    dashAndLineStyle.LineJoin = this.GetLineJoin((Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.LineJoin) inlineShapeObject.ShapeContainer.GetPropertyValue(470));
    dashAndLineStyle.DashCap = this.GetLineCap((Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.LineCap) inlineShapeObject.ShapeContainer.GetPropertyValue(471));
    return dashAndLineStyle;
  }

  private Color GetColorBorder(InlineShapeObject inlineShapeObject)
  {
    return inlineShapeObject.ShapeContainer.ShapeOptions.Properties.ContainsKey(448) ? WordColor.ConvertRGBToColor(inlineShapeObject.ShapeContainer.ShapeOptions.GetPropertyValue(448)) : Color.Black;
  }

  private DashCap GetLineCap(Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.LineCap lineCap)
  {
    switch (lineCap)
    {
      case Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.LineCap.Round:
        return DashCap.Round;
      case Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.LineCap.Square:
        return DashCap.Triangle;
      default:
        return DashCap.Flat;
    }
  }

  private System.Drawing.Drawing2D.LineJoin GetLineJoin(Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.LineJoin lineJoin)
  {
    switch (lineJoin)
    {
      case Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.LineJoin.Bevel:
        return System.Drawing.Drawing2D.LineJoin.Bevel;
      case Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.LineJoin.Miter:
        return System.Drawing.Drawing2D.LineJoin.Miter;
      default:
        return System.Drawing.Drawing2D.LineJoin.Round;
    }
  }

  private Pen GetPictureBorderPen(InlineShapeObject inlineShape, BorderCode borderCode, Pen pen)
  {
    TextBoxLineStyle lineStyle = TextBoxLineStyle.Simple;
    return this.GetDashStyle(inlineShape.GetDashStyle((BorderStyle) borderCode.BorderType, ref lineStyle), pen);
  }

  private Pen GetDashAndLineStyle(InlineShapeObject inlineShape, Pen pen)
  {
    LineDashing propertyValue1 = (LineDashing) inlineShape.ShapeContainer.GetPropertyValue(462);
    TextBoxLineStyle propertyValue2 = (TextBoxLineStyle) inlineShape.ShapeContainer.GetPropertyValue(461);
    pen = this.GetDashStyle(propertyValue1, pen);
    switch (propertyValue2)
    {
      case TextBoxLineStyle.Double:
        pen.CompoundArray = new float[4]
        {
          0.0f,
          0.3333333f,
          0.6666667f,
          1f
        };
        break;
      case TextBoxLineStyle.ThickThin:
        pen.CompoundArray = new float[4]
        {
          0.0f,
          0.6f,
          0.73333f,
          1f
        };
        break;
      case TextBoxLineStyle.ThinThick:
        pen.CompoundArray = new float[4]
        {
          0.0f,
          0.16666f,
          0.3f,
          1f
        };
        break;
      case TextBoxLineStyle.Triple:
        pen.CompoundArray = new float[6]
        {
          0.0f,
          0.1666667f,
          0.3333333f,
          0.6666667f,
          0.8333333f,
          1f
        };
        break;
    }
    return pen;
  }

  private Pen GetDashStyle(LineDashing lineDashing, Pen pen)
  {
    switch (lineDashing)
    {
      case LineDashing.Solid:
        pen.DashStyle = DashStyle.Solid;
        break;
      case LineDashing.Dash:
        pen.DashPattern = new float[2]{ 3f, 0.5f };
        break;
      case LineDashing.Dot:
        pen.DashStyle = DashStyle.Dot;
        break;
      case LineDashing.DashGEL:
        pen.DashStyle = DashStyle.Dash;
        break;
      case LineDashing.LongDashGEL:
        pen.DashPattern = new float[2]{ 8f, 2f };
        break;
      case LineDashing.DashDotGEL:
        pen.DashStyle = DashStyle.DashDot;
        break;
      case LineDashing.LongDashDotGEL:
        pen.DashPattern = new float[4]{ 8f, 2f, 1f, 2f };
        break;
      case LineDashing.LongDashDotDotGEL:
        pen.DashPattern = new float[6]
        {
          8f,
          2f,
          1f,
          2f,
          1f,
          2f
        };
        break;
    }
    return pen;
  }

  private void CropImageBounds(WPicture picture, ref RectangleF bounds, ref SizeF size)
  {
    TileRectangle fillRectangle = picture.FillRectangle;
    float num1 = size.Height - (float) ((double) size.Height * ((double) fillRectangle.TopOffset + (double) fillRectangle.BottomOffset) / 100.0);
    float num2 = size.Width - (float) ((double) size.Width * ((double) fillRectangle.LeftOffset + (double) fillRectangle.RightOffset) / 100.0);
    float num3 = size.Height * 100f / num1;
    float num4 = (float) ((double) (size.Width * 100f / num2) * (double) size.Width / 100.0);
    float num5 = (float) ((double) num3 * (double) size.Height / 100.0);
    if ((double) fillRectangle.LeftOffset < 0.0 && (double) fillRectangle.RightOffset < 0.0)
    {
      bounds.X += (float) ((double) fillRectangle.LeftOffset / ((double) fillRectangle.LeftOffset + (double) fillRectangle.RightOffset) * ((double) size.Width - (double) num4));
      size.Width = num4;
    }
    else if ((double) fillRectangle.LeftOffset < 0.0)
    {
      bounds.X -= (float) ((double) fillRectangle.LeftOffset * (double) size.Width / 100.0);
      size.Width += (float) ((double) fillRectangle.LeftOffset * (double) size.Width / 100.0);
    }
    else if ((double) fillRectangle.RightOffset < 0.0)
      size.Width += (float) ((double) fillRectangle.RightOffset * (double) size.Width / 100.0);
    if ((double) fillRectangle.TopOffset < 0.0 && (double) fillRectangle.BottomOffset < 0.0)
    {
      bounds.Y += (float) ((double) fillRectangle.TopOffset / ((double) fillRectangle.TopOffset + (double) fillRectangle.BottomOffset) * ((double) size.Height - (double) num5));
      size.Height = num5;
    }
    else if ((double) fillRectangle.TopOffset < 0.0)
    {
      bounds.Y -= (float) ((double) fillRectangle.TopOffset * (double) size.Height / 100.0);
      size.Height += (float) ((double) fillRectangle.TopOffset * (double) size.Height / 100.0);
    }
    else
    {
      if ((double) fillRectangle.BottomOffset >= 0.0)
        return;
      size.Height += (float) ((double) fillRectangle.BottomOffset * (double) size.Height / 100.0);
    }
  }

  private RectangleF CropPosition(WPicture picture)
  {
    RectangleF rectangleF = new RectangleF();
    float num1 = 0.0f;
    float num2 = 0.0f;
    TileRectangle fillRectangle = picture.FillRectangle;
    if ((double) fillRectangle.LeftOffset > 0.0)
      rectangleF.X = (float) ((double) fillRectangle.LeftOffset * (double) picture.GetImage(picture.ImageBytes, false).Width / 100.0);
    if ((double) fillRectangle.TopOffset > 0.0)
      rectangleF.Y = (float) ((double) fillRectangle.TopOffset * (double) picture.GetImage(picture.ImageBytes, false).Height / 100.0);
    if ((double) fillRectangle.RightOffset > 0.0)
      num1 = (float) ((double) fillRectangle.RightOffset * (double) picture.GetImage(picture.ImageBytes, false).Width / 100.0);
    if ((double) fillRectangle.BottomOffset > 0.0)
      num2 = (float) ((double) fillRectangle.BottomOffset * (double) picture.GetImage(picture.ImageBytes, false).Height / 100.0);
    rectangleF.Width = (float) picture.GetImage(picture.ImageBytes, false).Width - (rectangleF.X + num1);
    rectangleF.Height = (float) picture.GetImage(picture.ImageBytes, false).Height - (rectangleF.Y + num2);
    rectangleF.Width = (double) rectangleF.Width < 1.0 ? 1f : ((double) rectangleF.Width > (double) picture.GetImage(picture.ImageBytes, false).Width ? (float) picture.GetImage(picture.ImageBytes, false).Width : rectangleF.Width);
    rectangleF.Height = (double) rectangleF.Height < 1.0 ? 1f : ((double) rectangleF.Height > (double) picture.GetImage(picture.ImageBytes, false).Height ? (float) picture.GetImage(picture.ImageBytes, false).Height : rectangleF.Height);
    return rectangleF;
  }

  internal void DrawEquationField(
    FontScriptType scriptType,
    LayoutedEQFields ltEQField,
    WCharacterFormat charFormat)
  {
    Brush brush = (Brush) this.GetBrush(this.GetTextColor(charFormat));
    StringFormat format = new StringFormat(this.StringFormt);
    format.FormatFlags &= ~StringFormatFlags.DirectionRightToLeft;
    this.EnsureComplexScript(charFormat);
    for (int index = 0; index < ltEQField.ChildEQFileds.Count; ++index)
    {
      if (ltEQField.ChildEQFileds[index] is TextEQField)
      {
        TextEQField childEqFiled = ltEQField.ChildEQFileds[index] as TextEQField;
        Font font = childEqFiled.Font != null ? childEqFiled.Font : charFormat.GetFontToRender(scriptType);
        float num = 0.0f;
        string fontNameToRender = charFormat.GetFontNameToRender(scriptType);
        if (!this.IsListCharacter && (fontNameToRender == "DIN Offc" || fontNameToRender == "DIN OT") && this.currParagraph != null && this.currParagraph.IsContainDinOffcFont())
          num = childEqFiled.Bounds.Height * 0.2f;
        RectangleF layoutRectangle = new RectangleF(childEqFiled.Bounds.X, childEqFiled.Bounds.Y + num, childEqFiled.Bounds.Width, childEqFiled.Bounds.Height);
        this.Graphics.DrawString(childEqFiled.Text, font, brush, layoutRectangle, format);
      }
      else if (ltEQField.ChildEQFileds[index] is LineEQField)
      {
        LineEQField childEqFiled = ltEQField.ChildEQFileds[index] as LineEQField;
        this.Graphics.DrawLine(this.CreatePen(Color.Black, 0.5f), childEqFiled.Point1, childEqFiled.Point2);
      }
      else if (ltEQField.ChildEQFileds[index] != null)
      {
        if (ltEQField.ChildEQFileds[index].SwitchType == LayoutedEQFields.EQSwitchType.Radical)
          this.DrawRadicalSwitch(scriptType, ltEQField.ChildEQFileds[index], charFormat);
        else if (ltEQField.ChildEQFileds[index].SwitchType == LayoutedEQFields.EQSwitchType.Array)
          this.DrawArraySwitch(scriptType, ltEQField.ChildEQFileds[index], charFormat, ltEQField.ChildEQFileds[index].Alignment);
        else
          this.DrawEquationField(scriptType, ltEQField.ChildEQFileds[index], charFormat);
      }
    }
  }

  private void DrawArraySwitch(
    FontScriptType scriptType,
    LayoutedEQFields ltEQField,
    WCharacterFormat charFormat,
    StringAlignment arraySwitchAlignment)
  {
    Brush brush = (Brush) this.GetBrush(this.GetTextColor(charFormat));
    StringFormat format = new StringFormat(this.StringFormt);
    format.Alignment = arraySwitchAlignment;
    format.FormatFlags &= ~StringFormatFlags.DirectionRightToLeft;
    this.EnsureComplexScript(charFormat);
    for (int index = 0; index < ltEQField.ChildEQFileds.Count; ++index)
    {
      if (ltEQField.ChildEQFileds[index] is TextEQField)
      {
        TextEQField childEqFiled = ltEQField.ChildEQFileds[index] as TextEQField;
        Font font = childEqFiled.Font != null ? childEqFiled.Font : charFormat.GetFontToRender(scriptType);
        float num = 0.0f;
        string fontNameToRender = charFormat.GetFontNameToRender(scriptType);
        if (!this.IsListCharacter && (fontNameToRender == "DIN Offc" || fontNameToRender == "DIN OT") && this.currParagraph != null && this.currParagraph.IsContainDinOffcFont())
          num = childEqFiled.Bounds.Height * 0.2f;
        RectangleF layoutRectangle = new RectangleF(childEqFiled.Bounds.X, childEqFiled.Bounds.Y + num, childEqFiled.Bounds.Width, childEqFiled.Bounds.Height);
        this.Graphics.DrawString(childEqFiled.Text, font, brush, layoutRectangle, format);
      }
      else if (ltEQField.ChildEQFileds[index] is LineEQField)
      {
        LineEQField childEqFiled = ltEQField.ChildEQFileds[index] as LineEQField;
        this.Graphics.DrawLine(this.CreatePen(Color.Black, 0.5f), childEqFiled.Point1, childEqFiled.Point2);
      }
      else if (ltEQField.ChildEQFileds[index] != null)
        this.DrawArraySwitch(scriptType, ltEQField.ChildEQFileds[index], charFormat, arraySwitchAlignment);
    }
  }

  private void DrawRadicalSwitch(
    FontScriptType scriptType,
    LayoutedEQFields ltEQField,
    WCharacterFormat charFormat)
  {
    Brush brush = (Brush) this.GetBrush(this.GetTextColor(charFormat));
    StringFormat format = new StringFormat(this.StringFormt);
    format.FormatFlags &= ~StringFormatFlags.DirectionRightToLeft;
    this.EnsureComplexScript(charFormat);
    Pen pen = new Pen(Color.Black, 0.7f);
    pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
    GraphicsPath path = new GraphicsPath();
    for (int index = 0; index < ltEQField.ChildEQFileds.Count; ++index)
    {
      for (; index < ltEQField.ChildEQFileds.Count && ltEQField.ChildEQFileds[index] is LineEQField; ++index)
      {
        LineEQField childEqFiled = ltEQField.ChildEQFileds[index] as LineEQField;
        path.AddLine(childEqFiled.Point1, childEqFiled.Point2);
      }
      if (path.PointCount > 0)
      {
        this.Graphics.DrawPath(pen, path);
        path.Reset();
      }
      if (index < ltEQField.ChildEQFileds.Count && ltEQField.ChildEQFileds[index] is TextEQField)
      {
        TextEQField childEqFiled = ltEQField.ChildEQFileds[index] as TextEQField;
        Font font = childEqFiled.Font != null ? childEqFiled.Font : charFormat.GetFontToRender(scriptType);
        float num = 0.0f;
        string fontNameToRender = charFormat.GetFontNameToRender(scriptType);
        if (!this.IsListCharacter && (fontNameToRender == "DIN Offc" || fontNameToRender == "DIN OT") && this.currParagraph != null && this.currParagraph.IsContainDinOffcFont())
          num = childEqFiled.Bounds.Height * 0.2f;
        RectangleF layoutRectangle = new RectangleF(childEqFiled.Bounds.X, childEqFiled.Bounds.Y + num, childEqFiled.Bounds.Width, childEqFiled.Bounds.Height);
        this.Graphics.DrawString(childEqFiled.Text, font, brush, layoutRectangle, format);
      }
      else if (index < ltEQField.ChildEQFileds.Count)
        this.DrawEquationField(scriptType, ltEQField.ChildEQFileds[index], charFormat);
    }
  }

  private void EnsureComplexScript(WCharacterFormat charFormat)
  {
    if (this.EnableComplexScript || !charFormat.ComplexScript)
      return;
    this.EnableComplexScript = true;
  }

  private void AlignEqFieldSwitches(LayoutedEQFields ltEQField, float xPosition, float yPosition)
  {
    float yPosition1 = yPosition - this.GetTopMostY(ltEQField, yPosition);
    if ((double) yPosition1 == 0.0)
      return;
    this.ShiftEqFieldXYPosition(ltEQField, xPosition, yPosition1);
  }

  internal void ShiftEqFieldXYPosition(
    LayoutedEQFields ltEQField,
    float xPosition,
    float yPosition)
  {
    switch (ltEQField)
    {
      case TextEQField _:
        RectangleF bounds1 = (ltEQField as TextEQField).Bounds;
        (ltEQField as TextEQField).Bounds = new RectangleF(bounds1.X + xPosition, bounds1.Y + yPosition, bounds1.Width, bounds1.Height);
        break;
      case LineEQField _:
        PointF point1 = (ltEQField as LineEQField).Point1;
        PointF point2 = (ltEQField as LineEQField).Point2;
        (ltEQField as LineEQField).Point1 = new PointF(point1.X + xPosition, point1.Y + yPosition);
        (ltEQField as LineEQField).Point2 = new PointF(point2.X + xPosition, point2.Y + yPosition);
        RectangleF bounds2 = (ltEQField as LineEQField).Bounds;
        (ltEQField as LineEQField).Bounds = new RectangleF(bounds2.X + xPosition, bounds2.Y + yPosition, bounds2.Width, bounds2.Height);
        break;
      case null:
        break;
      default:
        ltEQField.Bounds = new RectangleF(ltEQField.Bounds.X + xPosition, ltEQField.Bounds.Y + yPosition, ltEQField.Bounds.Width, ltEQField.Bounds.Height);
        for (int index = 0; index < ltEQField.ChildEQFileds.Count; ++index)
          this.ShiftEqFieldXYPosition(ltEQField.ChildEQFileds[index], xPosition, yPosition);
        break;
    }
  }

  public void GenerateErrorFieldCode(
    LayoutedEQFields ltEQFiled,
    float xPosition,
    float yPosition,
    WCharacterFormat charFormat)
  {
    TextEQField textEqField = new TextEQField();
    textEqField.Text = "Error!";
    textEqField.Font = charFormat.Document.FontSettings.GetFont("Calibri", 11f, FontStyle.Bold);
    textEqField.Bounds = new RectangleF(new PointF(xPosition, yPosition), this.MeasureString(textEqField.Text, textEqField.Font, (StringFormat) null, charFormat, false));
    float ascent = this.GetAscent(textEqField.Font);
    this.ShiftEqFieldYPosition((LayoutedEQFields) textEqField, -ascent);
    ltEQFiled.ChildEQFileds.Add((LayoutedEQFields) textEqField);
    ltEQFiled.Bounds = textEqField.Bounds;
  }

  public void ShiftEqFieldYPosition(LayoutedEQFields LayoutedEQFields, float yPosition)
  {
    switch (LayoutedEQFields)
    {
      case TextEQField _:
        TextEQField textEqField = LayoutedEQFields as TextEQField;
        textEqField.Bounds = new RectangleF(textEqField.Bounds.X, textEqField.Bounds.Y + yPosition, textEqField.Bounds.Width, textEqField.Bounds.Height);
        break;
      case LineEQField _:
        LineEQField lineEqField = LayoutedEQFields as LineEQField;
        lineEqField.Point1 = new PointF(lineEqField.Point1.X, lineEqField.Point1.Y + yPosition);
        lineEqField.Point2 = new PointF(lineEqField.Point2.X, lineEqField.Point2.Y + yPosition);
        lineEqField.Bounds = new RectangleF(lineEqField.Bounds.X, lineEqField.Bounds.Y + yPosition, lineEqField.Bounds.Width, lineEqField.Bounds.Height);
        break;
      case null:
        break;
      default:
        LayoutedEQFields.Bounds = new RectangleF(LayoutedEQFields.Bounds.X, LayoutedEQFields.Bounds.Y + yPosition, LayoutedEQFields.Bounds.Width, LayoutedEQFields.Bounds.Height);
        for (int index = 0; index < LayoutedEQFields.ChildEQFileds.Count; ++index)
          this.ShiftEqFieldYPosition(LayoutedEQFields.ChildEQFileds[index], yPosition);
        break;
    }
  }

  internal float GetTopMostY(LayoutedEQFields ltEQField, float minY)
  {
    switch (ltEQField)
    {
      case TextEQField _:
        if ((double) ltEQField.Bounds.Y < (double) minY)
        {
          minY = ltEQField.Bounds.Y;
          goto case null;
        }
        goto case null;
      case LineEQField _:
        if ((double) (ltEQField as LineEQField).Point1.Y < (double) minY)
        {
          minY = (ltEQField as LineEQField).Point1.Y;
          goto case null;
        }
        goto case null;
      case null:
        return minY;
      default:
        if ((double) ltEQField.Bounds.Y < (double) minY)
          minY = ltEQField.Bounds.Y;
        using (List<LayoutedEQFields>.Enumerator enumerator = ltEQField.ChildEQFileds.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            float topMostY = this.GetTopMostY(enumerator.Current, minY);
            if ((double) topMostY < (double) minY)
              minY = topMostY;
          }
          goto case null;
        }
    }
  }

  internal void DrawString(
    FontScriptType scriptType,
    string text,
    WCharacterFormat charFormat,
    WParagraphFormat paraFormat,
    RectangleF bounds,
    float clipWidth,
    LayoutedWidget ltWidget)
  {
    if (text == null || (double) bounds.Height == 0.0 || (double) clipWidth == 0.0)
      return;
    text = text.Replace('\u001E'.ToString(), "-");
    text = text.Replace('\u00AD'.ToString(), "-");
    text = text.Replace('\u200D'.ToString(), "");
    bool flag1 = true;
    if (ltWidget != null && ltWidget.Widget.LayoutInfo is TabsLayoutInfo)
      flag1 = charFormat.HighlightColor.IsEmpty;
    if (text.Length == 0 && flag1)
      return;
    if (charFormat.Bidi || charFormat.ComplexScript)
    {
      if (!charFormat.GetBoldToRender())
        charFormat.Bold = false;
      else if (!charFormat.Bold)
        charFormat.Bold = true;
      if (!charFormat.GetItalicToRender())
        charFormat.Italic = false;
      else if (!charFormat.Italic)
        charFormat.Italic = true;
    }
    bool flag2 = this.HasUnderlineOrStricthrough(this.currTextRange, charFormat, scriptType);
    bool flag3 = this.m_commentId != null;
    Font font1 = this.IsListCharacter || this.currTextRange == null ? this.GetFont(scriptType, charFormat, text) : this.GetFont(this.currTextRange, charFormat, text);
    Color black = Color.Black;
    StringFormat stringFormat = new StringFormat(this.StringFormt);
    if (charFormat.Bidi)
      stringFormat.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
    else
      stringFormat.FormatFlags &= ~StringFormatFlags.DirectionRightToLeft;
    if (font1.Name == "Arial Narrow" && font1.Style == FontStyle.Bold)
      text = text.Replace(ControlChar.NonBreakingSpaceChar, ControlChar.SpaceChar);
    if (this.IsSoftHyphen(ltWidget))
    {
      text = "-";
      if ((double) bounds.Width == 0.0)
        bounds.Width = ltWidget.Bounds.Width;
    }
    if (this.IsTOC(this.currTextRange) && this.currParagraph != null && this.currParagraph.ParaStyle != null)
    {
      stringFormat = new StringFormat(this.StringFormt);
      if (this.currParagraph.ParaStyle.CharacterFormat.Bidi)
        stringFormat.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
      else
        stringFormat.FormatFlags &= ~StringFormatFlags.DirectionRightToLeft;
    }
    if (ltWidget != null && ltWidget.Widget.LayoutInfo is ParagraphLayoutInfo && this.currParagraph != null)
    {
      stringFormat = new StringFormat(this.StringFormt);
      if (this.currParagraph.ParagraphFormat.Bidi)
        stringFormat.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
      else
        stringFormat.FormatFlags &= ~StringFormatFlags.DirectionRightToLeft;
    }
    Brush brush = (Brush) this.GetBrush(this.GetTextColor(charFormat));
    float height = bounds.Height;
    float y = bounds.Y;
    if (paraFormat != null)
    {
      height = bounds.Height - paraFormat.Borders.Bottom.LineWidth - paraFormat.Borders.Top.LineWidth;
      y = bounds.Y + paraFormat.Borders.Top.LineWidth;
      if (this.currParagraph != null && this.currParagraph.OwnerTextBody.Owner is Shape && (double) (this.currParagraph.OwnerTextBody.Owner as Shape).TextLayoutingBounds.Y == (double) y)
      {
        y += (this.currParagraph.OwnerTextBody.Owner as Shape).LineFormat.Weight;
        height -= (this.currParagraph.OwnerTextBody.Owner as Shape).LineFormat.Weight;
      }
      else if (this.currParagraph != null && this.currParagraph.OwnerTextBody.Owner is ChildShape && (double) (this.currParagraph.OwnerTextBody.Owner as ChildShape).TextLayoutingBounds.Y == (double) y)
      {
        y += (this.currParagraph.OwnerTextBody.Owner as ChildShape).LineFormat.Weight;
        height -= (this.currParagraph.OwnerTextBody.Owner as ChildShape).LineFormat.Weight;
      }
    }
    this.ResetTransform();
    float scaling = charFormat.Scaling;
    bool isNeedToScale = (double) scaling != 100.0 && ((double) scaling >= 1.0 || (double) scaling <= 600.0);
    PointF empty = PointF.Empty;
    float rotationAngle = 0.0f;
    float rotation = 0.0f;
    bool flipH = false;
    bool flipV = false;
    bool flag4 = false;
    TextWrappingStyle textWrappingStyle = TextWrappingStyle.Square;
    if (this.currTextRange != null)
    {
      if (!(this.currTextRange.Owner is WParagraph wparagraph))
        wparagraph = this.currTextRange.GetOwnerParagraphValue();
      if (wparagraph != null)
      {
        Entity ownerEntity = wparagraph.GetOwnerEntity();
        switch (ownerEntity)
        {
          case ChildShape _:
            ChildShape childShape = ownerEntity as ChildShape;
            rotation = childShape.RotationToRender;
            flipH = childShape.FlipHorizantalToRender;
            flipV = childShape.FlipVerticalToRender;
            flag4 = childShape.TextFrame.Upright;
            flag3 = false;
            break;
          case Shape _:
            Shape shape = ownerEntity as Shape;
            rotation = shape.Rotation;
            flipH = shape.FlipHorizontal;
            flipV = shape.FlipVertical;
            textWrappingStyle = shape.WrapFormat.TextWrappingStyle;
            flag4 = shape.TextFrame.Upright;
            flag3 = false;
            break;
          case WTextBox _:
            WTextBox wtextBox = ownerEntity as WTextBox;
            rotation = wtextBox.Shape != null ? wtextBox.Shape.Rotation : wtextBox.TextBoxFormat.Rotation;
            flipH = wtextBox.TextBoxFormat.FlipHorizontal;
            flipV = wtextBox.TextBoxFormat.FlipVertical;
            textWrappingStyle = wtextBox.TextBoxFormat.TextWrappingStyle;
            flag4 = wtextBox.Shape != null && wtextBox.Shape.TextFrame.Upright;
            flag3 = false;
            break;
          case WTableCell _:
            Entity entity = (Entity) (ownerEntity as WTableCell);
            while (entity is WTableCell)
              entity = (entity as WTableCell).OwnerRow.OwnerTable.Owner;
            if (entity.Owner is WTextBox)
            {
              WTextBox owner = entity.Owner as WTextBox;
              rotation = owner.Shape != null ? owner.Shape.Rotation : owner.TextBoxFormat.Rotation;
              flipH = owner.TextBoxFormat.FlipHorizontal;
              flipV = owner.TextBoxFormat.FlipVertical;
              textWrappingStyle = owner.TextBoxFormat.TextWrappingStyle;
              flag4 = owner.Shape != null && owner.Shape.TextFrame.Upright;
              flag3 = false;
              break;
            }
            if (entity.Owner is Shape)
            {
              Shape owner = entity.Owner as Shape;
              rotation = owner.Rotation;
              flipH = owner.FlipHorizontal;
              flipV = owner.FlipVertical;
              textWrappingStyle = owner.WrapFormat.TextWrappingStyle;
              flag4 = owner.TextFrame.Upright;
              flag3 = false;
              break;
            }
            if (entity.Owner is ChildShape)
            {
              ChildShape owner = entity.Owner as ChildShape;
              rotation = owner.RotationToRender;
              flipH = owner.FlipHorizantalToRender;
              flipV = owner.FlipVerticalToRender;
              flag4 = owner.TextFrame.Upright;
              flag3 = false;
              break;
            }
            break;
        }
      }
    }
    bool drawLines = false;
    if (((double) rotation != 0.0 || (double) rotation == 0.0 && (flipH || flipV)) && textWrappingStyle != TextWrappingStyle.Tight && textWrappingStyle != TextWrappingStyle.Through)
    {
      if ((double) rotation > 360.0)
        rotation %= 360f;
      if ((double) rotation != 0.0 || flipV || flipH)
      {
        if (flipV)
          flipH = true;
        else if (flipH)
          flipH = false;
        if (!flag4)
          this.SetRotateTransform(this.m_rotateTransform, rotation, flipV, flipH);
        drawLines = true;
      }
    }
    RectangleF textBounds = bounds;
    if (this.currTextRange != null && this.currTextRange.m_layoutInfo != null && this.currTextRange.m_layoutInfo.IsVerticalText)
    {
      this.TransformGraphicsPosition(ltWidget, isNeedToScale, ref empty, ref rotationAngle, this.currParagraph);
      drawLines = true;
    }
    float clipTopPosition = this.GetClipTopPosition(bounds, false);
    if ((double) clipWidth == 0.0 || this.IsTextNeedToClip(ltWidget) || this.IsWidgetNeedToClipBasedOnXPosition(ltWidget, ref clipWidth, bounds))
    {
      this.ResetTransform();
    }
    else
    {
      RectangleF clipBounds = this.GetClipBounds(bounds, clipWidth, clipTopPosition);
      bool flag5 = this.IsNeedToClip(clipBounds);
      if (this.ClipBoundsContainer != null && this.ClipBoundsContainer.Count > 0)
      {
        RectangleF rectangleF = this.ClipBoundsContainer.Peek();
        if (this.currParagraph != null && this.currParagraph.GetOwnerEntity() is WTextBox)
        {
          WParagraph ownerParagraph = (this.currParagraph.GetOwnerEntity() as WTextBox).OwnerParagraph;
          if (ownerParagraph != null && ownerParagraph.ParagraphFormat.IsInFrame())
          {
            LayoutedWidget textBoxWidget = this.GetTextBoxWidget(ltWidget);
            rectangleF = textBoxWidget != null ? textBoxWidget.Bounds : rectangleF;
          }
          flag5 = !this.currParagraph.IsInCell && ((double) clipBounds.Bottom > (double) rectangleF.Bottom || (double) clipBounds.X < (double) rectangleF.X);
        }
      }
      if (flag5)
      {
        clipBounds = (double) clipTopPosition <= 0.0 ? this.ClipBoundsContainer.Peek() : this.UpdateClipBounds(clipBounds, false);
        if ((double) clipBounds.Width == 0.0)
        {
          this.ResetTransform();
          return;
        }
      }
      if ((double) bounds.Width > 0.0 && isNeedToScale)
      {
        this.RotateAndScaleTransform(ref bounds, ref clipBounds, scaling, empty, rotationAngle, false);
        clipWidth = clipBounds.Width;
        textBounds = new RectangleF(bounds.X, bounds.Y, bounds.Width, bounds.Height);
      }
      if (flag5 || (double) clipTopPosition > 0.0)
      {
        this.SetClip(clipBounds);
        drawLines = true;
      }
      if (!charFormat.TextBackgroundColor.IsEmpty && !flag3)
        this.Graphics.FillRectangle((Brush) this.GetBrush(charFormat.TextBackgroundColor), bounds.X, y, bounds.Width, height);
      if (!charFormat.HighlightColor.IsEmpty && !flag3)
        this.Graphics.FillRectangle((Brush) this.CreateSolidBrush(this.GetHightLightColor(charFormat.HighlightColor)), bounds.X, y, bounds.Width, height);
      if (this.IsListCharacter)
        bounds.Width = this.MeasureString(text, font1, (StringFormat) null, charFormat, true).Width;
      if ((double) bounds.Width > 0.0 && isNeedToScale && this.IsListCharacter)
        this.RotateAndScaleTransform(ref bounds, ref clipBounds, scaling, empty, rotationAngle, this.IsListCharacter);
      bool isSubSuperScriptNone = charFormat.SubSuperScript == SubSuperScript.None;
      float num = 0.0f;
      string fontNameToRender = charFormat.GetFontNameToRender(scriptType);
      if (!this.IsListCharacter && (fontNameToRender == "DIN Offc" || fontNameToRender == "DIN OT") && this.currParagraph != null && this.currParagraph.IsContainDinOffcFont())
        num = bounds.Height * 0.2f;
      this.EnsureComplexScript(charFormat);
      if (charFormat.Emboss)
      {
        Font font2 = !isSubSuperScriptNone ? charFormat.Document.FontSettings.GetFont(charFormat.GetFontNameToRender(scriptType), this.GetSubSuperScriptFontSize(charFormat.GetFontToRender(scriptType)), charFormat.GetFontToRender(scriptType).Style) : charFormat.GetFontToRender(scriptType);
        if (!charFormat.GetFontNameToRender(scriptType).Equals(font2.Name, StringComparison.OrdinalIgnoreCase) && this.m_privateFontStream != null && this.m_privateFontStream.ContainsKey(charFormat.GetFontNameToRender(scriptType)))
          font2 = this.GetPrivateFont(charFormat.GetFontNameToRender(scriptType), charFormat.GetFontSizeToRender(), font2.Style);
        SolidBrush solidBrush = this.CreateSolidBrush(Color.Gray);
        if (this.IsUnicode(text))
          this.DrawUnicodeString(text, font2, (Brush) solidBrush, new RectangleF(bounds.X - 0.2f, bounds.Y - 0.2f + num, bounds.Width, bounds.Height), stringFormat);
        else
          this.Graphics.DrawString(text, font2, (Brush) solidBrush, new RectangleF(bounds.X - 0.2f, bounds.Y - 0.2f + num, bounds.Width, bounds.Height), stringFormat);
      }
      if (charFormat.Engrave)
      {
        Font font3 = !isSubSuperScriptNone ? charFormat.Document.FontSettings.GetFont(charFormat.GetFontNameToRender(scriptType), charFormat.GetFontSizeToRender() / 1.5f, charFormat.GetFontToRender(scriptType).Style) : charFormat.GetFontToRender(scriptType);
        if (!charFormat.GetFontNameToRender(scriptType).Equals(font3.Name, StringComparison.OrdinalIgnoreCase) && this.m_privateFontStream != null && this.m_privateFontStream.ContainsKey(charFormat.GetFontNameToRender(scriptType)))
          font3 = this.GetPrivateFont(charFormat.GetFontNameToRender(scriptType), font3.Size, font3.Style);
        SolidBrush solidBrush = this.CreateSolidBrush(Color.Gray);
        if (this.IsUnicode(text))
          this.DrawUnicodeString(text, font3, (Brush) solidBrush, new RectangleF(bounds.X - 0.2f, bounds.Y - 0.2f + num, bounds.Width, bounds.Height), stringFormat);
        else
          this.Graphics.DrawString(text, font3, (Brush) solidBrush, new RectangleF(bounds.X - 0.2f, bounds.Y - 0.2f + num, bounds.Width, bounds.Height), stringFormat);
      }
      if (charFormat.AllCaps)
        text = text.ToUpper();
      if (!this.IsOwnerParagraphEmpty(text))
      {
        CharacterRangeType characterRangeType = ltWidget != null ? ltWidget.CharacterRange : CharacterRangeType.LTR;
        if (characterRangeType == CharacterRangeType.RTL)
          this.DrawRTLText(scriptType, characterRangeType, text, charFormat, font1, brush, bounds, stringFormat);
        else if (this.IsUnicode(text))
        {
          this.DrawChineseText(scriptType, characterRangeType, text, charFormat, font1, brush, bounds, stringFormat);
        }
        else
        {
          if (charFormat.BiDirectionalOverride == BiDirectionalOverride.RTL && text.Length > 0)
            this.ReverseString(ref text);
          ParagraphLayoutInfo paragraphLayoutInfo = (ParagraphLayoutInfo) null;
          if (ltWidget != null)
            paragraphLayoutInfo = ltWidget.Widget.LayoutInfo as ParagraphLayoutInfo;
          if (charFormat.SmallCaps && (paragraphLayoutInfo == null || paragraphLayoutInfo.CurrentListType != ListType.Bulleted || !this.IsListCharacter))
          {
            this.DrawSmallCapString(scriptType, characterRangeType, text, charFormat, bounds, font1, stringFormat, brush, (double) charFormat.CharacterSpacing != 0.0);
          }
          else
          {
            Font font4 = !isSubSuperScriptNone ? charFormat.Document.FontSettings.GetFont(fontNameToRender, this.GetSubSuperScriptFontSize(font1), font1.Style) : font1;
            if (!charFormat.GetFontNameToRender(scriptType).Equals(font4.Name, StringComparison.OrdinalIgnoreCase) && this.m_privateFontStream != null && this.m_privateFontStream != null && this.m_privateFontStream.ContainsKey(charFormat.GetFontNameToRender(scriptType)))
              font4 = this.GetPrivateFont(charFormat.GetFontNameToRender(scriptType), font4.Size, font4.Style);
            if ((double) charFormat.CharacterSpacing != 0.0 && (ltWidget != null ? (this.IsTabWidget(ltWidget) ? 0 : (this.currParagraph == null || !this.currParagraph.ParagraphFormat.Bidi ? 1 : (!(ltWidget.Widget.LayoutInfo is ParagraphLayoutInfo) ? 1 : 0))) : 1) != 0)
              this.DrawStringBasedOnCharSpacing(scriptType, characterRangeType, font4, brush, bounds, text, stringFormat, charFormat);
            else
              this.Graphics.DrawString(text, font4, brush, new RectangleF(bounds.X, bounds.Y + num, bounds.Width, bounds.Height), stringFormat);
            textBounds = new RectangleF(bounds.X, bounds.Y + num, bounds.Width, bounds.Height);
          }
        }
      }
      if ((this.ExportBookmarks & ExportBookmarkType.Headings) != ExportBookmarkType.None && (this.currParagraph != null || paraFormat != null) && !this.currParagraph.IsInCell)
      {
        int headingLevel = this.currParagraph.GetHeadingLevel(this.currParagraph.ParaStyle as WParagraphStyle, this.currParagraph);
        if (headingLevel >= 0 && headingLevel < 9 && !this.currParagraph.OmitHeadingStyles() && this.currParagraphIndex != this.currParagraph.Index)
        {
          DocumentLayouter.Bookmarks.Add(new BookmarkPosition(this.IsListCharacter ? $"{text}\t{this.currParagraph.GetDisplayText(this.currParagraph.Items)}" : this.currParagraph.GetDisplayText(this.currParagraph.Items), DocumentLayouter.PageNumber, bounds, headingLevel));
          this.currParagraphIndex = this.currParagraph.Index;
        }
      }
      if (flag2)
      {
        bool isSameLine = false;
        this.CheckPreOrNextSiblingIsTab(ref charFormat, ref textBounds, ltWidget, ref isSameLine);
        this.AddLineToCollection(text, isSubSuperScriptNone, font1, charFormat, drawLines, textBounds, isSameLine);
      }
      this.ResetTransform();
      if (flag5 || (double) clipTopPosition > 0.0)
        this.ResetClip();
      this.IsListCharacter = false;
    }
  }

  private void CheckPreOrNextSiblingIsTab(
    ref WCharacterFormat charFormat,
    ref RectangleF textBounds,
    LayoutedWidget ltWidget,
    ref bool isSameLine)
  {
    LayoutedWidget owner = ltWidget.Owner;
    for (int index = 0; index < owner.ChildWidgets.Count; ++index)
    {
      LayoutedWidget childWidget = owner.ChildWidgets[index];
      if (childWidget.Widget.LayoutInfo is WTextRange.LayoutTabInfo && charFormat.UnderlineStyle != UnderlineStyle.None && (this.IsSame(childWidget.Bounds.X, textBounds.Right, 1) || this.IsSame(childWidget.Bounds.Right, textBounds.X, 1)))
      {
        WCharacterFormat characterFormat = (childWidget.Widget as WTextRange).CharacterFormat;
        if (!(characterFormat.UnderlineColor == charFormat.UnderlineColor) || (double) charFormat.Position != (double) characterFormat.Position || characterFormat.UnderlineStyle != charFormat.UnderlineStyle || charFormat.SubSuperScript != SubSuperScript.None || !(this.GetTextColor(characterFormat) == this.GetTextColor(charFormat)))
          break;
        charFormat = characterFormat;
        textBounds.Y = childWidget.Bounds.Y;
        if (!this.IsSame(childWidget.Bounds.Right, textBounds.X, 1))
          break;
        isSameLine = true;
        break;
      }
    }
  }

  private List<RectangleF> CalculateTextBounds(
    string text,
    RectangleF textBounds,
    Font font,
    StringFormat format,
    WCharacterFormat charFormat)
  {
    Syncfusion.DocIO.DLS.OwnerHolder ownerBase = charFormat.OwnerBase;
    List<RectangleF> rectangleFList = new List<RectangleF>();
    RectangleF rectangleF1 = new RectangleF();
    RectangleF rectangleF2 = textBounds;
    string text1 = (string) null;
    int num = 0;
    for (int index1 = 0; index1 < text.Length; ++index1)
    {
      if (text[index1] != ' ')
      {
        text1 += (string) (object) text[index1];
        if (num != 0)
        {
          rectangleF2.Width = 0.0f;
          for (int index2 = 0; index2 < num; ++index2)
          {
            SizeF sizeF = this.MeasureString(" ", font, format);
            rectangleF2.Width += sizeF.Width;
          }
          rectangleF2.X += rectangleF2.Width;
          num = 0;
        }
      }
      else if (text[index1] == ' ')
      {
        ++num;
        if (text1 != null)
        {
          SizeF sizeF = this.MeasureString(text1, font, format);
          rectangleF2.Width = sizeF.Width;
          rectangleFList.Add(rectangleF2);
          rectangleF2.X += rectangleF2.Width;
          rectangleF2.Width = 0.0f;
          text1 = (string) null;
        }
      }
    }
    if (text1 != null)
    {
      SizeF sizeF = this.MeasureString(text1, font, format);
      rectangleF2.Width = sizeF.Width;
      rectangleFList.Add(rectangleF2);
    }
    return rectangleFList.Count > 0 ? rectangleFList : (List<RectangleF>) null;
  }

  private void AddLineToCollection(
    string text,
    bool isSubSuperScriptNone,
    Font font,
    WCharacterFormat charFormat,
    bool drawLines,
    RectangleF textBounds,
    bool isSameLine)
  {
    Font font1 = !isSubSuperScriptNone ? charFormat.Document.FontSettings.GetFont(font.Name, font.Size / 1.5f, font.Style) : font;
    WCharacterFormat wcharacterFormat = (WCharacterFormat) null;
    StringFormat format = new StringFormat(StringFormat.GenericTypographic);
    format.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;
    format.Trimming = StringTrimming.None;
    RectangleF rectangleF1 = new RectangleF();
    List<RectangleF> rectangleFList = new List<RectangleF>();
    RectangleF strikeThroughValue;
    if (charFormat.UnderlineStyle != UnderlineStyle.None)
    {
      if (drawLines)
      {
        this.DrawUnderLine(charFormat, textBounds);
      }
      else
      {
        if (this.underLineValues != null)
        {
          foreach (WCharacterFormat key in this.underLineValues.Keys)
            wcharacterFormat = key;
        }
        else
          this.underLineValues = new Dictionary<WCharacterFormat, List<RectangleF>>();
        if (wcharacterFormat != null)
        {
          strikeThroughValue = this.underLineValues[wcharacterFormat][this.underLineValues[wcharacterFormat].Count - 1];
          if (charFormat.SubSuperScript == SubSuperScript.SuperScript && wcharacterFormat.SubSuperScript == SubSuperScript.None || (double) charFormat.Position > 0.0 && (double) wcharacterFormat.Position == 0.0)
          {
            if (charFormat.UnderlineStyle == UnderlineStyle.Words)
            {
              List<RectangleF> textBounds1 = this.CalculateTextBounds(text, textBounds, font1, format, charFormat);
              if (textBounds1 != null)
              {
                foreach (RectangleF rectangleF2 in textBounds1)
                {
                  if ((double) strikeThroughValue.Right >= (double) rectangleF2.X)
                  {
                    strikeThroughValue.Width += rectangleF2.Width;
                    this.underLineValues[wcharacterFormat][this.underLineValues[wcharacterFormat].Count - 1] = strikeThroughValue;
                    strikeThroughValue = this.underLineValues[wcharacterFormat][this.underLineValues[wcharacterFormat].Count - 1];
                  }
                  else
                    rectangleFList.Add(rectangleF2);
                }
                if (rectangleFList.Count > 0)
                  this.underLineValues.Add(charFormat, rectangleFList);
              }
            }
            else
            {
              strikeThroughValue.Width = textBounds.X + textBounds.Width - strikeThroughValue.X;
              this.underLineValues[wcharacterFormat][this.underLineValues[wcharacterFormat].Count - 1] = strikeThroughValue;
            }
          }
          else if (charFormat.SubSuperScript == SubSuperScript.None && wcharacterFormat.SubSuperScript == SubSuperScript.SuperScript || (double) charFormat.Position == 0.0 && (double) wcharacterFormat.Position > 0.0)
          {
            if (charFormat.UnderlineStyle == UnderlineStyle.Words)
            {
              List<RectangleF> textBounds2 = this.CalculateTextBounds(text, textBounds, font1, format, charFormat);
              if (textBounds2 != null)
                this.underLineValues.Add(charFormat, textBounds2);
            }
            else
            {
              strikeThroughValue.Width += textBounds.Width;
              this.underLineValues.Remove(wcharacterFormat);
              rectangleFList.Add(strikeThroughValue);
              this.underLineValues.Add(charFormat, rectangleFList);
            }
          }
          else if (isSameLine || this.IsSameLine(strikeThroughValue.Right, textBounds.X, charFormat, wcharacterFormat) || this.IsSameLine(strikeThroughValue.X, textBounds.Right, charFormat, wcharacterFormat))
          {
            if (this.IsSameLine(strikeThroughValue.X, textBounds.Right, charFormat, wcharacterFormat))
              strikeThroughValue.X = textBounds.X;
            strikeThroughValue.Width += textBounds.Width;
            if (charFormat.UnderlineStyle == UnderlineStyle.Words)
            {
              List<RectangleF> textBounds3 = this.CalculateTextBounds(text, textBounds, font1, format, charFormat);
              if (textBounds3 != null)
                this.underLineValues.Add(charFormat, textBounds3);
            }
            else if ((double) font1.Size > (double) wcharacterFormat.Font.Size)
            {
              strikeThroughValue.Y = textBounds.Y;
              this.underLineValues.Remove(wcharacterFormat);
              rectangleFList.Add(strikeThroughValue);
              this.underLineValues.Add(charFormat, rectangleFList);
            }
            else
            {
              rectangleFList.Add(strikeThroughValue);
              this.underLineValues[wcharacterFormat] = rectangleFList;
            }
          }
          else if (charFormat.UnderlineStyle == UnderlineStyle.Words)
          {
            List<RectangleF> textBounds4 = this.CalculateTextBounds(text, textBounds, font1, format, charFormat);
            if (textBounds4 != null)
              this.underLineValues.Add(charFormat, textBounds4);
          }
          else
          {
            rectangleFList.Add(textBounds);
            this.underLineValues.Add(charFormat, rectangleFList);
          }
        }
        else if (charFormat.UnderlineStyle == UnderlineStyle.Words)
        {
          List<RectangleF> textBounds5 = this.CalculateTextBounds(text, textBounds, font1, format, charFormat);
          if (textBounds5 != null)
            this.underLineValues.Add(charFormat, textBounds5);
        }
        else
        {
          rectangleFList.Add(textBounds);
          this.underLineValues.Add(charFormat, rectangleFList);
        }
      }
    }
    WCharacterFormat key1 = (WCharacterFormat) null;
    if (!charFormat.Strikeout && !charFormat.DoubleStrike)
      return;
    if (drawLines)
    {
      this.DrawStrikeThrough(charFormat, textBounds);
    }
    else
    {
      if (this.strikeThroughValues != null)
      {
        foreach (WCharacterFormat key2 in this.strikeThroughValues.Keys)
          key1 = key2;
      }
      else
        this.strikeThroughValues = new Dictionary<WCharacterFormat, RectangleF>();
      if (key1 != null && (double) font1.Size == (double) key1.Font.Size && (double) charFormat.Position == (double) key1.Position && charFormat.SubSuperScript == key1.SubSuperScript && this.IsSame(this.strikeThroughValues[key1].Right, textBounds.X, 2))
      {
        strikeThroughValue = this.strikeThroughValues[key1];
        strikeThroughValue.Width += textBounds.Width;
        this.strikeThroughValues[key1] = strikeThroughValue;
      }
      else
        this.strikeThroughValues.Add(charFormat, textBounds);
    }
  }

  private bool IsSameLine(
    float boundsRight,
    float boundsX,
    WCharacterFormat charFormat,
    WCharacterFormat preCharFormat)
  {
    return this.IsSame(boundsRight, boundsX, 1) && preCharFormat.UnderlineColor == charFormat.UnderlineColor && (double) charFormat.Position == (double) preCharFormat.Position && preCharFormat.UnderlineStyle == charFormat.UnderlineStyle && charFormat.SubSuperScript == SubSuperScript.None && this.GetTextColor(preCharFormat) == this.GetTextColor(charFormat);
  }

  private bool IsSame(float value1, float value2, int digit)
  {
    return Math.Round((double) value1, digit) == Math.Round((double) value2, digit);
  }

  private bool HasUnderlineOrStricthrough(
    WTextRange txtRange,
    WCharacterFormat charFormat,
    FontScriptType scriptType)
  {
    bool flag = false;
    if (!this.IsListCharacter && this.currTextRange != null)
    {
      if (txtRange.CharacterFormat != null && (!(txtRange.CharacterFormat.CharStyleName == "Hyperlink") || !this.IsTOC(txtRange)))
        flag = true;
      if (txtRange != null)
      {
        if (txtRange.Text.Trim(' ') == string.Empty && !(((IWidget) txtRange).LayoutInfo is TabsLayoutInfo) && (charFormat.UnderlineStyle != UnderlineStyle.None || charFormat.GetFontToRender(txtRange.ScriptType).Strikeout))
        {
          if (txtRange.NextSibling == null)
            flag = false;
          else if (txtRange.NextSibling != null)
          {
            nextSibling = txtRange.NextSibling as Entity;
            while (nextSibling is WTextRange)
            {
              if (!((nextSibling as WTextRange).Text.Trim(' ') == string.Empty) || (nextSibling as WTextRange).m_layoutInfo is TabsLayoutInfo || !(nextSibling.NextSibling is Entity nextSibling))
                break;
            }
            if (nextSibling != null && nextSibling is WTextRange)
            {
              if ((nextSibling as WTextRange).Text.Trim(' ') != string.Empty || (nextSibling as WTextRange).m_layoutInfo is TabsLayoutInfo)
                goto label_24;
            }
            if (!(nextSibling is InlineContentControl) || !this.IsContentControlHavingTextRange(nextSibling as InlineContentControl))
              flag = false;
          }
        }
      }
    }
    else
    {
      if (charFormat.UnderlineStyle != UnderlineStyle.None || charFormat.Strikeout || charFormat.DoubleStrike)
        flag = true;
      if (charFormat.CharStyleName != null && charFormat.CharStyleName.ToLower().Equals("hyperlink") && (this.currTextRange == null || !this.IsTOC(this.currTextRange)) && !charFormat.HasKey(7))
        flag = true;
      if (this.IsListCharacter && this.currParagraph != null && this.currParagraph.ListFormat.ListType != ListType.NoList && this.currParagraph.ListFormat.CurrentListLevel != null)
      {
        if (this.currParagraph.ListFormat.CurrentListLevel.CharacterFormat.GetFontToRender(scriptType).Underline)
          flag = true;
        else if (charFormat.UnderlineStyle != UnderlineStyle.None)
          flag = false;
      }
    }
label_24:
    return flag;
  }

  private bool IsContentControlHavingTextRange(InlineContentControl inlineContentControl)
  {
    Entity entity = (Entity) inlineContentControl;
    for (int index = 0; index < inlineContentControl.ParagraphItems.Count; ++index)
    {
      entity = (Entity) inlineContentControl.ParagraphItems[index];
      if (entity is InlineContentControl)
        return this.IsContentControlHavingTextRange(entity as InlineContentControl);
      if (!(entity is BookmarkStart) && !(entity is BookmarkEnd))
        break;
    }
    if (entity is WTextRange wtextRange)
    {
      if (wtextRange.Text.Trim(' ') != string.Empty || wtextRange.m_layoutInfo is TabsLayoutInfo)
        return true;
    }
    return false;
  }

  private Font GetPrivateFont(string fontName, float fontsize, FontStyle style)
  {
    if (this.FontNames.ContainsKey(fontName))
      fontName = this.FontNames[fontName];
    foreach (FontFamily family in this.PrivateFonts.Families)
    {
      if (family.Name.Equals(fontName, StringComparison.OrdinalIgnoreCase))
        return new Font(family, fontsize, style);
    }
    return new Font(fontName, fontsize, style);
  }

  private bool IsTabWidget(LayoutedWidget ltWidget)
  {
    return ltWidget.Widget is WTextRange && (ltWidget.Widget as WTextRange).m_layoutInfo is TabsLayoutInfo && ((ltWidget.Widget as WTextRange).m_layoutInfo as TabsLayoutInfo).CurrTabLeader == Syncfusion.Layouting.TabLeader.NoLeader;
  }

  private LayoutedWidget GetTextBoxWidget(LayoutedWidget ltWidget)
  {
    LayoutedWidget textBoxWidget = (LayoutedWidget) null;
    while (ltWidget != null)
    {
      ltWidget = ltWidget.Owner;
      if (ltWidget != null && ltWidget.Widget is WTextBox)
      {
        textBoxWidget = ltWidget;
        break;
      }
    }
    return textBoxWidget;
  }

  private void ReverseString(ref string text)
  {
    char[] charArray = text.ToCharArray();
    string empty = string.Empty;
    for (int index = charArray.Length - 1; index > -1; --index)
      empty += (string) (object) charArray[index];
    text = empty;
  }

  private void DrawSmallCapString(
    FontScriptType scriptType,
    CharacterRangeType characterRangeType,
    string text,
    WCharacterFormat charFormat,
    RectangleF bounds,
    Font font,
    StringFormat format,
    Brush textBrush,
    bool isCharacterSpacing)
  {
    Font font1 = charFormat.Document.FontSettings.GetFont(font.Name, (double) font.Size * 0.8 > 3.0 ? font.Size * 0.8f : 2f, font.Style);
    float num1 = 0.0f;
    SizeF empty1 = SizeF.Empty;
    float ascent1 = this.GetAscent(font);
    float num2 = (double) ascent1 > (double) bounds.Height ? bounds.Height : ascent1;
    float ascent2 = this.GetAscent(font1);
    float num3 = (double) ascent2 > (double) bounds.Height ? bounds.Height : ascent2;
    string empty2 = string.Empty;
    string empty3 = string.Empty;
    List<char> charList = new List<char>();
    List<string> stringList = new List<string>();
    float num4 = 0.0f;
    string fontNameToRender = charFormat.GetFontNameToRender(scriptType);
    if (!this.IsListCharacter && (fontNameToRender == "DIN Offc" || fontNameToRender == "DIN OT") && this.currParagraph != null && this.currParagraph.IsContainDinOffcFont())
      num4 = bounds.Height * 0.2f;
    foreach (char c in text)
    {
      if (char.IsUpper(c) || !char.IsLetter(c) && !c.Equals(ControlChar.SpaceChar))
      {
        if (empty3.Length != 0)
        {
          charList.Add('s');
          stringList.Add(empty3.ToUpper());
          empty3 = string.Empty;
        }
        empty2 += c.ToString();
      }
      else
      {
        if (empty2.Length != 0)
        {
          charList.Add('c');
          stringList.Add(empty2);
          empty2 = string.Empty;
        }
        empty3 += c.ToString();
      }
    }
    if (empty3.Length != 0)
    {
      charList.Add('s');
      stringList.Add(empty3.ToUpper());
      string empty4 = string.Empty;
    }
    else if (empty2.Length != 0)
    {
      charList.Add('c');
      stringList.Add(empty2);
      string empty5 = string.Empty;
    }
    SizeF sizeF;
    for (int index = 0; index < charList.Count; ++index)
    {
      if (charList[index] == 'c')
      {
        sizeF = this.MeasureString(stringList[index], font, format, charFormat, false, true);
        Font font2 = charFormat == null || charFormat.SubSuperScript == SubSuperScript.None ? font : charFormat.Document.FontSettings.GetFont(font.Name, this.GetSubSuperScriptFontSize(font), font.Style);
        if (!charFormat.GetFontNameToRender(scriptType).Equals(font2.Name, StringComparison.OrdinalIgnoreCase) && this.m_privateFontStream != null && this.m_privateFontStream.ContainsKey(charFormat.GetFontNameToRender(scriptType)))
          font2 = this.GetPrivateFont(charFormat.GetFontNameToRender(scriptType), font2.Size, font2.Style);
        if (isCharacterSpacing)
          this.DrawStringBasedOnCharSpacing(scriptType, characterRangeType, font2, textBrush, new RectangleF(bounds.X + num1, bounds.Y, sizeF.Width, bounds.Height), stringList[index], format, charFormat);
        else
          this.Graphics.DrawString(stringList[index], font2, textBrush, new RectangleF(bounds.X + num1, bounds.Y + num4, bounds.Width, bounds.Height), format);
        num1 += sizeF.Width;
      }
      else
      {
        sizeF = this.MeasureString(stringList[index].ToLower(), font, format, charFormat, false, true);
        Font font3 = charFormat == null || charFormat.SubSuperScript == SubSuperScript.None ? font1 : charFormat.Document.FontSettings.GetFont(font1.Name, this.GetSubSuperScriptFontSize(font1), font1.Style);
        if (isCharacterSpacing)
          this.DrawStringBasedOnCharSpacing(scriptType, characterRangeType, font3, textBrush, new RectangleF(bounds.X + num1, bounds.Y + (num2 - num3), sizeF.Width, bounds.Height), stringList[index], format, charFormat);
        else
          this.Graphics.DrawString(stringList[index], font3, textBrush, new RectangleF(bounds.X + num1, bounds.Y + (num2 - num3) + num4, bounds.Width, bounds.Height), format);
        num1 += sizeF.Width;
      }
    }
  }

  private bool IsTextNeedToClip(LayoutedWidget ltWidget)
  {
    if (ltWidget == null || ltWidget.Widget.LayoutInfo.IsVerticalText || this.currParagraph == null || !this.currParagraph.IsInCell)
      return false;
    LayoutedWidget owner = ltWidget.Owner;
    LayoutedWidget layoutedWidget = ltWidget.Owner;
    for (; owner != null && (!(owner.Widget is WTableRow) || !(owner.Widget is Shape)); owner = owner.Owner)
    {
      if (owner.Widget is WTableCell)
        layoutedWidget = owner;
    }
    if (layoutedWidget != null && layoutedWidget.Widget is WTableCell && (layoutedWidget.Widget.LayoutInfo is CellLayoutInfo ? ((layoutedWidget.Widget.LayoutInfo as CellLayoutInfo).IsRowMergeStart ? 1 : 0) : 0) == 0 && owner != null && owner.Widget is WTableRow)
      return Math.Round((double) ltWidget.Bounds.Y, 2) >= Math.Round((double) owner.Bounds.Bottom, 2);
    if (owner != null && owner.Widget is Shape)
      return Math.Round((double) ltWidget.Bounds.Y, 2) >= Math.Round((double) (owner.Widget as Shape).TextLayoutingBounds.Bottom, 2);
    return owner != null && owner.Widget is ChildShape && Math.Round((double) ltWidget.Bounds.Y, 2) >= Math.Round((double) (owner.Widget as ChildShape).TextLayoutingBounds.Bottom, 2);
  }

  private bool IsWidgetNeedToClipBasedOnXPosition(
    LayoutedWidget ltWidget,
    ref float clipWidth,
    RectangleF bounds)
  {
    float num = -1f;
    if (ltWidget != null && !ltWidget.Widget.LayoutInfo.IsVerticalText && this.currParagraph != null)
    {
      LayoutedWidget owner = ltWidget.Owner;
      WTextBox baseEntity1 = owner.Widget is Entity ? this.GetBaseEntity(owner.Widget as Entity) as WTextBox : (WTextBox) null;
      if (this.currParagraph.IsInCell)
      {
        LayoutedWidget layoutedWidget = ltWidget.Owner;
        for (; owner != null && !(owner.Widget is WTableRow); owner = owner.Owner)
        {
          if (owner.Widget is WTableCell)
            layoutedWidget = owner;
        }
        if (layoutedWidget != null && layoutedWidget.Widget is WTableCell)
          num = layoutedWidget.Bounds.Right;
      }
      else if (this.currParagraph.OwnerTextBody.Owner is Shape)
        num = (this.currParagraph.OwnerTextBody.Owner as Shape).TextLayoutingBounds.Right;
      else if (baseEntity1 != null)
      {
        num = baseEntity1.TextLayoutingBounds.Right;
        if ((double) baseEntity1.TextBoxFormat.Width > 0.0)
          num += baseEntity1.TextBoxFormat.InternalMargin.Right;
      }
      else if (this.currParagraph.ParagraphFormat.IsInFrame())
      {
        num = bounds.Right;
      }
      else
      {
        Entity baseEntity2 = this.GetBaseEntity((Entity) this.currParagraph);
        while (owner != null && !(owner.Widget is WSection) && (!(owner.Widget is SplitWidgetContainer) || !((owner.Widget as SplitWidgetContainer).RealWidgetContainer is WSection)))
          owner = owner.Owner;
        if (owner != null && baseEntity2 is WSection && (baseEntity2 as WSection).Columns.Count > 1)
        {
          int columnIndex = this.GetColumnIndex(baseEntity2 as WSection, owner.Bounds);
          if (columnIndex != (baseEntity2 as WSection).Columns.Count - 1)
            num = (float) ((double) owner.Bounds.X + (double) (baseEntity2 as WSection).Columns[columnIndex].Width + (double) (baseEntity2 as WSection).Columns[columnIndex].Space / 2.0);
        }
      }
    }
    if ((double) num > 0.0 && (double) bounds.X > (double) num)
      return true;
    if ((double) num > 0.0 && (double) bounds.Right > (double) num)
      clipWidth = num - bounds.X;
    return false;
  }

  public int GetColumnIndex(WSection section, RectangleF sectionBounds)
  {
    int columnIndex = 0;
    float num1 = this.m_pageMarginLeft;
    for (int index = 0; index < section.Columns.Count; ++index)
    {
      float num2 = num1 + section.Columns[index].Width;
      if ((double) sectionBounds.X < (double) num2)
      {
        columnIndex = index;
        break;
      }
      num1 = num2 + section.Columns[index].Space;
    }
    return columnIndex;
  }

  private float GetClipTopPosition(RectangleF bounds, bool isInlinePicture)
  {
    float clipTopPosition = 0.0f;
    float num1 = 0.0f;
    LineSpacingRule lineSpacingRule = LineSpacingRule.Multiple;
    if (this.currParagraph != null)
    {
      num1 = Math.Abs(this.currParagraph.ParagraphFormat.LineSpacing);
      lineSpacingRule = this.currParagraph.ParagraphFormat.LineSpacingRule;
    }
    if (this.currParagraph != null && (lineSpacingRule == LineSpacingRule.Exactly || (double) bounds.Height * ((double) num1 / 12.0) < 12.0 && lineSpacingRule == LineSpacingRule.Multiple && !isInlinePicture) && (double) num1 < (double) bounds.Height)
    {
      float num2 = 0.0f;
      if (((IWidget) this.currParagraph).LayoutInfo is ParagraphLayoutInfo layoutInfo)
        num2 = layoutInfo.Margins.Top;
      if ((double) bounds.Height * ((double) num1 / 12.0) < 12.0 && lineSpacingRule == LineSpacingRule.Multiple)
        num1 = bounds.Height * (num1 / 12f);
      if ((double) num2 + (double) num1 < (double) bounds.Height)
        clipTopPosition = (float) (((double) bounds.Height - (double) num1 - (double) num2) / 2.0);
    }
    return clipTopPosition;
  }

  public Font GetDefaultFont(FontScriptType scriptType, Font font, WCharacterFormat charFormat)
  {
    if (charFormat.HasValue(72) && !TextSplitter.IsEastAsiaScript(scriptType))
    {
      string fontNameFromHint = charFormat.GetFontNameFromHint(scriptType);
      if (charFormat.FontNameNonFarEast != fontNameFromHint && !charFormat.IsThemeFont(charFormat.FontNameNonFarEast))
        return charFormat.Document.FontSettings.GetFont(charFormat.FontNameNonFarEast, font.Size, font.Style);
    }
    return font;
  }

  internal void DrawStringBasedOnCharSpacing(
    FontScriptType scriptType,
    CharacterRangeType characterRangeType,
    Font font,
    Brush textBrush,
    RectangleF bounds,
    string text,
    StringFormat format,
    WCharacterFormat charFormat)
  {
    if (scriptType == FontScriptType.Arabic)
    {
      ArabicShapeRenderer arabicShapeRenderer = new ArabicShapeRenderer();
      text = arabicShapeRenderer.Shape(text.ToCharArray(), 0);
      arabicShapeRenderer.Dispose();
    }
    if ((format.FormatFlags & StringFormatFlags.DirectionRightToLeft) == StringFormatFlags.DirectionRightToLeft && characterRangeType == CharacterRangeType.RTL)
    {
      char[] charArray = text.ToCharArray();
      Array.Reverse((Array) charArray);
      text = new string(charArray);
    }
    float num1 = 0.0f;
    foreach (char ch in text)
      num1 += this.MeasureString(ch.ToString(), font, format).Width;
    float width1 = (bounds.Width - num1) / (float) text.Length;
    float num2 = 0.0f;
    string fontNameToRender = charFormat.GetFontNameToRender(scriptType);
    if (!this.IsListCharacter && (fontNameToRender == "DIN Offc" || fontNameToRender == "DIN OT") && this.currParagraph != null && this.currParagraph.IsContainDinOffcFont())
      num2 = bounds.Height * 0.2f;
    float num3 = 0.0f;
    foreach (char ch in text)
    {
      float width2 = this.MeasureString(ch.ToString(), font, format).Width;
      if (ch == '\uF06F' && font.Name == "Arial")
      {
        Font font1 = charFormat.Document.FontSettings.GetFont("Wingdings", font.Size, font.Style);
        if (font1.Name == "Wingdings")
          font = font1;
      }
      if ((double) bounds.X <= (double) bounds.X + (double) num3)
      {
        this.Graphics.DrawString(ch.ToString(), font, textBrush, new RectangleF(bounds.X + num3, bounds.Y + num2, width2, bounds.Height), format);
        if ((double) width1 > 0.0)
          this.Graphics.DrawString(" ", font, textBrush, new RectangleF(bounds.X + num3 + width2, bounds.Y + num2, width1, bounds.Height), format);
      }
      else
        this.Graphics.DrawString(ch.ToString(), font, textBrush, new RectangleF(bounds.X, bounds.Y + num2, width2, bounds.Height), format);
      num3 += width2 + width1;
    }
  }

  private void TransformGraphicsPosition(
    LayoutedWidget ltWidget,
    bool isNeedToScale,
    ref PointF translatePoints,
    ref float rotationAngle,
    WParagraph ownerParagraph)
  {
    Entity ownerEntity1 = ownerParagraph.GetOwnerEntity();
    if (ownerParagraph.IsInCell)
    {
      WTableCell ownerEntity2 = ownerParagraph.GetOwnerEntity() as WTableCell;
      LayoutedWidget cellLayoutedWidget = this.GetOwnerCellLayoutedWidget(ltWidget);
      if (cellLayoutedWidget == null)
        return;
      RectangleF bounds = cellLayoutedWidget.Owner.Bounds;
      if (ownerEntity2.CellFormat.TextDirection == TextDirection.VerticalTopToBottom)
      {
        translatePoints = new PointF(bounds.X + bounds.Y + bounds.Width, bounds.Y - bounds.X);
        rotationAngle = 90f;
        if (isNeedToScale)
          return;
        this.Graphics.TranslateTransform(translatePoints.X, translatePoints.Y);
        this.Graphics.RotateTransform(rotationAngle);
      }
      else
      {
        translatePoints = new PointF(bounds.X - bounds.Y, bounds.X + bounds.Y + bounds.Height);
        rotationAngle = 270f;
        if (isNeedToScale)
          return;
        this.Graphics.TranslateTransform(translatePoints.X, translatePoints.Y);
        this.Graphics.RotateTransform(rotationAngle);
      }
    }
    else if (ownerEntity1 is WTextBox)
    {
      WTextBox wtextBox = ownerEntity1 as WTextBox;
      float boxContentHeight = this.GetLayoutedTextBoxContentHeight(ltWidget);
      float shiftVerticalText = this.GetWidthToShiftVerticalText(wtextBox.TextBoxFormat.TextVerticalAlignment, boxContentHeight, wtextBox.TextLayoutingBounds.Height);
      if (wtextBox.TextBoxFormat.TextDirection == TextDirection.VerticalTopToBottom || wtextBox.TextBoxFormat.TextDirection == TextDirection.VerticalFarEast)
      {
        translatePoints = new PointF(wtextBox.TextLayoutingBounds.X + wtextBox.TextLayoutingBounds.Y + wtextBox.TextLayoutingBounds.Height - shiftVerticalText, wtextBox.TextLayoutingBounds.Y - wtextBox.TextLayoutingBounds.X);
        rotationAngle = 90f;
        if (isNeedToScale)
          return;
        this.Graphics.TranslateTransform(wtextBox.TextLayoutingBounds.X + wtextBox.TextLayoutingBounds.Y + wtextBox.TextLayoutingBounds.Height - shiftVerticalText, wtextBox.TextLayoutingBounds.Y - wtextBox.TextLayoutingBounds.X);
        this.Graphics.RotateTransform(90f);
      }
      else
      {
        translatePoints = new PointF(wtextBox.TextLayoutingBounds.X - wtextBox.TextLayoutingBounds.Y + shiftVerticalText, wtextBox.TextLayoutingBounds.X + wtextBox.TextLayoutingBounds.Y + wtextBox.TextLayoutingBounds.Width);
        rotationAngle = 270f;
        if (isNeedToScale)
          return;
        this.Graphics.TranslateTransform(wtextBox.TextLayoutingBounds.X - wtextBox.TextLayoutingBounds.Y + shiftVerticalText, wtextBox.TextLayoutingBounds.X + wtextBox.TextLayoutingBounds.Y + wtextBox.TextLayoutingBounds.Width);
        this.Graphics.RotateTransform(270f);
      }
    }
    else if (ownerEntity1 is Shape)
    {
      Shape shape = ownerEntity1 as Shape;
      if (shape.TextFrame.TextDirection == TextDirection.VerticalTopToBottom || shape.TextFrame.TextDirection == TextDirection.VerticalFarEast)
      {
        translatePoints = new PointF(shape.TextLayoutingBounds.X + shape.TextLayoutingBounds.Y + shape.TextLayoutingBounds.Height, shape.TextLayoutingBounds.Y - shape.TextLayoutingBounds.X);
        rotationAngle = 90f;
        if (isNeedToScale)
          return;
        this.Graphics.TranslateTransform(shape.TextLayoutingBounds.X + shape.TextLayoutingBounds.Y + shape.TextLayoutingBounds.Height, shape.TextLayoutingBounds.Y - shape.TextLayoutingBounds.X);
        this.Graphics.RotateTransform(90f);
      }
      else
      {
        translatePoints = new PointF(shape.TextLayoutingBounds.X - shape.TextLayoutingBounds.Y, shape.TextLayoutingBounds.Width + shape.TextLayoutingBounds.X + shape.TextLayoutingBounds.Y);
        rotationAngle = 270f;
        if (isNeedToScale)
          return;
        this.Graphics.TranslateTransform(shape.TextLayoutingBounds.X - shape.TextLayoutingBounds.Y, shape.TextLayoutingBounds.Width + shape.TextLayoutingBounds.X + shape.TextLayoutingBounds.Y);
        this.Graphics.RotateTransform(270f);
      }
    }
    else
    {
      if (!(ownerParagraph.Owner.Owner is ChildShape))
        return;
      ChildShape owner = ownerParagraph.Owner.Owner as ChildShape;
      if (owner.TextFrame.TextDirection == TextDirection.VerticalTopToBottom || owner.TextFrame.TextDirection == TextDirection.VerticalFarEast)
      {
        translatePoints = new PointF(owner.TextLayoutingBounds.X + owner.TextLayoutingBounds.Y + owner.TextLayoutingBounds.Height, owner.TextLayoutingBounds.Y - owner.TextLayoutingBounds.X);
        rotationAngle = 90f;
        if (isNeedToScale)
          return;
        this.Graphics.TranslateTransform(owner.TextLayoutingBounds.X + owner.TextLayoutingBounds.Y + owner.TextLayoutingBounds.Height, owner.TextLayoutingBounds.Y - owner.TextLayoutingBounds.X);
        this.Graphics.RotateTransform(90f);
      }
      else
      {
        ref PointF local = ref translatePoints;
        double x1 = (double) owner.TextLayoutingBounds.X;
        RectangleF textLayoutingBounds = owner.TextLayoutingBounds;
        double y1 = (double) textLayoutingBounds.Y;
        double x2 = x1 - y1;
        textLayoutingBounds = owner.TextLayoutingBounds;
        double width1 = (double) textLayoutingBounds.Width;
        textLayoutingBounds = owner.TextLayoutingBounds;
        double x3 = (double) textLayoutingBounds.X;
        double num1 = width1 + x3;
        textLayoutingBounds = owner.TextLayoutingBounds;
        double y2 = (double) textLayoutingBounds.Y;
        double y3 = num1 + y2;
        PointF pointF = new PointF((float) x2, (float) y3);
        local = pointF;
        rotationAngle = 270f;
        if (isNeedToScale)
          return;
        Graphics graphics = this.Graphics;
        textLayoutingBounds = owner.TextLayoutingBounds;
        double x4 = (double) textLayoutingBounds.X;
        textLayoutingBounds = owner.TextLayoutingBounds;
        double y4 = (double) textLayoutingBounds.Y;
        double dx = x4 - y4;
        textLayoutingBounds = owner.TextLayoutingBounds;
        double width2 = (double) textLayoutingBounds.Width;
        textLayoutingBounds = owner.TextLayoutingBounds;
        double x5 = (double) textLayoutingBounds.X;
        double num2 = width2 + x5;
        textLayoutingBounds = owner.TextLayoutingBounds;
        double y5 = (double) textLayoutingBounds.Y;
        double dy = num2 + y5;
        graphics.TranslateTransform((float) dx, (float) dy);
        this.Graphics.RotateTransform(270f);
      }
    }
  }

  private LayoutedWidget GetOwnerCellLayoutedWidget(LayoutedWidget ltWidget)
  {
    LayoutedWidget cellLayoutedWidget = (LayoutedWidget) null;
    while (ltWidget != null)
    {
      ltWidget = ltWidget.Owner;
      if (ltWidget != null && (ltWidget.Widget is SplitWidgetContainer && (ltWidget.Widget as SplitWidgetContainer).RealWidgetContainer is WTableCell || ltWidget.Widget is WTableCell))
        break;
    }
    if (ltWidget != null && (ltWidget.Widget is SplitWidgetContainer && (ltWidget.Widget as SplitWidgetContainer).RealWidgetContainer is WTableCell || ltWidget.Widget is WTableCell))
      cellLayoutedWidget = ltWidget;
    return cellLayoutedWidget;
  }

  private float GetLayoutedTextBoxContentHeight(LayoutedWidget ltWidget)
  {
    while (ltWidget != null && !(ltWidget.Widget is WTextBody))
      ltWidget = ltWidget.Owner;
    if (ltWidget != null && ltWidget.Owner != null && ltWidget.Owner.Widget is WTextBox)
    {
      WTextBox widget = ltWidget.Owner.Widget as WTextBox;
      return (float) ((double) ltWidget.Bounds.Height + (double) widget.TextBoxFormat.InternalMargin.Top + (double) widget.TextBoxFormat.InternalMargin.Bottom - (double) widget.TextBoxFormat.LineWidth / 2.0);
    }
    return ltWidget != null && ltWidget.Owner != null && ltWidget.Owner.Widget is ChildShape ? (ltWidget.Owner.Widget as ChildShape).Height : 0.0f;
  }

  private float GetWidthToShiftVerticalText(
    VerticalAlignment verticalAlignment,
    float cellLayoutedHeight,
    float cellHeight)
  {
    float shiftVerticalText = 0.0f;
    switch ((byte) verticalAlignment)
    {
      case 1:
        shiftVerticalText = (float) (((double) cellHeight - (double) cellLayoutedHeight) / 2.0);
        break;
      case 2:
        shiftVerticalText = cellHeight - cellLayoutedHeight;
        break;
    }
    if ((double) shiftVerticalText < 0.0)
      shiftVerticalText = 0.0f;
    return shiftVerticalText;
  }

  internal RectangleF GetClipBounds(RectangleF bounds, float clipWidth, float clipTop)
  {
    float x = bounds.X;
    float y = bounds.Y + clipTop;
    if ((double) x % 0.75 != 0.0 && Math.Round((double) x % 0.75, 2) > 0.02)
      x = (double) (x - (float) Math.Round((double) x % 0.75, 2)) % 0.75 >= 0.03 ? (float) Math.Round((double) x - Math.Round((double) x % 0.75, 2) - 0.75, 2) : (float) Math.Round((double) x - Math.Round((double) x % 0.75, 2), 2);
    if ((double) y % 0.75 != 0.0 && Math.Round((double) y % 0.75, 2) > 0.02)
      y = (double) (y - (float) Math.Round((double) y % 0.75, 2)) % 0.75 >= 0.03 ? (float) Math.Round((double) y - Math.Round((double) y % 0.75, 2) - 0.75, 2) : (float) Math.Round((double) y - Math.Round((double) y % 0.75, 2), 2);
    clipWidth += bounds.X - x;
    float height = bounds.Height + bounds.Y - y;
    return new RectangleF(x, y, clipWidth, height);
  }

  private RectangleF UpdateClipBounds(RectangleF clipBounds, RectangleF ownerClipBounds)
  {
    if ((double) ownerClipBounds.X > (double) clipBounds.X)
      clipBounds.X = ownerClipBounds.X;
    if ((double) ownerClipBounds.Y > (double) clipBounds.Y)
      clipBounds.Y = ownerClipBounds.Y;
    if ((double) ownerClipBounds.Y + (double) ownerClipBounds.Width < (double) clipBounds.Y + (double) clipBounds.Width)
      clipBounds.Width = ownerClipBounds.Y + ownerClipBounds.Width - clipBounds.Y;
    if ((double) ownerClipBounds.X + (double) ownerClipBounds.Height < (double) clipBounds.X + (double) clipBounds.Height)
      clipBounds.Height = ownerClipBounds.X + ownerClipBounds.Height - clipBounds.X;
    clipBounds.Width = (double) clipBounds.Width < 0.0 ? 0.0f : clipBounds.Width;
    clipBounds.Height = (double) clipBounds.Height < 0.0 ? 0.0f : clipBounds.Height;
    return clipBounds;
  }

  internal RectangleF UpdateClipBoundsBasedOnOwner(
    RectangleF clipBounds,
    RectangleF ownerClipBounds)
  {
    if ((double) ownerClipBounds.X > (double) clipBounds.X)
      clipBounds.X = ownerClipBounds.X;
    if ((double) ownerClipBounds.Right < (double) clipBounds.Right)
      clipBounds.Width = ownerClipBounds.Right - clipBounds.X;
    if ((double) ownerClipBounds.Y > (double) clipBounds.Y)
    {
      clipBounds.Height -= ownerClipBounds.Y - clipBounds.Y;
      clipBounds.Y = ownerClipBounds.Y;
    }
    if ((double) ownerClipBounds.Bottom < (double) clipBounds.Bottom)
      clipBounds.Height = ownerClipBounds.Bottom - clipBounds.Y;
    clipBounds.Width = (double) clipBounds.Width < 0.0 ? 0.0f : clipBounds.Width;
    clipBounds.Height = (double) clipBounds.Height < 0.0 ? 0.0f : clipBounds.Height;
    return clipBounds;
  }

  private float GetCellHeightForVerticalText(Entity ent)
  {
    float heightForVerticalText1 = 0.0f;
    if (ent is ParagraphItem)
    {
      WTableCell ownerTextBody = (ent as ParagraphItem).GetOwnerParagraphValue().OwnerTextBody as WTableCell;
      float num1 = 0.0f;
      if (ownerTextBody != null)
      {
        if ((double) ownerTextBody.OwnerRow.OwnerTable.TableFormat.CellSpacing > 0.0)
          num1 = ownerTextBody.OwnerRow.OwnerTable.TableFormat.CellSpacing * 2f;
        float heightForVerticalText2 = ownerTextBody.Width;
        if (((IWidget) ownerTextBody).LayoutInfo is CellLayoutInfo layoutInfo)
        {
          float num2 = layoutInfo.Paddings.Left + layoutInfo.Margins.Left - num1;
          float num3 = layoutInfo.Paddings.Right + layoutInfo.Margins.Right - num1;
          heightForVerticalText2 = heightForVerticalText2 - num2 - num3;
        }
        return heightForVerticalText2;
      }
    }
    return heightForVerticalText1;
  }

  private void DrawRTLText(
    FontScriptType scriptType,
    CharacterRangeType characterRangeType,
    string text,
    WCharacterFormat charFormat,
    Font font,
    Brush textBrush,
    RectangleF bounds,
    StringFormat format)
  {
    if ((double) charFormat.CharacterSpacing != 0.0)
    {
      if (charFormat != null && charFormat.SmallCaps)
      {
        this.DrawSmallCapString(scriptType, characterRangeType, text, charFormat, bounds, font, format, textBrush, true);
      }
      else
      {
        Font font1 = charFormat == null || charFormat.SubSuperScript == SubSuperScript.None ? font : charFormat.Document.FontSettings.GetFont(font.Name, this.GetSubSuperScriptFontSize(font), font.Style);
        this.DrawStringBasedOnCharSpacing(scriptType, characterRangeType, font1, textBrush, bounds, text, format, charFormat);
      }
    }
    else if (!charFormat.Bidi)
    {
      font = this.GetDefaultFont(scriptType, font, charFormat);
      float num = 0.0f;
      string fontNameToRender = charFormat.GetFontNameToRender(scriptType);
      if (!this.IsListCharacter && (fontNameToRender == "DIN Offc" || fontNameToRender == "DIN OT") && this.currParagraph != null && this.currParagraph.IsContainDinOffcFont())
        num = bounds.Height * 0.2f;
      Font font2 = font;
      if (font2.Name == "Arial" && this.IsInvalidCharacter(text))
        font2 = charFormat.Document.FontSettings.GetFont("Arial Unicode MS", font2.Size, font2.Style);
      if (charFormat.SmallCaps)
      {
        this.DrawSmallCapString(scriptType, characterRangeType, text, charFormat, bounds, font2, format, textBrush, false);
      }
      else
      {
        Font font3 = charFormat == null || charFormat.SubSuperScript == SubSuperScript.None ? font2 : charFormat.Document.FontSettings.GetFont(font2.Name, this.GetSubSuperScriptFontSize(font2), font2.Style);
        if (font3.Name == "Arial Narrow" && font3.Style == FontStyle.Bold)
          text = text.Replace(ControlChar.NonBreakingSpaceChar, ControlChar.SpaceChar);
        if (!charFormat.GetFontNameToRender(scriptType).Equals(font3.Name, StringComparison.OrdinalIgnoreCase) && this.m_privateFontStream != null && this.m_privateFontStream.ContainsKey(charFormat.GetFontNameToRender(scriptType)))
          font3 = this.GetPrivateFont(charFormat.GetFontNameToRender(scriptType), font3.Size, font3.Style);
        this.Graphics.DrawString(text, font3, textBrush, new RectangleF(bounds.X, bounds.Y + num, bounds.Width, bounds.Height), format);
      }
    }
    else
      this.DrawUnicodeString(scriptType, characterRangeType, text, charFormat, font, textBrush, bounds, format);
  }

  private void DrawChineseText(
    FontScriptType scriptType,
    CharacterRangeType characterRangeType,
    string text,
    WCharacterFormat charFormat,
    Font font,
    Brush textBrush,
    RectangleF bounds,
    StringFormat format)
  {
    text = text.Replace('\u200D'.ToString(), "");
    if (text.Length == 0)
      return;
    if ((double) charFormat.CharacterSpacing != 0.0)
    {
      if (charFormat != null && charFormat.SmallCaps)
      {
        this.DrawSmallCapString(scriptType, characterRangeType, text, charFormat, bounds, font, format, textBrush, true);
      }
      else
      {
        Font font1 = charFormat == null || charFormat.SubSuperScript == SubSuperScript.None ? font : charFormat.Document.FontSettings.GetFont(font.Name, this.GetSubSuperScriptFontSize(font), font.Style);
        this.DrawStringBasedOnCharSpacing(scriptType, characterRangeType, font1, textBrush, bounds, text, format, charFormat);
      }
    }
    else
    {
      char[] charArray = text.ToCharArray();
      string text1 = (string) null;
      string text2 = (string) null;
      Font font2 = font;
      if (font2.Name == "Arial" || font2.Name == "Times New Roman" || font2.Name == "Trebuchet MS")
        font2 = charFormat.Document.FontSettings.GetFont("Arial Unicode MS", font2.Size, font2.Style);
      float num1 = 0.0f;
      string fontNameToRender = charFormat.GetFontNameToRender(scriptType);
      if (!this.IsListCharacter && (fontNameToRender == "DIN Offc" || fontNameToRender == "DIN OT") && this.currParagraph != null && this.currParagraph.IsContainDinOffcFont())
        num1 = bounds.Height * 0.2f;
      if (!charFormat.ComplexScript)
        font = this.GetDefaultFont(scriptType, font, charFormat);
      float ascent1 = this.GetAscent(font);
      float ascent2 = this.GetAscent(font2);
      float num2 = 0.0f;
      float num3 = 0.0f;
      if (font.Name != font2.Name && this.IsUnicodeText(text))
      {
        if ((double) ascent2 > (double) ascent1)
          num2 = ascent2 - ascent1;
        if ((double) ascent1 > (double) ascent2)
          num3 = ascent1 - ascent2;
        for (int index = 0; index < charArray.Length; ++index)
        {
          if (!this.IsUnicodeText(charArray[index].ToString()))
          {
            text1 += (string) (object) charArray[index];
            if (text2 != null)
            {
              float width = this.MeasureString(text2, font2, format, charFormat, false, true).Width;
              if (charFormat != null && charFormat.SmallCaps)
              {
                this.DrawSmallCapString(scriptType, characterRangeType, text, charFormat, new RectangleF(new PointF(bounds.X, bounds.Y - num3), bounds.Size), font, format, textBrush, false);
              }
              else
              {
                Font font3 = charFormat == null || charFormat.SubSuperScript == SubSuperScript.None ? font2 : charFormat.Document.FontSettings.GetFont(font2.Name, this.GetSubSuperScriptFontSize(font2), font2.Style);
                if (!charFormat.GetFontNameToRender(scriptType).Equals(font3.Name, StringComparison.OrdinalIgnoreCase) && this.m_privateFontStream != null && this.m_privateFontStream.ContainsKey(charFormat.GetFontNameToRender(scriptType)))
                  font3 = this.GetPrivateFont(charFormat.GetFontNameToRender(scriptType), font3.Size, font3.Style);
                this.DrawUnicodeString(text2, font3, textBrush, new RectangleF(bounds.X, bounds.Y - num3 + num1, bounds.Width, bounds.Height), format);
              }
              bounds.X += width;
              text2 = (string) null;
            }
          }
          else
          {
            text2 += (string) (object) charArray[index];
            if (text1 != null)
            {
              float width = this.MeasureString(text1, font, format, charFormat, false, true).Width;
              this.DrawUnicodeText(scriptType, characterRangeType, text1, charFormat, font, textBrush, new RectangleF(bounds.X, bounds.Y + num2, bounds.Width, bounds.Height), format);
              bounds.X += width;
              text1 = (string) null;
            }
          }
        }
        if (text2 != null)
        {
          if (charFormat != null && charFormat.SmallCaps)
          {
            this.DrawSmallCapString(scriptType, characterRangeType, text, charFormat, new RectangleF(new PointF(bounds.X, bounds.Y - num3), bounds.Size), font, format, textBrush, false);
          }
          else
          {
            Font font4 = charFormat == null || charFormat.SubSuperScript == SubSuperScript.None ? font2 : charFormat.Document.FontSettings.GetFont(font2.Name, this.GetSubSuperScriptFontSize(font2), font2.Style);
            this.DrawUnicodeString(text2, font4, textBrush, new RectangleF(bounds.X, bounds.Y - num3 + num1, bounds.Width, bounds.Height), format);
          }
        }
        else
        {
          if (text1 == null)
            return;
          this.DrawUnicodeText(scriptType, characterRangeType, text1, charFormat, font, textBrush, new RectangleF(bounds.X, bounds.Y + num2, bounds.Width, bounds.Height), format);
        }
      }
      else
        this.DrawUnicodeText(scriptType, characterRangeType, text, charFormat, font, textBrush, bounds, format);
    }
  }

  private bool IsInvalidCharacter(string text)
  {
    foreach (char ch in text)
    {
      if (ch != '★')
        return false;
    }
    return true;
  }

  private void DrawUnicodeText(
    FontScriptType scriptType,
    CharacterRangeType characterRangeType,
    string text,
    WCharacterFormat charFormat,
    Font font,
    Brush textBrush,
    RectangleF bounds,
    StringFormat format)
  {
    char[] charArray = text.ToCharArray();
    string text1 = (string) null;
    string text2 = (string) null;
    Font font1 = font;
    float num = 0.0f;
    string fontNameToRender = charFormat.GetFontNameToRender(scriptType);
    if (!this.IsListCharacter && (fontNameToRender == "DIN Offc" || fontNameToRender == "DIN OT") && this.currParagraph != null && this.currParagraph.IsContainDinOffcFont())
      num = bounds.Height * 0.2f;
    for (int index = 0; index < charArray.Length; ++index)
    {
      if (charArray[index] < 'ÿ')
      {
        text1 += (string) (object) charArray[index];
        if (text2 != null)
        {
          if (font1.Name == "Arial" && this.IsInvalidCharacter(text2))
            font1 = charFormat.Document.FontSettings.GetFont("Arial Unicode MS", font1.Size, font1.Style);
          float width = this.MeasureString(text2, font1, format, charFormat, false, true).Width;
          if (charFormat.SmallCaps)
          {
            this.DrawSmallCapString(scriptType, characterRangeType, text2, charFormat, bounds, font1, format, textBrush, false);
          }
          else
          {
            Font font2 = charFormat == null || charFormat.SubSuperScript == SubSuperScript.None ? font1 : charFormat.Document.FontSettings.GetFont(font1.Name, this.GetSubSuperScriptFontSize(font1), font1.Style);
            if (font2.Name == "Arial Narrow" && font2.Style == FontStyle.Bold)
              text2 = text2.Replace(ControlChar.NonBreakingSpaceChar, ControlChar.SpaceChar);
            if (!charFormat.GetFontNameToRender(scriptType).Equals(font2.Name, StringComparison.OrdinalIgnoreCase) && this.m_privateFontStream != null && this.m_privateFontStream.ContainsKey(charFormat.GetFontNameToRender(scriptType)))
              font2 = this.GetPrivateFont(charFormat.GetFontNameToRender(scriptType), font2.Size, font2.Style);
            this.DrawUnicodeString(text2, font2, textBrush, new RectangleF(bounds.X, bounds.Y + num, bounds.Width, bounds.Height), format);
          }
          bounds.X += width;
          text2 = (string) null;
        }
      }
      else
      {
        text2 += (string) (object) charArray[index];
        if (text1 != null)
        {
          float width = this.MeasureString(text1, font, format, charFormat, false, true).Width;
          this.DrawUnicodeString(scriptType, characterRangeType, text1, charFormat, font, textBrush, new RectangleF(bounds.X, bounds.Y, width, bounds.Height), format);
          bounds.X += width;
          text1 = (string) null;
        }
      }
    }
    if (text2 != null)
    {
      if (charFormat.SmallCaps)
      {
        this.DrawSmallCapString(scriptType, characterRangeType, text2, charFormat, bounds, font, format, textBrush, false);
      }
      else
      {
        Font font3 = charFormat == null || charFormat.SubSuperScript == SubSuperScript.None ? font : charFormat.Document.FontSettings.GetFont(font.Name, this.GetSubSuperScriptFontSize(font), font.Style);
        if (font3.Name == "Arial Narrow" && font3.Style == FontStyle.Bold)
          text2 = text2.Replace(ControlChar.NonBreakingSpaceChar, ControlChar.SpaceChar);
        this.DrawUnicodeString(text2, font3, textBrush, new RectangleF(bounds.X, bounds.Y + num, bounds.Width, bounds.Height), format);
      }
    }
    else
    {
      if (text1 == null)
        return;
      float width = this.MeasureString(text1, font, format, charFormat, false, true).Width;
      this.DrawUnicodeString(scriptType, characterRangeType, text1, charFormat, font, textBrush, new RectangleF(bounds.X, bounds.Y, width, bounds.Height), format);
    }
  }

  internal void DrawUnicodeString(
    FontScriptType scriptType,
    CharacterRangeType characterRangeType,
    string text,
    WCharacterFormat charFormat,
    Font font,
    Brush textBrush,
    RectangleF bounds,
    StringFormat format)
  {
    float num = 0.0f;
    string fontNameToRender = charFormat.GetFontNameToRender(scriptType);
    if (!this.IsListCharacter && (fontNameToRender == "DIN Offc" || fontNameToRender == "DIN OT") && this.currParagraph != null && this.currParagraph.IsContainDinOffcFont())
      num = bounds.Height * 0.2f;
    if (charFormat != null && charFormat.SmallCaps)
    {
      this.DrawSmallCapString(scriptType, characterRangeType, text, charFormat, bounds, font, format, textBrush, false);
    }
    else
    {
      Font font1 = charFormat == null || charFormat.SubSuperScript == SubSuperScript.None ? font : charFormat.Document.FontSettings.GetFont(font.Name, this.GetSubSuperScriptFontSize(font), font.Style);
      if (font1.Name == "Arial Narrow" && font1.Style == FontStyle.Bold)
        text = text.Replace(ControlChar.NonBreakingSpaceChar, ControlChar.SpaceChar);
      if (charFormat != null && !charFormat.GetFontNameToRender(scriptType).Equals(font1.Name, StringComparison.OrdinalIgnoreCase) && this.PrivateFonts != null && this.HasPrivateFont(charFormat.GetFontNameToRender(scriptType)))
        font1 = this.GetPrivateFont(charFormat.GetFontNameToRender(scriptType), font1.Size, font1.Style);
      RectangleF layoutRectangle = new RectangleF(bounds.X, bounds.Y + num, bounds.Width, bounds.Height);
      this.Graphics.DrawString(text, font1, textBrush, layoutRectangle, format);
    }
  }

  internal bool IsOwnerParagraphEmpty(string text)
  {
    bool flag = false;
    if (text == " " && this.currParagraph != null && this.currParagraph.Text == "" && this.currParagraph.Items.Count == 0)
      flag = true;
    return flag;
  }

  internal void DrawJustifiedLine(
    WTextRange txtRange,
    string text,
    WCharacterFormat charFormat,
    WParagraphFormat paraFormat,
    RectangleF bounds,
    LayoutedWidget ltWidget)
  {
    if (text == null)
      return;
    FontScriptType scriptType = txtRange.ScriptType;
    if (ltWidget.IsContainsSpaceCharAtEnd)
      text = text.TrimEnd(ControlChar.SpaceChar);
    int characterRange = (int) txtRange.CharacterRange;
    char ch = '\u001E';
    if (text.Contains(ch.ToString()))
      text = text.Replace(ch.ToString(), "-");
    float num1 = 0.0f;
    string[] strArray = text.Split(' ');
    bool flag1 = this.HasUnderlineOrStricthrough(txtRange, charFormat, scriptType);
    Font font1 = this.currTextRange == null || this.currTextRange.m_layoutInfo == null ? this.GetFont(scriptType, charFormat, text) : this.currTextRange.m_layoutInfo.Font.GetFont(this.currTextRange.Document);
    StringFormat stringFormat = new StringFormat(this.StringFormt);
    if (charFormat.Bidi)
      stringFormat.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
    else
      stringFormat.FormatFlags &= ~StringFormatFlags.DirectionRightToLeft;
    Brush brush = (Brush) this.GetBrush(this.GetTextColor(charFormat));
    Font font2 = this.GetDefaultFont(scriptType, font1, charFormat);
    if (font1.Name != font2.Name && !this.IsUnicodeText(text))
      font2 = font1;
    if (this.IsSoftHyphen(ltWidget))
    {
      text = "-";
      strArray[0] = text;
      if ((double) bounds.Width == 0.0)
        bounds.Width = ltWidget.Bounds.Width;
    }
    bool flag2 = font1.Name != font2.Name && this.IsUnicodeText(text);
    this.EnsureComplexScript(charFormat);
    if (!charFormat.TextBackgroundColor.IsEmpty)
      this.Graphics.FillRectangle((Brush) this.GetBrush(charFormat.TextBackgroundColor), bounds.X, bounds.Y, bounds.Width, bounds.Height);
    if (ltWidget.Owner.ChildWidgets[0] == ltWidget)
    {
      strArray = text.TrimStart().Split(' ');
      SizeF sizeF1 = this.MeasureString(text, font1, font2, stringFormat, charFormat);
      SizeF sizeF2 = this.MeasureString(text.TrimStart(), font1, font2, stringFormat, charFormat);
      if (sizeF1 != sizeF2)
      {
        num1 = sizeF1.Width - sizeF2.Width;
        bounds.X += num1;
        text = text.TrimStart();
      }
    }
    if (!charFormat.HighlightColor.IsEmpty)
    {
      SolidBrush solidBrush = this.CreateSolidBrush(this.GetHightLightColor(charFormat.HighlightColor));
      SizeF sizeF3 = new SizeF();
      SizeF sizeF4 = !flag2 ? this.MeasureString(text, font2, stringFormat, charFormat, false, true) : this.MeasureString(text, font1, font2, stringFormat, charFormat);
      this.Graphics.FillRectangle((Brush) solidBrush, bounds.X, bounds.Y, bounds.Width, sizeF4.Height);
    }
    this.ResetTransform();
    RectangleF rectangleF = ltWidget.Bounds;
    float width1 = rectangleF.Width;
    float clipTopPosition = this.GetClipTopPosition(bounds, false);
    float scaling = charFormat.Scaling;
    bool isNeedToScale = (double) scaling != 100.0 && ((double) scaling >= 1.0 || (double) scaling <= 600.0);
    PointF empty = PointF.Empty;
    float rotationAngle = 0.0f;
    float rotation = 0.0f;
    bool flipH = false;
    bool flipV = false;
    bool flag3 = false;
    TextWrappingStyle textWrappingStyle = TextWrappingStyle.Square;
    if (this.currTextRange != null)
    {
      if (!(this.currTextRange.Owner is WParagraph wparagraph))
        wparagraph = this.currTextRange.GetOwnerParagraphValue();
      if (wparagraph != null)
      {
        Entity ownerEntity = wparagraph.GetOwnerEntity();
        switch (ownerEntity)
        {
          case ChildShape _:
            ChildShape childShape = ownerEntity as ChildShape;
            rotation = childShape.RotationToRender;
            flipH = childShape.FlipHorizantalToRender;
            flipV = childShape.FlipVerticalToRender;
            flag3 = childShape.TextFrame.Upright;
            break;
          case Shape _:
            Shape shape = ownerEntity as Shape;
            rotation = shape.Rotation;
            flipH = shape.FlipHorizontal;
            flipV = shape.FlipVertical;
            textWrappingStyle = shape.WrapFormat.TextWrappingStyle;
            flag3 = shape.TextFrame.Upright;
            break;
          case WTextBox _:
            WTextBox wtextBox = ownerEntity as WTextBox;
            rotation = wtextBox.Shape != null ? wtextBox.Shape.Rotation : wtextBox.TextBoxFormat.Rotation;
            flipH = wtextBox.TextBoxFormat.FlipHorizontal;
            flipV = wtextBox.TextBoxFormat.FlipVertical;
            textWrappingStyle = wtextBox.TextBoxFormat.TextWrappingStyle;
            flag3 = wtextBox.Shape != null && wtextBox.Shape.TextFrame.Upright;
            break;
          case WTableCell _:
            Entity entity = (Entity) (ownerEntity as WTableCell);
            while (entity is WTableCell)
              entity = (entity as WTableCell).OwnerRow.OwnerTable.Owner;
            if (entity.Owner is WTextBox)
            {
              WTextBox owner = entity.Owner as WTextBox;
              rotation = owner.Shape != null ? owner.Shape.Rotation : owner.TextBoxFormat.Rotation;
              flipH = owner.TextBoxFormat.FlipHorizontal;
              flipV = owner.TextBoxFormat.FlipVertical;
              textWrappingStyle = owner.TextBoxFormat.TextWrappingStyle;
              flag3 = owner.Shape != null && owner.Shape.TextFrame.Upright;
              break;
            }
            if (entity.Owner is Shape)
            {
              Shape owner = entity.Owner as Shape;
              rotation = owner.Rotation;
              flipH = owner.FlipHorizontal;
              flipV = owner.FlipVertical;
              textWrappingStyle = owner.WrapFormat.TextWrappingStyle;
              flag3 = owner.TextFrame.Upright;
              break;
            }
            if (entity.Owner is ChildShape)
            {
              ChildShape owner = entity.Owner as ChildShape;
              rotation = owner.RotationToRender;
              flipH = owner.FlipHorizantalToRender;
              flipV = owner.FlipVerticalToRender;
              flag3 = owner.TextFrame.Upright;
              break;
            }
            break;
        }
      }
    }
    RectangleF bounds1 = (double) num1 != 0.0 ? new RectangleF(bounds.X - num1, bounds.Y, bounds.Width, bounds.Height) : bounds;
    if ((double) width1 == 0.0 || this.IsWidgetNeedToClipBasedOnXPosition(ltWidget, ref width1, bounds1))
    {
      this.ResetTransform();
    }
    else
    {
      RectangleF clipBounds = this.GetClipBounds(bounds, width1, clipTopPosition);
      if ((double) bounds.Width > 0.0 && isNeedToScale)
      {
        this.RotateAndScaleTransform(ref bounds, ref clipBounds, scaling, empty, rotationAngle, false);
        width1 = clipBounds.Width;
      }
      bool isSubSuperScriptNone = charFormat.SubSuperScript == SubSuperScript.None;
      RectangleF textBounds = bounds;
      bool drawLines = false;
      if (((double) rotation != 0.0 || (double) rotation == 0.0 && (flipH || flipV)) && textWrappingStyle != TextWrappingStyle.Tight && textWrappingStyle != TextWrappingStyle.Through)
      {
        if ((double) rotation > 360.0)
          rotation %= 360f;
        if ((double) rotation != 0.0 || flipV || flipH)
        {
          if (flipV)
            flipH = true;
          else if (flipH)
            flipH = false;
          if (!flag3)
            this.SetRotateTransform(this.m_rotateTransform, rotation, flipV, flipH);
          drawLines = true;
        }
      }
      float num2 = 0.0f;
      string fontNameToRender = charFormat.GetFontNameToRender(scriptType);
      if (!this.IsListCharacter && (fontNameToRender == "DIN Offc" || fontNameToRender == "DIN OT") && this.currParagraph != null && this.currParagraph.IsContainDinOffcFont())
        num2 = bounds.Height * 0.2f;
      if (charFormat.Emboss)
      {
        Font font3 = !isSubSuperScriptNone ? charFormat.Document.FontSettings.GetFont(charFormat.GetFontNameToRender(scriptType), this.GetSubSuperScriptFontSize(charFormat.GetFontToRender(scriptType)), charFormat.GetFontToRender(scriptType).Style) : charFormat.GetFontToRender(scriptType);
        if (!charFormat.GetFontNameToRender(scriptType).Equals(font3.Name, StringComparison.OrdinalIgnoreCase) && this.m_privateFontStream != null && this.m_privateFontStream.ContainsKey(charFormat.GetFontNameToRender(scriptType)))
          font3 = this.GetPrivateFont(charFormat.GetFontNameToRender(scriptType), font3.Size, font3.Style);
        SolidBrush solidBrush = this.CreateSolidBrush(Color.Gray);
        if (this.IsUnicode(text))
          this.DrawUnicodeString(text, font3, (Brush) solidBrush, new RectangleF(bounds.X + 0.2f, bounds.Y + 0.2f + num2, bounds.Width, bounds.Height), stringFormat);
        else
          this.Graphics.DrawString(text, font3, (Brush) solidBrush, new RectangleF(bounds.X + 0.2f, bounds.Y + 0.2f + num2, bounds.Width, bounds.Height), stringFormat);
        textBounds = new RectangleF(bounds.X + 0.2f, bounds.Y + 0.2f + num2, bounds.Width, bounds.Height);
      }
      if (charFormat.Engrave)
      {
        Font font4 = !isSubSuperScriptNone ? charFormat.Document.FontSettings.GetFont(charFormat.GetFontNameToRender(scriptType), charFormat.GetFontSizeToRender() / 1.5f, charFormat.GetFontToRender(scriptType).Style) : charFormat.GetFontToRender(scriptType);
        if (!charFormat.GetFontNameToRender(scriptType).Equals(font4.Name, StringComparison.OrdinalIgnoreCase) && this.m_privateFontStream != null && this.m_privateFontStream.ContainsKey(charFormat.GetFontNameToRender(scriptType)))
          font4 = this.GetPrivateFont(charFormat.GetFontNameToRender(scriptType), font4.Size, font4.Style);
        SolidBrush solidBrush = this.CreateSolidBrush(Color.Gray);
        if (this.IsUnicode(text))
          this.DrawUnicodeString(text, font4, (Brush) solidBrush, new RectangleF(bounds.X - 0.2f, bounds.Y - 0.2f + num2, bounds.Width, bounds.Height), stringFormat);
        else
          this.Graphics.DrawString(text, font4, (Brush) solidBrush, new RectangleF(bounds.X - 0.2f, bounds.Y - 0.2f + num2, bounds.Width, bounds.Height), stringFormat);
        textBounds = new RectangleF(bounds.X - 0.2f, bounds.Y - 0.2f + num2, bounds.Width, bounds.Height);
      }
      if (charFormat.AllCaps)
        text = text.ToUpper();
      SizeF sizeF5 = new SizeF();
      SizeF sizeF6 = !flag2 ? this.MeasureString(text, font2, stringFormat, charFormat, false, false) : this.MeasureString(text, font1, font2, stringFormat, charFormat);
      rectangleF = ltWidget.Bounds;
      if ((double) rectangleF.Width != (double) Convert.ToSingle(sizeF6.Width + ltWidget.SubWidth) && text != string.Empty)
      {
        rectangleF = ltWidget.Bounds;
        float num3 = rectangleF.Width - num1 - Convert.ToSingle(sizeF6.Width + ltWidget.SubWidth);
        ltWidget.SubWidth += num3;
        ltWidget.WordSpace = ltWidget.Spaces != 0 ? Convert.ToSingle(ltWidget.SubWidth / (float) ltWidget.Spaces) : 0.0f;
      }
      StringFormat format = new StringFormat(StringFormat.GenericTypographic);
      stringFormat.FormatFlags &= ~StringFormatFlags.LineLimit;
      format.FormatFlags &= ~StringFormatFlags.LineLimit;
      format.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
      format.FormatFlags |= StringFormatFlags.NoClip;
      text.Replace(" ", string.Empty);
      SizeF sizeF7;
      float width2;
      if (flag2)
      {
        sizeF7 = this.MeasureString(text, font1, font2, stringFormat, charFormat);
        width2 = sizeF7.Width;
      }
      else
      {
        sizeF7 = this.MeasureString(text, font2, stringFormat, charFormat, false, true);
        width2 = sizeF7.Width;
      }
      float num4 = 0.0f;
      for (int index = 0; index < strArray.Length; ++index)
      {
        if (flag2)
        {
          double num5 = (double) num4;
          sizeF7 = this.MeasureString(strArray[index], font1, font2, stringFormat, charFormat);
          double width3 = (double) sizeF7.Width;
          num4 = (float) (num5 + width3);
        }
        else
        {
          double num6 = (double) num4;
          sizeF7 = this.MeasureString(strArray[index], font2, stringFormat, charFormat, false, true);
          double width4 = (double) sizeF7.Width;
          num4 = (float) (num6 + width4);
        }
      }
      float num7 = 0.0f;
      if (ltWidget.Spaces > 0)
        num7 = (width2 - num4) / (float) ltWidget.Spaces;
      float num8 = scaling / 100f;
      if ((double) num8 != 1.0)
      {
        clipBounds = new RectangleF(clipBounds.X * num8, clipBounds.Y, clipBounds.Width * num8, clipBounds.Height);
        bounds = new RectangleF(bounds.X * num8, bounds.Y, bounds.Width * num8, bounds.Height);
      }
      if (this.currTextRange != null && this.currTextRange.m_layoutInfo != null && this.currTextRange.m_layoutInfo.IsVerticalText)
        this.TransformGraphicsPosition(ltWidget, isNeedToScale, ref empty, ref rotationAngle, this.currParagraph);
      if ((double) bounds.Width > 0.0 && isNeedToScale)
      {
        this.RotateAndScaleTransform(ref bounds, ref clipBounds, scaling, empty, rotationAngle, false);
        width1 = clipBounds.Width;
      }
      double x1 = (double) bounds.X;
      rectangleF = ltWidget.Bounds;
      double width5 = (double) rectangleF.Width;
      float x2 = (float) (x1 + width5);
      for (int index = 0; index < strArray.Length; ++index)
      {
        bool flag4 = false;
        float width6;
        if (font1.Name != font2.Name && this.IsUnicodeText(strArray[index]))
        {
          sizeF7 = this.MeasureString(strArray[index], font1, font2, stringFormat, charFormat);
          width6 = sizeF7.Width;
        }
        else
        {
          sizeF7 = this.MeasureString(strArray[index], font2, stringFormat, charFormat, false, true);
          width6 = sizeF7.Width;
        }
        if (charFormat.AllCaps)
          strArray[index] = strArray[index].ToUpper();
        if ((ltWidget.CharacterRange & CharacterRangeType.RTL) == CharacterRangeType.RTL)
        {
          format.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
          x2 -= width6;
          this.DrawRTLText(scriptType, ltWidget.CharacterRange, strArray[index], charFormat, font2, brush, new RectangleF(x2, bounds.Y, width6, bounds.Height), format);
        }
        else if (this.IsUnicode(strArray[index]))
        {
          this.DrawChineseText(scriptType, ltWidget.CharacterRange, strArray[index], charFormat, font1, brush, new RectangleF(bounds.X, bounds.Y, width6, bounds.Height), format);
          flag4 = true;
        }
        else
        {
          float num9 = ltWidget.SubWidth / num8;
          if ((double) num9 < 0.0)
            num9 = 0.0f;
          double num10 = (double) width1;
          double x3 = (double) bounds.X;
          rectangleF = ltWidget.Bounds;
          double num11 = (double) rectangleF.X / (double) num8;
          double num12 = x3 - num11;
          float width7 = (float) (num10 - num12) + num9;
          if ((double) width7 < 0.0)
            width7 = 0.0f;
          clipBounds = new RectangleF(bounds.X, bounds.Y, width7, bounds.Height);
          if (this.IsNeedToClip(clipBounds))
            clipBounds = this.ClipBoundsContainer.Peek();
          this.SetClip(clipBounds);
          rectangleF = this.Graphics.ClipBounds;
          if ((double) rectangleF.Width == 0.0)
          {
            this.Graphics.ResetTransform();
            this.ResetClip();
            continue;
          }
          if (charFormat.SmallCaps)
          {
            this.DrawSmallCapString(scriptType, ltWidget.CharacterRange, strArray[index], charFormat, new RectangleF(bounds.X, bounds.Y, width6, bounds.Height), font2, format, brush, false);
          }
          else
          {
            Font font5 = !isSubSuperScriptNone ? charFormat.Document.FontSettings.GetFont(font2.Name, font2.Size / 1.5f, font2.Style) : font2;
            if (!charFormat.GetFontNameToRender(scriptType).Equals(font5.Name, StringComparison.OrdinalIgnoreCase) && this.m_privateFontStream != null && this.m_privateFontStream.ContainsKey(charFormat.GetFontNameToRender(scriptType)))
              font5 = this.GetPrivateFont(charFormat.GetFontNameToRender(scriptType), font5.Size, font5.Style);
            if ((double) charFormat.CharacterSpacing != 0.0)
              this.DrawStringBasedOnCharSpacing(scriptType, ltWidget.CharacterRange, font5, brush, new RectangleF(bounds.X, bounds.Y, width6, bounds.Height), strArray[index], stringFormat, charFormat);
            else
              this.Graphics.DrawString(strArray[index], font5, brush, new RectangleF(bounds.X, bounds.Y + num2, width6, bounds.Height), format);
          }
        }
        Font font6 = flag4 ? font1 : font2;
        float num13 = num7 + ltWidget.WordSpace / num8;
        if (font6.Underline && index != strArray.Length - 1)
        {
          string str = "";
          double num14;
          double width8;
          for (; (double) num13 > 0.0; num13 = (float) (num14 - width8))
          {
            str += " ";
            num14 = (double) num13;
            sizeF7 = this.MeasureString(" ", font2, stringFormat);
            width8 = (double) sizeF7.Width;
          }
          sizeF7 = this.MeasureString(str, font6, stringFormat);
          float width9 = sizeF7.Width;
          Font font7 = !isSubSuperScriptNone ? charFormat.Document.FontSettings.GetFont(font6.Name, font6.Size / 1.5f, font6.Style) : font6;
          if (!charFormat.GetFontNameToRender(scriptType).Equals(font7.Name, StringComparison.OrdinalIgnoreCase) && this.m_privateFontStream != null && this.m_privateFontStream.ContainsKey(charFormat.GetFontNameToRender(scriptType)))
            font7 = this.GetPrivateFont(charFormat.GetFontNameToRender(scriptType), font7.Size, font7.Style);
          this.Graphics.DrawString(str, font7, brush, new RectangleF(bounds.X + width6, bounds.Y + num2, width9, bounds.Height), format);
        }
        if (!ltWidget.IsLastLine && index != strArray.Length - 1)
        {
          RectangleF layoutRectangle = new RectangleF(bounds.X + width6, bounds.Y + num2, num7 + ltWidget.WordSpace, bounds.Height);
          Font font8 = !isSubSuperScriptNone ? charFormat.Document.FontSettings.GetFont(font6.Name, font6.Size / 1.5f, font6.Style) : font6;
          if (!charFormat.GetFontNameToRender(scriptType).Equals(font8.Name, StringComparison.OrdinalIgnoreCase) && this.m_privateFontStream != null && this.m_privateFontStream.ContainsKey(charFormat.GetFontNameToRender(scriptType)))
            font8 = this.GetPrivateFont(charFormat.GetFontNameToRender(scriptType), font8.Size, font8.Style);
          if ((ltWidget.CharacterRange & CharacterRangeType.RTL) == CharacterRangeType.RTL)
          {
            layoutRectangle = new RectangleF(x2 - (num7 + ltWidget.WordSpace), bounds.Y + num2, num7 + ltWidget.WordSpace, bounds.Height);
            x2 -= ltWidget.WordSpace / num8 + num7;
          }
          this.Graphics.DrawString(" ", font8, brush, layoutRectangle);
          float num15 = width6 + ltWidget.WordSpace / num8 + num7;
          bounds.X += num15;
        }
        else
        {
          bounds.X = bounds.X + width6 + num7;
          if ((ltWidget.CharacterRange & CharacterRangeType.RTL) == CharacterRangeType.RTL)
            x2 -= num7;
        }
        this.ResetClip();
      }
      if (flag1)
      {
        bool isSameLine = false;
        this.CheckPreOrNextSiblingIsTab(ref charFormat, ref textBounds, ltWidget, ref isSameLine);
        this.AddLineToCollection(text, isSubSuperScriptNone, font1, charFormat, drawLines, textBounds, isSameLine);
      }
      this.ResetTransform();
    }
  }

  private void RotateAndScaleTransform(
    ref RectangleF bounds,
    ref RectangleF clipBounds,
    float scaleFactor,
    PointF translatePoints,
    float rotationAngle,
    bool isListCharacter)
  {
    scaleFactor /= 100f;
    if ((double) scaleFactor == 0.0)
      scaleFactor = 1f;
    if (!isListCharacter)
    {
      this.ScaleTransformMatrix(scaleFactor, translatePoints, rotationAngle);
      bounds = new RectangleF(bounds.X / scaleFactor, bounds.Y, bounds.Width / scaleFactor, bounds.Height);
      clipBounds = new RectangleF(clipBounds.X / scaleFactor, clipBounds.Y, clipBounds.Width / scaleFactor, clipBounds.Height);
    }
    else
      bounds = new RectangleF(bounds.X, bounds.Y, bounds.Width / scaleFactor, bounds.Height);
  }

  private void ScaleTransformMatrix(float scaleFactor, PointF translatePoints, float rotationAngle)
  {
    Matrix matrix = new Matrix();
    matrix.Rotate(rotationAngle);
    matrix.Scale(scaleFactor, 1f);
    matrix.Translate(translatePoints.X, translatePoints.Y, MatrixOrder.Append);
    this.Graphics.Transform = matrix;
  }

  private void DrawParagraphBorders(
    WParagraph paragraph,
    WParagraphFormat paraFormat,
    LayoutedWidget ltWidget,
    bool isParagraphMarkIsHidden)
  {
    RectangleF bounds = ltWidget.Bounds;
    if (paragraph.m_layoutInfo is ParagraphLayoutInfo layoutInfo1 && layoutInfo1.SkipBottomBorder && layoutInfo1.SkipLeftBorder && layoutInfo1.SkipRightBorder && layoutInfo1.SkipTopBorder && layoutInfo1.SkipHorizonatalBorder)
      return;
    if (ltWidget.ChildWidgets.Count == 1 && (double) ltWidget.ChildWidgets[0].Bounds.Width == 0.0)
    {
      LayoutedWidget childWidget = ltWidget.ChildWidgets[0];
      bool flag = false;
      for (int index = 0; index < childWidget.ChildWidgets.Count; ++index)
      {
        if (!(childWidget.ChildWidgets[index].Widget is BookmarkStart) && !(childWidget.ChildWidgets[index].Widget is BookmarkEnd))
        {
          if (childWidget.ChildWidgets[index].Widget is Break && (childWidget.ChildWidgets[index].Widget as Break).BreakType == BreakType.PageBreak && index == childWidget.ChildWidgets.Count - 1)
            flag = true;
          else
            break;
        }
      }
      if (flag)
        return;
    }
    if (paragraph.GetOwnerEntity() is WTableCell ownerEntity && ownerEntity.Owner is WTableRow owner && owner.m_layoutInfo is RowLayoutInfo layoutInfo2 && paragraph.IsInCell && !layoutInfo2.IsExactlyRowHeight)
      bounds.Height -= paragraph.ParagraphFormat.AfterSpacing;
    if (ltWidget.Owner != null && isParagraphMarkIsHidden)
      this.AddNextParagraphBounds(ltWidget, ref bounds);
    bool flag1 = false;
    bool flag2 = false;
    ILayoutInfo layoutInfo3 = ltWidget.Widget.LayoutInfo;
    if (!(layoutInfo3 is ParagraphLayoutInfo paragraphLayoutInfo))
      return;
    float num1 = paragraphLayoutInfo.FirstLineIndent;
    if ((double) num1 > 0.0)
      num1 = 0.0f;
    float num2 = paragraphLayoutInfo.Paddings.Left + num1;
    float right = paragraphLayoutInfo.Paddings.Right;
    IEntity adjacentParagraph1 = paragraph.NextSibling;
    if (adjacentParagraph1 == null && paragraph.OwnerTextBody.Owner is BlockContentControl)
      adjacentParagraph1 = (paragraph.OwnerTextBody.Owner as BlockContentControl).NextSibling;
    if (adjacentParagraph1 is BlockContentControl)
      adjacentParagraph1 = (IEntity) (adjacentParagraph1 as BlockContentControl).GetFirstParagraphOfSDTContent();
    if (ltWidget.IsLastItemInPage && ltWidget.TextTag == "Splitted")
    {
      flag1 = true;
      bounds.Height -= paragraph.ParagraphFormat.Borders.Bottom.Space;
    }
    if (ltWidget.IsLastItemInPage && ltWidget.TextTag != "Splitted")
      layoutInfo1.SkipBottomBorder = paragraph.ParagraphFormat.Borders.Bottom.BorderType == BorderStyle.None;
    if (adjacentParagraph1 is WParagraph && !ltWidget.IsLastItemInPage && !(adjacentParagraph1 as WParagraph).ParagraphFormat.Borders.NoBorder)
      flag1 = paragraph.IsAdjacentParagraphHaveSameBorders(adjacentParagraph1 as WParagraph, paragraphLayoutInfo.Margins.Left + num1);
    IEntity adjacentParagraph2 = paragraph.PreviousSibling;
    if (adjacentParagraph2 == null && paragraph.OwnerTextBody.Owner is BlockContentControl)
      adjacentParagraph2 = (paragraph.OwnerTextBody.Owner as BlockContentControl).PreviousSibling;
    if (adjacentParagraph2 is BlockContentControl)
      adjacentParagraph2 = (IEntity) (adjacentParagraph2 as BlockContentControl).GetLastParagraphOfSDTContent();
    Borders previousBorder = (Borders) null;
    if (adjacentParagraph2 is WParagraph && !layoutInfo3.IsFirstItemInPage)
    {
      previousBorder = (adjacentParagraph2 as WParagraph).ParagraphFormat.Borders;
      if (!previousBorder.NoBorder)
        flag2 = paragraph.IsAdjacentParagraphHaveSameBorders(adjacentParagraph2 as WParagraph, paragraphLayoutInfo.Margins.Left + num1);
    }
    else if (layoutInfo1 != null)
    {
      layoutInfo1.SkipTopBorder = paragraph.ParagraphFormat.Borders.Top.BorderType == BorderStyle.None;
      layoutInfo1.SkipHorizonatalBorder = true;
    }
    bool flag3 = false;
    if (ltWidget.ChildWidgets.Count > 0 && Math.Round((double) ltWidget.Bounds.Y, 2) != Math.Round((double) ltWidget.ChildWidgets[0].Bounds.Y, 2))
    {
      float num3 = 0.0f;
      if (previousBorder != null)
        num3 = previousBorder.Horizontal.GetLineWidthValue() + previousBorder.Horizontal.Space;
      bounds.Y = ltWidget.ChildWidgets[0].Bounds.Y - (layoutInfo1.SkipTopBorder ? (layoutInfo1.SkipHorizonatalBorder ? 0.0f : num3) : paraFormat.Borders.Top.GetLineWidthValue() + paraFormat.Borders.Top.Space);
      bounds.Height = ltWidget.Bounds.Bottom - bounds.Y;
      flag3 = true;
    }
    Entity entity = paragraph.GetOwnerEntity();
    if (layoutInfo1.SkipBottomBorder)
    {
      bounds.Height += layoutInfo1.BottomMargin;
      if (!paraFormat.BackColor.IsEmpty && !flag1)
        bounds.Height -= layoutInfo1.BottomMargin;
    }
    bounds.Width = bounds.Width - (layoutInfo3 as ILayoutSpacingsInfo).Margins.Left - paraFormat.RightIndent;
    if (ltWidget.ChildWidgets.Count == 1 && ltWidget.TextTag != "Splitted" && !paragraph.IsInCell && paragraph.ParagraphFormat.IsFrame && (double) paragraph.ParagraphFormat.FrameWidth == 0.0 && !paragraph.ParagraphFormat.IsNextParagraphInSameFrame() && !paragraph.ParagraphFormat.IsPreviousParagraphInSameFrame())
      bounds.Width = ltWidget.ChildWidgets[0].Bounds.Width;
    bounds.X += num2;
    bounds.Width += Math.Abs(num2 + right);
    if (!flag3)
      bounds.Height = bounds.Bottom - bounds.Y;
    Borders borders = paraFormat.Borders;
    List<Border> borderList = new List<Border>();
    if (!layoutInfo1.SkipHorizonatalBorder && flag2)
    {
      if (!layoutInfo1.SkipLeftBorder && !layoutInfo1.SkipRightBorder)
      {
        borderList.Add(previousBorder.Horizontal);
        borderList.Add(borders.Left);
        borderList.Add(borders.Right);
        if (!(previousBorder.Horizontal.Color == borders.Left.Color) || !(previousBorder.Horizontal.Color == borders.Right.Color))
        {
          if ((double) previousBorder.Horizontal.LineWidth == (double) borders.Left.LineWidth && (double) previousBorder.Horizontal.LineWidth == (double) borders.Right.LineWidth)
            borderList.Sort((IComparer<Border>) new SortByColorBrightness());
          else if ((double) previousBorder.Horizontal.LineWidth == (double) borders.Left.LineWidth)
            this.SortTwoBorders(borderList, previousBorder.Horizontal, borders.Left, borders, true);
          else if ((double) previousBorder.Horizontal.LineWidth == (double) borders.Right.LineWidth)
            this.SortTwoBorders(borderList, previousBorder.Horizontal, borders.Right, borders, false);
        }
      }
      else if (!layoutInfo1.SkipLeftBorder)
      {
        borderList.Add(previousBorder.Horizontal);
        borderList.Add(borders.Left);
        this.SortTwoBorders(borderList, previousBorder.Horizontal, borders.Left, (Borders) null, false);
      }
      else if (!layoutInfo1.SkipRightBorder)
      {
        borderList.Add(previousBorder.Horizontal);
        borderList.Add(borders.Right);
        this.SortTwoBorders(borderList, previousBorder.Horizontal, borders.Right, (Borders) null, false);
      }
      else
        borderList.Add(previousBorder.Horizontal);
    }
    if (borderList.Count > 0)
    {
      if (!layoutInfo1.SkipBottomBorder && !flag1)
        borderList.Add(borders.Bottom);
    }
    else
    {
      if (!layoutInfo1.SkipLeftBorder)
        borderList.Add(borders.Left);
      if (!layoutInfo1.SkipRightBorder)
        borderList.Add(borders.Right);
      if (!layoutInfo1.SkipTopBorder && !flag2)
        borderList.Add(borders.Top);
      if (!layoutInfo1.SkipBottomBorder && !flag1)
        borderList.Add(borders.Bottom);
    }
    if (borderList.Count <= 0)
      return;
    RectangleF clippingBounds = RectangleF.Empty;
    if (this.ClipBoundsContainer != null && this.ClipBoundsContainer.Count > 0 && !(entity is WTextBox))
      clippingBounds = this.ClipBoundsContainer.Peek();
    this.SetClip(clippingBounds);
    bool flag4 = false;
    while (entity is WTableCell)
      entity = (entity as WTableCell).OwnerRow.OwnerTable.Owner;
    if (entity is WTextBody)
      entity = entity.Owner;
    if (entity is WTextBox && (double) (entity as WTextBox).TextBoxFormat.Rotation != 0.0)
    {
      flag4 = true;
      this.SetRotateTransform(this.m_rotateTransform, (entity as WTextBox).TextBoxFormat.Rotation, (entity as WTextBox).TextBoxFormat.FlipVertical, (entity as WTextBox).TextBoxFormat.FlipHorizontal);
    }
    else if (entity is Shape && (double) (entity as Shape).Rotation != 0.0)
    {
      flag4 = true;
      this.SetRotateTransform(this.m_rotateTransform, (entity as Shape).Rotation, (entity as Shape).FlipVertical, (entity as Shape).FlipHorizontal);
    }
    else if (entity is ChildShape && (double) (entity as ChildShape).Rotation != 0.0)
    {
      flag4 = true;
      ChildShape shapeFrame = entity as ChildShape;
      RectangleF textLayoutingBounds = shapeFrame.TextLayoutingBounds;
      if (shapeFrame.AutoShapeType == AutoShapeType.Rectangle)
        textLayoutingBounds.Height = this.GetLayoutedTextBoxContentHeight(ltWidget);
      this.Rotate((ParagraphItem) shapeFrame, shapeFrame.Rotation, shapeFrame.FlipVertical, shapeFrame.FlipHorizantal, textLayoutingBounds);
    }
    this.DrawParagraphBorders(borderList, bounds, borders, previousBorder, paragraph, ltWidget);
    if (flag4)
      this.ResetTransform();
    if (!(clippingBounds != RectangleF.Empty))
      return;
    this.ResetClip();
  }

  private void DrawParagraphBorders(
    List<Border> borderRenderingOrder,
    RectangleF bounds,
    Borders borders,
    Borders previousBorder,
    WParagraph paragraph,
    LayoutedWidget ltWidget)
  {
    bounds.X -= 1.5f;
    bounds.Width += 3f;
    double num1;
    float betweenBorderLineWidth = (float) (num1 = 0.0);
    float bottomBorderLineWidth = (float) num1;
    float topBorderLineWidth = (float) num1;
    float rightBorderLineWidth = (float) num1;
    float leftBorderLineWidth = (float) num1;
    int num2;
    bool isMultiLineTopBorder = (num2 = 0) != 0;
    bool isMultiLineRightBorder = num2 != 0;
    bool isMultiLineLeftBorder = num2 != 0;
    bool isMultiLineHorizontalBorder = num2 != 0;
    bool isMultiLineBottomBorder = num2 != 0;
    foreach (Border border in borderRenderingOrder)
    {
      switch (border.BorderPosition)
      {
        case Border.BorderPositions.Left:
          isMultiLineLeftBorder = this.IsMultiLineParagraphBorder(border.BorderType);
          leftBorderLineWidth = border.GetLineWidthValue() / 2f;
          continue;
        case Border.BorderPositions.Top:
          isMultiLineTopBorder = this.IsMultiLineParagraphBorder(border.BorderType);
          topBorderLineWidth = border.GetLineWidthValue() / 2f;
          continue;
        case Border.BorderPositions.Right:
          isMultiLineRightBorder = this.IsMultiLineParagraphBorder(border.BorderType);
          rightBorderLineWidth = border.GetLineWidthValue() / 2f;
          continue;
        case Border.BorderPositions.Bottom:
          isMultiLineBottomBorder = this.IsMultiLineParagraphBorder(border.BorderType);
          bottomBorderLineWidth = border.GetLineWidthValue() / 2f;
          continue;
        case Border.BorderPositions.Horizontal:
          isMultiLineHorizontalBorder = this.IsMultiLineParagraphBorder(border.BorderType);
          betweenBorderLineWidth = border.GetLineWidthValue() / 2f;
          continue;
        default:
          continue;
      }
    }
    foreach (Border border in borderRenderingOrder)
    {
      switch (border.BorderPosition)
      {
        case Border.BorderPositions.Left:
          this.DrawLeftBorder(borderRenderingOrder, bounds, borders, border, previousBorder, isMultiLineTopBorder, isMultiLineBottomBorder, isMultiLineLeftBorder, isMultiLineHorizontalBorder, leftBorderLineWidth, topBorderLineWidth, betweenBorderLineWidth, bottomBorderLineWidth, paragraph, ltWidget);
          continue;
        case Border.BorderPositions.Top:
          this.DrawTopBorder(borderRenderingOrder, bounds, borders, border, isMultiLineTopBorder, topBorderLineWidth, leftBorderLineWidth, rightBorderLineWidth);
          continue;
        case Border.BorderPositions.Right:
          this.DrawRightBorder(borderRenderingOrder, bounds, borders, border, previousBorder, isMultiLineTopBorder, isMultiLineHorizontalBorder, isMultiLineRightBorder, isMultiLineBottomBorder, rightBorderLineWidth, topBorderLineWidth, betweenBorderLineWidth, bottomBorderLineWidth, paragraph, ltWidget);
          continue;
        case Border.BorderPositions.Bottom:
          this.DrawBottomBorder(borderRenderingOrder, bounds, borders, border, isMultiLineBottomBorder, leftBorderLineWidth, bottomBorderLineWidth, rightBorderLineWidth, paragraph, ltWidget);
          continue;
        case Border.BorderPositions.Horizontal:
          this.DrawHorizontalBorder(borderRenderingOrder, bounds, borders, border, isMultiLineLeftBorder, isMultiLineRightBorder, isMultiLineHorizontalBorder, betweenBorderLineWidth, leftBorderLineWidth, rightBorderLineWidth, paragraph, ltWidget);
          continue;
        default:
          continue;
      }
    }
  }

  private void DrawHorizontalBorder(
    List<Border> borderRenderingOrder,
    RectangleF bounds,
    Borders borders,
    Border border,
    bool isMultiLineLeftBorder,
    bool isMultiLineRightBorder,
    bool isMultiLineHorizontalBorder,
    float betweenBorderLineWidth,
    float leftBorderLineWidth,
    float rightBorderLineWidth,
    WParagraph paragraph,
    LayoutedWidget ltWidget)
  {
    bool isLeftBorderSame = false;
    bool isRightBorderSame = false;
    bool isOverlapLeft = false;
    bool isOverlapRight = false;
    Border leftBorder = (Border) null;
    Border rightBorder = (Border) null;
    if (borderRenderingOrder.Contains(borders.Left))
    {
      if (isMultiLineLeftBorder)
      {
        isLeftBorderSame = border.BorderType == borders.Left.BorderType && (double) betweenBorderLineWidth == (double) leftBorderLineWidth;
        isOverlapLeft = borderRenderingOrder.IndexOf(border) > borderRenderingOrder.IndexOf(borders.Left);
      }
      leftBorder = borders.Left;
    }
    if (borderRenderingOrder.Contains(borders.Right))
    {
      if (isMultiLineRightBorder)
      {
        isRightBorderSame = border.BorderType == borders.Right.BorderType && (double) betweenBorderLineWidth == (double) rightBorderLineWidth;
        isOverlapRight = borderRenderingOrder.IndexOf(border) > borderRenderingOrder.IndexOf(borders.Right);
      }
      rightBorder = borders.Right;
    }
    float left = bounds.Left;
    float right = bounds.Right;
    float x1 = isLeftBorderSame ? left - leftBorderLineWidth : left + leftBorderLineWidth;
    float x2 = isRightBorderSame ? right + rightBorderLineWidth : right - rightBorderLineWidth;
    if (isMultiLineHorizontalBorder)
      this.DrawMultiLineBetweenBorder(border, new PointF(x1, bounds.Top), new PointF(x2, bounds.Top), isLeftBorderSame, isRightBorderSame, leftBorder, rightBorder, isOverlapLeft, isOverlapRight);
    else
      this.DrawParagraphBorder(border, new PointF(x1, bounds.Top + betweenBorderLineWidth), new PointF(x2, bounds.Top + betweenBorderLineWidth));
  }

  private void DrawLeftBorder(
    List<Border> borderRenderingOrder,
    RectangleF bounds,
    Borders borders,
    Border border,
    Borders previousBorder,
    bool isMultiLineTopBorder,
    bool isMultiLineBottomBorder,
    bool isMultiLineLeftBorder,
    bool isMultiLineHorizontalBorder,
    float leftBorderLineWidth,
    float topBorderLineWidth,
    float betweenBorderLineWidth,
    float bottomBorderLineWidth,
    WParagraph paragraph,
    LayoutedWidget ltWidget)
  {
    float num1 = 0.0f;
    float num2 = 0.0f;
    bool isTopBorderSame = false;
    bool isBottomBorderSame = false;
    bool isBetweenBorderSame = false;
    Entity ownerEntity = paragraph.GetOwnerEntity();
    if (ownerEntity is WTextBox && paragraph.Index == (ownerEntity as WTextBox).TextBoxBody.ChildEntities.Count - 1 || ownerEntity is Shape && paragraph.Index == (ownerEntity as Shape).TextBody.ChildEntities.Count - 1)
      bounds.Height = (float) ((double) ltWidget.Owner.Owner.Bounds.Bottom - (double) bounds.Y - 4.0);
    if (borderRenderingOrder.Contains(borders.Top) && isMultiLineTopBorder)
    {
      isTopBorderSame = border.BorderType == borders.Top.BorderType && (double) leftBorderLineWidth == (double) topBorderLineWidth;
      num1 = isTopBorderSame ? 0.0f : topBorderLineWidth * 2f;
    }
    else if (previousBorder != null && borderRenderingOrder.Contains(previousBorder.Horizontal) && isMultiLineHorizontalBorder)
      isBetweenBorderSame = border.BorderType == previousBorder.Horizontal.BorderType && (double) leftBorderLineWidth == (double) betweenBorderLineWidth && (previousBorder.Horizontal.Color == border.Color || borderRenderingOrder.IndexOf(previousBorder.Horizontal) > borderRenderingOrder.IndexOf(border));
    if (borderRenderingOrder.Contains(borders.Bottom) && isMultiLineBottomBorder)
    {
      isBottomBorderSame = border.BorderType == borders.Bottom.BorderType && (double) leftBorderLineWidth == (double) bottomBorderLineWidth;
      num2 = isBottomBorderSame ? 0.0f : bottomBorderLineWidth * 2f;
    }
    if (isMultiLineLeftBorder)
      this.DrawMultiLineLeftBorder(border, new PointF(bounds.Left - leftBorderLineWidth, bounds.Top + num1), new PointF(bounds.Left - leftBorderLineWidth, bounds.Bottom - bottomBorderLineWidth * 2f), isTopBorderSame, isBetweenBorderSame, isBottomBorderSame);
    else
      this.DrawParagraphBorder(border, new PointF(bounds.Left, bounds.Top + num1), new PointF(bounds.Left, bounds.Bottom - num2));
  }

  private void DrawRightBorder(
    List<Border> borderRenderingOrder,
    RectangleF bounds,
    Borders borders,
    Border border,
    Borders previousBorder,
    bool isMultiLineTopBorder,
    bool isMultiLineHorizontalBorder,
    bool isMultiLineRightBorder,
    bool isMultiLineBottomBorder,
    float rightBorderLineWidth,
    float topBorderLineWidth,
    float betweenBorderLineWidth,
    float bottomBorderLineWidth,
    WParagraph paragraph,
    LayoutedWidget ltWidget)
  {
    float num1 = 0.0f;
    float num2 = 0.0f;
    bool isTopBorderSame = false;
    bool isBottomBorderSame = false;
    bool isBetweenBorderSame = false;
    Entity ownerEntity = paragraph.GetOwnerEntity();
    if (ownerEntity is WTextBox && paragraph.Index == (ownerEntity as WTextBox).TextBoxBody.ChildEntities.Count - 1 || ownerEntity is Shape && paragraph.Index == (ownerEntity as Shape).TextBody.ChildEntities.Count - 1)
      bounds.Height = (float) ((double) ltWidget.Owner.Owner.Bounds.Bottom - (double) bounds.Y - 4.0);
    if (borderRenderingOrder.Contains(borders.Top) && isMultiLineTopBorder)
    {
      isTopBorderSame = border.BorderType == borders.Top.BorderType && (double) rightBorderLineWidth == (double) topBorderLineWidth;
      num1 = isTopBorderSame ? 0.0f : topBorderLineWidth * 2f;
    }
    else if (previousBorder != null && borderRenderingOrder.Contains(previousBorder.Horizontal) && isMultiLineHorizontalBorder)
      isBetweenBorderSame = border.BorderType == previousBorder.Horizontal.BorderType && (double) rightBorderLineWidth == (double) betweenBorderLineWidth && (previousBorder.Horizontal.Color == border.Color || borderRenderingOrder.IndexOf(previousBorder.Horizontal) > borderRenderingOrder.IndexOf(border));
    if (borderRenderingOrder.Contains(borders.Bottom) && isMultiLineBottomBorder)
    {
      isBottomBorderSame = border.BorderType == borders.Bottom.BorderType && (double) rightBorderLineWidth == (double) bottomBorderLineWidth;
      num2 = isBottomBorderSame ? 0.0f : bottomBorderLineWidth * 2f;
    }
    if (isMultiLineRightBorder)
      this.DrawMultiLineRightBorder(border, new PointF(bounds.Right - rightBorderLineWidth, bounds.Top + num1), new PointF(bounds.Right - rightBorderLineWidth, bounds.Bottom - bottomBorderLineWidth * 2f), isTopBorderSame, isBetweenBorderSame, isBottomBorderSame);
    else
      this.DrawParagraphBorder(border, new PointF(bounds.Right, bounds.Top + num1), new PointF(bounds.Right, bounds.Bottom - num2));
  }

  private void DrawTopBorder(
    List<Border> borderRenderingOrder,
    RectangleF bounds,
    Borders borders,
    Border border,
    bool isMultiLineTopBorder,
    float topBorderLineWidth,
    float leftBorderLineWidth,
    float rightBorderLineWidth)
  {
    bool isLeftBorderSame = false;
    bool isRightBorderSame = false;
    if (borderRenderingOrder.Contains(borders.Left))
      isLeftBorderSame = border.BorderType == borders.Left.BorderType && (double) topBorderLineWidth == (double) leftBorderLineWidth;
    if (borderRenderingOrder.Contains(borders.Right))
      isRightBorderSame = border.BorderType == borders.Right.BorderType && (double) topBorderLineWidth == (double) rightBorderLineWidth;
    if (isMultiLineTopBorder)
      this.DrawMultiLineTopBorder(border, new PointF(bounds.Left - leftBorderLineWidth, bounds.Top), new PointF(bounds.Right + rightBorderLineWidth, bounds.Top), isLeftBorderSame, isRightBorderSame);
    else
      this.DrawParagraphBorder(border, new PointF(bounds.Left - leftBorderLineWidth, bounds.Top + topBorderLineWidth), new PointF(bounds.Right + rightBorderLineWidth, bounds.Top + topBorderLineWidth));
  }

  private void DrawBottomBorder(
    List<Border> borderRenderingOrder,
    RectangleF bounds,
    Borders borders,
    Border border,
    bool isMultiLineBottomBorder,
    float leftBorderLineWidth,
    float bottomBorderLineWidth,
    float rightBorderLineWidth,
    WParagraph paragraph,
    LayoutedWidget ltWidget)
  {
    bool isLeftBorderSame = false;
    bool isRightBorderSame = false;
    Entity ownerEntity = paragraph.GetOwnerEntity();
    if (ownerEntity is WTextBox && paragraph.Index == (ownerEntity as WTextBox).TextBoxBody.ChildEntities.Count - 1 || ownerEntity is Shape && paragraph.Index == (ownerEntity as Shape).TextBody.ChildEntities.Count - 1)
      bounds.Y = (float) ((double) ltWidget.Owner.Owner.Bounds.Bottom - (double) bounds.Height - 4.0);
    if (borderRenderingOrder.Contains(borders.Left))
      isLeftBorderSame = border.BorderType == borders.Left.BorderType && (double) leftBorderLineWidth == (double) bottomBorderLineWidth;
    if (borderRenderingOrder.Contains(borders.Right))
      isRightBorderSame = border.BorderType == borders.Right.BorderType && (double) rightBorderLineWidth == (double) bottomBorderLineWidth;
    if (isMultiLineBottomBorder)
      this.DrawMultiLineBottomBorder(border, new PointF(bounds.Left - leftBorderLineWidth, bounds.Bottom - bottomBorderLineWidth * 2f), new PointF(bounds.Right + rightBorderLineWidth, bounds.Bottom - bottomBorderLineWidth * 2f), isLeftBorderSame, isRightBorderSame);
    else
      this.DrawParagraphBorder(border, new PointF(bounds.Left - leftBorderLineWidth, bounds.Bottom - bottomBorderLineWidth), new PointF(bounds.Right + rightBorderLineWidth, bounds.Bottom - bottomBorderLineWidth));
  }

  private void SortTwoBorders(
    List<Border> renderingOrderList,
    Border firstBorder,
    Border secondBorder,
    Borders borders,
    bool isLeftBorder)
  {
    if (firstBorder.Color == secondBorder.Color || (double) firstBorder.LineWidth != (double) secondBorder.LineWidth)
      return;
    if (borders != null)
      renderingOrderList.Remove(isLeftBorder ? borders.Right : borders.Left);
    renderingOrderList.Sort((IComparer<Border>) new SortByColorBrightness());
    if (borders == null)
      return;
    renderingOrderList.Add(isLeftBorder ? borders.Right : borders.Left);
  }

  private void AddNextParagraphBounds(LayoutedWidget layoutedWidget, ref RectangleF bounds)
  {
    while (!(layoutedWidget.Owner.Widget is WSection) && (!(layoutedWidget.Widget is SplitWidgetContainer) || !((layoutedWidget.Widget as SplitWidgetContainer).RealWidgetContainer is WSection)))
      layoutedWidget = layoutedWidget.Owner;
    for (int index = layoutedWidget.Owner.ChildWidgets.IndexOf(layoutedWidget) + 1; index < layoutedWidget.Owner.ChildWidgets.Count; ++index)
    {
      bounds.Height += layoutedWidget.Owner.ChildWidgets[index].Bounds.Bottom - bounds.Bottom;
      if (!(layoutedWidget.Owner.ChildWidgets[index].Widget is WParagraph) || !(layoutedWidget.Owner.ChildWidgets[index].Widget as WParagraph).BreakCharacterFormat.Hidden)
        break;
    }
  }

  internal void DrawRevisionMark(PointF start, PointF end, Color lineColor, float lineWidth)
  {
    RectangleF clipBounds = this.Graphics.ClipBounds;
    this.Graphics.ResetClip();
    this.Graphics.DrawLine(this.CreatePen(lineColor, lineWidth), start, end);
    this.Graphics.SetClip(clipBounds, CombineMode.Replace);
  }

  private void DrawBorder(Border border, PointF start, PointF end)
  {
    if (border.BorderType == BorderStyle.Cleared || (border.BorderType != BorderStyle.None || border.HasNoneStyle) && border.BorderType == BorderStyle.None)
      return;
    this.Graphics.DrawLine(this.GetPen(border, false), start, end);
  }

  private void DrawParagraphBorder(Border border, PointF start, PointF end)
  {
    if (border.BorderType == BorderStyle.Cleared || (border.BorderType != BorderStyle.None || border.HasNoneStyle) && border.BorderType == BorderStyle.None)
      return;
    this.Graphics.DrawLine(this.GetPen(border, true), start, end);
  }

  private void DrawBorder(CellLayoutInfo.CellBorder border, PointF start, PointF end)
  {
    this.Graphics.DrawLine(this.GetPen(border.BorderType, border.RenderingLineWidth, border.BorderColor), start, end);
  }

  internal virtual void DrawTable(WTable table, LayoutedWidget ltWidget)
  {
    if ((double) table.TableFormat.CellSpacing > 0.0)
    {
      RectangleF bounds1 = ltWidget.Bounds;
      Borders borders = table.TableFormat.Borders;
      CellLayoutInfo.CellBorder border1 = new CellLayoutInfo.CellBorder(borders.Left.BorderType, borders.Left.Color, borders.Left.GetLineWidthValue(), borders.Left.LineWidth);
      CellLayoutInfo.CellBorder border2 = new CellLayoutInfo.CellBorder(borders.Right.BorderType, borders.Right.Color, borders.Right.GetLineWidthValue(), borders.Right.LineWidth);
      CellLayoutInfo.CellBorder border3 = new CellLayoutInfo.CellBorder(borders.Top.BorderType, borders.Top.Color, borders.Top.GetLineWidthValue(), borders.Top.LineWidth);
      CellLayoutInfo.CellBorder cellBorder = new CellLayoutInfo.CellBorder(borders.Bottom.BorderType, borders.Bottom.Color, borders.Bottom.GetLineWidthValue(), borders.Bottom.LineWidth);
      if (borders.Left.IsBorderDefined && (double) border1.RenderingLineWidth != 0.0)
      {
        bounds1.X += border1.RenderingLineWidth / 2f;
        bounds1.Width -= border1.RenderingLineWidth / 2f;
      }
      if (borders.Right.IsBorderDefined && (double) border2.RenderingLineWidth != 0.0)
        bounds1.Width -= border2.RenderingLineWidth / 2f;
      if (borders.Top.IsBorderDefined && (double) border3.RenderingLineWidth != 0.0)
      {
        bounds1.Y += border3.RenderingLineWidth / 2f;
        bounds1.Height -= border3.RenderingLineWidth / 2f;
      }
      if (table.TableFormat.TextureStyle != TextureStyle.TextureNone)
        this.DrawTextureStyle(table.TableFormat.TextureStyle, table.TableFormat.ForeColor, table.TableFormat.BackColor, ltWidget.Bounds);
      else if (!table.TableFormat.BackColor.IsEmpty)
        this.Graphics.FillRectangle((Brush) this.CreateSolidBrush(table.TableFormat.BackColor), ltWidget.Bounds);
      RectangleF bounds2 = ltWidget.Bounds;
      if (borders.Left.IsBorderDefined && border1.BorderType != BorderStyle.Cleared && border1.BorderType != BorderStyle.None && table.Rows[0] != null && ((IWidget) table.Rows[0]).LayoutInfo is RowLayoutInfo && ((IWidget) table.Rows[0]).LayoutInfo is RowLayoutInfo layoutInfo1)
      {
        float x = bounds2.Left + layoutInfo1.Paddings.Left;
        this.DrawBorder(border1, new PointF(x, bounds2.Top), new PointF(x, bounds2.Bottom));
      }
      if (borders.Right.IsBorderDefined && border2.BorderType != BorderStyle.Cleared && border2.BorderType != BorderStyle.None)
        this.DrawBorder(border2, new PointF(bounds2.Right, bounds2.Top), new PointF(bounds2.Right, bounds2.Bottom));
      if (borders.Top.IsBorderDefined && border3.BorderType != BorderStyle.Cleared && border3.BorderType != BorderStyle.None && table.Rows[0] != null && ((IWidget) table.Rows[0]).LayoutInfo is RowLayoutInfo && ((IWidget) table.Rows[0]).LayoutInfo is RowLayoutInfo layoutInfo2)
      {
        float x1 = (float) ((double) bounds2.Left + (double) layoutInfo2.Paddings.Left - (double) border1.RenderingLineWidth / 2.0);
        float x2 = bounds2.Right + border2.RenderingLineWidth / 2f;
        float y = bounds2.Top + border3.RenderingLineWidth / 2f;
        this.DrawBorder(border3, new PointF(x1, y), new PointF(x2, y));
      }
      if (borders.Bottom.IsBorderDefined && cellBorder.BorderType != BorderStyle.Cleared && cellBorder.BorderType != BorderStyle.None && table.Rows[0] != null && ((IWidget) table.Rows[0]).LayoutInfo is RowLayoutInfo && ((IWidget) table.Rows[0]).LayoutInfo is RowLayoutInfo layoutInfo3)
      {
        float x3 = (float) ((double) bounds2.Left + (double) layoutInfo3.Paddings.Left - (double) border1.RenderingLineWidth / 2.0);
        float x4 = bounds2.Right + border2.RenderingLineWidth / 2f;
        float y = bounds2.Bottom - cellBorder.RenderingLineWidth / 2f;
        this.DrawBorder(border3, new PointF(x3, y), new PointF(x4, y));
      }
    }
    if (ltWidget.ChildWidgets.Count <= 0)
      return;
    ltWidget.ChildWidgets[0].Widget.LayoutInfo.IsFirstItemInPage = true;
    ltWidget.ChildWidgets[ltWidget.ChildWidgets.Count - 1].IsLastItemInPage = true;
    for (int index = 0; index < ltWidget.ChildWidgets[ltWidget.ChildWidgets.Count - 1].ChildWidgets.Count; ++index)
      ltWidget.ChildWidgets[ltWidget.ChildWidgets.Count - 1].ChildWidgets[index].IsLastItemInPage = true;
  }

  internal virtual void DrawTableRow(WTableRow row, LayoutedWidget ltWidget)
  {
  }

  internal virtual void DrawTableCell(WTableCell cell, LayoutedWidget ltWidget)
  {
    if (!(ltWidget.Widget.LayoutInfo is CellLayoutInfo layoutInfo) || layoutInfo.IsRowMergeContinue || !(ltWidget.Owner.Widget is WTableRow))
      return;
    RectangleF bounds = ltWidget.Bounds;
    Entity entity = (Entity) cell;
    while (entity is WTableCell)
      entity = (entity as WTableCell).OwnerRow.OwnerTable.Owner;
    bool flag1 = false;
    if (entity.Owner is WTextBox)
    {
      WTextBox owner = entity.Owner as WTextBox;
      if ((double) owner.TextBoxFormat.Rotation != 0.0)
      {
        flag1 = true;
        this.SetRotateTransform(this.m_rotateTransform, owner.TextBoxFormat.Rotation, owner.TextBoxFormat.FlipVertical, owner.TextBoxFormat.FlipHorizontal);
      }
    }
    else if (entity.Owner is Shape)
    {
      Shape owner = entity.Owner as Shape;
      if ((double) owner.Rotation != 0.0)
      {
        flag1 = true;
        this.SetRotateTransform(this.m_rotateTransform, owner.Rotation, owner.FlipVertical, owner.FlipHorizontal);
      }
    }
    else if (entity.Owner is ChildShape)
    {
      ChildShape owner = entity.Owner as ChildShape;
      RectangleF textLayoutingBounds = owner.TextLayoutingBounds;
      if (owner.AutoShapeType == AutoShapeType.Rectangle)
        textLayoutingBounds.Height = this.GetLayoutedTextBoxContentHeight(ltWidget);
      if ((double) owner.Rotation != 0.0)
      {
        flag1 = true;
        this.Rotate((ParagraphItem) owner, owner.Rotation, owner.FlipVertical, owner.FlipHorizantal, textLayoutingBounds);
      }
    }
    if ((double) layoutInfo.Paddings.Left > 0.0)
    {
      bounds.X += layoutInfo.Paddings.Left;
      bounds.Width -= layoutInfo.Paddings.Left;
    }
    if ((double) layoutInfo.Paddings.Right > 0.0)
      bounds.Width -= layoutInfo.Paddings.Right;
    if ((double) layoutInfo.Paddings.Top > 0.0)
    {
      bool flag2 = ltWidget.Owner != null && ltWidget.Owner.Owner != null && ltWidget.Owner.Owner.ChildWidgets.IndexOf(ltWidget.Owner) == 0;
      bounds.Y += flag2 ? layoutInfo.TopPadding : layoutInfo.UpdatedTopPadding;
      bounds.Height -= flag2 ? layoutInfo.TopPadding : layoutInfo.UpdatedTopPadding;
    }
    bool flag3 = false;
    if ((cell.TextureStyle != TextureStyle.TextureNone || !cell.CellFormat.BackColor.IsEmpty) && this.IsNeedToClip(bounds))
    {
      flag3 = true;
      RectangleF rectangleF = this.ClipBoundsContainer.Peek();
      if (cell.Document.Settings.CompatibilityMode != CompatibilityMode.Word2013 && this.IsTableInTextBoxOrShape((Entity) cell, false))
        rectangleF = this.TextboxClipBounds(cell.OwnerRow.OwnerTable, rectangleF);
      this.SetClip(rectangleF);
    }
    if (cell.TextureStyle != TextureStyle.TextureNone)
      this.DrawTextureStyle(cell.CellFormat.TextureStyle, cell.CellFormat.ForeColor, cell.CellFormat.BackColor, bounds);
    else if (!cell.CellFormat.BackColor.IsEmpty && cell.Owner is WTableRow)
    {
      if ((double) ltWidget.Bounds.Height < (double) layoutInfo.UpdatedTopPadding && cell.GridSpan > (short) 1 && (cell.Owner as WTableRow).HeightType == TableRowHeightType.Exactly)
        this.FillCellColor(ltWidget);
      else
        this.Graphics.FillRectangle((Brush) this.CreateSolidBrush(cell.CellFormat.BackColor), bounds);
    }
    if (flag1)
      this.ResetTransform();
    if (!flag3)
      return;
    this.ResetClip();
  }

  private void FillCellColor(LayoutedWidget ltWidget)
  {
    if (!(ltWidget.Owner.Widget is WTableRow))
      return;
    int rowIndex = (ltWidget.Owner.Widget as WTableRow).GetRowIndex();
    List<LayoutedWidget> childWidgets = (List<LayoutedWidget>) ltWidget.Owner.Owner.ChildWidgets[rowIndex - 1].ChildWidgets;
    float num = 0.0f;
    foreach (LayoutedWidget layoutedWidget in childWidgets)
    {
      RectangleF bounds = ltWidget.Bounds;
      if (layoutedWidget.Widget is WTableCell)
      {
        Border bottom = (layoutedWidget.Widget as WTableCell).CellFormat.Borders.Bottom;
        bounds.X += num;
        bounds.Width = layoutedWidget.Bounds.Width;
        float lineWidthValue = bottom.GetLineWidthValue();
        if ((double) layoutedWidget.Bounds.X < (double) ltWidget.Bounds.Right && (double) layoutedWidget.Bounds.Right > (double) ltWidget.Bounds.X)
        {
          if ((double) lineWidthValue > 0.0 && (double) lineWidthValue <= (double) bounds.Height)
          {
            bounds.Y += lineWidthValue;
            bounds.Height -= lineWidthValue;
          }
          num += layoutedWidget.Bounds.Width;
          if (ltWidget.Widget is WTableCell)
            this.Graphics.FillRectangle((Brush) this.CreateSolidBrush((ltWidget.Widget as WTableCell).CellFormat.BackColor), bounds);
        }
      }
    }
  }

  internal bool IsTexBoxHaveBackgroundPicture(WTextBox textbox)
  {
    return textbox.TextBoxFormat.FillEfects.Picture != null;
  }

  internal void DrawTextureStyle(
    TextureStyle textureStyle,
    Color foreColor,
    Color backColor,
    RectangleF bounds)
  {
    if (backColor.IsEmpty)
      backColor = Color.White;
    if (textureStyle.ToString().Contains("Percent"))
    {
      float percent = float.Parse(textureStyle.ToString().Replace("Texture", "").Replace("Percent", "").Replace("Pt", "."), (IFormatProvider) CultureInfo.InvariantCulture);
      this.Graphics.FillRectangle((Brush) this.GetBrush(this.GetForeColor(foreColor, backColor, percent)), bounds);
    }
    this.FillTexture(textureStyle, foreColor, backColor, bounds);
  }

  private Color GetForeColor(Color foreColor, Color backColor, float percent)
  {
    foreColor = Color.FromArgb(this.GetColorValue((int) foreColor.R, (int) backColor.R, percent, foreColor.IsEmpty, backColor.IsEmpty), this.GetColorValue((int) foreColor.G, (int) backColor.G, percent, foreColor.IsEmpty, backColor.IsEmpty), this.GetColorValue((int) foreColor.B, (int) backColor.B, percent, foreColor.IsEmpty, backColor.IsEmpty));
    return foreColor;
  }

  private int GetColorValue(
    int foreColorValue,
    int backColorValue,
    float percent,
    bool isForeColorEmpty,
    bool isBackColorEmpty)
  {
    return (double) percent != 100.0 ? (!isForeColorEmpty ? (!isBackColorEmpty ? backColorValue + (int) Math.Round((double) foreColorValue * ((double) percent / 100.0)) - (int) Math.Round((double) backColorValue * ((double) percent / 100.0)) : (int) Math.Round((double) foreColorValue * ((double) percent / 100.0))) : (!isBackColorEmpty ? (int) Math.Round((double) backColorValue * (1.0 - (double) percent / 100.0)) : (int) Math.Round((double) byte.MaxValue * (1.0 - (double) percent / 100.0)))) : foreColorValue;
  }

  private void FillTexture(
    TextureStyle textureStyle,
    Color foreColor,
    Color backColor,
    RectangleF bounds)
  {
    if (foreColor.IsEmpty)
      foreColor = textureStyle != TextureStyle.TextureSolid ? (WordColor.IsNotVeryDarkColor(backColor) ? Color.Black : Color.White) : (WordColor.IsNotVeryDarkColor(foreColor) ? Color.White : Color.Black);
    switch (textureStyle)
    {
      case TextureStyle.TextureSolid:
        this.Graphics.FillRectangle((Brush) this.CreateSolidBrush(foreColor), bounds);
        break;
      case TextureStyle.TextureDarkHorizontal:
        this.Graphics.FillRectangle((Brush) this.CreateHatchBrush(HatchStyle.Horizontal, foreColor, backColor), bounds);
        break;
      case TextureStyle.TextureDarkVertical:
        this.Graphics.FillRectangle((Brush) this.CreateHatchBrush(HatchStyle.DarkVertical, foreColor, backColor), bounds);
        break;
      case TextureStyle.TextureDarkDiagonalDown:
        this.Graphics.FillRectangle((Brush) this.CreateHatchBrush(HatchStyle.DarkDownwardDiagonal, foreColor, backColor), bounds);
        break;
      case TextureStyle.TextureDarkDiagonalUp:
        this.Graphics.FillRectangle((Brush) this.CreateHatchBrush(HatchStyle.DarkUpwardDiagonal, foreColor, backColor), bounds);
        break;
      case TextureStyle.TextureDarkDiagonalCross:
        this.Graphics.FillRectangle((Brush) this.CreateHatchBrush(HatchStyle.DiagonalCross, foreColor, backColor), bounds);
        break;
      case TextureStyle.TextureHorizontal:
        this.Graphics.FillRectangle((Brush) this.CreateHatchBrush(HatchStyle.LightHorizontal, foreColor, backColor), bounds);
        break;
      case TextureStyle.TextureVertical:
        this.Graphics.FillRectangle((Brush) this.CreateHatchBrush(HatchStyle.LightVertical, foreColor, backColor), bounds);
        break;
      case TextureStyle.TextureDiagonalDown:
        this.Graphics.FillRectangle((Brush) this.CreateHatchBrush(HatchStyle.LightDownwardDiagonal, foreColor, backColor), bounds);
        break;
      case TextureStyle.TextureDiagonalUp:
        this.Graphics.FillRectangle((Brush) this.CreateHatchBrush(HatchStyle.LightUpwardDiagonal, foreColor, backColor), bounds);
        break;
      case TextureStyle.TextureCross:
        this.Graphics.FillRectangle((Brush) this.CreateHatchBrush(HatchStyle.Cross, foreColor, backColor), bounds);
        break;
      case TextureStyle.TextureDiagonalCross:
        this.Graphics.FillRectangle((Brush) this.CreateHatchBrush(HatchStyle.DiagonalCross, foreColor, backColor), bounds);
        break;
    }
  }

  internal virtual void DrawCellBorders(WTableCell cell, LayoutedWidget ltWidget)
  {
    CellLayoutInfo layoutInfo1 = ltWidget.Widget.LayoutInfo as CellLayoutInfo;
    RectangleF rectangleF = ltWidget.Bounds;
    if (layoutInfo1 != null && layoutInfo1.IsRowMergeStart && layoutInfo1.SkipBottomBorder)
      rectangleF.Height = ltWidget.Owner.Bounds.Height;
    bool flag1 = false;
    bool flag2 = false;
    if ((double) cell.OwnerRow.RowFormat.CellSpacing > 0.0 || (double) cell.OwnerRow.OwnerTable.TableFormat.CellSpacing > 0.0 && layoutInfo1 != null)
    {
      flag1 = true;
      if (!cell.OwnerRow.OwnerTable.TableFormat.Bidi && layoutInfo1 != null)
        rectangleF = new RectangleF(rectangleF.Left + (layoutInfo1.LeftBorder != null ? layoutInfo1.LeftBorder.RenderingLineWidth / 2f : 0.0f), rectangleF.Top, rectangleF.Width - (float) ((layoutInfo1.LeftBorder != null ? (double) layoutInfo1.LeftBorder.RenderingLineWidth / 2.0 : 0.0) + (layoutInfo1.RightBorder != null ? (double) layoutInfo1.RightBorder.RenderingLineWidth / 2.0 : 0.0)), rectangleF.Height);
      else if (cell.OwnerRow.OwnerTable.TableFormat.Bidi && layoutInfo1 != null)
        rectangleF = new RectangleF(rectangleF.Left + (layoutInfo1.RightBorder != null ? layoutInfo1.RightBorder.RenderingLineWidth / 2f : 0.0f), rectangleF.Top, rectangleF.Width - (float) ((layoutInfo1.RightBorder != null ? (double) layoutInfo1.RightBorder.RenderingLineWidth / 2.0 : 0.0) + (layoutInfo1.LeftBorder != null ? (double) layoutInfo1.LeftBorder.RenderingLineWidth / 2.0 : 0.0)), rectangleF.Height);
    }
    LayoutedWidget owner = ltWidget.Owner.Owner;
    bool isBiDiTable = false;
    if (owner.Widget is WTable && (owner.Widget as WTable).TableFormat.Bidi)
      isBiDiTable = true;
    int num1 = owner.ChildWidgets.IndexOf(ltWidget.Owner);
    bool isFirstRow = num1 == 0;
    bool flag3 = num1 == owner.ChildWidgets.Count - 1;
    bool isLastCell = ltWidget.Owner.ChildWidgets.IndexOf(ltWidget) == ltWidget.Owner.ChildWidgets.Count - 1;
    bool isFirstCell = ltWidget.Owner.ChildWidgets.IndexOf(ltWidget) == 0;
    RowLayoutInfo layoutInfo2 = ((IWidget) cell.OwnerRow).LayoutInfo as RowLayoutInfo;
    if (!isFirstRow && owner.ChildWidgets[num1 - 1].Widget is WTableRow && (owner.ChildWidgets[num1 - 1].Widget as WTableRow).IsHeader && layoutInfo1 != null && layoutInfo1.UpdatedSplittedTopBorders != null)
    {
      layoutInfo1.UpdatedTopBorders.Clear();
      foreach (CellLayoutInfo.CellBorder key in layoutInfo1.UpdatedSplittedTopBorders.Keys)
        layoutInfo1.UpdatedTopBorders.Add(key, layoutInfo1.UpdatedSplittedTopBorders[key]);
    }
    if (!(owner.Widget as WTable).TableFormat.Bidi && !layoutInfo1.SkipLeftBorder && layoutInfo1.LeftBorder != null)
    {
      if (this.IsMultiLineBorder(layoutInfo1.LeftBorder.BorderType))
      {
        this.DrawMultiLineLeftBorder(layoutInfo1, layoutInfo1.LeftBorder, new PointF(rectangleF.Left - layoutInfo1.LeftBorder.RenderingLineWidth / 2f, rectangleF.Top), new PointF(rectangleF.Left - layoutInfo1.LeftBorder.RenderingLineWidth / 2f, rectangleF.Bottom), isFirstRow, flag3 || flag1 && !layoutInfo1.IsRowMergeContinue, isFirstCell, isLastCell);
      }
      else
      {
        CellLayoutInfo.CellBorder cellBorder = isFirstRow ? layoutInfo1.TopBorder : (layoutInfo1.UpdatedTopBorders.Count > 0 ? new List<CellLayoutInfo.CellBorder>((IEnumerable<CellLayoutInfo.CellBorder>) layoutInfo1.UpdatedTopBorders.Keys)[0] : (CellLayoutInfo.CellBorder) null);
        float num2 = cellBorder == null || !this.IsMultiLineBorder(cellBorder.BorderType) || isFirstCell ? 0.0f : cellBorder.RenderingLineWidth;
        this.DrawBorder(layoutInfo1.LeftBorder, new PointF(rectangleF.Left, rectangleF.Top + num2), new PointF(rectangleF.Left, rectangleF.Bottom));
      }
    }
    else if (!layoutInfo1.SkipLeftBorder && layoutInfo1.LeftBorder != null && (owner.Widget as WTable).TableFormat.Bidi)
    {
      if (this.IsMultiLineBorder(layoutInfo1.LeftBorder.BorderType))
      {
        this.DrawMultiLineRightBorder(layoutInfo1, layoutInfo1.LeftBorder, new PointF(rectangleF.Right - layoutInfo1.LeftBorder.RenderingLineWidth / 2f, rectangleF.Top), new PointF(rectangleF.Right - layoutInfo1.LeftBorder.RenderingLineWidth / 2f, rectangleF.Bottom), isFirstRow, flag3 || flag1 && !layoutInfo1.IsRowMergeContinue, isFirstCell, isLastCell);
      }
      else
      {
        CellLayoutInfo.CellBorder cellBorder = isFirstRow ? layoutInfo1.TopBorder : (layoutInfo1.UpdatedTopBorders.Count > 0 ? new List<CellLayoutInfo.CellBorder>((IEnumerable<CellLayoutInfo.CellBorder>) layoutInfo1.UpdatedTopBorders.Keys)[0] : (CellLayoutInfo.CellBorder) null);
        float num3 = cellBorder == null || !this.IsMultiLineBorder(cellBorder.BorderType) || isFirstCell ? 0.0f : cellBorder.RenderingLineWidth;
        this.DrawBorder(layoutInfo1.LeftBorder, new PointF(rectangleF.Right, rectangleF.Top + num3), new PointF(rectangleF.Right, rectangleF.Bottom));
      }
    }
    if (!(owner.Widget as WTable).TableFormat.Bidi && !layoutInfo1.SkipRightBorder && layoutInfo1.RightBorder != null)
    {
      if (this.IsMultiLineBorder(layoutInfo1.RightBorder.BorderType))
      {
        this.DrawMultiLineRightBorder(layoutInfo1, layoutInfo1.RightBorder, new PointF(rectangleF.Right - layoutInfo1.RightBorder.RenderingLineWidth / 2f, rectangleF.Top), new PointF(rectangleF.Right - layoutInfo1.RightBorder.RenderingLineWidth / 2f, rectangleF.Bottom), isFirstRow, flag3 || flag1 && !layoutInfo1.IsRowMergeContinue, isFirstCell, isLastCell);
      }
      else
      {
        CellLayoutInfo.CellBorder cellBorder = isFirstRow ? layoutInfo1.TopBorder : (layoutInfo1.UpdatedTopBorders.Count > 0 ? new List<CellLayoutInfo.CellBorder>((IEnumerable<CellLayoutInfo.CellBorder>) layoutInfo1.UpdatedTopBorders.Keys)[layoutInfo1.UpdatedTopBorders.Count - 1] : (CellLayoutInfo.CellBorder) null);
        float num4 = cellBorder == null || !this.IsMultiLineBorder(cellBorder.BorderType) || isLastCell ? 0.0f : cellBorder.RenderingLineWidth;
        this.DrawBorder(layoutInfo1.RightBorder, new PointF(rectangleF.Right, rectangleF.Top + num4), new PointF(rectangleF.Right, rectangleF.Bottom));
      }
    }
    else if (!layoutInfo1.SkipRightBorder && layoutInfo1.RightBorder != null && (owner.Widget as WTable).TableFormat.Bidi)
    {
      if (this.IsMultiLineBorder(layoutInfo1.RightBorder.BorderType))
      {
        this.DrawMultiLineLeftBorder(layoutInfo1, layoutInfo1.RightBorder, new PointF(rectangleF.Left - layoutInfo1.RightBorder.RenderingLineWidth / 2f, rectangleF.Top), new PointF(rectangleF.Left - layoutInfo1.RightBorder.RenderingLineWidth / 2f, rectangleF.Bottom), isFirstRow, flag3 || flag1 && !layoutInfo1.IsRowMergeContinue, isFirstCell, isLastCell);
      }
      else
      {
        CellLayoutInfo.CellBorder cellBorder = isFirstRow ? layoutInfo1.TopBorder : (layoutInfo1.UpdatedTopBorders.Count > 0 ? new List<CellLayoutInfo.CellBorder>((IEnumerable<CellLayoutInfo.CellBorder>) layoutInfo1.UpdatedTopBorders.Keys)[layoutInfo1.UpdatedTopBorders.Count - 1] : (CellLayoutInfo.CellBorder) null);
        float num5 = cellBorder == null || !this.IsMultiLineBorder(cellBorder.BorderType) || isLastCell ? 0.0f : cellBorder.RenderingLineWidth;
        this.DrawBorder(layoutInfo1.RightBorder, new PointF(rectangleF.Left, rectangleF.Top + num5), new PointF(rectangleF.Left, rectangleF.Bottom));
      }
    }
    if (!layoutInfo1.SkipTopBorder && (!layoutInfo1.IsRowMergeContinue || isLastCell && layoutInfo1.UpdatedTopBorders.Count > 1) && !layoutInfo1.IsColumnMergeContinue)
    {
      if (layoutInfo1.UpdatedTopBorders.Count > 0 && !isFirstRow)
      {
        float num6 = 0.0f;
        List<CellLayoutInfo.CellBorder> cellBorderList = new List<CellLayoutInfo.CellBorder>((IEnumerable<CellLayoutInfo.CellBorder>) layoutInfo1.UpdatedTopBorders.Keys);
        for (int index = 0; index < cellBorderList.Count; ++index)
        {
          if (cellBorderList[index] != null && cellBorderList[index].BorderType != BorderStyle.Cleared && cellBorderList[index].BorderType != BorderStyle.None)
          {
            if (this.IsMultiLineBorder(cellBorderList[index].BorderType))
            {
              float num7 = 0.0f;
              float num8 = 0.0f;
              if (!(owner.Widget as WTable).TableFormat.Bidi)
              {
                float num9 = cellBorderList[index].AdjCellLeftBorder != null ? cellBorderList[index].AdjCellLeftBorder.RenderingLineWidth / 2f : 0.0f;
                num7 = index != 0 || layoutInfo1.LeftBorder == null || (double) layoutInfo1.LeftBorder.RenderingLineWidth / 2.0 <= (double) num9 ? num9 : layoutInfo1.LeftBorder.RenderingLineWidth / 2f;
                float num10 = cellBorderList[index].AdjCellRightBorder != null ? cellBorderList[index].AdjCellRightBorder.RenderingLineWidth / 2f : 0.0f;
                num8 = index != cellBorderList.Count - 1 || layoutInfo1.RightBorder == null || (double) layoutInfo1.RightBorder.RenderingLineWidth / 2.0 <= (double) num10 ? num10 : layoutInfo1.RightBorder.RenderingLineWidth / 2f;
              }
              else if ((owner.Widget as WTable).TableFormat.Bidi)
              {
                float num11 = cellBorderList[index].AdjCellRightBorder != null ? cellBorderList[index].AdjCellRightBorder.RenderingLineWidth / 2f : 0.0f;
                num7 = index != 0 || layoutInfo1.RightBorder == null || (double) layoutInfo1.RightBorder.RenderingLineWidth / 2.0 <= (double) num11 ? num11 : layoutInfo1.RightBorder.RenderingLineWidth / 2f;
                float num12 = cellBorderList[index].AdjCellLeftBorder != null ? cellBorderList[index].AdjCellLeftBorder.RenderingLineWidth / 2f : 0.0f;
                num8 = index != cellBorderList.Count - 1 || layoutInfo1.LeftBorder == null || (double) layoutInfo1.LeftBorder.RenderingLineWidth / 2.0 <= (double) num12 ? num12 : layoutInfo1.LeftBorder.RenderingLineWidth / 2f;
              }
              this.DrawMultiLineTopBorder(layoutInfo1, cellBorderList[index], new PointF(rectangleF.Left - num7 + num6, rectangleF.Top), new PointF(rectangleF.Left + num6 + layoutInfo1.UpdatedTopBorders[cellBorderList[index]] + num8, rectangleF.Top), index == 0, index == cellBorderList.Count - 1);
            }
            else
            {
              float num13 = 0.0f;
              float num14 = 0.0f;
              if (!(owner.Widget as WTable).TableFormat.Bidi)
              {
                float num15 = cellBorderList[index].AdjCellLeftBorder != null ? (this.IsMultiLineBorder(cellBorderList[index].AdjCellLeftBorder.BorderType) ? (float) -((double) cellBorderList[index].AdjCellLeftBorder.RenderingLineWidth / 2.0) : cellBorderList[index].AdjCellLeftBorder.RenderingLineWidth / 2f) : 0.0f;
                num13 = index != 0 || layoutInfo1.LeftBorder == null || layoutInfo1.SkipLeftBorder || (double) layoutInfo1.LeftBorder.RenderingLineWidth / 2.0 <= (double) num15 ? num15 : (this.IsMultiLineBorder(layoutInfo1.LeftBorder.BorderType) ? (float) -((double) layoutInfo1.LeftBorder.RenderingLineWidth / 2.0) : layoutInfo1.LeftBorder.RenderingLineWidth / 2f);
                float num16 = cellBorderList[index].AdjCellRightBorder != null ? (this.IsMultiLineBorder(cellBorderList[index].AdjCellRightBorder.BorderType) ? (float) -((double) cellBorderList[index].AdjCellRightBorder.RenderingLineWidth / 2.0) : cellBorderList[index].AdjCellRightBorder.RenderingLineWidth / 2f) : 0.0f;
                num14 = index != cellBorderList.Count - 1 || layoutInfo1.RightBorder == null || layoutInfo1.SkipRightBorder || (double) layoutInfo1.RightBorder.RenderingLineWidth / 2.0 <= (double) num16 ? num16 : (this.IsMultiLineBorder(layoutInfo1.RightBorder.BorderType) ? (float) -((double) layoutInfo1.RightBorder.RenderingLineWidth / 2.0) : layoutInfo1.RightBorder.RenderingLineWidth / 2f);
              }
              else if ((owner.Widget as WTable).TableFormat.Bidi)
              {
                float num17 = cellBorderList[index].AdjCellRightBorder != null ? (this.IsMultiLineBorder(cellBorderList[index].AdjCellRightBorder.BorderType) ? (float) -((double) cellBorderList[index].AdjCellRightBorder.RenderingLineWidth / 2.0) : cellBorderList[index].AdjCellRightBorder.RenderingLineWidth / 2f) : 0.0f;
                num13 = index != 0 || layoutInfo1.RightBorder == null || layoutInfo1.SkipRightBorder || (double) layoutInfo1.RightBorder.RenderingLineWidth / 2.0 <= (double) num17 ? num17 : (this.IsMultiLineBorder(layoutInfo1.RightBorder.BorderType) ? (float) -((double) layoutInfo1.RightBorder.RenderingLineWidth / 2.0) : layoutInfo1.RightBorder.RenderingLineWidth / 2f);
                float num18 = cellBorderList[index].AdjCellLeftBorder != null ? (this.IsMultiLineBorder(cellBorderList[index].AdjCellLeftBorder.BorderType) ? (float) -((double) cellBorderList[index].AdjCellLeftBorder.RenderingLineWidth / 2.0) : cellBorderList[index].AdjCellLeftBorder.RenderingLineWidth / 2f) : 0.0f;
                num14 = index != cellBorderList.Count - 1 || layoutInfo1.LeftBorder == null || layoutInfo1.SkipLeftBorder || (double) layoutInfo1.LeftBorder.RenderingLineWidth / 2.0 <= (double) num18 ? num18 : (this.IsMultiLineBorder(layoutInfo1.LeftBorder.BorderType) ? (float) -((double) layoutInfo1.LeftBorder.RenderingLineWidth / 2.0) : layoutInfo1.LeftBorder.RenderingLineWidth / 2f);
              }
              PointF start = new PointF(rectangleF.Left - num13 + num6, rectangleF.Top + cellBorderList[index].RenderingLineWidth / 2f);
              PointF end = new PointF(rectangleF.Left + num6 + layoutInfo1.UpdatedTopBorders[cellBorderList[index]] + num14, rectangleF.Top + cellBorderList[index].RenderingLineWidth / 2f);
              this.DrawBorder(cellBorderList[index], start, end);
            }
          }
          num6 += layoutInfo1.UpdatedTopBorders[cellBorderList[index]];
        }
      }
      else if (layoutInfo1.TopBorder != null && layoutInfo1.TopBorder.BorderType != BorderStyle.Cleared && layoutInfo1.TopBorder.BorderType != BorderStyle.None)
      {
        if (this.IsMultiLineBorder(layoutInfo1.TopBorder.BorderType))
        {
          if (!(owner.Widget as WTable).TableFormat.Bidi)
            this.DrawMultiLineTopBorder(layoutInfo1, layoutInfo1.TopBorder, new PointF(rectangleF.Left - (layoutInfo1.LeftBorder != null ? layoutInfo1.LeftBorder.RenderingLineWidth / 2f : 0.0f), rectangleF.Top), new PointF(rectangleF.Right + (layoutInfo1.RightBorder != null ? layoutInfo1.RightBorder.RenderingLineWidth / 2f : 0.0f), rectangleF.Top), true, true);
          else if ((owner.Widget as WTable).TableFormat.Bidi)
            this.DrawMultiLineTopBorder(layoutInfo1, layoutInfo1.TopBorder, new PointF(rectangleF.Left - (layoutInfo1.RightBorder != null ? layoutInfo1.RightBorder.RenderingLineWidth / 2f : 0.0f), rectangleF.Top), new PointF(rectangleF.Right + (layoutInfo1.LeftBorder != null ? layoutInfo1.LeftBorder.RenderingLineWidth / 2f : 0.0f), rectangleF.Top), true, true);
        }
        else
        {
          float num19 = 0.0f;
          float num20 = 0.0f;
          if (!(owner.Widget as WTable).TableFormat.Bidi)
          {
            num19 = layoutInfo1.LeftBorder == null || layoutInfo1.SkipLeftBorder ? 0.0f : layoutInfo1.LeftBorder.RenderingLineWidth / 2f;
            num20 = layoutInfo1.RightBorder == null || layoutInfo1.SkipRightBorder ? 0.0f : layoutInfo1.RightBorder.RenderingLineWidth / 2f;
          }
          else if ((owner.Widget as WTable).TableFormat.Bidi)
          {
            num19 = layoutInfo1.RightBorder == null || layoutInfo1.SkipRightBorder ? 0.0f : layoutInfo1.RightBorder.RenderingLineWidth / 2f;
            num20 = layoutInfo1.LeftBorder == null || layoutInfo1.SkipLeftBorder ? 0.0f : layoutInfo1.LeftBorder.RenderingLineWidth / 2f;
          }
          PointF start = new PointF(rectangleF.Left - num19, rectangleF.Top + layoutInfo1.TopBorder.RenderingLineWidth / 2f);
          PointF end = new PointF(rectangleF.Right + num20, rectangleF.Top + layoutInfo1.TopBorder.RenderingLineWidth / 2f);
          this.DrawBorder(layoutInfo1.TopBorder, start, end);
        }
      }
    }
    if (!layoutInfo1.SkipBottomBorder && (flag3 || flag1 && !layoutInfo1.IsRowMergeContinue || layoutInfo2.IsRowSplittedByFloatingItem) && layoutInfo1.BottomBorder != null)
    {
      if (this.IsMultiLineBorder(layoutInfo1.BottomBorder.BorderType))
      {
        if (!(owner.Widget as WTable).TableFormat.Bidi)
          this.DrawMultiLineBottomBorder(layoutInfo1, new PointF(rectangleF.Left - (layoutInfo1.LeftBorder != null ? layoutInfo1.LeftBorder.RenderingLineWidth / 2f : 0.0f), rectangleF.Bottom), new PointF(rectangleF.Right - (layoutInfo1.RightBorder != null ? layoutInfo1.RightBorder.RenderingLineWidth / 2f : 0.0f), rectangleF.Bottom), isBiDiTable);
        else if ((owner.Widget as WTable).TableFormat.Bidi)
          this.DrawMultiLineBottomBorder(layoutInfo1, new PointF(rectangleF.Left - (layoutInfo1.RightBorder != null ? layoutInfo1.RightBorder.RenderingLineWidth / 2f : 0.0f), rectangleF.Bottom), new PointF(rectangleF.Right - (layoutInfo1.LeftBorder != null ? layoutInfo1.LeftBorder.RenderingLineWidth / 2f : 0.0f), rectangleF.Bottom), isBiDiTable);
      }
      else
      {
        if (cell.NextSibling != null && ((cell.NextSibling as WTableCell).m_layoutInfo as CellLayoutInfo).SkipBottomBorder)
          flag2 = true;
        if (!(owner.Widget as WTable).TableFormat.Bidi)
          this.DrawBorder(layoutInfo1.BottomBorder, new PointF(rectangleF.Left - (layoutInfo1.LeftBorder != null ? layoutInfo1.LeftBorder.RenderingLineWidth / 2f : 0.0f), rectangleF.Bottom + layoutInfo1.BottomPadding / 2f), new PointF(rectangleF.Right + (!isLastCell && !flag1 && !flag2 || layoutInfo1.RightBorder == null ? 0.0f : layoutInfo1.RightBorder.RenderingLineWidth / 2f), rectangleF.Bottom + layoutInfo1.BottomPadding / 2f));
        else if ((owner.Widget as WTable).TableFormat.Bidi)
          this.DrawBorder(layoutInfo1.BottomBorder, new PointF(rectangleF.Left - (layoutInfo1.RightBorder != null ? layoutInfo1.RightBorder.RenderingLineWidth / 2f : 0.0f), rectangleF.Bottom + layoutInfo1.BottomPadding / 2f), new PointF(rectangleF.Right + (!isLastCell && !flag1 && !flag2 || layoutInfo1.LeftBorder == null ? 0.0f : layoutInfo1.LeftBorder.RenderingLineWidth / 2f), rectangleF.Bottom + layoutInfo1.BottomPadding / 2f));
      }
    }
    Border diagonalDown = cell.CellFormat.Borders.DiagonalDown;
    if (diagonalDown.IsBorderDefined && diagonalDown.BorderType != BorderStyle.Cleared || diagonalDown.BorderType != BorderStyle.None)
      this.DrawBorder(diagonalDown, new PointF(rectangleF.Left, rectangleF.Top), new PointF(rectangleF.Right, rectangleF.Bottom));
    Border diagonalUp = cell.CellFormat.Borders.DiagonalUp;
    if ((!diagonalUp.IsBorderDefined || diagonalUp.BorderType == BorderStyle.Cleared) && diagonalUp.BorderType == BorderStyle.None)
      return;
    this.DrawBorder(diagonalUp, new PointF(rectangleF.Left, rectangleF.Bottom), new PointF(rectangleF.Right, rectangleF.Top));
  }

  private void DrawMultiLineLeftBorder(
    CellLayoutInfo cellLayoutInfo,
    CellLayoutInfo.CellBorder leftBorder,
    PointF start,
    PointF end,
    bool isFirstRow,
    bool isLastRow,
    bool isFirstCell,
    bool isLastCell)
  {
    switch (leftBorder.BorderType)
    {
      case BorderStyle.Double:
      case BorderStyle.ThinThickSmallGap:
      case BorderStyle.ThinThinSmallGap:
      case BorderStyle.ThinThickMediumGap:
      case BorderStyle.ThickThinMediumGap:
      case BorderStyle.ThinThickLargeGap:
      case BorderStyle.ThickThinLargeGap:
        this.DrawDoubleLineLeftBorder(cellLayoutInfo, leftBorder, start, end, isFirstRow, isLastRow, isFirstCell, isLastCell);
        break;
    }
  }

  private void DrawDoubleLineLeftBorder(
    CellLayoutInfo cellLayoutInfo,
    CellLayoutInfo.CellBorder leftBorder,
    PointF start,
    PointF end,
    bool isFirstRow,
    bool isLastRow,
    bool isFirstCell,
    bool isLastCell)
  {
    float[] borderLineWidthArray = this.GetBorderLineWidthArray(leftBorder.BorderType, leftBorder.BorderLineWidth);
    Pen pen1 = this.GetPen(leftBorder.BorderType, borderLineWidthArray[0], leftBorder.BorderColor);
    PointF pt1_1 = new PointF(start.X + borderLineWidthArray[0] / 2f, start.Y);
    PointF pt2_1 = new PointF(end.X + borderLineWidthArray[0] / 2f, end.Y);
    if (cellLayoutInfo.PrevCellTopBorder != null && cellLayoutInfo.PrevCellTopBorder.BorderType == leftBorder.BorderType && (double) cellLayoutInfo.PrevCellTopBorder.RenderingLineWidth == (double) leftBorder.RenderingLineWidth)
      pt1_1 = new PointF(pt1_1.X, pt1_1.Y + borderLineWidthArray[0] + borderLineWidthArray[1]);
    if (isLastRow && cellLayoutInfo.PrevCellBottomBorder != null && cellLayoutInfo.PrevCellBottomBorder.BorderType != leftBorder.BorderType && (double) cellLayoutInfo.PrevCellBottomBorder.RenderingLineWidth != (double) leftBorder.RenderingLineWidth && cellLayoutInfo.BottomBorder != null && cellLayoutInfo.BottomBorder.BorderType == leftBorder.BorderType && (double) cellLayoutInfo.BottomBorder.RenderingLineWidth == (double) leftBorder.RenderingLineWidth || isFirstCell && cellLayoutInfo.BottomBorder != null && cellLayoutInfo.BottomBorder.BorderType == leftBorder.BorderType && (double) cellLayoutInfo.BottomBorder.RenderingLineWidth == (double) leftBorder.RenderingLineWidth)
      pt2_1 = new PointF(pt2_1.X, pt2_1.Y + borderLineWidthArray[0] + borderLineWidthArray[1] + borderLineWidthArray[2]);
    this.Graphics.DrawLine(pen1, pt1_1, pt2_1);
    PointF pt1_2 = new PointF((float) ((double) start.X + (double) borderLineWidthArray[0] + (double) borderLineWidthArray[1] + (double) borderLineWidthArray[2] / 2.0), start.Y);
    PointF pt2_2 = new PointF((float) ((double) end.X + (double) borderLineWidthArray[0] + (double) borderLineWidthArray[1] + (double) borderLineWidthArray[2] / 2.0), end.Y);
    Pen pen2 = this.GetPen(leftBorder.BorderType, borderLineWidthArray[2], leftBorder.BorderColor);
    CellLayoutInfo.CellBorder cellBorder = isFirstRow ? cellLayoutInfo.TopBorder : (cellLayoutInfo.UpdatedTopBorders.Count > 0 ? new List<CellLayoutInfo.CellBorder>((IEnumerable<CellLayoutInfo.CellBorder>) cellLayoutInfo.UpdatedTopBorders.Keys)[0] : (CellLayoutInfo.CellBorder) null);
    if (cellBorder != null && cellBorder.BorderType == leftBorder.BorderType && (double) cellBorder.RenderingLineWidth == (double) cellLayoutInfo.LeftBorder.RenderingLineWidth)
      pt1_2 = new PointF(pt1_2.X, pt1_2.Y + borderLineWidthArray[0] + borderLineWidthArray[1]);
    this.Graphics.DrawLine(pen2, pt1_2, pt2_2);
  }

  private void DrawMultiLineRightBorder(
    CellLayoutInfo cellLayoutInfo,
    CellLayoutInfo.CellBorder rightBorder,
    PointF start,
    PointF end,
    bool isFirstRow,
    bool isLastRow,
    bool isFirstCell,
    bool isLastCell)
  {
    switch (rightBorder.BorderType)
    {
      case BorderStyle.Double:
      case BorderStyle.ThinThickSmallGap:
      case BorderStyle.ThinThinSmallGap:
      case BorderStyle.ThinThickMediumGap:
      case BorderStyle.ThickThinMediumGap:
      case BorderStyle.ThinThickLargeGap:
      case BorderStyle.ThickThinLargeGap:
        this.DrawDoubleLineRightBorder(cellLayoutInfo, rightBorder, start, end, isFirstRow, isLastRow, isFirstCell, isLastCell);
        break;
    }
  }

  private void DrawDoubleLineRightBorder(
    CellLayoutInfo cellLayoutInfo,
    CellLayoutInfo.CellBorder rightBorder,
    PointF start,
    PointF end,
    bool isFirstRow,
    bool isLastRow,
    bool isFirstCell,
    bool isLastCell)
  {
    float[] borderLineWidthArray = this.GetBorderLineWidthArray(rightBorder.BorderType, rightBorder.BorderLineWidth);
    Pen pen1 = this.GetPen(rightBorder.BorderType, borderLineWidthArray[0], rightBorder.BorderColor);
    CellLayoutInfo.CellBorder cellBorder = isFirstRow ? cellLayoutInfo.TopBorder : (cellLayoutInfo.UpdatedTopBorders.Count > 0 ? new List<CellLayoutInfo.CellBorder>((IEnumerable<CellLayoutInfo.CellBorder>) cellLayoutInfo.UpdatedTopBorders.Keys)[cellLayoutInfo.UpdatedTopBorders.Count - 1] : (CellLayoutInfo.CellBorder) null);
    PointF pt1_1 = new PointF(start.X + borderLineWidthArray[0] / 2f, start.Y);
    PointF pt2_1 = new PointF(end.X + borderLineWidthArray[0] / 2f, end.Y);
    if (cellBorder != null && cellBorder.BorderType == rightBorder.BorderType && (double) cellBorder.RenderingLineWidth == (double) rightBorder.RenderingLineWidth)
      pt1_1 = new PointF(pt1_1.X, pt1_1.Y + borderLineWidthArray[0] + borderLineWidthArray[1]);
    this.Graphics.DrawLine(pen1, pt1_1, pt2_1);
    PointF pt1_2 = new PointF((float) ((double) start.X + (double) borderLineWidthArray[0] + (double) borderLineWidthArray[1] + (double) borderLineWidthArray[2] / 2.0), start.Y);
    PointF pt2_2 = new PointF((float) ((double) end.X + (double) borderLineWidthArray[0] + (double) borderLineWidthArray[1] + (double) borderLineWidthArray[2] / 2.0), end.Y);
    Pen pen2 = this.GetPen(rightBorder.BorderType, borderLineWidthArray[2], rightBorder.BorderColor);
    if (cellLayoutInfo.NextCellTopBorder != null && cellLayoutInfo.NextCellTopBorder.BorderType == rightBorder.BorderType && (double) cellLayoutInfo.NextCellTopBorder.RenderingLineWidth == (double) rightBorder.RenderingLineWidth)
      pt1_2 = new PointF(pt1_2.X, pt1_2.Y + borderLineWidthArray[0] + borderLineWidthArray[1]);
    if (isLastRow && cellLayoutInfo.NextCellBottomBorder != null && cellLayoutInfo.NextCellBottomBorder.BorderType != rightBorder.BorderType && (double) cellLayoutInfo.NextCellBottomBorder.RenderingLineWidth != (double) rightBorder.RenderingLineWidth && cellLayoutInfo.BottomBorder != null && cellLayoutInfo.BottomBorder.BorderType == rightBorder.BorderType && (double) cellLayoutInfo.BottomBorder.RenderingLineWidth == (double) rightBorder.RenderingLineWidth || isLastCell && cellLayoutInfo.BottomBorder != null && cellLayoutInfo.BottomBorder.BorderType == rightBorder.BorderType && (double) cellLayoutInfo.BottomBorder.RenderingLineWidth == (double) rightBorder.RenderingLineWidth)
      pt2_2 = new PointF(pt2_2.X, pt2_2.Y + borderLineWidthArray[0] + borderLineWidthArray[1] + borderLineWidthArray[2]);
    this.Graphics.DrawLine(pen2, pt1_2, pt2_2);
  }

  private void DrawMultiLineBottomBorder(
    CellLayoutInfo cellLayoutInfo,
    PointF start,
    PointF end,
    bool isBiDiTable)
  {
    switch (cellLayoutInfo.BottomBorder.BorderType)
    {
      case BorderStyle.Double:
      case BorderStyle.ThinThickSmallGap:
      case BorderStyle.ThinThinSmallGap:
      case BorderStyle.ThinThickMediumGap:
      case BorderStyle.ThickThinMediumGap:
      case BorderStyle.ThinThickLargeGap:
      case BorderStyle.ThickThinLargeGap:
        this.DrawDoubleLineBottomBorder(cellLayoutInfo, start, end, isBiDiTable);
        break;
    }
  }

  private void DrawDoubleLineBottomBorder(
    CellLayoutInfo cellLayoutInfo,
    PointF start,
    PointF end,
    bool isBiDiTable)
  {
    float[] borderLineWidthArray = this.GetBorderLineWidthArray(cellLayoutInfo.BottomBorder.BorderType, cellLayoutInfo.BottomBorder.BorderLineWidth);
    Pen pen1 = this.GetPen(cellLayoutInfo.BottomBorder.BorderType, borderLineWidthArray[0], cellLayoutInfo.BottomBorder.BorderColor);
    PointF pt1_1 = new PointF(start.X, start.Y + borderLineWidthArray[0] / 2f);
    PointF pt2_1 = new PointF(end.X, end.Y + borderLineWidthArray[0] / 2f);
    if (!isBiDiTable && cellLayoutInfo.LeftBorder != null && cellLayoutInfo.LeftBorder.BorderType == cellLayoutInfo.BottomBorder.BorderType && (double) cellLayoutInfo.LeftBorder.RenderingLineWidth == (double) cellLayoutInfo.BottomBorder.RenderingLineWidth)
      pt1_1 = new PointF(pt1_1.X + borderLineWidthArray[0] + borderLineWidthArray[1], pt1_1.Y);
    else if (isBiDiTable && cellLayoutInfo.RightBorder != null && cellLayoutInfo.RightBorder.BorderType == cellLayoutInfo.BottomBorder.BorderType && (double) cellLayoutInfo.RightBorder.RenderingLineWidth == (double) cellLayoutInfo.BottomBorder.RenderingLineWidth)
      pt1_1 = new PointF(pt1_1.X + borderLineWidthArray[0] + borderLineWidthArray[1], pt1_1.Y);
    if (!isBiDiTable && cellLayoutInfo.RightBorder != null && cellLayoutInfo.RightBorder.BorderType == cellLayoutInfo.BottomBorder.BorderType && (double) cellLayoutInfo.RightBorder.RenderingLineWidth == (double) cellLayoutInfo.BottomBorder.RenderingLineWidth)
      pt2_1 = new PointF(pt2_1.X + borderLineWidthArray[0], pt2_1.Y);
    else if (isBiDiTable && cellLayoutInfo.LeftBorder != null && cellLayoutInfo.LeftBorder.BorderType == cellLayoutInfo.BottomBorder.BorderType && (double) cellLayoutInfo.LeftBorder.RenderingLineWidth == (double) cellLayoutInfo.BottomBorder.RenderingLineWidth)
      pt2_1 = new PointF(pt2_1.X + borderLineWidthArray[0], pt2_1.Y);
    this.Graphics.DrawLine(pen1, pt1_1, pt2_1);
    Pen pen2 = this.GetPen(cellLayoutInfo.BottomBorder.BorderType, borderLineWidthArray[2], cellLayoutInfo.BottomBorder.BorderColor);
    PointF pt1_2 = new PointF(start.X, (float) ((double) start.Y + (double) borderLineWidthArray[0] + (double) borderLineWidthArray[1] + (double) borderLineWidthArray[2] / 2.0));
    PointF pt2_2 = new PointF(end.X, (float) ((double) start.Y + (double) borderLineWidthArray[0] + (double) borderLineWidthArray[1] + (double) borderLineWidthArray[2] / 2.0));
    if (!isBiDiTable && cellLayoutInfo.RightBorder != null && cellLayoutInfo.RightBorder.BorderType == cellLayoutInfo.BottomBorder.BorderType && (double) cellLayoutInfo.RightBorder.RenderingLineWidth == (double) cellLayoutInfo.BottomBorder.RenderingLineWidth)
      pt2_2 = new PointF(pt2_2.X + borderLineWidthArray[0] + borderLineWidthArray[1] + borderLineWidthArray[2], pt2_2.Y);
    else if (isBiDiTable && cellLayoutInfo.RightBorder != null && cellLayoutInfo.LeftBorder != null && cellLayoutInfo.LeftBorder.BorderType == cellLayoutInfo.BottomBorder.BorderType && (double) cellLayoutInfo.LeftBorder.RenderingLineWidth == (double) cellLayoutInfo.BottomBorder.RenderingLineWidth)
      pt2_2 = new PointF(pt2_2.X + borderLineWidthArray[0] + borderLineWidthArray[1] + borderLineWidthArray[2], pt2_2.Y);
    this.Graphics.DrawLine(pen2, pt1_2, pt2_2);
  }

  private void DrawMultiLineTopBorder(
    CellLayoutInfo cellLayoutInfo,
    CellLayoutInfo.CellBorder topBorder,
    PointF start,
    PointF end,
    bool isStart,
    bool isEnd)
  {
    switch (topBorder.BorderType)
    {
      case BorderStyle.Double:
      case BorderStyle.ThinThickSmallGap:
      case BorderStyle.ThinThinSmallGap:
      case BorderStyle.ThinThickMediumGap:
      case BorderStyle.ThickThinMediumGap:
      case BorderStyle.ThinThickLargeGap:
      case BorderStyle.ThickThinLargeGap:
        this.DrawDoubleLineTopBorder(cellLayoutInfo, topBorder, start, end, isStart, isEnd);
        break;
    }
  }

  private void DrawDoubleLineTopBorder(
    CellLayoutInfo cellLayoutInfo,
    CellLayoutInfo.CellBorder topBorder,
    PointF start,
    PointF end,
    bool isStart,
    bool isEnd)
  {
    float[] borderLineWidthArray = this.GetBorderLineWidthArray(topBorder.BorderType, topBorder.BorderLineWidth);
    Pen pen1 = this.GetPen(topBorder.BorderType, borderLineWidthArray[0], topBorder.BorderColor);
    PointF pt1_1 = new PointF(start.X, start.Y + borderLineWidthArray[0] / 2f);
    PointF pt2_1 = new PointF(end.X, end.Y + borderLineWidthArray[0] / 2f);
    if (topBorder.AdjCellLeftBorder != null && topBorder.AdjCellLeftBorder.BorderType == topBorder.BorderType && (double) topBorder.AdjCellLeftBorder.RenderingLineWidth == (double) topBorder.RenderingLineWidth)
      pt1_1 = new PointF(pt1_1.X + borderLineWidthArray[0] + borderLineWidthArray[1], pt1_1.Y);
    if (topBorder.AdjCellRightBorder != null && topBorder.AdjCellRightBorder.BorderType == topBorder.BorderType && (double) topBorder.AdjCellRightBorder.RenderingLineWidth == (double) topBorder.RenderingLineWidth)
      pt2_1 = new PointF(pt2_1.X - (borderLineWidthArray[1] + borderLineWidthArray[2]), pt2_1.Y);
    this.Graphics.DrawLine(pen1, pt1_1, pt2_1);
    Pen pen2 = this.GetPen(topBorder.BorderType, borderLineWidthArray[2], topBorder.BorderColor);
    PointF pt1_2 = new PointF(start.X, (float) ((double) start.Y + (double) borderLineWidthArray[0] + (double) borderLineWidthArray[1] + (double) borderLineWidthArray[2] / 2.0));
    PointF pt2_2 = new PointF(end.X, (float) ((double) end.Y + (double) borderLineWidthArray[0] + (double) borderLineWidthArray[1] + (double) borderLineWidthArray[2] / 2.0));
    if (isStart && cellLayoutInfo.LeftBorder != null && cellLayoutInfo.LeftBorder.BorderType == topBorder.BorderType && (double) cellLayoutInfo.LeftBorder.RenderingLineWidth == (double) topBorder.RenderingLineWidth)
      pt1_2 = new PointF(pt1_2.X + borderLineWidthArray[0] + borderLineWidthArray[1], pt1_2.Y);
    if (isEnd && cellLayoutInfo.RightBorder != null && cellLayoutInfo.RightBorder.BorderType == topBorder.BorderType && (double) cellLayoutInfo.RightBorder.RenderingLineWidth == (double) topBorder.RenderingLineWidth)
      pt2_2 = new PointF(pt2_2.X - (borderLineWidthArray[1] + borderLineWidthArray[2]), pt2_2.Y);
    this.Graphics.DrawLine(pen2, pt1_2, pt2_2);
  }

  private bool IsMultiLineBorder(BorderStyle borderType)
  {
    switch (borderType)
    {
      case BorderStyle.Double:
      case BorderStyle.ThinThickSmallGap:
      case BorderStyle.ThinThinSmallGap:
      case BorderStyle.ThinThickMediumGap:
      case BorderStyle.ThickThinMediumGap:
      case BorderStyle.ThinThickLargeGap:
      case BorderStyle.ThickThinLargeGap:
        return true;
      default:
        return false;
    }
  }

  private bool IsMultiLineParagraphBorder(BorderStyle borderType)
  {
    switch (borderType)
    {
      case BorderStyle.Double:
      case BorderStyle.Triple:
      case BorderStyle.ThinThickSmallGap:
      case BorderStyle.ThinThinSmallGap:
      case BorderStyle.ThinThickThinSmallGap:
      case BorderStyle.ThinThickMediumGap:
      case BorderStyle.ThickThinMediumGap:
      case BorderStyle.ThickThickThinMediumGap:
      case BorderStyle.ThinThickLargeGap:
      case BorderStyle.ThickThinLargeGap:
      case BorderStyle.ThinThickThinLargeGap:
        return true;
      default:
        return false;
    }
  }

  private float[] GetBorderLineWidthArray(BorderStyle borderType, float lineWidth)
  {
    float[] borderLineWidthArray = new float[1]{ lineWidth };
    switch (borderType)
    {
      case BorderStyle.Double:
        borderLineWidthArray = new float[3]{ 1f, 1f, 1f };
        break;
      case BorderStyle.Triple:
        borderLineWidthArray = new float[5]
        {
          1f,
          1f,
          1f,
          1f,
          1f
        };
        break;
      case BorderStyle.ThinThickSmallGap:
        borderLineWidthArray = new float[3]
        {
          1f,
          -0.75f,
          -0.75f
        };
        break;
      case BorderStyle.ThinThinSmallGap:
        borderLineWidthArray = new float[3]
        {
          -0.75f,
          -0.75f,
          1f
        };
        break;
      case BorderStyle.ThinThickThinSmallGap:
        borderLineWidthArray = new float[5]
        {
          -0.75f,
          -0.75f,
          1f,
          -0.75f,
          -0.75f
        };
        break;
      case BorderStyle.ThinThickMediumGap:
        borderLineWidthArray = new float[3]
        {
          1f,
          0.5f,
          0.5f
        };
        break;
      case BorderStyle.ThickThinMediumGap:
        borderLineWidthArray = new float[3]
        {
          0.5f,
          0.5f,
          1f
        };
        break;
      case BorderStyle.ThickThickThinMediumGap:
        borderLineWidthArray = new float[5]
        {
          0.5f,
          0.5f,
          1f,
          0.5f,
          0.5f
        };
        break;
      case BorderStyle.ThinThickLargeGap:
        borderLineWidthArray = new float[3]
        {
          -1.5f,
          1f,
          -0.75f
        };
        break;
      case BorderStyle.ThickThinLargeGap:
        borderLineWidthArray = new float[3]
        {
          -0.75f,
          1f,
          -1.5f
        };
        break;
      case BorderStyle.ThinThickThinLargeGap:
        borderLineWidthArray = new float[5]
        {
          -0.75f,
          1f,
          -1.5f,
          1f,
          -0.75f
        };
        break;
    }
    if (borderLineWidthArray.Length == 1)
      return new float[1]{ lineWidth };
    for (int index = 0; index < borderLineWidthArray.Length; ++index)
    {
      if ((double) borderLineWidthArray[index] >= 0.0)
        borderLineWidthArray[index] *= lineWidth;
      else
        borderLineWidthArray[index] = Math.Abs(borderLineWidthArray[index]);
    }
    return borderLineWidthArray;
  }

  private bool IsDoubleBorder(Border border)
  {
    switch (border.BorderType)
    {
      case BorderStyle.Double:
      case BorderStyle.ThinThickSmallGap:
      case BorderStyle.ThinThinSmallGap:
      case BorderStyle.ThinThickMediumGap:
      case BorderStyle.ThickThinMediumGap:
      case BorderStyle.ThinThickLargeGap:
      case BorderStyle.ThickThinLargeGap:
        return true;
      default:
        return false;
    }
  }

  private void DrawMultiLineLeftBorder(
    Border leftBorder,
    PointF start,
    PointF end,
    bool isTopBorderSame,
    bool isBetweenBorderSame,
    bool isBottomBorderSame)
  {
    if (this.IsDoubleBorder(leftBorder))
      this.DrawDoubleLineLeftBorder(leftBorder, start, end, isTopBorderSame, isBetweenBorderSame, isBottomBorderSame);
    else
      this.DrawTripleLineLeftBorder(leftBorder, start, end, isTopBorderSame, isBetweenBorderSame, isBottomBorderSame);
  }

  private void DrawDoubleLineLeftBorder(
    Border leftBorder,
    PointF start,
    PointF end,
    bool isTopBorderSame,
    bool isBetweenBorderSame,
    bool isBottomBorderSame)
  {
    float[] borderLineWidthArray = this.GetBorderLineWidthArray(leftBorder.BorderType, leftBorder.LineWidth);
    Pen pen1 = this.GetPen(leftBorder.BorderType, borderLineWidthArray[0], leftBorder.Color);
    PointF pt1_1 = new PointF(start.X + borderLineWidthArray[0] / 2f, start.Y);
    PointF pt2_1 = new PointF(end.X + borderLineWidthArray[0] / 2f, end.Y);
    if (isBottomBorderSame)
      pt2_1 = new PointF(pt2_1.X, pt2_1.Y + borderLineWidthArray[0] + borderLineWidthArray[1] + borderLineWidthArray[2]);
    this.Graphics.DrawLine(pen1, pt1_1, pt2_1);
    Pen pen2 = this.GetPen(leftBorder.BorderType, borderLineWidthArray[2], leftBorder.Color);
    PointF pt1_2 = new PointF((float) ((double) start.X + (double) borderLineWidthArray[0] + (double) borderLineWidthArray[1] + (double) borderLineWidthArray[2] / 2.0), start.Y);
    PointF pt2_2 = new PointF((float) ((double) end.X + (double) borderLineWidthArray[0] + (double) borderLineWidthArray[1] + (double) borderLineWidthArray[2] / 2.0), end.Y);
    if (isTopBorderSame || isBetweenBorderSame)
      pt1_2 = new PointF(pt1_2.X, pt1_2.Y + borderLineWidthArray[0] + borderLineWidthArray[1]);
    this.Graphics.DrawLine(pen2, pt1_2, pt2_2);
  }

  private void DrawTripleLineLeftBorder(
    Border leftBorder,
    PointF start,
    PointF end,
    bool isTopBorderSame,
    bool isBetweenBorderSame,
    bool isBottomBorderSame)
  {
    float[] borderLineWidthArray = this.GetBorderLineWidthArray(leftBorder.BorderType, leftBorder.LineWidth);
    Pen pen1 = this.GetPen(leftBorder.BorderType, borderLineWidthArray[0], leftBorder.Color);
    PointF pt1_1 = new PointF(start.X + borderLineWidthArray[0] / 2f, start.Y);
    PointF pt2_1 = new PointF(end.X + borderLineWidthArray[0] / 2f, end.Y);
    if (isBottomBorderSame)
      pt2_1 = new PointF(pt2_1.X, pt2_1.Y + borderLineWidthArray[0] + borderLineWidthArray[1] + borderLineWidthArray[2] + borderLineWidthArray[3] + borderLineWidthArray[4]);
    this.Graphics.DrawLine(pen1, pt1_1, pt2_1);
    Pen pen2 = this.GetPen(leftBorder.BorderType, borderLineWidthArray[2], leftBorder.Color);
    PointF pt1_2 = new PointF(start.X + (float) ((double) borderLineWidthArray[0] + (double) borderLineWidthArray[1] + (double) borderLineWidthArray[2] / 2.0), start.Y);
    PointF pt2_2 = new PointF(end.X + (float) ((double) borderLineWidthArray[0] + (double) borderLineWidthArray[1] + (double) borderLineWidthArray[2] / 2.0), end.Y);
    if (isTopBorderSame)
      pt1_2 = new PointF(pt1_2.X, pt1_2.Y + borderLineWidthArray[0] + borderLineWidthArray[1]);
    if (isBottomBorderSame)
      pt2_2 = new PointF(pt2_2.X, pt2_2.Y + borderLineWidthArray[0] + borderLineWidthArray[1] + borderLineWidthArray[2]);
    this.Graphics.DrawLine(pen2, pt1_2, pt2_2);
    Pen pen3 = this.GetPen(leftBorder.BorderType, borderLineWidthArray[4], leftBorder.Color);
    PointF pt1_3 = new PointF((float) ((double) start.X + (double) borderLineWidthArray[0] + (double) borderLineWidthArray[1] + (double) borderLineWidthArray[2] + (double) borderLineWidthArray[3] + (double) borderLineWidthArray[4] / 2.0), start.Y);
    PointF pt2_3 = new PointF((float) ((double) end.X + (double) borderLineWidthArray[0] + (double) borderLineWidthArray[1] + (double) borderLineWidthArray[2] + (double) borderLineWidthArray[3] + (double) borderLineWidthArray[4] / 2.0), end.Y);
    if (isTopBorderSame || isBetweenBorderSame)
      pt1_3 = new PointF(pt1_3.X, pt1_3.Y + borderLineWidthArray[0] + borderLineWidthArray[1] + borderLineWidthArray[2] + borderLineWidthArray[3]);
    this.Graphics.DrawLine(pen3, pt1_3, pt2_3);
  }

  private void DrawMultiLineRightBorder(
    Border rightBorder,
    PointF start,
    PointF end,
    bool isTopBorderSame,
    bool isBetweenBorderSame,
    bool isBottomBorderSame)
  {
    if (this.IsDoubleBorder(rightBorder))
      this.DrawDoubleLineRightBorder(rightBorder, start, end, isTopBorderSame, isBetweenBorderSame, isBottomBorderSame);
    else
      this.DrawTripleLineRightBorder(rightBorder, start, end, isTopBorderSame, isBetweenBorderSame, isBottomBorderSame);
  }

  private void DrawDoubleLineRightBorder(
    Border rightBorder,
    PointF start,
    PointF end,
    bool isTopBorderSame,
    bool isBetweenBorderSame,
    bool isBottomBorderSame)
  {
    float[] borderLineWidthArray = this.GetBorderLineWidthArray(rightBorder.BorderType, rightBorder.LineWidth);
    Pen pen1 = this.GetPen(rightBorder.BorderType, borderLineWidthArray[0], rightBorder.Color);
    PointF pt1_1 = new PointF(start.X + borderLineWidthArray[0] / 2f, start.Y);
    PointF pt2_1 = new PointF(end.X + borderLineWidthArray[0] / 2f, end.Y);
    if (isTopBorderSame || isBetweenBorderSame)
      pt1_1 = new PointF(pt1_1.X, pt1_1.Y + borderLineWidthArray[0] + borderLineWidthArray[1]);
    this.Graphics.DrawLine(pen1, pt1_1, pt2_1);
    Pen pen2 = this.GetPen(rightBorder.BorderType, borderLineWidthArray[2], rightBorder.Color);
    PointF pt1_2 = new PointF((float) ((double) start.X + (double) borderLineWidthArray[0] + (double) borderLineWidthArray[1] + (double) borderLineWidthArray[2] / 2.0), start.Y);
    PointF pt2_2 = new PointF((float) ((double) end.X + (double) borderLineWidthArray[0] + (double) borderLineWidthArray[1] + (double) borderLineWidthArray[2] / 2.0), end.Y);
    if (isBottomBorderSame)
      pt2_2 = new PointF(pt2_2.X, pt2_2.Y + borderLineWidthArray[0] + borderLineWidthArray[1] + borderLineWidthArray[2]);
    this.Graphics.DrawLine(pen2, pt1_2, pt2_2);
  }

  private void DrawTripleLineRightBorder(
    Border rightBorder,
    PointF start,
    PointF end,
    bool isTopBorderSame,
    bool isBetweenBorderSame,
    bool isBottomBorderSame)
  {
    float[] borderLineWidthArray = this.GetBorderLineWidthArray(rightBorder.BorderType, rightBorder.LineWidth);
    Pen pen1 = this.GetPen(rightBorder.BorderType, borderLineWidthArray[0], rightBorder.Color);
    PointF pt1_1 = new PointF(start.X + borderLineWidthArray[0] / 2f, start.Y);
    PointF pt2_1 = new PointF(end.X + borderLineWidthArray[0] / 2f, end.Y);
    if (isTopBorderSame || isBetweenBorderSame)
      pt1_1 = new PointF(pt1_1.X, pt1_1.Y + borderLineWidthArray[0] + borderLineWidthArray[1] + borderLineWidthArray[2] + borderLineWidthArray[3]);
    this.Graphics.DrawLine(pen1, pt1_1, pt2_1);
    Pen pen2 = this.GetPen(rightBorder.BorderType, borderLineWidthArray[2], rightBorder.Color);
    PointF pt1_2 = new PointF((float) ((double) start.X + (double) borderLineWidthArray[0] + (double) borderLineWidthArray[1] + (double) borderLineWidthArray[2] / 2.0), start.Y);
    PointF pt2_2 = new PointF((float) ((double) end.X + (double) borderLineWidthArray[0] + (double) borderLineWidthArray[1] + (double) borderLineWidthArray[2] / 2.0), end.Y);
    if (isTopBorderSame)
      pt1_2 = new PointF(pt1_2.X, pt1_2.Y + borderLineWidthArray[0] + borderLineWidthArray[1]);
    if (isBottomBorderSame)
      pt2_2 = new PointF(pt2_2.X, pt2_2.Y + borderLineWidthArray[0] + borderLineWidthArray[1] + borderLineWidthArray[2]);
    this.Graphics.DrawLine(pen2, pt1_2, pt2_2);
    Pen pen3 = this.GetPen(rightBorder.BorderType, borderLineWidthArray[4], rightBorder.Color);
    PointF pt1_3 = new PointF((float) ((double) start.X + (double) borderLineWidthArray[0] + (double) borderLineWidthArray[1] + ((double) borderLineWidthArray[2] + (double) borderLineWidthArray[3] + (double) borderLineWidthArray[4] / 2.0)), start.Y);
    PointF pt2_3 = new PointF((float) ((double) end.X + (double) borderLineWidthArray[0] + (double) borderLineWidthArray[1] + (double) borderLineWidthArray[2] + (double) borderLineWidthArray[3] + (double) borderLineWidthArray[4] / 2.0), end.Y);
    if (isBottomBorderSame)
      pt2_3 = new PointF(pt2_3.X, pt2_3.Y + borderLineWidthArray[0] + borderLineWidthArray[1] + borderLineWidthArray[2] + borderLineWidthArray[3] + borderLineWidthArray[4]);
    this.Graphics.DrawLine(pen3, pt1_3, pt2_3);
  }

  private void DrawMultiLineTopBorder(
    Border topBorder,
    PointF start,
    PointF end,
    bool isLeftBorderSame,
    bool isRightBorderSame)
  {
    if (this.IsDoubleBorder(topBorder))
      this.DrawDoubleLineTopBorder(topBorder, start, end, isLeftBorderSame, isRightBorderSame);
    else
      this.DrawTripleLineTopBorder(topBorder, start, end, isLeftBorderSame, isRightBorderSame);
  }

  private void DrawDoubleLineTopBorder(
    Border topBorder,
    PointF start,
    PointF end,
    bool isLeftBorderSame,
    bool isRightBorderSame)
  {
    float[] borderLineWidthArray = this.GetBorderLineWidthArray(topBorder.BorderType, topBorder.LineWidth);
    this.Graphics.DrawLine(this.GetPen(topBorder.BorderType, borderLineWidthArray[0], topBorder.Color), new PointF(start.X, start.Y + borderLineWidthArray[0] / 2f), new PointF(end.X, end.Y + borderLineWidthArray[0] / 2f));
    Pen pen = this.GetPen(topBorder.BorderType, borderLineWidthArray[2], topBorder.Color);
    PointF pt1 = new PointF(start.X, (float) ((double) start.Y + (double) borderLineWidthArray[0] + (double) borderLineWidthArray[1] + (double) borderLineWidthArray[2] / 2.0));
    PointF pt2 = new PointF(end.X, (float) ((double) end.Y + (double) borderLineWidthArray[0] + (double) borderLineWidthArray[1] + (double) borderLineWidthArray[2] / 2.0));
    if (isLeftBorderSame)
      pt1 = new PointF(pt1.X + borderLineWidthArray[0] + borderLineWidthArray[1], pt1.Y);
    if (isRightBorderSame)
      pt2 = new PointF(pt2.X - (borderLineWidthArray[1] + borderLineWidthArray[2]), pt2.Y);
    this.Graphics.DrawLine(pen, pt1, pt2);
  }

  private void DrawTripleLineTopBorder(
    Border topBorder,
    PointF start,
    PointF end,
    bool isLeftBorderSame,
    bool isRightBorderSame)
  {
    float[] borderLineWidthArray = this.GetBorderLineWidthArray(topBorder.BorderType, topBorder.LineWidth);
    this.Graphics.DrawLine(this.GetPen(topBorder.BorderType, borderLineWidthArray[0], topBorder.Color), new PointF(start.X, start.Y + borderLineWidthArray[0] / 2f), new PointF(end.X, end.Y + borderLineWidthArray[0] / 2f));
    Pen pen1 = this.GetPen(topBorder.BorderType, borderLineWidthArray[2], topBorder.Color);
    PointF pt1_1 = new PointF(start.X, (float) ((double) start.Y + (double) borderLineWidthArray[0] + (double) borderLineWidthArray[1] + (double) borderLineWidthArray[2] / 2.0));
    PointF pt2_1 = new PointF(end.X, (float) ((double) end.Y + (double) borderLineWidthArray[0] + (double) borderLineWidthArray[1] + (double) borderLineWidthArray[2] / 2.0));
    if (isLeftBorderSame)
      pt1_1 = new PointF(pt1_1.X + borderLineWidthArray[0] + borderLineWidthArray[1], pt1_1.Y);
    if (isRightBorderSame)
      pt2_1 = new PointF(pt2_1.X - (borderLineWidthArray[3] + borderLineWidthArray[4]), pt2_1.Y);
    this.Graphics.DrawLine(pen1, pt1_1, pt2_1);
    Pen pen2 = this.GetPen(topBorder.BorderType, borderLineWidthArray[4], topBorder.Color);
    PointF pt1_2 = new PointF(start.X, (float) ((double) start.Y + (double) borderLineWidthArray[0] + (double) borderLineWidthArray[1] + (double) borderLineWidthArray[2] + (double) borderLineWidthArray[3] + (double) borderLineWidthArray[4] / 2.0));
    PointF pt2_2 = new PointF(end.X, (float) ((double) end.Y + (double) borderLineWidthArray[0] + (double) borderLineWidthArray[1] + (double) borderLineWidthArray[2] + (double) borderLineWidthArray[3] + (double) borderLineWidthArray[4] / 2.0));
    if (isLeftBorderSame)
      pt1_2 = new PointF(pt1_2.X + borderLineWidthArray[0] + borderLineWidthArray[1] + borderLineWidthArray[2] + borderLineWidthArray[3], pt1_2.Y);
    if (isRightBorderSame)
      pt2_2 = new PointF(pt2_2.X - (borderLineWidthArray[1] + borderLineWidthArray[2] + borderLineWidthArray[3] + borderLineWidthArray[4]), pt2_2.Y);
    this.Graphics.DrawLine(pen2, pt1_2, pt2_2);
  }

  private void DrawMultiLineBottomBorder(
    Border bottomBorder,
    PointF start,
    PointF end,
    bool isLeftBorderSame,
    bool isRightBorderSame)
  {
    if (this.IsDoubleBorder(bottomBorder))
      this.DrawDoubleLineBottomBorder(bottomBorder, start, end, isLeftBorderSame, isRightBorderSame);
    else
      this.DrawTripleLineBottomBorder(bottomBorder, start, end, isLeftBorderSame, isRightBorderSame);
  }

  private void DrawDoubleLineBottomBorder(
    Border bottomBorder,
    PointF start,
    PointF end,
    bool isLeftBorderSame,
    bool isRightBorderSame)
  {
    float[] borderLineWidthArray = this.GetBorderLineWidthArray(bottomBorder.BorderType, bottomBorder.LineWidth);
    Pen pen = this.GetPen(bottomBorder.BorderType, borderLineWidthArray[0], bottomBorder.Color);
    PointF pt1 = new PointF(start.X, start.Y + borderLineWidthArray[0] / 2f);
    PointF pt2 = new PointF(end.X, end.Y + borderLineWidthArray[0] / 2f);
    if (isLeftBorderSame)
      pt1 = new PointF(pt1.X + borderLineWidthArray[0] + borderLineWidthArray[1], pt1.Y);
    if (isRightBorderSame)
      pt2 = new PointF(pt2.X - (borderLineWidthArray[1] + borderLineWidthArray[2]), pt2.Y);
    this.Graphics.DrawLine(pen, pt1, pt2);
    this.Graphics.DrawLine(this.GetPen(bottomBorder.BorderType, borderLineWidthArray[2], bottomBorder.Color), new PointF(start.X, (float) ((double) start.Y + (double) borderLineWidthArray[0] + (double) borderLineWidthArray[1] + (double) borderLineWidthArray[2] / 2.0)), new PointF(end.X, (float) ((double) end.Y + (double) borderLineWidthArray[0] + (double) borderLineWidthArray[1] + (double) borderLineWidthArray[2] / 2.0)));
  }

  private void DrawDoubleLine(
    WCharacterFormat charFormat,
    BorderStyle borderType,
    float lineWidth,
    PointF start,
    PointF end)
  {
    float[] borderLineWidthArray = this.GetBorderLineWidthArray(borderType, lineWidth);
    Color textColor = this.GetTextColor(charFormat);
    Color borderColor = Color.Black;
    if (textColor != Color.Black)
      borderColor = textColor;
    this.Graphics.DrawLine(this.GetPen(borderType, borderLineWidthArray[0], borderColor), new PointF(start.X, start.Y + borderLineWidthArray[0] / 2f), new PointF(end.X, end.Y + borderLineWidthArray[0] / 2f));
    this.Graphics.DrawLine(this.GetPen(borderType, borderLineWidthArray[2], borderColor), new PointF(start.X, (float) ((double) start.Y + (double) borderLineWidthArray[0] + (double) borderLineWidthArray[1] + (double) borderLineWidthArray[2] / 2.0)), new PointF(end.X, (float) ((double) end.Y + (double) borderLineWidthArray[0] + (double) borderLineWidthArray[1] + (double) borderLineWidthArray[2] / 2.0)));
  }

  private void DrawTripleLineBottomBorder(
    Border bottomBorder,
    PointF start,
    PointF end,
    bool isLeftBorderSame,
    bool isRightBorderSame)
  {
    float[] borderLineWidthArray = this.GetBorderLineWidthArray(bottomBorder.BorderType, bottomBorder.LineWidth);
    Pen pen1 = this.GetPen(bottomBorder.BorderType, borderLineWidthArray[0], bottomBorder.Color);
    PointF pt1_1 = new PointF(start.X, start.Y + borderLineWidthArray[0] / 2f);
    PointF pt2_1 = new PointF(end.X, end.Y + borderLineWidthArray[0] / 2f);
    if (isLeftBorderSame)
      pt1_1 = new PointF(pt1_1.X + borderLineWidthArray[0] + borderLineWidthArray[1] + borderLineWidthArray[2] + borderLineWidthArray[3], pt1_1.Y);
    if (isRightBorderSame)
      pt2_1 = new PointF(pt2_1.X - (borderLineWidthArray[1] + borderLineWidthArray[2] + borderLineWidthArray[3] + borderLineWidthArray[4]), pt2_1.Y);
    this.Graphics.DrawLine(pen1, pt1_1, pt2_1);
    Pen pen2 = this.GetPen(bottomBorder.BorderType, borderLineWidthArray[2], bottomBorder.Color);
    PointF pt1_2 = new PointF(start.X, (float) ((double) start.Y + (double) borderLineWidthArray[0] + (double) borderLineWidthArray[1] + (double) borderLineWidthArray[2] / 2.0));
    PointF pt2_2 = new PointF(end.X, (float) ((double) end.Y + (double) borderLineWidthArray[0] + (double) borderLineWidthArray[1] + (double) borderLineWidthArray[2] / 2.0));
    if (isLeftBorderSame)
      pt1_2 = new PointF(pt1_2.X + borderLineWidthArray[0] + borderLineWidthArray[1], pt1_2.Y);
    if (isRightBorderSame)
      pt2_2 = new PointF(pt2_2.X - (borderLineWidthArray[1] + borderLineWidthArray[2]), pt2_2.Y);
    this.Graphics.DrawLine(pen2, pt1_2, pt2_2);
    this.Graphics.DrawLine(this.GetPen(bottomBorder.BorderType, borderLineWidthArray[4], bottomBorder.Color), new PointF(start.X, (float) ((double) start.Y + (double) borderLineWidthArray[0] + (double) borderLineWidthArray[1] + (double) borderLineWidthArray[2] + (double) borderLineWidthArray[3] + (double) borderLineWidthArray[4] / 2.0)), new PointF(end.X, (float) ((double) end.Y + (double) borderLineWidthArray[0] + (double) borderLineWidthArray[1] + (double) borderLineWidthArray[2] + (double) borderLineWidthArray[3] + (double) borderLineWidthArray[4] / 2.0)));
  }

  private void DrawMultiLineBetweenBorder(
    Border betweenBorder,
    PointF start,
    PointF end,
    bool isLeftBorderSame,
    bool isRightBorderSame,
    Border leftBorder,
    Border rightBorder,
    bool isOverlapLeft,
    bool isOverlapRight)
  {
    if (this.IsDoubleBorder(betweenBorder))
      this.DrawDoubleLineBetweenBorder(betweenBorder, start, end, isLeftBorderSame, isRightBorderSame, leftBorder, rightBorder, isOverlapLeft, isOverlapRight);
    else
      this.DrawTripleLineBetweenBorder(betweenBorder, start, end, isLeftBorderSame, isRightBorderSame, leftBorder, rightBorder, isOverlapLeft, isOverlapRight);
  }

  private void DrawDoubleLineBetweenBorder(
    Border betweenBorder,
    PointF start,
    PointF end,
    bool isLeftBorderSame,
    bool isRightBorderSame,
    Border leftBorder,
    Border rightBorder,
    bool isOverlapLeft,
    bool isOverlapRight)
  {
    float[] borderLineWidthArray1 = this.GetBorderLineWidthArray(betweenBorder.BorderType, betweenBorder.LineWidth);
    float[] borderLineWidthArray2 = leftBorder == null || isLeftBorderSame ? (float[]) null : this.GetBorderLineWidthArray(leftBorder.BorderType, leftBorder.LineWidth);
    float[] borderLineWidthArray3 = rightBorder == null || isRightBorderSame ? (float[]) null : this.GetBorderLineWidthArray(rightBorder.BorderType, rightBorder.LineWidth);
    Pen pen1 = this.GetPen(betweenBorder.BorderType, borderLineWidthArray1[0], betweenBorder.Color);
    PointF pt1_1 = new PointF(start.X, start.Y + borderLineWidthArray1[0] / 2f);
    PointF pt2_1 = new PointF(end.X, end.Y + borderLineWidthArray1[0] / 2f);
    if (isLeftBorderSame)
      pt1_1 = new PointF(pt1_1.X + borderLineWidthArray1[0] + borderLineWidthArray1[1], pt1_1.Y);
    else if (borderLineWidthArray2 != null)
      pt1_1 = new PointF(pt1_1.X + this.GetLeftRightLineWidht(borderLineWidthArray2, true), pt1_1.Y);
    if (isRightBorderSame)
      pt2_1 = new PointF(pt2_1.X - (borderLineWidthArray1[1] + borderLineWidthArray1[2]), pt2_1.Y);
    else if (borderLineWidthArray3 != null)
      pt2_1 = new PointF(pt2_1.X - this.GetLeftRightLineWidht(borderLineWidthArray3, false), pt2_1.Y);
    this.Graphics.DrawLine(pen1, pt1_1, pt2_1);
    Pen pen2 = this.GetPen(betweenBorder.BorderType, borderLineWidthArray1[2], betweenBorder.Color);
    PointF pt1_2 = new PointF(start.X, (float) ((double) start.Y + (double) borderLineWidthArray1[0] + (double) borderLineWidthArray1[1] + (double) borderLineWidthArray1[2] / 2.0));
    PointF pt2_2 = new PointF(end.X, (float) ((double) end.Y + (double) borderLineWidthArray1[0] + (double) borderLineWidthArray1[1] + (double) borderLineWidthArray1[2] / 2.0));
    if (isLeftBorderSame)
      pt1_2 = new PointF(pt1_2.X + borderLineWidthArray1[0] + borderLineWidthArray1[1], pt1_2.Y);
    else if (borderLineWidthArray2 != null)
      pt1_2 = new PointF(pt1_2.X + this.GetLeftRightLineWidht(borderLineWidthArray2, true), pt1_2.Y);
    if (isRightBorderSame)
      pt2_2 = new PointF(pt2_2.X - (borderLineWidthArray1[1] + borderLineWidthArray1[2]), pt2_2.Y);
    else if (borderLineWidthArray3 != null)
      pt2_2 = new PointF(pt2_2.X - this.GetLeftRightLineWidht(borderLineWidthArray3, false), pt2_2.Y);
    this.Graphics.DrawLine(pen2, pt1_2, pt2_2);
    if (!isOverlapRight && !isOverlapLeft)
      return;
    if (isOverlapLeft)
    {
      PointF pt1_3 = new PointF(start.X + borderLineWidthArray1[0] / 2f, start.Y);
      PointF pt2_3 = new PointF(start.X + borderLineWidthArray1[0] / 2f, start.Y + borderLineWidthArray1[0] + borderLineWidthArray1[1] + borderLineWidthArray1[2]);
      this.Graphics.DrawLine(pen1, pt1_3, pt2_3);
    }
    if (!isOverlapRight)
      return;
    PointF pt1_4 = new PointF(end.X - borderLineWidthArray1[0] / 2f, end.Y);
    PointF pt2_4 = new PointF(end.X - borderLineWidthArray1[0] / 2f, end.Y + borderLineWidthArray1[0] + borderLineWidthArray1[1] + borderLineWidthArray1[2]);
    this.Graphics.DrawLine(pen1, pt1_4, pt2_4);
  }

  private void DrawTripleLineBetweenBorder(
    Border betweenBorder,
    PointF start,
    PointF end,
    bool isLeftBorderSame,
    bool isRightBorderSame,
    Border leftBorder,
    Border rightBorder,
    bool isOverlapLeft,
    bool isOverlapRight)
  {
    float[] borderLineWidthArray1 = this.GetBorderLineWidthArray(betweenBorder.BorderType, betweenBorder.LineWidth);
    float[] borderLineWidthArray2 = leftBorder == null || isLeftBorderSame ? (float[]) null : this.GetBorderLineWidthArray(leftBorder.BorderType, leftBorder.LineWidth);
    float[] borderLineWidthArray3 = rightBorder == null || isRightBorderSame ? (float[]) null : this.GetBorderLineWidthArray(rightBorder.BorderType, rightBorder.LineWidth);
    Pen pen1 = this.GetPen(betweenBorder.BorderType, borderLineWidthArray1[0], betweenBorder.Color);
    PointF pt1_1 = new PointF(start.X, start.Y + borderLineWidthArray1[0] / 2f);
    PointF pt2_1 = new PointF(end.X, end.Y + borderLineWidthArray1[0] / 2f);
    if (isLeftBorderSame)
      pt1_1 = new PointF(pt1_1.X + borderLineWidthArray1[0] + borderLineWidthArray1[1] + borderLineWidthArray1[2] + borderLineWidthArray1[3], pt1_1.Y);
    else if (borderLineWidthArray2 != null)
      pt1_1 = new PointF(pt1_1.X + this.GetLeftRightLineWidht(borderLineWidthArray2, true), pt1_1.Y);
    if (isRightBorderSame)
      pt2_1 = new PointF(pt2_1.X - (borderLineWidthArray1[1] + borderLineWidthArray1[2] + borderLineWidthArray1[3] + borderLineWidthArray1[4]), pt2_1.Y);
    else if (borderLineWidthArray3 != null)
      pt2_1 = new PointF(pt2_1.X - this.GetLeftRightLineWidht(borderLineWidthArray3, false), pt2_1.Y);
    this.Graphics.DrawLine(pen1, pt1_1, pt2_1);
    Pen pen2 = this.GetPen(betweenBorder.BorderType, borderLineWidthArray1[2], betweenBorder.Color);
    PointF pt1_2 = new PointF(start.X, (float) ((double) start.Y + (double) borderLineWidthArray1[0] + (double) borderLineWidthArray1[1] + (double) borderLineWidthArray1[2] / 2.0));
    PointF pt2_2 = new PointF(end.X, (float) ((double) end.Y + (double) borderLineWidthArray1[0] + (double) borderLineWidthArray1[1] + (double) borderLineWidthArray1[2] / 2.0));
    if (isLeftBorderSame)
      pt1_2 = new PointF((float) ((double) pt1_2.X + (double) borderLineWidthArray1[0] + (double) borderLineWidthArray1[1] + (isOverlapLeft ? 0.0 : (double) borderLineWidthArray1[2] + (double) borderLineWidthArray1[3])), pt1_2.Y);
    else if (borderLineWidthArray2 != null)
      pt1_2 = new PointF(pt1_2.X + this.GetLeftRightLineWidht(borderLineWidthArray2, true), pt1_2.Y);
    if (isRightBorderSame)
      pt2_2 = new PointF(pt2_2.X - ((isOverlapRight ? 0.0f : borderLineWidthArray1[1] + borderLineWidthArray1[2]) + borderLineWidthArray1[3] + borderLineWidthArray1[4]), pt2_2.Y);
    else if (borderLineWidthArray3 != null)
      pt2_2 = new PointF(pt2_2.X - this.GetLeftRightLineWidht(borderLineWidthArray3, false), pt2_2.Y);
    this.Graphics.DrawLine(pen2, pt1_2, pt2_2);
    Pen pen3 = this.GetPen(betweenBorder.BorderType, borderLineWidthArray1[4], betweenBorder.Color);
    PointF pt1_3 = new PointF(start.X, (float) ((double) start.Y + (double) borderLineWidthArray1[0] + (double) borderLineWidthArray1[1] + (double) borderLineWidthArray1[2] + (double) borderLineWidthArray1[3] + (double) borderLineWidthArray1[4] / 2.0));
    PointF pt2_3 = new PointF(end.X, (float) ((double) end.Y + (double) borderLineWidthArray1[0] + (double) borderLineWidthArray1[1] + (double) borderLineWidthArray1[2] + (double) borderLineWidthArray1[3] + (double) borderLineWidthArray1[4] / 2.0));
    if (isLeftBorderSame)
      pt1_3 = new PointF(pt1_3.X + borderLineWidthArray1[0] + borderLineWidthArray1[1] + borderLineWidthArray1[2] + borderLineWidthArray1[3], pt1_3.Y);
    else if (borderLineWidthArray2 != null)
      pt1_3 = new PointF(pt1_3.X + this.GetLeftRightLineWidht(borderLineWidthArray2, true), pt1_3.Y);
    if (isRightBorderSame)
      pt2_3 = new PointF(pt2_3.X - (borderLineWidthArray1[1] + borderLineWidthArray1[2] + borderLineWidthArray1[3] + borderLineWidthArray1[4]), pt2_3.Y);
    else if (borderLineWidthArray3 != null)
      pt2_3 = new PointF(pt2_3.X - this.GetLeftRightLineWidht(borderLineWidthArray3, false), pt2_3.Y);
    this.Graphics.DrawLine(pen3, pt1_3, pt2_3);
    if (!isOverlapRight && !isOverlapLeft)
      return;
    if (isOverlapLeft)
    {
      PointF pt1_4 = new PointF(start.X + borderLineWidthArray1[0] / 2f, start.Y);
      PointF pt2_4 = new PointF(pt1_4.X, start.Y + borderLineWidthArray1[0] + borderLineWidthArray1[1] + borderLineWidthArray1[2] + borderLineWidthArray1[3] + borderLineWidthArray1[4]);
      this.Graphics.DrawLine(pen1, pt1_4, pt2_4);
      pt1_4 = new PointF((float) ((double) start.X + (double) borderLineWidthArray1[0] + (double) borderLineWidthArray1[1] + (double) borderLineWidthArray1[2] / 2.0), start.Y);
      pt2_4 = new PointF(pt1_4.X, pt2_4.Y);
      this.Graphics.DrawLine(pen1, pt1_4, pt2_4);
    }
    if (!isOverlapRight)
      return;
    PointF pt1_5 = new PointF(end.X - borderLineWidthArray1[0] / 2f, end.Y);
    PointF pt2_5 = new PointF(pt1_5.X, end.Y + borderLineWidthArray1[0] + borderLineWidthArray1[1] + borderLineWidthArray1[2] + borderLineWidthArray1[3] + borderLineWidthArray1[4]);
    this.Graphics.DrawLine(pen1, pt1_5, pt2_5);
    pt1_5 = new PointF(end.X - (float) ((double) borderLineWidthArray1[0] + (double) borderLineWidthArray1[1] + (double) borderLineWidthArray1[2] / 2.0), end.Y);
    pt2_5 = new PointF(pt1_5.X, pt2_5.Y);
    this.Graphics.DrawLine(pen1, pt1_5, pt2_5);
  }

  private float GetLeftRightLineWidht(float[] lineArray, bool isLeft)
  {
    float num = 0.0f;
    if (lineArray.Length > 4)
      return num + (lineArray[1] + lineArray[2] + lineArray[3]) + (isLeft ? lineArray[0] : lineArray[4]);
    return lineArray.Length > 2 ? num + lineArray[1] + (isLeft ? lineArray[0] : lineArray[2]) : num;
  }

  internal void DrawBackgroundColor(Color bgColor, int width, int height)
  {
    using (SolidBrush solidBrush = this.CreateSolidBrush(bgColor))
    {
      this.Graphics.FillRectangle((Brush) solidBrush, new Rectangle(0, 0, width, height));
      solidBrush.Dispose();
    }
  }

  internal void DrawBackgroundImage(Image image, WPageSetup pageSetup)
  {
    this.Graphics.DrawImage(this.GetImage(image), 0.0f, 0.0f, pageSetup.PageSize.Width, pageSetup.PageSize.Height);
  }

  internal void DrawWatermark(Watermark watermark, WPageSetup pageSetup, RectangleF bounds)
  {
    switch (watermark.Type)
    {
      case WatermarkType.PictureWatermark:
        this.DrawImageWatermark(watermark as PictureWatermark, bounds, pageSetup);
        break;
      case WatermarkType.TextWatermark:
        this.DrawTextWatermark(watermark as TextWatermark, bounds, pageSetup);
        break;
    }
  }

  internal void Draw(Page page)
  {
    this.m_pageMarginLeft = page.Setup.Margins.Left;
    if (page.DocSection.Document.DOP.MirrorMargins && page.Number % 2 == 0)
      this.m_pageMarginLeft = page.Setup.Margins.Right;
    bool flag1 = false;
    float x = (float) ((double) page.DocSection.PageSetup.PageSize.Width - (double) page.DocSection.PageSetup.Margins.Right + 10.0);
    if (page.PageWidgets.Count != 0)
    {
      if (page.DocSection.Document.Background.Type == BackgroundType.Picture && page.BackgroundImage != null)
        this.DrawBackgroundImage(page.BackgroundImage, page.Setup);
      if (page.DocSection.Document.Background.Type == BackgroundType.Color)
        this.DrawBackgroundColor(page.BackgroundColor, (int) page.Setup.PageSize.Width, (int) page.Setup.PageSize.Height);
      if (page.PageWidgets[0].Widget is HeaderFooter)
      {
        Watermark watermark = (page.PageWidgets[0].Widget as HeaderFooter).Watermark;
        if ((page.PageWidgets[0].Widget as HeaderFooter).WriteWatermark && !this.IsEmptyWaterMark(watermark))
        {
          if (watermark != null && watermark.OrderIndex != int.MaxValue)
            flag1 = this.IsWaterMarkNeedToBeDraw(page);
          if (!flag1)
            this.DrawWatermark(watermark, page.Setup, new RectangleF(page.PageWidgets[2].Bounds.X, page.PageWidgets[2].Bounds.Y, page.Setup.ClientWidth, page.PageWidgets[1].Bounds.Y - page.PageWidgets[2].Bounds.Y));
        }
      }
      if (page.DocSection.Document.TrackChangesBalloonCount > 0)
        this.Graphics.FillRectangle((Brush) this.CreateSolidBrush(Color.FromArgb(240 /*0xF0*/, 240 /*0xF0*/, 240 /*0xF0*/)), new RectangleF(x, 0.0f, 250f, page.DocSection.PageSetup.PageSize.Height));
    }
    bool flag2 = true;
    bool flag3 = false;
    for (int index1 = 0; index1 < page.PageWidgets.Count; ++index1)
    {
      LayoutedWidget pageWidget = page.PageWidgets[index1];
      bool isHaveToInitLayoutInfo = !(pageWidget.Widget is HeaderFooter);
      if ((page.NumberOfBehindWidgetsInHeader > 0 || page.NumberOfBehindWidgetsInFooter > 0) && flag2)
      {
        flag2 = false;
        int length = page.NumberOfBehindWidgetsInHeader + page.NumberOfBehindWidgetsInFooter;
        this.DrawBehindWidgets(page.BehindWidgets, pageWidget.Widget, length, isHaveToInitLayoutInfo);
      }
      else if (!(pageWidget.Widget is HeaderFooter) && page.BehindWidgets.Count > 0)
        this.DrawBehindWidgets(page.BehindWidgets, pageWidget.Widget, page.BehindWidgets.Count, true);
      if (!(pageWidget.Widget is HeaderFooter) && !flag3)
      {
        for (int index2 = 0; index2 < page.FootnoteWidgets.Count; ++index2)
          this.Draw(page.FootnoteWidgets[index2], true);
        for (int index3 = 0; index3 < page.EndnoteWidgets.Count; ++index3)
          this.Draw(page.EndnoteWidgets[index3], true);
        flag3 = true;
      }
      this.Draw(pageWidget, isHaveToInitLayoutInfo);
      this.DrawOverLappedShapeWidgets(isHaveToInitLayoutInfo);
      if (index1 == 1 && flag1)
        this.DrawWatermark((page.PageWidgets[0].Widget as HeaderFooter).Watermark, page.Setup, new RectangleF(page.PageWidgets[2].Bounds.X, page.PageWidgets[2].Bounds.Y, page.Setup.ClientWidth, page.PageWidgets[1].Bounds.Y - page.PageWidgets[2].Bounds.Y));
      if (this.currParagraph != null)
        this.currParagraph = (WParagraph) null;
      if (!(pageWidget.Widget is HeaderFooter))
      {
        if (this.m_commentMarks != null)
          this.DrawCommentMarks(page.DocSection.Document.RevisionOptions);
        for (int index4 = 0; index4 < page.TrackChangesMarkups.Count; ++index4)
        {
          RevisionOptions revisionOptions = page.DocSection.Document.RevisionOptions;
          TrackChangesMarkups trackChangesMarkup = page.TrackChangesMarkups[index4];
          RectangleF rect = new RectangleF(x + 20f, trackChangesMarkup.LtWidget.Bounds.Y, 210f, trackChangesMarkup.LtWidget.Bounds.Height);
          float num = trackChangesMarkup is CommentsMarkups ? (trackChangesMarkup as CommentsMarkups).ExtraSpacing : 0.0f;
          PointF pt1 = new PointF(trackChangesMarkup.Position.X, trackChangesMarkup.Position.Y - num);
          PointF pointF = new PointF(x - 5f, trackChangesMarkup.Position.Y - num);
          PointF pt2 = new PointF(x + 20f, trackChangesMarkup.LtWidget.Bounds.Y + 10f);
          Color color = Color.Red;
          bool isResolvedComment = false;
          if (trackChangesMarkup is CommentsMarkups)
          {
            isResolvedComment = (trackChangesMarkup as CommentsMarkups).Comment.IsResolved;
            color = !isResolvedComment ? this.GetRevisionColor(revisionOptions.CommentColor) : this.GetRevisionColor(revisionOptions.CommentColor, false, isResolvedComment);
          }
          else if (trackChangesMarkup.TypeOfMarkup == RevisionType.Deletions)
            color = this.GetRevisionColor(revisionOptions.DeletedTextColor);
          else if (trackChangesMarkup.TypeOfMarkup == RevisionType.Formatting)
            color = this.GetRevisionColor(revisionOptions.RevisedPropertiesColor);
          Pen pen1 = this.CreatePen(color, 1f);
          Pen pen2 = this.CreatePen(color, 0.5f);
          pen2.DashStyle = DashStyle.Dot;
          this.Graphics.DrawLine(pen2, pt1, pointF);
          this.Graphics.DrawLine(pen2, pointF, pt2);
          this.DrawRoundedRectangle(pen1, rect, trackChangesMarkup is CommentsMarkups ? this.GetRevisionFillColor(revisionOptions.CommentColor, isResolvedComment) : Color.White);
          if (!(trackChangesMarkup is CommentsMarkups))
            this.DrawMarkupTriangles(trackChangesMarkup.Position, color);
          List<KeyValuePair<string, bool>> keyValuePairList = (List<KeyValuePair<string, bool>>) null;
          if (this.m_previousLineCommentStartMarks != null && this.m_previousLineCommentStartMarks.Count > 0)
          {
            keyValuePairList = this.m_previousLineCommentStartMarks;
            this.m_previousLineCommentStartMarks = (List<KeyValuePair<string, bool>>) null;
          }
          this.Draw(trackChangesMarkup.LtWidget, true);
          if (keyValuePairList != null)
            this.m_previousLineCommentStartMarks = keyValuePairList;
        }
      }
    }
  }

  private void DrawRoundedRectangle(Pen pen, RectangleF rect, Color fillColor)
  {
    float num = 6f;
    GraphicsPath graphicsPath = this.CreateGraphicsPath();
    graphicsPath.AddArc(rect.X, rect.Y, num, num, 180f, 90f);
    graphicsPath.AddArc(rect.Right - num, rect.Y, num, num, 270f, 90f);
    graphicsPath.AddArc(rect.Right - num, rect.Bottom - num, num, num, 0.0f, 90f);
    graphicsPath.AddArc(rect.X, rect.Bottom - num, num, num, 90f, 90f);
    graphicsPath.CloseFigure();
    this.Graphics.DrawPath(pen, graphicsPath);
    this.Graphics.FillPath((Brush) this.CreateSolidBrush(fillColor), graphicsPath);
  }

  private void DrawMarkupTriangles(PointF position, Color revisionColor)
  {
    Brush solidBrush = (Brush) this.CreateSolidBrush(revisionColor);
    GraphicsPath graphicsPath = this.CreateGraphicsPath();
    PointF pointF1 = new PointF(position.X - 2f, position.Y);
    PointF pointF2 = new PointF(position.X + 2f, position.Y);
    position.Y -= 3f;
    graphicsPath.StartFigure();
    graphicsPath.AddLine(pointF1, pointF2);
    graphicsPath.AddLine(pointF2, position);
    graphicsPath.AddLine(position, pointF1);
    graphicsPath.CloseFigure();
    this.Graphics.FillPath(solidBrush, graphicsPath);
  }

  private bool IsEmptyWaterMark(Watermark waterMark)
  {
    if (waterMark is TextWatermark)
    {
      if ((waterMark as TextWatermark).Text.Trim(ControlChar.SpaceChar) == string.Empty)
        return true;
    }
    return waterMark is PictureWatermark && (waterMark as PictureWatermark).Picture == null;
  }

  private bool IsWaterMarkNeedToBeDraw(Page page)
  {
    bool beDraw = false;
    if (page.PageWidgets[0].Widget is HeaderFooter)
    {
      for (int index = 0; index < (page.PageWidgets[0].Widget as HeaderFooter).ChildEntities.Count; ++index)
      {
        if ((page.PageWidgets[0].Widget as HeaderFooter).ChildEntities[index] is WParagraph)
          beDraw = this.IsWaterMarkInParagraph((page.PageWidgets[0].Widget as HeaderFooter).ChildEntities[index] as WParagraph, page);
        else if ((page.PageWidgets[0].Widget as HeaderFooter).ChildEntities[index] is WTable)
          beDraw = this.IsWaterMarkInTable((page.PageWidgets[0].Widget as HeaderFooter).ChildEntities[index] as WTable, page);
        if (beDraw)
          return true;
      }
    }
    return beDraw;
  }

  private bool IsWaterMarkInParagraph(WParagraph paragraph, Page page)
  {
    bool flag = false;
    for (int index = 0; index < paragraph.ChildEntities.Count; ++index)
    {
      if (paragraph.ChildEntities[index] is WPicture)
      {
        if ((paragraph.ChildEntities[index] as WPicture).TextWrappingStyle != TextWrappingStyle.Inline)
        {
          WPicture childEntity = paragraph.ChildEntities[index] as WPicture;
          flag = this.IsWaterMarkOrderHasChanged(childEntity.OrderIndex, childEntity.IsBelowText, page);
        }
      }
      else if (paragraph.ChildEntities[index] is Shape && (paragraph.ChildEntities[index] as Shape).WrapFormat.TextWrappingStyle != TextWrappingStyle.Inline)
      {
        Shape childEntity = paragraph.ChildEntities[index] as Shape;
        flag = this.IsWaterMarkOrderHasChanged(childEntity.ZOrderPosition, childEntity.IsBelowText, page);
      }
      if (flag)
        return true;
    }
    return flag;
  }

  private bool IsWaterMarkInTable(WTable table, Page page)
  {
    bool flag = false;
    for (int index1 = 0; index1 < table.Rows.Count; ++index1)
    {
      for (int index2 = 0; index2 < table.Rows[index1].Cells.Count; ++index2)
      {
        WTableCell cell = table.Rows[index1].Cells[index2];
        for (int index3 = 0; index3 < cell.ChildEntities.Count; ++index3)
        {
          if (cell.ChildEntities[index3] is WParagraph)
            flag = this.IsWaterMarkInParagraph(cell.ChildEntities[index3] as WParagraph, page);
          else if (cell.ChildEntities[index3] is WTable)
            flag = this.IsWaterMarkInTable(cell.ChildEntities[index3] as WTable, page);
          if (flag)
            return true;
        }
      }
    }
    return flag;
  }

  private bool IsWaterMarkOrderHasChanged(int OrderIndex, bool IsBelowText, Page page)
  {
    if (page.DocSection.Document.Settings.CompatibilityMode.ToString() == "Word2013")
    {
      if (OrderIndex < (this.GetCurrentHeader(page.DocSection) as HeaderFooter).Watermark.OrderIndex)
        return true;
    }
    else if (OrderIndex < (this.GetCurrentHeader(page.DocSection) as HeaderFooter).Watermark.OrderIndex || IsBelowText)
      return true;
    return false;
  }

  internal void Draw(SplitWidgetContainer widget, LayoutedWidget layoutedWidget)
  {
    if (layoutedWidget.ChildWidgets.Count > 0)
    {
      LayoutedWidget childWidget1 = layoutedWidget.ChildWidgets[0];
      if (childWidget1.Widget is SplitWidgetContainer && childWidget1.ChildWidgets.Count > 0)
      {
        WParagraph realWidgetContainer = (childWidget1.Widget as SplitWidgetContainer).RealWidgetContainer as WParagraph;
        LayoutedWidget childWidget2 = childWidget1.ChildWidgets[childWidget1.ChildWidgets.Count - 1];
        int count = childWidget2.ChildWidgets.Count;
        if (count > 0 && childWidget2.ChildWidgets[0].HorizontalAlign != HAlignment.Distributed && (childWidget2.ChildWidgets[count - 1].Widget is SplitStringWidget || childWidget2.ChildWidgets[count - 1].Widget is WTextRange))
        {
          for (int index = 0; index < childWidget2.ChildWidgets.Count; ++index)
            childWidget2.ChildWidgets[index].IsLastLine = true;
          this.AlignChildWidgets(childWidget2, realWidgetContainer);
        }
      }
    }
    this.Draw((IWidget) widget.RealWidgetContainer, layoutedWidget);
  }

  private LayoutedWidgetList SortLayoutWidgetsWithXPosition(LayoutedWidgetList childWidgets)
  {
    LayoutedWidgetList layoutedWidgetList = new LayoutedWidgetList();
label_10:
    for (int index1 = 0; index1 <= childWidgets.Count - 1; ++index1)
    {
      LayoutedWidget childWidget = childWidgets[index1];
      int index2 = 0;
      while (layoutedWidgetList.Count != 0)
      {
        if (index2 == layoutedWidgetList.Count)
        {
          layoutedWidgetList.Add(childWidget);
          goto label_10;
        }
        if ((double) childWidget.Bounds.X < (double) layoutedWidgetList[index2].Bounds.X)
        {
          layoutedWidgetList.Insert(index2, childWidget);
          goto label_10;
        }
        double x1 = (double) childWidget.Bounds.X;
        double x2 = (double) layoutedWidgetList[index2].Bounds.X;
        ++index2;
      }
      layoutedWidgetList.Insert(0, childWidget);
    }
    return layoutedWidgetList;
  }

  private void AlignChildWidgets(LayoutedWidget LastLine, WParagraph paragraph)
  {
    LayoutedWidgetList layoutedWidgetList = this.SortLayoutWidgetsWithXPosition(LastLine.ChildWidgets);
    bool flag = false;
    if (paragraph != null)
      flag = paragraph.ParagraphFormat.Bidi;
    int index1 = 0;
    if (flag)
      index1 = layoutedWidgetList.Count - 1;
    if (paragraph != null && paragraph.IsLineNumbersEnabled() && !flag)
      index1 = 1;
    if (flag)
    {
      layoutedWidgetList[index1].Bounds = new RectangleF(layoutedWidgetList[index1].Bounds.X + Convert.ToSingle((float) layoutedWidgetList[index1].Spaces * layoutedWidgetList[index1].WordSpace), layoutedWidgetList[index1].Bounds.Y, layoutedWidgetList[index1].Bounds.Width - Convert.ToSingle((float) layoutedWidgetList[index1].Spaces * layoutedWidgetList[index1].WordSpace), layoutedWidgetList[index1].Bounds.Height);
      float x = layoutedWidgetList[index1].Bounds.X;
      for (int index2 = index1 - 1; index2 >= 0; --index2)
      {
        LayoutedWidget layoutedWidget1 = layoutedWidgetList[index2 + 1];
        LayoutedWidget layoutedWidget2 = layoutedWidgetList[index2];
        if (!(layoutedWidget1.Widget is WTextBox) && !(layoutedWidget1.Widget is WPicture) && !(layoutedWidget1.Widget is WChart) && !(layoutedWidget1.Widget is Shape) && !(layoutedWidget1.Widget is GroupShape) && !(layoutedWidget2.Widget is WTextBox) && !(layoutedWidget2.Widget is WPicture) && !(layoutedWidget2.Widget is WChart) && !(layoutedWidget2.Widget is Shape) && !(layoutedWidget2.Widget is GroupShape))
        {
          float width = layoutedWidget2.Bounds.Width - ((double) layoutedWidget2.SubWidth != 0.0 ? Convert.ToSingle((float) layoutedWidget2.Spaces * layoutedWidget2.WordSpace) : 0.0f);
          x -= width;
          layoutedWidget2.Bounds = new RectangleF(x, layoutedWidget2.Bounds.Y, width, layoutedWidget2.Bounds.Height);
        }
      }
    }
    else
    {
      layoutedWidgetList[index1].Bounds = new RectangleF(layoutedWidgetList[index1].Bounds.X, layoutedWidgetList[index1].Bounds.Y, layoutedWidgetList[index1].Bounds.Width - Convert.ToSingle((float) layoutedWidgetList[index1].Spaces * layoutedWidgetList[index1].WordSpace), layoutedWidgetList[index1].Bounds.Height);
      for (int index3 = index1 + 1; index3 < layoutedWidgetList.Count; ++index3)
      {
        LayoutedWidget layoutedWidget3 = layoutedWidgetList[index3 - 1];
        LayoutedWidget layoutedWidget4 = layoutedWidgetList[index3];
        if (!(layoutedWidget3.Widget is WTextBox) && !(layoutedWidget3.Widget is WPicture) && !(layoutedWidget3.Widget is WChart) && !(layoutedWidget3.Widget is Shape) && !(layoutedWidget3.Widget is GroupShape) && !(layoutedWidget4.Widget is WTextBox) && !(layoutedWidget4.Widget is WPicture) && !(layoutedWidget4.Widget is WChart) && !(layoutedWidget4.Widget is Shape) && !(layoutedWidget4.Widget is GroupShape))
        {
          float width = layoutedWidget4.Bounds.Width - ((double) layoutedWidget4.SubWidth != 0.0 ? Convert.ToSingle((float) layoutedWidget4.Spaces * layoutedWidget4.WordSpace) : 0.0f);
          layoutedWidget4.Bounds = new RectangleF(layoutedWidget3.Bounds.X + layoutedWidget3.Bounds.Width, layoutedWidget4.Bounds.Y, width, layoutedWidget4.Bounds.Height);
        }
      }
    }
    layoutedWidgetList.Clear();
  }

  internal void Draw(IWidgetContainer widget, LayoutedWidget ltWidget)
  {
    this.DrawImpl(widget, ltWidget);
  }

  internal virtual void DrawImpl(IWidgetContainer widget, LayoutedWidget ltWidget)
  {
  }

  internal void Draw(IWidget widget, LayoutedWidget layoutedWidget)
  {
    switch (widget)
    {
      case Entity _:
        switch ((widget as Entity).EntityType)
        {
          case EntityType.Paragraph:
            this.Draw(widget as WParagraph, layoutedWidget);
            return;
          case EntityType.BlockContentControl:
            this.Draw(widget as BlockContentControl, layoutedWidget);
            return;
          case EntityType.InlineContentControl:
            this.Draw(widget as InlineContentControl, layoutedWidget);
            return;
          case EntityType.Table:
            this.Draw(widget as WTable, layoutedWidget);
            return;
          case EntityType.TableRow:
            this.Draw(widget as WTableRow, layoutedWidget);
            return;
          case EntityType.TableCell:
            this.Draw(widget as WTableCell, layoutedWidget);
            return;
          case EntityType.TextRange:
            this.Draw(widget as WTextRange, layoutedWidget);
            return;
          case EntityType.Picture:
            this.Draw(widget as WPicture, layoutedWidget);
            return;
          case EntityType.Field:
            this.Draw(widget as WField, layoutedWidget);
            return;
          case EntityType.DropDownFormField:
            this.Draw(widget as WDropDownFormField, layoutedWidget);
            return;
          case EntityType.CheckBox:
            this.Draw(widget as WCheckBox, layoutedWidget);
            return;
          case EntityType.BookmarkStart:
            this.Draw(widget as BookmarkStart, layoutedWidget);
            return;
          case EntityType.Footnote:
            this.DrawImpl(widget as WFootnote, layoutedWidget);
            return;
          case EntityType.TextBox:
            this.Draw(widget as WTextBox, layoutedWidget);
            return;
          case EntityType.Symbol:
            this.Draw(widget as WSymbol, layoutedWidget);
            return;
          case EntityType.Chart:
            this.Draw(widget as WChart, layoutedWidget);
            return;
          case EntityType.CommentMark:
            this.Draw(widget as WCommentMark, layoutedWidget);
            return;
          case EntityType.OleObject:
            this.Draw(widget as WOleObject, layoutedWidget);
            return;
          case EntityType.AbsoluteTab:
            this.Draw(widget as WAbsoluteTab, layoutedWidget);
            return;
          case EntityType.AutoShape:
            this.Draw(widget as Shape, layoutedWidget);
            return;
          case EntityType.ChildShape:
            this.Draw(widget as ChildShape, layoutedWidget);
            return;
          case EntityType.Math:
            MathRenderer mathRenderer = new MathRenderer(this);
            mathRenderer.Draw(widget as WMath, layoutedWidget);
            mathRenderer.Dispose();
            return;
          default:
            this.DrawImpl(layoutedWidget);
            return;
        }
      case SplitWidgetContainer _:
        this.Draw(widget as SplitWidgetContainer, layoutedWidget);
        break;
      case SplitStringWidget _:
        this.Draw(widget as SplitStringWidget, layoutedWidget);
        break;
      case SplitTableWidget _:
        this.Draw(widget as SplitTableWidget, layoutedWidget);
        break;
      case LeafEmtyWidget _:
        this.Draw(widget as LeafEmtyWidget, layoutedWidget);
        break;
      default:
        this.DrawImpl(layoutedWidget);
        break;
    }
  }

  private bool IsNeedToSkip(IWidget widget)
  {
    bool skip = false;
    if (widget is Entity)
    {
      switch ((widget as Entity).EntityType)
      {
        case EntityType.TextBox:
          if (!(widget as WTextBox).Visible && (double) (widget as WTextBox).TextBoxFormat.Rotation != 0.0)
          {
            skip = true;
            break;
          }
          break;
        case EntityType.AutoShape:
          if (!(widget as Shape).Visible && (double) (widget as Shape).Rotation != 0.0)
          {
            skip = true;
            break;
          }
          break;
        case EntityType.GroupShape:
          if (!(widget as GroupShape).Visible)
          {
            skip = true;
            break;
          }
          break;
        case EntityType.ChildShape:
          if (!(widget as ChildShape).Visible)
          {
            skip = true;
            break;
          }
          break;
        case EntityType.ChildGroupShape:
          if (!(widget as ChildGroupShape).Visible)
          {
            skip = true;
            break;
          }
          break;
      }
    }
    return skip;
  }

  internal void DrawPageBorder(int pageNumber, PageCollection pageCollection)
  {
    bool flag = false;
    Page page = pageCollection[pageNumber];
    switch (page.Setup.PageBordersApplyType)
    {
      case PageBordersApplyType.AllPages:
        flag = true;
        break;
      case PageBordersApplyType.FirstPage:
        if (pageNumber == 0 || pageNumber > 0 && page.Setup.OwnerBase != pageCollection[pageNumber - 1].Setup.OwnerBase)
        {
          flag = true;
          break;
        }
        break;
      case PageBordersApplyType.AllExceptFirstPage:
        if (pageNumber > 0 && page.Setup.OwnerBase == pageCollection[pageNumber - 1].Setup.OwnerBase)
        {
          flag = true;
          break;
        }
        break;
    }
    if (!flag || page.PageWidgets.Count == 0)
      return;
    this.DrawPageBorder(page.Setup, page.PageWidgets[0].Bounds, page.PageWidgets[1].Bounds, page.PageWidgets[2].Bounds);
  }

  internal void Draw(BookmarkStart bookmarkStart, LayoutedWidget ltWidget)
  {
    if (bookmarkStart.Name.StartsWithExt("_") || (this.ExportBookmarks & ExportBookmarkType.Bookmarks) == ExportBookmarkType.None)
      return;
    DocumentLayouter.Bookmarks.Add(new BookmarkPosition(bookmarkStart.Name, DocumentLayouter.PageNumber, ltWidget.Bounds));
  }

  private bool IsNeedToClip(RectangleF itemBounds)
  {
    if (this.ClipBoundsContainer == null || this.ClipBoundsContainer.Count == 0)
      return false;
    RectangleF rectangleF = this.ClipBoundsContainer.Peek();
    return Math.Round((double) itemBounds.X, 2) < Math.Round((double) rectangleF.X, 2) || (double) itemBounds.Y < (double) rectangleF.Y || (double) itemBounds.Right > (double) rectangleF.Right || Math.Round((double) itemBounds.Bottom) >= Math.Round((double) rectangleF.Bottom);
  }

  private Color GetHightLightColor(Color hightLightColor)
  {
    if (hightLightColor == Color.Green)
      return Color.FromArgb((int) byte.MaxValue, 0, (int) byte.MaxValue, 0);
    if (hightLightColor == Color.Gold)
      return Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 128 /*0x80*/, 0);
    return hightLightColor.Name == "808080" ? Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/) : hightLightColor;
  }

  internal bool IsTextRange(WParagraph paragraph)
  {
    for (int index = 0; index < paragraph.ChildEntities.Count; ++index)
    {
      Entity childEntity = paragraph.ChildEntities[index];
      if (this.formFieldEnd != null)
      {
        if (paragraph.ChildEntities.Contains((IEntity) this.formFieldEnd))
        {
          index = this.formFieldEnd.Index;
          this.formFieldEnd = (WFieldMark) null;
        }
        else
          break;
      }
      else if (childEntity.EntityType == EntityType.TextFormField || childEntity.EntityType == EntityType.CheckBox || childEntity.EntityType == EntityType.DropDownFormField)
      {
        WField wfield = childEntity as WField;
        if (wfield.FieldEnd != null && this.formFieldEnd == null)
        {
          if (wfield.FieldEnd.GetOwnerParagraphValue() == paragraph)
          {
            index = wfield.FieldEnd.Index;
          }
          else
          {
            this.formFieldEnd = wfield.FieldEnd;
            break;
          }
        }
      }
      else if (childEntity is WTextRange wtextRange && !(wtextRange.PreviousSibling is WOleObject) && !(wtextRange.m_layoutInfo is TabsLayoutInfo) && !(wtextRange.Text == "") && !Syncfusion.Layouting.StringParser.IsWhitespace(wtextRange.Text))
        return true;
    }
    return false;
  }

  internal void Draw(LayoutedWidget layoutedWidget, bool isHaveToInitLayoutInfo)
  {
    if (this.IsNeedToSkip(layoutedWidget.Widget))
      return;
    this.Draw(layoutedWidget.Widget, layoutedWidget);
    Entity ent = layoutedWidget.Widget is ParagraphItem ? layoutedWidget.Widget as Entity : (layoutedWidget.Widget is SplitStringWidget ? (layoutedWidget.Widget as SplitStringWidget).RealStringWidget as Entity : (Entity) null);
    if (ent != null)
    {
      this.UpdateBookmarkTargetPosition(ent, layoutedWidget);
      this.CreateBookmarkRerefeceLink(ent, layoutedWidget);
    }
    int index1 = 0;
    for (int count = layoutedWidget.ChildWidgets.Count; index1 < count; ++index1)
    {
      LayoutedWidget childWidget = layoutedWidget.ChildWidgets[index1];
      Entity ownerLayoutedWidget = this.GetOwnerLayoutedWidget(childWidget);
      WParagraph paragraphWidget = this.GetParagraphWidget(childWidget);
      WTableCell cellWidget = this.GetCellWidget(childWidget);
      if (this.IsOverLappedShapeWidget(childWidget))
      {
        int orderIndex = this.GetOrderIndex(childWidget.Widget);
        if (!this.OverLappedShapeWidgets.ContainsKey(orderIndex))
        {
          this.OverLappedShapeWidgets.Add(orderIndex, childWidget);
          if (this.AutoTag)
            this.AutoTagIndex.Add(orderIndex, ++this.autoTagCount);
        }
        layoutedWidget.ChildWidgets.RemoveAt(index1);
        --index1;
        --count;
      }
      else if (childWidget.IsBehindWidget())
      {
        layoutedWidget.ChildWidgets.RemoveAt(index1);
        if (isHaveToInitLayoutInfo)
          childWidget.InitLayoutInfoAll();
        --index1;
        --count;
      }
      else if (ownerLayoutedWidget is WTableRow)
        this.Draw(childWidget, isHaveToInitLayoutInfo);
      else if (ownerLayoutedWidget is WTable && (ownerLayoutedWidget as WTable).TableFormat.Bidi && childWidget.Widget is WTableRow)
      {
        childWidget.ChildWidgets.Reverse();
        this.Draw(childWidget, isHaveToInitLayoutInfo);
      }
      else if (cellWidget != null && ownerLayoutedWidget is WTableCell)
      {
        Syncfusion.Layouting.Spacings margins = (childWidget.Widget.LayoutInfo as CellLayoutInfo).Margins;
        RectangleF clipBounds = !cellWidget.m_layoutInfo.IsVerticalText ? new RectangleF(childWidget.Bounds.X - margins.Left, childWidget.Bounds.Y, childWidget.Bounds.Width + margins.Left + margins.Right, childWidget.Bounds.Height) : new RectangleF(childWidget.Bounds.X - margins.Bottom, childWidget.Bounds.Y, childWidget.Bounds.Width + margins.Top + margins.Bottom, childWidget.Bounds.Height);
        bool isVerticalText = (ownerLayoutedWidget as IWidget).LayoutInfo.IsVerticalText;
        clipBounds = this.UpdateClipBounds(clipBounds, isVerticalText);
        if ((double) clipBounds.Width != 0.0 && (double) clipBounds.Height != 0.0)
        {
          this.ClipBoundsContainer.Push(clipBounds);
          this.Draw(childWidget, isHaveToInitLayoutInfo);
          this.ClipBoundsContainer.Pop();
        }
      }
      else if (childWidget != null && paragraphWidget != null && !paragraphWidget.IsInCell && paragraphWidget.ParagraphFormat.IsFrame && !(ownerLayoutedWidget is WParagraph))
      {
        RectangleF bounds = childWidget.Bounds;
        WParagraphFormat paragraphFormat = paragraphWidget.ParagraphFormat;
        ParagraphLayoutInfo layoutInfo = childWidget.Widget.LayoutInfo as ParagraphLayoutInfo;
        if (!bounds.IsEmpty && ((double) paragraphFormat.FrameHeight != 0.0 || (double) paragraphFormat.FrameWidth != 0.0) && layoutInfo != null)
        {
          RectangleF rectangleF = layoutedWidget.GetFrameClipBounds(bounds, paragraphFormat, layoutInfo);
          rectangleF = this.UpdateClipBounds(rectangleF, false);
          rectangleF = this.GetClipBounds(rectangleF, rectangleF.Width, 0.0f);
          if ((double) rectangleF.Width != 0.0)
          {
            this.ClipBoundsContainer.Push(rectangleF);
            this.Draw(childWidget, isHaveToInitLayoutInfo);
            this.ClipBoundsContainer.Pop();
          }
        }
        else
          this.Draw(childWidget, isHaveToInitLayoutInfo);
      }
      else if (childWidget != null)
      {
        switch (ownerLayoutedWidget)
        {
          case WTextBox _:
          case Shape _:
          case ChildShape _:
            float num = 0.0f;
            bool flag1 = false;
            bool flag2 = false;
            switch (ownerLayoutedWidget)
            {
              case WTextBox _:
                WTextBox wtextBox = ownerLayoutedWidget as WTextBox;
                num = wtextBox.Shape != null ? wtextBox.Shape.Rotation : wtextBox.TextBoxFormat.Rotation;
                flag1 = wtextBox.TextBoxFormat.FlipHorizontal;
                flag2 = wtextBox.TextBoxFormat.FlipVertical;
                break;
              case Shape _:
                Shape shape = ownerLayoutedWidget as Shape;
                num = shape.Rotation;
                flag1 = shape.FlipHorizontal;
                flag2 = shape.FlipVertical;
                break;
              case ChildShape _:
                ChildShape childShape = ownerLayoutedWidget as ChildShape;
                num = childShape.RotationToRender;
                flag1 = childShape.FlipHorizantalToRender;
                flag2 = childShape.FlipVerticalToRender;
                break;
            }
            if ((double) num != 0.0 || (double) num == 0.0 && (flag1 || flag2))
            {
              this.m_rotateTransform = new RectangleF(layoutedWidget.Bounds.X, layoutedWidget.Bounds.Y, layoutedWidget.Bounds.Width, layoutedWidget.Bounds.Height);
              this.Draw(childWidget, isHaveToInitLayoutInfo);
              this.m_rotateTransform = new RectangleF();
              break;
            }
            this.Draw(childWidget, isHaveToInitLayoutInfo);
            break;
          default:
            this.Draw(childWidget, isHaveToInitLayoutInfo);
            break;
        }
        if (childWidget.Widget is Shape || childWidget.Widget is WTextBox || childWidget.Widget is GroupShape || childWidget.Widget is ChildShape)
          this.currParagraph = (childWidget.Widget as ParagraphItem).OwnerParagraph;
        if (this.IsLineItemDrawn(childWidget) && (this.underLineValues != null || this.strikeThroughValues != null))
          this.DrawLine(this.underLineValues, this.strikeThroughValues);
      }
      else
        Trace.WriteLine("object is null", "LayoutedWidget.Draw()");
    }
    if (layoutedWidget.Widget is WTable && layoutedWidget.ChildWidgets.Count > 0)
    {
      for (int index2 = 0; index2 < layoutedWidget.ChildWidgets.Count; ++index2)
      {
        LayoutedWidget childWidget1 = layoutedWidget.ChildWidgets[index2];
        if ((layoutedWidget.Widget as WTable).TableFormat.Bidi)
          childWidget1.ChildWidgets.Reverse();
        for (int index3 = 0; index3 < childWidget1.ChildWidgets.Count; ++index3)
        {
          LayoutedWidget childWidget2 = childWidget1.ChildWidgets[index3];
          WTableCell cellWidget = this.GetCellWidget(childWidget2);
          WTableRow ownerLayoutedWidget = this.GetOwnerLayoutedWidget(childWidget2) as WTableRow;
          bool flag3 = false;
          bool flag4 = false;
          if (cellWidget != null)
          {
            Entity owner = cellWidget.OwnerRow.OwnerTable.Owner;
            if (owner is WTextBody)
              owner = owner.Owner;
            if (owner is WTextBox && (double) (owner as WTextBox).TextBoxFormat.Rotation != 0.0)
            {
              flag3 = true;
              WTextBoxFormat textBoxFormat = (owner as WTextBox).TextBoxFormat;
              this.SetRotateTransform(this.m_rotateTransform, textBoxFormat.Rotation, textBoxFormat.FlipVertical, textBoxFormat.FlipHorizontal);
            }
            else if (owner is Shape && (double) (owner as Shape).Rotation != 0.0)
            {
              Shape shape = owner as Shape;
              flag3 = true;
              this.SetRotateTransform(this.m_rotateTransform, shape.Rotation, shape.FlipVertical, shape.FlipHorizontal);
            }
            else if (owner is ChildShape && (double) (owner as ChildShape).Rotation != 0.0)
            {
              ChildShape shapeFrame = owner as ChildShape;
              RectangleF textLayoutingBounds = shapeFrame.TextLayoutingBounds;
              if (shapeFrame.AutoShapeType == AutoShapeType.Rectangle)
                textLayoutingBounds.Height = this.GetLayoutedTextBoxContentHeight(childWidget2);
              flag3 = true;
              this.Rotate((ParagraphItem) shapeFrame, shapeFrame.Rotation, shapeFrame.FlipVertical, shapeFrame.FlipHorizantal, textLayoutingBounds);
            }
            if (this.ClipBoundsContainer != null && this.ClipBoundsContainer.Count > 0)
            {
              flag4 = true;
              RectangleF rectangleF = this.ClipBoundsContainer.Peek();
              if (ownerLayoutedWidget.Document.Settings.CompatibilityMode != CompatibilityMode.Word2013 && this.IsTableInTextBoxOrShape(childWidget2.Widget as Entity, false))
                rectangleF = this.TextboxClipBounds(ownerLayoutedWidget.Owner as WTable, rectangleF);
              this.SetClip(rectangleF);
            }
            this.DrawCellBorders(cellWidget, childWidget2);
          }
          if (flag3)
            this.ResetTransform();
          if (flag4)
            this.ResetClip();
        }
      }
    }
    if (!this.IsNeedToRemoveClipBounds(layoutedWidget.Widget))
      return;
    if (layoutedWidget.Widget is WTable)
    {
      WParagraphFormat paragraphFormat = (layoutedWidget.Widget as WTable).Rows[0].Cells[0].Paragraphs[0].ParagraphFormat;
      if ((double) paragraphFormat.FrameWidth == 0.0 && (double) paragraphFormat.FrameHeight == 0.0)
        return;
      this.ClipBoundsContainer.Pop();
    }
    else
      this.ClipBoundsContainer.Pop();
  }

  private bool IsLineItemDrawn(LayoutedWidget ltWidget)
  {
    return ltWidget.ChildWidgets.Count > 0 && (ltWidget.Widget is WParagraph && (ltWidget.ChildWidgets[0].Widget is ParagraphItem || ltWidget.ChildWidgets[0].Widget is SplitStringWidget) || ltWidget.Widget is SplitWidgetContainer && (ltWidget.Widget as SplitWidgetContainer).RealWidgetContainer is WParagraph && (ltWidget.ChildWidgets[0].Widget is ParagraphItem || ltWidget.ChildWidgets[0].Widget is SplitStringWidget));
  }

  private void DrawLine(
    Dictionary<WCharacterFormat, List<RectangleF>> underLineValues,
    Dictionary<WCharacterFormat, RectangleF> strikeThroughValues)
  {
    if (underLineValues != null)
    {
      foreach (KeyValuePair<WCharacterFormat, List<RectangleF>> underLineValue in underLineValues)
      {
        List<RectangleF> rectangleFList = underLineValue.Value;
        WCharacterFormat key = underLineValue.Key;
        bool isNeedToScale = false;
        this.ScaleTransform(key, ref isNeedToScale);
        foreach (RectangleF textBounds in rectangleFList)
          this.DrawUnderLine(key, textBounds);
        if (isNeedToScale)
          this.ResetTransform();
      }
      underLineValues.Clear();
      underLineValues = (Dictionary<WCharacterFormat, List<RectangleF>>) null;
    }
    if (strikeThroughValues == null)
      return;
    foreach (KeyValuePair<WCharacterFormat, RectangleF> strikeThroughValue in strikeThroughValues)
    {
      RectangleF textBounds = strikeThroughValue.Value;
      WCharacterFormat key = strikeThroughValue.Key;
      bool isNeedToScale = false;
      this.ScaleTransform(key, ref isNeedToScale);
      this.DrawStrikeThrough(key, textBounds);
      if (isNeedToScale)
        this.ResetTransform();
    }
    strikeThroughValues.Clear();
    strikeThroughValues = (Dictionary<WCharacterFormat, RectangleF>) null;
  }

  private void ScaleTransform(WCharacterFormat characterFormat, ref bool isNeedToScale)
  {
    float scaling = characterFormat.Scaling;
    isNeedToScale = (double) scaling != 100.0 && ((double) scaling >= 1.0 || (double) scaling <= 600.0);
    if (!isNeedToScale)
      return;
    this.ResetTransform();
    float scaleFactor = scaling / 100f;
    if ((double) scaleFactor == 0.0)
      scaleFactor = 1f;
    this.ScaleTransformMatrix(scaleFactor, PointF.Empty, 0.0f);
  }

  private bool IsNeedToChangeUnderLineWidth(string fontName)
  {
    return fontName == "Arial" || fontName == "Times New Roman" || fontName == "Century Gothic" || fontName == "Cambria" || fontName == "Verdana";
  }

  private void DrawUnderLine(WCharacterFormat characterFormat, RectangleF textBounds)
  {
    float x = textBounds.X;
    float y1 = textBounds.Y;
    Font font1 = characterFormat.Font;
    Font font2 = characterFormat.SubSuperScript != SubSuperScript.None ? characterFormat.Document.FontSettings.GetFont(font1.Name, font1.Size / 1.5f, font1.Style) : font1;
    float lineWidth = font2.Size / 15f;
    float y2 = y1 + (float) ((double) this.GetAscent(font2) + (double) this.GetDescent(font2) - 2.0 * (double) lineWidth);
    if (characterFormat.Bold && this.IsNeedToChangeUnderLineWidth(font2.Name))
    {
      lineWidth = font2.Size / 9.5f;
      y2 = (float) ((double) textBounds.Y + (double) this.GetAscent(font2) + (double) this.GetDescent(font2) - 0.64999997615814209 * (double) lineWidth);
    }
    if (characterFormat.UnderlineStyle == UnderlineStyle.DashLongHeavy)
      y2 = (float) ((double) textBounds.Y + (double) this.GetAscent(font2) + 1.5 * (double) lineWidth);
    else if (characterFormat.UnderlineStyle == UnderlineStyle.DashHeavy || characterFormat.UnderlineStyle == UnderlineStyle.DotDashHeavy)
      y2 = (float) ((double) textBounds.Y + (double) this.GetAscent(font2) + 1.75 * (double) lineWidth);
    this.Graphics.DrawLine(this.CreatePen(characterFormat, lineWidth), new PointF(x, y2), new PointF(x + textBounds.Width, y2));
  }

  private void DrawStrikeThrough(WCharacterFormat characterFormat, RectangleF textBounds)
  {
    float x1 = textBounds.X;
    float y1 = textBounds.Y;
    Font font1 = characterFormat.Font;
    Font font2 = characterFormat.SubSuperScript != SubSuperScript.None ? characterFormat.Document.FontSettings.GetFont(font1.Name, font1.Size / 1.5f, font1.Style) : font1;
    float lineWidth = font2.Size / 15f;
    if (characterFormat.Strikeout)
    {
      float y2 = y1 + (this.GetAscent(font2) - this.GetDescent(font2));
      this.Graphics.DrawLine(this.CreatePen(characterFormat, lineWidth), new PointF(x1, y2), new PointF(x1 + textBounds.Width, y2));
    }
    else
    {
      if (!characterFormat.DoubleStrike)
        return;
      float x2 = textBounds.X;
      float y3 = textBounds.Y + (this.GetAscent(font2) - (this.GetDescent(font2) + lineWidth));
      PointF start = new PointF(x2, y3);
      PointF end = new PointF(x2 + textBounds.Width, y3);
      this.DrawDoubleLine(characterFormat, BorderStyle.Double, lineWidth, start, end);
    }
  }

  private Pen CreatePen(WCharacterFormat charFormat, float lineWidth)
  {
    Color textColor = this.GetTextColor(charFormat);
    Color lineColor = Color.Black;
    if (textColor != Color.Black)
      lineColor = textColor;
    if (!charFormat.UnderlineColor.IsEmpty && charFormat.UnderlineColor.ToArgb() != 0 && charFormat.UnderlineColor != Color.Black)
      lineColor = charFormat.UnderlineColor;
    return this.GetPen(charFormat.UnderlineStyle, lineWidth, lineColor);
  }

  private bool IsNeedToRemoveClipBounds(IWidget widget)
  {
    if (this.ClipBoundsContainer == null || this.ClipBoundsContainer.Count == 0)
      return false;
    if (widget is ChildShape)
    {
      switch ((widget as ChildShape).ElementType)
      {
        case EntityType.TextBox:
        case EntityType.AutoShape:
          return true;
      }
    }
    if (widget is WTextBox || widget is Shape)
      return true;
    return widget is WTable && (widget as WTable).IsFrame;
  }

  private RectangleF TextboxClipBounds(WTable table, RectangleF clipBounds)
  {
    float num = 0.0f;
    float leftPadding = table.Rows[0].Cells[0].GetLeftPadding();
    for (int index = 0; index < table.Rows.Count; ++index)
    {
      if (table.Rows[index].Cells.Count > 0 && table.Rows[index].Cells.Count - 1 < table.Rows[index].Cells.Count)
      {
        float rightPadding = table.Rows[index].Cells[table.Rows[index].Cells.Count - 1].GetRightPadding();
        if ((double) rightPadding > (double) num)
          num = rightPadding;
      }
    }
    RectangleF rectangleF = new RectangleF(clipBounds.X, clipBounds.Y, clipBounds.Width, clipBounds.Height);
    rectangleF.X -= leftPadding;
    rectangleF.Width += leftPadding + num;
    return rectangleF;
  }

  private RectangleF UpdateClipBounds(RectangleF clipBounds, bool reverseClipping)
  {
    if (this.ClipBoundsContainer == null)
      this.ClipBoundsContainer = new Stack<RectangleF>();
    if (this.ClipBoundsContainer.Count > 0)
    {
      RectangleF ownerClipBounds = this.ClipBoundsContainer.Peek();
      if (reverseClipping)
      {
        ownerClipBounds = new RectangleF(ownerClipBounds.X, ownerClipBounds.Y, ownerClipBounds.Height, ownerClipBounds.Width);
        clipBounds = this.UpdateClipBounds(clipBounds, ownerClipBounds);
      }
      else
        clipBounds = this.UpdateClipBoundsBasedOnOwner(clipBounds, ownerClipBounds);
    }
    return clipBounds;
  }

  private void SetClip(RectangleF clippingBounds)
  {
    if (!(clippingBounds != RectangleF.Empty))
      return;
    this.Graphics.SetClip(this.GetClipBounds(clippingBounds, clippingBounds.Width, 0.0f), CombineMode.Replace);
  }

  private void ResetClip() => this.Graphics.ResetClip();

  internal void ResetTransform() => this.Graphics.ResetTransform();

  private void SetRotateTransform(RectangleF rotateBounds, float rotation, bool flipV, bool flipH)
  {
    this.Graphics.Transform = this.GetTransformMatrix(rotateBounds, rotation, flipH, flipV);
  }

  public void SetScaleTransform(float sx, float sy)
  {
    this.Graphics.ScaleTransform(sx, sy, MatrixOrder.Append);
  }

  private RectangleF GetClippingBounds(LayoutedWidget cellltWidget)
  {
    IWidget widget = cellltWidget.Widget;
    if (widget is WTableCell && (widget as WTableCell).Index == 0)
    {
      WParagraph childEntity = (widget as WTableCell).ChildEntities.Count > 0 ? (widget as WTableCell).ChildEntities[0] as WParagraph : (WParagraph) null;
      if (childEntity != null && childEntity.ParagraphFormat.IsFrame && ((double) childEntity.ParagraphFormat.FrameWidth > 0.0 || (double) childEntity.ParagraphFormat.FrameHeight > 0.0))
      {
        ushort num = (ushort) ((double) childEntity.ParagraphFormat.FrameHeight * 20.0);
        bool flag = ((int) num & 32768 /*0x8000*/) != 0;
        float width = childEntity.ParagraphFormat.FrameWidth;
        float height = flag ? 0.0f : (float) (((int) num & (int) short.MaxValue) / 20);
        LayoutedWidget firstItemInFrame = this.GetFirstItemInFrame(cellltWidget.Owner.Owner.Owner.ChildWidgets, cellltWidget.Owner.Owner.Owner.ChildWidgets.IndexOf(cellltWidget.Owner.Owner), childEntity.ParagraphFormat);
        float x;
        if ((double) width <= 0.0)
        {
          width = cellltWidget.Owner.Owner.Bounds.Width;
          x = cellltWidget.Owner.Owner.Bounds.X;
        }
        else
          x = firstItemInFrame.Bounds.X;
        float y;
        if ((double) height <= 0.0)
        {
          height = cellltWidget.Owner.Owner.Bounds.Height;
          y = cellltWidget.Owner.Owner.Bounds.Y;
        }
        else
          y = firstItemInFrame.Bounds.Y;
        return new RectangleF(x, y, width, height);
      }
    }
    return RectangleF.Empty;
  }

  private void Draw(WParagraph paragraph, LayoutedWidget ltWidget)
  {
    this.DrawImpl(ltWidget);
    this.currParagraph = paragraph;
    bool flag1 = ltWidget.ChildWidgets.Count > 0 && ltWidget.ChildWidgets[0].Widget == paragraph;
    if (ltWidget.Widget is WParagraph || ltWidget.Widget is SplitWidgetContainer && (ltWidget.Widget as SplitWidgetContainer).RealWidgetContainer is WParagraph)
    {
      WParagraph wparagraph = ltWidget.Widget is WParagraph ? ltWidget.Widget as WParagraph : (ltWidget.Widget as SplitWidgetContainer).RealWidgetContainer as WParagraph;
      if ((ltWidget.IsTrackChanges || wparagraph.IsInsertRevision || wparagraph.IsDeleteRevision || flag1 && wparagraph != null && (wparagraph.IsChangedPFormat || wparagraph.BreakCharacterFormat.IsChangedFormat)) && paragraph.Document.RevisionOptions.ShowRevisionBars)
      {
        PointF start = new PointF(0.0f, 0.0f);
        PointF end = new PointF(0.0f, 0.0f);
        float num1 = 0.0f;
        if (wparagraph.GetOwnerSection((Entity) wparagraph) is WSection)
          num1 = (wparagraph.GetOwnerSection((Entity) wparagraph) as WSection).PageSetup.Margins.Left;
        if ((wparagraph.OwnerTextBody.Owner is Shape || wparagraph.OwnerTextBody.Owner is WTextBox) && (wparagraph.OwnerTextBody.Owner as ParagraphItem).IsInsertRevision && !(wparagraph.OwnerTextBody.Owner as ParagraphItem).IsFloatingItem(false))
        {
          LayoutedWidget layoutedWidget = ltWidget;
          while (layoutedWidget != null && !(layoutedWidget.Widget is WTextBox) && !(layoutedWidget.Widget is Shape))
            layoutedWidget = layoutedWidget.Owner;
          if (layoutedWidget != null)
          {
            start = new PointF(num1 / 2f, layoutedWidget.Bounds.Y);
            end = new PointF(num1 / 2f, layoutedWidget.Bounds.Y + layoutedWidget.Bounds.Height);
          }
        }
        else
        {
          LayoutedWidget owner = ltWidget.Owner;
          bool flag2 = false;
          bool flag3 = false;
          if (owner.ChildWidgets.Count == 1)
          {
            flag2 = true;
            flag3 = true;
          }
          else if (owner.ChildWidgets[0] == ltWidget)
            flag2 = true;
          else if (owner.ChildWidgets[owner.ChildWidgets.Count - 1] == ltWidget)
            flag3 = true;
          float num2 = wparagraph.PreviousSibling != null ? (wparagraph.PreviousSibling is WParagraph ? (wparagraph.PreviousSibling as WParagraph).ParagraphFormat.AfterSpacing : 0.0f) : 0.0f;
          float num3 = (double) wparagraph.ParagraphFormat.BeforeSpacing > (double) num2 ? wparagraph.ParagraphFormat.BeforeSpacing - num2 : 0.0f;
          float num4 = flag2 ? num3 + wparagraph.ParagraphFormat.Borders.Top.GetLineWidthValue() + wparagraph.ParagraphFormat.Borders.Top.Space : 0.0f;
          float num5 = flag3 ? (wparagraph.IsInCell ? 0.0f : wparagraph.ParagraphFormat.AfterSpacing) + wparagraph.ParagraphFormat.Borders.Bottom.GetLineWidthValue() + wparagraph.ParagraphFormat.Borders.Bottom.Space : 0.0f;
          start = new PointF(num1 / 2f, ltWidget.Bounds.Y - num4);
          float num6 = !wparagraph.IsChangedPFormat || !flag1 ? ltWidget.Bounds.Height : ltWidget.ChildWidgets[0].Bounds.Height;
          end = new PointF(num1 / 2f, ltWidget.Bounds.Y + num6 + num5);
        }
        RevisionOptions revisionOptions = wparagraph.Document.RevisionOptions;
        this.DrawRevisionMark(start, end, this.GetRevisionColor(revisionOptions.RevisionBarsColor), revisionOptions.RevisionMarkWidth);
      }
    }
    if (ltWidget.Widget is SplitWidgetContainer && (ltWidget.Widget as SplitWidgetContainer).RealWidgetContainer is WParagraph)
    {
      WParagraph realWidgetContainer = (ltWidget.Widget as SplitWidgetContainer).RealWidgetContainer as WParagraph;
      int count = realWidgetContainer.ChildEntities.Count;
      int index = count - 1 - (ltWidget.Widget as SplitWidgetContainer).Count;
      if (!paragraph.IsInCell && count > (ltWidget.Widget as SplitWidgetContainer).Count && index >= 0 && index < count && realWidgetContainer.ChildEntities[index] is Break && realWidgetContainer.ChildEntities[index] is Break childEntity && (childEntity.BreakType == BreakType.PageBreak || childEntity.BreakType == BreakType.ColumnBreak) && !paragraph.SplitWidgetContainerDrawn)
      {
        flag1 = true;
        paragraph.SplitWidgetContainerDrawn = true;
      }
    }
    if (flag1)
      this.DrawParagraph(paragraph, ltWidget);
    else if (this.IsLinesInteresectWithFloatingItems(ltWidget, false))
    {
      WParagraphFormat paragraphFormat = paragraph.ParagraphFormat;
      bool isEmpty = paragraphFormat.BackColor.IsEmpty;
      if (paragraphFormat.TextureStyle != TextureStyle.TextureNone || !isEmpty)
        this.DrawLineBackGroundColors(paragraph, ltWidget);
    }
    bool flag4 = ltWidget.ChildWidgets.Count > 0 && (ltWidget.ChildWidgets[0].Widget is SplitWidgetContainer ? (ltWidget.ChildWidgets[0].Widget as SplitWidgetContainer).RealWidgetContainer == paragraph : ltWidget.ChildWidgets[0].Widget == paragraph);
    RevisionOptions revisionOptions1 = paragraph.Document.RevisionOptions;
    if (flag4 || revisionOptions1.CommentDisplayMode != CommentDisplayMode.ShowInBalloons || ltWidget.ChildWidgets.Count <= 0)
      return;
    switch (this.GetBaseEntity((Entity) paragraph))
    {
      case Shape _:
        break;
      case WTextBox _:
        break;
      case GroupShape _:
        break;
      case HeaderFooter _:
        break;
      default:
        if (this.m_previousLineCommentStartMarks == null)
          this.m_previousLineCommentStartMarks = new List<KeyValuePair<string, bool>>();
        this.DrawCommentHighlighter(ltWidget, paragraph.Document);
        break;
    }
  }

  private void DrawCommentHighlighter(LayoutedWidget ltWidget, WordDocument document)
  {
    RevisionOptions revisionOptions = document.RevisionOptions;
    LayoutedWidget maximumHeightWidget = this.GetMaximumHeightWidget(ltWidget);
    float height = maximumHeightWidget != null ? maximumHeightWidget.Bounds.Height : 0.0f;
    float y = maximumHeightWidget != null ? maximumHeightWidget.Bounds.Y : 0.0f;
    bool isResolvedComment = false;
    if (this.m_previousLineCommentStartMarks.Count > 0)
      isResolvedComment = this.m_previousLineCommentStartMarks[0].Value;
    for (int index = 0; index < ltWidget.ChildWidgets.Count; ++index)
    {
      LayoutedWidget childWidget = ltWidget.ChildWidgets[index];
      if (childWidget.Widget is WCommentMark && (childWidget.Widget as WCommentMark).Comment != null)
      {
        WCommentMark widget = childWidget.Widget as WCommentMark;
        if (widget.Type == CommentMarkType.CommentStart && widget.Comment.CommentRangeEnd != null)
        {
          if (this.m_previousLineCommentStartMarks.Count == 0 && widget.Comment.IsResolved)
            isResolvedComment = true;
          this.m_previousLineCommentStartMarks.Add(new KeyValuePair<string, bool>(widget.CommentId, widget.Comment.IsResolved));
        }
        else if (widget.Type == CommentMarkType.CommentEnd)
        {
          if (this.ContainsKey(widget.CommentId, this.m_previousLineCommentStartMarks))
            this.m_previousLineCommentStartMarks.Remove(this.GetKeyValuePair(widget.CommentId, this.m_previousLineCommentStartMarks));
          if (this.m_previousLineCommentStartMarks.Count > 0)
            isResolvedComment = this.m_previousLineCommentStartMarks[0].Value;
        }
        childWidget.Bounds = new RectangleF(childWidget.Bounds.X, childWidget.Bounds.Y, childWidget.Bounds.Width, height);
      }
      if (this.m_previousLineCommentStartMarks.Count > 0)
      {
        RectangleF bounds = new RectangleF(childWidget.Bounds.X, y, childWidget.Bounds.Width, height);
        this.DrawCommentHighlighter(revisionOptions, bounds, isResolvedComment);
      }
    }
  }

  private KeyValuePair<string, bool> GetKeyValuePair(
    string inputKey,
    List<KeyValuePair<string, bool>> keyValuePairCollection)
  {
    foreach (KeyValuePair<string, bool> keyValuePair in keyValuePairCollection)
    {
      if (keyValuePair.Key == inputKey)
        return keyValuePair;
    }
    return new KeyValuePair<string, bool>(string.Empty, false);
  }

  private bool ContainsKey(
    string inputKey,
    List<KeyValuePair<string, bool>> keyValuePairCollection)
  {
    return this.GetKeyValuePair(inputKey, keyValuePairCollection).Key == inputKey;
  }

  private void DrawCommentHighlighter(
    RevisionOptions revisionOptions,
    RectangleF bounds,
    bool isResolvedComment)
  {
    if ((double) bounds.Width <= 0.0)
      return;
    this.Graphics.FillRectangle((Brush) this.CreateSolidBrush(this.GetRevisionFillColor(revisionOptions.CommentColor, isResolvedComment)), bounds);
  }

  private LayoutedWidget GetMaximumHeightWidget(LayoutedWidget ltWidget)
  {
    LayoutedWidget maximumHeightWidget = (LayoutedWidget) null;
    float num = 0.0f;
    for (int index = 0; index < ltWidget.ChildWidgets.Count; ++index)
    {
      LayoutedWidget childWidget = ltWidget.ChildWidgets[index];
      if ((!(childWidget.Widget is WPicture) && !(childWidget.Widget is Shape) && !(childWidget.Widget is WTextBox) && !(childWidget.Widget is WChart) && !(childWidget.Widget is GroupShape) || (childWidget.Widget as ParagraphItem).GetTextWrappingStyle() == TextWrappingStyle.Inline) && (double) num < (double) childWidget.Bounds.Height)
      {
        num = childWidget.Bounds.Height;
        maximumHeightWidget = childWidget;
      }
    }
    return maximumHeightWidget;
  }

  private bool IsLinesInteresectWithFloatingItems(LayoutedWidget ltWidget, bool isLineContainer)
  {
    if (!isLineContainer)
      ltWidget = ltWidget.Owner;
    for (int index = 0; index < ltWidget.ChildWidgets.Count; ++index)
    {
      if (ltWidget.ChildWidgets[index].IntersectingBounds.Count > 0)
        return true;
    }
    return false;
  }

  private void DrawLineBackGroundColors(WParagraph paragraph, LayoutedWidget ltWidget)
  {
    bool resetTransform = false;
    RectangleF paragraphBackGroundColor = this.GetBoundsToDrawParagraphBackGroundColor(paragraph, ltWidget, paragraph.BreakCharacterFormat.Hidden, true, ref resetTransform);
    if ((double) paragraphBackGroundColor.Width <= 0.0 || (double) paragraphBackGroundColor.Height <= 0.0)
      return;
    IEntity nextSibling = paragraph.NextSibling;
    WParagraphFormat paragraphFormat = paragraph.ParagraphFormat;
    if (ltWidget.Owner.ChildWidgets[ltWidget.Owner.ChildWidgets.Count - 1] == ltWidget && paragraph.GetOwnerEntity() is WTableCell && paragraph.GetOwnerEntity() is WTableCell ownerEntity && ownerEntity.OwnerRow != null && ((IWidget) ownerEntity.OwnerRow).LayoutInfo is RowLayoutInfo)
    {
      RowLayoutInfo layoutInfo = ((IWidget) ownerEntity.OwnerRow).LayoutInfo as RowLayoutInfo;
      if (paragraph.IsInCell && !layoutInfo.IsExactlyRowHeight && (nextSibling == null || !(nextSibling is WParagraph) || !(nextSibling as WParagraph).ParagraphFormat.BackColor.IsEmpty || !ltWidget.IsLastItemInPage))
        paragraphBackGroundColor.Height -= paragraphFormat.AfterSpacing;
    }
    List<RectangleF> colorRenderingBounds = this.GetBackGroundColorRenderingBounds(ltWidget, paragraphBackGroundColor);
    if (paragraphFormat.TextureStyle != TextureStyle.TextureNone)
    {
      for (int index = 0; index < colorRenderingBounds.Count; ++index)
        this.DrawTextureStyle(paragraphFormat.TextureStyle, paragraphFormat.ForeColor, paragraphFormat.BackColor, colorRenderingBounds[index]);
    }
    else if (!paragraphFormat.BackColor.IsEmpty)
    {
      for (int index = 0; index < colorRenderingBounds.Count; ++index)
        this.Graphics.FillRectangle((Brush) this.CreateSolidBrush(paragraphFormat.BackColor), colorRenderingBounds[index]);
    }
    colorRenderingBounds.Clear();
    if (!resetTransform)
      return;
    this.ResetTransform();
  }

  private List<RectangleF> GetBackGroundColorRenderingBounds(
    LayoutedWidget ltWidget,
    RectangleF remaingingBounds)
  {
    List<RectangleF> intersectingBounds = ltWidget.IntersectingBounds;
    List<RectangleF> colorRenderingBounds = new List<RectangleF>();
    if (intersectingBounds.Count > 0)
    {
      RectangleF itemsRenderingBounds = this.GetInnerItemsRenderingBounds(ltWidget);
      bool isNeedToFindFillColorRenderingBounds = true;
      remaingingBounds = this.FindFillColorBounds(ref isNeedToFindFillColorRenderingBounds, itemsRenderingBounds, intersectingBounds, remaingingBounds);
      if (isNeedToFindFillColorRenderingBounds)
      {
        for (int index = 0; index < ltWidget.IntersectingBounds.Count; ++index)
        {
          RectangleF intersectingBound = ltWidget.IntersectingBounds[index];
          if ((double) remaingingBounds.X < (double) intersectingBound.X)
          {
            RectangleF rectangleF = new RectangleF(remaingingBounds.X, remaingingBounds.Y, ltWidget.IntersectingBounds[index].X - remaingingBounds.X, remaingingBounds.Height);
            colorRenderingBounds.Add(rectangleF);
            remaingingBounds = new RectangleF(ltWidget.IntersectingBounds[index].Right, remaingingBounds.Y, remaingingBounds.Right - ltWidget.IntersectingBounds[index].Right, remaingingBounds.Height);
          }
          else if ((double) remaingingBounds.X > (double) intersectingBound.Right)
            remaingingBounds = new RectangleF(intersectingBound.Right, remaingingBounds.Y, remaingingBounds.Right - intersectingBound.Right, remaingingBounds.Height);
        }
      }
      colorRenderingBounds.Add(remaingingBounds);
      intersectingBounds.Clear();
    }
    return colorRenderingBounds;
  }

  private RectangleF FindFillColorBounds(
    ref bool isNeedToFindFillColorRenderingBounds,
    RectangleF childItemBounds,
    List<RectangleF> intersectingBoundsCollection,
    RectangleF remaingingBounds)
  {
    if ((double) childItemBounds.X < (double) intersectingBoundsCollection[0].X && (double) childItemBounds.Right < (double) intersectingBoundsCollection[0].X)
    {
      remaingingBounds = new RectangleF(remaingingBounds.X, remaingingBounds.Y, intersectingBoundsCollection[0].X - remaingingBounds.X, remaingingBounds.Height);
      isNeedToFindFillColorRenderingBounds = false;
    }
    else if ((double) childItemBounds.X >= (double) intersectingBoundsCollection[intersectingBoundsCollection.Count - 1].Right)
    {
      remaingingBounds = new RectangleF(intersectingBoundsCollection[intersectingBoundsCollection.Count - 1].Right, remaingingBounds.Y, remaingingBounds.Right - intersectingBoundsCollection[intersectingBoundsCollection.Count - 1].Right, remaingingBounds.Height);
      isNeedToFindFillColorRenderingBounds = false;
    }
    else
    {
      for (int index = 0; index + 1 < intersectingBoundsCollection.Count; ++index)
      {
        RectangleF intersectingBounds1 = intersectingBoundsCollection[index];
        RectangleF intersectingBounds2 = intersectingBoundsCollection[index + 1];
        if ((double) childItemBounds.X >= (double) intersectingBounds1.Right && (double) childItemBounds.Right < (double) intersectingBounds2.X)
        {
          remaingingBounds = new RectangleF(intersectingBounds1.Right, remaingingBounds.Y, intersectingBounds2.X - intersectingBounds1.Right, remaingingBounds.Height);
          isNeedToFindFillColorRenderingBounds = false;
          break;
        }
      }
    }
    return remaingingBounds;
  }

  public RectangleF GetInnerItemsRenderingBounds(LayoutedWidget ltWidget)
  {
    RectangleF bounds = ltWidget.Bounds;
    float num1 = float.MaxValue;
    float num2 = 0.0f;
    for (int index = 0; index < ltWidget.ChildWidgets.Count; ++index)
    {
      if (ltWidget.ChildWidgets[index].Widget is ParagraphItem widget && !widget.IsFloatingItem(false))
      {
        if ((double) num1 > (double) ltWidget.ChildWidgets[index].Bounds.X)
          num1 = ltWidget.ChildWidgets[index].Bounds.X;
        if ((double) num2 < (double) ltWidget.ChildWidgets[index].Bounds.Right)
          num2 = ltWidget.ChildWidgets[index].Bounds.Right;
      }
    }
    return new RectangleF((double) num1 != 3.4028234663852886E+38 ? num1 : bounds.X, bounds.Y, num2 - num1, bounds.Height);
  }

  internal void Draw(SplitTableWidget splitTableWidget, LayoutedWidget layoutedWidget)
  {
    throw new NotImplementedException();
  }

  private void Draw(BlockContentControl SDT, LayoutedWidget ltWidget)
  {
  }

  private void Draw(InlineContentControl SDT, LayoutedWidget ltWidget)
  {
  }

  private void Draw(WCommentMark commentMark, LayoutedWidget ltWidget)
  {
    this.AddCommentMark(commentMark, ltWidget);
  }

  private void Draw(WAbsoluteTab absoluteTab, LayoutedWidget ltWidget)
  {
    this.DrawAbsoluteTab(absoluteTab, ltWidget);
  }

  internal void Draw(WChart chart, LayoutedWidget ltWidget)
  {
    if (chart.Document.ChartToImageConverter == null)
      return;
    chart.InitializeOfficeChartToImageConverter();
    this.DrawChart(chart, ltWidget);
  }

  internal void DrawChart(WChart chart, LayoutedWidget widget)
  {
    RectangleF bounds = widget.Bounds;
    MemoryStream memoryStream = new MemoryStream();
    chart.OfficeChart.Width = (double) chart.Width;
    chart.OfficeChart.Height = (double) chart.Height;
    chart.OfficeChart.SaveAsImage((Stream) memoryStream);
    if (memoryStream.Length > 0L)
    {
      this.Graphics.DrawImage(this.GetImage(this.CreateImageFromStream(memoryStream)), bounds);
      this.Graphics.ResetTransform();
    }
    memoryStream.Dispose();
  }

  internal void Draw(WCheckBox checkBox, LayoutedWidget ltWidget)
  {
    if (this.PreserveFormField)
      this.m_editableFormFieldinEMF.Add(ltWidget);
    else
      this.DrawCheckBox(checkBox, ltWidget);
  }

  internal void Draw(WDropDownFormField dropDownFormField, LayoutedWidget ltWidget)
  {
    if (this.PreserveFormField)
      this.m_editableFormFieldinEMF.Add(ltWidget);
    else
      this.DrawString(dropDownFormField.ScriptType, dropDownFormField.DropDownValue, dropDownFormField.CharacterFormat, (WParagraphFormat) null, ltWidget.Bounds, ltWidget.Bounds.Width, ltWidget);
  }

  private void Draw(WField field, LayoutedWidget ltWidget)
  {
    switch (field.FieldType)
    {
      case FieldType.FieldRef:
      case FieldType.FieldPageRef:
        string bkName = (string) null;
        if (!field.IsBookmarkCrossRefField(ref bkName))
          break;
        this.CurrentRefField = field;
        this.CurrentBookmarkName = bkName;
        break;
      case FieldType.FieldNumPages:
        WField wfield1 = field;
        this.currTextRange = field.GetCurrentTextRange();
        this.DrawString(this.currTextRange.ScriptType, field.FieldResult, this.currTextRange.CharacterFormat, wfield1.GetOwnerParagraphValue().ParagraphFormat, ltWidget.Bounds, ltWidget.Bounds.Width, ltWidget);
        this.AddLinkToBookmark(ltWidget.Bounds, "NUMPAGES-" + field.FieldResult, true);
        break;
      case FieldType.FieldPage:
        WField wfield2 = field;
        this.currTextRange = field.GetCurrentTextRange();
        this.DrawString(this.currTextRange.ScriptType, ltWidget.TextTag, this.currTextRange.CharacterFormat, wfield2.GetOwnerParagraphValue().ParagraphFormat, ltWidget.Bounds, ltWidget.Bounds.Width, ltWidget);
        this.AddLinkToBookmark(ltWidget.Bounds, "PAGE-" + ltWidget.TextTag, true);
        break;
      case FieldType.FieldExpression:
        WField wfield3 = field;
        for (int index = 0; index < DocumentLayouter.EquationFields.Count; ++index)
        {
          if (DocumentLayouter.EquationFields[index].EQFieldEntity == wfield3)
          {
            this.AlignEqFieldSwitches(DocumentLayouter.EquationFields[index].LayouttedEQField, ltWidget.Bounds.X, ltWidget.Bounds.Y);
            this.currTextRange = field.GetCurrentTextRange();
            this.DrawEquationField(this.currTextRange.ScriptType, DocumentLayouter.EquationFields[index].LayouttedEQField, this.currTextRange.CharacterFormat);
            break;
          }
        }
        break;
      case FieldType.FieldAutoNum:
        WField wfield4 = field;
        this.currTextRange = field.GetCurrentTextRange();
        this.DrawString(this.currTextRange.ScriptType, ltWidget.TextTag, this.currTextRange.CharacterFormat, wfield4.GetOwnerParagraphValue().ParagraphFormat, ltWidget.Bounds, ltWidget.Bounds.Width, ltWidget);
        break;
      case FieldType.FieldDocVariable:
        WField wfield5 = field;
        ltWidget.TextTag = field.Document.Variables[wfield5.FieldValue];
        this.DrawString(wfield5.ScriptType, ltWidget.TextTag, wfield5.CharacterFormat, wfield5.OwnerParagraph.ParagraphFormat, ltWidget.Bounds, ltWidget.Bounds.Width, ltWidget);
        break;
      case FieldType.FieldSectionPages:
        WField wfield6 = field;
        this.currTextRange = field.GetCurrentTextRange();
        this.DrawString(this.currTextRange.ScriptType, ltWidget.TextTag, this.currTextRange.CharacterFormat, wfield6.GetOwnerParagraphValue().ParagraphFormat, ltWidget.Bounds, ltWidget.Bounds.Width, ltWidget);
        this.AddLinkToBookmark(ltWidget.Bounds, "SECTIONPAGES-" + ltWidget.TextTag, true);
        break;
      case FieldType.FieldHyperlink:
        this.currHyperlink = new Hyperlink(field);
        break;
    }
  }

  internal void Draw(WOleObject oleObject, LayoutedWidget ltWidget)
  {
    if (oleObject == null || oleObject.OlePicture == null)
      return;
    this.DrawPicture(oleObject.OlePicture, ltWidget);
  }

  internal void Draw(WPicture picture, LayoutedWidget ltWidget)
  {
    this.DrawPicture(picture, ltWidget);
  }

  internal void Draw(WSymbol symbol, LayoutedWidget ltWidget) => this.DrawSymbol(symbol, ltWidget);

  private void Draw(WTable table, LayoutedWidget ltWidget)
  {
    if (table.IsFrame)
    {
      WParagraph paragraph = table.Rows[0].Cells[0].Paragraphs[0];
      if (!ltWidget.Bounds.IsEmpty && ((double) paragraph.ParagraphFormat.FrameHeight != 0.0 || (double) paragraph.ParagraphFormat.FrameWidth != 0.0) && paragraph.m_layoutInfo != null)
      {
        RectangleF clipBounds = this.GetClippingBounds(ltWidget.ChildWidgets[0].ChildWidgets[0]);
        if ((double) paragraph.ParagraphFormat.FrameWidth < (double) ltWidget.Bounds.Width)
          clipBounds = new RectangleF(clipBounds.X, clipBounds.Y, clipBounds.Width + paragraph.ParagraphFormat.FrameHorizontalDistanceFromText, clipBounds.Height);
        RectangleF bounds = this.UpdateClipBounds(clipBounds, false);
        bounds = this.GetClipBounds(bounds, bounds.Width, 0.0f);
        this.ClipBoundsContainer.Push(bounds);
      }
    }
    this.DrawTable(table, ltWidget);
  }

  private LayoutedWidget GetFirstItemInFrame(
    LayoutedWidgetList layoutedWidgets,
    int index,
    WParagraphFormat originalFormat)
  {
    for (; index - 1 > -1; --index)
    {
      WParagraphFormat wparagraphFormat = new WParagraphFormat();
      LayoutedWidget layoutedWidget = layoutedWidgets[index - 1];
      if (layoutedWidget.Widget is WTable)
        wparagraphFormat = (layoutedWidget.Widget as WTable).Rows[0].Cells[0].Paragraphs[0].ParagraphFormat;
      else if (layoutedWidget.Widget is WParagraph)
        wparagraphFormat = (layoutedWidget.Widget as WParagraph).ParagraphFormat;
      if (!wparagraphFormat.IsInSameFrame(originalFormat))
        break;
    }
    return layoutedWidgets[index];
  }

  private void Draw(WTableCell cell, LayoutedWidget ltWidget)
  {
    if (cell.CellFormat.IsFormattingChange && cell.Document.RevisionOptions.ShowRevisionBars && cell.GetOwnerSection((Entity) cell) is WSection)
    {
      float left = (cell.GetOwnerSection((Entity) cell) as WSection).PageSetup.Margins.Left;
      PointF start = new PointF(left / 2f, ltWidget.Bounds.Y);
      PointF end = new PointF(left / 2f, ltWidget.Bounds.Y + ltWidget.Bounds.Height);
      RevisionOptions revisionOptions = cell.Document.RevisionOptions;
      this.DrawRevisionMark(start, end, this.GetRevisionColor(revisionOptions.RevisionBarsColor), revisionOptions.RevisionMarkWidth);
    }
    this.DrawTableCell(cell, ltWidget);
  }

  private void Draw(WTableRow row, LayoutedWidget ltWidget)
  {
    if ((row.IsInsertRevision || row.RowFormat.IsChangedFormat || row.IsDeleteRevision && row.Document.RevisionOptions.ShowDeletedText) && row.Document.RevisionOptions.ShowRevisionBars && row.GetOwnerSection((Entity) row) is WSection)
    {
      float left = (row.GetOwnerSection((Entity) row) as WSection).PageSetup.Margins.Left;
      PointF start = new PointF(left / 2f, ltWidget.Bounds.Y);
      PointF end = new PointF(left / 2f, ltWidget.Bounds.Y + ltWidget.Bounds.Height);
      RevisionOptions revisionOptions = row.Document.RevisionOptions;
      this.DrawRevisionMark(start, end, this.GetRevisionColor(revisionOptions.RevisionBarsColor), revisionOptions.RevisionMarkWidth);
    }
    this.DrawTableRow(row, ltWidget);
  }

  internal void Draw(WTextBox textBox, LayoutedWidget ltWidget)
  {
    RectangleF clipBounds = ltWidget.Bounds;
    clipBounds.Y += textBox.TextBoxFormat.InternalMargin.Top;
    clipBounds.Height -= textBox.TextBoxFormat.InternalMargin.Top + textBox.TextBoxFormat.InternalMargin.Bottom;
    if (textBox.TextBoxFormat.TextDirection != TextDirection.Horizontal && textBox.TextBoxFormat.TextDirection != TextDirection.HorizontalFarEast)
    {
      RectangleF rectangleF = clipBounds;
      rectangleF.Width += rectangleF.Height;
      rectangleF.Height = rectangleF.Width - rectangleF.Height;
      rectangleF.Width -= rectangleF.Height;
      clipBounds = rectangleF;
    }
    this.ClipBoundsContainer.Push(this.UpdateClipBounds(clipBounds, false));
    if (!textBox.Visible)
      return;
    this.DrawTextBox(textBox, ltWidget);
  }

  internal void Draw(WTextFormField textFormField, LayoutedWidget ltWidget)
  {
  }

  internal void Draw(LeafEmtyWidget leafEmtyWidget, LayoutedWidget layoutedWidget)
  {
  }

  internal void Draw(Shape shape, LayoutedWidget ltWidget)
  {
    RectangleF rectangleF1 = this.UpdateClipBounds(ltWidget.Bounds, false);
    if (((IWidget) shape).LayoutInfo.IsVerticalText)
    {
      RectangleF rectangleF2 = rectangleF1;
      rectangleF2.Width += rectangleF2.Height;
      rectangleF2.Height = rectangleF2.Width - rectangleF2.Height;
      rectangleF2.Width -= rectangleF2.Height;
      rectangleF1 = rectangleF2;
    }
    this.ClipBoundsContainer.Push(rectangleF1);
    if (!shape.Visible)
      return;
    this.DrawShape(shape, ltWidget);
  }

  internal void Draw(ChildShape shape, LayoutedWidget ltWidget)
  {
    this.DrawChildShape(shape, ltWidget);
  }

  internal void Draw(SplitStringWidget splitStringWidget, LayoutedWidget layoutedWidget)
  {
    this.Draw(splitStringWidget.RealStringWidget, layoutedWidget, splitStringWidget.SplittedText);
    this.DrawImpl(layoutedWidget);
  }

  internal void Draw(WTextRange textRange, LayoutedWidget ltWidget)
  {
    string text = ltWidget.TextTag != null ? ltWidget.TextTag : textRange.Text;
    this.Draw((IStringWidget) textRange, ltWidget, text);
  }

  internal void Draw(IStringWidget stringWidget, LayoutedWidget ltWidget, string text)
  {
    if (!this.PreserveFormField)
    {
      this.DrawTextRange(stringWidget as WTextRange, ltWidget, text);
      this.DrawImpl(ltWidget);
    }
    else
    {
      WTextRange wtextRange = ltWidget.Widget is WTextRange ? ltWidget.Widget as WTextRange : stringWidget as WTextRange;
      if (wtextRange != null && wtextRange.PreviousSibling is WFieldMark)
      {
        WFieldMark previousSibling = wtextRange.PreviousSibling as WFieldMark;
        if (previousSibling.ParentField is WTextFormField)
        {
          WTextFormField parentField = previousSibling.ParentField as WTextFormField;
          if (this.m_editableTextFormTextRange == null && previousSibling.Type != FieldMarkType.FieldEnd)
          {
            this.m_editableFormFieldinEMF.Add(ltWidget);
            this.m_editableTextFormTextRange = wtextRange;
            this.m_editableTextFormBounds = ltWidget.Bounds;
            if (string.IsNullOrEmpty(parentField.Name))
              parentField.Name = "Textformfield";
            for (int index = ltWidget.Owner.ChildWidgets.Count - 1; index >= 0; --index)
            {
              if (ltWidget.Owner.ChildWidgets[index].Widget is WTextRange || ltWidget.Owner.ChildWidgets[index].Widget is SplitStringWidget)
              {
                this.m_lastTextRangeIndex = index;
                break;
              }
            }
          }
        }
      }
      if (this.m_editableTextFormTextRange != null)
      {
        if (this.m_lastTextRangeIndex != 0 && this.m_editableTextFormTextRange != wtextRange)
          this.m_editableTextFormBounds.Width += ltWidget.Bounds.Width;
        if (ltWidget.Widget == ltWidget.Owner.ChildWidgets[this.m_lastTextRangeIndex].Widget)
          this.m_lastTextRangeIndex = 0;
        if (ltWidget.Widget is SplitStringWidget && (double) this.m_pageMarginLeft <= (double) ltWidget.Bounds.X)
          this.m_editableTextFormText += (ltWidget.Widget as SplitStringWidget).SplittedText;
        if (ltWidget.Widget is WTextRange && (double) this.m_pageMarginLeft <= (double) ltWidget.Bounds.X)
          this.m_editableTextFormText += (ltWidget.Widget as WTextRange).Text;
        string str1 = "";
        if (this.m_editableTextFormTextRange.PreviousSibling is WFieldMark)
        {
          WFieldMark previousSibling = this.m_editableTextFormTextRange.PreviousSibling as WFieldMark;
          if (previousSibling.ParentField is WTextFormField)
            str1 = (previousSibling.ParentField as WTextFormField).Text;
        }
        string str2 = str1.Replace(" ", string.Empty);
        this.m_editableTextFormText = this.m_editableTextFormText.Replace(" ", string.Empty);
        if (!(this.m_editableTextFormText == str2))
          return;
        int index = this.m_editableFormFieldinEMF.Count - 1;
        this.m_editableFormFieldinEMF[index].Bounds = new RectangleF(this.m_editableFormFieldinEMF[index].Bounds.X, this.m_editableFormFieldinEMF[index].Bounds.Y, this.m_editableTextFormBounds.Width, this.m_editableFormFieldinEMF[index].Bounds.Height);
        this.m_editableTextFormBounds.Width = 0.0f;
        this.m_editableTextFormTextRange = (WTextRange) null;
        this.m_editableTextFormText = "";
      }
      else
      {
        this.DrawTextRange(stringWidget as WTextRange, ltWidget, text);
        this.DrawImpl(ltWidget);
      }
    }
  }

  internal void DrawImpl(LayoutedWidget ltWidget)
  {
  }

  internal void DrawImpl(WFootnote footNote, LayoutedWidget ltWidget)
  {
    if (!(((IWidget) footNote).LayoutInfo is FootnoteLayoutInfo layoutInfo))
      return;
    this.DrawTextRange(layoutInfo.TextRange, ltWidget, layoutInfo.FootnoteID);
  }

  private Entity GetOwnerLayoutedWidget(LayoutedWidget ltWidget)
  {
    return ltWidget.Owner != null ? (ltWidget.Owner.Widget is Entity ? ltWidget.Owner.Widget as Entity : (ltWidget.Owner.Widget is SplitWidgetContainer ? (ltWidget.Owner.Widget as SplitWidgetContainer).RealWidgetContainer as Entity : (Entity) null)) : (Entity) null;
  }

  private WParagraph GetParagraphWidget(LayoutedWidget ltWidget)
  {
    if (ltWidget.Widget is WParagraph)
      return ltWidget.Widget as WParagraph;
    return !(ltWidget.Widget is SplitWidgetContainer) ? (WParagraph) null : (ltWidget.Widget as SplitWidgetContainer).RealWidgetContainer as WParagraph;
  }

  private WTableCell GetCellWidget(LayoutedWidget ltWidget)
  {
    if (ltWidget.Widget is WTableCell)
      return ltWidget.Widget as WTableCell;
    return !(ltWidget.Widget is SplitWidgetContainer) ? (WTableCell) null : (ltWidget.Widget as SplitWidgetContainer).RealWidgetContainer as WTableCell;
  }

  private bool IsOverLappedShapeWidget(LayoutedWidget ltWidget)
  {
    if (ltWidget.Widget is WPicture && (ltWidget.Widget as WPicture).TextWrappingStyle != TextWrappingStyle.Inline && (ltWidget.Widget as WPicture).TextWrappingStyle != TextWrappingStyle.Behind && ((ltWidget.Widget as WPicture).Document.Settings.CompatibilityMode == CompatibilityMode.Word2013 || !(ltWidget.Widget as WPicture).IsBelowText || (ltWidget.Widget as WPicture).TextWrappingStyle != TextWrappingStyle.Tight && (ltWidget.Widget as WPicture).TextWrappingStyle != TextWrappingStyle.Through) || ltWidget.Widget is WChart && (ltWidget.Widget as WChart).WrapFormat.TextWrappingStyle != TextWrappingStyle.Inline && (ltWidget.Widget as WChart).WrapFormat.TextWrappingStyle != TextWrappingStyle.Behind && ((ltWidget.Widget as WChart).Document.Settings.CompatibilityMode == CompatibilityMode.Word2013 || !(ltWidget.Widget as WChart).IsBelowText || (ltWidget.Widget as WChart).WrapFormat.TextWrappingStyle != TextWrappingStyle.Tight && (ltWidget.Widget as WChart).WrapFormat.TextWrappingStyle != TextWrappingStyle.Through) || ltWidget.Widget is Shape && (ltWidget.Widget as Shape).WrapFormat.TextWrappingStyle != TextWrappingStyle.Inline && (ltWidget.Widget as Shape).WrapFormat.TextWrappingStyle != TextWrappingStyle.Behind && ((ltWidget.Widget as Shape).Document.Settings.CompatibilityMode == CompatibilityMode.Word2013 || !(ltWidget.Widget as Shape).IsBelowText || (ltWidget.Widget as Shape).WrapFormat.TextWrappingStyle != TextWrappingStyle.Tight && (ltWidget.Widget as Shape).WrapFormat.TextWrappingStyle != TextWrappingStyle.Through) || ltWidget.Widget is GroupShape && (ltWidget.Widget as GroupShape).WrapFormat.TextWrappingStyle != TextWrappingStyle.Inline && (ltWidget.Widget as GroupShape).WrapFormat.TextWrappingStyle != TextWrappingStyle.Behind && ((ltWidget.Widget as GroupShape).Document.Settings.CompatibilityMode == CompatibilityMode.Word2013 || !(ltWidget.Widget as GroupShape).IsBelowText || (ltWidget.Widget as GroupShape).WrapFormat.TextWrappingStyle != TextWrappingStyle.Tight && (ltWidget.Widget as GroupShape).WrapFormat.TextWrappingStyle != TextWrappingStyle.Through))
      return true;
    if (!(ltWidget.Widget is WTextBox) || (ltWidget.Widget as WTextBox).TextBoxFormat.TextWrappingStyle == TextWrappingStyle.Inline || (ltWidget.Widget as WTextBox).TextBoxFormat.TextWrappingStyle == TextWrappingStyle.Behind)
      return false;
    if ((ltWidget.Widget as WTextBox).Document.Settings.CompatibilityMode == CompatibilityMode.Word2013 || !(ltWidget.Widget as WTextBox).TextBoxFormat.IsBelowText)
      return true;
    return (ltWidget.Widget as WTextBox).TextBoxFormat.TextWrappingStyle != TextWrappingStyle.Tight && (ltWidget.Widget as WTextBox).TextBoxFormat.TextWrappingStyle != TextWrappingStyle.Through;
  }

  private RectangleF UpdateWaterMarkPosition(
    ParagraphItem pItem,
    WPageSetup pageSetup,
    RectangleF bounds)
  {
    float num1 = 0.0f;
    float num2 = 0.0f;
    TextWrappingStyle textWrappingStyle = TextWrappingStyle.Behind;
    if (pItem is WPicture)
    {
      WPicture wpicture = pItem as WPicture;
      num1 = wpicture.Width;
      num2 = wpicture.Height;
      textWrappingStyle = wpicture.TextWrappingStyle;
    }
    else if (pItem is TextWatermark textWatermark1)
    {
      num1 = textWatermark1.Width;
      num2 = textWatermark1.Height;
      textWrappingStyle = textWatermark1.TextWrappingStyle;
    }
    if (textWrappingStyle != TextWrappingStyle.Inline)
    {
      float num3 = 0.0f;
      float num4 = 0.0f;
      VerticalOrigin verticalOrigin = VerticalOrigin.Margin;
      HorizontalOrigin horizontalOrigin = HorizontalOrigin.Margin;
      ShapeVerticalAlignment verticalAlignment = ShapeVerticalAlignment.None;
      ShapeHorizontalAlignment horizontalAlignment = ShapeHorizontalAlignment.None;
      switch (pItem)
      {
        case WPicture _:
          WPicture wpicture = pItem as WPicture;
          num3 = wpicture.VerticalPosition;
          num4 = wpicture.HorizontalPosition;
          verticalOrigin = wpicture.VerticalOrigin;
          horizontalOrigin = wpicture.HorizontalOrigin;
          verticalAlignment = wpicture.VerticalAlignment;
          horizontalAlignment = wpicture.HorizontalAlignment;
          break;
        case TextWatermark textWatermark2:
          num3 = textWatermark2.VerticalPosition;
          num4 = textWatermark2.HorizontalPosition;
          verticalOrigin = textWatermark2.VerticalOrigin;
          horizontalOrigin = textWatermark2.HorizontalOrigin;
          verticalAlignment = textWatermark2.VerticalAlignment;
          horizontalAlignment = textWatermark2.HorizontalAlignment;
          break;
      }
      float num5 = bounds.Top + num3;
      float num6 = bounds.Left + num4;
      switch (verticalOrigin)
      {
        case VerticalOrigin.Margin:
          switch (verticalAlignment)
          {
            case ShapeVerticalAlignment.Top:
              num5 = bounds.Y;
              break;
            case ShapeVerticalAlignment.Center:
              num5 = (double) num2 > (double) pageSetup.PageSize.Height ? (float) (((double) pageSetup.PageSize.Height - (double) num2) / 2.0) + (float) (((double) pageSetup.Margins.Top - (double) pageSetup.Margins.Bottom) / 2.0) : bounds.Y + (float) (((double) bounds.Height - (double) num2) / 2.0);
              break;
            case ShapeVerticalAlignment.Bottom:
              num5 = bounds.Y + bounds.Height - num2;
              break;
          }
          break;
        case VerticalOrigin.Page:
        case VerticalOrigin.TopMargin:
          num5 = num3;
          switch (verticalAlignment)
          {
            case ShapeVerticalAlignment.Top:
              num5 = 0.0f;
              break;
            case ShapeVerticalAlignment.Center:
              num5 = (float) (((double) pageSetup.PageSize.Height - (double) num2) / 2.0);
              break;
            case ShapeVerticalAlignment.Bottom:
              num5 = pageSetup.PageSize.Height - num2;
              break;
          }
          break;
      }
      switch (horizontalOrigin)
      {
        case HorizontalOrigin.Margin:
        case HorizontalOrigin.Column:
          switch (horizontalAlignment)
          {
            case ShapeHorizontalAlignment.Left:
              num6 = bounds.Left;
              break;
            case ShapeHorizontalAlignment.Center:
              num6 = bounds.X + (float) (((double) bounds.Width - (double) num1) / 2.0);
              break;
            case ShapeHorizontalAlignment.Right:
              num6 = bounds.Left + bounds.Width - num1;
              break;
          }
          break;
        case HorizontalOrigin.Page:
          num6 = num4;
          switch (horizontalAlignment)
          {
            case ShapeHorizontalAlignment.Left:
              num6 = 0.0f;
              break;
            case ShapeHorizontalAlignment.Center:
              num6 = (float) (((double) pageSetup.PageSize.Width - (double) num1) / 2.0);
              break;
            case ShapeHorizontalAlignment.Right:
              num6 = pageSetup.PageSize.Width - num1;
              break;
          }
          break;
      }
      if ((textWrappingStyle == TextWrappingStyle.Square || textWrappingStyle == TextWrappingStyle.Through || textWrappingStyle == TextWrappingStyle.Tight) && verticalOrigin == VerticalOrigin.Paragraph && pItem.Document.Settings.CompatibilityMode != CompatibilityMode.Word2013)
      {
        if ((double) num6 < 0.0)
          num6 = 0.0f;
        if ((double) num5 < 0.0)
          num5 = 0.0f;
      }
      bounds.X = num6;
      bounds.Y = num5;
    }
    bounds.Height = num2;
    bounds.Width = num1;
    return bounds;
  }

  private void DrawTextWatermark(
    TextWatermark textWatermark,
    RectangleF bounds,
    WPageSetup pageSetup)
  {
    Font font = textWatermark.Document.FontSettings.GetFont(textWatermark.FontName, (double) textWatermark.Size == 1.0 ? this.GetFontSize(textWatermark) : textWatermark.Size, FontStyle.Regular);
    if (font.Name == "Arial Narrow" && font.Style == FontStyle.Bold)
      textWatermark.Text = textWatermark.Text.Replace(ControlChar.NonBreakingSpaceChar, ControlChar.SpaceChar);
    if ((double) textWatermark.Width == -1.0 || (double) textWatermark.Height == -1.0)
      textWatermark.SetDefaultSize();
    bounds = this.UpdateWaterMarkPosition((ParagraphItem) textWatermark, pageSetup, bounds);
    bounds = new RectangleF(bounds.X, bounds.Y, textWatermark.Width, textWatermark.Height);
    Rectangle destRect = new Rectangle((int) bounds.X, (int) bounds.Y, (int) textWatermark.Width, (int) textWatermark.Height);
    Bitmap bitmap = this.ConvertAsImage(textWatermark, font);
    ColorMatrix colorMatrix = this.CreateColorMatrix();
    if (textWatermark.Semitransparent)
      colorMatrix.Matrix33 = 0.5f;
    ImageAttributes imageAttributes = this.CreateImageAttributes();
    imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
    if (textWatermark.Rotation != 0)
    {
      bounds = new RectangleF(bounds.X, bounds.Y, bounds.Width, bounds.Height);
      this.Graphics.Transform = this.GetTransformMatrix(bounds, (float) textWatermark.Rotation);
      destRect = new Rectangle((int) bounds.X, (int) bounds.Y, (int) bounds.Width, (int) bounds.Height);
    }
    this.Graphics.DrawImage(this.GetImage((Image) bitmap), destRect, 0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel, imageAttributes);
    if (textWatermark.Rotation != 0)
      this.ResetTransform();
    bitmap.Dispose();
  }

  private Bitmap ConvertAsImage(TextWatermark textWatermark, Font font)
  {
    Graphics graphics1 = Graphics.FromImage((Image) new Bitmap(1, 1));
    Bitmap bitmap = new Bitmap((int) graphics1.MeasureString(textWatermark.Text, font).Width, (int) graphics1.MeasureString(textWatermark.Text, font).Height);
    Graphics graphics2 = Graphics.FromImage((Image) bitmap);
    graphics2.TextRenderingHint = TextRenderingHint.AntiAlias;
    graphics2.SmoothingMode = SmoothingMode.HighQuality;
    graphics2.InterpolationMode = InterpolationMode.HighQualityBicubic;
    Color color = textWatermark.Color;
    graphics2.DrawString(textWatermark.Text, font, (Brush) new SolidBrush(color), 0.0f, 0.0f);
    graphics2.Dispose();
    return bitmap;
  }

  private Color ChangeColorBrightness(Color color, float correctionFactor)
  {
    float r = (float) color.R;
    float g = (float) color.G;
    float b = (float) color.B;
    float red;
    float green;
    float blue;
    if ((double) correctionFactor < 0.0)
    {
      correctionFactor = 1f + correctionFactor;
      red = r * correctionFactor;
      green = g * correctionFactor;
      blue = b * correctionFactor;
    }
    else
    {
      red = ((float) byte.MaxValue - r) * correctionFactor + r;
      green = ((float) byte.MaxValue - g) * correctionFactor + g;
      blue = ((float) byte.MaxValue - b) * correctionFactor + b;
    }
    return Color.FromArgb((int) color.A, (int) red, (int) green, (int) blue);
  }

  internal void DrawPageBorder(
    WPageSetup pageSetup,
    RectangleF headerBounds,
    RectangleF footerBounds,
    RectangleF pageBounds)
  {
    switch (pageSetup.PageBorderOffsetFrom)
    {
      case PageBorderOffsetFrom.Text:
        PointF[] pointFArray = new PointF[2];
        if (pageSetup.Borders.Left.BorderType != BorderStyle.None)
        {
          PointF[] leftBorderPoints = this.GetLeftBorderPoints(pageSetup, headerBounds, footerBounds, pageBounds);
          this.DrawBorder(pageSetup.Borders.Left, leftBorderPoints[0], leftBorderPoints[1]);
        }
        if (pageSetup.Borders.Right.BorderType != BorderStyle.None)
        {
          PointF[] rightBorderPoints = this.GetRightBorderPoints(pageSetup, headerBounds, footerBounds, pageBounds);
          this.DrawBorder(pageSetup.Borders.Right, rightBorderPoints[0], rightBorderPoints[1]);
        }
        if (pageSetup.Borders.Top.BorderType != BorderStyle.None)
        {
          PointF[] topBorderPoints = this.GetTopBorderPoints(pageSetup, headerBounds, footerBounds, pageBounds);
          this.DrawBorder(pageSetup.Borders.Top, topBorderPoints[0], topBorderPoints[1]);
        }
        if (pageSetup.Borders.Bottom.BorderType == BorderStyle.None)
          break;
        PointF[] bottomBorderPoints = this.GetBottomBorderPoints(pageSetup, headerBounds, footerBounds, pageBounds);
        this.DrawBorder(pageSetup.Borders.Bottom, bottomBorderPoints[0], bottomBorderPoints[1]);
        break;
      case PageBorderOffsetFrom.PageEdge:
        float space1 = pageSetup.Borders.Left.Space;
        float num = pageSetup.Borders.Right.Space + pageSetup.Borders.Right.LineWidth / 2f;
        float space2 = pageSetup.Borders.Top.Space;
        float space3 = pageSetup.Borders.Bottom.Space;
        if (pageSetup.Borders.Left.BorderType != BorderStyle.None)
          this.DrawBorder(pageSetup.Borders.Left, new PointF(space1, space2), new PointF(space1, pageSetup.PageSize.Height - space3));
        if (pageSetup.Borders.Right.BorderType != BorderStyle.None)
          this.DrawBorder(pageSetup.Borders.Right, new PointF(pageSetup.PageSize.Width - num, space2), new PointF(pageSetup.PageSize.Width - num, pageSetup.PageSize.Height - space3));
        if (pageSetup.Borders.Top.BorderType != BorderStyle.None)
          this.DrawBorder(pageSetup.Borders.Top, new PointF(space1, space2), new PointF(pageSetup.PageSize.Width - num, space2));
        if (pageSetup.Borders.Bottom.BorderType == BorderStyle.None)
          break;
        this.DrawBorder(pageSetup.Borders.Bottom, new PointF(space1, pageSetup.PageSize.Height - space3), new PointF(pageSetup.PageSize.Width - num, pageSetup.PageSize.Height - space3));
        break;
    }
  }

  private PointF[] GetLeftBorderPoints(
    WPageSetup pageSetup,
    RectangleF headerBounds,
    RectangleF footerBounds,
    RectangleF pageBounds)
  {
    float space1 = pageSetup.Borders.Left.Space;
    float space2 = pageSetup.Borders.Top.Space;
    float space3 = pageSetup.Borders.Bottom.Space;
    float num = pageSetup.PageSize.Height - ((double) pageSetup.Margins.Bottom > (double) footerBounds.Height + (double) pageSetup.FooterDistance ? pageSetup.Margins.Bottom : footerBounds.Height + pageSetup.FooterDistance);
    PointF[] leftBorderPoints = new PointF[2];
    if (pageSetup.Document.BordersSurroundHeader && pageSetup.Document.BordersSurroundFooter)
    {
      leftBorderPoints[0] = new PointF(headerBounds.X - space1, (double) headerBounds.Height == 0.0 ? pageBounds.Y - space2 : headerBounds.Y - space2);
      leftBorderPoints[1] = new PointF(headerBounds.X - space1, footerBounds.Bottom + space3);
    }
    else if (pageSetup.Document.BordersSurroundHeader)
    {
      leftBorderPoints[0] = new PointF(headerBounds.X - space1, (double) headerBounds.Height == 0.0 ? pageBounds.Y - space2 : headerBounds.Y - space2);
      leftBorderPoints[1] = new PointF(headerBounds.X - space1, num + space3);
    }
    else if (pageSetup.Document.BordersSurroundFooter)
    {
      leftBorderPoints[0] = new PointF(headerBounds.X - space1, pageBounds.Y - space2);
      leftBorderPoints[1] = new PointF(headerBounds.X - space1, footerBounds.Bottom + space3);
    }
    else
    {
      leftBorderPoints[0] = new PointF(headerBounds.X - space1, pageBounds.Y - space2);
      leftBorderPoints[1] = new PointF(headerBounds.X - space1, num + space3);
    }
    return leftBorderPoints;
  }

  private PointF[] GetRightBorderPoints(
    WPageSetup pageSetup,
    RectangleF headerBounds,
    RectangleF footerBounds,
    RectangleF pageBounds)
  {
    float space1 = pageSetup.Borders.Right.Space;
    float space2 = pageSetup.Borders.Top.Space;
    float space3 = pageSetup.Borders.Bottom.Space;
    float num = pageSetup.PageSize.Height - ((double) pageSetup.Margins.Bottom > (double) footerBounds.Height + (double) pageSetup.FooterDistance ? pageSetup.Margins.Bottom : footerBounds.Height + pageSetup.FooterDistance);
    PointF[] rightBorderPoints = new PointF[2];
    if (pageSetup.Document.BordersSurroundHeader && pageSetup.Document.BordersSurroundFooter)
    {
      rightBorderPoints[0] = new PointF(pageSetup.ClientWidth + headerBounds.X + space1, (double) headerBounds.Height == 0.0 ? pageBounds.Y - space2 : headerBounds.Y - space2);
      rightBorderPoints[1] = new PointF(pageSetup.ClientWidth + headerBounds.X + space1, footerBounds.Bottom + space3);
    }
    else if (pageSetup.Document.BordersSurroundHeader)
    {
      rightBorderPoints[0] = new PointF(pageSetup.ClientWidth + headerBounds.X + space1, (double) headerBounds.Height == 0.0 ? pageBounds.Y - space2 : headerBounds.Y - space2);
      rightBorderPoints[1] = new PointF(pageSetup.ClientWidth + headerBounds.X + space1, num + space3);
    }
    else if (pageSetup.Document.BordersSurroundFooter)
    {
      rightBorderPoints[0] = new PointF(pageSetup.ClientWidth + headerBounds.X + space1, pageBounds.Y - space2);
      rightBorderPoints[1] = new PointF(pageSetup.ClientWidth + headerBounds.X + space1, footerBounds.Bottom + space3);
    }
    else
    {
      rightBorderPoints[0] = new PointF(pageSetup.ClientWidth + headerBounds.X + space1, pageBounds.Y - space2);
      rightBorderPoints[1] = new PointF(pageSetup.ClientWidth + headerBounds.X + space1, num + space3);
    }
    return rightBorderPoints;
  }

  private PointF[] GetBottomBorderPoints(
    WPageSetup pageSetup,
    RectangleF headerBounds,
    RectangleF footerBounds,
    RectangleF pageBounds)
  {
    float space1 = pageSetup.Borders.Left.Space;
    float space2 = pageSetup.Borders.Right.Space;
    float space3 = pageSetup.Borders.Bottom.Space;
    float num = pageSetup.PageSize.Height - ((double) pageSetup.Margins.Bottom > (double) footerBounds.Height + (double) pageSetup.FooterDistance ? pageSetup.Margins.Bottom : footerBounds.Height + pageSetup.FooterDistance);
    PointF[] bottomBorderPoints = new PointF[2];
    if (pageSetup.Document.BordersSurroundHeader && pageSetup.Document.BordersSurroundFooter)
    {
      bottomBorderPoints[0] = new PointF(headerBounds.X - space1, footerBounds.Bottom + space3);
      bottomBorderPoints[1] = new PointF(pageSetup.ClientWidth + headerBounds.X + space2, footerBounds.Bottom + space3);
    }
    else if (pageSetup.Document.BordersSurroundHeader)
    {
      bottomBorderPoints[0] = new PointF(headerBounds.X - space1, num + space3);
      bottomBorderPoints[1] = new PointF(pageSetup.ClientWidth + headerBounds.X + space2, num + space3);
    }
    else if (pageSetup.Document.BordersSurroundFooter)
    {
      bottomBorderPoints[0] = new PointF(headerBounds.X - space1, footerBounds.Bottom + space3);
      bottomBorderPoints[1] = new PointF(pageSetup.ClientWidth + headerBounds.X + space2, footerBounds.Bottom + space3);
    }
    else
    {
      bottomBorderPoints[0] = new PointF(headerBounds.X - space1, num + space3);
      bottomBorderPoints[1] = new PointF(pageSetup.ClientWidth + headerBounds.X + space2, num + space3);
    }
    return bottomBorderPoints;
  }

  private PointF[] GetTopBorderPoints(
    WPageSetup pageSetup,
    RectangleF headerBounds,
    RectangleF footerBounds,
    RectangleF pageBounds)
  {
    float space1 = pageSetup.Borders.Left.Space;
    float space2 = pageSetup.Borders.Right.Space;
    float space3 = pageSetup.Borders.Top.Space;
    PointF[] topBorderPoints = new PointF[2];
    topBorderPoints[0] = new PointF(headerBounds.X - space1, pageBounds.Y - space3);
    if (pageSetup.Document.BordersSurroundHeader)
    {
      topBorderPoints[0] = new PointF(headerBounds.X - space1, (double) headerBounds.Height == 0.0 ? pageBounds.Y - space3 : headerBounds.Y - space3);
      topBorderPoints[1] = new PointF(pageSetup.ClientWidth + headerBounds.X + space2, (double) headerBounds.Height == 0.0 ? pageBounds.Y - space3 : headerBounds.Y - space3);
    }
    else
    {
      topBorderPoints[0] = new PointF(headerBounds.X - space1, pageBounds.Y - space3);
      topBorderPoints[1] = new PointF(pageSetup.ClientWidth + headerBounds.X + space2, pageBounds.Y - space3);
    }
    return topBorderPoints;
  }

  private float GetFontSize(TextWatermark textWatermark)
  {
    float fontSize1 = 8f;
    int num = 0;
    float fontSize2 = 8f;
    float width = textWatermark.Width;
    Graphics graphics = Graphics.FromImage((Image) new Bitmap(1, 1));
    while ((double) num <= (double) width)
    {
      Font font = textWatermark.Document.FontSettings.GetFont(textWatermark.FontName, fontSize2, FontStyle.Regular);
      if (font.Name == "Arial Narrow" && font.Style == FontStyle.Bold)
        textWatermark.Text = textWatermark.Text.Replace(ControlChar.NonBreakingSpaceChar, ControlChar.SpaceChar);
      num = (int) graphics.MeasureString(textWatermark.Text, font).Width;
      if ((double) num <= (double) width)
      {
        fontSize1 = fontSize2;
        ++fontSize2;
      }
    }
    return fontSize1;
  }

  private void DrawImageWatermark(
    PictureWatermark pictureWatermark,
    RectangleF bounds,
    WPageSetup pageSetup)
  {
    Bitmap bitmap = new Bitmap(pictureWatermark.Picture);
    bounds = this.UpdateWaterMarkPosition((ParagraphItem) pictureWatermark.WordPicture, pageSetup, bounds);
    if (pictureWatermark.Washout)
    {
      float num1 = 0.86f;
      float num2 = 0.3f;
      float gamma = 1f;
      float[][] newColorMatrix = new float[5][]
      {
        new float[5]{ num2, 0.0f, 0.0f, 0.0f, 0.0f },
        new float[5]{ 0.0f, num2, 0.0f, 0.0f, 0.0f },
        new float[5]{ 0.0f, 0.0f, num2, 0.0f, 0.0f },
        new float[5]{ 0.0f, 0.0f, 0.0f, 1f, 0.0f },
        new float[5]{ num1, num1, num1, 1f, 1f }
      };
      ImageAttributes imageAttributes = this.CreateImageAttributes();
      imageAttributes.SetColorMatrix(this.CreateColorMatrix(newColorMatrix), ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
      imageAttributes.SetGamma(gamma, ColorAdjustType.Bitmap);
      this.Graphics.DrawImage(this.GetImage((Image) bitmap), new Rectangle((int) bounds.X, (int) bounds.Y, (int) bounds.Width, (int) bounds.Height), 0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel, imageAttributes);
    }
    else
      this.Graphics.DrawImage(pictureWatermark.Picture, new Rectangle((int) bounds.X, (int) bounds.Y, (int) bounds.Width, (int) bounds.Height));
  }

  internal void DrawCheckBox(WCheckBox checkbox, LayoutedWidget ltWidget)
  {
    Pen pen = this.CreatePen(this.GetTextColor(checkbox.CharacterFormat));
    pen.Width = 0.7f;
    RectangleF rectangleF = new RectangleF(ltWidget.Bounds.X + pen.Width, ltWidget.Bounds.Y + pen.Width, ltWidget.Bounds.Width - 2f * pen.Width, ltWidget.Bounds.Height - 2f * pen.Width);
    this.Graphics.DrawRectangle(pen, rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);
    if (!checkbox.Checked)
      return;
    PointF pt1 = new PointF(rectangleF.X, rectangleF.Y);
    PointF pt2 = new PointF(rectangleF.Right, rectangleF.Bottom);
    this.Graphics.DrawLine(pen, pt1, pt2);
    pt1 = new PointF(rectangleF.Right, rectangleF.Top);
    pt2 = new PointF(rectangleF.Left, rectangleF.Bottom);
    this.Graphics.DrawLine(pen, pt1, pt2);
  }

  internal void DrawShape(Shape shape, LayoutedWidget ltWidget)
  {
    RectangleF rectangleF1 = ltWidget.Bounds;
    this.ResetTransform();
    Pen pen = this.CreatePen(!shape.LineFormat.Color.IsEmpty ? shape.LineFormat.Color : (!shape.LineFormat.ForeColor.IsEmpty ? shape.LineFormat.ForeColor : Color.Black));
    if (shape.AutoShapeType != AutoShapeType.StraightConnector && shape.AutoShapeType != AutoShapeType.ElbowConnector)
      pen.Alignment = PenAlignment.Inset;
    this.GetDashStyle(shape.LineFormat.DashStyle, pen);
    pen.Width = shape.LineFormat.Weight;
    if (shape.Owner is GroupShape || shape.Owner is ChildGroupShape)
    {
      RectangleF bounds = ltWidget.Bounds;
      RectangleF rectangleF2 = (double) shape.Rotation == 0.0 ? this.UpdateClipBounds(bounds, false) : this.UpdateClipBounds(bounds);
      this.ClipBoundsContainer.Push(ltWidget.Bounds);
    }
    else if ((double) shape.Rotation != 0.0)
    {
      RectangleF boundingBoxCoordinates = this.GetBoundingBoxCoordinates(new RectangleF(0.0f, 0.0f, shape.Width, shape.Height), shape.Rotation);
      rectangleF1 = new RectangleF(rectangleF1.X - boundingBoxCoordinates.X, rectangleF1.Y - boundingBoxCoordinates.Y, shape.Width, shape.Height);
      ltWidget.Bounds = rectangleF1;
      if (!shape.TextFrame.Upright)
      {
        this.ClipBoundsContainer.Pop();
        this.ClipBoundsContainer.Push(ltWidget.Bounds);
      }
    }
    float rotation = shape.Rotation;
    bool flipH = shape.FlipHorizontal;
    bool flipV = shape.FlipVertical;
    if (((double) rotation != 0.0 || (double) rotation == 0.0 && (flipH || flipV)) && shape.WrapFormat.TextWrappingStyle != TextWrappingStyle.Tight && shape.WrapFormat.TextWrappingStyle != TextWrappingStyle.Through)
    {
      if ((double) rotation > 360.0)
        rotation %= 360f;
      if (shape.AutoShapeType == AutoShapeType.Line || shape.AutoShapeType == AutoShapeType.StraightConnector)
      {
        flipH = false;
        flipV = false;
      }
      if ((double) rotation != 0.0 || flipV || flipH)
        this.Graphics.Transform = this.GetTransformMatrix(rectangleF1, rotation, flipH, flipV);
    }
    GraphicsPath graphicsPath = this.GetGraphicsPath(shape, rectangleF1, ref pen);
    if (!shape.IsHorizontalRule && graphicsPath.PointCount == 0 && (double) rectangleF1.Width <= 0.0 ^ (double) rectangleF1.Height <= 0.0)
    {
      if ((double) rectangleF1.Width <= 0.0)
        rectangleF1.Width = 0.1f;
      else if ((double) rectangleF1.Height <= 0.0)
        rectangleF1.Height = 0.1f;
      graphicsPath.AddRectangle(rectangleF1);
    }
    if (graphicsPath.PointCount > 0)
    {
      if (shape.FillFormat.Fill && this.IsShapeNeedToBeFill(shape.AutoShapeType))
      {
        Color color = shape.FillFormat.Color.IsEmpty || (double) shape.FillFormat.Transparency == 100.0 ? (!shape.FillFormat.ForeColor.IsEmpty ? shape.FillFormat.ForeColor : Color.Empty) : shape.FillFormat.Color;
        if (shape.FillFormat.FillType == FillType.FillGradient && shape.FillFormat.GradientFill != null && shape.FillFormat.GradientFill.GradientStops.Count > 0)
          color = shape.FillFormat.GradientFill.GradientStops[shape.FillFormat.GradientFill.GradientStops.Count - 1].Color;
        if (color != Color.Empty)
          this.Graphics.FillPath((Brush) this.CreateSolidBrush(color), graphicsPath);
      }
      if (shape.LineFormat.Line)
      {
        pen.LineJoin = this.GetLineJoin(shape.LineFormat.LineJoin);
        if (shape.LineFormat.Style == LineStyle.ThinThin && (shape.AutoShapeType == AutoShapeType.Line || shape.AutoShapeType == AutoShapeType.StraightConnector) && !shape.FlipHorizontal && !shape.FlipVertical && !this.IsArrowPreserved(shape))
          this.DrawLineShapeBasedOnLineType(shape, rectangleF1, pen);
        else
          this.Graphics.DrawPath(pen, graphicsPath);
      }
    }
    this.ResetTransform();
    if ((double) shape.Rotation == 0.0 || !shape.TextFrame.Upright)
      return;
    shape.TextLayoutingBounds = this.GetBoundingBoxCoordinates(ltWidget.Bounds, shape.Rotation);
  }

  private void DrawLineShapeBasedOnLineType(Shape shape, RectangleF bounds, Pen pen)
  {
    pen.Width /= 3f;
    GraphicsPath graphicsPath1 = this.CreateGraphicsPath();
    PointF[] pointsBasedOnFlip = this.GetLinePointsBasedOnFlip(shape.FlipHorizontal, shape.FlipVertical, bounds);
    graphicsPath1.AddLines(pointsBasedOnFlip);
    this.Graphics.DrawPath(pen, graphicsPath1);
    PointF pointF1 = pointsBasedOnFlip[0];
    PointF pointF2 = pointsBasedOnFlip[1];
    pointF1.X += pen.Width * 2f;
    pointF2.X += pen.Width * 2f;
    pointsBasedOnFlip[0] = pointF1;
    pointsBasedOnFlip[1] = pointF2;
    GraphicsPath graphicsPath2 = this.CreateGraphicsPath();
    graphicsPath2.AddLines(pointsBasedOnFlip);
    this.Graphics.DrawPath(pen, graphicsPath2);
  }

  private void DrawLineShapeBasedOnLineType(ChildShape shape, RectangleF bounds, Pen pen)
  {
    pen.Width /= 3f;
    GraphicsPath graphicsPath1 = this.CreateGraphicsPath();
    PointF[] pointsBasedOnFlip = this.GetLinePointsBasedOnFlip(shape.FlipHorizantal, shape.FlipVertical, bounds);
    graphicsPath1.AddLines(pointsBasedOnFlip);
    this.Graphics.DrawPath(pen, graphicsPath1);
    PointF pointF1 = pointsBasedOnFlip[0];
    PointF pointF2 = pointsBasedOnFlip[1];
    pointF1.X += pen.Width * 2f;
    pointF2.X += pen.Width * 2f;
    pointsBasedOnFlip[0] = pointF1;
    pointsBasedOnFlip[1] = pointF2;
    GraphicsPath graphicsPath2 = this.CreateGraphicsPath();
    graphicsPath2.AddLines(pointsBasedOnFlip);
    this.Graphics.DrawPath(pen, graphicsPath2);
  }

  private bool IsArrowPreserved(Shape shape)
  {
    switch (shape.LineFormat.EndArrowheadStyle)
    {
      case ArrowheadStyle.ArrowheadTriangle:
      case ArrowheadStyle.ArrowheadOpen:
        return true;
      default:
        switch (shape.LineFormat.BeginArrowheadStyle)
        {
          case ArrowheadStyle.ArrowheadTriangle:
          case ArrowheadStyle.ArrowheadOpen:
            return true;
          default:
            return false;
        }
    }
  }

  private bool IsArrowPreserved(ChildShape shape)
  {
    switch (shape.LineFormat.EndArrowheadStyle)
    {
      case ArrowheadStyle.ArrowheadTriangle:
      case ArrowheadStyle.ArrowheadOpen:
        return true;
      default:
        switch (shape.LineFormat.BeginArrowheadStyle)
        {
          case ArrowheadStyle.ArrowheadTriangle:
          case ArrowheadStyle.ArrowheadOpen:
            return true;
          default:
            return false;
        }
    }
  }

  internal void DrawChildShape(ChildShape childShape, LayoutedWidget ltWidget)
  {
    switch (childShape.ElementType)
    {
      case EntityType.Picture:
        WPicture picture = childShape.GetOwnerGroupShape().ConvertChildShapeToPicture(childShape);
        picture.SetOwner((Syncfusion.DocIO.DLS.OwnerHolder) childShape.Owner);
        picture.FlipHorizontal = childShape.FlipHorizantalToRender;
        picture.FlipVertical = childShape.FlipVerticalToRender;
        picture.Rotation = childShape.RotationToRender;
        this.DrawPicture(picture, ltWidget);
        break;
      case EntityType.TextBox:
        WTextBox textbox = childShape.GetOwnerGroupShape().ConvertChildShapeToTextbox(childShape);
        textbox.SetOwner((Syncfusion.DocIO.DLS.OwnerHolder) childShape.Owner);
        textbox.TextBoxFormat.FlipHorizontal = childShape.FlipHorizantalToRender;
        textbox.TextBoxFormat.FlipVertical = childShape.FlipVerticalToRender;
        textbox.TextBoxFormat.Rotation = childShape.RotationToRender;
        this.DrawTextBox(textbox, ltWidget);
        break;
      case EntityType.Chart:
        if (childShape.Chart.Document.ChartToImageConverter == null)
          break;
        childShape.Chart.InitializeOfficeChartToImageConverter();
        childShape.Chart.Width = childShape.Width;
        childShape.Chart.Height = childShape.Height;
        this.DrawChart(childShape.Chart, ltWidget);
        break;
      case EntityType.AutoShape:
        Shape shape = childShape.GetOwnerGroupShape().ConvertChildShapeToShape(childShape);
        shape.SetOwner((Syncfusion.DocIO.DLS.OwnerHolder) childShape.Owner);
        shape.FlipHorizontal = childShape.FlipHorizantalToRender;
        shape.FlipVertical = childShape.FlipVerticalToRender;
        shape.Rotation = childShape.RotationToRender;
        this.DrawShape(shape, ltWidget);
        break;
    }
  }

  private bool IsShapeNeedToBeFill(AutoShapeType shapeType)
  {
    switch (shapeType)
    {
      case AutoShapeType.Line:
      case AutoShapeType.ElbowConnector:
      case AutoShapeType.CurvedConnector:
      case AutoShapeType.StraightConnector:
      case AutoShapeType.BentConnector2:
      case AutoShapeType.BentConnector4:
      case AutoShapeType.BentConnector5:
      case AutoShapeType.CurvedConnector2:
      case AutoShapeType.CurvedConnector4:
      case AutoShapeType.CurvedConnector5:
        return false;
      default:
        return true;
    }
  }

  private void Rotate(
    ParagraphItem shapeFrame,
    float rotation,
    bool flipV,
    bool flipH,
    RectangleF rect)
  {
    float ang = rotation;
    for (ParagraphItem paragraphItem = shapeFrame; paragraphItem.Owner is GroupShape || paragraphItem.Owner is ChildGroupShape; paragraphItem = paragraphItem.Owner as ParagraphItem)
    {
      float num = paragraphItem.Owner is GroupShape ? (paragraphItem.Owner as GroupShape).Rotation : (paragraphItem.Owner as ChildGroupShape).Rotation;
      ang += num;
    }
    if ((double) ang > 360.0)
      ang %= 360f;
    if ((double) ang <= 0.0 && !flipV && !flipH)
      return;
    this.Graphics.Transform = this.GetTransformMatrix(rect, ang, flipV, flipH);
  }

  private Matrix GetTransformMatrix(RectangleF bounds, float ang, bool flipH, bool flipV)
  {
    Matrix matrix = new Matrix();
    Matrix target1 = new Matrix(1f, 0.0f, 0.0f, -1f, 0.0f, 0.0f);
    Matrix target2 = new Matrix(-1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
    PointF point = new PointF(bounds.X + bounds.Width / 2f, bounds.Y + bounds.Height / 2f);
    if (flipV)
    {
      this.MatrixMultiply(matrix, target1, MatrixOrder.Append);
      this.MatrixTranslate(matrix, 0.0f, point.Y * 2f, MatrixOrder.Append);
    }
    if (flipH)
    {
      this.MatrixMultiply(matrix, target2, MatrixOrder.Append);
      this.MatrixTranslate(matrix, point.X * 2f, 0.0f, MatrixOrder.Append);
    }
    this.MatrixRotate(matrix, ang, point, MatrixOrder.Append);
    return matrix;
  }

  private GraphicsPath GetGraphicsPath(Shape shape, RectangleF bounds, ref Pen pen)
  {
    ShapePath shapePath = new ShapePath(bounds, shape.ShapeGuide);
    GraphicsPath graphicsPath = this.CreateGraphicsPath();
    switch (shape.AutoShapeType)
    {
      case AutoShapeType.Rectangle:
      case AutoShapeType.FlowChartProcess:
        graphicsPath.AddRectangle(bounds);
        return graphicsPath;
      case AutoShapeType.Parallelogram:
      case AutoShapeType.FlowChartData:
        return shapePath.GetParallelogramPath();
      case AutoShapeType.Trapezoid:
        return shape.Is2007Shape ? shapePath.GetFlowChartManualOperationPath() : shapePath.GetTrapezoidPath();
      case AutoShapeType.Diamond:
      case AutoShapeType.FlowChartDecision:
        PointF[] points1 = new PointF[4]
        {
          new PointF(bounds.X, bounds.Y + bounds.Height / 2f),
          new PointF(bounds.X + bounds.Width / 2f, bounds.Y),
          new PointF(bounds.Right, bounds.Y + bounds.Height / 2f),
          new PointF(bounds.X + bounds.Width / 2f, bounds.Bottom)
        };
        graphicsPath.AddLines(points1);
        graphicsPath.CloseFigure();
        break;
      case AutoShapeType.RoundedRectangle:
        return shapePath.GetRoundedRectanglePath();
      case AutoShapeType.Octagon:
        return shapePath.GetOctagonPath();
      case AutoShapeType.IsoscelesTriangle:
        return shapePath.GetTrianglePath();
      case AutoShapeType.RightTriangle:
        PointF[] points2 = new PointF[3]
        {
          new PointF(bounds.X, bounds.Bottom),
          new PointF(bounds.X, bounds.Y),
          new PointF(bounds.Right, bounds.Bottom)
        };
        graphicsPath.AddLines(points2);
        graphicsPath.CloseFigure();
        return graphicsPath;
      case AutoShapeType.Oval:
        graphicsPath.AddEllipse(bounds);
        return graphicsPath;
      case AutoShapeType.Hexagon:
        return shapePath.GetHexagonPath();
      case AutoShapeType.Cross:
        return shapePath.GetCrossPath();
      case AutoShapeType.RegularPentagon:
        return shapePath.GetRegularPentagonPath();
      case AutoShapeType.Can:
        return shapePath.GetCanPath();
      case AutoShapeType.Cube:
        return shapePath.GetCubePath();
      case AutoShapeType.Bevel:
        return shapePath.GetBevelPath();
      case AutoShapeType.FoldedCorner:
        return shapePath.GetFoldedCornerPath();
      case AutoShapeType.SmileyFace:
        GraphicsPath[] smileyFacePath = shapePath.GetSmileyFacePath();
        int num = 0;
        Color color1 = !shape.FillFormat.Color.IsEmpty ? shape.FillFormat.Color : (!shape.FillFormat.ForeColor.IsEmpty ? shape.FillFormat.ForeColor : Color.Empty);
        if (shape.FillFormat.FillType == FillType.FillGradient && shape.FillFormat.GradientFill != null && shape.FillFormat.GradientFill.GradientStops.Count > 0)
          color1 = shape.FillFormat.GradientFill.GradientStops[shape.FillFormat.GradientFill.GradientStops.Count - 1].Color;
        foreach (GraphicsPath path in smileyFacePath)
        {
          if (num == 2)
            color1 = this.ChangeColorBrightness(color1, -0.2f);
          ++num;
          if (color1 != Color.Empty)
            this.Graphics.FillPath((Brush) this.CreateSolidBrush(color1), path);
          this.Graphics.DrawPath(pen, path);
        }
        break;
      case AutoShapeType.Donut:
        return shapePath.GetDonutPath(0.0);
      case AutoShapeType.NoSymbol:
        return shapePath.GetNoSymbolPath();
      case AutoShapeType.BlockArc:
        return shapePath.GetBlockArcPath();
      case AutoShapeType.Heart:
        return shapePath.GetHeartPath();
      case AutoShapeType.LightningBolt:
        return shapePath.GetLightningBoltPath();
      case AutoShapeType.Sun:
        return shapePath.GetSunPath();
      case AutoShapeType.Moon:
        return shapePath.GetMoonPath();
      case AutoShapeType.Arc:
        GraphicsPath[] arcPath = shapePath.GetArcPath();
        Color color2 = !shape.FillFormat.Color.IsEmpty ? shape.FillFormat.Color : (!shape.FillFormat.ForeColor.IsEmpty ? shape.FillFormat.ForeColor : Color.Empty);
        if (shape.FillFormat.FillType == FillType.FillGradient && shape.FillFormat.GradientFill != null && shape.FillFormat.GradientFill.GradientStops.Count > 0)
          color2 = shape.FillFormat.GradientFill.GradientStops[shape.FillFormat.GradientFill.GradientStops.Count - 1].Color;
        if (color2 != Color.Empty && shape.FillFormat.Fill && !shape.FillFormat.IsDefaultFill)
          this.Graphics.FillPath((Brush) this.CreateSolidBrush(color2), arcPath[1]);
        if (shape.LineFormat.Line)
        {
          this.Graphics.DrawPath(pen, arcPath[0]);
          break;
        }
        break;
      case AutoShapeType.DoubleBracket:
        return shapePath.GetDoubleBracketPath();
      case AutoShapeType.DoubleBrace:
        return shapePath.GetDoubleBracePath();
      case AutoShapeType.Plaque:
        return shapePath.GetPlaquePath();
      case AutoShapeType.LeftBracket:
        return shapePath.GetLeftBracketPath();
      case AutoShapeType.RightBracket:
        return shapePath.GetRightBracketPath();
      case AutoShapeType.LeftBrace:
        return shapePath.GetLeftBracePath();
      case AutoShapeType.RightBrace:
        return shapePath.GetRightBracePath();
      case AutoShapeType.RightArrow:
        return shapePath.GetRightArrowPath();
      case AutoShapeType.LeftArrow:
        return shapePath.GetLeftArrowPath();
      case AutoShapeType.UpArrow:
        return shapePath.GetUpArrowPath();
      case AutoShapeType.DownArrow:
        return shapePath.GetDownArrowPath();
      case AutoShapeType.LeftRightArrow:
        return shapePath.GetLeftRightArrowPath();
      case AutoShapeType.UpDownArrow:
        return shapePath.GetUpDownArrowPath();
      case AutoShapeType.QuadArrow:
        return shapePath.GetQuadArrowPath();
      case AutoShapeType.LeftRightUpArrow:
        return shapePath.GetLeftRightUpArrowPath();
      case AutoShapeType.BentArrow:
        return shapePath.GetBentArrowPath();
      case AutoShapeType.UTurnArrow:
        return shapePath.GetUTrunArrowPath();
      case AutoShapeType.LeftUpArrow:
        return shapePath.GetLeftUpArrowPath();
      case AutoShapeType.BentUpArrow:
        return shapePath.GetBentUpArrowPath();
      case AutoShapeType.CurvedRightArrow:
        return shapePath.GetCurvedRightArrowPath();
      case AutoShapeType.CurvedLeftArrow:
        return shapePath.GetCurvedLeftArrowPath();
      case AutoShapeType.CurvedUpArrow:
        return shapePath.GetCurvedUpArrowPath();
      case AutoShapeType.CurvedDownArrow:
        return shapePath.GetCurvedDownArrowPath();
      case AutoShapeType.StripedRightArrow:
        return shapePath.GetStripedRightArrowPath();
      case AutoShapeType.NotchedRightArrow:
        return shapePath.GetNotchedRightArrowPath();
      case AutoShapeType.Pentagon:
        return shapePath.GetPentagonPath();
      case AutoShapeType.Chevron:
        return shapePath.GetChevronPath();
      case AutoShapeType.RightArrowCallout:
        return shapePath.GetRightArrowCalloutPath();
      case AutoShapeType.LeftArrowCallout:
        return shapePath.GetLeftArrowCalloutPath();
      case AutoShapeType.UpArrowCallout:
        return shapePath.GetUpArrowCalloutPath();
      case AutoShapeType.DownArrowCallout:
        return shapePath.GetDownArrowCalloutPath();
      case AutoShapeType.LeftRightArrowCallout:
        return shapePath.GetLeftRightArrowCalloutPath();
      case AutoShapeType.QuadArrowCallout:
        return shapePath.GetQuadArrowCalloutPath();
      case AutoShapeType.CircularArrow:
        return shapePath.GetCircularArrowPath();
      case AutoShapeType.FlowChartAlternateProcess:
        return shapePath.GetFlowChartAlternateProcessPath();
      case AutoShapeType.FlowChartPredefinedProcess:
        return shapePath.GetFlowChartPredefinedProcessPath();
      case AutoShapeType.FlowChartInternalStorage:
        return shapePath.GetFlowChartInternalStoragePath();
      case AutoShapeType.FlowChartDocument:
        return shapePath.GetFlowChartDocumentPath();
      case AutoShapeType.FlowChartMultiDocument:
        return shapePath.GetFlowChartMultiDocumentPath();
      case AutoShapeType.FlowChartTerminator:
        return shapePath.GetFlowChartTerminatorPath();
      case AutoShapeType.FlowChartPreparation:
        return shapePath.GetFlowChartPreparationPath();
      case AutoShapeType.FlowChartManualInput:
        return shapePath.GetFlowChartManualInputPath();
      case AutoShapeType.FlowChartManualOperation:
        return shapePath.GetFlowChartManualOperationPath();
      case AutoShapeType.FlowChartConnector:
        return shapePath.GetFlowChartConnectorPath();
      case AutoShapeType.FlowChartOffPageConnector:
        return shapePath.GetFlowChartOffPageConnectorPath();
      case AutoShapeType.FlowChartCard:
        return shapePath.GetFlowChartCardPath();
      case AutoShapeType.FlowChartPunchedTape:
        return shapePath.GetFlowChartPunchedTapePath();
      case AutoShapeType.FlowChartSummingJunction:
        return shapePath.GetFlowChartSummingJunctionPath();
      case AutoShapeType.FlowChartOr:
        return shapePath.GetFlowChartOrPath();
      case AutoShapeType.FlowChartCollate:
        return shapePath.GetFlowChartCollatePath();
      case AutoShapeType.FlowChartSort:
        return shapePath.GetFlowChartSortPath();
      case AutoShapeType.FlowChartExtract:
        return shapePath.GetFlowChartExtractPath();
      case AutoShapeType.FlowChartMerge:
        return shapePath.GetFlowChartMergePath();
      case AutoShapeType.FlowChartStoredData:
        return shapePath.GetFlowChartOnlineStoragePath();
      case AutoShapeType.FlowChartDelay:
        return shapePath.GetFlowChartDelayPath();
      case AutoShapeType.FlowChartSequentialAccessStorage:
        return shapePath.GetFlowChartSequentialAccessStoragePath();
      case AutoShapeType.FlowChartMagneticDisk:
        return shapePath.GetFlowChartMagneticDiskPath();
      case AutoShapeType.FlowChartDirectAccessStorage:
        return shapePath.GetFlowChartDirectAccessStoragePath();
      case AutoShapeType.FlowChartDisplay:
        return shapePath.GetFlowChartDisplayPath();
      case AutoShapeType.Explosion1:
        return shapePath.GetExplosion1();
      case AutoShapeType.Explosion2:
        return shapePath.GetExplosion2();
      case AutoShapeType.Star4Point:
        return shapePath.GetStar4Point();
      case AutoShapeType.Star5Point:
        return shapePath.GetStar5Point();
      case AutoShapeType.Star8Point:
        return shapePath.GetStar8Point();
      case AutoShapeType.Star16Point:
        return shapePath.GetStar16Point();
      case AutoShapeType.Star24Point:
        return shapePath.GetStar24Point();
      case AutoShapeType.Star32Point:
        return shapePath.GetStar32Point();
      case AutoShapeType.UpRibbon:
        return shapePath.GetUpRibbon();
      case AutoShapeType.DownRibbon:
        return shapePath.GetDownRibbon();
      case AutoShapeType.CurvedUpRibbon:
        return shapePath.GetCurvedUpRibbon();
      case AutoShapeType.CurvedDownRibbon:
        return shapePath.GetCurvedDownRibbon();
      case AutoShapeType.VerticalScroll:
        return shapePath.GetVerticalScroll();
      case AutoShapeType.HorizontalScroll:
        GraphicsPath[] horizontalScroll = shapePath.GetHorizontalScroll();
        Color color3 = !shape.FillFormat.Color.IsEmpty ? shape.FillFormat.Color : (!shape.FillFormat.ForeColor.IsEmpty ? shape.FillFormat.ForeColor : Color.Empty);
        if (shape.FillFormat.FillType == FillType.FillGradient && shape.FillFormat.GradientFill != null && shape.FillFormat.GradientFill.GradientStops.Count > 0)
          color3 = shape.FillFormat.GradientFill.GradientStops[shape.FillFormat.GradientFill.GradientStops.Count - 1].Color;
        foreach (GraphicsPath path in horizontalScroll)
        {
          if (color3 != Color.Empty)
            this.Graphics.FillPath((Brush) this.CreateSolidBrush(color3), path);
          this.Graphics.DrawPath(pen, path);
        }
        break;
      case AutoShapeType.Wave:
        return shapePath.GetWave();
      case AutoShapeType.DoubleWave:
        return shapePath.GetDoubleWave();
      case AutoShapeType.RectangularCallout:
        return shapePath.GetRectangularCalloutPath();
      case AutoShapeType.RoundedRectangularCallout:
        return shapePath.GetRoundedRectangularCalloutPath();
      case AutoShapeType.OvalCallout:
        return shapePath.GetOvalCalloutPath();
      case AutoShapeType.CloudCallout:
        return shapePath.GetCloudCalloutPath();
      case AutoShapeType.LineCallout1:
      case AutoShapeType.LineCallout1NoBorder:
        return shapePath.GetLineCallout1Path();
      case AutoShapeType.LineCallout2:
      case AutoShapeType.LineCallout2NoBorder:
        return shapePath.GetLineCallout2Path();
      case AutoShapeType.LineCallout3:
      case AutoShapeType.LineCallout3NoBorder:
        return shapePath.GetLineCallout3Path();
      case AutoShapeType.LineCallout1AccentBar:
      case AutoShapeType.LineCallout1BorderAndAccentBar:
        return shapePath.GetLineCallout1AccentBarPath();
      case AutoShapeType.LineCallout2AccentBar:
      case AutoShapeType.LineCallout2BorderAndAccentBar:
        return shapePath.GetLineCallout2AccentBarPath();
      case AutoShapeType.LineCallout3AccentBar:
      case AutoShapeType.LineCallout3BorderAndAccentBar:
        return shapePath.GetLineCallout3AccentBarPath();
      case AutoShapeType.DiagonalStripe:
        return shapePath.GetDiagonalStripePath();
      case AutoShapeType.Pie:
        return shapePath.GetPiePath();
      case AutoShapeType.Decagon:
        return shapePath.GetDecagonPath();
      case AutoShapeType.Heptagon:
        return shapePath.GetHeptagonPath();
      case AutoShapeType.Dodecagon:
        return shapePath.GetDodecagonPath();
      case AutoShapeType.Star6Point:
        return shapePath.GetStar6Point();
      case AutoShapeType.Star7Point:
        return shapePath.GetStar7Point();
      case AutoShapeType.Star10Point:
        return shapePath.GetStar10Point();
      case AutoShapeType.Star12Point:
        return shapePath.GetStar12Point();
      case AutoShapeType.RoundSingleCornerRectangle:
        return shapePath.GetRoundSingleCornerRectanglePath();
      case AutoShapeType.RoundSameSideCornerRectangle:
        return shapePath.GetRoundSameSideCornerRectanglePath();
      case AutoShapeType.RoundDiagonalCornerRectangle:
        return shapePath.GetRoundDiagonalCornerRectanglePath();
      case AutoShapeType.SnipAndRoundSingleCornerRectangle:
        return shapePath.GetSnipAndRoundSingleCornerRectanglePath();
      case AutoShapeType.SnipSingleCornerRectangle:
        return shapePath.GetSnipSingleCornerRectanglePath();
      case AutoShapeType.SnipSameSideCornerRectangle:
        return shapePath.GetSnipSameSideCornerRectanglePath();
      case AutoShapeType.SnipDiagonalCornerRectangle:
        return shapePath.GetSnipDiagonalCornerRectanglePath();
      case AutoShapeType.Frame:
        return shapePath.GetFramePath();
      case AutoShapeType.HalfFrame:
        return shapePath.GetHalfFramePath();
      case AutoShapeType.Teardrop:
        return shapePath.GetTearDropPath();
      case AutoShapeType.Chord:
        return shapePath.GetChordPath();
      case AutoShapeType.L_Shape:
        return shapePath.GetL_ShapePath();
      case AutoShapeType.MathPlus:
        return shapePath.GetMathPlusPath();
      case AutoShapeType.MathMinus:
        return shapePath.GetMathMinusPath();
      case AutoShapeType.MathMultiply:
        return shapePath.GetMathMultiplyPath();
      case AutoShapeType.MathDivision:
        return shapePath.GetMathDivisionPath();
      case AutoShapeType.MathEqual:
        return shapePath.GetMathEqualPath();
      case AutoShapeType.MathNotEqual:
        return shapePath.GetMathNotEqualPath();
      case AutoShapeType.Cloud:
        return shapePath.GetCloudPath();
      case AutoShapeType.Line:
      case AutoShapeType.StraightConnector:
        float width = pen.Width;
        if ((double) pen.Width < 1.0)
          pen.Width = 1f;
        bool isArrowHeadExist = false;
        PointF[] pointsBasedOnFlip = this.GetLinePointsBasedOnFlip(shape.FlipHorizontal, shape.FlipVertical, bounds);
        this.DrawArrowHead(shape, pen, bounds, ref isArrowHeadExist, ref graphicsPath, pointsBasedOnFlip);
        if (!isArrowHeadExist)
          graphicsPath.AddLines(pointsBasedOnFlip);
        pen.Width = width;
        return graphicsPath;
      case AutoShapeType.ElbowConnector:
        return shapePath.GetBentConnectorPath();
      case AutoShapeType.CurvedConnector:
        return shapePath.GetCurvedConnectorPath();
      case AutoShapeType.BentConnector2:
        return shapePath.GetBendConnector2Path();
      case AutoShapeType.BentConnector4:
        return shapePath.GetBentConnector4Path();
      case AutoShapeType.BentConnector5:
        return shapePath.GetBentConnector5Path();
      case AutoShapeType.CurvedConnector2:
        return shapePath.GetCurvedConnector2Path();
      case AutoShapeType.CurvedConnector4:
        return shapePath.GetCurvedConnector4Path();
      case AutoShapeType.CurvedConnector5:
        return shapePath.GetCurvedConnector5Path();
    }
    if (shape.Is2007Shape && shape.VMLPathPoints != null && shape.VMLPathPoints.Count > 0)
      return shapePath.GetVMLCustomShapePath(shape.VMLPathPoints);
    return shape.Path2DList != null && shape.Path2DList.Count > 0 ? shapePath.GetCustomGeomentryPath(bounds, graphicsPath, shape) : graphicsPath;
  }

  private GraphicsPath GetGraphicsPath(ChildShape shape, RectangleF bounds, ref Pen pen)
  {
    ShapePath shapePath = new ShapePath(bounds, shape.ShapeGuide);
    GraphicsPath graphicsPath = this.CreateGraphicsPath();
    AutoShapeType autoShapeType = shape.AutoShapeType;
    if (autoShapeType == AutoShapeType.Unknown && shape.IsTextBoxShape)
      autoShapeType = AutoShapeType.Rectangle;
    switch (autoShapeType)
    {
      case AutoShapeType.Rectangle:
      case AutoShapeType.FlowChartProcess:
        graphicsPath.AddRectangle(bounds);
        return graphicsPath;
      case AutoShapeType.Parallelogram:
      case AutoShapeType.FlowChartData:
        return shapePath.GetParallelogramPath();
      case AutoShapeType.Trapezoid:
        return shape.Is2007Shape ? shapePath.GetFlowChartManualOperationPath() : shapePath.GetTrapezoidPath();
      case AutoShapeType.Diamond:
      case AutoShapeType.FlowChartDecision:
        PointF[] points1 = new PointF[4]
        {
          new PointF(bounds.X, bounds.Y + bounds.Height / 2f),
          new PointF(bounds.X + bounds.Width / 2f, bounds.Y),
          new PointF(bounds.Right, bounds.Y + bounds.Height / 2f),
          new PointF(bounds.X + bounds.Width / 2f, bounds.Bottom)
        };
        graphicsPath.AddLines(points1);
        graphicsPath.CloseFigure();
        break;
      case AutoShapeType.RoundedRectangle:
        return shapePath.GetRoundedRectanglePath();
      case AutoShapeType.Octagon:
        return shapePath.GetOctagonPath();
      case AutoShapeType.IsoscelesTriangle:
        return shapePath.GetTrianglePath();
      case AutoShapeType.RightTriangle:
        PointF[] points2 = new PointF[3]
        {
          new PointF(bounds.X, bounds.Bottom),
          new PointF(bounds.X, bounds.Y),
          new PointF(bounds.Right, bounds.Bottom)
        };
        graphicsPath.AddLines(points2);
        graphicsPath.CloseFigure();
        return graphicsPath;
      case AutoShapeType.Oval:
        graphicsPath.AddEllipse(bounds);
        return graphicsPath;
      case AutoShapeType.Hexagon:
        return shapePath.GetHexagonPath();
      case AutoShapeType.Cross:
        return shapePath.GetCrossPath();
      case AutoShapeType.RegularPentagon:
        return shapePath.GetRegularPentagonPath();
      case AutoShapeType.Can:
        return shapePath.GetCanPath();
      case AutoShapeType.Cube:
        return shapePath.GetCubePath();
      case AutoShapeType.Bevel:
        return shapePath.GetBevelPath();
      case AutoShapeType.FoldedCorner:
        return shapePath.GetFoldedCornerPath();
      case AutoShapeType.SmileyFace:
        GraphicsPath[] smileyFacePath = shapePath.GetSmileyFacePath();
        Color color1 = !shape.FillFormat.Color.IsEmpty ? shape.FillFormat.Color : (!shape.FillFormat.ForeColor.IsEmpty ? shape.FillFormat.ForeColor : Color.Empty);
        if (shape.FillFormat.FillType == FillType.FillGradient && shape.FillFormat.GradientFill != null && shape.FillFormat.GradientFill.GradientStops.Count > 0)
          color1 = shape.FillFormat.GradientFill.GradientStops[shape.FillFormat.GradientFill.GradientStops.Count - 1].Color;
        foreach (GraphicsPath path in smileyFacePath)
        {
          if (color1 != Color.Empty)
            this.Graphics.FillPath((Brush) this.CreateSolidBrush(color1), path);
          this.Graphics.DrawPath(pen, path);
        }
        break;
      case AutoShapeType.Donut:
        return shapePath.GetDonutPath(0.0);
      case AutoShapeType.NoSymbol:
        return shapePath.GetNoSymbolPath();
      case AutoShapeType.BlockArc:
        return shapePath.GetBlockArcPath();
      case AutoShapeType.Heart:
        return shapePath.GetHeartPath();
      case AutoShapeType.LightningBolt:
        return shapePath.GetLightningBoltPath();
      case AutoShapeType.Sun:
        return shapePath.GetSunPath();
      case AutoShapeType.Moon:
        return shapePath.GetMoonPath();
      case AutoShapeType.Arc:
        GraphicsPath[] arcPath = shapePath.GetArcPath();
        Color color2 = !shape.FillFormat.Color.IsEmpty ? shape.FillFormat.Color : (!shape.FillFormat.ForeColor.IsEmpty ? shape.FillFormat.ForeColor : Color.Empty);
        if (shape.FillFormat.FillType == FillType.FillGradient && shape.FillFormat.GradientFill != null && shape.FillFormat.GradientFill.GradientStops.Count > 0)
          color2 = shape.FillFormat.GradientFill.GradientStops[shape.FillFormat.GradientFill.GradientStops.Count - 1].Color;
        if (color2 != Color.Empty)
          this.Graphics.FillPath((Brush) this.CreateSolidBrush(color2), arcPath[1]);
        this.Graphics.DrawPath(pen, arcPath[0]);
        break;
      case AutoShapeType.DoubleBracket:
        return shapePath.GetDoubleBracketPath();
      case AutoShapeType.DoubleBrace:
        return shapePath.GetDoubleBracePath();
      case AutoShapeType.Plaque:
        return shapePath.GetPlaquePath();
      case AutoShapeType.LeftBracket:
        return shapePath.GetLeftBracketPath();
      case AutoShapeType.RightBracket:
        return shapePath.GetRightBracketPath();
      case AutoShapeType.LeftBrace:
        return shapePath.GetLeftBracePath();
      case AutoShapeType.RightBrace:
        return shapePath.GetRightBracePath();
      case AutoShapeType.RightArrow:
        return shapePath.GetRightArrowPath();
      case AutoShapeType.LeftArrow:
        return shapePath.GetLeftArrowPath();
      case AutoShapeType.UpArrow:
        return shapePath.GetUpArrowPath();
      case AutoShapeType.DownArrow:
        return shapePath.GetDownArrowPath();
      case AutoShapeType.LeftRightArrow:
        return shapePath.GetLeftRightArrowPath();
      case AutoShapeType.UpDownArrow:
        return shapePath.GetUpDownArrowPath();
      case AutoShapeType.QuadArrow:
        return shapePath.GetQuadArrowPath();
      case AutoShapeType.LeftRightUpArrow:
        return shapePath.GetLeftRightUpArrowPath();
      case AutoShapeType.BentArrow:
        return shapePath.GetBentArrowPath();
      case AutoShapeType.UTurnArrow:
        return shapePath.GetUTrunArrowPath();
      case AutoShapeType.LeftUpArrow:
        return shapePath.GetLeftUpArrowPath();
      case AutoShapeType.BentUpArrow:
        return shapePath.GetBentUpArrowPath();
      case AutoShapeType.CurvedRightArrow:
        return shapePath.GetCurvedRightArrowPath();
      case AutoShapeType.CurvedLeftArrow:
        return shapePath.GetCurvedLeftArrowPath();
      case AutoShapeType.CurvedUpArrow:
        return shapePath.GetCurvedUpArrowPath();
      case AutoShapeType.CurvedDownArrow:
        return shapePath.GetCurvedDownArrowPath();
      case AutoShapeType.StripedRightArrow:
        return shapePath.GetStripedRightArrowPath();
      case AutoShapeType.NotchedRightArrow:
        return shapePath.GetNotchedRightArrowPath();
      case AutoShapeType.Pentagon:
        return shapePath.GetPentagonPath();
      case AutoShapeType.Chevron:
        return shapePath.GetChevronPath();
      case AutoShapeType.RightArrowCallout:
        return shapePath.GetRightArrowCalloutPath();
      case AutoShapeType.LeftArrowCallout:
        return shapePath.GetLeftArrowCalloutPath();
      case AutoShapeType.UpArrowCallout:
        return shapePath.GetUpArrowCalloutPath();
      case AutoShapeType.DownArrowCallout:
        return shapePath.GetDownArrowCalloutPath();
      case AutoShapeType.LeftRightArrowCallout:
        return shapePath.GetLeftRightArrowCalloutPath();
      case AutoShapeType.QuadArrowCallout:
        return shapePath.GetQuadArrowCalloutPath();
      case AutoShapeType.CircularArrow:
        return shapePath.GetCircularArrowPath();
      case AutoShapeType.FlowChartAlternateProcess:
        return shapePath.GetFlowChartAlternateProcessPath();
      case AutoShapeType.FlowChartPredefinedProcess:
        return shapePath.GetFlowChartPredefinedProcessPath();
      case AutoShapeType.FlowChartInternalStorage:
        return shapePath.GetFlowChartInternalStoragePath();
      case AutoShapeType.FlowChartDocument:
        return shapePath.GetFlowChartDocumentPath();
      case AutoShapeType.FlowChartMultiDocument:
        return shapePath.GetFlowChartMultiDocumentPath();
      case AutoShapeType.FlowChartTerminator:
        return shapePath.GetFlowChartTerminatorPath();
      case AutoShapeType.FlowChartPreparation:
        return shapePath.GetFlowChartPreparationPath();
      case AutoShapeType.FlowChartManualInput:
        return shapePath.GetFlowChartManualInputPath();
      case AutoShapeType.FlowChartManualOperation:
        return shapePath.GetFlowChartManualOperationPath();
      case AutoShapeType.FlowChartConnector:
        return shapePath.GetFlowChartConnectorPath();
      case AutoShapeType.FlowChartOffPageConnector:
        return shapePath.GetFlowChartOffPageConnectorPath();
      case AutoShapeType.FlowChartCard:
        return shapePath.GetFlowChartCardPath();
      case AutoShapeType.FlowChartPunchedTape:
        return shapePath.GetFlowChartPunchedTapePath();
      case AutoShapeType.FlowChartSummingJunction:
        return shapePath.GetFlowChartSummingJunctionPath();
      case AutoShapeType.FlowChartOr:
        return shapePath.GetFlowChartOrPath();
      case AutoShapeType.FlowChartCollate:
        return shapePath.GetFlowChartCollatePath();
      case AutoShapeType.FlowChartSort:
        return shapePath.GetFlowChartSortPath();
      case AutoShapeType.FlowChartExtract:
        return shapePath.GetFlowChartExtractPath();
      case AutoShapeType.FlowChartMerge:
        return shapePath.GetFlowChartMergePath();
      case AutoShapeType.FlowChartStoredData:
        return shapePath.GetFlowChartOnlineStoragePath();
      case AutoShapeType.FlowChartDelay:
        return shapePath.GetFlowChartDelayPath();
      case AutoShapeType.FlowChartSequentialAccessStorage:
        return shapePath.GetFlowChartSequentialAccessStoragePath();
      case AutoShapeType.FlowChartMagneticDisk:
        return shapePath.GetFlowChartMagneticDiskPath();
      case AutoShapeType.FlowChartDirectAccessStorage:
        return shapePath.GetFlowChartDirectAccessStoragePath();
      case AutoShapeType.FlowChartDisplay:
        return shapePath.GetFlowChartDisplayPath();
      case AutoShapeType.Explosion1:
        return shapePath.GetExplosion1();
      case AutoShapeType.Explosion2:
        return shapePath.GetExplosion2();
      case AutoShapeType.Star4Point:
        return shapePath.GetStar4Point();
      case AutoShapeType.Star5Point:
        return shapePath.GetStar5Point();
      case AutoShapeType.Star8Point:
        return shapePath.GetStar8Point();
      case AutoShapeType.Star16Point:
        return shapePath.GetStar16Point();
      case AutoShapeType.Star24Point:
        return shapePath.GetStar24Point();
      case AutoShapeType.Star32Point:
        return shapePath.GetStar32Point();
      case AutoShapeType.UpRibbon:
        return shapePath.GetUpRibbon();
      case AutoShapeType.DownRibbon:
        return shapePath.GetDownRibbon();
      case AutoShapeType.CurvedUpRibbon:
        return shapePath.GetCurvedUpRibbon();
      case AutoShapeType.CurvedDownRibbon:
        return shapePath.GetCurvedDownRibbon();
      case AutoShapeType.VerticalScroll:
        return shapePath.GetVerticalScroll();
      case AutoShapeType.HorizontalScroll:
        GraphicsPath[] horizontalScroll = shapePath.GetHorizontalScroll();
        Color color3 = !shape.FillFormat.Color.IsEmpty ? shape.FillFormat.Color : (!shape.FillFormat.ForeColor.IsEmpty ? shape.FillFormat.ForeColor : Color.Empty);
        if (shape.FillFormat.FillType == FillType.FillGradient && shape.FillFormat.GradientFill != null && shape.FillFormat.GradientFill.GradientStops.Count > 0)
          color3 = shape.FillFormat.GradientFill.GradientStops[shape.FillFormat.GradientFill.GradientStops.Count - 1].Color;
        foreach (GraphicsPath path in horizontalScroll)
        {
          if (color3 != Color.Empty)
            this.Graphics.FillPath((Brush) this.CreateSolidBrush(color3), path);
          this.Graphics.DrawPath(pen, path);
        }
        break;
      case AutoShapeType.Wave:
        return shapePath.GetWave();
      case AutoShapeType.DoubleWave:
        return shapePath.GetDoubleWave();
      case AutoShapeType.RectangularCallout:
        return shapePath.GetRectangularCalloutPath();
      case AutoShapeType.RoundedRectangularCallout:
        return shapePath.GetRoundedRectangularCalloutPath();
      case AutoShapeType.OvalCallout:
        return shapePath.GetOvalCalloutPath();
      case AutoShapeType.CloudCallout:
        return shapePath.GetCloudCalloutPath();
      case AutoShapeType.LineCallout1:
      case AutoShapeType.LineCallout1NoBorder:
        return shapePath.GetLineCallout1Path();
      case AutoShapeType.LineCallout2:
      case AutoShapeType.LineCallout2NoBorder:
        return shapePath.GetLineCallout2Path();
      case AutoShapeType.LineCallout3:
      case AutoShapeType.LineCallout3NoBorder:
        return shapePath.GetLineCallout3Path();
      case AutoShapeType.LineCallout1AccentBar:
      case AutoShapeType.LineCallout1BorderAndAccentBar:
        return shapePath.GetLineCallout1AccentBarPath();
      case AutoShapeType.LineCallout2AccentBar:
      case AutoShapeType.LineCallout2BorderAndAccentBar:
        return shapePath.GetLineCallout2AccentBarPath();
      case AutoShapeType.LineCallout3AccentBar:
      case AutoShapeType.LineCallout3BorderAndAccentBar:
        return shapePath.GetLineCallout3AccentBarPath();
      case AutoShapeType.DiagonalStripe:
        return shapePath.GetDiagonalStripePath();
      case AutoShapeType.Pie:
        return shapePath.GetPiePath();
      case AutoShapeType.Decagon:
        return shapePath.GetDecagonPath();
      case AutoShapeType.Heptagon:
        return shapePath.GetHeptagonPath();
      case AutoShapeType.Dodecagon:
        return shapePath.GetDodecagonPath();
      case AutoShapeType.Star6Point:
        return shapePath.GetStar6Point();
      case AutoShapeType.Star7Point:
        return shapePath.GetStar7Point();
      case AutoShapeType.Star10Point:
        return shapePath.GetStar10Point();
      case AutoShapeType.Star12Point:
        return shapePath.GetStar12Point();
      case AutoShapeType.RoundSingleCornerRectangle:
        return shapePath.GetRoundSingleCornerRectanglePath();
      case AutoShapeType.RoundSameSideCornerRectangle:
        return shapePath.GetRoundSameSideCornerRectanglePath();
      case AutoShapeType.RoundDiagonalCornerRectangle:
        return shapePath.GetRoundDiagonalCornerRectanglePath();
      case AutoShapeType.SnipAndRoundSingleCornerRectangle:
        return shapePath.GetSnipAndRoundSingleCornerRectanglePath();
      case AutoShapeType.SnipSingleCornerRectangle:
        return shapePath.GetSnipSingleCornerRectanglePath();
      case AutoShapeType.SnipSameSideCornerRectangle:
        return shapePath.GetSnipSameSideCornerRectanglePath();
      case AutoShapeType.SnipDiagonalCornerRectangle:
        return shapePath.GetSnipDiagonalCornerRectanglePath();
      case AutoShapeType.Frame:
        return shapePath.GetFramePath();
      case AutoShapeType.HalfFrame:
        return shapePath.GetHalfFramePath();
      case AutoShapeType.Teardrop:
        return shapePath.GetTearDropPath();
      case AutoShapeType.Chord:
        return shapePath.GetChordPath();
      case AutoShapeType.L_Shape:
        return shapePath.GetL_ShapePath();
      case AutoShapeType.MathPlus:
        return shapePath.GetMathPlusPath();
      case AutoShapeType.MathMinus:
        return shapePath.GetMathMinusPath();
      case AutoShapeType.MathMultiply:
        return shapePath.GetMathMultiplyPath();
      case AutoShapeType.MathDivision:
        return shapePath.GetMathDivisionPath();
      case AutoShapeType.MathEqual:
        return shapePath.GetMathEqualPath();
      case AutoShapeType.MathNotEqual:
        return shapePath.GetMathNotEqualPath();
      case AutoShapeType.Cloud:
        return shapePath.GetCloudPath();
      case AutoShapeType.Line:
      case AutoShapeType.StraightConnector:
        if ((double) pen.Width < 1.0)
          pen.Width = 1f;
        bool isArrowHeadExist = false;
        PointF[] pointsBasedOnFlip = this.GetLinePointsBasedOnFlip(shape.FlipHorizantal, shape.FlipVertical, bounds);
        this.DrawArrowHead(shape, pen, bounds, ref isArrowHeadExist, ref graphicsPath, pointsBasedOnFlip);
        if (!isArrowHeadExist)
          graphicsPath.AddLines(pointsBasedOnFlip);
        return graphicsPath;
      case AutoShapeType.ElbowConnector:
        GraphicsPath bentConnectorPath = shapePath.GetBentConnectorPath();
        if (shape.FlipVertical || shape.FlipHorizantal)
          this.Graphics.Transform = this.GetTransformMatrix(bounds, 0.0f, shape.FlipHorizantal, shape.FlipVertical);
        return bentConnectorPath;
      case AutoShapeType.CurvedConnector:
        return shapePath.GetCurvedConnectorPath();
    }
    return graphicsPath;
  }

  public SizeF MeasureImage(WPicture image)
  {
    return new SizeF((float) ((double) image.Size.Width * (double) image.WidthScale / 100.0), (float) ((double) image.Size.Height * (double) image.HeightScale / 100.0));
  }

  public SizeF MeasurePictureBulletSize(WPicture picture, Font font)
  {
    SizeF sizeF = this.MeasureImage(picture);
    float num1 = (double) font.Size <= 4.0 ? 4f : font.Size;
    float num2 = (float) Math.Round((double) sizeF.Width / (double) sizeF.Height, MidpointRounding.AwayFromZero);
    float num3 = (float) Math.Round((double) sizeF.Height / (double) sizeF.Width, MidpointRounding.AwayFromZero);
    float num4 = (double) num2 <= 0.0 ? 1f : num2;
    float num5 = (double) num3 <= 0.0 ? 1f : num3;
    sizeF.Width = (double) num1 == 12.0 ? num4 * 10f : num4 * (float) ((double) num1 - 12.0 + 10.0);
    sizeF.Height = (double) num1 == 12.0 ? num5 * 10f : num5 * (float) ((double) num1 - 12.0 + 10.0);
    return sizeF;
  }

  public SizeF MeasureString(string text, Font font, StringFormat format)
  {
    return this.MeasureString(text, font, format, (WCharacterFormat) null, false);
  }

  public SizeF MeasureString(
    string text,
    Font font,
    StringFormat format,
    WCharacterFormat charFormat,
    bool isMeasureFromTabList)
  {
    return this.MeasureString(text, font, format, charFormat, isMeasureFromTabList, false);
  }

  public RectangleF GetExactStringBounds(string text, Font font)
  {
    RectangleF rectangleF = new RectangleF();
    using (GraphicsPath graphicsPath = new GraphicsPath())
    {
      graphicsPath.AddString(text, font.FontFamily, (int) font.Style, font.Size, Point.Empty, StringFormat.GenericTypographic);
      return graphicsPath.GetBounds();
    }
  }

  public SizeF MeasureString(
    string text,
    Font font,
    StringFormat format,
    WCharacterFormat charFormat,
    bool isMeasureFromTabList,
    bool isMeasureFromSmallCapString)
  {
    float num1 = 0.0f;
    float num2 = 0.0f;
    if (charFormat != null)
    {
      num1 = charFormat.CharacterSpacing;
      num2 = charFormat.Scaling;
    }
    if (text == null || text.Length == 0)
      return SizeF.Empty;
    text = text.Replace('\u001E'.ToString(), "-");
    text = text.Replace('\u00AD'.ToString(), "-");
    text = text.Replace('\u200D'.ToString(), "");
    if (text.Length == 0)
      return SizeF.Empty;
    if (format == null)
      format = new StringFormat(this.StringFormt);
    string name = font.Name;
    if (charFormat != null && name == "Arial Unicode MS")
    {
      FontStyle fontStyle = font.Style & ~FontStyle.Bold;
      font = charFormat.Document.FontSettings.GetFont(name, font.Size, fontStyle);
    }
    SizeF size = SizeF.Empty;
    if (text.Length > 4000)
    {
      int length = 4000;
      text = text.Substring(0, length);
    }
    if (charFormat != null && charFormat.AllCaps)
      text = text.ToUpper();
    StringBuilder stringBuilder = new StringBuilder(text).Replace('\u001E'.ToString(), "-");
    if (name == "Arial Narrow" && font.Style == FontStyle.Bold || name == "Bookman Old Style" && (font.Style == FontStyle.Regular || font.Style == FontStyle.Italic))
      stringBuilder = stringBuilder.Replace(ControlChar.NonBreakingSpaceChar, ControlChar.SpaceChar);
    text = stringBuilder.Replace('\u00AD'.ToString(), "-").ToString();
    Font font1 = charFormat == null || charFormat.SubSuperScript == SubSuperScript.None ? font : charFormat.Document.FontSettings.GetFont(name, this.GetSubSuperScriptFontSize(font), font.Style);
    size.Width = charFormat == null || !charFormat.IsKernFont ? this.GraphicsBmp.MeasureString(text, font1, new PointF(0.0f, 0.0f), format).Width : this.Graphics.MeasureString(text.ToString(), font1, new PointF(0.0f, 0.0f), format).Width;
    size.Height = this.Graphics.MeasureString(text[0].ToString(), font, new PointF(0.0f, 0.0f), format).Height;
    if (charFormat != null && charFormat.SmallCaps)
      this.MeasureSmallCapString(text, ref size, font1, format, charFormat);
    if (charFormat != null && (double) num2 != 100.0 && !isMeasureFromSmallCapString)
      size = new SizeF(size.Width * (num2 / 100f), size.Height);
    if ((double) num1 != 0.0)
      size.Width += (float) text.Length * num1;
    if (!isMeasureFromTabList && (charFormat == null || !charFormat.ComplexScript && text != "") && name == "Arial Unicode MS")
      size.Height = this.GetExceededLineHeightForArialUnicodeMSFont(font, false);
    switch (name)
    {
      case "MS Gothic":
        size.Height += 0.3154f * font.Size;
        break;
      case "Malgun Gothic":
        size.Height += 0.3994f * font.Size;
        break;
    }
    return size;
  }

  public float GetSubSuperScriptFontSize(Font font)
  {
    if (font.Name == "Arial Narrow" && (double) font.Size == 10.0)
      return font.Size / 1.54f;
    return (double) font.Size != 11.0 ? font.Size / 1.5f : font.Size / 1.58f;
  }

  internal float GetExceededLineHeightForArialUnicodeMSFont(Font font, bool isAscent)
  {
    float size = font.Size;
    float emHeight = (float) font.FontFamily.GetEmHeight(font.Style);
    float cellAscent = (float) font.FontFamily.GetCellAscent(font.Style);
    float cellDescent = (float) font.FontFamily.GetCellDescent(font.Style);
    float lineSpacing = (float) font.FontFamily.GetLineSpacing(font.Style);
    float num1 = size / emHeight;
    int num2 = (int) ((double) cellAscent + (double) cellDescent - (double) emHeight);
    int num3 = num2 / 2;
    int num4 = num2 - num3;
    int num5 = (int) (0.3 * ((double) cellAscent + (double) cellDescent)) - num2;
    int num6 = num5 > 0 ? num5 : 0;
    float num7 = (float) (int) ((double) cellAscent + (double) num3);
    float num8 = (float) (int) ((double) cellDescent + (double) num4);
    float num9 = (float) (int) ((double) num7 + (double) num8 + (double) num6);
    float num10 = num9 - (num7 + num8);
    if (isAscent)
      num9 -= num8 + num10;
    return num1 * (float) (int) num9;
  }

  public SizeF MeasureString(
    string text,
    Font font,
    Font defaultFont,
    StringFormat format,
    WCharacterFormat charFormat)
  {
    float num = 0.0f;
    if (charFormat != null)
      num = charFormat.CharacterSpacing;
    if (text == null || text.Length == 0)
      return SizeF.Empty;
    text = text.Replace('\u001E'.ToString(), "-");
    text = text.Replace('\u00AD'.ToString(), "-");
    text = text.Replace('\u200D'.ToString(), "");
    if (text.Length == 0)
      return SizeF.Empty;
    if (format == null)
      format = this.StringFormt;
    string name = font.Name;
    if (charFormat != null && name == "Arial Unicode MS")
    {
      FontStyle fontStyle = font.Style & ~FontStyle.Bold;
      font = charFormat.Document.FontSettings.GetFont(name, font.Size, fontStyle);
    }
    SizeF size = this.MeasureUnicodeString(text, font, defaultFont, format, charFormat);
    Font font1 = charFormat == null || charFormat.SubSuperScript == SubSuperScript.None ? font : charFormat.Document.FontSettings.GetFont(name, this.GetSubSuperScriptFontSize(font), font.Style);
    if (charFormat != null && charFormat.SmallCaps)
      this.MeasureSmallCapString(text, ref size, font1, format, charFormat);
    if ((double) num != 0.0)
      size.Width += (float) text.Length * num;
    if (charFormat != null)
    {
      if (!charFormat.ComplexScript)
      {
        if (!(text.Trim(' ') != ""))
          goto label_19;
      }
      else
        goto label_19;
    }
    if (name == "Arial Unicode MS")
      size.Height = this.GetExceededLineHeightForArialUnicodeMSFont(font, false);
label_19:
    return size;
  }

  private void MeasureSmallCapString(
    string text,
    ref SizeF size,
    Font font,
    StringFormat format,
    WCharacterFormat charFormat)
  {
    Font font1 = charFormat.Document.FontSettings.GetFont(font.Name, (double) font.Size * 0.8 > 3.0 ? font.Size * 0.8f : 2f, font.Style);
    string empty1 = string.Empty;
    string empty2 = string.Empty;
    foreach (char c in text)
    {
      if (char.IsUpper(c) || !char.IsLetter(c) && !c.Equals(ControlChar.SpaceChar))
        empty1 += c.ToString();
      else
        empty2 += c.ToString();
    }
    float num = !charFormat.IsKernFont ? this.GraphicsBmp.MeasureString(empty1, font, new PointF(0.0f, 0.0f), format).Width + this.GraphicsBmp.MeasureString(empty2.ToUpper(), font1, new PointF(0.0f, 0.0f), format).Width : this.Graphics.MeasureString(empty1, font, new PointF(0.0f, 0.0f), format).Width + this.Graphics.MeasureString(empty2.ToUpper(), font1, new PointF(0.0f, 0.0f), format).Width;
    size.Width = num;
  }

  private SizeF MeasureUnicodeString(
    string text,
    Font font,
    Font defaultFont,
    StringFormat format,
    WCharacterFormat charFormat)
  {
    char[] charArray = text.ToCharArray();
    string text1 = (string) null;
    string text2 = (string) null;
    Font font1 = font;
    if (font.Name == "Arial" || font.Name == "Times New Roman")
      font = charFormat.Document.FontSettings.GetFont("Arial Unicode MS", font.Size, font.Style);
    if (font.Name == "Arial Narrow" && font.Style == FontStyle.Bold)
      text = text.Replace(ControlChar.NonBreakingSpaceChar, ControlChar.SpaceChar);
    float num = 0.0f;
    for (int index = 0; index < charArray.Length; ++index)
    {
      if (!this.IsUnicodeText(charArray[index].ToString()))
      {
        text1 += (string) (object) charArray[index];
        if (text2 != null)
        {
          Font font2 = charFormat == null || charFormat.SubSuperScript == SubSuperScript.None ? font : charFormat.Document.FontSettings.GetFont(font.Name, this.GetSubSuperScriptFontSize(font), font.Style);
          if (charFormat != null && charFormat.IsKernFont)
            num += this.Graphics.MeasureString(text2, font2, new PointF(0.0f, 0.0f), format).Width;
          else
            num += this.GraphicsBmp.MeasureString(text2, font2, new PointF(0.0f, 0.0f), format).Width;
          text2 = (string) null;
        }
      }
      else
      {
        text2 += (string) (object) charArray[index];
        if (text1 != null)
        {
          Font font3 = charFormat == null || charFormat.SubSuperScript == SubSuperScript.None ? defaultFont : charFormat.Document.FontSettings.GetFont(defaultFont.Name, this.GetSubSuperScriptFontSize(defaultFont), defaultFont.Style);
          if (charFormat != null && charFormat.IsKernFont)
            num += this.Graphics.MeasureString(text1, font3, new PointF(0.0f, 0.0f), format).Width;
          else
            num += this.GraphicsBmp.MeasureString(text1, font3, new PointF(0.0f, 0.0f), format).Width;
          text1 = (string) null;
        }
      }
    }
    if (text2 != null)
    {
      Font font4 = charFormat == null || charFormat.SubSuperScript == SubSuperScript.None ? font : charFormat.Document.FontSettings.GetFont(font.Name, this.GetSubSuperScriptFontSize(font), font.Style);
      if (charFormat != null && charFormat.IsKernFont)
        num += this.Graphics.MeasureString(text2, font4, new PointF(0.0f, 0.0f), format).Width;
      else
        num += this.GraphicsBmp.MeasureString(text2, font4, new PointF(0.0f, 0.0f), format).Width;
    }
    else if (text1 != null)
    {
      Font font5 = charFormat == null || charFormat.SubSuperScript == SubSuperScript.None ? defaultFont : charFormat.Document.FontSettings.GetFont(defaultFont.Name, this.GetSubSuperScriptFontSize(defaultFont), defaultFont.Style);
      if (charFormat != null && charFormat.IsKernFont)
        num += this.Graphics.MeasureString(text1, font5, new PointF(0.0f, 0.0f), format).Width;
      else
        num += this.GraphicsBmp.MeasureString(text1, font5, new PointF(0.0f, 0.0f), format).Width;
    }
    if (font.Name == string.Empty)
      font = font1;
    return this.Graphics.MeasureString(text[0].ToString(), font, new PointF(0.0f, 0.0f), format) with
    {
      Width = num
    };
  }

  public SizeF MeasureTextRange(WTextRange txtRange, string text)
  {
    WParagraph ownerParagraph = txtRange.OwnerParagraph;
    WCharacterFormat charFormat = txtRange.CharacterFormat;
    if (txtRange.Text.Trim(' ') == string.Empty && ownerParagraph != null && ownerParagraph.Text == txtRange.Text)
      charFormat = ownerParagraph.BreakCharacterFormat;
    Font font = txtRange.m_layoutInfo != null ? txtRange.m_layoutInfo.Font.GetFont(txtRange.Document) : this.GetFont(txtRange, charFormat, text);
    StringFormat format = new StringFormat(this.StringFormt);
    if (charFormat.Bidi)
      format.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
    else
      format.FormatFlags &= ~StringFormatFlags.DirectionRightToLeft;
    if (this.IsTOC(txtRange) && txtRange.GetOwnerParagraphValue().ParaStyle != null)
    {
      format = new StringFormat(this.StringFormt);
      if (txtRange.OwnerParagraph.ParaStyle.CharacterFormat.Bidi)
        format.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
      else
        format.FormatFlags &= ~StringFormatFlags.DirectionRightToLeft;
    }
    if (text != null && charFormat.AllCaps)
      text = text.ToUpper();
    Font defaultFont = this.GetDefaultFont(txtRange.ScriptType, font, charFormat);
    return !(font.Name != defaultFont.Name) || !this.IsUnicodeText(text) ? this.MeasureString(text, font, format, charFormat, false) : this.MeasureString(text, font, defaultFont, format, charFormat);
  }

  public float GetAscent(Font font) => (float) this.FontMetric.Ascent(font);

  public float GetDescent(Font font) => (float) this.FontMetric.Descent(font);

  public void MatrixTranslate(Matrix matrix, float x, float y, MatrixOrder matrixOrder)
  {
    matrix.Translate(x, y, matrixOrder);
  }

  public void MatrixMultiply(Matrix matrix, Matrix target, MatrixOrder matrixOrder)
  {
    matrix.Multiply(target, matrixOrder);
  }

  private void MatrixRotate(Matrix matrix, float angle, PointF point, MatrixOrder matrixOrder)
  {
    matrix.RotateAt(angle, point, matrixOrder);
  }

  private GraphicsPath CreateGraphicsPath() => new GraphicsPath();

  private Bitmap CreateBitmap(int width, int height) => new Bitmap(width, height);

  private Graphics GetGraphicsFromImage(Bitmap bmp) => Graphics.FromImage((Image) bmp);

  private void DrawUnicodeString(
    string text,
    Font font,
    Brush brush,
    RectangleF rectangle,
    StringFormat stringFormat)
  {
    this.Graphics.DrawString(text, font, brush, rectangle.X, rectangle.Y, stringFormat);
  }

  private Image GetImage(Image image) => image;

  private byte[] GetImage(byte[] imageBytes) => imageBytes;

  private Image CreateImageFromStream(MemoryStream stream) => Image.FromStream((Stream) stream);

  private HatchBrush CreateHatchBrush(HatchStyle hatchstyle, Color foreColor, Color backColor)
  {
    return new HatchBrush(hatchstyle, foreColor, backColor);
  }

  private Pen CreatePen(Color color) => new Pen(color);

  private Pen CreatePen(Color color, float width) => new Pen(color, width);

  private SolidBrush CreateSolidBrush(Color color) => new SolidBrush(color);

  private ColorMatrix CreateColorMatrix() => new ColorMatrix();

  private ColorMatrix CreateColorMatrix(float[][] newColorMatrix)
  {
    return new ColorMatrix(newColorMatrix);
  }

  private ImageAttributes CreateImageAttributes() => new ImageAttributes();

  private void DrawArrowHead(
    ChildShape shape,
    Pen pen,
    RectangleF bounds,
    ref bool isArrowHeadExist,
    ref GraphicsPath path,
    PointF[] linePoints)
  {
    isArrowHeadExist = false;
    PointF endPoint = new PointF(0.0f, 0.0f);
    switch (shape.LineFormat.EndArrowheadStyle)
    {
      case ArrowheadStyle.ArrowheadTriangle:
        this.DrawCloseEndArrowHead(shape, pen, bounds, linePoints, ref endPoint, ref isArrowHeadExist, ref path);
        break;
      case ArrowheadStyle.ArrowheadStealth:
        this.DrawStealthEndArrowHead(shape, pen, bounds, linePoints, ref endPoint, ref isArrowHeadExist, ref path);
        break;
      case ArrowheadStyle.ArrowheadOpen:
        this.DrawOpenEndArrowHead(shape, pen, bounds, linePoints, ref endPoint, ref isArrowHeadExist, ref path);
        break;
    }
    switch (shape.LineFormat.BeginArrowheadStyle)
    {
      case ArrowheadStyle.ArrowheadTriangle:
        this.DrawCloseBeginArrowHead(shape, pen, bounds, linePoints, ref endPoint, ref isArrowHeadExist, ref path);
        break;
      case ArrowheadStyle.ArrowheadStealth:
        this.DrawStealthBeginArrowHead(shape, pen, bounds, linePoints, ref endPoint, ref isArrowHeadExist, ref path);
        break;
      case ArrowheadStyle.ArrowheadOpen:
        this.DrawOpenBeginArrowHead(shape, pen, bounds, linePoints, ref endPoint, ref isArrowHeadExist, ref path);
        break;
    }
  }

  private void DrawArrowHead(
    Shape shape,
    Pen pen,
    RectangleF bounds,
    ref bool isArrowHeadExist,
    ref GraphicsPath path,
    PointF[] linePoints)
  {
    isArrowHeadExist = false;
    PointF endPoint = new PointF(0.0f, 0.0f);
    switch (shape.LineFormat.EndArrowheadStyle)
    {
      case ArrowheadStyle.ArrowheadTriangle:
        this.DrawCloseEndArrowHead(shape, pen, bounds, linePoints, ref endPoint, ref isArrowHeadExist, ref path);
        break;
      case ArrowheadStyle.ArrowheadStealth:
        this.DrawStealthEndArrowHead(shape, pen, bounds, linePoints, ref endPoint, ref isArrowHeadExist, ref path);
        break;
      case ArrowheadStyle.ArrowheadOpen:
        this.DrawOpenEndArrowHead(shape, pen, bounds, linePoints, ref endPoint, ref isArrowHeadExist, ref path);
        break;
    }
    switch (shape.LineFormat.BeginArrowheadStyle)
    {
      case ArrowheadStyle.ArrowheadTriangle:
        this.DrawCloseBeginArrowHead(shape, pen, bounds, linePoints, ref endPoint, ref isArrowHeadExist, ref path);
        break;
      case ArrowheadStyle.ArrowheadStealth:
        this.DrawStealthBeginArrowHead(shape, pen, bounds, linePoints, ref endPoint, ref isArrowHeadExist, ref path);
        break;
      case ArrowheadStyle.ArrowheadOpen:
        this.DrawOpenBeginArrowHead(shape, pen, bounds, linePoints, ref endPoint, ref isArrowHeadExist, ref path);
        break;
    }
  }

  private void DrawOpenEndArrowHead(
    Shape shape,
    Pen pen,
    RectangleF bounds,
    PointF[] linePoints,
    ref PointF endPoint,
    ref bool isArrowHeadExist,
    ref GraphicsPath path)
  {
    PointF[] arrowHeadPoints = this.FindArrowHeadPoints(shape, pen, bounds, linePoints, true, false);
    if (shape.LineFormat.BeginArrowheadStyle != ArrowheadStyle.ArrowheadTriangle && shape.LineFormat.BeginArrowheadStyle != ArrowheadStyle.ArrowheadOpen)
    {
      path.AddLine(linePoints[0].X, linePoints[0].Y, arrowHeadPoints[0].X, arrowHeadPoints[0].Y);
      path.CloseFigure();
    }
    else
      endPoint = arrowHeadPoints[0];
    this.AddOpenArrowHeadPoints(arrowHeadPoints, ref path);
    isArrowHeadExist = true;
  }

  private void DrawOpenEndArrowHead(
    ChildShape shape,
    Pen pen,
    RectangleF bounds,
    PointF[] linePoints,
    ref PointF endPoint,
    ref bool isArrowHeadExist,
    ref GraphicsPath path)
  {
    PointF[] arrowHeadPoints = this.FindArrowHeadPoints(shape, pen, bounds, linePoints, true, false);
    if (shape.LineFormat.BeginArrowheadStyle != ArrowheadStyle.ArrowheadTriangle && shape.LineFormat.BeginArrowheadStyle != ArrowheadStyle.ArrowheadOpen)
    {
      path.AddLine(linePoints[0].X, linePoints[0].Y, arrowHeadPoints[0].X, arrowHeadPoints[0].Y);
      path.CloseFigure();
    }
    else
      endPoint = arrowHeadPoints[0];
    this.AddOpenArrowHeadPoints(arrowHeadPoints, ref path);
    isArrowHeadExist = true;
  }

  private void DrawCloseEndArrowHead(
    Shape shape,
    Pen pen,
    RectangleF bounds,
    PointF[] linePoints,
    ref PointF endPoint,
    ref bool isArrowHeadExist,
    ref GraphicsPath path)
  {
    PointF[] arrowHeadPoints = this.FindArrowHeadPoints(shape, pen, bounds, linePoints, false, false);
    if (shape.LineFormat.BeginArrowheadStyle != ArrowheadStyle.ArrowheadTriangle && shape.LineFormat.BeginArrowheadStyle != ArrowheadStyle.ArrowheadOpen)
    {
      path.AddLine(linePoints[0].X, linePoints[0].Y, arrowHeadPoints[0].X, arrowHeadPoints[0].Y);
      path.CloseFigure();
    }
    else
      endPoint = arrowHeadPoints[0];
    this.AddCloseArrowHeadPoints(arrowHeadPoints, pen);
    isArrowHeadExist = true;
  }

  private void DrawStealthEndArrowHead(
    Shape shape,
    Pen pen,
    RectangleF bounds,
    PointF[] linePoints,
    ref PointF endPoint,
    ref bool isArrowHeadExist,
    ref GraphicsPath path)
  {
    PointF[] arrowHeadPoints = this.FindArrowHeadPoints(shape, pen, bounds, linePoints, false, false);
    if (shape.LineFormat.BeginArrowheadStyle != ArrowheadStyle.ArrowheadTriangle && shape.LineFormat.BeginArrowheadStyle != ArrowheadStyle.ArrowheadOpen)
    {
      path.AddLine(linePoints[0].X, linePoints[0].Y, arrowHeadPoints[0].X, arrowHeadPoints[0].Y);
      path.CloseFigure();
    }
    else
      endPoint = arrowHeadPoints[0];
    this.AddStealthArrowHeadPoints(arrowHeadPoints, pen);
    isArrowHeadExist = true;
  }

  private void DrawCloseEndArrowHead(
    ChildShape shape,
    Pen pen,
    RectangleF bounds,
    PointF[] linePoints,
    ref PointF endPoint,
    ref bool isArrowHeadExist,
    ref GraphicsPath path)
  {
    PointF[] arrowHeadPoints = this.FindArrowHeadPoints(shape, pen, bounds, linePoints, false, false);
    if (shape.LineFormat.BeginArrowheadStyle != ArrowheadStyle.ArrowheadTriangle && shape.LineFormat.BeginArrowheadStyle != ArrowheadStyle.ArrowheadOpen)
    {
      path.AddLine(linePoints[0].X, linePoints[0].Y, arrowHeadPoints[0].X, arrowHeadPoints[0].Y);
      path.CloseFigure();
    }
    else
      endPoint = arrowHeadPoints[0];
    this.AddCloseArrowHeadPoints(arrowHeadPoints, pen);
    isArrowHeadExist = true;
  }

  private void DrawStealthEndArrowHead(
    ChildShape shape,
    Pen pen,
    RectangleF bounds,
    PointF[] linePoints,
    ref PointF endPoint,
    ref bool isArrowHeadExist,
    ref GraphicsPath path)
  {
    PointF[] arrowHeadPoints = this.FindArrowHeadPoints(shape, pen, bounds, linePoints, false, false);
    if (shape.LineFormat.BeginArrowheadStyle != ArrowheadStyle.ArrowheadTriangle && shape.LineFormat.BeginArrowheadStyle != ArrowheadStyle.ArrowheadOpen)
    {
      path.AddLine(linePoints[0].X, linePoints[0].Y, arrowHeadPoints[0].X, arrowHeadPoints[0].Y);
      path.CloseFigure();
    }
    else
      endPoint = arrowHeadPoints[0];
    this.AddStealthArrowHeadPoints(arrowHeadPoints, pen);
    isArrowHeadExist = true;
  }

  private void DrawOpenBeginArrowHead(
    Shape shape,
    Pen pen,
    RectangleF bounds,
    PointF[] linePoints,
    ref PointF endPoint,
    ref bool isArrowHeadExist,
    ref GraphicsPath path)
  {
    PointF[] arrowHeadPoints = this.FindArrowHeadPoints(shape, pen, bounds, linePoints, true, true);
    path.StartFigure();
    if ((double) endPoint.X == 0.0 && (double) endPoint.Y == 0.0)
      path.AddLine(linePoints[1].X, linePoints[1].Y, arrowHeadPoints[0].X, arrowHeadPoints[0].Y);
    else
      path.AddLine(endPoint.X, endPoint.Y, arrowHeadPoints[0].X, arrowHeadPoints[0].Y);
    path.CloseFigure();
    this.AddOpenArrowHeadPoints(arrowHeadPoints, ref path);
    isArrowHeadExist = true;
  }

  private void DrawOpenBeginArrowHead(
    ChildShape shape,
    Pen pen,
    RectangleF bounds,
    PointF[] linePoints,
    ref PointF endPoint,
    ref bool isArrowHeadExist,
    ref GraphicsPath path)
  {
    PointF[] arrowHeadPoints = this.FindArrowHeadPoints(shape, pen, bounds, linePoints, true, true);
    path.StartFigure();
    if ((double) endPoint.X == 0.0 && (double) endPoint.Y == 0.0)
      path.AddLine(linePoints[1].X, linePoints[1].Y, arrowHeadPoints[0].X, arrowHeadPoints[0].Y);
    else
      path.AddLine(endPoint.X, endPoint.Y, arrowHeadPoints[0].X, arrowHeadPoints[0].Y);
    path.CloseFigure();
    this.AddOpenArrowHeadPoints(arrowHeadPoints, ref path);
    isArrowHeadExist = true;
  }

  private void DrawCloseBeginArrowHead(
    Shape shape,
    Pen pen,
    RectangleF bounds,
    PointF[] linePoints,
    ref PointF endPoint,
    ref bool isArrowHeadExist,
    ref GraphicsPath path)
  {
    PointF[] arrowHeadPoints = this.FindArrowHeadPoints(shape, pen, bounds, linePoints, false, true);
    path.StartFigure();
    if ((double) endPoint.X == 0.0 && (double) endPoint.Y == 0.0)
      path.AddLine(linePoints[1].X, linePoints[1].Y, arrowHeadPoints[0].X, arrowHeadPoints[0].Y);
    else
      path.AddLine(endPoint.X, endPoint.Y, arrowHeadPoints[0].X, arrowHeadPoints[0].Y);
    path.CloseFigure();
    this.AddCloseArrowHeadPoints(arrowHeadPoints, pen);
    isArrowHeadExist = true;
  }

  private void DrawCloseBeginArrowHead(
    ChildShape shape,
    Pen pen,
    RectangleF bounds,
    PointF[] linePoints,
    ref PointF endPoint,
    ref bool isArrowHeadExist,
    ref GraphicsPath path)
  {
    PointF[] arrowHeadPoints = this.FindArrowHeadPoints(shape, pen, bounds, linePoints, false, true);
    path.StartFigure();
    if ((double) endPoint.X == 0.0 && (double) endPoint.Y == 0.0)
      path.AddLine(linePoints[1].X, linePoints[1].Y, arrowHeadPoints[0].X, arrowHeadPoints[0].Y);
    else
      path.AddLine(endPoint.X, endPoint.Y, arrowHeadPoints[0].X, arrowHeadPoints[0].Y);
    path.CloseFigure();
    this.AddCloseArrowHeadPoints(arrowHeadPoints, pen);
    isArrowHeadExist = true;
  }

  private void DrawStealthBeginArrowHead(
    Shape shape,
    Pen pen,
    RectangleF bounds,
    PointF[] linePoints,
    ref PointF endPoint,
    ref bool isArrowHeadExist,
    ref GraphicsPath path)
  {
    PointF[] arrowHeadPoints = this.FindArrowHeadPoints(shape, pen, bounds, linePoints, false, true);
    path.StartFigure();
    if ((double) endPoint.X == 0.0 && (double) endPoint.Y == 0.0)
      path.AddLine(linePoints[1].X, linePoints[1].Y, arrowHeadPoints[0].X, arrowHeadPoints[0].Y);
    else
      path.AddLine(endPoint.X, endPoint.Y, arrowHeadPoints[0].X, arrowHeadPoints[0].Y);
    path.CloseFigure();
    this.AddStealthArrowHeadPoints(arrowHeadPoints, pen);
    isArrowHeadExist = true;
  }

  private void DrawStealthBeginArrowHead(
    ChildShape shape,
    Pen pen,
    RectangleF bounds,
    PointF[] linePoints,
    ref PointF endPoint,
    ref bool isArrowHeadExist,
    ref GraphicsPath path)
  {
    PointF[] arrowHeadPoints = this.FindArrowHeadPoints(shape, pen, bounds, linePoints, false, true);
    path.StartFigure();
    if ((double) endPoint.X == 0.0 && (double) endPoint.Y == 0.0)
      path.AddLine(linePoints[1].X, linePoints[1].Y, arrowHeadPoints[0].X, arrowHeadPoints[0].Y);
    else
      path.AddLine(endPoint.X, endPoint.Y, arrowHeadPoints[0].X, arrowHeadPoints[0].Y);
    path.CloseFigure();
    this.AddStealthArrowHeadPoints(arrowHeadPoints, pen);
    isArrowHeadExist = true;
  }

  private void AddCloseArrowHeadPoints(PointF[] points, Pen pen)
  {
    GraphicsPath graphicsPath = this.CreateGraphicsPath();
    PointF[] points1 = new PointF[3]
    {
      points[1],
      points[2],
      points[3]
    };
    graphicsPath.AddPolygon(points1);
    graphicsPath.CloseFigure();
    this.Graphics.FillPolygon((Brush) new SolidBrush(pen.Color), points1);
    this.Graphics.DrawPath(pen, graphicsPath);
  }

  private void AddStealthArrowHeadPoints(PointF[] points, Pen pen)
  {
    GraphicsPath graphicsPath = this.CreateGraphicsPath();
    PointF[] points1 = new PointF[4];
    points1[0] = points[1];
    points1[1] = points[2];
    points1[2] = points[3];
    float x = (float) (((double) points1[0].X + (double) points1[1].X + (double) points1[2].X) / 3.0);
    float y = (float) (((double) points1[0].Y + (double) points1[1].Y + (double) points1[2].Y) / 3.0);
    points1[3] = new PointF(x, y);
    graphicsPath.AddPolygon(points1);
    graphicsPath.CloseFigure();
    this.Graphics.FillPolygon((Brush) new SolidBrush(pen.Color), points1);
    this.Graphics.DrawPath(pen, graphicsPath);
  }

  private void AddOpenArrowHeadPoints(PointF[] points, ref GraphicsPath path)
  {
    path.AddLine(points[1], points[2]);
    path.AddLine(points[2], points[3]);
  }

  private void GetOpenArrowDefaultValues(
    LineFormat lineFormat,
    float lineWidth,
    ref float arrowLength,
    ref float arrowAngle,
    ref float adjustValue,
    bool isFromBeginArrow)
  {
    LineEndWidth lineEndWidth = lineFormat.EndArrowheadWidth;
    if (isFromBeginArrow)
      lineEndWidth = lineFormat.BeginArrowheadWidth;
    switch (lineEndWidth)
    {
      case LineEndWidth.NarrowArrow:
        LineEndLength endArrowheadLength = lineFormat.EndArrowheadLength;
        this.GetOpenNarrowArrowDefaultValues(this.GetArrowHeadLength(lineFormat, isFromBeginArrow), lineWidth, ref arrowLength, ref arrowAngle, ref adjustValue);
        break;
      case LineEndWidth.MediumWidthArrow:
        this.GetOpenMediumArrowDefaultValues(this.GetArrowHeadLength(lineFormat, isFromBeginArrow), lineWidth, ref arrowLength, ref arrowAngle, ref adjustValue);
        break;
      case LineEndWidth.WideArrow:
        this.GetOpenWideArrowDefaultValues(this.GetArrowHeadLength(lineFormat, isFromBeginArrow), lineWidth, ref arrowLength, ref arrowAngle, ref adjustValue);
        break;
    }
  }

  private void GetCloseArrowDefaultValues(
    LineFormat lineFormat,
    float lineWidth,
    ref float arrowLength,
    ref float arrowAngle,
    ref float adjustValue,
    bool isFromBeginArrow)
  {
    LineEndWidth lineEndWidth = lineFormat.EndArrowheadWidth;
    if (isFromBeginArrow)
      lineEndWidth = lineFormat.BeginArrowheadWidth;
    switch (lineEndWidth)
    {
      case LineEndWidth.NarrowArrow:
        LineEndLength endArrowheadLength = lineFormat.EndArrowheadLength;
        this.GetCloseNarrowArrowDefaultValues(this.GetArrowHeadLength(lineFormat, isFromBeginArrow), lineWidth, ref arrowLength, ref arrowAngle, ref adjustValue);
        break;
      case LineEndWidth.MediumWidthArrow:
        this.GetCloseMediumArrowDefaultValues(this.GetArrowHeadLength(lineFormat, isFromBeginArrow), lineWidth, ref arrowLength, ref arrowAngle, ref adjustValue);
        break;
      case LineEndWidth.WideArrow:
        this.GetCloseWideArrowDefaultValues(this.GetArrowHeadLength(lineFormat, isFromBeginArrow), lineWidth, ref arrowLength, ref arrowAngle, ref adjustValue);
        break;
    }
  }

  private LineEndLength GetArrowHeadLength(LineFormat lineFormat, bool isFromBeginArrow)
  {
    LineEndLength arrowHeadLength = lineFormat.EndArrowheadLength;
    if (isFromBeginArrow)
      arrowHeadLength = lineFormat.BeginArrowheadLength;
    return arrowHeadLength;
  }

  private void GetCloseNarrowArrowDefaultValues(
    LineEndLength arrowHeadLength,
    float lineWidth,
    ref float arrowLength,
    ref float arrowAngle,
    ref float adjustValue)
  {
    switch (arrowHeadLength)
    {
      case LineEndLength.ShortArrow:
        arrowLength = (double) lineWidth > 1.0 ? lineWidth * 0.37f : 2.7f;
        arrowAngle = 26f;
        adjustValue = lineWidth * 1.15f;
        break;
      case LineEndLength.MediumLenArrow:
        arrowLength = (double) lineWidth > 1.0 ? (float) Math.Round((double) lineWidth * 0.97000002861022949) : 4.2f;
        arrowAngle = 18.5f;
        adjustValue = lineWidth * 1.59f;
        break;
      case LineEndLength.LongArrow:
        arrowLength = (double) lineWidth > 1.0 ? (float) Math.Round((double) lineWidth * 2.0499999523162842) : 9f;
        arrowAngle = 11.3f;
        adjustValue = lineWidth * 2.52f;
        break;
    }
  }

  private void GetCloseMediumArrowDefaultValues(
    LineEndLength arrowHeadLength,
    float lineWidth,
    ref float arrowLength,
    ref float arrowAngle,
    ref float adjustValue)
  {
    switch (arrowHeadLength)
    {
      case LineEndLength.ShortArrow:
        arrowLength = (double) lineWidth > 1.0 ? lineWidth * 0.845f : 3.5f;
        arrowAngle = 37f;
        adjustValue = lineWidth * 0.83f;
        break;
      case LineEndLength.MediumLenArrow:
        arrowLength = (double) lineWidth > 1.0 ? lineWidth * 1.5f : 5f;
        arrowAngle = 26.5f;
        adjustValue = lineWidth * 1.15f;
        break;
      case LineEndLength.LongArrow:
        arrowLength = (double) lineWidth > 1.0 ? (float) Math.Round((double) lineWidth * 2.869999885559082) : 8f;
        arrowAngle = 16.65f;
        adjustValue = lineWidth * 1.75f;
        break;
    }
  }

  private void GetCloseWideArrowDefaultValues(
    LineEndLength arrowHeadLength,
    float lineWidth,
    ref float arrowLength,
    ref float arrowAngle,
    ref float adjustValue)
  {
    switch (arrowHeadLength)
    {
      case LineEndLength.ShortArrow:
        arrowLength = (double) lineWidth > 1.0 ? lineWidth * 1.36f : 4.5f;
        arrowAngle = 51.5f;
        adjustValue = lineWidth * 0.65f;
        break;
      case LineEndLength.MediumLenArrow:
        arrowLength = (double) lineWidth > 1.0 ? lineWidth * 2.24f : 6.2f;
        arrowAngle = 39.7f;
        adjustValue = lineWidth * 0.78f;
        break;
      case LineEndLength.LongArrow:
        arrowLength = (double) lineWidth > 1.0 ? (float) Math.Round((double) lineWidth * 3.7799999713897705) : 9.45f;
        arrowAngle = 26.55f;
        adjustValue = lineWidth * 1.13f;
        break;
    }
  }

  private void GetOpenNarrowArrowDefaultValues(
    LineEndLength arrowHeadLength,
    float lineWidth,
    ref float arrowLength,
    ref float arrowAngle,
    ref float adjustValue)
  {
    switch (arrowHeadLength)
    {
      case LineEndLength.ShortArrow:
        arrowLength = (double) lineWidth > 1.0 ? lineWidth * 2.8f : 5f;
        arrowAngle = 32f;
        adjustValue = lineWidth * 0.9f;
        break;
      case LineEndLength.MediumLenArrow:
        arrowLength = (double) lineWidth > 1.0 ? (float) Math.Round((double) lineWidth * 3.5) : 7f;
        arrowAngle = 22f;
        adjustValue = lineWidth * 1.3f;
        break;
      case LineEndLength.LongArrow:
        arrowLength = (double) lineWidth > 1.0 ? (float) Math.Round((double) lineWidth * 5.0) : 9.5f;
        arrowAngle = 15.5f;
        adjustValue = lineWidth * 1.83f;
        break;
    }
  }

  private void GetOpenMediumArrowDefaultValues(
    LineEndLength arrowHeadLength,
    float lineWidth,
    ref float arrowLength,
    ref float arrowAngle,
    ref float adjustValue)
  {
    switch (arrowHeadLength)
    {
      case LineEndLength.ShortArrow:
        arrowLength = (double) lineWidth > 1.0 ? lineWidth * 3f : 5.5f;
        arrowAngle = 41f;
        adjustValue = lineWidth * 0.75f;
        break;
      case LineEndLength.MediumLenArrow:
        arrowLength = (double) lineWidth > 1.0 ? (float) Math.Round((double) lineWidth * 3.8) : 7f;
        arrowAngle = 30f;
        adjustValue = lineWidth;
        break;
      case LineEndLength.LongArrow:
        arrowLength = (double) lineWidth > 1.0 ? (float) Math.Round((double) lineWidth * 5.0) : 10f;
        arrowAngle = 21f;
        adjustValue = lineWidth * 1.35f;
        break;
    }
  }

  private void GetOpenWideArrowDefaultValues(
    LineEndLength arrowHeadLength,
    float lineWidth,
    ref float arrowLength,
    ref float arrowAngle,
    ref float adjustValue)
  {
    switch (arrowHeadLength)
    {
      case LineEndLength.ShortArrow:
        arrowLength = (double) lineWidth > 1.0 ? lineWidth * 3.7f : 6.5f;
        arrowAngle = 52f;
        adjustValue = lineWidth * 0.65f;
        break;
      case LineEndLength.MediumLenArrow:
        arrowLength = (double) lineWidth > 1.0 ? (float) Math.Round((double) lineWidth * 4.2) : 8f;
        arrowAngle = 40f;
        adjustValue = lineWidth;
        break;
      case LineEndLength.LongArrow:
        arrowLength = (double) lineWidth > 1.0 ? (float) Math.Round((double) lineWidth * 5.6999998092651367) : 10.5f;
        arrowAngle = 29f;
        adjustValue = lineWidth;
        break;
    }
  }

  private double FindAngleToLeftAndRightHeadPoint(
    bool isFlipHorizontal,
    bool isFlipVertical,
    float width,
    PointF point1,
    PointF point2,
    bool isFromBeginArrow)
  {
    double andRightHeadPoint;
    if (isFlipHorizontal && isFlipVertical)
    {
      andRightHeadPoint = (double) width != 0.0 ? 180.0 - this.RadianToDegree(this.FindArrowHeadAngleRadians(point1, point2, true)) : 360.0 - this.RadianToDegree(this.FindArrowHeadAngleRadians(point1, point2, true));
      if (isFromBeginArrow && (double) width != 0.0)
        andRightHeadPoint -= 180.0;
    }
    else if (isFlipVertical || isFlipHorizontal)
    {
      andRightHeadPoint = 360.0 - this.RadianToDegree(this.FindArrowHeadAngleRadians(point1, point2, false));
    }
    else
    {
      andRightHeadPoint = 360.0 - this.RadianToDegree(this.FindArrowHeadAngleRadians(point1, point2, true));
      if (isFromBeginArrow && (double) width != 0.0)
        andRightHeadPoint -= 180.0;
    }
    return andRightHeadPoint;
  }

  private double FindAngleToLeftAndRightHeadPoint(
    Shape shape,
    PointF point1,
    PointF point2,
    bool isFromBeginArrow)
  {
    double andRightHeadPoint;
    if (shape.FlipHorizontal && shape.FlipVertical)
    {
      andRightHeadPoint = (double) shape.Width != 0.0 ? 180.0 - this.RadianToDegree(this.FindArrowHeadAngleRadians(point1, point2, true)) : 360.0 - this.RadianToDegree(this.FindArrowHeadAngleRadians(point1, point2, true));
      if (isFromBeginArrow && (double) shape.Width != 0.0)
        andRightHeadPoint -= 180.0;
    }
    else if (shape.FlipVertical || shape.FlipHorizontal)
    {
      andRightHeadPoint = 360.0 - this.RadianToDegree(this.FindArrowHeadAngleRadians(point1, point2, false));
    }
    else
    {
      andRightHeadPoint = 360.0 - this.RadianToDegree(this.FindArrowHeadAngleRadians(point1, point2, true));
      if (isFromBeginArrow && (double) shape.Width != 0.0)
        andRightHeadPoint -= 180.0;
    }
    return andRightHeadPoint;
  }

  private double FindArrowHeadAngleRadians(
    PointF point1,
    PointF point2,
    bool isFromSeparateOrientation)
  {
    PointF pointF1 = new PointF(isFromSeparateOrientation ? point1.X : 0.0f, point2.Y);
    PointF pointF2 = point2;
    PointF pointF3 = point2;
    PointF pointF4 = point1;
    return Math.Atan2((double) pointF2.Y - (double) pointF1.Y, (double) pointF2.X - (double) pointF1.X) - Math.Atan2((double) pointF4.Y - (double) pointF3.Y, (double) pointF4.X - (double) pointF3.X);
  }

  private PointF FindBaseLineEndPoint(
    bool isFlipHorizontal,
    bool isFlipVertical,
    float width,
    float height,
    PointF[] linePoints,
    float adjustValue,
    bool isFromBeginArrow)
  {
    float x = 0.0f;
    float y = 0.0f;
    if (isFlipHorizontal && isFlipVertical || isFlipHorizontal)
    {
      double degree = (double) width != 0.0 ? 180.0 - this.RadianToDegree(this.FindAngleRadians(linePoints, false)) : 360.0 - this.RadianToDegree(this.FindAngleRadians(linePoints, false));
      this.GetEndPointForBaseLine(isFromBeginArrow, degree, Math.Sqrt((double) width * (double) width + (double) height * (double) height), adjustValue, linePoints, ref x, ref y);
    }
    else if (isFlipVertical)
    {
      double degree = 360.0 - this.RadianToDegree(this.FindAngleRadians(linePoints, false));
      this.GetEndPointForBaseLine(isFromBeginArrow, degree, Math.Sqrt((double) width * (double) width + (double) height * (double) height), adjustValue, linePoints, ref x, ref y);
    }
    else
    {
      double degree = this.RadianToDegree(this.FindAngleRadians(linePoints, true));
      this.GetEndPointForBaseLine(isFromBeginArrow, degree, Math.Sqrt((double) width * (double) width + (double) height * (double) height), adjustValue, linePoints, ref x, ref y);
    }
    return new PointF(x, y);
  }

  private void GetEndPointForBaseLine(
    bool isFromBeginArrow,
    double degree,
    double length,
    float adjustValue,
    PointF[] linePoints,
    ref float x,
    ref float y)
  {
    if (isFromBeginArrow)
    {
      degree -= 180.0;
      this.GetEndPoint(this.Degree2Radian(degree), (float) length - adjustValue, linePoints[1].X, linePoints[1].Y, ref x, ref y);
    }
    else
      this.GetEndPoint(this.Degree2Radian(degree), (float) length - adjustValue, linePoints[0].X, linePoints[0].Y, ref x, ref y);
  }

  private double FindAngleRadians(PointF[] linePoints, bool isFromBottomToTop)
  {
    PointF linePoint1 = linePoints[0];
    PointF pointF1 = new PointF(linePoints[1].X, isFromBottomToTop ? linePoints[1].Y : linePoints[0].Y);
    PointF linePoint2 = linePoints[0];
    PointF pointF2 = new PointF(linePoints[1].X, isFromBottomToTop ? linePoints[0].Y : linePoints[1].Y);
    return Math.Atan2((double) pointF1.Y - (double) linePoint1.Y, (double) pointF1.X - (double) linePoint1.X) - Math.Atan2((double) pointF2.Y - (double) linePoint2.Y, (double) pointF2.X - (double) linePoint2.X);
  }

  private PointF[] FindArrowHeadPoints(
    Shape shape,
    Pen pen,
    RectangleF bounds,
    PointF[] linePoints,
    bool isFromOpenArrow,
    bool isFromBeginArrow)
  {
    PointF[] points = new PointF[4];
    float arrowLength = 0.0f;
    float arrowAngle = 0.0f;
    float adjustValue = 0.0f;
    this.GetArrowDefaultValues(shape.LineFormat, pen, ref arrowLength, ref arrowAngle, ref adjustValue, isFromOpenArrow, isFromBeginArrow);
    points[0] = this.FindBaseLineEndPoint(shape.FlipHorizontal, shape.FlipVertical, bounds.Width, bounds.Height, linePoints, adjustValue, isFromBeginArrow);
    this.FindLeftRightHeadPoints(shape.FlipHorizontal, shape.FlipVertical, bounds.Width, linePoints, ref points, arrowAngle, arrowLength, isFromBeginArrow);
    return points;
  }

  private PointF[] FindArrowHeadPoints(
    ChildShape shape,
    Pen pen,
    RectangleF bounds,
    PointF[] linePoints,
    bool isFromOpenArrow,
    bool isFromBeginArrow)
  {
    PointF[] points = new PointF[4];
    float arrowLength = 0.0f;
    float arrowAngle = 0.0f;
    float adjustValue = 0.0f;
    this.GetArrowDefaultValues(shape.LineFormat, pen, ref arrowLength, ref arrowAngle, ref adjustValue, isFromOpenArrow, isFromBeginArrow);
    points[0] = this.FindBaseLineEndPoint(shape.FlipHorizantal, shape.FlipVertical, shape.Width, shape.Height, linePoints, adjustValue, isFromBeginArrow);
    this.FindLeftRightHeadPoints(shape.FlipHorizantal, shape.FlipHorizantal, shape.Width, linePoints, ref points, arrowAngle, arrowLength, isFromBeginArrow);
    return points;
  }

  private void FindLeftRightHeadPoints(
    bool isFlipHorizontal,
    bool isFlipVertical,
    float width,
    PointF[] linePoints,
    ref PointF[] points,
    float arrowAngle,
    float arrowLength,
    bool isFromBeginArrow)
  {
    PointF point1 = new PointF();
    PointF point2 = new PointF();
    this.ConstrucBasetLine(isFromBeginArrow, points[0], linePoints, ref point1, ref point2);
    double andRightHeadPoint = this.FindAngleToLeftAndRightHeadPoint(isFlipHorizontal, isFlipVertical, width, point1, point2, isFromBeginArrow);
    float end_x = 0.0f;
    float end_y = 0.0f;
    this.GetEndPoint(this.Degree2Radian(andRightHeadPoint - (double) arrowAngle), arrowLength, point2.X, point2.Y, ref end_x, ref end_y);
    points[1] = new PointF(end_x, end_y);
    points[2] = new PointF(point2.X, point2.Y);
    this.GetEndPoint(this.Degree2Radian(andRightHeadPoint + (double) arrowAngle), arrowLength, point2.X, point2.Y, ref end_x, ref end_y);
    points[3] = new PointF(end_x, end_y);
  }

  private void ConstrucBasetLine(
    bool isFromBeginArrow,
    PointF points,
    PointF[] linePoints,
    ref PointF point1,
    ref PointF point2)
  {
    if (isFromBeginArrow)
    {
      point1 = new PointF(linePoints[1].X, linePoints[1].Y);
      point2 = points;
    }
    else
    {
      point1 = new PointF(linePoints[0].X, linePoints[0].Y);
      point2 = points;
    }
  }

  private void GetArrowDefaultValues(
    LineFormat lineFormat,
    Pen pen,
    ref float arrowLength,
    ref float arrowAngle,
    ref float adjustValue,
    bool isFromOpenArrow,
    bool isFromBeginArrow)
  {
    if (isFromOpenArrow)
      this.GetOpenArrowDefaultValues(lineFormat, pen.Width, ref arrowLength, ref arrowAngle, ref adjustValue, isFromBeginArrow);
    else
      this.GetCloseArrowDefaultValues(lineFormat, pen.Width, ref arrowLength, ref arrowAngle, ref adjustValue, isFromBeginArrow);
  }

  private double RadianToDegree(double angle) => angle * (180.0 / Math.PI);

  private double Degree2Radian(double a) => a * (Math.PI / 180.0);

  private void GetEndPoint(
    double angle,
    float len,
    float start_x,
    float start_y,
    ref float end_x,
    ref float end_y)
  {
    end_x = start_x + len * (float) Math.Cos(angle);
    end_y = start_y + len * (float) Math.Sin(angle);
  }

  private PointF[] GetLinePointsBasedOnFlip(
    bool isFlipHorizontal,
    bool isFlipVertical,
    RectangleF bounds)
  {
    PointF[] pointsBasedOnFlip = new PointF[2];
    if (isFlipHorizontal && isFlipVertical)
    {
      pointsBasedOnFlip[0] = new PointF(bounds.Right, bounds.Bottom);
      pointsBasedOnFlip[1] = new PointF(bounds.X, bounds.Y);
    }
    else if (isFlipVertical)
    {
      pointsBasedOnFlip[0] = new PointF(bounds.X, bounds.Bottom);
      pointsBasedOnFlip[1] = new PointF(bounds.Right, bounds.Y);
    }
    else if (isFlipHorizontal)
    {
      pointsBasedOnFlip[0] = new PointF(bounds.Right, bounds.Y);
      pointsBasedOnFlip[1] = new PointF(bounds.X, bounds.Bottom);
    }
    else
    {
      pointsBasedOnFlip[0] = new PointF(bounds.X, bounds.Y);
      pointsBasedOnFlip[1] = new PointF(bounds.Right, bounds.Bottom);
    }
    return pointsBasedOnFlip;
  }

  private bool IsSoftHyphen(LayoutedWidget ltWidget)
  {
    bool flag = false;
    if (ltWidget != null)
    {
      WTextRange wtextRange = ltWidget.Widget is WTextRange ? ltWidget.Widget as WTextRange : (ltWidget.Widget is SplitStringWidget ? (ltWidget.Widget as SplitStringWidget).RealStringWidget as WTextRange : (WTextRange) null);
      if (wtextRange != null && wtextRange.Text == '\u001F'.ToString() && (double) ltWidget.Bounds.Width > 0.0)
        flag = true;
    }
    return flag;
  }

  private StringFormat GetStringFormat(WCharacterFormat charFormat)
  {
    StringFormat stringFormat = new StringFormat(StringFormat.GenericTypographic);
    stringFormat.FormatFlags &= ~StringFormatFlags.LineLimit;
    stringFormat.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
    stringFormat.FormatFlags |= StringFormatFlags.NoClip;
    stringFormat.Trimming = StringTrimming.Word;
    if (charFormat.Bidi)
      stringFormat.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
    return stringFormat;
  }

  private SolidBrush GetBrush(Color color) => this.CreateSolidBrush(color);

  public Color GetTextColor(WCharacterFormat charFormat)
  {
    Color textColor = Color.Black;
    bool flag = false;
    WParagraph wparagraph = this.currParagraph;
    if (this.currTextRange != null && !(this.currTextRange.Owner is InlineContentControl))
      wparagraph = this.currTextRange.Owner as WParagraph;
    Entity entity = (Entity) null;
    if (wparagraph != null)
      entity = wparagraph.GetOwnerEntity();
    if (this.currTextRange != null && this.currTextRange.Document.RevisionOptions.ShowRevisionMarks)
    {
      RevisionOptions revisionOptions = this.currTextRange.Document.RevisionOptions;
      if (charFormat.IsInsertRevision && charFormat.IsNeedToShowInsertionMarkups())
        return this.GetRevisionColor(revisionOptions.InsertedTextColor, true);
      if (charFormat.IsDeleteRevision && charFormat.IsNeedToShowDeletionMarkups())
        return this.GetRevisionColor(revisionOptions.DeletedTextColor);
    }
    if (this.isTOCParagraphInHyperLink(this.currTextRange))
    {
      if (charFormat.PropertiesHash.ContainsKey(1))
      {
        WCharacterStyle charStyle = charFormat.CharStyle;
        textColor = charStyle == null || !(charStyle.Name == "Hyperlink") || !(charStyle.CharacterFormat.TextColor == charFormat.TextColor) ? charFormat.TextColor : (wparagraph != null ? (wparagraph.ParaStyle != null ? wparagraph.ParaStyle.CharacterFormat.TextColor : Color.Black) : Color.Black);
      }
      else
        textColor = wparagraph != null ? (wparagraph.ParaStyle != null ? wparagraph.ParaStyle.CharacterFormat.TextColor : Color.Black) : Color.Black;
      flag = textColor == Color.Empty;
    }
    else if (!charFormat.TextColor.IsEmpty)
    {
      textColor = charFormat.TextColor;
    }
    else
    {
      switch (entity)
      {
        case WTextBox _ when !(entity as WTextBox).TextBoxFormat.TextThemeColor.IsEmpty:
          textColor = (entity as WTextBox).TextBoxFormat.TextThemeColor;
          break;
        case Shape _:
          Color fontRefColor1 = (entity as Shape).FontRefColor;
          if (fontRefColor1 != Color.Empty)
          {
            textColor = fontRefColor1;
            if (!charFormat.TextBackgroundColor.IsEmpty)
            {
              textColor = WordColor.IsNotVeryDarkColor(charFormat.TextBackgroundColor) ? Color.Black : Color.White;
              break;
            }
            if (!wparagraph.ParagraphFormat.BackGroundColor.IsEmpty)
            {
              textColor = WordColor.IsNotVeryDarkColor(wparagraph.ParagraphFormat.BackGroundColor) ? Color.Black : Color.White;
              break;
            }
            if (!wparagraph.ParagraphFormat.BackColor.IsEmpty)
            {
              textColor = WordColor.IsNotVeryDarkColor(wparagraph.ParagraphFormat.BackColor) ? Color.Black : Color.White;
              break;
            }
            break;
          }
          flag = true;
          break;
        case ChildShape _:
          Color fontRefColor2 = (entity as ChildShape).FontRefColor;
          if (fontRefColor2 != Color.Empty)
          {
            textColor = fontRefColor2;
            break;
          }
          flag = true;
          break;
        default:
          flag = true;
          break;
      }
    }
    if (flag && wparagraph != null)
    {
      textColor = Color.Black;
      if (wparagraph.IsInCell && wparagraph.GetOwnerEntity() is WTableCell)
      {
        CellFormat cellFormat = (wparagraph.GetOwnerEntity() as WTableCell).CellFormat;
        if (!cellFormat.BackColor.IsEmpty && !WordColor.IsNotVeryDarkColor(cellFormat.BackColor))
          textColor = Color.White;
        TextureStyle textureStyle = cellFormat.TextureStyle;
        if (textureStyle != TextureStyle.TextureNone)
        {
          Color foreColor = cellFormat.ForeColor;
          Color backColor = cellFormat.BackColor;
          if (textureStyle.ToString().Contains("Percent"))
          {
            if (backColor.IsEmpty)
              backColor = Color.White;
            float percent = float.Parse(textureStyle.ToString().Replace("Texture", "").Replace("Percent", "").Replace("Pt", "."), (IFormatProvider) CultureInfo.InvariantCulture);
            foreColor = this.GetForeColor(foreColor, backColor, percent);
          }
          textColor = (foreColor.IsEmpty || !textureStyle.ToString().Contains("Percent")) && textureStyle != TextureStyle.TextureSolid || WordColor.IsNotVeryDarkColor(foreColor) ? Color.Black : Color.White;
        }
      }
      if (entity is WTextBox && (entity as WTextBox).TextBoxFormat != null && !(entity as WTextBox).TextBoxFormat.FillColor.IsEmpty && !WordColor.IsNotVeryDarkColor((entity as WTextBox).TextBoxFormat.FillColor) || entity is Shape && !(entity as Shape).FillFormat.Color.IsEmpty && !WordColor.IsNotVeryDarkColor((entity as Shape).FillFormat.Color))
        textColor = Color.White;
      if (!wparagraph.ParagraphFormat.BackColor.IsEmpty && !WordColor.IsNotVeryDarkColor(wparagraph.ParagraphFormat.BackColor))
        textColor = Color.White;
      if (!charFormat.TextBackgroundColor.IsEmpty)
        textColor = !WordColor.IsNotVeryDarkColor(charFormat.TextBackgroundColor) ? Color.White : Color.Black;
    }
    if (textColor == Color.Transparent)
      textColor = Color.White;
    return textColor;
  }

  public Font GetFont(WTextRange txtRange, WCharacterFormat charFormat, string text)
  {
    string str = (string) null;
    FontStyle fontStyle1 = FontStyle.Regular;
    if (txtRange != null && txtRange.OwnerParagraph != null && txtRange.OwnerParagraph == null)
      return this.GetFont(txtRange.ScriptType, charFormat, text);
    if (txtRange != null)
      str = txtRange.CharacterFormat.GetFontNameToRender(txtRange.ScriptType);
    float fontSize = charFormat.GetFontSizeToRender();
    if (txtRange != null && txtRange.CharacterFormat != null)
    {
      string charStyleName = txtRange.CharacterFormat.CharStyleName;
      FontStyle fontStyle2 = charFormat.GetFontStyle();
      if (!(charStyleName == "Hyperlink") || !this.IsTOC(txtRange))
        fontStyle1 = fontStyle2;
      if (this.IsTOC(txtRange))
        fontStyle1 = txtRange.CharacterFormat.CharStyle == null || (charStyleName == null || !(charStyleName.ToLower() == "hyperlink")) && txtRange.CharacterFormat.CharStyle.StyleId != 85 ? fontStyle2 : fontStyle2 & ~txtRange.CharacterFormat.CharStyle.CharacterFormat.GetFontStyle();
    }
    if (txtRange != null)
    {
      if (txtRange.Text.Trim(' ') == string.Empty)
      {
        ILayoutInfo layoutInfo = ((IWidget) txtRange).LayoutInfo;
      }
    }
    if (charFormat.HasValue(72) && this.IsUnicodeText(text) && txtRange != null)
      str = charFormat.GetFontNameFromHint(txtRange.ScriptType);
    if (str == "Times New Roman Bold")
      str = "Times New Roman";
    if ((double) fontSize == 0.0)
      fontSize = 0.5f;
    Font font;
    if (str == "ArialUnicodeMS")
    {
      string fontName = !charFormat.Document.FontSubstitutionTable.ContainsKey(str) ? "Arial" : charFormat.Document.FontSubstitutionTable[str];
      font = charFormat.Document.FontSettings.GetFont(fontName, fontSize, fontStyle1);
    }
    else
    {
      font = charFormat.Document.FontSettings.GetFont(str, fontSize, fontStyle1);
      if (this.FontNames != null && !this.FontNames.ContainsValue(font.Name))
        this.UpdateAlternateFont(charFormat, str, ref font);
    }
    if (txtRange != null && (txtRange.CharacterFormat.Bidi || txtRange.CharacterFormat.ComplexScript))
      font = this.UpdateBidiFont(txtRange.ScriptType, txtRange.CharacterFormat, font, fontSize, txtRange.CharacterFormat.GetFontStyle());
    return font;
  }

  private Color GetRevisionColor(RevisionColor revisionColor)
  {
    return this.GetRevisionColor(revisionColor, false);
  }

  private Color GetRevisionColor(RevisionColor revisionColor, bool isInsertText)
  {
    return this.GetRevisionColor(revisionColor, isInsertText, false);
  }

  private Color GetRevisionColor(
    RevisionColor revisionColor,
    bool isInsertText,
    bool isResolvedComment)
  {
    switch (revisionColor)
    {
      case RevisionColor.ByAuthor:
      case RevisionColor.Auto:
        if (isInsertText)
          return Color.FromArgb(46, 151, 211);
        return isResolvedComment ? Color.FromArgb(252, 177, 194) : Color.FromArgb(181, 8, 46);
      case RevisionColor.Black:
        return isResolvedComment ? Color.FromArgb(191, 191, 191) : Color.FromArgb(0, 0, 0);
      case RevisionColor.Blue:
        return isResolvedComment ? Color.FromArgb(202, 228, 244) : Color.FromArgb(46, 151, 211);
      case RevisionColor.BrightGreen:
        return isResolvedComment ? Color.FromArgb(224 /*0xE0*/, 232, 215) : Color.FromArgb(132, 163, 91);
      case RevisionColor.DarkBlue:
        return isResolvedComment ? Color.FromArgb(199, 220, 235) : Color.FromArgb(55, 110, 150);
      case RevisionColor.DarkRed:
        return isResolvedComment ? Color.FromArgb(244, 198, 202) : Color.FromArgb(136, 24, 36);
      case RevisionColor.DarkYellow:
        return isResolvedComment ? Color.FromArgb(248, 231, 201) : Color.FromArgb(224 /*0xE0*/, 154, 43);
      case RevisionColor.Gray25:
        return isResolvedComment ? Color.FromArgb(231, 232, 233) : Color.FromArgb(160 /*0xA0*/, 163, 169);
      case RevisionColor.Gray50:
        return isResolvedComment ? Color.FromArgb(209, 213, 216) : Color.FromArgb(80 /*0x50*/, 86, 94);
      case RevisionColor.Green:
        return isResolvedComment ? Color.FromArgb(190, 226, 196) : Color.FromArgb(44, 98, 52);
      case RevisionColor.Pink:
        return isResolvedComment ? Color.FromArgb(242, 204, 227) : Color.FromArgb(206, 51, 143);
      case RevisionColor.Red:
        return isResolvedComment ? Color.FromArgb(252, 177, 194) : Color.FromArgb(181, 8, 46);
      case RevisionColor.Teal:
        return isResolvedComment ? Color.FromArgb(187, 239, 244) : Color.FromArgb(27, 156, 171);
      case RevisionColor.Turquoise:
        return isResolvedComment ? Color.FromArgb(218, 239, 242) : Color.FromArgb(62, 175, 194);
      case RevisionColor.Violet:
        return isResolvedComment ? Color.FromArgb(219, 196, 230) : Color.FromArgb(99, 50, 119);
      case RevisionColor.White:
        return Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      case RevisionColor.Yellow:
        return isResolvedComment ? Color.FromArgb(254, 243, 218) : Color.FromArgb(250, 210, 114);
      case RevisionColor.ClassicRed:
        return isResolvedComment ? Color.FromArgb((int) byte.MaxValue, 191, 191) : Color.FromArgb((int) byte.MaxValue, 0, 0);
      case RevisionColor.ClassicBlue:
        return isResolvedComment ? Color.FromArgb(191, 191, (int) byte.MaxValue) : Color.FromArgb(0, 0, (int) byte.MaxValue);
      default:
        return Color.Empty;
    }
  }

  private Color GetRevisionFillColor(RevisionColor revisionColor, bool isResolvedComment)
  {
    switch (revisionColor)
    {
      case RevisionColor.ByAuthor:
      case RevisionColor.Auto:
        return isResolvedComment ? Color.FromArgb((int) byte.MaxValue, 244, 247) : Color.FromArgb(253, 215, 223);
      case RevisionColor.Black:
        return isResolvedComment ? Color.FromArgb(248, 248, 248) : Color.FromArgb(233, 233, 233);
      case RevisionColor.Blue:
        return isResolvedComment ? Color.FromArgb(247, 251, 253) : Color.FromArgb(220, 237, 248);
      case RevisionColor.BrightGreen:
        return isResolvedComment ? Color.FromArgb(251, 252, 248) : Color.FromArgb(235, 240 /*0xF0*/, 227);
      case RevisionColor.DarkBlue:
        return isResolvedComment ? Color.FromArgb(248, 250, 252) : Color.FromArgb(224 /*0xE0*/, 235, 243);
      case RevisionColor.DarkRed:
        return isResolvedComment ? Color.FromArgb(254, 245, 247) : Color.FromArgb(249, 219, 222);
      case RevisionColor.DarkYellow:
        return isResolvedComment ? Color.FromArgb(254, 251, 245) : Color.FromArgb(250, 238, 218);
      case RevisionColor.Gray25:
        return isResolvedComment ? Color.FromArgb(250, 250, 250) : Color.FromArgb(233, 235, 235);
      case RevisionColor.Gray50:
        return isResolvedComment ? Color.FromArgb(250, 250, 250) : Color.FromArgb(232, 234, 236);
      case RevisionColor.Green:
        return isResolvedComment ? Color.FromArgb(248, 252, 249) : Color.FromArgb(225, 242, 227);
      case RevisionColor.Pink:
        return isResolvedComment ? Color.FromArgb(253, 247, 250) : Color.FromArgb(247, 221, 236);
      case RevisionColor.Red:
        return isResolvedComment ? Color.FromArgb((int) byte.MaxValue, 244, 247) : Color.FromArgb(253, 215, 223);
      case RevisionColor.Teal:
        return isResolvedComment ? Color.FromArgb(245, 253, 254) : Color.FromArgb(218, 247, 250);
      case RevisionColor.Turquoise:
        return isResolvedComment ? Color.FromArgb(248, 251, 252) : Color.FromArgb(223, 241, 244);
      case RevisionColor.Violet:
        return isResolvedComment ? Color.FromArgb(251, 248, 252) : Color.FromArgb(237, 225, 242);
      case RevisionColor.White:
        return isResolvedComment ? Color.FromArgb(248, 248, 248) : Color.FromArgb(233, 233, 233);
      case RevisionColor.Yellow:
        return isResolvedComment ? Color.FromArgb((int) byte.MaxValue, 252, 244) : Color.FromArgb(254, 242, 214);
      case RevisionColor.ClassicRed:
        return isResolvedComment ? Color.FromArgb((int) byte.MaxValue, 244, 244) : Color.FromArgb((int) byte.MaxValue, 213, 213);
      case RevisionColor.ClassicBlue:
        return isResolvedComment ? Color.FromArgb(244, 244, (int) byte.MaxValue) : Color.FromArgb(213, 213, (int) byte.MaxValue);
      default:
        return Color.Empty;
    }
  }

  private void UpdateAlternateFont(WCharacterFormat charFormat, string fontName, ref Font font)
  {
    if (charFormat.Document != null && charFormat.Document.FontSubstitutionTable.ContainsKey(fontName) && fontName != font.Name)
      fontName = charFormat.Document.FontSubstitutionTable[fontName];
    if (charFormat.Document == null)
      return;
    font = charFormat.Document.FontSettings.GetFont(fontName, font.Size, font.Style);
  }

  public bool IsTOC(WTextRange txtRange)
  {
    return txtRange != null && txtRange.Owner is WParagraph && txtRange.OwnerParagraph.ChildEntities.FirstItem != null && (this.ParagraphContainsTOC(txtRange) || this.ParagraphContainsHyperlink((Entity) txtRange));
  }

  private bool ParagraphContainsTOC(WTextRange txtRange)
  {
    WParagraph ownerParagraph = txtRange.OwnerParagraph;
    for (int index = 0; index < ownerParagraph.ChildEntities.Count; ++index)
    {
      if (ownerParagraph.Items[index] is TableOfContent)
        return true;
    }
    return false;
  }

  private bool ParagraphContainsHyperlink(Entity entity)
  {
    if (entity.Owner is WParagraph owner)
    {
      for (int index = 0; index < owner.ChildEntities.Count; ++index)
      {
        if (owner.ChildEntities[index] is WField)
          return (owner.ChildEntities[index] as WField).FieldType == FieldType.FieldHyperlink && new Hyperlink(owner.ChildEntities[index] as WField).BookmarkName != null && new Hyperlink(owner.ChildEntities[index] as WField).BookmarkName.StartsWithExt("_Toc");
      }
    }
    return false;
  }

  internal bool isTOCParagraphInHyperLink(WTextRange txtRange)
  {
    if (txtRange != null && txtRange.Owner is WParagraph && txtRange.OwnerParagraph.ChildEntities.FirstItem != null)
    {
      IWParagraph owner = (IWParagraph) (txtRange.Owner as WParagraph);
      if (this.ParagraphContainsTOC(txtRange))
      {
        for (int index = 1; index < owner.ChildEntities.Count; ++index)
        {
          if (owner.ChildEntities[index] is WField && this.ParagraphContainsHyperlink(owner.ChildEntities[index]))
          {
            if (this.IsTextRangeFound(owner.ChildEntities[index] as WField, txtRange))
              return true;
            break;
          }
        }
      }
      else if (this.ParagraphContainsHyperlink(owner.ChildEntities.FirstItem))
      {
        for (int index = 0; index < owner.ChildEntities.Count; ++index)
        {
          if (owner.ChildEntities[index] is WField && this.ParagraphContainsHyperlink(owner.ChildEntities[index]) && this.IsTextRangeFound(owner.ChildEntities[index] as WField, txtRange))
            return true;
        }
      }
    }
    return false;
  }

  private bool IsTextRangeFound(WField hyperLinkField, WTextRange textRange)
  {
    if (hyperLinkField != null)
    {
      for (int index = 0; index < hyperLinkField.Range.Count; ++index)
      {
        if (hyperLinkField.Range.InnerList[index] is WTextRange && hyperLinkField.Range.InnerList[index] == textRange)
          return true;
      }
    }
    return false;
  }

  public Font GetFont(FontScriptType scriptType, WCharacterFormat charFormat, string text)
  {
    string str = charFormat.GetFontNameToRender(scriptType);
    float fontSize = charFormat.GetFontSizeToRender();
    FontStyle fontStyle = charFormat.GetFontStyle();
    if (charFormat.HasValue(72) && this.IsUnicodeText(text))
      str = charFormat.GetFontNameFromHint(scriptType);
    if (str == "Times New Roman Bold")
      str = "Times New Roman";
    if ((double) fontSize == 0.0)
      fontSize = 0.5f;
    Font font;
    if (str == "ArialUnicodeMS")
    {
      string fontName = !charFormat.Document.FontSubstitutionTable.ContainsKey(str) ? "Arial" : charFormat.Document.FontSubstitutionTable[str];
      font = charFormat.Document.FontSettings.GetFont(fontName, fontSize, fontStyle);
    }
    else
    {
      font = charFormat.Document.FontSettings.GetFont(str, fontSize, fontStyle);
      if (this.FontNames != null && !this.FontNames.ContainsValue(font.Name))
        this.UpdateAlternateFont(charFormat, str, ref font);
    }
    if (charFormat.Bidi || charFormat.ComplexScript)
      font = this.UpdateBidiFont(scriptType, charFormat, font, fontSize, fontStyle);
    return font;
  }

  private Font UpdateBidiFont(
    FontScriptType scriptType,
    WCharacterFormat charFormat,
    Font font,
    float fontSize,
    FontStyle fontStyle)
  {
    if ((double) charFormat.FontSize != (double) charFormat.GetFontSizeToRender())
      font = charFormat.Document.FontSettings.GetFont(font.Name, charFormat.GetFontSizeToRender(), font.Style);
    return font;
  }

  private bool HasPrivateFont(string fontName)
  {
    foreach (FontFamily family in this.PrivateFonts.Families)
    {
      if (family.Name.Equals(fontName, StringComparison.OrdinalIgnoreCase) || this.FontNames.ContainsKey(fontName))
        return true;
    }
    return false;
  }

  private StringAlignment GetStringAlignment(WParagraphFormat paraFormat)
  {
    StringAlignment stringAlignment = StringAlignment.Near;
    switch (paraFormat.GetAlignmentToRender())
    {
      case HorizontalAlignment.Center:
        stringAlignment = StringAlignment.Center;
        break;
      case HorizontalAlignment.Right:
        stringAlignment = StringAlignment.Far;
        break;
    }
    return stringAlignment;
  }

  private Pen GetPen(Border border, bool isParagraphBorder)
  {
    float lineWidth = border.LineWidth;
    Color color = Color.Black;
    if (!border.Color.IsEmpty && border.Color.ToArgb() != 0)
      color = border.Color;
    Pen pen = this.CreatePen(color, border.LineWidth);
    switch (border.BorderType)
    {
      case BorderStyle.Double:
        if (!isParagraphBorder)
        {
          pen.Width = lineWidth * 3f;
          pen.CompoundArray = new float[4]
          {
            0.0f,
            0.3333333f,
            0.6666667f,
            1f
          };
          break;
        }
        break;
      case BorderStyle.Dot:
        pen.DashPattern = new float[2]{ 1f, 2f };
        break;
      case BorderStyle.DashLargeGap:
        pen.DashPattern = new float[2]{ 3f, 5f };
        break;
      case BorderStyle.DotDash:
        pen.DashPattern = new float[4]{ 4f, 3f, 1f, 3f };
        break;
      case BorderStyle.DotDotDash:
        pen.DashPattern = new float[6]
        {
          8f,
          3f,
          1f,
          3f,
          1f,
          3f
        };
        break;
      case BorderStyle.Triple:
        pen.Width = lineWidth * 5f;
        pen.CompoundArray = new float[6]
        {
          0.0f,
          0.1666667f,
          0.3333333f,
          0.6666667f,
          0.8333333f,
          1f
        };
        break;
      case BorderStyle.DashSmallGap:
        pen.DashPattern = new float[2]{ 3f, 3f };
        break;
      case BorderStyle.Outset:
        pen.Alignment = PenAlignment.Outset;
        break;
      case BorderStyle.Inset:
        pen.Alignment = PenAlignment.Inset;
        break;
    }
    if (!isParagraphBorder)
    {
      pen.StartCap = System.Drawing.Drawing2D.LineCap.Square;
      pen.EndCap = System.Drawing.Drawing2D.LineCap.Square;
    }
    return pen;
  }

  private Pen GetPen(BorderStyle borderType, float borderLineWidth, Color borderColor)
  {
    Color color = Color.Black;
    if (!borderColor.IsEmpty && borderColor.ToArgb() != 0)
      color = borderColor;
    Pen pen = this.CreatePen(color, borderLineWidth);
    switch (borderType)
    {
      case BorderStyle.Dot:
        pen.DashPattern = new float[2]{ 1f, 2f };
        break;
      case BorderStyle.DashLargeGap:
        pen.DashPattern = new float[2]{ 3f, 5f };
        break;
      case BorderStyle.DotDash:
        pen.DashPattern = new float[4]{ 4f, 3f, 1f, 3f };
        break;
      case BorderStyle.DotDotDash:
        pen.DashPattern = new float[6]
        {
          8f,
          3f,
          1f,
          3f,
          1f,
          3f
        };
        break;
      case BorderStyle.DashSmallGap:
        pen.DashPattern = new float[2]{ 3f, 3f };
        break;
      case BorderStyle.Outset:
        pen.Alignment = PenAlignment.Outset;
        break;
      case BorderStyle.Inset:
        pen.Alignment = PenAlignment.Inset;
        break;
    }
    return pen;
  }

  private Pen GetPen(UnderlineStyle underlineStyle, float lineWidth, Color lineColor)
  {
    Color color = Color.Black;
    if (!lineColor.IsEmpty && lineColor.ToArgb() != 0)
      color = lineColor;
    Pen pen = this.CreatePen(color, lineWidth);
    switch (underlineStyle)
    {
      case UnderlineStyle.Dotted:
        pen.DashPattern = new float[2]{ 1f, 2f };
        break;
      case UnderlineStyle.Dash:
        pen.DashPattern = new float[2]{ 3f, 5f };
        break;
      case UnderlineStyle.DotDash:
        pen.DashPattern = new float[4]{ 4f, 3f, 1f, 3f };
        break;
      case UnderlineStyle.DotDotDash:
        pen.DashPattern = new float[6]
        {
          8f,
          3f,
          1f,
          3f,
          1f,
          3f
        };
        break;
      case UnderlineStyle.DashHeavy:
        pen.Width = lineWidth * 1.75f;
        pen.DashPattern = new float[2]{ 3.05f, 1.628f };
        break;
      case UnderlineStyle.DotDashHeavy:
        pen.Width = lineWidth * 1.75f;
        pen.DashPattern = new float[4]
        {
          2.25f,
          1.03f,
          0.5f,
          1.03f
        };
        break;
      case UnderlineStyle.DashLong:
        pen.DashPattern = new float[2]{ 10.8f, 5.41f };
        break;
      case UnderlineStyle.DashLongHeavy:
        pen.Width = lineWidth * 1.75f;
        pen.DashPattern = new float[2]{ 6.17f, 3.09f };
        break;
    }
    return pen;
  }

  private Image ScaleImage(Image srcImage, float width, float height)
  {
    if ((double) srcImage.Width <= (double) width && (double) srcImage.Height <= (double) height)
      return srcImage;
    int width1 = srcImage.Width;
    int height1 = srcImage.Height;
    int x1 = 0;
    int y1 = 0;
    int width2 = (int) width;
    int height2 = (int) height;
    if (width2 <= 0)
      width2 = 1;
    if (height2 <= 0)
      height2 = 1;
    Bitmap bmp = new Bitmap(width2, height2);
    bmp.SetResolution(srcImage.HorizontalResolution, srcImage.VerticalResolution);
    int x2 = 0;
    int y2 = 0;
    using (Graphics graphicsFromImage = this.GetGraphicsFromImage(bmp))
    {
      graphicsFromImage.InterpolationMode = InterpolationMode.HighQualityBicubic;
      graphicsFromImage.DrawImage(this.GetImage(srcImage), new RectangleF((float) x1, (float) y1, (float) width2, (float) height2), new RectangleF((float) x2, (float) y2, (float) width1, (float) height1), GraphicsUnit.Pixel);
      graphicsFromImage.Dispose();
    }
    return (Image) bmp;
  }

  private void AddLinkToBookmark(RectangleF bounds, string bookmarkName, bool isTargetNull)
  {
    DocumentLayouter.BookmarkHyperlink bmhyperlink1 = (DocumentLayouter.BookmarkHyperlink) null;
    for (int index = 0; index < DrawingContext.BookmarkHyperlinksList.Count; ++index)
    {
      foreach (KeyValuePair<string, DocumentLayouter.BookmarkHyperlink> keyValuePair in DrawingContext.BookmarkHyperlinksList[index])
      {
        if (keyValuePair.Key == bookmarkName)
        {
          bmhyperlink1 = keyValuePair.Value;
          break;
        }
      }
      if (bmhyperlink1 != null)
      {
        if (bmhyperlink1.SourceBounds != RectangleF.Empty)
        {
          float x = bmhyperlink1.SourceBounds.X;
          float y = bmhyperlink1.SourceBounds.Y;
          float width = bmhyperlink1.SourceBounds.Width;
          float height = bmhyperlink1.SourceBounds.Height;
          if ((double) x > (double) bounds.X)
            x = bounds.X;
          if ((double) y > (double) bounds.Y)
            y = bounds.Y;
          if ((double) bmhyperlink1.SourceBounds.Right < (double) bounds.Right)
            width = bounds.Right - x;
          if ((double) bmhyperlink1.SourceBounds.Bottom < (double) bounds.Bottom)
            height = bounds.Bottom - y;
          bmhyperlink1.SourceBounds = new RectangleF(x, y, width, height);
          this.UpdateBookmarkTargetBoundsAndPageNumber(bmhyperlink1, bmhyperlink1.HyperlinkValue);
        }
        else if (bmhyperlink1.TargetBounds != RectangleF.Empty)
        {
          bmhyperlink1.SourceBounds = bounds;
          bmhyperlink1.SourcePageNumber = DocumentLayouter.PageNumber;
        }
      }
    }
    if (bmhyperlink1 != null)
      return;
    Dictionary<string, DocumentLayouter.BookmarkHyperlink> dictionary = new Dictionary<string, DocumentLayouter.BookmarkHyperlink>();
    DocumentLayouter.BookmarkHyperlink bmhyperlink2 = new DocumentLayouter.BookmarkHyperlink();
    bmhyperlink2.HyperlinkValue = bookmarkName;
    bmhyperlink2.SourceBounds = bounds;
    bmhyperlink2.SourcePageNumber = DocumentLayouter.PageNumber;
    if (isTargetNull)
      bmhyperlink2.IsTargetNull = isTargetNull;
    else
      this.UpdateBookmarkTargetBoundsAndPageNumber(bmhyperlink2, bookmarkName);
    dictionary.Add(bmhyperlink2.HyperlinkValue, bmhyperlink2);
    DrawingContext.BookmarkHyperlinksList.Add(dictionary);
  }

  private void AddHyperLink(Hyperlink hyperlink, RectangleF bounds)
  {
    string str1 = "";
    string str2 = string.Empty;
    if (hyperlink.Field.IsLocal && hyperlink.Field.LocalReference != null && hyperlink.Field.LocalReference != string.Empty)
      str2 = "#" + hyperlink.Field.LocalReference;
    switch (hyperlink.Type)
    {
      case HyperlinkType.FileLink:
        str1 = hyperlink.FilePath + str2;
        break;
      case HyperlinkType.WebLink:
        str1 = hyperlink.Uri + str2;
        break;
      case HyperlinkType.EMailLink:
        str1 = hyperlink.Uri;
        break;
      case HyperlinkType.Bookmark:
        string bookmarkName = hyperlink.Field.FieldValue.Replace("\"", string.Empty);
        if (hyperlink.BookmarkName.ToLower().StartsWithExt("_toc"))
          bounds = (double) bounds.X >= (double) this.CurrParagraphBounds.X ? this.CurrParagraphBounds : new RectangleF(bounds.X, this.CurrParagraphBounds.Y, bounds.Width, this.CurrParagraphBounds.Height);
        this.AddLinkToBookmark(bounds, bookmarkName, false);
        return;
    }
    this.m_hyperLinks.Add(new Dictionary<string, RectangleF>()
    {
      {
        str1,
        bounds
      }
    });
  }

  private void UpdateBookmarkTargetBoundsAndPageNumber(
    DocumentLayouter.BookmarkHyperlink bmhyperlink,
    string bmHyperlinkValue)
  {
    foreach (BookmarkPosition bookmark in DocumentLayouter.Bookmarks)
    {
      if (bookmark.BookmarkName == bmHyperlinkValue)
      {
        bmhyperlink.TargetBounds = bookmark.Bounds;
        bmhyperlink.TargetPageNumber = bookmark.PageNumber;
        break;
      }
    }
  }

  private void UpdateTOCLevel(WParagraph paragraph, DocumentLayouter.BookmarkHyperlink bookmark)
  {
    if (bookmark.HyperlinkValue == null || !bookmark.HyperlinkValue.StartsWithExt("_Toc"))
      return;
    string styleName = paragraph.StyleName;
    string str1 = styleName == null ? "normal" : styleName.ToLower().Replace(" ", "");
    foreach (TableOfContent tableOfContent in paragraph.Document.TOC.Values)
    {
      tableOfContent.UpdateTOCStyleLevels();
      int num = 1;
      foreach (KeyValuePair<int, List<string>> tocLevel in tableOfContent.TOCLevels)
      {
        foreach (string str2 in tocLevel.Value)
        {
          if (str1 == str2.ToLower().Replace(" ", ""))
          {
            bookmark.TOCLevel = num;
            bookmark.TOCText = paragraph.Text;
            break;
          }
        }
        ++num;
      }
    }
  }

  public int GetSplitIndexByOffset(
    string text,
    ITextMeasurable measurer,
    double offset,
    bool bSplitByChar,
    bool bIsInCell,
    float clientWidth,
    float clientActiveAreaWidth,
    bool isSplitByCharacter)
  {
    if (text == null)
      throw new ArgumentNullException(nameof (text));
    if (measurer == null)
      throw new ArgumentNullException("strWidget");
    if (offset < 0.0)
      throw new ArgumentOutOfRangeException(nameof (offset), (object) offset, "Value can not be less 0");
    int splitIndexByOffset = -1;
    if ((double) clientWidth == 0.0)
      clientWidth = (float) offset;
    if (text.Length != 0 && (offset > 0.0 || (double) clientWidth < 0.0))
    {
      int startIndex = 0;
      double num = 0.0;
      int wordLength;
      for (; (wordLength = this.GetWordLength(text, startIndex)) > -1; startIndex += wordLength)
      {
        SizeF sizeF = measurer.Measure(text.Substring(startIndex, wordLength));
        if ((double) sizeF.Width + num > offset)
        {
          startIndex = 0;
          break;
        }
        num += (double) sizeF.Width;
      }
      int resIndex = startIndex - 1;
      splitIndexByOffset = this.UpdateResIndex(text, measurer, resIndex, bSplitByChar, bIsInCell, offset, clientWidth, clientActiveAreaWidth, isSplitByCharacter);
    }
    return splitIndexByOffset;
  }

  internal int UpdateResIndex(
    string text,
    ITextMeasurable measurer,
    int resIndex,
    bool bSplitByChar,
    bool bIsInCell,
    double offset,
    float clientWidth,
    float clientActiveAreaWidth,
    bool isSplitByCharacter)
  {
    if (resIndex < 0 || bSplitByChar || bIsInCell)
    {
      for (int index = 0; index < text.Length; ++index)
      {
        SizeF sizeF = measurer.Measure(text.Substring(0, index + 1));
        if ((double) sizeF.Width > offset)
        {
          resIndex = index - 1;
          if (resIndex == -1 && bIsInCell)
          {
            float cellWidth = this.GetCellWidth((ParagraphItem) (measurer as WTextRange));
            if ((double) sizeF.Width > (double) cellWidth)
              resIndex = 0;
          }
          if ((double) clientWidth < 0.0)
          {
            resIndex = 0;
            break;
          }
          break;
        }
      }
    }
    string[] strArray = text.Split(' ');
    if (this.IsUnicodeText(text))
    {
      if (resIndex > -1)
      {
        if (text.Length > resIndex + 1 && this.IsBeginCharacter(text[resIndex + 1]) || this.IsLeadingCharacter(text[resIndex]))
          --resIndex;
        if (text.Length > resIndex + 1 && this.IsOverFlowCharacter(text[resIndex + 1]))
          ++resIndex;
      }
    }
    else if (strArray.Length >= 1 && (double) measurer.Measure(strArray[0]).Width > (double) clientActiveAreaWidth && !isSplitByCharacter)
      resIndex = -1;
    if (resIndex < 0)
      resIndex = text.Length - 1;
    return resIndex;
  }

  internal bool IsLeadingCharacter(char c)
  {
    switch (c)
    {
      case '"':
      case '$':
      case '\'':
      case '(':
      case '[':
      case '{':
      case '\u009D':
      case '〈':
      case '《':
      case '「':
      case '『':
      case '【':
      case '〔':
      case '＄':
      case '）':
      case 'ｋ':
      case 'ｚ':
      case '￡':
      case '￥':
        return true;
      default:
        return false;
    }
  }

  public bool IsBeginCharacter(char c)
  {
    switch (c)
    {
      case '!':
      case '%':
      case ')':
      case ',':
      case '.':
      case '?':
      case ']':
      case '}':
      case '、':
      case '〉':
      case '》':
      case '」':
      case '〕':
      case '〟':
      case '〪':
      case 'う':
      case 'え':
      case 'つ':
      case 'ね':
      case 'は':
      case 'や':
      case 'ゆ':
      case 'ア':
      case 'イ':
      case 'エ':
      case 'ク':
      case 'ー':
      case '）':
      case '．':
      case '］':
      case '＿':
      case '｝':
        return true;
      default:
        return false;
    }
  }

  internal bool IsOverFlowCharacter(char c)
  {
    switch (c)
    {
      case ',':
      case '.':
      case '`':
      case '．':
      case '｡':
        return true;
      default:
        return false;
    }
  }

  public bool IsUnicodeText(string text)
  {
    bool flag = false;
    if (text != null)
    {
      for (int index = 0; index < text.Length; ++index)
      {
        char ch = text[index];
        if (ch >= '　' && ch <= 'ヿ' || ch >= '\uFF00' && ch <= '\uFFEF' || ch >= '一' && ch <= '龯' || ch >= '㐀' && ch <= '\u4DBF' || ch >= '가' && ch <= '\uFFEF' || ch >= '\u0D80' && ch <= '\u0DFF')
        {
          flag = true;
          break;
        }
      }
    }
    return flag;
  }

  public Entity GetPreviousSibling(WTextRange textRange)
  {
    Entity previousSibling = textRange.PreviousSibling as Entity;
    while (previousSibling != null && (!(previousSibling is WTextRange) || (previousSibling as IWidget).LayoutInfo.IsSkip) && (previousSibling is BookmarkStart || previousSibling is BookmarkEnd || previousSibling is WFieldMark || previousSibling is IWidget && (previousSibling as IWidget).LayoutInfo != null && (previousSibling as IWidget).LayoutInfo.IsSkip))
      previousSibling = previousSibling.PreviousSibling as Entity;
    return previousSibling;
  }

  public float GetCellWidth(ParagraphItem paraItem)
  {
    if (!(paraItem.Owner is WParagraph wparagraph))
    {
      if (paraItem.Owner is InlineContentControl || paraItem.Owner is XmlParagraphItem)
        wparagraph = paraItem.GetOwnerParagraphValue();
      else if (paraItem is WTextRange && (paraItem as WTextRange).ParaItemCharFormat.BaseFormat.OwnerBase is WParagraph)
        wparagraph = (paraItem as WTextRange).ParaItemCharFormat.BaseFormat.OwnerBase as WParagraph;
    }
    float cellWidth = 0.0f;
    if (wparagraph != null)
    {
      WTextBody wtextBody = wparagraph.OwnerTextBody;
      while (!(wtextBody is WTableCell) && wtextBody != null)
      {
        if (wtextBody.Owner is BlockContentControl)
          wtextBody = (wtextBody.Owner as BlockContentControl).Owner as WTextBody;
      }
      if (wtextBody != null && ((IWidget) wtextBody).LayoutInfo is CellLayoutInfo && ((IWidget) wtextBody).LayoutInfo is CellLayoutInfo layoutInfo)
        cellWidth = layoutInfo.CellContentLayoutingBounds.Width;
    }
    if (wparagraph != null && ((IWidget) wparagraph).LayoutInfo is ParagraphLayoutInfo)
    {
      ParagraphLayoutInfo layoutInfo = ((IWidget) wparagraph).LayoutInfo as ParagraphLayoutInfo;
      cellWidth -= (float) ((double) layoutInfo.Margins.Left + (double) layoutInfo.Margins.Right + (layoutInfo.IsFirstLine ? (double) layoutInfo.FirstLineIndent + (double) layoutInfo.ListTab : 0.0));
    }
    if ((double) cellWidth < 0.0)
      cellWidth = 0.0f;
    return cellWidth;
  }

  internal string GetPdfFontCollectionKey(Font font, bool isUnicode)
  {
    return $"{font.Name.ToLower()};{(object) font.Style};{(object) font.Size};{isUnicode.ToString()}";
  }

  internal bool IsUnicode(string text)
  {
    if (text == null)
      return false;
    for (int index = 0; index < text.Length; ++index)
    {
      if (text[index] > 'ÿ')
        return true;
    }
    return false;
  }

  private int GetWordLength(string text, int startIndex)
  {
    int wordLength = -1;
    if (text.Length - startIndex > 0)
    {
      int index = startIndex;
      for (int length = text.Length; index < length; ++index)
      {
        if (index == length - 1)
        {
          wordLength = 1;
          break;
        }
        if (text[index] == ' ' && text[index + 1] != ' ')
        {
          wordLength = index - startIndex + 1;
          break;
        }
      }
    }
    return wordLength;
  }

  public new void Close()
  {
    if (this.m_graphics != null)
    {
      this.m_graphics.Dispose();
      this.m_graphics = (Graphics) null;
    }
    if (this.m_graphicsBmp != null)
    {
      this.m_graphicsBmp.Dispose();
      this.m_graphicsBmp = (Graphics) null;
    }
    if (this.m_hyperLinks != null)
    {
      this.m_hyperLinks.Clear();
      this.m_hyperLinks = (List<Dictionary<string, RectangleF>>) null;
    }
    if (this.m_overLappedShapeWidgets != null)
    {
      this.m_overLappedShapeWidgets.Clear();
      this.m_overLappedShapeWidgets = (Dictionary<int, LayoutedWidget>) null;
    }
    if (this.ClipBoundsContainer != null)
    {
      this.ClipBoundsContainer.Clear();
      this.ClipBoundsContainer = (Stack<RectangleF>) null;
    }
    if (this.m_fontmetric != null)
      this.m_fontmetric = (FontMetric) null;
    if (this.m_commentMarks != null)
    {
      this.m_commentMarks.Clear();
      this.m_commentMarks = (List<PointF[]>) null;
    }
    if (this.m_previousLineCommentStartMarks == null)
      return;
    this.m_previousLineCommentStartMarks.Clear();
    this.m_previousLineCommentStartMarks = (List<KeyValuePair<string, bool>>) null;
  }

  internal void DrawBehindWidgets(
    LayoutedWidgetList behindWidgets,
    IWidget ownerWidget,
    int length,
    bool isHaveToInitLayoutInfo)
  {
    Dictionary<int, LayoutedWidget> dictionary = new Dictionary<int, LayoutedWidget>();
    for (int index = 0; index < length; ++index)
    {
      int orderIndex = this.GetOrderIndex(behindWidgets[index].Widget);
      if (!dictionary.ContainsKey(orderIndex))
        dictionary.Add(orderIndex, behindWidgets[index]);
    }
    List<int> intList = new List<int>((IEnumerable<int>) dictionary.Keys);
    intList.Sort();
    foreach (int key in intList)
    {
      this.currParagraph = this.GetOwnerParagraph(dictionary[key]);
      this.Draw(dictionary[key], isHaveToInitLayoutInfo);
      behindWidgets.Remove(dictionary[key]);
    }
    dictionary.Clear();
    intList.Clear();
    this.m_orderIndex = 1;
  }

  internal int GetOrderIndex(IWidget widget)
  {
    int orderIndex = 0;
    switch (widget)
    {
      case WPicture _:
        orderIndex = (widget as WPicture).OrderIndex;
        break;
      case Shape _:
        orderIndex = (widget as Shape).ZOrderPosition;
        break;
      case WTextBox _:
        orderIndex = (widget as WTextBox).TextBoxFormat.OrderIndex;
        break;
      case GroupShape _:
        orderIndex = (widget as GroupShape).ZOrderPosition;
        break;
      case WChart _:
        orderIndex = (widget as WChart).ZOrderPosition;
        break;
      case WOleObject _:
        if ((widget as WOleObject).OlePicture != null)
        {
          orderIndex = (widget as WOleObject).OlePicture.OrderIndex;
          break;
        }
        break;
    }
    if (orderIndex == int.MaxValue || orderIndex == 0)
    {
      orderIndex = this.m_orderIndex;
      ++this.m_orderIndex;
    }
    return orderIndex;
  }

  public float GetLineWidth(BorderCode border)
  {
    float lineWidth = 0.0f;
    if (border.BorderType != (byte) 0)
    {
      lineWidth = (float) ((int) border.LineWidth / 8);
      if ((double) lineWidth == 0.0)
        lineWidth = 0.75f;
    }
    return lineWidth;
  }

  public float GetLineWidth(WPicture picture)
  {
    float result = 0.0f;
    if (picture.PictureShape.ShapeContainer.ShapeOptions != null && picture.PictureShape.ShapeContainer.ShapeOptions.LineProperties.Line)
    {
      if (picture.PictureShape.ShapeContainer.ShapeOptions.Properties.ContainsKey(459))
        result = float.TryParse(picture.PictureShape.ShapeContainer.GetPropertyValue(459).ToString(), NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result) ? result / 12700f : 0.75f;
      else if (picture.PictureShape.ShapeContainer.ShapeOptions != null && (picture.PictureShape.ShapeContainer.ShapeOptions.Properties.ContainsKey(448) || picture.Document.Settings.CompatibilityMode != CompatibilityMode.Word2013))
        result = 0.75f;
    }
    return result;
  }

  internal class DefaultBorders : RowFormat
  {
    public Border Vertical => this.Borders.Vertical;

    public Border Horizontal => this.Borders.Horizontal;

    public DefaultBorders(RowFormat format)
    {
      this.InitBorder(this.Borders.Horizontal, format.Borders.Horizontal);
      this.InitBorder(this.Borders.Vertical, format.Borders.Vertical);
    }

    private void InitBorder(Border destination, Border sourse)
    {
      destination.BorderType = sourse.BorderType;
      destination.Color = sourse.Color;
      destination.LineWidth = sourse.LineWidth;
      destination.Shadow = sourse.Shadow;
      destination.Space = sourse.Space;
      destination.HasNoneStyle = sourse.HasNoneStyle;
    }
  }
}
