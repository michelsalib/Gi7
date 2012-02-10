namespace Gi7.Client.Model
{
    public class Branch : BoolModel
    {
        public string Name { get; set; }
        public BranchCommit Commit { get; set; }
    }
}
