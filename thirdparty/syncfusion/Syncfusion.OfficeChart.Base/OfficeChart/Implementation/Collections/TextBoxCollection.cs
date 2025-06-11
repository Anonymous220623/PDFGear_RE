// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Collections.TextBoxCollection
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Charts;
using Syncfusion.OfficeChart.Implementation.Shapes;
using Syncfusion.OfficeChart.Parser.Biff_Records.Charts;
using Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing;
using System;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Collections;

internal class TextBoxCollection : CollectionBaseEx<ITextBoxShape>, ITextBoxes
{
  private WorksheetBaseImpl m_sheet;

  public TextBoxCollection(IApplication application, object parent)
    : base(application, parent)
  {
    this.m_sheet = this.FindParent(typeof (WorksheetBaseImpl), true) as WorksheetBaseImpl;
    if (this.m_sheet == null)
      throw new ArgumentOutOfRangeException(nameof (parent));
  }

  public void AddTextBox(ITextBoxShape textbox)
  {
    if (textbox == null)
      throw new ArgumentNullException(nameof (textbox));
    this.Add(textbox);
  }

  public new ITextBoxShape this[int index] => this.List[index];

  public ITextBoxShape this[string name]
  {
    get
    {
      ITextBoxShape textBoxShape1 = (ITextBoxShape) null;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        ITextBoxShape textBoxShape2 = this[index];
        if (textBoxShape2.Name == name)
        {
          textBoxShape1 = textBoxShape2;
          break;
        }
      }
      return textBoxShape1;
    }
  }

  public ITextBoxShape AddTextBox(int row, int column, int height, int width)
  {
    TextBoxShapeImpl textBoxShapeImpl = this.m_sheet.Shapes.AddTextBox() as TextBoxShapeImpl;
    MsofbtClientAnchor clientAnchor = textBoxShapeImpl.ClientAnchor;
    clientAnchor.LeftColumn = column - 1;
    clientAnchor.TopRow = row - 1;
    clientAnchor.RightColumn = column;
    clientAnchor.BottomRow = row;
    clientAnchor.LeftOffset = 0;
    clientAnchor.RightOffset = 0;
    clientAnchor.TopOffset = 0;
    clientAnchor.BottomOffset = 0;
    textBoxShapeImpl.Width = width;
    textBoxShapeImpl.Height = height;
    if (this.Parent is ChartImpl parent)
    {
      ChartParentAxisImpl chartParentAxisImpl = parent.PrimaryParentAxis != null ? parent.PrimaryParentAxis : parent.SecondaryParentAxis;
      if (chartParentAxisImpl != null)
      {
        ChartAxisParentRecord parentAxisRecord = chartParentAxisImpl.ParentAxisRecord;
        if (parentAxisRecord.XAxisLength == 0)
        {
          parentAxisRecord.TopLeftX = 328;
          parentAxisRecord.TopLeftY = 243;
          parentAxisRecord.XAxisLength = 3125;
          parentAxisRecord.YAxisLength = 3283;
        }
      }
    }
    return (ITextBoxShape) textBoxShapeImpl;
  }
}
