// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Drawing.Picture
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Drawing;
using Syncfusion.Presentation.SlideImplementation;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.Presentation.Drawing;

internal class Picture : Shape, IPicture, ISlideItem
{
  private int _brightness;
  private int _bright;
  private int contrast;
  private int amount;
  private int _colorTemp;
  private int _saturation;
  private string _imgEmbedId;
  private int _contrast;
  private int _amount;
  private int _threshold;
  private bool _grayScale;
  private List<ColorObject> _duoTone;
  private List<ColorObject> _colorChange;
  private string _embedId;
  private bool _flag;
  private bool _isEmbed;
  private bool _isLink;
  private bool _isUseAlpha;
  private string _linkId;
  private string _imageExtension;
  private byte[] _basStream;
  private FormatPicture _formatPicture;
  private string _pictureId;
  private string _picturePath;
  private string _svgPictureId;
  private string _svgPicturePath;
  private byte[] _svgImageData;
  private bool _isUpdatedSvg;
  internal string SvgRelationId;

  internal Picture(BaseSlide baseSlide)
    : base(ShapeType.Pic, baseSlide)
  {
    this._amount = 100000;
    this._threshold = -1;
    this._isUseAlpha = true;
  }

  internal int Brightness
  {
    get => this._brightness;
    set => this._brightness = value;
  }

  internal int Bright
  {
    get => this._bright;
    set => this._bright = value;
  }

  internal int ColorTemp
  {
    get => this._colorTemp;
    set => this._colorTemp = value;
  }

  internal int ImageContrast
  {
    get => this.contrast;
    set => this.contrast = value;
  }

  internal int ImageAmount
  {
    get => this.amount;
    set => this.amount = value;
  }

  internal int Saturation
  {
    get => this._saturation;
    set => this._saturation = value;
  }

  internal bool IsUseAlpha
  {
    get => this._isUseAlpha;
    set => this._isUseAlpha = value;
  }

  internal int Contrast
  {
    get => this._contrast;
    set => this._contrast = value;
  }

  internal List<ColorObject> DuoTone
  {
    get
    {
      if (this._duoTone != null)
      {
        foreach (ColorObject colorObject in this._duoTone)
          colorObject.UpdateColorObject((object) this.BaseSlide.Presentation);
      }
      else
        this._duoTone = new List<ColorObject>(2);
      return this._duoTone;
    }
  }

  internal List<ColorObject> ColorChange
  {
    get
    {
      if (this._colorChange != null)
      {
        foreach (ColorObject colorObject in this._colorChange)
          colorObject.UpdateColorObject((object) this.BaseSlide.Presentation);
      }
      else
        this._colorChange = new List<ColorObject>(2);
      return this._colorChange;
    }
  }

  internal int Amount
  {
    get => this._amount;
    set => this._amount = value;
  }

  internal int Threshold
  {
    get => this._threshold;
    set => this._threshold = value;
  }

  internal bool GrayScale
  {
    get => this._grayScale;
    set => this._grayScale = value;
  }

  internal bool IsEmbed
  {
    get => this._isEmbed;
    set => this._isEmbed = value;
  }

  internal bool IsLink
  {
    get => this._isLink;
    set => this._isLink = value;
  }

  internal string LinkId
  {
    get => this._linkId;
    set => this._linkId = value;
  }

  internal bool Flag
  {
    get => this._flag;
    set => this._flag = value;
  }

  internal string EmbedId
  {
    get => this._embedId;
    set => this._embedId = value;
  }

  internal string ImageEmbedId
  {
    get => this._imgEmbedId;
    set => this._imgEmbedId = value;
  }

  internal FormatPicture FormatPicture
  {
    get => this._formatPicture ?? (this._formatPicture = new FormatPicture());
  }

  public byte[] SvgData
  {
    get => this._svgImageData;
    set
    {
      if (value == null || value.Length <= 0)
        return;
      this._svgImageData = value;
      if (this._svgPicturePath == null)
        throw new Exception("Image is not available in the PowerPoint presentation");
      string str = "ppt/" + this._svgPicturePath.Remove(0, 3);
      int imageFormat = (int) this.ObtainImageFormat(true);
      this.BaseSlide.Presentation.DataHolder.Archive.RemoveItem(str);
      MemoryStream stream = new MemoryStream(value);
      this.BaseSlide.Presentation.DataHolder.AddImageToArchive(str, stream);
    }
  }

