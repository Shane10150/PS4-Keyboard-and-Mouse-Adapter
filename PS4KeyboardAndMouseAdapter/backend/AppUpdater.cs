﻿using NuGet;
using Serilog;
using Squirrel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PS4KeyboardAndMouseAdapter.backend
{
    class AppUpdater
    {

        private bool IsNewerVersionAvailable(List<ReleaseEntry> releaseEntries)
        {
            SemanticVersion currentVersion = VersionUtil.GetSemanticVersion();
            Log.Information("AppUpdater.UpdateIfAvailable() current version " + currentVersion.Version.ToString());

            bool newerVersion = false;
            foreach (ReleaseEntry releaseEntry in releaseEntries)
            {
                SemanticVersion newVersion = releaseEntry.Version;
                Log.Information("AppUpdater.UpdateIfAvailable() 'new' version found: " + newVersion.Version.ToString());

                if (newVersion > currentVersion)
                {
                    newerVersion = true;
                    Log.Information("AppUpdater.UpdateIfAvailable() newer version found");
                }
            }

            return newerVersion;
        }

        private void RemoveNewShortcuts()
        {
            try
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                File.Delete(Path.Combine(desktopPath, "EasyHookSvc.lnk"));
                File.Delete(Path.Combine(desktopPath, "EasyHookSvc64.lnk"));
                File.Delete(Path.Combine(desktopPath, "EasyHookSvc32.lnk"));
            }
            catch (Exception ex)
            {
                Log.Error("Shortcut deletion failed:" + ex.Message);
            }
        }

        // Log notes
        // logically we want Log.Debug, however this happens prior to the log level being set,
        // So we are using Log.Information so we dont lose information
        public async Task UpdateIfAvailable()
        {
            Log.Information("AppUpdater.UpdateIfAvailable() start");
            try
            {

                Log.Information("AppUpdater.UpdateIfAvailable() trying GitHubUpdateManager");
                using (UpdateManager mgr = await UpdateManager.GitHubUpdateManager("https://github.com/starshinata/PS4-Keyboard-and-Mouse-Adapter"))
                {
                    Log.Information("AppUpdater.UpdateIfAvailable() update check");
                    UpdateInfo updateResult = await mgr.CheckForUpdate();
                    Log.Information("AppUpdater.UpdateIfAvailable() update check returned");

                    SemanticVersion nugetCurrentVersion = mgr.CurrentlyInstalledVersion();
                    Log.Information("AppUpdater.UpdateIfAvailable() nugetCurrentVersion " + nugetCurrentVersion.Version.ToString());

                    bool newerVersion = IsNewerVersionAvailable(updateResult.ReleasesToApply);

                    if (newerVersion)
                    {
                        Log.Information("AppUpdater.UpdateIfAvailable() trying UpdateApp");
                        await mgr.UpdateApp();
                        RemoveNewShortcuts();
                        Log.Information("AppUpdater.UpdateIfAvailable() UpdateApp complete");
                    }
                    else
                    {
                        Log.Information("AppUpdater.UpdateIfAvailable() updates skipped");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error("AppUpdater.UpdateIfAvailable() Github Update failed: " + ex.Message);
            }
            Log.Information("AppUpdater.UpdateIfAvailable() update check completed");
        }

    }
}
