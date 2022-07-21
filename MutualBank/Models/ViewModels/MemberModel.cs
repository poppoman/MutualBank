namespace MutualBank.Models.ViewModels
{
    public class MemberModel
    {
        public IEnumerable<string> casetitle { get; set; }

        public IQueryable<int> caseid { get; set; }
    }
}

