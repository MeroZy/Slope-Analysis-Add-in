using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Slope_Analysis_Add_in.Revit;
using Slope_Analysis_Add_in.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Slope_Analysis_Add_in.Methods;

namespace Slope_Analysis_Add_in.Revit
{
    static class RvtUtlis
    {
        public static IList<Reference> selectedRefs;
        public static void SelectFloors(UIDocument uidoc)
        {
            selectedRefs = null;
            try
            {
                selectedRefs = uidoc.Selection.PickObjects(
                    ObjectType.Element,
                    new FloorSelectionFilter(),
                    "Select floors to analyze slopes");
                ExtCmd.Mainform.No_Selection.Text = selectedRefs.Count.ToString();
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                TaskDialog.Show("Slope Analysis", "Operation Canceled");
                ExtCmd.Mainform.Focus();
                return;
            }
            if (selectedRefs == null || selectedRefs.Count == 0)
            {
                TaskDialog.Show("Slope Analysis", "Operation Canceled : No floors were selected.");
                ExtCmd.Mainform.Focus();
                return;
            }
            ExtCmd.Mainform.Focus();
        }
        public static void Analysis(Document doc, UIDocument uidoc, double start, double end)
        {
            

            List<Face> inrange_green = new List<Face>();
            List<Face> outrange_red = new List<Face>();

            //start = 9;
            //end = 60;

            Color Red = new Color(255, 0, 0);
            Color Green = new Color(0, 255, 0);

            Options opt = new Options
            {
                ComputeReferences = true,
                DetailLevel = ViewDetailLevel.Fine
            };

            //double start_range = double.Parse(Mainform.tb_start_range.Text);
            //double end_range = double.Parse(Mainform.tb_end_range.Text);

            



            


                foreach (Reference referenceFloor in selectedRefs)
                {
                    Floor floor = doc.GetElement(referenceFloor) as Floor;

                    if (floor == null) continue;

                    GeometryElement geomElem = floor.get_Geometry(opt);

                    foreach (GeometryObject geomObj in geomElem)
                    {
                        if (geomObj is Solid solid)
                        {
                            if (solid.Volume == 0 || solid.Faces.IsEmpty) continue;

                            foreach (Face face in solid.Faces)
                            {
                                if (face is PlanarFace pf)
                                {

                                    if (pf.FaceNormal.Z > 0.001) //tollerance for negelct vertical faces
                                    {

                                        double angleRad = pf.FaceNormal.AngleTo(XYZ.BasisZ);
                                        double slopePct = angleRad.Rad2Pct(); //%

                                        if (start <= slopePct && slopePct <= end)
                                        {
                                            inrange_green.Add(pf);
                                        }
                                        else
                                        {
                                            outrange_red.Add(pf);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                VisualizeMethod.Visualize(doc, inrange_green, outrange_red);
                
        }
        public class FloorSelectionFilter : ISelectionFilter
        {
            public bool AllowElement(Element elem)
            {
                return elem is Floor;
            }

            public bool AllowReference(Reference reference, XYZ position)
            {
                return false;
            }
        }
    }
}
