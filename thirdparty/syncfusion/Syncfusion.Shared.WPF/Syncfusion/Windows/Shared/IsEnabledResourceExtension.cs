// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.IsEnabledResourceExtension
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

#nullable disable
namespace Syncfusion.Windows.Shared;

public class IsEnabledResourceExtension : MarkupExtension
{
  public IsEnabledResourceExtension(string id) => this.ID = id;

  public string ID { get; set; }

  public override object ProvideValue(IServiceProvider serviceProvider)
  {
    object enabledResource;
    try
    {
      enabledResource = this.GetEnabledResource(serviceProvider);
    }
    catch (Exception ex)
    {
      throw new InvalidOperationException("Resource for enabled state can not be found.", ex);
    }
    object disabledResource;
    try
    {
      disabledResource = this.GetDisabledResource(serviceProvider);
    }
    catch (Exception ex)
    {
      throw new InvalidOperationException("Resource for disabled state can not be found.", ex);
    }
    IsEnabledToResourceConverter resourceConverter = new IsEnabledToResourceConverter(new EnabledDisabledResources()
    {
      DisabledResource = disabledResource,
      EnabledResource = enabledResource
    });
    return new Binding()
    {
      Path = new PropertyPath((object) UIElement.IsEnabledProperty),
      RelativeSource = new RelativeSource(RelativeSourceMode.Self),
      Converter = ((IValueConverter) resourceConverter)
    }.ProvideValue(serviceProvider);
  }

  private object GetEnabledResource(IServiceProvider serviceProvider)
  {
    return new StaticResourceExtension(new IsEnabledResourceKeyExtension(ResourceKeyState.Enabled, this.ID).ProvideValue(serviceProvider)).ProvideValue(serviceProvider);
  }

  private object GetDisabledResource(IServiceProvider serviceProvider)
  {
    return new StaticResourceExtension(new IsEnabledResourceKeyExtension(ResourceKeyState.Disabled, this.ID).ProvideValue(serviceProvider)).ProvideValue(serviceProvider);
  }
}
