namespace Gi7.Model
{
    public class BoolModel
    {
        public static implicit operator bool(BoolModel p)
        {
            return p != null;
        }
    }
}