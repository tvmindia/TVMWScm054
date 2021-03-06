﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCManager.DataAccessObject.DTO;

namespace SCManager.BusinessService.Contracts
{
    public interface IItemBusiness
    {
        List<Item> GetAllItems(UA UA,string filter);
        List<Item> GetItemByID(UA UA,string ID);
        object InsertItem(Item itemObj);
        string UpdateItem(Item itemObj);
        string DeleteItem(string ID);
        List<Item> GetAllItemCode(UA UA);
        List<Item> GetAllUOMs();
        List<Item> GetAllServiceTypeItems(UA UA);
        List<Item> GetAllItemsByTechnician(string empID, UA UA);
    }
}
