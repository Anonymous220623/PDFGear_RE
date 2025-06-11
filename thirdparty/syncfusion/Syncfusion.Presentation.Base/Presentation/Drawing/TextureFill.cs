// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Drawing.TextureFill
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Drawing;
using Syncfusion.Presentation.SlideImplementation;
using Syncfusion.Presentation.SmartArtImplementation;
using Syncfusion.Presentation.TableImplementation;
using Syncfusion.Presentation.Themes;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.Presentation.Drawing;

internal class TextureFill : IPictureFill
{
  private Fill _fillFormat;
  private object _formatOption;
  private string _imageRelationId;
  private TextureFillType _textureType;
  private List<ColorObject> _duoTone;
  private Shape _baseShape;
  private string _texturePath;
  private byte[] _basStream;
  private string _imageExtension;
  private int _transparency;
  private List<ColorObject> _duoToneForImageConversion;
  private FormatPicture _formatPicture;
  private TileMode _tileMode;

  internal TextureFill(Fill fillFormat, Shape shape)
  {
    this._formatOption = (object) new TilePicOption();
    this._fillFormat = fillFormat;
    this._transparency = 100000;
    this._baseShape = shape;
  }

  public TextureFill(Fill fillFormat)
  {
    this._fillFormat = fillFormat;
    this._formatOption = (object) new TilePicOption();
    this._transparency = 100000;
  }

  public double OffsetTop
  {
    get => this.PicFormatOption != null ? this.PicFormatOption.Top : 0.0;
    set
    {
      if (this.TileMode != TileMode.Stretch)
        throw new Exception("Tile mode must be 'TileMode.Stretch' to use this 'OffsetTop' property");
      if (value < -100000.0)
        throw new ArgumentException(" 'OffsetTop' value must be greater than or equal to -100000");
      if (value > 100000.0)
        throw new ArgumentException(" 'OffsetTop' value must be less than or equal to 100000");
      if (this._fillFormat.Parent is Background)
      {
        Background parent = this._fillFormat.Parent as Background;
        if (parent.GetFillFormat().FillType == FillType.Automatic)
          parent.SetFill((IFill) this._fillFormat);
      }
      if (this.PicFormatOption == null)
      {
        PicFormatOption picFormatOption = new PicFormatOption();
      }
      this.PicFormatOption.SetTop((int) value * 1000);
    }
  }

  public double OffsetLeft
  {
    get => this.PicFormatOption != null ? this.PicFormatOption.Left : 0.0;
    set
    {
      if (this.TileMode != TileMode.Stretch)
        throw new Exception("Tile mode must be 'TileMode.Stretch' to use this 'OffsetLeft' property");
      if (value < -100000.0)
        throw new ArgumentException("'OffsetLeft' value must be greater than or equal to -100000");
      if (value > 100000.0)
        throw new ArgumentException("'OffsetLeft' value must be less than or equal to 100000");
      if (this._fillFormat.Parent is Background)
      {
        Background parent = this._fillFormat.Parent as Background;
        if (parent.GetFillFormat().FillType == FillType.Automatic)
          parent.SetFill((IFill) this._fillFormat);
      }
      if (this.PicFormatOption == null)
      {
        PicFormatOption picFormatOption = new PicFormatOption();
      }
      this.PicFormatOption.SetLeft((int) value * 1000);
    }
  }

  public double OffsetBottom
  {
    get => this.PicFormatOption != null ? this.PicFormatOption.Bottom : 0.0;
    set
    {
      if (this.TileMode != TileMode.Stretch)
        throw new Exception("Tile mode must be 'TileMode.Stretch' to use this 'OffsetBottom' property");
      if (value < -100000.0)
        throw new ArgumentException("'OffsetBottom' value must be greater than or equal to -100000");
      if (value > 100000.0)
        throw new ArgumentException("'OffsetBottom' value must be less than or equal to 100000");
      if (this._fillFormat.Parent is Background)
      {
        Background parent = this._fillFormat.Parent as Background;
        if (parent.GetFillFormat().FillType == FillType.Automatic)
          parent.SetFill((IFill) this._fillFormat);
      }
      if (this.PicFormatOption == null)
      {
        PicFormatOption picFormatOption = new PicFormatOption();
      }
      this.PicFormatOption.SetBottom((int) value * 1000);
    }
  }

