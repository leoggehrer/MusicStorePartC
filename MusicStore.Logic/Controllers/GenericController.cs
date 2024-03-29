﻿using System;
using System.Collections.Generic;
using System.Linq;
using MusicStore.Contracts.Client;
using MusicStore.Logic.DataContext;

namespace MusicStore.Logic.Controllers
{
    internal abstract partial class GenericController<E, I> : ControllerObject, IControllerAccess<I>
        where E : Entities.IdentityObject, I, Contracts.ICopyable<I>, new()
        where I : Contracts.IIdentifiable
    {
        protected abstract IEnumerable<E> Set { get; }

        protected GenericController(IContext context)
            : base(context)
        {

        }
        protected GenericController(ControllerObject controllerObject)
            : base(controllerObject)
        {

        }

        #region Sync-Methods
        public int Count()
        {
            return Context.Count<I, E>();
        }
        public virtual IEnumerable<I> GetAll()
        {
            return Set.Select(i =>
            {
                var result = new E();

                result.CopyProperties(i);
                return result;
            });
        }
        public virtual I GetById(int id)
        {
            var result = default(E);
            var item = Set.SingleOrDefault(i => i.Id == id);

            if (item != null)
            {
                result = new E();
                result.CopyProperties(item);
            }
            return result;
        }
        public virtual I Create()
        {
            return new E();
        }

        protected virtual void BeforeInserting(I entity)
        {

        }
        public virtual I Insert(I entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            BeforeInserting(entity);
            var result = Context.Insert<I, E>(entity);
            AfterInserted(result);
            return result;
        }
        protected virtual void AfterInserted(E entity)
        {

        }

        protected virtual void BeforeUpdating(I entity)
        {

        }
        public virtual void Update(I entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            BeforeUpdating(entity);
            var updateEntity = Context.Update<I, E>(entity);

            if (updateEntity != null)
            {
                AfterUpdated(updateEntity);
            }
            else
            {
                throw new Exception("Entity can't find!");
            }
        }
        protected virtual void AfterUpdated(E entity)
        {

        }

        protected virtual void BeforeDeleting(int id)
        {

        }
        public void Delete(int id)
        {
            BeforeDeleting(id);
            var item = Context.Delete<I, E>(id);

            if (item != null)
            {
                AfterDeleted(item);
            }
        }
        protected virtual void AfterDeleted(E entity)
        {

        }

        public void SaveChanges()
        {
            Context.Save();
        }
        #endregion Sync-Methods
    }
}
