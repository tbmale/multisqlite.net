/*
 * Created by SharpDevelop.
 * User: io
 * Date: 04/02/2018
 * Time: 16:18
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace multisqlite
{
	/// <summary>
	/// Description of SQLiteCommand.
	/// </summary>
	public class mSQLiteCommand
	{
		List<System.Data.SQLite.SQLiteCommand> _sqlcommands;
		string _sqlcommstr;
		mSQLiteConnection _sqlconn;
		
		public mSQLiteConnection Connection{
			get{return _sqlconn;}
			set{_sqlconn=value;}
		}
		public mSQLiteCommand()
		{
			_sqlcommands=new List<System.Data.SQLite.SQLiteCommand>();
			_sqlconn=null;
		}
		public mSQLiteCommand(string command)
		{
			_sqlcommands=new List<System.Data.SQLite.SQLiteCommand>();
			_sqlcommstr=command;
			_sqlconn=null;
		}
		public mSQLiteCommand(string command,mSQLiteConnection sconn)
		{
			_sqlcommands=new List<System.Data.SQLite.SQLiteCommand>();
			_sqlcommstr=command;
			_sqlconn=sconn;
		}
		public static object Execute(string sqlstr, System.Data.SQLite.SQLiteExecuteType executeType, System.Data.CommandBehavior commandBehavior, string connectionString, params object[] args){
			return null;
		}
		public mSQLiteDataReader ExecuteReader(System.Data.CommandBehavior commandbehavior=System.Data.CommandBehavior.Default){
			if(_sqlcommstr.Split(' ')[0].ToUpperInvariant()!="SELECT")throw(new System.Data.SQLite.SQLiteException("SELECT sql statement expected (for modifying queries use ExecuteNonQuery)"));
			if(_sqlcommstr.ToUpperInvariant().Contains("GROUP BY"))throw(new NotImplementedException("GROUP BY clause not (yet) implemented."));
			mSQLiteDataReader mreader=new mSQLiteDataReader();
			if(_sqlconn==null)throw(new System.Data.SQLite.SQLiteException("Connection object not specified."));
			foreach(var conn in _sqlconn){
				_sqlcommands.Add(new System.Data.SQLite.SQLiteCommand(_sqlcommstr));
				_sqlcommands[_sqlcommands.Count-1].Connection=conn;
				mreader.tables.Add(_sqlcommands[_sqlcommands.Count-1].ExecuteReader());
			}
			return mreader;
		}
		public int ExecuteNonQuery(System.Data.CommandBehavior commandbehavior=System.Data.CommandBehavior.Default){
			int result=0;
			if(_sqlcommstr.Split(' ')[0].ToUpperInvariant()=="INSERT"){
				var comm=new System.Data.SQLite.SQLiteCommand(_sqlcommstr,_sqlconn[_sqlconn.last_slice_no]);
				result=comm.ExecuteNonQuery();
				return result;
			}
			foreach(var conn in _sqlconn){
				var comm=new System.Data.SQLite.SQLiteCommand(_sqlcommstr,conn);
				result+=comm.ExecuteNonQuery();
			}
			return result;
			throw(new NotImplementedException());
		}
	}
}