  public double OffsetRight
  {
    get => this.PicFormatOption != null ? this.PicFormatOption.Right : 0.0;
    set
    {
      if (this.TileMode != TileMode.Stretch)
        throw new Exception("Tile mode must be 'TileMode.Stretch' to use this 'OffsetRight' property");
      if (value < -100000.0)
        throw new ArgumentException("'OffsetRight' value must be greater than or equal to -100000");
      if (value > 100000.0)
        throw new ArgumentException("'OffsetRight' value must be less than or equal to 100000");
      if (this._fillFormat.Parent is Background)
      {
        Background parent = this._fillFormat.Parent as Background;
        if (parent.GetFillFormat().FillType == FillType.Automatic)
          parent.SetFill((IFill) this._fillFormat);
      }
      if (this.PicFormatOption == null)
      {
        PicFormatOption picFormatOption = new PicFormatOption();
      }
      this.PicFormatOption.SetRight((int) value * 1000);
    }
  }

  public double OffsetX
  {
    get => this.TilePicOption != null ? this.TilePicOption.OffsetX : 0.0;
    set
    {
      if (this.TileMode != TileMode.Tile)
        throw new Exception("Tile mode must be 'TileMode.Tile' to use this 'OffsetX' property");
      if (value < -1584.0)
        throw new ArgumentException("'OffsetX' value must be greater than or equal to -1584");
      if (value > 1584.0)
        throw new ArgumentException("'OffsetX' value must be less than or equal to 1584");
      if (this._fillFormat.Parent is Background)
      {
        Background parent = this._fillFormat.Parent as Background;
        if (parent.GetFillFormat().FillType == FillType.Automatic)
          parent.SetFill((IFill) this._fillFormat);
      }
      if (this.TilePicOption == null)
      {
        TilePicOption tilePicOption = new TilePicOption();
      }
      this.TilePicOption.OffsetX = value;
    }
  }

  public double OffsetY
  {
    get => this.TilePicOption != null ? this.TilePicOption.OffsetY : 0.0;
    set
    {
      if (this.TileMode != TileMode.Tile)
        throw new Exception("Tile mode must be 'TileMode.Tile' to use this 'OffsetY' property");
      if (value < -1584.0)
        throw new ArgumentException("'OffsetY' value must be greater than or equal to -1584");
      if (value > 1584.0)
        throw new ArgumentException("'OffsetY' value must be less than or equal to 1584");
      if (this._fillFormat.Parent is Background)
      {
        Background parent = this._fillFormat.Parent as Background;
        if (parent.GetFillFormat().FillType == FillType.Automatic)
          parent.SetFill((IFill) this._fillFormat);
      }
      if (this.TilePicOption == null)
      {
        TilePicOption tilePicOption = new TilePicOption();
      }
      this.TilePicOption.OffsetY = value;
    }
  }

  public double ScaleX
  {
    get => this.TilePicOption != null ? this.TilePicOption.ScaleX : 100.0;
    set
    {
      if (this.TileMode != TileMode.Tile)
        throw new Exception("Tile mode must be 'TileMode.Tile' to use this 'ScaleX' property");
      if (value < 0.0 || value > 100.0)
        throw new ArgumentException("The value range of 'ScaleX' must be within 0 to 100");
      if (this._fillFormat.Parent is Background)
      {
        Background parent = this._fillFormat.Parent as Background;
        if (parent.GetFillFormat().FillType == FillType.Automatic)
          parent.SetFill((IFill) this._fillFormat);
      }
      if (this.TilePicOption == null)
      {
        TilePicOption tilePicOption = new TilePicOption();
      }
      this.TilePicOption.ScaleX = value;
    }
  }

