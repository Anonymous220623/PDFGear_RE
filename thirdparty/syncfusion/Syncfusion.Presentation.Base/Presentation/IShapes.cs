// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.IShapes
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.Presentation;

public interface IShapes : IEnumerable<ISlideItem>, IEnumerable
{
  int Count { get; }

  ISlideItem this[int index] { get; }

  int Add(ISlideItem shape);

  void Insert(int index, ISlideItem value);

  void RemoveAt(int index);

  void Remove(ISlideItem item);

  int IndexOf(ISlideItem value);

  void Clear();

  IPresentationChart AddChart(double left, double top, double width, double height);

  IPresentationChart AddChart(
    Stream excelStream,
    int sheetNumber,
    string dataRange,
    RectangleF bounds);

  IPresentationChart AddChart(
    object[][] data,
    double left,
    double top,
    double width,
    double height);

  IPresentationChart AddChart(
    IEnumerable enumerable,
    double left,
    double top,
    double width,
    double height);

  IPicture AddPicture(Stream pictureStream, double left, double top, double width, double height);

  IPicture AddPicture(
    Stream svgStream,
    Stream imageStream,
    double left,
    double top,
    double width,
    double height);

  IShape AddShape(AutoShapeType type, double left, double top, double width, double height);

  IConnector AddConnector(
    ConnectorType connectorType,
    IShape beginShape,
    int beginSiteIndex,
    IShape endShape,
    int endSiteIndex);

  IConnector AddConnector(
    ConnectorType connectorType,
    double beginX,
    double beginY,
    double endX,
    double endY);

  ISmartArt AddSmartArt(
    SmartArtType smartArtType,
    double left,
    double top,
    double width,
    double height);

  IOleObject AddOleObject(Stream image, string progId, Stream oleData);

  IOleObject AddOleObject(Stream image, string progId, string pathLink);

  IGroupShape AddGroupShape(double left, double top, double width, double height);

  ITable AddTable(
    int rowCount,
    int columnCount,
    double left,
    double top,
    double width,
    double height);

  IShape AddTextBox(double left, double top, double width, double height);
}
