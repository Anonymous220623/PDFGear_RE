// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Drawing.Shapes
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Compression.Zip;
using Syncfusion.Presentation.Charts;
using Syncfusion.Presentation.RichText;
using Syncfusion.Presentation.SlideImplementation;
using Syncfusion.Presentation.SmartArtImplementation;
using Syncfusion.Presentation.TableImplementation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Xml;

#nullable disable
namespace Syncfusion.Presentation.Drawing;

internal class Shapes : IShapes, IEnumerable<ISlideItem>, IEnumerable
{
  private List<ISlideItem> _list;
  private BaseSlide _baseSlide;
  private GroupShape _groupShape;

  internal Shapes(BaseSlide baseSlide)
  {
    this._baseSlide = baseSlide;
    this._list = new List<ISlideItem>();
  }

  internal Shapes(GroupShape groupShape, BaseSlide baseSlide)
  {
    this._groupShape = groupShape;
    this._baseSlide = baseSlide;
    this._list = new List<ISlideItem>();
  }

  internal BaseSlide BaseSlide => this._baseSlide;

  internal List<PlaceholderType> GetPlaceholderTypes()
  {
    List<PlaceholderType> placeholderTypes = new List<PlaceholderType>();
    foreach (IShape shape in this._list)
    {
      if (shape.PlaceholderFormat != null)
      {
        PlaceholderType type = shape.PlaceholderFormat.Type;
        switch (type)
        {
          case PlaceholderType.SlideNumber:
          case PlaceholderType.Footer:
          case PlaceholderType.Date:
            continue;
          default:
            placeholderTypes.Add(type);
            continue;
        }
      }
    }
    return placeholderTypes;
  }

  internal GroupShape AddGroup(string groupName, Shape[] groupItems)
  {
    GroupShape groupShape = new GroupShape(groupName, this._baseSlide);
    groupShape.SetSlideItemType(SlideItemType.GroupShape);
    this.Add((Shape) groupShape);
    foreach (Shape groupItem in groupItems)
      groupShape.Add(groupItem);
    return groupShape;
  }

  internal void Add(Shape shape)
  {
    if (shape.ShapeId == 0)
    {
      ++this.BaseSlide.LastShapeId;
      shape.ShapeId = (int) this.BaseSlide.LastShapeId;
    }
    else if ((int) this.BaseSlide.LastShapeId < (int) (ushort) shape.ShapeId)
      this.BaseSlide.LastShapeId = (ushort) shape.ShapeId;
    this._list.Add((ISlideItem) shape);
  }

  internal void AddLayoutBlank()
  {
  }

  internal void AddLayoutComparison()
  {
    this.AddPlaceholder(PlaceholderType.Title, PlaceholderSize.None, Orientation.None, 0);
    this.AddPlaceholder(PlaceholderType.Body, PlaceholderSize.None, Orientation.None, 1);
    this.AddPlaceholder(PlaceholderType.Body, PlaceholderSize.Half, Orientation.None, 2);
    this.AddPlaceholder(PlaceholderType.Body, PlaceholderSize.Quarter, Orientation.None, 3);
    this.AddPlaceholder(PlaceholderType.Body, PlaceholderSize.Quarter, Orientation.None, 4);
  }

  internal void AddLayoutContentWithCaption()
  {
    this.AddPlaceholder(PlaceholderType.Title, PlaceholderSize.None, Orientation.None, 0);
    this.AddPlaceholder(PlaceholderType.Body, PlaceholderSize.None, Orientation.None, 1);
    this.AddPlaceholder(PlaceholderType.Body, PlaceholderSize.Half, Orientation.None, 2);
  }

  internal void AddLayoutObject()
  {
    this.AddPlaceholder(PlaceholderType.Title, PlaceholderSize.None, Orientation.None, 0);
    this.AddPlaceholder(PlaceholderType.Body, PlaceholderSize.Half, Orientation.None, 1);
  }

  internal void AddLayoutPictureWithCaption()
  {
    this.AddPlaceholder(PlaceholderType.Title, PlaceholderSize.None, Orientation.None, 0);
    this.AddPlaceholder(PlaceholderType.Picture, PlaceholderSize.None, Orientation.None, 1);
    this.AddPlaceholder(PlaceholderType.Body, PlaceholderSize.Half, Orientation.None, 2);
  }

  internal void AddLayoutSectionHeader()
  {
    this.AddPlaceholder(PlaceholderType.Title, PlaceholderSize.None, Orientation.None, 0);
    this.AddPlaceholder(PlaceholderType.Body, PlaceholderSize.None, Orientation.None, 1);
  }

