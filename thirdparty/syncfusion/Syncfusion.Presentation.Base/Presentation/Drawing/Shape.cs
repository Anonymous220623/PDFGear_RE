// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Drawing.Shape
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Charts;
using Syncfusion.Presentation.Layouting;
using Syncfusion.Presentation.Rendering;
using Syncfusion.Presentation.RichText;
using Syncfusion.Presentation.SlideImplementation;
using Syncfusion.Presentation.SmartArtImplementation;
using Syncfusion.Presentation.TableImplementation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Xml;

#nullable disable
namespace Syncfusion.Presentation.Drawing;

internal class Shape : IShape, ISlideItem
{
  private Dictionary<string, Stream> _preservedElements;
  private AutoShapeType _autoShapeType;
  private BaseSlide _baseSlide;
  private string _description;
  private DrawingType _drawingType;
  private Syncfusion.Presentation.Drawing.Fill _fillFormat;
  private string videoRelationId;
  private string _videoPath;
  private EffectList _effectList;
  private Graphics _graphics;
  private GDIRenderer _gdiRenderer;
  private bool _fLocksText;
  private bool _fPublished;
  private GroupShape _groupShape;
  private bool _isHidden;
  private bool _isPhoto;
  private bool _isUserDrawn;
  private Syncfusion.Presentation.Drawing.LineFormat _lineFormat;
  private string _macro;
  private Placeholder _placeHolder;
  private ShapeFrame _shapeFrame;
  private int _shapeId;
  private string _shapeName;
  private ShapeType _shapeType;
  private Syncfusion.Presentation.RichText.TextBody _textFrame;
  private string _textLink;
  private string _title;
  private bool _isLockAspectRatio;
  private bool _isGroupedShapeFrameChanged;
  private Dictionary<string, string> _guideList;
  private Dictionary<string, string> _avList;
  private List<Path2D> _path2DList;
  private bool _isPresetGeom;
  private bool _isCustomGeom;
  private bool _isBgFill;
  private ShapeInfo _shapeInfo;
  private Dictionary<string, Stream> _styleStream;
  private Syncfusion.Presentation.RichText.Hyperlink _hyperlink;
  private ShapeFrame _groupFrame;
  private SlideItemType _slideItemType;
  private bool _isGroupFill;
  private bool _isDisposed;
  private bool _hasLineProperties;

  internal Shape(ShapeType shapeType, BaseSlide baseSlide)
  {
    this._shapeType = shapeType;
    this._shapeFrame = new ShapeFrame(this);
    this._baseSlide = baseSlide;
    this._preservedElements = new Dictionary<string, Stream>();
    this._fLocksText = false;
    switch (this)
    {
      case Table _:
      case SmartArt _:
        this._isPresetGeom = true;
        break;
    }
    this._fillFormat = new Syncfusion.Presentation.Drawing.Fill(this);
    this._shapeName = "";
  }

  internal GroupShape Group
  {
    get => this.IsInGroupShape() ? this._groupShape : (GroupShape) null;
    set => this._groupShape = value;
  }

  internal bool IsGroup => this._shapeType == ShapeType.GrpSp;

  internal bool IsInGroupShape() => this._groupShape != null && this._groupShape.IsGroup;

  internal void SetGroupShape(GroupShape groupShape) => this._groupShape = groupShape;

  public int ConnectionSiteCount
  {
    get => AutoShapeHelper.GetAutoShapeConnectionSiteCount(this._autoShapeType);
  }

  public AutoShapeType AutoShapeType
  {
    get => this._autoShapeType;
    set => this._autoShapeType = value;
  }

  public string Description
  {
    get => this._description;
    set => this._description = value;
  }

  public IHyperLink Hyperlink => (IHyperLink) this._hyperlink;

  public IFill Fill => (IFill) this._fillFormat;

  internal string VideoRelationId
  {
    get => this.videoRelationId;
    set => this.videoRelationId = value;
  }

  internal bool GetPresetGeometry()
  {
    if (this.PlaceholderFormat != null && this.PlaceholderFormat.Type == PlaceholderType.Picture && !this.IsPresetGeometry && !this.IsCustomGeometry && this._baseSlide is Slide)
    {
      Slide baseSlide = (Slide) this._baseSlide;
      string layoutIndex = baseSlide.ObtainLayoutIndex();
      if (layoutIndex != null && baseSlide.Presentation.GetSlideLayout().ContainsKey(layoutIndex))
      {
        foreach (Shape shape in (IEnumerable<ISlideItem>) baseSlide.Presentation.GetSlideLayout()[layoutIndex].Shapes)
        {
          if (shape.PlaceholderFormat != null && (int) shape.GetPlaceholder().Index == (int) this.GetPlaceholder().Index && shape.IsPresetGeometry)
            return true;
        }
      }
    }
    else if (this.IsPresetGeometry)
      return true;
    return false;
  }

  internal bool GetCustomGeometry()
  {
    if (this.PlaceholderFormat != null && this.PlaceholderFormat.Type == PlaceholderType.Picture && !this.IsCustomGeometry && !this.IsPresetGeometry && this._baseSlide is Slide)
    {
      Slide baseSlide = (Slide) this._baseSlide;
      string layoutIndex = baseSlide.ObtainLayoutIndex();
      if (layoutIndex != null && baseSlide.Presentation.GetSlideLayout().ContainsKey(layoutIndex))
      {
        foreach (Shape shape in (IEnumerable<ISlideItem>) baseSlide.Presentation.GetSlideLayout()[layoutIndex].Shapes)
        {
          if (shape.PlaceholderFormat != null && (int) shape.GetPlaceholder().Index == (int) this.GetPlaceholder().Index && shape.IsCustomGeometry)
            return true;
        }
      }
    }
    else if (this.IsCustomGeometry)
      return true;
    return false;
  }

  internal AutoShapeType GetAutoShapeType()
  {
    if (this.AutoShapeType == ~AutoShapeType.Unknown && this.PlaceholderFormat != null && this.PlaceholderFormat.Type == PlaceholderType.Picture && this._baseSlide is Slide)
    {
      Slide baseSlide = (Slide) this._baseSlide;
      string layoutIndex = baseSlide.ObtainLayoutIndex();
      if (layoutIndex != null && baseSlide.Presentation.GetSlideLayout().ContainsKey(layoutIndex))
      {
        foreach (Shape shape in (IEnumerable<ISlideItem>) baseSlide.Presentation.GetSlideLayout()[layoutIndex].Shapes)
        {
          if (shape.PlaceholderFormat != null && (int) shape.GetPlaceholder().Index == (int) this.GetPlaceholder().Index)
            return shape.AutoShapeType;
        }
      }
    }
    return this.AutoShapeType;
  }

  internal List<Path2D> GetPath2DList()
  {
    if (this.Path2DList == null && this.PlaceholderFormat != null && this.PlaceholderFormat.Type == PlaceholderType.Picture && this._baseSlide is Slide)
    {
      Slide baseSlide = (Slide) this._baseSlide;
      string layoutIndex = baseSlide.ObtainLayoutIndex();
      if (layoutIndex != null && baseSlide.Presentation.GetSlideLayout().ContainsKey(layoutIndex))
      {
        foreach (Shape shape in (IEnumerable<ISlideItem>) baseSlide.Presentation.GetSlideLayout()[layoutIndex].Shapes)
        {
          if (shape.PlaceholderFormat != null && (int) shape.GetPlaceholder().Index == (int) this.GetPlaceholder().Index)
            return shape.Path2DList;
        }
      }
    }
    return this.Path2DList;
  }

  internal IFill GetDefaultFillFormat()
  {
    switch (this._fillFormat.FillType)
    {
      case FillType.Automatic:
        IFill defaultFillFormat1 = (IFill) new Syncfusion.Presentation.Drawing.Fill(this);
        if (this._shapeType == ShapeType.Sp || this._shapeType == ShapeType.CxnSp)
        {
          switch (this._drawingType)
          {
            case DrawingType.TextBox:
              if (this.PreservedElements != null && this.PreservedElements.Count != 0)
              {
                Stream stream;
                if (this.PreservedElements.TryGetValue("style", out stream) && stream != null)
                {
                  defaultFillFormat1.FillType = FillType.Solid;
                  defaultFillFormat1.SolidFill.Color = this.GetThemeColor("fillRef");
                }
                else
                  defaultFillFormat1.FillType = FillType.None;
              }
              else
                defaultFillFormat1.FillType = FillType.None;
              return defaultFillFormat1;
            case DrawingType.PlaceHolder:
              FillType fillType = FillType.Automatic;
              if (this._baseSlide is Slide)
              {
                Slide baseSlide = (Slide) this._baseSlide;
                string layoutIndex = baseSlide.ObtainLayoutIndex();
                if (layoutIndex != null && baseSlide.Presentation.GetSlideLayout().ContainsKey(layoutIndex))
                {
                  LayoutSlide layoutSlide = baseSlide.Presentation.GetSlideLayout()[layoutIndex];
                  foreach (Shape shape in (IEnumerable<ISlideItem>) layoutSlide.Shapes)
                  {
                    if (shape.PlaceholderFormat != null)
                    {
                      if (this._placeHolder != null)
                      {
                        if (Helper.CheckPlaceholder(shape.PlaceholderFormat, (IPlaceholderFormat) this._placeHolder))
                        {
                          fillType = shape.Fill.FillType;
                          if (fillType != FillType.Automatic)
                          {
                            defaultFillFormat1 = shape.Fill;
                            break;
                          }
                          break;
                        }
                      }
                      else
                        break;
                    }
                  }
                  if (fillType == FillType.Automatic)
                  {
                    foreach (Shape shape in (IEnumerable<ISlideItem>) ((BaseSlide) layoutSlide.MasterSlide).Shapes)
                    {
                      if (shape.PlaceholderFormat != null)
                      {
                        if (this._placeHolder != null)
                        {
                          if (Helper.CheckPlaceholder(shape.PlaceholderFormat, (IPlaceholderFormat) this._placeHolder, true) || this.CheckMasterWithLayoutShape(shape, (Shapes) layoutSlide.Shapes))
                          {
                            if (shape.Fill.FillType != FillType.Automatic)
                            {
                              defaultFillFormat1 = shape.Fill;
                              break;
                            }
                            break;
                          }
                        }
                        else
                          break;
                      }
                    }
                  }
                }
              }
              if (defaultFillFormat1.FillType == FillType.Gradient)
              {
                GradientFill gradientFill1 = (GradientFill) defaultFillFormat1.GradientFill;
                if (gradientFill1.ShadeProperties == null)
                {
                  GradientFill gradientFill2 = (GradientFill) this._baseSlide.BaseTheme.FillFormats[2].GradientFill;
                  gradientFill1.ShadeProperties = gradientFill2.ShadeProperties;
                }
              }
              return defaultFillFormat1;
            default:
              if (this._fillFormat.FillType == FillType.Automatic && this._drawingType == DrawingType.None && (this.BaseSlide is Slide || this.BaseSlide is LayoutSlide || this.BaseSlide is MasterSlide))
              {
                if (this.PreservedElements.Count != 0)
                {
                  if (this.Group != null && this.Group.Fill.FillType != FillType.Automatic)
                    return this.Group.Fill;
                  if (!this.PreservedElements.ContainsKey("style"))
                    return (IFill) this._fillFormat;
                  if (this.BaseSlide.Presentation.Theme.FillFormats[this.GetIndex("fillRef")].FillType == FillType.Gradient)
                  {
                    new Syncfusion.Presentation.Drawing.Fill(this).FillType = FillType.Gradient;
                    IColor themeColor = this.GetThemeColor("fillRef");
                    Syncfusion.Presentation.Drawing.Fill gradientFromTheme = this.GetGradientFromTheme("fillRef");
                    foreach (GradientStop gradientStop in (IEnumerable<IGradientStop>) gradientFromTheme.GradientFill.GradientStops)
                    {
                      ColorObject colorObject = gradientStop.GetColorObject();
                      colorObject.IsGradient = true;
                      colorObject.ReplaceColor = (themeColor as ColorObject).ReplaceColor;
                    }
                    return (IFill) gradientFromTheme;
                  }
                  defaultFillFormat1.FillType = FillType.Solid;
                  IColor themeColor1 = this.GetThemeColor("fillRef");
                  if (themeColor1.ToArgb() == ColorObject.Transparent.ToArgb() && !(themeColor1 as ColorObject).IsUpdatedColor)
                    return (IFill) this._fillFormat;
                  defaultFillFormat1.SolidFill.Color = themeColor1;
                  return defaultFillFormat1;
                }
                return this.Group != null && this.Group.Fill.FillType != FillType.Automatic ? this.Group.Fill : (IFill) this._fillFormat;
              }
              break;
          }
        }
        return defaultFillFormat1;
      case FillType.Gradient:
        IFill defaultFillFormat2 = (IFill) new Syncfusion.Presentation.Drawing.Fill(this);
        if (this._drawingType == DrawingType.None)
        {
          defaultFillFormat2.FillType = FillType.Gradient;
          if (this.PreservedElements.Count == 0)
            return (IFill) this._fillFormat;
          if (!this.PreservedElements.ContainsKey("style"))
            return (IFill) this._fillFormat;
          Syncfusion.Presentation.Drawing.Fill gradientFromTheme = this.GetGradientFromTheme("fillRef");
          if (gradientFromTheme != null)
          {
            GradientFill gradientFill3 = (GradientFill) defaultFillFormat2.GradientFill;
            GradientFill gradientFill4 = (GradientFill) gradientFromTheme.GradientFill;
            if (gradientFill4 == null)
              return (IFill) this._fillFormat;
            gradientFill3.ShadeProperties = gradientFill4.ShadeProperties;
            gradientFill3.RotWithShape = gradientFill4.RotWithShape;
            gradientFill3.ShadeProperties = gradientFill4.ShadeProperties;
            gradientFill3.TileRectangle = gradientFill4.TileRectangle;
            gradientFill3.Type = gradientFill4.Type;
            gradientFill3.SetGradientStops(((GradientFill) this._fillFormat.GradientFill).GradientStops);
            return defaultFillFormat2;
          }
          break;
        }
        break;
    }
    return (IFill) this._fillFormat;
  }

