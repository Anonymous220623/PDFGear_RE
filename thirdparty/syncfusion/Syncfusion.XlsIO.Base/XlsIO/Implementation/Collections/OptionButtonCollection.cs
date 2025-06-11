// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Collections.OptionButtonCollection
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing;
using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Collections;

public class OptionButtonCollection : CollectionBaseEx<object>, IOptionButtons
{
  public const int AverageWidth = 140;
  public const int AverageHeight = 20;
  private WorksheetBaseImpl m_worksheet;

  public OptionButtonCollection(IApplication application, object parent)
    : base(application, parent)
  {
    this.m_worksheet = this.FindParent(typeof (WorksheetBaseImpl), true) as WorksheetBaseImpl;
    if (this.m_worksheet == null)
      throw new ArgumentOutOfRangeException(nameof (parent));
  }

  public void AddOptionButton(IOptionButtonShape optionButton)
  {
    if (optionButton == null)
      throw new ArgumentNullException("option button");
    this.AddDefaultEvents(optionButton);
    this.Add((object) optionButton);
  }

  internal void AddDefaultEvents(IOptionButtonShape optionButton)
  {
    OptionButtonShapeImpl optionButtonShapeImpl = (OptionButtonShapeImpl) optionButton;
    optionButtonShapeImpl.InvokeEvent = true;
    optionButtonShapeImpl.CheckStateChanged += new ValueChangedEventHandler(this.OptionButtonCheckStateChanged);
    optionButtonShapeImpl.LinkedCellValueChanged += new ValueChangedEventHandler(this.OptionButtonLinkedCellChanged);
  }

  private void OptionButtonCheckStateChanged(
    object obj,
    ValueChangedEventArgs valueChangedEventArgs)
  {
    IOptionButtonShape optionButtonShape = (IOptionButtonShape) obj;
    System.Collections.Generic.List<OptionButtonShapeImpl> optionButtonShapeImplList = new System.Collections.Generic.List<OptionButtonShapeImpl>();
    int num1 = (int) (obj as OptionButtonShapeImpl).OldObjId;
    int index1 = 0;
    bool flag1 = true;
    while (flag1)
    {
      OptionButtonShapeImpl optionButtonShapeImpl = (OptionButtonShapeImpl) this[index1];
      if (optionButtonShapeImplList.Contains(optionButtonShapeImpl))
        flag1 = false;
      if (flag1)
      {
        if ((long) optionButtonShapeImpl.OldObjId == (long) num1)
        {
          optionButtonShapeImplList.Add(optionButtonShapeImpl);
          num1 = optionButtonShapeImpl.NextButtonId;
        }
        ++index1;
        if (index1 == this.Count)
          index1 = 0;
      }
    }
    IRange range = (IRange) null;
    int num2 = 0;
    int index2 = 0;
    for (int count = this.Count; index2 < count; ++index2)
    {
      OptionButtonShapeImpl optionButtonShapeImpl = (OptionButtonShapeImpl) this[index2];
      if (optionButtonShapeImplList.Contains(optionButtonShapeImpl))
      {
        optionButtonShapeImpl.InvokeEvent = false;
        bool flag2 = false;
        if (optionButtonShapeImpl.IsFirstButton && optionButtonShapeImpl.LinkedCell != null || range != null)
        {
          if (optionButtonShapeImpl.IsFirstButton)
          {
            optionButtonShape = (IOptionButtonShape) optionButtonShapeImpl;
            range = (IRange) null;
            num2 = 0;
          }
          if (range == null)
            range = this[index2].LinkedCell;
          flag2 = true;
          optionButtonShapeImpl.LinkedCell = range;
          ++num2;
        }
        if (optionButtonShapeImpl != obj)
          optionButtonShapeImpl.CheckState = ExcelCheckState.Unchecked;
        else if (flag2 && (obj as OptionButtonShapeImpl).CheckState == ExcelCheckState.Checked)
          optionButtonShape.LinkedCell.Number = (double) num2;
        optionButtonShapeImpl.InvokeEvent = true;
      }
    }
    optionButtonShapeImplList.Clear();
  }

  private void OptionButtonLinkedCellChanged(
    object obj,
    ValueChangedEventArgs valueChangedEventArgs)
  {
    IRange newValue = (IRange) valueChangedEventArgs.newValue;
    int index = 0;
    for (int count = this.Count; index < count; ++index)
    {
      OptionButtonShapeImpl optionButtonShapeImpl = (OptionButtonShapeImpl) this[index];
      optionButtonShapeImpl.InvokeEvent = false;
      optionButtonShapeImpl.LinkedCell = newValue;
      if (optionButtonShapeImpl.CheckState == ExcelCheckState.Checked && newValue != null)
        newValue.Number = (double) (index + 1);
      optionButtonShapeImpl.InvokeEvent = true;
    }
  }

