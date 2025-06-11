// Decompiled with JetBrains decompiler
// Type: HandyControl.Properties.Langs.Lang
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace HandyControl.Properties.Langs;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class Lang
{
  private static ResourceManager resourceMan;
  private static CultureInfo resourceCulture;

  internal Lang()
  {
  }

  [EditorBrowsable(EditorBrowsableState.Advanced)]
  public static ResourceManager ResourceManager
  {
    get
    {
      if (Lang.resourceMan == null)
        Lang.resourceMan = new ResourceManager("HandyControl.Properties.Langs.Lang", typeof (Lang).Assembly);
      return Lang.resourceMan;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Advanced)]
  public static CultureInfo Culture
  {
    get => Lang.resourceCulture;
    set => Lang.resourceCulture = value;
  }

  public static string All => Lang.ResourceManager.GetString(nameof (All), Lang.resourceCulture);

  public static string Am => Lang.ResourceManager.GetString(nameof (Am), Lang.resourceCulture);

  public static string Cancel
  {
    get => Lang.ResourceManager.GetString(nameof (Cancel), Lang.resourceCulture);
  }

  public static string Clear
  {
    get => Lang.ResourceManager.GetString(nameof (Clear), Lang.resourceCulture);
  }

  public static string Close
  {
    get => Lang.ResourceManager.GetString(nameof (Close), Lang.resourceCulture);
  }

  public static string CloseAll
  {
    get => Lang.ResourceManager.GetString(nameof (CloseAll), Lang.resourceCulture);
  }

  public static string CloseOther
  {
    get => Lang.ResourceManager.GetString(nameof (CloseOther), Lang.resourceCulture);
  }

  public static string Confirm
  {
    get => Lang.ResourceManager.GetString(nameof (Confirm), Lang.resourceCulture);
  }

  public static string ErrorImgPath
  {
    get => Lang.ResourceManager.GetString(nameof (ErrorImgPath), Lang.resourceCulture);
  }

  public static string ErrorImgSize
  {
    get => Lang.ResourceManager.GetString(nameof (ErrorImgSize), Lang.resourceCulture);
  }

  public static string Find => Lang.ResourceManager.GetString(nameof (Find), Lang.resourceCulture);

  public static string FormatError
  {
    get => Lang.ResourceManager.GetString(nameof (FormatError), Lang.resourceCulture);
  }

  public static string Interval10m
  {
    get => Lang.ResourceManager.GetString(nameof (Interval10m), Lang.resourceCulture);
  }

  public static string Interval1h
  {
    get => Lang.ResourceManager.GetString(nameof (Interval1h), Lang.resourceCulture);
  }

  public static string Interval1m
  {
    get => Lang.ResourceManager.GetString(nameof (Interval1m), Lang.resourceCulture);
  }

  public static string Interval2h
  {
    get => Lang.ResourceManager.GetString(nameof (Interval2h), Lang.resourceCulture);
  }

  public static string Interval30m
  {
    get => Lang.ResourceManager.GetString(nameof (Interval30m), Lang.resourceCulture);
  }

  public static string Interval30s
  {
    get => Lang.ResourceManager.GetString(nameof (Interval30s), Lang.resourceCulture);
  }

  public static string Interval5m
  {
    get => Lang.ResourceManager.GetString(nameof (Interval5m), Lang.resourceCulture);
  }

  public static string IsNecessary
  {
    get => Lang.ResourceManager.GetString(nameof (IsNecessary), Lang.resourceCulture);
  }

  public static string Jump => Lang.ResourceManager.GetString(nameof (Jump), Lang.resourceCulture);

  public static string LangComment
  {
    get => Lang.ResourceManager.GetString(nameof (LangComment), Lang.resourceCulture);
  }

  public static string Miscellaneous
  {
    get => Lang.ResourceManager.GetString(nameof (Miscellaneous), Lang.resourceCulture);
  }

  public static string NextPage
  {
    get => Lang.ResourceManager.GetString(nameof (NextPage), Lang.resourceCulture);
  }

  public static string No => Lang.ResourceManager.GetString(nameof (No), Lang.resourceCulture);

  public static string NoData
  {
    get => Lang.ResourceManager.GetString(nameof (NoData), Lang.resourceCulture);
  }

  public static string OutOfRange
  {
    get => Lang.ResourceManager.GetString(nameof (OutOfRange), Lang.resourceCulture);
  }

  public static string PageMode
  {
    get => Lang.ResourceManager.GetString(nameof (PageMode), Lang.resourceCulture);
  }

  public static string Pm => Lang.ResourceManager.GetString(nameof (Pm), Lang.resourceCulture);

  public static string PngImg
  {
    get => Lang.ResourceManager.GetString(nameof (PngImg), Lang.resourceCulture);
  }

  public static string PreviousPage
  {
    get => Lang.ResourceManager.GetString(nameof (PreviousPage), Lang.resourceCulture);
  }

  public static string ScrollMode
  {
    get => Lang.ResourceManager.GetString(nameof (ScrollMode), Lang.resourceCulture);
  }

  public static string Tip => Lang.ResourceManager.GetString(nameof (Tip), Lang.resourceCulture);

  public static string TooLarge
  {
    get => Lang.ResourceManager.GetString(nameof (TooLarge), Lang.resourceCulture);
  }

  public static string TwoPageMode
  {
    get => Lang.ResourceManager.GetString(nameof (TwoPageMode), Lang.resourceCulture);
  }

  public static string Unknown
  {
    get => Lang.ResourceManager.GetString(nameof (Unknown), Lang.resourceCulture);
  }

  public static string UnknownSize
  {
    get => Lang.ResourceManager.GetString(nameof (UnknownSize), Lang.resourceCulture);
  }

  public static string Yes => Lang.ResourceManager.GetString(nameof (Yes), Lang.resourceCulture);

  public static string ZoomIn
  {
    get => Lang.ResourceManager.GetString(nameof (ZoomIn), Lang.resourceCulture);
  }

  public static string ZoomOut
  {
    get => Lang.ResourceManager.GetString(nameof (ZoomOut), Lang.resourceCulture);
  }
}
