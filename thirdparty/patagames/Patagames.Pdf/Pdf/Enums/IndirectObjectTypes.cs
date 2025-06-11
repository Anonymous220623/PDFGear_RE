// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.IndirectObjectTypes
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>
/// The following enumerator describe each object type, as well as how to create and refer to indirect objects.
/// </summary>
public enum IndirectObjectTypes
{
  /// <summary>Unknown object type</summary>
  Invalid,
  /// <summary>
  /// PDF provides boolean objects identified by the keywords true and false.
  /// Boolean objects can be used as the values of array elements and dictionary entries,
  /// and can also occur in PostScript calculator functions as the results of boolean
  /// and relational operators and as operands to the conditional operators if and ifelse.
  /// </summary>
  Boolean,
  /// <summary>
  /// PDF provides two types of numeric objects: integer and real. Integer objects represent mathematical integers within a certain interval centered at 0. Real objects approximate mathematical real numbers, but with limited range and precision;
  /// </summary>
  /// <remarks>
  /// <para>PDF provides two types of numeric objects: integer and real. Integer objects represent mathematical integers within a certain interval centered at 0. Real objects approximate mathematical real numbers, but with limited range and precision; they are typically represented in fixed-point form rather than floating-point form. The range and precision of numbers are limited by the internal representations used in the computer on which the PDF consumer application is running; Appendix C gives these limits for typical implementations.</para>
  /// <para>An integer is written as one or more decimal digits optionally preceded by a sign:</para>
  /// <para>123 43445 +17 −98 0</para>
  /// <para>The value is interpreted as a signed decimal integer and is converted to an integer object. If it exceeds the implementation limit for integers, it is converted to a real object.</para>
  /// <para>A real value is written as one or more decimal digits with an optional sign and a leading, trailing, or embedded period (decimal point):</para>
  /// <para>34.5 −3.62 +123.6 4. −.002 0.0</para>
  /// <para>The value is interpreted as a real number and is converted to a real object. If it exceeds the implementation limit for real numbers, an error occurs.</para>
  /// <note type="note">PDF does not support the PostScript syntax for numbers with nondecimal radices (such as 16#FFFE) or in exponential format (such as 6.02E23).</note>
  /// </remarks>
  Number,
  /// <summary>
  /// A string object consists of a series of bytes—unsigned integer values in the range 0 to 255.
  /// </summary>
  /// <remarks>
  /// <para>String objects are not integer objects, but are stored in a more compact format. The length of a string may be subject to implementation limits;</para>
  /// <para>String objects can be written in two ways:</para>
  /// <list type="bullet">
  /// <item>As a sequence of literal characters enclosed in parentheses ( ); see “Literal Strings,” below”</item>
  /// <item>As hexadecimal data enclosed in angle brackets; see “Hexadecimal Strings”</item>
  /// </list>
  /// <para>Literal Strings</para>
  /// <para>A literal string is written as an arbitrary number of characters enclosed in parentheses. Any characters may appear in a string except unbalanced parentheses and the backslash, which must be treated specially. Balanced pairs of parentheses within a string require no special treatment.</para>
  /// <para>Hexadecimal Strings</para>
  /// <para>Strings may also be written in hexadecimal form, which is useful for including arbitrary binary data in a PDF file. A hexadecimal string is written as a sequence of hexadecimal digits (0–9 and either A–F or a–f) enclosed within angle brackets (&lt; and &gt;):</para>
  /// <para>&lt;4E6F762073686D6F7A206B6120706F702E&gt;</para>
  /// <para>Each pair of hexadecimal digits defines one byte of the string. White-space characters (such as space, tab, carriage return, line feed, and form feed) are ignored.</para>
  /// <para>If the final digit of a hexadecimal string is missing—that is, if there is an odd number of digits—the final digit is assumed to be 0. For example:</para>
  /// <para>&lt;901FA3&gt;</para>
  /// <para>is a 3-byte string consisting of the characters whose hexadecimal codes are 90, 1F, and A3, but</para>
  /// <para>&lt;901FA&gt;</para>
  /// <para>is a 3-byte string containing the characters whose hexadecimal codes are 90, 1F, and A0.</para>
  /// </remarks>
  String,
  /// <summary>
  /// A name object is an atomic symbol uniquely defined by a sequence of characters. Uniquely defined means that any two name objects made up of the same sequence of characters are identically the same object. Atomic means that a name has no internal structure; although it is defined by a sequence of characters, those characters are not considered elements of the name.
  /// </summary>
  /// <remarks>
  /// <para>A slash character (/) introduces a name. The slash is not part of the name but is a prefix indicating that the following sequence of characters constitutes a name. There can be no white-space characters between the slash and the first character in the name. The name may include any regular characters, but not delimiter or white-space characters (see Section 3.1, “Lexical Conventions”). Uppercase and lowercase letters are considered distinct: /A and /a are different names. The following examples are valid literal names:</para>
  /// <para>
  /// /Name1
  /// /Name1<br />
  /// /ASomewhatLongerName<br />
  /// /A; Name_With−Various***Characters?<br />
  /// /1.2<br />
  /// /$$<br />
  /// /@pattern<br />
  /// /.notdef<br />
  /// </para>
  /// <note type="note">The token / (a slash followed by no regular characters) is a valid name.</note>
  /// <para>Beginning with PDF 1.2, any character except null (character code 0) may be included in a name by writing its 2-digit hexadecimal code, preceded by the number sign character (#); </para>
  /// </remarks>
  Name,
  /// <summary>
  /// An array object is a one-dimensional collection of objects arranged sequentially. Unlike arrays in many other computer languages, PDF arrays may be heterogeneous; that is, an array’s elements may be any combination of numbers, strings, dictionaries, or any other objects, including other arrays.
  /// </summary>
  Array,
  /// <summary>
  /// A dictionary object is an associative table containing pairs of objects, known as the dictionary’s entries. The first element of each entry is the key and the second element is the value. The key must be a name (unlike dictionary keys in PostScript, which may be objects of any type). The value can be any kind of object, including another dictionary.
  /// </summary>
  Dictionary,
  /// <summary>
  /// A stream object, like a string object, is a sequence of bytes. However, a PDF application can read a stream incrementally, while a string must be read in its entirety. Furthermore, a stream can be of unlimited length, whereas a string is subject to an implementation limit. For this reason, objects with potentially large amounts of data, such as images and page descriptions, are represented as streams.
  /// </summary>
  /// <remarks>
  /// <note type="note">As with strings, this section describes only the syntax for writing a stream as a sequence of bytes. What those bytes represent is determined by the context in which the stream is referenced.</note>
  /// </remarks>
  Stream,
  /// <summary>
  /// The null object has a type and value that are unequal to those of any other object. There is only one object of type null, denoted by the keyword null. An indirect object reference to a nonexistent object is treated the same as a null object. Specifying the null object as the value of a dictionary entry is equivalent to omitting the entry entirely.
  /// </summary>
  Null,
  /// <summary>
  /// Any object in a PDF file may be labeled as an indirect object.
  /// This gives the object a unique object identifier by which other objects can refer to it
  /// (for example, as an element of an array or as the value of a dictionary entry).
  /// </summary>
  /// <remarks>
  /// The object identifier consists of two parts:
  /// <list type="bullet">
  /// <item>A positive integer object number.
  /// Indirect objects are often numbered sequentially within a PDF file, but this is not required;
  /// object numbers may be assigned in any arbitrary order.</item>
  /// <item>A non-negative integer generation number. In a newly created file,
  /// all indirect objects have generation numbers of 0. Nonzero generation numbers may be introduced when
  /// the file is later updated; </item>
  /// </list>
  /// Together, the combination of an object number and a generation number uniquely identifies an indirect object.
  /// The object retains the same object number and generation number throughout its existence, even if its value is modified.
  /// </remarks>
  Reference,
}
