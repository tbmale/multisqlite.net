/*
 * Created by SharpDevelop.
 * User: io
 * Date: 01/02/2018
 * Time: 18:54
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace multisqlite
{
	/// <summary>
	/// Description of multisqlite.
	/// </summary>
	public class SQLiteConnection
	{
		List<System.Data.SQLite.SQLiteConnection> _connectionlist;
		public SQLiteConnection(string connstr)
		{
			_connectionlist=new List<System.Data.SQLite.SQLiteConnection>();
			foreach(string singleconnstr in getConnectionStrings(connstr)){
				_connectionlist.Add(new System.Data.SQLite.SQLiteConnection(singleconnstr));
			        	}
			}
//		public Connection()
//		{
//
//		}

		List<string> getConnectionStrings(string connstr){
			var connstrobj=new SQLiteConnectionStringBuilder(connstr);
			if(connstrobj.DataSource.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
				throw new ArgumentException("Invalid DataSource.");
			string dirstr=Path.GetDirectoryName(connstrobj.DataSource);
			string filename=Path.GetFileName(connstrobj.DataSource);
			string searchdir;
			if(!String.IsNullOrEmpty(dirstr) && Directory.Exists(dirstr))
				searchdir=dirstr;
			else
				searchdir=Directory.GetCurrentDirectory();
			string[] files=Directory.GetFiles(searchdir,filename+"*");
			List<string> result=new List<string>();
			if(files.Length==0){
				File.Create(Path.Combine(searchdir,filename)).Dispose();
				connstrobj.DataSource=filename;
				result.Add(connstr);
			}
			else foreach(string file in files){
				connstrobj.DataSource=file;
				result.Add(connstrobj.ConnectionString);
				}
			return result;
		}
		
		
	}
}