  internal void PrepareForSerialization()
  {
    foreach (OptionButtonCollection optionGroup in this.GetOptionGroups())
    {
      IRange linkedCell = optionGroup[0].LinkedCell;
      int num = linkedCell != null ? (int) linkedCell.Number : optionGroup.Count + 1;
      if (num == 0)
      {
        foreach (OptionButtonShapeImpl optionButtonShapeImpl in (CollectionBase<object>) optionGroup)
        {
          optionButtonShapeImpl.InvokeEvent = false;
          optionButtonShapeImpl.CheckState = ExcelCheckState.Unchecked;
          optionButtonShapeImpl.InvokeEvent = true;
        }
      }
      else if (num > 0 && num <= optionGroup.Count)
      {
        OptionButtonShapeImpl optionButtonShapeImpl = optionGroup[num - 1] as OptionButtonShapeImpl;
        optionButtonShapeImpl.InvokeEvent = false;
        optionButtonShapeImpl.CheckState = ExcelCheckState.Checked;
        optionButtonShapeImpl.InvokeEvent = true;
      }
    }
  }

  internal System.Collections.Generic.List<OptionButtonCollection> GetOptionGroups()
  {
    System.Collections.Generic.List<OptionButtonCollection> optionGroups = new System.Collections.Generic.List<OptionButtonCollection>();
    int num = -1;
    OptionButtonCollection buttonCollection = new OptionButtonCollection(this.Application, this.Parent);
    foreach (OptionButtonShapeImpl optionButtonShapeImpl in (CollectionBase<object>) this)
    {
      buttonCollection.Add((object) optionButtonShapeImpl);
      if (num == -1)
        num = optionButtonShapeImpl.Index;
      else if (num == optionButtonShapeImpl.NextButtonId)
      {
        num = -1;
        optionGroups.Add(buttonCollection);
        buttonCollection = new OptionButtonCollection(this.Application, this.Parent);
      }
    }
    return optionGroups;
  }

  public IOptionButtonShape this[int index] => this.List[index] as IOptionButtonShape;

  public IOptionButtonShape this[string name]
  {
    get
    {
      IOptionButtonShape optionButtonShape1 = (IOptionButtonShape) null;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        IOptionButtonShape optionButtonShape2 = this[index];
        if (optionButtonShape2.Name == name)
        {
          optionButtonShape1 = optionButtonShape2;
          break;
        }
      }
      return optionButtonShape1;
    }
  }

  public IOptionButtonShape AddOptionButton(int row, int column, int height, int width)
  {
    OptionButtonShapeImpl optionButtonShapeImpl = this.m_worksheet.Shapes.AddOptionButton() as OptionButtonShapeImpl;
    MsofbtClientAnchor clientAnchor = optionButtonShapeImpl.ClientAnchor;
    clientAnchor.LeftColumn = column - 1;
    clientAnchor.TopRow = row - 1;
    clientAnchor.RightColumn = column;
    clientAnchor.BottomRow = row;
    clientAnchor.LeftOffset = 0;
    clientAnchor.RightOffset = 0;
    clientAnchor.TopOffset = 0;
    clientAnchor.BottomOffset = 0;
    optionButtonShapeImpl.Workbook.CreateFont();
    optionButtonShapeImpl.Fill.BackColor = ColorExtension.Empty;
    optionButtonShapeImpl.Fill.ForeColor = ColorExtension.Empty;
    optionButtonShapeImpl.Line.BackColor = ColorExtension.White;
    optionButtonShapeImpl.HasLineFormat = false;
    optionButtonShapeImpl.HasFill = false;
    optionButtonShapeImpl.Width = width;
    optionButtonShapeImpl.Height = height;
    optionButtonShapeImpl.EvaluateTopLeftPosition();
    if (this.Count == 1)
      optionButtonShapeImpl.IsFirstButton = true;
    return (IOptionButtonShape) optionButtonShapeImpl;
  }

  public IOptionButtonShape AddOptionButton() => this.AddOptionButton(10, 10, 20, 140);

  public IOptionButtonShape AddOptionButton(int row, int column)
  {
    return this.AddOptionButton(row, column, 20, 140);
  }
}
