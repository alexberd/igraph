//http://thebuildingcoder.typepad.com/blog/2013/08/setting-a-default-3d-view-orientation.html

#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Runtime.InteropServices;
//using Revit.Elements;

#endregion

namespace RevitAddin_IGraph
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        ////load the c++ script "betweennness"
        //[DllImport("igraphtest.dll", EntryPoint = "betweenness")]
        //static extern IntPtr fnwrapper_intarr(
        //    [In, MarshalAs(UnmanagedType.LPArray, SizeConst = 1000)]
        //    int[] edges,
        //    int[] weights,
        //    int Size_vectors
        //);


        ////load the c++ script "memory release"
        //[DllImport("igraphtest.dll", EntryPoint = "memory_release")]
        //static extern void memory_release(IntPtr ptr);

        [DllImport("igraphtest.dll", EntryPoint = "betweenness")]

        public static extern IntPtr fnwrapper_intarr(
        [In, MarshalAs(UnmanagedType.LPArray, SizeConst = 1000)]
            int[] edges,
            int[] weights,
            int Size_vectors
        );


        [DllImport("igraphtest.dll", EntryPoint = "memory_release")]
        public static extern void memory_release(IntPtr ptr);



        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {

            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            // Access current selection

            Selection sel = uidoc.Selection;

            // Retrieve elements from database

            FilteredElementCollector RoomsFiltered
              = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfCategory(BuiltInCategory.INVALID)
                .OfClass(typeof(SpatialElement));

            FilteredElementCollector DoorsFiltered
              = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfCategory(BuiltInCategory.OST_Doors);
            //.OfClass(typeof(Opening));

            List<Element> Rooms = new List<Element>();
            List<Element> Doors = new List<Element>();

            foreach (Element i in RoomsFiltered)
            {
                Rooms.Add(i);
            }
            foreach (Element i in DoorsFiltered)
            {
                Doors.Add(i);
            }

            List<int> test = new List<int>();

            Dictionary<ElementId, int> room_map = new Dictionary<ElementId, int>(); //old_room, new_room
            //room_map.Add(55, 100);

            for (int i = 0; i < Rooms.Count; i++)
            {
                if (!room_map.ContainsKey(Rooms[i].Id))
                    room_map.Add(Rooms[i].Id, room_map.Count);
            }

            List<int> connections = new List<int>();
            List<int> lengths = new List<int>();

            for (int i = 0; i < Doors.Count; i++)
            {

                Autodesk.Revit.DB.FamilyInstance ii = Doors[i] as Autodesk.Revit.DB.FamilyInstance;
                SpatialElement thisToRoom = ii.ToRoom;
                SpatialElement thisFromRoom = ii.FromRoom;
                //If door is an "Internal Door"
                if (thisFromRoom != null & thisToRoom != null)
                {
                    connections.Add(room_map[thisToRoom.Id]);
                    connections.Add(room_map[thisFromRoom.Id]);

                    //calculate the distances between doors and rooms
                    double TRx = ((Autodesk.Revit.DB.LocationPoint)thisToRoom.Location).Point.X;
                    double TRy = ((Autodesk.Revit.DB.LocationPoint)thisToRoom.Location).Point.Y;
                    double FRx = ((Autodesk.Revit.DB.LocationPoint)thisToRoom.Location).Point.X;
                    double FRy = ((Autodesk.Revit.DB.LocationPoint)thisToRoom.Location).Point.Y;
                    double DRx = ((Autodesk.Revit.DB.LocationPoint)Doors[i].Location).Point.X;
                    double DRy = ((Autodesk.Revit.DB.LocationPoint)Doors[i].Location).Point.Y;
                    double RT_DR = Math.Sqrt(Math.Pow(TRx - DRx, 2) + Math.Pow(TRy - DRy, 2));
                    double FR_DR = Math.Sqrt(Math.Pow(FRx - DRx, 2) + Math.Pow(FRy - DRy, 2));
                    double dist = RT_DR + FR_DR;
                    lengths.Add((int)dist);
                }

            }



            /////////////////////////////////////////
            //Algorithm made just to pass arguments//
            /////////////////////////////////////////
            int Size_nodes = room_map.Count;
            int Size_vectors = connections.Count; //1000000;
            int Size_lengths = lengths.Count;
            List<float> results = new List<float>();


            if (Size_vectors > 2)
            {
                //int[] input = { 1, 2, 1, 3, 2, 3, 3, 4, 3, 5, 2, 5, 5, 7, 5, 6 };
                //int[] weights = { 1, 1, 1, 1, 1, 2, 1, 1 };

                //int Size_nodes1 = 8;//    100000000;
                //int Size_vectors1 = input.Length / 2; //=  100000000;

                //Console.WriteLine("Calculation Started");

                //IntPtr ptr1 = fnwrapper_intarr(input, weights, Size_vectors1);
                ////IntPtr ptr = fnwrapper_intarr(arr, Size_nodes, Size_vectors);
                //double[] result = new double[Size_nodes1];
                //Marshal.Copy(ptr1, result, 0, Size_nodes1);
                //Debug.Print(result.ToString());
                //memory_release(ptr1);

                ////string arr = "asd";
                int[] arr = new int[Size_vectors];
                int[] arr2 = new int[Size_lengths];
                //int[] arr = { 1, 2, 1, 3, 2, 3, 3, 4, 3, 5, 2, 5, 5, 7, 5, 6 };
                //int[] arr2 = { 1, 1, 1, 1, 1, 1, 1, 1 };

                //int[] input = { 1, 2, 1, 3, 2, 3, 3, 4, 3, 5, 2, 5, 5, 7, 5, 6 };

                //for (int i = 0; i < input.length; i++)
                //{
                //    arr[i] = input[i];
                //}

                for (int i = 0; i < connections.Count; i++)
                {
                    arr[i] = connections[i];
                    //results.Add(arr[i]);
                }


                for (int i = 0; i < lengths.Count; i++)
                {
                    arr2[i] = (int)lengths[i];
                    //results.Add(arr2[i]);
                }

                ///////////////////////////////////////////
                //Console.WriteLine("Hello World!");

                ////ShowMe();
                ////double [] alex= { 1, 2, 1, 3, 2, 3, 3, 4, 3, 5};
                ////int [] alex = main1(arr);
                ////////////////////////////////////////////

                IntPtr ptr = fnwrapper_intarr(arr, arr2, Size_vectors/2);
                //return arr;
                double[] result = new double[Size_nodes];
                Marshal.Copy(ptr, result, 0, Size_nodes);
                Debug.Print(result.ToString());
                memory_release(ptr);

                using (Transaction tx = new Transaction(doc))
                {
                    tx.Start("Transaction Name");
                    int Count = 0;
                    foreach (KeyValuePair<ElementId, int> entry in room_map)
                    {
                        //if (entry.Value==Count)
                        results.Add((float)result[Count]);
                        setParameterOfElement(doc.GetElement(entry.Key), "Comments", (result[Count]).ToString());

                        //setParameterOfElement(room_map[Rooms[i].Id.IntegerValue], "Comments", (result[i]).ToString());
                        Count++;
                    }
                    //setParameterOfElement
                    tx.Commit();
                }

            }


            //return results;
            //return lengths;
            //return test;



            // Filtered element collector is iterable
            //foreach (Element e in col)
            //{
            //    Debug.Print(e.Name);
            //}

            // Modify document within a transaction
            //using (Transaction tx = new Transaction(doc))
            //{
            //    tx.Start("Transaction Name");
            //    tx.Commit();
            //}

            return Result.Succeeded;
        }
        static void setParameterOfElement(Autodesk.Revit.DB.Element i, String p, String f)
        {
            foreach (Autodesk.Revit.DB.Parameter para in i.Parameters)
            {
                if (para.Definition.Name == p)
                {
                    //return para.AsString();
                    //using (Transaction t = new Transaction(doc, "Set Parameter"))
                    {
                        //t.Start();
                        para.Set(f);
                        //return;
                        ///t.Commit();
                    }
                }
            }
            //return null;
        }
    }
}
