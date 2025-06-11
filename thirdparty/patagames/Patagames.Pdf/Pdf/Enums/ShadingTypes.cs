// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.ShadingTypes
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>Represents the shading type.</summary>
public enum ShadingTypes
{
  /// <summary>Invalid typeof shading</summary>
  Invalid,
  /// <summary>
  /// Type 1 (Function-Based) Shadings. Define the color of every point in the domain using a mathematical function(not necessarily smooth or continuous).
  /// </summary>
  FunctionBasedShading,
  /// <summary>
  /// Type 2 (Axial) Shadings. Define a color blend along a line between two points, optionally extended beyond the boundary points by continuing the boundary colors.
  /// </summary>
  AxialShading,
  /// <summary>
  /// Type 3 (Radial) Shadings. Define a blend between two circles, optionally extended beyond the boundary circles by continuing the boundary colors. This type of shading is commonly used to represent three-dimensional spheres and cones.
  /// </summary>
  RadialShading,
  /// <summary>
  /// Type 4 Shadings (Free-Form Gouraud-Shaded Triangle Meshes). Define a common construct used by many three-dimensional applications to represent complex colored and shaded shapes. Vertices are specified in free-form geometry.
  /// </summary>
  FreeFormGouraudTriangleMeshShading,
  /// <summary>
  /// Type 5 Shadings (Lattice-Form Gouraud-Shaded Triangle Meshes). Are based on the same geometrical construct as type 4 but with vertices specified as a pseudorectangular lattice.
  /// </summary>
  LatticeFormGouraudTriangleMeshShading,
  /// <summary>
  /// Type 6 Shadings (Coons Patch Meshes). Construct a shading from one or more color patches, each bounded by four cubic Bézier curves.
  /// </summary>
  CoonsPatchMeshShading,
  /// <summary>
  /// Type 7 Shadings (Tensor-Product Patch Meshes). Are similar to type 6 but with additional control points in each patch, affording greater control over color mapping.
  /// </summary>
  TensorProductPatchMeshShading,
}
