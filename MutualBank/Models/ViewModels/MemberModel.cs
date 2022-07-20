namespace MutualBank.Models.ViewModels
{
    public class MemberModel
    {
        public int userid { get; set; }
        public IEnumerable<string> casetitle { get; set; }
        public IQueryable<bool> caseNeed { get; set; }
    }
}

