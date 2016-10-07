/*
 * 由SharpDevelop创建。
 * 用户： qscq
 * 日期: 2014/8/6
 * 时间: 15:58
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;﻿﻿﻿﻿
	﻿﻿using System.Collections.Generic;
﻿﻿using System.ComponentModel;

﻿﻿using System.Text;﻿﻿
	﻿﻿using System.Security.Cryptography;//加密部分
﻿﻿using System.IO;

namespace Moon.Mvc
{
	/// <summary>
	/// 安全类
	/// </summary>
	public static class WebSecurityUtil
	{
		public static string FilterScripts(string html)
		{
			System.Text.RegularExpressions.Regex regex1 = new System.Text.RegularExpressions.Regex(@"<script[\s\S]+</script *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
			System.Text.RegularExpressions.Regex regex2 = new System.Text.RegularExpressions.Regex(@" href *= *[\s\S]*script *:", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
			var ret = regex1.Replace(html, "");
			ret = regex2.Replace(ret, "");
			return ret;
		}
		private const string MOON_KEY = "qinshich";
		readonly static byte[] KEYS = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
		private static string GetKey()
		{
			var str = DateTime.Now.Year.ToString();
			if (DateTime.Now.Month < 10)
			{
				str +="0"+ DateTime.Now.Month.ToString();
			}
			else
			{
				str += DateTime.Now.Month;
			}
			str += "MM";
			return str;
		}
		public static string EncryptByDes(string encryptString)
		{
			string encryptKey = GetKey();
			try
			{
				byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
				byte[] rgbIV = KEYS;
				byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
				DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
				MemoryStream mStream = new MemoryStream();
				CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
				cStream.Write(inputByteArray, 0, inputByteArray.Length);
				cStream.FlushFinalBlock();
				string ret = Convert.ToBase64String(mStream.ToArray());
				cStream.Close();
				return ret;
			}
			catch (Exception ex)
			{
				Moon.Orm.Util.LogUtil.Exception(ex);
				return encryptString;
			}
		}
		public static string DecryptByDes(string decryptString)
		{
			string decryptKey = GetKey();
			try
			{
				byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
				byte[] rgbIV = KEYS;
				byte[] inputByteArray = Convert.FromBase64String(decryptString);
				DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
				MemoryStream mStream = new MemoryStream();
				CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
				cStream.Write(inputByteArray, 0, inputByteArray.Length);
				cStream.FlushFinalBlock();
				string ret = Encoding.UTF8.GetString(mStream.ToArray());
				cStream.Close();
				return ret;
			}
			catch (Exception ex)
			{
				Moon.Orm.Util.LogUtil.Exception(ex);
				return decryptString;
			}
		}
	}
}
