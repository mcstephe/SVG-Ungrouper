using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace SVG_Ungrouper
{
    /// <summary>
    /// Automatically parses an SVG file and ungroups all objects.
    /// Works on a single layer named "layer1". 
    /// Supports Inkscape SVG schema. 
    /// http://stackoverflow.com/questions/14967293/remove-parent-node-without-childs-nodes 
    /// http://stackoverflow.com/questions/14592213/selenium-webdriver-clicking-on-elements-within-an-svg-using-xpath
    /// </summary>
    class Program
    {

        static void Main(string[] args)
        {
            string inputFile = "";
            string outputFile = "";

            if (args == null)
            {
                Console.WriteLine("Please specify an input and output filename.");
                
            }

            try {
                inputFile = args[0];
            }
            catch
            {
                Console.WriteLine("Error 1.");
                return; 
            }
            try
            {
                outputFile = args[1];
            }
            catch
            {
                Console.WriteLine("Error 2.");
                return; 
            }
            
            Console.WriteLine("Reading " + inputFile);
                        
            XElement root = XElement.Load(inputFile);

            var parent = root.XPathSelectElement(".//*[local-name()=\"g\" and @id=\"layer1\"]");
            
            while (HasGroup(parent))
            {
                var removes = root.XPathSelectElements(".//*[local-name()=\"g\" and @id!=\"layer1\"]");

                foreach (XElement node in removes.ToArray())
                {
                    //node.Remove();
                    parent.Add(node.Elements());


                    //node.AddBeforeSelf(node.Elements());
                    node.Remove();
                }

            }
            

            Console.WriteLine("Writing to: " + outputFile);
            root.Save(outputFile);


        }

        /// <summary>
        /// True/false condition whether XElement has grouped children.
        /// </summary>
        /// <param name="parentToCheck">Element to check for grouped children.</param>
        /// <returns>True if it has a group.</returns>
        static bool HasGroup(XElement parentToCheck)
        {
            
            //First check to see if we have any elements.
            if (parentToCheck.HasElements)
            {
                //Second check to see if the elements are "groups"
                if (parentToCheck.Name.LocalName.Equals("g"))// && !parentToCheck.Attribute("id").Equals("layer1"))
                {
                    //Console.WriteLine(parent.Attribute("id"));
                    return true; 
                }
                else
                {
                    return false; 
                }

            }
            else
            {
                return false; 
            }

        }
    }
}
