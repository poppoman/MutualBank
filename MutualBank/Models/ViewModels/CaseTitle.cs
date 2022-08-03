namespace MutualBank.Models.ViewModels
{
    public class CaseTitle
    {
        public string casetitle { get; set; }

        public int caseid { get; set; }
        
        public bool casehelp { get; set; }

        public List<MsgandRead> casemsg { get; set; }

        public int read { get; set; }

        public string caseadddate { get; set; }
    }
}