  internal int GetIndex(string localName)
  {
    int index = 0;
    Stream data;
    if (this.PreservedElements.TryGetValue("style", out data) && data != null && data.Length > 0L)
    {
      data.Position = 0L;
      XmlReader reader = UtilityMethods.CreateReader(data);
      reader.ReadToFollowing(localName, "http://schemas.openxmlformats.org/drawingml/2006/main");
      if (reader.NodeType != XmlNodeType.None)
      {
        switch (localName)
        {
          case "fillRef":
            int num = Convert.ToInt32(reader.GetAttribute("idx"));
            switch (num)
            {
              case 1001:
                num = 1;
                break;
              case 1002:
                num = 2;
                break;
              case 1003:
                num = 3;
                break;
            }
            return num > 0 ? num - 1 : num;
          default:
            reader.Read();
            this.SkipWhitespaces(reader);
            break;
        }
      }
    }
    return index;
  }

  private Syncfusion.Presentation.Drawing.Fill GetGradientFromTheme(string localName)
  {
    Stream data;
    if (this.PreservedElements.TryGetValue("style", out data) && data != null && data.Length > 0L)
    {
      data.Position = 0L;
      XmlReader reader = UtilityMethods.CreateReader(data);
      reader.ReadToFollowing(localName, "http://schemas.openxmlformats.org/drawingml/2006/main");
      if (reader.NodeType != XmlNodeType.None)
      {
        switch (localName)
        {
          case "fillRef":
            string attribute = reader.GetAttribute("idx");
            switch (attribute)
            {
              case "0":
              case "1000":
                return (Syncfusion.Presentation.Drawing.Fill) null;
              default:
                Syncfusion.Presentation.Drawing.Fill fill = (Syncfusion.Presentation.Drawing.Fill) null;
                switch (attribute)
                {
                  case "1002":
                  case "2":
                    fill = this._baseSlide.Presentation.Theme.FillFormats[1];
                    break;
                  case "1003":
                  case "3":
                    fill = this._baseSlide.Presentation.Theme.FillFormats[2];
                    break;
                }
                return fill != null && fill.FillType == FillType.Gradient ? fill : (Syncfusion.Presentation.Drawing.Fill) null;
            }
        }
      }
    }
    return (Syncfusion.Presentation.Drawing.Fill) null;
  }

  internal bool CheckMasterWithLayoutShape(Shape masterShape, Shapes layoutShapes)
  {
    if (this._placeHolder != null && this.GetPlaceholder().GetPlaceholderType() == PlaceholderType.SlideNumber)
      return false;
    foreach (Shape layoutShape in layoutShapes)
    {
      double num1 = masterShape.ShapeFrame.GetIsFrameChanged() || masterShape.DrawingType != DrawingType.PlaceHolder ? masterShape.ShapeFrame.GetDefaultTop() : 0.0;
      double num2 = masterShape.ShapeFrame.GetIsFrameChanged() || masterShape.DrawingType != DrawingType.PlaceHolder ? masterShape.ShapeFrame.GetDefaultHeight() : 0.0;
      double num3 = layoutShape.ShapeFrame.GetDefaultTop();
      double num4 = layoutShape.ShapeFrame.GetDefaultHeight();
      if (!layoutShape.ShapeFrame.GetIsFrameChanged() && layoutShape.DrawingType == DrawingType.PlaceHolder)
      {
        if (!(layoutShape.BaseSlide is Slide) || !(layoutShape.BaseSlide is NotesSlide))
          num3 = 0.0;
        num4 = 0.0;
      }
      if (num3 == num1 && num4 == num2)
        return true;
    }
    return false;
  }

  public double Height
  {
    get
    {
      if (!this._isGroupedShapeFrameChanged && this.IsInGroupShape() && this.DrawingType != DrawingType.PlaceHolder)
      {
        double groupShapeProperties = this.GetGroupShapeProperties("height");
        if (groupShapeProperties != 0.0)
          return groupShapeProperties;
      }
      return this._shapeFrame.GetDefaultHeight();
    }
    set
    {
      if (this.IsInGroupShape())
        this._isGroupedShapeFrameChanged = true;
      this._shapeFrame.Height = value;
      if (!(this is PresentationChart))
        return;
      ((PresentationChart) this).GetChartImpl().Height = value;
    }
  }

  public bool Hidden
  {
    get => this._isHidden;
    set => this._isHidden = value;
  }

  private Dictionary<string, long> UpdateGroupShapeBounds(
    List<RectangleF> groupShapeBounds,
    List<float> groupShapeRotations,
    List<bool?> groupShapeHorzFlips,
    List<bool?> groupShapeVertFlips,
    Shape shape)
  {
    long num1 = 0;
    long num2 = 0;
    long num3 = 0;
    long num4 = 0;
    long num5 = 0;
    if (shape.Group != null)
      this.UpdateGroupShapeBounds(groupShapeBounds, groupShapeRotations, groupShapeHorzFlips, groupShapeVertFlips, (Shape) shape.Group);
    shape.UpdateGroupFrame(groupShapeBounds, groupShapeRotations, groupShapeHorzFlips, groupShapeVertFlips, shape);
    if (shape.GroupFrame != null)
    {
      num1 = shape.GroupFrame.OffsetX;
      num2 = shape.GroupFrame.OffsetY;
      num3 = shape.GroupFrame.OffsetCX;
      num4 = shape.GroupFrame.OffsetCY;
      num5 = (long) shape.GroupFrame.Rotation;
    }
    return new Dictionary<string, long>()
    {
      {
        "left",
        num1
      },
      {
        "top",
        num2
      },
      {
        "width",
        num3
      },
      {
        "height",
        num4
      },
      {
        "rotation",
        num5
      }
    };
  }

  private double GetGroupShapeProperties(string propType)
  {
    Dictionary<string, long> dictionary = this.UpdateGroupShapeBounds(new List<RectangleF>(), new List<float>(), new List<bool?>(), new List<bool?>(), this);
    return propType == "rotation" ? (double) (dictionary[propType] / 60000L) : Helper.EmuToPoint(dictionary[propType]);
  }

  public double Left
  {
    get
    {
      if (!this._isGroupedShapeFrameChanged && this.IsInGroupShape() && this.DrawingType != DrawingType.PlaceHolder)
      {
        double groupShapeProperties = this.GetGroupShapeProperties("left");
        if (groupShapeProperties != 0.0)
          return groupShapeProperties;
      }
      return this._shapeFrame.GetDefaultLeft();
    }
    set
    {
      if (this.IsInGroupShape())
        this._isGroupedShapeFrameChanged = true;
      this._shapeFrame.Left = value;
    }
  }

  public ILineFormat LineFormat
  {
    get
    {
      if (this._shapeType == ShapeType.Point || this._shapeType == ShapeType.Drawing || this._shapeType == ShapeType.GraphicFrame && this._slideItemType == SlideItemType.SmartArt)
        this._hasLineProperties = true;
      return (ILineFormat) this._lineFormat ?? (ILineFormat) (this._lineFormat = new Syncfusion.Presentation.Drawing.LineFormat(this));
    }
  }

  public int ShapeId
  {
    get => this._shapeId;
    set => this._shapeId = value;
  }

  public string ShapeName
  {
    get => this._shapeName;
    set => this._shapeName = value;
  }

  public ITextBody TextBody
  {
    get => (ITextBody) this._textFrame ?? (ITextBody) (this._textFrame = new Syncfusion.Presentation.RichText.TextBody(this));
  }

  public string Title
  {
    get => this._title;
    set => this._title = value;
  }

