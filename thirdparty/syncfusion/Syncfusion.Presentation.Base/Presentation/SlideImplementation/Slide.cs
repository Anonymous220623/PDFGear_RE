// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.SlideImplementation.Slide
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Drawing;
using Syncfusion.Presentation.Drawing;
using Syncfusion.Presentation.Layouting;
using Syncfusion.Presentation.RichText;
using Syncfusion.Presentation.SmartArtImplementation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.Presentation.SlideImplementation;

internal class Slide : BaseSlide, ISlide, IBaseSlide
{
  private SlideInfo _slideInfo;
  private List<Dictionary<string, RectangleF>> _hyperlinks;
  private string _slideId;
  private string _layoutTarget;
  private string _layoutIndex;
  private bool _showMasterShape;
  private SlideLayoutType _layoutType;
  private bool _isSlideVisible = true;
  private Syncfusion.Presentation.Presentation _presentation;
  private bool _isCloned;
  private string _notesRelationId;
  private string _commentRelationId;
  private Syncfusion.Presentation.SlideImplementation.NotesSlide _notesSlide;
  private bool? _hasNotes;
  private bool? _hasComment;
  private string _sectionId;
  private bool _isPortableRendering;
  private Syncfusion.Presentation.CommentImplementation.Comments _commentList;

  internal Slide(Syncfusion.Presentation.Presentation presentation, string slideId)
    : base(presentation)
  {
    this._slideId = slideId;
    this._presentation = presentation;
    this._showMasterShape = true;
  }

  internal void EnablePortableRendering(bool settings) => this._isPortableRendering = settings;

  public IShape AddTextBox(double left, double top, double width, double height)
  {
    return this.Shapes.AddTextBox(left, top, width, height);
  }

