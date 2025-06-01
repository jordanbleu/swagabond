using Moq;
using Moq.AutoMock;

namespace Swagabond.Tests.Extensions;

public static class TransformerMockExtensions
{
    /// <summary>
    /// Incredibly hacky method that requires your ITransformer to have a method called "FromOpenApi" that takes any
    /// number of params.  This will setup your mock so that it returns a new instance of TResult when FromOpenApi is called. 
    /// </summary>
    /// <param name="mock"></param>
    /// <param name="modifyResult"></param>
    /// <typeparam name="TTransformer"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public static void SetupMockTransformer<TTransformer, TResult>(this Mock<TTransformer> mock, Action<TResult> modifyResult = null) 
        where TTransformer : class
    {
        mock.SetupWithAny<TTransformer, TResult>("FromOpenApi")
            .Returns(() =>
            {
                var result = Activator.CreateInstance<TResult>();
                modifyResult?.Invoke(result);
                return result;
            });
    }

}