  public byte[] ImageData
  {
    get
    {
      return this._basStream != null && this._basStream.Length > 0 ? (byte[]) this._basStream.Clone() : (byte[]) null;
    }
    set
    {
      if (value == null || value.Length <= 0)
        return;
      if (this.SvgData != null)
        this._isUpdatedSvg = true;
      MemoryStream memoryStream = new MemoryStream(value);
      this.AddImageStream((Stream) memoryStream);
      if (this._picturePath == null)
        return;
      string str1 = "ppt/" + this._picturePath.Remove(0, 3);
      if (this._pictureId == null)
      {
        List<IPicture> list = ((PicturesCollection) this.BaseSlide.Pictures).GetList();
        int num1 = 0;
        foreach (Picture picture in list)
        {
          if (picture.ShapeName != this.ShapeName)
          {
            int num2 = int.Parse(picture._embedId.Remove(0, 3));
            if (num2 > num1)
              num1 = num2;
          }
        }
        int imageFormat = (int) this.ObtainImageFormat(false);
        this._pictureId = $"rId{(object) num1}{(object) 1}";
        this._embedId = this._pictureId;
        this._picturePath = $"../media/image{(object) (this.BaseSlide.Presentation.ImageCount + 1)}{this._imageExtension}";
        string str2 = "ppt/" + this._picturePath.Remove(0, 3);
      }
      else
      {
        string picturePath = this._picturePath;
        this._picturePath = string.Empty;
        if (this.BaseSlide.Presentation.DataHolder.IsImageRefereceExist(this.BaseSlide, picturePath))
        {
          this._picturePath = picturePath;
          ++this.BaseSlide.Presentation.ImageCount;
          int startIndex = this._picturePath.Substring(this._picturePath.Length - 4) == "jpeg" ? this._picturePath.Length - 6 : this._picturePath.Length - 5;
          this._picturePath = this._picturePath.Remove(startIndex, 1);
          this._picturePath = this._picturePath.Insert(startIndex, this.BaseSlide.Presentation.ImageCount.ToString());
          this.BaseSlide.Presentation.DataHolder.AddImageToArchive("ppt/" + this._picturePath.Remove(0, 3), memoryStream);
          if (!this.BaseSlide.IsImageReferenceExists(picturePath))
            return;
          string relationId = Helper.GenerateRelationId(this.BaseSlide.TopRelation);
          this._pictureId = relationId;
          this.EmbedId = relationId;
        }
        else
        {
          this._picturePath = picturePath;
          this.BaseSlide.Presentation.DataHolder.Archive.RemoveItem(str1);
          this.BaseSlide.Presentation.DataHolder.AddImageToArchive(str1, memoryStream);
        }
      }
    }
  }

  public ImageFormat ImageFormat => this.ObtainImageFormat(this.SvgData != null);

  internal void AddImage(Stream svgStream, Stream rasterImgStream)
  {
    this.AddImageStream(svgStream, rasterImgStream);
    this.IsEmbed = true;
  }

  internal void AddImage(Stream stream)
  {
    this.AddImageStream(stream);
    this.IsEmbed = true;
  }

  internal string GetImagePath() => this._picturePath;

  internal void SetImagePath(string path) => this._picturePath = path;

  internal void AddImageStream(Stream svgStreamInput, Stream rasterImgStreamInput)
  {
    MemoryStream output1 = new MemoryStream();
    Picture.CopyStream(rasterImgStreamInput, (Stream) output1);
    this._basStream = output1.ToArray();
    MemoryStream output2 = new MemoryStream();
    Picture.CopyStream(svgStreamInput, (Stream) output2);
    this._svgImageData = output2.ToArray();
  }

