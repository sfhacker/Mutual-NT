
namespace EIV.OData.Client.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public interface IODataServiceContext : IDisposable
    {
        bool IsConnected { get; }

        string BatchRelativePath { set; }

        bool Connect<X>(string serviceUri) where X : class;
        void Disconnect();

        // params: Id=2
        string GenerateQueryUri<T>(string entitySet, IDictionary<string, object> entityParams, IDictionary<string, object> filters) where T : class;
        string GenerateFunctionQueryUri<T>(string entitySet, string functionName, IDictionary<string, object> functionParams) where T : class;

        // We need to pass some filters here!
        ObservableCollection<T> LoadEntityItems<T>(string entitySetName) where T : class;

        // IList<IODataResponse> ExecuteBatch(IList<IODataOperation> operations);
        // TODO:
        IList<object> ExecuteBatch(IList<object> operations);
    }
}