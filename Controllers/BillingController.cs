using HotelWebApi.UserModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using HotelWebApi.Dtos.Billing;

namespace HotelWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillingController : Controller
    {
        private readonly FrankiesHotelContext _context;
        private readonly IMapper mapper;

        public static IWebHostEnvironment _environment;
        public BillingController(IWebHostEnvironment environment, FrankiesHotelContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
            _environment = environment;
        }

        [HttpGet("id")]
        public async Task<ActionResult<IEnumerable<Billing>>> GetBillings(int id,bool isCorporate,string serviceType)
        {
            // Get the current date
            DateTime currentDate = DateTime.Now.Date;

            // Find bookings with null CheckInTime and BookEnd date before today
            List<Billing> guestBilling = new List<Billing>();
            List<CompanyBill> corporateBilling = new List<CompanyBill>();
            if (isCorporate)
            {

                corporateBilling = await _context.CompanyBills
                .Where(b =>
                    b.CompanyId == id &&(
                    (serviceType.ToLower().Contains("all_charges") && b.Description.Contains("Accommodation for Room") == false) ||
                    (serviceType.ToLower().Contains("rooms_only") && b.Description.Contains("Accommodation for Room")) ||
                    (serviceType.ToLower().Contains("debit_notes") && (b.Credit == null && (b.IsPayment == false || b.IsPayment == null) && b.Debit != null && b.RoomId == null)) ||
                    (serviceType.ToLower().Contains("credit_notes") && (b.Debit == null && (b.IsPayment == false || b.IsPayment == null) && b.Credit != null && b.RoomId == null)) ||
                    (serviceType.ToLower().Contains("services_only") && (b.Credit == null && (b.IsPayment == false || b.IsPayment == null) && b.Debit != null && b.Description.Contains("Accommodation for Room") == false) && b.RoomId != null))
                )
                .ToListAsync();
                return Ok(corporateBilling);
            }
            else
            {
               
                guestBilling = await _context.Billings
                .Where(b =>
                    b.CustomerId == id && (
                    (serviceType.ToLower().Contains("all_charges")) ||
                    (serviceType.ToLower().Contains("rooms_only") && b.Description.Contains("Accommodation for Room")) ||
                    (serviceType.ToLower().Contains("debit_notes") && (b.Credit == null && (b.IsPayment == false || b.IsPayment == null) && b.Debit != null && b.RoomId == null)) ||
                    (serviceType.ToLower().Contains("credit_notes") && (b.Debit == null && (b.IsPayment == false || b.IsPayment == null) && b.Credit != null && b.RoomId == null)) ||
                    (serviceType.ToLower().Contains("services_only") && (b.Credit == null && (b.IsPayment == false || b.IsPayment == null) && b.Debit != null && b.Description.Contains("Accommodation for Room") == false && b.RoomId != null)))
                )
                .ToListAsync();
                return Ok(guestBilling);
                //guestBilling = await _context.Billings.Where(b => b.CustomerId == id).ToListAsync();
            }

     
        }

        [HttpGet("billcompany")]
        public async Task<ActionResult<IEnumerable<CompanyBill>>> GetCompanyBills()
        {
            var companyBills = await _context.CompanyBills.ToListAsync();
            return Ok(companyBills);
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<Billing>>> PostBillings(BillingCreateDto billingCreate)
        {

            var cedisRate = await _context.Currencies.Where(x => x.Id == 1).ToListAsync();
            billingCreate.timestamp = DateTime.Now;
            billingCreate.CurrencyRate = cedisRate[0].Rate;
            var billing = mapper.Map<Billing>(billingCreate);
            var billingHistory = mapper.Map<BillingsHistory>(billingCreate);

            await _context.Billings.AddAsync(billing);
            await _context.BillingsHistories.AddAsync(billingHistory);
            await _context.SaveChangesAsync();

            return Ok(billing.Id);
        }
        [HttpPost("companyPayment")]
        public async Task<ActionResult<IEnumerable<CompanyBill>>> companyPayment(CompanyBill billingCreate)
        {
            //var billing = mapper.Map<Billing>(billingCreate);
            if (billingCreate==null)
            {
                return StatusCode(501, "payment data is empty");
            }

            billingCreate.Timestamp = DateTime.Now;
            await _context.CompanyBills.AddAsync(billingCreate);
         
            await _context.SaveChangesAsync();

            return Ok();
        }
        [HttpPost("addCompanyBills")]
        public async Task<ActionResult<int>> PostCompanyBillings(PostCorporateBillingDto corporateBilling)
        {
            corporateBilling.timestamp = DateTime.Now;
            var billing = mapper.Map<Billing>(corporateBilling);
            var billingHistory = mapper.Map<BillingsHistory>(corporateBilling);
            var companyBillsData = new List<CompanyBill>();

            if (corporateBilling.Bills.Length != 0)
            {
                foreach (var item in corporateBilling.Bills)
                {
                    

                    var existingBillings = await _context.Billings
                                                  .Where(te => te.Id == item)
                                                  .FirstOrDefaultAsync();
                    if (existingBillings != null)
                    {
                        var companyBill = new CompanyBill
                        {
                            CompanyId = corporateBilling.CompanyId,
                            CustomerId = corporateBilling.customerId,
                            Debit= existingBillings.Debit,
                            CustomerBookingId = corporateBilling.CustomerBookingId,
                            Description = corporateBilling.description,
                            Currency = existingBillings.Currency,
                            ReceiptNumber = corporateBilling.ReceiptNumber,
                            IsPayment = corporateBilling.isPayment,
                            Timestamp = corporateBilling.timestamp,
                            RoomId = existingBillings.RoomId,
                            BillingId=existingBillings.Id,
                            RoomRate=existingBillings.RoomRate
                            
                            
                        };
                        companyBillsData.Add(companyBill);


                    }
                    
                    if (existingBillings != null)
                    {
                        _context.Entry(existingBillings).State = EntityState.Detached;
                    }


                    existingBillings = null;
                }
            }

            await _context.Billings.AddAsync(billing);
            await _context.CompanyBills.AddRangeAsync(companyBillsData);
            await _context.BillingsHistories.AddAsync(billingHistory);
            await _context.SaveChangesAsync();

            return Ok(billing.Id);
        }



        [HttpPost("BillingTransfer")]
        public async Task<ActionResult<IEnumerable<Billing>>> TransferBillings(BillingTransferCreateDto transferCreate)
        {
            //var billing = mapper.Map<Billing>(billingCreate);
            //var billingHistory = mapper.Map<BillingsHistory>(billingCreate);

            Billing billingToTransfer = new Billing(); 
            Billing billingToTransferSource = new Billing(); 
            BillingsHistory billingHistoryToTransfer = new BillingsHistory();
            BillingsHistory billingHistoryToTransferSource = new BillingsHistory();

            //Crediting bill from source
            billingToTransferSource.CustomerId = transferCreate.customerId;
            billingToTransferSource.Debit = null;
            billingToTransferSource.Credit = transferCreate.amount;
            billingToTransferSource.Currency = transferCreate.currency;
            billingToTransferSource.Description = transferCreate.description;
            billingToTransferSource.Timestamp = DateTime.Now;
            await _context.Billings.AddAsync(billingToTransferSource);
            //await _context.SaveChangesAsync();

            //Crediting bill from source (History)
            billingHistoryToTransferSource.CustomerId = transferCreate.customerId;
            billingHistoryToTransferSource.Credit = transferCreate.amount;
            //billingHistoryToTransfer.Debit = null;
            billingHistoryToTransferSource.Currency = transferCreate.currency;
            billingHistoryToTransferSource.Description = transferCreate.description;
            billingHistoryToTransferSource.Timestamp = DateTime.Now;
            await _context.BillingsHistories.AddAsync(billingHistoryToTransferSource);
            //await _context.SaveChangesAsync();


            //Transfering billing to receiver
            billingToTransfer.CustomerId = transferCreate.receiverId;
            billingToTransfer.CustomerIdTransferedFrom = transferCreate.customerId;
            billingToTransfer.Debit = transferCreate.amount;
            //billingToTransfer.Credit = null;
            billingToTransfer.Currency = transferCreate.currency;
            billingToTransfer.Description = transferCreate.description;
            billingToTransfer.Timestamp = DateTime.Now;
            await _context.Billings.AddAsync(billingToTransfer);
            //await _context.SaveChangesAsync();

            //Transfering billing to receiver (History)
            billingHistoryToTransfer.CustomerId = transferCreate.receiverId;
            billingHistoryToTransfer.CustomerIdTransferedFrom = transferCreate.customerId;
            billingHistoryToTransfer.Debit = transferCreate.amount;
            //billingHistoryToTransfer.Credit = null;
            billingHistoryToTransfer.Currency = transferCreate.currency;
            billingHistoryToTransfer.Description = transferCreate.description;
            billingHistoryToTransfer.Timestamp = DateTime.Now;
            await _context.BillingsHistories.AddAsync(billingHistoryToTransfer);

            await _context.SaveChangesAsync();

            return Ok();
        }
        [HttpDelete("id")]
        public async Task<ActionResult<Billing>> DeleteBilling(int id)
        {
            var billing = await _context.Billings.FindAsync(id);
            _context.Billings.Remove(billing);
            await _context.SaveChangesAsync();
            return Ok();
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
