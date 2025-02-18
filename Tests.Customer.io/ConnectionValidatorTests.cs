using Apps.Customer.io.Connections;
using Blackbird.Applications.Sdk.Common.Authentication;
using Tests.Customer.io.Base;

namespace Tests.Customer.io;

[TestClass]
public class ConnectionValidatorTests : TestBase
{
    [TestMethod]
    public async Task ValidatesCorrectConnection()
    {
        var validator = new ConnectionValidator();

        var result = await validator.ValidateConnection(Creds, CancellationToken.None);
        Assert.IsTrue(result.IsValid);
        Console.WriteLine(result.Message);
    }

    [TestMethod]
    public async Task DoesNotValidateIncorrectConnection()
    {
        var validator = new ConnectionValidator();

        var newCredentials = Creds.Select(x => new AuthenticationCredentialsProvider(x.KeyName, x.Value + "_incorrect"));
        var result = await validator.ValidateConnection(newCredentials, CancellationToken.None);
        Assert.IsFalse(result.IsValid);
    }
}