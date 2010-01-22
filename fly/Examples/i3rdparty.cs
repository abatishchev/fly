using System;

namespace ThirdPartlyLib
{
    public interface I3rdParty
    {
        string Foo(string value);
    }

    public class c3rdParty : I3rdParty
    {
        public string Foo(string value)
        {
            return value.ToLower();
        }
    }
}
