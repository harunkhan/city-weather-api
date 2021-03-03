using CityInfo.Data.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CityInfo.BL.GenericRepository
{
	/// Generic Repository class for Entity Operations
	public class GenericRepository<TEntity> where TEntity : class
	{
		internal CityInfoDbContext Context;
		internal DbSet<TEntity> DbSet;

		/// Public Constructor,initializes privately declared local variables.
		public GenericRepository(CityInfoDbContext context) {
			this.Context = context;
			this.DbSet = context.Set<TEntity>();
		}

		/// generic Get method for Entities
		public virtual IEnumerable<TEntity> Get() {
			IQueryable<TEntity> query = DbSet;
			return query.ToList();
		}

		/// Generic get method on the basis of id for Entities.
		public virtual TEntity GetByID(object id) {
			return DbSet.Find(id);
		}

		/// generic Insert method for the entities
		public virtual void Insert(TEntity entity) {
			DbSet.Add(entity);
		}

		/// Generic Delete method for the entities
		public virtual void Delete(object id) {
			TEntity entityToDelete = DbSet.Find(id);
			Delete(entityToDelete);
		}

		/// Generic Delete method for the entities
		public virtual void Delete(TEntity entityToDelete) {
			if (Context.Entry(entityToDelete).State == EntityState.Detached) {
				DbSet.Attach(entityToDelete);
			}
			DbSet.Remove(entityToDelete);
		}

		/// Generic update method for the entities
		public virtual void Update(TEntity entityToUpdate) {
			DbSet.Attach(entityToUpdate);
			Context.Entry(entityToUpdate).State = EntityState.Modified;
		}

		/// generic method to get many record on the basis of a condition.
		public virtual IEnumerable<TEntity> GetMany(Func<TEntity, bool> where) {
			return DbSet.Where(where).ToList();
		}

		/// generic method to get many record on the basis of a condition but query able.
		public virtual IQueryable<TEntity> GetManyQueryable(Func<TEntity, bool> where) {
			return DbSet.Where(where).AsQueryable();
		}

		/// generic get method , fetches data for the entities on the basis of condition.
		public TEntity Get(Func<TEntity, Boolean> where) {
			return DbSet.Where(where).FirstOrDefault<TEntity>();
		}

		/// generic delete method , deletes data for the entities on the basis of condition.
		public void Delete(Func<TEntity, Boolean> where) {
			IQueryable<TEntity> objects = DbSet.Where<TEntity>(where).AsQueryable();
			foreach (TEntity obj in objects)
				DbSet.Remove(obj);
		}

		/// generic method to fetch all the records from db
		public virtual IEnumerable<TEntity> GetAll() {
			return DbSet.ToList();
		}

		/// Inclue multiple
		public IQueryable<TEntity> GetWithInclude(
			System.Linq.Expressions.Expression<Func<TEntity,
			bool>> predicate, params string[] include) {
			IQueryable<TEntity> query = this.DbSet;
			query = include.Aggregate(query, (current, inc) => current.Include(inc));
			return query.Where(predicate);
		}

		/// Generic method to check if entity exists
		public bool Exists(object primaryKey) {
			return DbSet.Find(primaryKey) != null;
		}

		/// Gets a single record by the specified criteria (usually the unique identifier)
		public TEntity GetSingle(Func<TEntity, bool> predicate) {
			return DbSet.Single<TEntity>(predicate);
		}

		/// The first record matching the specified criteria
		public TEntity GetFirst(Func<TEntity, bool> predicate) {
			return DbSet.First<TEntity>(predicate);
		}
	}
}

