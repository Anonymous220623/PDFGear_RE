// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.PdfXfaRadioButtonGroup
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Xfa;

public class PdfXfaRadioButtonGroup : PdfXfaField
{
  internal PdfXfaRadioButtonListItem m_radioList = new PdfXfaRadioButtonListItem();
  internal int selectedItem;
  private PdfXfaFlowDirection m_layout;
  internal PdfXfaForm parent;
  internal SizeF Size = SizeF.Empty;
  private bool m_readOnly;

  public bool ReadOnly
  {
    get => this.m_readOnly;
    set => this.m_readOnly = value;
  }

  public PdfXfaRadioButtonListItem Items
  {
    get => this.m_radioList;
    set
    {
      if (value == null)
        return;
      this.m_radioList = value;
    }
  }

  public PdfXfaFlowDirection FlowDirection
  {
    get => this.m_layout;
    set => this.m_layout = value;
  }

  public PdfXfaRadioButtonGroup(string name) => this.Name = name;

  internal void Save(XfaWriter xfaWriter)
  {
    xfaWriter.Write.WriteStartElement("exclGroup");
    string str = "group1";
    if (this.Name != null && this.Name != "")
      str = this.Name;
    xfaWriter.Write.WriteAttributeString("name", str);
    if (this.FlowDirection == PdfXfaFlowDirection.Horizontal)
    {
      xfaWriter.Write.WriteAttributeString("layout", "lr-tb");
      xfaWriter.Write.WriteAttributeString("w", this.parent.Width.ToString() + "pt");
    }
    else
      xfaWriter.Write.WriteAttributeString("layout", "tb");
    int num = 1;
    foreach (PdfXfaRadioButtonField radio in (List<PdfXfaRadioButtonField>) this.m_radioList)
    {
      if (radio.IsChecked)
        this.selectedItem = num;
      radio.Save(xfaWriter, num++);
    }
    xfaWriter.WriteMargins(this.Margins);
    xfaWriter.Write.WriteEndElement();
  }

  public object Clone() => (object) (PdfXfaRadioButtonGroup) this.MemberwiseClone();
}
