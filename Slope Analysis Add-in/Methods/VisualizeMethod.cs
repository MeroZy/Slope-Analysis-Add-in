using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;

namespace Slope_Analysis_Add_in.Methods
{
    public static class VisualizeMethod
    {
        private const string GreenShapeName = "Floor_Analysis_Green";
        private const string RedShapeName = "Floor_Analysis_Red";
        private const double Thickness = 0.01; 

        public static void Visualize(Document doc, List<Face> greenFaces, List<Face> redFaces)
        {

            using (var tns = new Transaction(doc, "Analyse"))
            {
                tns.Start();
                ClearExisting(doc);
                if (greenFaces != null && greenFaces.Any())
                {
                    CreateDirectShape(doc, greenFaces, new Color(0, 255, 0), GreenShapeName);
                }

                if (redFaces != null && redFaces.Any())
                {
                    CreateDirectShape(doc, redFaces, new Color(255, 0, 0), RedShapeName);
                }
                tns.Commit();
            }
        }

        public static void ClearExisting(Document doc)
        {
            var shapesToDelete = new FilteredElementCollector(doc)
                .OfClass(typeof(DirectShape))
                .Cast<DirectShape>()
                .Where(x => x.Name == GreenShapeName || x.Name == RedShapeName)
                .Select(x => x.Id)
                .ToList();

            if (shapesToDelete.Any())
            {
                    doc.Delete(shapesToDelete);

            }
        }

        private static void CreateDirectShape(Document doc, List<Face> faces, Color color, string shapeName) //gpt help
        {
            List<GeometryObject> solids = new List<GeometryObject>();

            foreach (Face face in faces)
            {
                try
                {
                    IList<CurveLoop> loops = face.GetEdgesAsCurveLoops(); //get face boundry

                    XYZ normal = XYZ.BasisZ;
                    if (face is PlanarFace pf)
                    {
                        normal = pf.FaceNormal;
                    }

                    //create extrustion
                    Solid solid = GeometryCreationUtilities.CreateExtrusionGeometry(loops, normal, Thickness);
                    solids.Add(solid);
                }
                catch
                {
                    continue;
                }
            }

            if (solids.Count == 0) return;

            DirectShape ds = DirectShape.CreateElement(doc, new ElementId(BuiltInCategory.OST_GenericModel));
            ds.Name = shapeName; // naming
            ds.SetShape(solids);
            ds.Pinned = true; // non selectable 

            // coloring
            OverrideGraphicSettings ogs = new OverrideGraphicSettings();
            ogs.SetSurfaceForegroundPatternId(GetSolidFillPatternId(doc));
            ogs.SetSurfaceForegroundPatternColor(color);

            doc.ActiveView.SetElementOverrides(ds.Id, ogs);
        }

        private static ElementId GetSolidFillPatternId(Document doc)
        {
            return new FilteredElementCollector(doc)
                .OfClass(typeof(FillPatternElement))
                .Cast<FillPatternElement>()
                .First(x => x.GetFillPattern().IsSolidFill)
                .Id;
        }
    }
}
