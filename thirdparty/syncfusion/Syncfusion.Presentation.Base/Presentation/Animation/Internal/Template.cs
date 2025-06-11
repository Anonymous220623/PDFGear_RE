// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Animation.Internal.Template
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation.Animation.Internal;

internal class Template
{
  private uint level;
  private List<object> timeNodeList;

  internal uint Level
  {
    get => this.level;
    set => this.level = value;
  }

  internal List<object> TimeNodeList
  {
    get => this.timeNodeList;
    set => this.timeNodeList = value;
  }

  internal Template Clone() => (Template) this.MemberwiseClone();
}
