// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WCheckBox
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using Syncfusion.DocIO.Rendering;
using Syncfusion.Layouting;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class WCheckBox : WFormField, ILeafWidget, IWidget
{
  private int m_checkBoxSize;
  private byte m_bFlags;
  private CheckBoxSizeType m_sizeType;

  public override EntityType EntityType => EntityType.CheckBox;

  public int CheckBoxSize
  {
    get => this.m_checkBoxSize;
    set
    {
      this.m_checkBoxSize = value >= 1 && value <= 1584 ? value : throw new ArgumentOutOfRangeException("The measurement must be between 1pt to 1584pt");
    }
  }

  public bool DefaultCheckBoxValue
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  public bool Checked
  {
    get
    {
      switch (this.Value)
      {
        case 0:
          return false;
        case 1:
          return true;
        case 25:
          return this.DefaultCheckBoxValue;
        default:
          throw new ArgumentException("Unsupported checkbox field value found.");
      }
    }
    set => this.Value = value ? 1 : 0;
  }

  public CheckBoxSizeType SizeType
  {
    get
    {
      return (this.Params & 1024 /*0x0400*/) != 1024 /*0x0400*/ ? CheckBoxSizeType.Auto : CheckBoxSizeType.Exactly;
    }
    set
    {
      this.m_sizeType = value;
      if (value == CheckBoxSizeType.Exactly)
        this.Params = (int) (short) BaseWordRecord.SetBitsByMask(this.Params, 1024 /*0x0400*/, 10, 1);
      else
        this.Params = (int) (short) BaseWordRecord.SetBitsByMask(this.Params, 1024 /*0x0400*/, 10, 0);
    }
  }

  public WCheckBox(IWordDocument doc)
    : base(doc)
  {
    this.m_curFormFieldType = FormFieldType.CheckBox;
    this.m_paraItemType = ParagraphItemType.CheckBox;
    this.FieldType = FieldType.FieldFormCheckBox;
    this.Params = 229;
    this.m_checkBoxSize = 20;
  }

  protected override object CloneImpl() => (object) (WCheckBox) base.CloneImpl();

  protected override void ReadXmlAttributes(IXDLSAttributeReader reader)
  {
    base.ReadXmlAttributes(reader);
    if (reader.HasAttribute("CheckBoxSize"))
      this.m_checkBoxSize = (int) reader.ReadShort("CheckBoxSize");
    if (reader.HasAttribute("DefaultCheckBoxValue"))
      this.DefaultCheckBoxValue = reader.ReadBoolean("DefaultCheckBoxValue");
    if (!reader.HasAttribute("CheckBoxSizeType"))
      return;
    this.SizeType = (CheckBoxSizeType) reader.ReadEnum("CheckBoxSizeType", typeof (CheckBoxSizeType));
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    base.WriteXmlAttributes(writer);
    writer.WriteValue("CheckBoxSize", this.m_checkBoxSize);
    writer.WriteValue("CheckBoxSizeType", (Enum) this.SizeType);
    writer.WriteValue("DefaultCheckBoxValue", this.DefaultCheckBoxValue);
  }

  internal void SetCheckBoxSizeValue(int checkBoxSize) => this.m_checkBoxSize = checkBoxSize;

  protected override void CreateLayoutInfo()
  {
    base.CreateLayoutInfo();
    Font font = this.Document.FontSettings.GetFont(this.CharacterFormat.GetFontToRender(this.ScriptType).Name, this.CharacterFormat.GetFontToRender(this.ScriptType).Size, FontStyle.Regular);
    if (this.m_sizeType != CheckBoxSizeType.Auto)
      font = this.Document.FontSettings.GetFont(this.CharacterFormat.GetFontToRender(this.ScriptType).Name, (float) this.m_checkBoxSize, FontStyle.Regular);
    this.m_layoutInfo.Font = new SyncFont(font);
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
    float checkBoxSize = this.GetCheckBoxSize(dc);
    return new SizeF(checkBoxSize, checkBoxSize);
  }

  internal float GetCheckBoxSize(DrawingContext dc)
  {
    WCharacterFormat characterFormat = this.CharacterFormat;
    float fontSize = this.SizeType != CheckBoxSizeType.Auto ? (float) this.m_checkBoxSize : characterFormat.FontSize;
    if (this.m_layoutInfo == null)
      this.CreateLayoutInfo();
    Font font1 = this.m_layoutInfo.Font.GetFont(this.Document);
    Font font2 = this.Document.FontSettings.GetFont(font1.Name, fontSize, font1.Style);
    Font font3 = characterFormat == null || characterFormat.SubSuperScript == SubSuperScript.None ? font2 : this.Document.FontSettings.GetFont(font2.Name, dc.GetSubSuperScriptFontSize(font2), font2.Style);
    return dc.MeasureString("0x25A1", font3, new StringFormat(dc.StringFormt)).Height;
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
