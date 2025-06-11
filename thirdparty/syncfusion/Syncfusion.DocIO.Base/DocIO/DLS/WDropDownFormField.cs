// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WDropDownFormField
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.Rendering;
using Syncfusion.DocIO.DLS.XML;
using Syncfusion.DocIO.Rendering;
using Syncfusion.Layouting;
using System.Drawing;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class WDropDownFormField : WFormField, ILeafWidget, IWidget
{
  private short m_defaultDropDownValue;
  private WDropDownCollection m_dropDownItems;

  public WDropDownFormField(IWordDocument doc)
    : base(doc)
  {
    this.m_curFormFieldType = FormFieldType.DropDown;
    this.m_paraItemType = ParagraphItemType.DropDownFormField;
    this.FieldType = FieldType.FieldFormDropDown;
    this.Params = 32998;
    this.m_dropDownItems = new WDropDownCollection(this.Document);
  }

  public override EntityType EntityType => EntityType.DropDownFormField;

  public int DropDownSelectedIndex
  {
    get => this.Value != 25 ? this.Value : (int) this.m_defaultDropDownValue;
    set => this.Value = value;
  }

  public WDropDownCollection DropDownItems => this.m_dropDownItems;

  internal int DefaultDropDownValue
  {
    get => (int) this.m_defaultDropDownValue;
    set => this.m_defaultDropDownValue = (short) value;
  }

  internal string DropDownValue
  {
    get
    {
      if (this.m_dropDownItems.Count > 0)
      {
        int num = this.DropDownSelectedIndex > 0 ? this.DropDownSelectedIndex : this.DefaultDropDownValue;
        if (num < 0 || num > this.m_dropDownItems.Count)
          return this.m_dropDownItems[0].Text;
        if (num < this.m_dropDownItems.Count)
          return this.m_dropDownItems[this.DropDownSelectedIndex].Text;
      }
      return "     ";
    }
    set
    {
      for (int index = 0; index < this.m_dropDownItems.Count; ++index)
      {
        if (string.Compare(this.m_dropDownItems[index].Text, value, true) == 0)
        {
          this.DropDownSelectedIndex = index;
          break;
        }
      }
    }
  }

  protected override object CloneImpl()
  {
    WDropDownFormField wdropDownFormField = (WDropDownFormField) base.CloneImpl();
    wdropDownFormField.m_dropDownItems = new WDropDownCollection(this.Document);
    this.m_dropDownItems.CloneTo(wdropDownFormField.m_dropDownItems);
    return (object) wdropDownFormField;
  }

  internal override void Close()
  {
    if (this.m_dropDownItems != null)
    {
      this.m_dropDownItems.Close();
      this.m_dropDownItems = (WDropDownCollection) null;
    }
    base.Close();
  }

  protected override void ReadXmlAttributes(IXDLSAttributeReader reader)
  {
    base.ReadXmlAttributes(reader);
    if (!reader.HasAttribute("DefaultDrowDownValue"))
      return;
    this.m_defaultDropDownValue = reader.ReadShort("DefaultDrowDownValue");
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    base.WriteXmlAttributes(writer);
    writer.WriteValue("DefaultDrowDownValue", (int) this.m_defaultDropDownValue);
  }

  protected override void InitXDLSHolder()
  {
    base.InitXDLSHolder();
    this.XDLSHolder.AddElement("dropdown-items", (object) this.m_dropDownItems);
  }

  protected override void CreateLayoutInfo()
  {
    base.CreateLayoutInfo();
    this.m_layoutInfo.Font = new SyncFont(DocumentLayouter.DrawingContext.GetFont(this.ScriptType, this.CharacterFormat, this.DropDownValue));
  }

  internal override void InitLayoutInfo(Entity entity, ref bool isLastTOCEntry)
  {
    this.m_layoutInfo = (ILayoutInfo) null;
    if (this != entity)
      return;
    isLastTOCEntry = true;
  }

  SizeF ILeafWidget.Measure(DrawingContext dc)
  {
    return dc.MeasureString(this.DropDownValue, this.CharacterFormat.GetFontToRender(this.ScriptType), (StringFormat) null, this.CharacterFormat, false);
  }

  ILayoutInfo IWidget.LayoutInfo
  {
    get
    {
      if (this.m_layoutInfo == null)
        this.CreateLayoutInfo();
      return this.m_layoutInfo;
    }
  }

  void IWidget.InitLayoutInfo() => this.m_layoutInfo = (ILayoutInfo) null;

  void IWidget.InitLayoutInfo(IWidget widget)
  {
  }
}