  public double ScaleY
  {
    get => this.TilePicOption != null ? this.TilePicOption.ScaleY : 100.0;
    set
    {
      if (this.TileMode != TileMode.Tile)
        throw new Exception("Tile mode must be 'TileMode.Tile' to use this 'ScaleY' property");
      if (value < 0.0 || value > 100.0)
        throw new ArgumentException("The value range of 'ScaleY' must be within 0 to 100");
      if (this._fillFormat.Parent is Background)
      {
        Background parent = this._fillFormat.Parent as Background;
        if (parent.GetFillFormat().FillType == FillType.Automatic)
          parent.SetFill((IFill) this._fillFormat);
      }
      if (this.TilePicOption == null)
      {
        TilePicOption tilePicOption = new TilePicOption();
      }
      this.TilePicOption.ScaleY = value;
    }
  }

  public RectangleAlignmentType AlignmentType
  {
    get
    {
      return this.TilePicOption != null ? this.TilePicOption.AlignmentType : RectangleAlignmentType.TopLeft;
    }
    set
    {
      if (this.TileMode != TileMode.Tile)
        throw new Exception("Tile mode must be 'TileMode.Tile' to use this 'AlignmentType' property");
      if (this._fillFormat.Parent is Background)
      {
        Background parent = this._fillFormat.Parent as Background;
        if (parent.GetFillFormat().FillType == FillType.Automatic)
          parent.SetFill((IFill) this._fillFormat);
      }
      if (this.TilePicOption == null)
      {
        TilePicOption tilePicOption = new TilePicOption();
      }
      this.TilePicOption.AlignmentType = value;
    }
  }

  public MirrorType MirrorType
  {
    get => this.TilePicOption != null ? this.TilePicOption.MirrorType : MirrorType.None;
    set
    {
      if (this.TileMode != TileMode.Tile)
        throw new Exception("Tile mode must be 'TileMode.Tile' to use this 'MirrorType' property");
      if (this._fillFormat.Parent is Background)
      {
        Background parent = this._fillFormat.Parent as Background;
        if (parent.GetFillFormat().FillType == FillType.Automatic)
          parent.SetFill((IFill) this._fillFormat);
      }
      if (this.TilePicOption == null)
      {
        TilePicOption tilePicOption = new TilePicOption();
      }
      this.TilePicOption.MirrorType = value;
    }
  }

  public int Transparency
  {
    get => this.GetPictureTransparency();
    set
    {
      if (value < 0 || value > 100)
        throw new ArgumentException("Invalid Transparency " + value.ToString());
      if (this._fillFormat.Parent is Background)
      {
        Background parent = this._fillFormat.Parent as Background;
        if (parent.GetFillFormat().FillType == FillType.Automatic)
          parent.SetFill((IFill) this._fillFormat);
      }
      this._transparency = (100 - value) * 1000;
    }
  }

  internal List<ColorObject> DuoTone => this._duoTone ?? (this._duoTone = new List<ColorObject>(2));

  internal List<ColorObject> GetDefaultDuoTone()
  {
    List<ColorObject> defaultDuoTone = new List<ColorObject>(2);
    int index = 0;
    foreach (ColorObject colorObject in this._duoTone)
    {
      if (colorObject.ThemeColorValue == "phClr")
      {
        this._duoToneForImageConversion[index].UpdateColorObject((object) this._fillFormat.Presentation);
        defaultDuoTone.Add(this._duoToneForImageConversion[index]);
        ++index;
      }
      else
      {
        colorObject.UpdateColorObject((object) this._fillFormat.Presentation);
        defaultDuoTone.Add(colorObject);
        ++index;
      }
    }
    return defaultDuoTone;
  }

