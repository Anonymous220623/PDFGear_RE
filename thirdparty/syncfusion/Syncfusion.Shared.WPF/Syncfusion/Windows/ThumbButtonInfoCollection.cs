// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.ThumbButtonInfoCollection
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.Windows;

#nullable disable
namespace Syncfusion.Windows;

public class ThumbButtonInfoCollection : FreezableCollection<ThumbButtonInfo>
{
  private static ThumbButtonInfoCollection s_empty;

  protected override Freezable CreateInstanceCore() => (Freezable) new ThumbButtonInfoCollection();

  internal static ThumbButtonInfoCollection Empty
  {
    get
    {
      if (ThumbButtonInfoCollection.s_empty == null)
      {
        ThumbButtonInfoCollection buttonInfoCollection = new ThumbButtonInfoCollection();
        buttonInfoCollection.Freeze();
        ThumbButtonInfoCollection.s_empty = buttonInfoCollection;
      }
      return ThumbButtonInfoCollection.s_empty;
    }
  }
}
