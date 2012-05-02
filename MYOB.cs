using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using StocktakrClient.com.stocktakr;


namespace StocktakrClient
{
	public class MYOB
	{

		public static List<LocalItem> GetRecentlyModifiedItemList()
		{
		     string RMDBLocation = Properties.Settings.Default.RMDBLocation;

		     DateTime lastSync = Helpers.GetLastSync();

		     List<LocalItem> itemList = new List<LocalItem>();

		     if (lastSync != DateTime.MinValue)
		     {	
			     try
			     {
			          OleDbConnection RMDBconnection = null;
			          OleDbDataReader dbReader = null;

			          RMDBconnection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0; User Id=; Password=; Data Source=" + RMDBLocation);
			          RMDBconnection.Open();

			          OleDbCommand syncCmd = RMDBconnection.CreateCommand();
			          //Get customers.
					string commandText = "SELECT stock_id, Barcode, description, cost, sell, quantity, static_quantity from Stock where stock_id > 0 and date_modified > ? ";
			       
					syncCmd.CommandText = commandText;
					syncCmd.Parameters.Add("@date_modified", OleDbType.Date).Value = lastSync;

			          dbReader = syncCmd.ExecuteReader();

			          if (dbReader.HasRows)
			          {
			               while (dbReader.Read())
			               {
			                    LocalItem newItem = new LocalItem();
			                    newItem.product_code = dbReader.GetValue(0).ToString();
			                    newItem.product_barcode = dbReader.GetValue(1).ToString();
			                    newItem.description = (string)dbReader.GetValue(2);
			                    newItem.cost_price = dbReader.GetDecimal(3);
			                    newItem.sale_price = dbReader.GetDecimal(4);
			                    newItem.quantity = dbReader.GetDouble(5);

			                    newItem.is_static = dbReader.GetBoolean(6);

			                    itemList.Add(newItem);
			               }
			          }

			          dbReader.Close();
			          RMDBconnection.Close();
			     }
			     catch (Exception ex)
			     {
			          throw;
			     }
			}
			Helpers.CreateSyncTimestamp();
			return itemList;
		}
		
		public static List<LocalItem> GetCompleteItemList()
		{
			string RMDBLocation = Properties.Settings.Default.RMDBLocation;

			List<LocalItem> itemList = new List<LocalItem>();

			try
			{
				OleDbConnection RMDBconnection = null;
				OleDbDataReader dbReader = null;

				RMDBconnection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0; User Id=; Password=; Data Source=" + RMDBLocation);
				RMDBconnection.Open();

				OleDbCommand SyncCmd = RMDBconnection.CreateCommand();
				//Get customers.
				string commandText = "SELECT stock_id, Barcode, description, cost, sell, quantity, static_quantity from Stock where stock_id > 0";
				
				SyncCmd.CommandText = commandText;

				dbReader = SyncCmd.ExecuteReader();

				if (dbReader.HasRows)
				{
					while (dbReader.Read())
					{
						LocalItem newItem = new LocalItem();
						newItem.product_code = dbReader.GetValue(0).ToString();
						newItem.product_barcode = dbReader.GetValue(1).ToString();
						newItem.description = (string)dbReader.GetValue(2);
						newItem.cost_price = dbReader.GetDecimal(3);
						newItem.sale_price = dbReader.GetDecimal(4);
						newItem.quantity = dbReader.GetDouble(5);

						newItem.is_static = dbReader.GetBoolean(6);


						itemList.Add(newItem);
					}
				}

				dbReader.Close();
				RMDBconnection.Close();
			}
			catch (Exception ex)
			{
				throw;
			}
			Helpers.CreateSyncTimestamp();
			return itemList;
		}

		

		public static int CommitStocktakeToPOSDatabase()
		{

			var transactionList = Helpers.GetStocktakeTransactionsToCommit();

			string RMDBLocation = Properties.Settings.Default.RMDBLocation;
			try
			{
				OleDbConnection RMDBconnection = null;
				
				RMDBconnection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0; User Id=; Password=; Data Source=" + RMDBLocation);
				RMDBconnection.Open();
				
				foreach (var item in transactionList)
				{

					OleDbCommand insertCommand = RMDBconnection.CreateCommand();
					string commandText = "INSERT INTO StockTake (stocktake_date, stock_id, quantity, date_modified) VALUES (?, ?, ?, ?)";

					insertCommand.CommandText = commandText;

					insertCommand.Parameters.Add("@stocktake_date", OleDbType.Date).Value = item.stocktake_datetime;
					insertCommand.Parameters.Add("@stock_id", OleDbType.Double).Value = Convert.ToDouble(item.product_code);
					insertCommand.Parameters.Add("@quantity", OleDbType.Double).Value = item.quantity;
					insertCommand.Parameters.Add("@date_modified", OleDbType.Date).Value = item.stocktake_datetime;

					insertCommand.ExecuteNonQuery();
				}
				
				RMDBconnection.Close();
				
			}
			catch (Exception ex)
			{
				throw;
			}

			Helpers.DeleteStocktakeTransactions();
			
			return transactionList.Count();
		}

		
	}
}
