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

          public static void DeletePurchaseOrders()
          {
               string StocktakrDBLocation = Properties.Settings.Default.StocktakrDBLocation;

               try
               {
                    OleDbConnection DBconnection = null;


                    DBconnection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0; User Id=; Password=; Data Source=" + StocktakrDBLocation);

                    DBconnection.Open();

                    OleDbCommand deleteCommand = DBconnection.CreateCommand();
                    deleteCommand.CommandText = "delete * From PurchaseOrders";
                    deleteCommand.ExecuteNonQuery();

                    deleteCommand.CommandText = "delete * From PurchaseOrderItems";
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

          public static void DownloadPurchaseOrders(LocalPurchaseOrder[] orderList)
          {
               string StocktakrDBLocation = Properties.Settings.Default.StocktakrDBLocation;

               try
               {
                    OleDbConnection DBconnection = null;


                    DBconnection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0; User Id=; Password=; Data Source=" + StocktakrDBLocation);

                    DBconnection.Open();

                    foreach (var order in orderList)
                    {
                         OleDbCommand insertCommand = DBconnection.CreateCommand();
                         string commandText = "INSERT INTO PurchaseOrders (purchaseorder_id, supplier_code ,person, order_datetime) VALUES (?, ?, ?, ?)";

                         insertCommand.CommandText = commandText;

                         insertCommand.Parameters.Add("@purchaseorder_id", OleDbType.Integer).Value = order.purchaseorder_id;
                         insertCommand.Parameters.Add("@supplier_code", OleDbType.VarWChar).Value = order.supplier_code;
                         insertCommand.Parameters.Add("@person", OleDbType.VarWChar).Value = order.person;
                         insertCommand.Parameters.Add("@order_datetime", OleDbType.Date).Value = order.order_datetime;
                         insertCommand.ExecuteNonQuery();

                         foreach (var item in order.itemList)
                         {
                              insertCommand = DBconnection.CreateCommand();
                              commandText = "INSERT INTO PurchaseOrderItems (purchaseorder_id, product_code, product_barcode, description, quantity) VALUES (?, ?, ?, ?, ?)";

                              insertCommand.CommandText = commandText;
                              insertCommand.Parameters.Add("@purchaseorder_id", OleDbType.Integer).Value = order.purchaseorder_id;
                              insertCommand.Parameters.Add("@product_code", OleDbType.VarWChar).Value = item.product_code;
                              insertCommand.Parameters.Add("@product_barcode", OleDbType.VarWChar).Value = item.product_barcode;
                              insertCommand.Parameters.Add("@description", OleDbType.VarWChar).Value = item.description;
                              insertCommand.Parameters.Add("@quantity", OleDbType.Double).Value = item.quantity;
                              
                              insertCommand.ExecuteNonQuery();
                         }
                    }

                    DBconnection.Close();
               }
               catch
               {
                    throw;
               }
          }

          public static List<LocalPurchaseOrder> GetPurchaseOrdersToCommit()
          {
               string StocktakrDBLocation = Properties.Settings.Default.StocktakrDBLocation;
               List<LocalPurchaseOrder> orderList = new List<LocalPurchaseOrder>();

               try
               {

                    OleDbConnection DBconnection = null;
                    OleDbDataReader dbReader = null;

                    DBconnection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0; User Id=; Password=; Data Source=" + StocktakrDBLocation);


                    OleDbCommand syncCmd = DBconnection.CreateCommand();
                    
                    syncCmd.CommandText = "SELECT purchaseorder_id,supplier_code,person, order_datetime from PurchaseOrders";

                    DBconnection.Open();
                    dbReader = syncCmd.ExecuteReader();

                    if (dbReader.HasRows)
                    {
                         while (dbReader.Read())
                         {
                              var newPurchaseOrder = new LocalPurchaseOrder();


                              newPurchaseOrder.purchaseorder_id = dbReader.GetInt32(0);
                              newPurchaseOrder.supplier_code = dbReader.GetString(1);
                              newPurchaseOrder.person = dbReader.GetString(2);
                              newPurchaseOrder.order_datetime = dbReader.GetDateTime(3);

                              orderList.Add(newPurchaseOrder);
                         }
                    }

                    dbReader.Close();


                    syncCmd.CommandText = "SELECT product_code, product_barcode, description, quantity FROM PurchaseOrderItems WHERE purchaseorder_id = ?";

                    foreach (var order in orderList)
                    {
                         List<LocalPurchaseOrderItem> itemList = new List<LocalPurchaseOrderItem>();
                         syncCmd.Parameters.Add("@purchaseorder_id", OleDbType.VarWChar).Value = order.purchaseorder_id;
                         dbReader = syncCmd.ExecuteReader();
                         if (dbReader.HasRows)
                         {
                              while (dbReader.Read())
                              {
                                   var newPurchaseOrderItem = new LocalPurchaseOrderItem();

                                   newPurchaseOrderItem.product_code = dbReader.GetString(0);
                                   newPurchaseOrderItem.product_barcode = dbReader.GetString(1);
                                   newPurchaseOrderItem.description = dbReader.GetString(2);
                                   newPurchaseOrderItem.quantity = dbReader.GetDouble(3);

                                   itemList.Add(newPurchaseOrderItem);
                              }
                              order.itemList = itemList.ToArray();
                         }
                         dbReader.Close();
                    }

                    
                    DBconnection.Close();
               }
               catch (Exception ex)
               {
                    throw;
               }
               return orderList;

          }

          public static void DownloadReceivedGoodsOrders(LocalReceivedGoodsOrder[] orderList)
          {
               string StocktakrDBLocation = Properties.Settings.Default.StocktakrDBLocation;

               try
               {
                    OleDbConnection DBconnection = null;


                    DBconnection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0; User Id=; Password=; Data Source=" + StocktakrDBLocation);

                    DBconnection.Open();

                    foreach (var order in orderList)
                    {
                         OleDbCommand insertCommand = DBconnection.CreateCommand();
                         string commandText = "INSERT INTO ReceivedGoodsOrders (receivedgoodsorder_id, supplier_code ,person, order_datetime) VALUES (?, ?, ?, ?)";

                         insertCommand.CommandText = commandText;

                         insertCommand.Parameters.Add("@receivedgoodsorder_id", OleDbType.Integer).Value = order.receivedgoodsorder_id;
                         insertCommand.Parameters.Add("@supplier_code", OleDbType.VarWChar).Value = order.supplier_code;
                         insertCommand.Parameters.Add("@person", OleDbType.VarWChar).Value = order.person;
                         insertCommand.Parameters.Add("@order_datetime", OleDbType.Date).Value = order.order_datetime;
                         insertCommand.ExecuteNonQuery();

                         foreach (var item in order.itemList)
                         {
                              insertCommand = DBconnection.CreateCommand();
                              commandText = "INSERT INTO ReceivedGoodsOrderItems (receivedgoodsorder_id, product_code, product_barcode, description, quantity) VALUES (?, ?, ?, ?, ?)";

                              insertCommand.CommandText = commandText;
                              insertCommand.Parameters.Add("@receivedgoodsorder_id", OleDbType.Integer).Value = order.receivedgoodsorder_id;
                              insertCommand.Parameters.Add("@product_code", OleDbType.VarWChar).Value = item.product_code;
                              insertCommand.Parameters.Add("@product_barcode", OleDbType.VarWChar).Value = item.product_barcode;
                              insertCommand.Parameters.Add("@description", OleDbType.VarWChar).Value = item.description;
                              insertCommand.Parameters.Add("@quantity", OleDbType.Double).Value = item.quantity;

                              insertCommand.ExecuteNonQuery();
                         }
                    }

                    DBconnection.Close();
               }
               catch
               {
                    throw;
               }
          }

          public static List<LocalReceivedGoodsOrder> GetReceivedGoodsOrdersToCommit()
          {
               string StocktakrDBLocation = Properties.Settings.Default.StocktakrDBLocation;
               List<LocalReceivedGoodsOrder> orderList = new List<LocalReceivedGoodsOrder>();

               try
               {

                    OleDbConnection DBconnection = null;
                    OleDbDataReader dbReader = null;

                    DBconnection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0; User Id=; Password=; Data Source=" + StocktakrDBLocation);


                    OleDbCommand syncCmd = DBconnection.CreateCommand();

                    syncCmd.CommandText = "SELECT receivedgoodsorder_id,supplier_code,person, order_datetime from ReceivedGoodsOrders";

                    DBconnection.Open();
                    dbReader = syncCmd.ExecuteReader();

                    if (dbReader.HasRows)
                    {
                         while (dbReader.Read())
                         {
                              var newReceivedGoodsOrder = new LocalReceivedGoodsOrder();


                              newReceivedGoodsOrder.receivedgoodsorder_id = dbReader.GetInt32(0);
                              newReceivedGoodsOrder.supplier_code = dbReader.GetString(1);
                              newReceivedGoodsOrder.person = dbReader.GetString(2);
                              newReceivedGoodsOrder.order_datetime = dbReader.GetDateTime(3);

                              orderList.Add(newReceivedGoodsOrder);
                         }
                    }

                    dbReader.Close();


                    syncCmd.CommandText = "SELECT product_code, product_barcode, description, quantity FROM ReceivedGoodsOrderItems WHERE receivedgoodsorder_id = ?";

                    foreach (var order in orderList)
                    {
                         List<LocalReceivedGoodsOrderItem> itemList = new List<LocalReceivedGoodsOrderItem>();
                         syncCmd.Parameters.Add("@receivedgoodsorder_id", OleDbType.VarWChar).Value = order.receivedgoodsorder_id;
                         dbReader = syncCmd.ExecuteReader();
                         if (dbReader.HasRows)
                         {
                              while (dbReader.Read())
                              {
                                   var newReceivedGoodsOrderItem = new LocalReceivedGoodsOrderItem();

                                   newReceivedGoodsOrderItem.product_code = dbReader.GetString(0);
                                   newReceivedGoodsOrderItem.product_barcode = dbReader.GetString(1);
                                   newReceivedGoodsOrderItem.description = dbReader.GetString(2);
                                   newReceivedGoodsOrderItem.quantity = dbReader.GetDouble(3);

                                   itemList.Add(newReceivedGoodsOrderItem);
                              }
                              order.itemList = itemList.ToArray();
                         }
                    }

                    dbReader.Close();
                    DBconnection.Close();
               }
               catch (Exception ex)
               {
                    throw;
               }
               return orderList;

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
