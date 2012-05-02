using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;

using System.Threading;
using StocktakrClient.com.stocktakr;


namespace StocktakrClient
{
	class Helpers
	{
		public static void CreateSyncTimestamp()
		{
			string StocktakrDBLocation = Properties.Settings.Default.StocktakrDBLocation;
			try
			{
				OleDbConnection StocktakrDBconnection = null;


				StocktakrDBconnection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0; User Id=; Password=; Data Source=" + StocktakrDBLocation);
				StocktakrDBconnection.Open();

				OleDbCommand syncCmd = StocktakrDBconnection.CreateCommand();

				string commandText = "INSERT INTO Syncs (sync_datetime) VALUES (?)";
				//Save latest sync.
				syncCmd.CommandText = commandText;

				syncCmd.Parameters.Add("@sync_datetime", OleDbType.Date).Value = DateTime.Now;
				int numRowsAffected = syncCmd.ExecuteNonQuery();
				StocktakrDBconnection.Close();
			}
			catch
			{
				throw;
			}
		}

		public static DateTime GetLastSync()
		{
			string StocktakrDBLocation = Properties.Settings.Default.StocktakrDBLocation;

			DateTime lastSync = new DateTime();

			try
			{
				OleDbConnection DBconnection = null;
				OleDbDataReader dbReader = null;

				DBconnection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0; User Id=; Password=; Data Source=" + StocktakrDBLocation);
				DBconnection.Open();

				OleDbCommand syncCmd = DBconnection.CreateCommand();

				//Check to see if there are any rows				
				syncCmd.CommandText = "SELECT COUNT(*) from Syncs";
			
				int count = (int)syncCmd.ExecuteScalar();

				if (count > 0)
				{
					syncCmd.CommandText = "SELECT MAX(sync_datetime) from Syncs";
					dbReader = syncCmd.ExecuteReader();

					if (dbReader.Read())
					{
						lastSync = dbReader.GetDateTime(0);
					}
				}
				DBconnection.Close();
			}
			catch
			{
				throw;
			}
			return lastSync;
		}

		public static void DeleteStocktakeTransactions()
		{
			string StocktakrDBLocation = Properties.Settings.Default.StocktakrDBLocation;

			try
			{
				OleDbConnection DBconnection = null;


				DBconnection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0; User Id=; Password=; Data Source=" + StocktakrDBLocation);

				DBconnection.Open();

				OleDbCommand deleteCommand = DBconnection.CreateCommand();
				string commandText = "delete * From StocktakeTransactions";

				deleteCommand.CommandText = commandText;
				deleteCommand.ExecuteNonQuery();				
				DBconnection.Close();
			}
			catch (Exception ex)
			{
				throw;
			}
		}
		

		public static void DownloadStockTakeTransactions(LocalStocktakeTransaction[] transactionList)
		{			
			string StocktakrDBLocation = Properties.Settings.Default.StocktakrDBLocation;

			try
			{
				OleDbConnection DBconnection = null;


				DBconnection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0; User Id=; Password=; Data Source=" + StocktakrDBLocation);

				DBconnection.Open();			

				foreach (var transaction in transactionList)
				{

					OleDbCommand insertCommand = DBconnection.CreateCommand();
					string commandText = "INSERT INTO StocktakeTransactions (product_code, product_barcode, description, quantity, stocktake_datetime, person) VALUES (?, ?, ?, ?, ?, ?)";
					
					insertCommand.CommandText = commandText;

                         insertCommand.Parameters.Add("@product_code", OleDbType.VarWChar).Value = transaction.product_code;
                         insertCommand.Parameters.Add("@product_barcode", OleDbType.VarWChar).Value = transaction.product_barcode;
					insertCommand.Parameters.Add("@description", OleDbType.VarWChar).Value = transaction.description;
					insertCommand.Parameters.Add("@quantity", OleDbType.Double).Value = transaction.quantity;					
					insertCommand.Parameters.Add("@stocktake_date", OleDbType.Date).Value = transaction.stocktake_datetime;
					insertCommand.Parameters.Add("@person", OleDbType.VarWChar).Value = transaction.person;

					insertCommand.ExecuteNonQuery();
				}

				DBconnection.Close();
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public static List<LocalStocktakeTransaction> GetStocktakeTransactionsToCommit()
		{
			string StocktakrDBLocation = Properties.Settings.Default.StocktakrDBLocation;
			List<LocalStocktakeTransaction> transactionList = new List<LocalStocktakeTransaction>();

			try
			{

				OleDbConnection DBconnection = null;
				OleDbDataReader dbReader = null;

				DBconnection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0; User Id=; Password=; Data Source=" + StocktakrDBLocation);


				OleDbCommand syncCmd = DBconnection.CreateCommand();
				//Get customers.
				string commandText = "SELECT product_code, quantity, stocktake_datetime from StocktakeTransactions";
				syncCmd.CommandText = commandText;

				DBconnection.Open();
				dbReader = syncCmd.ExecuteReader();

				if (dbReader.HasRows)
				{
					while (dbReader.Read())
					{
						var newTransaction = new LocalStocktakeTransaction();

						newTransaction.product_code = dbReader.GetValue(0).ToString();
						newTransaction.quantity = dbReader.GetDouble(1);
						newTransaction.stocktake_datetime = dbReader.GetDateTime(2);

						transactionList.Add(newTransaction);
					}
				}

				dbReader.Close();
				DBconnection.Close();
			}
			catch (Exception ex)
			{
				throw;
			}
			return transactionList;
		}

		public static bool TestDPDBConnection()
		{
			string StocktakrDBLocation = Properties.Settings.Default.StocktakrDBLocation;

			try
			{
				OleDbConnection DBconnection = null;


				DBconnection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0; User Id=; Password=; Data Source=" + StocktakrDBLocation);

				DBconnection.Open();
				DBconnection.Close();
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