  public double Top
  {
    get
    {
      if (!this._isGroupedShapeFrameChanged && this.IsInGroupShape() && this.DrawingType != DrawingType.PlaceHolder)
      {
        double groupShapeProperties = this.GetGroupShapeProperties("top");
        if (groupShapeProperties != 0.0)
          return groupShapeProperties;
      }
      return this._shapeFrame.GetDefaultTop();
    }
    set
    {
      if (this.IsInGroupShape())
        this._isGroupedShapeFrameChanged = true;
      this._shapeFrame.Top = value;
    }
  }

  public double Width
  {
    get
    {
      if (!this._isGroupedShapeFrameChanged && this.IsInGroupShape() && this.DrawingType != DrawingType.PlaceHolder)
      {
        double groupShapeProperties = this.GetGroupShapeProperties("width");
        if (groupShapeProperties != 0.0)
          return groupShapeProperties;
      }
      return this._shapeFrame.GetDefaultWidth();
    }
    set
    {
      if (this.IsInGroupShape())
        this._isGroupedShapeFrameChanged = true;
      this._shapeFrame.Width = value;
      if (!(this is PresentationChart))
        return;
      ((PresentationChart) this).GetChartImpl().Width = value;
    }
  }

  internal ShapeInfo ShapeInfo => this._shapeInfo;

  internal string GetVideoPath() => this._videoPath;

  internal void SetVideoPath(string linkAttribute)
  {
    RelationCollection topRelation = this.BaseSlide.TopRelation;
    if (topRelation == null)
      return;
    this._videoPath = topRelation.GetItemPathByRelation(linkAttribute);
  }

  internal bool HasLineProperties
  {
    get => this._hasLineProperties;
    set => this._hasLineProperties = value;
  }

  internal bool IsLineShape
  {
    get
    {
      return this.AutoShapeType == AutoShapeType.Arc || this.AutoShapeType == AutoShapeType.LeftBrace || this.AutoShapeType == AutoShapeType.LeftBracket || this.AutoShapeType == AutoShapeType.RightBrace || this.AutoShapeType == AutoShapeType.RightBracket || this.AutoShapeType == AutoShapeType.DoubleBrace || this.AutoShapeType == AutoShapeType.DoubleBracket;
    }
  }

  internal bool IsGroupFill
  {
    get => this._isGroupFill;
    set
    {
      this._isGroupFill = value;
      if (!(this is GroupShape))
        return;
      foreach (Shape shape in (IEnumerable<ISlideItem>) ((GroupShape) this).Shapes)
        shape.IsGroupFill = value;
    }
  }

  public SlideItemType SlideItemType => this._slideItemType;

  internal DrawingType DrawingType
  {
    get => this._drawingType;
    set => this._drawingType = value;
  }

  internal EffectList EffectList
  {
    get => this._effectList;
    set => this._effectList = value;
  }

  internal bool FLocksText
  {
    get => this._fLocksText;
    set => this._fLocksText = value;
  }

  internal bool FPublished
  {
    get => this._fPublished;
    set => this._fPublished = value;
  }

  internal bool IsPhoto
  {
    get => this._isPhoto;
    set => this._isPhoto = value;
  }

  internal bool IsUserDrawn
  {
    get => this._isUserDrawn;
    set => this._isUserDrawn = value;
  }

  internal string Macro
  {
    get => this._macro;
    set => this._macro = value;
  }

  public IPlaceholderFormat PlaceholderFormat => (IPlaceholderFormat) this._placeHolder;

  internal ShapeType ShapeType => this._shapeType;

  internal string TextLink
  {
    get => this._textLink;
    set => this._textLink = value;
  }

  internal Dictionary<string, Stream> PreservedElements
  {
    get => this._preservedElements;
    set => this._preservedElements = value;
  }

  public int Rotation
  {
    get
    {
      if (!this._isGroupedShapeFrameChanged && this.IsInGroupShape() && this.DrawingType != DrawingType.PlaceHolder)
      {
        double groupShapeProperties = this.GetGroupShapeProperties("rotation");
        if (groupShapeProperties != 0.0)
          return (int) groupShapeProperties;
      }
      return (int) Math.Round((double) this._shapeFrame.Rotation / 60000.0);
    }
    set
    {
      if (value < -3600 || value > 3600)
        throw new ArgumentException("Invalid rotation value");
      if (this.IsInGroupShape())
        this._isGroupedShapeFrameChanged = true;
      this._shapeFrame.SetRotationValue(value * 60000);
    }
  }

  internal bool IsLockAspectRatio
  {
    get => this._isLockAspectRatio;
    set => this._isLockAspectRatio = value;
  }

  internal BaseSlide BaseSlide
  {
    get => this._baseSlide;
    set => this._baseSlide = value;
  }

  internal ShapeFrame ShapeFrame => this._shapeFrame;

  internal ShapeFrame GroupFrame => this._groupFrame;

  internal List<Path2D> Path2DList
  {
    get => this._path2DList;
    set => this._path2DList = value;
  }

  internal bool IsPresetGeometry
  {
    get => this._isPresetGeom;
    set => this._isPresetGeom = value;
  }

  internal bool IsCustomGeometry
  {
    get => this._isCustomGeom;
    set => this._isCustomGeom = value;
  }

  public bool IsBgFill
  {
    get => this._isBgFill;
    set => this._isBgFill = value;
  }

  internal Dictionary<string, Stream> StyleStream
  {
    get => this._styleStream ?? (this._styleStream = new Dictionary<string, Stream>());
  }

  internal virtual void Layout()
  {
    this._shapeInfo = new ShapeInfo((IShape) this);
    if (!this.IsInGroupShape())
    {
      float x;
      float y;
      float width;
      float height;
      switch (this)
      {
        case SmartArtShape _:
          SmartArt parentSmartArt = ((SmartArtShape) this).ParentSmartArt;
          if (parentSmartArt.CreatedSmartArt)
          {
            x = (float) (Helper.EmuToPoint((int) this.ShapeFrame.OffsetX) + 159.83999633789063);
            y = (float) (Helper.EmuToPoint((int) this.ShapeFrame.OffsetY) + 56.880001068115234);
          }
          else
          {
            x = (float) (Helper.EmuToPoint((int) this.ShapeFrame.OffsetX) + Helper.EmuToPoint((int) parentSmartArt.ShapeFrame.OffsetX));
            y = (float) (Helper.EmuToPoint((int) this.ShapeFrame.OffsetY) + Helper.EmuToPoint((int) parentSmartArt.ShapeFrame.OffsetY));
          }
          width = (float) this.ShapeFrame.GetDefaultWidth();
          height = (float) this.ShapeFrame.GetDefaultHeight();
          break;
        case SmartArt _:
          if (((SmartArt) this).CreatedSmartArt)
          {
            x = 159.84f;
            y = 56.88f;
            width = 640f;
            height = 426.6666f;
            break;
          }
          goto default;
        default:
          x = (float) this.ShapeFrame.GetDefaultLeft();
          y = (float) this.ShapeFrame.GetDefaultTop();
          width = (float) this.ShapeFrame.GetDefaultWidth();
          height = (float) this.ShapeFrame.GetDefaultHeight();
          break;
      }
      this._shapeInfo.Bounds = new RectangleF(x, y, width, height);
    }
    else
      this._shapeInfo.Bounds = new RectangleF((float) Helper.EmuToPoint((int) this.GroupFrame.OffsetX), (float) Helper.EmuToPoint((int) this.GroupFrame.OffsetY), (float) Helper.EmuToPoint((int) this.GroupFrame.OffsetCX), (float) Helper.EmuToPoint((int) this.GroupFrame.OffsetCY));
    RectangleF layoutRect = this is SmartArtShape ? new RectangleF(0.0f, 0.0f, this._shapeInfo.Bounds.Width, this._shapeInfo.Bounds.Height) : this.GetBoundsToLayoutShapeTextBody(this._shapeInfo.Bounds);
    this.UpdateShapeBoundsToLayoutTextBody(ref layoutRect, this._shapeInfo.Bounds);
    switch (this._textFrame.ObatinTextDirection())
    {
      case TextDirection.Vertical:
      case TextDirection.Vertical270:
      case TextDirection.EastAsianVertical:
        float width1 = layoutRect.Width;
        layoutRect.Width = layoutRect.Height;
        layoutRect.Height = width1;
        break;
    }
    this._shapeInfo.TextLayoutingBounds = layoutRect;
    float usedHeight = 0.0f;
    float maxWidth = 0.0f;
    bool isWrap = this._textFrame.WrapText;
    IParagraphs paragraphs = this._textFrame.Paragraphs;
    int count = paragraphs.Count;
    bool isMultiColumn = false;
    if ((this.DrawingType == DrawingType.None || this.DrawingType == DrawingType.TextBox || this.DrawingType == DrawingType.PlaceHolder) && (this.TextBody as Syncfusion.Presentation.RichText.TextBody).NumberOfColumns > 1)
    {
      isMultiColumn = true;
      isWrap = true;
      float singleColumnWidth = this.GetSingleColumnWidth(layoutRect);
      layoutRect.Width = singleColumnWidth;
    }
    for (int index = 0; index < count; ++index)
      ((Paragraph) paragraphs[index]).Layout(layoutRect, ref usedHeight, isWrap, ref maxWidth);
    if (isMultiColumn)
    {
      this.SplitAsMultiColumn(paragraphs, layoutRect);
      if (this.ShapeInfo.ColumnsInfo.Count > 1)
        usedHeight = this.ShapeInfo.ColumnsInfo[0].Height;
    }
    this.LayoutXYPosition(layoutRect.Height, layoutRect.Width, usedHeight, maxWidth, isMultiColumn);
  }

  private void SplitAsMultiColumn(IParagraphs paragraphCollection, RectangleF textBodyBounds)
  {
    this.ShapeInfo.ColumnsInfo = new List<ColumnInfo>();
    float height = textBodyBounds.Height;
    int paraIndex = 0;
    int lineIndex = 0;
    Paragraph paragraph1 = paragraphCollection[paragraphCollection.Count - 1] as Paragraph;
    int num1 = paragraph1.ParagraphInfo != null ? paragraph1.ParagraphInfo.LineInfoCollection.Count : 1;
    while (paraIndex < paragraphCollection.Count || lineIndex < num1 - 1)
    {
      bool isLastColumn = (this.TextBody as Syncfusion.Presentation.RichText.TextBody).NumberOfColumns == this.ShapeInfo.ColumnsInfo.Count + 1;
      if ((this.TextBody as Syncfusion.Presentation.RichText.TextBody).AutoFitType == AutoMarginType.NoAutoFit || (this.TextBody as Syncfusion.Presentation.RichText.TextBody).AutoFitType == AutoMarginType.NotDefined)
      {
        isLastColumn = false;
        if (this.ShapeInfo.ColumnsInfo.Count == (this.TextBody as Syncfusion.Presentation.RichText.TextBody).NumberOfColumns)
        {
          float num2 = this.GetMinimumHeight(this.ShapeInfo.ColumnsInfo, paragraphCollection);
          Paragraph paragraph2 = paragraphCollection[paraIndex] as Paragraph;
          if (paragraph2.ParagraphInfo != null)
          {
            Syncfusion.Presentation.Layouting.LineInfo lineInfo = paragraph2.ParagraphInfo.LineInfoCollection[lineIndex];
            if ((double) num2 > (double) lineInfo.Height)
              num2 = lineInfo.Height;
          }
          height += num2;
          this.ShapeInfo.ColumnsInfo.Clear();
          paraIndex = 0;
          lineIndex = 0;
        }
      }
      this.ShapeInfo.ColumnsInfo.Add(this.CreateColumnInfo(paragraphCollection, height, ref paraIndex, ref lineIndex, isLastColumn));
    }
    this.SplitColumn(paragraphCollection, textBodyBounds);
  }

