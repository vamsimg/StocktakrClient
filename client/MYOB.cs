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
          private static decimal GSTmultiplier = (decimal)1.1;

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
			          //Get items.
					string commandText = "SELECT stock_id, Barcode, description, cost, sell, quantity, static_quantity, supplier_id from Stock where stock_id > 0 and date_modified > ? ";
			       
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

                                   int supplierID = dbReader.GetInt32(7);
                                   if (supplierID != 0)
                                   {
                                        newItem.supplier_code = supplierID.ToString();
                                   }

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
				string commandText = "SELECT stock_id, Barcode, description, cost, sell, quantity, static_quantity, supplier_id from Stock where stock_id > 0";
				
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

                              int supplierID = dbReader.GetInt32(7);                              
                              newItem.supplier_code = supplierID.ToString();                             
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

          public static List<LocalSupplier> GetCompleteSupplierList()
          {
               string RMDBLocation = Properties.Settings.Default.RMDBLocation;

               List<LocalSupplier> suppliers = new List<LocalSupplier>();

               try
               {
                    OleDbConnection RMDBconnection = null;
                    OleDbDataReader dbReader = null;

                    RMDBconnection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0; User Id=; Password=; Data Source=" + RMDBLocation);
                    RMDBconnection.Open();

                    OleDbCommand SyncCmd = RMDBconnection.CreateCommand();
                    //Get customers.
                    string commandText = "SELECT supplier_id, supplier from Supplier";

                    SyncCmd.CommandText = commandText;

                    dbReader = SyncCmd.ExecuteReader();

                    if (dbReader.HasRows)
                    {
                         while (dbReader.Read())
                         {
                              LocalSupplier newSupplier = new LocalSupplier();
                              newSupplier.supplier_code = dbReader.GetValue(0).ToString();
                              newSupplier.name = dbReader.GetString(1);

                              suppliers.Add(newSupplier);
                         }
                    }

                    dbReader.Close();
                    RMDBconnection.Close();
               }
               catch (Exception ex)
               {
                    throw;
               }               
               return suppliers;
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

          public static int CommitPurchaseOrdersToPOSDatabase()
          {
               var orderList = Helpers.GetPurchaseOrdersToCommit();

               int staffID = GetLastStaffID();

               string RMDBLocation = Properties.Settings.Default.RMDBLocation;
               OleDbConnection RMDBconnection = null;
               OleDbDataReader dbReader = null;
               RMDBconnection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0; User Id=; Password=; Data Source=" + RMDBLocation);
               RMDBconnection.Open();

               try
               {
                    

                    string insertOrderCommandText = "INSERT INTO Orders (order_id,order_date, due_date, staff_id, supplier_id, order_suffix, comments) VALUES (?, ?, ?, ?, ?, ?,?)";
                    string insertItemCommandText = "INSERT INTO OrdersLine (line_id, order_id, supplier_id, stock_id, cost_ex, cost_inc, goods_tax, quantity, supcode) VALUES (? ,?, ?, ?, ?, ?, ?, ?, ?)";
                    string getItemCommandText = "SELECT goods_tax, cost, supplier_id from Stock where stock_id = ?";
                    string getLastOrderID = "Select Max(order_id) from Orders";
                    string getLastLineID = "Select Max(line_id) from OrdersLine";
                   

                    foreach (var order in orderList)
                    {

                         OleDbCommand insertOrderCommand = RMDBconnection.CreateCommand();

                         insertOrderCommand.CommandText = getLastOrderID;

                         int newOrderId = 1;
                         Object result = insertOrderCommand.ExecuteScalar();
                         if (result != DBNull.Value)
                         {
                              newOrderId = (int)result + 1;
                         }

                         insertOrderCommand.CommandText = insertOrderCommandText;
                         insertOrderCommand.Parameters.Add("@order_id", OleDbType.Integer).Value = newOrderId;
                         insertOrderCommand.Parameters.Add("@order_date", OleDbType.Date).Value = DateTime.Now;
                         insertOrderCommand.Parameters.Add("@due_date", OleDbType.Date).Value = DateTime.Now.AddDays(7);
                         insertOrderCommand.Parameters.Add("@staff_id", OleDbType.Integer).Value = staffID;
                         insertOrderCommand.Parameters.Add("@supplier_id", OleDbType.Integer).Value = Convert.ToInt32(order.supplier_code);
                         insertOrderCommand.Parameters.Add("@order_suffix", OleDbType.VarChar).Value = "s_" + order.purchaseorder_id.ToString();
                         insertOrderCommand.Parameters.Add("@comments", OleDbType.VarChar).Value = order.person;

                         insertOrderCommand.ExecuteNonQuery();



                         foreach (var item in order.itemList)
                         {
                              OleDbCommand getCmd = RMDBconnection.CreateCommand();
                              //Get Item details.		          

                              getCmd.CommandText = getItemCommandText;
                              getCmd.Parameters.Add("@stock_id", OleDbType.Integer).Value = item.product_code;
                              dbReader = getCmd.ExecuteReader();

                              if (dbReader.HasRows)
                              {
                                   dbReader.Read();

                                   string tax = dbReader.GetString(0);
                                   decimal cost = dbReader.GetDecimal(1);
                                   int supplier_id = dbReader.GetInt32(2);

                                   dbReader.Close();
                                   
                                   OleDbCommand insertItemCommand = RMDBconnection.CreateCommand();
                                   insertItemCommand.CommandText = getLastLineID;
                                   
                                   int newLineID = 1;
                                   result = insertItemCommand.ExecuteScalar();
                                   if(result != DBNull.Value)
                                   {
                                        newLineID = (int)insertItemCommand.ExecuteScalar() + 1;
                                   }

                                   insertItemCommand.CommandText = insertItemCommandText;

                                   insertItemCommand.Parameters.Add("@line_id", OleDbType.Integer).Value = newLineID;
                                   insertItemCommand.Parameters.Add("@order_id", OleDbType.Integer).Value = newOrderId;
                                   insertItemCommand.Parameters.Add("@supplier_id", OleDbType.Integer).Value = supplier_id;
                                   insertItemCommand.Parameters.Add("@stock_id", OleDbType.Integer).Value = Convert.ToDouble(item.product_code);
                                   insertItemCommand.Parameters.Add("@cost_ex", OleDbType.Currency).Value = cost;
                                   if (tax == "GST")
                                   {
                                        insertItemCommand.Parameters.Add("@cost_inc", OleDbType.Currency).Value = cost * GSTmultiplier;
                                   }
                                   else
                                   {
                                        insertItemCommand.Parameters.Add("@cost_inc", OleDbType.Currency).Value = cost;
                                   }
                                   insertItemCommand.Parameters.Add("@goods_tax", OleDbType.VarChar).Value = tax;
                                   insertItemCommand.Parameters.Add("@quantity", OleDbType.Double).Value = item.quantity;

                                   string supcode = GetSupCode(RMDBconnection, supplier_id, Convert.ToDouble(item.product_code));

                                   if (String.IsNullOrEmpty(supcode))
                                   {
                                        insertItemCommand.Parameters.Add("@supcode", OleDbType.VarChar).Value = item.product_barcode;
                                   }
                                   else
                                   {
                                        insertItemCommand.Parameters.Add("@supcode", OleDbType.VarChar).Value = supcode;
                                   }
                                  
                                   insertItemCommand.ExecuteNonQuery();
                              }
                         }
                    }



                    Helpers.DeletePurchaseOrders();
               }
               catch (Exception ex)
               {
                    throw;
               }
               finally
               {
                    RMDBconnection.Close();
               }
               
               
               return orderList.Count();
          }

          private static string GetSupCode(OleDbConnection RMDBconnection, int supplier_id, double stock_id)
          {
               string getSupplierCode = "Select supcode from SupplierCode where supplier_id = ? and stock_id = ?";


               OleDbCommand getSupplierCodeCommand = RMDBconnection.CreateCommand();
               getSupplierCodeCommand.CommandText = getSupplierCode;
               getSupplierCodeCommand.Parameters.Add("@supplier_id", OleDbType.Integer).Value = supplier_id;
               getSupplierCodeCommand.Parameters.Add("@stock_id", OleDbType.Double).Value = Convert.ToDouble(stock_id);


               string supcode = (string)getSupplierCodeCommand.ExecuteScalar();
               return supcode;
          }


          private static int GetLastStaffID()
          {
               string RMDBLocation = Properties.Settings.Default.RMDBLocation;
               int staffID;
               try
               {
                    OleDbConnection RMDBconnection = null;
                    OleDbDataReader dbReader = null;

                    RMDBconnection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0; User Id=; Password=; Data Source=" + RMDBLocation);
                    RMDBconnection.Open();

                    OleDbCommand SyncCmd = RMDBconnection.CreateCommand();
                    //Get customers.
                    string commandText = "SELECT Max(staff_id) from Staff";

                    SyncCmd.CommandText = commandText;

                    staffID = (int)SyncCmd.ExecuteScalar();                   
                    
                    RMDBconnection.Close();
               }
               catch (Exception ex)
               {
                    throw;
               }
               return staffID;
          }

          //private static Dictionary<int, string> GetStaffMembers()
          //{
          //     string RMDBLocation = Properties.Settings.Default.RMDBLocation;

          //     var dict = new Dictionary<int, string>();

          //     try
          //     {
          //          OleDbConnection RMDBconnection = null;
          //          OleDbDataReader dbReader = null;

          //          RMDBconnection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0; User Id=; Password=; Data Source=" + RMDBLocation);
          //          RMDBconnection.Open();

          //          OleDbCommand SyncCmd = RMDBconnection.CreateCommand();
          //          //Get customers.
          //          string commandText = "SELECT staff_id, given_names, surname from Staff";

          //          SyncCmd.CommandText = commandText;

          //          dbReader = SyncCmd.ExecuteReader();

          //          if (dbReader.HasRows)
          //          {
          //               while (dbReader.Read())
          //               {
          //                    int staff_id = dbReader.GetInt32(0);
          //                    string first_name = dbReader.GetString(1);
          //                    string last_name = dbReader.GetString(2);

          //                    string full_name = first_name + " " + last_name;
          //                    dict.Add(staff_id, full_name);
          //               }
          //          }

          //          dbReader.Close();
          //          RMDBconnection.Close();
          //     }
          //     catch (Exception ex)
          //     {
          //          throw;
          //     }
          //     return dict;              
          //}
	}
}
