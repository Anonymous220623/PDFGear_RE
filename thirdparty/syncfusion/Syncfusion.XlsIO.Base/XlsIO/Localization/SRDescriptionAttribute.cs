// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Localization.SRDescriptionAttribute
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.ComponentModel;

#nullable disable
namespace Syncfusion.XlsIO.Localization;

[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
internal sealed class SRDescriptionAttribute(string description) : DescriptionAttribute(description)
{
  private bool replaced;

  public override string Description
  {
    get
    {
      if (!this.replaced)
      {
        this.replaced = true;
        this.DescriptionValue = SR.GetString(base.Description);
      }
      return base.Description;
    }
  }
}
