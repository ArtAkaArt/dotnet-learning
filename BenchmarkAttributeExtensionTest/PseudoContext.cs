namespace Test 
{ 
    public class PseudoContext
    {
        public IDictionary<string, object> ActionArguments;
        public PseudoContext(IDictionary<string,object> args)
        {
            ActionArguments = args;
        }
    }
}
