using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.OleDb;
using StocktakrClient.com.stocktakr;
using System.Data;
using System.Reflection;


namespace StocktakrClient
{
	class MicrosoftRMS
	{
		public static string MakeConnectionString(string location, string DBname, string user, string password)
		{
			return String.Format("Data Source={0};Initial Catalog = {1}; User ID = {2}; Password = {3}", location, DBname, user, password);
		}	
	
		public static List<LocalItem> GetRecentlyModifiedItemList()
		{
			string RMDBLocation = Properties.Settings.Default.RMDBLocation;

			DateTime lastSync = Helpers.GetLastSync();

			List<LocalItem> itemList = new List<LocalItem>();

			if (lastSync != DateTime.MinValue)
			{
				try
				{
					// create a connection object
					SqlConnection connection = new SqlConnection(MakeConnectionString(Properties.Settings.Default.POSServerLocation,
																		 Properties.Settings.Default.POSServerDBName,
																		 Properties.Settings.Default.POSServerUser,
																		 Properties.Settings.Default.POSServerPassword));

					// create a command object
					SqlCommand selectCommand = connection.CreateCommand();
					//Get customer.
					selectCommand.CommandText = String.Format("select ID, ItemLookupCode, Description, Cost, SalePrice, Quantity from Item where LastUpdated > '{0}'", lastSync.ToString("yyyy-MM-dd HH:mm:ss"));

					SqlDataReader itemDataReader = selectCommand.ExecuteReader();

					if (itemDataReader.HasRows)
					{
						while (itemDataReader.Read())
						{
							LocalItem newItem = new LocalItem();
							int tempItemID = (int)itemDataReader["ID"];
							newItem.product_code = tempItemID.ToString();

							newItem.product_barcode = (string)itemDataReader["ItemLookupCode"];
							newItem.description = (string)itemDataReader["Description"];
							newItem.cost_price = (decimal)itemDataReader["Cost"];
							newItem.sale_price = (decimal)itemDataReader["SalePrice"];
							newItem.quantity = (double)itemDataReader["Quantity"];
							newItem.is_static = false;

							itemList.Add(newItem);
						}
					}
					itemDataReader.Close();
					connection.Close();
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
			List<LocalItem> itemList = new List<LocalItem>();

			try
			{
				// create a connection object
				SqlConnection connection = new SqlConnection(MakeConnectionString(Properties.Settings.Default.POSServerLocation,
																		Properties.Settings.Default.POSServerDBName,
																		Properties.Settings.Default.POSServerUser,
																		Properties.Settings.Default.POSServerPassword));

				// create a command object
				SqlCommand selectCommand = connection.CreateCommand();

				connection.Open();

				//Get customers.


				//Get customer.
				selectCommand.CommandText = String.Format("select ID, ItemLookupCode, Description, Cost, Price, Quantity from Item ");
				SqlDataReader itemDataReader = selectCommand.ExecuteReader();

				if (itemDataReader.HasRows)
				{
					while (itemDataReader.Read())
					{
						LocalItem newItem = new LocalItem();
						int tempItemID = (int)itemDataReader["ID"];
						newItem.product_code = tempItemID.ToString();

						newItem.product_barcode = (string)itemDataReader["ItemLookupCode"];
						newItem.description = (string)itemDataReader["Description"];
						newItem.cost_price = (decimal)itemDataReader["Cost"];
						newItem.sale_price = (decimal)itemDataReader["Price"];
						newItem.quantity = (double)itemDataReader["Quantity"];
						newItem.is_static = false;

						itemList.Add(newItem);
					}
				}
				itemDataReader.Close();
				connection.Close();
			}
			catch (Exception ex)
			{
				throw;
			}
			Helpers.CreateSyncTimestamp();
			return itemList;
		}



          public static int CommitStocktakeToPOSDatabase(string code)
          {
               SqlConnection connection = new SqlConnection(MakeConnectionString(Properties.Settings.Default.POSServerLocation,
                                                                                   Properties.Settings.Default.POSServerDBName,
                                                                                   Properties.Settings.Default.POSServerUser,
                                                                                   Properties.Settings.Default.POSServerPassword));

               try
               {
                    var transactionList = Helpers.GetStocktakeTransactionsToCommit();
                    int count = transactionList.Count(); 
                    // create a connection object
                    int newPhysicalInventoryID = CreateNewPhysicalInventory(connection, code);

                    if (InsertTransactions(transactionList, newPhysicalInventoryID))
                    {
                         Helpers.DeleteStocktakeTransactions();
                    }
                    return count;
               }
               catch
               {
                    throw;
               }
               
          }

          private static int CreateNewPhysicalInventory(SqlConnection connection,string code)
          {
             
               DateTime openTime = DateTime.Now;               
               string description = "Imported by Stocktakr";
               //Tell the SqlCommand what query to execute and what SqlConnection to use.  
               using (SqlCommand sqlCmd = new SqlCommand("INSERT INTO dbo.PhysicalInventory(OpenTime, Description,Code) VALUES (@OpenTime, @Description,@Code)", connection))
               {

                    //Add SqlParameters to the SqlCommand  
                    sqlCmd.Parameters.AddWithValue("@OpenTime", openTime);
                    sqlCmd.Parameters.AddWithValue("@Description", description);
                    sqlCmd.Parameters.AddWithValue("@Code", code);

                    //Open the SqlConnection before executing the query.  
                    try
                    {
                         connection.Open();
                         sqlCmd.ExecuteNonQuery();
                    }
                    catch
                    {
                         throw;
                    }
                    finally
                    {
                         connection.Close();
                    }
               }

               using (SqlCommand sqlCmd = new SqlCommand("select ID from dbo.PhysicalInventory where Code = @Code", connection))
               {

                    //Add SqlParameters to the SqlCommand                     
                    sqlCmd.Parameters.AddWithValue("@Code", code);

                    //Open the SqlConnection before executing the query.  
                    try
                    {
                         connection.Open();
                         return (int)sqlCmd.ExecuteScalar();
                    }
                    catch
                    {
                         throw;
                    }
                    finally
                    {
                         connection.Close();
                    }
               }
          }     

          private static bool InsertTransactions(List<LocalStocktakeTransaction> transactionList, int newPhysicalInventoryID)
          {
               string connString = (MakeConnectionString(Properties.Settings.Default.POSServerLocation,
                                                                                   Properties.Settings.Default.POSServerDBName,
                                                                                   Properties.Settings.Default.POSServerUser,
                                                                                   Properties.Settings.Default.POSServerPassword));

               

               DataTable newTable = new DataTable();

               newTable.Columns.Add("PhysicalInventoryID");
               newTable.Columns.Add("CountTime");
               newTable.Columns.Add("ItemID");
               newTable.Columns.Add("QuantityCounted");
               
             
               foreach (var entry in transactionList)
               {
                    newTable.Rows.Add(newPhysicalInventoryID,entry.stocktake_datetime, Convert.ToInt32(entry.product_code),Convert.ToDouble(entry.quantity));
               }

               bool success = false;

               try
               {
                    using (SqlBulkCopy sbc = new SqlBulkCopy(connString))
                    {
                         sbc.DestinationTableName = "dbo.PhysicalInventoryEntry";
        
                         // Number of records to be processed in one go
                         sbc.BatchSize = 1000;
  
                         // Map the Source Column from DataTabel to the Destination Columns in SQL Server 2005 Person Table
                         sbc.ColumnMappings.Add("PhysicalInventoryID", "PhysicalInventoryID");
                         sbc.ColumnMappings.Add("CountTime", "CountTime");                    
                         sbc.ColumnMappings.Add("ItemID", "ItemID");
                         sbc.ColumnMappings.Add("QuantityCounted", "QuantityCounted");

                        

                         // Finally write to server
                         sbc.WriteToServer(newTable);
                         sbc.Close();
                         success = true;
                    }
               }
               catch (Exception ex)
               {                   
                    throw ex;
               }
               
               return success;
          }
     }
}
