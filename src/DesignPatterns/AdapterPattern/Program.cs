using CrystalDecisions.CrystalReports;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace AdapterPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Adapter Pattern!");

            MotorolaRadioTest();

            HyteriaRadioTest();

            // CrystalReportTest();
        }

        private static void CrystalReportTest()
        {
            IReportAdapter reportAdapter = new CrystalReportsAdapter();
            reportAdapter.Generate("raport1.rpt", "raport1.pdf");
        }

        private static void MotorolaRadioTest()
        {
            IRadioAdapter radio = new MotorolaRadioAdapter("1234");
            radio.Send(10, "Hello World!");
        }

        private static void HyteriaRadioTest()
        {
            IRadioAdapter radio = new HyteraRadioAdapter();
            radio.Send(10, "Hello World!");
        }
    }

    


}
