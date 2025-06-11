// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Collections.CheckBoxCollection
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing;
using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Collections;

public class CheckBoxCollection : CollectionBaseEx<object>, ICheckBoxes
{
  private WorksheetBaseImpl m_sheet;

  public CheckBoxCollection(IApplication application, object parent)
    : base(application, parent)
  {
    this.m_sheet = this.FindParent(typeof (WorksheetBaseImpl), true) as WorksheetBaseImpl;
    if (this.m_sheet == null)
      throw new ArgumentOutOfRangeException(nameof (parent));
  }

  public void AddCheckBox(ICheckBoxShape checkbox)
  {
    if (checkbox == null)
      throw new ArgumentNullException(nameof (checkbox));
    this.Add((object) checkbox);
  }

  public ICheckBoxShape this[int index] => this.List[index] as ICheckBoxShape;

  public ICheckBoxShape this[string name]
  {
    get
    {
      ICheckBoxShape checkBoxShape1 = (ICheckBoxShape) null;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        ICheckBoxShape checkBoxShape2 = this[index];
        if (checkBoxShape2.Name == name)
        {
          checkBoxShape1 = checkBoxShape2;
          break;
        }
      }
      return checkBoxShape1;
    }
  }

  public ICheckBoxShape AddCheckBox(int row, int column, int height, int width)
  {
    CheckBoxShapeImpl checkBoxShapeImpl = this.m_sheet.Shapes.AddCheckBox() as CheckBoxShapeImpl;
    MsofbtClientAnchor clientAnchor = checkBoxShapeImpl.ClientAnchor;
    clientAnchor.LeftColumn = column - 1;
    clientAnchor.TopRow = row - 1;
    clientAnchor.RightColumn = column;
    clientAnchor.BottomRow = row;
    clientAnchor.LeftOffset = 0;
    clientAnchor.RightOffset = 0;
    clientAnchor.TopOffset = 0;
    clientAnchor.BottomOffset = 0;
    checkBoxShapeImpl.Fill.BackColor = ColorExtension.Empty;
    checkBoxShapeImpl.Line.BackColor = ColorExtension.White;
    checkBoxShapeImpl.HasLineFormat = false;
    checkBoxShapeImpl.HasFill = false;
    checkBoxShapeImpl.Width = width;
    checkBoxShapeImpl.Height = height;
    checkBoxShapeImpl.EvaluateTopLeftPosition();
    return (ICheckBoxShape) checkBoxShapeImpl;
  }
}
