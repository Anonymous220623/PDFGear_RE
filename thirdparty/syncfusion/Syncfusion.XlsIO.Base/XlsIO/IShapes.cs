// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IShapes
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO;

public interface IShapes : IParentApplication, IEnumerable
{
  IPictureShape AddPicture(Image image, string pictureName, ExcelImageFormat imageFormat);

  IPictureShape AddPicture(string fileName);

  ICommentShape AddComment(string commentText, bool bIsParseOptions);

  ICommentShape AddComment(string commentText);

  ICommentShape AddComment();

  IChartShape AddChart();

  IShape AddCopy(IShape sourceShape);

  IShape AddCopy(
    IShape sourceShape,
    Dictionary<string, string> hashNewNames,
    List<int> arrFontIndexes);

  ITextBoxShapeEx AddTextBox();

  ICheckBoxShape AddCheckBox();

  IOptionButtonShape AddOptionButton();

  IComboBoxShape AddComboBox();

  IShape AddAutoShapes(
    AutoShapeType autoShapeType,
    int topRow,
    int leftColumn,
    int height,
    int width);

  IGroupShape Group(IShape[] groupItems);

  void Ungroup(IGroupShape groupShape);

  void Ungroup(IGroupShape groupShape, bool isAll);

  int Count { get; }

  IShape this[int index] { get; }

  IShape this[string strShapeName] { get; }
}
