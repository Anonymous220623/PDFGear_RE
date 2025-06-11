// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.IdentityLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using System.ComponentModel;
using System.Security.Principal;
using System.Text;
using System.Threading;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("identity")]
[ThreadSafe]
public class IdentityLayoutRenderer : LayoutRenderer
{
  [DefaultValue(":")]
  public string Separator { get; set; } = ":";

  [DefaultValue(true)]
  public bool Name { get; set; } = true;

  [DefaultValue(true)]
  public bool AuthType { get; set; } = true;

  [DefaultValue(true)]
  public bool IsAuthenticated { get; set; } = true;

  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    IIdentity identity = IdentityLayoutRenderer.GetValue();
    if (identity == null)
      return;
    string str = string.Empty;
    if (this.IsAuthenticated)
    {
      builder.Append(str);
      str = this.Separator;
      builder.Append(identity.IsAuthenticated ? "auth" : "notauth");
    }
    if (this.AuthType)
    {
      builder.Append(str);
      str = this.Separator;
      builder.Append(identity.AuthenticationType);
    }
    if (!this.Name)
      return;
    builder.Append(str);
    builder.Append(identity.Name);
  }

  private static IIdentity GetValue() => Thread.CurrentPrincipal?.Identity;
}
