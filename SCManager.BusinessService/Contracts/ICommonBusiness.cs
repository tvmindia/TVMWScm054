using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCManager.BusinessService.Contracts
{
    public interface ICommonBusiness
    {
        string GetXMLfromObject(List<Form8Detail> myObj, string mandatoryProperties, UA ua);
        string GetXMLfromForm8BDetail(List<Form8BDetail> myObj, string mandatoryProperties, UA ua);
        string GetXMLfromLocalPurchaseDetail(List<LocalPurchaseDetail> myObj, string mandatoryProperties, UA ua);
        string GetXMLfromOpenDetail(List<OpeningDetail> myObj, string mandatoryProperties, UA ua);
        string ConvertCurrency(decimal value, int DecimalPoints = 0);
        string GetXMLfromTCRObject(List<TCRBillEntryDetail> tcrDetailObj, string mandatoryProperties, UA ua);
        string GetXMLfromICRObject(List<ICRBillEntryDetail> icrDetailObj, string mandatoryProperties, UA ua);
        string GetXMLfromOfficeObject(List<OfficeBillEntryDetail> officeDetailObj, string mandatoryProperties, UA ua);
        string GetXMLfromOtherSCReceiptObject(List<ReceiveFromOtherScDetail> receiveFromOtherScDetailObj, string mandatoryProperties, UA ua);
    }
}
