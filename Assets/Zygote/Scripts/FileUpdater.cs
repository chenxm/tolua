using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Security.Cryptography;
using Utility;



public class FileUpdater
{
	private static ILogger logger = Debug.unityLogger;

	private string GetResourceUrl(string[] lines)
	{
		string url = string.Empty;
		for (int i = 0; i < lines.Length; i++)
		{
			if (lines[i].StartsWith("#@"))
			{
				url = lines[i].Substring(2);
				break;
			}
		}
		return url;
	}

	public float Progress { get; private set; }
	public string Error { get; private set; }

	/// <summary>
	/// 启动更新下载
	/// </summary>
	public IEnumerator Update(string local, string remote, string additionInfo, float downloadTimeout)
	{
		//string fileListPath = local + "/Manifest.txt";
		string listUrl = remote + "/Manifest.txt?v=" + additionInfo;
		logger.Log("LoadUpdate---->>>" + listUrl);

		string filesText = string.Empty;

		Progress = 0.0f;

		//下载Manifest文件
		float timeBegin = Time.time;
		using (WWW www = new WWW(listUrl))
		{
			while (!www.isDone)
			{
				float duration = Time.time - timeBegin;
				if (www.bytesDownloaded < 10 && duration > downloadTimeout)
				{
					Error = "Timeout";
					yield break;
				}
				yield return null;
			}
			if (!string.IsNullOrEmpty(www.error))
			{
				Error = www.error;
				yield break;
			}
			filesText = www.text;
		}

		// 取资源文件路径
		string[] files = filesText.Split('\n');
		string tmpPath = GetResourceUrl(files);
		if (!string.IsNullOrEmpty(tmpPath))
		{
			remote = tmpPath;
		}

		// 下载文件
		int fileCount = files.Length;
		for (int i = 0; i < fileCount; i++)
		{
			if (string.IsNullOrEmpty(files[i]) || files[i].StartsWith("#")) continue;
			string[] keyValue = files[i].Split('|');
			if (keyValue.Length < 2) continue;
			string f = keyValue[0];
			string localfile = (local + "/" + f).Trim();
			string path = Path.GetDirectoryName(localfile);
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			string fileUrl = remote + "/" + f + "?v=" + additionInfo;
			bool canUpdate = !File.Exists(localfile);
			if (!canUpdate)
			{
				string remoteMd5 = keyValue[1].Trim();
				string localMd5 = Util.MD5file(localfile);
				canUpdate = !remoteMd5.Equals(localMd5);
				if (canUpdate) File.Delete(localfile);
			}
			if (canUpdate)
			{   //本地缺少文件
				logger.Log(fileUrl);
				Progress = (float)i / fileCount;

				using (WWW www = new WWW(fileUrl))
				{
					yield return www;
					if (!string.IsNullOrEmpty(www.error))
					{
						OnUpdateFailed(fileUrl, www.error);
						//yield return new WaitForSeconds(0.5f);
						//i--;
						continue;
					}
					File.WriteAllBytes(localfile, www.bytes);
				}
			}
		}

		Progress = 1.0f;
	}

	void OnUpdateFailed(string file, string error)
	{
		FileUpdater.logger.LogWarning("Updater", "Download " + file + " failed: " + error);
	}

}
