using Denver.Facade.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Parça Ekleme Testi

            PartManagerFacade partManagerFacade = new PartManagerFacade();
            var retCode=partManagerFacade.AddToStock(9, 100, 90.95, 1000, "Vakum Tüpü", 500, "Rio Vakum Tüpleri Firması", "Sanayi tipi 500 miliwatt çekim gücüne sahip vakum tüpüdür");
            Console.WriteLine("Parça eklenme durumu ->" + retCode.ToString());


            #endregion
        }
    }
}
