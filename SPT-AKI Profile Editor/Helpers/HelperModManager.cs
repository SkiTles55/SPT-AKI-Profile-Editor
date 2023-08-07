﻿using SPT_AKI_Profile_Editor.Core;
using System;
using System.IO;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Helpers
{
    public interface IHelperModManager
    {
        public string DbPath { get; }
        public HelperModStatus HelperModStatus { get; }
        public bool UpdateAvailable { get; }
        public bool IsInstalled { get; }
        public bool DbFilesExist { get; }

        public void InstallMod();

        public void RemoveMod();

        public void UpdateMod();
    }

    public class HelperModManager : IHelperModManager
    {
        private readonly string helperDbPath;
        private readonly string modPath;

        private readonly string srcDirName = "src";
        private readonly string modScriptFileName = "mod.ts";
        private readonly string packageJsonFileName = "package.json";
        private readonly string modSourceDirName = "ModHelper";
        private readonly string modScriptSourceFileName = "mod.ts-source";

        public HelperModManager(string modPath = "user\\mods\\ProfileEditorHelper")
        {
            this.modPath = modPath;
            helperDbPath = Path.Combine(modPath, "exportedDB");
        }

        public HelperModStatus HelperModStatus => CheckModStatus();
        public bool UpdateAvailable => HelperModStatus == HelperModStatus.UpdateAvailable;
        public bool IsInstalled => HelperModStatus == HelperModStatus.Installed;

        public bool DbFilesExist => CheckDbStatus();

        public string DbPath => helperDbPath;

        public void InstallMod()
        {
            var fullModPath = GetFullModPath();
            if (!Directory.Exists(fullModPath))
                Directory.CreateDirectory(fullModPath);
            var srcPath = Path.Combine(fullModPath, srcDirName);
            if (!Directory.Exists(srcPath))
                Directory.CreateDirectory(srcPath);
            var srcFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                           modSourceDirName,
                                           modScriptSourceFileName);
            File.Copy(srcFilePath, Path.Combine(srcPath, modScriptFileName), true);
            var packageJsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                               modSourceDirName,
                                               packageJsonFileName);
            File.Copy(packageJsonPath, Path.Combine(fullModPath, packageJsonFileName), true);
        }

        public void RemoveMod()
        {
            var fullModPath = GetFullModPath();
            Directory.Delete(fullModPath, true);
        }

        public void UpdateMod()
        {
            throw new NotImplementedException();
        }

        private string GetFullModPath() => Path.Combine(AppData.AppSettings.ServerPath, modPath);

        private HelperModStatus CheckModStatus()
        {
            var fullModPath = GetFullModPath();
            if (File.Exists(Path.Combine(fullModPath, srcDirName, modScriptFileName))
                && File.Exists(Path.Combine(fullModPath, packageJsonFileName)))
                return HelperModStatus.Installed;
            return HelperModStatus.NotInstalled;
        }

        private bool CheckDbStatus()
        {
            var fullDbPath = Path.Combine(AppData.AppSettings.ServerPath, helperDbPath);
            if (Directory.Exists(fullDbPath)
                && Directory.GetFiles(fullDbPath, "*.json").Any()
                && Directory.GetDirectories(fullDbPath).Any())
                return true;
            return false;
        }
    }
}