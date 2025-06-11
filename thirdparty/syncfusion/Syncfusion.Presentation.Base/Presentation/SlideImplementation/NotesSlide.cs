// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.SlideImplementation.NotesSlide
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Drawing;
using Syncfusion.Presentation.Layouting;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.Presentation.SlideImplementation;

internal class NotesSlide : BaseSlide, INotesSlide, IBaseSlide
{
  private Syncfusion.Presentation.RichText.TextBody _textBody;
  private int _index;
  private Slide _parentSlide;
  private bool _isPortableRendering;
  private SlideInfo _slideInfo;
  private System.Drawing.Image _thumbnailImage;

  internal void EnablePortableRendering(bool settings) => this._isPortableRendering = settings;

  internal NotesSlide(Slide slide, int index)
    : base(slide.Presentation)
  {
    this._textBody = new Syncfusion.Presentation.RichText.TextBody((BaseSlide) this);
    this._parentSlide = slide;
    this._index = index;
    this.TopRelation = new RelationCollection();
  }

  public ITextBody NotesTextBody
  {
    get
    {
      if (this._textBody.Paragraphs.Count == 0)
        this._textBody.AddParagraph("");
      return (ITextBody) this._textBody;
    }
  }

  internal Slide ParentSlide => this._parentSlide;

  internal int Index => this._index;

  internal bool IsPortableRendering => this._isPortableRendering;

  internal void SetTextBody(Syncfusion.Presentation.RichText.TextBody textBody)
  {
    this._textBody = textBody;
  }

  internal Stream ConvertToImage(Syncfusion.Drawing.ImageFormat imageFormat)
  {
    this.Presentation.SetExportingSlide(this);
    Stream image = new Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter().ConvertToImage(this, imageFormat);
    image.Position = 0L;
    return image;
  }

  internal void Layout()
  {
    this._slideInfo = new SlideInfo(this);
    this._slideInfo.Bounds = new RectangleF(0.0f, 0.0f, (float) Syncfusion.Presentation.Drawing.Helper.EmuToPoint(((NotesSize) this.Presentation.NotesSize).CX), (float) Syncfusion.Presentation.Drawing.Helper.EmuToPoint(((NotesSize) this.Presentation.NotesSize).CY));
    foreach (Shape shape in (IEnumerable<ISlideItem>) this.Shapes)
      this._parentSlide.Layout(shape);
  }

  public System.Drawing.Image ThumbnailImage
  {
    get => this._thumbnailImage;
    set
    {
      if (value == null)
        return;
      foreach (Shape shape in (IEnumerable<ISlideItem>) this.Shapes)
      {
        if (shape.PlaceholderFormat != null && shape.GetPlaceholder().GetPlaceholderType() == PlaceholderType.SlideImage)
        {
          Fill fill = new Fill(shape);
          TextureFill pictureFill = fill.PictureFill as TextureFill;
          MemoryStream memoryStream = new MemoryStream();
          value.Save((Stream) memoryStream, System.Drawing.Imaging.ImageFormat.Png);
          pictureFill.ImageBytes = memoryStream.ToArray();
          shape.SetFill(fill);
          break;
        }
      }
      this._thumbnailImage = value;
    }
  }

  public NotesSlide Clone()
  {
    NotesSlide newParent = (NotesSlide) this.MemberwiseClone();
    if (this._slideInfo != null)
      newParent._slideInfo = this._slideInfo.Clone();
    if (this._thumbnailImage != null)
      newParent._thumbnailImage = (System.Drawing.Image) this._thumbnailImage.Clone();
    this.Clone((BaseSlide) newParent);
    if (this._textBody != null)
    {
      Shape shape1 = (Shape) null;
      foreach (Shape shape2 in (IEnumerable<ISlideItem>) newParent.Shapes)
      {
        if (shape2.SlideItemType == SlideItemType.Placeholder && shape2.PlaceholderFormat != null && shape2.PlaceholderFormat.Type == PlaceholderType.Body)
        {
          shape1 = shape2;
          break;
        }
      }
      if (shape1 != null)
        newParent.SetTextBody(shape1.TextBody as Syncfusion.Presentation.RichText.TextBody);
    }
    return newParent;
  }

  internal void SetParent(Slide slide) => this._parentSlide = slide;
}
