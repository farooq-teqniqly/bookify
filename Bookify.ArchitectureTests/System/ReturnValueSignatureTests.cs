namespace Bookify.ArchitectureTests.System
{
    public class ReturnValueSignatureTests
    {
        [Fact]
        public void Repository_Interface_Methods_Should_Return_Result_Or_Result_Of_T()
        {
            TestHelpers.AssertInterfaceMethodsReturnResultOrResultOfT("Repository");
        }

        [Fact]
        public void Service_Interface_Methods_Should_Return_Result_Or_Result_Of_T()
        {
            TestHelpers.AssertInterfaceMethodsReturnResultOrResultOfT("Service");
        }
    }
}
