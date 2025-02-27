using System;
using System.IO;
using System.Reflection;
using System.Text;
using Redpenguin.Core.StateManagement;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.UnityLinker;

namespace Core.Editor
{
    public class Linker : IUnityLinkerProcessor
    {
        private const string LinkXML = "Packages/com.redpenguin.core/Runtime/link.xml";

        public int callbackOrder => 0;

        [MenuItem("Tools/Core/Generate Link XML")]
        private static void Write()
        {
            var path = Path.GetFullPath("Assets/Core/Runtime/link.xml");
            using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
            {
                writer.WriteLine("<linker>");
                WriteAssembly(writer, Assembly.GetAssembly(typeof(GameStateMachine)));
                writer.WriteLine("</linker>");
            }
            AssetDatabase.Refresh();
        }


        public string GenerateAdditionalLinkXmlFile(BuildReport report, UnityLinkerBuildPipelineData data)
        {
            return Path.GetFullPath(LinkXML);
        }

        private static void WriteAssembly(StreamWriter writer, Assembly assembly)
        {
            writer.WriteLine($"  <assembly fullname=\"{assembly.GetName().Name}\">");

            foreach (var type in assembly.GetTypes())
            {
                WriteType(writer, type);
            }

            writer.WriteLine("  </assembly>");
        }

        private static void WriteType(StreamWriter writer, Type type)
        {
            writer.WriteLine($"    <type fullname=\"{type.FullName}\" preserve=\"all\" />");
        }
    }
}