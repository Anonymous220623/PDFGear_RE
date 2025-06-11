// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Controls.SR
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Globalization;
using System.Resources;

#nullable disable
namespace Syncfusion.Windows.Controls;

internal static class SR
{
  private static ResourceManager _resourceManager = new ResourceManager("Syncfusion.Windows.Shared.Properties.Resources", typeof (SR).Assembly);

  internal static string Get(SRID id)
  {
    try
    {
      return SR._resourceManager.GetString(id.String);
    }
    catch
    {
      return "";
    }
  }

  internal static string Get(SRID id, params object[] args)
  {
    string format = SR._resourceManager.GetString(id.String);
    if (format != null && args != null && args.Length > 0)
      format = string.Format((IFormatProvider) CultureInfo.CurrentCulture, format, args);
    return format;
  }
}
