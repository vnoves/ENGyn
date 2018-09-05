﻿using System.Windows.Controls;
using System.Xml;
using Autodesk.Navisworks.Api;
using TUM.CMS.VplControl.Nodes;
using Autodesk.Navisworks.Api.Clash;
using TUM.CMS.VplControl.Core;
using System.Windows.Data;

using System.Collections.Generic;
using System.Windows;

namespace NW_GraphicPrograming.Nodes.Navisworks
{
    public class GetElementFromSearch : Node
    {
        public GetElementFromSearch(VplControl hostCanvas)
            : base(hostCanvas)
        {
            AddInputPortToNode("Document", typeof(object));
            AddInputPortToNode("SelectionSets", typeof(object));
            AddOutputPortToNode("Output", typeof(object));



            AddControlToNode(new Label() { Content = "Get Element From Search", FontSize = 13, FontWeight = FontWeights.Bold });


        }


        public override void Calculate()
        {
            var input = InputPorts[1].Data;
            var output = new List<ModelItemCollection>();

            if (input != null && InputPorts[0].Data != null && InputPorts[0].Data.GetType() == typeof(Document))
            {
                var document = InputPorts[0].Data as Document;
                var type = input.GetType();

                if (type == typeof(SelectionSet))
                {
                    var selectionSet = input as SelectionSet;

                    ModelItemCollection searchResults =
                    selectionSet.Search.FindAll(Autodesk.Navisworks.Api.Application.ActiveDocument, false);
                    output.Add(searchResults);

                }
                bool tt = type.GetType().IsGenericType;
                var TTT = type.GetGenericTypeDefinition();
               if (input.GetType().IsGenericType && input.GetType().GetGenericTypeDefinition() == typeof(List<>))
                {
                    foreach (var item in input as List<SelectionSet>)
                    {
                        var selectionSet = item as SelectionSet;
                       
                        ModelItemCollection searchResults =
                        selectionSet.Search.FindAll(Autodesk.Navisworks.Api.Application.ActiveDocument, false);
                        output.Add(searchResults);
                    }
                }

            }
           var objects =  new List<object>();
            foreach (var item in output)
            {
                List<ModelItem> modelitems = new List<ModelItem>();
                foreach (var model in item)
                {
                    modelitems.Add(model);
                }
                objects.Add(modelitems);
            }
            OutputPorts[0].Data = objects;
        }


        public override void SerializeNetwork(XmlWriter xmlWriter)
        {
            base.SerializeNetwork(xmlWriter);

            // add your xml serialization methods here
        }

        public override void DeserializeNetwork(XmlReader xmlReader)
        {
            base.DeserializeNetwork(xmlReader);

            // add your xml deserialization methods here
        }

        public override Node Clone()
        {
            return new GetElementFromSearch(HostCanvas)
            {
                Top = Top,
                Left = Left
            };

        }
    }

}