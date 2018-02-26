using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MakeManifest
{
	class Program
	{
		static void Main(string[] args)
		{
			string pattern = (args.Length > 0) ? args[0] : Directory.GetCurrentDirectory();
			if (args.Length > 0)
			{
				pattern = args[0];
			}
			else
			{
				Console.WriteLine("Please input pattern.");
				pattern = Console.ReadLine();
			}
			string targetDirectory = (args.Length > 1) ? args[1] : Directory.GetCurrentDirectory();
			string version = args.Length > 2 ? args[2] : "1.0.0";
			Make(pattern, targetDirectory, version);
		}

		private static void Make(string pattern, string targetDirectory, string version)
		{
			Regex reg = new Regex(pattern, RegexOptions.IgnoreCase);
			string[] files = Directory.GetFiles(targetDirectory, "*", SearchOption.AllDirectories);
			string newFilePath = Path.Combine(targetDirectory, "Manifest.txt");
			using (StreamWriter sw = new StreamWriter(newFilePath, false))
			{
				sw.WriteLine("##" + version);
				for (int i = 0; i < files.Length; i++)
				{
					string file = files[i];

					if(!reg.IsMatch(file))
					{
						continue;
					}

					string md5 = MD5file(file);
					string value = file.Replace(targetDirectory, string.Empty);
					value = value.TrimStart(new char[] { '\\', '/' });
					value = value.Replace('\\', '/');
					sw.WriteLine(value + "|" + md5);
				}
			}

		}

		/// <summary>
		/// 计算文件的MD5值
		/// </summary>
		public static string MD5file(string file)
		{
			try
			{
				FileStream fs = new FileStream(file, FileMode.Open);
				MD5 md5 = new MD5CryptoServiceProvider();
				byte[] retVal = md5.ComputeHash(fs);
				fs.Close();

				StringBuilder sb = new StringBuilder();
				for (int i = 0; i < retVal.Length; i++)
				{
					sb.Append(retVal[i].ToString("x2"));
				}
				return sb.ToString();
			}
			catch (Exception ex)
			{
				throw new Exception("md5file() fail, error:" + ex.Message);
			}
		}

	}
}
