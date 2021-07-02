using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PurpleDepot.Interface.Model;

namespace PurpleDepot.Data
{
	public interface IItemContext
	{
		public RegistryItem? GetItem(RegistryItem item);
		public EntityEntry<TEntity> Add<TEntity>([NotNull] TEntity entity) where TEntity : class;
	}
}