  private float GetMinimumHeight(
    List<ColumnInfo> columnInfoCollection,
    IParagraphs paragraphCollection)
  {
    float minimumHeight = 0.0f;
    for (int index = 1; index < columnInfoCollection.Count; ++index)
    {
      ColumnInfo columnInfo = columnInfoCollection[index];
      Syncfusion.Presentation.Layouting.LineInfo lineInfo = (paragraphCollection[columnInfo.ParagraphStartIndex] as Paragraph).ParagraphInfo.LineInfoCollection[columnInfo.LineStartIndex];
      if ((double) minimumHeight == 0.0 || (double) minimumHeight > (double) lineInfo.Height)
        minimumHeight = lineInfo.Height;
    }
    return minimumHeight;
  }

  private void SplitColumn(IParagraphs paragraphCollection, RectangleF textBodyBounds)
  {
    if (this.ShapeInfo.ColumnsInfo.Count <= 1)
      return;
    float width = textBodyBounds.Width;
    double spaceBetweenColumns = (this.TextBody as Syncfusion.Presentation.RichText.TextBody).SpaceBetweenColumns;
    int numberOfColumns = (this.TextBody as Syncfusion.Presentation.RichText.TextBody).NumberOfColumns;
    if ((this.TextBody as Syncfusion.Presentation.RichText.TextBody).RTLColumns)
    {
      float num = (float) ((double) (numberOfColumns - 1) * (double) width + (double) (numberOfColumns - 1) * spaceBetweenColumns);
      foreach (Paragraph paragraph in (IEnumerable<IParagraph>) paragraphCollection)
      {
        if (paragraph.ParagraphInfo != null)
        {
          foreach (Syncfusion.Presentation.Layouting.LineInfo lineInfo in paragraph.ParagraphInfo.LineInfoCollection)
          {
            foreach (TextInfo textInfo in lineInfo.TextInfoCollection)
              textInfo.X += num;
          }
        }
      }
    }
    for (int index1 = 1; index1 < this.ShapeInfo.ColumnsInfo.Count; ++index1)
    {
      ColumnInfo columnInfo = this.ShapeInfo.ColumnsInfo[index1];
      float num1 = width + (float) spaceBetweenColumns;
      float y = textBodyBounds.Y;
      for (int paragraphStartIndex = columnInfo.ParagraphStartIndex; paragraphStartIndex < paragraphCollection.Count; ++paragraphStartIndex)
      {
        int num2 = 0;
        if (paragraphStartIndex == columnInfo.ParagraphStartIndex)
          num2 = columnInfo.LineStartIndex;
        Paragraph paragraph = paragraphCollection[paragraphStartIndex] as Paragraph;
        if (paragraph.ParagraphInfo != null)
        {
          ParagraphInfo paragraphInfo = paragraph.ParagraphInfo;
          for (int index2 = num2; index2 < paragraphInfo.LineInfoCollection.Count; ++index2)
          {
            Syncfusion.Presentation.Layouting.LineInfo lineInfo = paragraphInfo.LineInfoCollection[index2];
            if (paragraphStartIndex != columnInfo.ParagraphStartIndex && index2 == 0)
              y += paragraph.LayoutSpaceBefore(lineInfo);
            float height = lineInfo.Height;
            float lineSpace = paragraph.GetLineSpace(ref height);
            float num3 = y + lineSpace;
            bool flag = lineInfo.HasDifferentHeight();
            foreach (TextInfo textInfo in lineInfo.TextInfoCollection)
            {
              textInfo.Y = !flag ? num3 : lineInfo.MaximumAscent - textInfo.Ascent + num3;
              if (!(this.TextBody as Syncfusion.Presentation.RichText.TextBody).RTLColumns)
                textInfo.X += num1;
              else
                textInfo.X -= num1;
            }
            y += height;
            if (index2 == paragraphInfo.LineInfoCollection.Count - 1)
              y += paragraph.LayoutSpaceAfter(paragraphInfo.LineInfoCollection);
          }
        }
      }
    }
  }

  private ColumnInfo CreateColumnInfo(
    IParagraphs paragraphCollection,
    float textBodyHeight,
    ref int paraIndex,
    ref int lineIndex,
    bool isLastColumn)
  {
    float num = 0.0f;
    ColumnInfo columnInfo = new ColumnInfo(this.TextBody);
    columnInfo.ParagraphStartIndex = paraIndex;
    columnInfo.LineStartIndex = lineIndex;
    while (paraIndex < paragraphCollection.Count)
    {
      Paragraph paragraph = paragraphCollection[paraIndex] as Paragraph;
      if (paraIndex != columnInfo.ParagraphStartIndex)
        lineIndex = 0;
      if (paragraph.ParagraphInfo != null)
      {
        ParagraphInfo paragraphInfo = paragraph.ParagraphInfo;
        while (lineIndex < paragraphInfo.LineInfoCollection.Count)
        {
          Syncfusion.Presentation.Layouting.LineInfo lineInfo = paragraphInfo.LineInfoCollection[lineIndex];
          if (lineIndex == 0 && paraIndex != columnInfo.ParagraphStartIndex)
            num += paragraph.LayoutSpaceBefore(lineInfo);
          float height = lineInfo.Height;
          double lineSpace = (double) paragraph.GetLineSpace(ref height);
          if ((double) num + (double) height > (double) textBodyHeight && !isLastColumn)
          {
            columnInfo.Height = num;
            return columnInfo;
          }
          num += height;
          if (lineIndex == paragraphInfo.LineInfoCollection.Count - 1)
            num += paragraph.LayoutSpaceAfter(paragraphInfo.LineInfoCollection);
          ++lineIndex;
        }
      }
      ++paraIndex;
    }
    columnInfo.Height = num;
    return columnInfo;
  }

  private float GetSingleColumnWidth(RectangleF bounds)
  {
    int numberOfColumns = (this.TextBody as Syncfusion.Presentation.RichText.TextBody).NumberOfColumns;
    double spaceBetweenColumns = (this.TextBody as Syncfusion.Presentation.RichText.TextBody).SpaceBetweenColumns;
    float num1 = bounds.Width / (float) numberOfColumns;
    if (spaceBetweenColumns > 0.0 && numberOfColumns > 1)
    {
      float num2 = (float) spaceBetweenColumns * (float) (numberOfColumns - 1) / (float) numberOfColumns;
      num1 -= num2;
    }
    return (double) num1 <= 0.0 ? 0.0f : num1;
  }

  internal void UpdateShapeBoundsToLayoutTextBody(ref RectangleF layoutRect, RectangleF shapeBounds)
  {
    layoutRect.Height -= layoutRect.Y;
    layoutRect.Y += shapeBounds.Y;
    layoutRect.Width -= layoutRect.X;
    layoutRect.X += shapeBounds.X;
    float defaultLeftMargin = (float) ((Syncfusion.Presentation.RichText.TextBody) this.TextBody).GetDefaultLeftMargin();
    float defaultTopMargin = (float) ((Syncfusion.Presentation.RichText.TextBody) this.TextBody).GetDefaultTopMargin();
    float defaultRightMargin = (float) ((Syncfusion.Presentation.RichText.TextBody) this.TextBody).GetDefaultRightMargin();
    float defaultBottomMargin = (float) ((Syncfusion.Presentation.RichText.TextBody) this.TextBody).GetDefaultBottomMargin();
    layoutRect.X += defaultLeftMargin;
    layoutRect.Y += defaultTopMargin;
    layoutRect.Width -= defaultLeftMargin + defaultRightMargin;
    layoutRect.Height -= defaultTopMargin + defaultBottomMargin;
  }

