using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCManager.RepositoryServices.Contracts;
using SCManager.BusinessService.Contracts;
using SCManager.DataAccessObject.DTO;

namespace SCManager.BusinessService.Services
{
    public class ItemBusiness : IItemBusiness
    {
        private IItemRepository _itemRepository;

        public ItemBusiness(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public List<Item> GetAllItems(UA UA)
        {
            List<Item> Itemlist = null;
            Itemlist = _itemRepository.GetAllItems(UA);
            return Itemlist;

        }
    }
}