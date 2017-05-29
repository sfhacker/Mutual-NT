
namespace Mutual_NT.ServiceContext
{
    using EIV.TrackableEntities;
    using Microsoft.OData.Client;
    using System;

    public sealed class ODataServiceContext
    {
        private readonly DataServiceContext serviceContext = null;
        public ODataServiceContext(DataServiceContext container)
        {
            this.serviceContext = container;
        }

        public void ProcessOperations(TrackableEntities list)
        {
            bool rst;

            if (this.serviceContext == null)
            {
                return;
            }

            if (list == null)
            {
                return;
            }

            // We could have entities of different type within the same collection, couldn't we?
            /*
            if (string.IsNullOrEmpty(list.Name))
            {
                return;
            }*/

            foreach (TrackableEntity item in list.Items)
            {
                object entity = item.Entity;

                switch (item.OperationType)
                {
                    case TrackableEntity.Operation.Insert:

                        // I need to make it generic!
                        string entityName = string.Empty;

                        Type entityType = entity.GetType();
                        if (string.IsNullOrEmpty(item.EntityName))
                        {
                            entityName = entityType.Name;
                        }
                        else
                        {
                            entityName = item.EntityName;
                        }

                        this.serviceContext.AddObject(entityName, entity);
                        break;

                    // if the entity's context is null
                    // then, runtime exception
                    // run GetEntities() first
                    case TrackableEntity.Operation.Update:
                        rst = this.IsValidEntity(entity);
                        if (rst) // should we throw an exception here?
                        {
                            this.serviceContext.UpdateObject(entity);
                        }
                        break;

                    // if the entity's context is null
                    // then, runtime exception
                    // run GetEntities() first
                    case TrackableEntity.Operation.Delete:
                        rst = this.IsValidEntity(entity);
                        if (rst)
                        {
                            this.serviceContext.DeleteObject(entity);
                        }
                        break;
                }
            }
        }

        // Make it private later on
        // Only applicable to PUT/PATCH/DELETE methods
        private bool IsValidEntity(object entity)
        {
            bool rst;

            object newEntity = null;
            Uri entityUri = null;

            if (entity == null)
            {
                return false;
            }
            rst = this.serviceContext.TryGetUri(entity, out entityUri);
            if (!rst)
            {
                return false;
            }

            Type entityType = entity.GetType();

            rst = this.serviceContext.TryGetEntity(entityUri, out newEntity);
            if (!rst)
            {
                return false;
            }
            return true;
        }
    }
}