// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.PinnableListBoxParams
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;

#nullable disable
namespace Syncfusion.Windows.Shared;

[Serializable]
public class PinnableListBoxParams
{
  public string PinItemsSortDescription { get; set; }

  public string UnPinItemsSortDescription { get; set; }

  public PinnableListBoxParams()
  {
  }

  public PinnableListBoxParams(string pinItemsSortDescription, string unPinItemsSortDescription)
  {
    this.PinItemsSortDescription = pinItemsSortDescription;
    this.UnPinItemsSortDescription = unPinItemsSortDescription;
  }
}
