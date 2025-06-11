// Decompiled with JetBrains decompiler
// Type: Syncfusion.XPS.StoryFragment
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

[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlRoot("StoryFragment", Namespace = "http://schemas.microsoft.com/xps/2005/06/documentstructure", IsNullable = false)]
[GeneratedCode("xsd", "2.0.50727.3038")]
[XmlType(Namespace = "http://schemas.microsoft.com/xps/2005/06/documentstructure")]
[Serializable]
public class StoryFragment
{
  private Break storyBreakField;
  private object[] itemsField;
  private Break storyBreak1Field;
  private string storyNameField;
  private string fragmentNameField;
  private FragmentType fragmentTypeField;

  [XmlElement(Order = 0)]
  public Break StoryBreak
  {
    get => this.storyBreakField;
    set => this.storyBreakField = value;
  }

  [XmlElement("ListStructure", typeof (List), Order = 1)]
  [XmlElement("FigureStructure", typeof (Figure), Order = 1)]
  [XmlElement("ParagraphStructure", typeof (Paragraph), Order = 1)]
  [XmlElement("SectionStructure", typeof (Section), Order = 1)]
  [XmlElement("TableStructure", typeof (Table), Order = 1)]
  public object[] Items
  {
    get => this.itemsField;
    set => this.itemsField = value;
  }

  [XmlElement("StoryBreak", Order = 2)]
  public Break StoryBreak1
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
  public FragmentType FragmentType
  {
    get => this.fragmentTypeField;
    set => this.fragmentTypeField = value;
  }
}
