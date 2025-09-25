//using HotelWebApi.Data;
using HotelWebApi.Controllers;
using HotelWebApi.Dtos.Billing;
using HotelWebApi.Dtos.Booking;
using HotelWebApi.Dtos.Company;
using HotelWebApi.Dtos.Currency;
using HotelWebApi.Dtos.Guest;
using HotelWebApi.Dtos.GuestService;
using HotelWebApi.Dtos.HouseKeeping;
using HotelWebApi.Dtos.Notes;
using HotelWebApi.Dtos.PaymentNote;
using HotelWebApi.Dtos.PaymentMethod;

using HotelWebApi.Dtos.Room;
using HotelWebApi.Dtos.ServiceCategory.cs;
using HotelWebApi.Dtos.ServiceDetails;
using HotelWebApi.Dtos.Tax;
using HotelWebApi.Dtos.User;
using HotelWebApi.UserModels;
using AutoMapper;
using HotelWebApi.Dtos.AuditTrail;
using HotelWebApi.Dtos.RoomType;

namespace HotelWebApi.Helpers
{
    public class AutoMappers : Profile
    {
        public AutoMappers()
        {
            CreateMap<UserCreateDto, User>().ReverseMap();
            CreateMap<UserPutDto, User>().ReverseMap();
            CreateMap<UserRoleCreateDto, Role>().ReverseMap();
            CreateMap<TaxCreateDto, TaxTable>().ReverseMap();
            CreateMap<TaxPutDto, TaxTable>().ReverseMap();

            //CreateMap<RoomCategoryCreateDto, Room>().ReverseMap();
            CreateMap<BookingCreateDto, Booking>().ReverseMap();
            CreateMap<NewGuestBookingCreateDto, Booking>().ReverseMap();
            CreateMap<BookingPutDto, Booking>().ReverseMap();
            CreateMap<BookingDto, Booking>().ReverseMap();
            CreateMap<CurrencyCreateDto, Currency>().ReverseMap();
            CreateMap<CurrencyPutDto, Currency>().ReverseMap();

            CreateMap<CompanyCreateDto, Company>().ReverseMap();
            CreateMap<CompanyPutDto, Company>().ReverseMap();

            CreateMap<PaymentMethodCreateDto, PaymentMethod>().ReverseMap();
            CreateMap<PaymentMethodPutDto, PaymentMethod>().ReverseMap();

            CreateMap<PaymentNoteCreateDto, PaymentNote>().ReverseMap();
            CreateMap<PaymentNotePutDto, PaymentNote>().ReverseMap();

            CreateMap<BillingCreateDto, Billing>().ReverseMap();
            CreateMap<BillingCreateDto, BillingsHistory>().ReverseMap();
            CreateMap<PostCorporateBillingDto, BillingsHistory>().ReverseMap();
            CreateMap<PostCorporateBillingDto, Billing>().ReverseMap();
            CreateMap<RoomTypeCreateDto, RoomType>().ReverseMap();
            CreateMap<RoomDto, Room>().ReverseMap();

            CreateMap<GuestDto, Guest>().ReverseMap();
            CreateMap<GuestCreateDto, Guest>().ReverseMap();
            CreateMap<NewGuestBookingCreateDto, Guest>().ReverseMap();

            CreateMap<NoteDto, Note>().ReverseMap();
            CreateMap<NoteCreateDto, Note>().ReverseMap();

            CreateMap<ServiceCreateDto, ServiceCategory>().ReverseMap();
            CreateMap<ServiceDetailDto, ServiceDetail>().ReverseMap();
            CreateMap<HouseKeepingItemDto, HouseKeepingItem>().ReverseMap();
            CreateMap<GuestServiceDto, GuestService>().ReverseMap();
            CreateMap<AuditTrailCreateDto, AuditTrail>().ReverseMap();


        }
    }
}
