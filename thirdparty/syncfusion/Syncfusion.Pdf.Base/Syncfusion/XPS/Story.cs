// Decompiled with JetBrains decompiler
// Type: Syncfusion.XPS.Story
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
[GeneratedCode("xsd", "2.0.50727.3038")]
[XmlRoot("Story", Namespace = "http://schemas.microsoft.com/xps/2005/06/documentstructure", IsNullable = false)]
[DesignerCategory("code")]
[XmlType(Namespace = "http://schemas.microsoft.com/xps/2005/06/documentstructure")]
[Serializable]
public class Story
{
  private Syncfusion.XPS.StoryFragmentReference[] storyFragmentReferenceField;
  private string storyNameField;

  [XmlElement("StoryFragmentReference")]
  public Syncfusion.XPS.StoryFragmentReference[] StoryFragmentReference
  {
    get => this.storyFragmentReferenceField;
    set => this.storyFragmentReferenceField = value;
  }

  [XmlAttribute]
  public string StoryName
  {
    get => this.storyNameField;
    set => this.storyNameField = value;
  }
}