  internal void AddImageStream(Stream input)
  {
    MemoryStream output = new MemoryStream();
    Picture.CopyStream(input, (Stream) output);
    if (this.SvgRelationId != null && !this._isUpdatedSvg)
      this._svgImageData = output.ToArray();
    else
      this._basStream = output.ToArray();
  }

  internal void AddImageStream(string embedattrib)
  {
    RelationCollection topRelation = this.BaseSlide.TopRelation;
    if (topRelation == null)
      return;
    string itemPathByRelation = topRelation.GetItemPathByRelation(embedattrib);
    if (!(itemPathByRelation.ToLower() != "null"))
      return;
    if (itemPathByRelation.EndsWith(".svg"))
    {
      this._svgPicturePath = itemPathByRelation;
      this._svgPictureId = embedattrib;
    }
    else
    {
      this._picturePath = itemPathByRelation;
      this._pictureId = embedattrib;
    }
    if (itemPathByRelation == null)
      return;
    string[] strArray = itemPathByRelation.Split('/');
    string imagePath = $"ppt/{strArray[strArray.Length - 2]}{(object) '/'}{strArray[strArray.Length - 1]}";
    Stream imageStream = this.BaseSlide.Presentation.DataHolder.GetImageStream(imagePath);
    if (imagePath.EndsWith(".svg"))
      this.SvgRelationId = embedattrib;
    if (imageStream != null && imageStream.Length > 0L)
      this.AddImageStream(imageStream);
    topRelation.GetImageRemoveList().Add(embedattrib);
  }

