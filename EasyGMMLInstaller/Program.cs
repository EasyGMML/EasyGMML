using System;
using Microsoft.Win32;
using Gameloop.Vdf;

namespace EasyGMMLInstaller
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            dynamic libaryFolders = VdfConvert.Deserialize(File.ReadAllText("C:\\Program Files (x86)\\Steam\\steamapps\\libraryfolders.vdf"));
            string sm = libaryFolders.ToJson().Get("0");
            // string balls = libaryFolders.Value.;
            // Console.WriteLine(libaryFolders.Value.apps); // Prints 3

            if (Environment.Is64BitOperatingSystem)
            {
                string installPath = (string)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\WOW6432Node\\Valve\\Steam", "InstallPath", null);

                Console.WriteLine(Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\WOW6432Node\\Valve\\Steam", "InstallPath", null));
            }
            else
            {
                Console.WriteLine(Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Valve\\Steam", "InstallPath", null));

            }
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}