  public INotesSlide AddNotesSlide()
  {
    if (this._notesSlide != null)
      return (INotesSlide) this._notesSlide;
    this._notesSlide = new Syncfusion.Presentation.SlideImplementation.NotesSlide(this, ++this._presentation.NotesSlideCount);
    this.Presentation.BuiltInDocumentProperties.NoteCount = this.Presentation.NotesSlideCount;
    ((Syncfusion.Presentation.Drawing.Shapes) this._notesSlide.Shapes).AddNotesShapes();
    string relationIdentifier = Syncfusion.Presentation.Drawing.Helper.GenerateRelationIdentifier(this.TopRelation);
    string str = $"notesSlides/notesSlide{this._notesSlide.Index.ToString()}.xml";
    this.TopRelation.Add(relationIdentifier, new Relation(relationIdentifier, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/notesSlide", "../" + str, (string) null));
    this.InitializeNotesMasterSlide();
    this._presentation.AddOverrideContentType("/ppt/" + str, "application/vnd.openxmlformats-officedocument.presentationml.notesSlide+xml");
    this._hasNotes = new bool?(true);
    this._notesSlide.TopRelation.Add("rId1", new Relation("rId1", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/notesMaster", "../notesMasters/notesMaster1.xml", (string) null));
    this._notesSlide.TopRelation.Add("rId2", new Relation("rId2", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/slide", $"../slides/slide{Syncfusion.Presentation.Drawing.Helper.ToString(this.SlideNumber)}.xml", (string) null));
    return (INotesSlide) this._notesSlide;
  }

  internal void InitializeNotesMasterSlide()
  {
    if (this._presentation.NotesMaster != null && this._presentation.NotesList.Count > 0)
      return;
    NotesMasterSlide notesMaster1 = new NotesMasterSlide(this._presentation, "");
    this.InitializeNotesMasterRelation(notesMaster1);
    this._presentation.DataHolder.ParseThemeFromDefaultContent(notesMaster1.TopRelation, notesMaster1.Theme);
    int num1 = 2;
    NotesMasterSlide notesMaster2 = notesMaster1;
    int shapeId1 = num1;
    int num2 = shapeId1 + 1;
    this.InitializeShapeForNotesMaster(notesMaster2, shapeId1, PlaceholderType.Header, 0, 0, 2971800, 458788);
    NotesMasterSlide notesMaster3 = notesMaster1;
    int shapeId2 = num2;
    int num3 = shapeId2 + 1;
    this.InitializeShapeForNotesMaster(notesMaster3, shapeId2, PlaceholderType.Date, 3884613, 0, 2971800, 458788);
    NotesMasterSlide notesMaster4 = notesMaster1;
    int shapeId3 = num3;
    int num4 = shapeId3 + 1;
    this.InitializeShapeForNotesMaster(notesMaster4, shapeId3, PlaceholderType.SlideImage, 685800, 1143000, 5486400, 3086100);
    NotesMasterSlide notesMaster5 = notesMaster1;
    int shapeId4 = num4;
    int num5 = shapeId4 + 1;
    this.InitializeShapeForNotesMaster(notesMaster5, shapeId4, PlaceholderType.Body, 685800, 4400550, 5486400, 3600450);
    NotesMasterSlide notesMaster6 = notesMaster1;
    int shapeId5 = num5;
    int num6 = shapeId5 + 1;
    this.InitializeShapeForNotesMaster(notesMaster6, shapeId5, PlaceholderType.Footer, 0, 8685213, 2971800, 458787);
    NotesMasterSlide notesMaster7 = notesMaster1;
    int shapeId6 = num6;
    int num7 = shapeId6 + 1;
    this.InitializeShapeForNotesMaster(notesMaster7, shapeId6, PlaceholderType.SlideNumber, 3884613, 8685213, 2971800, 458787);
    Syncfusion.Presentation.RichText.TextBody textBody = new Syncfusion.Presentation.RichText.TextBody((BaseSlide) notesMaster1);
    Paragraph paragraph = new Paragraph(textBody.Paragraphs as Paragraphs);
    paragraph.HorizontalAlignment = HorizontalAlignmentType.Left;
    paragraph.SetMarginLeft(0);
    paragraph.Font.FontSize = 12f;
    paragraph.Font.FontName = "Calibri";
    Fill fill = ((Syncfusion.Presentation.RichText.Font) paragraph.Font).Fill as Fill;
    fill.FillType = FillType.Solid;
    ColorObject colorObject = new ColorObject(true);
    colorObject.SetColor(ColorType.Theme, "tx1");
    ((SolidFill) fill.SolidFill).SetColorObject(colorObject);
    notesMaster1.NotesTextStyle = textBody;
    notesMaster1.NotesTextStyle.StyleList.Add("lvl1pPr", paragraph);
    this._presentation.NotesMaster = notesMaster1;
    Relation relation = new Relation(Syncfusion.Presentation.Drawing.Helper.GenerateRelationId(this._presentation.TopRelation), "http://schemas.openxmlformats.org/officeDocument/2006/relationships/notesMaster", "notesMasters/notesMaster1.xml", (string) null);
    this._presentation.TopRelation.Add(relation.Id, relation);
    this._presentation.NotesList = new Dictionary<string, string>(1)
    {
      {
        relation.Id,
        ""
      }
    };
    this._presentation.AddOverrideContentType("/ppt/notesMasters/notesMaster1.xml", "application/vnd.openxmlformats-officedocument.presentationml.notesMaster+xml");
  }

  private void InitializeShapeForNotesMaster(
    NotesMasterSlide notesMaster,
    int shapeId,
    PlaceholderType placeholderType,
    int left,
    int top,
    int width,
    int height)
  {
    Shape shape = new Shape(ShapeType.Sp, (BaseSlide) notesMaster);
    shape.SetSlideItemType(SlideItemType.Placeholder);
    shape.ShapeId = shapeId;
    Placeholder placeholder = new Placeholder(shape);
    shape.DrawingType = DrawingType.PlaceHolder;
    placeholder.SetType(placeholderType);
    shape.SetPlaceholder(placeholder);
    shape.ShapeFrame.SetAnchor(new bool?(false), new bool?(false), -1, (long) left, (long) top, (long) width, (long) height);
    shape.IsCustomGeometry = false;
    shape.AutoShapeType = AutoShapeType.Rectangle;
    Syncfusion.Presentation.RichText.TextBody textBody = shape.TextBody as Syncfusion.Presentation.RichText.TextBody;
    textBody.SetMargin(91440, 45720, 91440, 45720);
    textBody.SetTextDirection(TextDirection.Horizontal);
    switch (placeholderType)
    {
      case PlaceholderType.Body:
        shape.ShapeName = "Notes Placeholder 4";
        placeholder.AssignSize(PlaceholderSize.Quarter);
        placeholder.SetIndex("3");
        (textBody.Paragraphs.Add("Edit Master text styles") as Paragraph).IndentLevelNumber = 0;
        (textBody.Paragraphs.Add("Second level") as Paragraph).IndentLevelNumber = 1;
        (textBody.Paragraphs.Add("Third level") as Paragraph).IndentLevelNumber = 2;
        (textBody.Paragraphs.Add("Fourth level") as Paragraph).IndentLevelNumber = 3;
        (textBody.Paragraphs.Add("Fifth level") as Paragraph).IndentLevelNumber = 4;
        break;
      case PlaceholderType.SlideNumber:
        shape.ShapeName = "Slide Number Placeholder 6";
        placeholder.AssignSize(PlaceholderSize.Quarter);
        placeholder.SetIndex("5");
        Paragraph paragraph1 = textBody.Paragraphs.Add() as Paragraph;
        paragraph1.HorizontalAlignment = HorizontalAlignmentType.Right;
        TextPart textPart1 = paragraph1.AddTextPart() as TextPart;
        textPart1.UniqueId = $"{{{Guid.NewGuid().ToString()}}}";
        textPart1.Type = "slidenum";
        textPart1.Text = "‹#›";
        textBody.SetVerticalAlign(VerticalAlignment.Bottom);
        break;
      case PlaceholderType.Header:
        shape.ShapeName = "Header Placeholder 1";
        PlaceholderSize placeholderSize = PlaceholderSize.Quarter;
        placeholder.AssignSize(placeholderSize);
        (textBody.AddParagraph() as Paragraph).HorizontalAlignment = HorizontalAlignmentType.Left;
        break;
      case PlaceholderType.Footer:
        shape.ShapeName = "Footer Placeholder 5";
        placeholder.AssignSize(PlaceholderSize.Quarter);
        placeholder.SetIndex("4");
        textBody.SetVerticalAlign(VerticalAlignment.Bottom);
        (textBody.Paragraphs.Add() as Paragraph).HorizontalAlignment = HorizontalAlignmentType.Left;
        break;
      case PlaceholderType.Date:
        shape.ShapeName = "Date Placeholder 2";
        placeholder.SetIndex("1");
        Paragraph paragraph2 = textBody.Paragraphs.Add() as Paragraph;
        paragraph2.HorizontalAlignment = HorizontalAlignmentType.Right;
        TextPart textPart2 = paragraph2.TextParts.Add() as TextPart;
        textPart2.UniqueId = $"{{{Guid.NewGuid().ToString()}}}";
        textPart2.Type = "datetimeFigureOut";
        textPart2.Text = DateTime.Now.ToString();
        break;
      case PlaceholderType.SlideImage:
        shape.ShapeName = "Slide Image Placeholder 3";
        placeholder.SetIndex("2");
        textBody.SetVerticalAlign(VerticalAlignment.Middle);
        LineFormat lineFormat = shape.LineFormat as LineFormat;
        lineFormat.SetWidth(12700);
        lineFormat.Fill.SolidFill.Color = ColorObject.Black;
        break;
    }
    if (placeholderType == PlaceholderType.Header || placeholderType == PlaceholderType.Footer || placeholderType == PlaceholderType.Date || placeholderType == PlaceholderType.SlideNumber)
    {
      Paragraph paragraph3 = new Paragraph((Paragraphs) textBody.Paragraphs);
      paragraph3.IsWithinList = true;
      Syncfusion.Presentation.RichText.Font font = new Syncfusion.Presentation.RichText.Font(paragraph3);
      font.SetFontSize(1200);
      paragraph3.SetFont(font);
      textBody.StyleList.Add("lvl1pPr", paragraph3);
      if (placeholderType == PlaceholderType.Header || placeholderType == PlaceholderType.Footer)
        paragraph3.SetAlignmentType(HorizontalAlignment.Left);
      else
        paragraph3.SetAlignmentType(HorizontalAlignment.Right);
    }
    (notesMaster.Shapes as Syncfusion.Presentation.Drawing.Shapes).Add(shape);
    notesMaster.ColorMap = new Dictionary<string, string>()
    {
      {
        "bg1",
        "lt1"
      },
      {
        "tx1",
        "dk1"
      },
      {
        "bg2",
        "lt2"
      },
      {
        "tx2",
        "dk2"
      },
      {
        "accent1",
        "accent1"
      },
      {
        "accent2",
        "accent2"
      },
      {
        "accent3",
        "accent3"
      },
      {
        "accent4",
        "accent4"
      },
      {
        "accent5",
        "accent5"
      },
      {
        "accent6",
        "accent6"
      },
      {
        "hlink",
        "hlink"
      },
      {
        "folHlink",
        "folHlink"
      }
    };
  }

  private void InitializeNotesMasterRelation(NotesMasterSlide notesMaster)
  {
    notesMaster.TopRelation = new RelationCollection();
    int num = 0;
    if (this._presentation.HandoutList != null)
      num = this._presentation.HandoutList.Count;
    int themeCount = num + 1;
    Relation relation = new Relation("rId1", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/theme", this.GenerateTarget(ref themeCount), (string) null);
    notesMaster.TopRelation.Add(relation.Id, relation);
    this._presentation.AddOverrideContentType($"/ppt/theme/theme{themeCount.ToString()}.xml", "application/vnd.openxmlformats-officedocument.theme+xml");
  }

  private string GenerateTarget(ref int themeCount)
  {
    bool masterThemeIncluded = false;
    return this.GetTargetByThemeCount(ref themeCount, ref masterThemeIncluded);
  }

  private string GetTargetByThemeCount(ref int themeCount, ref bool masterThemeIncluded)
  {
    bool masterThemeIncluded1 = masterThemeIncluded;
    foreach (BaseSlide master in (IEnumerable<IMasterSlide>) this._presentation.Masters)
    {
      foreach (Relation relation in master.TopRelation.GetRelationList())
      {
        if (relation.Target.Equals($"../theme/theme{themeCount.ToString()}.xml"))
        {
          masterThemeIncluded1 = true;
          ++themeCount;
          return this.GetTargetByThemeCount(ref themeCount, ref masterThemeIncluded1);
        }
      }
    }
    if (!masterThemeIncluded1)
      themeCount += this._presentation.MasterList.Count;
    return $"../theme/theme{(++themeCount).ToString()}.xml";
  }

  internal string LayoutTarget => this._layoutTarget;

  internal bool IsPortableRendering => this._isPortableRendering;

  internal string LayoutIndex
  {
    get => this._layoutIndex;
    set => this._layoutIndex = value;
  }

  internal bool ShowMasterShape
  {
    get => this._showMasterShape;
    set => this._showMasterShape = value;
  }

  public ILayoutSlide LayoutSlide
  {
    get
    {
      string layoutIndex = this.ObtainLayoutIndex();
      return layoutIndex != null && this.Presentation.GetSlideLayout().ContainsKey(layoutIndex) ? (ILayoutSlide) this.Presentation.GetSlideLayout()[layoutIndex] : (ILayoutSlide) null;
    }
  }

  public IComments Comments
  {
    get => (IComments) this._commentList ?? (IComments) (this._commentList = new Syncfusion.Presentation.CommentImplementation.Comments(this));
  }

  internal SlideLayoutType LayoutType
  {
    get => this._layoutType;
    set => this._layoutType = value;
  }

  public uint SlideID
  {
    get => Syncfusion.Presentation.Drawing.Helper.ToUInt(this._slideId);
    set => this._slideId = value.ToString();
  }

  public int SlideNumber
  {
    get
    {
      ISlides slides = this.Presentation.Slides;
      for (int index = 0; index < slides.Count; ++index)
      {
        if (slides[index] is Slide slide && this._slideId == slide._slideId)
          return this.Presentation.FirstSlideNumber + index;
      }
      return -1;
    }
  }

  public bool Visible
  {
    get => this._isSlideVisible;
    set => this._isSlideVisible = value;
  }

  internal void Layout()
  {
    this._slideInfo = new SlideInfo((ISlide) this);
    this._slideInfo.Bounds = new RectangleF(0.0f, 0.0f, (float) this.Presentation.SlideSize.Width, (float) this.Presentation.SlideSize.Height);
    foreach (IShape shape in (IEnumerable<ISlideItem>) this.Shapes)
      this.Layout(shape as Shape);
    if (!this.ShowMasterShape)
      return;
    string layoutIndex = this.ObtainLayoutIndex();
    if (layoutIndex == null)
      return;
    Syncfusion.Presentation.SlideImplementation.LayoutSlide layoutSlide = this.Presentation.GetSlideLayout()[layoutIndex];
    if (layoutSlide.ShowMasterShape)
    {
      foreach (IShape shape1 in (IEnumerable<ISlideItem>) layoutSlide.MasterSlide.Shapes)
      {
        if (shape1 is Shape shape2 && shape2.DrawingType != DrawingType.PlaceHolder)
          this.Layout(shape2);
      }
    }
    foreach (IShape shape3 in (IEnumerable<ISlideItem>) layoutSlide.Shapes)
    {
      if (shape3 is Shape shape4 && shape4.DrawingType != DrawingType.PlaceHolder)
        this.Layout(shape4);
    }
  }

  private void LayoutSmartArt(SmartArt smartArt)
  {
    smartArt.InitializeDrawingShapes();
    foreach (KeyValuePair<Guid, SmartArtShape> smartArtShape in smartArt.DataModel.SmartArtShapeCollection)
      smartArtShape.Value.Layout();
  }

  private void LayoutGroupShape(
    Shape groupShape,
    List<RectangleF> groupShapeBounds,
    List<float> groupShapeRotations,
    List<bool?> groupShapeHorzFlips,
    List<bool?> groupShapeVertFlips)
  {
    if (!(groupShape is GroupShape))
      return;
    groupShape.UpdateGroupFrame(groupShapeBounds, groupShapeRotations, groupShapeHorzFlips, groupShapeVertFlips, groupShape);
    foreach (Shape groupedShape in ((GroupShape) groupShape).GetGroupedShapes())
    {
      if (groupedShape.ShapeType == ShapeType.GrpSp)
      {
        this.LayoutGroupShape(groupedShape, groupShapeBounds, groupShapeRotations, groupShapeHorzFlips, groupShapeVertFlips);
      }
      else
      {
        groupedShape.UpdateGroupFrame(groupShapeBounds, groupShapeRotations, groupShapeHorzFlips, groupShapeVertFlips, groupedShape);
        this.Layout(groupedShape);
      }
    }
    if (groupShapeRotations.Count > 0)
      groupShapeRotations.RemoveAt(groupShapeRotations.Count - 1);
    if (groupShapeBounds.Count > 0)
      groupShapeBounds.RemoveAt(groupShapeBounds.Count - 1);
    if (groupShapeHorzFlips.Count > 0)
      groupShapeHorzFlips.RemoveAt(groupShapeHorzFlips.Count - 1);
    if (groupShapeVertFlips.Count <= 0)
      return;
    groupShapeVertFlips.RemoveAt(groupShapeVertFlips.Count - 1);
  }

  private bool IsShapeNeedToBeLayout(AutoShapeType shapeType)
  {
    switch (shapeType)
    {
      case AutoShapeType.Line:
      case AutoShapeType.StraightConnector:
      case AutoShapeType.CurvedConnector:
        return false;
      default:
        return true;
    }
  }

  internal List<Dictionary<string, RectangleF>> UriHyperlinks
  {
    get => this._hyperlinks ?? (this._hyperlinks = new List<Dictionary<string, RectangleF>>());
  }

  internal System.Drawing.Imaging.ImageFormat ChangeImageFormat(Syncfusion.Drawing.ImageFormat imageFormat)
  {
    switch (imageFormat)
    {
      case Syncfusion.Drawing.ImageFormat.Unknown:
        return System.Drawing.Imaging.ImageFormat.Bmp;
      case Syncfusion.Drawing.ImageFormat.Bmp:
        return System.Drawing.Imaging.ImageFormat.Bmp;
      case Syncfusion.Drawing.ImageFormat.Emf:
        return System.Drawing.Imaging.ImageFormat.Bmp;
      case Syncfusion.Drawing.ImageFormat.Gif:
        return System.Drawing.Imaging.ImageFormat.Gif;
      case Syncfusion.Drawing.ImageFormat.Jpeg:
        return System.Drawing.Imaging.ImageFormat.Jpeg;
      case Syncfusion.Drawing.ImageFormat.Png:
        return System.Drawing.Imaging.ImageFormat.Png;
      case Syncfusion.Drawing.ImageFormat.Wmf:
        return System.Drawing.Imaging.ImageFormat.Bmp;
      case Syncfusion.Drawing.ImageFormat.Icon:
        return System.Drawing.Imaging.ImageFormat.Icon;
      case Syncfusion.Drawing.ImageFormat.Exif:
        return System.Drawing.Imaging.ImageFormat.Exif;
      case Syncfusion.Drawing.ImageFormat.MemoryBmp:
        return System.Drawing.Imaging.ImageFormat.MemoryBmp;
      case Syncfusion.Drawing.ImageFormat.Tiff:
        return System.Drawing.Imaging.ImageFormat.Tiff;
      default:
        return System.Drawing.Imaging.ImageFormat.Bmp;
    }
  }

  public System.Drawing.Image ConvertToImage(ImageType imageType)
  {
    this.Presentation.SetExportingSlide((ISlide) this);
    if (this.Presentation.IsPdfConversion == (byte) 0 && !this.Presentation.HasLicense())
      this.Presentation.AddWaterMark((ISlide) this);
    return new Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter().ConvertToImage(this, imageType);
  }

  public Stream ConvertToImage(Syncfusion.Drawing.ImageFormat imageFormat)
  {
    this.Presentation.SetExportingSlide((ISlide) this);
    if (this.Presentation.IsPdfConversion == (byte) 0 && !this.Presentation.HasLicense())
      this.Presentation.AddWaterMark((ISlide) this);
    Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter toImageConverter = new Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter();
    Stream stream = (Stream) new MemoryStream();
    if (Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.IsAzureCompatible)
    {
      System.Drawing.Image image = toImageConverter.ConvertToImage(this, ImageType.Bitmap);
      System.Drawing.Imaging.ImageFormat format = this.ChangeImageFormat(imageFormat);
      image.Save(stream, format);
      image.Dispose();
    }
    else
      stream = toImageConverter.ConvertToImage(this, imageFormat);
    stream.Position = 0L;
    return stream;
  }

  internal string ObtainLayoutIndex()
  {
    string key = (string) null;
    MasterSlides masters = (MasterSlides) this.Presentation.Masters;
    if (masters.Count > 0)
    {
      foreach (MasterSlide masterSlide in masters)
      {
        foreach (Relation relation in this.TopRelation.GetRelationList())
        {
          if (relation.Target.Contains("slideLayout"))
          {
            this._layoutTarget = relation.Target;
            key = masterSlide.TopRelation.GetIdByTarget(this._layoutTarget);
            break;
          }
        }
        if (key != null)
          return this._layoutIndex = masterSlide.LayoutList[key];
      }
    }
    return (string) null;
  }

  internal Syncfusion.Presentation.SlideImplementation.LayoutSlide GetLayoutSlideFromPresentation(
    Syncfusion.Presentation.Presentation presentation)
  {
    MasterSlides masters = (MasterSlides) presentation.Masters;
    if (masters.Count > 0)
    {
      foreach (MasterSlide masterSlide in masters)
      {
        foreach (Relation relation in this.TopRelation.GetRelationList())
        {
          if (relation.Target.Contains("slideLayout"))
          {
            this._layoutTarget = relation.Target;
            masterSlide.TopRelation.GetIdByTarget(this._layoutTarget);
            break;
          }
        }
      }
    }
    return this._layoutIndex != null ? presentation.GetSlideLayout()[this._layoutIndex] : (Syncfusion.Presentation.SlideImplementation.LayoutSlide) null;
  }

  internal SlideInfo SlideInfo => this._slideInfo;

  internal void AddShapesFromLayout(SlideLayoutType ppSlideLayout, Slide slide)
  {
    this._layoutType = ppSlideLayout;
    Syncfusion.Presentation.Drawing.Shapes shapes = (Syncfusion.Presentation.Drawing.Shapes) slide.Shapes;
    switch (ppSlideLayout)
    {
      case SlideLayoutType.Custom:
      case SlideLayoutType.TitleOnly:
        this.AddRelationToSlide(6);
        if (slide.Presentation.Created || this.LayoutSlide == null)
        {
          shapes.AddLayoutTitleOnly();
          break;
        }
        break;
      case SlideLayoutType.Title:
        this.AddRelationToSlide(1);
        if (slide.Presentation.Created || this.LayoutSlide == null)
        {
          shapes.AddLayoutTitle();
          break;
        }
        break;
      case SlideLayoutType.TitleAndContent:
        this.AddRelationToSlide(2);
        if (slide.Presentation.Created || this.LayoutSlide == null)
        {
          shapes.AddLayoutObject();
          break;
        }
        break;
      case SlideLayoutType.SectionHeader:
        this.AddRelationToSlide(3);
        if (slide.Presentation.Created || this.LayoutSlide == null)
        {
          shapes.AddLayoutSectionHeader();
          break;
        }
        break;
      case SlideLayoutType.TwoContent:
        this.AddRelationToSlide(4);
        if (slide.Presentation.Created || this.LayoutSlide == null)
        {
          shapes.AddLayoutTwoObjects();
          break;
        }
        break;
      case SlideLayoutType.Comparison:
        this.AddRelationToSlide(5);
        if (slide.Presentation.Created || this.LayoutSlide == null)
        {
          shapes.AddLayoutComparison();
          break;
        }
        break;
      case SlideLayoutType.Blank:
        this.AddRelationToSlide(7);
        if (slide.Presentation.Created || this.LayoutSlide == null)
        {
          shapes.AddLayoutBlank();
          break;
        }
        break;
      case SlideLayoutType.ContentWithCaption:
        this.AddRelationToSlide(8);
        if (slide.Presentation.Created || this.LayoutSlide == null)
        {
          shapes.AddLayoutContentWithCaption();
          break;
        }
        break;
      case SlideLayoutType.PictureWithCaption:
        this.AddRelationToSlide(9);
        if (slide.Presentation.Created || this.LayoutSlide == null)
        {
          shapes.AddLayoutPictureWithCaption();
          break;
        }
        break;
      case SlideLayoutType.TitleAndVerticalText:
        this.AddRelationToSlide(10);
        if (slide.Presentation.Created || this.LayoutSlide == null)
        {
          shapes.AddLayoutVerticalText();
          break;
        }
        break;
      case SlideLayoutType.VerticalTitleAndText:
        this.AddRelationToSlide(11);
        if (slide.Presentation.Created || this.LayoutSlide == null)
        {
          shapes.AddLayoutVerticalTitleAndText();
          break;
        }
        break;
    }
    if (slide.Presentation.Created || this.LayoutSlide == null)
      return;
    this.AddLayoutSlideShapesToSlide(slide, this.LayoutSlide);
  }

  internal void AddLayoutSlideShapesToSlide(Slide slide, ILayoutSlide layoutSlide)
  {
    foreach (IShape shape in (IEnumerable<ISlideItem>) layoutSlide.Shapes)
    {
      if (shape.PlaceholderFormat != null)
        slide.AddShapesFromPlaceholderType(shape, layoutSlide);
    }
  }

  internal void AddShapesFromPlaceholderType(IShape shape, ILayoutSlide layout)
  {
    Syncfusion.Presentation.Drawing.Shapes shapes = (Syncfusion.Presentation.Drawing.Shapes) this.Shapes;
    if (shape.PlaceholderFormat.Type != PlaceholderType.Date && shape.PlaceholderFormat.Type != PlaceholderType.Footer && shape.PlaceholderFormat.Type != PlaceholderType.SlideNumber)
    {
      shapes.AddPlaceholder((shape.PlaceholderFormat as Placeholder).Type, (shape.PlaceholderFormat as Placeholder).Size, Orientation.None, (int) (shape.PlaceholderFormat as Placeholder).Index);
    }
    else
    {
      if (layout.LayoutType == SlideLayoutType.Title && !this.Presentation.ShowSpecialPlsOnTitleSld)
        return;
      Dictionary<string, bool> dictionary = (Dictionary<string, bool>) null;
      if ((layout as Syncfusion.Presentation.SlideImplementation.LayoutSlide).HeaderFooter != null)
        dictionary = (layout as Syncfusion.Presentation.SlideImplementation.LayoutSlide).HeaderFooter;
      else if ((layout.MasterSlide as MasterSlide).HeaderFooter != null)
        dictionary = (layout.MasterSlide as MasterSlide).HeaderFooter;
      if (dictionary == null)
        return;
      string key = "";
      switch (shape.PlaceholderFormat.Type)
      {
        case PlaceholderType.SlideNumber:
          key = "sldNum";
          break;
        case PlaceholderType.Footer:
          key = "ftr";
          break;
        case PlaceholderType.Date:
          key = "dt";
          break;
      }
      if (dictionary.ContainsKey(key))
      {
        bool flag;
        dictionary.TryGetValue(key, out flag);
        if (!flag)
          return;
        shapes.AddPlaceholder(shape, (shape.PlaceholderFormat as Placeholder).Type, (shape.PlaceholderFormat as Placeholder).Size, Orientation.None, (int) (shape.PlaceholderFormat as Placeholder).Index);
      }
      else
        shapes.AddPlaceholder(shape, (shape.PlaceholderFormat as Placeholder).Type, (shape.PlaceholderFormat as Placeholder).Size, Orientation.None, (int) (shape.PlaceholderFormat as Placeholder).Index);
    }
  }

  internal void Layout(Shape shape)
  {
    switch (shape.ShapeType)
    {
      case ShapeType.Sp:
      case ShapeType.GraphicFrame:
      case ShapeType.CxnSp:
        if (!this.IsShapeNeedToBeLayout(shape.AutoShapeType))
          break;
        shape.Layout();
        if (!(shape is SmartArt))
          break;
        this.LayoutSmartArt((SmartArt) shape);
        break;
      case ShapeType.GrpSp:
        List<RectangleF> groupShapeBounds = new List<RectangleF>();
        List<float> groupShapeRotations = new List<float>();
        List<bool?> groupShapeHorzFlips = new List<bool?>();
        List<bool?> groupShapeVertFlips = new List<bool?>();
        this.LayoutGroupShape(shape, groupShapeBounds, groupShapeRotations, groupShapeHorzFlips, groupShapeVertFlips);
        groupShapeBounds.Clear();
        groupShapeRotations.Clear();
        groupShapeHorzFlips.Clear();
        groupShapeVertFlips.Clear();
        break;
    }
  }

  internal void AddRelationToSlide(int num)
  {
    if (this.TopRelation != null)
      return;
    Relation relation = new Relation("rId1", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/slideLayout", $"../slideLayouts/slideLayout{(object) num}.xml", (string) null);
    this.TopRelation = new RelationCollection();
    this.TopRelation.Add(relation.Id, relation);
  }

  internal override void Close()
  {
    base.Close();
    this._presentation = (Syncfusion.Presentation.Presentation) null;
    if (this._slideInfo == null)
      return;
    this._slideInfo = (SlideInfo) null;
  }

  public ISlide Clone()
  {
    Slide slide = (Slide) this.MemberwiseClone();
    slide.IsCloned = true;
    if (this._slideInfo != null)
    {
      slide._slideInfo = this._slideInfo.Clone();
      slide._slideInfo.SetParent(slide);
    }
    slide._hasNotes = this._hasNotes;
    if (this._notesSlide != null)
    {
      slide._notesSlide = this._notesSlide.Clone();
      slide._notesSlide.SetParent(slide);
    }
    slide._hasComment = this._hasComment;
    if (slide._commentList != null)
    {
      slide._commentList = this._commentList.CloneComments();
      slide._commentList.SetParent(slide);
    }
    this.Clone((BaseSlide) slide);
    return (ISlide) slide;
  }

  internal override void SetParent(Syncfusion.Presentation.Presentation presentation)
  {
    this._presentation = presentation;
    base.SetParent(presentation);
  }

  internal bool IsCloned
  {
    get => this._isCloned;
    set => this._isCloned = value;
  }

  internal bool HasNotes
  {
    get
    {
      if (!this._hasNotes.HasValue)
      {
        foreach (Relation relation in this.TopRelation.GetRelationList())
        {
          if (relation.Type.Contains("notesSlide"))
          {
            this._notesRelationId = relation.Id;
            this._hasNotes = new bool?(true);
          }
        }
      }
      return this._hasNotes.HasValue && this._hasNotes.Value;
    }
  }

  internal bool HasComment
  {
    get
    {
      if (!this._hasComment.HasValue)
      {
        foreach (Relation relation in this.TopRelation.GetRelationList())
        {
          if (relation.Type.Contains("comments"))
          {
            this._commentRelationId = relation.Id;
            this._hasComment = new bool?(true);
          }
        }
      }
      return this._hasComment.HasValue && this._hasComment.Value;
    }
  }

  internal string NotesRelationId => this._notesRelationId;

  internal string CommentRelationId
  {
    get => this._commentRelationId;
    set => this._commentRelationId = value;
  }

  internal void SetNotesSlide(Syncfusion.Presentation.SlideImplementation.NotesSlide notesSlide)
  {
    this._notesSlide = notesSlide;
  }

  public void RemoveNotesSlide()
  {
    if (this._notesSlide == null)
      return;
    string partName = this.TopRelation.RemoveRelationByTypeContains("notesSlide");
    if (partName != null)
      this._presentation.RemoveOverrideContentType(partName);
    this._hasNotes = new bool?(false);
    this._notesSlide.Close();
    this._notesSlide = (Syncfusion.Presentation.SlideImplementation.NotesSlide) null;
    this.Presentation.BuiltInDocumentProperties.NoteCount = --this.Presentation.NotesSlideCount;
  }

  public INotesSlide NotesSlide => (INotesSlide) this._notesSlide;

  public void MoveToSection(int sectionIndex)
  {
    if (this._sectionId == null)
      this._sectionId = ((Syncfusion.Presentation.SlideImplementation.Section) this._presentation.Sections[sectionIndex]).ID;
    Syncfusion.Presentation.SlideImplementation.Section section = (Syncfusion.Presentation.SlideImplementation.Section) this.Section;
    if (section != null && section.SlideIdList.Contains(this._slideId.ToString()))
      section.RemoveSlide(this);
    ((Syncfusion.Presentation.SlideImplementation.Section) this._presentation.Sections[sectionIndex]).AddSlide(this);
  }

  public ISection Section
  {
    get => ((Sections) this._presentation.Sections).GetSectionByID(this._sectionId);
  }

  internal string SectionId
  {
    get => this._sectionId;
    set => this._sectionId = value;
  }
}