  private RectangleF GetBoundsToLayoutShapeTextBody(RectangleF bounds)
  {
    Dictionary<string, float> shapeFormula = new FormulaValues(bounds, this.ShapeGuide).ParseShapeFormula(this.AutoShapeType);
    switch (this.AutoShapeType)
    {
      case AutoShapeType.Parallelogram:
      case AutoShapeType.Hexagon:
      case AutoShapeType.Cross:
      case AutoShapeType.SmileyFace:
      case AutoShapeType.NoSymbol:
      case AutoShapeType.FlowChartTerminator:
      case AutoShapeType.FlowChartSummingJunction:
      case AutoShapeType.FlowChartOr:
      case AutoShapeType.Star16Point:
      case AutoShapeType.Star24Point:
      case AutoShapeType.Star32Point:
      case AutoShapeType.Wave:
      case AutoShapeType.OvalCallout:
      case AutoShapeType.SnipSameSideCornerRectangle:
      case AutoShapeType.Teardrop:
        return new RectangleF(shapeFormula["il"], shapeFormula["it"], shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.Trapezoid:
        return new RectangleF(shapeFormula["il"], shapeFormula["it"], shapeFormula["ir"], bounds.Height);
      case AutoShapeType.Diamond:
      case AutoShapeType.FlowChartDecision:
      case AutoShapeType.FlowChartCollate:
        return new RectangleF(bounds.Width / 4f, bounds.Height / 4f, shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.RoundedRectangle:
      case AutoShapeType.Octagon:
      case AutoShapeType.Plaque:
      case AutoShapeType.RoundedRectangularCallout:
      case AutoShapeType.SnipDiagonalCornerRectangle:
        return new RectangleF(shapeFormula["il"], shapeFormula["il"], shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.IsoscelesTriangle:
        return new RectangleF(shapeFormula["x1"], bounds.Height / 2f, shapeFormula["x3"], bounds.Height);
      case AutoShapeType.RightTriangle:
        return new RectangleF(bounds.Width / 12f, shapeFormula["it"], shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.Oval:
      case AutoShapeType.Donut:
      case AutoShapeType.BlockArc:
      case AutoShapeType.Arc:
      case AutoShapeType.CircularArrow:
      case AutoShapeType.FlowChartConnector:
      case AutoShapeType.FlowChartSequentialAccessStorage:
      case AutoShapeType.DoubleWave:
      case AutoShapeType.CloudCallout:
      case AutoShapeType.Chord:
      case AutoShapeType.Cloud:
        return new RectangleF(shapeFormula["il"], shapeFormula["it"], shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.RegularPentagon:
        return new RectangleF(shapeFormula["x2"], shapeFormula["it"], shapeFormula["x3"], shapeFormula["y2"]);
      case AutoShapeType.Can:
        return new RectangleF(0.0f, shapeFormula["y2"], bounds.Width, shapeFormula["y3"]);
      case AutoShapeType.Cube:
        return new RectangleF(0.0f, shapeFormula["y1"], shapeFormula["x4"], bounds.Height);
      case AutoShapeType.Bevel:
        return new RectangleF(shapeFormula["x1"], shapeFormula["x1"], shapeFormula["x2"], shapeFormula["y2"]);
      case AutoShapeType.FoldedCorner:
        return new RectangleF(0.0f, 0.0f, bounds.Width, shapeFormula["y2"]);
      case AutoShapeType.Heart:
        return new RectangleF(shapeFormula["il"], bounds.Height / 4f, shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.LightningBolt:
        return new RectangleF(shapeFormula["x4"], shapeFormula["y4"], shapeFormula["x9"], shapeFormula["y10"]);
      case AutoShapeType.Sun:
        return new RectangleF(shapeFormula["x9"], shapeFormula["y9"], shapeFormula["x8"], shapeFormula["y8"]);
      case AutoShapeType.Moon:
        return new RectangleF(shapeFormula["g12w"], shapeFormula["g15h"], shapeFormula["g0w"], shapeFormula["g16h"]);
      case AutoShapeType.DoubleBracket:
      case AutoShapeType.DoubleBrace:
      case AutoShapeType.FlowChartAlternateProcess:
        return new RectangleF(shapeFormula["il"], shapeFormula["il"], shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.LeftBracket:
      case AutoShapeType.LeftBrace:
        return new RectangleF(shapeFormula["il"], shapeFormula["it"], bounds.Width, shapeFormula["ib"]);
      case AutoShapeType.RightBracket:
      case AutoShapeType.RightBrace:
        return new RectangleF(0.0f, shapeFormula["it"], shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.RightArrow:
        return new RectangleF(0.0f, shapeFormula["y1"], shapeFormula["x2"], shapeFormula["y2"]);
      case AutoShapeType.LeftArrow:
        return new RectangleF(shapeFormula["x1"], shapeFormula["y1"], bounds.Width, shapeFormula["y2"]);
      case AutoShapeType.UpArrow:
      case AutoShapeType.MathEqual:
        return new RectangleF(shapeFormula["x1"], shapeFormula["y1"], shapeFormula["x2"], bounds.Height);
      case AutoShapeType.DownArrow:
        return new RectangleF(shapeFormula["x1"], 0.0f, shapeFormula["x2"], shapeFormula["y2"]);
      case AutoShapeType.LeftRightArrow:
        return new RectangleF(shapeFormula["x1"], shapeFormula["y1"], shapeFormula["x4"], shapeFormula["y2"]);
      case AutoShapeType.UpDownArrow:
        return new RectangleF(shapeFormula["x1"], shapeFormula["y1"], shapeFormula["x2"], shapeFormula["y4"]);
      case AutoShapeType.QuadArrow:
        return new RectangleF(shapeFormula["il"], shapeFormula["y3"], shapeFormula["ir"], shapeFormula["y4"]);
      case AutoShapeType.LeftRightUpArrow:
        return new RectangleF(shapeFormula["il"], shapeFormula["y3"], shapeFormula["ir"], shapeFormula["y5"]);
      case AutoShapeType.UTurnArrow:
      case AutoShapeType.FlowChartProcess:
      case AutoShapeType.RectangularCallout:
      case AutoShapeType.StraightConnector:
        return new RectangleF(0.0f, 0.0f, bounds.Width, bounds.Height);
      case AutoShapeType.LeftUpArrow:
        return new RectangleF(shapeFormula["il"], shapeFormula["y3"], shapeFormula["x4"], shapeFormula["y5"]);
      case AutoShapeType.BentUpArrow:
        return new RectangleF(0.0f, shapeFormula["y2"], shapeFormula["x4"], bounds.Height);
      case AutoShapeType.StripedRightArrow:
        return new RectangleF(shapeFormula["x4"], shapeFormula["y1"], shapeFormula["x6"], shapeFormula["y2"]);
      case AutoShapeType.NotchedRightArrow:
        return new RectangleF(shapeFormula["x1"], shapeFormula["y1"], shapeFormula["x3"], shapeFormula["y2"]);
      case AutoShapeType.Pentagon:
      case AutoShapeType.RoundSingleCornerRectangle:
        return new RectangleF(0.0f, 0.0f, shapeFormula["ir"], bounds.Height);
      case AutoShapeType.Chevron:
        return new RectangleF(shapeFormula["il"], 0.0f, shapeFormula["ir"], bounds.Height);
      case AutoShapeType.RightArrowCallout:
        return new RectangleF(0.0f, 0.0f, shapeFormula["x2"], bounds.Height);
      case AutoShapeType.LeftArrowCallout:
        return new RectangleF(shapeFormula["x2"], 0.0f, bounds.Width, bounds.Height);
      case AutoShapeType.UpArrowCallout:
        return new RectangleF(0.0f, shapeFormula["y2"], bounds.Width, bounds.Height);
      case AutoShapeType.DownArrowCallout:
        return new RectangleF(0.0f, 0.0f, bounds.Width, shapeFormula["y2"]);
      case AutoShapeType.LeftRightArrowCallout:
        return new RectangleF(shapeFormula["x2"], 0.0f, shapeFormula["x3"], bounds.Height);
      case AutoShapeType.UpDownArrowCallout:
        return new RectangleF(0.0f, shapeFormula["y2"], bounds.Width, shapeFormula["y3"]);
      case AutoShapeType.QuadArrowCallout:
        return new RectangleF(shapeFormula["x2"], shapeFormula["y2"], shapeFormula["x7"], shapeFormula["y7"]);
      case AutoShapeType.FlowChartData:
        return new RectangleF(bounds.Width / 5f, 0.0f, shapeFormula["x5"], bounds.Height);
      case AutoShapeType.FlowChartPredefinedProcess:
        return new RectangleF(bounds.Width / 8f, 0.0f, shapeFormula["x2"], bounds.Height);
      case AutoShapeType.FlowChartInternalStorage:
        return new RectangleF(bounds.Width / 8f, bounds.Height / 8f, bounds.Width, bounds.Height);
      case AutoShapeType.FlowChartDocument:
        return new RectangleF(0.0f, 0.0f, bounds.Width, shapeFormula["y1"]);
      case AutoShapeType.FlowChartMultiDocument:
        return new RectangleF(0.0f, shapeFormula["y2"], shapeFormula["x5"], shapeFormula["y8"]);
      case AutoShapeType.FlowChartPreparation:
        return new RectangleF(bounds.Width / 5f, 0.0f, shapeFormula["x2"], bounds.Height);
      case AutoShapeType.FlowChartManualInput:
      case AutoShapeType.FlowChartCard:
        return new RectangleF(0.0f, bounds.Height / 5f, bounds.Width, bounds.Height);
      case AutoShapeType.FlowChartManualOperation:
        return new RectangleF(bounds.Width / 5f, 0.0f, shapeFormula["x3"], bounds.Height);
      case AutoShapeType.FlowChartOffPageConnector:
        return new RectangleF(0.0f, 0.0f, bounds.Width, shapeFormula["y1"]);
      case AutoShapeType.FlowChartPunchedTape:
        return new RectangleF(0.0f, bounds.Height / 5f, bounds.Width, shapeFormula["ib"]);
      case AutoShapeType.FlowChartSort:
        return new RectangleF(bounds.Width / 4f, bounds.Height / 4f, shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.FlowChartExtract:
        return new RectangleF(bounds.Width / 4f, bounds.Height / 2f, shapeFormula["x2"], bounds.Height);
      case AutoShapeType.FlowChartMerge:
        return new RectangleF(bounds.Width / 4f, 0.0f, shapeFormula["x2"], bounds.Height / 2f);
      case AutoShapeType.FlowChartStoredData:
        return new RectangleF(bounds.Width / 6f, 0.0f, shapeFormula["x2"], bounds.Height);
      case AutoShapeType.FlowChartDelay:
        return new RectangleF(0.0f, shapeFormula["it"], shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.FlowChartMagneticDisk:
        return new RectangleF(0.0f, bounds.Height / 3f, bounds.Width, shapeFormula["y3"]);
      case AutoShapeType.FlowChartDirectAccessStorage:
        return new RectangleF(bounds.Width / 6f, 0.0f, shapeFormula["x2"], bounds.Height);
      case AutoShapeType.FlowChartDisplay:
        return new RectangleF(bounds.Width / 6f, 0.0f, shapeFormula["x2"], bounds.Height);
      case AutoShapeType.Explosion1:
        return new RectangleF(shapeFormula["x5"], shapeFormula["y3"], shapeFormula["x21"], shapeFormula["y9"]);
      case AutoShapeType.Explosion2:
        return new RectangleF(shapeFormula["x5"], shapeFormula["y3"], shapeFormula["x19"], shapeFormula["y17"]);
      case AutoShapeType.Star4Point:
        return new RectangleF(shapeFormula["sx1"], shapeFormula["sy1"], shapeFormula["sx2"], shapeFormula["sy2"]);
      case AutoShapeType.Star5Point:
        return new RectangleF(shapeFormula["sx1"], shapeFormula["sy1"], shapeFormula["sx4"], shapeFormula["sy3"]);
      case AutoShapeType.Star8Point:
        return new RectangleF(shapeFormula["sx1"], shapeFormula["sy1"], shapeFormula["sx4"], shapeFormula["sy4"]);
      case AutoShapeType.UpRibbon:
        return new RectangleF(shapeFormula["x2"], 0.0f, shapeFormula["x9"], shapeFormula["y2"]);
      case AutoShapeType.DownRibbon:
        return new RectangleF(shapeFormula["x2"], shapeFormula["y2"], shapeFormula["x9"], bounds.Height);
      case AutoShapeType.CurvedUpRibbon:
        return new RectangleF(shapeFormula["x2"], shapeFormula["y6"], shapeFormula["x5"], shapeFormula["rh"]);
      case AutoShapeType.CurvedDownRibbon:
        return new RectangleF(shapeFormula["x2"], shapeFormula["q1"], shapeFormula["x5"], shapeFormula["y6"]);
      case AutoShapeType.VerticalScroll:
        return new RectangleF(shapeFormula["ch"], shapeFormula["ch"], shapeFormula["x6"], shapeFormula["y4"]);
      case AutoShapeType.HorizontalScroll:
        return new RectangleF(shapeFormula["ch"], shapeFormula["ch"], shapeFormula["x4"], shapeFormula["y6"]);
      case AutoShapeType.DiagonalStripe:
        return new RectangleF(0.0f, 0.0f, shapeFormula["x3"], shapeFormula["y3"]);
      case AutoShapeType.Pie:
        return new RectangleF(shapeFormula["il"], shapeFormula["it"], shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.Decagon:
        return new RectangleF(shapeFormula["x1"], shapeFormula["y2"], shapeFormula["x4"], shapeFormula["y3"]);
      case AutoShapeType.Heptagon:
        return new RectangleF(shapeFormula["x2"], shapeFormula["y1"], shapeFormula["x5"], shapeFormula["ib"]);
      case AutoShapeType.Dodecagon:
        return new RectangleF(shapeFormula["x1"], shapeFormula["y1"], shapeFormula["x4"], shapeFormula["y4"]);
      case AutoShapeType.Star6Point:
        return new RectangleF(shapeFormula["sx1"], shapeFormula["sy1"], shapeFormula["sx4"], shapeFormula["sy2"]);
      case AutoShapeType.Star7Point:
        return new RectangleF(shapeFormula["sx2"], shapeFormula["sy1"], shapeFormula["sx5"], shapeFormula["sy3"]);
      case AutoShapeType.Star10Point:
        return new RectangleF(shapeFormula["sx2"], shapeFormula["sy2"], shapeFormula["sx5"], shapeFormula["sy3"]);
      case AutoShapeType.Star12Point:
        return new RectangleF(shapeFormula["sx2"], shapeFormula["sy2"], shapeFormula["sx5"], shapeFormula["sy5"]);
      case AutoShapeType.RoundSameSideCornerRectangle:
        return new RectangleF(shapeFormula["il"], shapeFormula["tdx"], shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.RoundDiagonalCornerRectangle:
        return new RectangleF(shapeFormula["dx"], shapeFormula["dx"], shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.SnipAndRoundSingleCornerRectangle:
        return new RectangleF(shapeFormula["il"], shapeFormula["il"], shapeFormula["ir"], bounds.Height);
      case AutoShapeType.SnipSingleCornerRectangle:
        return new RectangleF(0.0f, shapeFormula["it"], shapeFormula["ir"], bounds.Height);
      case AutoShapeType.Frame:
        return new RectangleF(shapeFormula["x1"], shapeFormula["x1"], shapeFormula["x4"], shapeFormula["y4"]);
      case AutoShapeType.Corner:
        return new RectangleF(0.0f, shapeFormula["it"], shapeFormula["ir"], bounds.Height);
      case AutoShapeType.MathPlus:
        return new RectangleF(shapeFormula["x1"], shapeFormula["y2"], shapeFormula["x4"], shapeFormula["y3"]);
      case AutoShapeType.MathMinus:
        return new RectangleF(shapeFormula["x1"], shapeFormula["y1"], shapeFormula["x2"], shapeFormula["y2"]);
      case AutoShapeType.MathMultiply:
        return new RectangleF(shapeFormula["xA"], shapeFormula["yB"], shapeFormula["xE"], shapeFormula["yH"]);
      case AutoShapeType.MathDivision:
        return new RectangleF(shapeFormula["x1"], shapeFormula["y3"], shapeFormula["x3"], shapeFormula["y4"]);
      case AutoShapeType.MathNotEqual:
        return new RectangleF(shapeFormula["x1"], shapeFormula["y1"], shapeFormula["x8"], shapeFormula["y4"]);
      default:
        return new RectangleF(0.0f, 0.0f, bounds.Width, bounds.Height);
    }
  }

  private void LayoutXYPosition(
    float shapeHeight,
    float shapeWidth,
    float usedHeight,
    float maxWidth,
    bool isMultiColumn)
  {
    float num1 = 0.0f;
    float num2 = 0.0f;
    Paragraphs paragraphs = (Paragraphs) this._textFrame.Paragraphs;
    switch (this._textFrame.GetDefaultVerticalAlign())
    {
      case VerticalAlignmentType.Middle:
        num1 = (float) (((double) shapeHeight - (double) usedHeight) / 2.0);
        break;
      case VerticalAlignmentType.Bottom:
        num1 = shapeHeight - usedHeight;
        break;
    }
    if (this._textFrame.GetDefaultAnchorCenter() && paragraphs.Count > 0 && paragraphs.HasSameTextAlignment && !isMultiColumn)
    {
      switch (((Paragraph) paragraphs[0]).GetDefaultAlignmentType())
      {
        case HorizontalAlignmentType.Left:
          num2 = (float) (((double) shapeWidth - (double) maxWidth) / 2.0);
          break;
        case HorizontalAlignmentType.Right:
          num2 = (float) (((double) shapeWidth - (double) maxWidth) / -2.0);
          break;
      }
    }
    if ((double) num1 == 0.0 && (double) num2 == 0.0)
      return;
    foreach (IParagraph paragraph in paragraphs)
    {
      if (((Paragraph) paragraph).ParagraphInfo != null)
      {
        foreach (Syncfusion.Presentation.Layouting.LineInfo lineInfo in ((Paragraph) paragraph).ParagraphInfo.LineInfoCollection)
        {
          foreach (TextInfo textInfo in lineInfo.TextInfoCollection)
          {
            textInfo.Y += num1;
            textInfo.X += num2;
          }
        }
      }
    }
  }

  internal void SetTextBody(Syncfusion.Presentation.RichText.TextBody textBody)
  {
    this._textFrame = textBody;
  }

  internal bool HasAdditionalGraphicsPath()
  {
    AutoShapeType autoShapeType = this.GetAutoShapeType();
    switch (autoShapeType)
    {
      case AutoShapeType.SmileyFace:
      case AutoShapeType.Arc:
        return true;
      default:
        return autoShapeType == AutoShapeType.HorizontalScroll;
    }
  }

  internal bool IsFitWithInBounds()
  {
    AutoShapeType autoShapeType = this.GetAutoShapeType();
    switch (autoShapeType)
    {
      case AutoShapeType.RectangularCallout:
      case AutoShapeType.OvalCallout:
      case AutoShapeType.CloudCallout:
        return false;
      default:
        return autoShapeType != AutoShapeType.RoundedRectangularCallout;
    }
  }

  internal Syncfusion.Presentation.Drawing.Fill GetFillFormat() => this._fillFormat;

  internal void AddDefaultTextStylestream()
  {
    if (this._baseSlide.Presentation.PreservedElements.ContainsKey("defaultTextStyle"))
      return;
    Stream defaultContentZip = this._baseSlide.Presentation.DataHolder.GetItemFromDefaultContentZip("ppt/presentation.xml");
    if (defaultContentZip == null || defaultContentZip.Length <= 0L)
      return;
    defaultContentZip.Position = 0L;
    UtilityMethods.CreateReader(defaultContentZip).ReadToFollowing("defaultTextStyle", "http://schemas.openxmlformats.org/presentationml/2006/main");
    this._baseSlide.Presentation.PreservedElements.Add("defaultTextStyle", defaultContentZip);
  }

  internal void AddTextStyleStream()
  {
    if (this._baseSlide.SlidePrsvedElts.ContainsKey("txStyles"))
      return;
    Stream defaultContentZip = this._baseSlide.Presentation.DataHolder.GetItemFromDefaultContentZip("slideMasters/slideMaster1.xml");
    if (defaultContentZip == null || defaultContentZip.Length <= 0L)
      return;
    defaultContentZip.Position = 0L;
    UtilityMethods.CreateReader(defaultContentZip).ReadToFollowing("txStyles", "http://schemas.openxmlformats.org/presentationml/2006/main");
    this._baseSlide.SlidePrsvedElts.Add("txSTyle", defaultContentZip);
  }

  internal void SetPlaceholder(Placeholder placeholder) => this._placeHolder = placeholder;

  public IHyperLink SetHyperlink(string link) => (IHyperLink) this.AddHyperlink(link);

  public void RemoveHyperlink()
  {
    this._hyperlink.Close();
    if (this._baseSlide is Slide)
      (this._baseSlide as Slide).TopRelation.RemoveRelationByKeword("hyperlink");
    this._hyperlink = (Syncfusion.Presentation.RichText.Hyperlink) null;
  }

  public Syncfusion.Presentation.RichText.Hyperlink AddHyperlink(string url)
  {
    if (url == null)
      throw new ArgumentException("Link cannot be null");
    int result = 0;
    this._hyperlink = new Syncfusion.Presentation.RichText.Hyperlink(this);
    ISlides slides = this._baseSlide.Presentation.Slides;
    if (int.TryParse(url, out result) && result >= 0 && result < slides.Count)
    {
      this.AddHyperLink(slides[result]);
      return this._hyperlink;
    }
    if (url.StartsWith("www"))
      url = "http://" + url;
    else if (url.Contains("@") && !url.StartsWith("@") && !url.EndsWith("."))
    {
      if (!url.StartsWith("mailto:"))
        url = "mailto:" + url;
    }
    else if (url.Contains("#"))
    {
      url.IndexOf("#");
      url = url.Remove(result, url.Length - result);
    }
    this._hyperlink.ActionString = !url.ToLowerInvariant().EndsWith(".pptx") ? "ppaction://hlinkfile" : "ppaction://hlinkpres";
    this._hyperlink.SetLink(url);
    return this._hyperlink;
  }

  public void AddHyperLink(ISlide slide)
  {
    this._hyperlink = new Syncfusion.Presentation.RichText.Hyperlink(this);
    this._hyperlink.ActionString = "ppaction://hlinksldjump";
    this._hyperlink.SetTargetSlide(slide);
    this._hyperlink.SetTargetSlideRelation();
  }

  internal void SetHyperlink(Syncfusion.Presentation.RichText.Hyperlink hyperLink)
  {
    this._hyperlink = hyperLink;
  }

  internal LineJoinType GetLineJoinTypeFromStyle(string localName)
  {
    Stream data;
    if (this.PreservedElements.TryGetValue("style", out data) && data != null && data.Length > 0L)
    {
      data.Position = 0L;
      XmlReader reader = UtilityMethods.CreateReader(data);
      reader.ReadToFollowing(localName, "http://schemas.openxmlformats.org/drawingml/2006/main");
      if (reader.NodeType != XmlNodeType.None && reader.LocalName == "lnRef")
      {
        int num = Helper.ToInt(reader.GetAttribute("idx"));
        switch (num)
        {
          case 0:
          case 1000:
            return LineJoinType.None;
          default:
            LineJoinType lineJoinType = this._baseSlide.BaseTheme.LineFormats[num - 1].LineJoinType;
            return lineJoinType != LineJoinType.None ? lineJoinType : LineJoinType.Round;
        }
      }
    }
    return LineJoinType.None;
  }

  internal void SetSlideItemType(SlideItemType slideItemType)
  {
    this._slideItemType = slideItemType;
  }

  internal LineCapStyle GetLineCapStyleFromStyle(string localName)
  {
    Stream data;
    if (this.PreservedElements.TryGetValue("style", out data) && data != null && data.Length > 0L)
    {
      data.Position = 0L;
      XmlReader reader = UtilityMethods.CreateReader(data);
      reader.ReadToFollowing(localName, "http://schemas.openxmlformats.org/drawingml/2006/main");
      if (reader.NodeType != XmlNodeType.None && reader.LocalName == "lnRef")
      {
        int num = Helper.ToInt(reader.GetAttribute("idx"));
        switch (num)
        {
          case 0:
          case 1000:
            return LineCapStyle.None;
          default:
            return this._baseSlide.BaseTheme.LineFormats[num - 1].CapStyle;
        }
      }
    }
    return LineCapStyle.None;
  }

  internal Dictionary<string, string> ShapeGuide
  {
    get => this._guideList ?? (this._guideList = new Dictionary<string, string>());
  }

  internal bool IsDefaultSize()
  {
    return this._shapeFrame.OffsetX != 0L && this._shapeFrame.OffsetY != 0L && this._shapeFrame.OffsetCY != 0L && this._shapeFrame.OffsetCY != 0L;
  }

  internal Placeholder GetPlaceholder() => this._placeHolder;

  internal Dictionary<string, string> GetGuideList()
  {
    return this._guideList ?? (this._guideList = new Dictionary<string, string>());
  }

  internal Dictionary<string, string> GetAvList()
  {
    return this._avList ?? (this._avList = new Dictionary<string, string>());
  }

  internal void SetGuideList(Dictionary<string, string> guideList) => this._guideList = guideList;

  internal void SetFill(Syncfusion.Presentation.Drawing.Fill fill) => this._fillFormat = fill;

  internal void UpdateGroupFrame(
    List<RectangleF> groupShapeBounds,
    List<float> groupShapeRotations,
    List<bool?> groupShapeHorzFlips,
    List<bool?> groupShapeVertFlips,
    Shape shape)
  {
    if (shape is GroupShape)
    {
      if (shape._groupShape == null)
      {
        RectangleF rectangleF = new RectangleF((float) Helper.EmuToPoint(shape.ShapeFrame.OffsetX), (float) Helper.EmuToPoint(shape.ShapeFrame.OffsetY), (float) Helper.EmuToPoint(shape.ShapeFrame.OffsetCX), (float) Helper.EmuToPoint(shape.ShapeFrame.OffsetCY));
        groupShapeBounds.Add(rectangleF);
      }
      int rotation = shape.ShapeFrame.Rotation;
      if (rotation != -1)
        groupShapeRotations.Add((float) rotation / 60000f);
      else
        groupShapeRotations.Add(0.0f);
      groupShapeHorzFlips.Add(shape.ShapeFrame.GetFlipH());
      groupShapeVertFlips.Add(shape.ShapeFrame.GetFlipV());
    }
    if (shape._groupFrame != null || !shape.IsInGroupShape())
      return;
    shape._groupFrame = new ShapeFrame(shape);
    ShapeFrame shapeFrame = shape._groupShape.GroupFrame ?? shape._groupShape.ShapeFrame;
    double num1 = Math.Round((double) shapeFrame.OffsetCX / (double) shapeFrame.ChOffsetCX, 4);
    double num2 = Math.Round((double) shapeFrame.OffsetCY / (double) shapeFrame.ChOffsetCY, 4);
    long num3 = (long) Math.Round((double) shapeFrame.OffsetX + (double) (shape.ShapeFrame.OffsetX - shapeFrame.ChOffsetX) * num1);
    long num4 = (long) Math.Round((double) shapeFrame.OffsetY + (double) (shape.ShapeFrame.OffsetY - shapeFrame.ChOffsetY) * num2);
    long offsetCx = shape.ShapeFrame.OffsetCX;
    long offsetCy = shape.ShapeFrame.OffsetCY;
    int rotation1 = shape.ShapeFrame.Rotation == -1 ? 0 : shape.ShapeFrame.Rotation % 21600000;
    if (rotation1 >= 2700000 && rotation1 <= 8099999 || rotation1 >= 13500000 && rotation1 <= 18899999)
    {
      double num5 = num1;
      num1 = num2;
      num2 = num5;
      num3 = (long) ((double) num3 - (double) offsetCx * (num1 - num2) / 2.0);
      num4 = (long) ((double) num4 - (double) offsetCy * (num2 - num1) / 2.0);
    }
    long num6 = (long) Math.Round((double) offsetCx * num1);
    long num7 = (long) Math.Round((double) offsetCy * num2);
    shape._groupFrame.SetChildAnchor(shape.ShapeFrame.ChOffsetX, shape.ShapeFrame.ChOffsetY, shape.ShapeFrame.ChOffsetCX, shape.ShapeFrame.ChOffsetCY);
    if (!shape.IsGroup)
    {
      float num8 = 0.0f;
      bool flag1 = false;
      bool flag2 = false;
      for (int index = 0; index <= groupShapeRotations.Count; ++index)
      {
        float num9 = index != groupShapeRotations.Count ? groupShapeRotations[index] : (float) (rotation1 / 60000);
        if (index > 0)
        {
          if (groupShapeHorzFlips[index - 1].HasValue)
            flag1 ^= groupShapeHorzFlips[index - 1].Value;
          if (groupShapeVertFlips[index - 1].HasValue)
            flag2 ^= groupShapeVertFlips[index - 1].Value;
        }
        if (flag1 ^ flag2)
          num8 -= num9;
        else
          num8 += num9;
      }
      if ((double) num8 != -1.0)
        rotation1 = (int) ((double) (num8 % 360f) * 60000.0) % 21600000;
      RectangleF childShapeBounds = new RectangleF((float) Helper.EmuToPoint(num3), (float) Helper.EmuToPoint(num4), (float) Helper.EmuToPoint(num6), (float) Helper.EmuToPoint(num7));
      for (int index = groupShapeBounds.Count - 1; index >= 0; --index)
      {
        bool flag3 = false;
        bool flag4 = false;
        float groupShapeRotation = groupShapeRotations[index];
        RectangleF groupShapeBound = groupShapeBounds[index];
        if (groupShapeHorzFlips[index].HasValue)
          flag3 = groupShapeHorzFlips[index].Value;
        if (groupShapeVertFlips[index].HasValue)
          flag4 = groupShapeVertFlips[index].Value;
        if (flag3 || flag4)
        {
          PointF[] pointFArray = new PointF[4]
          {
            new PointF(childShapeBounds.X, childShapeBounds.Y),
            new PointF(childShapeBounds.X + childShapeBounds.Width, childShapeBounds.Y),
            new PointF(childShapeBounds.Right, childShapeBounds.Bottom),
            new PointF(childShapeBounds.X, childShapeBounds.Y + childShapeBounds.Height)
          };
          Matrix matrix1 = new Matrix();
          PointF pointF = new PointF(groupShapeBound.X + groupShapeBound.Width / 2f, groupShapeBound.Y + groupShapeBound.Height / 2f);
          Matrix matrix2 = new Matrix(1f, 0.0f, 0.0f, -1f, 0.0f, 0.0f);
          Matrix matrix3 = new Matrix(-1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
          if (flag4)
          {
            matrix1.Multiply(matrix2, MatrixOrder.Append);
            matrix1.Translate(0.0f, pointF.Y * 2f, MatrixOrder.Append);
          }
          if (flag3)
          {
            matrix1.Multiply(matrix3, MatrixOrder.Append);
            matrix1.Translate(pointF.X * 2f, 0.0f, MatrixOrder.Append);
          }
          matrix1.TransformPoints(pointFArray);
          childShapeBounds = Shape.CreateRect(pointFArray);
        }
        if ((double) groupShapeRotation != 0.0)
          childShapeBounds = this.GetChildShapePositionToDraw(groupShapeBound, groupShapeRotation, childShapeBounds);
      }
      num3 = Helper.PointToEmuLong((double) childShapeBounds.Left);
      num4 = Helper.PointToEmuLong((double) childShapeBounds.Top);
      num6 = Helper.PointToEmuLong((double) childShapeBounds.Width);
      num7 = Helper.PointToEmuLong((double) childShapeBounds.Height);
    }
    if (!shape.IsGroup && (this.IsGroupFlipV(shape.Group) || this.IsGroupFlipH(shape.Group)))
    {
      int flipHcount = this.GetFlipHCount(shape.Group, shape.ShapeFrame.FlipHorizontal ? 1 : 0);
      int flipVcount = this.GetFlipVCount(shape.Group, shape.ShapeFrame.FlipVertical ? 1 : 0);
      bool flag5 = flipHcount % 2 != 0;
      bool flag6 = flipVcount % 2 != 0;
      shape._groupFrame.SetAnchor(new bool?(flag6), new bool?(flag5), rotation1, num3, num4, num6, num7);
    }
    else
      shape._groupFrame.SetAnchor(new bool?(shape.ShapeFrame.FlipVertical), new bool?(shape.ShapeFrame.FlipHorizontal), rotation1, num3, num4, num6, num7);
    if (!(shape is GroupShape))
      return;
    RectangleF rectangleF1 = new RectangleF((float) Helper.EmuToPoint(num3), (float) Helper.EmuToPoint(num4), (float) Helper.EmuToPoint(num6), (float) Helper.EmuToPoint(num7));
    groupShapeBounds.Add(rectangleF1);
  }

  private bool IsGroupRotated(List<float> groupShapeRotations)
  {
    if (groupShapeRotations.Count > 1)
    {
      for (int index = groupShapeRotations.Count - 2; index >= 0; --index)
      {
        float groupShapeRotation = groupShapeRotations[index];
        if ((double) groupShapeRotation != 0.0 && (double) groupShapeRotation != -1.0)
          return true;
      }
    }
    return false;
  }

  internal bool IsGroupFlipH(GroupShape group)
  {
    for (; group != null; group = group.Group)
    {
      if (group.ShapeFrame.FlipHorizontal)
        return true;
    }
    return false;
  }

  internal bool IsGroupFlipV(GroupShape group)
  {
    for (; group != null; group = group.Group)
    {
      if (group.ShapeFrame.FlipVertical)
        return true;
    }
    return false;
  }

  internal int GetFlipHCount(GroupShape group, int count)
  {
    for (; group != null; group = group.Group)
    {
      if (group.ShapeFrame.FlipHorizontal)
        ++count;
    }
    return count;
  }

  internal int GetFlipVCount(GroupShape group, int count)
  {
    for (; group != null; group = group.Group)
    {
      if (group.ShapeFrame.FlipVertical)
        ++count;
    }
    return count;
  }

  private static RectangleF CreateRect(PointF[] points)
  {
    float x1 = float.MaxValue;
    float y1 = float.MaxValue;
    float num1 = float.MinValue;
    float num2 = float.MinValue;
    int length = points.Length;
    for (int index = 0; index < length; ++index)
    {
      float x2 = points[index].X;
      float y2 = points[index].Y;
      if ((double) x2 < (double) x1)
        x1 = x2;
      if ((double) x2 > (double) num1)
        num1 = x2;
      if ((double) y2 < (double) y1)
        y1 = y2;
      if ((double) y2 > (double) num2)
        num2 = y2;
    }
    return new RectangleF(x1, y1, num1 - x1, num2 - y1);
  }

  private RectangleF GetChildShapePositionToDraw(
    RectangleF groupShapeBounds,
    float groupShapeRotation,
    RectangleF childShapeBounds)
  {
    double num1 = (double) groupShapeBounds.X + (double) groupShapeBounds.Width / 2.0;
    double num2 = (double) groupShapeBounds.Y + (double) groupShapeBounds.Height / 2.0;
    if ((double) groupShapeRotation > 360.0)
      groupShapeRotation %= 360f;
    double num3 = (double) groupShapeRotation * Math.PI / 180.0;
    double num4 = Math.Sin(num3);
    double num5 = Math.Cos(num3);
    double num6 = (double) childShapeBounds.X + (double) childShapeBounds.Width / 2.0;
    double num7 = (double) childShapeBounds.Y + (double) childShapeBounds.Height / 2.0;
    double num8 = num1 + ((double) childShapeBounds.X - num1) * num5 - ((double) childShapeBounds.Y - num2) * num4;
    double num9 = num2 + ((double) childShapeBounds.X - num1) * num4 + ((double) childShapeBounds.Y - num2) * num5;
    double num10 = num1 + (num6 - num1) * num5 - (num7 - num2) * num4;
    double num11 = num2 + (num6 - num1) * num4 + (num7 - num2) * num5;
    double num12 = (360.0 - (double) groupShapeRotation) * Math.PI / 180.0;
    double num13 = Math.Sin(num12);
    double num14 = Math.Cos(num12);
    return new RectangleF((float) (num10 + (num8 - num10) * num14 - (num9 - num11) * num13), (float) (num11 + (num8 - num10) * num13 + (num9 - num11) * num14), childShapeBounds.Width, childShapeBounds.Height);
  }

  internal IColor GetThemeColor(string localName)
  {
    Stream data;
    if (this.PreservedElements.TryGetValue("style", out data) && data != null && data.Length > 0L)
    {
      data.Position = 0L;
      XmlReader reader = UtilityMethods.CreateReader(data);
      reader.ReadToFollowing(localName, "http://schemas.openxmlformats.org/drawingml/2006/main");
      if (reader.NodeType != XmlNodeType.None)
      {
        switch (localName)
        {
          case "fillRef":
            switch (reader.GetAttribute("idx"))
            {
              case "0":
              case "1000":
                return ColorObject.Transparent;
            }
            break;
          case "fontRef":
            switch (reader.GetAttribute("idx"))
            {
              case "0":
              case "1000":
                return ColorObject.Transparent;
            }
            break;
          case "lnRef":
            switch (reader.GetAttribute("idx"))
            {
              case "0":
              case "1000":
                return ColorObject.Transparent;
            }
            break;
        }
        reader.Read();
        this.SkipWhitespaces(reader);
        if (reader.LocalName == "schemeClr")
        {
          ColorObject color = new ColorObject(true);
          DrawingParser.ParseScheme(reader, color, (MasterSlide) this._baseSlide.Presentation.Masters[0]);
          color.UpdateColorObject((object) this._baseSlide.Presentation);
          return (IColor) color;
        }
      }
    }
    return ColorObject.Empty;
  }

  internal string GetFontFromStyle(string localName)
  {
    Stream data;
    if (this.PreservedElements.TryGetValue("style", out data) && data != null && data.Length > 0L)
    {
      data.Position = 0L;
      XmlReader reader = UtilityMethods.CreateReader(data);
      reader.ReadToFollowing(localName, "http://schemas.openxmlformats.org/drawingml/2006/main");
      if (reader.NodeType != XmlNodeType.None && reader.LocalName == "fontRef")
      {
        string attribute = reader.GetAttribute("idx");
        switch (attribute)
        {
          case "0":
          case "1000":
            return (string) null;
          default:
            return attribute;
        }
      }
    }
    return (string) null;
  }

  internal double GetLineWidthFromStyle(string localName)
  {
    Stream data = (Stream) null;
    if (this.PreservedElements != null && this.PreservedElements.Count != 0 && !this.PreservedElements.ContainsKey("style"))
      return this._baseSlide.BaseTheme.LineFormats[0].Weight;
    if (this.PreservedElements != null && this.PreservedElements.TryGetValue("style", out data) && data != null && data.Length > 0L)
    {
      data.Position = 0L;
      XmlReader reader = UtilityMethods.CreateReader(data);
      reader.ReadToFollowing(localName, "http://schemas.openxmlformats.org/drawingml/2006/main");
      if (reader.NodeType != XmlNodeType.None && reader.LocalName == "lnRef")
      {
        int num = Helper.ToInt(reader.GetAttribute("idx"));
        switch (num)
        {
          case 0:
          case 1000:
            return 0.0;
          default:
            return this._baseSlide.BaseTheme.LineFormats[num - 1].Weight;
        }
      }
    }
    return 0.0;
  }

  private void SkipWhitespaces(XmlReader reader)
  {
    if (reader.NodeType == XmlNodeType.Element)
      return;
    while (reader.NodeType == XmlNodeType.Whitespace)
      reader.Read();
  }

  public virtual ISlideItem Clone()
  {
    Shape shape = (Shape) this.MemberwiseClone();
    this.Clone(shape);
    return (ISlideItem) shape;
  }

  internal void Clone(Shape shape)
  {
    if (this._effectList != null)
      shape._effectList = this._effectList.Clone();
    shape._fillFormat = this._fillFormat.Clone();
    shape._fillFormat.SetParent((object) shape);
    shape._gdiRenderer = (GDIRenderer) null;
    if (this._shapeInfo != null)
    {
      shape._shapeInfo = this._shapeInfo.Clone();
      shape._shapeInfo.SetParent(shape);
    }
    if (this._groupFrame != null)
    {
      shape._groupFrame = this._groupFrame.Clone();
      shape._groupFrame.SetParent(shape);
    }
    if (this._guideList != null)
      shape._guideList = Helper.CloneDictionary(this._guideList);
    if (this._hyperlink != null)
    {
      shape._hyperlink = this._hyperlink.Clone();
      shape._hyperlink.SetParent(shape);
    }
    if (this._lineFormat != null)
    {
      shape._lineFormat = this._lineFormat.Clone();
      shape._lineFormat.SetParent(shape);
    }
    if (this._path2DList != null)
      shape._path2DList = this.ClonePath2DList();
    if (this._placeHolder != null)
    {
      shape._placeHolder = this._placeHolder.Clone();
      shape._placeHolder.SetParent(shape);
    }
    if (this._preservedElements != null)
    {
      shape._preservedElements = Helper.CloneDictionary(this._preservedElements);
      if (shape._preservedElements.ContainsKey("custDataLst"))
        shape._preservedElements.Remove("custDataLst");
    }
    shape._shapeFrame = this._shapeFrame.Clone();
    shape._shapeFrame.SetParent(shape);
    if (this._styleStream != null)
      shape._styleStream = Helper.CloneDictionary(this._styleStream);
    if (this._textFrame != null)
    {
      shape._textFrame = this._textFrame.Clone();
      shape._textFrame.SetParent(shape);
    }
    shape._baseSlide = (BaseSlide) null;
  }

  private List<Path2D> ClonePath2DList()
  {
    List<Path2D> path2DList = new List<Path2D>();
    foreach (Path2D path2D1 in this._path2DList)
    {
      Path2D path2D2 = path2D1.Clone();
      path2DList.Add(path2D2);
    }
    return path2DList;
  }

  internal virtual void SetParent(BaseSlide newParent)
  {
    this._baseSlide = newParent;
    if (this._textFrame != null)
      this._textFrame.SetParent(newParent);
    if (this._lineFormat == null)
      return;
    this._lineFormat.SetParent(this);
  }

  internal virtual void Close()
  {
    if (this._isDisposed)
      return;
    if (this._preservedElements != null)
    {
      foreach (KeyValuePair<string, Stream> preservedElement in this._preservedElements)
        preservedElement.Value.Dispose();
      this._preservedElements.Clear();
      this._preservedElements = (Dictionary<string, Stream>) null;
    }
    if (this._fillFormat != null)
    {
      this._fillFormat.Close();
      this._fillFormat = (Syncfusion.Presentation.Drawing.Fill) null;
    }
    if (this._effectList != null)
      this._effectList.Close();
    if (this._lineFormat != null)
    {
      this._lineFormat.Close();
      this._lineFormat = (Syncfusion.Presentation.Drawing.LineFormat) null;
    }
    if (this._placeHolder != null)
    {
      this._placeHolder.Close();
      this._placeHolder = (Placeholder) null;
    }
    if (this._shapeFrame != null)
    {
      this._shapeFrame.Close();
      this._shapeFrame = (ShapeFrame) null;
    }
    if (this._textFrame != null)
    {
      this._textFrame.Close();
      this._textFrame = (Syncfusion.Presentation.RichText.TextBody) null;
    }
    if (this._guideList != null)
    {
      this._guideList.Clear();
      this._guideList = (Dictionary<string, string>) null;
    }
    if (this._path2DList != null)
    {
      foreach (Path2D path2D in this._path2DList)
        path2D.Close();
      this._path2DList.Clear();
      this._path2DList = (List<Path2D>) null;
    }
    if (this._styleStream != null)
    {
      foreach (KeyValuePair<string, Stream> keyValuePair in this._styleStream)
        keyValuePair.Value.Dispose();
      this._styleStream.Clear();
      this._styleStream = (Dictionary<string, Stream>) null;
    }
    if (this._groupFrame != null)
    {
      this._groupFrame.Close();
      this._groupFrame = (ShapeFrame) null;
    }
    if (this._groupShape != null)
      this._groupShape = (GroupShape) null;
    if (this._hyperlink != null)
    {
      this._hyperlink.Close();
      this._hyperlink = (Syncfusion.Presentation.RichText.Hyperlink) null;
    }
    this._baseSlide = (BaseSlide) null;
    if (this._graphics != null)
    {
      this._graphics.Dispose();
      this._graphics = (Graphics) null;
    }
    if (this._gdiRenderer != null)
      this._gdiRenderer = (GDIRenderer) null;
    if (this._shapeInfo != null)
      this._shapeInfo = (ShapeInfo) null;
    this._isDisposed = true;
  }

  internal void SetParent(Syncfusion.Presentation.Presentation presentation)
  {
    if (this._effectList != null)
      this._effectList.SetParent(presentation);
    if (this._textFrame == null)
      return;
    this._textFrame.SetParent(presentation);
  }
}
