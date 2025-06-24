
using System;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

public static class GoogleSheets {

	public static void DownloadGoogleSheet(string sheetId, string tabId, string savePath, Action callback = null) {

		string downloadUrl = String.Format("https://docs.google.com/spreadsheets/d/{0}/export?format=tsv&gid={1}", sheetId, tabId);
		HttpWebRequest request = WebRequest.Create(downloadUrl) as HttpWebRequest;
		HttpWebResponse response = (HttpWebResponse)request.GetResponse();
		WebHeaderCollection header = response.Headers;

		var encoding = ASCIIEncoding.UTF8;
		using (var reader = new StreamReader(response.GetResponseStream(), encoding)) {

			string responseText = reader.ReadToEnd();

			File.WriteAllText(savePath, responseText);
			Debug.Log(String.Format("Downloaded file seved to: {0}", savePath));

			callback?.Invoke();

		}

	}
	
}
