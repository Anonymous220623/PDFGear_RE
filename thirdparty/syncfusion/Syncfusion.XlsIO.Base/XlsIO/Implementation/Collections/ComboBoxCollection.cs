// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Collections.ComboBoxCollection
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing;
using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Collections;

public class ComboBoxCollection : CollectionBaseEx<IComboBoxShape>, IComboBoxes
{
  private WorksheetBaseImpl m_sheet;

  public IComboBoxShape AddComboBox(int row, int column, int height, int width)
  {
    ComboBoxShapeImpl comboBoxShapeImpl = this.m_sheet.Shapes.AddComboBox() as ComboBoxShapeImpl;
    MsofbtClientAnchor clientAnchor = comboBoxShapeImpl.ClientAnchor;
    clientAnchor.LeftColumn = column - 1;
    clientAnchor.TopRow = row - 1;
    clientAnchor.RightColumn = column;
    clientAnchor.BottomRow = row;
    clientAnchor.LeftOffset = 0;
    clientAnchor.RightOffset = 0;
    clientAnchor.TopOffset = 0;
    clientAnchor.BottomOffset = 0;
    comboBoxShapeImpl.Width = width;
    comboBoxShapeImpl.Height = height;
    comboBoxShapeImpl.EvaluateTopLeftPosition();
    return (IComboBoxShape) comboBoxShapeImpl;
  }

  public IComboBoxShape this[string name]
  {
    get
    {
      IComboBoxShape comboBoxShape1 = (IComboBoxShape) null;
      int i = 0;
      for (int count = this.Count; i < count; ++i)
      {
        IComboBoxShape comboBoxShape2 = this[i];
        if (comboBoxShape2.Name == name)
        {
          comboBoxShape1 = comboBoxShape2;
          break;
        }
      }
      return comboBoxShape1;
    }
  }

  public ComboBoxCollection(IApplication application, object parent)
    : base(application, parent)
  {
    this.m_sheet = this.FindParent(typeof (WorksheetBaseImpl), true) as WorksheetBaseImpl;
    if (this.m_sheet == null)
      throw new ArgumentOutOfRangeException(nameof (parent));
  }

  public void AddComboBox(IComboBoxShape combobox)
  {
    if (combobox == null)
      throw new ArgumentNullException(nameof (combobox));
    this.Add(combobox);
  }
}
