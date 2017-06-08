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
        public List<Item> GetAllItemsByTechnician(string empID,UA UA)
        {
            List<Item> Itemlist = null;
            Itemlist = _itemRepository.GetAllItemsByTechnician(empID,UA);
            return Itemlist;

        }
        public List<Item> GetAllServiceTypeItems(UA UA)
        {
            List<Item> Itemlist = null;
            Itemlist = _itemRepository.GetAllServiceTypeItems(UA);
            return Itemlist;

        }
        public List<Item> GetAllItemCode(UA UA)
        {
            List<Item> Itemlist = null;
            Itemlist = _itemRepository.GetAllItemCode(UA);
            return Itemlist;

        }
        public List<Item> GetAllUOMs()
        {
            List<Item> Itemlist = null;
            Itemlist = _itemRepository.GetAllUOMs();
            return Itemlist;
        }
        public List<Item> GetItemByID(UA UA,string ID)
        {
            List<Item> Itemlist = null;
            Itemlist = _itemRepository.GetItemByID(UA,ID);
            return Itemlist;

        }
     
        public object InsertItem(Item itemObj)
        {
            object result = null;
            try
            {
                result = _itemRepository.InsertItem(itemObj);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public string UpdateItem(Item itemObj)
        {
            string status = null;
            try
            {
                status = _itemRepository.UpdateItem(itemObj);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return status;
        }

        public string DeleteItem(string ID)
        {
            string status = null;
            try
            {
                status = _itemRepository.DeleteItem(ID);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return status;
        }
    }
}