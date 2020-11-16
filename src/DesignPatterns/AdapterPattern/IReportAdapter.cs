namespace CrystalDecisions.CrystalReports
{
    public interface IReportAdapter
    {
        void Generate(string templateFilename, string outputFilename);
    }


}
