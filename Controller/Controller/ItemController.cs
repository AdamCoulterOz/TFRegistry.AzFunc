using System;
using System.IO;
using System.Threading.Tasks;
using PurpleDepot.Data;
using PurpleDepot.Interface.Exceptions;
using PurpleDepot.Interface.Model;
using PurpleDepot.Interface.Storage;

namespace PurpleDepot.Controller
{
	public abstract class ItemController
	{
		protected readonly IItemContext _itemContext;
		protected readonly IStorageProvider _storageProvider;

		public ItemController(IItemContext itemContext, IStorageProvider storageProvider)
		{
			_itemContext = itemContext;
			_storageProvider = storageProvider;
		}

		public async Task Ingest(RegistryItem newItem, Stream stream)
		{
			var item = _itemContext.GetItem(newItem);

			if(item is not null && !HasVersion(item, newItem.Version))
				throw new AlreadyExistsException(item);

			if (item is null)
			{
				item = newItem;
				item.Id = Guid.NewGuid();
				_itemContext.Add(item);
			}

			item.AddVersion(newItem.Version);
			Validate(item);

			var fileKey = item.FileId(newItem.Version.Version);
			if (fileKey is null)
				return request.CreateStringResponse(HttpStatusCode.InternalServerError, "There was an issue trying to create the new version.");

			bool hadContent = await _storageProvider.UploadZipAsync(fileKey.Value, stream, ObjectType.Module);

			if (hadContent)
			{
				_moduleContext.SaveChanges();
				return request.CreateResponse(HttpStatusCode.Created);
			}
			else
			{
				return request.CreateStringResponse(HttpStatusCode.BadRequest, "Uploaded file had zero bytes, and the module was not saved.");
			}
		}

		protected abstract bool Validate(RegistryItem item);
		protected abstract bool HasVersion(RegistryItem item, RegistryItemVersion version);
	}
}