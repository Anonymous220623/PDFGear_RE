// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ODF.Base.ODFImplementation.GraphicProperties
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.ODF.Base.ODFImplementation;

internal class GraphicProperties
{
  private bool m_autoGrowHeight;
  private bool m_autoGrowWidth;
  private string m_blue;
  private uint m_captionAngle;
  private CaptionAngleType m_captionAngleType;
  private string m_captionEscape;
  private CaptionEscapeDirection m_captionEscapeDirection;
  private bool m_captionFitLineLength;
  private string m_captionGap;
  private string m_captionLineLength;
  private CaptionType m_captionType;
  private bool m_colorInversion;
  private ColorMode m_colorMode;
  private string m_costrast;
  private uint m_decimalPlaces;
  private DrawAspect m_drawAspect;
  private string m_endGuide;
  private string m_endLineSpacingHorizontal;
  private string m_endLineSpacingVertical;
  private FillType m_fill;
  private Color m_fillColor;
  private string m_fillGradientName;
  private string m_fillHatchName;
  private bool m_fillHatchSolid;
  private string m_fillImageHeight;
  private string m_fillImageName;
  private RefPoint m_fillImageRefPoint;
  private string m_fillImageRefPointX;
  private string m_fillImageRefPointY;
  private string m_fillImageWidth;
  private bool m_fitToContour;
  private bool m_fitToSize;
  private bool m_frameDisplayBorder;
  private bool m_frameDisplayScrollbar;
  private string m_frameMarginHorizontal;
  private string m_frameMarginVertical;
  private string m_gamma;
  private int m_gradientStepCount;
  private string m_green;
  private string m_guideDistance;
  private string m_guideOverhang;
  private int m_imageOpacity;
  private int m_lineDistance;
  private int m_luminance;
  private string m_markerEnd;
  private bool m_markerEndCenter;
  private string m_markerEndWidth;
  private string m_markerStart;
  private bool m_markerStartCenter;
  private string m_markerStartWidth;
  private string m_measureAlign;
  private string m_measureVerticalAlign;
  private uint m_oleDrawAspect;
  private string m_opacity;
  private string m_opacityName;
  private bool m_parallel;
  private bool m_placing;
  private string m_red;
  private string m_secondaryFillColor;
  private Shadow m_drawShadow;
  private string m_shadowColor;
  private string m_shadowOffsetX;
  private string m_shadowOffsetY;
  private string m_shadowOpacity;
  private bool m_showUnit;
  private string m_startGuide;
  private string m_startLineSpacingHorizontal;
  private string m_startLineSpacingVertical;
  private Stroke m_stroke;
  private string m_strokeDash;
  private string m_strokeDashNames;
  private StrokeLineJoin m_strokeLineJoin;
  private string m_symbolColor;
  private string m_textAreaHorizontalAlign;
  private string m_textAreaVerticalAlign;
  private string m_tileRepeatOffset;
  private Unit m_unit;
  private string m_visibleAreaHeight;
  private string m_visibleAreaWidth;
  private string m_visibleAreaTop;
  private string m_visibleAreaLeft;
  private string m_wrapInfluenceOnPosition;
  private FillRule m_fillRule;
  private string m_height;
  private Color m_strokeColor;
  private StrokeLineCap m_strokeLineCap;
  private string m_strokeOpacity;
  private string m_strokeWidth;
  private string m_width;
  private int m_x;
  private int m_y;

  internal bool AutoGrowHeight
  {
    get => this.m_autoGrowHeight;
    set => this.m_autoGrowHeight = value;
  }

  internal bool AutoGrowWidth
  {
    get => this.m_autoGrowWidth;
    set => this.m_autoGrowWidth = value;
  }

  internal string Blue
  {
    get => this.m_blue;
    set => this.m_blue = value;
  }

  internal uint CaptionAngle
  {
    get => this.m_captionAngle;
    set => this.m_captionAngle = value;
  }

  internal CaptionAngleType CaptionAngleType
  {
    get => this.m_captionAngleType;
    set => this.m_captionAngleType = value;
  }

  internal string CaptionEscape
  {
    get => this.m_captionEscape;
    set => this.m_captionEscape = value;
  }

  internal CaptionEscapeDirection CaptionEscapeDirection
  {
    get => this.m_captionEscapeDirection;
    set => this.m_captionEscapeDirection = value;
  }

  internal bool CaptionFitLineLength
  {
    get => this.m_captionFitLineLength;
    set => this.m_captionFitLineLength = value;
  }

  internal string CaptionGap
  {
    get => this.m_captionGap;
    set => this.m_captionGap = value;
  }

  internal string CaptionLineLength
  {
    get => this.m_captionLineLength;
    set => this.m_captionLineLength = value;
  }

  internal CaptionType CaptionType
  {
    get => this.m_captionType;
    set => this.m_captionType = value;
  }

  internal bool ColorInversion
  {
    get => this.m_colorInversion;
    set => this.m_colorInversion = value;
  }

  internal ColorMode ColorMode
  {
    get => this.m_colorMode;
    set => this.m_colorMode = value;
  }

