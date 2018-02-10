/*
 * Created by SharpDevelop.
 * User: io
 * Date: 01/02/2018
 * Time: 18:54
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Text.RegularExpressions;

namespace multisqlite
{
	/// <summary>
	/// Description of multisqlite.
	/// </summary>
	public class mSQLiteConnection:IEnumerable<System.Data.SQLite.SQLiteConnection>
	{
		List<System.Data.SQLite.SQLiteConnection> _connectionlist;
		SQLiteConnectionStringBuilder connstrobj;
		double slice_size;
		string db_definition;
		public object property(string key){
			if(!connstrobj.ContainsKey(key))return null;
			return connstrobj[key];
		}
		internal int last_slice_no{
			get{return _connectionlist.Count-1;}
		}
		internal double last_slice_size{
			get{
				return new FileInfo(new SQLiteConnectionStringBuilder(_connectionlist[last_slice_no].ConnectionString).DataSource).Length;
			}
		}
		public mSQLiteConnection(string connstr)
		{
			db_definition=null;
			_connectionlist=new List<System.Data.SQLite.SQLiteConnection>();
			connstrobj=new SQLiteConnectionStringBuilder(connstr);
			if(!connstrobj.ContainsKey("Slice size"))connstrobj.Add("Slice size","1Gb");
			slice_size = dehumanize((string)connstrobj["Slice size"])??1099511627776;
			foreach(string singleconnstr in getConnectionStrings(connstr)){
				_connectionlist.Add(new System.Data.SQLite.SQLiteConnection(singleconnstr));
			}
		}
		double? dehumanize(string humansize){
			double? result=null;
			var powers = new Dictionary<string,int>(){{"k", 1}, {"m", 2}, {"g", 3}, {"t", 4}};
			Regex re=new Regex(@"(\d+(?:\.\d+)?)\s?(k|m|g|t)?b?",RegexOptions.IgnoreCase);
			var match=re.Match(humansize);
			if(match.Groups.Count!=3)return null;
			result=Convert.ToDouble(match.Groups[1].Value)*Math.Pow(1024,powers[match.Groups[2].Value.ToLower()]);
			return result;
		}
		public IEnumerator<System.Data.SQLite.SQLiteConnection> GetEnumerator(){
			return _connectionlist.GetEnumerator();
			
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		
		List<string> getConnectionStrings(string connstr){
			if(connstrobj.DataSource.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
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
		public string GetSqlDataDefinition(SQLiteConnection conn){
			bool was_closed=(conn.State==System.Data.ConnectionState.Closed);
			if(was_closed)conn.Open();
			var comm=new SQLiteCommand("select sql from sqlite_master where name not like 'sqlite_%';",conn);
			var data=comm.ExecuteReader();
			string result="";
			while(data.Read())result+=data.GetString(0)+";";
			data.Close();
			if(was_closed)conn.Close();
			return result;
		}
		public void Open(){
			foreach(System.Data.SQLite.SQLiteConnection sconn in _connectionlist){
				sconn.Open();
				if(db_definition==null)
					db_definition=GetSqlDataDefinition(sconn);
			}
		}
		public void Close(){
			foreach(System.Data.SQLite.SQLiteConnection sconn in _connectionlist){
				sconn.Close();
			}
			string lastslicename=new SQLiteConnectionStringBuilder(_connectionlist[last_slice_no].ConnectionString).DataSource;
			var currentslicesize=new FileInfo(lastslicename).Length;
			if(last_slice_size>=slice_size){
				File.Create(lastslicename+"_").Dispose();
				_connectionlist.Add(new System.Data.SQLite.SQLiteConnection("Data source="+lastslicename+"_"));
				_connectionlist[last_slice_no].Open();
				var comm=new SQLiteCommand(db_definition,_connectionlist[last_slice_no]);
				comm.ExecuteNonQuery();
				_connectionlist[last_slice_no].Close();
			}
		}
		
	}
}