  internal List<ColorObject> GetDuoToneForImageConversion()
  {
    return this._duoToneForImageConversion = new List<ColorObject>(2);
  }

  public byte[] ImageBytes
  {
    get => this._basStream;
    set
    {
      if (value == null)
        return;
      MemoryStream memoryStream = new MemoryStream(value);
      this.AddImageStream((Stream) memoryStream);
      BaseSlide baseSlide = this._fillFormat.BaseSlide;
      if (baseSlide == null || this._texturePath == null)
        return;
      string str = "ppt/" + this._texturePath.Remove(0, 3);
      string texturePath = this._texturePath;
      this._texturePath = string.Empty;
      if (baseSlide.Presentation.DataHolder.IsImageRefereceExist(baseSlide, texturePath))
      {
        this._texturePath = texturePath;
        ++baseSlide.Presentation.ImageCount;
        int startIndex = this._texturePath.Substring(this._texturePath.Length - 4) == "jpeg" ? this._texturePath.Length - 6 : this._texturePath.Length - 5;
        this._texturePath = this._texturePath.Remove(startIndex, 1);
        this._texturePath = this._texturePath.Insert(startIndex, baseSlide.Presentation.ImageCount.ToString());
        string imagePath = "ppt/" + this._texturePath.Remove(0, 3);
        baseSlide.Presentation.DataHolder.AddImageToArchive(imagePath, memoryStream);
      }
      else
      {
        this._texturePath = texturePath;
        baseSlide.Presentation.DataHolder.Archive.RemoveItem(str);
        baseSlide.Presentation.DataHolder.AddImageToArchive(str, memoryStream);
      }
    }
  }

  internal Shape BaseShape => this._baseShape;

  internal string ImageRelationId
  {
    get => this._imageRelationId;
    set => this._imageRelationId = value;
  }

  internal PicFormatOption PicFormatOption
  {
    get
    {
      return this.CheckFormatOptionIsTile() ? (PicFormatOption) null : (PicFormatOption) this._formatOption;
    }
    set
    {
      this._formatOption = (object) value;
      this._tileMode = TileMode.Stretch;
    }
  }

  internal Fill FillFormat => this._fillFormat;

  internal TilePicOption TilePicOption
  {
    get
    {
      return this.CheckFormatOptionIsTile() ? (TilePicOption) this._formatOption : (TilePicOption) null;
    }
    set
    {
      this._formatOption = (object) value;
      this._tileMode = TileMode.Tile;
    }
  }

  public byte[] Data
  {
    get
    {
      return this._basStream != null && this._basStream.Length > 0 ? (byte[]) this._basStream.Clone() : (byte[]) null;
    }
    set
    {
      if (value == null || value.Length <= 0)
        return;
      this.AddImageStream((Stream) new MemoryStream(value));
    }
  }

  public TileMode TileMode
  {
    get => this._tileMode;
    set
    {
      if (this._fillFormat.Parent is Background)
      {
        Background parent = this._fillFormat.Parent as Background;
        if (parent.GetFillFormat().FillType == FillType.Automatic)
          parent.SetFill((IFill) this._fillFormat);
      }
      this._tileMode = value;
      if (this._tileMode == TileMode.Tile && !(this._formatOption is TilePicOption))
      {
        this._formatOption = (object) new TilePicOption()
        {
          MirrorType = MirrorType.None,
          OffsetX = 0.0,
          OffsetY = 0.0,
          ScaleX = 100.0,
          ScaleY = 100.0,
          AlignmentType = RectangleAlignmentType.TopLeft
        };
      }
      else
      {
        if (this._formatOption != null && (this._tileMode == TileMode.Tile || !(this._formatOption is TilePicOption)))
          return;
        this._formatOption = (object) new PicFormatOption();
      }
    }
  }

  internal ImageFormat ImageFormat => this.ObtainImageFormat();

  internal FormatPicture FormatPicture
  {
    get => this._formatPicture ?? (this._formatPicture = new FormatPicture());
  }

