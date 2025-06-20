﻿// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.AnnotationFlags
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>
/// Flags used by <see cref="P:Patagames.Pdf.Net.Annotations.PdfAnnotation.Flags" />
/// </summary>
/// <remarks>
/// If the NoZoom flag is set, the annotation always maintains the same fixed size on
/// the screen and is unaffected by the magnification level at which the page itself is
/// displayed. Similarly, if the NoRotate flag is set, the annotation retains its original
/// orientation on the screen when the page is rotated.
/// <para>
/// In either case, the annotation’s position is determined by the coordinates of the
/// upper-left corner of its annotation rectangle, as defined by the Rect entry in the
/// annotation dictionary and interpreted in the default user space of the page.When
/// the default user space is scaled or rotated, the positions of the other three corners
/// of the annotation rectangle are different in the altered user space than they were
/// in the original user space.The viewer application performs this alteration automatically.
/// However, it does not actually change the annotation’s Rect entry, which
/// continues to describe the annotation’s relationship with the unscaled, unrotated
/// user space.
/// </para>
/// </remarks>
[Flags]
public enum AnnotationFlags
{
  /// <summary>No any flags are setted.</summary>
  None = 0,
  /// <summary>
  /// If set, do not display the annotation if it does not belong to one of the standard annotation types and no annotation handler is available. If clear, display such an unknown annotation using an appearance stream specified by its appearance dictionary, if any.
  /// </summary>
  Invisible = 1,
  /// <summary>
  /// If set, do not display or print the annotation or allow it to interact
  /// with the user, regardless of its annotation type or whether an annotation
  /// handler is available. In cases where screen space is limited, the ability to hide
  /// and show annotations selectively can be used in combination with appearance
  /// streams to display auxiliary pop-up
  /// information similar in function to online help systems.
  /// </summary>
  Hidden = 2,
  /// <summary>
  /// If set, print the annotation when the page is printed. If clear, never
  /// print the annotation, regardless of whether it is displayed on the screen. This
  /// can be useful, for example, for annotations representing interactive pushbuttons, which would serve no meaningful purpose on the printed page.
  /// </summary>
  Print = 4,
  /// <summary>
  /// If set, do not scale the annotation’s appearance to match the magnification of the page. The location of the annotation on the page (defined by
  /// the upper-left corner of its annotation rectangle) remains fixed, regardless of
  /// the page magnification. See remarks for further discussion.
  /// </summary>
  NoZoom = 8,
  /// <summary>
  /// If set, do not rotate the annotation’s appearance to match the rotation of the page. The upper-left corner of the annotation rectangle remains in
  /// a fixed location on the page, regardless of the page rotation. See remarks for further discussion.
  /// </summary>
  NoRotate = 16, // 0x00000010
  /// <summary>
  /// If set, do not display the annotation on the screen or allow it to
  /// interact with the user. The annotation may be printed (depending on the
  /// setting of the Print flag) but should be considered hidden for purposes of onscreen display and user interaction.
  /// </summary>
  NoView = 32, // 0x00000020
  /// <summary>
  /// If set, do not allow the annotation to interact with the user. The
  /// annotation may be displayed or printed (depending on the settings of the
  /// NoView and Print flags) but should not respond to mouse clicks or change its
  /// appearance in response to mouse motions.
  /// <note type="note">This flag is ignored for widget annotations; its function is subsumed by the ReadOnly flag of the associated form field </note>
  /// </summary>
  ReadOnly = 64, // 0x00000040
  /// <summary>
  /// If set, do not allow the annotation to be deleted or its properties (including position and size) to be modified by the user. However, this flag does
  /// not restrict changes to the annotation’s contents, such as the value of a form field.
  /// </summary>
  Locked = 128, // 0x00000080
  /// <summary>
  /// If set, invert the interpretation of the NoView flag for certain
  /// events. A typical use is to have an annotation that appears only when a mouse cursor is held over it.
  /// </summary>
  ToggleNoView = 256, // 0x00000100
  /// <summary>
  /// If set, do not allow the contents of the annotation to be modified by
  /// the user. This flag does not restrict deletion of the annotation or changes to
  /// other annotation properties, such as position and size.
  /// </summary>
  LockedContents = 512, // 0x00000200
}
