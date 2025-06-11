// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.HeaderFooter.HeaderFooter
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Drawing;
using Syncfusion.Presentation.RichText;
using Syncfusion.Presentation.SlideImplementation;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation.HeaderFooter;

internal class HeaderFooter : IHeaderFooter
{
  private IBaseSlide _baseSlide;
  private HeaderFooterType _type;

  internal HeaderFooter(IBaseSlide baseSlide, HeaderFooterType type)
  {
    this._baseSlide = baseSlide;
    this._type = type;
  }

  public bool Visible
  {
    get => this.GetHeaderFooterShape(this.Type, this.BaseSlide) != null;
    set
    {
      if (!this.Visible && value)
      {
        this.AddHeaderFooterShape(this.Type);
      }
      else
      {
        if (!this.Visible || value)
          return;
        this.RemoveHeaderFooterShape(this.Type);
      }
    }
  }

  public string Text
  {
    get
    {
      Shape headerFooterShape = this.GetHeaderFooterShape(this.Type, this.BaseSlide);
      return headerFooterShape != null && headerFooterShape.TextBody != null ? headerFooterShape.TextBody.Text : string.Empty;
    }
    set
    {
      Shape headerFooterShape = this.GetHeaderFooterShape(this.Type, this.BaseSlide);
      if (headerFooterShape == null || headerFooterShape.TextBody == null || value == null)
        return;
      headerFooterShape.TextBody.Text = value;
    }
  }

  public DateTimeFormatType Format
  {
    get
    {
      if (this.Type == HeaderFooterType.DateAndTime)
      {
        Shape headerFooterShape = this.GetHeaderFooterShape(this.Type, this.BaseSlide);
        if (headerFooterShape != null && headerFooterShape.TextBody != null && headerFooterShape.TextBody.Paragraphs.Count > 0 && headerFooterShape.TextBody.Paragraphs[0].TextParts.Count > 0)
        {
          TextPart textPart = headerFooterShape.TextBody.Paragraphs[0].TextParts[0] as TextPart;
          if (textPart.UniqueId != null && textPart.Type != null)
            return Helper.GetDateTimeFormatType(textPart.Type);
        }
      }
      return DateTimeFormatType.None;
    }
    set
    {
      Shape shape = this.Type == HeaderFooterType.DateAndTime ? this.GetHeaderFooterShape(this.Type, this.BaseSlide) : throw new Exception("Format is valid only for DateAndTime.");
      if (shape == null || shape.TextBody == null)
        return;
      shape.TextBody.Text = string.Empty;
      TextPart textPart = shape.TextBody.Paragraphs[0].TextParts[0] as TextPart;
      if (value == DateTimeFormatType.None)
        return;
      textPart.UniqueId = $"{{{Guid.NewGuid().ToString()}}}";
      textPart.Type = Helper.GetDateTimeFieldType(value);
    }
  }

  internal IBaseSlide BaseSlide => this._baseSlide;

  internal HeaderFooterType Type => this._type;

  internal Shape GetHeaderFooterShape(HeaderFooterType headerFooterType, IBaseSlide baseSlide)
  {
    PlaceholderType placeHolderType = Helper.GetPlaceHolderType(headerFooterType);
    if (placeHolderType != PlaceholderType.Object)
    {
      foreach (Shape shape in (IEnumerable<ISlideItem>) baseSlide.Shapes)
      {
        if (shape.PlaceholderFormat != null && shape.PlaceholderFormat.Type == placeHolderType)
          return shape;
      }
    }
    return (Shape) null;
  }