  internal string Costrast
  {
    get => this.m_costrast;
    set => this.m_costrast = value;
  }

  internal uint DecimalPlaces
  {
    get => this.m_decimalPlaces;
    set => this.m_decimalPlaces = value;
  }

  internal DrawAspect DrawAspect
  {
    get => this.m_drawAspect;
    set => this.m_drawAspect = value;
  }

  internal string EndGuide
  {
    get => this.m_endGuide;
    set => this.m_endGuide = value;
  }

  internal string EndLineSpacingHorizontal
  {
    get => this.m_endLineSpacingHorizontal;
    set => this.m_endLineSpacingHorizontal = value;
  }

  internal string EndLineSpacingVertical
  {
    get => this.m_endLineSpacingVertical;
    set => this.m_endLineSpacingVertical = value;
  }

  internal FillType Fill
  {
    get => this.m_fill;
    set => this.m_fill = value;
  }

  internal Color FillColor
  {
    get => this.m_fillColor;
    set => this.m_fillColor = value;
  }

  internal string FillGradientName
  {
    get => this.m_fillGradientName;
    set => this.m_fillGradientName = value;
  }

  internal string FillHatchName
  {
    get => this.m_fillHatchName;
    set => this.m_fillHatchName = value;
  }

  internal bool FillHatchSolid
  {
    get => this.m_fillHatchSolid;
    set => this.m_fillHatchSolid = value;
  }

  internal string FillImageHeight
  {
    get => this.m_fillImageHeight;
    set => this.m_fillImageHeight = value;
  }

  internal string FillImageName
  {
    get => this.m_fillImageName;
    set => this.m_fillImageName = value;
  }

  internal RefPoint FillImageRefPoint
  {
    get => this.m_fillImageRefPoint;
    set => this.m_fillImageRefPoint = value;
  }

  internal string FillImageRefPointX
  {
    get => this.m_fillImageRefPointX;
    set => this.m_fillImageRefPointX = value;
  }

  internal string FillImageRefPointY
  {
    get => this.m_fillImageRefPointY;
    set => this.m_fillImageRefPointY = value;
  }

  internal string FillImageWidth
  {
    get => this.m_fillImageWidth;
    set => this.m_fillImageWidth = value;
  }

  internal bool FitToContour
  {
    get => this.m_fitToContour;
    set => this.m_fitToContour = value;
  }

  internal bool FitToSize
  {
    get => this.m_fitToSize;
    set => this.m_fitToSize = value;
  }

  internal bool FrameDisplayBorder
  {
    get => this.m_frameDisplayBorder;
    set => this.m_frameDisplayBorder = value;
  }

  internal bool FrameDisplayScrollbar
  {
    get => this.m_frameDisplayScrollbar;
    set => this.m_frameDisplayScrollbar = value;
  }

  internal string FrameMarginHorizontal
  {
    get => this.m_frameMarginHorizontal;
    set => this.m_frameMarginHorizontal = value;
  }

  internal string FrameMarginVertical
  {
    get => this.m_frameMarginVertical;
    set => this.m_frameMarginVertical = value;
  }

  internal string Gamma
  {
    get => this.m_gamma;
    set => this.m_gamma = value;
  }

  internal int GradientStepCount
  {
    get => this.m_gradientStepCount;
    set => this.m_gradientStepCount = value;
  }

  internal string Green
  {
    get => this.m_green;
    set => this.m_green = value;
  }

  internal string GuideDistance
  {
    get => this.m_guideDistance;
    set => this.m_guideDistance = value;
  }

  internal string GuideOverhang
  {
    get => this.m_guideOverhang;
    set => this.m_guideOverhang = value;
  }

  internal int ImageOpacity
  {
    get => this.m_imageOpacity;
    set => this.m_imageOpacity = value;
  }

  internal int LineDistance
  {
    get => this.m_lineDistance;
    set => this.m_lineDistance = value;
  }

  internal int Luminance
  {
    get => this.m_luminance;
    set => this.m_luminance = value;
  }

  internal string MarkerEnd
  {
    get => this.m_markerEnd;
    set => this.m_markerEnd = value;
  }

  internal bool MarkerEndCenter
  {
    get => this.m_markerEndCenter;
    set => this.m_markerEndCenter = value;
  }

  internal string MarkerEndWidth
  {
    get => this.m_markerEndWidth;
    set => this.m_markerEndWidth = value;
  }

  internal string MarkerStart
  {
    get => this.m_markerStart;
    set => this.m_markerStart = value;
  }

  internal bool MarkerStartCenter
  {
    get => this.m_markerStartCenter;
    set => this.m_markerStartCenter = value;
  }

  internal string MarkerStartWidth
  {
    get => this.m_markerStartWidth;
    set => this.m_markerStartWidth = value;
  }

  internal string MeasureAlign
  {
    get => this.m_measureAlign;
    set => this.m_measureAlign = value;
  }

  internal string MeasureVerticalAlign
  {
    get => this.m_measureVerticalAlign;
    set => this.m_measureVerticalAlign = value;
  }

