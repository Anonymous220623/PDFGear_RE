// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Animation.Internal.TemplateEffects
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation.Animation.Internal;

internal class TemplateEffects
{
  private Template template;

  internal TemplateEffects() => this.template = new Template();

  internal Template Template
  {
    get => this.template;
    set => this.template = value;
  }

  internal TemplateEffects Clone()
  {
    TemplateEffects templateEffects = (TemplateEffects) this.MemberwiseClone();
    templateEffects.template = this.template.Clone();
    return templateEffects;
  }
}