  internal void AddLayoutTitle()
  {
    this.AddPlaceholder(PlaceholderType.CenterTitle, PlaceholderSize.None, Orientation.None, 0);
    this.AddPlaceholder(PlaceholderType.Subtitle, PlaceholderSize.None, Orientation.None, 1);
  }

  internal void AddLayoutTitleOnly()
  {
    this.AddPlaceholder(PlaceholderType.Title, PlaceholderSize.None, Orientation.None, 0);
  }

  internal void AddLayoutTwoObjects()
  {
    this.AddPlaceholder(PlaceholderType.Title, PlaceholderSize.None, Orientation.None, 0);
    this.AddPlaceholder(PlaceholderType.Body, PlaceholderSize.Half, Orientation.None, 1);
    this.AddPlaceholder(PlaceholderType.Body, PlaceholderSize.Half, Orientation.None, 2);
  }

  internal void AddLayoutVerticalText()
  {
    this.AddPlaceholder(PlaceholderType.Title, PlaceholderSize.None, Orientation.None, 0);
    this.AddPlaceholder(PlaceholderType.Body, PlaceholderSize.None, Orientation.Vertical, 1);
  }

  internal void AddLayoutVerticalTitleAndText()
  {
    this.AddPlaceholder(PlaceholderType.Title, PlaceholderSize.None, Orientation.Vertical, 0);
    this.AddPlaceholder(PlaceholderType.Body, PlaceholderSize.None, Orientation.Vertical, 1);
  }

  internal void AddGroupShape(GroupShape groupShape, Shape[] shapes)
  {
    foreach (Shape shape in shapes)
    {
      shape.Group = groupShape;
      groupShape.Add(shape);
    }
  }

  internal void RemoveGroup(Shape shape)
  {
    if (shape.IsGroup)
    {
      Shape[] groupedShapes = ((GroupShape) shape).GetGroupedShapes();
      if (groupedShapes != null && groupedShapes.Length != 0)
      {
        foreach (Shape shape1 in groupedShapes)
          this.RemoveGroup(shape1);
      }
    }
    this._list.Remove((ISlideItem) shape);
  }

  public int Count => this._list.Count;

  public ISlideItem this[int index] => this._list[index];

