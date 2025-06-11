// Decompiled with JetBrains decompiler
// Type: SoundTouch.Net.NAudioSupport.Assets.Strings
// Assembly: SoundTouch.Net.NAudioSupport, Version=2.3.0.0, Culture=neutral, PublicKeyToken=fd94608088498f78
// MVID: 99206FE3-DB71-4C89-91A8-76F439C9BC37
// Assembly location: D:\PDFGear\bin\SoundTouch.Net.NAudioSupport.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace SoundTouch.Net.NAudioSupport.Assets;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
internal class Strings
{
  private static ResourceManager resourceMan;
  private static CultureInfo resourceCulture;

  internal Strings()
  {
  }

  [EditorBrowsable(EditorBrowsableState.Advanced)]
  internal static ResourceManager ResourceManager
  {
    get
    {
      if (Strings.resourceMan == null)
        Strings.resourceMan = new ResourceManager("SoundTouch.Net.NAudioSupport.Assets.Strings", typeof (Strings).Assembly);
      return Strings.resourceMan;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Advanced)]
  internal static CultureInfo Culture
  {
    get => Strings.resourceCulture;
    set => Strings.resourceCulture = value;
  }

  internal static string Argument_WaveFormat32BitsPerSample
  {
    get
    {
      return Strings.ResourceManager.GetString(nameof (Argument_WaveFormat32BitsPerSample), Strings.resourceCulture);
    }
  }

  internal static string Argument_WaveFormatIeeeFloat
  {
    get
    {
      return Strings.ResourceManager.GetString(nameof (Argument_WaveFormatIeeeFloat), Strings.resourceCulture);
    }
  }

  internal static string ObjectDisposed_StreamClosed
  {
    get
    {
      return Strings.ResourceManager.GetString(nameof (ObjectDisposed_StreamClosed), Strings.resourceCulture);
    }
  }
}