  public TextureFillType Type
  {
    get => this._textureType;
    set => this._textureType = value;
  }

  internal int GetPictureTransparency() => 100 - this._transparency / 1000;

  internal bool CheckFormatOptionIsTile() => this._formatOption is TilePicOption;

  internal void AssignTransparency(int transparency) => this._transparency = transparency;

  internal void SetPictureIdAndPath()
  {
    this._texturePath = (string) null;
    this._imageRelationId = (string) null;
  }

  internal string GetImagePath() => this._texturePath;

  internal void SetImagePath(string path) => this._texturePath = path;

  internal void SetDueTone(List<ColorObject> dueTones) => this._duoTone = dueTones;

  internal List<ColorObject> ObtainDuoTone() => this._duoTone;

  internal void AddImageStream()
  {
    BaseSlide baseSlide = (BaseSlide) null;
    if (this._baseShape != null)
      baseSlide = this._baseShape.BaseSlide;
    else if (this._fillFormat.Background != null)
      baseSlide = this._fillFormat.Background.BaseSlide;
    RelationCollection relationCollection1 = (RelationCollection) null;
    if (baseSlide != null)
    {
      if (this._baseShape != null && (this._baseShape.ShapeType == ShapeType.Point || this._baseShape.ShapeType == ShapeType.Drawing))
      {
        DataModel dataModel = ((SmartArtShape) this._baseShape).ParentSmartArt.DataModel;
        relationCollection1 = this._baseShape.ShapeType != ShapeType.Drawing || dataModel.DrawingRelation == null ? dataModel.TopRelation : dataModel.DrawingRelation;
      }
      else
        relationCollection1 = baseSlide.TopRelation;
    }
    else if (this._fillFormat.Parent is Cell)
    {
      baseSlide = ((Cell) this._fillFormat.Parent).Table.BaseSlide;
      relationCollection1 = baseSlide.TopRelation;
    }
    else if (this._fillFormat.Parent is Theme)
      relationCollection1 = ((Theme) this._fillFormat.Parent).TopRelation;
    RelationCollection relationCollection2 = relationCollection1 == null ? this._fillFormat.Presentation.TopRelation : relationCollection1;
    if (string.IsNullOrEmpty(this._imageRelationId))
      return;
    string itemPathByRelation = relationCollection2.GetItemPathByRelation(this._imageRelationId);
    this._texturePath = itemPathByRelation;
    if (itemPathByRelation == null)
      return;
    string[] strArray = itemPathByRelation.Split('/');
    string imagePath = $"ppt/{strArray[strArray.Length - 2]}{(object) '/'}{strArray[strArray.Length - 1]}";
    Stream input = baseSlide != null ? baseSlide.Presentation.DataHolder.GetImageStream(imagePath) : this._fillFormat.Presentation.DataHolder.GetImageStream(imagePath);
    if (input != null && input.Length > 0L)
      this.AddImageStream(input);
    relationCollection2.GetImageRemoveList().Add(this._imageRelationId);
  }

  internal void AddImageStream(Stream input)
  {
    MemoryStream output = new MemoryStream();
    TextureFill.CopyStream(input, (Stream) output);
    this._basStream = output.ToArray();
  }