  internal uint OleDrawAspect
  {
    get => this.m_oleDrawAspect;
    set => this.m_oleDrawAspect = value;
  }

  internal string Opacity
  {
    get => this.m_opacity;
    set => this.m_opacity = value;
  }

  internal string OpacityName
  {
    get => this.m_opacityName;
    set => this.m_opacityName = value;
  }

  internal bool Parallel
  {
    get => this.m_parallel;
    set => this.m_parallel = value;
  }

  internal bool Placing
  {
    get => this.m_placing;
    set => this.m_placing = value;
  }

  internal string Red
  {
    get => this.m_red;
    set => this.m_red = value;
  }

  internal string SecondaryFillCOlor
  {
    get => this.m_secondaryFillColor;
    set => this.m_secondaryFillColor = value;
  }

  internal Shadow DrawShadow
  {
    get => this.m_drawShadow;
    set => this.m_drawShadow = value;
  }

  internal string ShadowColor
  {
    get => this.m_shadowColor;
    set => this.m_shadowColor = value;
  }

  internal string ShadowOffsetX
  {
    get => this.m_shadowOffsetX;
    set => this.m_shadowOffsetX = value;
  }

  internal string ShadowOffsetY
  {
    get => this.m_shadowOffsetY;
    set => this.m_shadowOffsetY = value;
  }

  internal string ShadowOpacity
  {
    get => this.m_shadowOpacity;
    set => this.m_shadowOpacity = value;
  }

  internal bool ShowUnit
  {
    get => this.m_showUnit;
    set => this.m_showUnit = value;
  }

  internal string StartGuide
  {
    get => this.m_startGuide;
    set => this.m_startGuide = value;
  }

  internal string StartLineSpacingHorizontal
  {
    get => this.m_startLineSpacingHorizontal;
    set => this.m_startLineSpacingHorizontal = value;
  }

  internal string StartLineSpacingVertical
  {
    get => this.m_startLineSpacingVertical;
    set => this.m_startLineSpacingVertical = value;
  }

  internal Stroke Stroke
  {
    get => this.m_stroke;
    set => this.m_stroke = value;
  }

  internal string StrokeDash
  {
    get => this.m_strokeDash;
    set => this.m_strokeDash = value;
  }

  internal string StrokeDashNames
  {
    get => this.m_strokeDashNames;
    set => this.m_strokeDashNames = value;
  }

  internal StrokeLineJoin StrokeLineJoin
  {
    get => this.m_strokeLineJoin;
    set => this.m_strokeLineJoin = value;
  }

  internal string SymbolColor
  {
    get => this.m_symbolColor;
    set => this.m_symbolColor = value;
  }

  internal string TextAreaHorizontalAlign
  {
    get => this.m_textAreaHorizontalAlign;
    set => this.m_textAreaHorizontalAlign = value;
  }

  internal string TextAreaVerticalAlign
  {
    get => this.m_textAreaVerticalAlign;
    set => this.m_textAreaVerticalAlign = value;
  }

  internal string TitleRepeatOffset
  {
    get => this.m_tileRepeatOffset;
    set => this.m_tileRepeatOffset = value;
  }

  internal Unit Unit
  {
    get => this.m_unit;
    set => this.m_unit = value;
  }

  internal string VisibleAreaHeight
  {
    get => this.m_visibleAreaHeight;
    set => this.m_visibleAreaHeight = value;
  }

  internal string VisibleAreaWidth
  {
    get => this.m_visibleAreaWidth;
    set => this.m_visibleAreaWidth = value;
  }

  internal string VisibleAreaTop
  {
    get => this.m_visibleAreaTop;
    set => this.m_visibleAreaTop = value;
  }

  internal string VisibleAreaLeft
  {
    get => this.m_visibleAreaLeft;
    set => this.m_visibleAreaLeft = value;
  }

  internal string WrapInfluenceOnPosition
  {
    get => this.m_wrapInfluenceOnPosition;
    set => this.m_wrapInfluenceOnPosition = value;
  }

  internal FillRule FillRule
  {
    get => this.m_fillRule;
    set => this.m_fillRule = value;
  }

  internal string Height
  {
    get => this.m_height;
    set => this.m_height = value;
  }

  internal Color StrokeColor
  {
    get => this.m_strokeColor;
    set => this.m_strokeColor = value;
  }

  internal StrokeLineCap StrokeLineCap
  {
    get => this.m_strokeLineCap;
    set => this.m_strokeLineCap = value;
  }

  internal string StrokeOpacity
  {
    get => this.m_strokeOpacity;
    set => this.m_strokeOpacity = value;
  }

  internal string StrokeWidth
  {
    get => this.m_strokeWidth;
    set => this.m_strokeWidth = value;
  }

  internal string Width
  {
    get => this.m_width;
    set => this.m_width = value;
  }

  internal int X
  {
    get => this.m_x;
    set => this.m_x = value;
  }

  internal int Y
  {
    get => this.m_y;
    set => this.m_y = value;
  }
}