  internal void AddImageToArchive(bool isVectorImage)
  {
    int imageFormat = (int) this.ObtainImageFormat(isVectorImage);
    Relation relation;
    if (this._pictureId == null && this._picturePath == null)
    {
      string relationId = Helper.GenerateRelationId(this.BaseSlide.TopRelation);
      relation = new Relation(relationId, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image", $"../media/image{(object) ++this.BaseSlide.Presentation.ImageCount}{this._imageExtension}", (string) null);
      if (isVectorImage)
        this.SvgRelationId = relationId;
      else
        this.EmbedId = relationId;
      this.BaseSlide.Presentation.DataHolder.AddImage($"ppt/media/image{(object) this.BaseSlide.Presentation.ImageCount}{this._imageExtension}", (Stream) new MemoryStream(isVectorImage ? this.SvgData : this._basStream), relation);
    }
    else
    {
      relation = this._svgPictureId == null || !this.BaseSlide.TopRelation.Contains(this._pictureId) ? new Relation(this._pictureId, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image", this._picturePath, (string) null) : new Relation(this._svgPictureId, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image", this._svgPicturePath, (string) null);
      ++this.BaseSlide.Presentation.ImageCount;
    }
    this.BaseSlide.TopRelation.Add(relation.Id, relation);
    string partName = this._imageExtension.Replace(".", "");
    this.BaseSlide.Presentation.AddDefaultContentType(partName, "image/" + partName);
  }

  internal static void CopyStream(Stream input, Stream output)
  {
    if (input == null)
      throw new ArgumentNullException(nameof (input));
    if (output == null)
      throw new ArgumentNullException(nameof (output));
    input.Position = 0L;
    input.CopyTo(output);
  }

  private ImageFormat ObtainImageFormat(bool isSvg)
  {
    byte[] imageData = this.ImageData;
    if (imageData == null || imageData.Length <= 0)
      return ImageFormat.Unknown;
    Image image = Image.FromStream((Stream) new MemoryStream(imageData, false));
    ImageFormat rawFormat = image.RawFormat;
    image.Close();
    if (this.SvgData != null && isSvg)
    {
      this._imageExtension = ".svg";
      return this.GetImageFormat(rawFormat);
    }
    if (rawFormat.Equals((object) ImageFormat.Jpeg))
    {
      this._imageExtension = ".jpeg";
      return ImageFormat.Jpeg;
    }
    if (rawFormat.Equals((object) ImageFormat.Bmp))
    {
      this._imageExtension = ".bmp";
      return ImageFormat.Bmp;
    }
    if (rawFormat.Equals((object) ImageFormat.Png))
    {
      this._imageExtension = ".png";
      return ImageFormat.Png;
    }
    if (rawFormat.Equals((object) ImageFormat.Emf))
    {
      this._imageExtension = ".emf";
      return ImageFormat.Emf;
    }
    if (rawFormat.Equals((object) ImageFormat.Exif))
    {
      this._imageExtension = ".exif";
      return ImageFormat.Exif;
    }
    if (rawFormat.Equals((object) ImageFormat.Gif))
    {
      this._imageExtension = ".gif";
      return ImageFormat.Gif;
    }
    if (rawFormat.Equals((object) ImageFormat.Icon))
    {
      this._imageExtension = ".ico";
      return ImageFormat.Icon;
    }
    if (rawFormat.Equals((object) ImageFormat.MemoryBmp))
    {
      this._imageExtension = ".bmp";
      return ImageFormat.MemoryBmp;
    }
    if (rawFormat.Equals((object) ImageFormat.Tiff))
    {
      this._imageExtension = ".tiff";
      return ImageFormat.Tiff;
    }
    this._imageExtension = ".wmf";
    return ImageFormat.Wmf;
  }

  private ImageFormat GetImageFormat(ImageFormat rawFormat)
  {
    if (rawFormat.Equals((object) ImageFormat.Jpeg))
      return ImageFormat.Jpeg;
    if (rawFormat.Equals((object) ImageFormat.Bmp))
      return ImageFormat.Bmp;
    if (rawFormat.Equals((object) ImageFormat.Png))
      return ImageFormat.Png;
    if (rawFormat.Equals((object) ImageFormat.Emf))
      return ImageFormat.Emf;
    if (rawFormat.Equals((object) ImageFormat.Exif))
      return ImageFormat.Exif;
    if (rawFormat.Equals((object) ImageFormat.Gif))
      return ImageFormat.Gif;
    if (rawFormat.Equals((object) ImageFormat.Icon))
      return ImageFormat.Icon;
    if (rawFormat.Equals((object) ImageFormat.MemoryBmp))
      return ImageFormat.MemoryBmp;
    return rawFormat.Equals((object) ImageFormat.Tiff) ? ImageFormat.Tiff : ImageFormat.Wmf;
  }

  internal override void Close()
  {
    base.Close();
    this.Clear();
  }

  internal void Clear()
  {
    if (this._basStream != null)
      this._basStream = (byte[]) null;
    if (this._colorChange != null)
    {
      foreach (ColorObject colorObject in this._colorChange)
        colorObject.Close();
      this._colorChange.Clear();
      this._colorChange = (List<ColorObject>) null;
    }
    if (this._duoTone != null)
    {
      foreach (ColorObject colorObject in this._duoTone)
        colorObject.Close();
      this._duoTone.Clear();
      this._duoTone = (List<ColorObject>) null;
    }
    if (this._formatPicture == null)
      return;
    this._formatPicture.Close();
    this._formatPicture = (FormatPicture) null;
  }

  public override ISlideItem Clone()
  {
    Picture picture = (Picture) this.MemberwiseClone();
    this.Clone((Shape) picture);
    this.ClonePictureElements(picture);
    return (ISlideItem) picture;
  }

  internal override void SetParent(BaseSlide newParent) => base.SetParent(newParent);

  private void ClonePictureElements(Picture picture)
  {
    picture._basStream = Helper.CloneByteArray(this._basStream);
    if (this._colorChange != null)
      picture._colorChange = Helper.CloneColorObjectList(this._colorChange);
    if (this._duoTone != null)
      picture._duoTone = Helper.CloneColorObjectList(this._duoTone);
    if (this._formatPicture != null)
      picture._formatPicture = this._formatPicture.Clone();
    if (this._pictureId != null)
      picture._pictureId = (string) null;
    if (this._picturePath == null)
      return;
    picture._picturePath = (string) null;
  }

  internal IShape Clone(GroupShape newParentGroupShape)
  {
    Picture picture = (Picture) this.MemberwiseClone();
    this.Clone((Shape) picture);
    this.Group = newParentGroupShape;
    this.ClonePictureElements(picture);
    return (IShape) picture;
  }
}