  internal void AddImageToArchive()
  {
    int imageFormat = (int) this.ObtainImageFormat();
    Syncfusion.Presentation.Presentation presentation = (Syncfusion.Presentation.Presentation) null;
    RelationCollection topRelation = (RelationCollection) null;
    this.SetPresentation(ref presentation, ref topRelation);
    string id;
    if (this._imageRelationId == null)
    {
      id = Helper.GenerateRelationId(topRelation);
      this.ImageRelationId = id;
    }
    else
      id = this._imageRelationId;
    Relation relation;
    if (this._texturePath == null)
    {
      ++presentation.ImageCount;
      this._texturePath = $"../media/image{(object) presentation.ImageCount}{this._imageExtension}";
      relation = new Relation(id, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image", this._texturePath, (string) null);
      MemoryStream memoryStream = new MemoryStream(this._basStream);
      presentation.DataHolder.AddImage("ppt" + this._texturePath.Remove(0, 2), (Stream) memoryStream, relation);
    }
    else
    {
      relation = new Relation(id, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image", this._texturePath, (string) null);
      ++presentation.ImageCount;
    }
    if (topRelation.Contains(id))
      topRelation.Update(relation);
    else
      topRelation.Add(relation.Id, relation);
    string partName = this._imageExtension.Replace(".", "");
    presentation.AddDefaultContentType(partName, "image/" + partName);
  }

  private void SetPresentation(ref Syncfusion.Presentation.Presentation presentation, ref RelationCollection topRelation)
  {
    BaseSlide baseSlide = (BaseSlide) null;
    if (this._baseShape != null)
      baseSlide = this._baseShape.BaseSlide;
    else if (this._fillFormat.Background != null)
      baseSlide = this._fillFormat.Background.BaseSlide;
    else if (this._fillFormat.Parent is Theme)
    {
      Theme parent = (Theme) this._fillFormat.Parent;
      presentation = parent.BaseSlide.Presentation;
      topRelation = parent.TopRelation;
    }
    else
      baseSlide = this._fillFormat.BaseSlide;
    if (presentation != null && topRelation != null)
      return;
    if (this._baseShape != null && this._baseShape.ShapeType == ShapeType.Point)
    {
      DataModel dataModel = ((SmartArtShape) this._baseShape).ParentSmartArt.DataModel;
      topRelation = dataModel.TopRelation;
      presentation = baseSlide.Presentation;
    }
    else if (baseSlide != null)
    {
      presentation = baseSlide.Presentation;
      topRelation = baseSlide.TopRelation;
    }
    else
    {
      presentation = this._fillFormat.Presentation;
      topRelation = presentation.TopRelation;
    }
  }

  private ImageFormat ObtainImageFormat()
  {
    byte[] data = this.Data;
    if (data == null || data.Length <= 0)
      return ImageFormat.Unknown;
    ImageFormat rawFormat = Image.FromStream((Stream) new MemoryStream(data, false)).RawFormat;
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

  private static void CopyStream(Stream input, Stream output)
  {
    if (input == null)
      throw new ArgumentNullException(nameof (input));
    if (output == null)
      throw new ArgumentNullException(nameof (output));
    input.Position = 0L;
    input.CopyTo(output);
  }

  internal void Close() => this.ClearAll();

  private void ClearAll()
  {
    if (this._formatOption != null)
      this._formatOption = (object) null;
    if (this._duoTone != null)
    {
      foreach (ColorObject colorObject in this._duoTone)
        colorObject.Close();
      this._duoTone.Clear();
      this._duoTone = (List<ColorObject>) null;
    }
    this._fillFormat = (Fill) null;
    this._baseShape = (Shape) null;
  }

  public TextureFill Clone()
  {
    TextureFill textureFill = (TextureFill) this.MemberwiseClone();
    textureFill._basStream = Helper.CloneByteArray(this._basStream);
    if (this._duoTone != null)
      textureFill._duoTone = Helper.CloneColorObjectList(this._duoTone);
    if (this._formatOption != null)
    {
      if (this._formatOption is TilePicOption formatOption1)
        textureFill._formatOption = (object) formatOption1.Clone();
      if (this._formatOption is PicFormatOption formatOption2)
        textureFill._formatOption = (object) formatOption2.Clone();
      if (textureFill._texturePath != null)
        textureFill._texturePath = (string) null;
    }
    return textureFill;
  }

  internal void SetParent(Fill fill) => this._fillFormat = fill;

  internal void SetParent(Shape shape) => this._baseShape = shape;

  internal int ObtainTransparency() => this._transparency;
}
