// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Collections.PicturesCollection
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Shapes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Collections;

public class PicturesCollection : 
  CollectionBaseEx<IPictureShape>,
  IPictures,
  IParentApplication,
  IEnumerable
{
  private string DEF_PICTURE_NAME = "Picture";
  private string[] m_indexedpixel_notsupport = new string[5]
  {
    "Format1bppIndexed",
    "Format4bppIndexed",
    "Format8bppIndexed",
    "b96b3cac-0728-11d3-9d7b-0000f81ef32e",
    "b96b3cad-0728-11d3-9d7b-0000f81ef32e"
  };
  private WorksheetBaseImpl m_sheet;

  public IPictureShape this[string name]
  {
    get
    {
      IPictureShape pictureShape1 = (IPictureShape) null;
      int i = 0;
      for (int count = this.Count; i < count; ++i)
      {
        IPictureShape pictureShape2 = this[i];
        if (pictureShape2.Name == name)
        {
          pictureShape1 = pictureShape2;
          break;
        }
      }
      return pictureShape1;
    }
  }

  public IPictureShape AddPicture(Image image, string pictureName)
  {
    return this.AddPicture(image, pictureName, ExcelImageFormat.Original);
  }

  public IPictureShape AddPicture(Image image, string pictureName, ExcelImageFormat imageFormat)
  {
    return this.m_sheet.Shapes.AddPicture(image, pictureName, imageFormat);
  }

  public IPictureShape AddPicture(string strFileName)
  {
    return this.AddPicture(strFileName, ExcelImageFormat.Original);
  }

  public IPictureShape AddPicture(string strFileName, ExcelImageFormat imageFormat)
  {
    return this.m_sheet.Shapes.AddPicture(strFileName);
  }

  public IPictureShape AddPicture(int topRow, int leftColumn, Image image)
  {
    return this.AddPicture(topRow, leftColumn, image, ExcelImageFormat.Original);
  }

  public IPictureShape AddPicture(
    int topRow,
    int leftColumn,
    Image image,
    ExcelImageFormat imageFormat)
  {
    BitmapShapeImpl bitmapShapeImpl = image != null ? (BitmapShapeImpl) this.AddPicture(image, this.GeneratePictureName(), imageFormat) : throw new ArgumentNullException(nameof (image));
    bitmapShapeImpl.LeftColumn = leftColumn;
    bitmapShapeImpl.TopRow = topRow;
    bitmapShapeImpl.EvaluateTopLeftPosition();
    return (IPictureShape) bitmapShapeImpl;
  }

  public IPictureShape AddPicture(int topRow, int leftColumn, Stream stream)
  {
    return this.AddPicture(topRow, leftColumn, stream, ExcelImageFormat.Original);
  }

  public IPictureShape AddPicture(
    int topRow,
    int leftColumn,
    Stream svgStream,
    Stream imageStream)
  {
    BitmapShapeImpl bitmapShapeImpl = (BitmapShapeImpl) this.AddPicture(topRow, leftColumn, imageStream, ExcelImageFormat.Original);
    bitmapShapeImpl.SvgData = svgStream;
    return (IPictureShape) bitmapShapeImpl;
  }

  public IPictureShape AddPicture(
    int topRow,
    int leftColumn,
    Stream svgStream,
    Stream imageStream,
    int scaleWidth,
    int scaleHeight)
  {
    BitmapShapeImpl bitmapShapeImpl = (BitmapShapeImpl) this.AddPicture(topRow, leftColumn, imageStream, ExcelImageFormat.Original);
    bitmapShapeImpl.SvgData = svgStream;
    bitmapShapeImpl.Scale(scaleWidth, scaleHeight);
    return (IPictureShape) bitmapShapeImpl;
  }

  public IPictureShape AddPictureAsLink(
    int topRow,
    int leftColumn,
    int bottomRow,
    int rightColumn,
    string url)
  {
    if (!Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
      throw new ArgumentException("This is not a valid URL");
    BitmapShapeImpl bitmapShapeImpl = (this.m_sheet.Shapes as ShapesCollection).AddPictureAsLink(this.GeneratePictureName());
    bitmapShapeImpl.LeftColumn = leftColumn;
    bitmapShapeImpl.TopRow = topRow;
    bitmapShapeImpl.EvaluateTopLeftPosition();
    bitmapShapeImpl.RightColumn = rightColumn;
    bitmapShapeImpl.BottomRow = bottomRow;
    bitmapShapeImpl.UpdateHeight();
    bitmapShapeImpl.UpdateWidth();
    bitmapShapeImpl.ClearShapeOffset(true);
    bitmapShapeImpl.ExternalLink = url;
    return (IPictureShape) bitmapShapeImpl;
  }

  public IPictureShape AddPicture(
    int topRow,
    int leftColumn,
    Stream stream,
    ExcelImageFormat imageFormat)
  {
    Image image = stream != null ? ApplicationImpl.CreateImage(stream) : throw new ArgumentNullException(nameof (stream));
    return this.AddPicture(topRow, leftColumn, image, imageFormat);
  }

  public IPictureShape AddPicture(int topRow, int leftColumn, string fileName)
  {
    return this.AddPicture(topRow, leftColumn, fileName, ExcelImageFormat.Original);
  }

  public IPictureShape AddPicture(
    int topRow,
    int leftColumn,
    string fileName,
    ExcelImageFormat imageFormat)
  {
    switch (fileName)
    {
      case null:
        throw new ArgumentNullException("pictureName");
      case "":
        throw new ArgumentException("pictureName can't be empty");
      default:
        Image image = ApplicationImpl.CreateImage((Stream) new MemoryStream(File.ReadAllBytes(fileName)));
        IPictureShape pictureShape = this.AddPicture(topRow, leftColumn, image, imageFormat);
        pictureShape.Name = Path.GetFileNameWithoutExtension(fileName);
        return pictureShape;
    }
  }

  public IPictureShape AddPicture(
    int topRow,
    int leftColumn,
    int bottomRow,
    int rightColumn,
    Image image)
  {
    if (image == null)
      throw new ArgumentNullException(nameof (image));
    return this.AddPicture(topRow, leftColumn, bottomRow, rightColumn, image, ExcelImageFormat.Original);
  }

  public IPictureShape AddPicture(
    int topRow,
    int leftColumn,
    int bottomRow,
    int rightColumn,
    Image image,
    ExcelImageFormat imageFormat)
  {
    if (image == null)
      throw new ArgumentNullException(nameof (image));
    BitmapShapeImpl bitmapShapeImpl = (BitmapShapeImpl) this.AddPicture(topRow, leftColumn, image, imageFormat);
    bitmapShapeImpl.RightColumn = rightColumn;
    bitmapShapeImpl.BottomRow = bottomRow;
    bitmapShapeImpl.UpdateHeight();
    bitmapShapeImpl.UpdateWidth();
    bitmapShapeImpl.ClearShapeOffset(true);
    return (IPictureShape) bitmapShapeImpl;
  }

  public IPictureShape AddPicture(
    int topRow,
    int leftColumn,
    int bottomRow,
    int rightColumn,
    Stream stream)
  {
    return this.AddPicture(topRow, leftColumn, bottomRow, rightColumn, stream, ExcelImageFormat.Original);
  }

  public IPictureShape AddPicture(
    int topRow,
    int leftColumn,
    int bottomRow,
    int rightColumn,
    Stream stream,
    ExcelImageFormat imageFormat)
  {
    Image image = stream != null ? ApplicationImpl.CreateImage(stream) : throw new ArgumentNullException(nameof (stream));
    return this.AddPicture(topRow, leftColumn, bottomRow, rightColumn, image, imageFormat);
  }

  public IPictureShape AddPicture(
    int topRow,
    int leftColumn,
    int bottomRow,
    int rightColumn,
    string fileName)
  {
    return this.AddPicture(topRow, leftColumn, bottomRow, rightColumn, fileName, ExcelImageFormat.Original);
  }

  public IPictureShape AddPicture(
    int topRow,
    int leftColumn,
    int bottomRow,
    int rightColumn,
    string fileName,
    ExcelImageFormat imageFormat)
  {
    switch (fileName)
    {
      case null:
        throw new ArgumentNullException("pictureName");
      case "":
        throw new ArgumentException("pictureName can't be empty.");
      default:
        Image image = Image.FromFile(fileName);
        IPictureShape pictureShape = this.AddPicture(topRow, leftColumn, bottomRow, rightColumn, image);
        pictureShape.Name = Path.GetFileNameWithoutExtension(fileName);
        if (Array.IndexOf<string>(this.m_indexedpixel_notsupport, image.RawFormat.Guid.ToString()) == -1)
          image.Dispose();
        return pictureShape;
    }
  }

  public IPictureShape AddPicture(
    int topRow,
    int leftColumn,
    Image image,
    int scaleWidth,
    int scaleHeight)
  {
    return this.AddPicture(topRow, leftColumn, image, scaleWidth, scaleHeight, ExcelImageFormat.Original);
  }

  public IPictureShape AddPicture(
    int topRow,
    int leftColumn,
    Image image,
    int scaleWidth,
    int scaleHeight,
    ExcelImageFormat imageFormat)
  {
    if (image == null)
      throw new ArgumentNullException(nameof (image));
    IPictureShape pictureShape = this.AddPicture(topRow, leftColumn, image, imageFormat);
    pictureShape.Scale(scaleWidth, scaleHeight);
    return pictureShape;
  }

  public IPictureShape AddPicture(
    int topRow,
    int leftColumn,
    Stream stream,
    int scaleWidth,
    int scaleHeight)
  {
    return this.AddPicture(topRow, leftColumn, stream, scaleWidth, scaleHeight, ExcelImageFormat.Original);
  }

  public IPictureShape AddPicture(
    int topRow,
    int leftColumn,
    Stream stream,
    int scaleWidth,
    int scaleHeight,
    ExcelImageFormat imageFormat)
  {
    Image image = ApplicationImpl.CreateImage(stream);
    IPictureShape pictureShape = this.AddPicture(topRow, leftColumn, image, imageFormat);
    pictureShape.Scale(scaleWidth, scaleHeight);
    return pictureShape;
  }

  public IPictureShape AddPicture(
    int topRow,
    int leftColumn,
    string fileName,
    int scaleWidth,
    int scaleHeight)
  {
    return this.AddPicture(topRow, leftColumn, fileName, scaleWidth, scaleHeight, ExcelImageFormat.Original);
  }

  public IPictureShape AddPicture(
    int topRow,
    int leftColumn,
    string fileName,
    int scaleWidth,
    int scaleHeight,
    ExcelImageFormat imageFormat)
  {
    switch (fileName)
    {
      case null:
        throw new ArgumentNullException("pictureName");
      case "":
        throw new ArgumentException("pictureName can't be empty.");
      default:
        IPictureShape pictureShape = this.AddPicture(topRow, leftColumn, fileName, imageFormat);
        pictureShape.Scale(scaleWidth, scaleHeight);
        return pictureShape;
    }
  }

  public PicturesCollection(IApplication application, object parent)
    : base(application, parent)
  {
    this.SetParents();
  }

  internal void RemovePicture(IPictureShape picture) => this.InnerList.Remove(picture);

  internal void AddPicture(IPictureShape picture) => this.InnerList.Add(picture);

  private void SetParents()
  {
    this.m_sheet = this.FindParent(typeof (WorksheetBaseImpl), true) as WorksheetBaseImpl;
    if (this.m_sheet == null)
      throw new ArgumentNullException("Can't find parent worksheet.");
  }

  private string GeneratePictureName()
  {
    return CollectionBaseEx<IPictureShape>.GenerateDefaultName((ICollection<IPictureShape>) this, this.DEF_PICTURE_NAME);
  }
}
