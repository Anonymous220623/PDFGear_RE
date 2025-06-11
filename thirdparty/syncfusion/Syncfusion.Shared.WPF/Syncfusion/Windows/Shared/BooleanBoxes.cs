// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.BooleanBoxes
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

#nullable disable
namespace Syncfusion.Windows.Shared;

internal static class BooleanBoxes
{
  internal static object FalseBox = (object) false;
  internal static object TrueBox = (object) true;

  internal static object Box(bool value) => value ? BooleanBoxes.TrueBox : BooleanBoxes.FalseBox;
}
