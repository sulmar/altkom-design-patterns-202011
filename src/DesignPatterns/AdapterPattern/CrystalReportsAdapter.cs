namespace CrystalDecisions.CrystalReports
{

   


    public class CrystalReportsAdapter : IReportAdapter
    {

        private readonly ReportDocument rpt = new ReportDocument();

        public void Generate(string templateFilename, string outputFilename)
        {
            rpt.Load(templateFilename);

            ConnectionInfo connectInfo = new ConnectionInfo()
            {
                ServerName = "MyServer",
                DatabaseName = "MyDb",
                UserID = "myuser",
                Password = "mypassword"
            };

            foreach (Table table in rpt.Database.Tables)
            {
                table.LogOnInfo.ConnectionInfo = connectInfo;
                table.ApplyLogOnInfo(table.LogOnInfo);
            }

            rpt.ExportToDisk(ReportDocument.ExportFormatType.PortableDocFormat, outputFilename);
        }
    }


}
