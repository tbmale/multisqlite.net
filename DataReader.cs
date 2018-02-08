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
		bool m_fOpen;
		internal List <System.Data.SQLite.SQLiteDataReader> tables;
		internal mSQLiteDataReader()
		{
			tables= new List<System.Data.SQLite.SQLiteDataReader>();
		}
		
		public int Depth
		{
			/*
			 * Always return a value of zero if nesting is not supported.
			 */
			get { return 0;  }
		}

		public bool IsClosed
		{
			/*
			 * Keep track of the reader state - some methods should be
			 * disallowed if the reader is closed.
			 */
			get  { return !m_fOpen; }
		}

		public int RecordsAffected
		{
			/*
			 * RecordsAffected is only applicable to batch statements
			 * that include inserts/updates/deletes. The sample always
			 * returns -1.
			 */
			get { return -1; }
		}

		public void Close()
		{
			/*
			 * Close the reader. The sample only changes the state,
			 * but an actual implementation would also clean up any
			 * resources used by the operation. For example,
			 * cleaning up any resources waiting for data to be
			 * returned by the server.
			 */
			m_fOpen = false;
		}

		public bool NextResult()
		{
			// The sample only returns a single resultset. However,
			// DbDataAdapter expects NextResult to return a value.
			return false;
		}

		public bool Read()
		{
			// Return true if it is possible to advance and if you are still positioned
			// on a valid row. Because the data array in the resultset
			// is two-dimensional, you must divide by the number of columns.
			if (m_fOpen)
				return false;
			else
				return true;
		}

		public DataTable GetSchemaTable()
		{
			//$
			throw new NotSupportedException();
		}

		/****
		 * METHODS / PROPERTIES FROM IDataRecord.
		 ****/
		public int FieldCount
		{
			// Return the count of the number of columns, which in
			// this case is the size of the column metadata
			// array.
			get { return 0; }
		}

		public String GetName(int i)
		{
			return "unset";
		}

		public String GetDataTypeName(int i)
		{
			/*
			 * Usually this would return the name of the type
			 * as used on the back end, for example 'smallint' or 'varchar'.
			 * The sample returns the simple name of the .NET Framework type.
			 */
			return "string";
		}

		public Type GetFieldType(int i)
		{
			// Return the actual Type class for the data type.
			return Type.GetType("int");
		}

		public Object GetValue(int i)
		{
			return "unset";
		}

		public int GetValues(object[] values)
		{
			return 0;
		}

		public int GetOrdinal(string name)
		{
			// Look for the ordinal of the column with the same name and return it.
			return 0;
			// Throw an exception if the ordinal cannot be found.
			throw new IndexOutOfRangeException("Could not find specified column in results");
		}

		public object this [ int i ]
		{
			get { return "unset"; }
		}

		public object this [ String name ]
		{
			// Look up the ordinal and return
			// the value at that position.
			get { return this[GetOrdinal(name)]; }
		}

		public bool GetBoolean(int i)
		{
			/*
			 * Force the cast to return the type. InvalidCastException
			 * should be thrown if the data is not already of the correct type.
			 */
			return (bool)true;
		}

		public byte GetByte(int i)
		{
			/*
			 * Force the cast to return the type. InvalidCastException
			 * should be thrown if the data is not already of the correct type.
			 */
			return (byte)0;
		}

		public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
		{
			// The sample does not support this method.
			throw new NotSupportedException("GetBytes not supported.");
		}

		public char GetChar(int i)
		{
			/*
			 * Force the cast to return the type. InvalidCastException
			 * should be thrown if the data is not already of the correct type.
			 */
			throw new NotSupportedException("GetBytes not supported.");
		}

		public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
		{
			// The sample does not support this method.
			throw new NotSupportedException(" not supported.");
		}

		public Guid GetGuid(int i)
		{
			/*
			 * Force the cast to return the type. InvalidCastException
			 * should be thrown if the data is not already of the correct type.
			 */
			throw new NotSupportedException(" not supported.");
		}

		public Int16 GetInt16(int i)
		{
			/*
			 * Force the cast to return the type. InvalidCastException
			 * should be thrown if the data is not already of the correct type.
			 */
			throw new NotSupportedException(" not supported.");
		}

		public Int32 GetInt32(int i)
		{
			/*
			 * Force the cast to return the type. InvalidCastException
			 * should be thrown if the data is not already of the correct type.
			 */
			throw new NotSupportedException(" not supported.");
		}

		public Int64 GetInt64(int i)
		{
			/*
			 * Force the cast to return the type. InvalidCastException
			 * should be thrown if the data is not already of the correct type.
			 */
			throw new NotSupportedException(" not supported.");
		}

		public float GetFloat(int i)
		{
			/*
			 * Force the cast to return the type. InvalidCastException
			 * should be thrown if the data is not already of the correct type.
			 */
			throw new NotSupportedException(" not supported.");
		}

		public double GetDouble(int i)
		{
			/*
			 * Force the cast to return the type. InvalidCastException
			 * should be thrown if the data is not already of the correct type.
			 */
			throw new NotSupportedException(" not supported.");
		}

		public String GetString(int i)
		{
			/*
			 * Force the cast to return the type. InvalidCastException
			 * should be thrown if the data is not already of the correct type.
			 */
			throw new NotSupportedException(" not supported.");
		}

		public Decimal GetDecimal(int i)
		{
			/*
			 * Force the cast to return the type. InvalidCastException
			 * should be thrown if the data is not already of the correct type.
			 */
			throw new NotSupportedException(" not supported.");
		}

		public DateTime GetDateTime(int i)
		{
			/*
			 * Force the cast to return the type. InvalidCastException
			 * should be thrown if the data is not already of the correct type.
			 */
			throw new NotSupportedException(" not supported.");
		}

		public IDataReader GetData(int i)
		{
			/*
			 * The sample code does not support this method. Normally,
			 * this would be used to expose nested tables and
			 * other hierarchical data.
			 */
			throw new NotSupportedException("GetData not supported.");
		}

		public bool IsDBNull(int i)
		{
			throw new NotSupportedException(" not supported.");
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
			if (disposing)
			{
				try
				{
					this.Close();
				}
				catch (Exception e)
				{
					throw new SystemException("An exception of type " + e.GetType() +
					                          " was encountered while closing the TemplateDataReader.");
				}
			}
		}

	}
}
