// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WMath
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.Rendering;
using Syncfusion.DocIO.Rendering;
using Syncfusion.Layouting;
using Syncfusion.Office;
using System.Drawing;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class WMath : ParagraphItem, ILeafWidget, IWidget
{
  private OfficeMathParagraph m_mathPara;
  private IWordDocument m_doc;

  public IOfficeMathParagraph MathParagraph => (IOfficeMathParagraph) this.m_mathPara;

  public override EntityType EntityType => EntityType.Math;

  public bool IsInline => this.CheckMathIsInline();

  public WMath(IWordDocument doc)
    : base((WordDocument) doc)
  {
    this.m_mathPara = new OfficeMathParagraph((object) this);
    this.m_mathPara.DefaultMathCharacterFormat = (IOfficeRunFormat) new WCharacterFormat(doc, (Entity) this);
    (this.m_mathPara.DefaultMathCharacterFormat as WCharacterFormat).FontName = "Cambria Math";
    (this.m_mathPara.DefaultMathCharacterFormat as WCharacterFormat).Italic = true;
    this.m_doc = doc;
  }

  protected override object CloneImpl()
  {
    WMath owner = (WMath) base.CloneImpl();
    owner.m_mathPara = this.m_mathPara.Clone();
    (owner.MathParagraph as OfficeMathParagraph).SetOwner((object) owner);
    return (object) owner;
  }

  internal override void Close()
  {
    if (this.m_mathPara != null)
    {
      this.m_mathPara.Close();
      this.m_mathPara = (OfficeMathParagraph) null;
    }
    base.Close();
  }

  public void ChangeToDisplay()
  {
    if (!this.IsInline)
      return;
    ParagraphItemCollection paraItems = this.Owner is WParagraph ? (this.Owner as WParagraph).Items : (this.Owner is InlineContentControl ? (this.Owner as InlineContentControl).ParagraphItems : (ParagraphItemCollection) null);
    int num = paraItems.IndexOf((IEntity) this);
    bool flag1 = this.HasRenderableItemBeforeMath(num, paraItems);
    bool flag2 = this.HasRenderableItemAfterMath(num, paraItems);
    if (flag1 && !flag2)
    {
      Break @break = new Break(this.m_doc);
      paraItems.Insert(num, (IEntity) @break);
    }
    else if (!flag1 && flag2)
    {
      bool flag3 = false;
      int index1 = -1;
      for (int index2 = num + 1; index2 < paraItems.Count; ++index2)
      {
        if (this.IsRenderableItem(paraItems[index2]))
        {
          if (paraItems[index2] is WTextRange wtextRange && wtextRange.Text == " ")
          {
            index1 = index2;
            for (int index3 = index2 + 1; index3 < paraItems.Count; ++index3)
            {
              flag3 = this.IsRenderableItem(paraItems[index3]);
              if (flag3)
                break;
            }
          }
          else
          {
            flag3 = true;
            break;
          }
        }
      }
      if (!flag3 && index1 != -1)
      {
        paraItems[index1].RemoveSelf();
      }
      else
      {
        Break @break = new Break(this.m_doc);
        paraItems.Insert(num + 1, (IEntity) @break);
      }
    }
    else
    {
      if (!flag1)
        return;
      Break break1 = new Break(this.m_doc);
      paraItems.Insert(num + 1, (IEntity) break1);
      Break break2 = new Break(this.m_doc);
      paraItems.Insert(num, (IEntity) break2);
    }
  }

  internal byte[] GetAsImage()
  {
    try
    {
      DocumentLayouter documentLayouter = new DocumentLayouter();
      byte[] asImage = documentLayouter.ConvertAsImage((IWidget) this);
      documentLayouter.Close();
      return asImage;
    }
    catch
    {
      return (byte[]) null;
    }
  }

  internal bool IsRenderableItem(ParagraphItem item)
  {
    return item.EntityType != EntityType.BookmarkStart && item.EntityType != EntityType.BookmarkEnd && item.EntityType != EntityType.EditableRangeStart && item.EntityType != EntityType.EditableRangeEnd;
  }

  public void ChangeToInline()
  {
    if (this.IsInline)
      return;
    ParagraphItemCollection paraItems = this.Owner is WParagraph ? (this.Owner as WParagraph).Items : (this.Owner is InlineContentControl ? (this.Owner as InlineContentControl).ParagraphItems : (ParagraphItemCollection) null);
    int mathIndex = paraItems.IndexOf((IEntity) this);
    bool flag1 = this.HasRenderableItemBeforeMath(mathIndex, paraItems);
    bool flag2 = this.HasRenderableItemAfterMath(mathIndex, paraItems);
    if (!flag1 && !flag2)
      this.AddEmptyTextRange(paraItems, mathIndex + 1);
    else if (flag1 && !flag2)
      this.RemovePreviousBreak(mathIndex, paraItems);
    else if (!flag1)
    {
      this.RemoveNextBreak(paraItems);
    }
    else
    {
      this.RemovePreviousBreak(mathIndex, paraItems);
      this.RemoveNextBreak(paraItems);
    }
  }

  private bool HasRenderableItemAfterMath(int mathIndex, ParagraphItemCollection paraItems)
  {
    bool flag = false;
    int index1 = this.MathParagraph.Maths.Count - 1;
    int index2 = this.MathParagraph.Maths[index1].Functions.Count - 1;
    IOfficeMathRunElement officeMathRunElement = (IOfficeMathRunElement) null;
    if (index2 != -1)
      officeMathRunElement = this.MathParagraph.Maths[index1].Functions[index2] as IOfficeMathRunElement;
    if (officeMathRunElement != null && officeMathRunElement.Item is ParagraphItem && (officeMathRunElement.Item as ParagraphItem).EntityType == EntityType.Break)
    {
      flag = true;
    }
    else
    {
      for (int index3 = mathIndex + 1; index3 < paraItems.Count; ++index3)
      {
        flag = this.IsRenderableItem(paraItems[index3]);
        if (flag)
          break;
      }
    }
    return flag;
  }

  private bool HasRenderableItemBeforeMath(int mathIndex, ParagraphItemCollection paraItems)
  {
    bool flag = false;
    IOfficeMathRunElement officeMathRunElement = (IOfficeMathRunElement) null;
    if (this.MathParagraph.Maths[0].Functions.Count != 0)
      officeMathRunElement = this.MathParagraph.Maths[0].Functions[0] as IOfficeMathRunElement;
    if (officeMathRunElement != null && officeMathRunElement.Item is ParagraphItem && (officeMathRunElement.Item as ParagraphItem).EntityType == EntityType.Break)
    {
      flag = true;
    }
    else
    {
      for (int index = mathIndex - 1; index >= 0; --index)
      {
        flag = this.IsRenderableItem(paraItems[index]);
        if (flag)
          break;
      }
    }
    return flag;
  }

  private void RemovePreviousBreak(int mathIndex, ParagraphItemCollection paraItems)
  {
    if (this.MathParagraph.Maths[0].Functions[0] is IOfficeMathRunElement function && function.Item is ParagraphItem && (function.Item as ParagraphItem).EntityType == EntityType.Break)
    {
      this.MathParagraph.Maths[0].Functions.Remove((IOfficeMathEntity) function);
    }
    else
    {
      int index1 = -1;
      for (int index2 = mathIndex - 1; index2 >= 0; --index2)
      {
        if (paraItems[index2].EntityType == EntityType.Break)
        {
          index1 = index2;
          break;
        }
      }
      if (index1 != -1)
        paraItems.RemoveAt(index1);
    }
    mathIndex = paraItems.IndexOf((IEntity) this);
    if (mathIndex + 1 < paraItems.Count)
    {
      bool flag = false;
      for (int index = mathIndex + 1; index < paraItems.Count; ++index)
      {
        if (this.IsRenderableItem(paraItems[index]))
        {
          flag = true;
          break;
        }
      }
      if (flag)
        return;
      this.AddEmptyTextRange(paraItems, mathIndex + 1);
    }
    else
      this.AddEmptyTextRange(paraItems, mathIndex + 1);
  }

  private void AddEmptyTextRange(ParagraphItemCollection paraItems, int mathIndex)
  {
    paraItems.Insert(mathIndex, (IEntity) new WTextRange(this.m_doc)
    {
      Text = " "
    });
  }

  private void RemoveNextBreak(ParagraphItemCollection paraItems)
  {
    int num1 = paraItems.IndexOf((IEntity) this);
    int index1 = this.MathParagraph.Maths.Count - 1;
    int index2 = this.MathParagraph.Maths[index1].Functions.Count - 1;
    IOfficeMathRunElement officeMathRunElement = (IOfficeMathRunElement) null;
    if (index2 != -1)
      officeMathRunElement = this.MathParagraph.Maths[index1].Functions[index2] as IOfficeMathRunElement;
    if (officeMathRunElement != null && officeMathRunElement.Item is ParagraphItem && (officeMathRunElement.Item as ParagraphItem).EntityType == EntityType.Break)
    {
      this.MathParagraph.Maths[index1].Functions.Remove((IOfficeMathEntity) officeMathRunElement);
    }
    else
    {
      for (int index3 = num1 + 1; index3 < paraItems.Count; ++index3)
      {
        if (this.IsRenderableItem(paraItems[index3]) && paraItems[index3].EntityType == EntityType.Break)
        {
          paraItems[index3].RemoveSelf();
          break;
        }
      }
    }
    int num2 = paraItems.IndexOf((IEntity) this);
    if (num2 + 1 < paraItems.Count)
    {
      bool flag = false;
      for (int index4 = num2 + 1; index4 < paraItems.Count; ++index4)
      {
        if (this.IsRenderableItem(paraItems[index4]))
        {
          flag = true;
          break;
        }
      }
      if (flag)
        return;
      this.AddEmptyTextRange(paraItems, num2 + 1);
    }
    else
      this.AddEmptyTextRange(paraItems, num2 + 1);
  }

  internal void ApplyBaseFormat()
  {
    for (int index = 0; index < this.MathParagraph.Maths.Count; ++index)
      this.IterateOfficeMath(this.MathParagraph.Maths[index]);
  }

  private void IterateOfficeMath(IOfficeMath officeMath)
  {
    for (int index = 0; index < officeMath.Functions.Count; ++index)
      this.IterateIntoFunction(officeMath.Functions[index]);
  }

  private void IterateIntoFunction(IOfficeMathFunctionBase officeMathFunction)
  {
    switch (officeMathFunction.Type)
    {
      case MathFunctionType.Accent:
        this.IterateOfficeMath((officeMathFunction as IOfficeMathAccent).Equation);
        break;
      case MathFunctionType.Bar:
        this.IterateOfficeMath((officeMathFunction as IOfficeMathBar).Equation);
        break;
      case MathFunctionType.BorderBox:
        this.IterateOfficeMath((officeMathFunction as IOfficeMathBorderBox).Equation);
        break;
      case MathFunctionType.Box:
        this.IterateOfficeMath((officeMathFunction as IOfficeMathBox).Equation);
        break;
      case MathFunctionType.Delimiter:
        IOfficeMathDelimiter officeMathDelimiter = officeMathFunction as IOfficeMathDelimiter;
        for (int index = 0; index < officeMathDelimiter.Equation.Count; ++index)
          this.IterateOfficeMath(officeMathDelimiter.Equation[index]);
        break;
      case MathFunctionType.EquationArray:
        IOfficeMathEquationArray mathEquationArray = officeMathFunction as IOfficeMathEquationArray;
        for (int index = 0; index < mathEquationArray.Equation.Count; ++index)
          this.IterateOfficeMath(mathEquationArray.Equation[index]);
        break;
      case MathFunctionType.Fraction:
        IOfficeMathFraction officeMathFraction = officeMathFunction as IOfficeMathFraction;
        this.IterateOfficeMath(officeMathFraction.Numerator);
        this.IterateOfficeMath(officeMathFraction.Denominator);
        break;
      case MathFunctionType.Function:
        IOfficeMathFunction officeMathFunction1 = officeMathFunction as IOfficeMathFunction;
        this.IterateOfficeMath(officeMathFunction1.FunctionName);
        this.IterateOfficeMath(officeMathFunction1.Equation);
        break;
      case MathFunctionType.GroupCharacter:
        this.IterateOfficeMath((officeMathFunction as IOfficeMathGroupCharacter).Equation);
        break;
      case MathFunctionType.Limit:
        IOfficeMathLimit officeMathLimit = officeMathFunction as IOfficeMathLimit;
        this.IterateOfficeMath(officeMathLimit.Equation);
        this.IterateOfficeMath(officeMathLimit.Limit);
        break;
      case MathFunctionType.Matrix:
        IOfficeMathMatrix officeMathMatrix = officeMathFunction as IOfficeMathMatrix;
        for (int index1 = 0; index1 < officeMathMatrix.Rows.Count; ++index1)
        {
          for (int index2 = 0; index2 < (officeMathMatrix.Rows[index1] as OfficeMathMatrixRow).Arguments.Count; ++index2)
            this.IterateOfficeMath((officeMathMatrix.Rows[index1] as OfficeMathMatrixRow).Arguments[index2]);
        }
        break;
      case MathFunctionType.NArray:
        IOfficeMathNArray officeMathNarray = officeMathFunction as IOfficeMathNArray;
        this.IterateOfficeMath(officeMathNarray.Subscript);
        this.IterateOfficeMath(officeMathNarray.Superscript);
        this.IterateOfficeMath(officeMathNarray.Equation);
        break;
      case MathFunctionType.Phantom:
        this.IterateOfficeMath((officeMathFunction as IOfficeMathPhantom).Equation);
        break;
      case MathFunctionType.Radical:
        IOfficeMathRadical officeMathRadical = officeMathFunction as IOfficeMathRadical;
        this.IterateOfficeMath(officeMathRadical.Degree);
        this.IterateOfficeMath(officeMathRadical.Equation);
        break;
      case MathFunctionType.LeftSubSuperscript:
        OfficeMathLeftScript officeMathLeftScript = officeMathFunction as OfficeMathLeftScript;
        this.IterateOfficeMath(officeMathLeftScript.Subscript);
        this.IterateOfficeMath(officeMathLeftScript.Superscript);
        this.IterateOfficeMath(officeMathLeftScript.Equation);
        break;
      case MathFunctionType.SubSuperscript:
        IOfficeMathScript officeMathScript = officeMathFunction as IOfficeMathScript;
        this.IterateOfficeMath(officeMathScript.Equation);
        this.IterateOfficeMath(officeMathScript.Script);
        break;
      case MathFunctionType.RightSubSuperscript:
        IOfficeMathRightScript officeMathRightScript = officeMathFunction as IOfficeMathRightScript;
        this.IterateOfficeMath(officeMathRightScript.Equation);
        this.IterateOfficeMath(officeMathRightScript.Subscript);
        this.IterateOfficeMath(officeMathRightScript.Superscript);
        break;
      case MathFunctionType.RunElement:
        IOfficeMathRunElement officeMathRunElement = officeMathFunction as IOfficeMathRunElement;
        if (officeMathRunElement.Item == null || (officeMathRunElement.Item as ParagraphItem).ParaItemCharFormat == null || this.OwnerParagraph == null || this.OwnerParagraph.BreakCharacterFormat == null || this.OwnerParagraph.BreakCharacterFormat.BaseFormat == null)
          break;
        (officeMathRunElement.Item as ParagraphItem).ParaItemCharFormat.ApplyBase(this.OwnerParagraph.BreakCharacterFormat.BaseFormat);
        break;
    }
  }

  private bool CheckMathIsInline()
  {
    ParagraphItemCollection paraItems = this.Owner is WParagraph ? (this.Owner as WParagraph).Items : (this.Owner is InlineContentControl ? (this.Owner as InlineContentControl).ParagraphItems : (ParagraphItemCollection) null);
    if (paraItems != null && paraItems.Count > 0)
    {
      int mathIndex = paraItems.IndexOf((IEntity) this);
      bool flag1 = this.HasRenderableItemBeforeMath(mathIndex, paraItems);
      bool flag2 = this.HasRenderableItemAfterMath(mathIndex, paraItems);
      bool flag3 = this.IsMathAfterBreak(mathIndex, paraItems);
      bool flag4 = this.IsMathBeforeBreak(paraItems);
      if (!flag1 && flag2)
        return !flag4;
      if (flag1 && !flag2)
        return !flag3;
      if (flag1)
        return !flag3 && !flag4;
    }
    if (!(this.Owner is InlineContentControl) || (this.Owner as InlineContentControl).OwnerParagraph == null)
      return false;
    WParagraph ownerParagraph = (this.Owner as InlineContentControl).OwnerParagraph;
    int mathIndex1 = ownerParagraph.ChildEntities.IndexOf((IEntity) this.Owner);
    bool flag5 = this.HasRenderableItemBeforeMath(mathIndex1, ownerParagraph.Items);
    bool flag6 = this.HasRenderableItemAfterMath(mathIndex1, ownerParagraph.Items);
    return flag5 || flag6;
  }

  private bool IsMathBeforeBreak(ParagraphItemCollection paraItems)
  {
    bool flag = false;
    int index1 = this.MathParagraph.Maths.Count - 1;
    int index2 = this.MathParagraph.Maths[index1].Functions.Count - 1;
    IOfficeMathRunElement officeMathRunElement = (IOfficeMathRunElement) null;
    if (index2 != -1)
      officeMathRunElement = this.MathParagraph.Maths[index1].Functions[index2] as IOfficeMathRunElement;
    if (officeMathRunElement != null && officeMathRunElement.Item is ParagraphItem && (officeMathRunElement.Item as ParagraphItem).EntityType == EntityType.Break)
      flag = true;
    for (int index3 = index1 + 1; index3 < paraItems.Count && !flag; ++index3)
    {
      if (this.IsRenderableItem(paraItems[index3]))
        return paraItems[index3].EntityType == EntityType.Break;
    }
    return flag;
  }

  private bool IsMathAfterBreak(int mathIndex, ParagraphItemCollection paraItems)
  {
    IOfficeMathRunElement officeMathRunElement = (IOfficeMathRunElement) null;
    if (this.MathParagraph.Maths[0].Functions.Count != 0)
      officeMathRunElement = this.MathParagraph.Maths[0].Functions[0] as IOfficeMathRunElement;
    if (officeMathRunElement != null && officeMathRunElement.Item is ParagraphItem && (officeMathRunElement.Item as ParagraphItem).EntityType == EntityType.Break)
      return true;
    for (int index = mathIndex - 1; index >= 0; --index)
    {
      if (this.IsRenderableItem(paraItems[index]))
        return paraItems[index].EntityType == EntityType.Break;
    }
    return false;
  }

  protected override void CreateLayoutInfo()
  {
    this.m_layoutInfo = (ILayoutInfo) new LayoutInfo(ChildrenLayoutDirection.Horizontal);
    WParagraph wparagraph = this.OwnerParagraph;
    if (this.Owner is InlineContentControl || this.Owner is XmlParagraphItem || this.Owner is GroupShape || this.Owner is ChildGroupShape)
      wparagraph = this.GetOwnerParagraphValue();
    Entity ownerEntity = wparagraph.GetOwnerEntity();
    if (!wparagraph.IsInCell || !((IWidget) wparagraph).LayoutInfo.IsClipped)
    {
      switch (ownerEntity)
      {
        case Shape _:
        case WTextBox _:
        case ChildShape _:
          break;
        default:
          goto label_5;
      }
    }
    this.m_layoutInfo.IsClipped = true;
label_5:
    this.m_layoutInfo.IsVerticalText = ((IWidget) wparagraph).LayoutInfo.IsVerticalText;
  }

  internal override void InitLayoutInfo(Entity entity, ref bool isLastTOCEntry)
  {
    this.InitializingLayoutInfo();
    if (this != entity)
      return;
    isLastTOCEntry = true;
  }

  void IWidget.InitLayoutInfo() => this.InitializingLayoutInfo();

  private void InitializingLayoutInfo()
  {
    this.m_layoutInfo = (ILayoutInfo) null;
    for (int index1 = 0; index1 < this.MathParagraph.Maths.Count; ++index1)
    {
      IOfficeMath math = this.MathParagraph.Maths[index1];
      for (int index2 = 0; index2 < math.Functions.Count; ++index2)
      {
        IOfficeMathFunctionBase function = math.Functions[index2];
        if (function.Type == MathFunctionType.RunElement)
        {
          IOfficeMathRunElement officeMathRunElement = function as IOfficeMathRunElement;
          if (officeMathRunElement.Item is IWidget)
            (officeMathRunElement.Item as IWidget).InitLayoutInfo();
        }
      }
    }
  }

  SizeF ILeafWidget.Measure(DrawingContext dc) => new SizeF();
}
