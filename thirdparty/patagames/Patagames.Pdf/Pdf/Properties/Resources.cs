// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Properties.Resources
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace Patagames.Pdf.Properties;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
internal class Resources
{
  private static ResourceManager resourceMan;
  private static CultureInfo resourceCulture;

  internal Resources()
  {
  }

  /// <summary>
  ///   Returns the cached ResourceManager instance used by this class.
  /// </summary>
  [EditorBrowsable(EditorBrowsableState.Advanced)]
  internal static ResourceManager resourceLoader
  {
    get
    {
      if (Patagames.Pdf.Properties.Resources.resourceMan == null)
      {
        string name = "Patagames.Pdf.Properties.Resources";
        Assembly assembly = Pdfium.FindResource(ref name);
        if (assembly == (Assembly) null)
          assembly = typeof (Patagames.Pdf.Properties.Resources).Assembly;
        Patagames.Pdf.Properties.Resources.resourceMan = new ResourceManager(name, assembly);
      }
      return Patagames.Pdf.Properties.Resources.resourceMan;
    }
  }

  /// <summary>
  ///   Overrides the current thread's CurrentUICulture property for all
  ///   resource lookups using this strongly typed resource class.
  /// </summary>
  [EditorBrowsable(EditorBrowsableState.Advanced)]
  internal static CultureInfo Culture
  {
    get => Patagames.Pdf.Properties.Resources.resourceCulture;
    set => Patagames.Pdf.Properties.Resources.resourceCulture = value;
  }

  internal static string GetStringResource(string name)
  {
    // ISSUE: reference to a compiler-generated field
    return Patagames.Pdf.Properties.Resources.resourceLoader.GetString(name, Patagames.Pdf.Properties.Resources.resourceCulture);
  }

  /// <summary>Looks up a localized string similar to "Approved".</summary>
  internal static string RuberStampApproved
  {
    get => Patagames.Pdf.Properties.Resources.GetStringResource(nameof (RuberStampApproved));
  }

  /// <summary>
  ///   Looks up a localized string similar to "Not Approved".
  /// </summary>
  internal static string RuberStampNotApproved
  {
    get => Patagames.Pdf.Properties.Resources.GetStringResource(nameof (RuberStampNotApproved));
  }

  /// <summary>Looks up a localized string similar to "Final".</summary>
  internal static string RuberStampFinal => Patagames.Pdf.Properties.Resources.GetStringResource(nameof (RuberStampFinal));

  /// <summary>
  ///   Looks up a localized string similar to "Confidential".
  /// </summary>
  internal static string RuberStampConfidential
  {
    get => Patagames.Pdf.Properties.Resources.GetStringResource(nameof (RuberStampConfidential));
  }

  /// <summary>
  ///   Looks up a localized string similar to "For Public Release".
  /// </summary>
  internal static string RuberStampForPublicRelease
  {
    get => Patagames.Pdf.Properties.Resources.GetStringResource(nameof (RuberStampForPublicRelease));
  }

  /// <summary>
  ///   Looks up a localized string similar to "Not For Public Release".
  /// </summary>
  internal static string RuberStampNotForPublicRelease
  {
    get => Patagames.Pdf.Properties.Resources.GetStringResource(nameof (RuberStampNotForPublicRelease));
  }

  /// <summary>
  ///   Looks up a localized string similar to "For Comment".
  /// </summary>
  internal static string RuberStampForComment
  {
    get => Patagames.Pdf.Properties.Resources.GetStringResource(nameof (RuberStampForComment));
  }

  /// <summary>
  ///   Looks up a localized string similar to "Experimental".
  /// </summary>
  internal static string RuberStampExperimental
  {
    get => Patagames.Pdf.Properties.Resources.GetStringResource(nameof (RuberStampExperimental));
  }

  /// <summary>Looks up a localized string similar to "Expired".</summary>
  internal static string RuberStampExpired
  {
    get => Patagames.Pdf.Properties.Resources.GetStringResource(nameof (RuberStampExpired));
  }

  /// <summary>
  ///   Looks up a localized string similar to "Top Secret".
  /// </summary>
  internal static string RuberStampTopSecret
  {
    get => Patagames.Pdf.Properties.Resources.GetStringResource(nameof (RuberStampTopSecret));
  }

  /// <summary>Looks up a localized string similar to "Sold".</summary>
  internal static string RuberStampSold => Patagames.Pdf.Properties.Resources.GetStringResource(nameof (RuberStampSold));

  /// <summary>Looks up a localized string similar to "As Is".</summary>
  internal static string RuberStampAsIs => Patagames.Pdf.Properties.Resources.GetStringResource(nameof (RuberStampAsIs));

  /// <summary>
  ///   Looks up a localized string similar to "Departmental".
  /// </summary>
  internal static string RuberStampDepartmental
  {
    get => Patagames.Pdf.Properties.Resources.GetStringResource(nameof (RuberStampDepartmental));
  }

  /// <summary>Looks up a localized string similar to "Draft".</summary>
  internal static string RuberStampDraft => Patagames.Pdf.Properties.Resources.GetStringResource(nameof (RuberStampDraft));
}
