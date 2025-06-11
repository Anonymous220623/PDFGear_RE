// Decompiled with JetBrains decompiler
// Type: Syncfusion.XPS.OXPSStoryFragment
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

#nullable disable
namespace Syncfusion.XPS;

[XmlRoot("StoryFragment", Namespace = "http://schemas.openxps.org/oxps/v1.0/documentstructure", IsNullable = false)]
[GeneratedCode("xsd", "2.0.50727.3038")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://schemas.openxps.org/oxps/v1.0/documentstructure")]
[Serializable]
public class OXPSStoryFragment
{
  private OXPSBreak storyBreakField;
  private object[] itemsField;
  private OXPSBreak storyBreak1Field;
  private string storyNameField;
  private string fragmentNameField;
  private OXPSFragmentType fragmentTypeField;

  [XmlElement(Order = 0)]
  public OXPSBreak StoryBreak
  {
    get => this.storyBreakField;
    set => this.storyBreakField = value;
  }

  [XmlElement("TableStructure", typeof (OXPSTable), Order = 1)]
  [XmlElement("FigureStructure", typeof (OXPSFigure), Order = 1)]
  [XmlElement("ListStructure", typeof (OXPSList), Order = 1)]
  [XmlElement("ParagraphStructure", typeof (OXPSParagraph), Order = 1)]
  [XmlElement("SectionStructure", typeof (OXPSSection), Order = 1)]
  public object[] Items
  {
    get => this.itemsField;
    set => this.itemsField = value;
  }

  [XmlElement("StoryBreak", Order = 2)]
  public OXPSBreak StoryBreak1
  {
    get => this.storyBreak1Field;
    set => this.storyBreak1Field = value;
  }

  [XmlAttribute]
  public string StoryName
  {
    get => this.storyNameField;
    set => this.storyNameField = value;
  }

  [XmlAttribute]
  public string FragmentName
  {
    get => this.fragmentNameField;
    set => this.fragmentNameField = value;
  }

  [XmlAttribute]
  public OXPSFragmentType FragmentType
  {
    get => this.fragmentTypeField;
    set => this.fragmentTypeField = value;
  }
}
