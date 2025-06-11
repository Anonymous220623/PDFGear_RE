// Decompiled with JetBrains decompiler
// Type: Syncfusion.XPS.OXPSStoryFragments
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

[GeneratedCode("xsd", "2.0.50727.3038")]
[XmlRoot("StoryFragments", Namespace = "http://schemas.openxps.org/oxps/v1.0/documentstructure", IsNullable = false)]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://schemas.openxps.org/oxps/v1.0/documentstructure")]
[Serializable]
public class OXPSStoryFragments
{
  private OXPSStoryFragment[] storyFragmentField;

  [XmlElement("StoryFragment")]
  public OXPSStoryFragment[] StoryFragment
  {
    get => this.storyFragmentField;
    set => this.storyFragmentField = value;
  }
}
