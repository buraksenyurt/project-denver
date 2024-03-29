﻿using Denver.Facade.Parts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Parça Ekleme Testi

            //PartManagerFacade partManagerFacade = new PartManagerFacade();
            //var retCode=partManagerFacade.AddToStock(9, 100, 99M, 1000, "Vakum Tüpü", 500, "Rio Vakum Tüpleri Firması", "Sanayi tipi 500 miliwatt çekim gücüne sahip vakum tüpüdür");
            //Console.WriteLine("Parça eklenme durumu ->" + retCode.ToString());


            #endregion

            #region Parçe Çekme Testi

            PartManagerFacade partManagerFacade = new PartManagerFacade();
            DataSet partSet=partManagerFacade.GetParts();
            foreach (DataRow row in partSet.Tables[0].Rows)
            {
                Console.WriteLine("{0}-{1}-{2}-{3}-{4}", row["partCode"].ToString(), row["name"],row["quantity"].ToString(),row["supplier"],row["description"]);
            }
            #endregion
        }
    }
}
