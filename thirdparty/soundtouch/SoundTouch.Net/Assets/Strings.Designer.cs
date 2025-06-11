// Decompiled with JetBrains decompiler
// Type: SoundTouch.Assets.Strings
// Assembly: SoundTouch.Net, Version=2.3.0.0, Culture=neutral, PublicKeyToken=fd94608088498f78
// MVID: 3EFEC8B2-F004-4B74-B172-E7BC33D87326
// Assembly location: D:\PDFGear\bin\SoundTouch.Net.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace SoundTouch.Assets;

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
        Strings.resourceMan = new ResourceManager("SoundTouch.Assets.Strings", typeof (Strings).GetTypeInfo().Assembly);
      return Strings.resourceMan;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Advanced)]
  internal static CultureInfo Culture
  {
    get => Strings.resourceCulture;
    set => Strings.resourceCulture = value;
  }

  internal static string Argument_CoefficientsFilterNotDivisible
  {
    get
    {
      return Strings.ResourceManager.GetString(nameof (Argument_CoefficientsFilterNotDivisible), Strings.resourceCulture);
    }
  }

  internal static string Argument_EmptyCoefficients
  {
    get
    {
      return Strings.ResourceManager.GetString(nameof (Argument_EmptyCoefficients), Strings.resourceCulture);
    }
  }

  internal static string Argument_ExcessiveSampleRate
  {
    get
    {
      return Strings.ResourceManager.GetString(nameof (Argument_ExcessiveSampleRate), Strings.resourceCulture);
    }
  }

  internal static string Argument_IllegalNumberOfChannels
  {
    get
    {
      return Strings.ResourceManager.GetString(nameof (Argument_IllegalNumberOfChannels), Strings.resourceCulture);
    }
  }

  internal static string Argument_SampleRateTooSmall
  {
    get
    {
      return Strings.ResourceManager.GetString(nameof (Argument_SampleRateTooSmall), Strings.resourceCulture);
    }
  }

  internal static string InvalidOperation_ChannelsUndefined
  {
    get
    {
      return Strings.ResourceManager.GetString(nameof (InvalidOperation_ChannelsUndefined), Strings.resourceCulture);
    }
  }

  internal static string InvalidOperation_CoefficientsNotInitialized
  {
    get
    {
      return Strings.ResourceManager.GetString(nameof (InvalidOperation_CoefficientsNotInitialized), Strings.resourceCulture);
    }
  }

  internal static string InvalidOperation_OutputPipeOverwrite
  {
    get
    {
      return Strings.ResourceManager.GetString(nameof (InvalidOperation_OutputPipeOverwrite), Strings.resourceCulture);
    }
  }

  internal static string InvalidOperation_OutputPipeUnset
  {
    get
    {
      return Strings.ResourceManager.GetString(nameof (InvalidOperation_OutputPipeUnset), Strings.resourceCulture);
    }
  }

  internal static string InvalidOperation_OutputUndefined
  {
    get
    {
      return Strings.ResourceManager.GetString(nameof (InvalidOperation_OutputUndefined), Strings.resourceCulture);
    }
  }

  internal static string InvalidOperation_SampleRateUndefined
  {
    get
    {
      return Strings.ResourceManager.GetString(nameof (InvalidOperation_SampleRateUndefined), Strings.resourceCulture);
    }
  }
}
