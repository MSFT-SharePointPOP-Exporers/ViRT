﻿using System;
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
		private SqlConnection dbConnect = new SqlConnection("Data Source=FIDEL3127;Initial Catalog=VisDataTestCOSMOS;Integrated Security=True;");

		/*
		 * Default constructor
		 * Used when a person first enters the page.
		 * -1 refers to all "all networkId" and "all farmIDs)
		 * dataCenter = "all" means every data center's data is queried.
		 */
		public Reliability()
		{
			dataCenter = "all";
			networkID = -1;
			farmID = -1;
			pipeline = "overview";

			start = DateTime.Today.AddDays(-9);
			end = DateTime.Today.AddDays(-1);
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
			DataTable successTable = tagTable(successTag);
			DataTable failureTable = tagTable(failureTag);

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
		private DataTable tagTable(String pTag)
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
			SqlCommand queryCommand = new SqlCommand(query, dbConnect);
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
				//Console.WriteLine(per);
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
		public DataTable OverviewCalculate()
		{
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
			return retTable;
		}

		/*
		 * 
		 * 
		 * 
		 */
		public DataTable RawDataTable(String pComponent)
		{
			dbConnect.Open();
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

			DataTable successTable = tagTable(successTag);
			DataTable failureTable = tagTable(failureTag);

			DataTable formatSuccessTable = new DataTable();
			formatSuccessTable.Columns.Add("Date", typeof(DateTime));
			formatSuccessTable.Columns.Add("Tag", typeof(int));

			DataTable formatFailureTable = new DataTable();
			formatFailureTable.Columns.Add("Date", typeof(DateTime));
			formatFailureTable.Columns.Add("Tag", typeof(int));

			DataRow toAdd = formatSuccessTable.NewRow();

			for (int i = 0; i < successTable.Rows.Count; i++)
			{
				int time = (int)successTable.Rows[i]["Hour"];
				toAdd["Date"] = ((DateTime)successTable.Rows[i]["Date"]).AddHours(time);
				toAdd["Tag"] = successTable.Rows[i]["NumberOfHits"];
				formatSuccessTable.Rows.Add(toAdd);
				toAdd = formatSuccessTable.NewRow();
			}

			toAdd = formatFailureTable.NewRow();
			for (int i = 0; i < failureTable.Rows.Count; i++)
			{
				int time = (int)failureTable.Rows[i]["Hour"];
				toAdd["Date"] = ((DateTime)failureTable.Rows[i]["Date"]).AddHours(time);
				toAdd["Tag"] = failureTable.Rows[i]["NumberOfHits"];
				formatFailureTable.Rows.Add(toAdd);
				toAdd = formatFailureTable.NewRow();
			}

			DataTable dt = new DataTable();
			dt.Columns.Add("Date", typeof(DateTime));
			dt.Columns.Add(successTag, typeof(int));
			dt.Columns.Add(failureTag, typeof(int));
			toAdd = dt.NewRow();

			for(DateTime i = start; i < end; i = i.AddHours(1))
			{
				toAdd["Date"] = i;

				for (int j = 0; j < formatSuccessTable.Rows.Count; j++ )
				{
					if ((DateTime)formatSuccessTable.Rows[j]["Date"] == i)
					{
						toAdd[successTag] = formatSuccessTable.Rows[j]["Tag"];
					}
				}

				for (int j = 0; j < formatFailureTable.Rows.Count; j++)
				{
					if ((DateTime)formatFailureTable.Rows[j]["Date"] == i)
					{
						toAdd[failureTag] = formatFailureTable.Rows[j]["Tag"];
					}
				}

				dt.Rows.Add(toAdd);
				toAdd = dt.NewRow();
			}

			dbConnect.Close();
			return dt;
		}

		/*
		 * 
		 * 
		 */
		public DataTable PipelineCalculate(String pPipleine)
		{
			//connect to DB and query for 
			dbConnect.Open();
			
			//Get all components from pipeline
			String query = "SELECT Component FROM PipelineComponent WHERE Pipeline = '" + pPipleine + "'";
			SqlCommand queryCommand = new SqlCommand(query, dbConnect);
			SqlDataReader queryCommandReader = queryCommand.ExecuteReader();
			
			//componentsTable has all the components from the pipeline
			DataTable componentsTable = new DataTable();
			componentsTable.Load(queryCommandReader);

			//The number of components the pipeline has and stores the names into an array of strings
			int length = componentsTable.Rows.Count;
			string[] comps = new string[length];
			for (int i = 0; i < length; i++)
			{
				comps[i] = (string)componentsTable.Rows[i]["Component"];
			}

			//datePercent is an array which will hold all the tables with the calculated % for every component
			DataTable[] datePercents = new DataTable[length];

			//DataTable which hold the date and percent of all the components
			DataTable dt = new DataTable();
			dt.Columns.Add("Date", typeof(DateTime));

			//Goes through every component and pulls the percents
			//Also creates new columns for the date and percents table with all the components
			for (int i = 0; i < length; i++)
			{
				datePercents[i] = CalculateComponent(comps[i], dbConnect);
				dt.Columns.Add(comps[i], typeof(decimal));
			}
			DataRow toAdd = dt.NewRow();

			for (DateTime i = start; i < end; i = i.AddHours(1))
			{
				toAdd["Date"] = i;

				for(int j = 0; j < datePercents.Length; j++)
				{
					for (int k = 0; k < datePercents[j].Rows.Count; k++)
					{
						if((DateTime)datePercents[j].Rows[k]["Date"] == i)
						{
							toAdd[comps[j]] = datePercents[j].Rows[k]["Percent"];
						}
					}
				}
				dt.Rows.Add(toAdd);
				toAdd = dt.NewRow();
			}
			return dt; 
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