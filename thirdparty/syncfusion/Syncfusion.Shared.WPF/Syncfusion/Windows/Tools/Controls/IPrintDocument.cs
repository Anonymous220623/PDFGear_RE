// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Tools.Controls.IPrintDocument
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.Windows;

#nullable disable
namespace Syncfusion.Windows.Tools.Controls;

public interface IPrintDocument
{
  int TotalPages { get; set; }

  Size PageSize { get; set; }

  Size PrintablePageSize { get; set; }

  Thickness Margin { get; set; }

  FrameworkElement GetPage(int pageNo);

  void OnSetPageSize();
}
