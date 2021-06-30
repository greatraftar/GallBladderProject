﻿using Newtonsoft.Json;
using SimpleVideoCutter.Settings;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleVideoCutter
{
    public class VideoCutterSettings
    {
        private const string configFile = "config.json";

        public string DefaultInitialDirectory { get; set; } = "{UserVideos}";
        public string OutputDirectory { get; set; } = "{UserVideos}";
        public string OutputFilePattern { get; set; } = "{FileDate}-{FileNameWithoutExtension}.{Timestamp}{FileExtension}";
        public string FFmpegPath { get; set; } = null;
        public string[] VideoFilesExtensions { get; set; } = new string[] { ".mov", ".avi", ".mp4", ".wmv", ".rm", ".mpg", ".mkv", ".webm" };

        // nitin added
        public string[] listOfQuality { get; set; } = new string[] { "Bad", "Good", "Best"};
        public string[] listOfStep { get; set; } = new string[] { "Grab And Lift", "Remove the fat", "See Gallbladder", "Look for the triangle", "Critical View of Safety", "Cut the Artery","Separate gallbladder from liver" };
        public int HoverPreviewSize { get; set; } = 300;

        public bool Mute { get; set; } = false;
        public bool Autostart { get; set; } = true;
        public bool ShowTaskWindow { get; set; } = true;

        public FFmpegCutProfile[] FFmpegCutProfiles = new FFmpegCutProfile[]
        {
            // nitin added
            new FFmpegCutProfile() { Name = "lossless", Arguments = "-codec copy", FileType = null, Quality = null, Step = null},
        };

        public string SelectedFFmpegCutProfile { get; set; } = "lossless";

        public Rectangle MainWindowLocation { get; set; } = Rectangle.Empty;
        public bool MainWindowMaximized { get; set; } = false;
        public bool RestoreToolbarsLayout { get; set; } = true;

        public string ConfigVersion { get; set; } = "0.0.0";

        public string Language { get; set; }

        public static VideoCutterSettings Instance { get; }  = new VideoCutterSettings()
        {
            ConfigVersion = Utils.GetCurrentRelease()
        };

        protected VideoCutterSettings()
        {
        }

        protected string ConfigPath 
        { 
            get
            {
                return Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFile));
            } 
        }

        public void LoadSettings()
        {
            if (File.Exists(ConfigPath))
            {
                var json = File.ReadAllText(ConfigPath);
                try
                {
                    JsonConvert.PopulateObject(json, this);
                }
                catch (Exception)
                {
                }
            }

            // After upgrading to new release we avoid restoring layouts, 
            // as usually the strcutre of layouts change in new relese and trying 
            // to restore incompatble layout leads to empty screen. 
            var appVer = new Version(Utils.GetCurrentRelease());
            var configVer = new Version(ConfigVersion);
            if (configVer != appVer) 
            {
                RestoreToolbarsLayout = false; 
            }
        }
        public void StoreSettings()
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(ConfigPath, json);
        }
    }
}