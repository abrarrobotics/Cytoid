﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

public class Level
{

    [JsonIgnore] public string basePath;

    public bool ChartsLoaded;
    
    public string format { get; set; }
    public int version { get; set; }
    public string id { get; set; }
    public string title { get; set; }
    public string artist { get; set; }
    [Obsolete] public string composer { get; set; }
    public string illustrator { get; set; }
    public string charter { get; set; }
    public MusicSection music { get; set; }
    public MusicSection music_preview { get; set; }
    public BackgroundSection background { get; set; }
    public List<ChartSection> charts { get; set; }
    public ThemeSection theme { get; set; }
    public bool is_internal { get; set;  }

    public void LoadCharts()
    {
        charts.ForEach(chart =>
        {
            chart.LoadChart(this);
        });
        ChartsLoaded = true;
    }

    public string GetMusicPath(string chartType)
    {
        foreach (var chart in charts)
        {
            if (chart.type != chartType) continue;
            if (chart.music_override != null && chart.music_override.path != null)
            {
                return chart.music_override.path;
            }
        }
        return music.path;
    }

    public int GetDifficulty(string chartType)
    {
        foreach (var chart in charts)
        {
            if (chart.type != chartType) continue;
            return chart.difficulty;
        }
        return -1;
    }
    
    public class MusicSection
    {
        public string path { get; set; }
    }

    public class BackgroundSection
    {
        public string path { get; set; }
    }

    public class ChartSection
    {
        public string type { get; set; } 
        public int difficulty { get; set; }
        public string path { get; set; }
        public MusicSection music_override { get; set; }
        
        [JsonIgnore] public Chart chart;

        // C#: can't access outer members...
        // I miss Java and Kotlin
        public void LoadChart(Level level)
        {
            string chartText;
            if (level.is_internal && Application.platform == RuntimePlatform.Android)
            {
                var www = new WWW(level.basePath + path);
                while (!www.isDone)
                {
                }
                chartText = Encoding.UTF8.GetString(www.bytes);
            }
            else
            {
                chartText = File.ReadAllText(level.basePath + path, Encoding.UTF8);
            }
            chart = new Chart(chartText);
        }
        
    }

    public class ThemeSection
    {
        
        public string ring_color_1 { get; set; }
        public string ring_color_2 { get; set; }
        public string fill_color_1 { get; set; }
        public string fill_color_2 { get; set; }
        
    }

}

public class ChartType
{
    public const string Easy = "easy";
    public const string Hard = "hard";
    public const string Extreme = "extreme";
    
}