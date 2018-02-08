/*
 * Created by SharpDevelop.
 * User: io
 * Date: 06/02/2018
 * Time: 21:14
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace multisqlite
{
	/// <summary>
	/// Description of DataReader.
	/// </summary>
	public class mSQLiteDataReader:IDataReader
	{
		internal List <System.Data.SQLite.SQLiteDataReader> tables;
		int current_table;
		int step_count;
		internal mSQLiteDataReader()
		{
			tables= new List<System.Data.SQLite.SQLiteDataReader>();
			current_table=0;
		}
		
		public int Depth
		{
			get { return 0; }
		}
		public int StepCount
		{
			get { return 0; }
		}
		public int VisibleFieldCount
		{
			get{return tables[0].VisibleFieldCount;}
		}

		public bool HasRows
		{
			get{return tables[current_table].HasRows;}
		}
		public bool IsClosed
		{
			get  {
				bool closed=true;
				foreach(var reader in tables)closed=reader.IsClosed;
				return closed;
			}
		}

		public int RecordsAffected
		{
			get { return -1; }
		}

		public void Close()
		{
			foreach(var reader in tables)reader.Close();
		}

		public bool NextResult()
		{
			if(current_table>=tables.Count)return false;
			step_count++;
			bool result=tables[current_table].NextResult();
			if(!result){
				current_table++;
				if(current_table<tables.Count)result=tables[current_table].NextResult();
			}
			return result;
		}

		public bool Read()
		{
			if(current_table>=tables.Count)return false;
			step_count++;
			bool result=tables[current_table].Read();
			if(!result){
				current_table++;
				if(current_table<tables.Count)result=tables[current_table].Read();
			}
			return result;
		}

		public DataTable GetSchemaTable()
		{
			return tables[0].GetSchemaTable();
		}

		public int FieldCount
		{
			get { return tables[0].FieldCount; }
		}

		public String GetName(int i)
		{
			return tables[0].GetName(i);
		}

		public String GetDataTypeName(int i)
		{
			return tables[0].GetDataTypeName(i);
		}

		public Type GetFieldType(int i)
		{
			return tables[0].GetFieldType(i);
		}

		public Object GetValue(int i)
		{
			return tables[current_table].GetValue(i);
		}

		public int GetValues(object[] values)
		{
			return tables[current_table].GetValues(values);
		}

		public int GetOrdinal(string name)
		{
			return tables[current_table].GetOrdinal(name);
		}

		public object this [ int i ]
		{
			get { return tables[current_table][i]; }
		}

		public object this [ String name ]
		{
			get { return tables[current_table][tables[current_table].GetOrdinal(name)]; }
		}

		public bool GetBoolean(int i)
		{
			return tables[current_table].GetBoolean(i);
		}

		public byte GetByte(int i)
		{
			return tables[current_table].GetByte(i);
		}

		public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
		{
			return tables[current_table].GetBytes(i, fieldOffset, buffer, bufferoffset, length);
		}

		public char GetChar(int i)
		{
			return tables[current_table].GetChar(i);
		}

		public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
		{
			return tables[current_table].GetChars(i, fieldoffset, buffer, bufferoffset, length);
		}

		public Guid GetGuid(int i)
		{
			return tables[current_table].GetGuid(i);
		}

		public Int16 GetInt16(int i)
		{
			return tables[current_table].GetInt16(i);
		}

		public Int32 GetInt32(int i)
		{
			return tables[current_table].GetInt32(i);
		}

		public Int64 GetInt64(int i)
		{
			return tables[current_table].GetInt64(i);
		}

		public float GetFloat(int i)
		{
			return tables[current_table].GetFloat(i);
		}

		public double GetDouble(int i)
		{
			return tables[current_table].GetDouble(i);
		}

		public String GetString(int i)
		{
			return tables[current_table].GetString(i);
		}

		public Decimal GetDecimal(int i)
		{
			return tables[current_table].GetDecimal(i);
		}

		public DateTime GetDateTime(int i)
		{
			return tables[current_table].GetDateTime(i);
		}

		public IDataReader GetData(int i)
		{
			throw new NotSupportedException("GetData not supported.");
		}

		public bool IsDBNull(int i)
		{
			return tables[current_table].IsDBNull(i);
		}

		/*
		 * Implementation specific methods.
		 */

		void IDisposable.Dispose()
		{
			this.Dispose(true);
			System.GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			foreach(var reader in tables)reader.Close();
		}

	}
}
