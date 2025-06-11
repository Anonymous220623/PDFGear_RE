// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Properties.Error
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
internal class Error
{
  private static ResourceManager resourceMan;
  private static CultureInfo resourceCulture;

  internal Error()
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
      if (Error.resourceMan == null)
      {
        string name = "Patagames.Pdf.Properties.Error";
        Assembly assembly = Pdfium.FindResource(ref name);
        if (assembly == (Assembly) null)
          assembly = typeof (Error).Assembly;
        Error.resourceMan = new ResourceManager(name, assembly);
      }
      return Error.resourceMan;
    }
  }

  /// <summary>
  ///   Overrides the current thread's CurrentUICulture property for all
  ///   resource lookups using this strongly typed resource class.
  /// </summary>
  [EditorBrowsable(EditorBrowsableState.Advanced)]
  internal static CultureInfo Culture
  {
    get => Error.resourceCulture;
    set => Error.resourceCulture = value;
  }

  internal static string GetStringResource(string name)
  {
    // ISSUE: reference to a compiler-generated field
    return Error.resourceLoader.GetString(name, Error.resourceCulture);
  }

  /// <summary>
  /// Localized resource similar to "PointerManagerItem.Key is empty"
  /// </summary>
  public static string err0001 => Error.GetStringResource(nameof (err0001));

  /// <summary>
  /// Localized resource similar to "Unable to load library {0}"
  /// </summary>
  public static string err0002 => Error.GetStringResource(nameof (err0002));

  /// <summary>
  /// Localized resource similar to "File length cannot be less than zero"
  /// </summary>
  public static string err0003 => Error.GetStringResource(nameof (err0003));

  /// <summary>
  /// Localized resource similar to "Destination's name cannot be null or empty string"
  /// </summary>
  public static string err0004 => Error.GetStringResource(nameof (err0004));

  /// <summary>
  /// Localized resource similar to "Bookmark's title cannot be null or empty string"
  /// </summary>
  public static string err0005 => Error.GetStringResource(nameof (err0005));

  /// <summary>
  /// Localized resource similar to "Parametr 'index' must be between {0} and {1}"
  /// </summary>
  public static string err0006 => Error.GetStringResource(nameof (err0006));

  /// <summary>
  /// Localized resource similar to "PdfForm object is not Initialized. Please load document whith this object as parameter"
  /// </summary>
  public static string err0007 => Error.GetStringResource(nameof (err0007));

  /// <summary>
  /// Localized resource similar to "This instance of the PdfForms class already assigned with another document"
  /// </summary>
  public static string err0008 => Error.GetStringResource(nameof (err0008));

  /// <summary>
  /// Localized resource similar to "Document is not yet available"
  /// </summary>
  public static string err0009 => Error.GetStringResource(nameof (err0009));

  /// <summary>
  /// Localized resource similar to "Forms is not yet available"
  /// </summary>
  public static string err0010 => Error.GetStringResource(nameof (err0010));

  /// <summary>
  /// Localized resource similar to "This page is not yet available"
  /// </summary>
  public static string err0011 => Error.GetStringResource(nameof (err0011));

  /// <summary>
  /// Localized resource similar to "Unable to load DLL 'pdfium(.dll/.dylib)': The specified module could not be found.  Please make sure that you copy it to the application folder.  Alternatively, you can specify the full path to the pdfium(.dll/.dylib) file using specificPath parameter in the initialization method"
  /// </summary>
  public static string err0012 => Error.GetStringResource(nameof (err0012));

  /// <summary>
  /// Localized resource similar to "To set the document property instead of using LoadDocument method set the AllowSetDocument property to True. Please read the documentation before you do it."
  /// </summary>
  public static string err0013 => Error.GetStringResource(nameof (err0013));

  /// <summary>
  /// Localized resource similar to "Document cannot be null"
  /// </summary>
  public static string err0014 => Error.GetStringResource(nameof (err0014));

  /// <summary>
  /// Localized resource similar to "Document not properly loaded"
  /// </summary>
  public static string err0015 => Error.GetStringResource(nameof (err0015));

  /// <summary>
  /// Localized resource similar to "Font name cannot be null or empty string"
  /// </summary>
  public static string err0016 => Error.GetStringResource(nameof (err0016));

  /// <summary>
  /// Localized resource similar to "The operation cannot be completed because the SDK is not initialized.  You have to call PdfCommon.Initialize() method before you can call any PDF processing functions."
  /// </summary>
  public static string err0017 => Error.GetStringResource(nameof (err0017));

  /// <summary>
  /// Localized resource similar to "All instances of '{0}' class  should be disposed to prevent the memory leaks."
  /// </summary>
  public static string err0018 => Error.GetStringResource(nameof (err0018));

  /// <summary>
  /// Localized resource similar to "Unexpected error code."
  /// </summary>
  public static string err0019 => Error.GetStringResource(nameof (err0019));

  /// <summary>
  /// Localized resource similar to "File not in PDF format or corrupted."
  /// </summary>
  public static string err0020 => Error.GetStringResource(nameof (err0020));

  /// <summary>
  /// Localized resource similar to "File not found or could not be opened."
  /// </summary>
  public static string err0021 => Error.GetStringResource(nameof (err0021));

  /// <summary>
  /// Localized resource similar to "Requested font not found."
  /// </summary>
  public static string err0022 => Error.GetStringResource(nameof (err0022));

  /// <summary>
  /// Localized resource similar to "Image object is empty."
  /// </summary>
  public static string err0023 => Error.GetStringResource(nameof (err0023));

  /// <summary>
  /// Localized resource similar to "Password required or incorrect password."
  /// </summary>
  public static string err0024 => Error.GetStringResource(nameof (err0024));

  /// <summary>
  /// Localized resource similar to "The requested operation cannot be completed due to a license restrictions."
  /// </summary>
  public static string err0025 => Error.GetStringResource(nameof (err0025));

  /// <summary>
  /// Localized resource similar to "Page not found or content error."
  /// </summary>
  public static string err0026 => Error.GetStringResource(nameof (err0026));

  /// <summary>Localized resource similar to "Unknown error."</summary>
  public static string err0027 => Error.GetStringResource(nameof (err0027));

  /// <summary>
  /// Localized resource similar to "Can't convert PdfBitmap into System.Drawing.Bitmap."
  /// </summary>
  public static string err0028 => Error.GetStringResource(nameof (err0028));

  /// <summary>
  /// Localized resource similar to "Unsupported security scheme."
  /// </summary>
  public static string err0029 => Error.GetStringResource(nameof (err0029));

  /// <summary>
  /// Localized resource similar to "An element with the same key already exists in the Dictionary object."
  /// </summary>
  public static string err0030 => Error.GetStringResource(nameof (err0030));

  /// <summary>
  /// Localized resource similar to "The given object is a part of list of indirect objects. Please use the Add(PdfTypeBase, PdfIndirectList) method to add this item."
  /// </summary>
  public static string err0031 => Error.GetStringResource(nameof (err0031));

  /// <summary>
  /// Localized resource similar to "The given object is a part of list of indirect objects. Please use the Insert(int, PdfTypeBase, PdfIndirectList) method to insert this item."
  /// </summary>
  public static string err0032 => Error.GetStringResource(nameof (err0032));

  /// <summary>
  /// Localized resource similar to "The given object is a part of list of indirect objects. Please use the SetAt(int, PdfTypeBase, PdfIndirectList) method instead."
  /// </summary>
  public static string err0033 => Error.GetStringResource(nameof (err0033));

  /// <summary>
  /// Localized resource similar to "Can't find parent dictionary for given bookmark"
  /// </summary>
  public static string err0035 => Error.GetStringResource(nameof (err0035));

  /// <summary>
  /// Looks up a localized string similar to The annotations associated with given page  are invalid.
  /// </summary>
  public static string err0036 => Error.GetStringResource(nameof (err0036));

  /// <summary>
  /// Looks up a localized string similar to Can't create indirect object.
  /// </summary>
  public static string err0037 => Error.GetStringResource(nameof (err0037));

  /// <summary>
  /// Looks up a localized string similar to Given annotation is invalid.
  /// </summary>
  public static string err0038 => Error.GetStringResource(nameof (err0038));

  /// <summary>
  /// Looks up a localized string similar to Unknown color format.
  /// </summary>
  public static string err0039 => Error.GetStringResource(nameof (err0039));

  /// <summary>
  /// Looks up a localized string similar to The given popup annotation is invalid.
  /// </summary>
  public static string err0040 => Error.GetStringResource(nameof (err0040));

  /// <summary>
  /// Looks up a localized string similar to Unknown type  of annotations relationship: {0}.
  /// </summary>
  public static string err0041 => Error.GetStringResource(nameof (err0041));

  /// <summary>
  /// Looks up a localized string similar to Unexpected type of the '{0}' entry.
  /// </summary>
  public static string err0042 => Error.GetStringResource(nameof (err0042));

  /// <summary>
  /// Looks up a localized string similar to Non standard icon name.Please use ExtendedIconName property instead
  /// </summary>
  public static string err0043 => Error.GetStringResource(nameof (err0043));

  /// <summary>
  /// Looks up a localized string similar to The specified entry not found.Entry name:{0}
  /// </summary>
  public static string err0044 => Error.GetStringResource(nameof (err0044));

  /// <summary>
  /// Looks up a localized string similar to The '{0}' entry has unexpected value
  /// </summary>
  public static string err0045 => Error.GetStringResource(nameof (err0045));

  /// <summary>
  /// Looks up a localized string similar to The CalloutLine array must be an array of 2 or 3 points.
  /// </summary>
  public static string err0046 => Error.GetStringResource(nameof (err0046));

  /// <summary>
  /// Looks up a localized string similar to Valid value(s) for the {0} property {1}
  /// </summary>
  public static string err0047 => Error.GetStringResource(nameof (err0047));

  /// <summary>
  /// Looks up a localized string similar to Given dictionary is not valid PdfTypeDictionary object
  /// </summary>
  public static string err0048 => Error.GetStringResource(nameof (err0048));

  /// <summary>
  /// Looks up a localized string similar to The Maximum number of items has been exceeded
  /// </summary>
  public static string err0049 => Error.GetStringResource(nameof (err0049));

  /// <summary>
  /// Looks up a localized string similar to Unexpected type of object. Expected: {0}; Actual: {1}
  /// </summary>
  public static string err0050 => Error.GetStringResource(nameof (err0050));

  /// <summary>
  /// Localized resource similar to Collection is read-only.
  /// </summary>
  public static string err0051 => Error.GetStringResource(nameof (err0051));

  /// <summary>Localized resource similar to Fatal runtime error.</summary>
  public static string err0052 => Error.GetStringResource(nameof (err0052));

  /// <summary>
  /// Localized resource similar to Fill Mode may be None, Alternate or Winding..
  /// </summary>
  public static string err0053 => Error.GetStringResource(nameof (err0053));

  /// <summary>
  /// Localized resource similar to The reference list has a non-zero length, however, a null pointer was returned..
  /// </summary>
  public static string err0054 => Error.GetStringResource(nameof (err0054));

  /// <summary>
  /// Localized resource similar to The 'Trapped' tag is not allowed here. Please use Document.Info instead.
  /// </summary>
  public static string err0055 => Error.GetStringResource(nameof (err0055));

  /// <summary>
  ///   Your license key is out of date and cannot be used with this version of Pdfium.Net SDK. Please downgrade to the previous version allowed for using with this key. Please note that you can use any versions which were released within 12 months from the date of purchase. To use latest version of SDK, you must pass the renewed key obtained during renewal of maintenance subscription.If you have an active maintenance subscription, but you have not received the renewed key, please contact to support@patagames.com
  /// </summary>
  public static string err0056 => Error.GetStringResource(nameof (err0056));

  /// <summary>
  ///  Localized resource similar to "This method cannot accept newly created documents. If you pass such a document as an argument, the RequiredDataIsAbsentException will be thrown. To get the instance of the PdfDocument class that can be passed to the FromPdfDocument method, you must save the document to a temporary file or an array of bytes, and then open it with the PdfDocument.Load."
  /// </summary>
  public static string err0057 => Error.GetStringResource(nameof (err0057));

  /// <summary>
  ///  Localized resource similar to "System.Drawing.Image does not support {0} pixel format on this platform."
  /// </summary>
  public static string err0058 => Error.GetStringResource(nameof (err0058));

  /// <summary>
  ///  Localized resource similar to "The trial period for Pdfium.Net SDK has expired."
  /// </summary>
  public static string err0059 => Error.GetStringResource(nameof (err0059));

  /// <summary>
  ///  Localized resource similar to "Parameter '{0}' cannot be null or empty string."
  /// </summary>
  public static string err0060 => Error.GetStringResource(nameof (err0060));

  /// <summary>
  ///  Localized resource similar to "An element with the same name already exists in the PdfNameTreeCollection."
  /// </summary>
  public static string err0061 => Error.GetStringResource(nameof (err0061));

  /// <summary>
  ///  Localized resource similar to "The NameTree item could not be added."
  /// </summary>
  public static string err0062 => Error.GetStringResource(nameof (err0062));

  /// <summary>
  ///  Localized resource similar to "An array of functions cannot contain null."
  /// </summary>
  public static string err0063 => Error.GetStringResource(nameof (err0063));

  /// <summary>
  ///  Localized resource similar to "Parameter '{0}' cannot be {1}."
  /// </summary>
  public static string err0064 => Error.GetStringResource(nameof (err0064));

  /// <summary>
  ///  Localized resource similar to "Unknown function type."
  /// </summary>
  public static string err0065 => Error.GetStringResource(nameof (err0065));

  /// <summary>
  ///  Localized resource similar to "The underlying Dictionary/Stream is not in the correct format."
  /// </summary>
  public static string err0066 => Error.GetStringResource(nameof (err0066));

  /// <summary>
  ///  Localized resource similar to "The specified {0} is already in use by another {1}."
  /// </summary>
  public static string err0067 => Error.GetStringResource(nameof (err0067));

  /// <summary>
  ///  Localized resource similar to "The parent field is terminal. Only nonterminal fields can have child fields."
  /// </summary>
  public static string err0068 => Error.GetStringResource(nameof (err0068));

  /// <summary>
  ///  Localized resource similar to "The specified field cannot contain more than one control."
  /// </summary>
  public static string err0069 => Error.GetStringResource(nameof (err0069));

  /// <summary>
  ///  Localized resource similar to "The specified field cannot contain controls, since it is the parent of another field."
  /// </summary>
  public static string err0070 => Error.GetStringResource(nameof (err0070));

  /// <summary>
  ///  Localized resource similar to "The field '{0}' has not yet been added to the DOM. Please call Document.FormFill.InterForm.Fields.Add(field) method before access its properties or methods."
  /// </summary>
  public static string err0071 => Error.GetStringResource(nameof (err0071));

  /// <summary>
  ///  Localized resource similar to "This PdfControl has not yet been added to the DOM. Add the associated field to the Document.FormFill.InterForm.Fields collection, or call the Document.FormFill.InterForm.ReloadForms() method if the field is already in the collection."
  /// </summary>
  public static string err0072 => Error.GetStringResource(nameof (err0072));

  /// <summary>
  ///  Localized resource similar to "Only fields with at least one control can be added to the collection."
  /// </summary>
  public static string err0073 => Error.GetStringResource(nameof (err0073));

  /// <summary>
  /// Localized resource similar to "You need to have at least a Developer Small Business License to be able  to complete this operation."
  /// </summary>
  public static string errLicense1 => Error.GetStringResource(nameof (errLicense1));

  /// <summary>
  /// Localized resource similar to "You need to have at least a LITE License to be able  to complete this operation."
  /// </summary>
  public static string errLicense2 => Error.GetStringResource(nameof (errLicense2));

  /// <summary>
  /// Localized resource similar to "You need to have a  full license to be able  to complete this operation."
  /// </summary>
  public static string errLicense3 => Error.GetStringResource(nameof (errLicense3));

  /// <summary>
  /// Localized resource similar to "The specified license key is not supported on this platform."
  /// </summary>
  public static string errLicensePlatform => Error.GetStringResource(nameof (errLicensePlatform));

  /// <summary>
  /// Localized resource similar to "With the trial version the documents which size is smaller than 1024 Kb, or greater than 10 Mb can be loaded without any restrictions. For other documents the allowed ranges is 1.5 - 2 Mb; 2.5 - 3 Mb; 3.5 - 4 Mb and so on."
  /// </summary>
  public static string TrialLoadDocument => Error.GetStringResource(nameof (TrialLoadDocument));

  /// <summary>
  /// Localized resource similar to "Trial version. Please visit http://patagames.com"
  /// </summary>
  public static string TrialMsg => Error.GetStringResource(nameof (TrialMsg));
}
