// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IPictures
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO;

public interface IPictures : IParentApplication, IEnumerable
{
  int Count { get; }

  IPictureShape this[int Index] { get; }

  IPictureShape this[string name] { get; }

  IPictureShape AddPicture(Image image, string pictureName);

  IPictureShape AddPicture(Image image, string pictureName, ExcelImageFormat imageFormat);

  IPictureShape AddPicture(string strFileName);

  IPictureShape AddPicture(string strFileName, ExcelImageFormat imageFormat);

  IPictureShape AddPicture(int topRow, int leftColumn, Image image);

  IPictureShape AddPicture(int topRow, int leftColumn, Image image, ExcelImageFormat imageFormat);

  IPictureShape AddPicture(int topRow, int leftColumn, Stream stream);

  IPictureShape AddPicture(int topRow, int leftColumn, Stream svgStream, Stream imageStream);

  IPictureShape AddPicture(
    int topRow,
    int leftColumn,
    Stream svgStream,
    Stream imageStream,
    int scaleWidth,
    int scaleHeight);

  IPictureShape AddPicture(
    int topRow,
    int leftColumn,
    Stream stream,
    ExcelImageFormat imageFormat);

  IPictureShape AddPictureAsLink(
    int topRow,
    int leftColumn,
    int bottomRow,
    int rightColumn,
    string url);

  IPictureShape AddPicture(int topRow, int leftColumn, string fileName);

  IPictureShape AddPicture(
    int topRow,
    int leftColumn,
    string fileName,
    ExcelImageFormat imageFormat);

  IPictureShape AddPicture(
    int topRow,
    int leftColumn,
    int bottomRow,
    int rightColumn,
    Image image);

  IPictureShape AddPicture(
    int topRow,
    int leftColumn,
    int bottomRow,
    int rightColumn,
    Image image,
    ExcelImageFormat imageFormat);

  IPictureShape AddPicture(
    int topRow,
    int leftColumn,
    int bottomRow,
    int rightColumn,
    Stream stream);

  IPictureShape AddPicture(
    int topRow,
    int leftColumn,
    int bottomRow,
    int rightColumn,
    Stream stream,
    ExcelImageFormat imageFormat);

  IPictureShape AddPicture(
    int topRow,
    int leftColumn,
    int bottomRow,
    int rightColumn,
    string fileName);

  IPictureShape AddPicture(
    int topRow,
    int leftColumn,
    int bottomRow,
    int rightColumn,
    string fileName,
    ExcelImageFormat imageFormat);

  IPictureShape AddPicture(
    int topRow,
    int leftColumn,
    Image image,
    int scaleWidth,
    int scaleHeight);

  IPictureShape AddPicture(
    int topRow,
    int leftColumn,
    Image image,
    int scaleWidth,
    int scaleHeight,
    ExcelImageFormat imageFormat);

  IPictureShape AddPicture(
    int topRow,
    int leftColumn,
    Stream stream,
    int scaleWidth,
    int scaleHeight);

  IPictureShape AddPicture(
    int topRow,
    int leftColumn,
    Stream stream,
    int scaleWidth,
    int scaleHeight,
    ExcelImageFormat imageFormat);

  IPictureShape AddPicture(
    int topRow,
    int leftColumn,
    string fileName,
    int scaleWidth,
    int scaleHeight);

  IPictureShape AddPicture(
    int topRow,
    int leftColumn,
    string fileName,
    int scaleWidth,
    int scaleHeight,
    ExcelImageFormat imageFormat);
}
