using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace WebApplication1.App_Code
{
	public class Reliability
	{
		String dataCenter;
		int networkID;
		int farmID;
		String pipeline;
		DateTime start;
		DateTime end;

		/*
		 * 
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
		 * 
		 * 
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
		 * 
		 * 
		 * 
		 */
		public int CalculateComponent(String pComponent){
			int percentage = 0;
			//SqlConnection db = new SqlConnection("ViRT");
			//db.Open();
			//SqlCommand queryDB;

			String successTag;
			String failureTag;

			String query = "SELECT Date, Hour, NumberOfHits FROM ProdDollar_RandomJess ";
			String where = "WHERE Date > " + start + " AND Date < " + end;

			if (networkID == -1 && farmID == -1)
			{
				where = "AND ";
			}
			else if (networkID != -1 && farmID == -1)
			{

			}
			else if (networkID != -1 && farmID != -1)
			{

			}

			return percentage;
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


	}
}