  public IEnumerator<ISlideItem> GetEnumerator()
  {
    return (IEnumerator<ISlideItem>) this._list.GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._list.GetEnumerator();

  public int Add(ISlideItem shape)
  {
    if ((shape as Shape).BaseSlide == null)
      this.SetParentToClonedSlideItem(shape, this.BaseSlide);
    this._list.Add(shape);
    return this._list.IndexOf(shape);
  }

  public void Insert(int index, ISlideItem value)
  {
    if ((value as Shape).BaseSlide == null)
      this.SetParentToClonedSlideItem(value, this.BaseSlide);
    this._list.Insert(index, value);
  }

  public void RemoveAt(int index)
  {
    ISlideItem slideItem = this._list[index];
    if (slideItem.SlideItemType == SlideItemType.OleObject)
    {
      OleObject oleObject = slideItem as OleObject;
      oleObject.BaseSlide.TopRelation.RemoveRelation(oleObject.RelationId);
      this._baseSlide.TopRelation.RemoveRelationByKeword("vmlDrawing");
    }
    else if (slideItem.SlideItemType == SlideItemType.Picture)
    {
      Picture picture = slideItem as Picture;
      IList imageRemoveList = (IList) picture.BaseSlide.TopRelation.GetImageRemoveList();
      if (imageRemoveList.Contains((object) picture.EmbedId))
        imageRemoveList.Remove((object) picture.EmbedId);
    }
    if (slideItem is Shape && (slideItem as Shape).BaseSlide != null)
      (slideItem as Shape).BaseSlide.RemoveAnimationEffectsOfShape(slideItem as Shape);
    this._list.RemoveAt(index);
  }

  public void Remove(ISlideItem item)
  {
    if (item.SlideItemType == SlideItemType.OleObject)
    {
      OleObject oleObject = item as OleObject;
      oleObject.BaseSlide.TopRelation.RemoveRelation(oleObject.RelationId);
      this._baseSlide.TopRelation.RemoveRelationByKeword("vmlDrawing");
    }
    else if (item.SlideItemType == SlideItemType.Picture)
    {
      Picture picture = item as Picture;
      IList imageRemoveList = (IList) picture.BaseSlide.TopRelation.GetImageRemoveList();
      if (imageRemoveList.Contains((object) picture.EmbedId))
        imageRemoveList.Remove((object) picture.EmbedId);
    }
    if (item is Shape && (item as Shape).BaseSlide != null)
      (item as Shape).BaseSlide.RemoveAnimationEffectsOfShape(item as Shape);
    this._list.Remove(item);
  }

  public int IndexOf(ISlideItem value) => this._list.IndexOf(value);

  public void Clear()
  {
    if (this.BaseSlide != null)
    {
      if (this.BaseSlide.Timeline.MainSequence.Count > 0)
        this.BaseSlide.Timeline.MainSequence.Clear();
      if (this.BaseSlide.Timeline.InteractiveSequences.Count > 0)
        this.BaseSlide.Timeline.InteractiveSequences.Clear();
    }
    this.ClearShapesList();
  }

  public IPresentationChart AddChart(double left, double top, double width, double height)
  {
    PresentationChart shape = new PresentationChart(this._baseSlide);
    shape.SetSlideItemType(SlideItemType.Chart);
    this._baseSlide.Shapes.Add((ISlideItem) shape);
    shape.XPos = left;
    shape.YPos = top;
    shape.Width = width;
    shape.Height = height;
    return (IPresentationChart) shape;
  }

  public IPresentationChart AddChart(
    Stream excelStream,
    int sheetNumber,
    string dataRange,
    RectangleF bounds)
  {
    this.DetectExcelFileFromStream(excelStream);
    PresentationChart shape = new PresentationChart(this._baseSlide);
    shape.SetSlideItemType(SlideItemType.Chart);
    this._baseSlide.Presentation.DataHolder.ParseExcelStream(excelStream, shape.Workbook);
    this._baseSlide.Shapes.Add((ISlideItem) shape);
    shape.XPos = (double) bounds.Left;
    shape.YPos = (double) bounds.Top;
    shape.Width = (double) bounds.Width;
    shape.Height = (double) bounds.Height;
    shape.SetDataRange(sheetNumber, dataRange);
    return (IPresentationChart) shape;
  }

  private void DetectExcelFileFromStream(Stream stream)
  {
    long num = stream != null ? stream.Position : throw new ArgumentNullException("Excel stream should not be null");
    if (ZipArchive.ReadInt32(stream) != 67324752)
      throw new ArgumentException("Excel stream should be .xlsx format");
    stream.Position = num;
  }

  public IPresentationChart AddChart(
    object[][] data,
    double left,
    double top,
    double width,
    double height)
  {
    PresentationChart shape = new PresentationChart(this._baseSlide);
    shape.SetSlideItemType(SlideItemType.Chart);
    shape.SetChartData(data);
    this._baseSlide.Shapes.Add((ISlideItem) shape);
    shape.XPos = left;
    shape.YPos = top;
    shape.Width = width;
    shape.Height = height;
    return (IPresentationChart) shape;
  }

  public IPresentationChart AddChart(
    IEnumerable enumerable,
    double left,
    double top,
    double width,
    double height)
  {
    PresentationChart shape = new PresentationChart(this._baseSlide);
    shape.SetSlideItemType(SlideItemType.Chart);
    shape.SetDataRange(enumerable, 1, 1);
    this._baseSlide.Shapes.Add((ISlideItem) shape);
    shape.XPos = left;
    shape.YPos = top;
    shape.Width = width;
    shape.Height = height;
    return (IPresentationChart) shape;
  }

  public IShape AddTextBox(double left, double top, double width, double height)
  {
    Shape shape = new Shape(ShapeType.Sp, this._baseSlide);
    shape.SetSlideItemType(SlideItemType.AutoShape);
    shape.DrawingType = DrawingType.TextBox;
    shape.IsPresetGeometry = true;
    shape.ShapeFrame.Height = height;
    ((TextBody) shape.TextBody).AutoFitType = AutoMarginType.TextShapeAutoFit;
    shape.ShapeFrame.Width = width;
    shape.ShapeFrame.Top = top;
    shape.ShapeFrame.Left = left;
    this.Add(shape);
    shape.ShapeName = shape.DrawingType.ToString() + (object) shape.ShapeId;
    shape.Group = this._groupShape;
    return (IShape) shape;
  }

  public ISmartArt AddSmartArt(
    SmartArtType smartArtType,
    double left,
    double top,
    double width,
    double height)
  {
    SmartArt smartArt = new SmartArt(this._baseSlide);
    smartArt.SetSlideItemType(SlideItemType.SmartArt);
    smartArt.ShapeFrame.Left = left;
    smartArt.ShapeFrame.Top = top;
    smartArt.ShapeFrame.Width = width;
    smartArt.ShapeFrame.Height = height;
    smartArt.Group = this._groupShape;
    smartArt.ShapeName = smartArtType.ToString() + (object) smartArt.ShapeId;
    smartArt.CreatedSmartArt = true;
    smartArt.SetSmartArtType(smartArtType);
    (smartArt.Nodes as SmartArtNodes).AddSmartArtRelation();
    this.Add((Shape) smartArt);
    return (ISmartArt) smartArt;
  }

  public IShape AddShape(
    AutoShapeType type,
    double left,
    double top,
    double width,
    double height)
  {
    Shape shape = new Shape(ShapeType.Sp, this._baseSlide);
    shape.SetSlideItemType(SlideItemType.AutoShape);
    shape.AutoShapeType = type;
    shape.DrawingType = DrawingType.None;
    shape.ShapeFrame.Height = height;
    shape.ShapeFrame.Width = width;
    shape.ShapeFrame.Top = top;
    shape.Group = this._groupShape;
    if (shape.Group != null && (shape.Group.Fill.FillType != FillType.Automatic || shape.Group.IsGroupFill))
      shape.IsGroupFill = true;
    shape.AddDefaultTextStylestream();
    shape.ShapeFrame.Left = left;
    shape.IsPresetGeometry = true;
    this.AddPreservedElements(shape);
    this.Add(shape);
    shape.ShapeName = type.ToString() + (object) shape.ShapeId;
    return (IShape) shape;
  }

  public IConnector AddConnector(
    ConnectorType connectorType,
    IShape beginShape,
    int beginSiteIndex,
    IShape endShape,
    int endSiteIndex)
  {
    IConnector connector = this.AddConnector(connectorType, 0.0, 0.0, 100.0, 100.0);
    connector.BeginConnect(beginShape, beginSiteIndex);
    connector.EndConnect(endShape, endSiteIndex);
    return connector;
  }

  public IConnector AddConnector(
    ConnectorType connectorType,
    double beginX,
    double beginY,
    double endX,
    double endY)
  {
    if (beginX > 169056.0 || beginX < -169056.0 || beginY > 169056.0 || beginY < -169056.0 || endX > 169056.0 || endX < -169056.0 || endY > 169056.0 || endY < -169056.0)
      throw new ArgumentException("Connecor only support the values in between 169056 to -169055");
    Connector connector = new Connector(ShapeType.CxnSp, this._baseSlide);
    connector.SetSlideItemType(SlideItemType.ConnectionShape);
    connector.SetConnectorType(connectorType);
    connector.AutoShapeType = connector.GetConnectorTypeWithShapeType(connectorType);
    connector.DrawingType = DrawingType.None;
    connector.UpdateConnectorBounds(connector, (float) beginX, (float) beginY, (float) endX, (float) endY);
    connector.Group = this._groupShape;
    if (connector.Group != null && (connector.Group.Fill.FillType != FillType.Automatic || connector.Group.IsGroupFill))
      connector.IsGroupFill = true;
    connector.IsPresetGeometry = true;
    this.AddPreservedElements((Shape) connector);
    this.Add((Shape) connector);
    connector.ShapeName = connectorType.ToString() + (object) connector.ShapeId;
    return (IConnector) connector;
  }

  public IGroupShape AddGroupShape(double left, double top, double width, double height)
  {
    GroupShape groupShape = new GroupShape("", this._baseSlide);
    groupShape.SetSlideItemType(SlideItemType.GroupShape);
    groupShape.Height = height;
    groupShape.Width = width;
    groupShape.Top = top;
    groupShape.Left = left;
    groupShape.ShapeFrame.ChildHeight = height;
    groupShape.ShapeFrame.ChildLeft = left;
    groupShape.ShapeFrame.ChildTop = top;
    groupShape.ShapeFrame.ChildWidth = width;
    groupShape.IsPresetGeometry = true;
    this.Add((Shape) groupShape);
    groupShape.Group = this._groupShape;
    if (this._groupShape != null && groupShape.Group.Fill.FillType != FillType.Automatic)
      groupShape.IsGroupFill = true;
    groupShape.ShapeName = "Group" + (object) groupShape.ShapeId;
    return (IGroupShape) groupShape;
  }

  public IPicture AddPicture(Stream stream, double left, double top, double width, double height)
  {
    Picture picture = new Picture(this._baseSlide);
    picture.SetSlideItemType(SlideItemType.Picture);
    picture.IsPresetGeometry = true;
    picture.DrawingType = DrawingType.None;
    picture.AutoShapeType = AutoShapeType.Rectangle;
    picture.ShapeFrame.Height = height;
    picture.ShapeFrame.Width = width;
    picture.ShapeFrame.Top = top;
    picture.ShapeFrame.Left = left;
    picture.AddImage(stream);
    this.Add((Shape) picture);
    picture.Group = this._groupShape;
    picture.ShapeName = "Picture " + (object) picture.ShapeId;
    return (IPicture) picture;
  }

  public IPicture AddPicture(
    Stream svgStream,
    Stream imageStream,
    double left,
    double top,
    double width,
    double height)
  {
    Picture picture = new Picture(this._baseSlide);
    picture.SetSlideItemType(SlideItemType.Picture);
    picture.IsPresetGeometry = true;
    picture.DrawingType = DrawingType.None;
    picture.AutoShapeType = AutoShapeType.Rectangle;
    picture.ShapeFrame.Height = height;
    picture.ShapeFrame.Width = width;
    picture.ShapeFrame.Top = top;
    picture.ShapeFrame.Left = left;
    picture.AddImage(svgStream, imageStream);
    this.Add((Shape) picture);
    picture.Group = this._groupShape;
    picture.ShapeName = "Picture " + (object) picture.ShapeId;
    return (IPicture) picture;
  }

  public ITable AddTable(
    int rowCount,
    int columnCount,
    double left,
    double top,
    double width,
    double height)
  {
    Table table = new Table(this._baseSlide);
    table.SetSlideItemType(SlideItemType.Table);
    table.Height = height;
    table.Width = width;
    table.Left = left;
    table.Top = top;
    table.HasBandedRows = true;
    table.HasHeaderRow = true;
    table.Id = "{5C22544A-7EE6-4342-B048-85BDC9FD1C3A}";
    if (rowCount > 75 || columnCount > 75 || rowCount < 1 || columnCount < 1)
      throw new ArgumentException("Invalid RowCount/ColumnCount");
    if (rowCount != 0)
    {
      for (int index = 0; index < rowCount; ++index)
        table.AddRow(rowCount, columnCount);
    }
    if (columnCount != 0)
    {
      for (int columnIndex = 0; columnIndex < columnCount; ++columnIndex)
        table.AddColumn(columnCount, columnIndex);
    }
    this.Add((Shape) table);
    table.BuiltInStyle = BuiltInTableStyle.MediumStyle2Accent1;
    table.DrawingType = DrawingType.Table;
    table.ShapeName = "Table " + (object) table.ShapeId;
    return (ITable) table;
  }

  public IOleObject AddOleObject(Stream pictureStream, string oleName, Stream oleStream)
  {
    OleObject oleObject = new OleObject(this._baseSlide);
    this.AddOleObject(oleObject, pictureStream, oleName, oleStream, (string) null);
    return (IOleObject) oleObject;
  }

  public IOleObject AddOleObject(Stream pictureStream, string oleName, string pathLink)
  {
    OleObject oleObject = new OleObject(this._baseSlide);
    this.AddOleObject(oleObject, pictureStream, oleName, (Stream) null, pathLink);
    return (IOleObject) oleObject;
  }

  internal IPlaceholderFormat AddPlaceholder(
    PlaceholderType placeHolderType,
    double left,
    double top,
    double width,
    double height)
  {
    Placeholder placeholder = (Placeholder) this.AddPlaceholder(placeHolderType, PlaceholderSize.None, Orientation.None, 0);
    Shape baseShape = placeholder.GetBaseShape();
    baseShape.Height = height;
    baseShape.Width = width;
    baseShape.Left = left;
    baseShape.Top = top;
    baseShape.Group = this._groupShape;
    return (IPlaceholderFormat) placeholder;
  }

  internal void AddOleObject(
    OleObject oleObject,
    Stream pictureStream,
    string oleName,
    Stream oleStream,
    string pathLink)
  {
    Stream stream = (Stream) null;
    if (oleStream == null)
    {
      oleObject.SetLinkType(OleLinkType.Link);
      oleObject.SetLinkPath(pathLink);
    }
    else
    {
      stream = (Stream) new MemoryStream();
      OleObject.CopyStream(oleStream, stream);
      stream.Position = 0L;
      oleObject.SetLinkType(OleLinkType.Embed);
    }
    oleObject.GetOlePicture(pictureStream);
    oleObject.SetSlideItemType(SlideItemType.OleObject);
    oleObject.DrawingType = DrawingType.OleObject;
    ++this._baseSlide.Presentation.OleObjectCount;
    oleObject.RelationId = Helper.GenerateRelationId(this._baseSlide.TopRelation);
    oleObject.OleObjectType = Helper.ToOleType(oleName);
    string type = "http://schemas.openxmlformats.org/officeDocument/2006/relationships/package";
    string str;
    switch (oleObject.OleObjectType)
    {
      case OleObjectType.ExcelWorksheet:
        this._baseSlide.Presentation.AddDefaultContentType("xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        str = ".xlsx";
        break;
      case OleObjectType.PowerPointPresentation:
        this._baseSlide.Presentation.AddDefaultContentType("pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation");
        str = ".pptx";
        break;
      case OleObjectType.WordDocument:
        this._baseSlide.Presentation.AddDefaultContentType("docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
        str = ".docx";
        break;
      default:
        this._baseSlide.Presentation.AddDefaultContentType("bin", "application/vnd.openxmlformats-officedocument.oleObject");
        str = ".bin";
        stream = oleObject.CreateBinaryOLEStream(stream, oleName);
        type = "http://schemas.openxmlformats.org/officeDocument/2006/relationships/oleObject";
        break;
    }
    oleObject.OleExtension = str;
    if (oleObject.LinkType == OleLinkType.Embed)
    {
      Relation relation = new Relation(oleObject.RelationId, type, $"../embeddings/oleObject{this._baseSlide.Presentation.OleObjectCount.ToString()}{oleObject.OleExtension}", (string) null);
      this._baseSlide.TopRelation.Add(oleObject.RelationId, relation);
      oleObject.OleStream = stream;
      oleObject.SetFileName(Helper.GetFileName(relation.Target));
    }
    else
    {
      Relation relation = new Relation(oleObject.RelationId, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/oleObject", pathLink, "External");
      this._baseSlide.TopRelation.Add(oleObject.RelationId, relation);
    }
    oleObject.ProgID = oleName;
    this.Add((Shape) oleObject);
    oleObject.VmlShape = new VmlShape()
    {
      VmlShapeID = "_x0000_s1" + this._baseSlide.Presentation.OleObjectCount.ToString(),
      ShapeType = "#_x0000_t75",
      ImageRelationId = "rId" + this._baseSlide.Presentation.OleObjectCount.ToString()
    };
    if (!this._baseSlide.HasOLEObject)
    {
      int slideNumber = (this._baseSlide as Slide).SlideNumber;
      string relationId = Helper.GenerateRelationId(this._baseSlide.TopRelation);
      Relation relation = new Relation(relationId, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/vmlDrawing", $"../drawings/vmlDrawing{(object) slideNumber}.vml", (string) null);
      this._baseSlide.TopRelation.Add(relationId, relation);
    }
    oleObject.ShapeName = "Object " + (object) oleObject.ShapeId;
    if (this._baseSlide.Presentation.DefaultContentType.ContainsKey("vml"))
      return;
    this._baseSlide.Presentation.DefaultContentType.Add("vml", "application/vnd.openxmlformats-officedocument.vmlDrawing");
  }

  internal GroupShape GetGroup() => this._groupShape;

  internal IPlaceholderFormat AddPlaceholder(
    IShape srcShape,
    PlaceholderType placeHolderType,
    PlaceholderSize placeHolderSize,
    Orientation placeHolderDirection,
    int index)
  {
    Shape shape = new Shape(ShapeType.Sp, this._baseSlide);
    Placeholder placeholder = new Placeholder(shape);
    shape.SetSlideItemType(SlideItemType.Placeholder);
    shape.DrawingType = DrawingType.PlaceHolder;
    shape.SetTextBody(srcShape.TextBody as TextBody);
    this.Add(shape);
    string str = Helper.GetName(placeHolderType);
    if (placeHolderDirection == Orientation.Vertical)
      str = "Vertical Text Placeholder ";
    shape.ShapeName = str + (object) (shape.ShapeId - 1);
    shape.SetPlaceholder(placeholder);
    placeholder.SetPlaceholderValues(placeHolderType, placeHolderSize, placeHolderDirection, index.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    return shape.PlaceholderFormat;
  }

  internal IPlaceholderFormat AddPlaceholder(
    PlaceholderType placeHolderType,
    PlaceholderSize placeHolderSize,
    Orientation placeHolderDirection,
    int index)
  {
    Shape shape = new Shape(ShapeType.Sp, this._baseSlide);
    Placeholder placeholder = new Placeholder(shape);
    shape.SetSlideItemType(SlideItemType.Placeholder);
    shape.DrawingType = DrawingType.PlaceHolder;
    this.Add(shape);
    string str = Helper.GetName(placeHolderType);
    if (placeHolderDirection == Orientation.Vertical)
      str = "Vertical Text Placeholder ";
    shape.ShapeName = str + (object) (shape.ShapeId - 1);
    shape.SetPlaceholder(placeholder);
    placeholder.SetPlaceholderValues(placeHolderType, placeHolderSize, placeHolderDirection, index.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    return shape.PlaceholderFormat;
  }

  internal void AddPreservedElements(Shape shape)
  {
    MemoryStream output = new MemoryStream();
    using (XmlWriter xmlTextWriter = XmlWriter.Create((Stream) output))
    {
      xmlTextWriter.WriteStartDocument();
      xmlTextWriter.WriteStartElement("sld");
      xmlTextWriter.WriteAttributeString("xmlns", "a", (string) null, "http://schemas.openxmlformats.org/drawingml/2006/main");
      xmlTextWriter.WriteAttributeString("xmlns", "p", (string) null, "http://schemas.openxmlformats.org/presentationml/2006/main");
      xmlTextWriter.WriteStartElement("p", "style", "http://schemas.openxmlformats.org/presentationml/2006/main");
      DrawingSerializator.LnRef(xmlTextWriter, (IShape) shape);
      DrawingSerializator.FillRef(xmlTextWriter, (IShape) shape);
      DrawingSerializator.EffectRef(xmlTextWriter);
      DrawingSerializator.FontRef(xmlTextWriter);
      xmlTextWriter.WriteEndElement();
      xmlTextWriter.WriteEndElement();
      xmlTextWriter.WriteEndDocument();
      xmlTextWriter.Flush();
    }
    output.Position = 0L;
    shape.PreservedElements.Add("style", (Stream) output);
  }

  internal void ClearShapesList() => this._list.Clear();

  internal void SetParent(Syncfusion.Presentation.Presentation presentation)
  {
    foreach (ISlideItem slideItem in this._list)
    {
      ((Shape) slideItem).SetParent(presentation);
      Shape shape = (Shape) slideItem;
      if (shape.ShapeType == ShapeType.GrpSp)
        ((Shapes) ((GroupShape) shape).Shapes).SetParent(presentation);
    }
  }

  internal void AddNotesShapes()
  {
    this.AddPlaceholder(PlaceholderType.SlideImage, PlaceholderSize.None, Orientation.None, 0);
    (this._baseSlide as NotesSlide).SetTextBody((this.AddPlaceholder(PlaceholderType.Body, PlaceholderSize.None, Orientation.None, 1) as Placeholder).GetBaseShape().TextBody as TextBody);
    TextBody textBody = (this.AddPlaceholder(PlaceholderType.SlideNumber, PlaceholderSize.Quarter, Orientation.None, 10) as Placeholder).GetBaseShape().TextBody as TextBody;
    if (textBody.Paragraphs.Count == 0)
      textBody.Paragraphs.Add();
    Paragraph paragraph = textBody.Paragraphs[0] as Paragraph;
    paragraph.HorizontalAlignment = HorizontalAlignmentType.Right;
    TextPart textPart = paragraph.AddTextPart() as TextPart;
    textPart.UniqueId = $"{{{Guid.NewGuid().ToString()}}}";
    textPart.Type = "slidenum";
    textPart.Text = "1";
  }

  internal void SetParentToClonedSlideItem(ISlideItem slideItem, BaseSlide baseSlide)
  {
    (slideItem as Shape).BaseSlide = baseSlide;
    switch ((slideItem as Shape).ShapeType)
    {
      case ShapeType.Sp:
        ((slideItem as Shape).TextBody as TextBody).SetParent(baseSlide);
        break;
      case ShapeType.GrpSp:
        GroupShape groupShape = slideItem as GroupShape;
        using (IEnumerator<ISlideItem> enumerator = groupShape.Shapes.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            ISlideItem current = enumerator.Current;
            (groupShape.Shapes as Shapes).SetParentToClonedSlideItem(current, baseSlide);
          }
          break;
        }
      case ShapeType.GraphicFrame:
        if ((slideItem as Shape).DrawingType == DrawingType.Table)
        {
          Table table = slideItem as Table;
          table.SetTableStyleParent(baseSlide);
          using (IEnumerator<IRow> enumerator = table.Rows.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              foreach (ICell cell in (IEnumerable<ICell>) enumerator.Current.Cells)
                (cell.TextBody as TextBody).SetParent(baseSlide);
            }
            break;
          }
        }
        if ((slideItem as Shape).DrawingType == DrawingType.SmartArt)
        {
          SmartArt smartArt = slideItem as SmartArt;
          DataModel dataModel = smartArt.DataModel;
          dataModel.SetParent(smartArt);
          dataModel.PointCollection.SetParent(dataModel);
          dataModel.PointCollection.SetPointTextBodyParent(baseSlide);
          (smartArt.Nodes as SmartArtNodes).SetParent(smartArt);
          break;
        }
        if ((slideItem as Shape).DrawingType != DrawingType.OleObject)
          break;
        OleObject oleObject = slideItem as OleObject;
        if (oleObject.OlePicture == null)
          break;
        oleObject.OlePicture.BaseSlide = baseSlide;
        break;
    }
  }

  internal void Close() => this.ClearAll();

  private void ClearAll()
  {
    if (this._list != null)
    {
      foreach (Shape shape in this._list)
      {
        if (shape != null)
        {
          switch (shape.DrawingType)
          {
            case DrawingType.None:
            case DrawingType.TextBox:
            case DrawingType.PlaceHolder:
              if (shape.ShapeType == ShapeType.GrpSp)
              {
                shape.Close();
                break;
              }
              shape.Close();
              break;
            case DrawingType.Table:
              shape.Close();
              break;
            case DrawingType.Chart:
              shape.Close();
              break;
            case DrawingType.SmartArt:
              shape.Close();
              break;
            case DrawingType.OleObject:
              shape.Close();
              break;
          }
        }
      }
      this.ClearShapesList();
      this._list = (List<ISlideItem>) null;
    }
    this._groupShape = (GroupShape) null;
    this._baseSlide = (BaseSlide) null;
  }

  public Shapes Clone()
  {
    Shapes shapes = (Shapes) this.MemberwiseClone();
    shapes._list = this.CloneList();
    return shapes;
  }

  private List<ISlideItem> CloneList()
  {
    List<ISlideItem> slideItemList = new List<ISlideItem>();
    foreach (ISlideItem slideItem1 in this._list)
    {
      ISlideItem slideItem2 = (ISlideItem) null;
      Shape shape = (Shape) slideItem1;
      switch (shape.ShapeType)
      {
        case ShapeType.Sp:
          slideItem2 = shape.Clone();
          break;
        case ShapeType.GrpSp:
          slideItem2 = ((Shape) slideItem1).Clone();
          break;
        case ShapeType.GraphicFrame:
          slideItem2 = shape.DrawingType != DrawingType.Table ? (shape.DrawingType != DrawingType.SmartArt ? (shape.DrawingType != DrawingType.OleObject ? ((Shape) slideItem1).Clone() : ((Shape) slideItem1).Clone()) : ((Shape) slideItem1).Clone()) : ((Shape) slideItem1).Clone();
          break;
        case ShapeType.CxnSp:
          slideItem2 = ((Shape) slideItem1).Clone();
          break;
        case ShapeType.Pic:
          slideItem2 = ((Shape) slideItem1).Clone();
          break;
        case ShapeType.AlternateContent:
          slideItem2 = ((Shape) slideItem1).Clone();
          break;
        case ShapeType.Chart:
          slideItem2 = ((Shape) slideItem1).Clone();
          break;
      }
      if (slideItem2 != null)
        slideItemList.Add(slideItem2);
    }
    return slideItemList;
  }

  private List<ISlideItem> CloneList(BaseSlide newParent, GroupShape newParentGroupShape)
  {
    List<ISlideItem> slideItemList = new List<ISlideItem>();
    foreach (ISlideItem slideItem1 in this._list)
    {
      ISlideItem slideItem2 = (ISlideItem) null;
      Shape shape = (Shape) slideItem1;
      switch (shape.ShapeType)
      {
        case ShapeType.Sp:
        case ShapeType.CxnSp:
          slideItem2 = shape.Clone();
          ((Shape) slideItem2).Group = newParentGroupShape;
          break;
        case ShapeType.GrpSp:
          GroupShape groupShape = (GroupShape) slideItem1;
          slideItem2 = groupShape.Clone();
          groupShape.Group = newParentGroupShape;
          groupShape.SetParentForGroupChild(newParent);
          break;
        case ShapeType.GraphicFrame:
          if (shape.DrawingType == DrawingType.Table)
          {
            slideItem2 = ((Shape) slideItem1).Clone();
            break;
          }
          break;
        case ShapeType.Pic:
          slideItem2 = ((Shape) slideItem1).Clone();
          ((Shape) slideItem2).Group = newParentGroupShape;
          break;
      }
      slideItemList.Add(slideItem2);
    }
    return slideItemList;
  }

  internal void SetParent(BaseSlide newParent)
  {
    this._baseSlide = newParent;
    foreach (Shape shape in this._list)
    {
      if (shape.ShapeType == ShapeType.GrpSp)
        ((Shapes) ((GroupShape) shape).Shapes).SetParent(newParent);
      else if (shape.SlideItemType == SlideItemType.Table)
      {
        if (shape is Table table)
          table.SetParent(newParent);
      }
      else if (shape.DrawingType == DrawingType.SmartArt)
        shape.SetParent(newParent);
      else if (shape.DrawingType == DrawingType.OleObject)
        shape.SetParent(newParent);
      shape.SetParent(newParent);
    }
  }

  internal void SetParent(GroupShape groupShape)
  {
    foreach (Shape shape in this._list)
      shape.SetGroupShape(groupShape);
  }
}
