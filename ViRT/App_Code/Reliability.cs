using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace Test
{
	public class Reliability
	{
		private String dataCenter;
		private int networkID;
		private int farmID;
		private String pipeline;
		private DateTime start;
		private DateTime end;

		/*
		 * Default constructor
		 * Used when a person first enters the page.
		 * -1 refers to all "all networkId" and "all farmIDs)
		 * dataCenter = "all" means every data center's data is queried.
		 * 
		 */
		public Reliability()
		{
			dataCenter = "all";
			networkID = -1;
			farmID = -1;
			pipeline = "overview";

			start = DateTime.Now.AddDays(-1);
			end = DateTime.Now.AddDays(-8);
			//Time span is 7 days.
		}

		/*
		 * Constructor which initizalizes all the values.
		 * 
		 * @param pDataCenter specified Data Center
		 * @param pNetworkID specified Network ID
		 * @param pPipeline specified pipeline
		 * @param pStart start date in DateTime
		 * @param pEnd end date in DateTime
		 */
		public Reliability(String pDataCenter, int pNetworkID, int pFarmID, String pPipeline,
			DateTime pStart, DateTime pEnd)
		{
			dataCenter = pDataCenter;
			networkID = pNetworkID;
			farmID = pFarmID;
			pipeline = pPipeline;
			start = pStart;
			end = pEnd;
		}

		/*
		 * Calculates the reliability of a single component
		 * 
		 * @param pComponent	Component which reliability is calculated
		 * @return		A DataTable with the relailbity calculation of every every hour for the component
		 */
		private DataTable CalculateComponent(String pComponent, SqlConnection dbConnect){
			//get success and fail tags
			String query = "SELECT SuccessTag, FailureTag FROM Component WHERE Components = '" + pComponent + "'";
			SqlCommand queryCommand = new SqlCommand(query, dbConnect);
			SqlDataReader queryCommandReader = queryCommand.ExecuteReader();

			//Get success and failure tags into a table
			DataTable twoTags = new DataTable();
			twoTags.Load(queryCommandReader);

			//Pick out the two tags and convert 
			DataColumn col = twoTags.Columns[0];
			String successTag = (String)twoTags.Rows[0][col.ColumnName];
			col = twoTags.Columns[1];
			String failureTag = (String)twoTags.Rows[0][col.ColumnName];

			//DataTable which have date, hour and hits
			DataTable successTable = tagTable(successTag, dbConnect);
			DataTable failureTable = tagTable(failureTag, dbConnect);

			//return calculated table that has dates and percentages
			return Calculate(successTable, failureTable);
		}

		/*
		 * Retrieves the tags for a component
		 * 
		 * @param pTag		Tag to get info
		 * @param connect	The connection to DB
		 * @return		Table with all the entries for that tag
		 */
		private DataTable tagTable(String pTag , SqlConnection connect)
		{
			//Strings that create the query
			String query = "SELECT Date, Hour, NumberOfHits FROM ProdDollar_RandomJess";
			String where = " WHERE Tag = '" + pTag + "'";

			//Creates the remainer of the where portion of the query
			if (!dataCenter.Equals("all"))
			{
				if (networkID != -1 && farmID == -1)
				{
					where = where + " AND NetworkID = " + networkID.ToString();
				}
				else if (networkID != -1 && farmID != -1)
				{
					where += " AND NetworkID = " + networkID.ToString() + " AND FarmID = " + farmID.ToString();
				}
			}
			
			//concatenate the where to the original query
			query = query + where;

			//Gets the query info
			SqlCommand queryCommand = new SqlCommand(query, connect);
			SqlDataReader queryCommandReader = queryCommand.ExecuteReader();

			//Put query into a DataTable
			DataTable tagTable = new DataTable();
			tagTable.Load(queryCommandReader);

			//Return the table
			return tagTable;
		}

		/*
		 * Calculates the % for a specific time
		 * 
		 * @param sTable		Success table
		 * @param fTable		Failure table
		 * @return		Data and Time table
		 */
		private DataTable Calculate(DataTable sTable, DataTable fTable)
		{
			int length = sTable.Rows.Count;

			DataTable datePercent = new DataTable();
			datePercent.Clear();

			DataColumn colDateTime = new DataColumn("Date");
			colDateTime.DataType = System.Type.GetType("System.DateTime");

			DataColumn colPercent = new DataColumn("Percent");
			colPercent.DataType = System.Type.GetType("System.Decimal");

			datePercent.Columns.Add(colDateTime);
			datePercent.Columns.Add(colPercent);

			DataRow toAdd = datePercent.NewRow();
			int succHits;
			int failHits;
			decimal per;

			//calculate reliability
			for (int i = 0; i < length; i++)
			{
				int time = (int)sTable.Rows[i]["Hour"];
				toAdd["Date"] = ((DateTime)sTable.Rows[i]["Date"]).AddHours(time);

				succHits = (int)sTable.Rows[i]["NumberOfHits"];
				failHits  = (int)fTable.Rows[i]["NumberOfHits"];

				per = ((decimal)succHits / (succHits + failHits)) * 100;
				per = Math.Round(per, 4);
				Console.WriteLine(per);
				toAdd["Percent"] = per; 

				datePercent.Rows.Add(toAdd);

				if (!(i == length - 1))
				{
					toAdd = datePercent.NewRow();
				}
			}

			return datePercent;
		}

		/*
		 * 
		 * 
		 */
		public DataTable RawDataTable(String pComp)
		{
			SqlConnection dbConnect = new SqlConnection("Data Source=FIDEL3127;Initial Catalog=VisDataTestCOSMOS;Integrated Security=True;");
			dbConnect.Open();
			String query = "SELECT * FROM Pipeline";
			SqlCommand queryCommand = new SqlCommand(query, dbConnect);
			SqlDataReader queryCommandReader = queryCommand.ExecuteReader();

			DataTable tagTable = new DataTable();
			tagTable.Clear();

			DataColumn colDateTime = new DataColumn("Date");
			colDateTime.DataType = System.Type.GetType("System.DateTime");

			DataColumn colSTag = new DataColumn("SuccessTag");
			colSTag.DataType = System.Type.GetType("System.Int");

			DataColumn colFTag = new DataColumn("FailureTag");
			colFTag.DataType = System.Type.GetType("System.Int");

			return null;
		}

		/*
		 * 
		 * 
		 */
		public DataTable OverviewCalculate()
		{
			SqlConnection dbConnect = new SqlConnection("Data Source=FIDEL3127;Initial Catalog=VisDataTestCOSMOS;Integrated Security=True;");
			dbConnect.Open();
			String query = "SELECT * FROM Pipeline";
			SqlCommand queryCommand = new SqlCommand(query, dbConnect);
			SqlDataReader queryCommandReader = queryCommand.ExecuteReader();

			DataTable pipelineTable = new DataTable();
			pipelineTable.Load(queryCommandReader);

			int length = pipelineTable.Rows.Count;
			DataTable temp;
			decimal total = 0;
			
			DataTable retTable = new DataTable();
			retTable.Clear();

			DataColumn colDateTime = new DataColumn("Pipeline");
			colDateTime.DataType = System.Type.GetType("System.String");

			DataColumn colPercent = new DataColumn("Percent");
			colPercent.DataType = System.Type.GetType("System.Decimal");

			retTable.Columns.Add(colDateTime);
			retTable.Columns.Add(colPercent);

			DataRow toAdd = retTable.NewRow();

			for (int i = 0; i < length; i++)
			{
				temp = CalculateComponent((String)pipelineTable.Rows[i]["Pipeline"], dbConnect);

				for (int j = 0; j < temp.Rows.Count; j++)
				{
					total = total + (decimal)temp.Rows[j]["Percent"];
				}

				total = total / temp.Rows.Count;

				toAdd["Pipeline"] = (string)pipelineTable.Rows[i]["Pipleline"];
				toAdd["Percent"] = total;
				retTable.Rows.Add(toAdd);

				if (!(i == length - 1))
				{
					toAdd = retTable.NewRow();
				}
			}

			dbConnect.Close();
			return null;
		}

		/*
		 * 
		 * 
		 */
		public DataTable PipelineCalculate(String pPipleine)
		{
			SqlConnection dbConnect = new SqlConnection("Data Source=FIDEL3127;Initial Catalog=VisDataTestCOSMOS;Integrated Security=True;");
			dbConnect.Open();
			String query = "SELECT Component FROM PipelineComponent WHERE Pipeline = '" + pPipleine + "'";
			SqlCommand queryCommand = new SqlCommand(query, dbConnect);
			SqlDataReader queryCommandReader = queryCommand.ExecuteReader();
			DataTable componentsTable = new DataTable();
			componentsTable.Load(queryCommandReader);

			int length = componentsTable.Rows.Count;

			DataTable[] datePercents = new DataTable[length];

			for (int i = 0; i < length; i++)
			{
				datePercents[i] = CalculateComponent((string)componentsTable.Rows[i]["Component"], dbConnect);
			}

			DataTable dt = new DataTable();
			dt.Columns.Add("Date", typeof(DateTime));
			dt.Columns.Add("Comp1", typeof(decimal));
			dt.Columns.Add("Comp2", typeof(decimal));

			var result = from dataRows1 in datePercents[0].AsEnumerable()
						 join dataRows2 in datePercents[1].AsEnumerable()
						 on dataRows1.Field<DateTime>("Date") equals dataRows2.Field("Date")

						 select dt.LoadDataRow(new object[]
						 {
							 dataRows1.Field("Date"),
							 dataRows1.Field("Percent"),
							 dataRows2.Field("Percent")
						 },false);

			result.CopyToDataTable();
		}

		/*
		 * 
		 * 
		 */
		public void ChangeDate(DateTime pStart, DateTime pEnd)
		{
			start = pStart;
			end = pEnd;
		}

		/*
		 * 
		 * 
		 */
		public void ChangeDataCenter(String pDataCenter)
		{
			dataCenter = pDataCenter;
			networkID = -1;
			farmID = -1;
		}

		/*
		 * 
		 * 
		 * 
		 */
		public void ChangeNetworkID(int pNetworkID)
		{
			networkID = pNetworkID;
			farmID = -1;

			//Need to add the DataCenter to this.
		}
	}
}
/*
			String total = "";

			foreach (DataColumn column in successTable.Columns)
			{
				total += column.ColumnName + " | ";
			}

			Console.WriteLine(total);


			int topRows = 10;
			for (int i = 0; i < topRows; i++)
			{
				String rowText = string.Empty;
				foreach (DataColumn column in successTable.Columns)
				{
					rowText += successTable.Rows[i][column.ColumnName] + " | ";
				}
				Console.WriteLine(rowText);
			}
*/