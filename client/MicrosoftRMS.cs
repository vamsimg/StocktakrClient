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
                         connection.Open();

					//Get customer.
					selectCommand.CommandText = String.Format("select ID, ItemLookupCode, Description, Cost, SalePrice, Quantity, SupplierID from Item where LastUpdated > '{0}'", lastSync.ToString("yyyy-MM-dd HH:mm:ss"));

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

                                   int supplierID = (int)itemDataReader["SupplierID"];
                                   if (supplierID != 0)
                                   {
                                        newItem.supplier_code = supplierID.ToString();
                                   }

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
				selectCommand.CommandText = String.Format("select ID, ItemLookupCode, Description, Cost, Price, Quantity, SupplierID from Item ");
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

                              int supplierID = (int)itemDataReader["SupplierID"];
                              if (supplierID != 0)
                              {
                                   newItem.supplier_code = supplierID.ToString();
                              }

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

          public static List<LocalSupplier> GetCompleteSupplierList()
          {
               List<LocalSupplier> suppliers = new List<LocalSupplier>();

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
                    selectCommand.CommandText = String.Format("select ID, SupplierName from Supplier");
                    SqlDataReader supplierDataReader = selectCommand.ExecuteReader();

                    if (supplierDataReader.HasRows)
                    {
                         while (supplierDataReader.Read())
                         {
                              LocalSupplier newSupplier = new LocalSupplier();
                              int tempSupplierID = (int)supplierDataReader["ID"];
                              newSupplier.supplier_code = tempSupplierID.ToString();                              
                              newSupplier.name = (string)supplierDataReader["SupplierName"];
                              suppliers.Add(newSupplier);
                         }
                    }
                    supplierDataReader.Close();
                    connection.Close();
               }
               catch (Exception ex)
               {
                    throw;
               }
               
               return suppliers;
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

                    DateTime openingDatetime = transactionList.Min(t => t.stocktake_datetime).AddMinutes(-1);

                    // create a connection object
                    int newPhysicalInventoryID = CreateNewPhysicalInventory(connection, code, openingDatetime);

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

          private static int CreateNewPhysicalInventory(SqlConnection connection,string code, DateTime openTime)
          {                 
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

          public static int CommitPurchaseOrdersToPOSDatabase()
          {

               SqlConnection connection = new SqlConnection(MakeConnectionString(Properties.Settings.Default.POSServerLocation,
                                                                                  Properties.Settings.Default.POSServerDBName,
                                                                                  Properties.Settings.Default.POSServerUser,
                                                                                  Properties.Settings.Default.POSServerPassword));

               try
               {
                    var orderList = Helpers.GetPurchaseOrdersToCommit();
                    int count = orderList.Count();

                    foreach (var order in orderList)
                    {
                         int newPurchaseOrderID = CreateNewPurchaseOrder(connection, order);
                         InsertPurchaseOrderEntries(order.itemList, newPurchaseOrderID);
                    }
                    Helpers.DeletePurchaseOrders();
                    return count;
               }
               catch
               {
                    throw;
               }
          }
              


          private static int CreateNewPurchaseOrder(SqlConnection connection, LocalPurchaseOrder order)
          {
               DateTime createdDatetime = DateTime.Now;              
               //Tell the SqlCommand what query to execute and what SqlConnection to use.  
               using (SqlCommand sqlCmd = new SqlCommand("INSERT INTO dbo.PurchaseOrder(POTitle, PONumber, DateCreated, SupplierID, [TO]) VALUES (@POTitle, @PONumber,@DateCreated, @SupplierID, @To)", connection))
               {

                    //Add SqlParameters to the SqlCommand  
                    sqlCmd.Parameters.AddWithValue("@POTitle", order.person + "_stocktakr_" + order.order_datetime.ToString("yyyy-MM-dd") );
                    sqlCmd.Parameters.AddWithValue("@PONumber", "s_"+order.purchaseorder_id.ToString());
                    sqlCmd.Parameters.AddWithValue("@DateCreated", createdDatetime);
                    sqlCmd.Parameters.AddWithValue("@SupplierID", Convert.ToInt32(order.supplier_code));
                    sqlCmd.Parameters.AddWithValue("@To", order.supplier_name);
                    
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

               using (SqlCommand sqlCmd = new SqlCommand("select ID from dbo.PurchaseOrder where PONumber = @PONumber", connection))
               {

                    //Add SqlParameters to the SqlCommand                     
                    sqlCmd.Parameters.AddWithValue("@PONumber", "s_" + order.purchaseorder_id.ToString());

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

          private static bool InsertPurchaseOrderEntries(LocalPurchaseOrderItem[] entries, int newPurchaseOrderID)
          {
               string connString = (MakeConnectionString(Properties.Settings.Default.POSServerLocation,
                                                                                   Properties.Settings.Default.POSServerDBName,
                                                                                   Properties.Settings.Default.POSServerUser,
                                                                                   Properties.Settings.Default.POSServerPassword));

               DateTime createdDatetime = DateTime.Now;

               DataTable newTable = new DataTable();
               
               newTable.Columns.Add("ItemDescription");
               newTable.Columns.Add("LastUpdated");
               newTable.Columns.Add("PurchaseOrderID");
               newTable.Columns.Add("QuantityOrdered");
               newTable.Columns.Add("ItemID");
               newTable.Columns.Add("Price");
               newTable.Columns.Add("TaxRate");
               
               foreach (var entry in entries)
               {    
                    decimal finalCostPrice = 0;
                    int itemID = Convert.ToInt32(entry.product_code);
                    
                    var localItem = GetLocalItem(itemID, connString);
                    if(localItem != null)
                    {
                         decimal? supplierPrice = LookupSupplierPrice(itemID, Convert.ToInt32(localItem.supplier_code), connString);
                         if(supplierPrice != null)
                         {
                              finalCostPrice  = (decimal)supplierPrice;
                         }
                         else
                         {
                              finalCostPrice = localItem.cost_price;
                         }

                         double taxPercentage = GetTaxRateForItem(itemID, connString);
                         newTable.Rows.Add(entry.description, createdDatetime, newPurchaseOrderID, entry.quantity, itemID, finalCostPrice, taxPercentage);
                    }                    
               }

               bool success = false;

               try
               {
                    using (SqlBulkCopy sbc = new SqlBulkCopy(connString))
                    {
                         sbc.DestinationTableName = "dbo.PurchaseOrderEntry";
        
                         // Number of records to be processed in one go
                         sbc.BatchSize = 1000;
  
                         // Map the Source Column from DataTabel to the Destination Columns in SQL Server 2005 Person Table
                         sbc.ColumnMappings.Add("ItemDescription", "ItemDescription");
                         sbc.ColumnMappings.Add("LastUpdated", "LastUpdated");                    
                         sbc.ColumnMappings.Add("PurchaseOrderID", "PurchaseOrderID");
                         sbc.ColumnMappings.Add("QuantityOrdered", "QuantityOrdered");
                         sbc.ColumnMappings.Add("ItemID", "ItemID");
                         sbc.ColumnMappings.Add("Price", "Price");
                         sbc.ColumnMappings.Add("TaxRate", "TaxRate");
                        
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


          //Australian stores only.
          private static double GetTaxRateForItem(int itemID, string connString)
          {
               SqlConnection connection = new SqlConnection(connString);
               double taxPercentage = 0;
               using (SqlCommand sqlCmd = new SqlCommand("select ItemTax.Description from Item  inner join ItemTax on Item.TaxID = ItemTax.ID where Item.ID = @itemID", connection))
               {

                    //Add SqlParameters to the SqlCommand                     
                    sqlCmd.Parameters.AddWithValue("@ItemID", itemID);
                    

                    //Open the SqlConnection before executing the query.  
                    try
                    {
                         connection.Open();
                         string description = (string)sqlCmd.ExecuteScalar();
                         if (description == "GST")
                         {
                              taxPercentage = 10;
                         }
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

               return taxPercentage;
          }

          private static decimal LookupSupplierPrice(int itemID, int supplierID, string connString)
          {
               SqlConnection connection = new SqlConnection(connString);

 	          using (SqlCommand sqlCmd = new SqlCommand("select Cost from dbo.SupplierList where ItemID = @ItemID and SupplierID = @SupplierID", connection))
               {
                    //Add SqlParameters to the SqlCommand                     
                    sqlCmd.Parameters.AddWithValue("@ItemID", itemID);
                    sqlCmd.Parameters.AddWithValue("@SupplierID", supplierID);

                    //Open the SqlConnection before executing the query.  
                    try
                    {
                         connection.Open();
                         return (decimal)sqlCmd.ExecuteScalar();
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

          private static LocalItem GetLocalItem(int itemID, string connString)
		{
               LocalItem newItem = new LocalItem();
			try
			{
				// create a connection object
				SqlConnection connection = new SqlConnection(connString);

				// create a command object
				SqlCommand selectCommand = connection.CreateCommand();

				connection.Open();

				//Get customers.


				//Get customer.
				selectCommand.CommandText = String.Format("select ID, ItemLookupCode, Description, Cost, Price, Quantity, SupplierID from Item where ID = @ItemID");
                    selectCommand.Parameters.AddWithValue("@ItemID", itemID);

				SqlDataReader itemDataReader = selectCommand.ExecuteReader();

				if (itemDataReader.HasRows)
				{
					itemDataReader.Read();	
						
				     int tempItemID = (int)itemDataReader["ID"];
				     newItem.product_code = tempItemID.ToString();

				     newItem.product_barcode = (string)itemDataReader["ItemLookupCode"];
				     newItem.description = (string)itemDataReader["Description"];
				     newItem.cost_price = (decimal)itemDataReader["Cost"];
				     newItem.sale_price = (decimal)itemDataReader["Price"];
				     newItem.quantity = (double)itemDataReader["Quantity"];
				     newItem.is_static = false;
                         newItem.supplier_code = itemDataReader["SupplierID"].ToString();
				}
				itemDataReader.Close();
				connection.Close();
			}
			catch (Exception ex)
			{
				throw;
			}
			
			return newItem;
		}
     }
}
