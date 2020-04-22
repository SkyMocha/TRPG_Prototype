using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Logs : MonoBehaviour
{

    public static Text logsText;
    public static string text = "";
    static int cutoff = 12;

    public Logs () {
        logsText = GameObject.FindWithTag("Logs").gameObject.GetComponent(typeof(Text)) as Text;
    }

    public static void addEntry (string log) {
        logsText.text = "";
        text += log + "\n|";
        string[] s = text.Split('|');
        string[] final = new string[10];
        // Loops through and adds all lines to the final array
        for (int k = s.Length - 1, i = 9; k >= 0 && i >= 0; k--, i--) {
            string[] s_s = s[k].Split(' ');
            if (s_s.Length / cutoff < 2 && s_s.Length > cutoff)
                for (int j = 0; j < s_s.Length; j++)
                {
                    final[i] += s_s[j] + ' ';
                    if (j == (int)s_s.Length / 2)
                        final[i] += "\n";
                }
            else
                for (int j = 0; j < s_s.Length; j++)
                {
                    final[i] += s_s[j] + ' ';
                    if (j % cutoff == 0 && j != 0)
                        final[i] += "\n";
                }
        }
        // Goes through the final array backwards and adds all text to the logs
        foreach (string i in final)
            logsText.text += i;
    }
}
