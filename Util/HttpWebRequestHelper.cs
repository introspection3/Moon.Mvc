using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Moon.Mvc
{
	public class HttpWebRequestHelper
	{
		private const string VERSION = "Moon.DataCrawl-1.0";
		private const string POST_CONTENT_TYPE = "application/x-www-form-urlencoded";
		private CookieContainer _CookieContainer;
		public CookieContainer CookieContainer
		{
			get
			{
				return this._CookieContainer;
			}
		}
		public string ContentEncodeType
		{
			get;
			set;
		}
		public string Authorization{
			get;
			set;
		}
		public HttpWebRequestHelper()
		{
			this._CookieContainer = new CookieContainer();
		}
		public string GetAuthen(string loginURL, string postParameters)
		{
			try {
				
				
				HttpWebRequest myHttpWebRequest = this.GetMyHttpWebRequest(loginURL);
				if (!string.IsNullOrEmpty(postParameters))
				{
					myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
					myHttpWebRequest.Method = "POST";
					Encoding encoding = Encoding.GetEncoding("utf-8");
					byte[] bytes = encoding.GetBytes(postParameters);
					myHttpWebRequest.ContentLength = (long)bytes.Length;
					Stream requestStream = myHttpWebRequest.GetRequestStream();
					requestStream.Write(bytes, 0, bytes.Length);
					requestStream.Close();
				}
				HttpWebResponse httpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
				this._CookieContainer.Add(httpWebResponse.Cookies);
				Stream responseStream = httpWebResponse.GetResponseStream();
				StreamReader streamReader = new StreamReader(responseStream);
				string result = streamReader.ReadToEnd();
				streamReader.Close();
				responseStream.Close();
				httpWebResponse.Close();
				return result;
				
			} catch (Exception ex) {
				
				Moon.Orm.Util.LogUtil.Exception(ex);
				return "";
			}
		}
		private bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
		{
			return true;
		}
		private HttpWebRequest GetMyHttpWebRequest(string loginURL)
		{
			HttpWebRequest webRequest;
			if (loginURL.StartsWith("https", StringComparison.OrdinalIgnoreCase))
			{
				ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(this.CheckValidationResult);
				webRequest = (WebRequest.Create(loginURL) as HttpWebRequest);
				webRequest.ProtocolVersion = HttpVersion.Version10;
			}
			else
			{
				webRequest = (WebRequest.Create(loginURL) as HttpWebRequest);
			}
			webRequest.UserAgent = "Mozilla/5.0 (Windows NT 5.2) AppleWebKit/534.30 (KHTML, like Gecko) Chrome/12.0.742.122 Safari/534.30";
			webRequest.KeepAlive = true;
			webRequest.Method = "GET";
			
			webRequest.CookieContainer = this._CookieContainer;
			webRequest.Referer = loginURL;
			webRequest.MaximumAutomaticRedirections = 5;
			return webRequest;
		}
		public string GetAuthen(string loginURL)
		{
			return this.GetAuthen(loginURL, null);
		}
		public string GetContent(string url, string postParameters, string encodingType)
		{
			HttpWebRequest myHttpWebRequest = this.GetMyHttpWebRequest(url);
			if (!string.IsNullOrEmpty(postParameters))
			{
				myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
				myHttpWebRequest.Method = "POST";
				Encoding encoding = Encoding.GetEncoding("utf-8");
				byte[] bytes = encoding.GetBytes(postParameters);
				myHttpWebRequest.ContentLength = (long)bytes.Length;
				Stream requestStream = myHttpWebRequest.GetRequestStream();
				requestStream.Write(bytes, 0, bytes.Length);
				requestStream.Close();
			}
			HttpWebResponse httpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
			Stream responseStream = httpWebResponse.GetResponseStream();
			StreamReader streamReader;
			if (!string.IsNullOrEmpty(encodingType))
			{
				streamReader = new StreamReader(responseStream, Encoding.GetEncoding(encodingType));
			}
			else
			{
				streamReader = new StreamReader(responseStream);
			}
			string result = streamReader.ReadToEnd();
			streamReader.Close();
			responseStream.Close();
			httpWebResponse.Close();
			return result;
		}
		public string GetContent(string url, string postParameters)
		{
			return this.GetContent(url, postParameters, null);
		}
		public string GetContent(string url)
		{
			return this.GetContent(url, null, null);
		}
		public string GetCurrentCookie(string url)
		{
			CookieCollection cookies = this._CookieContainer.GetCookies(new Uri(url));
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Cookie cookie in cookies)
			{
				string value = cookie.Name + "=" + cookie.Value + ";";
				stringBuilder.AppendLine(value);
			}
			return stringBuilder.ToString();
		}
	}
}
