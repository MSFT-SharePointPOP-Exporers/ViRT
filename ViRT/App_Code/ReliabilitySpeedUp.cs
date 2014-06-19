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
			pipeline = "Overview";

			start = DateTime.Today.AddDays(-8);
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
		private DataTable CalculateComponent(String pComponent){
			//get success and fail tags
			String query = "SELECT SuccessTag, FailureTag FROM Component WHERE Component = '" + pComponent + "'";
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
			DataTable successTable = TagTable(successTag);
			DataTable failureTable = TagTable(failureTag);

			//return calculated table that has dates and percentages
			return CalculatePercent(successTable, failureTable);
		}

		/*
		 * Retrieves the Date, Hour, and Number of Hits for a single Tag
		 * 
		 * @param pTag		Tag to get info
		 * @param connect	The connection to DB
		 * @return		Table with all the entries for that tag
		 */
		private DataTable TagTable(String pTag)
		{
			//Strings that create the query
			String query = "SELECT Date, Hour, NumberOfHits FROM ProdDollar_RandomJess";
			String where = " WHERE Tag = '" + pTag + "' AND Date >= '" + start.ToString() + "' AND Date <= '" + end.ToString() + "'";

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
		 * Calculates the success percentage for every time where a component has both
		 * success and failure tags
		 * 
		 * Need to work on the feature where if one of the tags is missing, the percent is missing
		 * 
		 * @param sTable		Success table
		 * @param fTable		Failure table
		 * @return		Data and Time table
		 */
		private DataTable CalculatePercent(DataTable sTable, DataTable fTable)
		{
			int length = sTable.Rows.Count;

			DataTable datePercent = new DataTable();

			datePercent.Columns.Add("Date", typeof(DateTime));
			datePercent.Columns.Add("Percent", typeof(decimal));

			DataRow toAdd = datePercent.NewRow();
			int succHits;
			int failHits;
			decimal per;

			//calculate reliability
			//What if the dates dont match up? That would be bad
			//MUST BE FIXED!
			for (int i = 0; i < length; i++)
			{
				int time = (int)sTable.Rows[i]["Hour"];
				toAdd["Date"] = ((DateTime)sTable.Rows[i]["Date"]).AddHours(time);

				succHits = (int)sTable.Rows[i]["NumberOfHits"];
				failHits  = (int)fTable.Rows[i]["NumberOfHits"];

				per = ((decimal)succHits / (succHits + failHits)) * 100;
				per = Math.Round(per, 4);

				toAdd["Percent"] = per; 

				datePercent.Rows.Add(toAdd);

				toAdd = datePercent.NewRow();
			}

			return datePercent;
		}

		/*
		 * Calculates an overview of a pipeline
		 * 
		 * @param pPipeline		The pipeline to calculate the overview
		 * @return		DataTable with Component Column and Percent Column
		 */
		public DataTable OverviewCalculate(String pPipeline)
		{
			//Open db and create a new query that gets all the components in the pipeline
			dbConnect.Open();
			String query = "SELECT Component FROM PipelineComponent WHERE Pipeline = '" + pPipeline + "'";
			SqlCommand queryCommand = new SqlCommand(query, dbConnect);
			SqlDataReader queryCommandReader = queryCommand.ExecuteReader();

			//Put all the pipeline names into a DataTable
			DataTable componentTable = new DataTable();
			componentTable.Load(queryCommandReader);

			//Create new DatTable with two columns named Pipeline and Percent
			DataTable retTable = new DataTable();
			retTable.Columns.Add("Component", typeof(String));
			retTable.Columns.Add("Percent", typeof(decimal));

			//Create new variables needed to fill retTable
			DataRow toAdd = retTable.NewRow();
			DataTable temp;
			decimal total = 0;


			//Iterate the pipeline table
			for (int i = 0; i < componentTable.Rows.Count; i++)
			{
				//Temp holds the values of a single component in the pipeline
				temp = CalculateComponent((String)componentTable.Rows[i]["Component"]);

				//Check for divide by 0
				if (temp.Rows.Count != 0)
				{
					//Iterate through temp and add the values
					for (int j = 0; j < temp.Rows.Count; j++)
					{
						total = total + (decimal)temp.Rows[j]["Percent"];
					}

					total = total / temp.Rows.Count;
				}
				else
				{
					total = 0;
				}

				//Add the Component name and the totalAverage to the return table
				toAdd["Component"] = (string)componentTable.Rows[i]["Component"];
				toAdd["Percent"] = Math.Round(total, 4);
				retTable.Rows.Add(toAdd);

				toAdd = retTable.NewRow();

			}

			//Close connection and return the table
			dbConnect.Close();
			return retTable;
		}

		/*
		 * Retrieves the raw numbers by date and time for Success and Failure Tags of a Component
		 * 
		 * @param pComponent		The component which the raw numbers will be retreived for
		 * @return		DataTable with Dates, Success Tag Hits, and Failure Tag Hits
		 */
		public DataTable RawDataTable(String pComponent)
		{
			//Open connection and query DB
			dbConnect.Open();
			String query = "SELECT SuccessTag, FailureTag FROM Component WHERE Component = '" + pComponent + "'";
			SqlCommand queryCommand = new SqlCommand(query, dbConnect);
			SqlDataReader queryCommandReader = queryCommand.ExecuteReader();

			//Get success and failure tags into a table
			DataTable twoTags = new DataTable();
			twoTags.Load(queryCommandReader);

			//Get the names of the two tags
			DataColumn col = twoTags.Columns[0];
			String successTag = (String)twoTags.Rows[0][col.ColumnName];
			col = twoTags.Columns[1];
			String failureTag = (String)twoTags.Rows[0][col.ColumnName];

			//Fill DataTables with the hits for that tag
			DataTable successTable = TagTable(successTag);
			DataTable failureTable = TagTable(failureTag);

			//Since the dates are formatted weird in the DB, the dates need to be adjusted
			DataTable formatSuccessTable = new DataTable();
			formatSuccessTable.Columns.Add("Date", typeof(DateTime));
			formatSuccessTable.Columns.Add("Tag", typeof(int));

			DataTable formatFailureTable = new DataTable();
			formatFailureTable.Columns.Add("Date", typeof(DateTime));
			formatFailureTable.Columns.Add("Tag", typeof(int));

			DataRow toAdd = formatSuccessTable.NewRow();

			//Creating adjusted tables with dates which can be easily worked with
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

			//Create the DataTable which will be returned
			//Columns are Date, SuccessTagHits, FailureTagHits
			DataTable dt = new DataTable();
			dt.Columns.Add("Date", typeof(DateTime));
			dt.Columns.Add(successTag, typeof(int));
			dt.Columns.Add(failureTag, typeof(int));
			toAdd = dt.NewRow();

			int quickDateUpSuccess = 0;
			int quickDateUpFailure = 0;

			//Iterate through the entire timespan given in the object
			for(DateTime i = start; i < end; i = i.AddHours(1))
			{
				toAdd["Date"] = i;

				//Iterate through the successTable and add any entries which are present
				for (int j = quickDateUpSuccess; j < formatSuccessTable.Rows.Count; j++)
				{
					quickDateUpSuccess++;
					if ((DateTime)formatSuccessTable.Rows[j]["Date"] == i)
					{
						toAdd[successTag] = formatSuccessTable.Rows[j]["Tag"];
						j = formatSuccessTable.Rows.Count;
					}
				}

				//Iterate through the failureTable and add any entries which are present
				for (int j = quickDateUpFailure; j < formatFailureTable.Rows.Count; j++)
				{
					quickDateUpFailure++;
					if ((DateTime)formatFailureTable.Rows[j]["Date"] == i)
					{
						toAdd[failureTag] = formatFailureTable.Rows[j]["Tag"];
						j = formatFailureTable.Rows.Count;
					}
				}

				//Add the row and continue
				dt.Rows.Add(toAdd);
				toAdd = dt.NewRow();
			}

			//Close and return the DataTable
			dbConnect.Close();
			return dt;
		}

		/*
		 * Calculate all the percentages for a pipeline's components
		 * 
		 * @param pPipeline		The pipeline for all the components
		 * @return		A DataTable which holds the percentages of all the components for the given time
		 */
		public DataTable PipelineCalculate(String pPipeline)
		{
			//connect to DB and query for 
			dbConnect.Open();
			
			//Get all components from pipeline
			String query = "SELECT Component FROM PipelineComponent WHERE Pipeline = '" + pPipeline + "'";
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
				datePercents[i] = CalculateComponent(comps[i]);
				dt.Columns.Add(comps[i], typeof(decimal));
			}
			DataRow toAdd = dt.NewRow();

			//Iterate throught the entire time period
			for (DateTime i = start; i < end; i = i.AddHours(1))
			{
				toAdd["Date"] = i;
				//Iterate through all the datePercent components tables
				for(int j = 0; j < datePercents.Length; j++)
				{
					//Iterate through a datePercent table
					for (int k = 0; k < datePercents[j].Rows.Count; k++)
					{
						//Check if there are entries with the time i
						if((DateTime)datePercents[j].Rows[k]["Date"] == i)
						{
							toAdd[comps[j]] = datePercents[j].Rows[k]["Percent"];
						}
					}
				}
				//Add the entries to the return table
				dt.Rows.Add(toAdd);
				toAdd = dt.NewRow();
			}

			//Close db and return table
			dbConnect.Close();
			return dt; 
		}
		
		/*
		 * Changes the start and end date
		 * 
		 * @param pStart		New start date
		 * @param pEnd			New end date
		 */
		public void ChangeDate(DateTime pStart, DateTime pEnd)
		{
			start = pStart;
			end = pEnd;
		}

		/*
		 * Changes the Data Center and changes network and farm to default
		 * 
		 * @param pDataCenter		Desired new data center
		 */
		public void ChangeDataCenter(String pDataCenter)
		{
			dataCenter = pDataCenter;
			networkID = -1;
			farmID = -1;
		}

		/*
		 * Changes the Network and changes the farm to the default
		 * 
		 * @param pNetworkID		Desired new networkID
		 */
		public void ChangeNetworkID(int pNetworkID)
		{
			networkID = pNetworkID;
			farmID = -1;
		}
		
		/*
		 * Change the farmID
		 * 
		 * @param pFarmID		Desired new farmID
		 */
		public void ChangeFarmID(int pFarmID)
		{
			farmID = pFarmID;
		}

		/*
		 * Change both the Data Center and NetworkId and make farm default
		 * 
		 * @param pDataCenter		New DataCenter
		 * @param pNetworkID		New NetworkID
		 */
		public void ChangeDataCenterNetworkID(String pDataCenter, int pNetworkID)
		{
			dataCenter = pDataCenter;
			networkID = pNetworkID;
			farmID = -1;
		}

		/*
		 * Changes the NetworkID and the FarmID
		 * 
		 * @param pNetworkID		New NetworkID
		 * @param pFarmID			New FarmID
		 */
		public void ChangeNetworkIDFarmID(int pNetworkID, int pFarmID)
		{
			networkID = pNetworkID;
			farmID = pFarmID;
		}

		/*
		 * Change all location filters
		 * 
		 * @param pDataCenter		New DataCenter
		 * @param pNetworkID		New NetworkID
		 * @param pFarmID			New FarmID
		 */
		public void ChangeLocationFilter(String pDataCenter, int pNetworkID, int pFarmID)
		{
			dataCenter = pDataCenter;
			networkID = pNetworkID;
			farmID = pFarmID;
		}

		/*
		 * Changes all filters
		 * 
		 * @param pDataCenter		New DataCenter
		 * @param pNetworkID		New NetworkID
		 * @param pFarmID			New FarmID
		 * @param pPipeline			New Pipeline
		 * @param pStart			New start time
		 * @param pEnd				New End time
		 */
		public void ChangeAllFilters(String pDataCenter, int pNetworkID, int pFarmID, String pPipeline,
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
		 * Changes the pipeline
		 * 
		 * @param pPipeline		New Pipeline
		 */
		public void ChangePipeline(String pPipeline)
		{
			pipeline = pPipeline;
		}

		/*
		 * 
		 */
		public String[] getComponents(String pPipeline)
		{
			dbConnect.Open();
			String query = "SELECT Component FROM PipelineComponent WHERE Pipeline = '" + pPipeline + "'";
			SqlCommand queryCommand = new SqlCommand(query, dbConnect);
			SqlDataReader queryCommandReader = queryCommand.ExecuteReader();
			DataTable componentsForPipeline = new DataTable();
			componentsForPipeline.Load(queryCommandReader);


			String[] compsArray = new String[componentsForPipeline.Rows.Count];

			for (int i = 0; i < componentsForPipeline.Rows.Count; i++ )
			{
				compsArray[i] = (String)componentsForPipeline.Rows[i]["Component"];
			}

			dbConnect.Close();
			return compsArray;
		}
	}
}