  internal void AddHeaderFooterShape(HeaderFooterType headerFooterType)
  {
    IPlaceholderFormat placeholderFormat1 = (IPlaceholderFormat) null;
    Shape parentShape = (Shape) null;
    PlaceholderType placeHolderType = Helper.GetPlaceHolderType(headerFooterType);
    if (this.BaseSlide is Slide)
    {
      LayoutSlide layoutSlide = (this.BaseSlide as Slide).LayoutSlide as LayoutSlide;
      IMasterSlide masterSlide = layoutSlide.MasterSlide;
      parentShape = this.GetHeaderFooterShape(this.Type, (IBaseSlide) layoutSlide);
      if (parentShape != null)
      {
        Placeholder placeholderFormat2 = parentShape.PlaceholderFormat as Placeholder;
        placeholderFormat1 = (this.BaseSlide.Shapes as Shapes).AddPlaceholder(placeholderFormat2.Type, placeholderFormat2.Size, placeholderFormat2.Orientation, (int) placeholderFormat2.Index);
        if (this.Type == HeaderFooterType.Footer)
          (placeholderFormat1 as Placeholder).GetBaseShape().TextBody.Text = parentShape.TextBody.Text;
      }
    }
    else if (this.BaseSlide is LayoutSlide)
    {
      parentShape = this.GetHeaderFooterShape(this.Type, (IBaseSlide) ((this.BaseSlide as LayoutSlide).MasterSlide as MasterSlide));
      if (parentShape != null)
      {
        Placeholder placeholderFormat3 = parentShape.PlaceholderFormat as Placeholder;
        placeholderFormat1 = (this.BaseSlide.Shapes as Shapes).AddPlaceholder(placeholderFormat3.Type, placeholderFormat3.Size, placeholderFormat3.Orientation, (int) placeholderFormat3.Index);
        if (this.Type == HeaderFooterType.Footer)
          (placeholderFormat1 as Placeholder).GetBaseShape().TextBody.Text = parentShape.TextBody.Text;
      }
    }
    else if (this.BaseSlide is MasterSlide)
    {
      IBaseSlide baseSlide = this.BaseSlide;
      placeholderFormat1 = (this.BaseSlide.Shapes as Shapes).AddPlaceholder(placeHolderType, PlaceholderSize.Quarter, Orientation.None, this.GetConstantHeaderFooterIndex());
    }
    else if (this.BaseSlide is NotesSlide)
    {
      NotesMasterSlide notesMaster = (this.BaseSlide as NotesSlide).Presentation.NotesMaster;
      if (notesMaster != null)
        parentShape = this.GetHeaderFooterShape(this.Type, (IBaseSlide) notesMaster);
      if (parentShape != null)
      {
        Placeholder placeholderFormat4 = parentShape.PlaceholderFormat as Placeholder;
        placeholderFormat1 = (this.BaseSlide.Shapes as Shapes).AddPlaceholder(placeholderFormat4.Type, placeholderFormat4.Size, placeholderFormat4.Orientation, (int) placeholderFormat4.Index);
        if (this.Type == HeaderFooterType.Header || this.Type == HeaderFooterType.Footer)
          (placeholderFormat1 as Placeholder).GetBaseShape().TextBody.Text = parentShape.TextBody.Text;
      }
      else
        placeholderFormat1 = (this.BaseSlide.Shapes as Shapes).AddPlaceholder(placeHolderType, PlaceholderSize.Quarter, Orientation.None, this.GetConstantHeaderFooterIndex());
    }
    Shape headerFooterShape = (Shape) null;
    if (placeholderFormat1 != null)
      headerFooterShape = (placeholderFormat1 as Placeholder).GetBaseShape();
    if (headerFooterShape != null && (this.Type == HeaderFooterType.DateAndTime || this.Type == HeaderFooterType.SlideNumber))
      this.ApplyTextFieldProperties(headerFooterShape, parentShape);
    if (headerFooterShape == null || parentShape != null)
      return;
    this.ApplyPlaceHolderProperties(headerFooterShape);
  }

  internal void ApplyTextFieldProperties(Shape headerFooterShape, Shape parentShape)
  {
    headerFooterShape.TextBody.Text = string.Empty;
    TextPart textPart1 = headerFooterShape.TextBody.Paragraphs[0].TextParts[0] as TextPart;
    textPart1.UniqueId = $"{{{Guid.NewGuid().ToString()}}}";
    textPart1.Type = this.Type != HeaderFooterType.DateAndTime ? "slidenum" : "datetime1";
    if (parentShape == null || parentShape.TextBody == null || parentShape.TextBody.Paragraphs.Count <= 0 || parentShape.TextBody.Paragraphs[0].TextParts.Count <= 0)
      return;
    TextPart textPart2 = parentShape.TextBody.Paragraphs[0].TextParts[0] as TextPart;
    if (textPart2.Type == null || textPart2.Type == string.Empty)
    {
      textPart1.Type = (string) null;
      textPart1.Text = textPart2.Text;
    }
    else
    {
      if (this.Type != HeaderFooterType.DateAndTime || Helper.GetDateTimeFormatType(textPart2.Type) == DateTimeFormatType.None)
        return;
      textPart1.Type = textPart2.Type;
    }
  }

