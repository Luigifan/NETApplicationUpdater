using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace NETApplicationUpdater
{
    public static class NETApplicationUpdater
    {
        /// <summary>
        /// Checks is an update to your application is available
        /// </summary>
        /// <param name="urlToCheck">A direct URL to a file on your server containing some form of a plain text file containing the current version (and only this!)</param>
        /// <param name="currentAppVersion">Your current application's version</param>
        /// <returns>True if there's an update available, false if not</returns>
        public static bool UpdateAvailable(string urlToCheck, Version currentAppVersion)
        {
            WebClient wc = new WebClient();
            byte[] data = wc.DownloadData(urlToCheck);
            Version latestVersion = Version.Parse(Encoding.ASCII.GetString(data));
            int comparisonResult = currentAppVersion.CompareTo(latestVersion);
            if (comparisonResult <= -1)
                return true; //What do I do from here? Well, download a changelog and/or ask the user if they wish to update. I recommend using a seperate update application (an example comes with this solution)
            else
                return false;
        }

    }
}
