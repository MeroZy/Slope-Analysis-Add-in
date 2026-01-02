using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slope_Analysis_Add_in.Revit
{
    public class ExtEventHan : IExternalEventHandler
    {
        public Request Request { get; set; }
        public void Execute(UIApplication app)
        {
            switch (Request)
            {
                case Request.Analysis:
                    RvtUtlis.Analysis(ExtCmd.doc, ExtCmd.uidoc, RvtData.start_range, RvtData.end_range);
                    break;


                case Request.Reset:
                    using (var tns = new Transaction(ExtCmd.doc, "Clear Existing"))
                    {
                        tns.Start();
                        Methods.VisualizeMethod.ClearExisting(ExtCmd.doc);
                        tns.Commit();
                    }
                    break;

                case Request.SelectFloors:
                    RvtUtlis.SelectFloors(ExtCmd.uidoc);
                    break;
            }
            
        }

        public string GetName()
        {
            return "Slope Analysis";
        }
    }
    public enum Request
    {
        Analysis,
        Reset,
        SelectFloors
    }
}
