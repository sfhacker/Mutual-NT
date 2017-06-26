
namespace EIV.OData.Client.Context
{
    using Interface;
    using Microsoft.OData.Client;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public sealed class ODataServiceContext : IODataServiceContext
    {
        private static volatile ODataServiceContext _instance = null;
        private static readonly object InstanceLoker = new object();

        // Microsoft.OData.Client
        private DataServiceContext container = null;

        private bool isConnected;
        private string batchRelativePath;

        private string userName = null;
        private string password = null;
        private string domainName = null;

        private string errorMessage = string.Empty;
        private string statusMessage = string.Empty;

        private ODataServiceContext()
        {
            this.isConnected = false;
            this.batchRelativePath = null;

            this.container = null;

            this.userName = Environment.UserName;
            this.domainName = Environment.UserDomainName;
        }

        public static ODataServiceContext Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (InstanceLoker)
                    {
                        if (_instance == null)
                            _instance = new ODataServiceContext();
                    }
                }

                return _instance;
            }
        }

        public string UserName
        {
            get
            {
                return this.userName;
            }
            set
            {
                this.userName = value;
            }
        }

        public string Password
        {
            internal get
            {
                return this.password;
            }
            set
            {
                this.password = value;
            }
        }

        public string DomainName
        {
            get
            {
                return this.domainName;
            }
            set
            {
                this.domainName = value;
            }
        }

        public bool IsConnected
        {
            get
            {
                return this.isConnected;
            }
        }

        public string BatchRelativePath
        {
            set
            {
                this.batchRelativePath = value;
            }
        }
        public bool Connect<X>(string serviceUri) where X : class
        {
            Uri thisUri;

            if (string.IsNullOrEmpty(serviceUri))
            {
                return false;
            }

            bool rst = Uri.TryCreate(serviceUri, UriKind.Absolute, out thisUri);
            if (!rst)
            {
                return false;
            }
            if (this.isConnected)
            {
                return true;
            }

            Type contextType = typeof(X);

            // Paranoic!
            if (!this.ValidateObjectType(contextType))
            {
                return false;
            }

            try
            {
                // http://odata.github.io/odata.net/04-06-use-client-hooks-in-odata-client/
                this.container = Activator.CreateInstance(contextType, thisUri) as DataServiceContext;

                this.container.Format.UseJson();

                this.container.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;

                this.isConnected = true;

                return true;
            }
            catch (Exception ex)
            {
                // TODO
                //this.errors.Add(ex);
            }

            return false;
        }

        public void Disconnect()
        {
            if (!this.isConnected)
            {
                return;
            }

            this.container = null;

            this.isConnected = false;
        }

        public void Dispose()
        {
        }


        public string GenerateQueryUri<T>(string entitySet, IDictionary<string, object> entityParams, IDictionary<string, object> filters) where T : class
        {
            UriOperationParameter[] myParams = null;

            DataServiceQuery<T> query = null;

            if (!this.isConnected)
            {
                return null;
            }
            if (string.IsNullOrEmpty(entitySet))
            {
                return null;
            }

            if (entityParams == null)
            {
                query = this.container.CreateQuery<T>(entitySet);
            }
            else
            {
                int count = entityParams.Count;
                if (count > 0)
                {
                    myParams = new UriOperationParameter[count];

                    int i = 0;
                    foreach (string paramKey in entityParams.Keys)
                    {
                        // check for null value here
                        myParams[i++] = new UriOperationParameter(paramKey, entityParams[paramKey]);
                    }

                    query = this.container.CreateFunctionQuery<T>("", entitySet, false, myParams);
                }
            }
            if (query == null)
            {
                return null;
            }
            if (filters != null)
            {
                foreach (string filter in filters.Keys)
                {
                    object filterValue = filters[filter];
                    if (filterValue != null)
                    {
                        // Immutability here ?
                        query = query.AddQueryOption(filter, filterValue);
                    }
                }
            }

            return query.ToString();
        }

        public string GenerateFunctionQueryUri<T>(string entitySet, string functionName, IDictionary<string, object> functionParams) where T : class
        {
            UriOperationParameter[] myParams = null;

            DataServiceQuery<T> query = null;

            if (!this.isConnected)
            {
                return null;
            }
            if (string.IsNullOrEmpty(functionName))
            {
                return null;
            }
            // Are they manadatory?
            /*if (functionParams == null)
            {
                return null;
            }*/

            if (functionParams != null)
            {
                int count = functionParams.Count;
                if (count > 0)
                {
                    myParams = new UriOperationParameter[count];

                    int i = 0;
                    foreach (string paramKey in functionParams.Keys)
                    {
                        myParams[i++] = new UriOperationParameter(paramKey, functionParams[paramKey]);
                    }
                }
            }
            //query = this.container.CreateFunctionQuery<T>(entitySet, "EIV.Demo.WebService.PaisAction", false, myParams);
            // First param cannot be null, but empty
            // Last param cannot be null
            // Java service (OLingo) requires '()' at the end of the function name
            if (myParams == null)
            {
                query = this.container.CreateFunctionQuery<T>(entitySet, functionName, false);
            }
            else
            {
                // I guess I need a two-step process
                //query = this.container.CreateFunctionQuery<T>(entitySet, functionName, false, myParams);
                query = this.container.CreateFunctionQuery<T>("", entitySet, false, myParams);
            }
            if (query == null)
            {
                return null;
            }

            // Awful hack!
            string rstQuery = string.Empty;
            string preQuery = query.ToString();
            if (myParams == null)
            {
                rstQuery = preQuery;
            }
            else
            {
                rstQuery = string.Format("{0}/{1}", preQuery, functionName);
            }

            return rstQuery;
        }

        // entitySetName: "Paises"
        public ObservableCollection<T> LoadEntityItems<T>(string entitySetName) where T : class
        {
            IEnumerable<T> result = null;

            if (this.container == null)
            {
                return null;
            }
            if (string.IsNullOrEmpty(entitySetName))
            {
                return null;
            }
            try
            {
                // 1.- Query
                // The service is down or any other network related issue(s)
                var query = this.container.CreateQuery<T>(entitySetName);

                result = query.Execute();

                // Synchronous operation ???
                var list = result.ToList();

                return new ObservableCollection<T>(list);
            }
            catch (Exception ex)
            {
                //this.statusInfo.Text = ex.Message;
                this.errorMessage = ex.Message;
            }

            return null;
        }

        // TODO:
        // public IList<IODataResponse> ExecuteBatch(IList<IODataOperation> operations)
        public IList<object> ExecuteBatch(IList<object> operations)
        {
            int contentId;

            if (operations == null)
            {
                return null;
            }
            if (operations.Count < 1)
            {
                return null;
            }

            return null;
        }

        // BaseType = {Name = "DataServiceContext" FullName = "Microsoft.OData.Client.DataServiceContext"}
        private bool ValidateObjectType(Type objectType)
        {
            if (objectType == null)
            {
                return false;
            }

            if (objectType.BaseType != null)
            {
                string baseTypeName = objectType.BaseType.Name;
                string baseTypeFullName = objectType.BaseType.FullName;

                if (string.IsNullOrEmpty(baseTypeName) || string.IsNullOrEmpty(baseTypeFullName))
                {
                    return false;
                }

                // This should be a constant
                if (!baseTypeName.Equals("DataServiceContext"))
                {
                    return false;
                }

                // This should be a constant
                if (!baseTypeFullName.Equals("Microsoft.OData.Client.DataServiceContext"))
                {
                    return false;
                }

                return true;
            }

            return false;
        }
    }
}