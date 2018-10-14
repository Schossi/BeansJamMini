using Assets.Scripts.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Utilities
{
    public static class HighscoreManager
    {
        private static string _secretKey = "C5bvZKs7Uy"; // Edit this value and make sure it's the same as the one stored on the server
        public static string _addScoreURL = "https://blueswebapp.azurewebsites.net/addscore.php?"; //be sure to add a ? to your url
        public static string _highscoreURL = "https://blueswebapp.azurewebsites.net/getscores.php?";

        public static IEnumerator PostScore(string name, int score, int category, bool isMobile,Action onFinished, Action<string> onError)
        {
            //This connects to a server side php script that will add the name and score to a MySQL DB.
            // Supply it with a string representing the players name and the players score.
            string hash = MD5Test.Md5Sum(name + score + _secretKey);

            string post_url = _addScoreURL + "name=" + WWW.EscapeURL(name) + "&score=" + score + "&category=" + category + "&isMobile=" + isMobile + "&hash=" + hash;

            // Post the URL to the site and create a download object to get the result.
            WWW hs_post = new WWW(post_url);
            yield return hs_post; // Wait until the download is done

            if (hs_post.error == null)
            {
                onFinished();
            }
            else
            {
                onError("There was an error posting the high score: " + hs_post.error);
            }
        }

        public static IEnumerator GetScores(int category,bool isMobile,Action<string> onFinished,Action<string> onError)
        {
            WWW hs_get = new WWW(_highscoreURL + "category=" + category + "&isMobile=" + (isMobile ? 1 : 0));
            yield return hs_get;

            if (hs_get.error == null)
            {
                onFinished(hs_get.text);
            }
            else
            {
                onError("There was an error getting the high score: " + hs_get.error);
            }
        }
    }
}
