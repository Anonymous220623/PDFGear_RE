// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.SharedLocalizationResourceAccessor
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Windows.Shared.Resources;
using System.Reflection;
using System.Resources;

#nullable disable
namespace Syncfusion.Windows.Shared;

public class SharedLocalizationResourceAccessor : LocalizationResourceAccessor
{
  public static SharedLocalizationResourceAccessor Instance = new SharedLocalizationResourceAccessor();

  protected override Assembly GetControlAssembly() => this.GetType().Assembly;

  protected override string GetControlAssemblyDefaultNamespace() => "Syncfusion.Windows.Shared";

  protected override ResourceManager GetDefaultResourceManager()
  {
    return Syncfusion_Shared_Wpf.ResourceManager;
  }
}
