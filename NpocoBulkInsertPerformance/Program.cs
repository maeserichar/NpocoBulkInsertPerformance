using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

using NPoco;

namespace NpocoBulkInsertPerformance
{
	class MainClass
	{
		private static string _connStr = "Data Source=temp.db;Version=3;New=True;";
		private static IEnumerable<Poco> GetPocoList()
		{
			var pocoList = new List<Poco> ();

			for(var i = 0; i < 1000; i++)
			{
				pocoList.Add (new Poco () {
					Id = i,
					Message = "Poco number " + i
				});
			}

			return pocoList;
		}

		private static void CreatePocoTable(Database db)
		{
			db.Execute("drop table if exists poco;");
			db.Execute("create table poco(ID, Message);");
		}

		public static void Main (string[] args)
		{
			Console.WriteLine ("Starting performance test...");
			var sw = new Stopwatch ();

			// Arrange the objects to store
			var pocoList = GetPocoList ();

			Console.WriteLine ("Inserting {0} pocos with BulkInsert...", pocoList.Count ());
			sw.Start ();
			using (var db = new Database(_connStr, DatabaseType.SQLite))
			{
				CreatePocoTable (db);
				db.InsertBulk (pocoList);
			}
			sw.Stop ();
			Console.WriteLine ("Insertion of {0} pocos finished using BulkInsert.", pocoList.Count());
			Console.WriteLine ("Elapsed={0}", sw.Elapsed);

			sw = new Stopwatch ();
			Console.WriteLine ("Inserting {0} pocos with Insert inside tx...", pocoList.Count ());
			sw.Start ();
			using (var db = new Database(_connStr, DatabaseType.SQLite))
			{
				CreatePocoTable (db);
				db.BeginTransaction ();
				db.InsertBulk (pocoList);
				db.CompleteTransaction ();
			}
			sw.Stop ();
			Console.WriteLine ("Insertion of {0} pocos finished using Insert inside tx.", pocoList.Count());
			Console.WriteLine ("Elapsed={0}", sw.Elapsed);

			Console.WriteLine ("Performance test finished!");

			File.Delete ("temp.db");

		}
	}
}
