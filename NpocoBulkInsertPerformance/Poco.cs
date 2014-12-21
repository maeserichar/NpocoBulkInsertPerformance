using System;
using NPoco;

namespace NpocoBulkInsertPerformance
{
	[TableName("poco")]
	public class Poco
	{
		public int Id { get; set; }
		public string Message { get; set; } 
	}
}

