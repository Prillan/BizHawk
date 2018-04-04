﻿using System;
using System.ComponentModel;
using NLua;
using BizHawk.Emulation.Common;
using BizHawk.Client.Common;
using System.Text;
using System.Collections.Generic;
using System.Net.Http;
using System.Windows.Forms;


namespace BizHawk.Client.EmuHawk
{
	[Description("A library for communicating with other programs")]
	public sealed class CommunicationLuaLibrary : LuaLibraryBase
	{
		[RequiredService]
		private IEmulator Emulator { get; set; }

		[RequiredService]
		private IVideoProvider VideoProvider { get; set; }

		public CommunicationLuaLibrary(Lua lua)
			: base(lua) { }

		public CommunicationLuaLibrary(Lua lua, Action<string> logOutputCallback)
			: base(lua, logOutputCallback) { }

		public override string Name => "comm";

		//TO DO: not fully working yet!
		[LuaMethod("getluafunctionslist", "returns a list of implemented functions")]
		public static string GetLuaFunctionsList()
		{
			var list = new StringBuilder();
			foreach (var function in typeof(CommunicationLuaLibrary).GetMethods())
			{
				list.AppendLine(function.ToString());
			}

			return list.ToString();
		}

		[LuaMethod("socketConfig", "Set the ip and port to connect to, default is 127.0.0.1:9999")]
		public void SocketConfig(string ip, int port)
		{
			GlobalWin.SocketServer.Ip = ip;
			GlobalWin.SocketServer.Port = port;
		}

		[LuaMethod("socketConnect", "Connect to the server")]
		public bool SocketConnect()
		{
			GlobalWin.SocketServer.Connect();
			return GlobalWin.SocketServer.Connected;
		}

		[LuaMethod("socketSendScreenshot", "Sends a screenshot (PNG) to the server")]
		public void SocketSendScreenshot()
		{
			GlobalWin.SocketServer.WriteScreenshot();
		}

		[LuaMethod("socketSendLine", "sends a string to the Socket server")]
		public void SocketSend(string message)
		{
			GlobalWin.SocketServer.WriteLine(message);
		}
		[LuaMethod("socketReceiveLine", "receives a message from the Socket server")]
		public string SocketReceive()
		{
			return GlobalWin.SocketServer.ReadLine();
		}
		
		[LuaMethod("socketSetTimeout", "sets the timeout in milliseconds for receiving messages")]
		public void SocketSetTimeout(int timeout)
		{
			GlobalWin.SocketServer.ReceiveTimeout = timeout;
		}
		// All MemoryMappedFile related methods
		[LuaMethod("mmfSetFilename", "Sets the filename for the screenshots")]
		public void MmfSetFilename(string filename)
		{
			GlobalWin.memoryMappedFiles.SetFilename(filename);
		}
		[LuaMethod("mmfGetFilename", "Gets the filename for the screenshots")]
		public string MmfSetFilename()
		{
			return GlobalWin.memoryMappedFiles.GetFilename();
		}

		[LuaMethod("mmfScreenshot", "Saves screenshot to memory mapped file")]
		public int MmfScreenshot()
		{
			return GlobalWin.memoryMappedFiles.ScreenShotToFile();
		}

		[LuaMethod("mmfWrite", "Writes a string to a memory mapped file")]
		public int MmfWrite(string mmf_filename, string outputString)
		{
			return GlobalWin.memoryMappedFiles.WriteToFile(mmf_filename, Encoding.ASCII.GetBytes(outputString));
		}
		[LuaMethod("mmfRead", "Reads a string from a memory mapped file")]
		public string MmfRead(string mmf_filename, int expectedSize)
		{
			return GlobalWin.memoryMappedFiles.ReadFromFile(mmf_filename, expectedSize).ToString();
		}
		// All HTTP related methods
		[LuaMethod("httpTest", "tests HTTP connections")]
		public string HttpTest()
		{
			var list = new StringBuilder();
			list.AppendLine(GlobalWin.httpCommunication.TestGet());
			list.AppendLine(GlobalWin.httpCommunication.SendScreenshot());
			list.AppendLine("done testing");
			return list.ToString();
		}
		[LuaMethod("httpTestGet", "tests the HTTP GET connection")]
		public string HttpTestGet()
		{
			return GlobalWin.httpCommunication.TestGet();
		}
		[LuaMethod("httpGet", "makes a HTTP GET request")]
		public string HttpGet(string url)
		{
			return GlobalWin.httpCommunication.ExecGet(url);
		}

		[LuaMethod("httpPost", "makes a HTTP POST request")]
		public string HttpPost(string url, string payload)
		{
			return GlobalWin.httpCommunication.ExecPost(url, payload);
		}
		[LuaMethod("httpPostScreenshot", "HTTP POST screenshot")]
		public string HttpPostScreenshot()
		{
			return GlobalWin.httpCommunication.SendScreenshot();
		}
		[LuaMethod("httpSetTimeout", "Sets HTTP timeout in milliseconds")]
		public void HttpSetTimeout(int timeout)
		{
			GlobalWin.httpCommunication.SetTimeout(timeout);
		}
		[LuaMethod("httpSetPostUrl", "Sets HTTP POST URL")]
		public void HttpSetPostUrl(string url)
		{
			GlobalWin.httpCommunication.SetPostUrl(url);
		}
		[LuaMethod("httpSetGetUrl", "Sets HTTP GET URL")]
		public void HttpSetGetUrl(string url)
		{
			GlobalWin.httpCommunication.SetGetUrl(url);
		}
		[LuaMethod("httpGetPostUrl", "Gets HTTP POST URL")]
		public string HttpGetPostUrl()
		{
			return GlobalWin.httpCommunication.GetPostUrl();
		}
		[LuaMethod("httpGetGetUrl", "Gets HTTP GET URL")]
		public string HttpGetGetUrl()
		{
			return GlobalWin.httpCommunication.GetGetUrl();
		}
	}
}
