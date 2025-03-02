using Xunit;

namespace Light.GuardClauses.Tests.StringAssertions;

public class InvalidEmailAddresses : TheoryData<string>
{
    public InvalidEmailAddresses()
    {
        Add("plainaddress");
        Add("#@%^%#$@#$@#.com");
        Add("@domain.com");
        Add("Joe Smith <email@domain.com>");
        Add("email.domain.com");
        Add("email@domain@domain.com");
        Add(".email@domain.com");
        Add("email.@domain.com");
        Add("email..email@domain.com");
        Add("email@domain.com (Joe Smith)");
        Add("email@domain");
        Add("email@-domain.com");
        Add("email@111.222.333.44444");
        Add("email@domain..com");
        Add("email@256.256.256.256");
    }
}

public sealed class InvalidEmailAddressesWithNull : InvalidEmailAddresses
{
    public InvalidEmailAddressesWithNull()
    {
        Add(null);
    }
}
