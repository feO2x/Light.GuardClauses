using Xunit;

namespace Light.GuardClauses.Tests.StringAssertions;

public sealed class ValidEmailAddresses : TheoryData<string>
{
    public ValidEmailAddresses()
    {
        Add("email@domain.com");
        Add("firstname.lastname@domain.com");
        Add("email@subdomain.domain.com");
        Add("firstname+lastname@domain.com");
        Add("email@123.123.123.123");
        Add("1234567890@domain.com");
        Add("email@domain-one.com");
        Add("_______@domain.com");
        Add("email@domain.name");
        Add("email@domain.co.jp");
        Add("firstname-lastname@domain.com");
        Add("email@domain.museum"); // Long TLD (>4 chars)
        Add("email@domain.travel"); // Another long TLD
        Add("email@domain.photography"); // Even longer TLD
        Add("email@[IPv6:2001:db8::1]"); // IPv6 format
        Add("\"quoted\"@domain.com"); // Quoted local part
        Add("user.name+tag+sorting@example.com"); // Gmail-style + addressing
        Add("あいうえお@domain.com"); // Unicode character test
    }
}