  internal void ApplyPlaceHolderProperties(Shape headerFooterShape)
  {
    headerFooterShape.AutoShapeType = AutoShapeType.Rectangle;
    TextBody textBody = headerFooterShape.TextBody as TextBody;
    textBody.SetMargin(91440, 45720, 91440, 45720);
    textBody.IsAutoMargins = false;
    textBody.SetTextDirection(TextDirection.Horizontal);
    textBody.WrapText = false;
    Paragraph paragraph = new Paragraph((Paragraphs) textBody.Paragraphs);
    paragraph.IsWithinList = true;
    Font font = new Font(paragraph);
    font.SetFontSize(1200);
    paragraph.SetFont(font);
    textBody.StyleList.Add("lvl1pPr", paragraph);
    if (this.BaseSlide is Slide || this.BaseSlide is LayoutSlide || this.BaseSlide is MasterSlide)
    {
      switch (this.Type)
      {
        case HeaderFooterType.Footer:
          headerFooterShape.ShapeFrame.SetAnchor(new bool?(), new bool?(), -1, 4038600L, 6356350L, 4114800L, 365125L);
          paragraph.SetAlignmentType(HorizontalAlignment.Center);
          break;
        case HeaderFooterType.DateAndTime:
          headerFooterShape.ShapeFrame.SetAnchor(new bool?(), new bool?(), -1, 838200L, 6356350L, 2743200L, 365125L);
          paragraph.SetAlignmentType(HorizontalAlignment.Left);
          break;
        case HeaderFooterType.SlideNumber:
          headerFooterShape.ShapeFrame.SetAnchor(new bool?(), new bool?(), -1, 8610600L, 6356350L, 2743200L, 365125L);
          paragraph.SetAlignmentType(HorizontalAlignment.Right);
          break;
      }
      textBody.SetVerticalAlign(VerticalAlignment.Middle);
      font.Fill.FillType = FillType.Solid;
      font.IsColorSet = true;
      ColorObject colorObject = new ColorObject(true);
      colorObject.SetColor(ColorType.RGB, 0);
      colorObject.ColorTransFormCollection.AddColorTransForm(ColorMode.Tint, 75000);
      SolidFill solidFill = (SolidFill) font.Fill.SolidFill;
      font.SetColorObject(colorObject);
    }
    else
    {
      if (!(this.BaseSlide is NotesSlide))
        return;
      switch (this.Type)
      {
        case HeaderFooterType.Footer:
          headerFooterShape.ShapeFrame.SetAnchor(new bool?(), new bool?(), -1, 0L, 8685213L, 2971800L, 458787L);
          textBody.SetVerticalAlign(VerticalAlignment.Bottom);
          paragraph.SetAlignmentType(HorizontalAlignment.Left);
          break;
        case HeaderFooterType.DateAndTime:
          headerFooterShape.ShapeFrame.SetAnchor(new bool?(), new bool?(), -1, 3884613L, 0L, 2971800L, 458788L);
          paragraph.SetAlignmentType(HorizontalAlignment.Right);
          break;
        case HeaderFooterType.SlideNumber:
          headerFooterShape.ShapeFrame.SetAnchor(new bool?(), new bool?(), -1, 3884613L, 8685213L, 2971800L, 458787L);
          textBody.SetVerticalAlign(VerticalAlignment.Bottom);
          paragraph.SetAlignmentType(HorizontalAlignment.Right);
          break;
        case HeaderFooterType.Header:
          headerFooterShape.ShapeFrame.SetAnchor(new bool?(), new bool?(), -1, 0L, 0L, 2971800L, 458788L);
          paragraph.SetAlignmentType(HorizontalAlignment.Left);
          break;
      }
    }
  }

  internal int GetConstantHeaderFooterIndex()
  {
    switch (this.Type)
    {
      case HeaderFooterType.Footer:
        if (this.BaseSlide is Slide || this.BaseSlide is LayoutSlide)
          return 11;
        if (this.BaseSlide is MasterSlide)
          return 7;
        if (this.BaseSlide is NotesSlide)
          return 4;
        break;
      case HeaderFooterType.DateAndTime:
        if (this.BaseSlide is Slide || this.BaseSlide is LayoutSlide)
          return 10;
        if (this.BaseSlide is MasterSlide)
          return 2;
        if (this.BaseSlide is NotesSlide)
          return 1;
        break;
      case HeaderFooterType.SlideNumber:
        if (this.BaseSlide is Slide || this.BaseSlide is LayoutSlide)
          return 12;
        if (this.BaseSlide is MasterSlide)
          return 4;
        if (this.BaseSlide is NotesSlide)
          return 5;
        break;
      case HeaderFooterType.Header:
        if (this.BaseSlide is NotesSlide)
          return 2;
        break;
    }
    return 1;
  }

  internal void RemoveHeaderFooterShape(HeaderFooterType headerFooterType)
  {
    PlaceholderType placeHolderType = Helper.GetPlaceHolderType(headerFooterType);
    if (placeHolderType == PlaceholderType.Object)
      return;
    foreach (Shape shape in (IEnumerable<ISlideItem>) this.BaseSlide.Shapes)
    {
      if (shape.PlaceholderFormat != null && shape.PlaceholderFormat.Type == placeHolderType)
      {
        this.BaseSlide.Shapes.Remove((ISlideItem) shape);
        break;
      }
    }
  }

  internal Syncfusion.Presentation.HeaderFooter.HeaderFooter Clone()
  {
    return (Syncfusion.Presentation.HeaderFooter.HeaderFooter) this.MemberwiseClone();
  }

  internal void SetParent(IBaseSlide newParent) => this._baseSlide = newParent;
}
