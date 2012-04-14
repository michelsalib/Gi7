namespace Gi7.Client.Model
{
    public class Branch : BoolModel
    {
        public string Name { get; set; }

        public Commit Commit { get; set; }
    }
}
