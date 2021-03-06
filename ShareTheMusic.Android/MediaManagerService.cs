﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Net;
using Android.Widget;
using Android.Media;
using Xamarin.Forms;
using ShareTheMusic.Droid;
using System.IO;

[assembly: Dependency(typeof(MediaManagerService))]
namespace ShareTheMusic.Droid
{
    public class MediaManagerService : MediaManager
    {
        int clicks = 0;
        MediaPlayer player = new MediaPlayer();

        public bool PlayPause(string url)
        {
            if (clicks == 0)
            {
                try
                {
                    player.SetDataSource(url);
                    player.Prepare();
                    player.Start();
                    clicks++;
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            else if (clicks % 2 != 0)
            {
                player.Pause();
                clicks++;
                return false;
            }
            else
            {
                player.Start();
                clicks++;
                return true;
            }
        }

        public void Stop()
        {
            player.Stop();
            player = new MediaPlayer();
            clicks = 0;
        }

        public void Close()
        {
            if (player == null)
            {
                return;
            }
            player.Stop();
            player.Dispose();
            player = null;
        }

        public IList<string> GetFileLocation()
        {
            var files = new List<string>();
            foreach (var dir in RootDirectory())
            {
                if (Directory.Exists(dir))
                {
                    var file = Directory.EnumerateFiles(dir).ToList<string>();
                    file.ForEach(f => { if (f.EndsWith("mp3")) files.Add(f); });
                }
            }
            return files;
        }

        public IList<string> RootDirectory()
        {
            var Pathlist = new List<string>();
            try
            {
                var temp = new List<string>();
                
                Pathlist.Add(Android.OS.Environment.GetExternalStoragePublicDirectory(
                             Android.OS.Environment.DirectoryDocuments).AbsolutePath);
                Pathlist.Add(Android.OS.Environment.GetExternalStoragePublicDirectory(
                             Android.OS.Environment.DirectoryDownloads).AbsolutePath);
                Pathlist.Add(Android.OS.Environment.GetExternalStoragePublicDirectory(
                             Android.OS.Environment.DirectoryMusic).AbsolutePath);

                foreach (var path in Pathlist)
                {
                    if (Directory.Exists(path))
                    {
                        temp.AddRange(Directory.EnumerateDirectories(path).ToList());
                    }
                }
                Pathlist.AddRange(temp);
                return Pathlist;
            }
            catch (System.IO.IOException ex)
            {
                System.Diagnostics.Debug.WriteLine("Error finding media files", ex);
                throw ex;
            }
        }
    }
}