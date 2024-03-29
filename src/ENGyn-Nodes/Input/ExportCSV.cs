﻿using System.Windows.Controls;
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Clash;
using TUM.CMS.VplControl.Core;
using System.Windows;
using System.Collections.Generic;

using System.IO;
using System.Text;

namespace ENGyn.Nodes
{
    public class ExportAsCSV : Node
    {
        public ExportAsCSV(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("Input", typeof(object));
            AddInputPortToNode("Path", typeof(object));
            AddOutputPortToNode("Output", typeof(ClashResult));



            AddControlToNode(new Label() { Content = "Title", FontSize = 13, FontWeight = FontWeights.Bold });


        }


        public override void Calculate()
        {
            var input = InputPorts[0].Data;
            var path = InputPorts[1].Data;
            if (input != null && path !=null)
            {
                if (MainTools.IsList(input))
                {
                    var t = input.GetType();
                    ExportCSV(path.ToString(), input);
                }

            }

        }

   
        public static void ExportCSV(string filePath, object data)
        {
            using (var writer = new StreamWriter(System.IO.Path.GetFullPath(filePath)))
            {

                //TODO cast object to list
                foreach (var line in (System.Collections.IList)data)
                {
                    int count = 0;
                    if (MainTools.IsList(line))
                    {
                        var t = line.GetType();
                        var l = (System.Collections.ArrayList)line ;
                        foreach (var entry in l)
                        {
                           
                            writer.Write(MainTools.Quoted((entry ?? "").ToString().Replace("\n", string.Empty)));
                            if (++count < l.Count)
                                writer.Write(",");
                        }
                        writer.WriteLine();

                    }
                    else {

                            writer.Write(MainTools.Quoted((line ?? "").ToString().Replace("\n", string.Empty)));
                            
                                writer.Write(",");

                        writer.WriteLine();
                    }
                    
                }
            }
        }


        public override Node Clone()
        {
            return new ExportAsCSV(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
    }
    
}